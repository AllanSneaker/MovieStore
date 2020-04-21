using MediatR;

namespace MovieStore.Application.Movies.Commands
{
	public class DeleteMovieCommand : IRequest
	{
		public int Id { get; set; }
	}
}
