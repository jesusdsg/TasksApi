using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TasksApi.Data;
using TasksApi.Models;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Newtonsoft.Json.Linq;

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
        public UsersController(ILogger<UsersController> logger, DataContext context)
        {
            //Dependency Injection
            _logger = logger;
            _context = context;
        }



    }
}