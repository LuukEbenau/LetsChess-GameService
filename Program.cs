using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using NLog;
using NLog.Extensions.Logging;
using NLog.Web;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace LetsChess_GameService
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
			var config = new ConfigurationBuilder()
				.SetBasePath(Directory.GetCurrentDirectory())
				.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
				.AddJsonFile($"appsettings.{env}.json", optional: true, reloadOnChange: true).Build();

			LogManager.Configuration = new NLogLoggingConfiguration(config.GetSection("NLog"));
			var logger = NLogBuilder.ConfigureNLog(LogManager.Configuration).GetCurrentClassLogger();
			try
			{
				logger.Debug($"starting application '{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}'");
				CreateHostBuilder(args).Build().Run();
			}
			catch (Exception exception)
			{
				logger.Error(exception, $"Stopped '{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}' because of an exception");
				throw;
			}
			finally
			{
				NLog.LogManager.Shutdown();
			}
		}

		public static IHostBuilder CreateHostBuilder(string[] args) =>
			Host.CreateDefaultBuilder(args)
			.ConfigureAppConfiguration((hostingContext, config) =>
			{
				var env = hostingContext.HostingEnvironment;
				Console.WriteLine($"the environment is now: {env.EnvironmentName}");

				//TODO: hij pakt deze niet goed in kubernetes?
				config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
				.AddJsonFile($"appsettings.{env.EnvironmentName}.json",
								optional: true, reloadOnChange: true);
				config.AddEnvironmentVariables("LETSCHESS_");
			})
			.ConfigureWebHostDefaults(webBuilder =>
			{
				webBuilder.UseNLog().UseStartup<Startup>();
			}).ConfigureLogging(logging =>
			{
				logging.ClearProviders();
				logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
			});
	}
}
