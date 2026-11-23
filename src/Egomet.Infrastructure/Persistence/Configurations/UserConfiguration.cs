
using System;
using Egomet.Core.Aggregates.UserAggregate;
using Egomet.Core.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Egomet.Infrastructure.Persistence.Configurations;

public sealed class UserConfiguration
    : IEntityTypeConfiguration<UserAggregate>
{
    public void Configure(EntityTypeBuilder<UserAggregate> builder)
    {
        builder.ToTable("Users");

        builder.HasKey(p => p.Id);
        builder.HasIndex(p => p.Email).IsUnique();

        builder.Property(p => p.Id)
            .HasColumnName("Id")
            .HasConversion(
                identity => identity.Value,
                value => IdentityValueObject.From(value)
            )
            .ValueGeneratedNever();

        builder.Property(p => p.Email)
            .HasColumnType("citext")
            .HasColumnName("Email")
            .HasConversion(
                email => email.Value,
                value => EmailValueObject.Parse(value)
            )
            .IsRequired();

        builder.Property(p => p.PasswordHash)
            .HasColumnName("PasswordHash")
            .HasConversion(
                passwordHash => passwordHash.Value,
                value => PasswordHashValueObject.Parse(value)
            )
            .IsRequired();

        builder.OwnsOne(p => p.FullName, owned =>
        {
            owned.Property(p => p.FirstName)
                .HasColumnName("FirstName")
                .IsRequired();

            owned.Property(p => p.LastName)
                .HasColumnName("LastName")
                .IsRequired();
        });

        builder.Navigation(p => p.FullName)
            .IsRequired();

        builder.Property(p => p.Role)
            .HasColumnName("Role")
            .HasConversion(
                role => role.Value,
                value => RoleValueObject.From(value)
            )
            .IsRequired();

        builder.Property(p => p.Status)
            .HasColumnName("Status")
            .HasConversion(
                status => status.Value,
                value => StatusValueObject.From(value)
            )
            .IsRequired();

        builder.Ignore(p => p.Events);
    }
}
