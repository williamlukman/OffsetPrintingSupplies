using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.DomainModel;
using NSpec;
using Service.Service;
using Core.Interface.Service;
using Data.Context;
using System.Data.Entity;
using Data.Repository;
using Validation.Validation;
using Core.Constants;

namespace TestValidation
{

    public class SpecDeliveryOrder : nspec
    {
        ContactGroup baseGroup;
        Contact contact;
        Item item_batiktulis;
        Item item_busway;
        Item item_botolaqua;
        UoM Pcs;
        ItemType type;
        Warehouse warehouse;
        SalesOrder salesOrder1;
        SalesOrder salesOrder2;
        SalesOrderDetail salesOrderDetail_batiktulis_so1;
        SalesOrderDetail salesOrderDetail_busway_so1;
        SalesOrderDetail salesOrderDetail_botolaqua_so1;
        SalesOrderDetail salesOrderDetail_batiktulis_so2;
        SalesOrderDetail salesOrderDetail_busway_so2;
        SalesOrderDetail salesOrderDetail_botolaqua_so2;
        DeliveryOrder deliveryOrder1;
        DeliveryOrder deliveryOrder2;
        DeliveryOrder deliveryOrder3;
        DeliveryOrderDetail deliveryOrderDetail_batiktulis_do1;
        DeliveryOrderDetail deliveryOrderDetail_busway_do1;
        DeliveryOrderDetail deliveryOrderDetail_botolaqua_do1;
        DeliveryOrderDetail deliveryOrderDetail_batiktulis_do2a;
        DeliveryOrderDetail deliveryOrderDetail_batiktulis_do2b;
        DeliveryOrderDetail deliveryOrderDetail_busway_do2;
        DeliveryOrderDetail deliveryOrderDetail_botolaqua_do2;
        IContactService _contactService;
        IItemService _itemService;
        IStockMutationService _stockMutationService;
        IStockAdjustmentService _stockAdjustmentService;
        IStockAdjustmentDetailService _stockAdjustmentDetailService;
        ISalesOrderService _salesOrderService;
        ISalesOrderDetailService _salesOrderDetailService;
        ISalesInvoiceDetailService _salesInvoiceDetailService;
        ISalesInvoiceService _salesInvoiceService;
        IDeliveryOrderService _deliveryOrderService;
        IDeliveryOrderDetailService _deliveryOrderDetailService;
        IUoMService _uomService;
        IBlanketService _blanketService;
        IItemTypeService _itemTypeService;
        IWarehouseItemService _warehouseItemService;
        IWarehouseService _warehouseService;

        IPriceMutationService _priceMutationService;
        IContactGroupService _contactGroupService;
        IAccountService _accountService;
        IGeneralLedgerJournalService _generalLedgerJournalService;
        IClosingService _closingService;
        IServiceCostService _serviceCostService;

        private Account Asset, CurrentAsset, CashBank, AccountReceivable, GBCHReceivable, Inventory, Raw, FinishedGoods, PrepaidExpense, NonCurrentAsset;
        private Account Expense, COGS, COS, OperationalExpense, ManufacturingExpense, RecoveryExpense, ConversionExpense;
        private Account SellingGeneralAndAdministrationExpense, CashBankAdjustmentExpense, Discount, SalesAllowance, StockAdjustmentExpense;
        private Account NonOperationalExpense, DepreciationExpense, Amortization, InterestExpense, TaxExpense, DividentExpense;
        private Account Liability, CurrentLiability, AccountPayable, GBCHPayable, GoodsPendingClearance, PurchaseAllowance, UnearnedRevenue, NonCurrentLiability;
        private Account Equity, OwnersEquity, EquityAdjustment;
        private Account Revenue;

