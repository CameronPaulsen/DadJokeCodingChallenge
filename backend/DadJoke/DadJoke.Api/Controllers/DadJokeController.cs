using DadJoke.Clients;
using DadJoke.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using DadJoke.Extensions;

namespace DadJoke.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DadJokeController : ControllerBase
    {
        private readonly IDadJokeClient _client;

        public DadJokeController(IDadJokeClient client)
        {
            _client = client;
        }

        [HttpGet]
        public async Task<string> Get()
        {
            return await _client.GetJoke();
        }

        [HttpPost("_search/{searchTerm}")]
        public async Task<JokeGroup> SearchJoke(string searchTerm)
        {
            var jokes = await _client.SearchJokes(searchTerm);
            return jokes.PresentJokes(searchTerm);
        }
    }
}