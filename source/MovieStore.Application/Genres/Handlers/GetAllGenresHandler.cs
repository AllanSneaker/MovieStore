using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MovieStore.Application.Common.Interfaces;
using MovieStore.Application.Common.Mappings.DTO;
using MovieStore.Application.Genres.Queries;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MovieStore.Application.Genres.Handlers
{
	public class GetAllGenresHandler : IRequestHandler<GetAllGenresQuery, IEnumerable<GenreDto>>
	{
		private readonly IMovieStoreContext _context;
		private readonly IMapper _mapper;

		public GetAllGenresHandler(IMovieStoreContext context, IMapper mapper)
		{
			_context = context;
			_mapper = mapper;
		}

		public async Task<IEnumerable<GenreDto>> Handle(GetAllGenresQuery request, CancellationToken cancellationToken)
		{
			return await _context.Genres
					.ProjectTo<GenreDto>(_mapper.ConfigurationProvider)
					.OrderBy(t => t.Name)
					.ToListAsync(cancellationToken);
		}
	}
}
