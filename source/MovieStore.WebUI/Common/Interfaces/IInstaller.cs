using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MovieStore.WebUI.Common.Interfaces
{
	public interface IInstaller
	{
		IServiceCollection AddInstaller(IServiceCollection services, IConfiguration configuration);
	}
}
