using IdentityServer4.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MovieStore.Application.Common.Interfaces;
using MovieStore.Domain.Entities;
using MovieStore.Domain.Entities.JoinTables;
using MovieStore.Infrastructure.Identity;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace MovieStore.Infrastructure.Persistence
{
	public class MovieStoreContext : ApiAuthorizationDbContext<ApplicationUser>, IMovieStoreContext
	{
		//public MovieStoreContext(DbContextOptions<MovieStoreContext> options)
		//	: base(options)
		//{

		//}

		public MovieStoreContext(
			DbContextOptions options,
			IOptions<OperationalStoreOptions> operationalStoreOptions) 
			: base(options, operationalStoreOptions)
		{

		}

		protected override void OnModelCreating(ModelBuilder builder)
		{
			builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

			base.OnModelCreating(builder);
		}

		public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
		{
			return base.SaveChangesAsync(cancellationToken);
		}

		public DbSet<Movie> Movies { get; set; }
		public DbSet<Genre> Genres { get; set; }
		public DbSet<MovieGenre> MovieGenres { get; set; }
	}
}
