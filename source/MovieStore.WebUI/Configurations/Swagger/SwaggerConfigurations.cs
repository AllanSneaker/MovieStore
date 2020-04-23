using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;

namespace MovieStore.WebUI.Configurations.Swagger
{
	public static class SwaggerConfigurations
	{
		public static void SettingSwagger(this IApplicationBuilder app, IConfiguration configuration)
		{
			var swaggerProperties = new SwaggerProperties();
			configuration.GetSection(nameof(SwaggerProperties)).Bind(swaggerProperties);

			app.UseSwagger(option => { option.RouteTemplate = swaggerProperties.JsonRoute; });
			app.UseSwaggerUI(option => option.SwaggerEndpoint(swaggerProperties.UIEndpoint, swaggerProperties.Description));
		}
	}
}
