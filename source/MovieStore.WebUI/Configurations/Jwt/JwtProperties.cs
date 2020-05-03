using System;

namespace MovieStore.WebUI.Configurations.Jwt
{
	public class JwtProperties
	{
		public string Secret { get; set; }
		public TimeSpan TokenLifetime { get; set; }
	}
}
