using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TasksApi.Data;
using TasksApi.Models;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Newtonsoft.Json.Linq;
using Microsoft.EntityFrameworkCore;

namespace TasksApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly ILogger<UsersController> _logger;
        private readonly DataContext _context;
        public IConfiguration _config;

        /* The constructor  */
        public UsersController(ILogger<UsersController> logger, DataContext context, IConfiguration config)
        {
            //Dependency Injection
            _logger = logger;
            _context = context;
            _config = config;
        }

        /* Create User */
        /* [HttpPost]
        public async Task<ActionResult<User>> CreateUser(User user)
        {
            _context.Add(user);
            await _context.SaveChangesAsync();
            return new CreatedAtRouteResult("GetUser", new { id = user.Id }, user);
        } */


        /* Get Only one book */
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        /* Update book */
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateUser(int id, User user)
        {
            var exists = await _context.Users.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (id != exists?.Id)
            {
                return BadRequest(new { message = "User not found in Database", });
            }
            /* Update to indicate is being updated */
            //_context.Users.Update(user);
            _context.Entry(user).State = EntityState.Modified;
            var result = await _context.SaveChangesAsync();
            return Ok(new { data = result });
        }



    }
}