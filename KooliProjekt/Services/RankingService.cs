using KooliProjekt.Data;
using KooliProjekt.Search;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Services
{
    public class RankingService : IRankingService
    {
        private readonly ApplicationDbContext _context;

        public RankingService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<Ranking>> List(int page, int pageSize, RankingsSearch search)
        {
            var query = _context.Rankings
                .Include(r => r.Tournament)
                .Include(r => r.User)
                .AsQueryable();

            if (search != null)
            {
                if (!string.IsNullOrWhiteSpace(search.TournamentName))
                {
                    query = query.Where(r => r.Tournament.Name.Contains(search.TournamentName));
                }

                if (!string.IsNullOrWhiteSpace(search.UserEmail))
                {
                    query = query.Where(r => r.User.Email.Contains(search.UserEmail));
                }

                if (search.MinPoints.HasValue)
                {
                    query = query.Where(r => r.TotalPoints >= search.MinPoints.Value);
                }
            }

            return await query.GetPagedAsync(page, pageSize);
        }

        public async Task<Ranking> Get(int id)
        {
            return await _context.Rankings
                .Include(r => r.Tournament)
                .Include(r => r.User)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task Save(Ranking ranking)
        {
            if (ranking.Id == 0)
            {
                _context.Add(ranking);
            }
            else
            {
                _context.Update(ranking);
            }

            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var ranking = await _context.Rankings.FindAsync(id);
            if (ranking != null)
            {
                _context.Rankings.Remove(ranking);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<SelectList> GetTournamentsSelectList(int? selectedValue = null)
        {
            var tournaments = await _context.Tournaments.ToListAsync();
            return new SelectList(tournaments, "Id", "Name", selectedValue);
        }

        public async Task<SelectList> GetUsersSelectList(string selectedValue = null)
        {
            var users = await _context.Users.ToListAsync();
            return new SelectList(users, "Id", "Email", selectedValue);
        }
    }
}
