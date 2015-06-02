// <copyright company="Aeriandi Limited">
// Copyright (c) Aeriandi Limited. All Rights Reserved. Confidential and Proprietary Information of Aeriandi Limited.
// </copyright>

using System;

namespace Rover.Common.ServerVersion
{
    public class ServerVersionInputEvent : InputEvent
    {
        private readonly string _server;
        private readonly string _version;
        private readonly DateTime _bornOnUtc;
        private readonly DateTime _processStartedTimeUtc;
        private readonly DateTime _applicationDomainStartedTimeUtc;
        private readonly string _logicalEnvironment;

        public ServerVersionInputEvent(
            DateTime timestampUtc,
            String server ,
            String version,
            DateTime bornOnUtc,
            DateTime processStartedTimeUtc,        
            DateTime applicationDomainStartedTimeUtc,
            String logicalEnvironment) : base(timestampUtc)
        {
            _server = server;
            _version = version;
            _bornOnUtc = bornOnUtc;
            _processStartedTimeUtc = processStartedTimeUtc;
            _applicationDomainStartedTimeUtc = applicationDomainStartedTimeUtc;
            _logicalEnvironment = logicalEnvironment;
        }

        public string Server
        {
            get { return _server; }
        }

        public string Version
        {
            get { return _version; }
        }

        public DateTime BornOnUtc
        {
            get { return _bornOnUtc; }
        }

        public DateTime ProcessStartedTimeUtc
        {
            get { return _processStartedTimeUtc; }
        }

        public DateTime ApplicationDomainStartedTimeUtc
        {
            get { return _applicationDomainStartedTimeUtc; }
        }

        public string LogicalEnvironment
        {
            get { return _logicalEnvironment; }
        }


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
