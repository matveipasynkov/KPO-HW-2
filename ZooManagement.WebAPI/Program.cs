using ZooManagement.Application.Interfaces;
using ZooManagement.Application.Services;
using ZooManagement.Domain.Interfaces;
using ZooManagement.Infrastructure.EventDispatching;
using ZooManagement.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddSingleton<IAnimalRepository, InMemoryAnimalRepository>();
builder.Services.AddSingleton<IEnclosureRepository, InMemoryEnclosureRepository>();
builder.Services.AddSingleton<IFeedingScheduleRepository, InMemoryFeedingScheduleRepository>();

builder.Services.AddSingleton<IEventDispatcher, ConsoleEventDispatcher>();

builder.Services.AddScoped<IAnimalService, AnimalService>();
builder.Services.AddScoped<IEnclosureService, EnclosureService>();
builder.Services.AddScoped<IAnimalTransferService, AnimalTransferService>();
builder.Services.AddScoped<IFeedingOrganizationService, FeedingOrganizationService>();
builder.Services.AddScoped<IZooStatisticsService, ZooStatisticsService>();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Version = "v1",
        Title = "Zoo Management API",
        Description = "API для управления Московским Зоопарком"
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Zoo Management API V1");
    });
}

app.MapControllers();

app.Run();
