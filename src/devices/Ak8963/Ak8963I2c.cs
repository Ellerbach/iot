using System;
using System.Collections.Generic;
using System.Device.I2c;
using System.Text;

namespace Ak8963
{
    public class Ak8963I2c : Ak8963Interface
    {
        public override byte ReadByte(I2cDevice i2cDevice, Register reg)
        {
            i2cDevice.WriteByte((byte)reg);
            return i2cDevice.ReadByte();
        }

        public override void ReadByteArray(I2cDevice i2cDevice, Register reg, Span<byte> readBytes)
        {
            i2cDevice.WriteByte((byte)reg);
            i2cDevice.Read(readBytes);
        }

        public override void WriteRegister(I2cDevice i2cDevice, Register reg, byte data)
        {
            Span<byte> dataout = stackalloc byte[] { (byte)reg, data };
            i2cDevice.Write(dataout);
        }
    }
}
