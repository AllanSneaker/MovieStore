using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MovieStore.Domain.Entities;

namespace MovieStore.Infrastructure.Persistence.Configurations
{
	public class GenreConfiguration : IEntityTypeConfiguration<Genre>
	{
		public void Configure(EntityTypeBuilder<Genre> builder)
		{
			builder.Property(p => p.Id).HasColumnName("GenreID").HasColumnType("int");

			builder.Property(p => p.Name)
				.IsRequired()
				.HasMaxLength(25);

			builder.HasData
				(
					new Genre
					{
						Id = 1,
						Name = "Cartoons"
					},
					new Genre
					{
						Id = 2,
						Name = "Comedy"
					},
					new Genre
					{
						Id = 3,
						Name = "Action"
					}
				);
		}
	}
}
