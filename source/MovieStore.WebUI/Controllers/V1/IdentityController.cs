using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MovieStore.Application.Common.Interfaces;
using MovieStore.WebUI.Configurations.Jwt;
using MovieStore.WebUI.Contracts.V1;
using MovieStore.WebUI.Controllers.V1.Requests;
using MovieStore.WebUI.Controllers.V1.Responses;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MovieStore.WebUI.Controllers.V1
{
	public class IdentityController : ApiController
	{
		private readonly IIdentityService _identityService;
		private readonly JwtProperties _jwtProperties;

		public IdentityController(IIdentityService identityService, JwtProperties jwtProperties)
		{
			_identityService = identityService;
			_jwtProperties = jwtProperties;
		}

		[HttpPost(ApiRoutes.Identity.Register)]
		public async Task<IActionResult> Register([FromBody] UserRegistrationRequest request)
		{
			var authResponse = await _identityService.RegisterUserAsync(request.UserName, request.Password);

			if (!authResponse.Result.Succeeded)
			{
				return BadRequest(authResponse.Result.Errors);
			}

			var tokenResponse = GenerateTokenForUser(authResponse.UserId, authResponse.UserName);
			return Ok(tokenResponse);
		}

		[HttpPost(ApiRoutes.Identity.Login)]
		public async Task<IActionResult> Login([FromBody] UserLoginRequest request)
		{
			var authResponse = await _identityService.LoginUserAsync(request.UserName, request.Password);

			if (!authResponse.Result.Succeeded)
			{
				return BadRequest(authResponse.Result.Errors);
			}

			var tokenResponse = GenerateTokenForUser(authResponse.UserId, authResponse.UserName);
			return Ok(tokenResponse);
		}

		[HttpDelete(ApiRoutes.Identity.Delete)]
		public async Task<IActionResult> Delete(string id)
		{
			var res = await _identityService.DeleteUserAsync(id);

			if (!res.Succeeded)
				return BadRequest(res.Errors);

			return Ok(res.Succeeded);
		}

		private AuthTokenResponse GenerateTokenForUser(string userId, string userName)
		{
			var tokenHandler = new JwtSecurityTokenHandler();
			var key = Encoding.ASCII.GetBytes(_jwtProperties.Secret);
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
