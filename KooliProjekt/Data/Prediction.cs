using Microsoft.AspNetCore.Identity;

namespace KooliProjekt.Data
{
    public class Prediction
    {
        public int Id { get; set; }

        // Navigational properties
        public Matches Matches { get; set; }
        public int MatchesId { get; set; }

        public IdentityUser User { get; set; }
        public string UserId { get; set; }
    }
}
