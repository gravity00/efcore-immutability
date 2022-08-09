namespace EfCoreImmutabilitySample;

public abstract record Entity
{
    public long Id { get; init; }
}