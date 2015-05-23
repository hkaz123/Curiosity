using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rover.Common.ServerVersion
{
    public class ServerVersionInputEvent : InputEvent
    {
        public ServerVersionInputEvent(){}
        public ServerVersionInputEvent(
            String server ,
            String version,
            DateTime bornOnUtc,
            DateTime processStartedTimeUtc,        
            DateTime applicationDomainStartedTimeUtc,
            String logicalEnvironment )
        {
            Server = server;
            Version = version;
            BornOnUtc = bornOnUtc;
            ProcessStartedTimeUtc = processStartedTimeUtc;
            ApplicationDomainStartedTimeUtc = applicationDomainStartedTimeUtc;
            LogicalEnvironment = logicalEnvironment;
        }

        public String Server { get; set; }
        public String Version { get; set; }
        public DateTime BornOnUtc { get; set; }
        public DateTime ProcessStartedTimeUtc { get; set; }
        public DateTime ApplicationDomainStartedTimeUtc { get; set; }
        public String LogicalEnvironment { get; set; }

        public override string ToString()
        {
            return String.Format("Server:'{0}',Version:'{1}', BornOnUtc:'{2}', ProcessStartedTimeUtc:'{3}', ApplicationDomainStartedTimeUtc:'{4}', LogicalEnvironment:'{5}' ", 
                Server, 
                Version,
                BornOnUtc,
                ProcessStartedTimeUtc,
                ApplicationDomainStartedTimeUtc,
                LogicalEnvironment);
        }
    }
}
