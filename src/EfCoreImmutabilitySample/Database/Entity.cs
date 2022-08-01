namespace EfCoreImmutabilitySample.Database;

public abstract record Entity
{
    public long Id { get; init; }
}