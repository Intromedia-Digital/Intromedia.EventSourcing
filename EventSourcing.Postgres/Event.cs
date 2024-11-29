namespace EventSourcing.Postgres;

internal sealed class Event
{
    public Event(Guid streamId, IEvent @event)
    {
        StreamId = streamId;
        Id = @event.EventId;
        Version = @event.Version;
        Data = @event;
    }
    private Event()
    {
    }
    public long ClusterKey { get; set; }
    public Guid StreamId { get; set; }
    public Guid Id { get; set; }
    public IEvent Data { get; set; }
    public int Version { get; set; }
}
