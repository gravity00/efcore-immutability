namespace EfCoreImmutabilitySample;

public record PersonEntity(
    Guid ExternalId,
    string Forename,
    string Surname
) : Entity
{
    public string? MiddleName { get; init; }

    public DateOnly? Birthdate { get; init; }
}