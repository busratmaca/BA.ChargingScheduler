using Shared.Kernel.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BA.ChargingScheduler.Logic.Dtos
{
    public class ChargingScheduleDto
    {
        public string StartingTime { get; set; }
        public UsersettingsRequestDto UserSettings { get; set; }
        public CardataRequestDto CarData { get; set; }
    }

    public class UsersettingsRequestDto
    {
        public int DesiredStateOfCharge { get; set; }
        public string LeavingTime { get; set; }
        public int DirectChargingPercentage { get; set; }
        public List<TariffRequestDto> Tariffs { get; set; }
    }

    public class TariffRequestDto
    {
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public float EnergyPrice { get; set; }
    }

    public class CardataRequestDto
    {
        public int ChargePower { get; set; }
        public int BatteryCapacity { get; set; }
        public int CurrentBatteryLevel { get; set; }
    }
}
