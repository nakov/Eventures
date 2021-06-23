using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

using Eventures.Data;
using Eventures.WebAPI.Models;

namespace Eventures.WebAPI.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UsersController : Controller
    {
        private ApplicationDbContext dbContext;
        private UserManager<EventuresUser> userManager;
        private readonly IConfiguration _configuration;

        public UsersController(ApplicationDbContext context,
            IConfiguration configuration)
        {
            this.dbContext = context;
            this._configuration = configuration;

            var userStore = new UserStore<EventuresUser>(dbContext);
            var hasher = new PasswordHasher<EventuresUser>();
            var normalizer = new UpperInvariantLookupNormalizer();
            var factory = new LoggerFactory();
            var logger = new Logger<UserManager<EventuresUser>>(factory);
            this.userManager = new UserManager<EventuresUser>(
                userStore, null, hasher, null, null, normalizer, null, null, logger);
        }

        /// <summary>
        /// Logs a user in.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/users/login
        ///     {
        ///        "username": "someUsername",
        ///        "password": "somePassword"
        ///     }
        ///
        /// </remarks>
        /// <param name="model"></param>
        /// <response code="200">Returns "OK" with JWT token with expiration date.</response>
        /// <response code="401">Returns "Unauthorized".</response>    
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] ApiLoginModel model)
        {
            var user = await userManager.FindByNameAsync(model.Username);
            if (user != null && await userManager.CheckPasswordAsync(user, model.Password))
            {
                var userRoles = await userManager.GetRolesAsync(user);

                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }

                string jwtSecret = _configuration["JWT:Secret"];
                byte[] jwtSecretBytes = Encoding.UTF8.GetBytes(jwtSecret);
                var authSigningKey = new SymmetricSecurityKey(jwtSecretBytes);

                var token = new JwtSecurityToken(
                    issuer: _configuration["JWT:ValidIssuer"],
                    audience: _configuration["JWT:ValidAudience"],
                    expires: DateTime.Now.AddHours(3),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                    );

                return Ok(new ResponseWithToken
                {
                    Token = new JwtSecurityTokenHandler().WriteToken(token),
                    Expiration = token.ValidTo
                });
            }
            return Unauthorized();
        }

        /// <summary>
        /// Registers a new user.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/users/register
        ///     {
        ///         "username": "someUsername",
        ///         "email": "someUsername@mail.bg",
        ///         "firstName": "someName",
        ///         "lastName": "someLastName",
        ///         "password": "somePassword",
        ///         "confirmPassword": "somePassword"
        ///     }
        /// </remarks>
        /// <response code="200">Returns "OK" with "Success" status and "User created successfully! message".</response>
        /// <response code="500">Returns "InternalServerError" when user already exists or user creation failed.</response>    
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] ApiRegisterModel model)
        {
            var userExists = await userManager.FindByNameAsync(model.Username);
            if (userExists != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseMsg { Status = "Error", Message = "User already exists!" });

            EventuresUser user = new EventuresUser()
            {
                Email = model.Email,
                UserName = model.Username,
                FirstName = model.FirstName,
                LastName = model.LastName,
                SecurityStamp = Guid.NewGuid().ToString()
            };
            var result = await userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseMsg { Status = "Error", Message = "User creation failed! Please check user details and try again." });

            return Ok(new ResponseMsg { Status = "Success", Message = "User created successfully!" });
        }

        /// <summary>
        /// Gets a list with all users.
        /// </summary>
        /// <remarks>
        /// You should be an authenticated user!
        /// 
        /// Sample request:
        ///
        ///     GET /api/users
        ///     {
        ///         
        ///     }
        /// </remarks>
        /// <response code="200">Returns "OK" with a list of all users.</response>
        /// <response code="401">Returns "Unauthorized" when user is not authenticated.</response>    
        [Authorize]
        [HttpGet]
        public IActionResult GetAll()
        {
            var users = new List<ApiUserViewModel>();

            var dbUsers = dbContext.Users.ToList();
            foreach (var dbUser in dbUsers)
            {
                var user = new ApiUserViewModel
                {
                    Id = dbUser.Id,
                    FirstName = dbUser.FirstName,
                    LastName = dbUser.LastName,
                    Username = dbUser.UserName,
                    Email = dbUser.Email
                };
                users.Add(user);
            }
            return Ok(users);
        }
    }
}
