﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Iot.Device.Pn5180
{
    /// <summary>
    /// The radio frequence status
    /// </summary>
    public enum RadioFrequenceStatus
    {
        /// <summary>Idle</summary>
        Idle = 0,

        /// <summary>Wait Transmit</summary>
        WaitTransmit = 1,

        /// <summary>Transmitting</summary>
        Transmitting = 2,

        /// <summary>Wait Receive</summary>
        WaitReceive = 3,

        /// <summary>Wait For Data</summary>
        WaitForData = 4,

        /// <summary>Receiving</summary>
        Receiving = 5,

        /// <summary>LoopBack</summary>
        LoopBack = 6,

        /// <summary>Error</summary>
        Error = 9,
    }
}
