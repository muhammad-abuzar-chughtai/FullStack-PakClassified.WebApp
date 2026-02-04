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
    public class AdvertisementMap: IEntityTypeConfiguration<Advertisement>
    {
        public void Configure(EntityTypeBuilder<Advertisement> builder)
        {
            builder.ToTable("Advertisements");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Name)
                   .IsRequired()
                   .HasMaxLength(100);
            
            builder.Property(c => c.Title)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(c => c.Description)
                   .HasMaxLength(1000);

            builder.Property(c => c.Price)
                   .HasPrecision(18, 2);

            builder.Property(c => c.Likes)
                   .HasColumnType("int");

            builder.Property(c => c.StartsOn)
                  .IsRequired()
                  .HasColumnType("datetime2");

            builder.Property(c => c.EndsOn)
                  .IsRequired()
                  .HasColumnType("datetime2");

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

            builder.HasOne(p => p.CityArea)
                   .WithMany(c => c.Advertisements)
                   .HasForeignKey(p => p.CityAreaId)
                   .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(p => p.PostedBy)
                   .WithMany(c => c.Advertisements)
                   .HasForeignKey(p => p.PostedById)
                   .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(p => p.Status)
                   .WithMany(c => c.Advertisements)
                   .HasForeignKey(p => p.StatusId)
                   .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(p => p.Type)
                   .WithMany(c => c.Advertisements)
                   .HasForeignKey(p => p.TypeId)
                   .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(p => p.SubCategory)
                   .WithMany(c => c.Advertisements)
                   .HasForeignKey(p => p.SubCategoryId)
                   .OnDelete(DeleteBehavior.NoAction);

            builder.HasMany(s => s.Tags)
                   .WithMany(p => p.Advertisements)
                   .UsingEntity(j => j.ToTable("AdvertisementTagMapping"));


            builder.HasMany(s => s.Images)
                   .WithOne(p => p.Advertisement)
                   .HasForeignKey(p => p.AdvertisementId)
                   .OnDelete(DeleteBehavior.NoAction);

        }
    }
}
