using Newtonsoft.Json;
using Rover.Common;
using Rover.EventSink.Curiosity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Rover.EventSink
{
    public class CuriosityEventSink : IEventSink
    {
        private readonly String _curiosityUrl;

        public CuriosityEventSink(String curiosityUrl)
        {
            _curiosityUrl = curiosityUrl;
        }

        public void SetOutputEvents(IObservable<OutputEvent> outputEvents)
        {
            outputEvents.Subscribe(PostEventToCuriosityStore);
        }

        private void PostEventToCuriosityStore(OutputEvent outputEvent)
        {
            CuriosityEvent curiosityEvent = new CuriosityEvent();
            curiosityEvent.TimeStamp = outputEvent.TimeStampUtc;
            curiosityEvent.Name = outputEvent.Name;

            WebRequest request = WebRequest.Create(_curiosityUrl);
            request.Method = "POST";

            request.ContentType = "application/json";

            string postData = JsonConvert.SerializeObject(curiosityEvent);
            byte[] byteArray = Encoding.UTF8.GetBytes(postData);
            request.ContentLength = byteArray.Length;

            Stream dataStream = request.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();

            WebResponse response = request.GetResponse();
            dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            string responseFromServer = reader.ReadToEnd();
            reader.Close();
            dataStream.Close();
            response.Close();
        }
    }
}

    