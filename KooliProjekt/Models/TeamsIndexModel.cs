using KooliProjekt.Data;
using KooliProjekt.Search;

namespace KooliProjekt.Models
{
    public class TeamsIndexModel
    {
        public PagedResult<Team> Data { get; set; }
        public TeamsSearch Search { get; set; }
    }
}
