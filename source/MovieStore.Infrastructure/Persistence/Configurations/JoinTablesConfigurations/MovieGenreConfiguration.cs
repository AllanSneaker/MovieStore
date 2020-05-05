using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MovieStore.Domain.Entities.JoinTables;

namespace MovieStore.Infrastructure.Persistence.Configurations.JoinTablesConfigurations
{
	public class MovieGenreConfiguration : IEntityTypeConfiguration<MovieGenre>
	{
		public void Configure(EntityTypeBuilder<MovieGenre> builder)
		{
			builder.HasKey(mg => new { mg.MovieId, mg.GenreId })
				.IsClustered(false);

			builder.Property(p => p.MovieId).HasColumnName("MovieID").HasColumnType("int");

			builder.Property(p => p.GenreId).HasColumnName("GenreID").HasColumnType("int");

			builder.HasOne(d => d.Movie)
				.WithMany(p => p.MovieGenres)
				.HasForeignKey(d => d.MovieId)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("FK_MovieGenres_Movies");

			builder.HasOne(d => d.Genre)
				.WithMany(p => p.MovieGenres)
				.HasForeignKey(d => d.GenreId)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("FK_MovieGenres_Genres");
		}
	}
}
