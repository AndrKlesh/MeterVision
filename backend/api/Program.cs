using MeterVision.Api.Services;
using MeterVision.Api.Services.Abstractions;
using MeterVision.Api.Database;
using Microsoft.EntityFrameworkCore;
using MeterVision.Api.Middleware;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
//Регистрация генератора OpenAPI
builder.Services.AddOpenApi();

builder.Services.AddDbContext<AppDatabase>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("MeterVisionDb")));

builder.Services.AddScoped<IAuthService, AuthServiceStub>();
builder.Services.AddScoped<ICvService, CvServiceStub>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    // Генерация JSON-файла спецификации (обычно /openapi/v1.json)
    app.MapOpenApi();

    // Подключение Swagger UI для визуализации
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "v1");
        app.MapGet("/", () => Results.Redirect("/swagger"));
    });
}

app.UseMiddleware<BasicAuthMiddleware>();
app.UseHttpsRedirection();
//app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();


app.Run();
