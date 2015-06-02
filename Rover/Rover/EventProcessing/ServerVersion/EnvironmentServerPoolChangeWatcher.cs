// <copyright company="Aeriandi Limited">
// Copyright (c) Aeriandi Limited. All Rights Reserved. Confidential and Proprietary Information of Aeriandi Limited.
// </copyright>

using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using Rover.Common;
using Rover.Common.Framework.Time;
using Rover.Common.ServerVersion;

namespace Rover.EventProcessing.ServerVersion
{
    /// <summary>
    /// Fires an event when the servers in the pool change (either added or not seen for 5 mins)
    /// </summary>
    class EnvironmentServerPoolChangeWatcher : Watcher
    {        
        private readonly String _environment;
        private readonly HashSet<String> _currentServersInThePool = new HashSet<string>(); //You'll need to add a datetime to this Hasan for the "not seen for 5 minutes"

        public EnvironmentServerPoolChangeWatcher(
            ITimeProvider timeProvider,
            IObservable<ServerVersionInputEvent> inputEvents, 
            String environment, 
            Action<OutputEvent> onOutputEventGenerated) : base(timeProvider)
        {
            _environment = environment;

            var changes = DetectNewServer(inputEvents, environment);
            changes.Subscribe(onOutputEventGenerated);
        }

        public String Environment { get { return _environment; } }

        private IObservable<OutputEvent> DetectNewServer(IObservable<ServerVersionInputEvent> currentVersions, String logicalEnvironment)
        {
            return currentVersions
                .Where(cv => cv.LogicalEnvironment == logicalEnvironment)
                .Where(cv => !_currentServersInThePool.Contains(cv.Server))
                .DistinctUntilChanged(cv => cv.Server)
                .Select(cv => 
                        {
                            _currentServersInThePool.Add(cv.Server);

                            return new OutputEvent(
                                TimeProvider.UtcNow, 
                                "WebServer Added To Pool", 
                                String.Format("'{0}' server added '{1}'. Current servers are:'{2}'", 
                                cv.LogicalEnvironment, 
                                cv.Server,
                                String.Join(",",_currentServersInThePool)));
                        });
        }
    }
}