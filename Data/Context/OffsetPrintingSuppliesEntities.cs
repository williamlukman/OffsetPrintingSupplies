using Core.DomainModel;
using Data.Mapping;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;

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
            IList<String> tableNames = new List<String>();

            IList<String> financeNames = new List<String>()
                                        { "CashMutation", "CashBankAdjustment", "CashBankMutation", "CashBank"};
            IList<String> manufacturingNames = new List<String>()
                                        { "RollerWarehouseMutationDetail", "RollerWarehouseMutation",
                                          "RecoveryAccessoryDetail", "RecoveryOrderDetail", "RecoveryOrder",
                                          "CoreIdentificationDetail", "CoreIdentification",
                                          "BarringOrderDetail", "BarringOrder" };
            IList<String> purchaseOperationNames = new List<String>()
                                        { "PaymentVoucherDetail", "PaymentVoucher", "Payable", 
                                          "PurchaseInvoiceDetail", "PurchaseInvoice",
                                          "PurchaseReceivalDetail", "PurchaseReceival", "PurchaseOrderDetail", "PurchaseOrder" };
            IList<String> salesOperationNames = new List<String>()
                                        { "ReceiptVoucherDetail", "ReceiptVoucher", "Receivable",
                                          "SalesInvoiceDetail", "SalesInvoice",
                                          "DeliveryOrderDetail", "DeliveryOrder", "SalesOrderDetail", "SalesOrder"};
            IList<String> stockAndMasterNames = new List<String>()
                                        { "StockMutation", "WarehouseMutationOrderDetail", "WarehouseMutationOrder",
                                          "RollerBuilder", "StockAdjustmentDetail", "StockAdjustment", "WarehouseItem",
                                          "Warehouse", "Barring", "CoreBuilder", "Item", "ItemType", "UoM", "Contact",
                                          "RollerType", "Machine" };

            financeNames.ToList().ForEach(x => tableNames.Add(x));
            manufacturingNames.ToList().ForEach(x => tableNames.Add(x));
            purchaseOperationNames.ToList().ForEach(x => tableNames.Add(x));
            salesOperationNames.ToList().ForEach(x => tableNames.Add(x));
            stockAndMasterNames.ToList().ForEach(x => tableNames.Add(x));

            foreach (var tableName in tableNames)
            {
                Database.ExecuteSqlCommand(string.Format("DELETE FROM {0}", tableName));
            }
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

            modelBuilder.Configurations.Add(new BarringMapping());
            modelBuilder.Configurations.Add(new BarringOrderMapping());
            modelBuilder.Configurations.Add(new BarringOrderDetailMapping());
            modelBuilder.Configurations.Add(new CashBankMapping());
            modelBuilder.Configurations.Add(new CashBankAdjustmentMapping());
            modelBuilder.Configurations.Add(new CashBankMutationMapping());
            modelBuilder.Configurations.Add(new CashMutationMapping());
            modelBuilder.Configurations.Add(new CoreBuilderMapping());
            modelBuilder.Configurations.Add(new CoreIdentificationMapping());
            modelBuilder.Configurations.Add(new CoreIdentificationDetailMapping());
            modelBuilder.Configurations.Add(new DeliveryOrderMapping());
            modelBuilder.Configurations.Add(new DeliveryOrderDetailMapping());
            modelBuilder.Configurations.Add(new ContactMapping());
            modelBuilder.Configurations.Add(new ItemMapping());
            modelBuilder.Configurations.Add(new ItemTypeMapping());
            modelBuilder.Configurations.Add(new MachineMapping());
            modelBuilder.Configurations.Add(new PayableMapping());
            modelBuilder.Configurations.Add(new PaymentVoucherDetailMapping());
            modelBuilder.Configurations.Add(new PaymentVoucherMapping());
            modelBuilder.Configurations.Add(new PurchaseInvoiceDetailMapping());
            modelBuilder.Configurations.Add(new PurchaseInvoiceMapping());
            modelBuilder.Configurations.Add(new PurchaseOrderMapping());
            modelBuilder.Configurations.Add(new PurchaseOrderDetailMapping());
            modelBuilder.Configurations.Add(new PurchaseReceivalMapping());
            modelBuilder.Configurations.Add(new PurchaseReceivalDetailMapping());
            modelBuilder.Configurations.Add(new ReceivableMapping());
            modelBuilder.Configurations.Add(new ReceiptVoucherDetailMapping());
            modelBuilder.Configurations.Add(new ReceiptVoucherMapping());
            modelBuilder.Configurations.Add(new RecoveryAccessoryDetailMapping());
            modelBuilder.Configurations.Add(new RecoveryOrderMapping());
            modelBuilder.Configurations.Add(new RecoveryOrderDetailMapping());
            modelBuilder.Configurations.Add(new RollerBuilderMapping());
            modelBuilder.Configurations.Add(new RollerTypeMapping());
            modelBuilder.Configurations.Add(new RollerWarehouseMutationMapping());
            modelBuilder.Configurations.Add(new RollerWarehouseMutationDetailMapping());
            modelBuilder.Configurations.Add(new SalesInvoiceDetailMapping());
            modelBuilder.Configurations.Add(new SalesInvoiceMapping());
            modelBuilder.Configurations.Add(new SalesOrderMapping());
            modelBuilder.Configurations.Add(new SalesOrderDetailMapping());
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
        public DbSet<CashBank> CashBanks { get; set; }
        public DbSet<CashBankAdjustment> CashBankAdjustments { get; set; }
        public DbSet<CashBankMutation> CashBankMutations { get; set; }
        public DbSet<CashMutation> CashMutations { get; set; }
        public DbSet<CoreBuilder> CoreBuilders { get; set; }
        public DbSet<CoreIdentification> CoreIdentifications { get; set; }
        public DbSet<CoreIdentificationDetail> CoreIdentificationDetails { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<DeliveryOrderDetail> DeliveryOrderDetails { get; set; }
        public DbSet<DeliveryOrder> DeliveryOrders { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<ItemType> ItemTypes { get; set; }
        public DbSet<Machine> Machines { get; set; }
        public DbSet<Payable> Payables { get; set; }
        public DbSet<PaymentVoucherDetail> PaymentVoucherDetails { get; set; }
        public DbSet<PaymentVoucher> PaymentVouchers { get; set; }
        public DbSet<PurchaseInvoiceDetail> PurchaseInvoiceDetails { get; set; }
        public DbSet<PurchaseInvoice> PurchaseInvoices { get; set; }
        public DbSet<PurchaseOrderDetail> PurchaseOrderDetails { get; set; }
        public DbSet<PurchaseOrder> PurchaseOrders { get; set; }
        public DbSet<PurchaseReceivalDetail> PurchaseReceivalDetails { get; set; }
        public DbSet<PurchaseReceival> PurchaseReceivals { get; set; }
        public DbSet<Receivable> Receivables { get; set; }
        public DbSet<ReceiptVoucherDetail> ReceiptVoucherDetails { get; set; }
        public DbSet<ReceiptVoucher> ReceiptVouchers { get; set; }
        public DbSet<RecoveryAccessoryDetail> RecoveryAccessoryDetails { get; set; }
        public DbSet<RecoveryOrder> RecoveryOrders { get; set; }
        public DbSet<RecoveryOrderDetail> RecoveryOrderDetails { get; set; }
        public DbSet<RollerBuilder> RollerBuilders { get; set; }
        public DbSet<RollerType> RollerTypes { get; set; }
        public DbSet<RollerWarehouseMutationDetail> RollerWarehouseMutationDetails { get; set; }
        public DbSet<RollerWarehouseMutation> RollerWarehouseMutations { get; set; }
        public DbSet<SalesInvoiceDetail> SalesInvoiceDetails { get; set; }
        public DbSet<SalesInvoice> SalesInvoices { get; set; }
        public DbSet<SalesOrderDetail> SalesOrderDetails { get; set; }
        public DbSet<SalesOrder> SalesOrders { get; set; }
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