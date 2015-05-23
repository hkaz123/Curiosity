using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rover.Common
{
    public interface IEventSource<T> where T : InputEvent
    {
        IObservable<T> GetObservable();
    }
}
