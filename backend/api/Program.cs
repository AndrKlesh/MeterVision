using Microsoft.EntityFrameworkCore;
using MeterVision.Api.Middleware;
using MeterVision.Api.Application.Behaviors;
using FluentValidation;
using MeterVision.Api.Application.Abstractions.Services;
using MeterVision.Api.Infrastructure.Services;
using MeterVision.Api.Infrastructure.Database;
using MeterVision.Api.Infrastructure.Repositories;
using MeterVision.Api.Application.Abstractions.Repositories;
using MeterVision.Api.Infrastructure.Persistence.Repositories;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
//Регистрация генератора OpenAPI
builder.Services.AddOpenApi();

// Добавляем валидаторы из текущей сборки
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

// Добавляем Mediator
builder.Services.AddMediator(options =>
{
    // Меняем lifetime с Singleton (по умолчанию) на Scoped
    options.ServiceLifetime = ServiceLifetime.Scoped;
    // Регистрируем ваш обобщённый Behavior
    options.PipelineBehaviors = [typeof(ValidationBehavior<,>)];
});


// builder.Services.AddDbContext<AppDatabase>(options =>
//     options.UseNpgsql(builder.Configuration.GetConnectionString("MeterVisionDb"))
// );

builder.Services.AddDbContext<AppDatabase>(options =>
    options.UseInMemoryDatabase("MeterVisionDb")
);


builder.Services.AddScoped<IImageService, ImageService>();

builder.Services.AddScoped<IMeteringRepository, MeteringRepository>();
builder.Services.AddScoped<IMeteringService, MeteringService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ICvService, CvServiceStub>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    // Генерация JSON-файла спецификации
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
app.UseAuthorization();
app.MapControllers();

app.Run();
