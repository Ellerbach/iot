﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Iot.Device.GrovePiDevice.Models;

namespace Iot.Device.GrovePiDevice.Sensors
{
    /// <summary>
    /// Type of DHT sensors
    /// </summary>
    public enum DhtType
    {
        Dht11 = 0,
        Dht22 = 1,
        Dht21 = 2,
        Am2301 = 3
    }

    /// <summary>
    /// DhtSensor supports DHT familly sensors
    /// </summary>
    public class DhtSensor : ISensor<double[]>
    {
        private GrovePi _grovePi;
        private readonly double[] _lastTemHum = new double[2];

        /// <summary>
        /// 
        /// </summary>
        /// <param name="grovePi">The GrovePi class</param>
        /// <param name="port">The grove Port, need to be in the list of SupportedPorts</param>
        /// <param name="dhtType">The DHT type</param>
        public DhtSensor(GrovePi grovePi, GrovePort port, DhtType dhtType)
        {
            if (!SupportedPorts.Contains(port))
                throw new ArgumentException($"Error: grove Port not supported");
            _grovePi = grovePi;
            DhtType = dhtType;
            Port = port;
            // Ask for the temperature so we will have one in cache
            _grovePi.WriteCommand(GrovePiCommands.DhtTemp, Port, (byte)DhtType, 0);
        }

        /// <summary>
        /// Get the type of DHT sensor
        /// </summary>
        public DhtType DhtType { get; internal set; }

        /// <summary>
        /// Get an array of 2 double
        /// First is the temperature in degree Celsius
        /// Second is the relative humidity from 0.0 to 100.0
        /// </summary>
        public double[] Value
        {
            get
            {
                ReadSensor();
                return _lastTemHum;
            }
        }

        /// <summary>
        /// You need to read the sensorbefore getting the value
        /// </summary>
        public void ReadSensor()
        {
            _grovePi.WriteCommand(GrovePiCommands.DhtTemp, Port, (byte)DhtType, 0);
            // Wait a little bit to read the result
            Thread.Sleep(50);
            var retArray = _grovePi.ReadCommand(GrovePiCommands.DhtTemp, Port);
            _lastTemHum[0] = BitConverter.ToSingle(retArray.AsSpan(1, 4));
            _lastTemHum[1] = BitConverter.ToSingle(retArray.AsSpan(5, 4));
        }

        /// <summary>
        /// Returns the temperature and humidity in a formated way
        /// </summary>
        /// <returns>Returns the temperature and humidity in a formated way</returns>
        public override string ToString() => $"Temperature: {_lastTemHum[0].ToString("0.00")} °C, Humidity: {_lastTemHum[1].ToString("0.00")} %";

        /// <summary>
        /// Get the last temperature measured in Farenheit
        /// </summary>
        public double LastTemperatureInFarenheit => _lastTemHum[0] * 9 / 5 + 32;

        /// <summary>
        /// Get the last temperature measured in Celsius
        /// </summary>
        public double LastTemperature => _lastTemHum[0];

        /// <summary>
        /// Get the last measured relative humidy from 0.0 to 100.0
        /// </summary>
        public double LastRelativeHumidity => _lastTemHum[1];

        /// <summary>
        /// Get the name of the DHT sensor
        /// </summary>
        public string SensorName => $"{DhtType} Temperature and Humidity Sensor";

        /// <summary>
        /// grove sensor port
        /// </summary>
        public GrovePort Port { get; internal set; }

        /// <summary>
        /// Only Digital ports including the analogic sensors (A0 = D14, A1 = D15, A2 = D16)
        /// </summary>
        static public List<GrovePort> SupportedPorts => new List<GrovePort>()
        {
            GrovePort.DigitalPin2,
            GrovePort.DigitalPin3,
            GrovePort.DigitalPin4,
            GrovePort.DigitalPin5,
            GrovePort.DigitalPin6,
            GrovePort.DigitalPin7,
            GrovePort.DigitalPin8,
            GrovePort.DigitalPin14,
            GrovePort.DigitalPin15,
            GrovePort.DigitalPin16
        };
    }
}
