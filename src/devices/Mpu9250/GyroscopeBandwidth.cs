using System;
using System.Collections.Generic;
using System.Text;

namespace Mpu9250
{
    public enum GyroscopeBandwidth
    {
        Bandwidth0250Hz = 0,
        Bandwidth0184Hz = 1,
        Bandwidth0092Hz = 2,
        Bandwidth0041Hz = 3,
        Bandwidth0020Hz = 4,
        Bandwidth0010Hz = 5,
        Bandwidth0005Hz = 6,
        Bandwidth3600Hz = 7,
        Bandwidth3600HzFS32 = -1,
        Bandwidth8800HzFS32 = -2,
    }
}
