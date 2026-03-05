using System;
using System.Linq;
using KooliProjekt.Controllers;
using KooliProjekt.Data;
using KooliProjekt.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace KooliProjekt.IntegrationTests.Helpers
{
    public class FakeStartup //: Startup
    {
        public FakeStartup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public virtual void ConfigureServices(IServiceCollection services)
        {
            var dbGuid = Guid.NewGuid().ToString();
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("TestConnection"));
            });

            services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                    .AddEntityFrameworkStores<ApplicationDbContext>();

            // Register services
            services.AddScoped<ITeamService, TeamService>();
            services.AddScoped<ITournamentService, TournamentService>();
            services.AddScoped<IMatchesService, MatchesService>();
            services.AddScoped<IPredictionService, PredictionService>();
            services.AddScoped<IRankingService, RankingService>();

            services.AddControllersWithViews()
                    .AddApplicationPart(typeof(HomeController).Assembly);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseStaticFiles();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}/{pathStr?}");
            });

            var serviceScopeFactory = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>();
            using (var serviceScope = serviceScopeFactory.CreateScope())
            {
                var dbContext = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();
                if (dbContext == null)
                {
                    throw new NullReferenceException("Cannot get instance of dbContext");
                }

                if (dbContext.Database.GetDbConnection().ConnectionString.ToLower().Contains("my.db"))
                {
                    throw new Exception("LIVE SETTINGS IN TESTS!");
                }

                EnsureDatabase(dbContext, serviceScope.ServiceProvider);
            }
        }

        private void EnsureDatabase(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();

            if (!dbContext.Teams.Any() || !dbContext.Tournaments.Any())
            {
                var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
                SeedData.Generate(dbContext, userManager);
            }
        }
    }
}