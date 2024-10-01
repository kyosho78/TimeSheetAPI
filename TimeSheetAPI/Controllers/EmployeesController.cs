using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;
using TimeSheetAPI.Models;

namespace TimeSheetAPI.Controllers
{
    


    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        //Luodaan tietokantayhteys, db on Olio-ohjelmoinnin käsitteitä
        //private readonly tuntidbContext db = new();
        private readonly tuntidbContext db;

        public EmployeesController(tuntidbContext context)
        {
            db = context;
        }

        // GET: api/Employees
        [HttpGet]
        public ActionResult GetEmployees()
        {
            var employees = db.Employees.Where(e => e.Active == true);
            return Ok(employees);
        }





        


    }
}
