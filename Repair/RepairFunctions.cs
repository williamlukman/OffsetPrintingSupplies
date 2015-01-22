using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Text;
using System.IO;
using Core.Interface.Service;
using Service.Service;
using Data.Repository;
using Validation.Validation;
using Data.Context;
using System.Data.Entity;
using Core.DomainModel;
using System.Reflection;
using Core.Constants;

namespace Repair
{
    /// <summary>
    /// A static class for reflection type functions
    /// </summary>
    public static class Reflection
    {
        /// <summary>
        /// Extension for 'Object' that copies the properties to a destination object.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="destination">The destination.</param>
        public static void CopyProperties(this object source, object destination)
        {
            // If any this null throw an exception
            if (source == null || destination == null)
                throw new Exception("Source or/and Destination Objects are null");
            // Getting the Types of the objects
            Type typeDest = destination.GetType();
            Type typeSrc = source.GetType();
            // Collect all the valid properties to map
            var results = from srcProp in typeSrc.GetProperties()
                          let targetProperty = typeDest.GetProperty(srcProp.Name)
                          where srcProp.CanRead
                          && targetProperty != null
                          && (targetProperty.GetSetMethod(true) != null && !targetProperty.GetSetMethod(true).IsPrivate)
                          && (targetProperty.GetSetMethod().Attributes & MethodAttributes.Static) == 0
                          && targetProperty.PropertyType.IsAssignableFrom(srcProp.PropertyType)
                          select new { sourceProperty = srcProp, targetProperty = targetProperty };
            //map the properties
            foreach (var props in results)
            {
                props.targetProperty.SetValue(destination, props.sourceProperty.GetValue(source, null), null);
            }
        }
    }

    public class RepairFunctions
    {
        private static int logcount = 0;

        public IAccountService _accountService;
        public IBlanketService _blanketService;
        public IBlanketOrderService _blanketOrderService;
        public IBlanketOrderDetailService _blanketOrderDetailService;
        public IBlendingRecipeService _blendingRecipeService;
        public IBlendingRecipeDetailService _blendingRecipeDetailService;
        public IBlendingWorkOrderService _blendingWorkOrderService;
        public IRepackingService _repackingService;
        public ICashBankService _cashBankService;
        public ICashBankAdjustmentService _cashBankAdjustmentService;
        public ICashBankMutationService _cashBankMutationService;
        public ICashMutationService _cashMutationService;
        public IClosingService _closingService;
        public ICurrencyService _currencyService;
        public ICustomerItemService _customerItemService;
        public ICustomerStockAdjustmentDetailService _customerStockAdjustmentDetailService;
        public ICustomerStockAdjustmentService _customerStockAdjustmentService;
        public ICustomerStockMutationService _customerStockMutationService;
        public ICoreBuilderService _coreBuilderService;
        public ICoreIdentificationService _coreIdentificationService;
        public ICoreIdentificationDetailService _coreIdentificationDetailService;
        public IContactService _contactService;
        public IDeliveryOrderService _deliveryOrderService;
        public IDeliveryOrderDetailService _deliveryOrderDetailService;
        public IExchangeRateService _exchangeRateService;
        public IGeneralLedgerJournalService _generalLedgerJournalService;
        public IItemService _itemService;
        public IItemTypeService _itemTypeService;
        public IMachineService _machineService;
        public IPayableService _payableService;
        public IPaymentVoucherDetailService _paymentVoucherDetailService;
        public IPaymentVoucherService _paymentVoucherService;
        public IPriceMutationService _priceMutationService;
        public IPurchaseInvoiceDetailService _purchaseInvoiceDetailService;
        public IPurchaseInvoiceService _purchaseInvoiceService;
        public IPurchaseOrderService _purchaseOrderService;
        public IPurchaseOrderDetailService _purchaseOrderDetailService;
        public IPurchaseReceivalService _purchaseReceivalService;
        public IPurchaseReceivalDetailService _purchaseReceivalDetailService;
        public IReceivableService _receivableService;
        public IReceiptVoucherDetailService _receiptVoucherDetailService;
        public IReceiptVoucherService _receiptVoucherService;
        public IRecoveryAccessoryDetailService _recoveryAccessoryDetailService;
        public IRecoveryOrderDetailService _recoveryOrderDetailService;
        public IRecoveryOrderService _recoveryOrderService;
        public IRollerBuilderService _rollerBuilderService;
        public IRollerTypeService _rollerTypeService;
        public IRollerWarehouseMutationDetailService _rollerWarehouseMutationDetailService;
        public IRollerWarehouseMutationService _rollerWarehouseMutationService;
        public ISalesInvoiceDetailService _salesInvoiceDetailService;
        public ISalesInvoiceService _salesInvoiceService;
        public ISalesOrderService _salesOrderService;
        public ISalesOrderDetailService _salesOrderDetailService;
        public ISalesQuotationDetailService _salesQuotationDetailService;
        public ISalesQuotationService _salesQuotationService;
        public IServiceCostService _serviceCostService;
        public IStockAdjustmentDetailService _stockAdjustmentDetailService;
        public IStockAdjustmentService _stockAdjustmentService;
        public IStockMutationService _stockMutationService;
        public ITemporaryDeliveryOrderDetailService _temporaryDeliveryOrderDetailService;
        public ITemporaryDeliveryOrderService _temporaryDeliveryOrderService;
        public ITemporaryDeliveryOrderClearanceService _temporaryDeliveryOrderClearanceService;
        public ITemporaryDeliveryOrderClearanceDetailService _temporaryDeliveryOrderClearanceDetailService;
        public IUoMService _uomService;
        public IUserAccountService _userAccountService;
        public IUserMenuService _userMenuService;
        public IUserAccessService _userAccessService;
        public IValidCombService _validCombService;
        public IVirtualOrderDetailService _virtualOrderDetailService;
        public IVirtualOrderService _virtualOrderService;
        public IWarehouseItemService _warehouseItemService;
        public IWarehouseService _warehouseService;
        public IWarehouseMutationService _warehouseMutationService;
        public IWarehouseMutationDetailService _warehouseMutationDetailService;
        public IGLNonBaseCurrencyService _gLNonBaseCurrencyService;
        public IExchangeRateClosingService _exchangeRateClosingService;
        public IVCNonBaseCurrencyService _vCNonBaseCurrencyService;
        public ISalesInvoiceMigrationService _salesInvoiceMigrationService;
        public IPurchaseInvoiceMigrationService _purchaseInvoiceMigrationService;

        public struct sourceid {
                public string source;
                public int id;
            };

        public class StockMutationTranSource
        {
            public const string StockAdjustment = "stockadjustment";
            public const string CustomerStockAdjustment = "customerstockadjustment";
            public const string PurchaseOrder = "purchaseorder";
            public const string PurchaseReceival = "purchasereceival";
            public const string PurchaseInvoice = "purchaseinvoice";
            public const string Repacking = "repacking";
            public const string BlendingWorkOrder = "blendingworkorder";
            public const string CoreIdentification = "coreidentification";
            public const string RecoveryOrder = "recoveryorder";
            public const string RecoveryOrderDetail = "recoveryorderdetail";
            public const string BlanketOrder = "blanketorder";
            public const string BlanketOrderDetail = "blanketorderdetail";
            public const string RollerWarehouseMutation = "rollerwarehousemutation";
            public const string WarehouseMutation = "warehousemutation";
            public const string VirtualOrder = "virtualorder";
            public const string TemporaryDeliveryOrder = "temporarydeliveryorder";
            public const string TemporaryDeliveryOrderClearance = "temporarydeliveryorderclearance";
            public const string SalesOrder = "salesorder";
            public const string DeliveryOrder = "deliveryorder";
            public const string SalesInvoice = "salesinvoice";
        };

        public IList<String> stockMutationSources = new List<String>()
                                        {   
                                            "StockAdjustment",
                                            "CustomerStockAdjustment",
                                            "PurchaseOrder",
                                            "PurchaseReceival",
                                            "PurchaseInvoice",
                                            "Repacking", 
                                            "BlendingWorkOrder",
                                            "CoreIdentification",
                                            "RecoveryOrder", 
                                            "RecoveryOrderDetail", 
                                            "BlanketOrder",
                                            "BlanketOrderDetail",
                                            "RollerWarehouseMutation",
                                            "WarehouseMutation", 
                                            "VirtualOrder", 
                                            "TemporaryDeliveryOrder",
                                            "TemporaryDeliveryOrderClearance",
                                            "SalesOrder", 
                                            "DeliveryOrder", 
                                            "SalesInvoice", 
                                        };
        public IList<String> byproductDatas = new List<String>()
                                        {   
                                            //"CustomerStockMutation",
                                            //"StockMutation",
                                            //"GeneralLedgerJournal",
                                            //"GLNonBaseCurrency",
                                            "Receivable",
                                            "Payable", 
                                        };

        public class TranSource
        {
            public const string StockAdjustment = "stockadjustment";
            public const string WarehouseMutation = "warehousemutation";
            public const string CashBankAdjustment = "cashbankadjustment";
            public const string CashBankMutation = "cashbankmutation";
            public const string CustomPurchaseInvoice = "custompurchaseinvoice";
            public const string CashSalesInvoice = "cashsalesinvoice";
            public const string CashSalesReturn = "cashsalesreturn";
            public const string PaymentRequest = "paymentrequest";
            public const string ReceiptVoucher = "receiptvoucher";
            public const string PaymentVoucher = "paymentvoucher";
            public const string Memorial = "memorial";
        };

        public IList<String> userDatas = new List<String>()
                                        {   
                                            "MemorialDetail", "Memorial", "Account",
                                            "PaymentVoucherDetail", "PaymentVoucher",
                                            "ReceiptVoucherDetail", "ReceiptVoucher",
                                            "PaymentRequestDetail", "PaymentRequest",
                                            "CashBankAdjustment", "CashBankMutation", "CashBank",
                                            "CustomPurchaseInvoiceDetail", "CustomPurchaseInvoice",
                                            "CashSalesReturnDetail", "CashSalesReturn", 
                                            "CashSalesInvoiceDetail", "CashSalesInvoice",
                                            "WarehouseMutationDetail", "WarehouseMutation",
                                            "StockAdjustmentDetail", "StockAdjustment", "WarehouseItem", "Item",
                                            "Receivable", "Payable",
                                        };

        public IList<String> masterDatas = new List<String>()
                                        {
                                            "UserAccess", "UserMenu", "UserAccount",
                                            "GroupItemPrice", "QuantityPricing", "Warehouse", "ItemType", "UoM", "Contact", "ContactGroup", "Company",
                                        };

        public RepairFunctions()
        {
            _accountService = new AccountService(new AccountRepository(), new AccountValidator());
            _blanketService = new BlanketService(new BlanketRepository(), new BlanketValidator());
            _blanketOrderService = new BlanketOrderService(new BlanketOrderRepository(), new BlanketOrderValidator());
            _blanketOrderDetailService = new BlanketOrderDetailService(new BlanketOrderDetailRepository(), new BlanketOrderDetailValidator());
            _blendingRecipeService = new BlendingRecipeService(new BlendingRecipeRepository(), new BlendingRecipeValidator());
            _blendingRecipeDetailService = new BlendingRecipeDetailService(new BlendingRecipeDetailRepository(), new BlendingRecipeDetailValidator());
            _blendingWorkOrderService = new BlendingWorkOrderService(new BlendingWorkOrderRepository(), new BlendingWorkOrderValidator());
            _cashBankAdjustmentService = new CashBankAdjustmentService(new CashBankAdjustmentRepository(), new CashBankAdjustmentValidator());
            _cashBankMutationService = new CashBankMutationService(new CashBankMutationRepository(), new CashBankMutationValidator());
            _cashBankService = new CashBankService(new CashBankRepository(), new CashBankValidator());
            _cashMutationService = new CashMutationService(new CashMutationRepository(), new CashMutationValidator());
            _closingService = new ClosingService(new ClosingRepository(), new ClosingValidator());
            _coreBuilderService = new CoreBuilderService(new CoreBuilderRepository(), new CoreBuilderValidator());
            _coreIdentificationDetailService = new CoreIdentificationDetailService(new CoreIdentificationDetailRepository(), new CoreIdentificationDetailValidator());
            _coreIdentificationService = new CoreIdentificationService(new CoreIdentificationRepository(), new CoreIdentificationValidator());
            _contactService = new ContactService(new ContactRepository(), new ContactValidator());
            _currencyService = new CurrencyService(new CurrencyRepository(), new CurrencyValidator());
            _customerItemService = new CustomerItemService(new CustomerItemRepository(), new CustomerItemValidator());
            _customerStockAdjustmentDetailService = new CustomerStockAdjustmentDetailService(new CustomerStockAdjustmentDetailRepository(), new CustomerStockAdjustmentDetailValidator());
            _customerStockAdjustmentService = new CustomerStockAdjustmentService(new CustomerStockAdjustmentRepository(), new CustomerStockAdjustmentValidator());
            _customerStockMutationService = new CustomerStockMutationService(new CustomerStockMutationRepository(), new CustomerStockMutationValidator());
            _deliveryOrderService = new DeliveryOrderService(new DeliveryOrderRepository(), new DeliveryOrderValidator());
            _deliveryOrderDetailService = new DeliveryOrderDetailService(new DeliveryOrderDetailRepository(), new DeliveryOrderDetailValidator());
            _exchangeRateService = new ExchangeRateService(new ExchangeRateRepository(), new ExchangeRateValidator());
            _generalLedgerJournalService = new GeneralLedgerJournalService(new GeneralLedgerJournalRepository(), new GeneralLedgerJournalValidator());
            _itemService = new ItemService(new ItemRepository(), new ItemValidator());
            _itemTypeService = new ItemTypeService(new ItemTypeRepository(), new ItemTypeValidator());
            _machineService = new MachineService(new MachineRepository(), new MachineValidator());
            _payableService = new PayableService(new PayableRepository(), new PayableValidator());
            _paymentVoucherDetailService = new PaymentVoucherDetailService(new PaymentVoucherDetailRepository(), new PaymentVoucherDetailValidator());
            _paymentVoucherService = new PaymentVoucherService(new PaymentVoucherRepository(), new PaymentVoucherValidator());
            _priceMutationService = new PriceMutationService(new PriceMutationRepository(), new PriceMutationValidator());
            _purchaseInvoiceDetailService = new PurchaseInvoiceDetailService(new PurchaseInvoiceDetailRepository(), new PurchaseInvoiceDetailValidator());
            _purchaseInvoiceService = new PurchaseInvoiceService(new PurchaseInvoiceRepository(), new PurchaseInvoiceValidator());
            _purchaseOrderService = new PurchaseOrderService(new PurchaseOrderRepository(), new PurchaseOrderValidator());
            _purchaseOrderDetailService = new PurchaseOrderDetailService(new PurchaseOrderDetailRepository(), new PurchaseOrderDetailValidator());
            _purchaseReceivalService = new PurchaseReceivalService(new PurchaseReceivalRepository(), new PurchaseReceivalValidator());
            _purchaseReceivalDetailService = new PurchaseReceivalDetailService(new PurchaseReceivalDetailRepository(), new PurchaseReceivalDetailValidator());
            _receivableService = new ReceivableService(new ReceivableRepository(), new ReceivableValidator());
            _receiptVoucherDetailService = new ReceiptVoucherDetailService(new ReceiptVoucherDetailRepository(), new ReceiptVoucherDetailValidator());
            _receiptVoucherService = new ReceiptVoucherService(new ReceiptVoucherRepository(), new ReceiptVoucherValidator());
            _recoveryOrderDetailService = new RecoveryOrderDetailService(new RecoveryOrderDetailRepository(), new RecoveryOrderDetailValidator());
            _recoveryOrderService = new RecoveryOrderService(new RecoveryOrderRepository(), new RecoveryOrderValidator());
            _recoveryAccessoryDetailService = new RecoveryAccessoryDetailService(new RecoveryAccessoryDetailRepository(), new RecoveryAccessoryDetailValidator());
            _repackingService = new RepackingService(new RepackingRepository(), new RepackingValidator());
            _rollerBuilderService = new RollerBuilderService(new RollerBuilderRepository(), new RollerBuilderValidator());
            _rollerTypeService = new RollerTypeService(new RollerTypeRepository(), new RollerTypeValidator());
            _rollerWarehouseMutationDetailService = new RollerWarehouseMutationDetailService(new RollerWarehouseMutationDetailRepository(), new RollerWarehouseMutationDetailValidator());
            _rollerWarehouseMutationService = new RollerWarehouseMutationService(new RollerWarehouseMutationRepository(), new RollerWarehouseMutationValidator());
            _salesInvoiceDetailService = new SalesInvoiceDetailService(new SalesInvoiceDetailRepository(), new SalesInvoiceDetailValidator());
            _salesInvoiceService = new SalesInvoiceService(new SalesInvoiceRepository(), new SalesInvoiceValidator());
            _salesOrderService = new SalesOrderService(new SalesOrderRepository(), new SalesOrderValidator());
            _salesOrderDetailService = new SalesOrderDetailService(new SalesOrderDetailRepository(), new SalesOrderDetailValidator());
            _salesQuotationDetailService = new SalesQuotationDetailService(new SalesQuotationDetailRepository(), new SalesQuotationDetailValidator());
            _salesQuotationService = new SalesQuotationService(new SalesQuotationRepository(), new SalesQuotationValidator());
            _serviceCostService = new ServiceCostService(new ServiceCostRepository(), new ServiceCostValidator());
            _stockAdjustmentDetailService = new StockAdjustmentDetailService(new StockAdjustmentDetailRepository(), new StockAdjustmentDetailValidator());
            _stockAdjustmentService = new StockAdjustmentService(new StockAdjustmentRepository(), new StockAdjustmentValidator());
            _stockMutationService = new StockMutationService(new StockMutationRepository(), new StockMutationValidator());
            _temporaryDeliveryOrderDetailService = new TemporaryDeliveryOrderDetailService(new TemporaryDeliveryOrderDetailRepository(), new TemporaryDeliveryOrderDetailValidator());
            _temporaryDeliveryOrderService = new TemporaryDeliveryOrderService(new TemporaryDeliveryOrderRepository(), new TemporaryDeliveryOrderValidator());
            _temporaryDeliveryOrderClearanceDetailService = new TemporaryDeliveryOrderClearanceDetailService(new TemporaryDeliveryOrderClearanceDetailRepository(), new TemporaryDeliveryOrderClearanceDetailValidator());
            _temporaryDeliveryOrderClearanceService = new TemporaryDeliveryOrderClearanceService(new TemporaryDeliveryOrderClearanceRepository(), new TemporaryDeliveryOrderClearanceValidator());
            _userAccountService = new UserAccountService(new UserAccountRepository(), new UserAccountValidator());
            _userMenuService = new UserMenuService(new UserMenuRepository(), new UserMenuValidator());
            _userAccessService = new UserAccessService(new UserAccessRepository(), new UserAccessValidator());
            _validCombService = new ValidCombService(new ValidCombRepository(), new ValidCombValidator());
            _virtualOrderDetailService = new VirtualOrderDetailService(new VirtualOrderDetailRepository(), new VirtualOrderDetailValidator());
            _virtualOrderService = new VirtualOrderService(new VirtualOrderRepository(), new VirtualOrderValidator());
            _uomService = new UoMService(new UoMRepository(), new UoMValidator());
            _warehouseItemService = new WarehouseItemService(new WarehouseItemRepository(), new WarehouseItemValidator());
            _warehouseService = new WarehouseService(new WarehouseRepository(), new WarehouseValidator());
            _warehouseMutationService = new WarehouseMutationService(new WarehouseMutationRepository(), new WarehouseMutationValidator());
            _warehouseMutationDetailService = new WarehouseMutationDetailService(new WarehouseMutationDetailRepository(), new WarehouseMutationDetailValidator());
            _gLNonBaseCurrencyService = new GLNonBaseCurrencyService(new GLNonBaseCurrencyRepository(), new GLNonBaseCurrencyValidator());
            _exchangeRateClosingService = new ExchangeRateClosingService(new ExchangeRateClosingRepository(), new ExchangeRateClosingValidator());
            _vCNonBaseCurrencyService = new VCNonBaseCurrencyService(new VCNonBaseCurrencyRepository(), new VCNonBaseCurrencyValidator());
            _salesInvoiceMigrationService = new SalesInvoiceMigrationService(new SalesInvoiceMigrationRepository());
            _purchaseInvoiceMigrationService = new PurchaseInvoiceMigrationService(new PurchaseInvoiceMigrationRepository());
        }

        public void Log(IDictionary<string,string> Errors, string Table, string Name, int OldId, int NewId) 
        {
            //if (logcount >= 100)
            //{
            //    Console.Write("Press ENTER to Continue...............................................");
            //    Console.ReadLine();
            //    logcount = 0;
            //}
            if (Errors != null)
            {
                if (Errors.Any())
                {
                    foreach (var e in Errors)
                    {
                        Console.WriteLine("{0} {1} [{2}->{3}] : {4} {5}", Table, Name, OldId, NewId, e.Key, e.Value);
                        logcount++;
                    }
                }
                else if (OldId != NewId)
                {
                    Console.WriteLine("{0} {1} [{2}->{3}]", Table, Name, OldId, NewId);
                    logcount++;
                }
            }
        }

