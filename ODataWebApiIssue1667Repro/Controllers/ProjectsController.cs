using Microsoft.AspNet.OData;
using Repro.Lib.Data;
using Repro.Lib.Models;
using System.Linq;

namespace Repro.Controllers
{
    public class ProjectsController : ODataController
    {
        private ReproDbContext _db;

        public ProjectsController(ReproDbContext db)
        {
            _db = db;
        }

        [EnableQuery]
        public IQueryable<Project> Get()
        {
            return _db.Projects;
        }

        [EnableQuery]
        public SingleResult<Project> Get([FromODataUri] int key)
        {
            return SingleResult.Create(_db.Projects.Where(d => d.Id.Equals(key)));
        }

        [EnableQuery]
        public IQueryable<Project> KeyProjects()
        {
            return _db.Projects;
        }
    }
}
