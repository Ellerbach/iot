// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Device.I2c;
using System.Threading;
using Iot.Device.Imu;

namespace DemoMpu9250
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello MPU9250!");

            var mpui2CConnectionSettingmpus = new I2cConnectionSettings(1, Mpu9250.DefaultI2cAddress);
            Mpu9250 mpu9250 = new Mpu9250(I2cDevice.Create(mpui2CConnectionSettingmpus));
            var resSelfTest = mpu9250.RunGyroscopeAccelerometerSelfTest();
            Console.WriteLine($"Self test:");
            Console.WriteLine($"Gyro X = {resSelfTest.Item1.X} vs >0.005");
            Console.WriteLine($"Gyro Y = {resSelfTest.Item1.Y} vs >0.005");
            Console.WriteLine($"Gyro Z = {resSelfTest.Item1.Z} vs >0.005");
            Console.WriteLine($"Acc X = {resSelfTest.Item2.X} vs >0.005 & <0.015");
            Console.WriteLine($"Acc Y = {resSelfTest.Item2.Y} vs >0.005 & <0.015");
            Console.WriteLine($"Acc Z = {resSelfTest.Item2.Z} vs >0.005 & <0.015");
            Console.WriteLine("Running Gyroscope and Accelerometer calibration");
            mpu9250.CalibrateGyroscopeAccelerometer();
            Console.WriteLine("Calibration results:");
            Console.WriteLine($"Gyro X bias = {mpu9250.GyroscopeBias.X}");
            Console.WriteLine($"Gyro Y bias = {mpu9250.GyroscopeBias.Y}");
            Console.WriteLine($"Gyro Z bias = {mpu9250.GyroscopeBias.Z}");
            Console.WriteLine($"Acc X bias = {mpu9250.AccelerometerBias.X}");
            Console.WriteLine($"Acc Y bias = {mpu9250.AccelerometerBias.Y}");
            Console.WriteLine($"Acc Z bias = {mpu9250.AccelerometerBias.Z}");
            Console.WriteLine($"Check version magnetometer: {mpu9250.GetMagnetometerVersion()}");
            Console.WriteLine("Magnetometer calibration is taking couple of seconds, please be patient and don't touch the sensor! Please make sure you are not close to any magnetic field like magnet or phone.");
            var mag = mpu9250.CalibrateMagnetometer();
            Console.WriteLine($"Bias:");
            Console.WriteLine($"Mag X = {mpu9250.MagnometerBias.X}");
            Console.WriteLine($"Mag Y = {mpu9250.MagnometerBias.Y}");
            Console.WriteLine($"Mag Z = {mpu9250.MagnometerBias.Z}");
            Console.WriteLine("Press a key to continue");
            var readKey = Console.ReadKey();
            mpu9250.GyroscopeBandwidth = GyroscopeBandwidth.Bandwidth0250Hz;
            mpu9250.AccelerometerBandwidth = AccelerometerBandwidth.Bandwidth0460Hz;
            Console.Clear();

            while (!Console.KeyAvailable)
            {
                Console.CursorTop = 0;
                var gyro = mpu9250.GetGyroscopeReading();
                Console.WriteLine($"Gyro X = {gyro.X, 15}");
                Console.WriteLine($"Gyro Y = {gyro.Y, 15}");
                Console.WriteLine($"Gyro Z = {gyro.Z, 15}");
                var acc = mpu9250.GetAccelerometer();
                Console.WriteLine($"Acc X = {acc.X, 15}");
                Console.WriteLine($"Acc Y = {acc.Y, 15}");
                Console.WriteLine($"Acc Z = {acc.Z, 15}");
                Console.WriteLine($"Temp = {mpu9250.GetTemperature().Celsius.ToString("0.00")} °C");
                var magne = mpu9250.ReadMagnetometer(true);
                Console.WriteLine($"Mag X = {magne.X, 15}");
                Console.WriteLine($"Mag Y = {magne.Y, 15}");
                Console.WriteLine($"Mag Z = {magne.Z, 15}");
                Thread.Sleep(100);
            }
            
            readKey = Console.ReadKey();
            // SetWakeOnMotion
            mpu9250.SetWakeOnMotion(300, AccelerometerLowPowerFrequency.Frequency0Dot24Hz);
            // You'll need to attach the INT pin to a GPIO and read the level. Once going up, you have 
            // some data and the sensor is awake
            // In order to simulate this without a GPIO pin, you will see that the refresh rate is very low
            // Setup here at 0.24Hz which means, about every 4 seconds
            Console.Clear();

            while (!Console.KeyAvailable)
            {
                Console.CursorTop = 0;
                var acc = mpu9250.GetAccelerometer();
                Console.WriteLine($"Acc X = {acc.X, 15}");
                Console.WriteLine($"Acc Y = {acc.Y, 15}");
                Console.WriteLine($"Acc Z = {acc.Z, 15}");
                Thread.Sleep(100);
            }
        }
    }
}
