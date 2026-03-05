using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Team> Teams { get; set; }
        public DbSet<Tournament> Tournaments { get; set; }
        public DbSet<Matches> Matches { get; set; }
        public DbSet<Prediction> Predictions { get; set; }
        public DbSet<Ranking> Rankings { get; set; }
    }
}