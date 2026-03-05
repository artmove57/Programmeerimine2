using KooliProjekt.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Services
{
    public class PredictionService : IPredictionService
    {
        private readonly ApplicationDbContext _context;

        public PredictionService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<Prediction>> List(int page, int pageSize)
        {
            var query = _context.Predictions
                .Include(p => p.Matches)
                .Include(p => p.User);
            return await query.GetPagedAsync(page, pageSize);
        }

        public async Task<Prediction> Get(int id)
        {
            return await _context.Predictions
                .Include(p => p.Matches)
                .Include(p => p.User)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task Save(Prediction prediction)
        {
            if (prediction.Id == 0)
            {
                _context.Add(prediction);
            }
            else
            {
                _context.Update(prediction);
            }

            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var prediction = await _context.Predictions.FindAsync(id);
            if (prediction != null)
            {
                _context.Predictions.Remove(prediction);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<SelectList> GetMatchesSelectList(int? selectedValue = null)
        {
            var matches = await _context.Matches.ToListAsync();
            return new SelectList(matches, "Id", "Name", selectedValue);
        }

        public async Task<SelectList> GetUsersSelectList(string selectedValue = null)
        {
            var users = await _context.Users.ToListAsync();
            return new SelectList(users, "Id", "Email", selectedValue);
        }
    }
}
