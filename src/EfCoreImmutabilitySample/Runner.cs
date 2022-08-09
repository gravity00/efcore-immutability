using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace EfCoreImmutabilitySample;

public class Runner : IHostedService
{
    private readonly ILogger<Runner> _logger;
    private readonly SampleDbContext _context;

    public Runner(
        ILogger<Runner> logger,
        SampleDbContext context
    )
    {
        _logger = logger;
        _context = context;
    }

    public Task StopAsync(CancellationToken ct) => Task.CompletedTask;

    public async Task StartAsync(CancellationToken ct)
    {
        _logger.LogDebug("Setting database to a clean state");
        await _context.Database.EnsureDeletedAsync(ct);
        await _context.Database.EnsureCreatedAsync(ct);

        _logger.LogDebug("Creating an explicit database transaction");
        await using var tx = await _context.Database.BeginTransactionAsync(ct);

        _logger.LogDebug("Adding person");
        var addedPerson = await _context.CreateAsync(new PersonEntity(
            ExternalId: Guid.NewGuid(),
            Forename: "Clark",
            Surname: "Kent"
        )
        {
            MiddleName = "\"Superman\""
        }, ct);

        _logger.LogDebug("Updating person");
        var updatedPerson = await _context.UpdateAsync(addedPerson with
        {
            Birthdate = new DateOnly(1915, 04, 17)
        }, ct);

        _logger.LogInformation(@"Comparing ADDED with UPDATED instance
  ReferenceEquals:  {ReferenceEquals}
  PropertyEquals:   {PropertyEquals}",
            ReferenceEquals(addedPerson, updatedPerson), // should be false, different instances
            addedPerson == updatedPerson // should be false, one instance has the Birthdate defined
        );

        _logger.LogDebug("Getting person by id");
        var readPerson = await _context.ReadByIdAsync<PersonEntity>(addedPerson.Id, ct);

        _logger.LogInformation(@"Comparing UPDATED with READ instance
  ReferenceEquals:  {ReferenceEquals}
  PropertyEquals:   {PropertyEquals}",
            ReferenceEquals(updatedPerson, readPerson), // should be false, different instances
            updatedPerson == readPerson // should be true, both have equal properties
        );

        if (readPerson is not null)
        {
            _logger.LogDebug("Deleting person");
            await _context.DeleteAsync(readPerson, ct);
        }

        _logger.LogDebug("Commiting database transaction");
        await tx.CommitAsync(ct);
    }
}