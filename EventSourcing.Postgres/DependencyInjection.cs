using EventSourcing.Postgres;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

public static class DependencyInjection
{
    public static IEventSourcingPostgresBuilder UsePostgres(this IEventSourcingBuilder builder,
        string connectionString)
    {
        var postgresBuilder = new EventSourcingPostgresBuilder(builder.Services);

        postgresBuilder.Services.AddDbContextFactory<EventDbContext>((sp, options) =>
        {
            options.UseNpgsql(connectionString)
                .AddInterceptors(sp.GetServices<SaveChangesInterceptor>().ToArray());
        });

        postgresBuilder.Services.AddSingleton<IHostedService, Initializer>();

        return postgresBuilder;
    }
   
    public static IEventSourcingPostgresBuilder AddAppendStream<TStream>(this IEventSourcingPostgresBuilder builder)
        where TStream : IStream
    {
        builder.Services.AddSingleton<IAppendStream<TStream>, EventStreams<TStream>>();
        return builder;
    }
    public static IEventSourcingPostgresBuilder AddSubscription<TSubscription, TStream>(this IEventSourcingPostgresBuilder builder)
        where TSubscription : Subscription<TStream>
        where TStream : IStream
    {
        builder.Services.AddSingleton<Subscription<TStream>, TSubscription>();
        builder.Services.AddSingleton<SaveChangesInterceptor, Interceptor<TStream>>();
        return builder;
    }

}

