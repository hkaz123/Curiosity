// <copyright company="Aeriandi Limited">
// Copyright (c) Aeriandi Limited. All Rights Reserved. Confidential and Proprietary Information of Aeriandi Limited.
// </copyright>

using Rover.Common.Framework.Time;

namespace Rover.EventProcessing.ServerVersion
{
    abstract class Watcher
    {
        private readonly ITimeProvider _timeProvider;

        protected Watcher(ITimeProvider timeProvider)
        {
            _timeProvider = timeProvider;
        }

        protected ITimeProvider TimeProvider
        {
            get { return _timeProvider; }
        }
    }
}