namespace orchestrator.service;

using System.IO;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using orchestrator.service.Extensions;

public class Program
{
    public static void Main(string[] args)
    {
        StaticMethod.PrintMessage("Orchestrator Service");

        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureHostConfiguration(cfg =>
                {
                    cfg.SetBasePath(Directory.GetCurrentDirectory());
                    cfg.AddJsonFile("appsettings.json", true, true);
                    if (args.Length != 0)
                    {
                        cfg.AddJsonFile($"appsettings.{GetValueByKey(args, "environment")}.json", true, true);
                    }
                    cfg.AddEnvironmentVariables().Build();
                })
                .ConfigureServices((hostContext, services) => services
                        .AddServiceConfiguration(hostContext.Configuration)
                        .AddEventBus(hostContext.Configuration)
                        .AddHostedService<Worker>());

    private static string GetValueByKey(IEnumerable<string> args, string key) =>
        args.Single(x => x.Contains(key)).Split('=').Last();
}
