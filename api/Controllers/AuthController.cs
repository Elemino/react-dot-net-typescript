using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using api.Models;
using api.Services;
using api.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace api.Controllers
{
    public class AuthController : Controller
    {
        private readonly UserManager<Contributor> _userManager;
        private readonly IOptions<IdentityOptions> _identityOptions;
        private readonly JwtOptions _jwtOptions;
        private readonly SignInManager<Contributor> _signInManager;
        private readonly ILogger _logger;

        public AuthController(
            UserManager<Contributor> userManager,
            IOptions<IdentityOptions> identityOptions,
            IOptions<JwtOptions> jwtOptions,
            SignInManager<Contributor> signInManager,
            ILoggerFactory loggerFactory)
        {
            _userManager = userManager;
            _identityOptions = identityOptions;
            _jwtOptions = jwtOptions.Value;
            _signInManager = signInManager;
            _logger = loggerFactory.CreateLogger<AuthController>();
        }

        [AllowAnonymous]
        [HttpPost("~/api/auth/login")]
        [Produces("application/json")]
        public async Task<IActionResult> Login(string username, string password)
        {
            // Ensure the username and password is valid.
            var user = await _userManager.FindByNameAsync(username);
            if (user == null || !await _userManager.CheckPasswordAsync(user, password))
            {
                return BadRequest(new
                {
                    error = "", //OpenIdConnectConstants.Errors.InvalidGrant,
                    error_description = "The username or password is invalid."
                });
            }

            _logger.LogInformation($"User logged in (id: {user.Id})");

            // Generate and issue a JWT token
            var claims = new [] {
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
              };
          
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
              issuer: _jwtOptions.Issuer,
              audience: _jwtOptions.Issuer,
              claims: claims,
              expires: DateTime.Now.AddMinutes(30),
              signingCredentials: creds);

            return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
        }

        [AllowAnonymous]
        [HttpPost("~/api/auth/register")]
        public async Task<IActionResult> Register(NewContributor model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = new Contributor { UserName = model.username, FirstName = model.firstName, LastName = model.lastName, Email = model.email, EmailConfirmed = true };

            var result = await _userManager.CreateAsync(user, model.password);

            if (result.Succeeded)
            {
                _logger.LogInformation($"New user registered (id: {user.Id})");

                return Ok();
            }

            return BadRequest(new { general = result.Errors.Select(x => x.Description) });
        }
    }
}
