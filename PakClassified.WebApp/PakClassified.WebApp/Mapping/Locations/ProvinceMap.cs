using a._PakClassified.WebApp.Entities.Entities.Locations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace a._PakClassified.WebApp.Entities.Mapping.Locations
{
    public class ProvinceMap : IEntityTypeConfiguration<Province>

    {
        public void Configure(EntityTypeBuilder<Province> builder)
        {
            builder.ToTable("Provinces");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Name)
                   .IsRequired()
                   .HasMaxLength(100);

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

            builder.HasOne(p => p.Country)
                   .WithMany(c => c.Provinces)
                   .HasForeignKey(p => p.CountryId)
                   .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
