using KooliProjekt.Data;
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

        public async Task<PagedResult<Tournament>> List(int page, int pageSize)
        {
            return await _context.Tournaments.GetPagedAsync(page, pageSize);
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
