using Microsoft.AspNetCore.Authentication.JwtBearer;
using SBS.Application;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SBS.Application.Interfaces.Common;
using SBS.Application.Settings;
using SBS.Domain.Entities;
using SBS.Infrastructure;
using SBS.Infrastructure.CurrentUserService;
using SBS.Infrastructure.Persistence._Data;
using SmartBooking.Api.Extensions;
using System.Text;
using SBS.Application.Queries;

var webApplicationBuilder = WebApplication.CreateBuilder(args);

// Get allowed origins from configuration
var allowedOrigins = webApplicationBuilder.Configuration.GetSection("AllowedOrigins").Get<string[]>();

// Registring the Mediator
webApplicationBuilder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetReportQueryHandler).Assembly));

//Add CORS
webApplicationBuilder.Services.AddCors(options =>
{
	options.AddPolicy("AllowAngularApp",
       policy =>
		{
			policy.WithOrigins(allowedOrigins!) // frontend URL
				  .AllowAnyHeader()
				  .AllowAnyMethod()
				  .AllowCredentials();
		});
});

// Configure JWTSettings
webApplicationBuilder.Services.Configure<JWTSettings>(
    webApplicationBuilder.Configuration.GetSection("JwtSettings"));

var jwtSettings = webApplicationBuilder.Configuration
    .GetSection("JwtSettings")
    .Get<JWTSettings>();

// Add Infrastructure and Application Services
webApplicationBuilder.Services.AddInfrastructureServices(webApplicationBuilder.Configuration);
webApplicationBuilder.Services.AddApplicationDependencies(webApplicationBuilder.Configuration);

// Add Identity services (only once)
webApplicationBuilder.Services
    .AddIdentity<ApplicationUser, IdentityRole<Guid>>(options =>
    {
        options.User.RequireUniqueEmail = true;
    })
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

// Add Authentication with JWT
webApplicationBuilder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer( op =>
    {
        op.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidIssuer = jwtSettings!.Issuer,
            ValidAudience = jwtSettings.Audience,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret!))
        };
    });
webApplicationBuilder.Services.AddAuthorization();
// Register CurrentUserService
webApplicationBuilder.Services.AddHttpContextAccessor();
webApplicationBuilder.Services.AddScoped<ICurrentUserService, CurrentUserService>();

webApplicationBuilder.Services.AddHttpContextAccessor();

// Add Controllers and Swagger
webApplicationBuilder.Services.AddControllersWithViews();
webApplicationBuilder.Services.AddEndpointsApiExplorer();
webApplicationBuilder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "SmartBoking API", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});


var app = webApplicationBuilder.Build();


// Configure the HTTP request pipeline
app.UseSwagger();

app.UseSwaggerUI();
if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

// Use CORS
app.UseCors("AllowAngularApp");

// Enable Authentication & Authorization Middleware
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapControllers();
app.MapFallbackToFile("index.html");

// Database Initialization (via Extension Method)
await app.InitializeDbAsync();
app.Run();
