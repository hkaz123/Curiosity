// <copyright company="Aeriandi Limited">
// Copyright (c) Aeriandi Limited. All Rights Reserved. Confidential and Proprietary Information of Aeriandi Limited.
// </copyright>

using System;
using System.Reactive.Concurrency;

namespace Rover.Common.Framework.Time
{
    public class SchedularBasedTimeProvider : ITimeProvider
    {
        private readonly IScheduler _scheduler;

        public SchedularBasedTimeProvider(IScheduler scheduler)
        {
            if (null == scheduler)
            {

                throw new ArgumentNullException("scheduler");
            }

            _scheduler = scheduler;
        }

        public DateTime Now
        {
            get { return _scheduler.Now.LocalDateTime; }
        }

        public DateTime UtcNow
        {
            get
            {
                return _scheduler.Now.LocalDateTime.ToUniversalTime();
            }
        }
    }
}
