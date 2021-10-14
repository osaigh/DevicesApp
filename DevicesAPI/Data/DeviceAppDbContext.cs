using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevicesAPI.Models.DAOs;
using Microsoft.EntityFrameworkCore;

namespace DevicesAPI.Data
{
    public class DeviceAppDbContext : DbContext
    {
        #region Fields

        #endregion

        #region Properties
        public DbSet<Device> Devices { get; set; }
        public DbSet<DeviceUsage> DeviceUsages { get; set; }
        public DbSet<Address> Addresses { get; set; }

        #endregion

        #region Constructor
        public DeviceAppDbContext(DbContextOptions<DeviceAppDbContext> options) : base(options)
        {

        }
        #endregion

        #region Methods

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Address
            modelBuilder.Entity<Address>()
                        .ToTable("Addresses")
                        .HasKey(a => a.Id);

            modelBuilder.Entity<Address>()
                        .HasOne(a => a.Device)
                        .WithOne(d => d.Address)
                        .HasForeignKey<Device>(d => d.AddressId);

            //Devices
            modelBuilder.Entity<Device>()
                        .ToTable("Devices")
                        .HasKey(d => d.Id);


            //DeviceUsage
            modelBuilder.Entity<DeviceUsage>()
                        .ToTable("DeviceUsages")
                        .HasKey(d => d.Id);

            modelBuilder.Entity<DeviceUsage>()
                        .HasOne(d => d.Device)
                        .WithMany(d => d.DeviceUsages)
                        .HasForeignKey(d => d.DeviceId);
        }

        #endregion
    }
}
