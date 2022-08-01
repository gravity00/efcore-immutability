namespace EfCoreImmutabilitySample.Database;

public record CarEntity(
    Guid ExternalId,
    long ModelId,
    string Plate
) : Entity;