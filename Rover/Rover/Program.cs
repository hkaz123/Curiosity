using Rover.EventProcessing.ServerVersion;
using Rover.EventSink;
using Rover.EventSource.ServerVersion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rover
{
    class Program
    {
        static void Main(string[] args)
        {
            ServerVersionEventSource eventSource = new ServerVersionEventSource(new[] 
                {
                    "https://latest.liquid-contact.com/player/server.version",
                    "https://stage.liquid-contact.com/player/server.version",
                    "https://secure.liquid-contact.com/player/server.version"
                });
            eventSource.Start();

            ServerVersionEventProcessor serverVersionEventProcessor = new ServerVersionEventProcessor();
            serverVersionEventProcessor.AssociateEventSource(eventSource);
            serverVersionEventProcessor.AssociateEventSink(new ConsoleOutputEventSink());
            serverVersionEventProcessor.AssociateEventSink(new CuriosityEventSink("http://curiositystore.azurewebsites.net/api/curiosityevent/add"));

            Console.ReadKey();
        }
    }
}