        public void FixLegacyAccounts()
        {
            //var Asset = _accountService.UpdateOrCreateLegacyObject(new Account() { Name = "Asset", Code = Constant.AccountCode.Asset, LegacyCode = Constant.AccountLegacyCode.Asset, Level = 1, Group = (int)Constant.AccountGroup.Asset, IsLegacy = true });
            //var CashBank = _accountService.UpdateOrCreateLegacyObject(new Account() { Name = "CashBank", Code = Constant.AccountCode.CashBank, LegacyCode = Constant.AccountLegacyCode.CashBank, Level = 2, Group = (int)Constant.AccountGroup.Asset, ParentId = Asset.Id, IsLegacy = true });
            //var CashBankCash = _accountService.UpdateOrCreateLegacyObject(new Account() { Name = "Cash", Code = Constant.AccountCode.CashBankCash, LegacyCode = Constant.AccountLegacyCode.CashBankCash, Level = 3, Group = (int)Constant.AccountGroup.Asset, ParentId = CashBank.Id, IsLegacy = true });
            //var CashBankBank = _accountService.UpdateOrCreateLegacyObject(new Account() { Name = "Bank", Code = Constant.AccountCode.CashBankBank, LegacyCode = Constant.AccountLegacyCode.CashBankBank, Level = 3, Group = (int)Constant.AccountGroup.Asset, ParentId = CashBank.Id, IsLegacy = true });
            //var AccountReceivable = _accountService.UpdateOrCreateLegacyObject(new Account() { Name = "Account Receivable", Code = Constant.AccountCode.AccountReceivable, LegacyCode = Constant.AccountLegacyCode.AccountReceivable, Level = 2, Group = (int)Constant.AccountGroup.Asset, ParentId = Asset.Id, IsLegacy = true });
            //var AccountReceivablePPNmasuk3 = _accountService.UpdateOrCreateLegacyObject(new Account() { Name = "Account Receivable (PPN masukan)", Code = Constant.AccountCode.AccountReceivablePPNmasukan3, LegacyCode = Constant.AccountLegacyCode.AccountReceivablePPNmasukan3, Level = 3, Group = (int)Constant.AccountGroup.Asset, ParentId = AccountReceivable.Id, IsLegacy = true });
            //var AccountReceivablePPNmasuk = _accountService.UpdateOrCreateLegacyObject(new Account() { Name = "Account Receivable (PPN masukan)", IsUsedBySystem = true, IsLeaf = true, Code = Constant.AccountCode.AccountReceivablePPNmasukan, LegacyCode = Constant.AccountLegacyCode.AccountReceivablePPNmasukan, Level = 4, Group = (int)Constant.AccountGroup.Asset, ParentId = AccountReceivablePPNmasuk3.Id, IsLegacy = true });
            //var AccountReceivableTrading3 = _accountService.UpdateOrCreateLegacyObject(new Account() { Name = "Account Receivable (Trading)", Code = Constant.AccountCode.AccountReceivableTrading3, LegacyCode = Constant.AccountLegacyCode.AccountReceivableTrading3, Level = 3, Group = (int)Constant.AccountGroup.Asset, ParentId = AccountReceivable.Id, IsLegacy = true });
            //var AccountReceivableTrading = _accountService.UpdateOrCreateLegacyObject(new Account() { Name = "Account Receivable (Trading)", IsUsedBySystem = true, IsLeaf = true, Code = Constant.AccountCode.AccountReceivableTrading, LegacyCode = Constant.AccountLegacyCode.AccountReceivableTrading, Level = 4, Group = (int)Constant.AccountGroup.Asset, ParentId = AccountReceivableTrading3.Id, IsLegacy = true });
            //var GBCHReceivable2 = _accountService.UpdateOrCreateLegacyObject(new Account() { Name = "GBCH Receivable", Code = Constant.AccountCode.GBCHReceivable2, LegacyCode = Constant.AccountLegacyCode.GBCHReceivable2, Level = 2, Group = (int)Constant.AccountGroup.Asset, ParentId = Asset.Id, IsLegacy = true });
            //var GBCHReceivable3 = _accountService.UpdateOrCreateLegacyObject(new Account() { Name = "GBCH Receivable", Code = Constant.AccountCode.GBCHReceivable3, LegacyCode = Constant.AccountLegacyCode.GBCHReceivable3, Level = 3, Group = (int)Constant.AccountGroup.Asset, ParentId = GBCHReceivable2.Id, IsLegacy = true });
            //var GBCHReceivable = _accountService.UpdateOrCreateLegacyObject(new Account() { Name = "GBCH Receivable", IsUsedBySystem = true, IsLeaf = true, Code = Constant.AccountCode.GBCHReceivable, LegacyCode = Constant.AccountLegacyCode.GBCHReceivable, Level = 4, Group = (int)Constant.AccountGroup.Asset, ParentId = GBCHReceivable3.Id, IsLegacy = true });
            //var Inventory = _accountService.UpdateOrCreateLegacyObject(new Account() { Name = "Inventory", Code = Constant.AccountCode.Inventory, LegacyCode = Constant.AccountLegacyCode.Inventory, Level = 2, Group = (int)Constant.AccountGroup.Asset, ParentId = Asset.Id, IsLegacy = true });
            //var TradingGoods3 = _accountService.UpdateOrCreateLegacyObject(new Account() { Name = "Trading Goods", Code = Constant.AccountCode.TradingGoods3, LegacyCode = Constant.AccountLegacyCode.TradingGoods3, Level = 3, Group = (int)Constant.AccountGroup.Asset, ParentId = Inventory.Id, IsLegacy = true });
            //var TradingGoods = _accountService.UpdateOrCreateLegacyObject(new Account() { Name = "Trading Goods", IsUsedBySystem = true, IsLeaf = true, Code = Constant.AccountCode.TradingGoods, LegacyCode = Constant.AccountLegacyCode.TradingGoods, Level = 4, Group = (int)Constant.AccountGroup.Asset, ParentId = TradingGoods3.Id, IsLegacy = true });

            //var Expense = _accountService.UpdateOrCreateLegacyObject(new Account() { Name = "Expense", Code = Constant.AccountCode.Expense, LegacyCode = Constant.AccountLegacyCode.Expense, Level = 1, Group = (int)Constant.AccountGroup.Expense, IsLegacy = true });
            //var COGS2 = _accountService.UpdateOrCreateLegacyObject(new Account() { Name = "Cost Of Goods Sold", Code = Constant.AccountCode.COGS2, LegacyCode = Constant.AccountLegacyCode.COGS2, Level = 2, Group = (int)Constant.AccountGroup.Expense, ParentId = Expense.Id, IsLegacy = true });
            //var COGS3 = _accountService.UpdateOrCreateLegacyObject(new Account() { Name = "Cost Of Goods Sold", Code = Constant.AccountCode.COGS3, LegacyCode = Constant.AccountLegacyCode.COGS3, Level = 3, Group = (int)Constant.AccountGroup.Expense, ParentId = COGS2.Id, IsLegacy = true });
            //var COGS = _accountService.UpdateOrCreateLegacyObject(new Account() { Name = "Cost Of Goods Sold", IsUsedBySystem = true, IsLeaf = true, Code = Constant.AccountCode.COGS, LegacyCode = Constant.AccountLegacyCode.COGS, Level = 4, Group = (int)Constant.AccountGroup.Expense, ParentId = COGS3.Id, IsLegacy = true });
            //var OperationalExpenses = _accountService.UpdateOrCreateLegacyObject(new Account() { Name = "Operational Expenses", Code = Constant.AccountCode.OperationalExpenses, LegacyCode = Constant.AccountLegacyCode.OperationalExpenses, Level = 2, Group = (int)Constant.AccountGroup.Expense, ParentId = Expense.Id, IsLegacy = true });
            //var FreightOutExpense = _accountService.UpdateOrCreateLegacyObject(new Account() { Name = "Freight Out Expenses", IsLeaf = true, Code = Constant.AccountCode.FreightOutExpense, LegacyCode = Constant.AccountLegacyCode.FreightOutExpense, Level = 3, Group = (int)Constant.AccountGroup.Expense, ParentId = OperationalExpenses.Id, IsLegacy = true });
            //var CashBankAdjustmentExpense2 = _accountService.UpdateOrCreateLegacyObject(new Account() { Name = "CashBank Adjustment Expense", Code = Constant.AccountCode.CashBankAdjustmentExpense2, LegacyCode = Constant.AccountLegacyCode.CashBankAdjustmentExpense2, Level = 2, Group = (int)Constant.AccountGroup.Expense, ParentId = Expense.Id, IsLegacy = true });
            //var CashBankAdjustmentExpense3 = _accountService.UpdateOrCreateLegacyObject(new Account() { Name = "CashBank Adjustment Expense", Code = Constant.AccountCode.CashBankAdjustmentExpense3, LegacyCode = Constant.AccountLegacyCode.CashBankAdjustmentExpense3, Level = 3, Group = (int)Constant.AccountGroup.Expense, ParentId = CashBankAdjustmentExpense2.Id, IsLegacy = true });
            //var CashBankAdjustmentExpense = _accountService.UpdateOrCreateLegacyObject(new Account() { Name = "CashBank Adjustment Expense", IsUsedBySystem = true, IsLeaf = true, Code = Constant.AccountCode.CashBankAdjustmentExpense, LegacyCode = Constant.AccountLegacyCode.CashBankAdjustmentExpense, Level = 4, Group = (int)Constant.AccountGroup.Expense, ParentId = CashBankAdjustmentExpense3.Id, IsLegacy = true });
            //var SalesDiscount2 = _accountService.UpdateOrCreateLegacyObject(new Account() { Name = "Sales Discount", Code = Constant.AccountCode.SalesDiscountExpense2, LegacyCode = Constant.AccountLegacyCode.SalesDiscountExpense2, Level = 2, Group = (int)Constant.AccountGroup.Expense, ParentId = Expense.Id, IsLegacy = true });
            //var SalesDiscount3 = _accountService.UpdateOrCreateLegacyObject(new Account() { Name = "Sales Discount", Code = Constant.AccountCode.SalesDiscountExpense3, LegacyCode = Constant.AccountLegacyCode.SalesDiscountExpense3, Level = 3, Group = (int)Constant.AccountGroup.Expense, ParentId = SalesDiscount2.Id, IsLegacy = true });
            //var SalesDiscount = _accountService.UpdateOrCreateLegacyObject(new Account() { Name = "Sales Discount", IsUsedBySystem = true, IsLeaf = true, Code = Constant.AccountCode.SalesDiscountExpense, LegacyCode = Constant.AccountLegacyCode.SalesDiscountExpense, Level = 4, Group = (int)Constant.AccountGroup.Expense, ParentId = SalesDiscount3.Id, IsLegacy = true });
            //var SalesAllowance2 = _accountService.UpdateOrCreateLegacyObject(new Account() { Name = "Sales Allowance", Code = Constant.AccountCode.SalesAllowanceExpense2, LegacyCode = Constant.AccountLegacyCode.SalesAllowanceExpense2, Level = 2, Group = (int)Constant.AccountGroup.Expense, ParentId = Expense.Id, IsLegacy = true });
            //var SalesAllowance3 = _accountService.UpdateOrCreateLegacyObject(new Account() { Name = "Sales Allowance", Code = Constant.AccountCode.SalesAllowanceExpense3, LegacyCode = Constant.AccountLegacyCode.SalesAllowanceExpense3, Level = 3, Group = (int)Constant.AccountGroup.Expense, ParentId = SalesAllowance2.Id, IsLegacy = true });
            //var SalesAllowance = _accountService.UpdateOrCreateLegacyObject(new Account() { Name = "Sales Allowance", IsUsedBySystem = true, IsLeaf = true, Code = Constant.AccountCode.SalesAllowanceExpense, LegacyCode = Constant.AccountLegacyCode.SalesAllowanceExpense, Level = 4, Group = (int)Constant.AccountGroup.Expense, ParentId = SalesAllowance3.Id, IsLegacy = true });
            //var StockAdjustmentExpense2 = _accountService.UpdateOrCreateLegacyObject(new Account() { Name = "Stock Adjustment Expense", Code = Constant.AccountCode.StockAdjustmentExpense2, LegacyCode = Constant.AccountLegacyCode.StockAdjustmentExpense2, Level = 2, Group = (int)Constant.AccountGroup.Expense, ParentId = Expense.Id, IsLegacy = true });
            //var StockAdjustmentExpense3 = _accountService.UpdateOrCreateLegacyObject(new Account() { Name = "Stock Adjustment Expense", Code = Constant.AccountCode.StockAdjustmentExpense3, LegacyCode = Constant.AccountLegacyCode.StockAdjustmentExpense3, Level = 3, Group = (int)Constant.AccountGroup.Expense, ParentId = StockAdjustmentExpense2.Id, IsLegacy = true });
            //var StockAdjustmentExpense = _accountService.UpdateOrCreateLegacyObject(new Account() { Name = "Stock Adjustment Expense", IsUsedBySystem = true, IsLeaf = true, Code = Constant.AccountCode.StockAdjustmentExpense, LegacyCode = Constant.AccountLegacyCode.StockAdjustmentExpense, Level = 4, Group = (int)Constant.AccountGroup.Expense, ParentId = StockAdjustmentExpense3.Id, IsLegacy = true });
            //var SalesReturnExpense2 = _accountService.UpdateOrCreateLegacyObject(new Account() { Name = "Sales Return Expense", Code = Constant.AccountCode.SalesReturnExpense2, LegacyCode = Constant.AccountLegacyCode.SalesReturnExpense2, Level = 2, Group = (int)Constant.AccountGroup.Expense, ParentId = Expense.Id, IsLegacy = true });
            //var SalesReturnExpense3 = _accountService.UpdateOrCreateLegacyObject(new Account() { Name = "Sales Return Expense", Code = Constant.AccountCode.SalesReturnExpense3, LegacyCode = Constant.AccountLegacyCode.SalesReturnExpense3, Level = 3, Group = (int)Constant.AccountGroup.Expense, ParentId = SalesReturnExpense2.Id, IsLegacy = true });
            //var SalesReturnExpense = _accountService.UpdateOrCreateLegacyObject(new Account() { Name = "Sales Return Expense", IsUsedBySystem = true, IsLeaf = true, Code = Constant.AccountCode.SalesReturnExpense, LegacyCode = Constant.AccountLegacyCode.SalesReturnExpense, Level = 4, Group = (int)Constant.AccountGroup.Expense, ParentId = SalesReturnExpense3.Id, IsLegacy = true });
            //var FreightIn2 = _accountService.UpdateOrCreateLegacyObject(new Account() { Name = "Freight In", Code = Constant.AccountCode.FreightIn2, LegacyCode = Constant.AccountLegacyCode.FreightIn2, Level = 2, Group = (int)Constant.AccountGroup.Expense, ParentId = Expense.Id, IsLegacy = true });
            //var FreightIn3 = _accountService.UpdateOrCreateLegacyObject(new Account() { Name = "Freight In", Code = Constant.AccountCode.FreightIn3, LegacyCode = Constant.AccountLegacyCode.FreightIn3, Level = 3, Group = (int)Constant.AccountGroup.Expense, ParentId = FreightIn2.Id, IsLegacy = true });
            //var FreightIn = _accountService.UpdateOrCreateLegacyObject(new Account() { Name = "Freight In", IsUsedBySystem = true, IsLeaf = true, Code = Constant.AccountCode.FreightIn, LegacyCode = Constant.AccountLegacyCode.FreightIn, Level = 4, Group = (int)Constant.AccountGroup.Expense, ParentId = FreightIn3.Id, IsLegacy = true });
            //// Memorial Expenses
            //var NonOperationalExpenses2 = _accountService.UpdateOrCreateLegacyObject(new Account() { Name = "Non-Operational Expenses", Code = Constant.AccountCode.NonOperationalExpenses2, LegacyCode = Constant.AccountLegacyCode.NonOperationalExpenses2, Level = 2, Group = (int)Constant.AccountGroup.Expense, ParentId = Expense.Id, IsLegacy = true });
            //var NonOperationalExpenses = _accountService.UpdateOrCreateLegacyObject(new Account() { Name = "Non-Operational Expenses", IsLeaf = true, Code = Constant.AccountCode.NonOperationalExpenses, LegacyCode = Constant.AccountLegacyCode.NonOperationalExpenses, Level = 3, Group = (int)Constant.AccountGroup.Expense, ParentId = NonOperationalExpenses2.Id, IsLegacy = true });
            //var TaxExpense2 = _accountService.UpdateOrCreateLegacyObject(new Account() { Name = "Tax Expense", Code = Constant.AccountCode.Tax2, LegacyCode = Constant.AccountLegacyCode.Tax2, Level = 2, Group = (int)Constant.AccountGroup.Expense, ParentId = Expense.Id, IsLegacy = true });
            //var TaxExpense = _accountService.UpdateOrCreateLegacyObject(new Account() { Name = "Tax Expense", IsLeaf = true, Code = Constant.AccountCode.Tax, LegacyCode = Constant.AccountLegacyCode.Tax, Level = 3, Group = (int)Constant.AccountGroup.Expense, ParentId = TaxExpense2.Id, IsLegacy = true });
            //var Divident2 = _accountService.UpdateOrCreateLegacyObject(new Account() { Name = "Divident", Code = Constant.AccountCode.Divident2, LegacyCode = Constant.AccountLegacyCode.Divident2, Level = 2, Group = (int)Constant.AccountGroup.Expense, ParentId = Expense.Id, IsLegacy = true });
            //var Divident = _accountService.UpdateOrCreateLegacyObject(new Account() { Name = "Divident", IsLeaf = true, Code = Constant.AccountCode.Divident, LegacyCode = Constant.AccountLegacyCode.Divident, Level = 3, Group = (int)Constant.AccountGroup.Expense, ParentId = Divident2.Id, IsLegacy = true });
            //var InterestExpense2 = _accountService.UpdateOrCreateLegacyObject(new Account() { Name = "Interest Expense", Code = Constant.AccountCode.InterestExpense2, LegacyCode = Constant.AccountLegacyCode.InterestExpense2, Level = 2, Group = (int)Constant.AccountGroup.Expense, ParentId = Expense.Id, IsLegacy = true });
            //var InterestExpense = _accountService.UpdateOrCreateLegacyObject(new Account() { Name = "Interest Expense", IsLeaf = true, Code = Constant.AccountCode.InterestExpense, LegacyCode = Constant.AccountLegacyCode.InterestExpense, Level = 3, Group = (int)Constant.AccountGroup.Expense, ParentId = InterestExpense2.Id, IsLegacy = true });
            //var Depreciation2 = _accountService.UpdateOrCreateLegacyObject(new Account() { Name = "Depreciation", Code = Constant.AccountCode.Depreciation2, LegacyCode = Constant.AccountLegacyCode.Depreciation2, Level = 2, Group = (int)Constant.AccountGroup.Expense, ParentId = Expense.Id, IsLegacy = true });
            //var Depreciation = _accountService.UpdateOrCreateLegacyObject(new Account() { Name = "Depreciation", IsLeaf = true, Code = Constant.AccountCode.Depreciation, LegacyCode = Constant.AccountLegacyCode.Depreciation, Level = 3, Group = (int)Constant.AccountGroup.Expense, ParentId = Depreciation2.Id, IsLegacy = true });
            //var Amortization2 = _accountService.UpdateOrCreateLegacyObject(new Account() { Name = "Amortization", Code = Constant.AccountCode.Amortization2, LegacyCode = Constant.AccountLegacyCode.Amortization2, Level = 2, Group = (int)Constant.AccountGroup.Expense, ParentId = Expense.Id, IsLegacy = true });
            //var Amortization = _accountService.UpdateOrCreateLegacyObject(new Account() { Name = "Amortization", IsLeaf = true, Code = Constant.AccountCode.Amortization, LegacyCode = Constant.AccountLegacyCode.Amortization, Level = 3, Group = (int)Constant.AccountGroup.Expense, ParentId = Amortization2.Id, IsLegacy = true });

            //var Liability = _accountService.UpdateOrCreateLegacyObject(new Account() { Name = "Liability", Code = Constant.AccountCode.Liability, LegacyCode = Constant.AccountLegacyCode.Liability, Level = 1, Group = (int)Constant.AccountGroup.Liability, IsLegacy = true });
            //var AccountPayable = _accountService.UpdateOrCreateLegacyObject(new Account() { Name = "Account Payable", Code = Constant.AccountCode.AccountPayable, LegacyCode = Constant.AccountLegacyCode.AccountPayable, Level = 2, Group = (int)Constant.AccountGroup.Liability, ParentId = Liability.Id, IsLegacy = true });
            //var AccountPayableTrading3 = _accountService.UpdateOrCreateLegacyObject(new Account() { Name = "Account Payable (Trading)", Code = Constant.AccountCode.AccountPayableTrading3, LegacyCode = Constant.AccountLegacyCode.AccountPayableTrading3, Level = 3, Group = (int)Constant.AccountGroup.Liability, ParentId = AccountPayable.Id, IsLegacy = true });
            //var AccountPayableTrading = _accountService.UpdateOrCreateLegacyObject(new Account() { Name = "Account Payable (Trading)", IsUsedBySystem = true, IsLeaf = true, Code = Constant.AccountCode.AccountPayableTrading, LegacyCode = Constant.AccountLegacyCode.AccountPayableTrading, Level = 4, Group = (int)Constant.AccountGroup.Liability, ParentId = AccountPayableTrading3.Id, IsLegacy = true });
            //var AccountPayableNonTrading3 = _accountService.UpdateOrCreateLegacyObject(new Account() { Name = "Account Payable (Non Trading)", Code = Constant.AccountCode.AccountPayableNonTrading3, LegacyCode = Constant.AccountLegacyCode.AccountPayableNonTrading3, Level = 3, Group = (int)Constant.AccountGroup.Liability, ParentId = AccountPayable.Id, IsLegacy = true });
            //var AccountPayableNonTrading = _accountService.UpdateOrCreateLegacyObject(new Account() { Name = "Account Payable (Non Trading)", IsUsedBySystem = true, IsLeaf = true, Code = Constant.AccountCode.AccountPayableNonTrading, LegacyCode = Constant.AccountLegacyCode.AccountPayableNonTrading, Level = 4, Group = (int)Constant.AccountGroup.Liability, ParentId = AccountPayableNonTrading3.Id, IsLegacy = true });
            //var AccountPayablePPNkeluar3 = _accountService.UpdateOrCreateLegacyObject(new Account() { Name = "Account Payable (PPN keluaran)", Code = Constant.AccountCode.AccountPayablePPNkeluaran3, LegacyCode = Constant.AccountLegacyCode.AccountPayablePPNkeluaran3, Level = 3, Group = (int)Constant.AccountGroup.Liability, ParentId = AccountPayable.Id, IsLegacy = true });
            //var AccountPayablePPNkeluar = _accountService.UpdateOrCreateLegacyObject(new Account() { Name = "Account Payable (PPN keluaran)", IsUsedBySystem = true, IsLeaf = true, Code = Constant.AccountCode.AccountPayablePPNkeluaran, LegacyCode = Constant.AccountLegacyCode.AccountPayablePPNkeluaran, Level = 4, Group = (int)Constant.AccountGroup.Liability, ParentId = AccountPayablePPNkeluar3.Id, IsLegacy = true });
            //var GBCHPayable2 = _accountService.UpdateOrCreateLegacyObject(new Account() { Name = "GBCH Payable", Code = Constant.AccountCode.GBCHPayable2, LegacyCode = Constant.AccountLegacyCode.GBCHPayable2, Level = 2, Group = (int)Constant.AccountGroup.Liability, ParentId = Liability.Id, IsLegacy = true });
            //var GBCHPayable3 = _accountService.UpdateOrCreateLegacyObject(new Account() { Name = "GBCH Payable", Code = Constant.AccountCode.GBCHPayable3, LegacyCode = Constant.AccountLegacyCode.GBCHPayable3, Level = 3, Group = (int)Constant.AccountGroup.Liability, ParentId = GBCHPayable2.Id, IsLegacy = true });
            //var GBCHPayable = _accountService.UpdateOrCreateLegacyObject(new Account() { Name = "GBCH Payable", IsUsedBySystem = true, IsLeaf = true, Code = Constant.AccountCode.GBCHPayable, LegacyCode = Constant.AccountLegacyCode.GBCHPayable, Level = 4, Group = (int)Constant.AccountGroup.Liability, ParentId = GBCHPayable3.Id, IsLegacy = true });
            //var GoodsPendingClearance2 = _accountService.UpdateOrCreateLegacyObject(new Account() { Name = "Goods Pending Clearance", Code = Constant.AccountCode.GoodsPendingClearance2, LegacyCode = Constant.AccountLegacyCode.GoodsPendingClearance2, Level = 2, Group = (int)Constant.AccountGroup.Liability, ParentId = Liability.Id, IsLegacy = true });
            //var GoodsPendingClearance3 = _accountService.UpdateOrCreateLegacyObject(new Account() { Name = "Goods Pending Clearance", Code = Constant.AccountCode.GoodsPendingClearance3, LegacyCode = Constant.AccountLegacyCode.GoodsPendingClearance3, Level = 3, Group = (int)Constant.AccountGroup.Liability, ParentId = GoodsPendingClearance2.Id, IsLegacy = true });
            //var GoodsPendingClearance = _accountService.UpdateOrCreateLegacyObject(new Account() { Name = "Goods Pending Clearance", IsUsedBySystem = true, IsLeaf = true, Code = Constant.AccountCode.GoodsPendingClearance, LegacyCode = Constant.AccountLegacyCode.GoodsPendingClearance, Level = 4, Group = (int)Constant.AccountGroup.Liability, ParentId = GoodsPendingClearance3.Id, IsLegacy = true });
            //var PurchaseDiscount2 = _accountService.UpdateOrCreateLegacyObject(new Account() { Name = "Purchase Discount", Code = Constant.AccountCode.PurchaseDiscount2, LegacyCode = Constant.AccountLegacyCode.PurchaseDiscount2, Level = 2, Group = (int)Constant.AccountGroup.Liability, ParentId = Liability.Id, IsLegacy = true });
            //var PurchaseDiscount3 = _accountService.UpdateOrCreateLegacyObject(new Account() { Name = "Purchase Discount", Code = Constant.AccountCode.PurchaseDiscount3, LegacyCode = Constant.AccountLegacyCode.PurchaseDiscount3, Level = 3, Group = (int)Constant.AccountGroup.Liability, ParentId = PurchaseDiscount2.Id, IsLegacy = true });
            //var PurchaseDiscount = _accountService.UpdateOrCreateLegacyObject(new Account() { Name = "Purchase Discount", IsUsedBySystem = true, IsLeaf = true, Code = Constant.AccountCode.PurchaseDiscount, LegacyCode = Constant.AccountLegacyCode.PurchaseDiscount, Level = 4, Group = (int)Constant.AccountGroup.Liability, ParentId = PurchaseDiscount3.Id, IsLegacy = true });
            //var PurchaseAllowance2 = _accountService.UpdateOrCreateLegacyObject(new Account() { Name = "Purchase Allowance", Code = Constant.AccountCode.PurchaseAllowance2, LegacyCode = Constant.AccountLegacyCode.PurchaseAllowance2, Level = 2, Group = (int)Constant.AccountGroup.Liability, ParentId = Liability.Id, IsLegacy = true });
            //var PurchaseAllowance3 = _accountService.UpdateOrCreateLegacyObject(new Account() { Name = "Purchase Allowance", Code = Constant.AccountCode.PurchaseAllowance3, LegacyCode = Constant.AccountLegacyCode.PurchaseAllowance3, Level = 3, Group = (int)Constant.AccountGroup.Liability, ParentId = PurchaseAllowance2.Id, IsLegacy = true });
            //var PurchaseAllowance = _accountService.UpdateOrCreateLegacyObject(new Account() { Name = "Purchase Allowance", IsUsedBySystem = true, IsLeaf = true, Code = Constant.AccountCode.PurchaseAllowance, LegacyCode = Constant.AccountLegacyCode.PurchaseAllowance, Level = 4, Group = (int)Constant.AccountGroup.Liability, ParentId = PurchaseAllowance3.Id, IsLegacy = true });
            //var SalesReturnAllowance2 = _accountService.UpdateOrCreateLegacyObject(new Account() { Name = "Sales Return Allowance", Code = Constant.AccountCode.SalesReturnAllowance2, LegacyCode = Constant.AccountLegacyCode.SalesReturnAllowance2, Level = 2, Group = (int)Constant.AccountGroup.Liability, ParentId = Liability.Id, IsLegacy = true });
            //var SalesReturnAllowance3 = _accountService.UpdateOrCreateLegacyObject(new Account() { Name = "Sales Return Allowance", Code = Constant.AccountCode.SalesReturnAllowance3, LegacyCode = Constant.AccountLegacyCode.SalesReturnAllowance3, Level = 3, Group = (int)Constant.AccountGroup.Liability, ParentId = SalesReturnAllowance2.Id, IsLegacy = true });
            //var SalesReturnAllowance = _accountService.UpdateOrCreateLegacyObject(new Account() { Name = "Sales Return Allowance", IsUsedBySystem = true, IsLeaf = true, Code = Constant.AccountCode.SalesReturnAllowance, LegacyCode = Constant.AccountLegacyCode.SalesReturnAllowance, Level = 4, Group = (int)Constant.AccountGroup.Liability, ParentId = SalesReturnAllowance3.Id, IsLegacy = true });

            //var Equity = _accountService.UpdateOrCreateLegacyObject(new Account() { Name = "Equity", Code = Constant.AccountCode.Equity, LegacyCode = Constant.AccountLegacyCode.Equity, Level = 1, Group = (int)Constant.AccountGroup.Equity, IsLegacy = true });
            //var OwnersEquity = _accountService.UpdateOrCreateLegacyObject(new Account() { Name = "Owners Equity", Code = Constant.AccountCode.OwnersEquity, LegacyCode = Constant.AccountLegacyCode.OwnersEquity, Level = 2, Group = (int)Constant.AccountGroup.Equity, ParentId = Equity.Id, IsLegacy = true });
            //var EquityAdjustment3 = _accountService.UpdateOrCreateLegacyObject(new Account() { Name = "Equity Adjustment", Code = Constant.AccountCode.EquityAdjustment3, LegacyCode = Constant.AccountLegacyCode.EquityAdjustment3, Level = 3, Group = (int)Constant.AccountGroup.Equity, ParentId = OwnersEquity.Id, IsLegacy = true });
            //var EquityAdjustment = _accountService.UpdateOrCreateLegacyObject(new Account() { Name = "Equity Adjustment", IsUsedBySystem = true, IsLeaf = true, Code = Constant.AccountCode.EquityAdjustment, LegacyCode = Constant.AccountLegacyCode.EquityAdjustment, Level = 4, Group = (int)Constant.AccountGroup.Equity, ParentId = EquityAdjustment3.Id, IsLegacy = true });
            //var RetainedEarnings2 = _accountService.UpdateOrCreateLegacyObject(new Account() { Name = "Retained Earnings", Code = Constant.AccountCode.RetainedEarnings2, LegacyCode = Constant.AccountLegacyCode.RetainedEarnings2, Level = 2, Group = (int)Constant.AccountGroup.Equity, ParentId = Equity.Id, IsLegacy = true });
            //var RetainedEarnings = _accountService.UpdateOrCreateLegacyObject(new Account() { Name = "Retained Earnings", IsUsedBySystem = true, IsLeaf = true, Code = Constant.AccountCode.RetainedEarnings, LegacyCode = Constant.AccountLegacyCode.RetainedEarnings, Level = 3, Group = (int)Constant.AccountGroup.Equity, ParentId = RetainedEarnings2.Id, IsLegacy = true });

            //var Revenue = _accountService.UpdateOrCreateLegacyObject(new Account() { Name = "Revenue", Code = Constant.AccountCode.Revenue, LegacyCode = Constant.AccountLegacyCode.Revenue, Level = 1, Group = (int)Constant.AccountGroup.Revenue, IsLegacy = true });
            //var FreightOut2 = _accountService.UpdateOrCreateLegacyObject(new Account() { Name = "Freight Out", Code = Constant.AccountCode.FreightOut2, LegacyCode = Constant.AccountLegacyCode.FreightOut2, Level = 2, Group = (int)Constant.AccountGroup.Revenue, ParentId = Revenue.Id, IsLegacy = true });
            //var FreightOut = _accountService.UpdateOrCreateLegacyObject(new Account() { Name = "Freight Out", IsUsedBySystem = true, IsLeaf = true, Code = Constant.AccountCode.FreightOut, LegacyCode = Constant.AccountLegacyCode.FreightOut, Level = 3, Group = (int)Constant.AccountGroup.Revenue, ParentId = FreightOut2.Id, IsLegacy = true });
            //var Sales2 = _accountService.UpdateOrCreateLegacyObject(new Account() { Name = "Sales", Code = Constant.AccountCode.SalesRevenue2, LegacyCode = Constant.AccountLegacyCode.SalesRevenue2, Level = 2, Group = (int)Constant.AccountGroup.Revenue, ParentId = Revenue.Id, IsLegacy = true });
            //var Sales = _accountService.UpdateOrCreateLegacyObject(new Account() { Name = "Sales", IsUsedBySystem = true, IsLeaf = true, Code = Constant.AccountCode.SalesRevenue, LegacyCode = Constant.AccountLegacyCode.SalesRevenue, Level = 3, Group = (int)Constant.AccountGroup.Revenue, ParentId = Sales2.Id, IsLegacy = true });
        }

