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

namespace NSpec
{

    public class SpecPurchaseOrder : nspec
    {
        ContactGroup baseGroup;
        Contact contact;
        PurchaseOrderDetail poDetail1;
        PurchaseOrderDetail poDetail2;
        PurchaseOrder newPO;
        Item item1;
        Item item2;
        UoM Pcs;
        ItemType type;
        Warehouse warehouse;
        IItemService itemService;
        IContactService contactService;
        IPurchaseOrderService poService;
        IPurchaseOrderDetailService poDetailService;
        IStockMutationService stockMutationService;
        IUoMService _uomService;
        IBlanketService _blanketService;
        IItemTypeService _itemTypeService;
        IWarehouseItemService _warehouseItemService;
        IWarehouseService _warehouseService;

        IPriceMutationService _priceMutationService;
        IContactGroupService _contactGroupService;
        IStockAdjustmentService _stockAdjustmentService;
        IStockAdjustmentDetailService _stockAdjustmentDetailService;
        IStockMutationService _stockMutationService;
        IItemService _itemService;
        IAccountService _accountService;
        IGeneralLedgerJournalService _generalLedgerJournalService;

        private Account Asset, CurrentAsset, CashBank, AccountReceivable, GBCHReceivable, Inventory, Raw, FinishedGoods, PrepaidExpense, NonCurrentAsset;
        private Account Expense, COGS, COS, OperationalExpense, ManufacturingExpense, RecoveryExpense, ConversionExpense;
        private Account SellingGeneralAndAdministrationExpense, CashBankAdjustmentExpense, Discount, SalesAllowance, StockAdjustmentExpense, SampleAndTrialExpense;
        private Account NonOperationalExpense, DepreciationExpense, Amortization, InterestExpense, TaxExpense, DividentExpense;
        private Account Liability, CurrentLiability, AccountPayable, GBCHPayable, GoodsPendingClearance, PurchaseAllowance, UnearnedRevenue, AccountPayableNonTrading, NonCurrentLiability;
        private Account Equity, OwnersEquity, EquityAdjustment;
        private Account Revenue;

        int Quantity1;
        int Quantity2;

