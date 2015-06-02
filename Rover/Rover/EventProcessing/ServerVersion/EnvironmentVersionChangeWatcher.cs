// <copyright company="Aeriandi Limited">
// Copyright (c) Aeriandi Limited. All Rights Reserved. Confidential and Proprietary Information of Aeriandi Limited.
// </copyright>

using System;
using System.Reactive.Linq;
using Rover.Common;
using Rover.Common.Framework.Time;
using Rover.Common.ServerVersion;

namespace Rover.EventProcessing.ServerVersion
{
    /// <summary>
    /// Fires an event when the deployed version changes
    /// </summary>
    class EnvironmentVersionChangeWatcher : Watcher
    {        
        private readonly String _environment;

        public EnvironmentVersionChangeWatcher(
            ITimeProvider timeProvider,
            IObservable<ServerVersionInputEvent> inputEvents, 
            String environment,
            Action<OutputEvent> onOutputEventGenerated)
            : base(timeProvider)
        {
            _environment = environment;

            var changes = DetectVersionChangeOnEnvironment(inputEvents, environment);
            changes.Subscribe(onOutputEventGenerated);
        }

        public String Environment {  get { return _environment; } }

        private IObservable<OutputEvent> DetectVersionChangeOnEnvironment(IObservable<ServerVersionInputEvent> currentVersions, String logicalEnvironment)
        {
            return currentVersions
                .Where(cv => cv.LogicalEnvironment == logicalEnvironment)
                .DistinctUntilChanged(cv => cv.Version)
                .Select(cv => new OutputEvent(
                    TimeProvider.UtcNow, 
                    "WebServer Version Changed", 
                    String.Format("{0} changed version to {1}", cv.LogicalEnvironment, cv.Version)));
        }
    }
}