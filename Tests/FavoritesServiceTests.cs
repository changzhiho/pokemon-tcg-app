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
            _mockJsRuntime = new Mock<IJSRuntime>();
            _favoritesService = new FavoritesService(_mockJsRuntime.Object);
            
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
            // Arrange
            _mockJsRuntime
                .Setup(js => js.InvokeAsync<string>("localStorage.getItem", It.IsAny<object[]>()))
                .ReturnsAsync((string)null);

            // Act
            var result = await _favoritesService.GetFavoritesAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetFavoritesAsync_ReturnsFavorites_WhenFavoritesExist()
        {
            // Arrange
            var favorites = new List<FavoriteCard>
            {
                new FavoriteCard(_testCard)
            };
            
            var json = JsonSerializer.Serialize(favorites);
            
            _mockJsRuntime
                .Setup(js => js.InvokeAsync<string>("localStorage.getItem", It.IsAny<object[]>()))
                .ReturnsAsync(json);

            // Act
            var result = await _favoritesService.GetFavoritesAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal("test-id-1", result[0].Card.Id);
        }

        [Fact]
        public async Task AddFavoriteAsync_AddsFavorite_WhenCardIsNotAlreadyFavorite()
        {
            // Arrange
            _mockJsRuntime
                .Setup(js => js.InvokeAsync<string>("localStorage.getItem", It.IsAny<object[]>()))
                .ReturnsAsync((string)null);
            
            _mockJsRuntime
                .Setup(js => js.InvokeVoidAsync("localStorage.setItem", It.IsAny<object[]>()))
                .Returns(ValueTask.CompletedTask);

            // Act
            await _favoritesService.AddFavoriteAsync(_testCard);
            var result = await _favoritesService.GetFavoritesAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal("test-id-1", result[0].Card.Id);
            
            _mockJsRuntime.Verify(js => 
                js.InvokeVoidAsync("localStorage.setItem", It.IsAny<object[]>()), 
                Times.Once);
        }

        [Fact]
        public async Task RemoveFavoriteAsync_RemovesFavorite_WhenCardIsFavorite()
        {
            // Arrange
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

            // Act
            await _favoritesService.RemoveFavoriteAsync("test-id-1");
            var result = await _favoritesService.GetFavoritesAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
            
            _mockJsRuntime.Verify(js => 
                js.InvokeVoidAsync("localStorage.setItem", It.IsAny<object[]>()), 
                Times.Once);
        }

        [Fact]
        public async Task UpdateFavoriteAsync_UpdatesFavorite_WhenCardIsFavorite()
        {
            // Arrange
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

            var updatedFavorite = new FavoriteCard(_testCard)
            {
                Notes = "Test notes",
                Condition = "Mint",
                MarketPrice = "$10.00"
            };

            // Act
            await _favoritesService.UpdateFavoriteAsync(updatedFavorite);
            var result = await _favoritesService.GetFavoritesAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal("Test notes", result[0].Notes);
            Assert.Equal("Mint", result[0].Condition);
            Assert.Equal("$10.00", result[0].MarketPrice);
            
            _mockJsRuntime.Verify(js => 
                js.InvokeVoidAsync("localStorage.setItem", It.IsAny<object[]>()), 
                Times.Once);
        }
    }
}

