using System;
using System.Collections.Generic;
using System.Text;

namespace Mpu9250
{
    public enum AccelerometerLowPowerMode
    {
        Freq0Dot24Hz = 0,
        Freq0Dot49Hz = 1,
        Freq0Dot98Hz = 2,
        Freq1Dot95Hz = 3,
        Freq3Dot91Hz = 4,
        Freq7Dot81Hz = 5,
        Freq15Dot63Hz = 6,
        Freq31Dot25Hz = 7,
        Freq62Dot50Hz = 8,
        Freq125Hz = 9,
        Freq250Hz = 10,
        Freq500Hz = 11,
    }
}
