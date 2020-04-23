using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MovieStore.WebUI.Common.Interfaces;
using System;
using System.Linq;

namespace MovieStore.WebUI
{
	public static class DependencyInjectionInstaller
	{
		public static void AddWebUI(this IServiceCollection services, IConfiguration configuration)
		{
			var installers = typeof(Startup).Assembly.ExportedTypes.Where(x =>
				typeof(IInstaller).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
				.Select(Activator.CreateInstance).Cast<IInstaller>().ToList();

			installers.ForEach(installer => installer.AddInstaller(services, configuration));
		}
	}
}
