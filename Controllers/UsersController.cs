using Microsoft.AspNetCore.Mvc;
using TasksApi.Data;
using TasksApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Authorization;

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

        /* Get Only one User */
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


        /* Update user */
        [HttpPut("{id}"), Authorize]
        public async Task<ActionResult> UpdateUser(int id, User requestUser)
        {
            if (requestUser.Id != id)
            {
                Console.WriteLine("Aqui es " + requestUser.Id + " Mas " + id);
                return BadRequest();
            }
            /* Update to indicate is being updated */

            string encryptedPassword = BCrypt.Net.BCrypt.HashPassword(requestUser.Password);
            requestUser.Password = encryptedPassword;
            _context.Entry(requestUser).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
                return Ok(new { data = requestUser, message = "Updated sucessfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException!.Data);
            }
        }


        [HttpPatch("{id}")]
        public async Task<ActionResult> UpdatePatchUser(int id, [FromBody] JsonPatchDocument<User> userPatch)
        {
            if (userPatch != null)
            {
                var user = await _context.Users.FindAsync(id);
                if (user != null)
                {
                    try
                    {
                        userPatch.ApplyTo(user, ModelState);
                        await _context.SaveChangesAsync();
                        if (!ModelState.IsValid)
                        {
                            return BadRequest(ModelState);
                        }

                        return Ok(new { message = "User Sucessfully updated" });
                    }
                    catch (Exception ex)
                    {
                        return BadRequest(ex.InnerException!.Data);
                    }
                }
                return NotFound(new { message = "User not found in Database", });
            }
            return BadRequest(new { message = "Something wen't wrong", });

        }



    }
}