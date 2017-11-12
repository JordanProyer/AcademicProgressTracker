using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;
using System.Web.UI.WebControls;
using AcademicProgressTracker.Models;
using AcademicProgressTracker.ViewModels;
using Microsoft.AspNet.Identity;

namespace AcademicProgressTracker.Controllers.Api
{
    public class SetupController : ApiController
    {
        private readonly ApplicationDbContext _context;

        public SetupController()
        {
            _context = new ApplicationDbContext();
        }

        // Get: api/Setup/Courses
        [HttpGet]
        public IHttpActionResult Courses()
        {
            var courses = _context.Course.ToList();

            if (courses == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return Ok(courses);
        }

        // Get: api/Setup/Years
        [HttpGet]
        public IHttpActionResult Years()
        {
            var years = _context.Year.ToList();

            if (years == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return Ok(years);
        }

        // Get: api/Setup/Modules?courseid=1&yearid=3&optional=1
        [HttpGet]
        public IHttpActionResult Modules(int courseid, int yearid, int optional)
        {
            var modules = _context.Module.Where(x => x.CourseId == courseid
                                                && x.YearId == yearid
                                                && x.Optional == optional);

            if (modules == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            return Ok(modules);
        }

        // Get: api/Setup/Credits?moduleid=1&moduleid=2&moduleid=3
        [HttpPost]
        public IHttpActionResult Credits([FromUri]int[] moduleid)
        {
            var moduleDb = _context.Module;
            var moduleList = moduleDb.Select(x => x.Id).ToList();
            int totalCredits = 0;
            foreach (var id in moduleid)
            {
                if (moduleList.Contains(id))
                {
                    var creditValue = moduleDb.Where(x => x.Id == id).Select(y => y.Credits).Single();
                    totalCredits += creditValue;
                }
            }

            return Ok(totalCredits);
        }


        //Post = create
        // POST: api/setup/CreateUserModule?userId=3&moduleId=5
        [HttpPost]
        public IHttpActionResult CreateUserModule(int userId, int moduleId )
        {
            var userModule = new UserModules()
            {
                UserId = userId,
                ModuleId = moduleId
            };

            if (userModule == null)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            _context.UserModules.Add(userModule);
            _context.SaveChanges();

            return Created(new Uri(Request.RequestUri + "/" + userId + "/" + moduleId), String.Format(userId + " " + moduleId));
        }

        //Put = update
        // PUT: api/Setup/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Setup/5
        public void Delete(int id)
        {
        }
    }
}
