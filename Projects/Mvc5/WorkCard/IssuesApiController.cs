using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;
using Web.Models;

namespace Web
{
    public class IssuesApiController : ApiController
    {
        ApplicationDbContext dbContext = new ApplicationDbContext();
        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
        public IEnumerable<WorkIssue> GetToday()
        {
            var issues = dbContext.Issues
                .Where(t => (DbFunctions.TruncateTime(t.CreatedDate.Value) == DateTime.Now.Date) && !t.IsCompleted());
            return issues;
        }
        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}