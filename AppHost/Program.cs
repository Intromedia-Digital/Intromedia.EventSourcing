var builder = DistributedApplication.CreateBuilder(args);

var npgSql = builder.AddPostgres("Postgres")
    .AddDatabase("event-sourcing");

builder.AddProject<Projects.EventSourcing_Sample>("sample")
    .WithReference(npgSql)
    .WaitFor(npgSql);

builder.Build().Run();
