﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Mpu9250
{
    [Flags]
    public enum FifoModes
    {
        None = 0b0000_0000,
        I2CSlave0 = 0b0000_0001,
        I2CSlave1 = 0b0000_0010,
        I2CSlave2 = 0b0000_0100,
        Accelerometer = 0b0000_1000,
        GyrosocpeZ = 0b0001_0000,
        GyrosocpeY = 0b0010_0000,
        GyrosocpeX = 0b0100_0000,
        Temperature = 0b1000_0000
    }
}
