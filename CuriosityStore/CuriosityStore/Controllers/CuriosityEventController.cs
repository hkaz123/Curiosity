using CuriosityStore.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CuriosityStore.Controllers
{
    public class CuriosityEventController : ApiController
    {
        List<CuriosityEvent> _dummyEvents = new List<CuriosityEvent>()
        {
            new CuriosityEvent() { Name = "DanIsCool" },
            new CuriosityEvent() { Name = "HasanIsCooler" }
        };

        public IEnumerable<CuriosityEvent> GetAll()
        {
            return _dummyEvents;
        }

        public IEnumerable<CuriosityEvent> GetRecent(TimeSpan periodBack)
        {
            return _dummyEvents;
        }

        public void Add(CuriosityEvent curiosityEvent)
        {
            throw new NotImplementedException();
        }
    }
}
