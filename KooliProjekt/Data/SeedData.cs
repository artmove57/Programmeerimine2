using KooliProjekt.Data;
using Microsoft.AspNetCore.Identity;
using System.Diagnostics.CodeAnalysis;

namespace KooliProjekt.Data
{
    [ExcludeFromCodeCoverage]
    public static class SeedData
    {
        public static void Generate(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            // Check if data already exists
            if (context.Teams.Any())
            {
                return;
            }

            // Create 10 users
            var users = new List<IdentityUser>();
            for (int i = 1; i <= 10; i++)
            {
                var user = new IdentityUser
                {
                    UserName = $"user{i}@example.com",
                    Email = $"user{i}@example.com",
                    EmailConfirmed = true
                };
                userManager.CreateAsync(user, "Password123!").Wait();
                users.Add(user);
            }

            // Create 10 Tournaments
            var tournaments = new List<Tournament>();
            var tournamentNames = new[] { "Premier League", "La Liga", "Bundesliga", "Serie A", "Ligue 1", 
                                          "Champions League", "Europa League", "World Cup", "Euro Cup", "Copa America" };
            for (int i = 0; i < 10; i++)
            {
                var tournament = new Tournament
                {
                    Name = tournamentNames[i],
                    StartData = $"2024-{(i % 12) + 1:D2}-01",
                    EndData = $"2025-{(i % 12) + 1:D2}-01",
                    Description = $"Major football tournament - {tournamentNames[i]}"
                };
                tournaments.Add(tournament);
                context.Tournaments.Add(tournament);
            }

            // Create 10 Teams
            var teams = new List<Team>();
            var teamNames = new[] { "Manchester United", "Real Madrid", "Bayern Munich", "Barcelona", "Liverpool",
                                   "Juventus", "Paris SG", "Chelsea", "Arsenal", "AC Milan" };
            for (int i = 0; i < 10; i++)
            {
                var team = new Team
                {
                    Name = teamNames[i]
                };
                teams.Add(team);
                context.Teams.Add(team);
            }

            context.SaveChanges();

            // Create 15 Matches (more than 10 to show paging)
            var matches = new List<Matches>();
            for (int i = 1; i <= 15; i++)
            {
                var match = new Matches
                {
                    Name = $"Match {i}",
                    StartData = $"2024-{(i % 12) + 1:D2}-{(i % 28) + 1:D2}",
                    EndData = $"2024-{(i % 12) + 1:D2}-{(i % 28) + 1:D2}",
                    TotalPoints = i * 10,
                    TeamId = teams[i % 10].Id,
                    TournamentId = tournaments[i % 10].Id
                };
                matches.Add(match);
                context.Matches.Add(match);
            }

            context.SaveChanges();

            // Create 12 Predictions
            for (int i = 0; i < 12; i++)
            {
                var prediction = new Prediction
                {
                    MatchesId = matches[i % matches.Count].Id,
                    UserId = users[i % users.Count].Id
                };
                context.Predictions.Add(prediction);
            }

            // Create 12 Rankings
            for (int i = 0; i < 12; i++)
            {
                var ranking = new Ranking
                {
                    TotalPoints = (i + 1) * 50,
                    TournamentId = tournaments[i % tournaments.Count].Id,
                    UserId = users[i % users.Count].Id
                };
                context.Rankings.Add(ranking);
            }

            context.SaveChanges();
        }
    }
}