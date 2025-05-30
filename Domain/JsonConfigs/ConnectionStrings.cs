namespace Domain.JsonConfigs
{
    public sealed record ConnectionStrings
    {
        public string LocalDb { get; init; } = null!;

    }
}
