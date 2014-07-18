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
                                                            "BarringOrderDetail", "BarringOrder", "StockAdjustmentDetail", "StockAdjustment",
                                                            "StockMutation", "WarehouseItem", "Warehouse", 
                                                            "Barring", "CoreBuilder", "Item", "ItemType", "UoM", "Customer", "RollerType", "Machine",
                                                            };

            foreach (var tableName in tableNames)
            {
                Database.ExecuteSqlCommand(string.Format("DELETE FROM {0}", tableName));
            }
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Configurations.Add(new BarringMapping());
            modelBuilder.Configurations.Add(new BarringOrderMapping());
            modelBuilder.Configurations.Add(new BarringOrderDetailMapping());
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
            modelBuilder.Configurations.Add(new StockAdjustmentMapping());
            modelBuilder.Configurations.Add(new StockAdjustmentDetailMapping());
            modelBuilder.Configurations.Add(new StockMutationMapping());
            modelBuilder.Configurations.Add(new UoMMapping());
            modelBuilder.Configurations.Add(new WarehouseMapping());
            modelBuilder.Configurations.Add(new WarehouseItemMapping());
            modelBuilder.Configurations.Add(new WarehouseMutationOrderMapping());
            modelBuilder.Configurations.Add(new WarehouseMutationOrderDetailMapping());
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Barring> Barrings { get; set; }
        public DbSet<BarringOrder> BarringOrders { get; set; }
        public DbSet<BarringOrderDetail> BarringOrderDetails { get; set; }
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
        public DbSet<StockAdjustment> StockAdjustments { get; set; }
        public DbSet<StockAdjustmentDetail> StockAdjustmentDetails { get; set; }
        public DbSet<StockMutation> StockMutations { get; set; }
        public DbSet<UoM> UoMs { get; set; }
        public DbSet<Warehouse> Warehouses { get; set; }
        public DbSet<WarehouseItem> WarehouseItems { get; set; }
        public DbSet<WarehouseMutationOrder> WarehouseMutationOrders { get; set; }
        public DbSet<WarehouseMutationOrderDetail> WarehouseMutationOrderDetails { get; set; }
    }
}