        // http://www.tech-recipes.com/rx/1487/copy-an-existing-mysql-table-to-a-new-table/
        public int BackUp(Nullable<DateTime> startDate, Nullable<DateTime> endDate)
        {
            var db = new OffsetPrintingSuppliesEntities();
            using (db)
            {
                foreach (var tableName in masterDatas)
                {
                    // Create new table using source table structure, and then copy the data from source structure to new table
                    //db.Database.ExecuteSqlCommand(string.Format("CREATE TABLE 'Temp{0}' LIKE {0}; INSERT INTO 'Temp{0}' SELECT * FROM '{0}';", tableName));
                    //db.Database.ExecuteSqlCommand(string.Format("DELETE FROM 'Temp{0}'; CREATE TABLE 'Temp{0}' AS SELECT * FROM '{0}' WHERE IsDeleted=false;", tableName));
                    //List<string> a = new List<string>();
                    var a = db.Database.SqlQuery<string>(string.Format("select '[' + name + '], ' as [text()] from sys.columns where object_id = object_id('Temp{0}') for xml path('')", tableName)).ToList(); // where object_id = object_id('{0}')
                    string b = "";
                    foreach (var c in a) b += c;
                    b = b.Substring(0, b.Length - 2);
                    //db.Database.ExecuteSqlCommand(string.Format("SET IDENTITY_INSERT Temp{0} ON", tableName));
                    db.Database.ExecuteSqlCommand(string.Format("SET IDENTITY_INSERT Temp{0} ON; INSERT INTO Temp{0}({1}) SELECT {1} FROM {0} WHERE 1=1; SET IDENTITY_INSERT Temp{0} OFF;", tableName, b));
                    //db.Database.ExecuteSqlCommand(string.Format("SET IDENTITY_INSERT Temp{0} OFF", tableName));
                }

                foreach (var tableName in userDatas)
                {
                    if (tableName != "Account") // exclude manually created TempAccount table to fix/rearrange all accounts
                    {
                        var a = db.Database.SqlQuery<string>(string.Format("select '[' + name + '], ' as [text()] from sys.columns where object_id = object_id('Temp{0}') for xml path('')", tableName)).ToList(); // where object_id = object_id('{0}')
                        string b = "";
                        foreach (var c in a) b += c;
                        b = b.Substring(0, b.Length - 2);
                        // Create new table using source table structure, and then copy the data from source structure to new table
                        //db.Database.ExecuteSqlCommand(string.Format("CREATE TABLE Temp{0} LIKE {0}; INSERT Temp{0} SELECT * FROM {0};", tableName));
                        //db.Database.ExecuteSqlCommand(string.Format("DELETE FROM Temp{0}; CREATE TABLE Temp{0} AS SELECT * FROM {0} WHERE IsDeleted=false;", tableName));
                        db.Database.ExecuteSqlCommand(string.Format("SET IDENTITY_INSERT Temp{0} ON; INSERT INTO Temp{0}({1}) SELECT {1} FROM {0} WHERE 1=1; SET IDENTITY_INSERT Temp{0} OFF;", tableName, b));
                    }
                }
            }
            return 0;
        }

