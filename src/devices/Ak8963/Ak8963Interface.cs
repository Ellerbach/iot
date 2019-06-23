using System;
using System.Collections.Generic;
using System.Device.I2c;
using System.Text;

namespace Ak8963
{
    public abstract class Ak8963Interface
    {
        public abstract void WriteRegister(I2cDevice i2CDevice, Register reg, byte data);
        public abstract byte ReadByte(I2cDevice i2CDevice, Register reg);
        public abstract void ReadByteArray(I2cDevice i2CDevice, Register reg, Span<byte> readBytes);
    }
}
