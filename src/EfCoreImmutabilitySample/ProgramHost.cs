using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace EfCoreImmutabilitySample;

public class ProgramHost : IHostedService
{
    private readonly ILogger<ProgramHost> _logger;
    private readonly SampleDbContext _context;

    public ProgramHost(
        ILogger<ProgramHost> logger,
        SampleDbContext context
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

        _logger.LogDebug("Creating an explicit database transaction");
        await using var tx = await _context.Database.BeginTransactionAsync(ct);

        _logger.LogDebug("Adding person");
        var person = await _context.CreateAsync(new PersonEntity(
            Forename: "Bruce",
            Surname: "Wayne"
        ), ct);

        _logger.LogDebug("Updating person's birthdate");
        person = await _context.UpdateAsync(person with
        {
            MiddleNames = "Thomas",
            Birthdate = new DateOnly(1972, 02, 19)
        }, ct);

        _logger.LogDebug("Deleting person");
        await _context.DeleteAsync(person, ct);

        _logger.LogDebug("Getting person by id");
        person = await _context.ReadByIdAsync<PersonEntity>(person.Id, ct);

        if (person is not null)
            throw new InvalidOperationException("Person should be null");

        _logger.LogDebug("Commiting database transaction");
        await tx.CommitAsync(ct);
    }

    public Task StopAsync(CancellationToken ct) => Task.CompletedTask;
}