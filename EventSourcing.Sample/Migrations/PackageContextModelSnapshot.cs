﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace EventSouring.Sample.Migrations
{
    [DbContext(typeof(PackageContext))]
    partial class PackageContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Event", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("Id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<string>("Payload")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("Payload");

                    b.Property<DateTime?>("Published")
                        .HasColumnType("datetime2")
                        .HasColumnName("Published");

                    b.Property<Guid>("StreamId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("StreamId");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("datetime2")
                        .HasColumnName("Timestamp");

                    b.Property<int>("Version")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(0)
                        .HasColumnName("Version");

                    b.HasKey("Id")
                        .HasName("PK_Event");

                    b.HasIndex("Published")
                        .HasDatabaseName("IX_Event_Published");

                    b.HasIndex("StreamId")
                        .HasDatabaseName("IX_Event_StreamId");

                    b.HasIndex("StreamId", "Timestamp")
                        .HasDatabaseName("IX_Event_StreamId_Timestamp");

                    b.HasIndex("StreamId", "Version")
                        .HasDatabaseName("IX_Event_StreamId_Version");

                    b.ToTable("Event", (string)null);
                });
#pragma warning restore 612, 618
        }
    }
}