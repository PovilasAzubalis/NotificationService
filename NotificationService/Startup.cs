using NotificationService.Services;
using NotificationService.Services.Providers.SMS;
using Quartz;

namespace NotificationService;
public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();

        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
            {
                Title = "Notification Service API",
                Version = "v1",
                Description = "API for sending notifications via multiple channels and providers."
            });
        });

        // Register providers
        services.AddTransient<ISmsService, TwilioProvider>();
        services.AddTransient<ISmsService, SnsProvider>();

        // Register manager
        services.AddSingleton<NotificationManager>();

        // Configure Quartz
        services.AddQuartz(q => q.UseMicrosoftDependencyInjectionJobFactory());
        services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);

    }

    public static void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseRouting();
        app.UseEndpoints(endpoints => endpoints.MapControllers());
    }
}