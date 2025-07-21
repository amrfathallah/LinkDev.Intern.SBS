using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using SBS.Application;
using SBS.Application.Interfaces.Common;
using SBS.Application.Interfaces.Initializers;
using SBS.Application.Settings;
using SBS.Domain.Entities;
using SBS.Infrastructure;
using SBS.Infrastructure.CurrentUserService;
using SBS.Infrastructure.Persistence._Data;
using SBS.Infrastructure.Persistence.Initializers;
using SmartBooking.Api.Extensions;
using System.Text;

var webApplicationBuilder = WebApplication.CreateBuilder(args);

// Configure JWTSettings
webApplicationBuilder.Services.Configure<JWTSettings>(
    webApplicationBuilder.Configuration.GetSection("JWTSettings"));

var jwtSettings = webApplicationBuilder.Configuration
    .GetSection("JWTSettings")
    .Get<JWTSettings>();

// Add Infrastructure and Application Services
webApplicationBuilder.Services.AddInfrastructureServices(webApplicationBuilder.Configuration);
webApplicationBuilder.Services.AddApplicationServices();

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
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(op =>
    {
        op.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidIssuer = jwtSettings.Issuer,
            ValidAudience = jwtSettings.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret))
        };
    });

// Register CurrentUserService
webApplicationBuilder.Services.AddHttpContextAccessor();
webApplicationBuilder.Services.AddScoped<ICurrentUserService, CurrentUserService>();



// Add Controllers and Swagger
webApplicationBuilder.Services.AddControllersWithViews();
webApplicationBuilder.Services.AddEndpointsApiExplorer();
webApplicationBuilder.Services.AddSwaggerGen();

var app = webApplicationBuilder.Build();

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

// Enable Authentication & Authorization Middleware
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html");
app.MapControllers();

// Database Initialization (via Extension Method)
await app.InitializeDbAsync();

app.Run();