        void before_each()
        {
            var db = new OffsetPrintingSuppliesEntities();
            using (db)
            {
                db.DeleteAllTables();
                _contactService = new ContactService(new ContactRepository(), new ContactValidator());
                _itemService = new ItemService(new ItemRepository(), new ItemValidator());
                _stockMutationService = new StockMutationService(new StockMutationRepository(), new StockMutationValidator());
                _salesOrderService = new SalesOrderService(new SalesOrderRepository(), new SalesOrderValidator());
                _salesOrderDetailService = new SalesOrderDetailService(new SalesOrderDetailRepository(), new SalesOrderDetailValidator());
                _salesInvoiceService = new SalesInvoiceService(new SalesInvoiceRepository(), new SalesInvoiceValidator());
                _salesInvoiceDetailService = new SalesInvoiceDetailService(new SalesInvoiceDetailRepository(), new SalesInvoiceDetailValidator());
                _deliveryOrderService = new DeliveryOrderService(new DeliveryOrderRepository(), new DeliveryOrderValidator());
                _deliveryOrderDetailService = new DeliveryOrderDetailService(new DeliveryOrderDetailRepository(), new DeliveryOrderDetailValidator());
                _stockAdjustmentService = new StockAdjustmentService(new StockAdjustmentRepository(), new StockAdjustmentValidator());
                _stockAdjustmentDetailService = new StockAdjustmentDetailService(new StockAdjustmentDetailRepository(), new StockAdjustmentDetailValidator());
                _itemTypeService = new ItemTypeService(new ItemTypeRepository(), new ItemTypeValidator());
                _uomService = new UoMService(new UoMRepository(), new UoMValidator());
                _warehouseItemService = new WarehouseItemService(new WarehouseItemRepository(), new WarehouseItemValidator());
                _warehouseService = new WarehouseService(new WarehouseRepository(), new WarehouseValidator());
                _blanketService = new BlanketService(new BlanketRepository(), new BlanketValidator());

                _priceMutationService = new PriceMutationService(new PriceMutationRepository(), new PriceMutationValidator());
                _contactGroupService = new ContactGroupService(new ContactGroupRepository(), new ContactGroupValidator());
                _accountService = new AccountService(new AccountRepository(), new AccountValidator());
                _generalLedgerJournalService = new GeneralLedgerJournalService(new GeneralLedgerJournalRepository(), new GeneralLedgerJournalValidator());
                _closingService = new ClosingService(new ClosingRepository(), new ClosingValidator());
                _serviceCostService = new ServiceCostService(new ServiceCostRepository(), new ServiceCostValidator());

                baseGroup = _contactGroupService.CreateObject(Core.Constants.Constant.GroupType.Base, "Base Group", true);

                if (!_accountService.GetLegacyObjects().Any())
                {
                    Asset = _accountService.CreateLegacyObject(new Account() { Level = 1, Name = "Asset", Code = Constant.AccountCode.Asset, LegacyCode = Constant.AccountLegacyCode.Asset, Group = Constant.AccountGroup.Asset, IsLegacy = true }, _accountService);
                    CurrentAsset = _accountService.CreateLegacyObject(new Account() { Level = 2, Name = "Current Asset", Code = Constant.AccountCode.CurrentAsset, LegacyCode = Constant.AccountLegacyCode.CurrentAsset, Group = Constant.AccountGroup.Asset, ParentId = Asset.Id, IsLegacy = true }, _accountService);
                    CashBank = _accountService.CreateLegacyObject(new Account() { Level = 3, Name = "Cash & Bank", Code = Constant.AccountCode.CashBank, LegacyCode = Constant.AccountLegacyCode.CashBank, Group = Constant.AccountGroup.Asset, ParentId = CurrentAsset.Id, IsLegacy = true }, _accountService);
                    AccountReceivable = _accountService.CreateLegacyObject(new Account() { Level = 3, IsLeaf = true, Name = "Account Receivable", Code = Constant.AccountCode.AccountReceivable, LegacyCode = Constant.AccountLegacyCode.AccountReceivable, Group = Constant.AccountGroup.Asset, ParentId = CurrentAsset.Id, IsLegacy = true }, _accountService);
                    GBCHReceivable = _accountService.CreateLegacyObject(new Account() { Level = 3, IsLeaf = true, Name = "GBCH Receivable", Code = Constant.AccountCode.GBCHReceivable, LegacyCode = Constant.AccountLegacyCode.GBCHReceivable, Group = Constant.AccountGroup.Asset, ParentId = CurrentAsset.Id, IsLegacy = true }, _accountService);
                    Inventory = _accountService.CreateLegacyObject(new Account() { Level = 3, Name = "Inventory", Code = Constant.AccountCode.Inventory, LegacyCode = Constant.AccountLegacyCode.Inventory, Group = Constant.AccountGroup.Asset, ParentId = CurrentAsset.Id, IsLegacy = true }, _accountService);
                    Raw = _accountService.CreateLegacyObject(new Account() { Level = 4, IsLeaf = true, Name = "Raw Material", Code = Constant.AccountCode.Raw, LegacyCode = Constant.AccountLegacyCode.Raw, Group = Constant.AccountGroup.Asset, ParentId = Inventory.Id, IsLegacy = true }, _accountService);
                    FinishedGoods = _accountService.CreateLegacyObject(new Account() { Level = 4, IsLeaf = true, Name = "Finished Goods", Code = Constant.AccountCode.FinishedGoods, LegacyCode = Constant.AccountLegacyCode.FinishedGoods, Group = Constant.AccountGroup.Asset, ParentId = Inventory.Id, IsLegacy = true }, _accountService);
                    PrepaidExpense = _accountService.CreateLegacyObject(new Account() { Level = 3, IsLeaf = true, Name = "Prepaid Expense (Asset)", Code = Constant.AccountCode.PrepaidExpense, LegacyCode = Constant.AccountLegacyCode.PrepaidExpense, Group = Constant.AccountGroup.Asset, ParentId = CurrentAsset.Id, IsLegacy = true }, _accountService);
                    NonCurrentAsset = _accountService.CreateLegacyObject(new Account() { Level = 2, Name = "Noncurrent Asset", Code = Constant.AccountCode.NonCurrentAsset, LegacyCode = Constant.AccountLegacyCode.NonCurrentAsset, Group = Constant.AccountGroup.Asset, ParentId = Asset.Id, IsLegacy = true }, _accountService);

                    Expense = _accountService.CreateLegacyObject(new Account() { Level = 1, Name = "Expense", Code = Constant.AccountCode.Expense, LegacyCode = Constant.AccountLegacyCode.Expense, Group = Constant.AccountGroup.Expense, IsLegacy = true }, _accountService);
                    COGS = _accountService.CreateLegacyObject(new Account() { Level = 2, IsLeaf = true, Name = "Cost Of Goods Sold", Code = Constant.AccountCode.COGS, LegacyCode = Constant.AccountLegacyCode.COGS, Group = Constant.AccountGroup.Expense, ParentId = Expense.Id, IsLegacy = true }, _accountService);
                    COS = _accountService.CreateLegacyObject(new Account() { Level = 2, IsLeaf = true, Name = "Cost of Services", Code = Constant.AccountCode.COS, LegacyCode = Constant.AccountLegacyCode.COS, Group = Constant.AccountGroup.Expense, ParentId = Expense.Id, IsLegacy = true }, _accountService);
                    OperationalExpense = _accountService.CreateLegacyObject(new Account() { Level = 2, Name = "Operational Expense", Code = Constant.AccountCode.OperationalExpense, LegacyCode = Constant.AccountLegacyCode.OperationalExpense, Group = Constant.AccountGroup.Expense, ParentId = Expense.Id, IsLegacy = true }, _accountService);
                    ManufacturingExpense = _accountService.CreateLegacyObject(new Account() { Level = 3, Name = "Manufacturing Expense", Code = Constant.AccountCode.ManufacturingExpense, LegacyCode = Constant.AccountLegacyCode.ManufacturingExpense, Group = Constant.AccountGroup.Expense, ParentId = OperationalExpense.Id, IsLegacy = true }, _accountService);
                    RecoveryExpense = _accountService.CreateLegacyObject(new Account() { Level = 4, IsLeaf = true, Name = "Roller Recovery Expense", Code = Constant.AccountCode.RecoveryExpense, LegacyCode = Constant.AccountLegacyCode.RecoveryExpense, Group = Constant.AccountGroup.Expense, ParentId = ManufacturingExpense.Id, IsLegacy = true }, _accountService);
                    ConversionExpense = _accountService.CreateLegacyObject(new Account() { Level = 4, IsLeaf = true, Name = "Blanket Conversion Expense", Code = Constant.AccountCode.ConversionExpense, LegacyCode = Constant.AccountLegacyCode.ConversionExpense, Group = Constant.AccountGroup.Expense, ParentId = ManufacturingExpense.Id, IsLegacy = true }, _accountService);
                    SellingGeneralAndAdministrationExpense = _accountService.CreateLegacyObject(new Account() { Level = 3, Name = "Selling, General, and Administration Expense", Code = Constant.AccountCode.SellingGeneralAndAdministrationExpense, LegacyCode = Constant.AccountLegacyCode.SellingGeneralAndAdministrationExpense, Group = Constant.AccountGroup.Expense, ParentId = OperationalExpense.Id, IsLegacy = true }, _accountService);
                    CashBankAdjustmentExpense = _accountService.CreateLegacyObject(new Account() { Level = 4, IsLeaf = true, Name = "CashBank Adjustment Expense", Code = Constant.AccountCode.CashBankAdjustmentExpense, LegacyCode = Constant.AccountLegacyCode.CashBankAdjustmentExpense, Group = Constant.AccountGroup.Expense, ParentId = SellingGeneralAndAdministrationExpense.Id, IsLegacy = true }, _accountService);
                    Discount = _accountService.CreateLegacyObject(new Account() { Level = 4, IsLeaf = true, Name = "Discount", Code = Constant.AccountCode.Discount, LegacyCode = Constant.AccountLegacyCode.Discount, Group = Constant.AccountGroup.Expense, ParentId = SellingGeneralAndAdministrationExpense.Id, IsLegacy = true }, _accountService);
                    SalesAllowance = _accountService.CreateLegacyObject(new Account() { Level = 4, IsLeaf = true, Name = "Sales Allowance", Code = Constant.AccountCode.SalesAllowance, LegacyCode = Constant.AccountLegacyCode.SalesAllowance, Group = Constant.AccountGroup.Expense, ParentId = SellingGeneralAndAdministrationExpense.Id, IsLegacy = true }, _accountService);
                    StockAdjustmentExpense = _accountService.CreateLegacyObject(new Account() { Level = 4, IsLeaf = true, Name = "Stock Adjustment Expense", Code = Constant.AccountCode.StockAdjustmentExpense, LegacyCode = Constant.AccountLegacyCode.StockAdjustmentExpense, Group = Constant.AccountGroup.Expense, ParentId = SellingGeneralAndAdministrationExpense.Id, IsLegacy = true }, _accountService);
                    NonOperationalExpense = _accountService.CreateObject(new Account() { Level = 2, Name = "Non Operational Expense", Code = Constant.AccountCode.NonOperationalExpense, LegacyCode = Constant.AccountLegacyCode.NonOperationalExpense, Group = Constant.AccountGroup.Expense, ParentId = Expense.Id, IsLegacy = true }, _accountService);
                    DepreciationExpense = _accountService.CreateObject(new Account() { Level = 3, IsLeaf = true, Name = "Depreciation Expense", Code = Constant.AccountCode.DepreciationExpense, LegacyCode = Constant.AccountLegacyCode.DepreciationExpense, Group = Constant.AccountGroup.Expense, ParentId = NonOperationalExpense.Id, IsLegacy = true }, _accountService);
                    Amortization = _accountService.CreateObject(new Account() { Level = 3, IsLeaf = true, Name = "Amortization", Code = Constant.AccountCode.Amortization, LegacyCode = Constant.AccountLegacyCode.Amortization, Group = Constant.AccountGroup.Expense, ParentId = NonOperationalExpense.Id, IsLegacy = true }, _accountService);
                    InterestExpense = _accountService.CreateObject(new Account() { Level = 3, IsLeaf = true, Name = "Interest Expense", Code = Constant.AccountCode.InterestExpense, LegacyCode = Constant.AccountLegacyCode.InterestExpense, Group = Constant.AccountGroup.Expense, ParentId = NonOperationalExpense.Id, IsLegacy = true }, _accountService);
                    TaxExpense = _accountService.CreateObject(new Account() { Level = 3, IsLeaf = true, Name = "Tax Expense", Code = Constant.AccountCode.TaxExpense, LegacyCode = Constant.AccountLegacyCode.TaxExpense, Group = Constant.AccountGroup.Expense, ParentId = NonOperationalExpense.Id, IsLegacy = true }, _accountService);
                    DividentExpense = _accountService.CreateObject(new Account() { Level = 3, IsLeaf = true, Name = "Divident Expense", Code = Constant.AccountCode.DividentExpense, LegacyCode = Constant.AccountLegacyCode.DividentExpense, Group = Constant.AccountGroup.Expense, ParentId = NonOperationalExpense.Id, IsLegacy = true }, _accountService);

                    Liability = _accountService.CreateLegacyObject(new Account() { Level = 1, Name = "Liability", Code = Constant.AccountCode.Liability, LegacyCode = Constant.AccountLegacyCode.Liability, Group = Constant.AccountGroup.Liability, IsLegacy = true }, _accountService);
                    CurrentLiability = _accountService.CreateLegacyObject(new Account() { Level = 2, Name = "Current Liability", Code = Constant.AccountCode.CurrentLiability, LegacyCode = Constant.AccountLegacyCode.CurrentLiability, Group = Constant.AccountGroup.Liability, ParentId = Liability.Id, IsLegacy = true }, _accountService);
                    AccountPayable = _accountService.CreateLegacyObject(new Account() { Level = 3, IsLeaf = true, Name = "Account Payable", Code = Constant.AccountCode.AccountPayable, LegacyCode = Constant.AccountLegacyCode.AccountPayable, Group = Constant.AccountGroup.Liability, ParentId = CurrentLiability.Id, IsLegacy = true }, _accountService);
                    GBCHPayable = _accountService.CreateLegacyObject(new Account() { Level = 3, IsLeaf = true, Name = "GBCH Payable", Code = Constant.AccountCode.GBCHPayable, LegacyCode = Constant.AccountLegacyCode.GBCHPayable, Group = Constant.AccountGroup.Liability, ParentId = CurrentLiability.Id, IsLegacy = true }, _accountService);
                    GoodsPendingClearance = _accountService.CreateLegacyObject(new Account() { Level = 3, IsLeaf = true, Name = "Goods Pending Clearance", Code = Constant.AccountCode.GoodsPendingClearance, LegacyCode = Constant.AccountLegacyCode.GoodsPendingClearance, Group = Constant.AccountGroup.Liability, ParentId = CurrentLiability.Id, IsLegacy = true }, _accountService);
                    UnearnedRevenue = _accountService.CreateLegacyObject(new Account() { Level = 3, IsLeaf = true, Name = "Unearned Revenue", Code = Constant.AccountCode.UnearnedRevenue, LegacyCode = Constant.AccountLegacyCode.UnearnedRevenue, Group = Constant.AccountGroup.Liability, ParentId = CurrentLiability.Id, IsLegacy = true }, _accountService);
                    PurchaseAllowance = _accountService.CreateLegacyObject(new Account() { Level = 3, IsLeaf = true, Name = "Purchase Allowance", Code = Constant.AccountCode.PurchaseAllowance, LegacyCode = Constant.AccountLegacyCode.PurchaseAllowance, Group = Constant.AccountGroup.Liability, ParentId = CurrentLiability.Id, IsLegacy = true }, _accountService);
                    NonCurrentLiability = _accountService.CreateLegacyObject(new Account() { Level = 2, Name = "Noncurrent Liability", Code = Constant.AccountCode.NonCurrentLiability, LegacyCode = Constant.AccountLegacyCode.NonCurrentLiability, Group = Constant.AccountGroup.Liability, ParentId = Liability.Id, IsLegacy = true }, _accountService);

                    Equity = _accountService.CreateLegacyObject(new Account() { Level = 1, Name = "Equity", Code = Constant.AccountCode.Equity, LegacyCode = Constant.AccountLegacyCode.Equity, Group = Constant.AccountGroup.Equity, IsLegacy = true }, _accountService);
                    OwnersEquity = _accountService.CreateLegacyObject(new Account() { Level = 2, Name = "Owners Equity", Code = Constant.AccountCode.OwnersEquity, LegacyCode = Constant.AccountLegacyCode.OwnersEquity, Group = Constant.AccountGroup.Equity, ParentId = Equity.Id, IsLegacy = true }, _accountService);
                    EquityAdjustment = _accountService.CreateLegacyObject(new Account() { Level = 3, IsLeaf = true, Name = "Equity Adjustment", Code = Constant.AccountCode.EquityAdjustment, LegacyCode = Constant.AccountLegacyCode.EquityAdjustment, Group = Constant.AccountGroup.Equity, ParentId = OwnersEquity.Id, IsLegacy = true }, _accountService);

                    Revenue = _accountService.CreateLegacyObject(new Account() { Level = 1, IsLeaf = true, Name = "Revenue", Code = Constant.AccountCode.Revenue, LegacyCode = Constant.AccountLegacyCode.Revenue, Group = Constant.AccountGroup.Revenue, IsLegacy = true }, _accountService);
                }

                Pcs = new UoM()
                {
                    Name = "Pcs"
                };
                _uomService.CreateObject(Pcs);

                contact = new Contact()
                {
                    Name = "President of Indonesia",
                    Address = "Istana Negara Jl. Veteran No. 16 Jakarta Pusat",
                    ContactNo = "021 3863777",
                    PIC = "Mr. President",
                    PICContactNo = "021 3863777",
                    Email = "random@ri.gov.au"
                };
                contact = _contactService.CreateObject(contact, _contactGroupService);

                type = _itemTypeService.CreateObject("Item", "Item");

                warehouse = new Warehouse()
                {
                    Name = "Sentral Solusi Data",
                    Description = "Kali Besar Jakarta",
                    Code = "LCL"
                };
                warehouse = _warehouseService.CreateObject(warehouse, _warehouseItemService, _itemService);

                item_batiktulis = new Item()
                {
                    ItemTypeId = _itemTypeService.GetObjectByName("Item").Id,
                    Name = "Batik Tulis",
                    Category = "Item",
                    Sku = "bt123",
                    UoMId = Pcs.Id
                };

                item_batiktulis = _itemService.CreateObject(item_batiktulis, _uomService, _itemTypeService, _warehouseItemService, _warehouseService, _priceMutationService, _contactGroupService);

                item_busway = new Item()
                {
                    ItemTypeId = _itemTypeService.GetObjectByName("Item").Id,
                    Name = "Busway",
                    Category = "Item",
                    Description = "Untuk disumbangkan bagi kebutuhan DKI Jakarta",
                    Sku = "DKI002",
                    UoMId = Pcs.Id
                };
                item_busway = _itemService.CreateObject(item_busway, _uomService, _itemTypeService, _warehouseItemService, _warehouseService, _priceMutationService, _contactGroupService);

                item_botolaqua = new Item()
                {
                    ItemTypeId = _itemTypeService.GetObjectByName("Item").Id,
                    Name = "Botol Aqua",
                    Category = "Item",
                    Description = "Minuman",
                    Sku = "DKI003",
                    UoMId = Pcs.Id
                };
                item_botolaqua = _itemService.CreateObject(item_botolaqua, _uomService, _itemTypeService, _warehouseItemService, _warehouseService, _priceMutationService, _contactGroupService);

                StockAdjustment sa = new StockAdjustment() { AdjustmentDate = DateTime.Today, WarehouseId = warehouse.Id, Description = "item adjustment" };
                _stockAdjustmentService.CreateObject(sa, _warehouseService);
                StockAdjustmentDetail sadBatikTulis = new StockAdjustmentDetail()
                {
                    ItemId = item_batiktulis.Id,
                    Quantity = 1000,
                    StockAdjustmentId = sa.Id,
                    Price = 5000
                };
                _stockAdjustmentDetailService.CreateObject(sadBatikTulis, _stockAdjustmentService, _itemService, _warehouseItemService);

                StockAdjustmentDetail sadBusWay = new StockAdjustmentDetail()
                {
                    ItemId = item_busway.Id,
                    Quantity = 200,
                    StockAdjustmentId = sa.Id,
                    Price = 5000
                };
                _stockAdjustmentDetailService.CreateObject(sadBusWay, _stockAdjustmentService, _itemService, _warehouseItemService);

                StockAdjustmentDetail sadBotolAqua = new StockAdjustmentDetail()
                {
                    ItemId = item_botolaqua.Id,
                    Quantity = 20000,
                    StockAdjustmentId = sa.Id,
                    Price = 5000
                };
                _stockAdjustmentDetailService.CreateObject(sadBotolAqua, _stockAdjustmentService, _itemService, _warehouseItemService);

                _stockAdjustmentService.ConfirmObject(sa, DateTime.Today, _stockAdjustmentDetailService, _stockMutationService,
                                                      _itemService, _blanketService, _warehouseItemService, _accountService, _generalLedgerJournalService);

                salesOrder1 = _salesOrderService.CreateObject(contact.Id, new DateTime(2014, 07, 09), _contactService);
                salesOrder2 = _salesOrderService.CreateObject(contact.Id, new DateTime(2014, 04, 09), _contactService);
                salesOrderDetail_batiktulis_so1 = _salesOrderDetailService.CreateObject(salesOrder1.Id, item_batiktulis.Id, 500, 2000000, _salesOrderService, _itemService);
                salesOrderDetail_busway_so1 = _salesOrderDetailService.CreateObject(salesOrder1.Id, item_busway.Id, 91, 800000000, _salesOrderService, _itemService);
                salesOrderDetail_botolaqua_so1 = _salesOrderDetailService.CreateObject(salesOrder1.Id, item_botolaqua.Id, 2000, 5000, _salesOrderService, _itemService);
                salesOrderDetail_batiktulis_so2 = _salesOrderDetailService.CreateObject(salesOrder2.Id, item_batiktulis.Id, 40, 2000500, _salesOrderService, _itemService);
                salesOrderDetail_busway_so2 = _salesOrderDetailService.CreateObject(salesOrder2.Id, item_busway.Id, 3, 810000000, _salesOrderService, _itemService);
                salesOrderDetail_botolaqua_so2 = _salesOrderDetailService.CreateObject(salesOrder2.Id, item_botolaqua.Id, 340, 5500, _salesOrderService, _itemService);
                salesOrder1 = _salesOrderService.ConfirmObject(salesOrder1, DateTime.Today, _salesOrderDetailService, _stockMutationService, _itemService, _blanketService, _warehouseItemService);
                salesOrder2 = _salesOrderService.ConfirmObject(salesOrder2, DateTime.Today, _salesOrderDetailService, _stockMutationService, _itemService, _blanketService, _warehouseItemService);
            }
        }

