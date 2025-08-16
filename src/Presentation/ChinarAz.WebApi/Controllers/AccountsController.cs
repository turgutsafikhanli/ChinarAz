using ChinarAz.Application.Abstracts.Services;
using ChinarAz.Application.DTOs.UserDtos;
using ChinarAz.Application.Shared;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ChinarAz.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccountsController : ControllerBase
{
    private IUserService _userService { get; }
    public AccountsController(IUserService userService)
    {
        _userService = userService;

    }
    // GET: api/<AccountsController>
    [HttpGet("test-values")]
    public IEnumerable<string> Get()
    {
        return new string[] { "value1", "value2" };
    }

    // GET api/<AccountsController>/5
    [HttpGet("{id}")]
    public string Get(int id)
    {
        return "value";
    }

    // POST api/<AccountsController>
    [HttpPost("register")]
    [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.InternalServerError)]
    public async Task<IActionResult> Register([FromBody] UserRegisterDto dto)
    {
        var result = await _userService.Register(dto);
        return StatusCode((int)result.StatusCode, result);
    }

    [HttpPost("login")]
    [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.InternalServerError)]
    public async Task<IActionResult> Login([FromBody] UserLoginDto dto)
    {
        var result = await _userService.Login(dto);
        return StatusCode((int)result.StatusCode, result);
    }

    [HttpPost("refresh-token")]
    [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.InternalServerError)]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest dto)
    {
        var result = await _userService.RefreshTokenAsync(dto);
        return StatusCode((int)result.StatusCode, result);
    }


    [HttpGet("confirm-email")]
    [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.InternalServerError)]
    public async Task<IActionResult> ConfirmEmail([FromQuery] string userId, [FromQuery] string token)
    {
        var result = await _userService.ConfirmEmail(userId, token);
        return StatusCode((int)result.StatusCode, result);

    }
    // PUT api/<AccountsController>/5
    [HttpPut("{id}")]
    public void Put(int id, [FromBody] string value)
    {
    }

    // DELETE api/<AccountsController>/5
    [HttpDelete("{id}")]
    public void Delete(int id)
    {
    }

    [HttpPost("send-reset-password-email")]
    [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> SendResetPasswordEmail([FromBody] string email)
    {
        var result = await _userService.SendResetPasswordEmailAsync(email);
        if (result.StatusCode != HttpStatusCode.OK)
        {
            return StatusCode((int)result.StatusCode, result);
        }
        return Ok(result.Message);
    }

    [HttpPost("reset-password")]
    [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> ResetPassword([FromBody] UserResetPasswordDto dto)
    {
        var result = await _userService.ResetPasswordAsync(dto);
        if (result.StatusCode != HttpStatusCode.OK)
        {
            return StatusCode((int)result.StatusCode, result);
        }
        return Ok(result.Message);
    }

    [HttpGet("google-login")]
    public IActionResult GoogleLogin(string returnUrl = "/google-response")
    {
        var properties = new AuthenticationProperties
        {
            RedirectUri = Url.Action("GoogleResponse", "Accounts", new { returnUrl })
        };
        return Challenge(properties, GoogleDefaults.AuthenticationScheme);
    }

    [HttpGet("google-response")]
    public async Task<IActionResult> GoogleResponse(string returnUrl = "/")
    {
        var result = await HttpContext.AuthenticateAsync(IdentityConstants.ExternalScheme);
        if (!result.Succeeded)
            return BadRequest("External authentication error");

        var externalUser = result.Principal;
        var email = externalUser?.FindFirst(ClaimTypes.Email)?.Value;
        var name = externalUser?.FindFirst(ClaimTypes.Name)?.Value;

        if (email is null)
            return BadRequest("Google account has no email");

        // İstifadəçini sistemə daxil et və ya qeydiyyatdan keçir
        var tokenResponse = await _userService.HandleExternalLoginAsync(email, name);

        // JWT token cavabı qaytar
        return Ok(tokenResponse);
    }
    [HttpPost("add-roles")]
    public async Task<IActionResult> AddRolesToUser([FromBody] UserRoleAssignDto dto)
    {
        var result = await _userService.AddRole(dto);
        return StatusCode((int)result.StatusCode, result);
    }
}
