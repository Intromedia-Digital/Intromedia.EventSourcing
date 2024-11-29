﻿// <auto-generated />
using System;
using EventSourcing.Postgres;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace EventSourcing.Postgres.Migrations
{
    [DbContext(typeof(EventDbContext))]
    [Migration("20241129212229_Init")]
    partial class Init
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("EventSourcing.Postgres.Event", b =>
                {
                    b.Property<long>("ClusterKey")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("cluster_key");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("ClusterKey"));

                    b.Property<IEvent>("Data")
                        .IsRequired()
                        .HasColumnType("jsonb")
                        .HasColumnName("data");

                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<Guid>("StreamId")
                        .HasColumnType("uuid")
                        .HasColumnName("stream_id");

                    b.Property<int>("Version")
                        .HasColumnType("integer")
                        .HasColumnName("version");

                    b.HasKey("ClusterKey")
                        .HasName("pk_event");

                    b.HasIndex("StreamId")
                        .HasDatabaseName("idx_event_stream_id");

                    b.HasIndex("StreamId", "Id")
                        .IsUnique()
                        .HasDatabaseName("idx_event_stream_id_id");

                    b.HasIndex("StreamId", "Version")
                        .IsUnique()
                        .HasDatabaseName("idx_event_stream_id_version");

                    b.ToTable("event", (string)null);
                });
#pragma warning restore 612, 618
        }
    }
}
