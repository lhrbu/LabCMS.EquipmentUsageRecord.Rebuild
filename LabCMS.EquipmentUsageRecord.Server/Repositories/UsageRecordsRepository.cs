using LabCMS.EquipmentUsageRecord.Shared.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Npgsql;
using System.ComponentModel.DataAnnotations.Schema;

namespace LabCMS.EquipmentUsageRecord.Server.Repositories
{
    public class UsageRecordsRepository:DbContext
    {
        public UsageRecordsRepository(DbContextOptions<UsageRecordsRepository> options)
            : base(options) { }
        public DbSet<Project> Projects => Set<Project>();
        public DbSet<EquipmentHourlyRate> EquipmentHourlyRates => Set<EquipmentHourlyRate>();
        public DbSet<UsageRecord> UsageRecords => Set<UsageRecord>();
        //public DbSet<UsageRecord> UsageRecordsRecycleBin => Set<UsageRecord>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UsageRecord>()
                .HasIndex(item => item.ProjectId);
            modelBuilder.Entity<UsageRecord>()
                .HasIndex(item => item.EquipmentNo)
                .IncludeProperties(item=>item.TestType);
            modelBuilder.Entity<UsageRecord>()
                .HasIndex(item => item.StartTime);

            modelBuilder.Entity<Project>()
                .HasIndex(item => item.FullName);
        }
    }
}
