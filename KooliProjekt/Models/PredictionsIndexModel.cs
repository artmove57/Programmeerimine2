using KooliProjekt.Data;
using KooliProjekt.Search;

namespace KooliProjekt.Models
{
    public class PredictionsIndexModel
    {
        public PagedResult<Prediction> Data { get; set; }
        public PredictionsSearch Search { get; set; }
    }
}