        void before_each()
        {
            var db = new OffsetPrintingSuppliesEntities();
            using (db)
            {
                db.DeleteAllTables();
                itemService = new ItemService(new ItemRepository(), new ItemValidator());
                contactService = new ContactService(new ContactRepository(), new ContactValidator());
                poService = new PurchaseOrderService(new PurchaseOrderRepository(), new PurchaseOrderValidator());
                poDetailService = new PurchaseOrderDetailService(new PurchaseOrderDetailRepository(), new PurchaseOrderDetailValidator());
                stockMutationService = new StockMutationService(new StockMutationRepository(), new StockMutationValidator());
                _itemTypeService = new ItemTypeService(new ItemTypeRepository(), new ItemTypeValidator());
                _uomService = new UoMService(new UoMRepository(), new UoMValidator());
                _warehouseItemService = new WarehouseItemService(new WarehouseItemRepository(), new WarehouseItemValidator());
                _warehouseService = new WarehouseService(new WarehouseRepository(), new WarehouseValidator());
                _blanketService = new BlanketService(new BlanketRepository(), new BlanketValidator());

                _priceMutationService = new PriceMutationService(new PriceMutationRepository(), new PriceMutationValidator());
                _contactGroupService = new ContactGroupService(new ContactGroupRepository(), new ContactGroupValidator());
                _stockAdjustmentDetailService = new StockAdjustmentDetailService(new StockAdjustmentDetailRepository(), new StockAdjustmentDetailValidator());
                _stockAdjustmentService = new StockAdjustmentService(new StockAdjustmentRepository(), new StockAdjustmentValidator());
                _stockMutationService = new StockMutationService(new StockMutationRepository(), new StockMutationValidator());
                _itemService = new ItemService(new ItemRepository(), new ItemValidator());

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
                    SampleAndTrialExpense = _accountService.CreateLegacyObject(new Account() { Level = 4, IsLeaf = true, Name = "Sample And Trial Expense", Code = Constant.AccountCode.SampleAndTrialExpense, LegacyCode = Constant.AccountLegacyCode.SampleAndTrialExpense, Group = Constant.AccountGroup.Expense, ParentId = SellingGeneralAndAdministrationExpense.Id, IsLegacy = true }, _accountService);
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
                    AccountPayableNonTrading = _accountService.CreateLegacyObject(new Account() { Level = 3, IsLeaf = true, Name = "Account Payable (Non Trading)", Code = Constant.AccountCode.AccountPayableNonTrading, LegacyCode = Constant.AccountLegacyCode.AccountPayableNonTrading, Group = Constant.AccountGroup.Liability, ParentId = CurrentLiability.Id, IsLegacy = true }, _accountService);
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
                contact = contactService.CreateObject(contact, _contactGroupService);

                type = _itemTypeService.CreateObject("Item", "Item");

                warehouse = new Warehouse()
                {
                    Name = "Sentral Solusi Data",
                    Description = "Kali Besar Jakarta",
                    Code = "LCL"
                };
                warehouse = _warehouseService.CreateObject(warehouse, _warehouseItemService, itemService);

                item1 = new Item()
                {
                    ItemTypeId = _itemTypeService.GetObjectByName("Item").Id,
                    Name = "Batik Tulis",
                    Sku = "bt123",
                    UoMId = Pcs.Id
                };
                itemService.CreateObject(item1, _uomService, _itemTypeService, _warehouseItemService, _warehouseService, _priceMutationService, _contactGroupService);

                item2 = new Item()
                {
                    ItemTypeId = _itemTypeService.GetObjectByName("Item").Id,
                    Name = "Buku Gambar",
                    Sku = "bg123",
                    UoMId = Pcs.Id
                };
                itemService.CreateObject(item2, _uomService, _itemTypeService, _warehouseItemService, _warehouseService, _priceMutationService, _contactGroupService);

                StockAdjustment sa = new StockAdjustment() { AdjustmentDate = DateTime.Today, WarehouseId = warehouse.Id, Description = "item adjustment" };
                _stockAdjustmentService.CreateObject(sa, _warehouseService);
                StockAdjustmentDetail sadItem1 = new StockAdjustmentDetail()
                {
                    ItemId = item1.Id,
                    Quantity = 1000,
                    StockAdjustmentId = sa.Id,
                    Price = 5000
                };
                _stockAdjustmentDetailService.CreateObject(sadItem1, _stockAdjustmentService, _itemService, _warehouseItemService);

                StockAdjustmentDetail sadItem2 = new StockAdjustmentDetail()
                {
                    ItemId = item2.Id,
                    Quantity = 1000,
                    StockAdjustmentId = sa.Id,
                    Price = 5000
                };
                _stockAdjustmentDetailService.CreateObject(sadItem2, _stockAdjustmentService, _itemService, _warehouseItemService);

                _stockAdjustmentService.ConfirmObject(sa, DateTime.Today, _stockAdjustmentDetailService, _stockMutationService,
                                                      _itemService, _blanketService, _warehouseItemService, _accountService , _generalLedgerJournalService);

            }
        }

