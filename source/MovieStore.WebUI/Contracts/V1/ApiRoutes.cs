namespace MovieStore.WebUI.Contracts.V1
{
	public static class ApiRoutes
	{
		public const string Root = "api";
		public const string Version = "v1";
		public const string Base = Root + "/" + Version;

		public static class Movies
		{
			public const string GetAllMovies = Base + "/movies";
			public const string GetMovieDetails = Base + "/movies/{id}";
			public const string CreateMovie = Base + "/movies";
			public const string UpdateMovie = Base + "/movies";
			public const string DeleteMovie = Base + "/movies/{id}";
		}

		public static class Identity
		{
			public const string Register = Base + "/identity/register";
			public const string Login = Base + "/identity/login";
			public const string Delete = Base + "/identity/delete";
			public const string Refresh = Base + "/identity/refresh";
			public const string Role = Base + "/identity/role";
		}
	}
}
