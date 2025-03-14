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
            // Arrange: Set up a mock HttpMessageHandler that simulates a successful API response.
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(GetMockCardResponse()) // Mock data to be returned in the response.
            };

            // Configure the mock to return the fake response when the SendAsync method is called.
            mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(response);

            var httpClient = new HttpClient(mockHttpMessageHandler.Object);
            var service = new PokemonTcgService(httpClient);

            // Act: Call the method under test (SearchCards) with a search term "pikachu".
            var result = await service.SearchCards("pikachu");

            // Assert: Verify that the result is not null, contains 2 items, and that the first card's details are correct.
            Assert.NotNull(result);
            Assert.Equal(2, result.Count); // Check that there are two cards returned.
            Assert.Equal("test-id-1", result[0].Id); // Verify the first card's ID.
            Assert.Equal("Pikachu", result[0].Name); // Verify the first card's name.
        }

        [Fact]
        public async Task SearchCards_ReturnsEmptyList_WhenApiCallFails()
        {
            // Arrange: Set up a mock HttpMessageHandler that simulates a failed API response (throws an exception).
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

            // Act: Call the method under test (SearchCards) with a search term "pikachu".
            var result = await service.SearchCards("pikachu");

            // Assert: Verify that the result is not null and is an empty list when the API call fails.
            Assert.NotNull(result);
            Assert.Empty(result); // Verify that the result is an empty list due to the API failure.
        }

        [Fact]
        public async Task GetCardById_ReturnsCard_WhenApiCallSucceeds()
        {
            // Arrange: Set up a mock HttpMessageHandler that simulates a successful API response for a single card.
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(GetMockSingleCardResponse()) // Mock data for a single card response.
            };

            // Configure the mock to return the fake response when SendAsync is called.
            mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(response);

            var httpClient = new HttpClient(mockHttpMessageHandler.Object);
            var service = new PokemonTcgService(httpClient);

            // Act: Call the method under test (GetCardById) with a card ID "test-id-1".
            var result = await service.GetCardById("test-id-1");

            // Assert: Verify that the returned card is not null and that the card's ID and name are correct.
            Assert.NotNull(result);
            Assert.Equal("test-id-1", result.Id); // Verify the card ID.
            Assert.Equal("Pikachu", result.Name); // Verify the card's name.
        }

        [Fact]
        public async Task GetCardById_ReturnsNull_WhenApiCallFails()
        {
            // Arrange: Set up a mock HttpMessageHandler that simulates a failed API response (throws an exception).
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

            // Act: Call the method under test (GetCardById) with a card ID "test-id-1".
            var result = await service.GetCardById("test-id-1");

            // Assert: Verify that the result is null when the API call fails.
            Assert.Null(result); // The result should be null due to the API failure.
        }

        // Helper method that provides mock response data for multiple cards in a search query.
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

            return JsonSerializer.Serialize(response); // Serialize the response to a JSON string.
        }

        // Helper method that provides mock response data for a single card by ID.
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

            return JsonSerializer.Serialize(response); // Serialize the single card response to a JSON string.
        }
    }
}