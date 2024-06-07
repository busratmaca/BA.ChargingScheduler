using AutoMapper;
using BA.ChargingScheduler.Contract.Requests;
using BA.ChargingScheduler.Logic.Abstract;
using MediatR;
using Shared.Kernel.Abstracts;
using Shared.Kernel.AutoMapper;
using System.Text.Json;

namespace BA.ChargingScheduler.Service.Commands
{
    public class ChargingScheduleCommand : IBaseCommand<ChargingScheduleRequest, ChargingScheduleCommand>, IRequest<string>
    {
        #region Properties
        public string StartingTime { get; set; }
        public UsersettingsCommand UserSettings { get; set; }
        public CardataCommand CarData { get; set; }
        #endregion
         
        #region Constructor
        public ChargingScheduleCommand(string startingTime, UsersettingsRequest usersettings, CardataRequest cardata) // command nesnesini constructorda parametreden al 
        {
            StartingTime = startingTime;
            if (usersettings != null)
                UserSettings = UsersettingsCommand.FromRequest(usersettings);

            if (cardata != null)
                CarData = CardataCommand.FromRequest(cardata);
        }
        #endregion


        #region Methods
        public static ChargingScheduleCommand FromRequest(ChargingScheduleRequest request) => WithProfile<ChargingScheduleCommandProfile>.Map<ChargingScheduleCommand>(request);

        public class ChargingScheduleCommandHandler : IRequestHandler<ChargingScheduleCommand, string>
        {
            private readonly IChargingSchedulerLogic chargingSchedulerLogic;
            public ChargingScheduleCommandHandler(IChargingSchedulerLogic _chargingSchedulerLogic) => chargingSchedulerLogic = _chargingSchedulerLogic;
            public async Task<string> Handle(ChargingScheduleCommand request, CancellationToken cancellationToken)
            {
                var result = chargingSchedulerLogic.GenerateSchedule(
                    new Logic.Dtos.UsersettingsRequestDto
                    {
                        DesiredStateOfCharge = request.UserSettings.DesiredStateOfCharge,
                        DirectChargingPercentage = request.UserSettings.DirectChargingPercentage,
                        LeavingTime = request.UserSettings.LeavingTime,
                        Tariffs = request.UserSettings.Tariffs.Select( s => new Logic.Dtos.TariffRequestDto { EndTime = s.EndTime, EnergyPrice = s.EnergyPrice, StartTime = s.StartTime }).ToList()
                    },
                    new Logic.Dtos.CardataRequestDto
                    {
                        BatteryCapacity = request.CarData.BatteryCapacity,
                        ChargePower = request.CarData.ChargePower,
                        CurrentBatteryLevel = request.CarData.CurrentBatteryLevel
                    },
                    request.StartingTime                    
                    );
                return JsonSerializer.Serialize(result);
            }
        }

        #endregion
    }


    public class UsersettingsCommand
    {
        public int DesiredStateOfCharge { get; set; }
        public string LeavingTime { get; set; }
        public int DirectChargingPercentage { get; set; }
        public List<TariffCommand> Tariffs { get; set; }

        public UsersettingsCommand(int desiredStateOfCharge, string leavingTime, int directChargingPercentage, List<TariffRequest> tariffRequests)
        {
            DesiredStateOfCharge = desiredStateOfCharge;
            LeavingTime = leavingTime;
            DirectChargingPercentage = directChargingPercentage;
            Tariffs = new List<TariffCommand>();

            if (tariffRequests != null && tariffRequests.Count > 0)
                foreach (var item in tariffRequests)
                    Tariffs.Add(TariffCommand.FromRequest(item));

        }

        public static UsersettingsCommand FromRequest(UsersettingsRequest request) => WithProfile<UserSettingsCommandProfile>.Map<UsersettingsCommand>(request);
    }

    public class TariffCommand
    {
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public float EnergyPrice { get; set; }

        public TariffCommand(string startTime, string endTime, float energyPrice)
        {
            StartTime = startTime;
            EndTime = endTime;
            EnergyPrice = energyPrice;
        }

        public static TariffCommand FromRequest(TariffRequest request) => WithProfile<TariffCommandProfile>.Map<TariffCommand>(request);
    }

    public class CardataCommand
    {
        public int ChargePower { get; set; }
        public int BatteryCapacity { get; set; }
        public int CurrentBatteryLevel { get; set; }

        public CardataCommand(int chargePower, int batteryCapacity, int currentBatteryLevel)
        {
            ChargePower = chargePower;
            BatteryCapacity = batteryCapacity;
            CurrentBatteryLevel = currentBatteryLevel;
        }

        public static CardataCommand FromRequest(CardataRequest request) => WithProfile<CarDataCommandProfile>.Map<CardataCommand>(request);
    }


    #region AutoMapper Profile
    public class ChargingScheduleCommandProfile : Profile
    {
        public ChargingScheduleCommandProfile()
        {
            CreateMap<ChargingScheduleRequest, ChargingScheduleCommand>()
                 .ConstructUsing(x => new ChargingScheduleCommand
                  (
                     x.StartingTime,
                     x.UserSettings,
                     x.CarData
                 ))
                 .ForMember(x => x.UserSettings, opts => opts.Ignore())
                 .ForMember(x => x.CarData, opts => opts.Ignore());
        }
    }

    public class UserSettingsCommandProfile : Profile
    {
        public UserSettingsCommandProfile()
        {
            CreateMap<UsersettingsRequest, UsersettingsCommand>()
                .ConstructUsing(x => new UsersettingsCommand(
                        x.DesiredStateOfCharge,
                        x.LeavingTime,
                        x.DirectChargingPercentage,
                        x.Tariffs
                    )
                )
                .ForMember(x => x.Tariffs, opts => opts.Ignore());
        }
    }

    public class TariffCommandProfile : Profile
    {
        public TariffCommandProfile()
        {
            CreateMap<TariffRequest, TariffCommand>()
                .ConstructUsing(x => new TariffCommand(
                        x.StartTime,
                        x.EndTime,
                        x.EnergyPrice
                    )
                );
        }
    }

    public class CarDataCommandProfile : Profile
    {
        public CarDataCommandProfile()
        {
            CreateMap<CardataRequest, CardataCommand>()
                .ConstructUsing(x => new CardataCommand(
                        x.ChargePower,
                        x.BatteryCapacity,
                        x.CurrentBatteryLevel
                    )
                );
        }
    }
    #endregion
}
