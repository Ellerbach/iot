using Ak8963;
using System;
using System.Collections.Generic;
using System.Device;
using System.Device.I2c;
using System.Text;
using System.Threading;

namespace Mpu9250
{
    internal class Ak8963Attached : Ak8963Interface
    {

        public override void WriteRegister(I2cDevice i2cDevice, Ak8963.Register reg, byte data)
        {
            Span<byte> dataout = stackalloc byte[2] { (byte)Register.I2C_SLV0_ADDR, Ak8963.Ak8963.DefaultI2cAddress };
            i2cDevice.Write(dataout);
            dataout[0] = (byte)Register.I2C_SLV0_REG;
            dataout[1] = (byte)reg;
            i2cDevice.Write(dataout);
            dataout[0] = (byte)Register.I2C_SLV0_DO;
            dataout[1] = data;
            i2cDevice.Write(dataout);
            dataout[0] = (byte)Register.I2C_SLV0_CTRL;
            dataout[1] = 0x81;
            i2cDevice.Write(dataout);
        }

        public override byte ReadByte(I2cDevice i2cDevice, Ak8963.Register reg)
        {
            Span<byte> read = stackalloc byte[1] { 0 };
            ReadByteArray(i2cDevice, reg, read);
            return read[0];
        }

        public override void ReadByteArray(I2cDevice i2cDevice, Ak8963.Register reg, Span<byte> readBytes)
        {
            Span<byte> dataout = stackalloc byte[2] { (byte)Register.I2C_SLV0_ADDR, Ak8963.Ak8963.DefaultI2cAddress | 0x80 };
            i2cDevice.Write(dataout);
            dataout[0] = (byte)Register.I2C_SLV0_REG;
            dataout[1] = (byte)reg;
            i2cDevice.Write(dataout);
            dataout[0] = (byte)Register.I2C_SLV0_CTRL;
            dataout[1] = (byte)(0x80 | readBytes.Length);
            i2cDevice.Write(dataout);
            // Just need to wait a very little bit
            // For data transfer to happen and process on the MPU9250 side
            DelayHelper.DelayMicroseconds(200, false);
            //Thread.Sleep(1);
            i2cDevice.WriteByte((byte)Register.EXT_SENS_DATA_00);
            i2cDevice.Read(readBytes);
        }

    }
}