        public int OrderedRestoration(OffsetPrintingSuppliesEntities db, IDictionary<DateTime, sourceid> trans, bool reconfirm, bool repaid_reconcile)
        {
        //    int count = 0;
        //    // Sort and Process Documents
        //    var sa = db.TempStockAdjustments.OrderByDescending(x => x.Id);
        //    var wmo = db.TempWarehouseMutationOrders.OrderByDescending(x => x.Id);
        //    var cba = db.TempCashBankAdjustments.OrderByDescending(x => x.Id);
        //    var cbm = db.TempCashBankMutations.OrderByDescending(x => x.Id);
        //    var cpi = db.TempCustomPurchaseInvoices.OrderByDescending(x => x.Id);
        //    var csi = db.TempCashSalesInvoices.OrderByDescending(x => x.Id);
        //    var csr = db.TempCashSalesReturns.OrderByDescending(x => x.Id);
        //    var pr = db.TempPaymentRequests.OrderByDescending(x => x.Id);
        //    var rv = db.TempReceiptVouchers.OrderByDescending(x => x.Id);
        //    var pv = db.TempPaymentVouchers.OrderByDescending(x => x.Id);
        //    var mem = db.TempMemorials.OrderByDescending(x => x.Id);
        //    if (File.Exists(@"./OrderLog.txt"))
        //    {
        //        File.Delete(@"./OrderLog.txt");
        //    }
        //    var fstrm = new FileStream(@"./OrderLog.txt", FileMode.OpenOrCreate, FileAccess.Write);
        //    var writer = new StreamWriter(fstrm);
        //    // Loop from old to new
        //    foreach (var tran in trans.OrderBy(x => x.Key))
        //    {
        //        writer.WriteLine(tran.Key.ToString("dd/MM/yy : ") + tran.Value.source + " [" + tran.Value.id.ToString() + "]");
        //        switch (tran.Value.source.ToLower())
        //        {
        //            case TranSource.StockAdjustment:
        //                {
        //                    var rec = sa.Where(x => x.Id == tran.Value.id && !x.IsDeleted).FirstOrDefault();
        //                    if (rec == null) rec = sa.Where(x => x.Id == tran.Value.id).FirstOrDefault();
        //                    // ReCreate the objects
        //                    StockAdjustment obj = new StockAdjustment();
        //                    Reflection.CopyProperties(rec, obj);
        //                    obj.IsConfirmed = false;
        //                    obj.IsDeleted = false;
        //                    db.Database.ExecuteSqlCommand(string.Format("DBCC CHECKIDENT ({0}, RESEED, {1});", obj.GetType().Name.Split('_')[0], rec.Id - 1));
        //                    _stockAdjustmentService.CreateObject(obj, _warehouseService);
        //                    Log(obj.Errors, obj.GetType().Name.Split('_')[0], obj.Code, rec.Id, obj.Id);
        //                    if (obj != null)
        //                    {
        //                        // ReCreate the details
        //                        foreach (var det in db.TempStockAdjustmentDetails.Where(x => !x.IsDeleted && x.StockAdjustmentId == rec.Id).OrderBy(x => x.Id).ToList())
        //                        {
        //                            StockAdjustmentDetail detobj = new StockAdjustmentDetail();
        //                            Reflection.CopyProperties(det, detobj);
        //                            detobj.IsConfirmed = false;
        //                            detobj.IsDeleted = false;
        //                            db.Database.ExecuteSqlCommand(string.Format("DBCC CHECKIDENT ({0}, RESEED, {1});", detobj.GetType().Name.Split('_')[0], det.Id - 1));
        //                            _stockAdjustmentDetailService.CreateObject(detobj, _stockAdjustmentService, _itemService, _warehouseItemService);
        //                            Log(detobj.Errors, detobj.GetType().Name.Split('_')[0], detobj.Code, det.Id, detobj.Id);
        //                        }
        //                        // ReConfirm the objects
        //                        if (reconfirm && rec.IsConfirmed && !obj.IsConfirmed)
        //                        {
        //                            _stockAdjustmentService.ConfirmObject(obj, rec.ConfirmationDate.GetValueOrDefault(), _stockAdjustmentDetailService, _stockMutationService, _itemService, _barringService, _warehouseItemService, _generalLedgerJournalService, _accountService, _closingService);
        //                            Log(obj.Errors, obj.GetType().Name.Split('_')[0], obj.Code, rec.Id, obj.Id);
        //                        }
        //                    }
        //                    break;
        //                };
        //            case TranSource.WarehouseMutationOrder:
        //                {
        //                    var rec = wmo.Where(x => x.Id == tran.Value.id && !x.IsDeleted).FirstOrDefault();
        //                    if (rec == null) rec = wmo.Where(x => x.Id == tran.Value.id).FirstOrDefault();
        //                    // ReCreate the objects
        //                    WarehouseMutationOrder obj = new WarehouseMutationOrder();
        //                    Reflection.CopyProperties(rec, obj);
        //                    obj.IsConfirmed = false;
        //                    obj.IsDeleted = false; 
        //                    obj.IsCompleted = false;
        //                    db.Database.ExecuteSqlCommand(string.Format("DBCC CHECKIDENT ({0}, RESEED, {1});", obj.GetType().Name.Split('_')[0], rec.Id - 1));
        //                    _warehouseMutationOrderService.CreateObject(obj, _warehouseService);
        //                    Log(obj.Errors, obj.GetType().Name.Split('_')[0], obj.Code, rec.Id, obj.Id);
        //                    if (obj != null)
        //                    {
        //                        // ReCreate the details
        //                        foreach (var det in db.TempWarehouseMutationOrderDetails.Where(x => !x.IsDeleted && x.WarehouseMutationOrderId == rec.Id).OrderBy(x => x.Id).ToList())
        //                        {
        //                            WarehouseMutationOrderDetail detobj = new WarehouseMutationOrderDetail();
        //                            Reflection.CopyProperties(det, detobj);
        //                            detobj.IsConfirmed = false;
        //                            detobj.IsDeleted = false;
        //                            db.Database.ExecuteSqlCommand(string.Format("DBCC CHECKIDENT ({0}, RESEED, {1});", detobj.GetType().Name.Split('_')[0], det.Id - 1));
        //                            _warehouseMutationOrderDetailService.CreateObject(detobj, _warehouseMutationOrderService, _itemService, _warehouseItemService);
        //                            Log(detobj.Errors, detobj.GetType().Name.Split('_')[0], detobj.Code, det.Id, detobj.Id);
        //                        }
        //                        // ReConfirm the objects
        //                        if (reconfirm && rec.IsConfirmed && !obj.IsConfirmed)
        //                        {
        //                            _warehouseMutationOrderService.ConfirmObject(obj, rec.ConfirmationDate.GetValueOrDefault(), _warehouseMutationOrderDetailService, _itemService, _barringService, _warehouseItemService, _stockMutationService);
        //                            Log(obj.Errors, obj.GetType().Name.Split('_')[0], obj.Code, rec.Id, obj.Id);
        //                        }
        //                    }
        //                    break;
        //                };
        //            case TranSource.CashBankAdjustment:
        //                {
        //                    var rec = cba.Where(x => x.Id == tran.Value.id && !x.IsDeleted).FirstOrDefault();
        //                    if (rec == null) rec = cba.Where(x => x.Id == tran.Value.id).FirstOrDefault();
        //                    // ReCreate the objects
        //                    CashBankAdjustment obj = new CashBankAdjustment();
        //                    Reflection.CopyProperties(rec, obj);
        //                    obj.IsConfirmed = false;
        //                    obj.IsDeleted = false;
        //                    db.Database.ExecuteSqlCommand(string.Format("DBCC CHECKIDENT ({0}, RESEED, {1});", obj.GetType().Name.Split('_')[0], rec.Id - 1));
        //                    _cashBankAdjustmentService.CreateObject(obj, _cashBankService);
        //                    Log(obj.Errors, obj.GetType().Name.Split('_')[0], obj.Code, rec.Id, obj.Id);
        //                    if (obj != null)
        //                    {
        //                        // ReConfirm the objects
        //                        if (reconfirm && rec.IsConfirmed && !obj.IsConfirmed)
        //                        {
        //                            _cashBankAdjustmentService.ConfirmObject(obj, rec.ConfirmationDate.GetValueOrDefault(), _cashMutationService, _cashBankService, _generalLedgerJournalService, _accountService, _closingService);
        //                            Log(obj.Errors, obj.GetType().Name.Split('_')[0], obj.Code, rec.Id, obj.Id);
        //                        }
        //                    }
        //                    break;
        //                };
        //            case TranSource.CashBankMutation:
        //                {
        //                    var rec = cbm.Where(x => x.Id == tran.Value.id && !x.IsDeleted).FirstOrDefault();
        //                    if (rec == null) rec = cbm.Where(x => x.Id == tran.Value.id).FirstOrDefault();
        //                    // ReCreate the objects
        //                    CashBankMutation obj = new CashBankMutation();
        //                    Reflection.CopyProperties(rec, obj);
        //                    obj.IsConfirmed = false;
        //                    obj.IsDeleted = false;
        //                    db.Database.ExecuteSqlCommand(string.Format("DBCC CHECKIDENT ({0}, RESEED, {1});", obj.GetType().Name.Split('_')[0], rec.Id - 1));
        //                    _cashBankMutationService.CreateObject(obj, _cashBankService);
        //                    Log(obj.Errors, obj.GetType().Name.Split('_')[0], obj.Code, rec.Id, obj.Id);
        //                    if (obj != null)
        //                    {
        //                        // ReConfirm the objects
        //                        if (reconfirm && rec.IsConfirmed && !obj.IsConfirmed)
        //                        {
        //                            _cashBankMutationService.ConfirmObject(obj, rec.ConfirmationDate.GetValueOrDefault(), _cashMutationService, _cashBankService, _generalLedgerJournalService, _accountService, _closingService);
        //                            Log(obj.Errors, obj.GetType().Name.Split('_')[0], obj.Code, rec.Id, obj.Id);
        //                        }
        //                    }
        //                    break;
        //                };
        //            case TranSource.CustomPurchaseInvoice:
        //                {
        //                    var rec = cpi.Where(x => x.Id == tran.Value.id && !x.IsDeleted).FirstOrDefault();
        //                    if (rec == null) rec = cpi.Where(x => x.Id == tran.Value.id).FirstOrDefault();
        //                    var obj = _customPurchaseInvoiceService.GetObjectById(rec.Id);
        //                    TempPayable payable = db.TempPayables.Where(x => !x.IsDeleted && x.PayableSourceId == rec.Id && x.PayableSource == Constant.PayableSource.CustomPurchaseInvoice).OrderByDescending(x => x.Id).FirstOrDefault();
        //                    // ReCreate & ReConfirm
        //                    if (obj == null)
        //                    {
        //                        // ReCreate the objects
        //                        obj = new CustomPurchaseInvoice();
        //                        Reflection.CopyProperties(rec, obj);
        //                        obj.IsConfirmed = false;
        //                        obj.IsPaid = false;
        //                        obj.IsFullPayment = false;
        //                        obj.IsDeleted = false;
        //                        //if (rec.ConfirmationDate != null && rec.DueDate < rec.ConfirmationDate.GetValueOrDefault()) obj.DueDate = rec.ConfirmationDate;
        //                        //else if (rec.DueDate < rec.PurchaseDate) obj.DueDate = rec.PurchaseDate;
        //                        db.Database.ExecuteSqlCommand(string.Format("DBCC CHECKIDENT ({0}, RESEED, {1});", obj.GetType().Name.Split('_')[0], rec.Id - 1));
        //                        _customPurchaseInvoiceService.CreateObject(obj, _warehouseService, _contactService, _cashBankService);
        //                        Log(obj.Errors, obj.GetType().Name.Split('_')[0], obj.Code, rec.Id, obj.Id);
        //                        if (obj != null)
        //                        {
        //                            // ReCreate the details
        //                            foreach (var det in db.TempCustomPurchaseInvoiceDetails.Where(x => !x.IsDeleted && x.CustomPurchaseInvoiceId == rec.Id).OrderBy(x => x.Id).ToList())
        //                            {
        //                                CustomPurchaseInvoiceDetail detobj = new CustomPurchaseInvoiceDetail();
        //                                Reflection.CopyProperties(det, detobj);
        //                                detobj.IsDeleted = false;
        //                                db.Database.ExecuteSqlCommand(string.Format("DBCC CHECKIDENT ({0}, RESEED, {1});", detobj.GetType().Name.Split('_')[0], det.Id - 1));
        //                                _customPurchaseInvoiceDetailService.CreateObject(detobj, _customPurchaseInvoiceService, _itemService, _warehouseItemService);
        //                                Log(detobj.Errors, detobj.GetType().Name.Split('_')[0], detobj.Code, det.Id, detobj.Id);
        //                            }
                                    
        //                            // ReConfirm the objects
        //                            if (reconfirm && rec.IsConfirmed && !obj.IsConfirmed)
        //                            {
        //                                obj.Errors = new Dictionary<string, string>();
        //                                db.Database.ExecuteSqlCommand(string.Format("DBCC CHECKIDENT (Payable, RESEED, {0});", payable.Id - 1));
        //                                _customPurchaseInvoiceService.ConfirmObjectForRepair(obj, /*rec.PurchaseDate*/rec.ConfirmationDate.GetValueOrDefault(), payable.Code,
        //                                                                    _customPurchaseInvoiceDetailService, _contactService, _priceMutationService,
        //                                                                    _payableService, _customPurchaseInvoiceService, _warehouseItemService, _warehouseService, _itemService, _barringService, _stockMutationService,
        //                                                                    _generalLedgerJournalService, _accountService, _closingService);
        //                                Log(obj.Errors, obj.GetType().Name.Split('_')[0], obj.Code, rec.Id, obj.Id);
        //                            }
        //                        }
        //                    }
        //                    // RePaid the objects
        //                    else if (repaid_reconcile && rec.IsPaid && !obj.IsPaid)
        //                    {
        //                        TempPaymentVoucherDetail voucherDetail = db.TempPaymentVoucherDetails.Where(x => !x.IsDeleted && x.PayableId == payable.Id && x.Description.Contains("Automatic")).OrderByDescending(x => x.Id).FirstOrDefault();
        //                        if (voucherDetail == null)
        //                        {
        //                            //continue;
        //                            voucherDetail = db.TempPaymentVoucherDetails.Where(x => x.PayableId == payable.Id && x.Description.Contains("Automatic")).OrderByDescending(x => x.Id).FirstOrDefault();
        //                            Console.WriteLine("WARNING : Missing Automatic PaymentVoucherDetail on CPI[{0}]:{1} - Payable[{2}]:{3}", rec.Id, rec.Code, payable.Id, payable.Code);
        //                        }
        //                        if (voucherDetail != null)
        //                        {
        //                            TempPaymentVoucher voucher = db.TempPaymentVouchers.Where(x => !x.IsDeleted && x.Id == voucherDetail.PaymentVoucherId).OrderByDescending(x => x.Id).FirstOrDefault();
        //                            if (voucher == null)
        //                            {
        //                                voucher = db.TempPaymentVouchers.Where(x => x.Id == voucherDetail.PaymentVoucherId).OrderByDescending(x => x.Id).FirstOrDefault();
        //                            }
        //                            db.Database.ExecuteSqlCommand(string.Format("DBCC CHECKIDENT (PaymentVoucher, RESEED, {0});", voucher.Id - 1));
        //                            db.Database.ExecuteSqlCommand(string.Format("DBCC CHECKIDENT (PaymentVoucherDetail, RESEED, {0});", voucherDetail.Id - 1));
        //                            _customPurchaseInvoiceService.PaidObjectForRepair(obj, rec.AmountPaid.GetValueOrDefault(), rec.PaymentDate, voucher.Code, voucherDetail.Code, voucher.Id,
        //                                                                _cashBankService, _payableService, _paymentVoucherService, _paymentVoucherDetailService, _contactService, _cashMutationService,
        //                                                                _generalLedgerJournalService, _accountService, _closingService);
        //                            Log(obj.Errors, obj.GetType().Name.Split('_')[0], obj.Code, rec.Id, obj.Id);
        //                        }
        //                        // No vouchers found ?? buggy??
        //                        else
        //                        {
        //                            Console.WriteLine("WARNING : Unrecoverable PaymentVoucherDetail on CPI[{0}]:{1} - Payable[{2}]:{3} ! Need to be paid Manually", rec.Id, rec.Code, payable.Id, payable.Code);
        //                        }
        //                    }
        //                    break;
        //                };
        //            case TranSource.CashSalesInvoice:
        //                {
        //                    var rec = csi.Where(x => x.Id == tran.Value.id && !x.IsDeleted).FirstOrDefault();
        //                    if (rec == null) rec = csi.Where(x => x.Id == tran.Value.id).FirstOrDefault();
        //                    var obj = _cashSalesInvoiceService.GetObjectById(rec.Id);
        //                    TempReceivable receivable = db.TempReceivables.Where(x => !x.IsDeleted && x.ReceivableSourceId == rec.Id && x.ReceivableSource == Constant.ReceivableSource.CashSalesInvoice).OrderByDescending(x => x.Id).FirstOrDefault();
        //                    if (obj == null)
        //                    {
        //                        // ReCreate the objects
        //                        obj = new CashSalesInvoice();
        //                        Reflection.CopyProperties(rec, obj);
        //                        obj.IsConfirmed = false;
        //                        obj.IsPaid = false;
        //                        obj.IsFullPayment = false;
        //                        obj.IsDeleted = false;
        //                        //if (rec.ConfirmationDate != null && rec.DueDate < rec.ConfirmationDate.GetValueOrDefault()) obj.DueDate = rec.ConfirmationDate;
        //                        //else if (rec.DueDate < rec.SalesDate) obj.DueDate = rec.SalesDate;
        //                        db.Database.ExecuteSqlCommand(string.Format("DBCC CHECKIDENT ({0}, RESEED, {1});", obj.GetType().Name.Split('_')[0], rec.Id - 1));
        //                        _cashSalesInvoiceService.CreateObject(obj, _warehouseService, _cashBankService);
        //                        Log(obj.Errors, obj.GetType().Name.Split('_')[0], obj.Code, rec.Id, obj.Id);
        //                        if (obj != null)
        //                        {
        //                            // ReCreate the details
        //                            foreach (var det in db.TempCashSalesInvoiceDetails.Where(x => !x.IsDeleted && x.CashSalesInvoiceId == rec.Id).OrderBy(x => x.Id).ToList())
        //                            {
        //                                CashSalesInvoiceDetail detobj = new CashSalesInvoiceDetail();
        //                                Reflection.CopyProperties(det, detobj);
        //                                detobj.IsDeleted = false;
        //                                db.Database.ExecuteSqlCommand(string.Format("DBCC CHECKIDENT ({0}, RESEED, {1});", detobj.GetType().Name.Split('_')[0], det.Id - 1));
        //                                _cashSalesInvoiceDetailService.CreateObject(detobj, _cashSalesInvoiceService, _itemService, _warehouseItemService, _quantityPricingService);
        //                                Log(detobj.Errors, detobj.GetType().Name.Split('_')[0], detobj.Code, det.Id, detobj.Id);
        //                                if (detobj.Errors.Any())
        //                                {
        //                                    Item item = _itemService.GetObjectById(det.ItemId);
        //                                    Console.WriteLine("CSID Item SKU : {0} (QTY = {1})", item.Sku, detobj.Quantity);
        //                                }
        //                            }
        //                            // ReConfirm the objects
        //                            if (reconfirm && rec.IsConfirmed && !obj.IsConfirmed)
        //                            {
        //                                obj.Errors = new Dictionary<string, string>();
        //                                db.Database.ExecuteSqlCommand(string.Format("DBCC CHECKIDENT (Receivable, RESEED, {0});", receivable.Id - 1));
        //                                _cashSalesInvoiceService.ConfirmObjectForRepair(obj, /*rec.SalesDate*/rec.ConfirmationDate.GetValueOrDefault(), rec.Discount, rec.Tax, receivable.Code,
        //                                                                    _cashSalesInvoiceDetailService, _contactService, _priceMutationService,
        //                                                                    _receivableService, _cashSalesInvoiceService, _warehouseItemService, _warehouseService, _itemService, _barringService, _stockMutationService, _cashBankService,
        //                                                                    _generalLedgerJournalService, _accountService, _closingService);
        //                                Log(obj.Errors, obj.GetType().Name.Split('_')[0], obj.Code, rec.Id, obj.Id);
        //                            }
        //                        }
        //                    }
        //                    // RePaid the objects
        //                    else if (repaid_reconcile && rec.IsPaid && !obj.IsPaid)
        //                    {
        //                        TempReceiptVoucherDetail voucherDetail = db.TempReceiptVoucherDetails.Where(x => !x.IsDeleted && x.ReceivableId == receivable.Id && x.Description.Contains("Automatic")).OrderByDescending(x => x.Id).FirstOrDefault();
        //                        if (voucherDetail == null)
        //                        {
        //                            //continue;
        //                            voucherDetail = db.TempReceiptVoucherDetails.Where(x => x.ReceivableId == receivable.Id && x.Description.Contains("Automatic")).OrderByDescending(x => x.Id).FirstOrDefault();
        //                            Console.WriteLine("WARNING : Missing Automatic ReceiptVoucherDetail on CSI[{0}]:{1} - Receivable[{2}]:{3}", rec.Id, rec.Code, receivable.Id, receivable.Code);
        //                        }
        //                        if (voucherDetail != null)
        //                        {
        //                            TempReceiptVoucher voucher = db.TempReceiptVouchers.Where(x => !x.IsDeleted && x.Id == voucherDetail.ReceiptVoucherId).OrderByDescending(x => x.Id).FirstOrDefault();
        //                            if (voucher == null)
        //                            {
        //                                voucher = db.TempReceiptVouchers.Where(x => x.Id == voucherDetail.ReceiptVoucherId).OrderByDescending(x => x.Id).FirstOrDefault();
        //                            }
        //                            db.Database.ExecuteSqlCommand(string.Format("DBCC CHECKIDENT (ReceiptVoucher, RESEED, {0});", voucher.Id - 1));
        //                            db.Database.ExecuteSqlCommand(string.Format("DBCC CHECKIDENT (ReceiptVoucherDetail, RESEED, {0});", voucherDetail.Id - 1));
        //                            _cashSalesInvoiceService.PaidObjectForRepair(obj, rec.AmountPaid.GetValueOrDefault(), rec.Allowance, rec.PaymentDate, voucher.Code, voucherDetail.Code, voucher.Id,
        //                                                                _cashBankService, _receivableService, _receiptVoucherService, _receiptVoucherDetailService, _contactService, _cashMutationService, _cashSalesReturnService,
        //                                                                _generalLedgerJournalService, _accountService, _closingService);
        //                            Log(obj.Errors, obj.GetType().Name.Split('_')[0], obj.Code, rec.Id, obj.Id);
        //                        }
        //                        // No vouchers found ?? buggy??
        //                        else
        //                        {
        //                            Console.WriteLine("WARNING : Unrecoverable ReceiptVoucherDetail on CSI[{0}]:{1} - Receivable[{2}]:{3} ! Need to be paid Manually", rec.Id, rec.Code, receivable.Id, receivable.Code);
        //                        }
        //                    }
        //                    break;
        //                };
        //            case TranSource.CashSalesReturn:
        //                {
        //                    var rec = csr.Where(x => x.Id == tran.Value.id && !x.IsDeleted).FirstOrDefault();
        //                    if (rec == null) rec = csr.Where(x => x.Id == tran.Value.id).FirstOrDefault();
        //                    var obj = _cashSalesReturnService.GetObjectById(rec.Id);
        //                    TempPayable payable = db.TempPayables.Where(x => !x.IsDeleted && x.PayableSourceId == rec.Id && x.PayableSource == Constant.PayableSource.CashSalesReturn).OrderByDescending(x => x.Id).FirstOrDefault();
        //                    if (obj == null)
        //                    {
        //                        // ReCreate the objects
        //                        obj = new CashSalesReturn();
        //                        Reflection.CopyProperties(rec, obj);
        //                        obj.IsConfirmed = false;
        //                        obj.IsPaid = false;
        //                        obj.IsDeleted = false;
        //                        db.Database.ExecuteSqlCommand(string.Format("DBCC CHECKIDENT ({0}, RESEED, {1});", obj.GetType().Name.Split('_')[0], rec.Id - 1));
        //                        _cashSalesReturnService.CreateObject(obj, _cashSalesInvoiceService, _cashBankService);
        //                        Log(obj.Errors, obj.GetType().Name.Split('_')[0], obj.Code, rec.Id, obj.Id);
        //                        if (obj != null)
        //                        {
        //                            // ReCreate the details
        //                            foreach (var det in db.TempCashSalesReturnDetails.Where(x => !x.IsDeleted && x.CashSalesReturnId == rec.Id).OrderBy(x => x.Id).ToList())
        //                            {
        //                                CashSalesReturnDetail detobj = new CashSalesReturnDetail();
        //                                Reflection.CopyProperties(det, detobj);
        //                                detobj.IsDeleted = false;
        //                                db.Database.ExecuteSqlCommand(string.Format("DBCC CHECKIDENT ({0}, RESEED, {1});", detobj.GetType().Name.Split('_')[0], det.Id - 1));
        //                                _cashSalesReturnDetailService.CreateObject(detobj, _cashSalesReturnService, _cashSalesInvoiceDetailService);
        //                                Log(detobj.Errors, detobj.GetType().Name.Split('_')[0], detobj.Code, det.Id, detobj.Id);
        //                            }
        //                            // ReConfirm the objects
        //                            if (reconfirm && rec.IsConfirmed && !obj.IsConfirmed)
        //                            {
        //                                db.Database.ExecuteSqlCommand(string.Format("DBCC CHECKIDENT (Payable, RESEED, {0});", payable.Id - 1));
        //                                _cashSalesReturnService.ConfirmObjectForRepair(obj, /*rec.ReturnDate.GetValueOrDefault()*/rec.ConfirmationDate.GetValueOrDefault(), rec.Allowance, payable.Code,
        //                                                                    _cashSalesReturnDetailService, _contactService, _cashSalesInvoiceService, _cashSalesInvoiceDetailService, _priceMutationService,
        //                                                                    _payableService, _cashSalesReturnService, _warehouseItemService, _warehouseService, _itemService, _barringService, _stockMutationService,
        //                                                                    _generalLedgerJournalService, _accountService, _closingService);
        //                                Log(obj.Errors, obj.GetType().Name.Split('_')[0], obj.Code, rec.Id, obj.Id);
        //                            }
        //                        }
        //                    }
        //                    // RePaid the objects
        //                    else if (repaid_reconcile && rec.IsPaid && !obj.IsPaid)
        //                    {
        //                        TempPaymentVoucherDetail voucherDetail = db.TempPaymentVoucherDetails.Where(x => !x.IsDeleted && x.PayableId == payable.Id && x.Description.Contains("Automatic")).OrderByDescending(x => x.Id).FirstOrDefault();
        //                        if (voucherDetail == null)
        //                        {
        //                            //continue;
        //                            voucherDetail = db.TempPaymentVoucherDetails.Where(x => x.PayableId == payable.Id && x.Description.Contains("Automatic")).OrderByDescending(x => x.Id).FirstOrDefault();
        //                            Console.WriteLine("WARNING : Missing Automatic PaymentVoucherDetail on CSR[{0}]:{1} - Payable[{2}]:{3}", rec.Id, rec.Code, payable.Id, payable.Code);
        //                        }
        //                        if (voucherDetail != null)
        //                        {
        //                            TempPaymentVoucher voucher = db.TempPaymentVouchers.Where(x => !x.IsDeleted && x.Id == voucherDetail.PaymentVoucherId).OrderByDescending(x => x.Id).FirstOrDefault();
        //                            if (voucher == null)
        //                            {
        //                                voucher = db.TempPaymentVouchers.Where(x => x.Id == voucherDetail.PaymentVoucherId).OrderByDescending(x => x.Id).FirstOrDefault();
        //                            }
        //                            db.Database.ExecuteSqlCommand(string.Format("DBCC CHECKIDENT (PaymentVoucher, RESEED, {0});", voucher.Id - 1));
        //                            db.Database.ExecuteSqlCommand(string.Format("DBCC CHECKIDENT (PaymentVoucherDetail, RESEED, {0});", voucherDetail.Id - 1));
        //                            _cashSalesReturnService.PaidObjectForRepair(obj, rec.PaymentDate, voucher.Code, voucherDetail.Code, voucher.Id,
        //                                                                _cashBankService, _payableService, _paymentVoucherService, _paymentVoucherDetailService, _contactService, _cashMutationService,
        //                                                                _generalLedgerJournalService, _accountService, _closingService);
        //                            Log(obj.Errors, obj.GetType().Name.Split('_')[0], obj.Code, rec.Id, obj.Id);
        //                        }
        //                        // No vouchers found ?? buggy??
        //                        else
        //                        {
        //                            Console.WriteLine("WARNING : Unrecoverable PaymentVoucherDetail on CSR[{0}]:{1} - Payable[{2}]:{3} ! Need to be Paid Manually", rec.Id, rec.Code, payable.Id, payable.Code);
        //                        }
        //                    }
        //                    break;
        //                };
        //            case TranSource.PaymentRequest:
        //                {
        //                    var rec = pr.Where(x => x.Id == tran.Value.id && !x.IsDeleted).FirstOrDefault();
        //                    if (rec == null) rec = pr.Where(x => x.Id == tran.Value.id).FirstOrDefault();
        //                    // ReCreate the objects
        //                    TempPaymentRequestDetail tmp = db.TempPaymentRequestDetails.Where(x => !x.IsDeleted && x.PaymentRequestId == rec.Id && x.IsLegacy && x.Status == Constant.GeneralLedgerStatus.Credit).OrderByDescending(x => x.Id).FirstOrDefault();
        //                    PaymentRequest obj = new PaymentRequest();
        //                    Reflection.CopyProperties(rec, obj);
        //                    obj.IsConfirmed = false;
        //                    obj.IsDeleted = false;
        //                    db.Database.ExecuteSqlCommand(string.Format("DBCC CHECKIDENT ({0}, RESEED, {1});", obj.GetType().Name.Split('_')[0], rec.Id - 1));
        //                    db.Database.ExecuteSqlCommand(string.Format("DBCC CHECKIDENT (PaymentRequestDetail, RESEED, {0});", tmp.Id - 1));
        //                    _paymentRequestService.CreateObject(obj, _contactService, _paymentRequestDetailService, _accountService, _generalLedgerJournalService, _closingService);
        //                    Log(obj.Errors, obj.GetType().Name.Split('_')[0], obj.Code, rec.Id, obj.Id);
        //                    if (obj != null)
        //                    {
        //                        // ReCreate the details
        //                        foreach (var det in db.TempPaymentRequestDetails.Where(x => !x.IsDeleted && x.PaymentRequestId == rec.Id && !x.IsLegacy).OrderBy(x => x.Id).ToList())
        //                        {
        //                            PaymentRequestDetail detobj = new PaymentRequestDetail();
        //                            Reflection.CopyProperties(det, detobj);
        //                            detobj.IsConfirmed = false;
        //                            detobj.IsDeleted = false;
        //                            db.Database.ExecuteSqlCommand(string.Format("DBCC CHECKIDENT ({0}, RESEED, {1});", detobj.GetType().Name.Split('_')[0], det.Id - 1));
        //                            _paymentRequestDetailService.CreateObject(detobj, _paymentRequestService, _accountService);
        //                            Log(detobj.Errors, detobj.GetType().Name.Split('_')[0], detobj.Code, det.Id, detobj.Id);
        //                        }
        //                        TempPayable payable = db.TempPayables.Where(x => !x.IsDeleted && x.PayableSourceId == rec.Id && x.PayableSource == Constant.PayableSource.PaymentRequest).OrderByDescending(x => x.Id).FirstOrDefault();
        //                        // ReConfirm the objects
        //                        if (reconfirm && rec.IsConfirmed && !obj.IsConfirmed)
        //                        {
        //                            db.Database.ExecuteSqlCommand(string.Format("DBCC CHECKIDENT (Payable, RESEED, {0});", payable.Id - 1));
        //                            _paymentRequestService.ConfirmObject(obj, rec.ConfirmationDate.GetValueOrDefault(), _payableService, _paymentRequestDetailService,
        //                                                        _accountService, _generalLedgerJournalService, _closingService);
        //                            Log(obj.Errors, obj.GetType().Name.Split('_')[0], obj.Code, rec.Id, obj.Id);
        //                        }
        //                    }
        //                    break;
        //                };
        //            case TranSource.ReceiptVoucher:
        //                {
        //                    var rec = rv.Where(x => x.Id == tran.Value.id && !x.IsDeleted).FirstOrDefault();
        //                    if (rec == null) rec = rv.Where(x => x.Id == tran.Value.id).FirstOrDefault();
        //                    // ReCreate the objects
        //                    var obj = _receiptVoucherService.GetObjectById(rec.Id);
        //                    //ReceiptVoucher obj = new ReceiptVoucher();
        //                    //Reflection.CopyProperties(rec, obj);
        //                    //obj.IsConfirmed = false;
        //                    //obj.IsReconciled = false;
        //                    //obj.IsDeleted = false;
        //                    //db.Database.ExecuteSqlCommand(string.Format("DBCC CHECKIDENT ({0}, RESEED, {1});", obj.GetType().Name.Split('_')[0], rec.Id - 1));
        //                    //_receiptVoucherService.CreateObject(obj, _receiptVoucherDetailService, _receivableService, _contactService, _cashBankService);
        //                    //Log(obj.Errors, obj.GetType().Name.Split('_')[0], obj.Code, rec.Id, obj.Id);
        //                    if (obj != null)
        //                    {
        //                        if (!obj.IsConfirmed)
        //                        {
        //                            // ReCreate the details
        //                            foreach (var det in db.TempReceiptVoucherDetails.Where(x => !x.IsDeleted && x.ReceiptVoucherId == rec.Id && !x.Description.Contains("Automatic")).OrderBy(x => x.Id).ToList())
        //                            {
        //                                TempReceivable tmp = db.TempReceivables.Where(x => !x.IsDeleted && x.Id == det.ReceivableId).OrderByDescending(x => x.Id).FirstOrDefault();
        //                                if (tmp == null) tmp = db.TempReceivables.Where(x => x.Id == det.ReceivableId).OrderByDescending(x => x.Id).FirstOrDefault();
        //                                ReceiptVoucherDetail detobj = new ReceiptVoucherDetail();
        //                                Reflection.CopyProperties(det, detobj);
        //                                detobj.IsConfirmed = false;
        //                                detobj.IsDeleted = false;
        //                                var receivable = _receivableService.GetObjectBySource(tmp.ReceivableSource, tmp.ReceivableSourceId);
        //                                if (receivable == null) Console.WriteLine("Missing Receivable [SourceID={0}, Source={1}] on {2}:Code={3}", tmp.ReceivableSourceId, tmp.ReceivableSource, detobj.GetType().Name.Split('_')[0], detobj.Code);
        //                                detobj.ReceivableId = (receivable != null) ? receivable.Id : 0;
        //                                db.Database.ExecuteSqlCommand(string.Format("DBCC CHECKIDENT ({0}, RESEED, {1});", detobj.GetType().Name.Split('_')[0], det.Id - 1));
        //                                _receiptVoucherDetailService.CreateObject(detobj, _receiptVoucherService, _cashBankService, _receivableService);
        //                                Log(detobj.Errors, detobj.GetType().Name.Split('_')[0], detobj.Code, det.Id, detobj.Id);
        //                            }
        //                            // ReConfirm the objects
        //                            if (reconfirm && rec.IsConfirmed && !obj.IsConfirmed)
        //                            {
        //                                _receiptVoucherService.ConfirmObject(obj, rec.ConfirmationDate.GetValueOrDefault(), _receiptVoucherDetailService, _cashBankService, _receivableService, _cashMutationService,
        //                                                            _generalLedgerJournalService, _accountService, _closingService);
        //                                Log(obj.Errors, obj.GetType().Name.Split('_')[0], obj.Code, rec.Id, obj.Id);
        //                            }
        //                        }
        //                        // ReReconcile the objects
        //                        else if (repaid_reconcile && rec.IsReconciled && !obj.IsReconciled)
        //                        {
        //                            _receiptVoucherService.ReconcileObject(obj, rec.ReconciliationDate.GetValueOrDefault(), _receiptVoucherDetailService, _cashMutationService, _cashBankService, _receivableService, _generalLedgerJournalService, _accountService, _closingService);
        //                            Log(obj.Errors, obj.GetType().Name.Split('_')[0], obj.Code, rec.Id, obj.Id);
        //                        }
        //                    }
        //                    break;
        //                };
        //            case TranSource.PaymentVoucher:
        //                {
        //                    var rec = pv.Where(x => x.Id == tran.Value.id && !x.IsDeleted).FirstOrDefault();
        //                    if (rec == null) rec = pv.Where(x => x.Id == tran.Value.id).FirstOrDefault();
        //                    // ReCreate the objects
        //                    var obj = _paymentVoucherService.GetObjectById(rec.Id);
        //                    //PaymentVoucher obj = new PaymentVoucher();
        //                    //Reflection.CopyProperties(rec, obj);
        //                    //obj.IsConfirmed = false;
        //                    //obj.IsReconciled = false;
        //                    //obj.IsDeleted = false;
        //                    //db.Database.ExecuteSqlCommand(string.Format("DBCC CHECKIDENT ({0}, RESEED, {1});", obj.GetType().Name.Split('_')[0], rec.Id - 1));
        //                    //_paymentVoucherService.CreateObject(obj, _paymentVoucherDetailService, _payableService, _contactService, _cashBankService);
        //                    //Log(obj.Errors, obj.GetType().Name.Split('_')[0], obj.Code, rec.Id, obj.Id);
        //                    if (obj != null)
        //                    {
        //                        if (!obj.IsConfirmed)
        //                        {
        //                            // ReCreate the details
        //                            foreach (var det in db.TempPaymentVoucherDetails.Where(x => !x.IsDeleted && x.PaymentVoucherId == rec.Id && !x.Description.Contains("Automatic")).OrderBy(x => x.Id).ToList())
        //                            {
        //                                TempPayable tmp = db.TempPayables.Where(x => !x.IsDeleted && x.Id == det.PayableId).OrderByDescending(x => x.Id).FirstOrDefault();
        //                                if (tmp == null) tmp = db.TempPayables.Where(x => x.Id == det.PayableId).OrderByDescending(x => x.Id).FirstOrDefault();
        //                                PaymentVoucherDetail detobj = new PaymentVoucherDetail();
        //                                Reflection.CopyProperties(det, detobj);
        //                                detobj.IsConfirmed = false;
        //                                detobj.IsDeleted = false;
        //                                var payable = _payableService.GetObjectBySource(tmp.PayableSource, tmp.PayableSourceId);
        //                                if (payable == null) Console.WriteLine("Missing Payable [SourceID={0}, Source={1}] on {2}:Code={3}", tmp.PayableSourceId, tmp.PayableSource, detobj.GetType().Name.Split('_')[0], detobj.Code);
        //                                detobj.PayableId = (payable != null) ? payable.Id : 0;
        //                                db.Database.ExecuteSqlCommand(string.Format("DBCC CHECKIDENT ({0}, RESEED, {1});", detobj.GetType().Name.Split('_')[0], det.Id - 1));
        //                                _paymentVoucherDetailService.CreateObject(detobj, _paymentVoucherService, _cashBankService, _payableService);
        //                                Log(detobj.Errors, detobj.GetType().Name.Split('_')[0], detobj.Code, det.Id, detobj.Id);
        //                            }
        //                            // ReConfirm the objects
        //                            if (reconfirm && rec.IsConfirmed && !obj.IsConfirmed)
        //                            {
        //                                _paymentVoucherService.ConfirmObject(obj, rec.ConfirmationDate.GetValueOrDefault(), _paymentVoucherDetailService, _cashBankService, _payableService, _cashMutationService,
        //                                                            _generalLedgerJournalService, _accountService, _closingService);
        //                                Log(obj.Errors, obj.GetType().Name.Split('_')[0], obj.Code, rec.Id, obj.Id);
        //                            }
        //                        }
        //                        // ReReconcile the objects
        //                        else if (repaid_reconcile && rec.IsReconciled && !obj.IsReconciled)
        //                        {
        //                            _paymentVoucherService.ReconcileObject(obj, rec.ReconciliationDate.GetValueOrDefault(), _paymentVoucherDetailService, _cashMutationService, _cashBankService, _payableService, _generalLedgerJournalService, _accountService, _closingService);
        //                            Log(obj.Errors, obj.GetType().Name.Split('_')[0], obj.Code, rec.Id, obj.Id);
        //                        }
        //                    }
        //                    break;
        //                };
        //            case TranSource.Memorial:
        //                {
        //                    var rec = mem.Where(x => x.Id == tran.Value.id && !x.IsDeleted).FirstOrDefault();
        //                    if (rec == null) rec = mem.Where(x => x.Id == tran.Value.id).FirstOrDefault();
        //                    // ReCreate the objects
        //                    Memorial obj = new Memorial();
        //                    Reflection.CopyProperties(rec, obj);
        //                    obj.IsConfirmed = false;
        //                    obj.IsDeleted = false;
        //                    db.Database.ExecuteSqlCommand(string.Format("DBCC CHECKIDENT ({0}, RESEED, {1});", obj.GetType().Name.Split('_')[0], rec.Id - 1));
        //                    _memorialService.CreateObject(obj);
        //                    Log(obj.Errors, obj.GetType().Name.Split('_')[0], obj.Code, rec.Id, obj.Id);
        //                    if (obj != null)
        //                    {
        //                        // ReCreate the details
        //                        foreach (var det in db.TempMemorialDetails.Where(x => !x.IsDeleted && x.MemorialId == rec.Id).OrderBy(x => x.Id).ToList())
        //                        {
        //                            MemorialDetail detobj = new MemorialDetail();
        //                            Reflection.CopyProperties(det, detobj);
        //                            detobj.IsConfirmed = false;
        //                            detobj.IsDeleted = false;
        //                            db.Database.ExecuteSqlCommand(string.Format("DBCC CHECKIDENT ({0}, RESEED, {1});", detobj.GetType().Name.Split('_')[0], det.Id - 1));
        //                            _memorialDetailService.CreateObject(detobj, _memorialService, _accountService);
        //                            Log(detobj.Errors, detobj.GetType().Name.Split('_')[0], detobj.Code, det.Id, detobj.Id);
        //                        }
        //                        // ReConfirm the objects
        //                        if (reconfirm && rec.IsConfirmed && !obj.IsConfirmed)
        //                        {
        //                            _memorialService.ConfirmObject(obj, rec.ConfirmationDate.GetValueOrDefault(), _memorialDetailService, _accountService, _generalLedgerJournalService, _closingService);
        //                            Log(obj.Errors, obj.GetType().Name.Split('_')[0], obj.Code, rec.Id, obj.Id);
        //                        }
        //                    }
        //                    break;
        //                };
        //        }
        //    }
        //    writer.Close();
        //    fstrm.Close();
        //    return count;
        //}

        //public int Restore(Nullable<DateTime> startDate, Nullable<DateTime> endDate)
        //{
        //    IDictionary<DateTime, sourceid> trans = new Dictionary<DateTime, sourceid>(); // Document DateTime & ID
        //    //IList<KeyValuePair<DateTime, sourceid>> trans = new List<KeyValuePair<DateTime, sourceid>>(); // Document DateTime & ID

        //    var db = new OffsetPrintingSuppliesEntities();
        //    using (db)
        //    {
        //        foreach (var tableName in Enumerable.Reverse(masterDatas))
        //        {
        //            if (tableName == "UserAccess" || tableName == "UserMenu") continue;
        //            var a = db.Database.SqlQuery<string>(string.Format("select '[' + name + '], ' as [text()] from sys.columns where object_id = object_id('Temp{0}') for xml path('')", tableName)).ToList(); // where object_id = object_id('{0}')
        //            string b = "";
        //            foreach (var c in a) b += c;
        //            b = b.Substring(0, b.Length - 2);
        //            // Create new table using source table structure, and then copy the data from source structure to new table
        //            //db.Database.ExecuteSqlCommand(string.Format("CREATE TABLE Temp{0} LIKE {0}; INSERT Temp{0} SELECT * FROM {0};", tableName));
        //            //db.Database.ExecuteSqlCommand(string.Format("CREATE TABLE {0} AS SELECT * FROM Temp{0};", tableName));
        //            db.Database.ExecuteSqlCommand(string.Format("SET IDENTITY_INSERT {0} ON; INSERT INTO {0}({1}) SELECT {1} FROM Temp{0} WHERE 1=1; SET IDENTITY_INSERT {0} OFF;", tableName, b));
        //        }

        //        // Allow defining the ID
        //        foreach (var tableName in userDatas)
        //        {
        //            db.Database.ExecuteSqlCommand(string.Format("SET IDENTITY_INSERT [dbo].[{0}] ON;", tableName));
        //        }

        //        // Restore non-deleted records one-by-one
        //        //var tmps = db.Database.SqlQuery(typeof(TempCashSalesInvoice), "SELECT * FROM TempCashSalesInvoice");

        //        // Account
        //        //FixLegacyAccounts();
        //        foreach (var rec in db.TempAccounts.Where(x => !x.IsDeleted /*&& !x.IsLegacy*/ && !x.IsCashBankAccount).OrderBy(x => x.Code).ToList())
        //        {
        //            Account obj = new Account();
        //            Reflection.CopyProperties(rec, obj);
        //            //var oldparent = db.TempAccounts.Where(x => x.Id == rec.ParentId).FirstOrDefault();
        //            //var newparent = db.Accounts.Where(x => !x.IsDeleted && x.Name == oldparent.Name).OrderByDescending(x => x.Code).FirstOrDefault();
        //            //obj.ParentId = (newparent != null) ? (int?)newparent.Id : null;
        //            //obj.Id = 0;
        //            db.Database.ExecuteSqlCommand(string.Format("DBCC CHECKIDENT ({0}, RESEED, {1});", obj.GetType().Name.Split('_')[0], rec.Id - 1)); //using same ID for user's account have a possibility to use already existing ID if there are more legacy accounts than before
        //            _accountService.CreateObject(obj);
        //            Log(obj.Errors, obj.GetType().Name.Split('_')[0], obj.Name, rec.Id, obj.Id);
        //        }
        //        //Try to fix nulled ParentId
        //        //foreach (var obj in db.Accounts.Where(x => !x.IsDeleted && !x.IsLegacy && !x.IsCashBankAccount).OrderBy(x => x.Id).ToList())
        //        //{
        //        //    if (obj.ParentId == null)
        //        //    {
        //        //        var rec = db.TempAccounts.Where(x => x.Name == obj.Name && !x.IsDeleted && !x.IsLegacy && !x.IsCashBankAccount).OrderBy(x => x.Id).FirstOrDefault();
        //        //        var oldparent = db.TempAccounts.Where(x => x.Id == rec.ParentId).OrderByDescending(x => x.Id).FirstOrDefault();
        //        //        var newparent = db.Accounts.Where(x => !x.IsDeleted && x.Name == oldparent.Name).OrderByDescending(x => x.Id).FirstOrDefault();
        //        //        obj.ParentId = newparent.Id;
        //        //        _accountService.UpdateObject(obj, null);
        //        //    }
        //        //}
        //        // Strange Behaviour http://stackoverflow.com/questions/472578/dbcc-checkident-sets-identity-to-0
        //        int? mxid = db.Database.SqlQuery<int?>(string.Format("SELECT MAX(Id) FROM {0}", "Account")).FirstOrDefault();
        //        if (mxid == null || mxid <= 0) mxid = 1; // if RESEED=0, ID will start from 0 if it's newly created table (don't have record yet), if it already have record RESEED=0 will start from ID=1
        //        db.Database.ExecuteSqlCommand(string.Format("DBCC CHECKIDENT ('{0}', RESEED, {1});", "Account", mxid.GetValueOrDefault()));
        //        db.Database.ExecuteSqlCommand(string.Format("SET IDENTITY_INSERT [dbo].[{0}] OFF;", "Account"));

        //        // CashBank
        //        foreach (var rec in db.TempCashBanks.Where(x => !x.IsDeleted).OrderBy(x => x.Id).ToList())
        //        {
        //            CashBank obj = new CashBank();
        //            Reflection.CopyProperties(rec, obj);
        //            obj.Amount = 0;
        //            db.Database.ExecuteSqlCommand(string.Format("DBCC CHECKIDENT ({0}, RESEED, {1});", obj.GetType().Name.Split('_')[0], rec.Id - 1));
        //            _cashBankService.CreateObject(obj, _accountService);
        //            Log(obj.Errors, obj.GetType().Name.Split('_')[0], obj.Name, rec.Id, obj.Id);
        //        }

        //        //FixAccounts();

        //        // Item
        //        foreach (var rec in db.TempItems.Where(x => !x.IsDeleted).OrderBy(x => x.Id).ToList())
        //        {
        //            Item obj = new Item();
        //            Reflection.CopyProperties(rec, obj);
        //            obj.Quantity = 0;
        //            obj.PendingDelivery = 0;
        //            obj.PendingReceival = 0;
        //            obj.AvgPrice = 0;
        //            db.Database.ExecuteSqlCommand(string.Format("DBCC CHECKIDENT ({0}, RESEED, {1});", obj.GetType().Name.Split('_')[0], rec.Id - 1));
        //            _itemService.CreateObject(obj, _uomService, _itemTypeService, _warehouseItemService, _warehouseService, _priceMutationService, _contactGroupService);
        //            Log(obj.Errors, obj.GetType().Name.Split('_')[0], obj.Sku, rec.Id, obj.Id);
        //        }

        //        // WarehouseItem
        //        foreach (var rec in db.TempWarehouseItems.Where(x => !x.IsDeleted).OrderBy(x => x.Id).ToList())
        //        {
        //            WarehouseItem obj = new WarehouseItem();
        //            Reflection.CopyProperties(rec, obj);
        //            obj.Quantity = 0;
        //            obj.PendingDelivery = 0;
        //            obj.PendingReceival = 0;
        //            db.Database.ExecuteSqlCommand(string.Format("DBCC CHECKIDENT ({0}, RESEED, {1});", obj.GetType().Name.Split('_')[0], rec.Id - 1));
        //            _warehouseItemService.CreateObject(obj, _warehouseService, _itemService);
        //            string objcode = _warehouseService.GetObjectById(obj.WarehouseId).Code;
        //            Log(obj.Errors, obj.GetType().Name.Split('_')[0], objcode, rec.Id, obj.Id);
        //        }

        //        // StockAdjustment
        //        foreach (var rec in db.TempStockAdjustments.Where(x => !x.IsDeleted).OrderBy(x => x.Id).ToList())
        //        {
        //            var obj = new StockAdjustment();
        //            Reflection.CopyProperties(rec, obj);
        //            DateTime confirmdate = rec.ConfirmationDate ?? rec.AdjustmentDate;
        //            trans.Add(confirmdate.AddMilliseconds(rec.Id + 10000), new sourceid() { source = obj.GetType().Name.Split('_')[0], id = rec.Id });
        //        }

        //        // Warehouse Mutation
        //        foreach (var rec in db.TempWarehouseMutationOrders.Where(x => !x.IsDeleted).OrderBy(x => x.Id).ToList())
        //        {
        //            var obj = new WarehouseMutationOrder();
        //            Reflection.CopyProperties(rec, obj);
        //            DateTime confirmdate = rec.ConfirmationDate ?? rec.MutationDate;
        //            trans.Add(confirmdate.AddMilliseconds(rec.Id + 20000), new sourceid() { source = obj.GetType().Name.Split('_')[0], id = rec.Id });
        //        }

        //        // CashBankAdjustment
        //        foreach (var rec in db.TempCashBankAdjustments.Where(x => !x.IsDeleted).OrderBy(x => x.Id).ToList())
        //        {
        //            var obj = new CashBankAdjustment();
        //            Reflection.CopyProperties(rec, obj);
        //            DateTime confirmdate = rec.ConfirmationDate ?? rec.AdjustmentDate;
        //            trans.Add(confirmdate.AddMilliseconds(rec.Id + 30000), new sourceid() { source = obj.GetType().Name.Split('_')[0], id = rec.Id });
        //        }

        //        // CashBankMutation
        //        foreach (var rec in db.TempCashBankMutations.Where(x => !x.IsDeleted).OrderBy(x => x.Id).ToList())
        //        {
        //            var obj = new CashBankMutation();
        //            Reflection.CopyProperties(rec, obj);
        //            if (rec.ConfirmationDate == null)
        //            {
        //                Console.WriteLine("WARNING : Unknown Confirmation Date on CashBankMutation[{0}]:{1} ! Need to be Confirmed Manually", rec.Id, rec.Code);
        //            }
        //            //DateTime confirmdate = rec.ConfirmationDate ?? rec.MutationDate;
        //            trans.Add(rec.ConfirmationDate.GetValueOrDefault().AddMilliseconds(rec.Id + 40000), new sourceid() { source = obj.GetType().Name.Split('_')[0], id = rec.Id });
        //        }

        //        // CustomPurchaseInvoice
        //        foreach (var rec in db.TempCustomPurchaseInvoices.Where(x => !x.IsDeleted).OrderBy(x => x.Id).ToList())
        //        {
        //            var obj = new CustomPurchaseInvoice();
        //            Reflection.CopyProperties(rec, obj);
        //            DateTime confirmdate = rec.ConfirmationDate ?? rec.PurchaseDate;
        //            trans.Add(confirmdate.AddMilliseconds(rec.Id + 50000), new sourceid() { source = obj.GetType().Name.Split('_')[0], id = rec.Id });
        //            if (rec.IsPaid)
        //            {
        //                trans.Add(rec.PaymentDate.GetValueOrDefault().AddMilliseconds(rec.Id + 50500), new sourceid() { source = obj.GetType().Name.Split('_')[0], id = rec.Id });
        //            }
        //        }
                
        //        // CashSalesInvoice
        //        foreach (var rec in db.TempCashSalesInvoices.Where(x => !x.IsDeleted).OrderBy(x => x.Id).ToList())
        //        {
        //            var obj = new CashSalesInvoice();
        //            Reflection.CopyProperties(rec, obj);
        //            DateTime confirmdate = rec.ConfirmationDate ?? rec.SalesDate;
        //            trans.Add(confirmdate.AddMilliseconds(rec.Id + 60000), new sourceid() { source = obj.GetType().Name.Split('_')[0], id = rec.Id });
        //            if (rec.IsPaid)
        //            {
        //                trans.Add(rec.PaymentDate.GetValueOrDefault().AddMilliseconds(rec.Id + 60500), new sourceid() { source = obj.GetType().Name.Split('_')[0], id = rec.Id });
        //            }
        //        }
                
        //        // ReceiptVoucherDetail
        //        foreach (var det in db.TempReceiptVoucherDetails.Where(x => !x.IsDeleted && !x.Description.Contains("Automatic")).OrderBy(x => x.Id).ToList())
        //        {
        //            TempReceiptVoucher rec = db.TempReceiptVouchers.Where(x => !x.IsDeleted && x.Id == det.ReceiptVoucherId).OrderByDescending(x => x.Id).FirstOrDefault();
        //            if (rec == null) rec = db.TempReceiptVouchers.Where(x => x.Id == det.ReceiptVoucherId).OrderByDescending(x => x.Id).FirstOrDefault();
        //            ReceiptVoucher obj = _receiptVoucherService.GetObjectById(rec.Id);
        //            if (obj == null)
        //            {
        //                obj = new ReceiptVoucher();
        //                Reflection.CopyProperties(rec, obj);
        //                obj.IsConfirmed = false;
        //                obj.IsReconciled = false;
        //                obj.IsDeleted = false;
        //                db.Database.ExecuteSqlCommand(string.Format("DBCC CHECKIDENT ({0}, RESEED, {1});", obj.GetType().Name.Split('_')[0], rec.Id - 1));
        //                _receiptVoucherService.CreateObject(obj, _receiptVoucherDetailService, _receivableService, _contactService, _cashBankService);
        //                Log(obj.Errors, obj.GetType().Name.Split('_')[0], obj.Code, rec.Id, obj.Id);
        //                DateTime confirmdate = rec.ConfirmationDate ?? rec.ReceiptDate;
        //                trans.Add(confirmdate.AddMilliseconds(rec.Id + 70000), new sourceid() { source = obj.GetType().Name.Split('_')[0], id = rec.Id });
        //                if (rec.IsReconciled)
        //                {
        //                    trans.Add(rec.ReconciliationDate.GetValueOrDefault().AddMilliseconds(rec.Id + 70500), new sourceid() { source = obj.GetType().Name.Split('_')[0], id = rec.Id });
        //                }
        //            }
        //        }
        //        // ReceiptVoucher
        //        //foreach (var rec in db.TempReceiptVouchers.Where(x => !x.IsDeleted).OrderBy(x => x.Id).ToList())
        //        //{
        //        //    ReceiptVoucher obj = _receiptVoucherService.GetObjectById(rec.Id);
        //        //    if (obj != null)
        //        //    {
        //        //        if (rec.IsConfirmed && !obj.IsConfirmed)
        //        //        {
        //        //            _receiptVoucherService.ConfirmObject(obj, rec.ConfirmationDate.GetValueOrDefault(), _receiptVoucherDetailService, _cashBankService, _receivableService, _cashMutationService,
        //        //                                        _generalLedgerJournalService, _accountService, _closingService);
        //        //            Log(obj.Errors, obj.GetType().Name.Split('_')[0], obj.Code, rec.Id, obj.Id);
        //        //        }
        //        //        if (rec.IsReconciled && !obj.IsReconciled)
        //        //        {
        //        //            _receiptVoucherService.ReconcileObject(obj, rec.ReconciliationDate.GetValueOrDefault(), _receiptVoucherDetailService, _cashMutationService, _cashBankService, _receivableService, _generalLedgerJournalService, _accountService, _closingService);
        //        //            Log(obj.Errors, obj.GetType().Name.Split('_')[0], obj.Code, rec.Id, obj.Id);
        //        //        }
        //        //    }
        //        //}

        //        // CashSalesReturn
        //        foreach (var rec in db.TempCashSalesReturns.Where(x => !x.IsDeleted).OrderBy(x => x.Id).ToList())
        //        {
        //            var obj = new CashSalesReturn();
        //            Reflection.CopyProperties(rec, obj);
        //            DateTime confirmdate = rec.ConfirmationDate ?? rec.ReturnDate.GetValueOrDefault();
        //            trans.Add(confirmdate.AddMilliseconds(rec.Id + 80000), new sourceid() { source = obj.GetType().Name.Split('_')[0], id = rec.Id });
        //            if (rec.IsPaid)
        //            {
        //                trans.Add(rec.PaymentDate.GetValueOrDefault().AddMilliseconds(rec.Id + 80500), new sourceid() { source = obj.GetType().Name.Split('_')[0], id = rec.Id });
        //            }
        //        }

        //        // PaymentRequest
        //        foreach (var rec in db.TempPaymentRequests.Where(x => !x.IsDeleted).OrderBy(x => x.Id).ToList())
        //        {
        //            var obj = new PaymentRequest();
        //            Reflection.CopyProperties(rec, obj);
        //            DateTime confirmdate = rec.ConfirmationDate ?? rec.RequestedDate;
        //            trans.Add(confirmdate.AddMilliseconds(rec.Id + 90000), new sourceid() { source = obj.GetType().Name.Split('_')[0], id = rec.Id });
        //        }

        //        // PaymentVoucherDetail
        //        foreach (var det in db.TempPaymentVoucherDetails.Where(x => !x.IsDeleted && !x.Description.Contains("Automatic")).OrderBy(x => x.Id).ToList())
        //        {
        //            TempPaymentVoucher rec = db.TempPaymentVouchers.Where(x => !x.IsDeleted && x.Id == det.PaymentVoucherId).OrderByDescending(x => x.Id).FirstOrDefault();
        //            if (rec == null) rec = db.TempPaymentVouchers.Where(x => x.Id == det.PaymentVoucherId).OrderByDescending(x => x.Id).FirstOrDefault();
        //            PaymentVoucher obj = _paymentVoucherService.GetObjectById(rec.Id);
        //            if (obj == null)
        //            {
        //                obj = new PaymentVoucher();
        //                Reflection.CopyProperties(rec, obj);
        //                obj.IsConfirmed = false;
        //                obj.IsReconciled = false;
        //                obj.IsDeleted = false;
        //                db.Database.ExecuteSqlCommand(string.Format("DBCC CHECKIDENT ({0}, RESEED, {1});", obj.GetType().Name.Split('_')[0], rec.Id - 1));
        //                _paymentVoucherService.CreateObject(obj, _paymentVoucherDetailService, _payableService, _contactService, _cashBankService);
        //                Log(obj.Errors, obj.GetType().Name.Split('_')[0], obj.Code, rec.Id, obj.Id);
        //                DateTime confirmdate = rec.ConfirmationDate ?? rec.PaymentDate;
        //                trans.Add(confirmdate.AddMilliseconds(rec.Id + 100000), new sourceid() { source = obj.GetType().Name.Split('_')[0], id = rec.Id });
        //                if (rec.IsReconciled)
        //                {
        //                    trans.Add(rec.ReconciliationDate.GetValueOrDefault().AddMilliseconds(rec.Id + 100500), new sourceid() { source = obj.GetType().Name.Split('_')[0], id = rec.Id });
        //                }
        //            }
        //        }
        //        // PaymentVoucher
        //        //foreach (var rec in db.TempPaymentVouchers.Where(x => !x.IsDeleted).OrderBy(x => x.Id).ToList())
        //        //{
        //        //    PaymentVoucher obj = _paymentVoucherService.GetObjectById(rec.Id);
        //        //    if (obj != null)
        //        //    {
        //        //        if (rec.IsConfirmed && !obj.IsConfirmed)
        //        //        {
        //        //            _paymentVoucherService.ConfirmObject(obj, rec.ConfirmationDate.GetValueOrDefault(), _paymentVoucherDetailService, _cashBankService, _payableService, _cashMutationService,
        //        //                                        _generalLedgerJournalService, _accountService, _closingService);
        //        //            Log(obj.Errors, obj.GetType().Name.Split('_')[0], obj.Code, rec.Id, obj.Id);
        //        //        }
        //        //        if (rec.IsReconciled && !obj.IsReconciled)
        //        //        {
        //        //            _paymentVoucherService.ReconcileObject(obj, rec.ReconciliationDate.GetValueOrDefault(), _paymentVoucherDetailService, _cashMutationService, _cashBankService, _payableService, _generalLedgerJournalService, _accountService, _closingService);
        //        //            Log(obj.Errors, obj.GetType().Name.Split('_')[0], obj.Code, rec.Id, obj.Id);
        //        //        }
        //        //    }
        //        //}
                
        //        // Memorial
        //        foreach (var rec in db.TempMemorials.Where(x => !x.IsDeleted).OrderBy(x => x.Id).ToList())
        //        {
        //            var obj = new Memorial();
        //            Reflection.CopyProperties(rec, obj);
        //            if (rec.ConfirmationDate == null)
        //            {
        //                Console.WriteLine("WARNING : Unknown Confirmation Date on Memorial[{0}]:{1} ! Need to be Confirmed Manually", rec.Id, rec.Code);
        //            }
        //            //DateTime confirmdate = rec.ConfirmationDate ?? rec.RequestedDate;
        //            trans.Add(rec.ConfirmationDate.GetValueOrDefault().AddMilliseconds(rec.Id + 110000), new sourceid() { source = obj.GetType().Name.Split('_')[0], id = rec.Id });
        //        }

        //        // Sort and Process Purchases/Sales
        //        OrderedRestoration(db, trans, true, true);

        //        // DisAllow defining the ID (also Set SEED to the highest Id)
        //        foreach (var tableName in userDatas)
        //        {
        //            // Strange Behaviour http://stackoverflow.com/questions/472578/dbcc-checkident-sets-identity-to-0
        //            int? maxid = db.Database.SqlQuery<int?>(string.Format("SELECT MAX(Id) FROM {0}", tableName)).FirstOrDefault();
        //            if (maxid == null || maxid <= 0) maxid = 1; // if RESEED=0, ID will start from 0 if it's newly created table (don't have record yet), if it already have record RESEED=0 will start from ID=1
        //            db.Database.ExecuteSqlCommand(string.Format("DBCC CHECKIDENT ('{0}', RESEED, {1});", tableName, maxid.GetValueOrDefault()));
        //            db.Database.ExecuteSqlCommand(string.Format("SET IDENTITY_INSERT [dbo].[{0}] OFF;", tableName));
        //        }

        //    }
            return 0;
        }

