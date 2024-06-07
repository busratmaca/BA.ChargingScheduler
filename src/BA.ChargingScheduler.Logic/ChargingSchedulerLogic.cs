using BA.ChargingScheduler.Contract.Responses;
using BA.ChargingScheduler.Logic.Abstract;
using BA.ChargingScheduler.Logic.Dtos;
using System.Globalization;

namespace BA.ChargingScheduler.Logic
{
    public class ChargingSchedulerLogic : IChargingSchedulerLogic
    {
        public List<ChargingScheduleResponse> GenerateSchedule(UsersettingsRequestDto userSettings, CardataRequestDto carData, string startingTime)
        {
            List<ChargingScheduleResponse> schedule = new();


            DateTimeOffset startingDateTime = DateTimeOffset.Parse(startingTime, null, DateTimeStyles.RoundtripKind);
            DateTimeOffset leavingTime = DateTimeOffset.ParseExact(startingDateTime.ToString("yyyy-MM-dd") + "T" + userSettings.LeavingTime + "Z", "yyyy-MM-ddTHH:mmZ", null);

            TimeOnly startingTimeOnly = TimeOnly.FromDateTime(startingDateTime.DateTime);
            TimeOnly leavingTimeOnly = TimeOnly.Parse(userSettings.LeavingTime);



            if (leavingTimeOnly < startingTimeOnly)
                leavingTime = leavingTime.AddDays(1);



            decimal desiredChargeLevel = CalculateDesiredChargeLevel(userSettings.DesiredStateOfCharge, carData.BatteryCapacity);

            decimal directChargeThreshold = userSettings.DirectChargingPercentage / 100m * carData.BatteryCapacity;
            decimal currentChargeLevel = carData.CurrentBatteryLevel;

            decimal requiredCharge = Math.Max(0, desiredChargeLevel - currentChargeLevel);
            decimal chargePower = carData.ChargePower;

            DateTimeOffset currentTime = startingDateTime;

            foreach (var tariff in userSettings.Tariffs.OrderByDescending(x=>x.EnergyPrice))
            {            
                DateTimeOffset tariffStartTime = DateTimeOffset.ParseExact(currentTime.ToString("yyyy-MM-dd") + "T" + tariff.StartTime + "Z", "yyyy-MM-ddTHH:mmZ", null);
                DateTimeOffset tariffEndTime = DateTimeOffset.ParseExact(currentTime.ToString("yyyy-MM-dd") + "T" + tariff.EndTime + "Z", "yyyy-MM-ddTHH:mmZ", null);

                if (tariffEndTime <= tariffStartTime)
                {
                    tariffEndTime = tariffEndTime.AddDays(1);
                }

                if (tariffEndTime > leavingTime)
                {
                    tariffEndTime = leavingTime;
                }

                while (currentTime < tariffEndTime && requiredCharge > 0)
                {
                    decimal chargeDuration = (decimal)(tariffEndTime - currentTime).TotalHours;
                    decimal possibleCharge = chargePower * chargeDuration;

                    if (possibleCharge >= requiredCharge)
                    {
                        schedule.Add(new ChargingScheduleResponse
                        {
                            StartTime = currentTime.ToString("yyyy-MM-ddTHH:mm:ssZ"),
                            EndTime = currentTime.AddHours((double)(requiredCharge / chargePower)).ToString("yyyy-MM-ddTHH:mm:ssZ"),
                            IsCharging = true
                        });

                        currentTime = currentTime.AddHours((double)(requiredCharge / chargePower));
                        requiredCharge = 0;
                    }
                    else
                    {
                        schedule.Add(new ChargingScheduleResponse
                        {
                            StartTime = currentTime.ToString("yyyy-MM-ddTHH:mm:ssZ"),
                            EndTime = tariffEndTime.ToString("yyyy-MM-ddTHH:mm:ssZ"),
                            IsCharging = true
                        });

                        requiredCharge -= possibleCharge;
                        currentTime = tariffEndTime;
                    }
                }

                if (currentTime < tariffEndTime)
                {
                    schedule.Add(new ChargingScheduleResponse
                    {
                        StartTime = currentTime.ToString("yyyy-MM-ddTHH:mm:ssZ"),
                        EndTime = tariffEndTime.ToString("yyyy-MM-ddTHH:mm:ssZ"),
                        IsCharging = false
                    });

                    currentTime = tariffEndTime;
                }

                if (currentTime >= leavingTime)
                {
                    break;
                }
            }

            return schedule;
        }

        private static decimal CalculateDesiredChargeLevel(int desiredStateOfCharge, decimal batteryCapacity)
        {
            return desiredStateOfCharge / 100m * batteryCapacity;
        }
    }
}


