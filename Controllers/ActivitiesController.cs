using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TasksApi.Data;
using TasksApi.Models;

namespace TasksApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ActivitiesController : ControllerBase
    {
        private readonly ILogger<ActivitiesController> _logger;
        private readonly DataContext _context;

        /* The constructor  */
        public ActivitiesController(ILogger<ActivitiesController> logger, DataContext context)
        {
            //Dependency Injection
            _logger = logger;
            _context = context;
        }

        #region GET Methods

        /* Get list of books */
        [HttpGet, Authorize]
        public async Task<ActionResult<IEnumerable<Activity>>> GetActivities()
        {
            return await _context.Activities.ToListAsync();
        }

        /* Get Only one book */
        [HttpGet("{id}")]
        public async Task<ActionResult<Activity>> GetActivity(int id)
        {
            var activity = await _context.Activities.FindAsync(id);
            if (activity == null)
            {
                return NotFound();
            }

            return activity;
        }

        #endregion

        #region POST PUT DELETE Methods

        /* Create Book */
        [HttpPost]
        public async Task<ActionResult<Activity>> CreateBook(Activity activity)
        {
            _context.Add(activity);
            await _context.SaveChangesAsync();
            /* Return on created */
            return new CreatedAtRouteResult("GetActivity", new { id = activity.Id }, activity);
        }

        /* Update book */
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateActivity(int id, Activity activity)
        {
            if (activity.Id == id)
            {
                return BadRequest();
            }
            /* Update to indicate is being updated */
            _context.Entry(activity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return Ok();
        }


        /* Delete Book */
        [HttpDelete("{id}")]
        public async Task<ActionResult<Activity>> DeleteActivity(int id)
        {
            var activity = await _context.Activities.FindAsync(id);
            if (activity == null)
            {
                return NotFound();
            }

            _context.Activities.Remove(activity);
            await _context.SaveChangesAsync();

            return activity;
        }

        #endregion

    }
}