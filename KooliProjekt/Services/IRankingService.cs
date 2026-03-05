using KooliProjekt.Data;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace KooliProjekt.Services
{
    public interface IRankingService
    {
        Task<PagedResult<Ranking>> List(int page, int pageSize);
        Task<Ranking> Get(int id);
        Task Save(Ranking ranking);
        Task Delete(int id);
        Task<SelectList> GetTournamentsSelectList(int? selectedValue = null);
        Task<SelectList> GetUsersSelectList(string selectedValue = null);
    }
}
