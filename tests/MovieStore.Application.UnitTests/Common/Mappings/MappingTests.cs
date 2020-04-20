using AutoMapper;
using MovieStore.Application.Common.Mappings;
using MovieStore.Application.Common.Mappings.DTO;
using MovieStore.Domain.Entities;
using NUnit.Framework;
using System;

namespace MovieStore.Application.UnitTests.Common.Mappings
{
	public class MappingTests
	{
		private readonly IConfigurationProvider _configuration;
		private readonly IMapper _mapper;

		public MappingTests()
		{
			_configuration = new MapperConfiguration(cfg =>
			{
				cfg.AddProfile<MappingProfile>();
			});

			_mapper = _configuration.CreateMapper();
		}

		[Test]
		public void ShouldHaveValidConfiguration()
		{
			_configuration.AssertConfigurationIsValid();
		}

		[Test]
		[TestCase(typeof(Movie), typeof(MovieDto))]
		public void ShouldSupportMappingFromSourceToDestination(Type source, Type destination)
		{
			var instance = Activator.CreateInstance(source);

			_mapper.Map(instance, source, destination);
		}
	}
}
