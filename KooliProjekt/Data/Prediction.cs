using Microsoft.AspNetCore.Identity;
using System.Diagnostics.CodeAnalysis;

namespace KooliProjekt.Data
{
    [ExcludeFromCodeCoverage]
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
