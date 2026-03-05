using KooliProjekt.Data;
using KooliProjekt.Search;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Services
{
    public class TeamService : ITeamService
    {
        private readonly ApplicationDbContext _context;

        public TeamService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<Team>> List(int page, int pageSize, TeamsSearch search)
        {
            var query = _context.Teams.AsQueryable();

            if (search != null)
            {
                if (!string.IsNullOrWhiteSpace(search.Name))
                {
                    query = query.Where(t => t.Name.Contains(search.Name));
                }
            }

            return await query.GetPagedAsync(page, pageSize);
        }

        public async Task<Team> Get(int id)
        {
            return await _context.Teams.FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task Save(Team team)
        {
            if (team.Id == 0)
            {
                _context.Add(team);
            }
            else
            {
                _context.Update(team);
            }

            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var team = await _context.Teams.FindAsync(id);
            if (team != null)
            {
                _context.Teams.Remove(team);
                await _context.SaveChangesAsync();
            }
        }
    }
}
