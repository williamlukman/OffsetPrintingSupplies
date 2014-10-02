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

    public class SpecPurchaseReceival : nspec
    {
        ContactGroup baseGroup;
        Contact contact;
        Item item_batiktulis;
        Item item_busway;
        Item item_botolaqua;
        UoM Pcs;
        ItemType type;
        Warehouse warehouse;
        PurchaseOrder purchaseOrder1;
        PurchaseOrder purchaseOrder2;
        PurchaseOrderDetail purchaseOrderDetail_batiktulis_so1;
        PurchaseOrderDetail purchaseOrderDetail_busway_so1;
        PurchaseOrderDetail purchaseOrderDetail_botolaqua_so1;
        PurchaseOrderDetail purchaseOrderDetail_batiktulis_so2;
        PurchaseOrderDetail purchaseOrderDetail_busway_so2;
        PurchaseOrderDetail purchaseOrderDetail_botolaqua_so2;
        PurchaseReceival purchaseReceival1;
        PurchaseReceival purchaseReceival2;
        PurchaseReceival purchaseReceival3;
        PurchaseReceivalDetail purchaseReceivalDetail_batiktulis_do1;
        PurchaseReceivalDetail purchaseReceivalDetail_busway_do1;
        PurchaseReceivalDetail purchaseReceivalDetail_botolaqua_do1;
        PurchaseReceivalDetail purchaseReceivalDetail_batiktulis_do2a;
        PurchaseReceivalDetail purchaseReceivalDetail_batiktulis_do2b;
        PurchaseReceivalDetail purchaseReceivalDetail_busway_do2;
        PurchaseReceivalDetail purchaseReceivalDetail_botolaqua_do2;
        IContactService _contactService;
        IItemService _itemService;
        IStockMutationService _stockMutationService;
        IStockAdjustmentService _stockAdjustmentService;
        IStockAdjustmentDetailService _stockAdjustmentDetailService;
        IPurchaseInvoiceDetailService _purchaseInvoiceDetailService;
        IPurchaseInvoiceService _purchaseInvoiceService;
        IPurchaseOrderService _purchaseOrderService;
        IPurchaseOrderDetailService _purchaseOrderDetailService;
        IPurchaseReceivalService _purchaseReceivalService;
        IPurchaseReceivalDetailService _purchaseReceivalDetailService;
        IUoMService _uomService;
        IBlanketService _blanketService;
        IItemTypeService _itemTypeService;
        IWarehouseItemService _warehouseItemService;
        IWarehouseService _warehouseService;

        IPriceMutationService _priceMutationService;
        IContactGroupService _contactGroupService;
        IAccountService _accountService;
        IGeneralLedgerJournalService _generalLedgerJournalService;

        private Account Asset, CurrentAsset, CashBank, AccountReceivable, GBCHReceivable, Inventory, Raw, FinishedGoods, NonCurrentAsset;
        private Account Expense, COGS, SellingGeneralAndAdministrationExpense, CashBankAdjustmentExpense, Discount, SalesAllowance, StockAdjustmentExpense;
        private Account DepreciationExpense, Amortization, InterestExpense, TaxExpense, DividentExpense;
        private Account Liability, CurrentLiability, AccountPayable, GBCHPayable, GoodsPendingClearance, NonCurrentLiability;
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
                _purchaseInvoiceService = new PurchaseInvoiceService(new PurchaseInvoiceRepository(), new PurchaseInvoiceValidator());
                _purchaseInvoiceDetailService = new PurchaseInvoiceDetailService(new PurchaseInvoiceDetailRepository(), new PurchaseInvoiceDetailValidator());
                _purchaseOrderService = new PurchaseOrderService(new PurchaseOrderRepository(), new PurchaseOrderValidator());
                _purchaseOrderDetailService = new PurchaseOrderDetailService(new PurchaseOrderDetailRepository(), new PurchaseOrderDetailValidator());
                _purchaseReceivalService = new PurchaseReceivalService(new PurchaseReceivalRepository(), new PurchaseReceivalValidator());
                _purchaseReceivalDetailService = new PurchaseReceivalDetailService(new PurchaseReceivalDetailRepository(), new PurchaseReceivalDetailValidator());
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
                    FinishedGoods = _accountService.CreateLegacyObject(new Account() { Level = 4, IsLeaf = true, Name = "Finished Goods", Code = Constant.AccountCode.FinishedGoods, LegacyCode = Constant.AccountCode.FinishedGoods, Group = Constant.AccountGroup.Asset, ParentId = Inventory.Id, IsLegacy = true }, _accountService);
                    NonCurrentAsset = _accountService.CreateLegacyObject(new Account() { Level = 2, Name = "Noncurrent Asset", Code = Constant.AccountCode.NonCurrentAsset, LegacyCode = Constant.AccountLegacyCode.NonCurrentAsset, Group = Constant.AccountGroup.Asset, ParentId = Asset.Id, IsLegacy = true }, _accountService);

                    Expense = _accountService.CreateLegacyObject(new Account() { Level = 1, Name = "Expense", Code = Constant.AccountCode.Expense, LegacyCode = Constant.AccountLegacyCode.Expense, Group = Constant.AccountGroup.Expense, IsLegacy = true }, _accountService);
                    COGS = _accountService.CreateLegacyObject(new Account() { Level = 2, IsLeaf = true, Name = "Cost Of Goods Sold", Code = Constant.AccountCode.COGS, LegacyCode = Constant.AccountLegacyCode.COGS, Group = Constant.AccountGroup.Expense, ParentId = Expense.Id, IsLegacy = true }, _accountService);
                    SellingGeneralAndAdministrationExpense = _accountService.CreateLegacyObject(new Account() { Level = 2, Name = "Selling, General, and Administration Expense", Code = Constant.AccountCode.SellingGeneralAndAdministrationExpense, LegacyCode = Constant.AccountLegacyCode.SellingGeneralAndAdministrationExpense, Group = Constant.AccountGroup.Expense, ParentId = Expense.Id, IsLegacy = true }, _accountService);
                    CashBankAdjustmentExpense = _accountService.CreateLegacyObject(new Account() { Level = 3, IsLeaf = true, Name = "CashBank Adjustment Expense", Code = Constant.AccountCode.CashBankAdjustmentExpense, LegacyCode = Constant.AccountLegacyCode.CashBankAdjustmentExpense, Group = Constant.AccountGroup.Expense, ParentId = SellingGeneralAndAdministrationExpense.Id, IsLegacy = true }, _accountService);
                    Discount = _accountService.CreateLegacyObject(new Account() { Level = 3, IsLeaf = true, Name = "Discount", Code = Constant.AccountCode.Discount, LegacyCode = Constant.AccountLegacyCode.Discount, Group = Constant.AccountGroup.Expense, ParentId = SellingGeneralAndAdministrationExpense.Id, IsLegacy = true }, _accountService);
                    SalesAllowance = _accountService.CreateLegacyObject(new Account() { Level = 3, IsLeaf = true, Name = "Sales Allowance", Code = Constant.AccountCode.SalesAllowance, LegacyCode = Constant.AccountLegacyCode.SalesAllowance, Group = Constant.AccountGroup.Expense, ParentId = SellingGeneralAndAdministrationExpense.Id, IsLegacy = true }, _accountService);
                    StockAdjustmentExpense = _accountService.CreateLegacyObject(new Account() { Level = 3, IsLeaf = true, Name = "Stock Adjustment Expense", Code = Constant.AccountCode.StockAdjustmentExpense, LegacyCode = Constant.AccountLegacyCode.StockAdjustmentExpense, Group = Constant.AccountGroup.Expense, ParentId = SellingGeneralAndAdministrationExpense.Id, IsLegacy = true }, _accountService);
                    DepreciationExpense = _accountService.CreateObject(new Account() { Level = 2, IsLeaf = true, Name = "Depreciation Expense", Code = Constant.AccountCode.DepreciationExpense, LegacyCode = Constant.AccountLegacyCode.DepreciationExpense, Group = Constant.AccountGroup.Expense, ParentId = Expense.Id, IsLegacy = true }, _accountService);
                    Amortization = _accountService.CreateObject(new Account() { Level = 2, IsLeaf = true, Name = "Amortization", Code = Constant.AccountCode.Amortization, LegacyCode = Constant.AccountLegacyCode.Amortization, Group = Constant.AccountGroup.Expense, ParentId = Expense.Id, IsLegacy = true }, _accountService);
                    InterestExpense = _accountService.CreateObject(new Account() { Level = 2, IsLeaf = true, Name = "Interest Expense", Code = Constant.AccountCode.InterestExpense, LegacyCode = Constant.AccountLegacyCode.InterestExpense, Group = Constant.AccountGroup.Expense, ParentId = Expense.Id, IsLegacy = true }, _accountService);
                    TaxExpense = _accountService.CreateObject(new Account() { Level = 2, IsLeaf = true, Name = "Tax Expense", Code = Constant.AccountCode.TaxExpense, LegacyCode = Constant.AccountLegacyCode.TaxExpense, Group = Constant.AccountGroup.Expense, ParentId = Expense.Id, IsLegacy = true }, _accountService);
                    DividentExpense = _accountService.CreateObject(new Account() { Level = 2, IsLeaf = true, Name = "Divident Expense", Code = Constant.AccountCode.DividentExpense, LegacyCode = Constant.AccountLegacyCode.DividentExpense, Group = Constant.AccountGroup.Expense, ParentId = Expense.Id, IsLegacy = true }, _accountService);

                    Liability = _accountService.CreateLegacyObject(new Account() { Level = 1, Name = "Liability", Code = Constant.AccountCode.Liability, LegacyCode = Constant.AccountLegacyCode.Liability, Group = Constant.AccountGroup.Liability, IsLegacy = true }, _accountService);
                    CurrentLiability = _accountService.CreateLegacyObject(new Account() { Level = 2, Name = "Current Liability", Code = Constant.AccountCode.CurrentLiability, LegacyCode = Constant.AccountLegacyCode.CurrentLiability, Group = Constant.AccountGroup.Liability, ParentId = Liability.Id, IsLegacy = true }, _accountService);
                    AccountPayable = _accountService.CreateLegacyObject(new Account() { Level = 3, IsLeaf = true, Name = "Account Payable", Code = Constant.AccountCode.AccountPayable, LegacyCode = Constant.AccountLegacyCode.AccountPayable, Group = Constant.AccountGroup.Liability, ParentId = CurrentLiability.Id, IsLegacy = true }, _accountService);
                    GBCHPayable = _accountService.CreateLegacyObject(new Account() { Level = 3, IsLeaf = true, Name = "GBCH Payable", Code = Constant.AccountCode.GBCHPayable, LegacyCode = Constant.AccountLegacyCode.GBCHPayable, Group = Constant.AccountGroup.Liability, ParentId = CurrentLiability.Id, IsLegacy = true }, _accountService);
                    GoodsPendingClearance = _accountService.CreateLegacyObject(new Account() { Level = 3, IsLeaf = true, Name = "Goods Pending Clearance", Code = Constant.AccountCode.GoodsPendingClearance, LegacyCode = Constant.AccountLegacyCode.GoodsPendingClearance, Group = Constant.AccountGroup.Liability, ParentId = CurrentLiability.Id, IsLegacy = true }, _accountService);
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
                    Category = "Untuk disumbangkan bagi kebutuhan DKI Jakarta",
                    Sku = "DKI002",
                    UoMId = Pcs.Id
                };
                item_busway = _itemService.CreateObject(item_busway, _uomService, _itemTypeService, _warehouseItemService, _warehouseService, _priceMutationService, _contactGroupService);

                item_botolaqua = new Item()
                {
                    ItemTypeId = _itemTypeService.GetObjectByName("Item").Id,
                    Name = "Botol Aqua",
                    Category = "Minuman",
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
                    StockAdjustmentId = sa.Id
                };
                _stockAdjustmentDetailService.CreateObject(sadBatikTulis, _stockAdjustmentService, _itemService, _warehouseItemService);

                StockAdjustmentDetail sadBusWay = new StockAdjustmentDetail()
                {
                    ItemId = item_busway.Id,
                    Quantity = 200,
                    StockAdjustmentId = sa.Id
                };
                _stockAdjustmentDetailService.CreateObject(sadBusWay, _stockAdjustmentService, _itemService, _warehouseItemService);

                StockAdjustmentDetail sadBotolAqua = new StockAdjustmentDetail()
                {
                    ItemId = item_botolaqua.Id,
                    Quantity = 20000,
                    StockAdjustmentId = sa.Id
                };
                _stockAdjustmentDetailService.CreateObject(sadBotolAqua, _stockAdjustmentService, _itemService, _warehouseItemService);

                _stockAdjustmentService.ConfirmObject(sa, DateTime.Today, _stockAdjustmentDetailService, _stockMutationService,
                                                      _itemService, _blanketService, _warehouseItemService, _accountService, _generalLedgerJournalService);

                purchaseOrder1 = _purchaseOrderService.CreateObject(contact.Id, new DateTime(2014, 07, 09), _contactService);
                purchaseOrder2 = _purchaseOrderService.CreateObject(contact.Id, new DateTime(2014, 04, 09), _contactService);
                purchaseOrderDetail_batiktulis_so1 = _purchaseOrderDetailService.CreateObject(purchaseOrder1.Id, item_batiktulis.Id, 500, 2000000, _purchaseOrderService, _itemService);
                purchaseOrderDetail_busway_so1 = _purchaseOrderDetailService.CreateObject(purchaseOrder1.Id, item_busway.Id, 91, 800000000, _purchaseOrderService, _itemService);
                purchaseOrderDetail_botolaqua_so1 = _purchaseOrderDetailService.CreateObject(purchaseOrder1.Id, item_botolaqua.Id, 2000, 5000, _purchaseOrderService, _itemService);
                purchaseOrderDetail_batiktulis_so2 = _purchaseOrderDetailService.CreateObject(purchaseOrder2.Id, item_batiktulis.Id, 40, 2000500, _purchaseOrderService, _itemService);
                purchaseOrderDetail_busway_so2 = _purchaseOrderDetailService.CreateObject(purchaseOrder2.Id, item_busway.Id, 3, 810000000, _purchaseOrderService, _itemService);
                purchaseOrderDetail_botolaqua_so2 = _purchaseOrderDetailService.CreateObject(purchaseOrder2.Id, item_botolaqua.Id, 340, 5500, _purchaseOrderService, _itemService);
                purchaseOrder1 = _purchaseOrderService.ConfirmObject(purchaseOrder1, DateTime.Today, _purchaseOrderDetailService, _stockMutationService, _itemService, _blanketService, _warehouseItemService);
                purchaseOrder2 = _purchaseOrderService.ConfirmObject(purchaseOrder2, DateTime.Today, _purchaseOrderDetailService, _stockMutationService, _itemService, _blanketService, _warehouseItemService);
            }
        }

        void purchasereceival_validation()
        {
            it["validates_all_variables"] = () =>
            {
                contact.Errors.Count().should_be(0);
                item_batiktulis.Errors.Count().should_be(0);
                item_busway.Errors.Count().should_be(0);
                item_botolaqua.Errors.Count().should_be(0);
                purchaseOrder1.Errors.Count().should_be(0);
                purchaseOrder2.Errors.Count().should_be(0);
            };

            it["validates the item pending receival"] = () =>
            {
                item_batiktulis.PendingReceival.should_be(purchaseOrderDetail_batiktulis_so1.Quantity + purchaseOrderDetail_batiktulis_so2.Quantity);
                item_busway.PendingReceival.should_be(purchaseOrderDetail_busway_so1.Quantity + purchaseOrderDetail_busway_so2.Quantity);
                item_botolaqua.PendingReceival.should_be(purchaseOrderDetail_botolaqua_so1.Quantity + purchaseOrderDetail_botolaqua_so2.Quantity);
            };

            context["when confirming purchase receival"] = () =>
            {
                before = () =>
                {
                    purchaseReceival1 = _purchaseReceivalService.CreateObject(warehouse.Id, purchaseOrder1.Id, new DateTime(2000, 1, 1), _purchaseOrderService, _warehouseService);
                    purchaseReceival2 = _purchaseReceivalService.CreateObject(warehouse.Id, purchaseOrder2.Id, new DateTime(2014, 5, 5), _purchaseOrderService, _warehouseService);
                    purchaseReceival3 = _purchaseReceivalService.CreateObject(warehouse.Id, purchaseOrder1.Id, new DateTime(2014, 5, 5), _purchaseOrderService, _warehouseService);
                    purchaseReceivalDetail_batiktulis_do1 = _purchaseReceivalDetailService.CreateObject(purchaseReceival1.Id, item_batiktulis.Id, 400, purchaseOrderDetail_batiktulis_so1.Id, _purchaseReceivalService,
                                                                                                  _purchaseOrderDetailService, _purchaseOrderService, _itemService);
                    purchaseReceivalDetail_busway_do1 = _purchaseReceivalDetailService.CreateObject(purchaseReceival1.Id, item_busway.Id, 91, purchaseOrderDetail_busway_so1.Id, _purchaseReceivalService,
                                                                                                _purchaseOrderDetailService, _purchaseOrderService, _itemService);
                    purchaseReceivalDetail_botolaqua_do1 = _purchaseReceivalDetailService.CreateObject(purchaseReceival1.Id, item_botolaqua.Id, 2000, purchaseOrderDetail_botolaqua_so1.Id,  _purchaseReceivalService,
                                                                                                  _purchaseOrderDetailService, _purchaseOrderService, _itemService);
                    purchaseReceivalDetail_batiktulis_do2b = _purchaseReceivalDetailService.CreateObject(purchaseReceival2.Id, item_batiktulis.Id, 40, purchaseOrderDetail_batiktulis_so2.Id, _purchaseReceivalService,
                                                                                                                          _purchaseOrderDetailService, _purchaseOrderService, _itemService);
                    purchaseReceivalDetail_busway_do2 = _purchaseReceivalDetailService.CreateObject(purchaseReceival2.Id, item_busway.Id, 3, purchaseOrderDetail_busway_so2.Id, _purchaseReceivalService,
                                                                                                                          _purchaseOrderDetailService, _purchaseOrderService, _itemService);
                    purchaseReceivalDetail_botolaqua_do2 = _purchaseReceivalDetailService.CreateObject(purchaseReceival2.Id, item_botolaqua.Id, 340, purchaseOrderDetail_botolaqua_so2.Id, _purchaseReceivalService,
                                                                                                                          _purchaseOrderDetailService, _purchaseOrderService, _itemService);
                    purchaseReceivalDetail_batiktulis_do2a = _purchaseReceivalDetailService.CreateObject(purchaseReceival3.Id, item_batiktulis.Id, 100, purchaseOrderDetail_batiktulis_so1.Id, _purchaseReceivalService,
                                                                                                                          _purchaseOrderDetailService, _purchaseOrderService, _itemService);
                    purchaseReceival1 = _purchaseReceivalService.ConfirmObject(purchaseReceival1, DateTime.Today, _purchaseReceivalDetailService, _purchaseOrderService, _purchaseOrderDetailService, _stockMutationService,
                                                                               _itemService, _blanketService, _warehouseItemService);
                    purchaseReceival2 = _purchaseReceivalService.ConfirmObject(purchaseReceival2, DateTime.Today, _purchaseReceivalDetailService, _purchaseOrderService, _purchaseOrderDetailService, _stockMutationService,
                                                                               _itemService, _blanketService, _warehouseItemService);
                    purchaseReceival3 = _purchaseReceivalService.ConfirmObject(purchaseReceival3, DateTime.Today, _purchaseReceivalDetailService, _purchaseOrderService, _purchaseOrderDetailService, _stockMutationService,
                                                                               _itemService, _blanketService, _warehouseItemService);
                };

                it["validates_purchasereceivals"] = () =>
                {
                    purchaseReceival1.Errors.Count().should_be(0);
                    purchaseReceival2.Errors.Count().should_be(0);
                };

                it["deletes confirmed purchase receival"] = () =>
                {
                    purchaseReceival1 = _purchaseReceivalService.SoftDeleteObject(purchaseReceival1, _purchaseReceivalDetailService);
                    purchaseReceival1.Errors.Count().should_not_be(0);
                };

                it["unconfirm purchase receival"] = () =>
                {
                    purchaseReceival1 = _purchaseReceivalService.UnconfirmObject(purchaseReceival1, _purchaseReceivalDetailService, _purchaseInvoiceService, _purchaseInvoiceDetailService,
                                                                                 _purchaseOrderService, _purchaseOrderDetailService, _stockMutationService, _itemService, _blanketService, _warehouseItemService);
                    purchaseReceival1.Errors.Count().should_be(0);
                };

                it["validates item pending receival"] = () =>
                {
                    item_batiktulis.PendingReceival.should_be(0);
                    item_busway.PendingReceival.should_be(0);
                    item_botolaqua.PendingReceival.should_be(0);
                };
            };
        }
    }
}