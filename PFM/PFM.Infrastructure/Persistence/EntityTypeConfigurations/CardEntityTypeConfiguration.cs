using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PFM.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PFM.Infrastructure.Persistence.EntityTypeConfigurations
{
    public class CardEntityTypeConfiguration : IEntityTypeConfiguration<Card>
    {
        public void Configure(EntityTypeBuilder<Card> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id)
                .HasColumnName("id")
                .IsRequired();

            builder.Property(c => c.OwnerName)
                .HasColumnName("owner_name")
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(c => c.CardNumber)
                .HasColumnName("card_number")
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(c => c.ExpirationDate)
                .HasColumnName("expiration_date")
                .IsRequired();

            builder.Property(c => c.AvailableAmount)
                .HasColumnName("available_amount")
                .HasColumnType("decimal(20,2)")
                .IsRequired();

            builder.Property(c => c.ReservedAmount)
                .HasColumnName("reserved_amount")
                .HasColumnType("decimal(20,2)")
                .IsRequired();

            builder.Property(c => c.UserId)
                .HasColumnName("user_id")
                .IsRequired();

            builder.Property(c => c.CardType)
                .HasColumnName("card_type")
                .IsRequired();

            builder.HasOne(c => c.User)
                .WithMany(u => u.Cards)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
