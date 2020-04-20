using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MovieStore.Domain.Entities.JoinTables;

namespace MovieStore.Infrastructure.Persistence.Configurations.JoinTablesConfigurations
{
	public class MovieGenreConfiguration : IEntityTypeConfiguration<MovieGenre>
	{
		public void Configure(EntityTypeBuilder<MovieGenre> builder)
		{
			builder.HasKey(mg => new { mg.MovieId, mg.GenreId });
			builder.HasData(new MovieGenre { MovieId = 1, GenreId = 1 }, new MovieGenre { MovieId = 1, GenreId = 2 });
		}
	}
}
