using Rover.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;

namespace Rover.EventProcessing
{
    public abstract class EventProcessor<T> where T : InputEvent
    {
        private Subject<OutputEvent> _outputEvents = new Subject<OutputEvent>();
       
        public void AssociateEventSource(IEventSource<T> eventSource)
        {
            eventSource.GetObservable().Subscribe(OnAddInputEvent);
        }

        public void AssociateEventSink(IEventSink eventSink)
        {
            eventSink.SetOutputEvents(_outputEvents);
        }

        private IObservable<OutputEvent> GetOutputEvents()
        {
            return _outputEvents;
        }

        protected abstract void OnAddInputEvent(T evt);

        protected void AddOutputEvent(OutputEvent outputEvent)
        {
            _outputEvents.OnNext(outputEvent);
        }
    }
}
