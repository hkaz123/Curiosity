// <copyright company="Aeriandi Limited">
// Copyright (c) Aeriandi Limited. All Rights Reserved. Confidential and Proprietary Information of Aeriandi Limited.
// </copyright>

using Microsoft.Reactive.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rover.Common;
using Rover.Common.Framework.Time;
using Rover.Common.ServerVersion;
using Rover.EventProcessing.ServerVersion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Subjects;

namespace Rover.Test
{

    [TestClass]
    public class ServerVersionTests
    {
        [TestMethod]
        public void TestVersionChangePickedUpCorrectly()
        {
            Mock<IEventSource<ServerVersionInputEvent>> eventSourceMock =
                new Mock<IEventSource<ServerVersionInputEvent>>(MockBehavior.Strict);
            var inputEvents = new Subject<ServerVersionInputEvent>();
            eventSourceMock
                .Setup(esm => esm.GetObservable())
                .Returns(inputEvents);

            Mock<IEventSink> eventSinkMock = new Mock<IEventSink>(MockBehavior.Strict);
            IObservable<OutputEvent> outputEvents = null;

            eventSinkMock
                .Setup(esm => esm.SetOutputEvents(It.IsAny<IObservable<OutputEvent>>()))
                .Callback((IObservable<OutputEvent> oe) => outputEvents = oe);

            TestScheduler scheduler = new TestScheduler();
            ITimeProvider timeProvider = new SchedularBasedTimeProvider(scheduler);

            ServerVersionEventProcessor serverVersionEventProcessor = new ServerVersionEventProcessor(timeProvider);
            serverVersionEventProcessor.AssociateEventSource(eventSourceMock.Object);
            serverVersionEventProcessor.AssociateEventSink(eventSinkMock.Object);
            serverVersionEventProcessor.StartProcessing();

            Assert.IsNotNull(outputEvents);

            var outputEventsSeen = new List<OutputEvent>();
            outputEvents.Subscribe(outputEventsSeen.Add);
            
            DateTime startTime = new DateTime(2015,01,01,00,00,00,00);

            ServerVersionInputEventFactoryForTesting eventFactory = new ServerVersionInputEventFactoryForTesting(timeProvider);

            var eventsToSchedule = new List<Tuple<double, String, String, String>>
                         {
                             new Tuple<double, string, string, string>(0, "1AEROXFDAN001", "001", "Latest"),

                             new Tuple<double, string, string, string>(1, "1AEROXFDAN001", "001", "Secure"),
                             
                             new Tuple<double, string, string, string>(2, "1AEROXFDAN001", "001", "Latest"),
                             new Tuple<double, string, string, string>(3, "1AEROXFDAN001", "001", "Latest"),
                             new Tuple<double, string, string, string>(4, "1AEROXFDAN001", "001", "Latest"),
                             
                             new Tuple<double, string, string, string>(5, "1AEROXFDAN001", "001", "Secure"),
                             
                             new Tuple<double, string, string, string>(6, "1AEROXFDAN001", "001", "Latest"),
                             new Tuple<double, string, string, string>(7, "1AEROXFDAN001", "001", "Latest"),
                             new Tuple<double, string, string, string>(8, "1AEROXFDAN001", "001", "Latest"),

                             new Tuple<double, string, string, string>(9, "1AEROXFDAN001", "001", "Secure"),
                             new Tuple<double, string, string, string>(10, "1AEROXFDAN001", "001", "Secure"),
                             
                             new Tuple<double, string, string, string>(11, "1AEROXFDAN001", "001", "Latest"),
                             new Tuple<double, string, string, string>(12, "1AEROXFDAN001", "001", "Latest"),
                             new Tuple<double, string, string, string>(13, "1AEROXFDAN001", "001", "Latest"),

                             new Tuple<double, string, string, string>(14, "1AEROXFDAN001", "002", "Latest"),
                             new Tuple<double, string, string, string>(15, "1AEROXFHAS001", "002", "Latest")
                         };

            foreach (var eventDetail in eventsToSchedule)
            {
                Tuple<double, string, string, string> detail = eventDetail;

                scheduler.Schedule(new DateTimeOffset(startTime + TimeSpan.FromSeconds(eventDetail.Item1)),
                    () => inputEvents.OnNext(eventFactory.CreateServerVersionEvent(detail.Item2, detail.Item3, detail.Item4)));
            }

            Assert.AreEqual(0, outputEventsSeen.Count);

            scheduler.Start();

            Assert.AreEqual(6, outputEventsSeen.Count);

            Assert.AreEqual(3, outputEventsSeen.Count(oe => oe.Name == "WebServer Version Changed"));
            Assert.AreEqual(3, outputEventsSeen.Count(oe => oe.Name == "WebServer Added To Pool"));
            Assert.AreEqual(1, outputEventsSeen.Count(oe => oe.Description == "Latest changed version to 001"));
            Assert.AreEqual(1,
                outputEventsSeen.Count(
                    oe => oe.Description == "'Latest' server added '1AEROXFDAN001'. Current servers are:'1AEROXFDAN001'"));
            Assert.AreEqual(1, outputEventsSeen.Count(oe => oe.Description == "Latest changed version to 002"));
            Assert.AreEqual(1,
                outputEventsSeen.Count(
                    oe =>
                        oe.Description ==
                        "'Latest' server added '1AEROXFHAS001'. Current servers are:'1AEROXFDAN001,1AEROXFHAS001'"));

            //check the times are correct
            Assert.AreEqual(startTime + TimeSpan.FromSeconds(2), outputEventsSeen[0].TimeStampUtc);
            Assert.AreEqual(startTime + TimeSpan.FromSeconds(2), outputEventsSeen[1].TimeStampUtc);
            Assert.AreEqual(startTime + TimeSpan.FromSeconds(5), outputEventsSeen[2].TimeStampUtc);
            Assert.AreEqual(startTime + TimeSpan.FromSeconds(5), outputEventsSeen[3].TimeStampUtc);
            Assert.AreEqual(startTime + TimeSpan.FromSeconds(14), outputEventsSeen[4].TimeStampUtc);
            Assert.AreEqual(startTime + TimeSpan.FromSeconds(15), outputEventsSeen[5].TimeStampUtc);
        }       
    }
}