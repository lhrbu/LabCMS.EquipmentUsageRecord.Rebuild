using System;
using LabCMS.EquipmentUsageRecord.MachineDown.Models;
using LabCMS.EquipmentUsageRecord.Shared.Utils;
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
            modelBuilder.Entity<NotifiedToken>()
                .Property(item=>item.NotifiedDate)
                .HasConversion(EFCoreValueConverters.DataTimeOffsetUtcSecondsConverter);

            modelBuilder.Entity<MachineDownRecord>()
                .Property(item=>item.MachineDownDate)
                .HasConversion(EFCoreValueConverters.DataTimeOffsetUtcSecondsConverter);
            modelBuilder.Entity<MachineDownRecord>()
                .Property(item=>item.MachineRepairedDate)
                .HasConversion(EFCoreValueConverters.NullableDateTimeOffsetUtcSecondsConverter);

            modelBuilder.Entity<User>()
                .HasData(
                    new User{UserId="liha52",Email="Raccoon.Li@Hella.com"},
                    new User{UserId="zhaofe10",Email="F.Zhao@Hella.com"});

        }
    }
}