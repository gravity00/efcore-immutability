namespace EfCoreImmutabilitySample;

public record PersonEntity(
    string Forename,
    string Surname
) : Entity
{
    public string? MiddleName { get; init; }

    public DateOnly? Birthdate { get; init; }
}