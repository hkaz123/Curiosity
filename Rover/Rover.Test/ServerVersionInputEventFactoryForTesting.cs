// <copyright company="Aeriandi Limited">
// Copyright (c) Aeriandi Limited. All Rights Reserved. Confidential and Proprietary Information of Aeriandi Limited.
// </copyright>

using System;
using Rover.Common.Framework.Time;
using Rover.Common.ServerVersion;

namespace Rover.Test
{
    internal class ServerVersionInputEventFactoryForTesting
    {
        private readonly ITimeProvider _timeProvider;

        public ServerVersionInputEventFactoryForTesting(ITimeProvider timeProvider)
        {
            _timeProvider = timeProvider;
        }

        public ServerVersionInputEvent CreateServerVersionEvent(String server, String versionNumber, String environment)
        {
            return new ServerVersionInputEvent(
                _timeProvider.UtcNow,
                server,
                versionNumber,
                new DateTime(2015, 05, 05, 12, 00, 00), //These aren't used in this test
                new DateTime(2015, 05, 05, 12, 00, 00), //These aren't used in this test
                new DateTime(2015, 05, 05, 12, 00, 00), //These aren't used in this test
                environment);
        }
    }
}