using MeterVision.Api.Services;
using MeterVision.Api.Services.Abstractions;

var builder = WebApplication.CreateBuilder(args);

// Регистрируем заглушку сервиса распознавания
builder.Services.AddScoped<ICvService, CvServiceStub>();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
