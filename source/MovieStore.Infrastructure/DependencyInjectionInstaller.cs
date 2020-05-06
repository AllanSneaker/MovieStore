﻿using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MovieStore.Application.Common.Interfaces;
using MovieStore.Infrastructure.Identity;
using MovieStore.Infrastructure.Persistence;

namespace MovieStore.Infrastructure
{
	public static class DependencyInjectionInstaller
	{
		public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddDbContext<MovieStoreContext>(options =>
					options.UseSqlServer(
						configuration.GetConnectionString("DefaultConnection")));

			services.AddScoped<IMovieStoreContext>(provider => provider.GetService<MovieStoreContext>());

			services.AddDefaultIdentity<ApplicationUser>()
					.AddRoles<IdentityRole>()
					.AddEntityFrameworkStores<MovieStoreContext>();

			services.AddIdentityServer()
				.AddApiAuthorization<ApplicationUser, MovieStoreContext>();

			services.AddTransient<IIdentityService, IdentityService>();

			return services;
		}
	}
}
