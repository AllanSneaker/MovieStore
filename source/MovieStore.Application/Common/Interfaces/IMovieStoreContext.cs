using Microsoft.EntityFrameworkCore;
using MovieStore.Domain.Entities;

namespace MovieStore.Application.Common.Interfaces
{
	public interface IMovieStoreContext
	{
		DbSet<Movie> Movies { get; set; }
	}
}
