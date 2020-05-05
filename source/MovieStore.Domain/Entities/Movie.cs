using MovieStore.Domain.Entities.JoinTables;
using System;
using System.Collections.Generic;

namespace MovieStore.Domain.Entities
{
	public class Movie
	{
		public int Id { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
		public TimeSpan Duration { get; set; }
		public string ContentOwner { get; set; }
		public DateTime ReleaseDate { get; set; }
		public string Language { get; set; }
		public string Cast { get; set; }
		public string Director { get; set; }
		public string Producer { get; set; }
		public string Script { get; set; }
		public ICollection<MovieGenre> MovieGenres { get; set; }

		public Movie()
		{
			MovieGenres = new HashSet<MovieGenre>();
		}
	}
}
