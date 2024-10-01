using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TimeSheetAPI.Models;

namespace TimeSheetAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkassignmentsController : ControllerBase
    {
        //private readonly tuntidbContext db = new();
        private readonly tuntidbContext db;

        public WorkassignmentsController(tuntidbContext context)
        {
            db = context;
        }

        // GET: api/Workassignments
        [HttpGet]
        public ActionResult GetWorkAssignments()
        {
            var workAssignments = db.WorkAssignments.Where(w => w.Active == true);
            return Ok(workAssignments);
        }
    }
}
