using Rover.Common;
using Rover.Common.ServerVersion;
using Rover.EventProcessing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rover.EventProcessing.ServerVersion
{
    public class ServerVersionEventProcessor : EventProcessor<ServerVersionInputEvent>
    {
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
                    _watchers.Add(environment, new List<Watcher>() {
                        new EnvironmentVersionChangeWatcher(inputEvents, environment, AddOutputEvent),
                        new EnvironmentServerPoolChangeWatcher(inputEvents, environment, AddOutputEvent)
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

    abstract class Watcher { }

    /// <summary>
    /// Fires an event when the deployed version changes
    /// </summary>
    class EnvironmentVersionChangeWatcher : Watcher
    {        
        private readonly String _environment;

        public EnvironmentVersionChangeWatcher(IObservable<ServerVersionInputEvent> inputEvents, String environment, Action<OutputEvent> onOutputEventGenerated)
        {
            _environment = environment;

            var changes = DetectVersionChangeOnEnvironment(inputEvents, environment);
            changes.Subscribe(onOutputEventGenerated);
        }

        public String Environment {  get { return _environment; } }

        private static IObservable<OutputEvent> DetectVersionChangeOnEnvironment(IObservable<ServerVersionInputEvent> currentVersions, String logicalEnvironment)
        {
            return currentVersions
                .Where(cv => cv.LogicalEnvironment == logicalEnvironment)
                .DistinctUntilChanged(cv => cv.Version)
                .Select(cv => new OutputEvent(DateTime.UtcNow, "WebServer Version Changed", String.Format("{0} changed version to {1}", cv.LogicalEnvironment, cv.Version)));
        }
    }

    /// <summary>
    /// Fires an event when the servers in the pool change (either added or not seen for 5 mins)
    /// </summary>
    class EnvironmentServerPoolChangeWatcher : Watcher
    {
        private readonly String _environment;
        private HashSet<String> _currentServersInThePool = new HashSet<string>(); //You'll need to add a datetime to this Hasan for the "not seen for 5 minutes"

        public EnvironmentServerPoolChangeWatcher(IObservable<ServerVersionInputEvent> inputEvents, String environment, Action<OutputEvent> onOutputEventGenerated)
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

                    return new OutputEvent(DateTime.UtcNow, "WebServer Added To Pool", String.Format("'{0}' server added '{1}'. Current servers are:'{2}'", 
                        cv.LogicalEnvironment, 
                        cv.Server,
                        String.Join(",",_currentServersInThePool)));

                });
        }
    }

}
