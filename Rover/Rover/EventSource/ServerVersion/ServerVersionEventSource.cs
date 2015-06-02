// <copyright company="Aeriandi Limited">
// Copyright (c) Aeriandi Limited. All Rights Reserved. Confidential and Proprietary Information of Aeriandi Limited.
// </copyright>

using Rover.Common;
using Rover.Common.Framework.Time;
using Rover.Common.ServerVersion;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Rover.EventSource.ServerVersion
{
    /// <summary>
    /// Hits the web servers and generates Server.Version ServerVersionInputEvent events
    /// </summary>    
    public class ServerVersionEventSource : IEventSource<ServerVersionInputEvent>
    {
        private readonly ITimeProvider _timeProvider;
        private readonly List<String> _environmentUrls;
        private System.Threading.Timer _timer;
        private readonly Subject<ServerVersionInputEvent> _subject = new Subject<ServerVersionInputEvent>();
        
        public ServerVersionEventSource(
            ITimeProvider timeProvider,
            IEnumerable<String> environmentUrls)
        {
            _timeProvider = timeProvider;
            _environmentUrls = new List<string>(environmentUrls);
        }

        public void Start()
        {
            _timer = new System.Threading.Timer(OnTick, null, TimeSpan.Zero, TimeSpan.FromSeconds(1));
        }
        
        private async void OnTick(Object state)
        {
            foreach (String environmentUi in _environmentUrls)
            {
                String response = await GetURLContentsAsync(environmentUi);

                ServerVersionInputEvent inputEvent = ToServerVersionInputEvent(response);

                _subject.OnNext(inputEvent);
            }
        }
       
        private async Task<String> GetURLContentsAsync(string url)
        {
            var content = new MemoryStream();
            var webReq = (HttpWebRequest)WebRequest.Create(url);

            //We want a new TCP/IP connection so the load balancers give us a new connection to a new server in the pool
            webReq.KeepAlive = false;

            using (WebResponse response = await webReq.GetResponseAsync())
            using (Stream responseStream = response.GetResponseStream())
            {
                if (responseStream == null)
                {
                    throw new NullReferenceException("responseStream is null");
                }
            
                await responseStream.CopyToAsync(content);
            }
                        
            return Encoding.UTF8.GetString(content.ToArray());
        }
        
        private ServerVersionInputEvent ToServerVersionInputEvent(String response)
        {
            XElement serverVersion = XElement.Parse(response);

            return new ServerVersionInputEvent(
                _timeProvider.UtcNow,
                serverVersion.Elements("Server").First().Value,
                serverVersion.Elements("Version").First().Value,
                DateTime.Parse(serverVersion.Elements("BornOn").First().Value),
                DateTime.Parse(serverVersion.Elements("ProcessStartedTime").First().Value),
                DateTime.Parse(serverVersion.Elements("ApplicationDomainStartedTimeUTC").First().Value),
                serverVersion.Elements("LogicalEnvironment").First().Value);                 
        }
       
        public IObservable<ServerVersionInputEvent> GetObservable()
        {
            return _subject;
        }        
    }
}
