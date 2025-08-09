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
    public class UserEntityTypeConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(u => u.Id);

            builder.Property(u => u.Id)
                .HasColumnName("id")
                .IsRequired();

            builder.Property(u => u.FirstName)
                .HasColumnName("first_name")
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(u => u.LastName)
                .HasColumnName("last_name")
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(u => u.Email)
                .HasColumnName("email")
                .HasMaxLength(200)
                .IsRequired();

            builder.HasIndex(u => u.Email)
                .IsUnique();

            builder.Property(u => u.Password)
                .HasColumnName("password")
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(u => u.Address)
                .HasColumnName("address")
                .HasMaxLength(500)
                .IsRequired(false);

            builder.Property(u => u.PhoneNumber)
                .HasColumnName("phone_number")
                .HasMaxLength(50)
                .IsRequired(false);

            builder.Property(u => u.Birthday)
                .HasColumnName("birthday")
                .HasColumnType("date")
                .IsRequired();
        }
    }
}
