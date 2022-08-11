namespace EfCoreImmutabilitySample;

/// <summary>
/// Representation of a person
/// </summary>
/// <param name="Forename">Person first name</param>
/// <param name="Surname">Person last name</param>
public record PersonEntity(
    string Forename,
    string Surname
) : Entity
{
    /// <summary>
    /// Person middle names, separated by spaces
    /// </summary>
    public string? MiddleNames { get; init; }

    /// <summary>
    /// Person date of birth
    /// </summary>
    public DateOnly? Birthdate { get; init; }
}