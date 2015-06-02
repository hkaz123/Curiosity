// <copyright company="Aeriandi Limited">
// Copyright (c) Aeriandi Limited. All Rights Reserved. Confidential and Proprietary Information of Aeriandi Limited.
// </copyright>

using System;

namespace Rover.Common
{
    public class InputEvent
    {
        private readonly DateTime _timeStampUtc;

        public InputEvent(DateTime timeStampUtc)
        {
            _timeStampUtc = timeStampUtc;
        }

        public DateTime TimeStampUtc
        {
            get { return _timeStampUtc; }
        }
    }
}
