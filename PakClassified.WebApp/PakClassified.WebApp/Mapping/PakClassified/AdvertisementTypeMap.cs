using a._PakClassified.WebApp.Entities.Entities.PakClassified;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace a._PakClassified.WebApp.Entities.Mapping.PakClassified
{
    public class AdvertisementTypeMap : IEntityTypeConfiguration<AdvertisementType>
    {
        public void Configure(EntityTypeBuilder<AdvertisementType> builder)
        {
            builder.ToTable("AdvertisementTypes");

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
        }
    }
}
