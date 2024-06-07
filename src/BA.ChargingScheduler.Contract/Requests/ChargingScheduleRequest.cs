using Shared.Kernel.Abstracts;

namespace BA.ChargingScheduler.Contract.Requests
{

    public class ChargingScheduleRequest : IBaseRequestModel
    {
        public string StartingTime { get; set; }
        public UsersettingsRequest UserSettings { get; set; }
        public CardataRequest CarData { get; set; }
    }

    public class UsersettingsRequest
    {
        public int DesiredStateOfCharge { get; set; }
        public string LeavingTime { get; set; }
        public int DirectChargingPercentage { get; set; }
        public List<TariffRequest> Tariffs { get; set; }
    }

    public class TariffRequest
    {
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public float EnergyPrice { get; set; }
    }

    public class CardataRequest
    {
        public int ChargePower { get; set; }
        public int BatteryCapacity { get; set; }
        public int CurrentBatteryLevel { get; set; }
    }

}
