internal class Package2Subscription : EventSourcing.Postgres.Subscription<PackageStream>
{
    private readonly PackageProjection _projection;
    public Package2Subscription(PackageProjection projection)
    {
        _projection = projection;

        StartFrom(DateTime.MinValue);
        Subscribe<PackageReceived>();
    }
    public override async Task HandleEvent(Guid streamId, IEvent @event, CancellationToken cancellationToken)
    {
        await _projection.Apply(@event);
    }
}