using Microsoft.Extensions.DependencyInjection;

internal class EventSourcingPostgresBuilder : IEventSourcingPostgresBuilder
{
    public IServiceCollection Services { get; }
    public EventSourcingPostgresBuilder(IServiceCollection services)
    {
        Services = services;
    }
}