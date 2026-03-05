using Microsoft.AspNetCore.Identity;
using System.Diagnostics.CodeAnalysis;

namespace KooliProjekt.Data
{
    [ExcludeFromCodeCoverage]
    public class Ranking
    {
        public int Id { get; set; }
        public int TotalPoints { get; set; }

        // Navigational properties
        public Tournament Tournament { get; set; }
        public int TournamentId { get; set; }

        public IdentityUser User { get; set; }
        public string UserId { get; set; }
    }
}
