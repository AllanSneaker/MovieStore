using MovieStore.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MovieStore.Application.Common.Interfaces
{
	public interface IIdentityService
	{
		Task<string> GetUserNameAsync(string userId);
		Task<(Result Result, string UserId, string UserName, AuthTokenResponse AuthToken)> RegisterUserAsync(string userName, string password, string secret, TimeSpan tokenLifetime);
		Task<(Result Result, string UserId, string UserName, AuthTokenResponse AuthToken)> LoginUserAsync(string userName, string password, string secret, TimeSpan tokenLifetime);
		Task<Result> DeleteUserAsync(string userId);
		Task<(Result Result, AuthTokenResponse AuthTokenResponse)> RefreshTokenAsync(string token, string refreshToken, string secret, TimeSpan tokenLifetime);
		Task<(Result Result, string UserName)> AddRoleForUser(string userName, string role);
	}
}
