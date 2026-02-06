using a._PakClassified.WebApp.Entities.Entities.Locations;
using a._PakClassified.WebApp.Entities.Entities.PakClassified;
using a._PakClassified.WebApp.Entities.Entities.UserEntities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace a._PakClassified.WebApp.Entities.AppDbContext
{
    public class AppDBContext : DbContext
    {
        #region Locations
        public DbSet<Country> Countries { get; set; }
        public DbSet<Province> Provinces { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<CityArea> CityAreas { get; set; }
        #endregion

        #region Pak Classified
        public DbSet<Advertisement> Advertisements { get; set; }
        public DbSet<AdvertisementCategory> AdvertisementCategories { get; set; }
        public DbSet<AdvertisementSubCategory> AdvertisementSubCategories { get; set; }
        public DbSet<AdvertisementType> AdvertisementTypes { get; set; }
        public DbSet<AdvertisementStatus> AdvertisementStatuses { get; set; }
        public DbSet<AdvertisementTag> AdvertisementTags { get; set; }
        public DbSet<AdvertisementImage> AdvertisementImages { get; set; }
        #endregion

        #region User Entities
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        #endregion

        public AppDBContext() { }

        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options) { }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=PakClassified.WebApp;Integrated Security=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDBContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}