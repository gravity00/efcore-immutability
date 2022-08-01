namespace EfCoreImmutabilitySample.Database;

public record CarBrandEntity(
    Guid ExternalId,
    string Name
) : Entity
{
    public string? Description { get; init; }
}