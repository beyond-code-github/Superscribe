namespace Superscribe.WebApi2.MultipleCollectionsPerController.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Http;

    public class MailController : ApiController
    {
        private readonly dynamic[] mail =
            {
                new { Id = 1, Location = "Inbox", Subject = "value1" },
                new { Id = 2, Location = "Inbox", Subject = "value2" },
                new { Id = 3, Location = "Inbox", Subject = "value3" },
                new { Id = 4, Location = "Deleted", Subject = "value4" },
                new { Id = 5, Location = "Sent", Subject = "value5" }
            };

        public IEnumerable<object> Get()
        {
            return this.mail;
        }

        public IEnumerable<object> Get(string location)
        {
            return this.mail.Where(o => o.Location == location);
        }

        public object GetById(int id)
        {
            return this.mail.FirstOrDefault(o => o.Id == id);
        }
    }
}
