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
    public class PurchaseBuilder
    {
        public IAccountService _accountService;
        public IBlanketService _blanketService;
        public IBlanketOrderService _blanketOrderService;
        public IBlanketOrderDetailService _blanketOrderDetailService;
        public ICashBankService _cashBankService;
        public ICashBankAdjustmentService _cashBankAdjustmentService;
        public ICashBankMutationService _cashBankMutationService;
        public ICashMutationService _cashMutationService;
        public IClosingService _closingService;
        public ICurrencyService _currencyService;
        public ICustomerItemService _customerItemService;
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

        public ItemType typeAccessory, typeBar, typeBlanket, typeBearing, typeRollBlanket, typeCore, typeCompound, typeChemical,
                        typeConsumable, typeGlue, typeUnderpacking, typeRoller;
        public RollerType typeDamp, typeFoundDT, typeInkFormX, typeInkDistD, typeInkDistM, typeInkDistE,
                        typeInkDuctB, typeInkDistH, typeInkFormW, typeInkDistHQ, typeDampFormDQ, typeInkFormY;
        public UoM Pcs, Boxes, Tubs;

        public Warehouse localWarehouse;
        public Contact contact;
        public Item rollBlanket1, rollBlanket2, rollBlanket3;
        public StockAdjustment stockAdjustment;
        public StockAdjustmentDetail stockAD1, stockAD2;
        public CashBank cashBank, pettyCash;
        public CashBankAdjustment cashBankAdjustment;
        public PurchaseOrder po1, po2;
        public PurchaseOrderDetail po1a, po1b, po1c, po2a, po2b;
        public PurchaseReceival pr1, pr2, pr3;
        public PurchaseReceivalDetail pr1a, pr1b, pr2a, pr2b, pr1a2, pr1c;
        public PurchaseInvoice pi1, pi2, pi3;
        public PurchaseInvoiceDetail pi1a, pi1b, pi2a, pi2b, pi1a2, pi1c;
        public PaymentVoucher pv;
        public PaymentVoucherDetail pvd1, pvd2, pvd3;
        // currency
        public Currency currencyEUR, currencyUSD, currencyIDR;
        public ExchangeRate DayMinusTwoRateEUR, DayMinusOneRateEUR, DayRateEUR, DayMinusTwoRateUSD, DayMinusOneRateUSD, DayRateUSD;

        private Account Asset, CurrentAsset, CashBank, AccountReceivable, GBCHReceivable, Inventory, Raw, FinishedGoods,
                        PrepaidExpense, PiutangLainLain, NonCurrentAsset, UnrecognizedCapitalGain;
        private Account Expense, COGS, COS, OperationalExpense, SampleAndTrialExpense, ManufacturingExpense, RecoveryExpense, ConversionExpense,
                        ExchangeLoss;
        private Account SellingGeneralAndAdministrationExpense, CashBankAdjustmentExpense, Discount, SalesAllowance, StockAdjustmentExpense;
        private Account NonOperationalExpense, DepreciationExpense, Amortization, InterestExpense, TaxExpense, DividentExpense;
        private Account Liability, CurrentLiability, AccountPayable, GBCHPayable, GoodsPendingClearance, PurchaseAllowance, UnearnedRevenue,
                        HutangLainLain, NonCurrentLiability, TaxPayable;
        private Account Equity, OwnersEquity, EquityAdjustment, CapitalGain;
        private Account Revenue;

        public PurchaseBuilder()
        {
            _accountService = new AccountService(new AccountRepository(), new AccountValidator());
            _blanketService = new BlanketService(new BlanketRepository(), new BlanketValidator());
            _blanketOrderService = new BlanketOrderService(new BlanketOrderRepository(), new BlanketOrderValidator());
            _blanketOrderDetailService = new BlanketOrderDetailService(new BlanketOrderDetailRepository(), new BlanketOrderDetailValidator());
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

            _priceMutationService = new PriceMutationService(new PriceMutationRepository(), new PriceMutationValidator());
            _currencyService = new CurrencyService(new CurrencyRepository(), new CurrencyValidator());
            typeAccessory = _itemTypeService.CreateObject("Accessory", "Accessory");
            typeBar = _itemTypeService.CreateObject("Bar", "Bar");
            typeBlanket = _itemTypeService.CreateObject("Blanket", "Blanket", true);
            typeBearing = _itemTypeService.CreateObject("Bearing", "Bearing");
            typeRollBlanket = _itemTypeService.CreateObject("RollBlanket", "RollBlanket");
            typeChemical = _itemTypeService.CreateObject("Chemical", "Chemical");
            typeCompound = _itemTypeService.CreateObject("Compound", "Compound");
            typeConsumable = _itemTypeService.CreateObject("Consumable", "Consumable");
            typeCore = _itemTypeService.CreateObject("Core", "Core", true);
            typeGlue = _itemTypeService.CreateObject("Glue", "Glue");
            typeUnderpacking = _itemTypeService.CreateObject("Underpacking", "Underpacking");
            typeRoller = _itemTypeService.CreateObject("Roller", "Roller", true);

            typeDamp = _rollerTypeService.CreateObject("Damp", "Damp");
            typeFoundDT = _rollerTypeService.CreateObject("Found DT", "Found DT");
            typeInkFormX = _rollerTypeService.CreateObject("Ink Form X", "Ink Form X");
            typeInkDistD = _rollerTypeService.CreateObject("Ink Dist D", "Ink Dist D");
            typeInkDistM = _rollerTypeService.CreateObject("Ink Dist M", "Ink Dist M");
            typeInkDistE = _rollerTypeService.CreateObject("Ink Dist E", "Ink Dist E");
            typeInkDuctB = _rollerTypeService.CreateObject("Ink Duct B", "Ink Duct B");
            typeInkDistH = _rollerTypeService.CreateObject("Ink Dist H", "Ink Dist H");
            typeInkFormW = _rollerTypeService.CreateObject("Ink Form W", "Ink Form W");
            typeInkDistHQ = _rollerTypeService.CreateObject("Ink Dist HQ", "Ink Dist HQ");
            typeDampFormDQ = _rollerTypeService.CreateObject("Damp Form DQ", "Damp Form DQ");
            typeInkFormY = _rollerTypeService.CreateObject("Ink Form Y", "Ink Form Y");

            if (!_accountService.GetLegacyObjects().Any())
            {
                Asset = _accountService.CreateLegacyObject(new Account() { Level = 1, Name = "Asset", Code = Constant.AccountCode.Asset, LegacyCode = Constant.AccountLegacyCode.Asset, Group = Constant.AccountGroup.Asset, IsLegacy = true }, _accountService);
                CurrentAsset = _accountService.CreateLegacyObject(new Account() { Level = 2, Name = "Current Asset", Code = Constant.AccountCode.CurrentAsset, LegacyCode = Constant.AccountLegacyCode.CurrentAsset, Group = Constant.AccountGroup.Asset, ParentId = Asset.Id, IsLegacy = true }, _accountService);
                CashBank = _accountService.CreateLegacyObject(new Account() { Level = 3, Name = "Cash & Bank", Code = Constant.AccountCode.CashBank, LegacyCode = Constant.AccountLegacyCode.CashBank, Group = Constant.AccountGroup.Asset, ParentId = CurrentAsset.Id, IsLegacy = true }, _accountService);
                AccountReceivable = _accountService.CreateLegacyObject(new Account() { Level = 3, Name = "Account Receivable", Code = Constant.AccountCode.AccountReceivable, LegacyCode = Constant.AccountLegacyCode.AccountReceivable, Group = Constant.AccountGroup.Asset, ParentId = CurrentAsset.Id, IsLegacy = true }, _accountService);
                GBCHReceivable = _accountService.CreateLegacyObject(new Account() { Level = 3, Name = "GBCH Receivable", Code = Constant.AccountCode.GBCHReceivable, LegacyCode = Constant.AccountLegacyCode.GBCHReceivable, Group = Constant.AccountGroup.Asset, ParentId = CurrentAsset.Id, IsLegacy = true }, _accountService);
                Inventory = _accountService.CreateLegacyObject(new Account() { Level = 3, Name = "Inventory", Code = Constant.AccountCode.Inventory, LegacyCode = Constant.AccountLegacyCode.Inventory, Group = Constant.AccountGroup.Asset, ParentId = CurrentAsset.Id, IsLegacy = true }, _accountService);
                Raw = _accountService.CreateLegacyObject(new Account() { Level = 4, IsLeaf = true, Name = "Raw Material", Code = Constant.AccountCode.Raw, LegacyCode = Constant.AccountLegacyCode.Raw, Group = Constant.AccountGroup.Asset, ParentId = Inventory.Id, IsLegacy = true }, _accountService);
                FinishedGoods = _accountService.CreateLegacyObject(new Account() { Level = 4, IsLeaf = true, Name = "Finished Goods", Code = Constant.AccountCode.FinishedGoods, LegacyCode = Constant.AccountLegacyCode.FinishedGoods, Group = Constant.AccountGroup.Asset, ParentId = Inventory.Id, IsLegacy = true }, _accountService);
                PrepaidExpense = _accountService.CreateLegacyObject(new Account() { Level = 3, IsLeaf = true, Name = "Prepaid Expense (Asset)", Code = Constant.AccountCode.PrepaidExpense, LegacyCode = Constant.AccountLegacyCode.PrepaidExpense, Group = Constant.AccountGroup.Asset, ParentId = CurrentAsset.Id, IsLegacy = true }, _accountService);
                PiutangLainLain = _accountService.CreateLegacyObject(new Account() { Level = 3, IsLeaf = true, Name = "Piutang Lain Lain", Code = Constant.AccountCode.PiutangLainLain, LegacyCode = Constant.AccountLegacyCode.PiutangLainLain, Group = Constant.AccountGroup.Asset, ParentId = CurrentAsset.Id, IsLegacy = true }, _accountService);
                NonCurrentAsset = _accountService.CreateLegacyObject(new Account() { Level = 2, Name = "Noncurrent Asset", Code = Constant.AccountCode.NonCurrentAsset, LegacyCode = Constant.AccountLegacyCode.NonCurrentAsset, Group = Constant.AccountGroup.Asset, ParentId = Asset.Id, IsLegacy = true }, _accountService);
                UnrecognizedCapitalGain = _accountService.CreateObject(new Account() { Level = 2, IsLeaf = true, Name = "UnrecognizedCapitalGain", Code = Constant.AccountCode.UnrecognizedCapitalGain, LegacyCode = Constant.AccountLegacyCode.UnrecognizedCapitalGain, Group = Constant.AccountGroup.Asset, ParentId = Asset.Id, IsLegacy = true }, _accountService);

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
                ExchangeLoss = _accountService.CreateLegacyObject(new Account() { Level = 2, IsLeaf = true, Name = "Exchange Loss", Code = Constant.AccountCode.ExchangeLoss, LegacyCode = Constant.AccountLegacyCode.ExchangeLoss, Group = Constant.AccountGroup.Expense, ParentId = Expense.Id, IsLegacy = true }, _accountService);

                Liability = _accountService.CreateLegacyObject(new Account() { Level = 1, Name = "Liability", Code = Constant.AccountCode.Liability, LegacyCode = Constant.AccountLegacyCode.Liability, Group = Constant.AccountGroup.Liability, IsLegacy = true }, _accountService);
                CurrentLiability = _accountService.CreateLegacyObject(new Account() { Level = 2, Name = "Current Liability", Code = Constant.AccountCode.CurrentLiability, LegacyCode = Constant.AccountLegacyCode.CurrentLiability, Group = Constant.AccountGroup.Liability, ParentId = Liability.Id, IsLegacy = true }, _accountService);
                AccountPayable = _accountService.CreateLegacyObject(new Account() { Level = 3, Name = "Account Payable", Code = Constant.AccountCode.AccountPayable, LegacyCode = Constant.AccountLegacyCode.AccountPayable, Group = Constant.AccountGroup.Liability, ParentId = CurrentLiability.Id, IsLegacy = true }, _accountService);
                GBCHPayable = _accountService.CreateLegacyObject(new Account() { Level = 3, Name = "GBCH Payable", Code = Constant.AccountCode.GBCHPayable, LegacyCode = Constant.AccountLegacyCode.GBCHPayable, Group = Constant.AccountGroup.Liability, ParentId = CurrentLiability.Id, IsLegacy = true }, _accountService);
                GoodsPendingClearance = _accountService.CreateLegacyObject(new Account() { Level = 3, IsLeaf = true, Name = "Goods Pending Clearance", Code = Constant.AccountCode.GoodsPendingClearance, LegacyCode = Constant.AccountLegacyCode.GoodsPendingClearance, Group = Constant.AccountGroup.Liability, ParentId = CurrentLiability.Id, IsLegacy = true }, _accountService);
                UnearnedRevenue = _accountService.CreateLegacyObject(new Account() { Level = 3, IsLeaf = true, Name = "Unearned Revenue", Code = Constant.AccountCode.UnearnedRevenue, LegacyCode = Constant.AccountLegacyCode.UnearnedRevenue, Group = Constant.AccountGroup.Liability, ParentId = CurrentLiability.Id, IsLegacy = true }, _accountService);
                PurchaseAllowance = _accountService.CreateLegacyObject(new Account() { Level = 3, IsLeaf = true, Name = "Purchase Allowance", Code = Constant.AccountCode.PurchaseAllowance, LegacyCode = Constant.AccountLegacyCode.PurchaseAllowance, Group = Constant.AccountGroup.Liability, ParentId = CurrentLiability.Id, IsLegacy = true }, _accountService);
                HutangLainLain = _accountService.CreateLegacyObject(new Account() { Level = 3, IsLeaf = true, Name = "Hutang Lain Lain", Code = Constant.AccountCode.HutangLainLain, LegacyCode = Constant.AccountLegacyCode.HutangLainLain, Group = Constant.AccountGroup.Liability, ParentId = CurrentLiability.Id, IsLegacy = true }, _accountService);
                TaxPayable = _accountService.CreateLegacyObject(new Account() { Level = 3, IsLeaf = true, Name = "Tax Payable", Code = Constant.AccountCode.TaxPayable, LegacyCode = Constant.AccountLegacyCode.TaxPayable, Group = Constant.AccountGroup.Liability, ParentId = CurrentLiability.Id, IsLegacy = true }, _accountService);
                NonCurrentLiability = _accountService.CreateLegacyObject(new Account() { Level = 2, Name = "Noncurrent Liability", Code = Constant.AccountCode.NonCurrentLiability, LegacyCode = Constant.AccountLegacyCode.NonCurrentLiability, Group = Constant.AccountGroup.Liability, ParentId = Liability.Id, IsLegacy = true }, _accountService);

                Equity = _accountService.CreateLegacyObject(new Account() { Level = 1, Name = "Equity", Code = Constant.AccountCode.Equity, LegacyCode = Constant.AccountLegacyCode.Equity, Group = Constant.AccountGroup.Equity, IsLegacy = true }, _accountService);
                OwnersEquity = _accountService.CreateLegacyObject(new Account() { Level = 2, Name = "Owners Equity", Code = Constant.AccountCode.OwnersEquity, LegacyCode = Constant.AccountLegacyCode.OwnersEquity, Group = Constant.AccountGroup.Equity, ParentId = Equity.Id, IsLegacy = true }, _accountService);
                EquityAdjustment = _accountService.CreateLegacyObject(new Account() { Level = 3, IsLeaf = true, Name = "Equity Adjustment", Code = Constant.AccountCode.EquityAdjustment, LegacyCode = Constant.AccountLegacyCode.EquityAdjustment, Group = Constant.AccountGroup.Equity, ParentId = OwnersEquity.Id, IsLegacy = true }, _accountService);
                CapitalGain = _accountService.CreateLegacyObject(new Account() { Level = 2, Name = "CapitalGain", Code = Constant.AccountCode.ExchangeGain, LegacyCode = Constant.AccountLegacyCode.ExchangeGain, Group = Constant.AccountGroup.Equity, ParentId = Equity.Id, IsLegacy = true, IsLeaf = true }, _accountService);

                Revenue = _accountService.CreateLegacyObject(new Account() { Level = 1, IsLeaf = true, Name = "Revenue", Code = Constant.AccountCode.Revenue, LegacyCode = Constant.AccountLegacyCode.Revenue, Group = Constant.AccountGroup.Revenue, IsLegacy = true }, _accountService);
            }

            if (!_currencyService.GetAll().Any())
            {
                currencyIDR = new Currency();
                currencyIDR.IsBase = true;
                currencyIDR.Name = "IDR";
                currencyIDR = _currencyService.CreateObject(currencyIDR, _accountService);
            }
        }

        public void PopulateData()
        {
            PopulateMasterData();
            PopulateOrderAndReceivalData();
            PopulateInvoiceData();
            PopulateVoucher();
        }

        public void PopulateMasterData()
        {
            localWarehouse = new Warehouse()
            {
                Name = "Sentral Solusi Data",
                Description = "Kali Besar Jakarta",
                Code = "LCL"
            };
            localWarehouse = _warehouseService.CreateObject(localWarehouse, _warehouseItemService, _itemService);

            Pcs = new UoM()
            {
                Name = "Pcs"
            };
            _uomService.CreateObject(Pcs);

            Boxes = new UoM()
            {
                Name = "Boxes"
            };
            _uomService.CreateObject(Boxes);

            Tubs = new UoM()
            {
                Name = "Tubs"
            };
            _uomService.CreateObject(Tubs);

            rollBlanket1 = new Item()
            {
                ItemTypeId = _itemTypeService.GetObjectByName("RollBlanket").Id,
                Name = "RollBlanket1",
                Sku = "BLK1",
                UoMId = Pcs.Id
            };

            rollBlanket1 = _itemService.CreateObject(rollBlanket1, _uomService, _itemTypeService, _warehouseItemService, _warehouseService, _priceMutationService);

            rollBlanket2 = new Item()
            {
                ItemTypeId = _itemTypeService.GetObjectByName("RollBlanket").Id,
                Name = "RollBlanket2",
                Sku = "BLK2",
                UoMId = Pcs.Id
            };

            rollBlanket2 = _itemService.CreateObject(rollBlanket2, _uomService, _itemTypeService, _warehouseItemService, _warehouseService, _priceMutationService);

            rollBlanket3 = new Item()
            {
                ItemTypeId = _itemTypeService.GetObjectByName("RollBlanket").Id,
                Name = "RollBlanket3",
                Sku = "BLK3",
                UoMId = Pcs.Id
            };

            rollBlanket3 = _itemService.CreateObject(rollBlanket3, _uomService, _itemTypeService, _warehouseItemService, _warehouseService, _priceMutationService);

            StockAdjustment sa = new StockAdjustment() { WarehouseId = localWarehouse.Id, AdjustmentDate = DateTime.Today, Description = "Bar Related Adjustment" };
            _stockAdjustmentService.CreateObject(sa, _warehouseService);
            StockAdjustmentDetail sadRollBlanket1 = new StockAdjustmentDetail() { ItemId = rollBlanket1.Id, Quantity = 100000, StockAdjustmentId = sa.Id, Price = 5000 };
            _stockAdjustmentDetailService.CreateObject(sadRollBlanket1, _stockAdjustmentService, _itemService, _warehouseItemService);
            StockAdjustmentDetail sadRollBlanket2 = new StockAdjustmentDetail() { ItemId = rollBlanket2.Id, Quantity = 100000, StockAdjustmentId = sa.Id, Price = 5000 };
            _stockAdjustmentDetailService.CreateObject(sadRollBlanket2, _stockAdjustmentService, _itemService, _warehouseItemService);
            StockAdjustmentDetail sadRollBlanket3 = new StockAdjustmentDetail() { ItemId = rollBlanket3.Id, Quantity = 100000, StockAdjustmentId = sa.Id, Price = 5000 };
            _stockAdjustmentDetailService.CreateObject(sadRollBlanket3, _stockAdjustmentService, _itemService, _warehouseItemService);

            _stockAdjustmentService.ConfirmObject(sa, DateTime.Today, _stockAdjustmentDetailService, _stockMutationService, _itemService, _blanketService, _warehouseItemService,
                                                  _accountService, _generalLedgerJournalService);

            contact = new Contact()
            {
                Name = "President of Indonesia",
                Address = "Istana Negara Jl. Veteran No. 16 Jakarta Pusat",
                ContactNo = "021 3863777",
                PIC = "Mr. President",
                PICContactNo = "021 3863777",
                Email = "random@ri.gov.au",
                TaxCode = "01"
            };
            contact = _contactService.CreateObject(contact);

            currencyEUR = new Currency()
            {
                Name = "EURO",
                IsBase = false,
                IsDeleted = false,
                CreatedAt = DateTime.Now,
            };
            currencyEUR = _currencyService.CreateObject(currencyEUR, _accountService);

            currencyUSD = new Currency()
            {
                Name = "USD",
                IsBase = false,
                IsDeleted = false,
                CreatedAt = DateTime.Now
            };
            currencyUSD = _currencyService.CreateObject(currencyUSD, _accountService);

            DayMinusTwoRateEUR = new ExchangeRate()
            {
                CurrencyId = currencyEUR.Id,
                ExRateDate = DateTime.Today.AddDays(-2),
                Rate = 15100
            };
            DayMinusTwoRateEUR = _exchangeRateService.CreateObject(DayMinusTwoRateEUR);

            DayMinusOneRateEUR = new ExchangeRate()
            {
                CurrencyId = currencyEUR.Id,
                ExRateDate = DateTime.Today.AddDays(-2),
                Rate = 15050
            };
            DayMinusOneRateEUR = _exchangeRateService.CreateObject(DayMinusOneRateEUR);

            DayRateEUR = new ExchangeRate()
            {
                CurrencyId = currencyEUR.Id,
                ExRateDate = DateTime.Today.AddDays(-2),
                Rate = 15000
            };
            DayRateEUR = _exchangeRateService.CreateObject(DayRateEUR);

            DayMinusTwoRateUSD = new ExchangeRate()
            {
                CurrencyId = currencyUSD.Id,
                ExRateDate = DateTime.Today.AddDays(-2),
                Rate = 12100
            };
            DayMinusTwoRateUSD = _exchangeRateService.CreateObject(DayMinusTwoRateUSD);

            DayMinusOneRateUSD = new ExchangeRate()
            {
                CurrencyId = currencyUSD.Id,
                ExRateDate = DateTime.Today.AddDays(-2),
                Rate = 12150
            };
            DayMinusOneRateUSD = _exchangeRateService.CreateObject(DayMinusOneRateUSD);

            DayRateUSD = new ExchangeRate()
            {
                CurrencyId = currencyUSD.Id,
                ExRateDate = DateTime.Today.AddDays(-2),
                Rate = 12200
            };
            DayRateUSD = _exchangeRateService.CreateObject(DayRateUSD);

            cashBank = new CashBank()
            {
                Name = "Rekening BRI",
                Description = "Untuk cashflow",
                IsBank = true,
                CurrencyId = currencyIDR.Id
            };
            _cashBankService.CreateObject(cashBank, _accountService,_currencyService);

            cashBankAdjustment = new CashBankAdjustment()
            {
                CashBankId = cashBank.Id,
                Amount = 1000000000,
                AdjustmentDate = DateTime.Today
            };
            _cashBankAdjustmentService.CreateObject(cashBankAdjustment, _cashBankService);
            _cashBankAdjustmentService.ConfirmObject(cashBankAdjustment, DateTime.Now, _cashMutationService, _cashBankService,
                                                     _accountService, _generalLedgerJournalService, _closingService,_currencyService);
        }

        public void PopulateOrderAndReceivalData()
        {
            TimeSpan purchaseDate = new TimeSpan(10, 0, 0, 0);
            TimeSpan receivedDate = new TimeSpan(3, 0, 0 ,0);
            TimeSpan lateReceivedDate = new TimeSpan(2, 0, 0, 0);
            po1 = new PurchaseOrder()
            {
                PurchaseDate = DateTime.Today.Subtract(purchaseDate),
                ContactId = contact.Id,
                CurrencyId = currencyIDR.Id
            };
            _purchaseOrderService.CreateObject(po1, _contactService);

            po2 = new PurchaseOrder()
            {
                PurchaseDate = DateTime.Today.Subtract(purchaseDate),
                ContactId = contact.Id,
                CurrencyId = currencyIDR.Id
            };
            _purchaseOrderService.CreateObject(po2, _contactService);

            po1a = new PurchaseOrderDetail()
            {
                ItemId = rollBlanket1.Id,
                PurchaseOrderId = po1.Id,
                Quantity = 300,
                Price = 50000                
            };
            _purchaseOrderDetailService.CreateObject(po1a, _purchaseOrderService, _itemService);

            po1b = new PurchaseOrderDetail()
            {
                ItemId = rollBlanket2.Id,
                PurchaseOrderId = po1.Id,
                Quantity = 250,
                Price = 72000
            };
            _purchaseOrderDetailService.CreateObject(po1b, _purchaseOrderService, _itemService);

            po1c = new PurchaseOrderDetail()
            {
                ItemId = rollBlanket3.Id,
                PurchaseOrderId = po1.Id,
                Quantity = 100,
                Price = 100000
            };
            _purchaseOrderDetailService.CreateObject(po1c, _purchaseOrderService, _itemService);

            po2a = new PurchaseOrderDetail()
            {
                ItemId = rollBlanket1.Id,
                PurchaseOrderId = po2.Id,
                Quantity = 300,
                Price = 50000
            };
            _purchaseOrderDetailService.CreateObject(po2a, _purchaseOrderService, _itemService);

            po2b = new PurchaseOrderDetail()
            {
                ItemId = rollBlanket2.Id,
                PurchaseOrderId = po2.Id,
                Quantity = 250,
                Price = 72000
            };
            _purchaseOrderDetailService.CreateObject(po2b, _purchaseOrderService, _itemService);

            _purchaseOrderService.ConfirmObject(po1, po1.PurchaseDate, _purchaseOrderDetailService, _stockMutationService, _itemService, _blanketService, _warehouseItemService);
            _purchaseOrderService.ConfirmObject(po2, po2.PurchaseDate, _purchaseOrderDetailService, _stockMutationService, _itemService, _blanketService, _warehouseItemService);

            pr1 = new PurchaseReceival()
            {
                PurchaseOrderId = po1.Id,
                ReceivalDate = DateTime.Now.Subtract(receivedDate),
                WarehouseId = localWarehouse.Id
            };
            _purchaseReceivalService.CreateObject(pr1, _purchaseOrderService, _warehouseService);

            pr2 = new PurchaseReceival()
            {
                PurchaseOrderId = po2.Id,
                ReceivalDate = DateTime.Now.Subtract(receivedDate),
                WarehouseId = localWarehouse.Id
            };
            _purchaseReceivalService.CreateObject(pr2, _purchaseOrderService, _warehouseService);

            pr1a = new PurchaseReceivalDetail()
            {
                PurchaseOrderDetailId = po1a.Id,
                PurchaseReceivalId = pr1.Id,
                ItemId = po1a.ItemId,
                Quantity = po1a.Quantity - 100
            };
            _purchaseReceivalDetailService.CreateObject(pr1a, _purchaseReceivalService, _purchaseOrderDetailService, _purchaseOrderService, _itemService);

            pr1b = new PurchaseReceivalDetail()
            {
                PurchaseOrderDetailId = po1b.Id,
                PurchaseReceivalId = pr1.Id,
                ItemId = po1b.ItemId,
                Quantity = po1b.Quantity
            };
            _purchaseReceivalDetailService.CreateObject(pr1b, _purchaseReceivalService, _purchaseOrderDetailService, _purchaseOrderService, _itemService);

            pr2a = new PurchaseReceivalDetail()
            {
                PurchaseOrderDetailId = po2a.Id,
                PurchaseReceivalId = pr2.Id,
                ItemId = po2a.ItemId,
                Quantity = po2a.Quantity
            };
            _purchaseReceivalDetailService.CreateObject(pr2a, _purchaseReceivalService, _purchaseOrderDetailService, _purchaseOrderService, _itemService);

            pr2b = new PurchaseReceivalDetail()
            {
                PurchaseOrderDetailId = po2b.Id,
                PurchaseReceivalId = pr2.Id,
                ItemId = po2b.ItemId,
                Quantity = po2b.Quantity
            };
            _purchaseReceivalDetailService.CreateObject(pr2b, _purchaseReceivalService, _purchaseOrderDetailService, _purchaseOrderService, _itemService);

            pr3 = new PurchaseReceival()
            {
                PurchaseOrderId = po1.Id,
                ReceivalDate = DateTime.Now.Subtract(lateReceivedDate),
                WarehouseId = localWarehouse.Id
            };
            _purchaseReceivalService.CreateObject(pr3, _purchaseOrderService, _warehouseService);

            pr1c = new PurchaseReceivalDetail()
            {
                PurchaseReceivalId = pr3.Id,
                PurchaseOrderDetailId = po1c.Id,
                Quantity = po1c.Quantity,
                ItemId = po1c.ItemId
            };
            _purchaseReceivalDetailService.CreateObject(pr1c, _purchaseReceivalService, _purchaseOrderDetailService, _purchaseOrderService, _itemService);

            pr1a2 = new PurchaseReceivalDetail()
            {
                PurchaseReceivalId = pr3.Id,
                PurchaseOrderDetailId = po1a.Id,
                Quantity = 100,
                ItemId = po1a.ItemId
            };
            _purchaseReceivalDetailService.CreateObject(pr1a2, _purchaseReceivalService, _purchaseOrderDetailService, _purchaseOrderService, _itemService);

        }

        public void PopulateInvoiceData()
        {
            TimeSpan receivedDate = new TimeSpan(3, 0, 0, 0);
            TimeSpan lateReceivedDate = new TimeSpan(2, 0, 0, 0);
            _purchaseReceivalService.ConfirmObject(pr1, DateTime.Now.Subtract(receivedDate), _purchaseReceivalDetailService, _purchaseOrderService, _purchaseOrderDetailService, _stockMutationService,
                                       _itemService, _blanketService, _warehouseItemService, _accountService, _generalLedgerJournalService, _closingService, _currencyService, _exchangeRateService);
            _purchaseReceivalService.ConfirmObject(pr2, DateTime.Now.Subtract(receivedDate), _purchaseReceivalDetailService, _purchaseOrderService, _purchaseOrderDetailService, _stockMutationService,
                                                   _itemService, _blanketService, _warehouseItemService, _accountService, _generalLedgerJournalService, _closingService, _currencyService, _exchangeRateService);
            _purchaseReceivalService.ConfirmObject(pr3, DateTime.Now.Subtract(receivedDate), _purchaseReceivalDetailService, _purchaseOrderService, _purchaseOrderDetailService, _stockMutationService, 
                                                   _itemService, _blanketService, _warehouseItemService, _accountService, _generalLedgerJournalService, _closingService, _currencyService, _exchangeRateService);

            pi1 = new PurchaseInvoice()
            {
                InvoiceDate = DateTime.Today,
                Description = "Pembayaran PR1",
                PurchaseReceivalId = pr1.Id,
                Tax = 10,
                Discount = 0,
                DueDate = DateTime.Today.AddDays(14),
                CurrencyId = currencyIDR.Id
            };
            pi1 = _purchaseInvoiceService.CreateObject(pi1, _purchaseReceivalService);

            pi1a = new PurchaseInvoiceDetail()
            {
                PurchaseInvoiceId = pi1.Id,
                PurchaseReceivalDetailId = pr1a.Id,
                Quantity = pr1a.Quantity
            };
            pi1a = _purchaseInvoiceDetailService.CreateObject(pi1a, _purchaseInvoiceService, _purchaseOrderDetailService, _purchaseReceivalDetailService);

            pi1b = new PurchaseInvoiceDetail()
            {
                PurchaseInvoiceId = pi1.Id,
                PurchaseReceivalDetailId = pr1b.Id,
                Quantity = pr1b.Quantity
            };
            pi1b = _purchaseInvoiceDetailService.CreateObject(pi1b, _purchaseInvoiceService, _purchaseOrderDetailService, _purchaseReceivalDetailService);

            pi2 = new PurchaseInvoice()
            {
                InvoiceDate = DateTime.Today,
                Description = "Pembayaran PR2",
                PurchaseReceivalId = pr2.Id,
                Tax = 10,
                Discount = 5,
                DueDate = DateTime.Today.AddDays(14),
                CurrencyId = currencyIDR.Id
            };
            pi2 = _purchaseInvoiceService.CreateObject(pi2, _purchaseReceivalService);

            pi2a = new PurchaseInvoiceDetail()
            {
                PurchaseInvoiceId = pi2.Id,
                PurchaseReceivalDetailId = pr2a.Id,
                Quantity = pr2a.Quantity
            };
            pi2a = _purchaseInvoiceDetailService.CreateObject(pi2a, _purchaseInvoiceService, _purchaseOrderDetailService, _purchaseReceivalDetailService);

            pi2b = new PurchaseInvoiceDetail()
            {
                PurchaseInvoiceId = pi2.Id,
                PurchaseReceivalDetailId = pr2b.Id,
                Quantity = pr2b.Quantity
            };
            pi2b = _purchaseInvoiceDetailService.CreateObject(pi2b, _purchaseInvoiceService, _purchaseOrderDetailService, _purchaseReceivalDetailService);

            pi3 = new PurchaseInvoice()
            {
                InvoiceDate = DateTime.Today,
                Description = "Pembayaran PR3",
                PurchaseReceivalId = pr3.Id,
                Tax = 10,
                Discount = 0,
                DueDate = DateTime.Today.AddDays(14),
                CurrencyId = currencyIDR.Id
            };
            pi3 = _purchaseInvoiceService.CreateObject(pi3, _purchaseReceivalService);

            pi1a2 = new PurchaseInvoiceDetail()
            {
                PurchaseInvoiceId = pi3.Id,
                PurchaseReceivalDetailId = pr1a2.Id,
                Quantity = pr1a2.Quantity
            };
            pi1a2 = _purchaseInvoiceDetailService.CreateObject(pi1a2, _purchaseInvoiceService, _purchaseOrderDetailService, _purchaseReceivalDetailService);

            pi1c = new PurchaseInvoiceDetail()
            {
                PurchaseInvoiceId = pi3.Id,
                PurchaseReceivalDetailId = pr1c.Id,
                Quantity = pr1c.Quantity
            };
            pi1c = _purchaseInvoiceDetailService.CreateObject(pi1c, _purchaseInvoiceService, _purchaseOrderDetailService, _purchaseReceivalDetailService);

        }

        public void PopulateVoucher()
        {
            _purchaseInvoiceService.ConfirmObject(pi1, DateTime.Today, _purchaseInvoiceDetailService, _purchaseOrderService, _purchaseReceivalService,
                                                  _purchaseReceivalDetailService, _payableService, _accountService, _generalLedgerJournalService, _closingService, _currencyService, _exchangeRateService);
            _purchaseInvoiceService.ConfirmObject(pi2, DateTime.Today, _purchaseInvoiceDetailService, _purchaseOrderService, _purchaseReceivalService,
                                                  _purchaseReceivalDetailService, _payableService, _accountService, _generalLedgerJournalService, _closingService, _currencyService, _exchangeRateService);
            _purchaseInvoiceService.ConfirmObject(pi3, DateTime.Today, _purchaseInvoiceDetailService, _purchaseOrderService, _purchaseReceivalService,
                                                  _purchaseReceivalDetailService, _payableService, _accountService, _generalLedgerJournalService, _closingService, _currencyService, _exchangeRateService);

            pv = new PaymentVoucher()
            {
                ContactId = contact.Id,
                CashBankId = cashBank.Id,
                PaymentDate = DateTime.Today.AddDays(14),
                IsGBCH = true,
                DueDate = DateTime.Today.AddDays(14),
                RateToIDR = 1
            };
            _paymentVoucherService.CreateObject(pv, _paymentVoucherDetailService, _payableService, _contactService, _cashBankService, _currencyService);

            pvd1 = new PaymentVoucherDetail()
            {
                PaymentVoucherId = pv.Id,
                PayableId = _payableService.GetObjectBySource(Core.Constants.Constant.PayableSource.PurchaseInvoice, pi1.Id).Id,
                AmountPaid = pi1.AmountPayable,
                Description = "Payment buat Purchase Invoice 1",
                Rate = 1
            };
            _paymentVoucherDetailService.CreateObject(pvd1, _paymentVoucherService, _cashBankService, _payableService);

            pvd2 = new PaymentVoucherDetail()
            {
                PaymentVoucherId = pv.Id,
                PayableId = _payableService.GetObjectBySource(Core.Constants.Constant.PayableSource.PurchaseInvoice, pi2.Id).Id,
                AmountPaid = pi2.AmountPayable,
                Description = "Payment buat Purchase Invoice 2",
                Rate = 1,
            };
            _paymentVoucherDetailService.CreateObject(pvd2, _paymentVoucherService, _cashBankService, _payableService);

            pvd3 = new PaymentVoucherDetail()
            {
                PaymentVoucherId = pv.Id,
                PayableId = _payableService.GetObjectBySource(Core.Constants.Constant.PayableSource.PurchaseInvoice, pi3.Id).Id,
                AmountPaid = pi3.AmountPayable,
                Description = "Payment buat Purchase Invoice 3",
                Rate = 1
            };
            _paymentVoucherDetailService.CreateObject(pvd3, _paymentVoucherService, _cashBankService, _payableService);

            _paymentVoucherService.ConfirmObject(pv, DateTime.Today, _paymentVoucherDetailService, _cashBankService, _payableService, _cashMutationService,
                                                 _accountService, _generalLedgerJournalService, _closingService,_currencyService);

            _paymentVoucherService.ReconcileObject(pv, DateTime.Today.AddDays(10), _paymentVoucherDetailService, _cashMutationService, _cashBankService, _payableService,
                                                   _accountService, _generalLedgerJournalService, _closingService,_currencyService);
        }
    }
}
