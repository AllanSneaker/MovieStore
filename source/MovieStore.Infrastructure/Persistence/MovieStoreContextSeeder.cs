using Microsoft.AspNetCore.Identity;
using MovieStore.Application.Common.Interfaces;
using MovieStore.Domain.Entities;
using MovieStore.Domain.Entities.JoinTables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MovieStore.Infrastructure.Persistence
{
	public class MovieStoreContextSeeder
	{
		private static readonly Dictionary<int, Movie> Movies = new Dictionary<int, Movie>();
		private static readonly Dictionary<int, Genre> Genres = new Dictionary<int, Genre>();
		private static readonly Dictionary<int, MovieGenre> MovieGenres = new Dictionary<int, MovieGenre>();

		public static async Task SeedDefaultUserAsync(IIdentityService identity)
		{
			await identity.RegisterUserAsync("admin@localhost", "Xcv1290-", "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IlJvbWFuIExOYW1lIiwiaWF0IjoxMTEyMTMxNDE1fQ.S76azUYVvw7UBSnwFoHdjJo8o6dzz7xRLKrZzG2O_xI", TimeSpan.Parse("00:00:45"));

		}

		public static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
		{
			if (!await roleManager.RoleExistsAsync("Admin"))
			{
				var role = new IdentityRole("Admin");
				await roleManager.CreateAsync(role);
			}

			if (!await roleManager.RoleExistsAsync("User"))
			{
				var role = new IdentityRole("User");
				await roleManager.CreateAsync(role);
			}
		}

		public static async Task SeedAllAsync(IMovieStoreContext context)
		{
			if (context.Movies.Any())
			{
				return;
			}

			await SeedGenresAsync(context);
			await SeedMoviesAsync(context);
			await SeedMovieGenresAsync(context);
		}

		private static async Task SeedGenresAsync(IMovieStoreContext context)
		{
			Genres.Add(1, new Genre { Name = "Cartoons" });
			Genres.Add(2, new Genre { Name = "Comedy" });
			Genres.Add(3, new Genre { Name = "Action" });
			Genres.Add(4, new Genre { Name = "Fantasy" });
			Genres.Add(5, new Genre { Name = "Family" });

			foreach (var item in Genres.Values)
			{
				context.Genres.Add(item);
			}

			await context.SaveChangesAsync(CancellationToken.None);
		}

		private static async Task SeedMoviesAsync(IMovieStoreContext context)
		{
			Movies.Add(1, new Movie { Title = "Minions", ContentOwner = "NBCUniversal_ROW", ReleaseDate = DateTime.ParseExact("2015", "yyyy", null), Duration = TimeSpan.Parse("1:30:54"), Language = "English", Cast = "Sandra Bullock", Director = "Pierre Coffin", Script = "Brian Lynch", Description = "The story of Universal Pictures and Illumination Entertainment’s Minions begins at the dawn of time.Starting as single-celled yellow organisms, Minions evolve through the ages, perpetually serving the most despicable of masters." });

			Movies.Add(2, new Movie { Title = "Fantastic Beasts and Where to Find Them", ReleaseDate = DateTime.ParseExact("2017", "yyyy", null), ContentOwner = "Warner Bros. Entertainment", Duration = TimeSpan.Parse("2:12:49"), Language = "English", Cast = "Eddie Redmayne", Director = "David Yates", Producer = "J.K. Rowling" });

			foreach (var item in Movies.Values)
			{
				context.Movies.Add(item);
			}

			await context.SaveChangesAsync(CancellationToken.None);
		}

		private static async Task SeedMovieGenresAsync(IMovieStoreContext context)
		{
			MovieGenres.Add(1, new MovieGenre { MovieId = 1, GenreId = 1 });
			MovieGenres.Add(2, new MovieGenre { MovieId = 1, GenreId = 2 });
			MovieGenres.Add(3, new MovieGenre { MovieId = 1, GenreId = 5 });
			MovieGenres.Add(4, new MovieGenre { MovieId = 2, GenreId = 3 });
			MovieGenres.Add(5, new MovieGenre { MovieId = 2, GenreId = 4 });

			foreach (var item in MovieGenres.Values)
			{
				context.MovieGenres.Add(item);
			}

			await context.SaveChangesAsync(CancellationToken.None);
		}

	}
}
