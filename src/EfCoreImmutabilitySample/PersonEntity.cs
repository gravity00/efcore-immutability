namespace EfCoreImmutabilitySample;

public record PersonEntity(
    string Forename,
    string Surname
) : Entity
{
    public string? MiddleNames { get; init; }

    public DateOnly? Birthdate { get; init; }
}