using MovieStore.Application.Common.Models;
using System.Threading.Tasks;

namespace MovieStore.Application.Common.Interfaces
{
	public interface IIdentityService
	{
		Task<string> GetUserNameAsync(string userId);
		Task<(Result Result, string UserId, string UserName, AuthTokenResponse authToken)> RegisterUserAsync(string userName, string password, string secret);
		Task<(Result Result, string UserId, string UserName, AuthTokenResponse authToken)> LoginUserAsync(string userName, string password, string secret);
		Task<Result> DeleteUserAsync(string userId);
	}
}
