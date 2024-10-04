using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TimeSheetAPI.Models;
using TimeSheetApp.Models;

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

        // GET: api/Workassignments/5 START
        [HttpGet("start")]
        public bool Start([FromBody]Operation op)
        {
            //Muokataan tietokannassa olevaa WA riviä
            WorkAssignment? wa = db.WorkAssignments.Find(op.WorkAssignmentID);

            wa.InProgress = true;
            wa.WorkStartedAt = DateTime.Now.AddHours(1);
            db.SaveChanges();

            //Luodaan uusi Timesheet rivi tietokantaan
            Timesheet newEntry = new Timesheet()
            {
                IdWorkAssignment = op.WorkAssignmentID,
                StartTime = DateTime.Now.AddHours(1),
                Active = true,
                IdEmployee = op.EmployeeID,
                IdCustomer = wa.IdCustomer
                CreatedAt = DateTime.Now.AddHours(1),
                Comments = op.Comment
            };

            db.Timesheets.Add(newEntry);

            db.SaveChanges();

            return (true);
        }


    }
}
