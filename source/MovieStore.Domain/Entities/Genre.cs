using MovieStore.Domain.Entities.JoinTables;
using System.Collections.Generic;

namespace MovieStore.Domain.Entities
{
	public class Genre
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public ICollection<MovieGenre> MovieGenres { get; set; }

		public Genre()
		{
			MovieGenres = new HashSet<MovieGenre>();
		}
	}
}
