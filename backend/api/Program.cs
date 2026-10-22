using MeterVision.Api.Services;
using MeterVision.Api.Services.Abstractions;
using MeterVision.Api.Database;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();

builder.Services.AddDbContext<AppDatabase>(options =>
    options.UseInMemoryDatabase("MeterVisionDb"));

//builder.Services.AddScoped<IAuthService, AuthServiceStub>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ICvService, CvServiceStub>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    /// <summary>
    /// Добавляет тестового пользователя.
    /// </summary>
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<AppDatabase>();
    DbSeeder.Seed(db);
}


app.UseHttpsRedirection();
//app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();


app.Run();
