using System;
using LabCMS.EquipmentUsageRecord.MachineDown.Models;
using Microsoft.EntityFrameworkCore;

namespace LabCMS.EquipmentUsageRecord.MachineDown.Repositories
{
    public class MachineDownRecordsRepository:DbContext
    {
        public DbSet<MachineDownRecord> MachineDownRecords => Set<MachineDownRecord>();
        public DbSet<User> Users => Set<User>();
        public MachineDownRecordsRepository(DbContextOptions<MachineDownRecordsRepository> options)
            :base(options){}
    }
}