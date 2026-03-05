using KooliProjekt.Data;
using KooliProjekt.Services;
using Microsoft.AspNetCore.Mvc;

namespace KooliProjekt.Controllers
{
    public class TournamentsController : Controller
    {
        private readonly ITournamentService _tournamentService;

        public TournamentsController(ITournamentService tournamentService)
        {
            _tournamentService = tournamentService;
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            var data = await _tournamentService.List(page, 5);
            return View(data);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tournament = await _tournamentService.Get(id.Value);
            if (tournament == null)
            {
                return NotFound();
            }

            return View(tournament);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,StartData,EndData,Description")] Tournament tournament)
        {
            if (ModelState.IsValid)
            {
                await _tournamentService.Save(tournament);
                return RedirectToAction(nameof(Index));
            }
            return View(tournament);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tournament = await _tournamentService.Get(id.Value);
            if (tournament == null)
            {
                return NotFound();
            }
            return View(tournament);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,StartData,EndData,Description")] Tournament tournament)
        {
            if (id != tournament.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _tournamentService.Save(tournament);
                return RedirectToAction(nameof(Index));
            }
            return View(tournament);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tournament = await _tournamentService.Get(id.Value);
            if (tournament == null)
            {
                return NotFound();
            }

            return View(tournament);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _tournamentService.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
