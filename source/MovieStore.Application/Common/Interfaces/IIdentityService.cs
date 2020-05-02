using MovieStore.Application.Common.Models;
using System.Threading.Tasks;

namespace MovieStore.Application.Common.Interfaces
{
	public interface IIdentityService
	{
		Task<string> GetUserNameAsync(string userId);
		Task<(Result Result, string UserId, string UserName)> RegisterUserAsync(string userName, string password);
		Task<(Result Result, string UserId, string UserName)> LoginUserAsync(string userName, string password);
		Task<Result> DeleteUserAsync(string userId);
	}
}
