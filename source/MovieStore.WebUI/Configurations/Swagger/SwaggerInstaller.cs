using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using MovieStore.WebUI.Common.Interfaces;

namespace MovieStore.WebUI.Configurations.Swagger
{
	public class SwaggerInstaller : IInstaller
	{
		public IServiceCollection AddInstaller(IServiceCollection services, IConfiguration configuration)
		{
			services.AddSwaggerGen(setup =>
			{
				setup.SwaggerDoc("v1", new OpenApiInfo
				{
					Title = "Movie Store API",
					Version = "v1"
				});
			});

			return services;
		}
	}
}
