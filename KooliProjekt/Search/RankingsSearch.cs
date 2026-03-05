using System.Diagnostics.CodeAnalysis;

namespace KooliProjekt.Search
{
    [ExcludeFromCodeCoverage]
    public class RankingsSearch
    {
        public string TournamentName { get; set; }
        public string UserEmail { get; set; }
        public int? MinPoints { get; set; }
    }
}
