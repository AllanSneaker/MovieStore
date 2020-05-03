﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using MovieStore.WebUI.Common.Interfaces;
using System.Text;

namespace MovieStore.WebUI.Configurations.Jwt
{
	public class JwtInstaller : IInstaller
	{
		public IServiceCollection AddInstaller(IServiceCollection services, IConfiguration configuration)
		{
			var jwtProperties = new JwtProperties();
			configuration.Bind(nameof(jwtProperties), jwtProperties);
			services.AddSingleton(jwtProperties);

			var tokenValidationParameters = new TokenValidationParameters
			{
				ValidateIssuerSigningKey = true,
				IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtProperties.Secret)),
				ValidateIssuer = false,
				ValidateAudience = false,
				RequireExpirationTime = false,
				ValidateLifetime = true
			};
			services.AddSingleton(tokenValidationParameters);

			services.AddAuthentication(x =>
			{
				x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
				x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			})
			.AddJwtBearer(x =>
			{
				x.SaveToken = true;
				x.TokenValidationParameters = tokenValidationParameters;
			});

			return services;
		}
	}
}
