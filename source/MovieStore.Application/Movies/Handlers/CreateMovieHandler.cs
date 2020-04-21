using MediatR;
using MovieStore.Application.Common.Interfaces;
using MovieStore.Application.Movies.Commands;
using MovieStore.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace MovieStore.Application.Movies.Handlers
{
	public class CreateMovieHandler : IRequestHandler<CreateMovieCommand, int>
	{
		private readonly IMovieStoreContext _context;

		public CreateMovieHandler(IMovieStoreContext context)
		{
			_context = context;
		}

		public async Task<int> Handle(CreateMovieCommand request, CancellationToken cancellationToken)
		{
			var movie = new Movie
			{
				Cast = request.Cast,
				ContentOwner = request.ContentOwner,
				Description = request.Description,
				Director = request.Director,
				Duration = request.Duration,
				Language = request.Language,
				Producer = request.Producer,
				ReleaseDate = request.ReleaseDate,
				Script = request.Script,
				Title = request.Title
			};

			_context.Movies.Add(movie);
			await _context.SaveChangesAsync(cancellationToken);
			return movie.Id;
		}
	}
}
