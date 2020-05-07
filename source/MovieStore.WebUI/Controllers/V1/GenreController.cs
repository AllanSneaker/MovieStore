using Microsoft.AspNetCore.Mvc;
using MovieStore.Application.Genres.Queries;
using MovieStore.WebUI.Contracts.V1;
using System.Threading.Tasks;

namespace MovieStore.WebUI.Controllers.V1
{
    public class GenreController : ApiController
    {
        [HttpGet(ApiRoutes.Genres.GetAllGenres)]
        public async Task<IActionResult> GetAllGenres()
        {
            var query = new GetAllGenresQuery();
            var result = await Mediator.Send(query);
            return Ok(result);
        }
    }
}
