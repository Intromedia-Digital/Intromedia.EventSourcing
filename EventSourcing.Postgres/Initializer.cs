using EventSourcing.Postgres;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

internal sealed class Initializer(IDbContextFactory<EventDbContext> dbFactory) : IHostedService
{
    public Task StartAsync(CancellationToken cancellationToken)
    {
        using var db = dbFactory.CreateDbContext();
        db.Database.Migrate();
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}

