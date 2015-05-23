using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rover.Common
{
    public interface IEventSink
    {
        void SetOutputEvents(IObservable<OutputEvent> outputEvents);
    }
}
