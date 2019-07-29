﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Iot.Device.Pn532.AsTarget
{
    /// <summary>
    /// When PN532 is acting as a target, the baud rate
    /// it is engaged to
    /// </summary>
    public enum TargetBaudRateInialized
    {
        B106kbps = 0b0000_0000,
        B212kbps = 0b0001_0000,
        B424kbps = 0b0010_0000,
    }
}
