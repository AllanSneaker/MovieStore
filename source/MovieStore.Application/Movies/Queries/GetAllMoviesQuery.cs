using MediatR;
using MovieStore.Application.Common.Mappings.DTO;
using System.Collections.Generic;

namespace MovieStore.Application.Movies.Queries
{
	public class GetAllMoviesQuery : IRequest<List<MovieDto>>
	{

	}
}
