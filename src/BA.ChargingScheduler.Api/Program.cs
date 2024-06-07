using BA.ChargingScheduler.Core.Behaviors;
using BA.ChargingScheduler.Logic;
using BA.ChargingScheduler.Logic.Abstract;
using BA.ChargingScheduler.Service.FluentValidation;
using BA.ChargingScheduler.Service.MediatR;
using FluentValidation;
using MediatR;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
IConfiguration configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.Development.json", optional: true)
    .Build();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(IMediatR).Assembly));
builder.Services.AddValidatorsFromAssemblyContaining<IBaseValidator>();

builder.Services
                .AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

builder.Services.AddScoped<IChargingSchedulerLogic, ChargingSchedulerLogic>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
