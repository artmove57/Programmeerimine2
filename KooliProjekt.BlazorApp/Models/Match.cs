using System.ComponentModel.DataAnnotations;

namespace KooliProjekt.BlazorApp.Models
{
    /// <summary>
    /// Match model with validation attributes
    /// </summary>
    public class Match
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Match name is required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Match name must be between 2 and 100 characters")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Start date is required")]
        public string StartData { get; set; } = string.Empty;

        [Required(ErrorMessage = "End date is required")]
        public string EndData { get; set; } = string.Empty;

        [Range(0, 1000, ErrorMessage = "Total points must be between 0 and 1000")]
        public int TotalPoints { get; set; }

        [Required(ErrorMessage = "Team is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a team")]
        public int TeamId { get; set; }

        [Required(ErrorMessage = "Tournament is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a tournament")]
        public int TournamentId { get; set; }

        // Navigation properties (not sent to API, used for display)
        public Team? Team { get; set; }
        public Tournament? Tournament { get; set; }
    }

    /// <summary>
    /// Tournament model for dropdown
    /// </summary>
    public class Tournament
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