        public int OrderedConfirmation(OffsetPrintingSuppliesEntities db, IDictionary<DateTime, sourceid> trans, bool reconfirm, bool repaid_reconcile)
        {
            int count = 0;
            // Sort and Process Documents
            //var sa = db.StockAdjustments.OrderByDescending(x => x.Id);
            
            if (File.Exists(@"./OrderLog.txt"))
            {
                File.Delete(@"./OrderLog.txt");
            }
            var fstrm = new FileStream(@"./OrderLog.txt", FileMode.OpenOrCreate, FileAccess.Write);
            var writer = new StreamWriter(fstrm);
            int idx = 0;
            // Loop from old to new
            foreach (var tran in trans.OrderBy(x => x.Key))
            {
                idx++;
                writer.WriteLine(idx.ToString("D5") + tran.Key.ToString(" [dd/MM/yy] : ") + tran.Value.source + " [" + tran.Value.id.ToString() + "]");
                switch (tran.Value.source.ToLower())
                {
                    case StockMutationTranSource.StockAdjustment:
                        {
                            var obj = db.StockAdjustments.Where(x => x.Id == tran.Value.id && !x.IsDeleted).OrderByDescending(x => x.Id).FirstOrDefault();
                            if (obj == null) obj = db.StockAdjustments.Where(x => x.Id == tran.Value.id).FirstOrDefault();
                            if (obj != null)
                            {
                                // ReConfirm the objects
                                if (reconfirm && obj.IsConfirmed)
                                {
                                    foreach (var det in db.StockAdjustmentDetails.Where(x => !x.IsDeleted && x.StockAdjustmentId == obj.Id).ToList())
                                    {
                                        det.IsConfirmed = false;
                                        db.Entry(det).State = EntityState.Modified;
                                        db.SaveChanges();
                                    }

                                    obj.IsConfirmed = false;
                                    obj.IsDeleted = false;
                                    obj.Errors = new Dictionary<string, string>();
                                    //db.Database.ExecuteSqlCommand(string.Format("DBCC CHECKIDENT ({0}, RESEED, {1});", obj.GetType().Name.Split('_')[0], rec.Id - 1));
                                    _stockAdjustmentService.ConfirmObject(obj, obj.ConfirmationDate.GetValueOrDefault(), _stockAdjustmentDetailService, _stockMutationService, _itemService, _itemTypeService, _blanketService, _warehouseItemService, _accountService, _generalLedgerJournalService, _closingService);
                                    Log(obj.Errors, obj.GetType().Name.Split('_')[0], obj.Code, obj.Id, obj.Id);
                                }
                            }
                            break;
                        };
                    case StockMutationTranSource.CustomerStockAdjustment:
                        {
                            var obj = db.CustomerStockAdjustments.Where(x => x.Id == tran.Value.id && !x.IsDeleted).OrderByDescending(x => x.Id).FirstOrDefault();
                            if (obj == null) obj = db.CustomerStockAdjustments.Where(x => x.Id == tran.Value.id).FirstOrDefault();
                            if (obj != null)
                            {
                                // ReConfirm the objects
                                if (reconfirm && obj.IsConfirmed)
                                {
                                    foreach (var det in db.CustomerStockAdjustmentDetails.Where(x => !x.IsDeleted && x.CustomerStockAdjustmentId == obj.Id).ToList())
                                    {
                                        det.IsConfirmed = false;
                                        db.Entry(det).State = EntityState.Modified;
                                        db.SaveChanges();
                                    }

                                    obj.IsConfirmed = false;
                                    obj.IsDeleted = false;
                                    obj.Errors = new Dictionary<string, string>();
                                    //db.Database.ExecuteSqlCommand(string.Format("DBCC CHECKIDENT ({0}, RESEED, {1});", obj.GetType().Name.Split('_')[0], rec.Id - 1));
                                    _customerStockAdjustmentService.ConfirmObject(obj, obj.ConfirmationDate.GetValueOrDefault(), _customerStockAdjustmentDetailService, _customerStockMutationService, _itemService, _itemTypeService, _customerItemService, _warehouseItemService, _accountService, _generalLedgerJournalService, _closingService);
                                    Log(obj.Errors, "CustomerStockAdjustment", obj.Code, obj.Id, obj.Id);
                                }
                            }
                            break;
                        };
                    case StockMutationTranSource.PurchaseOrder:
                        {
                            var obj = db.PurchaseOrders.Where(x => x.Id == tran.Value.id && !x.IsDeleted).OrderByDescending(x => x.Id).FirstOrDefault();
                            if (obj == null) obj = db.PurchaseOrders.Where(x => x.Id == tran.Value.id).FirstOrDefault();
                            if (obj != null)
                            {
                                // ReConfirm the objects
                                if (reconfirm && obj.IsConfirmed)
                                {
                                    foreach (var det in db.PurchaseOrderDetails.Where(x => !x.IsDeleted && x.PurchaseOrderId == obj.Id).ToList())
                                    {
                                        det.IsConfirmed = false;
                                        det.IsAllReceived = false;
                                        det.PendingReceivalQuantity = det.Quantity;
                                        db.Entry(det).State = EntityState.Modified;
                                        db.SaveChanges();
                                    }

                                    obj.IsConfirmed = false;
                                    obj.IsDeleted = false;
                                    obj.IsReceivalCompleted = false;
                                    obj.Errors = new Dictionary<string, string>();
                                    //db.Database.ExecuteSqlCommand(string.Format("DBCC CHECKIDENT ({0}, RESEED, {1});", obj.GetType().Name.Split('_')[0], rec.Id - 1));
                                    _purchaseOrderService.ConfirmObject(obj, obj.ConfirmationDate.GetValueOrDefault(), _purchaseOrderDetailService, _stockMutationService, _itemService, _blanketService, _warehouseItemService);
                                    Log(obj.Errors, obj.GetType().Name.Split('_')[0], obj.Code, obj.Id, obj.Id);
                                }
                            }
                            break;
                        };
                    case StockMutationTranSource.PurchaseReceival:
                        {
                            var obj = db.PurchaseReceivals.Where(x => x.Id == tran.Value.id && !x.IsDeleted).OrderByDescending(x => x.Id).FirstOrDefault();
                            if (obj == null) obj = db.PurchaseReceivals.Where(x => x.Id == tran.Value.id).FirstOrDefault();
                            if (obj != null)
                            {
                                // ReConfirm the objects
                                if (reconfirm && obj.IsConfirmed)
                                {
                                    foreach (var det in db.PurchaseReceivalDetails.Where(x => !x.IsDeleted && x.PurchaseReceivalId == obj.Id).ToList())
                                    {
                                        det.IsConfirmed = false;
                                        det.IsAllInvoiced = false;
                                        det.PendingInvoicedQuantity = det.Quantity;
                                        db.Entry(det).State = EntityState.Modified;
                                        db.SaveChanges();
                                    }

                                    obj.IsConfirmed = false;
                                    obj.IsDeleted = false;
                                    obj.IsInvoiceCompleted = false;
                                    obj.Errors = new Dictionary<string, string>();
                                    //db.Database.ExecuteSqlCommand(string.Format("DBCC CHECKIDENT ({0}, RESEED, {1});", obj.GetType().Name.Split('_')[0], rec.Id - 1));
                                    _purchaseReceivalService.ConfirmObject(obj, obj.ConfirmationDate.GetValueOrDefault(), _purchaseReceivalDetailService, _purchaseOrderService, _purchaseOrderDetailService, _stockMutationService, _itemService, _itemTypeService, _blanketService, _warehouseItemService, _accountService, _generalLedgerJournalService, _closingService, _currencyService, _exchangeRateService);
                                    Log(obj.Errors, obj.GetType().Name.Split('_')[0], obj.Code, obj.Id, obj.Id);
                                }
                            }
                            break;
                        };
                    case StockMutationTranSource.PurchaseInvoice:
                        {
                            var obj = db.PurchaseInvoices.Where(x => x.Id == tran.Value.id && !x.IsDeleted).OrderByDescending(x => x.Id).FirstOrDefault();
                            if (obj == null) obj = db.PurchaseInvoices.Where(x => x.Id == tran.Value.id).FirstOrDefault();
                            if (obj != null)
                            {
                                // ReConfirm the objects
                                if (reconfirm && obj.IsConfirmed)
                                {
                                    foreach (var det in db.PurchaseInvoiceDetails.Where(x => !x.IsDeleted && x.PurchaseInvoiceId == obj.Id).ToList())
                                    {
                                        det.IsConfirmed = false;
                                        db.Entry(det).State = EntityState.Modified;
                                        db.SaveChanges();
                                    }

                                    obj.IsConfirmed = false;
                                    obj.IsDeleted = false;
                                    obj.Errors = new Dictionary<string, string>();
                                    var payable = db.Payables.Where(x => !x.IsDeleted && x.PayableSource == Constant.PayableSource.PurchaseInvoice && x.PayableSourceId == obj.Id).OrderByDescending(x => x.Id).FirstOrDefault();
                                    var id = payable.Id;
                                    var tbl = payable.GetType().Name.Split('_')[0];
                                    db.Payables.Remove(payable);
                                    db.Database.ExecuteSqlCommand(string.Format("DBCC CHECKIDENT ({0}, RESEED, {1});", tbl, id - 1));
                                    _purchaseInvoiceService.ConfirmObject(obj, obj.ConfirmationDate.GetValueOrDefault(), _purchaseInvoiceDetailService, _purchaseOrderService, _purchaseReceivalService, _purchaseReceivalDetailService, _payableService, _accountService, _generalLedgerJournalService, _closingService, _currencyService, _exchangeRateService, _gLNonBaseCurrencyService);
                                    Log(obj.Errors, obj.GetType().Name.Split('_')[0], obj.Code, obj.Id, obj.Id);
                                }
                            }
                            break;
                        };
                    case StockMutationTranSource.Repacking:
                        {
                            var obj = db.Repackings.Where(x => x.Id == tran.Value.id && !x.IsDeleted).OrderByDescending(x => x.Id).FirstOrDefault();
                            if (obj == null) obj = db.Repackings.Where(x => x.Id == tran.Value.id).FirstOrDefault();
                            if (obj != null)
                            {
                                // ReConfirm the objects
                                if (reconfirm && obj.IsConfirmed)
                                {
                                    obj.IsConfirmed = false;
                                    obj.IsDeleted = false;
                                    obj.Errors = new Dictionary<string, string>();
                                    //db.Database.ExecuteSqlCommand(string.Format("DBCC CHECKIDENT ({0}, RESEED, {1});", obj.GetType().Name.Split('_')[0], rec.Id - 1));
                                    _repackingService.ConfirmObject(obj, obj.ConfirmationDate.GetValueOrDefault(), _blendingRecipeService, _blendingRecipeDetailService, _stockMutationService, _blanketService, _itemService, _itemTypeService, _warehouseItemService, _generalLedgerJournalService, _accountService, _closingService);
                                    Log(obj.Errors, obj.GetType().Name.Split('_')[0], obj.Code, obj.Id, obj.Id);
                                }
                            }
                            break;
                        };
                    case StockMutationTranSource.BlendingWorkOrder:
                        {
                            var obj = db.BlendingWorkOrders.Where(x => x.Id == tran.Value.id && !x.IsDeleted).OrderByDescending(x => x.Id).FirstOrDefault();
                            if (obj == null) obj = db.BlendingWorkOrders.Where(x => x.Id == tran.Value.id).FirstOrDefault();
                            if (obj != null)
                            {
                                // ReConfirm the objects
                                if (reconfirm && obj.IsConfirmed)
                                {
                                    obj.IsConfirmed = false;
                                    obj.IsDeleted = false;
                                    obj.Errors = new Dictionary<string, string>();
                                    //db.Database.ExecuteSqlCommand(string.Format("DBCC CHECKIDENT ({0}, RESEED, {1});", obj.GetType().Name.Split('_')[0], rec.Id - 1));
                                    _blendingWorkOrderService.ConfirmObject(obj, obj.ConfirmationDate.GetValueOrDefault(), _blendingRecipeService, _blendingRecipeDetailService, _stockMutationService, _blanketService, _itemService, _itemTypeService, _warehouseItemService, _generalLedgerJournalService, _accountService, _closingService);
                                    Log(obj.Errors, obj.GetType().Name.Split('_')[0], obj.Code, obj.Id, obj.Id);
                                }
                            }
                            break;
                        };
                    case StockMutationTranSource.CoreIdentification:
                        {
                            var obj = db.CoreIdentifications.Where(x => x.Id == tran.Value.id && !x.IsDeleted).OrderByDescending(x => x.Id).FirstOrDefault();
                            if (obj == null) obj = db.CoreIdentifications.Where(x => x.Id == tran.Value.id).FirstOrDefault();
                            if (obj != null)
                            {
                                // ReConfirm the objects
                                if (reconfirm && obj.IsConfirmed)
                                {
                                    foreach (var det in db.CoreIdentificationDetails.Where(x => !x.IsDeleted && x.CoreIdentificationId == obj.Id).ToList())
                                    {
                                        det.IsConfirmed = false;
                                        det.IsDelivered = false;
                                        det.IsJobScheduled = false;
                                        det.IsRollerBuilt = false;
                                        db.Entry(det).State = EntityState.Modified;
                                        db.SaveChanges();
                                    }
                                    obj.IsConfirmed = false;
                                    obj.IsDeleted = false;
                                    obj.Errors = new Dictionary<string, string>();
                                    //db.Database.ExecuteSqlCommand(string.Format("DBCC CHECKIDENT ({0}, RESEED, {1});", obj.GetType().Name.Split('_')[0], rec.Id - 1));
                                    _coreIdentificationService.ConfirmObject(obj, obj.ConfirmationDate.GetValueOrDefault(), _coreIdentificationDetailService, _stockMutationService, _recoveryOrderService, _recoveryOrderDetailService, _coreBuilderService, _itemService, _warehouseItemService, _blanketService, _customerStockMutationService, _customerItemService);
                                    Log(obj.Errors, obj.GetType().Name.Split('_')[0], obj.Code, obj.Id, obj.Id);
                                }
                            }
                            break;
                        };
                    case StockMutationTranSource.RecoveryOrder:
                        {
                            var obj = db.RecoveryOrders.Where(x => x.Id == tran.Value.id && !x.IsDeleted).OrderByDescending(x => x.Id).FirstOrDefault();
                            if (obj == null) obj = db.RecoveryOrders.Where(x => x.Id == tran.Value.id).FirstOrDefault();
                            if (obj != null)
                            {
                                // ReConfirm the objects
                                if (reconfirm && obj.IsConfirmed)
                                {
                                    //foreach (var det in obj.RecoveryOrderDetails.Where(x => !x.IsDeleted && x.RecoveryOrderId == obj.Id).ToList())
                                    //{
                                    //    det.IsJobScheduled = false;
                                    //}

                                    obj.IsConfirmed = false;
                                    obj.IsDeleted = false;
                                    obj.IsCompleted = false;
                                    obj.QuantityFinal = 0;
                                    obj.QuantityRejected = 0;
                                    obj.Errors = new Dictionary<string, string>();
                                    //db.Database.ExecuteSqlCommand(string.Format("DBCC CHECKIDENT ({0}, RESEED, {1});", tbl, id - 1));
                                    _recoveryOrderService.ConfirmObject(obj, obj.ConfirmationDate.GetValueOrDefault(), _coreIdentificationDetailService, _coreIdentificationService, _recoveryOrderDetailService, _recoveryAccessoryDetailService, _coreBuilderService, _stockMutationService, _itemService, _blanketService, _warehouseItemService, _warehouseService);
                                    Log(obj.Errors, obj.GetType().Name.Split('_')[0], obj.Code, obj.Id, obj.Id);
                                }
                            }
                            break;
                        };
                    case StockMutationTranSource.RecoveryOrderDetail:
                        {
                            var det = db.RecoveryOrderDetails.Where(x => x.Id == tran.Value.id && !x.IsDeleted).OrderByDescending(x => x.Id).FirstOrDefault();
                            if (det == null) det = db.RecoveryOrderDetails.Where(x => x.Id == tran.Value.id).FirstOrDefault();
                            if (det != null)
                            {
                                // ReConfirm the objects
                                if (det.IsFinished)
                                {
                                    det.IsFinished = false;
                                    det.Errors = new Dictionary<string, string>();
                                    //db.Database.ExecuteSqlCommand(string.Format("DBCC CHECKIDENT ({0}, RESEED, {1});", tbl, id - 1));
                                    _recoveryOrderDetailService.FinishObject(det, det.FinishedDate.GetValueOrDefault(), _coreIdentificationService, _coreIdentificationDetailService, _recoveryOrderService, _recoveryAccessoryDetailService, _coreBuilderService, _rollerBuilderService, _itemService, _itemTypeService, _warehouseItemService, _blanketService, _stockMutationService, _accountService, _generalLedgerJournalService, _closingService, _serviceCostService, _customerStockMutationService, _customerItemService);
                                    Log(det.Errors, det.GetType().Name.Split('_')[0], "--", det.Id, det.Id);
                                }
                                else if (det.IsRejected)
                                {
                                    det.IsRejected = false;
                                    det.Errors = new Dictionary<string, string>();
                                    //db.Database.ExecuteSqlCommand(string.Format("DBCC CHECKIDENT ({0}, RESEED, {1});", tbl, id - 1));
                                    _recoveryOrderDetailService.RejectObject(det, det.RejectedDate.GetValueOrDefault(), _coreIdentificationService, _coreIdentificationDetailService, _recoveryOrderService, _recoveryAccessoryDetailService, _coreBuilderService, _rollerBuilderService, _itemService, _itemTypeService, _warehouseItemService, _blanketService, _stockMutationService, _accountService, _generalLedgerJournalService, _closingService);
                                    Log(det.Errors, det.GetType().Name.Split('_')[0], "--", det.Id, det.Id);
                                }
                            }
                            break;
                        };
                    case StockMutationTranSource.BlanketOrder:
                        {
                            var obj = db.BlanketOrders.Where(x => x.Id == tran.Value.id && !x.IsDeleted).OrderByDescending(x => x.Id).FirstOrDefault();
                            if (obj == null) obj = db.BlanketOrders.Where(x => x.Id == tran.Value.id).FirstOrDefault();
                            if (obj != null)
                            {
                                // ReConfirm the objects
                                if (reconfirm && obj.IsConfirmed)
                                {
                                    //foreach (var det in obj.BlanketOrderDetails.Where(x => !x.IsDeleted && x.BlanketOrderId == obj.Id).ToList())
                                    //{
                                    //    det.IsJobScheduled = false;
                                    //}

                                    obj.IsConfirmed = false;
                                    obj.IsDeleted = false;
                                    obj.IsCompleted = false;
                                    obj.QuantityFinal = 0;
                                    obj.QuantityRejected = 0;
                                    obj.Errors = new Dictionary<string, string>();
                                    //db.Database.ExecuteSqlCommand(string.Format("DBCC CHECKIDENT ({0}, RESEED, {1});", tbl, id - 1));
                                    _blanketOrderService.ConfirmObject(obj, obj.ConfirmationDate.GetValueOrDefault(), _blanketOrderDetailService, _blanketService, _itemService, _warehouseItemService);
                                    Log(obj.Errors, obj.GetType().Name.Split('_')[0], obj.Code, obj.Id, obj.Id);
                                }
                            }
                            break;
                        };
                    case StockMutationTranSource.BlanketOrderDetail:
                        {
                            var det = db.BlanketOrderDetails.Where(x => x.Id == tran.Value.id && !x.IsDeleted).OrderByDescending(x => x.Id).FirstOrDefault();
                            if (det == null) det = db.BlanketOrderDetails.Where(x => x.Id == tran.Value.id).FirstOrDefault();
                            if (det != null)
                            {
                                // ReConfirm the objects
                                if (det.IsFinished)
                                {
                                    det.IsFinished = false;
                                    det.Errors = new Dictionary<string, string>();
                                    //db.Database.ExecuteSqlCommand(string.Format("DBCC CHECKIDENT ({0}, RESEED, {1});", tbl, id - 1));
                                    _blanketOrderDetailService.FinishObject(det, det.FinishedDate.GetValueOrDefault(), _blanketOrderService, _stockMutationService, _blanketService, _itemService, _itemTypeService, _warehouseItemService, _accountService, _generalLedgerJournalService, _closingService);
                                    Log(det.Errors, det.GetType().Name.Split('_')[0], "--", det.Id, det.Id);
                                }
                                else if (det.IsRejected)
                                {
                                    det.IsRejected = false;
                                    det.Errors = new Dictionary<string, string>();
                                    //db.Database.ExecuteSqlCommand(string.Format("DBCC CHECKIDENT ({0}, RESEED, {1});", tbl, id - 1));
                                    _blanketOrderDetailService.RejectObject(det, det.FinishedDate.GetValueOrDefault(), _blanketOrderService, _stockMutationService, _blanketService, _itemService, _itemTypeService, _warehouseItemService, _accountService, _generalLedgerJournalService, _closingService);
                                    Log(det.Errors, det.GetType().Name.Split('_')[0], "--", det.Id, det.Id);
                                }
                            }
                            break;
                        };
                    case StockMutationTranSource.RollerWarehouseMutation:
                        {
                            var obj = db.RollerWarehouseMutations.Where(x => x.Id == tran.Value.id && !x.IsDeleted).OrderByDescending(x => x.Id).FirstOrDefault();
                            if (obj == null) obj = db.RollerWarehouseMutations.Where(x => x.Id == tran.Value.id).FirstOrDefault();
                            if (obj != null)
                            {
                                // ReConfirm the objects
                                if (reconfirm && obj.IsConfirmed)
                                {
                                    foreach (var det in db.RollerWarehouseMutationDetails.Where(x => !x.IsDeleted && x.RollerWarehouseMutationId == obj.Id).ToList())
                                    {
                                        det.IsConfirmed = false;
                                        db.Entry(det).State = EntityState.Modified;
                                        db.SaveChanges();
                                    }
                                    obj.IsConfirmed = false;
                                    obj.IsDeleted = false;
                                    obj.Errors = new Dictionary<string, string>();
                                    //db.Database.ExecuteSqlCommand(string.Format("DBCC CHECKIDENT ({0}, RESEED, {1});", obj.GetType().Name.Split('_')[0], rec.Id - 1));
                                    _rollerWarehouseMutationService.ConfirmObject(obj, obj.ConfirmationDate.GetValueOrDefault(), _rollerWarehouseMutationDetailService, _itemService, _blanketService, _warehouseItemService, _stockMutationService, _recoveryOrderDetailService, _recoveryOrderService, _coreIdentificationDetailService, _coreIdentificationService, _customerStockMutationService, _customerItemService);
                                    Log(obj.Errors, "RollerWarehouseMutation", obj.Code, obj.Id, obj.Id);
                                }
                            }
                            break;
                        };
                    case StockMutationTranSource.WarehouseMutation:
                        {
                            var obj = db.WarehouseMutations.Where(x => x.Id == tran.Value.id && !x.IsDeleted).OrderByDescending(x => x.Id).FirstOrDefault();
                            if (obj == null) obj = db.WarehouseMutations.Where(x => x.Id == tran.Value.id).FirstOrDefault();
                            if (obj != null)
                            {
                                // ReConfirm the objects
                                if (reconfirm && obj.IsConfirmed)
                                {
                                    foreach (var det in db.WarehouseMutationDetails.Where(x => !x.IsDeleted && x.WarehouseMutationId == obj.Id).ToList())
                                    {
                                        det.IsConfirmed = false;
                                        db.Entry(det).State = EntityState.Modified;
                                        db.SaveChanges();
                                    }
                                    obj.IsConfirmed = false;
                                    obj.IsDeleted = false;
                                    obj.Errors = new Dictionary<string, string>();
                                    //db.Database.ExecuteSqlCommand(string.Format("DBCC CHECKIDENT ({0}, RESEED, {1});", obj.GetType().Name.Split('_')[0], rec.Id - 1));
                                    _warehouseMutationService.ConfirmObject(obj, obj.ConfirmationDate.GetValueOrDefault(), _warehouseMutationDetailService, _itemService, _blanketService, _warehouseItemService, _stockMutationService);
                                    Log(obj.Errors, obj.GetType().Name.Split('_')[0], obj.Code, obj.Id, obj.Id);
                                }
                            }
                            break;
                        };
                    case StockMutationTranSource.VirtualOrder:
                        {
                            var obj = db.VirtualOrders.Where(x => x.Id == tran.Value.id && !x.IsDeleted).OrderByDescending(x => x.Id).FirstOrDefault();
                            if (obj == null) obj = db.VirtualOrders.Where(x => x.Id == tran.Value.id).FirstOrDefault();
                            if (obj != null)
                            {
                                // ReConfirm the objects
                                if (reconfirm && obj.IsConfirmed)
                                {
                                    foreach (var det in db.VirtualOrderDetails.Where(x => !x.IsDeleted && x.VirtualOrderId == obj.Id).ToList())
                                    {
                                        det.IsConfirmed = false;
                                        det.IsAllDelivered = false;
                                        det.PendingDeliveryQuantity = det.Quantity;
                                        db.Entry(det).State = EntityState.Modified;
                                        db.SaveChanges();
                                    }
                                    obj.IsConfirmed = false;
                                    obj.IsDeleted = false;
                                    obj.IsDeliveryCompleted = false;
                                    obj.Errors = new Dictionary<string, string>();
                                    //db.Database.ExecuteSqlCommand(string.Format("DBCC CHECKIDENT ({0}, RESEED, {1});", obj.GetType().Name.Split('_')[0], rec.Id - 1));
                                    _virtualOrderService.ConfirmObject(obj, obj.ConfirmationDate.GetValueOrDefault(), _virtualOrderDetailService, _stockMutationService, _itemService, _blanketService, _warehouseItemService);
                                    Log(obj.Errors, obj.GetType().Name.Split('_')[0], obj.Code, obj.Id, obj.Id);
                                }
                            }
                            break;
                        };
                    case StockMutationTranSource.TemporaryDeliveryOrder:
                        {
                            var obj = db.TemporaryDeliveryOrders.Where(x => x.Id == tran.Value.id && !x.IsDeleted).OrderByDescending(x => x.Id).FirstOrDefault();
                            if (obj == null) obj = db.TemporaryDeliveryOrders.Where(x => x.Id == tran.Value.id).FirstOrDefault();
                            if (obj != null)
                            {
                                // ReConfirm the objects
                                if (reconfirm && obj.IsConfirmed)
                                {
                                    foreach (var det in db.TemporaryDeliveryOrderDetails.Where(x => !x.IsDeleted && x.TemporaryDeliveryOrderId == obj.Id).ToList())
                                    {
                                        det.IsConfirmed = false;
                                        det.IsAllCompleted = false;
                                        det.RestockQuantity = 0;
                                        det.WasteQuantity = 0;
                                        db.Entry(det).State = EntityState.Modified;
                                        db.SaveChanges();
                                    }
                                    obj.IsConfirmed = false;
                                    obj.IsDeleted = false;
                                    obj.IsDeliveryCompleted = false;
                                    obj.Errors = new Dictionary<string, string>();
                                    //db.Database.ExecuteSqlCommand(string.Format("DBCC CHECKIDENT ({0}, RESEED, {1});", obj.GetType().Name.Split('_')[0], rec.Id - 1));
                                    _temporaryDeliveryOrderService.ConfirmObject(obj, obj.ConfirmationDate.GetValueOrDefault(), _temporaryDeliveryOrderDetailService, _virtualOrderService, _virtualOrderDetailService, _deliveryOrderService, _deliveryOrderDetailService, _salesOrderDetailService, _stockMutationService, _itemService, _blanketService, _warehouseItemService);
                                    Log(obj.Errors, "TemporaryDeliveryOrder", obj.Code, obj.Id, obj.Id);
                                }
                            }
                            break;
                        };
                    case StockMutationTranSource.TemporaryDeliveryOrderClearance:
                        {
                            var obj = db.TemporaryDeliveryOrderClearances.Where(x => x.Id == tran.Value.id && !x.IsDeleted).OrderByDescending(x => x.Id).FirstOrDefault();
                            if (obj == null) obj = db.TemporaryDeliveryOrderClearances.Where(x => x.Id == tran.Value.id).FirstOrDefault();
                            if (obj != null)
                            {
                                // ReConfirm the objects
                                if (reconfirm && obj.IsConfirmed)
                                {
                                    foreach (var det in db.TemporaryDeliveryOrderClearanceDetails.Where(x => !x.IsDeleted && x.TemporaryDeliveryOrderClearanceId == obj.Id).ToList())
                                    {
                                        det.IsConfirmed = false;
                                        db.Entry(det).State = EntityState.Modified;
                                        db.SaveChanges();
                                    }
                                    obj.IsConfirmed = false;
                                    obj.Errors = new Dictionary<string, string>();
                                    //db.Database.ExecuteSqlCommand(string.Format("DBCC CHECKIDENT ({0}, RESEED, {1});", obj.GetType().Name.Split('_')[0], rec.Id - 1));
                                    _temporaryDeliveryOrderClearanceService.ConfirmObject(obj, obj.ConfirmationDate.GetValueOrDefault(), _temporaryDeliveryOrderClearanceDetailService, _stockMutationService, _itemService, _itemTypeService, _blanketService, _warehouseItemService, _temporaryDeliveryOrderService, _temporaryDeliveryOrderDetailService, _generalLedgerJournalService, _accountService, _closingService);
                                    Log(obj.Errors, "TemporaryDeliveryOrderClearance", obj.Code, obj.Id, obj.Id);
                                }
                            }
                            break;
                        };
                    case StockMutationTranSource.SalesOrder:
                        {
                            var obj = db.SalesOrders.Where(x => x.Id == tran.Value.id && !x.IsDeleted).OrderByDescending(x => x.Id).FirstOrDefault();
                            if (obj == null) obj = db.SalesOrders.Where(x => x.Id == tran.Value.id).FirstOrDefault();
                            if (obj != null)
                            {
                                // ReConfirm the objects
                                if (reconfirm && obj.IsConfirmed)
                                {
                                    foreach (var det in db.SalesOrderDetails.Where(x => !x.IsDeleted && x.SalesOrderId == obj.Id).ToList())
                                    {
                                        det.IsConfirmed = false;
                                        det.IsAllDelivered = false;
                                        det.PendingDeliveryQuantity = det.Quantity;
                                        db.Entry(det).State = EntityState.Modified;
                                        db.SaveChanges();
                                    }
                                    obj.IsConfirmed = false;
                                    obj.IsDeliveryCompleted = false;
                                    obj.Errors = new Dictionary<string, string>();
                                    //db.Database.ExecuteSqlCommand(string.Format("DBCC CHECKIDENT ({0}, RESEED, {1});", obj.GetType().Name.Split('_')[0], rec.Id - 1));
                                    _salesOrderService.ConfirmObject(obj, obj.ConfirmationDate.GetValueOrDefault(), _salesOrderDetailService, _stockMutationService, _itemService, _blanketService, _warehouseItemService);
                                    Log(obj.Errors, obj.GetType().Name.Split('_')[0], obj.Code, obj.Id, obj.Id);
                                }
                            }
                            break;
                        };
                    case StockMutationTranSource.DeliveryOrder:
                        {
                            var obj = db.DeliveryOrders.Where(x => x.Id == tran.Value.id && !x.IsDeleted).OrderByDescending(x => x.Id).FirstOrDefault();
                            if (obj == null) obj = db.DeliveryOrders.Where(x => x.Id == tran.Value.id).FirstOrDefault();
                            if (obj != null)
                            {
                                // ReConfirm the objects
                                if (reconfirm && obj.IsConfirmed)
                                {
                                    foreach (var det in db.DeliveryOrderDetails.Where(x => !x.IsDeleted && x.DeliveryOrderId == obj.Id).ToList())
                                    {
                                        det.IsConfirmed = false;
                                        det.IsAllInvoiced = false;
                                        det.PendingInvoicedQuantity = det.Quantity;
                                        db.Entry(det).State = EntityState.Modified;
                                        db.SaveChanges();
                                    }
                                    obj.IsConfirmed = false;
                                    obj.IsInvoiceCompleted = false;
                                    obj.Errors = new Dictionary<string, string>();
                                    //db.Database.ExecuteSqlCommand(string.Format("DBCC CHECKIDENT ({0}, RESEED, {1});", obj.GetType().Name.Split('_')[0], rec.Id - 1));
                                    _deliveryOrderService.ConfirmObject(obj, obj.ConfirmationDate.GetValueOrDefault(), _deliveryOrderDetailService, _salesOrderService, _salesOrderDetailService, _stockMutationService, _itemService, _itemTypeService, _blanketService, _warehouseItemService, _accountService, _generalLedgerJournalService, _closingService, _serviceCostService, _temporaryDeliveryOrderDetailService, _temporaryDeliveryOrderService, _customerStockMutationService, _customerItemService, _currencyService, _exchangeRateService);
                                    Log(obj.Errors, obj.GetType().Name.Split('_')[0], obj.Code, obj.Id, obj.Id);
                                }
                            }
                            break;
                        };
                    case StockMutationTranSource.SalesInvoice:
                        {
                            var obj = db.SalesInvoices.Where(x => x.Id == tran.Value.id && !x.IsDeleted).OrderByDescending(x => x.Id).FirstOrDefault();
                            if (obj == null) obj = db.SalesInvoices.Where(x => x.Id == tran.Value.id).FirstOrDefault();
                            if (obj != null)
                            {
                                // ReConfirm the objects
                                if (reconfirm && obj.IsConfirmed)
                                {
                                    foreach (var det in db.SalesInvoiceDetails.Where(x => !x.IsDeleted && x.SalesInvoiceId == obj.Id).ToList())
                                    {
                                        det.IsConfirmed = false;
                                        db.Entry(det).State = EntityState.Modified;
                                        db.SaveChanges();
                                    }

                                    obj.IsConfirmed = false;
                                    obj.IsDeleted = false;
                                    obj.Errors = new Dictionary<string, string>();
                                    var receivable = db.Receivables.Where(x => !x.IsDeleted && x.ReceivableSource == Constant.ReceivableSource.SalesInvoice && x.ReceivableSourceId == obj.Id).OrderByDescending(x => x.Id).FirstOrDefault();
                                    var id = receivable.Id;
                                    var tbl = receivable.GetType().Name.Split('_')[0];
                                    db.Receivables.Remove(receivable);
                                    db.Database.ExecuteSqlCommand(string.Format("DBCC CHECKIDENT ({0}, RESEED, {1});", tbl, id - 1));
                                    _salesInvoiceService.ConfirmObject(obj, obj.ConfirmationDate.GetValueOrDefault(), _salesInvoiceDetailService, _salesOrderService, _salesOrderDetailService, _deliveryOrderService, _deliveryOrderDetailService, _receivableService, _accountService, _generalLedgerJournalService, _closingService, _serviceCostService, _rollerBuilderService, _itemService, _itemTypeService, _contactService, _exchangeRateService, _currencyService, _gLNonBaseCurrencyService);
                                    Log(obj.Errors, obj.GetType().Name.Split('_')[0], obj.Code, obj.Id, obj.Id);
                                }
                            }
                            break;
                        };
                    default : {
                        Console.WriteLine("Unknown Table : {0}", tran.Value.source);
                        break;
                    }
                }
            }
            writer.Close();
            fstrm.Close();
            return count;
        }

