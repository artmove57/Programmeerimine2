using KooliProjekt.Data;
using KooliProjekt.Search;
using System.Diagnostics.CodeAnalysis;

namespace KooliProjekt.Models
{
    [ExcludeFromCodeCoverage]
    public class MatchesIndexModel
    {
        public PagedResult<Matches> Data { get; set; }
        public MatchesSearch Search { get; set; }
    }
}
