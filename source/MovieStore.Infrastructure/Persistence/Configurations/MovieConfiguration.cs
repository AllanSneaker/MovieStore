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

		}
	}
}
