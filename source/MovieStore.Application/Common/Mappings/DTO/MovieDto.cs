using AutoMapper;
using MovieStore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MovieStore.Application.Common.Mappings.DTO
{
	public class MovieDto : IMapFrom<Movie>
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

		public IEnumerable<GenreDto> Genres { get; set; }

		public void Mapping(Profile profile)
		{
			profile.CreateMap<Movie, MovieDto>()
				.ForMember(dest => dest.Genres, opt => 
				opt.MapFrom(src => src.MovieGenres.Select(x => new GenreDto { Name = x.Genre.Name })));
		}
	}
}