        void deliveryorder_validation()
        {
            it["validates_all_variables"] = () =>
            {
                contact.Errors.Count().should_be(0);
                item_batiktulis.Errors.Count().should_be(0);
                item_busway.Errors.Count().should_be(0);
                item_botolaqua.Errors.Count().should_be(0);
                salesOrder1.Errors.Count().should_be(0);
                salesOrder2.Errors.Count().should_be(0);
            };

            it["validates the item pending delivery"] = () =>
            {
                salesOrderDetail_batiktulis_so1.IsConfirmed.should_be(true);
                salesOrderDetail_busway_so1.IsConfirmed.should_be(true);
                salesOrderDetail_botolaqua_so1.IsConfirmed.should_be(true);
                salesOrderDetail_batiktulis_so2.IsConfirmed.should_be(true);
                salesOrderDetail_busway_so2.IsConfirmed.should_be(true);
                salesOrderDetail_botolaqua_so2.IsConfirmed.should_be(true);

                item_batiktulis.PendingDelivery.should_be(salesOrderDetail_batiktulis_so1.Quantity + salesOrderDetail_batiktulis_so2.Quantity);
                item_busway.PendingDelivery.should_be(salesOrderDetail_busway_so1.Quantity + salesOrderDetail_busway_so2.Quantity);
                item_botolaqua.PendingDelivery.should_be(salesOrderDetail_botolaqua_so1.Quantity + salesOrderDetail_botolaqua_so2.Quantity);
            };

            context["when confirming delivery order"] = () =>
            {
                before = () =>
                {
                    deliveryOrder1 = _deliveryOrderService.CreateObject(warehouse.Id, salesOrder1.Id, new DateTime(2000, 1, 1), _salesOrderService, _warehouseService);
                    deliveryOrder2 = _deliveryOrderService.CreateObject(warehouse.Id, salesOrder2.Id, new DateTime(2014, 5, 5), _salesOrderService, _warehouseService);
                    deliveryOrder3 = _deliveryOrderService.CreateObject(warehouse.Id, salesOrder1.Id, new DateTime(2014, 5, 5), _salesOrderService, _warehouseService);
                    deliveryOrderDetail_batiktulis_do1 = _deliveryOrderDetailService.CreateObject(deliveryOrder1.Id, item_batiktulis.Id, 400, salesOrderDetail_batiktulis_so1.Id, _deliveryOrderService,
                                                                                                  _salesOrderDetailService, _salesOrderService, _itemService);
                    deliveryOrderDetail_busway_do1 = _deliveryOrderDetailService.CreateObject(deliveryOrder1.Id, item_busway.Id, 91, salesOrderDetail_busway_so1.Id, _deliveryOrderService,
                                                                                                _salesOrderDetailService, _salesOrderService, _itemService);
                    deliveryOrderDetail_botolaqua_do1 = _deliveryOrderDetailService.CreateObject(deliveryOrder1.Id, item_botolaqua.Id, 2000, salesOrderDetail_botolaqua_so1.Id,  _deliveryOrderService,
                                                                                                  _salesOrderDetailService, _salesOrderService, _itemService);
                    deliveryOrderDetail_batiktulis_do2b = _deliveryOrderDetailService.CreateObject(deliveryOrder2.Id, item_batiktulis.Id, 40, salesOrderDetail_batiktulis_so2.Id, _deliveryOrderService,
                                                                                                                          _salesOrderDetailService, _salesOrderService, _itemService);
                    deliveryOrderDetail_busway_do2 = _deliveryOrderDetailService.CreateObject(deliveryOrder2.Id, item_busway.Id, 3, salesOrderDetail_busway_so2.Id, _deliveryOrderService,
                                                                                                                          _salesOrderDetailService, _salesOrderService, _itemService);
                    deliveryOrderDetail_botolaqua_do2 = _deliveryOrderDetailService.CreateObject(deliveryOrder2.Id, item_botolaqua.Id, 340, salesOrderDetail_botolaqua_so2.Id, _deliveryOrderService,
                                                                                                                          _salesOrderDetailService, _salesOrderService, _itemService);
                    deliveryOrderDetail_batiktulis_do2a = _deliveryOrderDetailService.CreateObject(deliveryOrder3.Id, item_batiktulis.Id, 100, salesOrderDetail_batiktulis_so1.Id, _deliveryOrderService,
                                                                                                                          _salesOrderDetailService, _salesOrderService, _itemService);
                    deliveryOrder1 = _deliveryOrderService.ConfirmObject(deliveryOrder1, DateTime.Today, _deliveryOrderDetailService, _salesOrderService, _salesOrderDetailService, _stockMutationService, _itemService,
                                                                         _blanketService, _warehouseItemService, _accountService, _generalLedgerJournalService, _closingService, _serviceCostService);
                    deliveryOrder2 = _deliveryOrderService.ConfirmObject(deliveryOrder2, DateTime.Today, _deliveryOrderDetailService, _salesOrderService, _salesOrderDetailService, _stockMutationService, _itemService,
                                                                         _blanketService, _warehouseItemService, _accountService, _generalLedgerJournalService, _closingService, _serviceCostService);
                    deliveryOrder3 = _deliveryOrderService.ConfirmObject(deliveryOrder3, DateTime.Today, _deliveryOrderDetailService, _salesOrderService, _salesOrderDetailService, _stockMutationService, _itemService,
                                                                         _blanketService, _warehouseItemService, _accountService, _generalLedgerJournalService, _closingService, _serviceCostService);
                };

                it["validates_deliveryorders"] = () =>
                {
                    deliveryOrder1.Errors.Count().should_be(0);
                    deliveryOrder2.Errors.Count().should_be(0);
                };

                it["deletes confirmed delivery order"] = () =>
                {
                    deliveryOrder1 = _deliveryOrderService.SoftDeleteObject(deliveryOrder1, _deliveryOrderDetailService);
                    deliveryOrder1.Errors.Count().should_not_be(0);
                };

                it["unconfirm delivery order"] = () =>
                {
                    deliveryOrder1 = _deliveryOrderService.UnconfirmObject(deliveryOrder1, _deliveryOrderDetailService,
                                                                           _salesInvoiceService, _salesInvoiceDetailService, _salesOrderService,
                                                                           _salesOrderDetailService, _stockMutationService, _itemService,
                                                                           _blanketService, _warehouseItemService, _accountService, _generalLedgerJournalService, _closingService);
                    deliveryOrder1.Errors.Count().should_be(0);
                };

                it["validates item pending delivery"] = () =>
                {
                    item_batiktulis.PendingDelivery.should_be(0);
                    item_busway.PendingDelivery.should_be(0);
                    item_botolaqua.PendingDelivery.should_be(0);
                };
            };
        }
    }
}