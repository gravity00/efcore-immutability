using EfCoreImmutabilitySample.Database;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace EfCoreImmutabilitySample;

public class Runner : IHostedService
{
    private readonly ILogger<Runner> _logger;
    private readonly SampleDbContext _context;
    private readonly Repository<PersonEntity> _repository;

    public Runner(
        ILogger<Runner> logger,
        SampleDbContext context,
        Repository<PersonEntity> repository
    )
    {
        _logger = logger;
        _context = context;
        _repository = repository;
    }

    public Task StopAsync(CancellationToken ct) => Task.CompletedTask;

    public async Task StartAsync(CancellationToken ct)
    {
        _logger.LogDebug("Setting database to a clean state");
        await _context.Database.EnsureDeletedAsync(ct);
        await _context.Database.EnsureCreatedAsync(ct);

        _logger.LogDebug("Adding person");
        var addedPerson = await _repository.AddAsync(new PersonEntity(
            ExternalId: Guid.NewGuid(),
            Forename: "Clark",
            Surname: "Kent"
        )
        {
            MiddleName = "\"Superman\""
        }, ct);

        _logger.LogDebug("Updating person");
        var updatedPerson = await _repository.UpdateAsync(addedPerson with
        {
            Birthdate = new DateOnly(1915, 04, 17)
        }, ct);

        _logger.LogDebug(@"Comparing ADDED instance with UPDATED instance
ReferenceEquals:{ReferenceEquals}
PropertyEquals: {PropertyEquals}",
            ReferenceEquals(addedPerson, updatedPerson), // should be false, different instances
            addedPerson == updatedPerson // should be false, one has the Birthdate defined
        );

        _logger.LogDebug("Getting brand by id");
        var readPerson = await _repository.GetByIdAsync(addedPerson.Id, ct);

        _logger.LogDebug(@"Comparing UPDATED instance with READ instance
ReferenceEquals:{ReferenceEquals}
PropertyEquals: {PropertyEquals}",
            ReferenceEquals(updatedPerson, readPerson), // should be false, different instances
            updatedPerson == readPerson // should be true, both have equal properties
        );
    }
}