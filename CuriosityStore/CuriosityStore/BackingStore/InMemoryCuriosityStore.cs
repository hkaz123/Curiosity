using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CuriosityStore.BackingStore
{
    public class InMemoryCuriosityStore : ICuriosityStore
    {
        List<Curiosity> _curiosities = new List<Curiosity>();
        object _lock = new object();

        public void Add(Curiosity curiosity)
        {
            lock(_lock)
            {
                _curiosities.Add(curiosity);
            }
        }

        public IEnumerable<Curiosity> GetAll()
        {
            lock (_lock)
            {
                //Make a copy
                return _curiosities.ToList();
            }
        }

        public IEnumerable<Curiosity> GetSince(DateTime since)
        {
            lock (_lock)
            {
                return _curiosities.Where(c => c.TimeStamp > since).ToList();
            }             
        }
    }
}