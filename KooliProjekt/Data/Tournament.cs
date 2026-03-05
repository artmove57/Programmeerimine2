namespace KooliProjekt.Data
{
    public class Tournament
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string StartData { get; set; }
        public string EndData { get; set; }
        public string Description { get; set; }

        // Navigational properties
        public ICollection<Matches> Matches { get; set; }
        public ICollection<Ranking> Rankings { get; set; }

        public Tournament()
        {
            Matches = new List<Matches>();
            Rankings = new List<Ranking>();
        }
    }
}
