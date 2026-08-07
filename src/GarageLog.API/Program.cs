using System.Text;
using GarageLog.API.Middleware;
using GarageLog.Application;
using GarageLog.Infrastructure;
using GarageLog.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Register application layers
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

// Configure CORS for frontend communication
var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>();

builder.Services.AddCors(options =>
{
    options.AddPolicy(
        "AllowFrontend",
        policy =>
        {
            policy.WithOrigins(allowedOrigins ?? []).AllowAnyHeader().AllowAnyMethod().AllowCredentials();
        }
    );
});

// Configure JWT authentication using HttpOnly cookies
var jwtSettings = builder.Configuration.GetSection("JwtSettings");

var secret = jwtSettings["Secret"] ?? throw new InvalidOperationException("JWT Secret is missing");

builder
    .Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],

            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret)),
        };

        // Read JWT from authentication cookie
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                if (context.Request.Cookies.TryGetValue("auth_token", out var token))
                {
                    context.Token = token;
                }

                return Task.CompletedTask;
            },
        };
    });

// Enable authorization support
builder.Services.AddAuthorization();

// Register controllers
builder.Services.AddControllers();

// Health check endpoint for Docker/deployment monitoring
builder.Services.AddHealthChecks();

// Swagger API documentation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Apply migrations + seed data on startup
app.ApplyMigrations();

app.UseMiddleware<ExceptionHandlingMiddleware>();

// HTTPS is handled externally in production environments
if (!app.Environment.IsProduction())
{
    app.UseHttpsRedirection();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Configure request pipeline
app.UseCors("AllowFrontend");

app.UseAuthentication();

app.UseAuthorization();

// Map endpoints
app.MapHealthChecks("/health");

app.MapControllers();

app.Run();
