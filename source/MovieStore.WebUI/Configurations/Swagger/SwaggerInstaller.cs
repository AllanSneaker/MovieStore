using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using MovieStore.WebUI.Common.Interfaces;
using System.Collections.Generic;

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

				var security = new OpenApiSecurityRequirement
				{
					{
						new OpenApiSecurityScheme
						{
							Reference = new OpenApiReference
							{
								Type = ReferenceType.SecurityScheme,
								Id = "Bearer"
							}
						},

						new string[0]
					}
				};

				setup.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
				{
					Description = "Jwt Authorization header using the bearer scheme",
					Name = "Authorization",
					In = ParameterLocation.Header,
					Type = SecuritySchemeType.ApiKey
				});

				setup.AddSecurityRequirement(security);
			});

			return services;
		}
	}
}
