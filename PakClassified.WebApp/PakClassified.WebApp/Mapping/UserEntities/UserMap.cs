using a._PakClassified.WebApp.Entities.Entities.UserEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace a._PakClassified.WebApp.Entities.Mapping.UserEntities
{
    public class UserMap : IEntityTypeConfiguration<Entities.UserEntities.User>
    {
        public void Configure(EntityTypeBuilder<Entities.UserEntities.User> builder)
        {
            builder.ToTable("Users");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Name)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(c => c.Email)
                  .IsRequired()
                  .HasMaxLength(100);
            builder.HasIndex(c => c.Email)
                  .IsUnique();

            builder.Property(c => c.Password)
                  .IsRequired()
                  .HasMaxLength(100);

            builder.Property(c => c.ProfilePic)
                   .IsRequired()
                   .HasColumnType("varbinary(max)");

            builder.Property(u => u.ContactNo)
                .IsRequired()
                .HasColumnType("int")
                .HasMaxLength(15);

            builder.Property(c => c.DOB)
                   .IsRequired()
                   .HasColumnType("datetime2");

            builder.Property(u => u.SecQues)
                  .HasMaxLength(200);

            builder.Property(u => u.SecAns)
                   .HasMaxLength(200);

            builder.Property(c => c.CreatedBy)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(c => c.CreatedDate)
                   .IsRequired()
                   .HasColumnType("datetime2");

            builder.Property(c => c.LastModifiedBy)
                   .HasMaxLength(100);

            builder.Property(c => c.LastModifiedDate)
                   .HasColumnType("datetime2");

            builder.Property(c => c.IsActive)
                .IsRequired()
                .HasColumnType("bit");

            builder.HasOne(c => c.Role)
                   .WithMany(r => r.Users)
                   .HasForeignKey(c => c.RoleId)
                   .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
