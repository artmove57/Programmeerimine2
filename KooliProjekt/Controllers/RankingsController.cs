using KooliProjekt.Data;
using KooliProjekt.Services;
using Microsoft.AspNetCore.Mvc;

namespace KooliProjekt.Controllers
{
    public class RankingsController : Controller
    {
        private readonly IRankingService _rankingService;

        public RankingsController(IRankingService rankingService)
        {
            _rankingService = rankingService;
        }

        // GET: Rankings
        public async Task<IActionResult> Index(int page = 1)
        {
            var data = await _rankingService.List(page, 5);
            return View(data);
        }

        // GET: Rankings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ranking = await _rankingService.Get(id.Value);
            if (ranking == null)
            {
                return NotFound();
            }

            return View(ranking);
        }

        // GET: Rankings/Create
        public async Task<IActionResult> Create()
        {
            ViewData["TournamentId"] = await _rankingService.GetTournamentsSelectList();
            ViewData["UserId"] = await _rankingService.GetUsersSelectList();
            return View();
        }

        // POST: Rankings/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TotalPoints,TournamentId,UserId")] Ranking ranking)
        {
            ModelState.Remove("Tournament");
            ModelState.Remove("User");

            if (ModelState.IsValid)
            {
                await _rankingService.Save(ranking);
                return RedirectToAction(nameof(Index));
            }
            ViewData["TournamentId"] = await _rankingService.GetTournamentsSelectList(ranking.TournamentId);
            ViewData["UserId"] = await _rankingService.GetUsersSelectList(ranking.UserId);
            return View(ranking);
        }

        // GET: Rankings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ranking = await _rankingService.Get(id.Value);
            if (ranking == null)
            {
                return NotFound();
            }
            ViewData["TournamentId"] = await _rankingService.GetTournamentsSelectList(ranking.TournamentId);
            ViewData["UserId"] = await _rankingService.GetUsersSelectList(ranking.UserId);
            return View(ranking);
        }

        // POST: Rankings/Edit/5
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
                await _rankingService.Save(ranking);
                return RedirectToAction(nameof(Index));
            }
            ViewData["TournamentId"] = await _rankingService.GetTournamentsSelectList(ranking.TournamentId);
            ViewData["UserId"] = await _rankingService.GetUsersSelectList(ranking.UserId);
            return View(ranking);
        }

        // GET: Rankings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ranking = await _rankingService.Get(id.Value);
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
            await _rankingService.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
