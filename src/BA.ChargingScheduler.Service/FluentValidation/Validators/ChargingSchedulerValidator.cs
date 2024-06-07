using BA.ChargingScheduler.Service.Commands;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BA.ChargingScheduler.Service.FluentValidation.Validators
{
    public class ChargingSchedulerValidator : AbstractValidator<ChargingScheduleCommand>
    {
        public ChargingSchedulerValidator()
        {

            RuleFor(r => r.StartingTime).NotEmpty().NotNull().WithMessage("StartingTime cannot be null or empty");
        }
    }
}

