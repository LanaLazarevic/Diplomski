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
    public class AccountEntityTypeConfiguration : IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> builder)
        {
            builder.HasKey(a => a.Id);

            builder.Property(a => a.Id)
                .HasColumnName("id")
                .IsRequired();

            builder.Property(a => a.AccountNumber)
                .HasColumnName("account_number")
                .IsRequired();

            builder.Property(a => a.ReservedAmount)
                .HasColumnName("reserved_amount")
                .HasColumnType("decimal(20,2)")
                .IsRequired();

            builder.Property(a => a.AvailableAmount)
                .HasColumnName("available_amount")
                .HasColumnType("decimal(20,2)")
                .IsRequired();

            builder.Property(a => a.Currency)
                .HasColumnName("currency")
                .HasMaxLength(3)
                .IsRequired();

            builder.Property(a => a.AccountType)
                .HasColumnName("account_type")
                .IsRequired();

            builder.Property(a => a.IsActive)
                .HasColumnName("is_active")
                .IsRequired();

            builder.Property(a => a.UserId)
                .HasColumnName("user_id")
                .IsRequired();

            builder.HasOne(a => a.User)
                .WithMany(u => u.Accounts)
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(a => a.Cards)
                .WithOne(c => c.Account)
                .HasForeignKey(c => c.AccountId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
