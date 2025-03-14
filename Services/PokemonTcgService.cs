using System.Net.Http.Headers;
using System.Text.Json;
using PokemonTcgApp.Models;

namespace PokemonTcgApp.Services
{
    /// <summary>
    /// Service for interacting with the Pokemon TCG API.
    /// Provides methods to search for cards and get card details.
    /// </summary>
    public class PokemonTcgService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey = "VOTRE-CLE-API"; // Replace with your API key
        private readonly string _baseUrl = "https://api.pokemontcg.io/v2";

        /// <summary>
        /// Constructor that initializes the service with an HttpClient.
        /// </summary>
        public PokemonTcgService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.DefaultRequestHeaders.Add("X-Api-Key", _apiKey);
        }

        /// <summary>
        /// Search for Pokemon cards based on a query string.
        /// </summary>
        /// <param name="query">Search query (card name)</param>
        /// <param name="page">Page number for pagination</param>
        /// <param name="pageSize">Number of cards per page</param>
        /// <returns>List of Pokemon cards matching the search criteria</returns>
        public async Task<List<PokemonCard>> SearchCards(string query, int page = 1, int pageSize = 20)
        {
            try
            {
                // Build search query - if empty, return all cards
                string searchQuery = string.IsNullOrWhiteSpace(query) 
                    ? "" 
                    : $"&q=name:{query}*";
                
                // Construct the full URL with pagination parameters
                string url = $"{_baseUrl}/cards?page={page}&pageSize={pageSize}{searchQuery}";
                
                // Make the API request
                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();
                
                // Parse the JSON response
                var content = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<PokemonCardResponse>(content);
                
                // Return the list of cards or an empty list if null
                return result?.Data ?? new List<PokemonCard>();
            }
            catch (Exception ex)
            {
                // Log the error and return an empty list
                Console.WriteLine($"Error searching cards: {ex.Message}");
                return new List<PokemonCard>();
            }
        }

        /// <summary>
        /// Get a specific Pokemon card by its ID.
        /// </summary>
        /// <param name="id">The unique identifier of the card</param>
        /// <returns>The Pokemon card or null if not found</returns>
        public async Task<PokemonCard?> GetCardById(string id)
        {
            try
            {
                // Construct the URL for the specific card
                string url = $"{_baseUrl}/cards/{id}";
                
                // Make the API request
                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();
                
                // Parse the JSON response
                var content = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<SingleCardResponse>(content);
                
                // Return the card or null
                return result?.Data;
            }
            catch (Exception ex)
            {
                // Log the error and return null
                Console.WriteLine($"Error getting card by ID: {ex.Message}");
                return null;
            }
        }
    }

    /// <summary>
    /// Represents the response from the Pokemon TCG API when requesting a single card.
    /// </summary>
    public class SingleCardResponse
    {
        public PokemonCard? Data { get; set; }
    }
}

