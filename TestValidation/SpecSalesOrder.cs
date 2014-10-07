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

    public class SpecSalesOrder : nspec
    {
        ContactGroup baseGroup;
        Contact contact;
        Item item_sepatubola;
        Item item_batiktulis;
        SalesOrder salesOrder;
        SalesOrderDetail salesOrderDetail1;
        SalesOrderDetail salesOrderDetail2;
        UoM Pcs;
        ItemType type;
        Warehouse warehouse;
        IContactService _contactService;
        IItemService _itemService;
        ISalesOrderService _salesOrderService;
        ISalesOrderDetailService _salesOrderDetailService;
        IDeliveryOrderService _deliveryOrderService;
        IDeliveryOrderDetailService _deliveryOrderDetailService;
        IStockMutationService _stockMutationService;
        IUoMService _uomService;
        IBlanketService _blanketService;
        IItemTypeService _itemTypeService;
        IWarehouseItemService _warehouseItemService;
        IWarehouseService _warehouseService;
        IStockAdjustmentService _stockAdjustmentService;
        IStockAdjustmentDetailService _stockAdjustmentDetailService;

        IPriceMutationService _priceMutationService;
        IContactGroupService _contactGroupService;
        IAccountService _accountService;
        IGeneralLedgerJournalService _generalLedgerJournalService;

        int Quantity1;
        int Quantity2;

        private Account Asset, CurrentAsset, CashBank, AccountReceivable, GBCHReceivable, Inventory, Raw, FinishedGoods, NonCurrentAsset;
        private Account Expense, COGS, COS, OperationalExpense, ManufacturingExpense, RecoveryExpense, ConversionExpense;
        private Account SellingGeneralAndAdministrationExpense, CashBankAdjustmentExpense, Discount, SalesAllowance, StockAdjustmentExpense;
        private Account NonOperationalExpense, DepreciationExpense, Amortization, InterestExpense, TaxExpense, DividentExpense;
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
                _salesOrderService = new SalesOrderService(new SalesOrderRepository(), new SalesOrderValidator());
                _salesOrderDetailService = new SalesOrderDetailService(new SalesOrderDetailRepository(), new SalesOrderDetailValidator());
                _deliveryOrderService = new DeliveryOrderService(new DeliveryOrderRepository(), new DeliveryOrderValidator());
                _deliveryOrderDetailService = new DeliveryOrderDetailService(new DeliveryOrderDetailRepository(), new DeliveryOrderDetailValidator());
                _stockMutationService = new StockMutationService(new StockMutationRepository(), new StockMutationValidator());
                _stockAdjustmentDetailService = new StockAdjustmentDetailService(new StockAdjustmentDetailRepository(), new StockAdjustmentDetailValidator());
                _stockAdjustmentService = new StockAdjustmentService(new StockAdjustmentRepository(), new StockAdjustmentValidator());
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
                    FinishedGoods = _accountService.CreateLegacyObject(new Account() { Level = 4, IsLeaf = true, Name = "Finished Goods", Code = Constant.AccountCode.FinishedGoods, LegacyCode = Constant.AccountLegacyCode.FinishedGoods, Group = Constant.AccountGroup.Asset, ParentId = Inventory.Id, IsLegacy = true }, _accountService);
                    NonCurrentAsset = _accountService.CreateLegacyObject(new Account() { Level = 2, Name = "Noncurrent Asset", Code = Constant.AccountCode.NonCurrentAsset, LegacyCode = Constant.AccountLegacyCode.NonCurrentAsset, Group = Constant.AccountGroup.Asset, ParentId = Asset.Id, IsLegacy = true }, _accountService);

                    Expense = _accountService.CreateLegacyObject(new Account() { Level = 1, Name = "Expense", Code = Constant.AccountCode.Expense, LegacyCode = Constant.AccountLegacyCode.Expense, Group = Constant.AccountGroup.Expense, IsLegacy = true }, _accountService);
                    COGS = _accountService.CreateLegacyObject(new Account() { Level = 2, IsLeaf = true, Name = "Cost Of Goods Sold", Code = Constant.AccountCode.COGS, LegacyCode = Constant.AccountLegacyCode.COGS, Group = Constant.AccountGroup.Expense, ParentId = Expense.Id, IsLegacy = true }, _accountService);
                    COS = _accountService.CreateLegacyObject(new Account() { Level = 2, IsLeaf = true, Name = "Cost of Services", Code = Constant.AccountCode.COS, LegacyCode = Constant.AccountLegacyCode.COS, Group = Constant.AccountGroup.Expense, ParentId = Expense.Id, IsLegacy = true }, _accountService);
                    OperationalExpense = _accountService.CreateLegacyObject(new Account() { Level = 2, Name = "Operational Expense", Code = Constant.AccountCode.OperationalExpense, LegacyCode = Constant.AccountLegacyCode.OperationalExpense, Group = Constant.AccountGroup.Expense, ParentId = Expense.Id, IsLegacy = true }, _accountService);
                    ManufacturingExpense = _accountService.CreateLegacyObject(new Account() { Level = 3, Name = "Manufacturing Expense", Code = Constant.AccountCode.ManufacturingExpense, LegacyCode = Constant.AccountLegacyCode.ManufacturingExpense, Group = Constant.AccountGroup.Expense, ParentId = OperationalExpense.Id, IsLegacy = true }, _accountService);
                    RecoveryExpense = _accountService.CreateLegacyObject(new Account() { Level = 4, IsLeaf = true, Name = "Roller Recovery Expense", Code = Constant.AccountCode.RecoveryExpense, LegacyCode = Constant.AccountLegacyCode.RecoveryExpense, Group = Constant.AccountGroup.Expense, ParentId = ManufacturingExpense.Id, IsLegacy = true }, _accountService);
                    ConversionExpense = _accountService.CreateLegacyObject(new Account() { Level = 4, IsLeaf = true, Name = "Roller Recovery Expense", Code = Constant.AccountCode.ConversionExpense, LegacyCode = Constant.AccountLegacyCode.ConversionExpense, Group = Constant.AccountGroup.Expense, ParentId = ManufacturingExpense.Id, IsLegacy = true }, _accountService);
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
                _itemService.CreateObject(item_batiktulis, _uomService, _itemTypeService, _warehouseItemService, _warehouseService, _priceMutationService, _contactGroupService);

                item_sepatubola = new Item()
                {
                    ItemTypeId = _itemTypeService.GetObjectByName("Item").Id,
                    Name = "Sepatu Bola",
                    Category = "Item",
                    Sku = "sb123",
                    UoMId = Pcs.Id
                };
                _itemService.CreateObject(item_sepatubola, _uomService, _itemTypeService, _warehouseItemService, _warehouseService, _priceMutationService, _contactGroupService);

                StockAdjustment sa = new StockAdjustment() { AdjustmentDate = DateTime.Today, WarehouseId = warehouse.Id, Description = "item adjustment" };
                _stockAdjustmentService.CreateObject(sa, _warehouseService);
                StockAdjustmentDetail sadSepatuBola = new StockAdjustmentDetail()
                {
                    ItemId = item_sepatubola.Id,
                    Quantity = 1000,
                    StockAdjustmentId = sa.Id,
                    Price = 5000
                };
                _stockAdjustmentDetailService.CreateObject(sadSepatuBola, _stockAdjustmentService, _itemService, _warehouseItemService);

                StockAdjustmentDetail sadBatikTulis = new StockAdjustmentDetail()
                {
                    ItemId = item_batiktulis.Id,
                    Quantity = 1000,
                    StockAdjustmentId = sa.Id,
                    Price = 500
                };
                _stockAdjustmentDetailService.CreateObject(sadBatikTulis, _stockAdjustmentService, _itemService, _warehouseItemService);

                _stockAdjustmentService.ConfirmObject(sa, DateTime.Today, _stockAdjustmentDetailService, _stockMutationService,
                                                      _itemService, _blanketService, _warehouseItemService, _accountService, _generalLedgerJournalService);

            }
        }

        void salesorder_validation()
        {
            it["validate_contact_and_items"] = () =>
            {
                contact.Errors.Count().should_be(0);
                item_batiktulis.Errors.Count().should_be(0);
                item_sepatubola.Errors.Count().should_be(0);
            };

            it["create_salesorder"] = () =>
            {
                salesOrder = _salesOrderService.CreateObject(contact.Id, new DateTime(2000, 1, 1), _contactService);
                salesOrder.Errors.Count().should_be(0);
            };

            it["create_salesorder_with_no_contactid"] = () =>
            {
                salesOrder = new SalesOrder()
                {
                    SalesDate = DateTime.Now
                };
                _salesOrderService.CreateObject(salesOrder, _contactService);
                salesOrder.Errors.Count().should_not_be(0);
            };

            it["create_salesorder_with_no_elements"] = () =>
            {
                salesOrder = new SalesOrder();
                _salesOrderService.CreateObject(salesOrder, _contactService);
                salesOrder.Errors.Count().should_not_be(0);
            };

            context["validating_salesorder"] = () =>
            {
                before = () =>
                {
                    salesOrder = new SalesOrder
                    {
                        ContactId = contact.Id,
                        SalesDate = DateTime.Now
                    };
                    salesOrder = _salesOrderService.CreateObject(salesOrder, _contactService);
                };

                it["delete_salesorder"] = () =>
                {
                    salesOrder = _salesOrderService.SoftDeleteObject(salesOrder, _salesOrderDetailService);
                    salesOrder.Errors.Count().should_be(0);
                };

                it["delete_salesorder_and_details"] = () =>
                {
                    salesOrderDetail1 = _salesOrderDetailService.CreateObject(salesOrder.Id, item_batiktulis.Id, 5, 100000, _salesOrderService, _itemService);
                    salesOrderDetail2 = _salesOrderDetailService.CreateObject(salesOrder.Id, item_sepatubola.Id, 12, 850000, _salesOrderService, _itemService);
                    salesOrder = _salesOrderService.SoftDeleteObject(salesOrder, _salesOrderDetailService);
                    salesOrder.Errors.Count().should_not_be(0);
                };

                it["delete_salesorderdetail"] = () =>
                {
                    salesOrderDetail1 = _salesOrderDetailService.CreateObject(salesOrder.Id, item_batiktulis.Id, 5, 100000, _salesOrderService, _itemService);
                    salesOrderDetail2 = _salesOrderDetailService.CreateObject(salesOrder.Id, item_sepatubola.Id, 12, 850000, _salesOrderService, _itemService);
                    salesOrderDetail2 = _salesOrderDetailService.SoftDeleteObject(salesOrderDetail2);
                    salesOrderDetail2.Errors.Count().should_be(0);
                };

                it["create_salesorderdetails_with_same_item"] = () =>
                {
                    salesOrderDetail1 = _salesOrderDetailService.CreateObject(salesOrder.Id, item_batiktulis.Id, 5, 100000, _salesOrderService, _itemService);
                    salesOrderDetail2 = _salesOrderDetailService.CreateObject(salesOrder.Id, item_batiktulis.Id, 12, 850000, _salesOrderService, _itemService);
                    salesOrderDetail2.Errors.Count().should_not_be(0);
                };

                context["when confirming SO"] = () =>
                {
                    before = () =>
                    {
                        salesOrderDetail1 = _salesOrderDetailService.CreateObject(salesOrder.Id, item_batiktulis.Id, 5, 100000, _salesOrderService, _itemService);
                        salesOrderDetail2 = _salesOrderDetailService.CreateObject(salesOrder.Id, item_sepatubola.Id, 12, 850000, _salesOrderService, _itemService);
                        Quantity1 = item_batiktulis.PendingDelivery;
                        Quantity2 = item_sepatubola.PendingDelivery;
                        salesOrder = _salesOrderService.ConfirmObject(salesOrder, DateTime.Today, _salesOrderDetailService, _stockMutationService, _itemService, _blanketService, _warehouseItemService);
                    };

                    it["confirmed_salesorder"] = () =>
                    {
                        salesOrder.Errors.Count().should_be(0);
                    };

                    it["check_detailsconfirmation"] = () =>
                    {
                        IList<StockMutation> stockMutationsDetail1 = _stockMutationService.GetObjectsBySourceDocumentDetailForItem(item_batiktulis.Id, Core.Constants.Constant.SourceDocumentDetailType.SalesOrderDetail, salesOrderDetail1.Id);
                        stockMutationsDetail1.Count().should_be(1);
                        IList<StockMutation> stockMutationsDetail2 = _stockMutationService.GetObjectsBySourceDocumentDetailForItem(item_sepatubola.Id, Core.Constants.Constant.SourceDocumentDetailType.SalesOrderDetail, salesOrderDetail2.Id);
                        stockMutationsDetail2.Count().should_be(1);
                        salesOrderDetail1.IsConfirmed.should_be(true);
                        salesOrderDetail2.IsConfirmed.should_be(true);
                    };

                    it["delete_confirmed_salesorder"] = () =>
                    {
                        salesOrder = _salesOrderService.SoftDeleteObject(salesOrder, _salesOrderDetailService);
                        salesOrder.Errors.Count().should_not_be(0);
                    };

                    it["unfinish_salesorderdetail"] = () =>
                    {
                        _salesOrderService.UnconfirmObject(salesOrder, _salesOrderDetailService, _deliveryOrderService, _deliveryOrderDetailService, _stockMutationService,
                                                           _itemService, _blanketService, _warehouseItemService);
                        salesOrderDetail1.IsConfirmed.should_be(false);
                        salesOrderDetail2.IsConfirmed.should_be(false);
                        salesOrderDetail1.Errors.Count().should_be(0);
                        salesOrderDetail2.Errors.Count().should_be(0);
                        IList<StockMutation> stockMutationsDetail1 = _stockMutationService.GetObjectsBySourceDocumentDetailForItem(item_batiktulis.Id, Core.Constants.Constant.SourceDocumentDetailType.SalesOrderDetail, salesOrderDetail1.Id);
                        stockMutationsDetail1.Count().should_be(0);
                        IList<StockMutation> stockMutationsDetail2 = _stockMutationService.GetObjectsBySourceDocumentDetailForItem(item_sepatubola.Id, Core.Constants.Constant.SourceDocumentDetailType.SalesOrderDetail, salesOrderDetail2.Id);
                        stockMutationsDetail2.Count().should_be(0);
                    };

                    it["delete_unfinish_salesorderdetail"] = () =>
                    {
                        _salesOrderService.UnconfirmObject(salesOrder, _salesOrderDetailService, _deliveryOrderService, _deliveryOrderDetailService, _stockMutationService,
                                                           _itemService, _blanketService, _warehouseItemService);
                        salesOrderDetail2 = _salesOrderDetailService.SoftDeleteObject(salesOrderDetail2);
                        salesOrderDetail2.Errors.Count().should_be(0);
                        salesOrderDetail2.IsDeleted.should_be(true);
                    };

                    it["should increase pending delivery in item"] = () =>
                    {
                        Item NewItem1 = _itemService.GetObjectById(item_batiktulis.Id);
                        Item NewItem2 = _itemService.GetObjectById(item_sepatubola.Id);

                        int diff_1 = NewItem1.PendingDelivery - Quantity1;
                        diff_1.should_be(salesOrderDetail1.Quantity);

                        int diff_2 = NewItem2.PendingDelivery - Quantity2;
                        diff_2.should_be(salesOrderDetail2.Quantity);
                    };
                };
            };
        }
    }
}