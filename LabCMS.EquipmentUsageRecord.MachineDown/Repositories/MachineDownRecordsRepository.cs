using System;
using LabCMS.EquipmentUsageRecord.MachineDown.Models;
using Microsoft.EntityFrameworkCore;

namespace LabCMS.EquipmentUsageRecord.MachineDown.Repositories
{
    public class MachineDownRecordsRepository:DbContext
    {
        public DbSet<MachineDownRecord> MachineDownRecords => Set<MachineDownRecord>();
        public DbSet<User> Users => Set<User>();
        public DbSet<NotifiedToken> NotifiedTokens => Set<NotifiedToken>();
        public MachineDownRecordsRepository(DbContextOptions<MachineDownRecordsRepository> options)
            :base(options){}
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<NotifiedToken>()
                .HasIndex(item => item.NotifiedDate);
        }
    }
}