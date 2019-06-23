namespace Ak8963
{
    public enum MeasurementMode
    {         
        PowerDown = 0b0000,
        SingleMeasurement = 0b0001,
        //  0010 for 8 Hz
        ContinousMeasurement8Hz = 0b010,
        // 0110 for 100 Hz sample rates
        ContinousMeasurement100Hz = 0b0110,
        ExternalTriggedMeasurement = 0b0100,
        SelfTest = 0b1000,
        FuseRomAccess = 0b1111,

    }
}