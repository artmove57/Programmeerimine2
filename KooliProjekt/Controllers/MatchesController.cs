using KooliProjekt.Data;
using KooliProjekt.Models;
using KooliProjekt.Search;
using KooliProjekt.Services;
using Microsoft.AspNetCore.Mvc;

namespace KooliProjekt.Controllers
{
    public class MatchesController : Controller
    {
        private readonly IMatchesService _matchesService;

        public MatchesController(IMatchesService matchesService)
        {
            _matchesService = matchesService;
        }

        public async Task<IActionResult> Index(int page = 1, MatchesSearch search = null)
        {
            var model = new MatchesIndexModel
            {
                Data = await _matchesService.List(page, 5, search),
                Search = search ?? new MatchesSearch()
            };
            return View(model);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var matches = await _matchesService.Get(id.Value);
            if (matches == null)
            {
                return NotFound();
            }

            return View(matches);
        }

        public async Task<IActionResult> Create()
        {
            ViewData["TeamId"] = await _matchesService.GetTeamsSelectList();
            ViewData["TournamentId"] = await _matchesService.GetTournamentsSelectList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,StartData,EndData,TotalPoints,TeamId,TournamentId")] Matches matches)
        {
            ModelState.Remove("Team");
            ModelState.Remove("Tournament");
            ModelState.Remove("Predictions");

            if (ModelState.IsValid)
            {
                await _matchesService.Save(matches);
                return RedirectToAction(nameof(Index));
            }
            ViewData["TeamId"] = await _matchesService.GetTeamsSelectList(matches.TeamId);
            ViewData["TournamentId"] = await _matchesService.GetTournamentsSelectList(matches.TournamentId);
            return View(matches);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var matches = await _matchesService.Get(id.Value);
            if (matches == null)
            {
                return NotFound();
            }
            ViewData["TeamId"] = await _matchesService.GetTeamsSelectList(matches.TeamId);
            ViewData["TournamentId"] = await _matchesService.GetTournamentsSelectList(matches.TournamentId);
            return View(matches);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,StartData,EndData,TotalPoints,TeamId,TournamentId")] Matches matches)
        {
            if (id != matches.Id)
            {
                return NotFound();
            }

            ModelState.Remove("Team");
            ModelState.Remove("Tournament");
            ModelState.Remove("Predictions");

            if (ModelState.IsValid)
            {
                await _matchesService.Save(matches);
                return RedirectToAction(nameof(Index));
            }
            ViewData["TeamId"] = await _matchesService.GetTeamsSelectList(matches.TeamId);
            ViewData["TournamentId"] = await _matchesService.GetTournamentsSelectList(matches.TournamentId);
            return View(matches);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var matches = await _matchesService.Get(id.Value);
            if (matches == null)
            {
                return NotFound();
            }

            return View(matches);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _matchesService.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
