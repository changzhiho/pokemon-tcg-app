namespace PokemonTcgApp.Models
{
    /// <summary>
    /// Represents a Pokemon card that has been added to favorites.
    /// Extends the basic PokemonCard with additional user-specific information.
    /// </summary>
    public class FavoriteCard
    {
        // The Pokemon card that has been favorited
        public PokemonCard Card { get; set; } = new PokemonCard();
        
        // User's notes about this card
        public string Notes { get; set; } = string.Empty;
        
        // Condition of the card (e.g., Mint, Near Mint, etc.)
        public string Condition { get; set; } = string.Empty;
        
        // Market price of the card as entered by the user
        public string MarketPrice { get; set; } = string.Empty;

        // Default constructor
        public FavoriteCard() { }

        // Constructor that initializes with a Pokemon card
        public FavoriteCard(PokemonCard card)
        {
            Card = card;
        }
    }
}

