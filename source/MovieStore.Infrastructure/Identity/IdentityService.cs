using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MovieStore.Application.Common.Interfaces;
using MovieStore.Application.Common.Models;
using MovieStore.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
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
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly TokenValidationParameters _tokenValidationParameters;
        private readonly MovieStoreContext _context;

        public IdentityService(UserManager<ApplicationUser> userManager, 
            TokenValidationParameters tokenValidationParameters, MovieStoreContext context,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _tokenValidationParameters = tokenValidationParameters;
            _context = context;
        }

        public async Task<string> GetUserNameAsync(string userId)
        {
            var user = await _userManager.Users.FirstAsync(u => u.Id == userId);

            return user.UserName;
        }
        public async Task<(Result Result, string UserId, string UserName, AuthTokenResponse AuthToken)> RegisterUserAsync(string userName, string password, string secret, TimeSpan tokenLifetime)
        {
            var existingUser = await _userManager.FindByEmailAsync(userName);

            if (existingUser != null)
                return (Result.Failure(new[] { "User with this email address already exists" }), null, userName, null);

            var user = new ApplicationUser
            {
                UserName = userName,
                Email = userName,
            };

            var result = await _userManager.CreateAsync(user, password);

            if (!result.Succeeded)
                return (result.ToApplicationResult(), user.Id, user.UserName, null);

            var authToken = await GenerateTokenForUser(user, secret, tokenLifetime);

            return (result.ToApplicationResult(), user.Id, user.UserName, authToken);
        }

        public async Task<(Result Result, string UserId, string UserName, AuthTokenResponse AuthToken)> LoginUserAsync(string userName, string password, string secret, TimeSpan tokenLifetime)
        {
            var user = await _userManager.FindByEmailAsync(userName);

            if (user == null)
                return (Result.Failure(new string[] { "User does not exist" }), null, userName, null);

            var hasValidPassword = await _userManager.CheckPasswordAsync(user, password);

            if (!hasValidPassword)
                return (Result.Failure(new string[] { "User or password combination is wrong" }), null, userName, null);

            var authToken = await GenerateTokenForUser(user, secret, tokenLifetime);

            return (Result.Success(), user.Id, user.UserName, authToken);
        }

        public async Task<(Result Result, string UserName)> AddRoleForUser(string userName, string role)
        {
            var user = await _userManager.FindByEmailAsync(userName);

            if (user == null)
                return (Result.Failure(new string[] { "User does not exist" }), userName);

            if(!await _roleManager.RoleExistsAsync(role))
                return (Result.Failure(new string[] { "Role does not exist" }), null);

            if(await _userManager.IsInRoleAsync(user, role))
                return (Result.Failure(new string[] { $"The {userName} already has {role}" }), null);

            await _userManager.AddToRoleAsync(user, role);

            return (Result.Success(), userName);
        }

        public async Task<Result> DeleteUserAsync(string userId)
        {
            var user = _userManager.Users.SingleOrDefault(u => u.Id == userId);

            if (user != null)
            {
                await DeleteTokenAsync(user);
                return await DeleteUserAsync(user);
            }

            return Result.Failure(new string[] { "User does not exist" });
        }

        private async Task<Result> DeleteTokenAsync(ApplicationUser user)
        {
            var tokens = _context.RefreshTokens.Where(t => t.UserId == user.Id).ToList();

            if (tokens == null)
                return Result.Success();

            _context.RefreshTokens.RemoveRange(tokens);
            await _context.SaveChangesAsync();

            return Result.Success();
        }

        private async Task<Result> DeleteUserAsync(ApplicationUser user)
        {
            var result = await _userManager.DeleteAsync(user);

            return result.ToApplicationResult();
        }

        private async Task<AuthTokenResponse> GenerateTokenForUser(ApplicationUser user, string secret, TimeSpan tokenLifetime)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secret);
            //var tokenDescriptor = new SecurityTokenDescriptor
            //{
            //    Subject = new ClaimsIdentity(new[]
            //    {
            //        new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
            //        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            //        new Claim(JwtRegisteredClaimNames.Email, user.UserName),
            //        new Claim("id", user.Id)
            //    }),
            //    Expires = DateTime.UtcNow.Add(tokenLifetime),
            //    SigningCredentials =
            //        new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            //};

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("id", user.Id)
            };

            var userRoles = await _userManager.GetRolesAsync(user);
            foreach (var userRole in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, userRole));
                var role = await _roleManager.FindByNameAsync(userRole);
                if (role == null) continue;
                var roleClaims = await _roleManager.GetClaimsAsync(role);

                foreach (var roleClaim in roleClaims)
                {
                    if (claims.Contains(roleClaim))
                        continue;

                    claims.Add(roleClaim);
                }
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.Add(tokenLifetime),
                SigningCredentials =
                   new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            var refreshToken = new RefreshToken
            {
                JwtId = token.Id,
                UserId = user.Id,
                CreationDate = DateTime.UtcNow,
                ExpiryDate = DateTime.UtcNow.AddMonths(6)
            };

            await _context.RefreshTokens.AddAsync(refreshToken);
            await _context.SaveChangesAsync();

            var tokenResponse = new AuthTokenResponse
            {
                Token = tokenHandler.WriteToken(token),
                RefreshToken = refreshToken.Token
            };

            return tokenResponse;
        }

        public async Task<(Result Result, AuthTokenResponse AuthTokenResponse)> RefreshTokenAsync(string token, string refreshToken, string secret, TimeSpan tokenLifetime)
        {
            var validatedToken = GetPrincipalFromToken(token);

            if (validatedToken == null)
                return (Result.Failure(new[] { "Invalid Token" }), null);

            var expiryDateUnix =
                long.Parse(validatedToken.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Exp).Value);

            var expiryDateTimeUtc = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                .AddSeconds(expiryDateUnix);

            if (expiryDateTimeUtc > DateTime.UtcNow)
                return (Result.Failure(new[] { "This token hasn't expired yet" }), null);

            var jti = validatedToken.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Jti).Value;

            var storedRefreshToken = await _context.RefreshTokens.SingleOrDefaultAsync(x => x.Token == refreshToken);

            if (storedRefreshToken == null)
                return (Result.Failure(new[] { "This refresh token does not exist" }), null);

            if (DateTime.UtcNow > storedRefreshToken.ExpiryDate)
                return (Result.Failure(new[] { "This refresh token has expired" }), null);

            if (storedRefreshToken.Invalidated)
                return (Result.Failure(new[] { "This refresh token has been invalidated" }), null);

            if (storedRefreshToken.Used)
                return (Result.Failure(new[] { "This refresh token has been used" }), null);

            if (storedRefreshToken.JwtId != jti)
                return (Result.Failure(new[] { "This refresh token does not match this JWT" }), null);

            storedRefreshToken.Used = true;
            _context.RefreshTokens.Update(storedRefreshToken);
            await _context.SaveChangesAsync();

            var user = await _userManager.FindByIdAsync(validatedToken.Claims.Single(x => x.Type == "id").Value);

            var result = await GenerateTokenForUser(user, secret, tokenLifetime);

            return (Result.Success(), result);
        }

        private ClaimsPrincipal GetPrincipalFromToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                var tokenValidationParameters = _tokenValidationParameters.Clone();
                tokenValidationParameters.ValidateLifetime = false;
                var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var validatedToken);

                if (!IsJwtWithValidSecurityAlgorithm(validatedToken))
                    return null;

                return principal;
            }
            catch
            {
                return null;
            }
        }

        private bool IsJwtWithValidSecurityAlgorithm(SecurityToken validatedToken)
        {
            return (validatedToken is JwtSecurityToken jwtSecurityToken) &&
                   jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                       StringComparison.InvariantCultureIgnoreCase);
        }

    }
}