        public int ReConfirm(Nullable<DateTime> startDate, Nullable<DateTime> endDate)
        {
            IDictionary<DateTime, sourceid> trans = new Dictionary<DateTime, sourceid>(); // Document DateTime & ID
            //IList<KeyValuePair<DateTime, sourceid>> trans = new List<KeyValuePair<DateTime, sourceid>>(); // Document DateTime & ID

            var db = new OffsetPrintingSuppliesEntities();
            using (db)
            {
                //foreach (var tableName in Enumerable.Reverse(masterDatas))
                //{
                //    if (tableName == "UserAccess" || tableName == "UserMenu") continue;
                //    var a = db.Database.SqlQuery<string>(string.Format("select '[' + name + '], ' as [text()] from sys.columns where object_id = object_id('Temp{0}') for xml path('')", tableName)).ToList(); // where object_id = object_id('{0}')
                //    string b = "";
                //    foreach (var c in a) b += c;
                //    b = b.Substring(0, b.Length - 2);
                //    // Create new table using source table structure, and then copy the data from source structure to new table
                //    //db.Database.ExecuteSqlCommand(string.Format("CREATE TABLE Temp{0} LIKE {0}; INSERT Temp{0} SELECT * FROM {0};", tableName));
                //    //db.Database.ExecuteSqlCommand(string.Format("CREATE TABLE {0} AS SELECT * FROM Temp{0};", tableName));
                //    db.Database.ExecuteSqlCommand(string.Format("SET IDENTITY_INSERT {0} ON; INSERT INTO {0}({1}) SELECT {1} FROM Temp{0} WHERE 1=1; SET IDENTITY_INSERT {0} OFF;", tableName, b));
                //}

                // Allow defining the ID
                foreach (var tableName in byproductDatas)
                {
                    db.Database.ExecuteSqlCommand(string.Format("SET IDENTITY_INSERT [dbo].[{0}] ON;", tableName));
                }

                // Restore non-deleted records one-by-one
                //var tmps = db.Database.SqlQuery(typeof(TempCashSalesInvoice), "SELECT * FROM TempCashSalesInvoice");

                // Strange Behaviour http://stackoverflow.com/questions/472578/dbcc-checkident-sets-identity-to-0
                //int? mxid = db.Database.SqlQuery<int?>(string.Format("SELECT MAX(Id) FROM {0}", "Account")).FirstOrDefault();
                //if (mxid == null || mxid <= 0) mxid = 1; // if RESEED=0, ID will start from 0 if it's newly created table (don't have record yet), if it already have record RESEED=0 will start from ID=1
                //db.Database.ExecuteSqlCommand(string.Format("DBCC CHECKIDENT ('{0}', RESEED, {1});", "Account", mxid.GetValueOrDefault()));
                //db.Database.ExecuteSqlCommand(string.Format("SET IDENTITY_INSERT [dbo].[{0}] OFF;", "Account"));

                // StockAdjustment
                foreach (var rec in db.StockAdjustments.Where(x => !x.IsDeleted).ToList())
                {
                    if (rec.IsConfirmed)
                    {
                        DateTime confirmdate = rec.ConfirmationDate ?? rec.AdjustmentDate;
                        trans.Add(confirmdate.AddMilliseconds(rec.Id + 10000), new sourceid() { source = rec.GetType().Name.Split('_')[0], id = rec.Id });
                    }
                }

                // CustomerStockAdjustment
                foreach (var rec in db.CustomerStockAdjustments.Where(x => !x.IsDeleted).ToList())
                {
                    if (rec.IsConfirmed)
                    {
                        DateTime confirmdate = rec.ConfirmationDate ?? rec.AdjustmentDate;
                        string name = "CustomerStockAdjustment"; // rec.GetType().Name.Split('_')[0];
                        trans.Add(confirmdate.AddMilliseconds(rec.Id + 20000), new sourceid() { source = name, id = rec.Id });
                    }
                }

                // PurchaseOrder
                foreach (var rec in db.PurchaseOrders.Where(x => !x.IsDeleted).ToList())
                {
                    if (rec.IsConfirmed)
                    {
                        DateTime confirmdate = rec.ConfirmationDate ?? rec.PurchaseDate;
                        trans.Add(confirmdate.AddMilliseconds(rec.Id + 30000), new sourceid() { source = rec.GetType().Name.Split('_')[0], id = rec.Id });
                    }
                }

                // PurchaseReceival
                foreach (var rec in db.PurchaseReceivals.Where(x => !x.IsDeleted).ToList())
                {
                    if (rec.IsConfirmed)
                    {
                        DateTime confirmdate = rec.ConfirmationDate ?? rec.ReceivalDate;
                        trans.Add(confirmdate.AddMilliseconds(rec.Id + 40000), new sourceid() { source = rec.GetType().Name.Split('_')[0], id = rec.Id });
                    }
                }

                // PurchaseInvoice
                foreach (var rec in db.PurchaseInvoices.Where(x => !x.IsDeleted).ToList())
                {
                    if (rec.IsConfirmed)
                    {
                        DateTime confirmdate = rec.ConfirmationDate ?? rec.InvoiceDate;
                        trans.Add(confirmdate.AddMilliseconds(rec.Id + 50000), new sourceid() { source = rec.GetType().Name.Split('_')[0], id = rec.Id });
                    }
                }

                // Repacking
                foreach (var rec in db.Repackings.Where(x => !x.IsDeleted).ToList())
                {
                    if (rec.IsConfirmed)
                    {
                        DateTime confirmdate = rec.ConfirmationDate ?? rec.RepackingDate;
                        trans.Add(confirmdate.AddMilliseconds(rec.Id + 60000), new sourceid() { source = rec.GetType().Name.Split('_')[0], id = rec.Id });
                    }
                }

                // BlendingWorkOrder
                foreach (var rec in db.BlendingWorkOrders.Where(x => !x.IsDeleted).ToList())
                {
                    if (rec.IsConfirmed)
                    {
                        DateTime confirmdate = rec.ConfirmationDate ?? rec.BlendingDate;
                        trans.Add(confirmdate.AddMilliseconds(rec.Id + 70000), new sourceid() { source = rec.GetType().Name.Split('_')[0], id = rec.Id });
                    }
                }

                // CoreIdentification
                foreach (var rec in db.CoreIdentifications.Where(x => !x.IsDeleted).ToList())
                {
                    if (rec.IsConfirmed)
                    {
                        DateTime confirmdate = rec.ConfirmationDate ?? rec.IdentifiedDate;
                        trans.Add(confirmdate.AddMilliseconds(rec.Id + 80000), new sourceid() { source = rec.GetType().Name.Split('_')[0], id = rec.Id });
                    }
                }

                // RecoveryOrder
                foreach (var rec in db.RecoveryOrders.Where(x => !x.IsDeleted).ToList())
                {
                    if (rec.IsConfirmed)
                    {
                        DateTime confirmdate = rec.ConfirmationDate ?? rec.DueDate.GetValueOrDefault();
                        trans.Add(confirmdate.AddMilliseconds(rec.Id + 90000), new sourceid() { source = rec.GetType().Name.Split('_')[0], id = rec.Id });

                        foreach (var det in rec.RecoveryOrderDetails.Where(x => !x.IsDeleted).ToList())
                        {
                            if (det.IsFinished)
                            {
                                DateTime date = det.FinishedDate ?? confirmdate;
                                trans.Add(date.AddMilliseconds(det.Id + 95000), new sourceid() { source = det.GetType().Name.Split('_')[0], id = det.Id });
                            }
                            else if (det.IsRejected)
                            {
                                DateTime date = det.RejectedDate ?? confirmdate;
                                trans.Add(date.AddMilliseconds(det.Id + 95000), new sourceid() { source = det.GetType().Name.Split('_')[0], id = det.Id });
                            }
                        }
                    }
                }

                // BlanketOrder
                foreach (var rec in db.BlanketOrders.Where(x => !x.IsDeleted).ToList())
                {
                    if (rec.IsConfirmed)
                    {
                        DateTime confirmdate = rec.ConfirmationDate ?? rec.DueDate.GetValueOrDefault();
                        trans.Add(confirmdate.AddMilliseconds(rec.Id + 100000), new sourceid() { source = rec.GetType().Name.Split('_')[0], id = rec.Id });

                        foreach (var det in rec.BlanketOrderDetails.Where(x => !x.IsDeleted).ToList())
                        {
                            if (det.IsFinished)
                            {
                                DateTime date = det.FinishedDate ?? confirmdate;
                                trans.Add(date.AddMilliseconds(det.Id + 105000), new sourceid() { source = det.GetType().Name.Split('_')[0], id = det.Id });
                            }
                            else if (det.IsRejected)
                            {
                                DateTime date = det.RejectedDate ?? confirmdate;
                                trans.Add(date.AddMilliseconds(det.Id + 105000), new sourceid() { source = det.GetType().Name.Split('_')[0], id = det.Id });
                            }
                        }
                    }
                }

                // RollerWarehouseMutation
                foreach (var rec in db.RollerWarehouseMutations.Where(x => !x.IsDeleted).ToList())
                {
                    if (rec.IsConfirmed)
                    {
                        DateTime confirmdate = rec.ConfirmationDate ?? rec.MutationDate;
                        string name = "RollerWarehouseMutation"; // rec.GetType().Name.Split('_')[0];
                        trans.Add(confirmdate.AddMilliseconds(rec.Id + 110000), new sourceid() { source = name, id = rec.Id });
                    }
                }

                // WarehouseMutation
                foreach (var rec in db.WarehouseMutations.Where(x => !x.IsDeleted).ToList())
                {
                    if (rec.IsConfirmed)
                    {
                        DateTime confirmdate = rec.ConfirmationDate ?? rec.MutationDate;
                        trans.Add(confirmdate.AddMilliseconds(rec.Id + 120000), new sourceid() { source = rec.GetType().Name.Split('_')[0], id = rec.Id });
                    }
                }

                // VirtualOrder
                foreach (var rec in db.VirtualOrders.Where(x => !x.IsDeleted).ToList())
                {
                    if (rec.IsConfirmed)
                    {
                        DateTime confirmdate = rec.ConfirmationDate ?? rec.OrderDate;
                        trans.Add(confirmdate.AddMilliseconds(rec.Id + 130000), new sourceid() { source = rec.GetType().Name.Split('_')[0], id = rec.Id });
                    }
                }

                // TemporaryDeliveryOrder
                foreach (var rec in db.TemporaryDeliveryOrders.Where(x => !x.IsDeleted).ToList())
                {
                    DateTime confirmdate = rec.ConfirmationDate ?? rec.DeliveryDate;
                    if (rec.IsConfirmed)
                    {
                        string name = "TemporaryDeliveryOrder"; // rec.GetType().Name.Split('_')[0];
                        trans.Add(confirmdate.AddMilliseconds(rec.Id + 140000), new sourceid() { source = name, id = rec.Id });
                    }

                    //if (rec.IsPushed)
                    //{
                    //    DateTime date = rec.PushDate ?? confirmdate;
                    //    trans.Add(date.GetValueOrDefault().AddMilliseconds(rec.Id + 115000), new sourceid() { source = obj.GetType().Name.Split('_')[0], id = rec.Id });
                    //}
                }

                // TemporaryDeliveryOrderClearance
                foreach (var rec in db.TemporaryDeliveryOrderClearances.Where(x => !x.IsDeleted).ToList())
                {
                    if (rec.IsConfirmed)
                    {
                        DateTime confirmdate = rec.ConfirmationDate ?? rec.ClearanceDate;
                        string name = "TemporaryDeliveryOrderClearance"; // rec.GetType().Name.Split('_')[0];
                        trans.Add(confirmdate.AddMilliseconds(rec.Id + 150000), new sourceid() { source = name, id = rec.Id });
                    }
                }

                // SalesOrder
                foreach (var rec in db.SalesOrders.Where(x => !x.IsDeleted).ToList())
                {
                    if (rec.IsConfirmed)
                    {
                        DateTime confirmdate = rec.ConfirmationDate ?? rec.SalesDate;
                        trans.Add(confirmdate.AddMilliseconds(rec.Id + 160000), new sourceid() { source = rec.GetType().Name.Split('_')[0], id = rec.Id });
                    }
                }

                // DeliveryOrder
                foreach (var rec in db.DeliveryOrders.Where(x => !x.IsDeleted).ToList())
                {
                    if (rec.IsConfirmed)
                    {
                        DateTime confirmdate = rec.ConfirmationDate ?? rec.DeliveryDate;
                        trans.Add(confirmdate.AddMilliseconds(rec.Id + 170000), new sourceid() { source = rec.GetType().Name.Split('_')[0], id = rec.Id });
                    }
                }

                // SalesInvoice
                foreach (var rec in db.SalesInvoices.Where(x => !x.IsDeleted).ToList())
                {
                    if (rec.IsConfirmed)
                    {
                        DateTime confirmdate = rec.ConfirmationDate ?? rec.InvoiceDate;
                        trans.Add(confirmdate.AddMilliseconds(rec.Id + 180000), new sourceid() { source = rec.GetType().Name.Split('_')[0], id = rec.Id });
                    }
                }

                // Sort and Process Purchases/Sales
                OrderedConfirmation(db, trans, true, true);

                // DisAllow defining the ID (also Set SEED to the highest Id)
                foreach (var tableName in byproductDatas)
                {
                    // Strange Behaviour http://stackoverflow.com/questions/472578/dbcc-checkident-sets-identity-to-0
                    int? maxid = db.Database.SqlQuery<int?>(string.Format("SELECT MAX(Id) FROM {0}", tableName)).FirstOrDefault();
                    if (maxid == null || maxid <= 0) maxid = 1; // if RESEED=0, ID will start from 0 if it's newly created table (don't have record yet), if it already have record RESEED=0 will start from ID=1
                    db.Database.ExecuteSqlCommand(string.Format("DBCC CHECKIDENT ('{0}', RESEED, {1});", tableName, maxid.GetValueOrDefault()));
                    db.Database.ExecuteSqlCommand(string.Format("SET IDENTITY_INSERT [dbo].[{0}] OFF;", tableName));
                }

            }
            return 0;
        }

