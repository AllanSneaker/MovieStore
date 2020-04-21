using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MovieStore.Application.Common.Interfaces;
using MovieStore.Application.Common.Mappings.DTO;
using MovieStore.Application.Movies.Queries;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MovieStore.Application.Movies.Handlers
{
	public class GetAllMoviesHandler : IRequestHandler<GetAllMoviesQuery, List<MovieDto>>
	{

		private readonly IMovieStoreContext _context;
		private readonly IMapper _mapper;

		public GetAllMoviesHandler(IMovieStoreContext context, IMapper mapper)
		{
			_context = context;
			_mapper = mapper;
		}

		public async Task<List<MovieDto>> Handle(GetAllMoviesQuery request, CancellationToken cancellationToken)
		{
			return await _context.Movies
					.ProjectTo<MovieDto>(_mapper.ConfigurationProvider)
					.OrderBy(t => t.Id)
					.ToListAsync(cancellationToken);
		}
	}
}
