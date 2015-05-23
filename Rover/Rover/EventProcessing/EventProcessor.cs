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
        private Subject<T> _inputEvents = new Subject<T>();
        
        public void AssociateEventSource(IEventSource<T> eventSource)
        {
            //This multiplexes the event sources
            eventSource.GetObservable().Subscribe(_inputEvents.OnNext, _inputEvents.OnError, _inputEvents.OnCompleted);
        }

        public void AssociateEventSink(IEventSink eventSink)
        {
            eventSink.SetOutputEvents(_outputEvents);
        }

        public void StartProcessing()
        {
            OnStartProcessing(GetInputEvents());
        }

        protected abstract void OnStartProcessing(IObservable<T> inputEvents);

        private IObservable<OutputEvent> GetOutputEvents()
        {
            return _outputEvents;
        }

        private IObservable<T> GetInputEvents()
        {
            return _inputEvents;
        }

        protected void AddOutputEvent(OutputEvent outputEvent)
        {
            _outputEvents.OnNext(outputEvent);
        }
    }
}
