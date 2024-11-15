﻿namespace EasyJob_ProDG.Model.Cargo
{
    public interface IReefer
    {
        //R:SETP|VENTSETTING(String)|COMMODITY|LOADINGTEMP|SPECIAL|REMARK|
        abstract decimal SetTemperature { get; set; }
        abstract string Commodity { get; set; }
        abstract string VentSetting { get; set; }
        abstract decimal LoadTemperature { get; set; }
        abstract string ReeferSpecial { get; set; }
        abstract string ReeferRemark { get; set; }

        void ResetReefer();

    }
}
