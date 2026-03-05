using KooliProjekt.Data;
using KooliProjekt.Search;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Services
{
    public class TournamentService : ITournamentService
    {
        private readonly ApplicationDbContext _context;

        public TournamentService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<Tournament>> List(int page, int pageSize, TournamentsSearch search)
        {
            var query = _context.Tournaments.AsQueryable();

            if (search != null)
            {
                if (!string.IsNullOrWhiteSpace(search.Name))
                {
                    query = query.Where(t => t.Name.Contains(search.Name));
                }

                if (!string.IsNullOrWhiteSpace(search.Description))
                {
                    query = query.Where(t => t.Description.Contains(search.Description));
                }
            }

            return await query.GetPagedAsync(page, pageSize);
        }

        public async Task<Tournament> Get(int id)
        {
            return await _context.Tournaments.FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task Save(Tournament tournament)
        {
            if (tournament.Id == 0)
            {
                _context.Add(tournament);
            }
            else
            {
                _context.Update(tournament);
            }

            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var tournament = await _context.Tournaments.FindAsync(id);
            if (tournament != null)
            {
                _context.Tournaments.Remove(tournament);
                await _context.SaveChangesAsync();
            }
        }
    }
}
