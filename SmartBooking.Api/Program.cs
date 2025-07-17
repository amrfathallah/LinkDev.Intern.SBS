using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using SBS.Application;
using SBS.Application.Interfaces.Common;
using SBS.Domain.Entities;
using SBS.Infrastructure;
using SBS.Infrastructure.CurrentUserService;
using SBS.Infrastructure.Persistence._Data;
using SmartBooking.Api.Extensions;

var webApplicationBuilder = WebApplication.CreateBuilder(args);

// Add services to the container.

webApplicationBuilder.Services.AddControllersWithViews();


webApplicationBuilder.Services.AddEndpointsApiExplorer().AddSwaggerGen();

webApplicationBuilder.Services.AddHttpContextAccessor().AddScoped(typeof(ICurrentUserService), typeof(CurrentUserService));



// Add Identity services

webApplicationBuilder.Services
	.AddIdentity<ApplicationUser, IdentityRole<Guid>>()
	.AddEntityFrameworkStores<AppDbContext>()
	.AddDefaultTokenProviders();



webApplicationBuilder.Services.AddInfrastructureServices(webApplicationBuilder.Configuration);
webApplicationBuilder.Services.AddApplicationServices();

var app = webApplicationBuilder.Build();




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


app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html");

app.MapControllers();



//Update Database Initialization
await app.InitializeDbAsync();


app.Run();

