using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Mvc;
using Repro.Lib.Data;
using Repro.Lib.Models;
using System.Linq;

namespace Repro.Controllers
{
    public class EmployeesController : ODataController
    {
        private readonly ReproDbContext _db;

        public EmployeesController(ReproDbContext db)
        {
            _db = db;
        }

        [EnableQuery]
        public IQueryable<Employee> Get()
        {
            return _db.Employees.AsQueryable();
        }

        [EnableQuery]
        public SingleResult<Employee> Get([FromODataUri] int key)
        {
            return SingleResult.Create(_db.Employees.Where(d => d.Id.Equals(key)));
        }

        public IActionResult GetReports([FromODataUri] int key)
        {
            return Ok(_db.Employees.Where(d => d.ManagerId != null && d.ManagerId == key));
        }
    }
}
