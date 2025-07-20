using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using SBS.Application.Settings;
using SBS.Domain.Entities;
using SBS.Infrastructure;
using SBS.Infrastructure.Persistence._Data;
using SmartBooking.Api.Extensions;
using System.Security.Cryptography;
using System.Text;


var webApplicationBuilder = WebApplication.CreateBuilder(args);

// Configure JWTSettings
webApplicationBuilder.Services.Configure<JWTSettings>(
	webApplicationBuilder.Configuration.GetSection("JWTSettings"));

var jwtSettings = webApplicationBuilder.Configuration.GetSection("JWTSettings").Get<JWTSettings>();

webApplicationBuilder.Services.AddInfrastructureServices(webApplicationBuilder.Configuration);

webApplicationBuilder.Services.AddIdentity<ApplicationUser, IdentityRole<Guid>>(options =>
	{
    options.User.RequireUniqueEmail = true;
	})
.AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders();

// Add Authentication with JWT
webApplicationBuilder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).
	AddJwtBearer(op =>
	{
		op.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
		{
			ValidateIssuer = true,
			ValidateAudience = true,
			ValidateLifetime = true,
			ValidIssuer = jwtSettings.Issuer,
			ValidAudience = jwtSettings.Audience,
			IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret))
		};
	});


// Add services to the container.

webApplicationBuilder.Services.AddControllersWithViews();


webApplicationBuilder.Services.AddEndpointsApiExplorer().AddSwaggerGen();

webApplicationBuilder.Services.AddInfrastructureServices(webApplicationBuilder.Configuration);

var app = webApplicationBuilder.Build();



#region Configure Kestrel Middlewares
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{

	app.UseSwagger();
	app.UseSwaggerUI();

	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();


// Enable Authentication Middleware
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html");

app.MapControllers();


#region Update Database Initialization

await app.InitializeDbAsync();

#endregion


#endregion
app.Run();

