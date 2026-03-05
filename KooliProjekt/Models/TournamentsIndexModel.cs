using KooliProjekt.Data;
using KooliProjekt.Search;

namespace KooliProjekt.Models
{
    public class TournamentsIndexModel
    {
        public PagedResult<Tournament> Data { get; set; }
        public TournamentsSearch Search { get; set; }
    }
}
