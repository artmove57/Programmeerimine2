using KooliProjekt.Data;
using KooliProjekt.Search;
using System.Diagnostics.CodeAnalysis;

namespace KooliProjekt.Models
{
    [ExcludeFromCodeCoverage]
    public class TournamentsIndexModel
    {
        public PagedResult<Tournament> Data { get; set; }
        public TournamentsSearch Search { get; set; }
    }
}
