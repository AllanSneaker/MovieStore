﻿namespace MovieStore.Application.Common.Models
{
	public class AuthTokenResponse
	{
		public string Token { get; set; }
		public string RefreshToken { get; set; }
	}
}
