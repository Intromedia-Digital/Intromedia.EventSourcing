using EventSourcing.Postgres;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;


internal sealed class Interceptor<TStream> : SaveChangesInterceptor
    where TStream : IStream
{
    private readonly ILogger<Interceptor<TStream>> _logger;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    public Interceptor(ILogger<Interceptor<TStream>> logger, IServiceScopeFactory serviceScopeFactory)
    {
        _logger = logger;
        _serviceScopeFactory = serviceScopeFactory;
    }
    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        if (eventData.Context is not EventDbContext db)
            return await base.SavingChangesAsync(eventData, result, cancellationToken);

        var res = await base.SavingChangesAsync(eventData, result, cancellationToken);

        var events = db.ChangeTracker.Entries<Event>().Select(x => x.Entity);
        using var scope = _serviceScopeFactory.CreateScope();

        var subscriptions = scope.ServiceProvider.GetServices<Subscription<TStream>>();
        foreach (var sub in subscriptions)
        {
            foreach (var evnt in events
                .Where(e => sub.SubscribedEvents.Contains(e.Data.GetType()))
                .OrderBy(x => x.Data.Timestamp)
                .ThenBy(x => x.Version)
                )
            {
                await sub.HandleEvent(evnt.StreamId, evnt.Data, cancellationToken);
            }
        }

        return res;
    }

}