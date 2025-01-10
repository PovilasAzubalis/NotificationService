using NotificationService.Services;
using NotificationService.Services.Providers.Email;
using NotificationService.Services.Providers.SMS;
using Quartz;
using SendGrid;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();


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
    q.ScheduleJob<NotificationRetryJob>(trigger => trigger
        .WithIdentity("RetryTrigger")
        .StartNow()
        .WithSimpleSchedule(x => x.WithIntervalInMinutes(5).RepeatForever()));
});
builder.Services.AddQuartz(q => q.UseMicrosoftDependencyInjectionJobFactory());

// Ensure Quartz runs as a hosted service
builder.Services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);


builder.Services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);

// Notification Providers and Manager
builder.Services.AddTransient<ISmsService, TwilioProvider>();
builder.Services.AddTransient<ISmsService, SnsProvider>();
builder.Services.AddSingleton<NotificationManager>();
builder.Services.AddTransient<IEmailService, EmailService>();
builder.Services.AddSingleton<ISendGridClient>(new SendGridClient("your_sendgrid_api_key"));

var app = builder.Build();


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

app.Run();