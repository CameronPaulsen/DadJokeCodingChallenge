using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DadJoke.Clients
{
    public interface IDadJokeClient
    {
        public Task<string> GetJoke();

        public Task<List<string>> SearchJokes(string searchTerm);
    }

    public class DadJokeClient : IDadJokeClient
    {
        private readonly HttpClient _client;

        public DadJokeClient(HttpClient client)
        {
            _client = client;
            _client.BaseAddress = new Uri("https://icanhazdadjoke.com/");
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _client.DefaultRequestHeaders.Add("User-Agent", "Code Challenge (https://github.com/CameronPaulsen/DadJokeCodingChallenge)");
        }

        public async Task<string> GetJoke()
        {
            using var response = await _client.GetAsync("/");

            var apiResponse = await response.Content.ReadAsStringAsync();
            var joke = JsonSerializer.Deserialize<DadJokeResponse>(apiResponse);
            return joke?.Joke ?? throw new InvalidDataException();
        }

        public async Task<List<string>> SearchJokes(string searchTerm)
        {
            var query = new Dictionary<string, string?>
            {
                ["page"] = "1",
                ["limit"] = "30",
                ["term"] = searchTerm
            };

            using var response = await _client.GetAsync(QueryHelpers.AddQueryString("/search", query));

            var apiResponse = await response.Content.ReadAsStringAsync();
            var joke = JsonSerializer.Deserialize<DadJokeSearchResponse>(apiResponse);

            if (joke == null) throw new InvalidDataException();

            return joke.Results.Select(x => x.Joke).ToList();
        }

        private record DadJokeResponse(
            [property: JsonPropertyName("id")] string Id,

            [property: JsonPropertyName("joke")] string Joke,

            [property: JsonPropertyName("status")] int Status
        );

        private record DadJokeSearchResponse(
            [property: JsonPropertyName("current_page")]
            int CurrentPage,

            [property: JsonPropertyName("limit")] int Limit,

            [property: JsonPropertyName("next_page")]
            int NextPage,

            [property: JsonPropertyName("previous_page")]
            int PreviousPage,

            [property: JsonPropertyName("results")]
            List<DadJokeSearchResult> Results,

            [property: JsonPropertyName("search_term")]
            string SearchTerm,

            [property: JsonPropertyName("status")] int Status,

            [property: JsonPropertyName("total_jokes")]
            int TotalJokes,

            [property: JsonPropertyName("total_pages")]
            int TotalPages
        );

        private record DadJokeSearchResult(
            [property: JsonPropertyName("id")] string Id,

            [property: JsonPropertyName("joke")] string Joke
        );
    }
}