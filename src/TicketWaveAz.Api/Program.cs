using TicketWaveAz.Api.Configurations;
using TicketWaveAz.Api.Consumers;
using TicketWaveAz.Api.Middlewares;
using TicketWaveAz.Application;
using TicketWaveAz.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders();

builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
    });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services
    .AddLogConfiguration(builder.Configuration)
    .AddInfrastructure(builder.Configuration)
    .AddApplication();

builder.Services.AddHostedService<PaymentConfirmedConsumer>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder
                .WithOrigins("http://localhost:4200")
                .AllowAnyMethod()
                .SetIsOriginAllowed(_ => true)
                .AllowAnyHeader()
                .AllowCredentials();
        });
});


var app = builder.Build();

app.UseExceptionHandler();

app.UseCors("AllowAll");

if (app.Environment.IsDevelopment() || app.Environment.EnvironmentName == "Local")
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.UseHttpLogging();

app.UseMiddleware<LoggingMiddleware>();

app.Run();
