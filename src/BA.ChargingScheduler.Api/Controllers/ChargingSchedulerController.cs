using BA.ChargingScheduler.Contract.Contracts;
using BA.ChargingScheduler.Contract.Requests;
using BA.ChargingScheduler.Contract.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace BA.ChargingScheduler.Api.Controllers
{
    [ApiController]
    public class ChargingSchedulerController : ControllerBase
    {
        private IMediator mediator;
        public ChargingSchedulerController(IMediator _mediator) => mediator = _mediator;


        [HttpPost]
        [Produces("application/json")]
        [Route("charging-schedule")]
        [ProducesResponseType(typeof(Envelope<List<ChargingScheduleResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Envelope<List<ChargingScheduleResponse>>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ChargingSchedule([FromBody] ChargingScheduleRequest request, CancellationToken cancellationToken = new CancellationToken())
        {
            var command = Service.Commands.ChargingScheduleCommand.FromRequest(request);
            var result = await mediator.Send(command, cancellationToken); // Json string response dön, nesneye parse et
            return Ok(new Envelope<List<ChargingScheduleResponse>>(JsonSerializer.Deserialize<List<ChargingScheduleResponse>>(result)));

        }
    }
}
