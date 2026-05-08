using Application.Common.Behaviours;
using FluentValidation;
using Infrastructure;
using InkpactAPI.Common;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using System.Text;

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

var builder = WebApplication.CreateBuilder(args);

// ─────────────────────────────────────────────────────────
// 1. Infrastructure layer (DbContext, Repositories, UnitOfWork, JWT, Email)
// ─────────────────────────────────────────────────────────
builder.Services.AddInfrastructure(builder.Configuration);

// ─────────────────────────────────────────────────────────
// 2. MediatR — scan the Application assembly for handlers
// ─────────────────────────────────────────────────────────
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(LoggingBehaviour<,>).Assembly));

// ─────────────────────────────────────────────────────────
// 3. Pipeline Behaviours — ORDER MATTERS
// Logging runs FIRST (wraps everything), then Validation runs.
// If validation fails, the handler never runs.
// ─────────────────────────────────────────────────────────
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehaviour<,>));
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));

// ─────────────────────────────────────────────────────────
// 4. FluentValidation — auto-discover all validators in Application
// ─────────────────────────────────────────────────────────
builder.Services.AddValidatorsFromAssembly(typeof(LoggingBehaviour<,>).Assembly);

// ─────────────────────────────────────────────────────────
// 5. JWT Authentication
// ─────────────────────────────────────────────────────────
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettings["SecretKey"]
    ?? throw new InvalidOperationException("JWT SecretKey is not configured.");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
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
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddAuthorization();

// ─────────────────────────────────────────────────────────
// 6. CORS
// ─────────────────────────────────────────────────────────
builder.Services.AddCors(options =>
{
    options.AddPolicy("Default", policy =>
        policy.WithOrigins(
                "http://localhost:5173",       // Vite dev server
                "http://localhost:3000",       // common React dev port
                "https://inkpact.vercel.app",
                "https://inkpact-production-0e41.up.railway.app"   // future production frontend
            )
            .AllowAnyMethod()
            .AllowAnyHeader());
});

// ─────────────────────────────────────────────────────────
// 7. Controllers + HttpContextAccessor + OpenAPI
// ─────────────────────────────────────────────────────────
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
    });
builder.Services.AddHttpContextAccessor();
builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer<BearerSecuritySchemeTransformer>();
});

// ─────────────────────────────────────────────────────────
// Build app
// ─────────────────────────────────────────────────────────
var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<Infrastructure.Persistence.AppDbContext>();
    db.Database.Migrate();
}

// ─────────────────────────────────────────────────────────
// HTTP pipeline
// ─────────────────────────────────────────────────────────

    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options.WithTitle("Inkpact API")
               .WithTheme(ScalarTheme.Purple);
               
    });


app.UseMiddleware<InkpactAPI.Middleware.GlobalExceptionMiddleware>();
app.UseHttpsRedirection();
app.UseCors("Default");

// CRITICAL — Authentication MUST come before Authorization
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();