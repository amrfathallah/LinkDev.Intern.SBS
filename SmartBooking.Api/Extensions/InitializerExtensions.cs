using SBS.Application.Interfaces.Initializers;

namespace SmartBooking.Api.Extensions
{
	public static class InitializerExtensions
	{
		public static async Task<WebApplication> InitializeDbAsync(this WebApplication app)
		{
			using var scope = app.Services.CreateAsyncScope();
			var serviceProvider = scope.ServiceProvider;
			var dbInitializer = serviceProvider.GetRequiredService<IDbInitializer>();

			var loggingFactory = serviceProvider.GetRequiredService<ILoggerFactory>();

			try
			{

				await dbInitializer.InitializeDbAsync();
				await dbInitializer.SeedAsync();
			}
			catch (Exception ex)
			{

				var logger = loggingFactory.CreateLogger<Program>();
				logger.LogError(ex, "An error occurred while applying the migrations to initialize the database.");
			}

			return app;
		}
	}
}
