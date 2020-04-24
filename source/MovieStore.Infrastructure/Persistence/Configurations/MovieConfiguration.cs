using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MovieStore.Domain.Entities;
using System;

namespace MovieStore.Infrastructure.Persistence.Configurations
{
	public class MovieConfiguration : IEntityTypeConfiguration<Movie>
	{
		public void Configure(EntityTypeBuilder<Movie> builder)
		{

			builder.Property(p => p.Id).HasColumnName("MovieID").HasColumnType("int");

			builder.Property(p => p.Title)
				.IsRequired()
				.HasMaxLength(40);

			builder.Property(p => p.Language).HasMaxLength(40);

			builder.Property(p => p.Duration).HasDefaultValue(default(TimeSpan));

			builder.HasData
				(
					new Movie
					{
						Id = 1,
						Title = "Minions",
						ContentOwner = "NBCUniversal_ROW",
						ReleaseDate = DateTime.ParseExact("2015", "yyyy", null),
						Duration = TimeSpan.Parse("1:30:54"),
						Language = "English",
						Cast = "Sandra Bullock",
						Director = "Pierre Coffin",
						Script = "Brian Lynch",
						Description = "The story of Universal Pictures and Illumination Entertainment’s Minions begins at the dawn of time.Starting as single-celled yellow organisms, Minions evolve through the ages, perpetually serving the most despicable of masters.",
					}
				);
		}
	}
}
