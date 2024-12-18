using Cryptotracker.API.Jobs;
using Microsoft.AspNetCore.Mvc;
using Quartz;

namespace Cryptotracker.API.Utils;

public static class Extensions
{
    public static IServiceCollection ConfigureModelValidation(this IServiceCollection services)
    {
        return services.Configure<ApiBehaviorOptions>(options =>
        {
            options.InvalidModelStateResponseFactory = context =>
            {
                // Extract validation error messages
                var errors = context.ModelState
                    .Where(ms => ms.Value?.Errors.Count > 0)
                    .SelectMany(ms => ms.Value!.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToArray();

                // Combine all error messages into a single string, or handle them individually
                var detailMessage = string.Join("; ", errors);

                // Return a ProblemDetails object styled like your other error responses
                var problemDetails = new ProblemDetails
                {
                    Title = "Invalid input",
                    Detail = detailMessage,
                    Status = StatusCodes.Status400BadRequest
                };

                return new BadRequestObjectResult(problemDetails);
            };
        });

    }

    public static IServiceCollection AddCronJobs(this IServiceCollection services, ConfigurationManager configuration)
    {
        services.AddQuartz(q =>
        {
            var jobConfig = configuration.GetSection("Jobs:UpdateCryptoPrices");

            // Register the job
            var jobKey = new JobKey("UpdateCryptoPrices");
            q.AddJob<UpdateCryptoPricesJob>(opts => opts.WithIdentity(jobKey));

            q.AddTrigger(opts => opts
                .ForJob(jobKey)
                .WithIdentity("UpdateCryptoPrices-trigger")
                .WithCronSchedule(jobConfig.GetValue<string>("CronSchedule") ?? "0 */5 * ? * *")
            );
        });

        services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);

        return services;
    }

    public static IServiceCollection AddCorsPolicies(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("AllowFrontendOrigin",
                builder =>
                {
                    builder
                        .WithOrigins("http://localhost:5173")
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials();
                });
        });

        return services;
    }
}