        public int DeleteByProducts(Nullable<DateTime> startDate, Nullable<DateTime> endDate)
        {
            var hash = new HashSet<string>(stockMutationSources);
            var db = new OffsetPrintingSuppliesEntities();
            using (db)
            {
                foreach (var obj in db.WarehouseItems.ToList())
                {
                    var item = db.Items.Where(x => x.Id == obj.ItemId).FirstOrDefault();
                    obj.Quantity = 0;
                    obj.PendingDelivery = 0;
                    obj.PendingReceival = 0;
                    item.Quantity = 0;
                    item.PendingDelivery = 0;
                    item.PendingReceival = 0;
                    item.AvgPrice = 0;
                    item.Virtual = 0;
                    item.CustomerQuantity = 0;
                    item.CustomerVirtual = 0;
                    item.CustomerAvgPrice = 0;
                    db.Entry(obj).State = EntityState.Modified;
                    db.Entry(item).State = EntityState.Modified;
                    
                }
                foreach (var obj in db.CustomerItems.ToList())
                {
                    obj.Quantity = 0;
                    obj.Virtual = 0;
                    db.Entry(obj).State = EntityState.Modified;
                }
                db.SaveChanges();

                //foreach (var obj in db.Payables.Where(x => stockMutationSources.Contains(x.PayableSource)).ToList())
                //{
                //    db.Payables.Remove(obj);
                //}

                //foreach (var obj in db.Receivables.Where(x => stockMutationSources.Contains(x.ReceivableSource)).ToList())
                //{
                //    db.Receivables.Remove(obj);
                //}

                //foreach (var obj in db.GeneralLedgerJournals.Where(x => stockMutationSources.Contains(x.SourceDocument)).AsEnumerable())
                //{
                //    db.GeneralLedgerJournals.Remove(obj);
                //}
                //db.GeneralLedgerJournals.Where(x => hash.Contains(x.SourceDocument));
                string st = "";
                foreach (var s in stockMutationSources) st += "'" + s + "', ";
                st = st.Substring(0, st.Length - 2);
                db.Database.ExecuteSqlCommand(string.Format("DELETE FROM [{0}] WHERE [{1}] IN ({2});", "GeneralLedgerJournal", "SourceDocument", st));

                db.Database.ExecuteSqlCommand(string.Format("DELETE FROM [{0}];", "CustomerStockMutation"));

                db.Database.ExecuteSqlCommand(string.Format("DELETE FROM [{0}];", "StockMutation"));
            }
            return 0;
        }

