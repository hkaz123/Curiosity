// <copyright company="Aeriandi Limited">
// Copyright (c) Aeriandi Limited. All Rights Reserved. Confidential and Proprietary Information of Aeriandi Limited.
// </copyright>

using System;

namespace Rover.Common.Framework.Time
{
    public interface ITimeProvider
    {
        DateTime Now { get; }
        DateTime UtcNow { get; }
    }
}
