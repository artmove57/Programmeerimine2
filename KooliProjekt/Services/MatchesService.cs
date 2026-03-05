using KooliProjekt.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Services
{
    public class MatchesService : IMatchesService
    {
        private readonly ApplicationDbContext _context;

        public MatchesService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<Matches>> List(int page, int pageSize)
        {
            var query = _context.Matches
                .Include(m => m.Team)
                .Include(m => m.Tournament);
            return await query.GetPagedAsync(page, pageSize);
        }

        public async Task<Matches> Get(int id)
        {
            return await _context.Matches
                .Include(m => m.Team)
                .Include(m => m.Tournament)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task Save(Matches matches)
        {
            if (matches.Id == 0)
            {
                _context.Add(matches);
            }
            else
            {
                _context.Update(matches);
            }

            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var matches = await _context.Matches.FindAsync(id);
            if (matches != null)
            {
                _context.Matches.Remove(matches);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<SelectList> GetTeamsSelectList(int? selectedValue = null)
        {
            var teams = await _context.Teams.ToListAsync();
            return new SelectList(teams, "Id", "Name", selectedValue);
        }

        public async Task<SelectList> GetTournamentsSelectList(int? selectedValue = null)
        {
            var tournaments = await _context.Tournaments.ToListAsync();
            return new SelectList(tournaments, "Id", "Name", selectedValue);
        }
    }
}
