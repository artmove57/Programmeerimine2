using KooliProjekt.Data;
using KooliProjekt.Models;
using KooliProjekt.Search;
using KooliProjekt.Services;
using Microsoft.AspNetCore.Mvc;

namespace KooliProjekt.Controllers
{
    public class PredictionsController : Controller
    {
        private readonly IPredictionService _predictionService;

        public PredictionsController(IPredictionService predictionService)
        {
            _predictionService = predictionService;
        }

        public async Task<IActionResult> Index(int page = 1, PredictionsSearch search = null)
        {
            var model = new PredictionsIndexModel
            {
                Data = await _predictionService.List(page, 5, search),
                Search = search ?? new PredictionsSearch()
            };
            return View(model);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var prediction = await _predictionService.Get(id.Value);
            if (prediction == null)
            {
                return NotFound();
            }

            return View(prediction);
        }

        public async Task<IActionResult> Create()
        {
            ViewData["MatchesId"] = await _predictionService.GetMatchesSelectList();
            ViewData["UserId"] = await _predictionService.GetUsersSelectList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,MatchesId,UserId")] Prediction prediction)
        {
            ModelState.Remove("Matches");
            ModelState.Remove("User");

            if (ModelState.IsValid)
            {
                await _predictionService.Save(prediction);
                return RedirectToAction(nameof(Index));
            }
            ViewData["MatchesId"] = await _predictionService.GetMatchesSelectList(prediction.MatchesId);
            ViewData["UserId"] = await _predictionService.GetUsersSelectList(prediction.UserId);
            return View(prediction);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var prediction = await _predictionService.Get(id.Value);
            if (prediction == null)
            {
                return NotFound();
            }
            ViewData["MatchesId"] = await _predictionService.GetMatchesSelectList(prediction.MatchesId);
            ViewData["UserId"] = await _predictionService.GetUsersSelectList(prediction.UserId);
            return View(prediction);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,MatchesId,UserId")] Prediction prediction)
        {
            if (id != prediction.Id)
            {
                return NotFound();
            }

            ModelState.Remove("Matches");
            ModelState.Remove("User");

            if (ModelState.IsValid)
            {
                await _predictionService.Save(prediction);
                return RedirectToAction(nameof(Index));
            }
            ViewData["MatchesId"] = await _predictionService.GetMatchesSelectList(prediction.MatchesId);
            ViewData["UserId"] = await _predictionService.GetUsersSelectList(prediction.UserId);
            return View(prediction);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var prediction = await _predictionService.Get(id.Value);
            if (prediction == null)
            {
                return NotFound();
            }

            return View(prediction);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _predictionService.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
