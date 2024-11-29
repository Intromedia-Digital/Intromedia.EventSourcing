
using Microsoft.EntityFrameworkCore;

namespace EventSourcing.Postgres;

internal sealed class EventDbContext : DbContext
{
    public EventDbContext(DbContextOptions<EventDbContext> options) : base(options)
    {
    }
    public DbSet<Event> Events { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Event>(e =>
        {
            e.ToTable("event");

            e.HasKey(x => x.ClusterKey)
                .HasName("pk_event");

            e.Property(x => x.ClusterKey)
                .HasColumnName("cluster_key")
                .UseIdentityColumn();

            e.Property(x => x.StreamId)
                .HasColumnName("stream_id")
                .HasColumnType("uuid")
                .IsRequired();

            e.Property(x => x.Id)
                .HasColumnName("id")
                .HasColumnType("uuid")
                .IsRequired();

            e.Property(x => x.Version)
                .HasColumnName("version")
                .HasColumnType("integer")
                .IsRequired();

            e.Property(x => x.Data)
                .HasColumnName("data")
                .HasColumnType("jsonb")
                .IsRequired();

            e.HasIndex(x => x.StreamId)
                .HasDatabaseName("idx_event_stream_id");

            e.HasIndex(x => new { x.StreamId, x.Version })
                .IsUnique()
                .HasDatabaseName("idx_event_stream_id_version");

            e.HasIndex(x => new { x.StreamId, x.Id })
                .IsUnique()
                .HasDatabaseName("idx_event_stream_id_id");

        });
    }
}
