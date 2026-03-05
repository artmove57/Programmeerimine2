using KooliProjekt.Data;
using KooliProjekt.Search;
using System.Diagnostics.CodeAnalysis;

namespace KooliProjekt.Models
{
    [ExcludeFromCodeCoverage]
    public class RankingsIndexModel
    {
        public PagedResult<Ranking> Data { get; set; }
        public RankingsSearch Search { get; set; }
    }
}