        void po_validation()
        {
            it["prepares_valid_data"] = () =>
                {
                    item1.Errors.Count().should_be(0);
                    item2.Errors.Count().should_be(0);
                    contact.Errors.Count().should_be(0);
                };

            it["creates_po"] = () =>
                {
                    newPO = new PurchaseOrder
                    {
                        ContactId = contact.Id,
                        PurchaseDate = DateTime.Now
                    };
                    newPO = poService.CreateObject(newPO, contactService);
                    newPO.Errors.Count().should_be(0);
                    poService.GetObjectById(newPO.Id).should_not_be_null();
                };

            context["when creating PO Detail"] = () =>
                {
                    before = () =>
                    {
                        newPO = new PurchaseOrder
                        {
                            ContactId = contact.Id,
                            PurchaseDate = DateTime.Now
                        };
                        newPO = poService.CreateObject(newPO, contactService);
                    };

                    it["creates po detail"] = () =>
                    {
                        PurchaseOrderDetail poDetail = new PurchaseOrderDetail
                        {
                            PurchaseOrderId = newPO.Id,
                            ItemId = item1.Id,
                            Quantity = 5,
                            Price = 30000
                        };
                        poDetail = poDetailService.CreateObject(poDetail, poService, itemService);
                        poDetail.Errors.Count().should_be(0);
                    };

                    it["must not allow PO confirmation if there is no PO Detail"] = () =>
                    {
                        newPO = poService.ConfirmObject(newPO, DateTime.Today, poDetailService, stockMutationService, itemService, _blanketService, _warehouseItemService);
                        newPO.Errors.Count().should_not_be(0);
                    };

                    it["should not create po detail if there is no item id"] = () =>
                    {
                        PurchaseOrderDetail poDetail = new PurchaseOrderDetail
                        {
                            PurchaseOrderId = newPO.Id,
                            ItemId = 0,
                            Quantity = 5,
                            Price = 25000
                        };
                        poDetail = poDetailService.CreateObject(poDetail, poService, itemService);
                        poDetail.Errors.Count().should_not_be(0);
                    };

                    context["when creating PO Detail"] = () =>
                    {
                        before = () =>
                        {
                            poDetail1 = new PurchaseOrderDetail
                            {
                                PurchaseOrderId = newPO.Id,
                                ItemId = item1.Id,
                                Quantity = 5,
                                Price = 30000
                            };
                            poDetail1 = poDetailService.CreateObject(poDetail1, poService, itemService);
                        };

                        it["should be valid when creating PO Detail 1"] = () =>
                        {
                            poDetail1.Errors.Count.should_be(0);
                        };

                        it["should not be valid when creating PO Detail 2 with the same item"] = () =>
                        {
                            poDetail2 = new PurchaseOrderDetail
                            {
                                PurchaseOrderId = newPO.Id,
                                ItemId = item1.Id,
                                Quantity = 5,
                                Price = 2500000
                            };
                            poDetail2 = poDetailService.CreateObject(poDetail2, poService, itemService);
                            poDetail2.Errors.Count().should_not_be(0);
                        };

                        context["should not be valid when creating PO Detail 2 with the same item"] = () =>
                        {
                            before = () =>
                            {
                                poDetail2 = new PurchaseOrderDetail
                                {
                                    PurchaseOrderId = newPO.Id,
                                    ItemId = item2.Id,
                                    Quantity = 5,
                                    Price = 2500000
                                };
                                poDetail2 = poDetailService.CreateObject(poDetail2, poService, itemService);
                            };

                            it["should be valid when creating a new PO Detail 2"] = () =>
                            {
                                poDetail2.Errors.Count().should_be(0);
                            };

                            it["should not be valid when updating PO Detail 2 item to the same item as POD1"] = () =>
                            {
                                poDetail2.ItemId = item1.Id;
                                poDetailService.UpdateObject(poDetail2, poService, itemService);
                                poDetail2.Errors.Count().should_not_be(0);
                            };
                        };
                    };
                };

            context["when confirming PO"] = () =>
            {
                before = () =>
                {
                    newPO = new PurchaseOrder
                    {
                        ContactId = contact.Id,
                        PurchaseDate = DateTime.Now
                    };
                    newPO = poService.CreateObject(newPO, contactService);

                    poDetail1 = new PurchaseOrderDetail
                    {
                        PurchaseOrderId = newPO.Id,
                        ItemId = item1.Id,
                        Quantity = 5,
                        Price = 30000
                    };
                    poDetail1 = poDetailService.CreateObject(poDetail1, poService, itemService);

                    poDetail2 = new PurchaseOrderDetail
                    {
                        PurchaseOrderId = newPO.Id,
                        ItemId = item2.Id,
                        Quantity = 5,
                        Price = 25000
                    };
                    poDetail2 = poDetailService.CreateObject(poDetail2, poService, itemService);
                };

                it["allows confirmation"] = () =>
                {
                    newPO = poService.ConfirmObject(newPO, DateTime.Today, poDetailService, stockMutationService, itemService, _blanketService, _warehouseItemService);
                    newPO.IsConfirmed.should_be(true);
                };

                context["post PO confirm"] = () =>
                {
                    before = () =>
                    {
                        item1 = itemService.GetObjectById(item1.Id);
                        item2 = itemService.GetObjectById(item2.Id);
                        Quantity1 = item1.PendingReceival;
                        Quantity2 = item2.PendingReceival;
                        newPO = poService.ConfirmObject(newPO, DateTime.Today, poDetailService, stockMutationService, itemService, _blanketService, _warehouseItemService);
                    };

                    it["should increase pending receival in item"] = () =>
                    {
                        int diff_1 = item1.PendingReceival - Quantity1;
                        diff_1.should_be(poDetail1.Quantity);

                        int diff_2 = item2.PendingReceival - Quantity2;
                        diff_2.should_be(poDetail2.Quantity);
                    };

                    it["should create StockMutation"] = () =>
                    {
                        stockMutationService.GetObjectsBySourceDocumentDetailForItem(item1.Id, Core.Constants.Constant.SourceDocumentDetailType.PurchaseOrderDetail, poDetail1.Id).Count().should_be(1);
                        stockMutationService.GetObjectsBySourceDocumentDetailForItem(item2.Id, Core.Constants.Constant.SourceDocumentDetailType.PurchaseOrderDetail, poDetail2.Id).Count().should_be(1);
                    };
                };
            };
        }
    }
}