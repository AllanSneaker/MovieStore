using MediatR;
using MovieStore.Application.Common.Mappings.DTO;
using System.Collections.Generic;

namespace MovieStore.Application.Genres.Queries
{
	public class GetAllGenresQuery : IRequest<IEnumerable<GenreDto>>
	{
	}
}
