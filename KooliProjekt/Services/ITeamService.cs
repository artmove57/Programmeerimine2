using KooliProjekt.Data;
using KooliProjekt.Search;

namespace KooliProjekt.Services
{
    public interface ITeamService
    {
        Task<PagedResult<Team>> List(int page, int pageSize, TeamsSearch search);
        Task<Team> Get(int id);
        Task Save(Team team);
        Task Delete(int id);
    }
}
