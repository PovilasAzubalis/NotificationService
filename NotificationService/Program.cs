using NotificationService.Services;
using NotificationService.Services.Providers;
using Quartz;

var builder = WebApplication.CreateBuilder(args);

// Add services to the DI container
builder.Services.AddControllers();

// Add Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Notification Service API",
        Version = "v1",
        Description = "API for sending notifications via multiple channels and providers."
    });
});

// Add Quartz for scheduling
builder.Services.AddQuartz(q =>
{

    // Optionally, register jobs and triggers here
    q.ScheduleJob<NotificationRetryJob>(trigger => trigger
        .WithIdentity("RetryTrigger")
        .StartNow()
        .WithSimpleSchedule(x => x.WithIntervalInMinutes(5).RepeatForever()));
});

// Ensure Quartz runs as a hosted service
builder.Services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);

// Add Notification Providers and Manager
builder.Services.AddTransient<INotificationProvider, TwilioProvider>();
builder.Services.AddTransient<INotificationProvider, SnsProvider>();
builder.Services.AddSingleton<NotificationManager>();

var app = builder.Build();

// Configure middleware pipeline
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Notification Service API v1");
        options.RoutePrefix = string.Empty; // Makes Swagger available at the app's root URL
    });
}

app.UseRouting();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

// Start the application
app.Run();