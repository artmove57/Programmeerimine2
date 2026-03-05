using KooliProjekt.Data;
using KooliProjekt.Search;

namespace KooliProjekt.Services
{
    public interface ITournamentService
    {
        Task<PagedResult<Tournament>> List(int page, int pageSize, TournamentsSearch search);
        Task<Tournament> Get(int id);
        Task Save(Tournament tournament);
        Task Delete(int id);
    }
}
