using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rover.EventSource.ServerVersion;
using Rover.EventProcessing.ServerVersion;
using Moq;
using Rover.Common;
using Rover.Common.ServerVersion;
using System.Reactive.Subjects;
using System.Collections.Generic;
using System.Reactive.Concurrency;

namespace Rover.Test
{
    [TestClass]
    public class ServerVersionTests
    {
        [TestMethod]
        public void TestVersionChangePickedUpCorrectly()
        {
            Mock<IEventSource<ServerVersionInputEvent>> eventSourceMock = new Mock<IEventSource<ServerVersionInputEvent>>(MockBehavior.Strict);
            Subject<ServerVersionInputEvent> inputEvents = new Subject<ServerVersionInputEvent>();
            eventSourceMock
                .Setup(esm => esm.GetObservable())
                .Returns(inputEvents);

            Mock<IEventSink> eventSinkMock = new Mock<IEventSink>(MockBehavior.Strict);
            IObservable<OutputEvent> outputEvents = null;

            eventSinkMock
                .Setup(esm => esm.SetOutputEvents(It.IsAny<IObservable<OutputEvent>>()))
                .Callback((IObservable<OutputEvent> oe) => outputEvents = oe);

            ServerVersionEventProcessor serverVersionEventProcessor = new ServerVersionEventProcessor();
            serverVersionEventProcessor.AssociateEventSource(eventSourceMock.Object);
            serverVersionEventProcessor.AssociateEventSink(eventSinkMock.Object);
            serverVersionEventProcessor.StartProcessing();

            Assert.IsNotNull(outputEvents);

            var outputEventsSeen = new List<OutputEvent>();
            outputEvents.Subscribe(oe => outputEventsSeen.Add(oe));

            inputEvents.OnNext(new ServerVersionInputEvent() { LogicalEnvironment = "Latest", Server = "1AEROXFDAN001", Version = "001" });
            inputEvents.OnNext(new ServerVersionInputEvent() { LogicalEnvironment = "Secure", Server = "1AEROXFDAN001", Version = "001" });
            inputEvents.OnNext(new ServerVersionInputEvent() { LogicalEnvironment = "Latest", Server = "1AEROXFDAN001", Version = "001" });
            inputEvents.OnNext(new ServerVersionInputEvent() { LogicalEnvironment = "Latest", Server = "1AEROXFDAN001", Version = "001" });
            inputEvents.OnNext(new ServerVersionInputEvent() { LogicalEnvironment = "Latest", Server = "1AEROXFDAN001", Version = "001" });
            inputEvents.OnNext(new ServerVersionInputEvent() { LogicalEnvironment = "Secure", Server = "1AEROXFDAN001", Version = "001" });
            inputEvents.OnNext(new ServerVersionInputEvent() { LogicalEnvironment = "Latest", Server = "1AEROXFDAN001", Version = "001" });
            inputEvents.OnNext(new ServerVersionInputEvent() { LogicalEnvironment = "Latest", Server = "1AEROXFDAN001", Version = "001" });
            inputEvents.OnNext(new ServerVersionInputEvent() { LogicalEnvironment = "Latest", Server = "1AEROXFDAN001", Version = "001" });
            inputEvents.OnNext(new ServerVersionInputEvent() { LogicalEnvironment = "Secure", Server = "1AEROXFDAN001", Version = "001" });
            inputEvents.OnNext(new ServerVersionInputEvent() { LogicalEnvironment = "Secure", Server = "1AEROXFDAN001", Version = "001" });
            inputEvents.OnNext(new ServerVersionInputEvent() { LogicalEnvironment = "Latest", Server = "1AEROXFDAN001", Version = "001" });
            inputEvents.OnNext(new ServerVersionInputEvent() { LogicalEnvironment = "Latest", Server = "1AEROXFDAN001", Version = "001" });
            inputEvents.OnNext(new ServerVersionInputEvent() { LogicalEnvironment = "Latest", Server = "1AEROXFDAN001", Version = "001" });
            inputEvents.OnNext(new ServerVersionInputEvent() { LogicalEnvironment = "Latest", Server = "1AEROXFDAN001", Version = "002" });
            inputEvents.OnNext(new ServerVersionInputEvent() { LogicalEnvironment = "Latest", Server = "1AEROXFHAS001", Version = "002" });


            Assert.AreEqual(6, outputEventsSeen.Count);

            Assert.AreEqual(3, outputEventsSeen.Count(oe => oe.Name == "WebServer Version Changed"));
            Assert.AreEqual(3, outputEventsSeen.Count(oe => oe.Name == "WebServer Added To Pool"));
            Assert.AreEqual(1, outputEventsSeen.Count(oe => oe.Description == "Latest changed version to 001"));
            Assert.AreEqual(1, outputEventsSeen.Count(oe => oe.Description == "'Latest' server added '1AEROXFDAN001'. Current servers are:'1AEROXFDAN001'"));
            Assert.AreEqual(1, outputEventsSeen.Count(oe => oe.Description == "Latest changed version to 002"));
            Assert.AreEqual(1, outputEventsSeen.Count(oe => oe.Description == "'Latest' server added '1AEROXFHAS001'. Current servers are:'1AEROXFDAN001,1AEROXFHAS001'"));
        }
    }
}
