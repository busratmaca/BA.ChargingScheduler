using BA.ChargingScheduler.Contract.Responses;
using BA.ChargingScheduler.Logic.Dtos;

namespace BA.ChargingScheduler.Logic.Abstract
{
    public interface IChargingSchedulerLogic
    {
        List<ChargingScheduleResponse> GenerateSchedule(UsersettingsRequestDto userSettings, CardataRequestDto carData, string startingTime);
    }
}
