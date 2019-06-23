using System;
using System.Collections.Generic;
using System.Text;

namespace Mpu9250
{
    [Flags]
    public enum DisableModes
    {
        DisableNone = 0,
        DisableAccelerometerX = 0b0010_0000,
        DisableAccelerometerY = 0b0001_0000,
        DisableAccelerometerZ = 0b0000_1000,
        DisableGyroscopeX = 0b0000_0100,
        DisableGyroscopeY = 0b0000_0010,
        DisableGyroscopeZ = 0b0000_0001,
    }
}
