namespace KooliProjekt.Data
{
    public class Matches
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string StartData { get; set; }
        public string EndData { get; set; }
        public int TotalPoints { get; set; }

        // Navigational properties
        public Team Team { get; set; }
        public int TeamId { get; set; }

        public Tournament Tournament { get; set; }
        public int TournamentId { get; set; }

        public ICollection<Prediction> Predictions { get; set; }

        public Matches()
        {
            Predictions = new List<Prediction>();
        }
    }
}
