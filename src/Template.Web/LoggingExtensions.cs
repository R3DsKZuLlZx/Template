using Serilog;
using Serilog.Events;
using Serilog.Formatting.Compact;
using Serilog.Sinks.SystemConsole.Themes;

namespace Template.Web;

public static class LoggingExtensions
{
    public static LoggerConfiguration ConfigureDefaultSerilog(
        this LoggerConfiguration loggerConfiguration, 
        bool isDevelopment)
    {
        loggerConfiguration.Enrich.FromLogContext();

        loggerConfiguration.MinimumLevel.Information();
        loggerConfiguration.MinimumLevel.Override("Microsoft", LogEventLevel.Warning);
        loggerConfiguration.MinimumLevel.Override("System", LogEventLevel.Warning);
        loggerConfiguration.MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information);
        
        // Only one configuration can be used for the console, we cant have both a theme and a formatter. See https://github.com/serilog/serilog-sinks-console/issues/103
        if (isDevelopment)
        {
            loggerConfiguration.WriteTo.Console(theme: AnsiConsoleTheme.Code);
        }
        else
        {
            loggerConfiguration.WriteTo.Console(new CompactJsonFormatter());
        }
            
        return loggerConfiguration;
    }
}
