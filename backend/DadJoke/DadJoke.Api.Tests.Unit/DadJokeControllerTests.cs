using FluentAssertions;
using System.Collections.Generic;
using DadJoke.Clients;
using DadJoke.Controllers;
using NSubstitute;
using Xunit;

namespace DadJoke.Api.Tests.Unit
{
    public class DadJokeControllerTests
    {
        [Fact]
        public async void GetJoke_CallsClientAndReturnsJoke()
        {
            const string expected = "test joke";

            var client = Substitute.For<IDadJokeClient>();
            client.GetJoke().Returns(expected);

            var controller = new DadJokeController(client);

            var result = await controller.Get();
            result.Should().Be(expected);

            await client.Received(1).GetJoke();
        }

        [Fact]
        public async void SearchJoke_CallsClientAndReturnsJokes()
        {
            var expected = new List<string> { "test joke 1", "test joke 1" };
            const string term = "test";

            var client = Substitute.For<IDadJokeClient>();
            client.SearchJokes(term).Returns(expected);

            var controller = new DadJokeController(client);

            var result = await controller.SearchJoke(term);
            result.ShortJokes.Should().HaveCount(2);
            result.MediumJokes.Should().HaveCount(0);
            result.LongJokes.Should().HaveCount(0);

            await client.Received(1).SearchJokes(term);
        }
    }
}