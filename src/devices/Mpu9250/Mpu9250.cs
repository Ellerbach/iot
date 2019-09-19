﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Iot.Device.Magnometer;
using Iot.Units;
using System;
using System.Buffers.Binary;
using System.Device;
using System.Device.I2c;
using System.IO;
using System.Net.Http.Headers;
using System.Numerics;
using System.Threading;

namespace Iot.Device.Imu
{
    /// <summary>
    /// MPU9250 class. MPU9250 has an embedded gyroscope, accelerometer and temperature. It is built on an MPU6500 and it does offers a magnetometer thru an embedded AK8963.
    /// </summary>
    public class Mpu9250 : Mpu6500
    {
        private Ak8963 _ak8963;
        private bool _autoDispose;

        #region Magnetometer

        /// <summary>
        /// Get the magnetometer bias
        /// </summary>
        public Vector3 MagnometerBias => new Vector3(_ak8963.MagnometerBias.Y, _ak8963.MagnometerBias.X, -_ak8963.MagnometerBias.Z);

        /// <summary>
        /// Calibrate the magnetometer. Make sure your sensor is as far as possible of magnet
        /// Calculate as well the magnetometer bias
        /// </summary>
        /// <returns>Returns the factory calibration data</returns>
        public Vector3 CalibrateMagnetometer() => _wakeOnMotion ? Vector3.Zero : _ak8963.CalibrateMagnetometer();

        /// <summary>
        /// True if there is a data to read
        /// </summary>
        public bool HasDataToRead => !(_wakeOnMotion && _ak8963.HasDataToRead);

        /// <summary>
        /// Check if the magnetometer version is the correct one (0x48)
        /// </summary>
        /// <returns>Returns the Magnetometer version number</returns>
        /// <remarks>When the wake on motion is on, you can't read the magnetometer, so this function returns 0</remarks>
        public byte GetMagnetometerVersion() => _wakeOnMotion ? (byte)0 : _ak8963.GetDeviceInfo();

        /// <summary>
        /// Read the magnetometer and can wait for new data to be present
        /// </summary>
        /// <param name="waitForData">true to wait for new data</param>
        /// <returns>The data from the magnetometer</returns>
        public Vector3 ReadMagnetometer(bool waitForData = false)
        {
            TimeSpan timeout = TimeSpan.Zero;
            switch (_ak8963.MeasurementMode)
            {
                // TODO: find what is the value in the documentation, it should be pretty fast
                // But taking the same value as for the slowest one so th 8Hz one
                case MeasurementMode.SingleMeasurement:
                case MeasurementMode.ExternalTriggedMeasurement:
                case MeasurementMode.SelfTest:
                case MeasurementMode.ContinuousMeasurement8Hz:
                    // 8Hz measurement period plus 1 millisecond
                    timeout = TimeSpan.FromMilliseconds(126);
                    break;
                case MeasurementMode.ContinuousMeasurement100Hz:
                    // 100Hz measurement period plus 1 millisecond
                    timeout = TimeSpan.FromMilliseconds(11);
                    break;
                // Those cases are not measurement and should be 0 then                
                case MeasurementMode.FuseRomAccess:
                case MeasurementMode.PowerDown:
                default:
                    break;
            }
            return _wakeOnMotion ? Vector3.Zero : _ak8963.ReadMagnetometer(waitForData, timeout);
        }

        /// <summary>
        /// Select the magnetometer measurement mode
        /// </summary>
        public MeasurementMode MagnetometerMeasurementMode
        {
            get { return _ak8963.MeasurementMode; }
            set { _ak8963.MeasurementMode = value; }
        }

        /// <summary>
        /// Select the magnetometer output bit rate
        /// </summary>
        public OutputBitMode MagnetometerOutputBitMode
        {
            get { return _ak8963.OutputBitMode; }
            set { _ak8963.OutputBitMode = value; }
        }

        #endregion

        /// <summary>
        /// Initialize the MPU9250
        /// </summary>
        /// <param name="i2cDevice">The I2C device</param>
        /// <param name="autoDispose">Will automatically dispose the I2C device if true</param>
        public Mpu9250(I2cDevice i2cDevice, bool autoDispose = true) : base(i2cDevice)
        {
            // Setup I2C for the AK8963
            WriteRegister(Register.USER_CTRL, (byte)UserControls.I2C_MST_EN);
            // Speed of 400 kHz
            WriteRegister(Register.I2C_MST_CTRL, (byte)I2cBussFrequency.Frequency400kHz);
            _autoDispose = autoDispose;
            _ak8963 = new Ak8963(i2cDevice, new Ak8963Attached(), false);
            if (!_ak8963.CheckVersion())
            {
                // Try to reset the device first
                _ak8963.Reset();
                // Wait a bit
                if (!_ak8963.CheckVersion())
                {
                    // Try to reset the I2C Bus
                    WriteRegister(Register.USER_CTRL, (byte)UserControls.I2C_MST_RST);
                    Thread.Sleep(100);
                    // Resetup again
                    WriteRegister(Register.USER_CTRL, (byte)UserControls.I2C_MST_EN);
                    WriteRegister(Register.I2C_MST_CTRL, (byte)I2cBussFrequency.Frequency400kHz);
                    // Poorly documented time to wait after the I2C bus reset
                    // Found out that waiting a little bit is needed. Exact time may be lower
                    Thread.Sleep(100);
                    // Try one more time
                    if (!_ak8963.CheckVersion())
                        throw new IOException($"This device does not contain the correct signature 0x48 for a AK8963 embedded into the MPU9250");
                }
            }
        }

        /// <summary>
        /// Return true if the version of MPU9250 is the correct one
        /// </summary>
        /// <returns>True if success</returns>
        internal new bool CheckVersion()
        {
            // Check if the version is thee correct one
            return ReadByte(Register.WHO_AM_I) == 0x71;
        }

        /// <summary>
        /// Setup the Wake On Motion. This mode generate a rising signal on pin INT
        /// You can catch it with a normal GPIO and place an interruption on it if supported
        /// Reading the sensor won't give any value until it wakes up periodically
        /// Only Accelerator data is available in this mode
        /// </summary>
        /// <param name="accelerometerThreshold">Threshold of magnetometer x/y/z axes. LSB = 4mg. Range is 0mg to 1020mg</param>
        /// <param name="acceleratorLowPower">Frequency used to measure data for the low power consumption mode</param>
        public new void SetWakeOnMotion(uint accelerometerThreshold, AccelerometerLowPowerFrequency acceleratorLowPower)
        {
            // We can't use the magnetometer, only Accelerometer will be measured
            _ak8963.MeasurementMode = MeasurementMode.PowerDown;
            base.SetWakeOnMotion(accelerometerThreshold, acceleratorLowPower);
        }

        /// <summary>
        /// Cleanup everything
        /// </summary>
        public new void Dispose()
        {
            if (_autoDispose)
            {
                _i2cDevice?.Dispose();
                _i2cDevice = null;
            }
        }

    }
}
