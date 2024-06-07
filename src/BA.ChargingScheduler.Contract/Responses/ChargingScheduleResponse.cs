namespace BA.ChargingScheduler.Contract.Responses
{
    public class ChargingScheduleResponse
    {
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public bool IsCharging { get; set; }
    }
}
