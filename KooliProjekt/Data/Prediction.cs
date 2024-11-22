using Microsoft.AspNetCore.Identity;

namespace KooliProjekt.Data
{
    public class Prediction
    {
        public int Id {  get; set; }
        public Matches Matches { get; set; }
        public int MatchId { get; set; }
        public IdentityUser User { get; set; }
        public string UserId { get; set; }
    }
}
