namespace KooliProjekt.Data
{
    public class Matches
    {
        public int Id { get; set; }
        public Team Team { get; set; }
        public string Name { get; set; }
        public Tournament Tournament { get; set; }
        public string StartData { get; set; }
        public string EndData { get; set; }
        public Ranking Ranking { get; set; }
        public int TotalPoints { get; set; }
        public Prediction Prediction { get; set; }
        public int PredictionId { get; set; }

    }
}
