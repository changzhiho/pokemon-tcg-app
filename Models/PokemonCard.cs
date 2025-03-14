using System.Text.Json.Serialization;

namespace PokemonTcgApp.Models
{
    /// <summary>
    /// Represents a Pokemon card from the Pokemon TCG API.
    /// This class maps to the JSON response from the API.
    /// </summary>
    public class PokemonCard
    {
        // Unique identifier for the card
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        // Name of the Pokemon card
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        // Supertype of the card (e.g., Pokémon, Trainer, Energy)
        [JsonPropertyName("supertype")]
        public string Supertype { get; set; } = string.Empty;

        // Subtypes of the card (e.g., Basic, Stage 1, Item)
        [JsonPropertyName("subtypes")]
        public List<string> Subtypes { get; set; } = new List<string>();

        // Hit points of the Pokemon
        [JsonPropertyName("hp")]
        public string Hp { get; set; } = string.Empty;

        // Types of the Pokemon (e.g., Fire, Water)
        [JsonPropertyName("types")]
        public List<string> Types { get; set; } = new List<string>();

        // Images of the card
        [JsonPropertyName("images")]
        public CardImages Images { get; set; } = new CardImages();

        // Rarity of the card (e.g., Common, Uncommon, Rare)
        [JsonPropertyName("rarity")]
        public string Rarity { get; set; } = string.Empty;
    }

    /// <summary>
    /// Represents the images associated with a Pokemon card.
    /// </summary>
    public class CardImages
    {
        // URL to the small image of the card
        [JsonPropertyName("small")]
        public string Small { get; set; } = string.Empty;

        // URL to the large image of the card
        [JsonPropertyName("large")]
        public string Large { get; set; } = string.Empty;
    }

    /// <summary>
    /// Represents the response from the Pokemon TCG API when searching for cards.
    /// Contains a list of cards and pagination information.
    /// </summary>
    public class PokemonCardResponse
    {
        // List of Pokemon cards returned by the API
        [JsonPropertyName("data")]
        public List<PokemonCard> Data { get; set; } = new List<PokemonCard>();

        // Current page number
        [JsonPropertyName("page")]
        public int Page { get; set; }

        // Number of cards per page
        [JsonPropertyName("pageSize")]
        public int PageSize { get; set; }

        // Number of cards in the current page
        [JsonPropertyName("count")]
        public int Count { get; set; }

        // Total number of cards matching the search criteria
        [JsonPropertyName("totalCount")]
        public int TotalCount { get; set; }
    }
}

