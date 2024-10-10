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

        // GET: api/Workassignments joita ei ole softdeletattu ja jotka ovat aktiivisia
        [HttpGet]
        public ActionResult GetWorkAssignments()
        {
            var workAssignments = db.WorkAssignments.Where(w => w.Active == true && w.Completed == false);
            return Ok(workAssignments);
        }

        // GET: api/Workassignments/5 START
        [HttpPost("start")]
        public bool Start([FromBody]Operation op)
        {
            //haetaan tietokannassa olevaa WA riviä ja tallennetaan tietokantaan
            WorkAssignment? wa = db.WorkAssignments.Find(op.WorkAssignmentID);

            //Jos työtehtävä on käynnissä sitä ei voi aloittaa ja palautetaan false
            if (wa.InProgress == true)
            {
                return (false);
            }

            //Jos kaikki ok, Muokataan WA riviä
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
                IdCustomer = wa.IdCustomer,
                CreatedAt = DateTime.Now.AddHours(1),
                Comments = op.Comment
            };

            db.Timesheets.Add(newEntry);

            db.SaveChanges();

            return (true);
        }


        // GET: api/Workassignments/5 STOP
        [HttpPost("stop")]
        public bool Stop([FromBody] Operation op)
        {
            //haetaan tietokannassa olevaa WA riviä ja tallennetaan tietokantaan
            WorkAssignment? wa = db.WorkAssignments.Find(op.WorkAssignmentID);

            //Jos työtehtävä on käynnissä sitä ei voi aloittaa ja palautetaan false
            if (wa.InProgress == false)
            {
                return (false);
            }

            //Jos kaikki ok, Muokataan WA riviä
            wa.InProgress = false;
            wa.Completed = true;
            wa.CompletedAt = DateTime.Now.AddHours(1);
            db.SaveChanges();

            //Haetaan Timesheet rivi tietokannasta
            var ts = db.Timesheets.Where(ts => ts.IdWorkAssignment == op.WorkAssignmentID).FirstOrDefault();

            //Muokataan tarvittavat tiedot
            ts.StopTime = DateTime.Now.AddHours(1);
            ts.Comments = op.Comment;
            ts.LastModifiedAt = DateTime.Now.AddHours(1);


            db.SaveChanges();

            return (true);
        }


    }
}
