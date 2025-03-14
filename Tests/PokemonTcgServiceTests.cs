using System.Net;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Moq.Protected;
using PokemonTcgApp.Models;
using PokemonTcgApp.Services;
using Xunit;

namespace PokemonTcgApp.Tests
{
    public class PokemonTcgServiceTests
    {
        [Fact]
        public async Task SearchCards_ReturnsCards_WhenApiCallSucceeds()
        {
            // Arrange
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(GetMockCardResponse())
            };

            mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(response);

            var httpClient = new HttpClient(mockHttpMessageHandler.Object);
            var service = new PokemonTcgService(httpClient);

            // Act
            var result = await service.SearchCards("pikachu");

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal("test-id-1", result[0].Id);
            Assert.Equal("Pikachu", result[0].Name);
        }

        [Fact]
        public async Task SearchCards_ReturnsEmptyList_WhenApiCallFails()
        {
            // Arrange
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ThrowsAsync(new HttpRequestException("API Error"));

            var httpClient = new HttpClient(mockHttpMessageHandler.Object);
            var service = new PokemonTcgService(httpClient);

            // Act
            var result = await service.SearchCards("pikachu");

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetCardById_ReturnsCard_WhenApiCallSucceeds()
        {
            // Arrange
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(GetMockSingleCardResponse())
            };

            mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(response);

            var httpClient = new HttpClient(mockHttpMessageHandler.Object);
            var service = new PokemonTcgService(httpClient);

            // Act
            var result = await service.GetCardById("test-id-1");

            // Assert
            Assert.NotNull(result);
            Assert.Equal("test-id-1", result.Id);
            Assert.Equal("Pikachu", result.Name);
        }

        [Fact]
        public async Task GetCardById_ReturnsNull_WhenApiCallFails()
        {
            // Arrange
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ThrowsAsync(new HttpRequestException("API Error"));

            var httpClient = new HttpClient(mockHttpMessageHandler.Object);
            var service = new PokemonTcgService(httpClient);

            // Act
            var result = await service.GetCardById("test-id-1");

            // Assert
            Assert.Null(result);
        }

        private string GetMockCardResponse()
        {
            var response = new PokemonCardResponse
            {
                Data = new List<PokemonCard>
                {
                    new PokemonCard
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
                    },
                    new PokemonCard
                    {
                        Id = "test-id-2",
                        Name = "Pikachu V",
                        Supertype = "Pokémon",
                        Subtypes = new List<string> { "Basic", "V" },
                        Hp = "210",
                        Types = new List<string> { "Lightning" },
                        Images = new CardImages
                        {
                            Small = "https://example.com/pikachu-v-small.jpg",
                            Large = "https://example.com/pikachu-v-large.jpg"
                        },
                        Rarity = "Rare"
                    }
                },
                Page = 1,
                PageSize = 20,
                Count = 2,
                TotalCount = 2
            };

            return JsonSerializer.Serialize(response);
        }

        private string GetMockSingleCardResponse()
        {
            var response = new SingleCardResponse
            {
                Data = new PokemonCard
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
                }
            };

            return JsonSerializer.Serialize(response);
        }
    }
}

