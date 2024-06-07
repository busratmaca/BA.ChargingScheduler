using BA.ChargingScheduler.Logic.Dtos;
using BA.ChargingScheduler.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ChargingScheduler.Tests
{
    public class ChargingSchedulerLogicTests
    {
        [Fact]
        public void generates_schedule_within_same_day()
        {
            // Arrange
            var logic = new ChargingSchedulerLogic();
            var userSettings = new UsersettingsRequestDto
            {
                LeavingTime = "18:00",
                DesiredStateOfCharge = 80,
                DirectChargingPercentage = 20,
                Tariffs = new List<TariffRequestDto>
            {
                new TariffRequestDto { StartTime = "08:00", EndTime = "12:00" },
                new TariffRequestDto { StartTime = "14:00", EndTime = "18:00" }
            }
            };
            var carData = new CardataRequestDto
            {
                BatteryCapacity = 100,
                CurrentBatteryLevel = 20,
                ChargePower = 10
            };
            string startingTime = "2023-10-01T08:00:00Z";

            // Act
            var result = logic.GenerateSchedule(userSettings, carData, startingTime);

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
        }
    }
}
