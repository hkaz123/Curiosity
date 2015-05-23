using CuriosityStore.BackingStore;
using CuriosityStore.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CuriosityStore.Controllers
{
    public static class CuriosityExtensions
    {
        public static CuriosityEvent ToCuriosityEvent(this Curiosity curiosity)
        {
            return new CuriosityEvent() { TimeStamp = curiosity.TimeStamp, Name = curiosity.Name };        
        }

        public static IEnumerable<CuriosityEvent> ToCuriosityEvents(this IEnumerable<Curiosity> curiosity)
        {
            return curiosity.Select(ToCuriosityEvent).ToList();
        }

        public static Curiosity ToCuriosity(this CuriosityEvent  curiosityEvent)
        {
            return new Curiosity() { TimeStamp = curiosityEvent.TimeStamp, Name = curiosityEvent.Name };
        }
    }
}