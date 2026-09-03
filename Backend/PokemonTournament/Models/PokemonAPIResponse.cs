using System.Text.Json.Serialization;

namespace PokemonTournament.Models
{
    public class PokemonAPIResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("base_experience")]
        public int BaseExperience { get; set; }
        public List<PokemonSlotTypeInfo> Types { get; set; } = new List<PokemonSlotTypeInfo>();
    }

    public class PokemonSlotTypeInfo
    {
        public int Slot { get; set; }

        [JsonPropertyName("type")]
        public PokemonAPIType PokemonAPIType { get; set; } = new PokemonAPIType();
    }
    public class PokemonAPIType
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
    }
}
