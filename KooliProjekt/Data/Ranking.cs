using Microsoft.AspNetCore.Identity;

namespace KooliProjekt.Data
{
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
