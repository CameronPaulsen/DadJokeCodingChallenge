using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace DadJoke.Models
{
    public record JokeGroup(
        [property: JsonPropertyName("shortJokes")]
        IEnumerable<string> ShortJokes,
        [property: JsonPropertyName("mediumJokes")]
        IEnumerable<string> MediumJokes,
        [property: JsonPropertyName("longJokes")]
        IEnumerable<string> LongJokes
    );
}