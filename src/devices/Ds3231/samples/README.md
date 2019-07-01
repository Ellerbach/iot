# DS3231 - Samples

## Hardware Required
* DS3231
* Male/Female Jumper Wires

## Circuit
![](DS3231_circuit_bb.png)

* SCL - SCL
* SDA - SDA
* VCC - 5V
* GND - GND

## Code
```C#
I2cConnectionSettings settings = new I2cConnectionSettings(1, Ds3231.DefaultI2cAddress);
I2cDevice device = I2cDevice.Create(settings);

using (Ds3231 rtc = new Ds3231(device))
{
    // set DS3231 time
    rtc.DateTime = DateTime.Now;

    // loop
    while (true)
    {
        // read temperature
        double temp = rtc.Temperature.Celsius;
        // read time
        DateTime dt = rtc.DateTime;

        Console.WriteLine($"Time: {dt.ToString("yyyy/MM/dd HH:mm:ss")}");
        Console.WriteLine($"Temperature: {temp} ℃");
        Console.WriteLine();

        // wait for a second
        Thread.Sleep(1000);
    }
}

```

## Result
![](RunningResult.jpg)