        public int DeleteTemp(Nullable<DateTime> startDate, Nullable<DateTime> endDate)
        {
            var db = new OffsetPrintingSuppliesEntities();
            using (db)
            {
                foreach (var tableName in userDatas)
                {
                    if (tableName != "Account") // exclude manually created TempAccount table to fix/rearrange all accounts
                    {
                        //try
                        {
                            db.Database.ExecuteSqlCommand(string.Format("DBCC CHECKIDENT (Temp{0}, RESEED, 1); ALTER TABLE Temp{0} NOCHECK CONSTRAINT ALL; DELETE FROM Temp{0};", tableName)); // ALTER TABLE Temp{0} DROP CONSTRAINT ALL; // ALTER TABLE Temp{0} CHECK CONSTRAINT ALL;
                        }
                        //catch (Exception ex)
                        {

                        }
                    }
                }

                foreach (var tableName in masterDatas)
                {
                    db.Database.ExecuteSqlCommand(string.Format("DBCC CHECKIDENT (Temp{0}, RESEED, 1); ALTER TABLE Temp{0} NOCHECK CONSTRAINT ALL; DELETE FROM Temp{0};", tableName));
                }
            }
            return 0;
        }

        public int DeleteOriginal(Nullable<DateTime> startDate, Nullable<DateTime> endDate)
        {
            var db = new OffsetPrintingSuppliesEntities();
            using (db)
            {

                IList<String> tableNames = new List<String>();

                IList<String> userroleNames = new List<String>() { "UserMenu", "UserAccount", "UserAccess" };

                IList<String> financeNames = new List<String>()
                                        {   "PaymentVoucherDetail", "PaymentVoucher", "Payable",
                                            "ReceiptVoucherDetail", "ReceiptVoucher", "Receivable",
                                            "PaymentRequestDetail", "PaymentRequest",
                                            "CashMutation", "CashBankAdjustment", "CashBankMutation", "CashBank"};
                IList<String> manufacturingNames = new List<String>()
                                        { "RollerWarehouseMutationDetail", "RollerWarehouseMutation",
                                          "RecoveryAccessoryDetail", "RecoveryOrderDetail", "RecoveryOrder",
                                          "CoreIdentificationDetail", "CoreIdentification",
                                          "BarringOrderDetail", "BarringOrder" };
                IList<String> purchaseOperationNames = new List<String>()
                                        { "RetailPurchaseInvoice", "RetailPurchaseInvoiceDetail", "PurchaseInvoiceDetail", "PurchaseInvoice",
                                          "PurchaseReceivalDetail", "PurchaseReceival", "PurchaseOrderDetail", "PurchaseOrder",
                                          "CustomPurchaseInvoiceDetail", "CustomPurchaseInvoice" };
                IList<String> salesOperationNames = new List<String>()
                                        { "RetailSalesInvoiceDetail", "RetailSalesInvoice", "SalesInvoiceDetail", "SalesInvoice",
                                          "DeliveryOrderDetail", "DeliveryOrder", "SalesOrderDetail", "SalesOrder", "CashSalesReturnDetail", "CashSalesReturn", 
                                          "CashSalesInvoiceDetail", "CashSalesInvoice"};
                IList<String> stockAndMasterNames = new List<String>()
                                        { "GroupItemPrice", "QuantityPricing", "PriceMutation", "StockMutation", "WarehouseMutationOrderDetail", "WarehouseMutationOrder",
                                          "RollerBuilder", "StockAdjustmentDetail", "StockAdjustment", "WarehouseItem",
                                          "Warehouse", "CoreBuilder", "Item", "ItemType", "UoM", "Contact",
                                          "RollerType", "Machine", "ContactGroup", "Company"};

                IList<String> accountingNames = new List<String>() { "MemorialDetail", "Memorial", "GeneralLedgerJournal", "ValidComb", "Closing", "Account" };

                userroleNames.ToList().ForEach(x => tableNames.Add(x));
                financeNames.ToList().ForEach(x => tableNames.Add(x));
                manufacturingNames.ToList().ForEach(x => tableNames.Add(x));
                purchaseOperationNames.ToList().ForEach(x => tableNames.Add(x));
                salesOperationNames.ToList().ForEach(x => tableNames.Add(x));
                stockAndMasterNames.ToList().ForEach(x => tableNames.Add(x));
                accountingNames.ToList().ForEach(x => tableNames.Add(x));

                foreach (var tableName in tableNames)
                {
                    db.Database.ExecuteSqlCommand(string.Format("DBCC CHECKIDENT ({0}, RESEED, 1); ALTER TABLE {0} NOCHECK CONSTRAINT ALL; DELETE FROM {0}; ALTER TABLE {0} CHECK CONSTRAINT ALL;", tableName));
                }

                //foreach (var tableName in tableNames)
                //{
                //    db.Database.ExecuteSqlCommand(string.Format("DROP TABLE {0}", tableName));
                //}

            }
            return 0;
        }
    }
}