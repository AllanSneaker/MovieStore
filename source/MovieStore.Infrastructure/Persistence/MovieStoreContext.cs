using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MovieStore.Application.Common.Interfaces;
using MovieStore.Domain.Entities;
using System.Reflection;

namespace MovieStore.Infrastructure.Persistence
{
	public class MovieStoreContext : IdentityDbContext, IMovieStoreContext
	{
		public MovieStoreContext(DbContextOptions<MovieStoreContext> options)
			: base(options)
		{

		}

		protected override void OnModelCreating(ModelBuilder builder)
		{
			builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

			base.OnModelCreating(builder);
		}

		public DbSet<Movie> Movies { get; set; }
	}
}
