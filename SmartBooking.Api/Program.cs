using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using SBS.Application;
using SBS.Infrastructure;
using SBS.Application;
using SmartBooking.Api.Extensions;
using SBS.Application.Mapping;

var webApplicationBuilder = WebApplication.CreateBuilder(args);

// Add services to the container.

webApplicationBuilder.Services.AddControllersWithViews();

webApplicationBuilder.Services.AddCors(options =>
{
    options.AddPolicy("Origins", policy =>
    {
        policy.WithOrigins(
                "https://localhost:44417", // Development
                "https://smart-office-eqbvh2eddnf5fyee.westeurope-01.azurewebsites.net" // Production
              )
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });
});

webApplicationBuilder.Services.AddEndpointsApiExplorer().AddSwaggerGen();

webApplicationBuilder.Services.AddInfrastructureServices(webApplicationBuilder.Configuration);
webApplicationBuilder.Services.AddApplicationServices(webApplicationBuilder.Configuration);





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

app.UseCors("Origins");

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

