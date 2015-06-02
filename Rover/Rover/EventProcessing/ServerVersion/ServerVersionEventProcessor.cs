// <copyright company="Aeriandi Limited">
// Copyright (c) Aeriandi Limited. All Rights Reserved. Confidential and Proprietary Information of Aeriandi Limited.
// </copyright>

using Rover.Common.Framework.Time;
using Rover.Common.ServerVersion;
using System;
using System.Collections.Generic;
using System.Reactive.Linq;

namespace Rover.EventProcessing.ServerVersion
{
    public class ServerVersionEventProcessor : EventProcessor<ServerVersionInputEvent>
    {
        private readonly ITimeProvider _timeProvider;

        public ServerVersionEventProcessor(ITimeProvider timeProvider)
        {
            _timeProvider = timeProvider;
        }

        private readonly Object _lock = new object();
        private readonly Dictionary<String, List<Watcher>> _watchers = new Dictionary<string, List<Watcher>>();

        protected override void OnStartProcessing(IObservable<ServerVersionInputEvent> inputEvents)
        {
            var environmentChanges = DetectEnvironmentChanged(inputEvents);
            environmentChanges.Subscribe(environment => AddChangedWatchers(environment, inputEvents));
        }

        private void AddChangedWatchers(String environment, IObservable<ServerVersionInputEvent> inputEvents)
        {
            lock(_lock)
            {
                if (!_watchers.ContainsKey(environment))
                {
                    _watchers.Add(environment, new List<Watcher> {
                        new EnvironmentVersionChangeWatcher(_timeProvider, inputEvents, environment, AddOutputEvent),
                        new EnvironmentServerPoolChangeWatcher(_timeProvider, inputEvents, environment, AddOutputEvent)
                    });
                }
            }
        }

        private IObservable<String> DetectEnvironmentChanged(IObservable<ServerVersionInputEvent> currentVersions)
        {
            return currentVersions
                .DistinctUntilChanged(cv => cv.LogicalEnvironment)
                .Select(cv => cv.LogicalEnvironment);
        }
    }
}
