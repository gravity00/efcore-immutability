using EfCoreImmutabilitySample;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using var host = Host.CreateDefaultBuilder()
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