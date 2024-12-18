using Microsoft.AspNetCore.Mvc;
using Cryptotracker.API.Utils;
using Cryptotracker.Core.Interfaces.Services;
using CryptoTracker.Infrastructure.Services;
using Cryptotracker.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.ConfigureModelValidation();
builder.Services.AddHealthChecks();
builder.Services.AddCorsPolicies();

builder.Services.AddHttpClient<ICoinApiService, CoinApiService>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["CoinApi:BaseUrl"] ?? "https://api.coincap.io/v2/");
    client.DefaultRequestHeaders.Add("X-CoinAPI-Key", builder.Configuration["CoinApi:ApiKey"]);
});

builder.Services.AddHttpLogging(o => { });
builder.Services.AddCronJobs(builder.Configuration);

// Configure DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        b => b.MigrationsAssembly("Cryptotracker.Infrastructure")
    ));


// Add Swagger for testing
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseCors("AllowFrontendOrigin");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Set up a global exception handler for unhandled exceptions, that points to the "/error" endpoint.
app.UseExceptionHandler("/error");
app.Map("/error", (HttpContext httpContext) =>
{
    var problemDetails = new ProblemDetails
    {
        Status = StatusCodes.Status500InternalServerError,
        Title = "An unexpected error occurred!",
        Detail = "Please try again later or contact support if the problem persists."
    };

    return Results.Problem(problemDetails);
});

app.UseHttpLogging();

app.MapHealthChecks("/health");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
