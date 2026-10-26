using System.Text;
using System.Text.Json.Serialization;
using InsuranceManagement.API.Endpoints;
using InsuranceManagement.API.Middleware;
using InsuranceManagement.Domain.Models;
using InsuranceManagement.Application.Repositories;
using InsuranceManagement.Application.Services;
using InsuranceManagement.API.Services;
using InsuranceManagement.Application;
using InsuranceManagement.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, services, configuration) => configuration
    .ReadFrom.Configuration(context.Configuration)
    .ReadFrom.Services(services)
    .Enrich.FromLogContext()
    .WriteTo.Console());

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("The 'DefaultConnection' connection string is not configured.");

builder.Services.AddInsuranceApplication(builder.Configuration);
builder.Services.AddInsuranceInfrastructure(connectionString);
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped(typeof(ValidationFilter<>));
builder.Services.AddSingleton<DemoItemStore>();

builder.Services.AddOptions<JwtOptions>()
    .BindConfiguration(JwtOptions.SectionName)
    .ValidateDataAnnotations()
    .ValidateOnStart();

var jwtOptions = builder.Configuration.GetSection(JwtOptions.SectionName).Get<JwtOptions>()
    ?? throw new InvalidOperationException("The JWT configuration section is missing.");
if (Encoding.UTF8.GetByteCount(jwtOptions.SigningKey) < 32)
{
    throw new InvalidOperationException(
        "Configure Jwt:SigningKey with a secret of at least 32 bytes using user secrets or environment variables.");
}

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwtOptions.Issuer,
            ValidateAudience = true,
            ValidAudience = jwtOptions.Audience,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SigningKey)),
            ValidateLifetime = true,
            ClockSkew = TimeSpan.FromSeconds(30),
            RoleClaimType = System.Security.Claims.ClaimTypes.Role,
            NameClaimType = System.Security.Claims.ClaimTypes.NameIdentifier
        };
    });
builder.Services.AddAuthorization();
builder.Services.ConfigureHttpJsonOptions(options =>
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter()));
builder.Services.AddExceptionHandler<ApiExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Insurance Management API",
        Version = "v1",
        Description = "API for managing insurance customers, policies, claims, and access control."
    });
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Enter a JWT access token."
    });
    options.OperationFilter<AuthorizeOperationFilter>();

    var baseDirectory = AppContext.BaseDirectory;
    options.IncludeXmlComments(Path.Combine(baseDirectory, "InsuranceManagement.API.xml"));
    options.IncludeXmlComments(Path.Combine(baseDirectory, "InsuranceManagement.Application.xml"));
    options.IncludeXmlComments(Path.Combine(baseDirectory, "InsuranceManagement.Domain.xml"));
});

var app = builder.Build();

await AdministratorBootstrapper.CreateIfConfiguredAsync(app.Services, app.Configuration);

app.UseSerilogRequestLogging();
app.UseExceptionHandler();
app.UseStatusCodePages();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapCustomers();
app.MapPolicies();
app.MapClaims();
app.MapAuth();
app.MapDemoItemEndpoints();

app.Run();
