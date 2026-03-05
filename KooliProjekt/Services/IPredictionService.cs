using KooliProjekt.Data;
using KooliProjekt.Search;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace KooliProjekt.Services
{
    public interface IPredictionService
    {
        Task<PagedResult<Prediction>> List(int page, int pageSize, PredictionsSearch search);
        Task<Prediction> Get(int id);
        Task Save(Prediction prediction);
        Task Delete(int id);
        Task<SelectList> GetMatchesSelectList(int? selectedValue = null);
        Task<SelectList> GetUsersSelectList(string selectedValue = null);
    }
}
