﻿// <copyright company="Aeriandi Limited">
// Copyright (c) Aeriandi Limited. All Rights Reserved. Confidential and Proprietary Information of Aeriandi Limited.
// </copyright>

using System;

namespace Rover.Common
{
    public interface IEventSource<T> where T : InputEvent
    {
        IObservable<T> GetObservable();
    }
}
