using KooliProjekt.Data;
using KooliProjekt.Search;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace KooliProjekt.Services
{
    public interface IMatchesService
    {
        Task<PagedResult<Matches>> List(int page, int pageSize, MatchesSearch search);
        Task<Matches> Get(int id);
        Task Save(Matches matches);
        Task Delete(int id);
        Task<SelectList> GetTeamsSelectList(int? selectedValue = null);
        Task<SelectList> GetTournamentsSelectList(int? selectedValue = null);
    }
}
