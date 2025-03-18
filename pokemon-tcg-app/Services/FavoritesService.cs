using Microsoft.JSInterop;
using PokemonTcgApp.Models;
using System.Text.Json;

namespace PokemonTcgApp.Services
{
    /// <summary>
    /// Service for managing favorite Pokemon cards.
    /// Uses browser's localStorage to persist favorites between sessions.
    /// </summary>
    public class FavoritesService
    {
        private readonly IJSRuntime _jsRuntime;
        private readonly string _storageKey = "pokemon_favorites";
        private List<FavoriteCard> _favorites = new List<FavoriteCard>();
        private bool _isInitialized = false;

        /// <summary>
        /// Constructor that initializes the service with JavaScript runtime.
        /// </summary>
        public FavoritesService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        /// <summary>
        /// Initialize the service by loading favorites from localStorage.
        /// </summary>
        public async Task InitializeAsync()
        {
            if (!_isInitialized)
            {
                await LoadFavoritesAsync();
                _isInitialized = true;
            }
        }

        /// <summary>
        /// Get all favorite cards.
        /// </summary>
        /// <returns>List of favorite cards</returns>
        public async Task<List<FavoriteCard>> GetFavoritesAsync()
        {
            await InitializeAsync();
            return _favorites;
        }

        /// <summary>
        /// Check if a card is in favorites.
        /// </summary>
        /// <param name="cardId">The ID of the card to check</param>
        /// <returns>True if the card is in favorites, false otherwise</returns>
        public async Task<bool> IsFavoriteAsync(string cardId)
        {
            await InitializeAsync();
            return _favorites.Any(f => f.Card.Id == cardId);
        }

        /// <summary>
        /// Add a card to favorites.
        /// </summary>
        /// <param name="card">The card to add to favorites</param>
        public async Task AddFavoriteAsync(PokemonCard card)
        {
            await InitializeAsync();
            
            // Only add if not already in favorites
            if (!_favorites.Any(f => f.Card.Id == card.Id))
            {
                var favoriteCard = new FavoriteCard(card);
                _favorites.Add(favoriteCard);
                await SaveFavoritesAsync();
            }
        }

        /// <summary>
        /// Remove a card from favorites.
        /// </summary>
        /// <param name="cardId">The ID of the card to remove</param>
        public async Task RemoveFavoriteAsync(string cardId)
        {
            await InitializeAsync();
            
            // Find and remove the card if it exists
            var favorite = _favorites.FirstOrDefault(f => f.Card.Id == cardId);
            if (favorite != null)
            {
                _favorites.Remove(favorite);
                await SaveFavoritesAsync();
            }
        }

        /// <summary>
        /// Update a favorite card's information (notes, condition, market price).
        /// </summary>
        /// <param name="favoriteCard">The updated favorite card</param>
        public async Task UpdateFavoriteAsync(FavoriteCard favoriteCard)
        {
            await InitializeAsync();
            
            // Find and update the card if it exists
            var index = _favorites.FindIndex(f => f.Card.Id == favoriteCard.Card.Id);
            if (index >= 0)
            {
                _favorites[index] = favoriteCard;
                await SaveFavoritesAsync();
            }
        }

        /// <summary>
        /// Load favorites from localStorage.
        /// </summary>
        private async Task LoadFavoritesAsync()
        {
            try
            {
                // Get the JSON string from localStorage
                var json = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", _storageKey);
                
                // Deserialize the JSON string to a list of favorite cards
                if (!string.IsNullOrEmpty(json))
                {
                    _favorites = JsonSerializer.Deserialize<List<FavoriteCard>>(json) ?? new List<FavoriteCard>();
                }
            }
            catch (Exception ex)
            {
                // Log the error and initialize with an empty list
                Console.WriteLine($"Error loading favorites: {ex.Message}");
                _favorites = new List<FavoriteCard>();
            }
        }

        /// <summary>
        /// Save favorites to localStorage.
        /// </summary>
        private async Task SaveFavoritesAsync()
        {
            try
            {
                // Serialize the list of favorite cards to a JSON string
                var json = JsonSerializer.Serialize(_favorites);
                
                // Save the JSON string to localStorage
                await _jsRuntime.InvokeVoidAsync("localStorage.setItem", _storageKey, json);
            }
            catch (Exception ex)
            {
                // Log the error
                Console.WriteLine($"Error saving favorites: {ex.Message}");
            }
        }
    }
}

