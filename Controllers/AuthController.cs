using Microsoft.AspNetCore.Mvc;
using TasksApi.Data;
using Newtonsoft.Json;
using TasksApi.Models;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using System.Security.Claims;
using System.Text;
using TasksApi.Dto;
using Microsoft.EntityFrameworkCore;

namespace TasksApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {

        private readonly ILogger<AuthController> _logger;
        private readonly DataContext _context;
        public IConfiguration _config;
        public static User user = new();

        /* The constructor  */
        public AuthController(ILogger<AuthController> logger, DataContext context, IConfiguration config)
        {
            //Dependency Injection
            _logger = logger;
            _context = context;
            _config = config;
        }


        [HttpPost]
        [Route("signup")]
        public async Task<ActionResult<User>> SignUp(AuthDto request)
        {
            string encryptedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);
            user.Email = request.Email;
            user.Password = encryptedPassword;
            user.Address = request.Address;
            user.Username = request.Username;
            user.Rol = "Admin";

            try
            {
                _context.Add(user);
                await _context.SaveChangesAsync();
                return Ok(user);
            }
            catch (Exception ex)
            {
                //Validating responses
                var state = ex.InnerException!.Data["SqlState"]!.ToString();
                if (state == "23505")
                {
                    return BadRequest(new { message = "Email is already in use" });
                }
                return BadRequest(ex.InnerException!.Data);
            }
        }


        [HttpPost]
        [Route("signin")]
        public async Task<ActionResult<string>> SignIn(AuthDto request)
        {
            var user = await _context.Users.Where(x => x.Email == request.Email).FirstOrDefaultAsync();
            /* If not found */
            if (user!.Email != request.Email)
            {
                return BadRequest(new { message = "User not found", });
            }
            /* If password not match */
            if (!BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
            {
                return BadRequest(new { message = "Wrong password", });
            }

            string token = CreateToken(user);


            return Ok(new { accessToken = token, user = new { id = user.Id, email = user.Email, username = user.Username } });

        }


        public string CreateToken(User user)
        {
            List<Claim> claims = new()
            {
                new Claim(ClaimTypes.Email,user.Username)
            };
            /* Getting JWT from configuration */
            var jwtConfig = _config.GetSection("Jwt").Get<Jwt>();
            /* Extracting the key from config and encoding it */
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig!.Key));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);
            /* Create and write token */
            var token = new JwtSecurityToken(
                claims: claims, expires: DateTime.Now.AddMinutes(20), signingCredentials: credentials

            );
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }

    }



}