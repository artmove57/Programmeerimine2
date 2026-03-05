using KooliProjekt.Data;
using KooliProjekt.Search;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace KooliProjekt.Services
{
    public interface IRankingService
    {
        Task<PagedResult<Ranking>> List(int page, int pageSize, RankingsSearch search);
        Task<Ranking> Get(int id);
        Task Save(Ranking ranking);
        Task Delete(int id);
        Task<SelectList> GetTournamentsSelectList(int? selectedValue = null);
        Task<SelectList> GetUsersSelectList(string selectedValue = null);
    }
}
