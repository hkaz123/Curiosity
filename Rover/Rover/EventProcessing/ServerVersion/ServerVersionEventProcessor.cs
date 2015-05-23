using Rover.Common.ServerVersion;
using Rover.EventProcessing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rover.EventProcessing.ServerVersion
{
    public class ServerVersionEventProcessor : EventProcessor<ServerVersionInputEvent>
    {
        protected override void OnAddInputEvent(ServerVersionInputEvent evt)
        {
            AddOutputEvent(new Common.OutputEvent(DateTime.UtcNow, "ServerVersion Changed"));
        }
    }
}
