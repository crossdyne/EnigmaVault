using EnigmaVault.Password.Service.Domain.Models;
using EnigmaVault.Password.Service.Domain.ValueObjects.Password;
using EnigmaVault.Password.Service.Domain.ValueObjects.Tag;
using EnigmaVault.Password.Service.Domain.ValueObjects.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EnigmaVault.Password.Service.Infrastructure.Persistence.Configurations
{
    internal sealed class VaultItemConfiguration : IEntityTypeConfiguration<VaultItem>
    {
        public void Configure(EntityTypeBuilder<VaultItem> builder)
        {
            builder.ToTable("VaultItems");
            builder.HasKey(vi => vi.Id);

            builder.Property(vi => vi.Id)
                .HasColumnName("Id")
                .HasConversion(
                    id => id.Value,
                    value => VaultItemId.Create(value))
                .IsRequired();

            builder.Property(vi => vi.UserId)
                .HasColumnName("UserId")
                .HasConversion(
                    id => id.Value,
                    value => UserId.Create(value))
                .IsRequired();

            builder.Property(vi => vi.PasswordType)
                .HasColumnName("PasswordType")
                .HasConversion<string>()
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(vi => vi.EncryptedOverview)
                .HasColumnName("EncryptedOverview")
                .HasConversion(
                    data => (byte[])data,
                    dbValue => EncryptedData.Create(dbValue))
                .HasMaxLength(EncryptedData.MAX_LENGTH)
                .IsRequired()
                .Metadata.SetValueComparer(new ValueComparer<EncryptedData>(
                    (c1, c2) => Enumerable.SequenceEqual(c1.Value, c2.Value),
                    c => c.Value.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                    c => c));

            builder.Property(vi => vi.EncryptedDetails)
                .HasColumnName("EncryptedDetails")
                .HasConversion(
                  data => (byte[])data,
                  dbValue => EncryptedData.Create(dbValue))
                .HasMaxLength(EncryptedData.MAX_LENGTH)
                .IsRequired()
                .Metadata.SetValueComparer(new ValueComparer<EncryptedData>(
                    (c1, c2) => Enumerable.SequenceEqual(c1.Value, c2.Value),
                    c => c.Value.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                    c => c));

            builder.Property(vi => vi.IsFavorite)
                .HasColumnName("IsFavorite")
                .IsRequired();

            builder.Property(vi => vi.IsArchive)
                .HasColumnName("IsArchive")
                .IsRequired();

            builder.Property(vi => vi.IsInTrash)
                .HasColumnName("IsInTrash")
                .IsRequired();

            builder.Property(vi => vi.DeletedAt)
                .HasColumnName("DeletedAt")
                .IsRequired(false);

            builder.Property(vi => vi.DateAdded)
                .HasColumnName("DateAdded")
                .IsRequired();

            builder.Property(vi => vi.DateUpdated)
                .HasColumnName("DateUpdated")
                .IsRequired(false);

            builder.Property(vi => vi.IconId)
                .HasColumnName("IconId")
                .HasConversion(iconId => iconId.Value, db => IconId.Create(db))
                .IsRequired();

            builder.Property(vi => vi.Tags)
                .HasColumnName("TagsIds")
                .HasColumnType("uuid[]")
                .HasField("_tags")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasConversion(
                    tags => tags.Select(t => t.Value).ToArray(),
                    ids => (ids ?? Array.Empty<Guid>()).Select(id => TagId.Create(id)).ToList())
                 .Metadata.SetValueComparer(new ValueComparer<IReadOnlyCollection<TagId>>(
                     (c1, c2) => c1!.SequenceEqual(c2!),
                     c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.Value.GetHashCode())),
                     c => c.ToList()
            ));
        }
    }
}