using App.WindowsService;
using Microsoft.Extensions.Logging.Configuration;
using Microsoft.Extensions.Logging.EventLog;
using CliWrap;

const string ServiceName = "POSPrinterService";

if (args is { Length: 1 })
{
    try
    {
        string executablePath =
            Path.Combine(AppContext.BaseDirectory, "App.WindowsService.exe");

        if (args[0] is "/Install")
        {
            await Cli.Wrap("sc")
                .WithArguments(new[] { "create", ServiceName, $"binPath={executablePath}", "start=auto" })
                .ExecuteAsync();
            await Cli.Wrap("sc")
                .WithArguments(new[] { "start", ServiceName })
                .ExecuteAsync();
        }
        else if (args[0] is "/Uninstall")
        {
            await Cli.Wrap("sc")
                .WithArguments(new[] { "stop", ServiceName })
                .ExecuteAsync();

            await Cli.Wrap("sc")
                .WithArguments(new[] { "delete", ServiceName })
                .ExecuteAsync();
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex);
    }

    return;
}

Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

builder.Services.AddWindowsService(options =>
{
    options.ServiceName = "POSPrinterService";
});

LoggerProviderOptions.RegisterProviderOptions<
    EventLogSettings, EventLogLoggerProvider>(builder.Services);


builder.Services.AddHostedService<Worker>();

IHost host = builder.Build();

host.Run();
