using Microsoft.AspNetCore.Identity;

namespace KooliProjekt.Data
{
    public class Ranking
    {
        public Tournament Tournament { get; set; }
        public int TournamentId {  get; set; }
        public IdentityUser User { get; set; }
        public int userId {  get; set; }
        public int TotalPoints { get; set; }
    }
}
