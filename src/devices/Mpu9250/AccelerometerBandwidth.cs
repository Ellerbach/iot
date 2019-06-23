using System;
using System.Collections.Generic;
using System.Text;

namespace Mpu9250
{
    public enum AccelerometerBandwidth
    {
        Bandwidth1130Hz = 0,
        Bandwidth0460Hz = 0x08,
        Bandwidth0184Hz = 0x09,
        Bandwidth0092Hz = 0x0A,
        Bandwidth0041Hz = 0x0B,
        Bandwidth0020Hz = 0x0C,
        Bandwidth0010Hz = 0x0E,
        Bandwidth0005Hz = 0x0F,
    }
}
