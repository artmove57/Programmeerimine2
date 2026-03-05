using KooliProjekt.Data;
using KooliProjekt.Search;
using System.Diagnostics.CodeAnalysis;

namespace KooliProjekt.Models
{
    [ExcludeFromCodeCoverage]
    public class TeamsIndexModel
    {
        public PagedResult<Team> Data { get; set; }
        public TeamsSearch Search { get; set; }
    }
}
