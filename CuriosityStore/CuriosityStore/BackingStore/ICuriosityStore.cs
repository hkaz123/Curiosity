using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CuriosityStore.BackingStore
{
    interface ICuriosityStore
    {
        void Add(Curiosity curiosity);
        IEnumerable<Curiosity> GetAll();
        IEnumerable<Curiosity> GetSince(DateTime since);
    }
}
