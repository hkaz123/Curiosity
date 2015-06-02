// <copyright company="Aeriandi Limited">
// Copyright (c) Aeriandi Limited. All Rights Reserved. Confidential and Proprietary Information of Aeriandi Limited.
// </copyright>

using Rover.Common.Framework.Time;
using Rover.EventProcessing.ServerVersion;
using Rover.EventSink;
using Rover.EventSource.ServerVersion;
using System;

namespace Rover
{
    class Program
    {
        static void Main(string[] args)
        {
            ITimeProvider timeProvider = new SystemClockTimeProvider();

            ServerVersionEventSource eventSource = new ServerVersionEventSource(
                timeProvider,
                new[] 
                {
                    "https://latest.liquid-contact.com/player/server.version",
                    "https://stage.liquid-contact.com/player/server.version",
                    "https://secure.liquid-contact.com/player/server.version"
                });
            eventSource.Start();

            ServerVersionEventProcessor serverVersionEventProcessor = new ServerVersionEventProcessor(timeProvider);
            serverVersionEventProcessor.AssociateEventSource(eventSource);
            serverVersionEventProcessor.AssociateEventSink(new ConsoleOutputEventSink());
            serverVersionEventProcessor.AssociateEventSink(new CuriosityEventSink("http://curiositystore.azurewebsites.net/api/curiosityevent/add"));

            serverVersionEventProcessor.StartProcessing();

            Console.ReadKey();
        }
    }
}
