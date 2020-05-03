using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MovieStore.Application.Common.Interfaces;
using MovieStore.Application.Common.Models;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MovieStore.Infrastructure.Identity
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public IdentityService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<string> GetUserNameAsync(string userId)
        {
            var user = await _userManager.Users.FirstAsync(u => u.Id == userId);

            return user.UserName;
        }
        public async Task<(Result Result, string UserId, string UserName, AuthTokenResponse authToken)> RegisterUserAsync(string userName, string password, string secret)
        {
            var user = new ApplicationUser
            {
                UserName = userName,
                Email = userName,
            };

            var result = await _userManager.CreateAsync(user, password);

            var authToken = GenerateTokenForUser(user.Id, user.UserName, secret);

            return (result.ToApplicationResult(), user.Id, user.UserName, authToken);
        }

        public async Task<(Result Result, string UserId, string UserName, AuthTokenResponse authToken)> LoginUserAsync(string userName, string password, string secret)
        {
            var user = await _userManager.FindByEmailAsync(userName);

            if (user == null)
                return (Result.Failure(new string[] { "User does not exist" }), null, userName, null);

            var hasValidPassword = await _userManager.CheckPasswordAsync(user, password);

            if (!hasValidPassword)
                return (Result.Failure(new string[] { "User or password combination is wrong" }), null, userName, null);

            var authToken = GenerateTokenForUser(user.Id, user.UserName, secret);

            return (Result.Success(), user.Id, user.UserName, authToken);
        }

        public async Task<Result> DeleteUserAsync(string userId)
        {
            var user = _userManager.Users.SingleOrDefault(u => u.Id == userId);

            if (user != null)
            {
                return await DeleteUserAsync(user);
            }

            return Result.Failure(new string[] { "User does not exist" });
        }

        public async Task<Result> DeleteUserAsync(ApplicationUser user)
        {
            var result = await _userManager.DeleteAsync(user);

            return result.ToApplicationResult();
        }

        private AuthTokenResponse GenerateTokenForUser(string userId, string userName, string secret)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, userName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, userName),
                    new Claim("id", userId)
                }),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials =
                    new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenResponse = new AuthTokenResponse { Token = tokenHandler.WriteToken(token) };

            return tokenResponse;
        }
    }
}
