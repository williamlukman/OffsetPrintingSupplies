using Core.DomainModel;
using Data.Mapping;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Context
{
    public class OffsetPrintingSuppliesEntities : DbContext
    {
        public OffsetPrintingSuppliesEntities()
        {
            Database.SetInitializer<OffsetPrintingSuppliesEntities>(new DropCreateDatabaseAlways<OffsetPrintingSuppliesEntities>());
        }

        public void DeleteAllTables()
        {
            IList<String> tableNames = new List<String>() { "RecoveryAccessoryDetail", "RecoveryOrderDetail", "RecoveryOrder",
                                                            "CoreIdentificationDetail", "CoreIdentification", "RollerBuilder",
                                                            "CoreBuilder", "Item", "ItemType", "Customer", "RollerType", "Machine"};

            foreach (var tableName in tableNames)
            {
                Database.ExecuteSqlCommand(string.Format("DELETE FROM {0}", tableName));
            }
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Configurations.Add(new CoreBuilderMapping());
            modelBuilder.Configurations.Add(new CoreIdentificationMapping());
            modelBuilder.Configurations.Add(new CoreIdentificationDetailMapping());
            modelBuilder.Configurations.Add(new CustomerMapping());
            modelBuilder.Configurations.Add(new ItemMapping());
            modelBuilder.Configurations.Add(new ItemTypeMapping());
            modelBuilder.Configurations.Add(new MachineMapping());
            modelBuilder.Configurations.Add(new RecoveryAccessoryDetailMapping());
            modelBuilder.Configurations.Add(new RecoveryOrderMapping());
            modelBuilder.Configurations.Add(new RecoveryOrderDetailMapping());
            modelBuilder.Configurations.Add(new RollerBuilderMapping());
            modelBuilder.Configurations.Add(new RollerTypeMapping());

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<CoreBuilder> CoreBuilders { get; set; }
        public DbSet<CoreIdentification> CoreIdentifications { get; set; }
        public DbSet<CoreIdentificationDetail> CoreIdentificationDetails { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<ItemType> ItemTypes { get; set; }
        public DbSet<Machine> Machines { get; set; }
        public DbSet<RecoveryAccessoryDetail> RecoveryAccessoryDetails { get; set; }
        public DbSet<RecoveryOrder> RecoveryOrders { get; set; }
        public DbSet<RecoveryOrderDetail> RecoveryOrderDetails { get; set; }
        public DbSet<RollerBuilder> RollerBuilders { get; set; }
        public DbSet<RollerType> RollerTypes { get; set; }
    }
}