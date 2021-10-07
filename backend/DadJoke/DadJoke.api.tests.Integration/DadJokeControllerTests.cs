using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using DadJoke.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace DadJoke.Api.Tests.Integration
{
    public class DadJokeControllerTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;

        public DadJokeControllerTests(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task GetJokeReturnsExpectedResult()
        {
            var client = _factory.CreateClient();

            var response = await client.GetAsync("/DadJoke");

            response.EnsureSuccessStatusCode();

            var joke = await response.Content.ReadAsStringAsync();

            joke.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task SearchJokesReturnsExpectedResult()
        {
            var client = _factory.CreateClient();

            var response = await client.PostAsync("/DadJoke/_search/why", new StringContent(""));

            response.EnsureSuccessStatusCode();

            var jokes = JsonSerializer.Deserialize<JokeGroup>(await response.Content.ReadAsStringAsync());

            jokes.Should().NotBeNull();
            (jokes.ShortJokes.Count() + jokes.MediumJokes.Count() + jokes.LongJokes.Count()).Should().BeGreaterThan(1);
        }
    }
}