using Microsoft.EntityFrameworkCore;
using System.Data;

namespace EventSourcing.Postgres;
internal sealed class EventStreams<TStream> : IAppendStream<TStream>
where TStream : IStream
{
    private readonly IDbContextFactory<EventDbContext> _dbFactory;
    public EventStreams(IDbContextFactory<EventDbContext> dbFactory)
    {
        _dbFactory = dbFactory;
    }
    public async Task Append(Guid streamId, params IEvent[] events)
    {
        await using var db = _dbFactory.CreateDbContext();
        using var transaction = await db.Database.BeginTransactionAsync();
        foreach (var @event in events)
        {
            var evnt = new Event(streamId, @event);
            await db.Events.AddAsync(evnt);
        }
        await db.SaveChangesAsync();
        await transaction.CommitAsync();
    }
    public async Task<TState> BuildState<TState>(Guid streamId) where TState : IState<TStream>, new()
    {
        await using var db = _dbFactory.CreateDbContext();
        var events = await db.Events
            .Where(e => e.StreamId == streamId)
            .OrderBy(e => e.Version)
            .Select(e => e.Data)
            .ToListAsync();

        var state = new TState()
        {
            Id = streamId
        };

        foreach (var @event in events)
        {
            state.Apply(@event);
        }

        return state;
    }
}
