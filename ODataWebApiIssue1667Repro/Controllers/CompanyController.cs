using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Routing;
using Microsoft.AspNetCore.Mvc;
using Repro.Lib.Data;
using Repro.Lib.Models;
using System.Linq;

namespace Repro.Controllers
{
    public class CompanyController : ODataController
    {
        private readonly ReproDbContext _db;

        public CompanyController(ReproDbContext db)
        {
            _db = db;
        }

        [EnableQuery]
        public IActionResult Get()
        {
            return Ok(_db.Companies.Single());
        }

        [EnableQuery(MaxExpansionDepth = 4)]
        [ODataRoute("Company/Projects")]
        public IQueryable<Project> GetProjects()
        {
            return _db.Projects;
        }
    }
}
