using AutoMapper;
using MovieStore.Domain.Entities;

namespace MovieStore.Application.Common.Mappings.DTO
{
	public class GenreDto : IMapFrom<Genre>
	{
		public string Name { get; set; }
		public void Mapping(Profile profile)
		{
			profile.CreateMap<Genre, GenreDto>();
		}
	}
}
