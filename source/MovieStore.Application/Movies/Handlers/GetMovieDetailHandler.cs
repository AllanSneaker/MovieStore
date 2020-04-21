﻿using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MovieStore.Application.Common.Interfaces;
using MovieStore.Application.Common.Mappings.DTO;
using MovieStore.Application.Movies.Queries;
using System.Threading;
using System.Threading.Tasks;

namespace MovieStore.Application.Movies.Handlers
{
	public class GetMovieDetailHandler : IRequestHandler<GetMovieDetailsQuery, MovieDto>
	{
		private readonly IMovieStoreContext _context;
		private readonly IMapper _mapper;

		public GetMovieDetailHandler(IMovieStoreContext context, IMapper mapper)
		{
			_context = context;
			_mapper = mapper;
		}
		public async Task<MovieDto> Handle(GetMovieDetailsQuery request, CancellationToken cancellationToken)
		{
			return await _context.Movies
				.ProjectTo<MovieDto>(_mapper.ConfigurationProvider)
				.FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);
		}
	}
}
