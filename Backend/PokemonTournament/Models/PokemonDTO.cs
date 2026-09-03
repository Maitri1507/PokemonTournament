namespace PokemonTournament.Models
{
    public class PokemonDTO
    {
        public PokemonDTO(int id, string name, string type, int wins, int losses, int ties)
        {
            Id = id;
            Name = name;
            Type = type;
            Wins = wins;
            Losses = losses;
            Ties = ties;
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public int Wins { get; set; }
        public int Losses { get; set; }
        public int Ties { get; set; }
        public string Type { get; set; }
    }
}
