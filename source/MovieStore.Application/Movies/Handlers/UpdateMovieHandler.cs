using MediatR;
using MovieStore.Application.Common.Exceptions;
using MovieStore.Application.Common.Interfaces;
using MovieStore.Application.Movies.Commands;
using MovieStore.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace MovieStore.Application.Movies.Handlers
{
	public class UpdateMovieHandler : IRequestHandler<UpdateMovieCommand>
	{
		private readonly IMovieStoreContext _context;

		public UpdateMovieHandler(IMovieStoreContext context)
		{
			_context = context;
		}

		public async Task<Unit> Handle(UpdateMovieCommand request, CancellationToken cancellationToken)
		{
			var entity = await _context.Movies.FindAsync(request.Id);

			if (entity == null)
			{
				throw new NotFoundException(nameof(Movie), request.Id);
			}

			entity.Id = request.Id;
			entity.Cast = request.Cast;
			entity.ContentOwner = request.ContentOwner;
			entity.Description = request.Description;
			entity.Director = request.Director;
			entity.Duration = request.Duration;
			entity.Language = request.Language;
			entity.Producer = request.Producer;
			entity.ReleaseDate = request.ReleaseDate;
			entity.Script = request.Script;
			entity.Title = request.Title;

			await _context.SaveChangesAsync(cancellationToken);

			return Unit.Value;
		}
	}
}
