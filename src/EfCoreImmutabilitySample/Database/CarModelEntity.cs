namespace EfCoreImmutabilitySample.Database;

public record CarModelEntity(
    Guid ExternalId,
    long BrandId,
    string Name
) : Entity
{
    public string? Description { get; init; }
}