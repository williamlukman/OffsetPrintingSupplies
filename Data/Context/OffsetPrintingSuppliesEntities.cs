using Core.DomainModel;
using Data.Mapping;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using Data.Migrations;
using System.Linq;
using System.Text;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace Data.Context
{
    public class OffsetPrintingSuppliesEntities : DbContext
    { 
        public OffsetPrintingSuppliesEntities() : base("OffsetPrintingTestClosing")
        {
           Database.SetInitializer<OffsetPrintingSuppliesEntities>(new MigrateDatabaseToLatestVersion<OffsetPrintingSuppliesEntities, Configuration>());
           //Database.SetInitializer<OffsetPrintingSuppliesEntities>(new DropCreateDatabaseIfModelChanges<OffsetPrintingSuppliesEntities>());
           this.Configuration.LazyLoadingEnabled = true; // automatically loads virtual when accessed
        }

        public void DeleteAllTables()
        {
            IList<String> tableNames = new List<String>();

            IList<String> userroleNames = new List<String>()
                                        { "UserMenu", "UserAccount", "UserAccess" };
            IList<String> accountingNames = new List<String>() { "VCNonBaseCurrency", "GLNonBaseCurrency", "PaymentRequestDetail", "PaymentRequest", "MemorialDetail", "Memorial", "ValidComb", "Closing", "GeneralLedgerJournal",
                                        "Account" };
            IList<String> manufacturingNames = new List<String>()
                                        { "RollerWarehouseMutationDetail", "RollerWarehouseMutation",
                                          "RecoveryAccessoryDetail", "RecoveryOrderDetail", "RecoveryOrder",
                                          "CoreAccessoryDetail", "CoreIdentificationDetail", "CoreIdentification",
                                          "BlanketOrderDetail", "BlanketOrder", "BlendingWorkOrder" };
            IList<String> purchaseOperationNames = new List<String>()
                                        { "PurchaseDownPaymentAllocationDetail", "PurchaseDownPaymentAllocation", "PaymentVoucherDetail",
                                          "PurchaseDownPayment", "PaymentVoucher",
                                          "PurchaseAllowanceDetail", "PurchaseAllowance", "Payable", "RetailPurchaseInvoiceDetail",
                                          "RetailPurchaseInvoice", "PurchaseInvoiceDetail", "PurchaseInvoice",
                                          "PurchaseReceivalDetail", "PurchaseReceival", "PurchaseOrderDetail", "PurchaseOrder" };
            IList<String> salesOperationNames = new List<String>()
                                        { "SalesDownPaymentAllocationDetail", "SalesDownPaymentAllocation", "ReceiptVoucherDetail",
                                          "SalesDownPayment", "ReceiptVoucher", "SalesAllowanceDetail", "SalesAllowance", "Receivable",
                                          "RetailSalesInvoiceDetail", "RetailSalesInvoice", "SalesInvoiceDetail", "SalesInvoice",
                                          "DeliveryOrderDetail", "TemporaryDeliveryOrderClearanceDetail", "TemporaryDeliveryOrderClearance", "TemporaryDeliveryOrderDetail", "TemporaryDeliveryOrder", "DeliveryOrder",
                                          "SalesOrderDetail", "SalesOrder", "SalesQuotationDetail", "SalesQuotation", 
                                          "VirtualOrderDetail", "VirtualOrder"};
            IList<String> financeNames = new List<String>() {
                                          "CashMutation", "CashBankAdjustment", "CashBankMutation", "CashBank" ,"ExchangeRate","Currency"};
            IList<String> stockAndMasterNames = new List<String>()
                                        { "PriceMutation", "CustomerStockMutation", "StockMutation", "CustomerItem", "WarehouseMutationDetail", "WarehouseMutation",
                                          "ServiceCost", "RollerBuilder", "CustomerStockAdjustmentDetail", "CustomerStockAdjustment", "StockAdjustmentDetail", "StockAdjustment",
                                          "WarehouseItem", "Warehouse", "Compound", "BlendingRecipeDetail", "BlendingRecipe", "Blanket", "CoreBuilder",
                                          "Item", "ItemType", "UoM", "Contact",
                                          "RollerType", "Machine", "Company" };

            userroleNames.ToList().ForEach(x => tableNames.Add(x));
            accountingNames.ToList().ForEach(x => tableNames.Add(x));
            manufacturingNames.ToList().ForEach(x => tableNames.Add(x));
            purchaseOperationNames.ToList().ForEach(x => tableNames.Add(x));
            salesOperationNames.ToList().ForEach(x => tableNames.Add(x));
            financeNames.ToList().ForEach(x => tableNames.Add(x));
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

            modelBuilder.Configurations.Add(new AccountMapping());
            modelBuilder.Configurations.Add(new BlanketMapping());
            modelBuilder.Configurations.Add(new BlanketOrderMapping());
            modelBuilder.Configurations.Add(new BlanketOrderDetailMapping());
            modelBuilder.Configurations.Add(new BlendingRecipeMapping());
            modelBuilder.Configurations.Add(new BlendingRecipeDetailMapping());
            modelBuilder.Configurations.Add(new BlendingWorkOrderMapping());
            modelBuilder.Configurations.Add(new CashBankMapping());
            modelBuilder.Configurations.Add(new CashBankAdjustmentMapping());
            modelBuilder.Configurations.Add(new CashBankMutationMapping());
            modelBuilder.Configurations.Add(new CashMutationMapping());
            modelBuilder.Configurations.Add(new ClosingMapping());
            modelBuilder.Configurations.Add(new CompanyMapping());
            modelBuilder.Configurations.Add(new CompoundMapping());
            modelBuilder.Configurations.Add(new ContactMapping());
            modelBuilder.Configurations.Add(new CoreBuilderMapping());
            modelBuilder.Configurations.Add(new CoreIdentificationMapping());
            modelBuilder.Configurations.Add(new CoreAccessoryDetailMapping());
            modelBuilder.Configurations.Add(new CoreIdentificationDetailMapping());
            modelBuilder.Configurations.Add(new CustomerItemMapping());
            modelBuilder.Configurations.Add(new CustomerStockAdjustmentMapping());
            modelBuilder.Configurations.Add(new CustomerStockAdjustmentDetailMapping());
            modelBuilder.Configurations.Add(new CustomerStockMutationMapping());
            modelBuilder.Configurations.Add(new DeliveryOrderMapping());
            modelBuilder.Configurations.Add(new DeliveryOrderDetailMapping());
            modelBuilder.Configurations.Add(new GeneralLedgerJournalMapping());
            modelBuilder.Configurations.Add(new ItemMapping());
            modelBuilder.Configurations.Add(new ItemTypeMapping());
            modelBuilder.Configurations.Add(new MachineMapping());
            modelBuilder.Configurations.Add(new MemorialMapping());
            modelBuilder.Configurations.Add(new MemorialDetailMapping());
            modelBuilder.Configurations.Add(new PurchaseDownPaymentAllocationDetailMapping());
            modelBuilder.Configurations.Add(new PurchaseDownPaymentAllocationMapping());
            modelBuilder.Configurations.Add(new PurchaseDownPaymentMapping());
            modelBuilder.Configurations.Add(new PurchaseAllowanceDetailMapping());
            modelBuilder.Configurations.Add(new PurchaseAllowanceMapping());
            modelBuilder.Configurations.Add(new PayableMapping());
            modelBuilder.Configurations.Add(new PaymentRequestMapping());
            modelBuilder.Configurations.Add(new PaymentRequestDetailMapping());
            modelBuilder.Configurations.Add(new PaymentVoucherDetailMapping());
            modelBuilder.Configurations.Add(new PaymentVoucherMapping());
            modelBuilder.Configurations.Add(new PriceMutationMapping());
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
            modelBuilder.Configurations.Add(new RetailPurchaseInvoiceMapping());
            modelBuilder.Configurations.Add(new RetailPurchaseInvoiceDetailMapping());
            modelBuilder.Configurations.Add(new RetailSalesInvoiceMapping());
            modelBuilder.Configurations.Add(new RetailSalesInvoiceDetailMapping());
            modelBuilder.Configurations.Add(new RollerBuilderMapping());
            modelBuilder.Configurations.Add(new RollerTypeMapping());
            modelBuilder.Configurations.Add(new RollerWarehouseMutationMapping());
            modelBuilder.Configurations.Add(new RollerWarehouseMutationDetailMapping());
            modelBuilder.Configurations.Add(new SalesDownPaymentAllocationMapping());
            modelBuilder.Configurations.Add(new SalesDownPaymentAllocationDetailMapping());
            modelBuilder.Configurations.Add(new SalesDownPaymentMapping());
            modelBuilder.Configurations.Add(new SalesAllowanceDetailMapping());
            modelBuilder.Configurations.Add(new SalesAllowanceMapping());
            modelBuilder.Configurations.Add(new SalesInvoiceDetailMapping());
            modelBuilder.Configurations.Add(new SalesInvoiceMapping());
            modelBuilder.Configurations.Add(new SalesOrderMapping());
            modelBuilder.Configurations.Add(new SalesOrderDetailMapping());
            modelBuilder.Configurations.Add(new SalesQuotationDetailMapping());
            modelBuilder.Configurations.Add(new SalesQuotationMapping());
            modelBuilder.Configurations.Add(new ServiceCostMapping());
            modelBuilder.Configurations.Add(new StockAdjustmentMapping());
            modelBuilder.Configurations.Add(new StockAdjustmentDetailMapping());
            modelBuilder.Configurations.Add(new StockMutationMapping());
            modelBuilder.Configurations.Add(new TemporaryDeliveryOrderMapping());
            modelBuilder.Configurations.Add(new TemporaryDeliveryOrderDetailMapping());
            modelBuilder.Configurations.Add(new TemporaryDeliveryOrderClearanceMapping());
            modelBuilder.Configurations.Add(new TemporaryDeliveryOrderClearanceDetailMapping());
            modelBuilder.Configurations.Add(new UoMMapping());
            modelBuilder.Configurations.Add(new UserAccountMapping());
            modelBuilder.Configurations.Add(new UserMenuMapping());
            modelBuilder.Configurations.Add(new UserAccessMapping());
            modelBuilder.Configurations.Add(new ValidCombMapping());
            modelBuilder.Configurations.Add(new VirtualOrderMapping());
            modelBuilder.Configurations.Add(new VirtualOrderDetailMapping());
            modelBuilder.Configurations.Add(new WarehouseMapping());
            modelBuilder.Configurations.Add(new WarehouseItemMapping());
            modelBuilder.Configurations.Add(new WarehouseMutationMapping());
            modelBuilder.Configurations.Add(new WarehouseMutationDetailMapping());
            modelBuilder.Configurations.Add(new CurrencyMapping());
            modelBuilder.Configurations.Add(new ExchangeRateMapping());
            modelBuilder.Configurations.Add(new VCNonBaseCurrencyMapping());
            modelBuilder.Configurations.Add(new GLNonBaseCurrencyMapping());

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<Blanket> Blankets { get; set; }
        public DbSet<BlanketOrder> BlanketOrders { get; set; }
        public DbSet<BlanketOrderDetail> BlanketOrderDetails { get; set; }
        public DbSet<BlendingRecipe> BlendingRecipes { get; set; }
        public DbSet<BlendingRecipeDetail> BlendingRecipeDetails { get; set; }
        public DbSet<BlendingWorkOrder> BlendingWorkOrders { get; set; }
        public DbSet<CashBank> CashBanks { get; set; }
        public DbSet<CashBankAdjustment> CashBankAdjustments { get; set; }
        public DbSet<CashBankMutation> CashBankMutations { get; set; }
        public DbSet<CashMutation> CashMutations { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Compound> Compounds { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<CoreBuilder> CoreBuilders { get; set; }
        public DbSet<CoreIdentification> CoreIdentifications { get; set; }
        public DbSet<CoreAccessoryDetail> CoreAccessoryDetails { get; set; }
        public DbSet<CoreIdentificationDetail> CoreIdentificationDetails { get; set; }
        public DbSet<CustomerItem> CustomerItems { get; set; }
        public DbSet<CustomerStockAdjustment> CustomerStockAdjustments { get; set; }
        public DbSet<CustomerStockAdjustmentDetail> CustomerStockAdjustmentDetails { get; set; }
        public DbSet<CustomerStockMutation> CustomerStockMutations { get; set; }
        public DbSet<DeliveryOrderDetail> DeliveryOrderDetails { get; set; }
        public DbSet<DeliveryOrder> DeliveryOrders { get; set; }
        public DbSet<GeneralLedgerJournal> GeneralLedgerJournals { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<ItemType> ItemTypes { get; set; }
        public DbSet<Machine> Machines { get; set; }
        public DbSet<Memorial> Memorials { get; set; }
        public DbSet<MemorialDetail> MemorialDetails { get; set; }
        public DbSet<Payable> Payables { get; set; }
        public DbSet<PaymentRequest> PaymentRequests { get; set; }
        public DbSet<PaymentVoucherDetail> PaymentVoucherDetails { get; set; }
        public DbSet<PaymentVoucher> PaymentVouchers { get; set; }
        public DbSet<PurchaseDownPaymentAllocationDetail> PurchaseDownPaymentAllocationDetails { get; set; }
        public DbSet<PurchaseDownPaymentAllocation> PurchaseDownPaymentAllocations { get; set; }
        public DbSet<PurchaseDownPayment> PurchaseDownPayments { get; set; }
        public DbSet<PurchaseAllowanceDetail> PurchaseAllowanceDetails { get; set; }
        public DbSet<PurchaseAllowance> PurchaseAllowances { get; set; }
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
        public DbSet<SalesDownPaymentAllocationDetail> SalesDownPaymentAllocationDetails { get; set; }
        public DbSet<SalesDownPaymentAllocation> SalesDownPaymentAllocations { get; set; }
        public DbSet<SalesDownPayment> SalesDownPayments { get; set; }
        public DbSet<SalesAllowanceDetail> SalesAllowanceDetails { get; set; }
        public DbSet<SalesAllowance> SalesAllowances { get; set; }
        public DbSet<SalesInvoiceDetail> SalesInvoiceDetails { get; set; }
        public DbSet<SalesInvoice> SalesInvoices { get; set; }
        public DbSet<SalesOrderDetail> SalesOrderDetails { get; set; }
        public DbSet<SalesOrder> SalesOrders { get; set; }
        public DbSet<SalesQuotationDetail> SalesQuotationDetails { get; set; }
        public DbSet<SalesQuotation> SalesQuotations { get; set; }
        public DbSet<ServiceCost> ServiceCosts { get; set; }
        public DbSet<StockAdjustment> StockAdjustments { get; set; }
        public DbSet<StockAdjustmentDetail> StockAdjustmentDetails { get; set; }
        public DbSet<StockMutation> StockMutations { get; set; }
        public DbSet<TemporaryDeliveryOrder> TemporaryDeliveryOrders { get; set; }
        public DbSet<TemporaryDeliveryOrderDetail> TemporaryDeliveryOrderDetails { get; set; }
        public DbSet<TemporaryDeliveryOrderClearance> TemporaryDeliveryOrderClearances { get; set; }
        public DbSet<TemporaryDeliveryOrderClearanceDetail> TemporaryDeliveryOrderClearanceDetails { get; set; }
        public DbSet<UoM> UoMs { get; set; }
        public DbSet<UserAccount> UserAccounts { get; set; }
        public DbSet<UserMenu> UserMenus { get; set; }
        public DbSet<UserAccess> UserAccesses { get; set; }
        public DbSet<ValidComb> ValidCombs { get; set; }
        public DbSet<VirtualOrder> VirtualOrders { get; set; }
        public DbSet<VirtualOrderDetail> VirtualOrderDetails { get; set; }
        public DbSet<Warehouse> Warehouses { get; set; }
        public DbSet<WarehouseItem> WarehouseItems { get; set; }
        public DbSet<WarehouseMutation> WarehouseMutations { get; set; }
        public DbSet<WarehouseMutationDetail> WarehouseMutationDetails { get; set; }
        public DbSet<PriceMutation> PriceMutations { get; set; }
        public DbSet<RetailPurchaseInvoice> RetailPurchaseInvoices { get; set; }
        public DbSet<RetailPurchaseInvoiceDetail> RetailPurchaseInvoiceDetails { get; set; }
        public DbSet<RetailSalesInvoice> RetailSalesInvoices { get; set; }
        public DbSet<RetailSalesInvoiceDetail> RetailSalesInvoiceDetails { get; set; }
        public DbSet<Currency> Currencys { get; set; } 
        public DbSet<ExchangeRate> ExchangeRates { get; set; }
        public DbSet<GLNonBaseCurrency> GLNonBaseCurrencys { get; set; }
        public DbSet<VCNonBaseCurrency> VCNonBaseCurrencys { get; set; } 
         
    }
}