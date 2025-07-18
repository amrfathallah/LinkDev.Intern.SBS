using Microsoft.AspNetCore.Builder;
using SBS.Infrastructure;
using SBS.Application;
using SmartBooking.Api.Extensions;

var webApplicationBuilder = WebApplication.CreateBuilder(args);

// Add services to the container.

webApplicationBuilder.Services.AddControllersWithViews();


webApplicationBuilder.Services.AddEndpointsApiExplorer().AddSwaggerGen();

webApplicationBuilder.Services.AddInfrastructureServices(webApplicationBuilder.Configuration);
webApplicationBuilder.Services.AddApplicationDependencies(webApplicationBuilder.Configuration);


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

