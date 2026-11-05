using MeterVision.Api.Services;
using MeterVision.Api.Services.Abstractions;
using MeterVision.Api.Database;
using Microsoft.EntityFrameworkCore;
using MeterVision.Api.Middleware;
using MeterVision.Api.Application.Behaviors;
using FluentValidation;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
//Регистрация генератора OpenAPI
builder.Services.AddOpenApi();

// Добавляем Mediator
builder.Services.AddMediator(options =>
{
    // Регистрируем ваш обобщённый Behavior
    options.PipelineBehaviors = [typeof(ValidationBehavior<,>)];
});

// Добавляем валидаторы из текущей сборки
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

builder.Services.AddDbContext<AppDatabase>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("MeterVisionDb"))
);

builder.Services.AddScoped<IAuthService, AuthServiceStub>();
builder.Services.AddSingleton<ICvService, CvServiceStub>();

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
