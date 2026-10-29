using MeterVision.Api.Services;
using MeterVision.Api.Services.Abstractions;
using MeterVision.Api.Database;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();

builder.Services.AddDbContext<AppDatabase>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("MeterVisionDb")));

builder.Services.AddScoped<IAuthService, AuthServiceStub>();
builder.Services.AddScoped<IReadingService, ReadingServiceStub>();
builder.Services.AddScoped<ICvService, CvServiceStub>();
builder.Services.AddScoped<IMeterService, MeterService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}


app.UseHttpsRedirection();
//app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();


app.Run();
