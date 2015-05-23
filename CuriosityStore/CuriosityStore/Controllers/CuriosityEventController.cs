using CuriosityStore.BackingStore;
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
        private readonly static ICuriosityStore _inMemoryCuriosityStore = new InMemoryCuriosityStore();

        public IEnumerable<CuriosityEvent> GetAll()
        {
            return GetCuriosityStore().GetAll().ToCuriosityEvents();
        }

        public IEnumerable<CuriosityEvent> GetRecent(DateTime since)
        {
            return GetCuriosityStore().GetSince(since).ToCuriosityEvents();
        }

        public void Add(CuriosityEvent curiosityEvent)
        {
            GetCuriosityStore().Add(curiosityEvent.ToCuriosity());
        }

        private ICuriosityStore GetCuriosityStore()
        {
            return _inMemoryCuriosityStore;
        }
    }
}
