using System.Text.Json;
using Microsoft.JSInterop;
using Moq;
using PokemonTcgApp.Models;
using PokemonTcgApp.Services;
using Xunit;

namespace PokemonTcgApp.Tests
{
    public class FavoritesServiceTests
    {
        private readonly Mock<IJSRuntime> _mockJsRuntime;
        private readonly FavoritesService _favoritesService;
        private readonly PokemonCard _testCard;

        public FavoritesServiceTests()
        {
            // Mock JavaScript runtime to simulate localStorage behavior
            _mockJsRuntime = new Mock<IJSRuntime>();
            _favoritesService = new FavoritesService(_mockJsRuntime.Object);
            
            // Create a sample Pokémon card for testing
            _testCard = new PokemonCard
            {
                Id = "test-id-1",
                Name = "Pikachu",
                Supertype = "Pokémon",
                Subtypes = new List<string> { "Basic" },
                Hp = "70",
                Types = new List<string> { "Lightning" },
                Images = new CardImages
                {
                    Small = "https://example.com/pikachu-small.jpg",
                    Large = "https://example.com/pikachu-large.jpg"
                },
                Rarity = "Common"
            };
        }

        [Fact]
        public async Task GetFavoritesAsync_ReturnsEmptyList_WhenNoFavoritesExist()
        {
            // Arrange: Simulate an empty localStorage (no saved favorites)
            _mockJsRuntime
                .Setup(js => js.InvokeAsync<string>("localStorage.getItem", It.IsAny<object[]>()))
                .ReturnsAsync((string)null);

            // Act: Call the method to retrieve favorites
            var result = await _favoritesService.GetFavoritesAsync();

            // Assert: Ensure the returned list is not null and is empty
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetFavoritesAsync_ReturnsFavorites_WhenFavoritesExist()
        {
            // Arrange: Simulate localStorage containing a favorite card
            var favorites = new List<FavoriteCard>
            {
                new FavoriteCard(_testCard)
            };
            var json = JsonSerializer.Serialize(favorites);
            
            _mockJsRuntime
                .Setup(js => js.InvokeAsync<string>("localStorage.getItem", It.IsAny<object[]>()))
                .ReturnsAsync(json);

            // Act: Retrieve favorites from the service
            var result = await _favoritesService.GetFavoritesAsync();

            // Assert: Ensure the retrieved list contains the correct favorite card
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal("test-id-1", result[0].Card.Id);
        }

        [Fact]
        public async Task AddFavoriteAsync_AddsFavorite_WhenCardIsNotAlreadyFavorite()
        {
            // Arrange: Simulate an empty localStorage
            _mockJsRuntime
                .Setup(js => js.InvokeAsync<string>("localStorage.getItem", It.IsAny<object[]>()))
                .ReturnsAsync((string)null);
            
            _mockJsRuntime
                .Setup(js => js.InvokeVoidAsync("localStorage.setItem", It.IsAny<object[]>()))
                .Returns(ValueTask.CompletedTask);

            // Act: Add a favorite card and retrieve the updated list
            await _favoritesService.AddFavoriteAsync(_testCard);
            var result = await _favoritesService.GetFavoritesAsync();

            // Assert: Ensure the favorite card has been added correctly
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal("test-id-1", result[0].Card.Id);
            
            // Verify that localStorage.setItem was called once
            _mockJsRuntime.Verify(js => 
                js.InvokeVoidAsync("localStorage.setItem", It.IsAny<object[]>()), 
                Times.Once);
        }

        [Fact]
        public async Task RemoveFavoriteAsync_RemovesFavorite_WhenCardIsFavorite()
        {
            // Arrange: Simulate localStorage containing a favorite card
            var favorites = new List<FavoriteCard>
            {
                new FavoriteCard(_testCard)
            };
            var json = JsonSerializer.Serialize(favorites);
            
            _mockJsRuntime
                .Setup(js => js.InvokeAsync<string>("localStorage.getItem", It.IsAny<object[]>()))
                .ReturnsAsync(json);
            
            _mockJsRuntime
                .Setup(js => js.InvokeVoidAsync("localStorage.setItem", It.IsAny<object[]>()))
                .Returns(ValueTask.CompletedTask);

            // Act: Remove the favorite card and retrieve the updated list
            await _favoritesService.RemoveFavoriteAsync("test-id-1");
            var result = await _favoritesService.GetFavoritesAsync();

            // Assert: Ensure the list is now empty
            Assert.NotNull(result);
            Assert.Empty(result);
            
            // Verify that localStorage.setItem was called once
            _mockJsRuntime.Verify(js => 
                js.InvokeVoidAsync("localStorage.setItem", It.IsAny<object[]>()), 
                Times.Once);
        }

        [Fact]
        public async Task UpdateFavoriteAsync_UpdatesFavorite_WhenCardIsFavorite()
        {
            // Arrange: Simulate localStorage containing an existing favorite card
            var favorites = new List<FavoriteCard>
            {
                new FavoriteCard(_testCard)
            };
            var json = JsonSerializer.Serialize(favorites);
            
            _mockJsRuntime
                .Setup(js => js.InvokeAsync<string>("localStorage.getItem", It.IsAny<object[]>()))
                .ReturnsAsync(json);
            
            _mockJsRuntime
                .Setup(js => js.InvokeVoidAsync("localStorage.setItem", It.IsAny<object[]>()))
                .Returns(ValueTask.CompletedTask);

            // Create an updated version of the favorite card with new details
            var updatedFavorite = new FavoriteCard(_testCard)
            {
                Notes = "Test notes",
                Condition = "Mint",
                MarketPrice = "$10.00"
            };

            // Act: Update the favorite card and retrieve the updated list
            await _favoritesService.UpdateFavoriteAsync(updatedFavorite);
            var result = await _favoritesService.GetFavoritesAsync();

            // Assert: Ensure the card details were updated successfully
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal("Test notes", result[0].Notes);
            Assert.Equal("Mint", result[0].Condition);
            Assert.Equal("$10.00", result[0].MarketPrice);
            
            // Verify that localStorage.setItem was called once
            _mockJsRuntime.Verify(js => 
                js.InvokeVoidAsync("localStorage.setItem", It.IsAny<object[]>()), 
                Times.Once);
        }
    }
}