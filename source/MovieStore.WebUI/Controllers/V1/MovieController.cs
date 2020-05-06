using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieStore.Application.Movies.Commands;
using MovieStore.Application.Movies.Queries;
using MovieStore.WebUI.Contracts.V1;
using System.Threading.Tasks;

namespace MovieStore.WebUI.Controllers.V1
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class MovieController : ApiController
    {

        [HttpGet(ApiRoutes.Movies.GetAllMovies)]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> GetAllMovies()
        {
            var query = new GetAllMoviesQuery();
            var result = await Mediator.Send(query);
            return Ok(result);

        }

        [HttpGet(ApiRoutes.Movies.GetMovieDetails)]
        public async Task<IActionResult> GetMovieDetail(int id)
        {
            var query = new GetMovieDetailsQuery { Id = id };
            var result = await Mediator.Send(query);
            return Ok(result);
        }

        [HttpPost(ApiRoutes.Movies.CreateMovie)]
        public async Task<IActionResult> CreateMovie([FromBody] CreateMovieCommand command)
        {
            var result = await Mediator.Send(command);
            return Ok(result);
        }

        [HttpPut(ApiRoutes.Movies.UpdateMovie)]
        public async Task<IActionResult> UpdateMovie([FromBody] UpdateMovieCommand command)
        {
            await Mediator.Send(command);

            return NoContent();
        }

        [HttpDelete(ApiRoutes.Movies.DeleteMovie)]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteMovie(int id)
        {
            await Mediator.Send(new DeleteMovieCommand { Id = id });

            return NoContent();
        }
    }
}