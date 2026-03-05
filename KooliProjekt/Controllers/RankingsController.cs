using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using KooliProjekt.Data;

namespace KooliProjekt.Controllers
{
    public class RankingsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RankingsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Rankings
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Rankings.Include(r => r.Tournament).Include(r => r.User);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Rankings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ranking = await _context.Rankings
                .Include(r => r.Tournament)
                .Include(r => r.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ranking == null)
            {
                return NotFound();
            }

            return View(ranking);
        }

        // GET: Rankings/Create
        public IActionResult Create()
        {
            ViewData["TournamentId"] = new SelectList(_context.Tournaments, "Id", "Name");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Email");
            return View();
        }

        // POST: Rankings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TotalPoints,TournamentId,UserId")] Ranking ranking)
        {
            ModelState.Remove("Tournament");
            ModelState.Remove("User");

            if (ModelState.IsValid)
            {
                _context.Add(ranking);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["TournamentId"] = new SelectList(_context.Tournaments, "Id", "Name", ranking.TournamentId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Email", ranking.UserId);
            return View(ranking);
        }

        // GET: Rankings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ranking = await _context.Rankings.FindAsync(id);
            if (ranking == null)
            {
                return NotFound();
            }
            ViewData["TournamentId"] = new SelectList(_context.Tournaments, "Id", "Name", ranking.TournamentId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Email", ranking.UserId);
            return View(ranking);
        }

        // POST: Rankings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TotalPoints,TournamentId,UserId")] Ranking ranking)
        {
            if (id != ranking.Id)
            {
                return NotFound();
            }

            ModelState.Remove("Tournament");
            ModelState.Remove("User");

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ranking);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RankingExists(ranking.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["TournamentId"] = new SelectList(_context.Tournaments, "Id", "Name", ranking.TournamentId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Email", ranking.UserId);
            return View(ranking);
        }

        // GET: Rankings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ranking = await _context.Rankings
                .Include(r => r.Tournament)
                .Include(r => r.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ranking == null)
            {
                return NotFound();
            }

            return View(ranking);
        }

        // POST: Rankings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ranking = await _context.Rankings.FindAsync(id);
            if (ranking != null)
            {
                _context.Rankings.Remove(ranking);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RankingExists(int id)
        {
            return _context.Rankings.Any(e => e.Id == id);
        }
    }
}
