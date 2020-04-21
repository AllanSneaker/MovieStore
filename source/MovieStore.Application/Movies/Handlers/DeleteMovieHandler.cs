using MediatR;
using MovieStore.Application.Common.Exceptions;
using MovieStore.Application.Common.Interfaces;
using MovieStore.Application.Movies.Commands;
using MovieStore.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace MovieStore.Application.Movies.Handlers
{
	public class DeleteMovieHandler : IRequestHandler<DeleteMovieCommand>
	{
		private readonly IMovieStoreContext _context;

		public DeleteMovieHandler(IMovieStoreContext context)
		{
			_context = context;
		}

		public async Task<Unit> Handle(DeleteMovieCommand request, CancellationToken cancellationToken)
		{
			var entity = await _context.Movies.FindAsync(request.Id);

			if (entity == null)
			{
				throw new NotFoundException(nameof(Movie), request.Id);
			}

			_context.Movies.Remove(entity);
			await _context.SaveChangesAsync(cancellationToken);

			return Unit.Value;
		}
	}
}
