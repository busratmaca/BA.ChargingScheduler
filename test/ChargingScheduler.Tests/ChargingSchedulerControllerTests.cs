using BA.ChargingScheduler.Api.Controllers;
using BA.ChargingScheduler.Contract.Responses;
using BA.ChargingScheduler.Logic.Dtos;
using BA.ChargingScheduler.Logic;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Moq;
using System.Text.Json;
using Xunit;


namespace ChargingScheduler.Tests
{
    public class ChargingSchedulerControllerTests
    {
        [Fact]
        public async Task valid_request_returns_200_ok_status()
        {
            // Arrange
            var mediatorMock = new Mock<IMediator>();
            var controller = new ChargingSchedulerController(mediatorMock.Object);
            var request = new BA.ChargingScheduler.Contract.Requests.ChargingScheduleRequest { /* initialize with valid data */ };
            var response = JsonSerializer.Serialize(new List<ChargingScheduleResponse> { /* initialize with valid data */ });

            mediatorMock.Setup(m => m.Send(It.IsAny<IRequest<string>>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(response);

            // Act
            var result = await controller.ChargingSchedule(request);

            // Assert
            Assert.IsAssignableFrom<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.Equal(200, okResult.StatusCode);
        }


    }
}