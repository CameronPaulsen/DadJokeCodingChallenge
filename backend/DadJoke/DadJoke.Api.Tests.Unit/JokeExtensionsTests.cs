using FluentAssertions;
using System.Collections.Generic;
using System.Linq;
using DadJoke.Extensions;
using Xunit;

namespace DadJoke.Api.Tests.Unit
{
    public class JokeExtensionsTests
    {
        [Theory]
        [InlineData("hello world", "hello", "[hello] world")]
        [InlineData("Hello world", "hello", "[Hello] world")]
        [InlineData("hello world he", "he", "[he]llo world [he]")]
        public void JokePresentingHighlightsCorrectTerm(string input, string term, string expected)
        {
            new List<string> { input }.PresentJokes(term).ShortJokes.First().Should().Be(expected);
        }

        [Fact]
        public void JokePresentingGroupsJokesCorrectly()
        {
            var jokeGroups = Enumerable.Range(1, 50).Select(x => string.Join(" ", Enumerable.Repeat("word", x)))
                .PresentJokes("x");

            jokeGroups.ShortJokes.Should().HaveCount(9);

            jokeGroups.ShortJokes.ToList().ForEach(joke =>
                joke.Split(" ").Length.Should().BeLessThan(10, "all short jokes should have less than 10 words"));


            jokeGroups.MediumJokes.Should().HaveCount(10);
            jokeGroups.MediumJokes.ToList().ForEach(joke =>
                joke.Split(" ").Length.Should()
                    .BeLessThan(20, "all medium jokes should have less than 20 words").And
                    .BeGreaterOrEqualTo(10, "all medium jokes should have greater than or equal to 10 words"));


            jokeGroups.LongJokes.Should().HaveCount(31);
            jokeGroups.LongJokes.ToList().ForEach(joke =>
                joke.Split(" ").Length.Should()
                    .BeGreaterOrEqualTo(20, "all short jokes should have less than 10 words"));
        }
    }
}