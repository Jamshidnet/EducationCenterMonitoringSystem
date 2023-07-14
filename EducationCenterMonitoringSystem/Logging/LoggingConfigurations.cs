using Serilog.Events;
using Serilog;
using Serilog.Sinks.TelegramBot;

namespace EducationCenterMonitoringSystem.Logging
{
    public static class LoggingConfigurations
    {
        public static void UseLogging(IConfiguration configuration)
        {

            Log.Logger = new LoggerConfiguration()
                       .ReadFrom.Configuration(configuration)
                       .WriteTo.TelegramBot(
                           token: configuration["TelegramBot:Token"],
                           chatId: configuration["TelegramBot:ChatId"],
                           restrictedToMinimumLevel: LogEventLevel.Information
                       )

                       .Enrich.FromLogContext()
                       .CreateLogger();
        }


    }
}
