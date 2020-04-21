using Microsoft.AspNetCore.Mvc;
using MovieStore.Application.Movies.Commands;
using MovieStore.Application.Movies.Queries;
using System.Threading.Tasks;

namespace MovieStore.WebUI.Controllers
{
    public class MovieController : ApiController
    {

        [HttpGet("api/v1/movies")]
        public async Task<IActionResult> GetAllMovies()
        {
            var query = new GetAllMoviesQuery();
            var result = await Mediator.Send(query);
            return Ok(result);

        }

        [HttpGet("api/v1/movies/{id}")]
        public async Task<IActionResult> GetMovieDetail(int id)
        {
            var query = new GetMovieDetailsQuery { Id = id };
            var result = await Mediator.Send(query);
            return Ok(result);
        }

        [HttpPost("api/v1/movies")]
        public async Task<IActionResult> Create([FromBody] CreateMovieCommand command)
        {
            var result = await Mediator.Send(command);
            return Ok(result);
        }

        [HttpPut("api/v1/movies")]
        public async Task<IActionResult> Update([FromBody] UpdateMovieCommand command)
        {
            await Mediator.Send(command);

            return NoContent();
        }

        [HttpDelete("api/v1/movies/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await Mediator.Send(new DeleteMovieCommand { Id = id });

            return NoContent();
        }
    }
}