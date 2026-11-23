
using System;
using Egomet.Core.Messaging.Outbox;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Egomet.Infrastructure.Persistence.Configurations;

public sealed class EventMessageConfiguration
    : IEntityTypeConfiguration<EventMessage>
{
    public void Configure(EntityTypeBuilder<EventMessage> builder)
    {
        builder.ToTable("Events");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .HasColumnName("Id")
            .ValueGeneratedNever();

        builder.Property(p => p.Type)
            .HasColumnName("Type")
            .IsRequired();

        builder.Property(p => p.Payload)
            .HasColumnType("jsonb")
            .HasColumnName("Payload")
            .IsRequired();

        builder.Property(p => p.Timestamp)
            .HasColumnName("Timestamp")
            .IsRequired();

        builder.Property(p => p.Published)
            .HasColumnName("Published")
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(p => p.PublishedAt)
            .HasColumnName("PublishedAt");

        builder.Property(p => p.CorrelationId)
            .HasColumnName("CorrelationId")
            .IsRequired();

        builder.HasIndex(p => new
        {
            p.Published,
            p.Timestamp
        })
        .HasDatabaseName("IX_Events_Published_Timestamp");
    }
}
