using KooliProjekt.Data;
using Microsoft.AspNetCore.Identity;

namespace KooliProjekt.Data
{
    public static class SeedData
    {
        public static void Generate(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            if (context.Matches.Any())
            {
                return;
            }

            var user = new IdentityUser
            {
                UserName = "newuser@example.com",
                Email = "newuser@example.com",
                NormalizedUserName = "NEWUSER@EXAMPLE.COM",
                NormalizedEmail = "NEWUSER@EXAMPLE.COM"
            };

            userManager.CreateAsync(user, "Password123!").Wait();

            var tournament1 = new Tournament
            {
                Name = "Bundesliga",
                StartData = "2024-08-01",
                EndData = "2025-05-31",
                Description = "Football Match in German, 22 players are playing"
            };
            context.Tournaments.Add(tournament1);

            var team1 = new Team
            {
                Name = "Francfurt Eintraht"
            };
            context.Teams.Add(team1);

            context.SaveChanges();

            var matches1 = new Matches
            {
                Name = "Match 1",
                StartData = "2024-08-15",
                EndData = "2024-08-15",
                TotalPoints = 0,
                TeamId = team1.Id,
                TournamentId = tournament1.Id
            };
            context.Matches.Add(matches1);

            var ranking1 = new Ranking
            {
                TotalPoints = 100,
                TournamentId = tournament1.Id,
                UserId = user.Id
            };
            context.Rankings.Add(ranking1);

            context.SaveChanges();
        }
    }
}