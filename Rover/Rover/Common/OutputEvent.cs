using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rover.Common
{
    public class OutputEvent
    {
        public OutputEvent(){}
        public OutputEvent(
            DateTime timeStampUtc, 
            String name,
            string description)
        {
            TimeStampUtc = timeStampUtc;
            Name = name;
            Description = description;
        }


        public DateTime TimeStampUtc { get; set; }
        public String Name { get; set; }
        public String Description { get; set; }
        public String TriggeredBy { get; set; }

        public override string ToString()
        {
            return String.Format("{0}:{1}", Name, Description);
        }
    }
}
