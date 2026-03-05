using KooliProjekt.Data;
using KooliProjekt.Search;

namespace KooliProjekt.Models
{
    public class MatchesIndexModel
    {
        public PagedResult<Matches> Data { get; set; }
        public MatchesSearch Search { get; set; }
    }
}
