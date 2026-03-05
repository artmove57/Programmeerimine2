using System.Diagnostics.CodeAnalysis;

namespace KooliProjekt.Data
{
    [ExcludeFromCodeCoverage]
    public class Team
    {
        public int Id { get; set; }
        public string Name { get; set; }

        // Navigational property
        public ICollection<Matches> Matches { get; set; }

        public Team()
        {
            Matches = new List<Matches>();
        }
    }
}




