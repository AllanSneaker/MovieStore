using Microsoft.AspNetCore.Mvc;
using MovieStore.Application.Common.Interfaces;
using MovieStore.WebUI.Configurations.Jwt;
using MovieStore.WebUI.Contracts.V1;
using MovieStore.WebUI.Controllers.V1.Requests;
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
			var authResponse = await _identityService.RegisterUserAsync(request.UserName, request.Password, _jwtProperties.Secret);

			if (!authResponse.Result.Succeeded)
			{
				return BadRequest(authResponse.Result.Errors);
			}

			var tokenResponse = authResponse.authToken;
			return Ok(tokenResponse);
		}

		[HttpPost(ApiRoutes.Identity.Login)]
		public async Task<IActionResult> Login([FromBody] UserLoginRequest request)
		{
			var authResponse = await _identityService.LoginUserAsync(request.UserName, request.Password, _jwtProperties.Secret);

			if (!authResponse.Result.Succeeded)
			{
				return BadRequest(authResponse.Result.Errors);
			}

			var tokenResponse = authResponse.authToken;
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
	}
}
