﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace Iot.Device.Card.CreditCardProcessing
{
    /// <summary>
    /// Th esourvce of a Tag
    /// </summary>
    [Flags]
    public enum Source
    {
        Icc,
        Terminal,
        Issuer
    }
}