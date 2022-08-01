using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using var host = Host.CreateDefaultBuilder()
    .ConfigureLogging(builder =>
    {
        builder.SetMinimumLevel(LogLevel.Trace);
    })
    .Build();

await host.RunAsync();