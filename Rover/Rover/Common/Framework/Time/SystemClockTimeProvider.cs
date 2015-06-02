// <copyright company="Aeriandi Limited">
// Copyright (c) Aeriandi Limited. All Rights Reserved. Confidential and Proprietary Information of Aeriandi Limited.
// </copyright>

using System;

namespace Rover.Common.Framework.Time
{
    public class SystemClockTimeProvider : ITimeProvider
    {
        public DateTime Now { get { return DateTime.Now; }}
        public DateTime UtcNow { get { return DateTime.UtcNow; } }
    }
}
