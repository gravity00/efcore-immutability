namespace EfCoreImmutabilitySample;

/// <summary>
/// Base class for all database entities
/// </summary>
public abstract record Entity
{
    /// <summary>
    /// Unique identifier
    /// </summary>
    public long Id { get; init; }
}