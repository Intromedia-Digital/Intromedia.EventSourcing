﻿using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

internal sealed class SubscriptionReader<TSubscription, TStream>(
    object serviceKey,
    IServiceScopeFactory serviceScopeFactory
    ) : IHostedService
    where TStream : IStream
    where TSubscription : Subscription<TStream>
{
    private ChangeFeedProcessor changeFeedProcessor;
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var scope = serviceScopeFactory.CreateScope();
        var subscription = scope.ServiceProvider.GetRequiredKeyedService<TSubscription>(serviceKey);
        var stream = scope.ServiceProvider.GetRequiredKeyedService<TStream>(serviceKey);
        var cosmosClient = scope.ServiceProvider.GetRequiredKeyedService<CosmosClient>(serviceKey);
        var options = scope.ServiceProvider.GetRequiredKeyedService<CosmosDatabaseOptions>(serviceKey);

        var db = cosmosClient.GetDatabase(options.DatabaseId);

        string leaseContainerId = $"{stream.Name}_leases";
        var leaseContainer = await db.CreateContainerIfNotExistsAsync(leaseContainerId, "/id");

        changeFeedProcessor = cosmosClient.GetContainer(options.DatabaseId, stream.Name)
            .GetChangeFeedProcessorBuilder<Event>(subscription.GetType().FullName, (changes, ct) => HandleChanges(changes, subscription, ct))
            .WithInstanceName($"{Environment.MachineName}_{Environment.ProcessId}")
            .WithLeaseContainer(leaseContainer.Container)
            .WithStartTime(subscription.StartTime.ToUniversalTime())
            .WithMaxItems(1)
            .Build();

        await changeFeedProcessor.StartAsync();
    }
    public static async Task HandleChanges(IReadOnlyCollection<Event> changes, TSubscription subscription, CancellationToken cancellationToken)
    {
        foreach (var change in changes)
        {
            if (change.Data is IEvent evnt)
            {
                // Check if the event is subscribed
                if (subscription.SubscribedEvents.Any(x => x == evnt.GetType()))
                    await subscription.HandleEvent(change.StreamId, evnt, cancellationToken);
            }
        }
    }
    public async Task StopAsync(CancellationToken cancellationToken)
    {
        if (changeFeedProcessor is not null)
            await changeFeedProcessor.StopAsync();
    }
}
