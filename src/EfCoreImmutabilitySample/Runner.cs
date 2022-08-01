using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace EfCoreImmutabilitySample;

public class Runner : IHostedService
{
    private readonly ILogger<Runner> _logger;
    private readonly EfCoreImmutabilityDbContext _context;

    public Runner(
        ILogger<Runner> logger,
        EfCoreImmutabilityDbContext context
    )
    {
        _logger = logger;
        _context = context;
    }

    public async Task StartAsync(CancellationToken ct)
    {
        _logger.LogDebug("Setting database to a clean state");
        await _context.Database.EnsureDeletedAsync(ct);
        await _context.Database.EnsureCreatedAsync(ct);
    }

    public Task StopAsync(CancellationToken ct) => Task.CompletedTask;
}