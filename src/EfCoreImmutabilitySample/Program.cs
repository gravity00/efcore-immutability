﻿using EfCoreImmutabilitySample;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using var host = Host.CreateDefaultBuilder()
    .ConfigureLogging(builder =>
    {
        builder.SetMinimumLevel(LogLevel.Trace);
    })
    .ConfigureServices(services =>
    {
        services.AddDbContext<EfCoreImmutabilityDbContext>(options =>
        {
            options.UseSqlite("Data Source=:memory:");
        });
        services.AddHostedService<Runner>();
    })
    .Build();

await host.RunAsync();