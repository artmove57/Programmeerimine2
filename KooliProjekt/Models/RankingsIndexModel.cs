using KooliProjekt.Data;
using KooliProjekt.Search;

namespace KooliProjekt.Models
{
    public class RankingsIndexModel
    {
        public PagedResult<Ranking> Data { get; set; }
        public RankingsSearch Search { get; set; }
    }
}
