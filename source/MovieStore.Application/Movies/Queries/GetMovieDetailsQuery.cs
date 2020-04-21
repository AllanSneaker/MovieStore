using MediatR;
using MovieStore.Application.Common.Mappings.DTO;

namespace MovieStore.Application.Movies.Queries
{
	public class GetMovieDetailsQuery : IRequest<MovieDto>
	{
		public int Id { get; set; }
	}
}
