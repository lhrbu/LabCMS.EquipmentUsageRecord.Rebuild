using LabCMS.EquipmentUsageRecord.Shared.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Npgsql;
using System.ComponentModel.DataAnnotations.Schema;
using LabCMS.EquipmentUsageRecord.Shared.Utils;
using System.IO;

namespace LabCMS.EquipmentUsageRecord.Shared.Repositories
{
    public class UsageRecordsRepository : DbContext
    {
        public UsageRecordsRepository(DbContextOptions<UsageRecordsRepository> options)
            : base(options) { }
        public DbSet<Project> Projects => Set<Project>();
        public DbSet<EquipmentHourlyRate> EquipmentHourlyRates => Set<EquipmentHourlyRate>();
        public DbSet<UsageRecord> UsageRecords => Set<UsageRecord>();
        public DbSet<ActiveProjectIndex> ActiveProjectIndices => Set<ActiveProjectIndex>();
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UsageRecord>()
                .HasIndex(item => item.ProjectNo);
            modelBuilder.Entity<UsageRecord>()
                .HasIndex(item => item.EquipmentNo)
                .IncludeProperties(item => item.TestType);
            modelBuilder.Entity<UsageRecord>()
                .HasIndex(item => item.StartTime);
            modelBuilder.Entity<UsageRecord>()
                .Property(item => item.StartTime)
                .HasConversion(EFCoreValueConverters.DataTimeOffsetUtcSecondsConverter);
            modelBuilder.Entity<UsageRecord>()
                .Property(item => item.EndTime)
                .HasConversion(EFCoreValueConverters.DataTimeOffsetUtcSecondsConverter);

            modelBuilder.Entity<Project>()
                .HasIndex(item => item.Name)
                .IsUnique();
        }

    }
}
