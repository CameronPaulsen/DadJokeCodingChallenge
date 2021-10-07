using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using DadJoke.Models;

namespace DadJoke.Extensions
{
    public static class JokeExtensions
    {
        private enum JokeSize
        {
            Small,
            Medium,
            Large
        }

        public static JokeGroup PresentJokes(this IEnumerable<string> jokes, string term)
        {
            var groups = jokes.Select(x => x.HighlightTerm(term)).GroupBy(x => x.GetJokeSize()).ToList();

            return new JokeGroup(
                ShortJokes: groups.FirstOrDefault(x => x.Key == JokeSize.Small)?.ToList() ?? new(),
                MediumJokes: groups.FirstOrDefault(x => x.Key == JokeSize.Medium)?.ToList() ?? new(),
                LongJokes: groups.FirstOrDefault(x => x.Key == JokeSize.Large)?.ToList() ?? new()
            );
        }

        private static JokeSize GetJokeSize(this string joke) => Regex.Matches(joke, @"[\S]+").Count switch
        {
            < 10 => JokeSize.Small,
            < 20 => JokeSize.Medium,
            _ => JokeSize.Large
        };

        private static string HighlightTerm(this string joke, string term)
        {
            return Regex.Replace(joke, $@"{term}", match => $"[{match}]", RegexOptions.IgnoreCase);
        }
    }
}