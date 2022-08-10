using EfCoreImmutabilitySample;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using var host = Host.CreateDefaultBuilder()
    .ConfigureLogging(builder =>
    {
        builder.SetMinimumLevel(LogLevel.Information);
    })
    .ConfigureServices(services =>
    {
        services.AddDbContext<SampleDbContext>(options =>
        {
            var connectionString = new SqliteConnectionStringBuilder
            {
                DataSource = Path.Combine(
                    Path.GetTempPath(),
                    "efcore-immutability-sample.sqlite3"
                )
            }.ConnectionString;

            options.UseSqlite(connectionString)
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        });
        services.AddHostedService<ProgramHost>();
    })
    .Build();

await host.RunAsync();