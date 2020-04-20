using Microsoft.EntityFrameworkCore;
using MovieStore.Domain.Entities;
using MovieStore.Domain.Entities.JoinTables;

namespace MovieStore.Application.Common.Interfaces
{
	public interface IMovieStoreContext
	{
		DbSet<Movie> Movies { get; set; }
		DbSet<Genre> Genres { get; set; }
		DbSet<MovieGenre> MovieGenres { get; set; }
	}
}
