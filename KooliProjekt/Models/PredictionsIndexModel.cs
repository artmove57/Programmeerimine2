using KooliProjekt.Data;
using KooliProjekt.Search;
using System.Diagnostics.CodeAnalysis;

namespace KooliProjekt.Models
{
    [ExcludeFromCodeCoverage]
    public class PredictionsIndexModel
    {
        public PagedResult<Prediction> Data { get; set; }
        public PredictionsSearch Search { get; set; }
    }
}
