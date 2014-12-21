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
    public class DataBuilder
    {
        public IAccountService _accountService;
        public IBlanketService _blanketService;
        public IBlanketOrderService _blanketOrderService;
        public IBlanketOrderDetailService _blanketOrderDetailService;
        public IBlendingRecipeService _blendingRecipeService;
        public IBlendingRecipeDetailService _blendingRecipeDetailService;
        public IBlendingWorkOrderService _blendingWorkOrderService;
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
        public CashBank cashBank, pettyCash, cashBank1, cashBank2;
        public CashBankAdjustment cashBankAdjustment, cashBankAdjustment2, cashBankAdjustment3;
        public CashBankMutation cashBankMutation;

        public UserAccount admin, user;
        public UserMenu menudata, menufinance;
        public UserAccess admindata, userdata, adminfinance, userfinance;

        public ItemType typeAdhesiveBlanket, typeAdhesiveRoller, typeAccessory, typeBar, typeBlanket, typeBearing, typeRollBlanket, typeCore, typeCompound, typeChemical,
                        typeConsumable, typeGlue, typeUnderpacking, typeRoller;
        public RollerType typeDamp, typeFoundDT, typeInkFormX, typeInkDistD, typeInkDistM, typeInkDistE,
                        typeInkDuctB, typeInkDistH, typeInkFormW, typeInkDistHQ, typeDampFormDQ, typeInkFormY;
        public UoM Pcs, Boxes, Tubs, Bottles;
        public Item item, itemAdhesiveBlanket, itemAdhesiveRoller, itemCompound, itemCompound1, itemCompound2, itemAccessory1, itemAccessory2;
        public Warehouse localWarehouse, movingWarehouse;
        public Contact contact;
        public Machine machine;
        public CoreBuilder coreBuilder, coreBuilder1, coreBuilder2, coreBuilder3, coreBuilder4;
        public CoreIdentification coreIdentification, coreIdentificationInHouse, coreIdentificationContact;
        public CoreIdentificationDetail coreIdentificationDetail, coreIDInHouse1, coreIDInHouse2, coreIDInHouse3,
                                        coreIDContact1, coreIDContact2, coreIDContact3;
        public RollerBuilder rollerBuilder, rollerBuilder1, rollerBuilder2, rollerBuilder3, rollerBuilder4;
        public RecoveryOrder recoveryOrder, recoveryOrderContact, recoveryOrderInHouse;
        public RecoveryOrderDetail recoveryODContact1, recoveryODContact2, recoveryODContact3,
                                   recoveryODInHouse1, recoveryODInHouse2, recoveryODInHouse3;
        public RecoveryOrder recoveryOrderContact2, recoveryOrderInHouse2;
        public RecoveryOrderDetail recoveryODContact2b, recoveryODInHouse3b;
        public RecoveryAccessoryDetail accessory1, accessory2, accessory3, accessory4;
        public Item bargeneric, barleft1, barleft2, barright1, barright2;
        public Item rollBlanket1, rollBlanket2, rollBlanket3;
        public Item itemBlending, itemBlendingDet1, itemBlendingDet2, itemBlendingDet3;
        public BlendingRecipe blending;
        public BlendingRecipeDetail blendingDet1, blendingDet2, blendingDet3;
        public Blanket blanket1, blanket2, blanket3;
        public BlanketOrder blanketOrderContact;
        public BlendingWorkOrder blendingWorkOrder;
        public BlanketOrderDetail blanketODContact1, blanketODContact2, blanketODContact3, blanketODContact4; 
        public WarehouseMutation warehouseMutation;
        public WarehouseMutationDetail wmoDetail1, wmoDetail2, wmoDetail3, wmoDetail4, wmoDetail5, wmoDetail6,
                                            wmoDetail7, wmoDetail8, wmoDetail9;
        public RollerWarehouseMutation rollerWarehouseMutationContact, rollerWarehouseMutationInHouse;
        public RollerWarehouseMutationDetail rwmDetailContact1, rwmDetailContact2, rwmDetailContact3,
                                             rwmDetailInHouse1, rwmDetailInHouse2, rwmDetailInHouse3;
        public StockAdjustment stockAdjustment, sa;
        public StockAdjustmentDetail stockAD, stockAD1, stockAD2, stockAD3, stockAD4;
        public StockAdjustmentDetail sad1, sad2, sad3, sad4, sad5, sadAdhesiveRoller, sadAdhesiveBlanket;
        public StockAdjustmentDetail sadBlendingItem1, sadBlendingItem2, sadBlendingItem3, sadBlendingItem4;

        public CustomerStockAdjustment customerStockAdjustment, csa;
        public CustomerStockAdjustmentDetail cstockAD, cstockAD1, cstockAD2, cstockAD3, cstockAD4;

        public SalesOrder salesOrder1, salesOrder2, salesOrder3;
        public SalesOrderDetail salesOD1a, salesOD1b, salesOD2a, salesOD2b, salesOD3a, salesOD3b;
        public DeliveryOrder deliveryOrder1, deliveryOrder2, deliveryOrder3;
        public DeliveryOrderDetail deliveryOD1a, deliveryOD1b, deliveryOD2a, deliveryOD2b, deliveryOD3a, deliveryOD3b;
        public SalesInvoice salesInvoice1, salesInvoice2, salesInvoice3;
        public SalesInvoiceDetail salesID1a, salesID1b, salesID2a, salesID2b, salesID3a, salesID3b;
        public ReceiptVoucher receiptVoucher1, receiptVoucher2, receiptVoucher3;
        public ReceiptVoucherDetail receiptVD1a, receiptVD1b, receiptVD2a, receiptVD2b, receiptVD3a, receiptVD3b;

        public PurchaseOrder purchaseOrder1;
        public PurchaseOrderDetail purchaseOD1a, purchaseOD1b;
        public PurchaseReceival purchaseReceival1;
        public PurchaseReceivalDetail purchaseRD1a, purchaseRD1b;
        public PurchaseInvoice purchaseInvoice1;
        public PurchaseInvoiceDetail purchaseID1a, purchaseID1b;
        public PaymentVoucher paymentVoucher1;
        public PaymentVoucherDetail paymentVD1a, paymentVD1b;

        // extended variable
        public decimal usedCoreBuilderQuantity, usedCoreBuilder1Quantity, usedCoreBuilder2Quantity, usedCoreBuilder3Quantity, usedCoreBuilder4Quantity;
        public decimal usedRollerBuilderQuantity, usedRollerBuilder1Quantity, usedRollerBuilder2Quantity, usedRollerBuilder3Quantity, usedRollerBuilder4Quantity;
        public decimal usedCoreBuilderFinal, usedCoreBuilder1Final, usedCoreBuilder2Final, usedCoreBuilder3Final, usedCoreBuilder4Final;
        public decimal usedRollerBuilderFinal, usedRollerBuilder1Final, usedRollerBuilder2Final, usedRollerBuilder3Final, usedRollerBuilder4Final;
        public decimal accessory1quantity;

        // purchase
        public PurchaseOrder po1, po2;
        public PurchaseOrderDetail po1a, po1b, po1c, po2a, po2b;
        public PurchaseReceival pr1, pr2, pr3;
        public PurchaseReceivalDetail pr1a, pr1b, pr2a, pr2b, pr1a2, pr1c;
        public PurchaseInvoice pi1, pi2, pi3;
        public PurchaseInvoiceDetail pi1a, pi1b, pi2a, pi2b, pi1a2, pi1c;
        public PaymentVoucher pv;
        public PaymentVoucherDetail pvd1, pvd2, pvd3;

        // sales
        public SalesOrder so1, so2;
        public SalesOrderDetail so1a, so1b, so1c, so2a, so2b;
        public DeliveryOrder do1, do2, do3;
        public DeliveryOrderDetail do1a, do1b, do2a, do2b, do1a2, do1c;
        public SalesInvoice si1, si2, si3;
        public SalesInvoiceDetail si1a, si1b, si2a, si2b, si1a2, si1c;
        public ReceiptVoucher rv;
        public ReceiptVoucherDetail rvd1, rvd2, rvd3;

        // virtual order
        public VirtualOrder virtualOrder1, virtualOrder2;
        public VirtualOrderDetail voD1a, voD1b, voD2a, voD2b;
        public TemporaryDeliveryOrder temporaryDeliveryOrder1, temporaryDeliveryOrder2;
        public TemporaryDeliveryOrderDetail tdoD1a, tdoD1b, tdoD2a, tdoD2b;
        public TemporaryDeliveryOrderClearance tdoc1, tdoc2, tdoc3, tdoc4, tdoc5;
        public TemporaryDeliveryOrderClearanceDetail tdocd1a, tdocd2a, tdocd2b, tdocd3a, tdocd3b, tdocd4a, tdocd4b, tdocd5b;
        public SalesOrder sales1, sales2;
        public SalesOrderDetail salesDetail1a, salesDetail1b, salesDetail2a, salesDetail2b;
        public DeliveryOrder delivery1, delivery2;
        public DeliveryOrderDetail deliveryDetail1a, deliveryDetail1b, deliveryDetail2a, deliveryDetail2b;

        public SalesOrder GramSalesOrder1;
        public SalesOrderDetail GramSOD1a, GramSOD1b;
        public DeliveryOrder GramDeliveryOrder1;
        public TemporaryDeliveryOrder GramTDO1, GramTDO2;
        public TemporaryDeliveryOrderDetail GramTDOD1a, GramTDOD1b, GramTDOD2a, GramTDOD2b;
        public TemporaryDeliveryOrderClearance GramTDOC1R, GramTDOC1W, GramTDOC2R;
        public TemporaryDeliveryOrderClearanceDetail GramTDOC1Ra, GramTDOC1Rb, GramTDOC1Wa, GramTDOC2Ra, GramTDOC2Rb;
        public DeliveryOrderDetail GramDOD1a, GramDOD1b;
        public SalesInvoice GramSI1;
        public SalesInvoiceDetail GramSI1a, GramSI1b;

        // currency
        public Currency currencyEUR, currencyUSD, currencyIDR;
        public ExchangeRate DayMinusTwoRateEUR, DayMinusOneRateEUR, DayRateEUR, DayMinusTwoRateUSD, DayMinusOneRateUSD, DayRateUSD;

        public Closing thisMonthClosing;

        public DataBuilder()
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

            typeAdhesiveBlanket = _itemTypeService.CreateObject("AdhesiveBlanket", "AdhesiveBlanket");
            typeAdhesiveRoller = _itemTypeService.CreateObject("AdhesiveRoller", "AdhesiveRoller");
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

            admin = _userAccountService.FindOrCreateSysAdmin();
            // COLUMN: Code, variablename, Name, Group, Level, xxx, ParentId, IsLegacy, IsLeaf, LegacyCode
            // REGEX:
            // ^([^,]*),([^,]*),([^,]*),([^,]*),([^,]*),([^,]*),([^,]*),([^,]*),([^,]*),([^,]*)$
            // REPLACE with:
            // Account $2 = _accountService.CreateObject(new Account() { Code = "$1", Name = "$3", Group = $4, Level = $5, ParentId = $7, IsLegacy = $8, IsLeaf = $9, LegacyCode = "$10" }, _accountService);
            if (!_accountService.GetLegacyObjects().Any())
            {
                Account AKTIVA1 = _accountService.CreateObject(new Account() { Code = "1", Name = "AKTIVA", Group = 1, Level = 1, ParentId = null, IsLegacy = true, IsLeaf = false, LegacyCode = "A1" }, _accountService);
                Account AKTIVALANCAR2 = _accountService.CreateObject(new Account() { Code = "11", Name = "AKTIVA LANCAR ", Group = 1, Level = 2, ParentId = AKTIVA1.Id, IsLegacy = true, IsLeaf = false, LegacyCode = "A11" }, _accountService);
                Account KASDANSETARAKAS3 = _accountService.CreateObject(new Account() { Code = "1101", Name = "KAS DAN SETARA KAS ", Group = 1, Level = 3, ParentId = AKTIVALANCAR2.Id, IsLegacy = true, IsLeaf = false, LegacyCode = "A1101" }, _accountService);
                Account KASDANBANK4 = _accountService.CreateObject(new Account() { Code = "110101", Name = "KAS DAN BANK", Group = 1, Level = 4, ParentId = KASDANSETARAKAS3.Id, IsLegacy = true, IsLeaf = false, LegacyCode = "A110101" }, _accountService);
                Account DEPOSITOBERJANGKA3 = _accountService.CreateObject(new Account() { Code = "1102", Name = "DEPOSITO BERJANGKA", Group = 1, Level = 3, ParentId = AKTIVALANCAR2.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account DEPOSITOBERJANGKA4 = _accountService.CreateObject(new Account() { Code = "110201", Name = "DEPOSITO BERJANGKA", Group = 1, Level = 4, ParentId = DEPOSITOBERJANGKA3.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account PIUTANGUSAHA3 = _accountService.CreateObject(new Account() { Code = "1103", Name = "PIUTANG USAHA", Group = 1, Level = 3, ParentId = AKTIVALANCAR2.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account PIUTANGUSAHA4 = _accountService.CreateObject(new Account() { Code = "110301", Name = "PIUTANG USAHA", Group = 1, Level = 4, ParentId = PIUTANGUSAHA3.Id, IsLegacy = true, IsLeaf = false, LegacyCode = "A1102" }, _accountService);
                Account PIUTANGLAINLAIN3 = _accountService.CreateObject(new Account() { Code = "1104", Name = "PIUTANG LAIN-LAIN", Group = 1, Level = 3, ParentId = AKTIVALANCAR2.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account PIUTANGLAINLAIN4 = _accountService.CreateObject(new Account() { Code = "110401", Name = "PIUTANG LAIN-LAIN", Group = 1, Level = 4, ParentId = PIUTANGLAINLAIN3.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account PIUTANGLAINNYA5 = _accountService.CreateObject(new Account() { Code = "11040103", Name = "PIUTANG LAINNYA", Group = 1, Level = 5, ParentId = PIUTANGLAINLAIN4.Id, IsLegacy = true, IsLeaf = true, LegacyCode = "A1106" }, _accountService);
                Account PIUTANGGBCH4 = _accountService.CreateObject(new Account() { Code = "110402", Name = "PIUTANG GBCH", Group = 1, Level = 4, ParentId = PIUTANGLAINLAIN3.Id, IsLegacy = true, IsLeaf = false, LegacyCode = "A1103" }, _accountService);
                Account PERSEDIAANBARANG3 = _accountService.CreateObject(new Account() { Code = "1105", Name = "PERSEDIAAN BARANG", Group = 1, Level = 3, ParentId = AKTIVALANCAR2.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account PERSEDIAANBARANG4 = _accountService.CreateObject(new Account() { Code = "110501", Name = "PERSEDIAAN BARANG", Group = 1, Level = 4, ParentId = PERSEDIAANBARANG3.Id, IsLegacy = true, IsLeaf = false, LegacyCode = "A1104" }, _accountService);
                Account PERSEDPRINTINGCHEMICALS5 = _accountService.CreateObject(new Account() { Code = "11050001", Name = "PERSED. PRINTING CHEMICALS", Group = 1, Level = 5, ParentId = PERSEDIAANBARANG4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account PERSEDPRINTINGBLANKET5 = _accountService.CreateObject(new Account() { Code = "11050002", Name = "PERSED. PRINTING BLANKET", Group = 1, Level = 5, ParentId = PERSEDIAANBARANG4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account PERSEDPRINTINGROLLERS5 = _accountService.CreateObject(new Account() { Code = "11050003", Name = "PERSED. PRINTING ROLLERS", Group = 1, Level = 5, ParentId = PERSEDIAANBARANG4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account PERSEDBARANGLAINNYA5 = _accountService.CreateObject(new Account() { Code = "11050004", Name = "PERSED. BARANG LAINNYA", Group = 1, Level = 5, ParentId = PERSEDIAANBARANG4.Id, IsLegacy = true, IsLeaf = true, LegacyCode = "A1104002" }, _accountService);
                Account PERSEDBAHANPEMBANTU5 = _accountService.CreateObject(new Account() { Code = "11050005", Name = "PERSED. BAHAN PEMBANTU", Group = 1, Level = 5, ParentId = PERSEDIAANBARANG4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account BAHANBAKUCHEMICALS5 = _accountService.CreateObject(new Account() { Code = "11050101", Name = "BAHAN BAKU CHEMICALS", Group = 1, Level = 5, ParentId = PERSEDIAANBARANG4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account BAHANBAKUBLANKET5 = _accountService.CreateObject(new Account() { Code = "11050102", Name = "BAHAN BAKU BLANKET", Group = 1, Level = 5, ParentId = PERSEDIAANBARANG4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account BAHANBAKUROLLERS5 = _accountService.CreateObject(new Account() { Code = "11050103", Name = "BAHAN BAKU ROLLERS", Group = 1, Level = 5, ParentId = PERSEDIAANBARANG4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account BAHANBAKUOTHER5 = _accountService.CreateObject(new Account() { Code = "11050104", Name = "BAHAN BAKU OTHER", Group = 1, Level = 5, ParentId = PERSEDIAANBARANG4.Id, IsLegacy = true, IsLeaf = true, LegacyCode = "A1104001" }, _accountService);
                Account UANGMUKAPEMBELIAN3 = _accountService.CreateObject(new Account() { Code = "1106", Name = "UANG MUKA PEMBELIAN", Group = 1, Level = 3, ParentId = AKTIVALANCAR2.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account UANGMUKAPEMBELIAN4 = _accountService.CreateObject(new Account() { Code = "110601", Name = "UANG MUKA PEMBELIAN", Group = 1, Level = 4, ParentId = UANGMUKAPEMBELIAN3.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account UANGMUKAPEMBELIANLOKAL5 = _accountService.CreateObject(new Account() { Code = "11060001", Name = "UANG MUKA PEMBELIAN LOKAL", Group = 1, Level = 5, ParentId = UANGMUKAPEMBELIAN4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account UANGMUKAPEMBELIANIMPORT5 = _accountService.CreateObject(new Account() { Code = "11060002", Name = "UANG MUKA PEMBELIAN IMPORT", Group = 1, Level = 5, ParentId = UANGMUKAPEMBELIAN4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account UANGMUKALAINNYA5 = _accountService.CreateObject(new Account() { Code = "11060003", Name = "UANG MUKA LAINNYA", Group = 1, Level = 5, ParentId = UANGMUKAPEMBELIAN4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account PAJAKDIBAYARDIMUKA3 = _accountService.CreateObject(new Account() { Code = "1107", Name = "PAJAK DIBAYAR DI MUKA", Group = 1, Level = 3, ParentId = AKTIVALANCAR2.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account PAJAKDIBAYARDIMUKA4 = _accountService.CreateObject(new Account() { Code = "110701", Name = "PAJAK DIBAYAR DI MUKA", Group = 1, Level = 4, ParentId = PAJAKDIBAYARDIMUKA3.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account PPHPS225 = _accountService.CreateObject(new Account() { Code = "11070001", Name = "PPH PS 22", Group = 1, Level = 5, ParentId = PAJAKDIBAYARDIMUKA4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account PPHPS235 = _accountService.CreateObject(new Account() { Code = "11070002", Name = "PPH PS 23", Group = 1, Level = 5, ParentId = PAJAKDIBAYARDIMUKA4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account PPHPS255 = _accountService.CreateObject(new Account() { Code = "11070003", Name = "PPH PS 25", Group = 1, Level = 5, ParentId = PAJAKDIBAYARDIMUKA4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account PPNMASUKAN5 = _accountService.CreateObject(new Account() { Code = "11070004", Name = "PPN MASUKAN", Group = 1, Level = 5, ParentId = PAJAKDIBAYARDIMUKA4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account PPHPS245 = _accountService.CreateObject(new Account() { Code = "11070005", Name = "PPH PS 24", Group = 1, Level = 5, ParentId = PAJAKDIBAYARDIMUKA4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account BIAYADIBAYARDIMUKA3 = _accountService.CreateObject(new Account() { Code = "1108", Name = "BIAYA DIBAYAR DIMUKA", Group = 1, Level = 3, ParentId = AKTIVALANCAR2.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account BIAYADIBAYARDIMUKA4 = _accountService.CreateObject(new Account() { Code = "110801", Name = "BIAYA DIBAYAR DIMUKA", Group = 1, Level = 4, ParentId = BIAYADIBAYARDIMUKA3.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account BDDASURANSIKENDARAAN5 = _accountService.CreateObject(new Account() { Code = "11080001", Name = "BDD-ASURANSI KENDARAAN ", Group = 1, Level = 5, ParentId = BIAYADIBAYARDIMUKA4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account BDDASURANSIGEDUNG5 = _accountService.CreateObject(new Account() { Code = "11080002", Name = "BDD-ASURANSI GEDUNG", Group = 1, Level = 5, ParentId = BIAYADIBAYARDIMUKA4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account BDDASSINVENTARISKANTOR5 = _accountService.CreateObject(new Account() { Code = "11080003", Name = "BDD-ASS. INVENTARIS KANTOR", Group = 1, Level = 5, ParentId = BIAYADIBAYARDIMUKA4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account BDDASSMESINPERALATAN5 = _accountService.CreateObject(new Account() { Code = "11080004", Name = "BDD-ASS. MESIN-PERALATAN", Group = 1, Level = 5, ParentId = BIAYADIBAYARDIMUKA4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account BDDSEWAGEDUNGKANTOR5 = _accountService.CreateObject(new Account() { Code = "11080006", Name = "BDD-SEWA GEDUNG/KANTOR", Group = 1, Level = 5, ParentId = BIAYADIBAYARDIMUKA4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account AKTIVATETAP2 = _accountService.CreateObject(new Account() { Code = "14", Name = "AKTIVA TETAP", Group = 1, Level = 2, ParentId = AKTIVA1.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account TANAH3 = _accountService.CreateObject(new Account() { Code = "1401", Name = "TANAH", Group = 1, Level = 3, ParentId = AKTIVATETAP2.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account TANAH4 = _accountService.CreateObject(new Account() { Code = "140101", Name = "TANAH", Group = 1, Level = 4, ParentId = TANAH3.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account TANAH5 = _accountService.CreateObject(new Account() { Code = "140101001", Name = "TANAH", Group = 1, Level = 5, ParentId = TANAH4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account BANGUNAN3 = _accountService.CreateObject(new Account() { Code = "1402", Name = "BANGUNAN", Group = 1, Level = 3, ParentId = AKTIVATETAP2.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account BANGUNAN4 = _accountService.CreateObject(new Account() { Code = "140201", Name = "BANGUNAN", Group = 1, Level = 4, ParentId = BANGUNAN3.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account BANGUNAN5 = _accountService.CreateObject(new Account() { Code = "14020001", Name = "BANGUNAN", Group = 1, Level = 5, ParentId = BANGUNAN4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account KENDARAANBERMOTORD3 = _accountService.CreateObject(new Account() { Code = "1403", Name = "KENDARAAN BERMOTOR (D)", Group = 1, Level = 3, ParentId = AKTIVATETAP2.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account KENDARAANBERMOTORD4 = _accountService.CreateObject(new Account() { Code = "140301", Name = "KENDARAAN BERMOTOR (D)", Group = 1, Level = 4, ParentId = KENDARAANBERMOTORD3.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account KENDARAANBERMOTORD5 = _accountService.CreateObject(new Account() { Code = "14030001", Name = "KENDARAAN BERMOTOR (D)", Group = 1, Level = 5, ParentId = KENDARAANBERMOTORD4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account INVENTARISKANTOR3 = _accountService.CreateObject(new Account() { Code = "1405", Name = "INVENTARIS KANTOR", Group = 1, Level = 3, ParentId = AKTIVATETAP2.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account INVENTARISKANTOR4 = _accountService.CreateObject(new Account() { Code = "140501", Name = "INVENTARIS KANTOR", Group = 1, Level = 4, ParentId = INVENTARISKANTOR3.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account INVENTARISKANTOR5 = _accountService.CreateObject(new Account() { Code = "14050001", Name = "INVENTARIS KANTOR", Group = 1, Level = 5, ParentId = INVENTARISKANTOR4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account MESINDANPERALATAN3 = _accountService.CreateObject(new Account() { Code = "1406", Name = "MESIN DAN PERALATAN", Group = 1, Level = 3, ParentId = AKTIVATETAP2.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account MESINDANPERALATAN4 = _accountService.CreateObject(new Account() { Code = "140601", Name = "MESIN DAN PERALATAN", Group = 1, Level = 4, ParentId = MESINDANPERALATAN3.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account MESINDANPERALATAN5 = _accountService.CreateObject(new Account() { Code = "14060001", Name = "MESIN DAN PERALATAN", Group = 1, Level = 5, ParentId = MESINDANPERALATAN4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account INSTALASILISTRIK3 = _accountService.CreateObject(new Account() { Code = "1407", Name = "INSTALASI LISTRIK", Group = 1, Level = 3, ParentId = AKTIVATETAP2.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account INSTALASILISTRIK4 = _accountService.CreateObject(new Account() { Code = "140701", Name = "INSTALASI LISTRIK", Group = 1, Level = 4, ParentId = INSTALASILISTRIK3.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account INSTALASILISTRIK5 = _accountService.CreateObject(new Account() { Code = "14070001", Name = "INSTALASI LISTRIK", Group = 1, Level = 5, ParentId = INSTALASILISTRIK4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account AKTIVALEASING5 = _accountService.CreateObject(new Account() { Code = "14070002", Name = "AKTIVA LEASING", Group = 1, Level = 5, ParentId = INSTALASILISTRIK4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account AKUMULASIPENYUSUTAN3 = _accountService.CreateObject(new Account() { Code = "1408", Name = "AKUMULASI PENYUSUTAN", Group = 1, Level = 3, ParentId = AKTIVATETAP2.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "X2401" }, _accountService);
                Account AKUMULASIPENYUSUTAN4 = _accountService.CreateObject(new Account() { Code = "140801", Name = "AKUMULASI PENYUSUTAN", Group = 1, Level = 4, ParentId = AKUMULASIPENYUSUTAN3.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account AKUMPENYBANGUNAN5 = _accountService.CreateObject(new Account() { Code = "14080001", Name = "AKUM. PENY. BANGUNAN", Group = 1, Level = 5, ParentId = AKUMULASIPENYUSUTAN4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account AKUMPENYKENDBERMOTORD5 = _accountService.CreateObject(new Account() { Code = "14080002", Name = "AKUM. PENY. KEND. BERMOTOR-D", Group = 1, Level = 5, ParentId = AKUMULASIPENYUSUTAN4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account AKUMPENYINVENTARISKANTOR5 = _accountService.CreateObject(new Account() { Code = "14080004", Name = "AKUM. PENY. INVENTARIS KANTOR", Group = 1, Level = 5, ParentId = AKUMULASIPENYUSUTAN4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account AKUMPENYMESINPERALATAN5 = _accountService.CreateObject(new Account() { Code = "14080005", Name = "AKUM. PENY. MESIN-PERALATAN", Group = 1, Level = 5, ParentId = AKUMULASIPENYUSUTAN4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account AKUMPENYINSTALASILISTRIK5 = _accountService.CreateObject(new Account() { Code = "14080006", Name = "AKUM. PENY. INSTALASI LISTRIK", Group = 1, Level = 5, ParentId = AKUMULASIPENYUSUTAN4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account AKUMPENYAKTIVALEASING5 = _accountService.CreateObject(new Account() { Code = "14080007", Name = "AKUM. PENY. AKTIVA LEASING", Group = 1, Level = 5, ParentId = AKUMULASIPENYUSUTAN4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account AKTIVALAINLAIN2 = _accountService.CreateObject(new Account() { Code = "15", Name = "AKTIVA LAIN-LAIN", Group = 1, Level = 2, ParentId = AKTIVA1.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account UANGJAMINAN3 = _accountService.CreateObject(new Account() { Code = "1501", Name = "UANG JAMINAN", Group = 1, Level = 3, ParentId = AKTIVALAINLAIN2.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account UANGJAMINAN4 = _accountService.CreateObject(new Account() { Code = "150101", Name = "UANG JAMINAN", Group = 1, Level = 4, ParentId = UANGJAMINAN3.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account UANGJAMINAN5 = _accountService.CreateObject(new Account() { Code = "15010001", Name = "UANG JAMINAN", Group = 1, Level = 5, ParentId = UANGJAMINAN4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account BEBANUSAHA1 = _accountService.CreateObject(new Account() { Code = "2", Name = "BEBAN USAHA", Group = 2, Level = 1, ParentId = null, IsLegacy = true, IsLeaf = false, LegacyCode = "X2" }, _accountService);
                Account BEBANPEMASARAN2 = _accountService.CreateObject(new Account() { Code = "61", Name = "BEBAN PEMASARAN", Group = 2, Level = 2, ParentId = BEBANUSAHA1.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account BIAYAIKLANDANREKLAME3 = _accountService.CreateObject(new Account() { Code = "6101", Name = "BIAYA IKLAN DAN REKLAME", Group = 2, Level = 3, ParentId = BEBANPEMASARAN2.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account BIAYAIKLANDANREKLAME4 = _accountService.CreateObject(new Account() { Code = "610101", Name = "BIAYA IKLAN DAN REKLAME", Group = 2, Level = 4, ParentId = BIAYAIKLANDANREKLAME3.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account BIAYAIKLANDANREKLAME5 = _accountService.CreateObject(new Account() { Code = "61010001", Name = "BIAYA IKLAN DAN REKLAME", Group = 2, Level = 5, ParentId = BIAYAIKLANDANREKLAME4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account BIAYAPAMERAN3 = _accountService.CreateObject(new Account() { Code = "6102", Name = "BIAYA PAMERAN", Group = 2, Level = 3, ParentId = BEBANPEMASARAN2.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account BIAYAPAMERAN4 = _accountService.CreateObject(new Account() { Code = "610201", Name = "BIAYA PAMERAN", Group = 2, Level = 4, ParentId = BIAYAPAMERAN3.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account BIAYAPAMERAN5 = _accountService.CreateObject(new Account() { Code = "61020001", Name = "BIAYA PAMERAN", Group = 2, Level = 5, ParentId = BIAYAPAMERAN4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account BIAYAPROMOSI3 = _accountService.CreateObject(new Account() { Code = "6103", Name = "BIAYA PROMOSI", Group = 2, Level = 3, ParentId = BEBANPEMASARAN2.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account BIAYAPROMOSI4 = _accountService.CreateObject(new Account() { Code = "610301", Name = "BIAYA PROMOSI", Group = 2, Level = 4, ParentId = BIAYAPROMOSI3.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account BIAYAPROMOSI5 = _accountService.CreateObject(new Account() { Code = "61030001", Name = "BIAYA PROMOSI", Group = 2, Level = 5, ParentId = BIAYAPROMOSI4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account BIAYAPERJALANANDINAS3 = _accountService.CreateObject(new Account() { Code = "6105", Name = "BIAYA PERJALANAN DINAS", Group = 2, Level = 3, ParentId = BEBANPEMASARAN2.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account BIAYAPERJALANANDINAS4 = _accountService.CreateObject(new Account() { Code = "610501", Name = "BIAYA PERJALANAN DINAS", Group = 2, Level = 4, ParentId = BIAYAPERJALANANDINAS3.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account BIAYAPERJALANANDINAS5 = _accountService.CreateObject(new Account() { Code = "61050001", Name = "BIAYA PERJALANAN DINAS", Group = 2, Level = 5, ParentId = BIAYAPERJALANANDINAS4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account BIAYAPENGIRIMAN3 = _accountService.CreateObject(new Account() { Code = "6106", Name = "BIAYA PENGIRIMAN", Group = 2, Level = 3, ParentId = BEBANPEMASARAN2.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account BIAYAPENGIRIMAN4 = _accountService.CreateObject(new Account() { Code = "610601", Name = "BIAYA PENGIRIMAN", Group = 2, Level = 4, ParentId = BIAYAPENGIRIMAN3.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account BIAYAPENGIRIMAN5 = _accountService.CreateObject(new Account() { Code = "61060001", Name = "BIAYA PENGIRIMAN", Group = 2, Level = 5, ParentId = BIAYAPENGIRIMAN4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account BIAYAKEMASAN3 = _accountService.CreateObject(new Account() { Code = "6107", Name = "BIAYA KEMASAN", Group = 2, Level = 3, ParentId = BEBANPEMASARAN2.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account BIAYAKEMASAN4 = _accountService.CreateObject(new Account() { Code = "610701", Name = "BIAYA KEMASAN", Group = 2, Level = 4, ParentId = BIAYAKEMASAN3.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account BIAYAKEMASAN5 = _accountService.CreateObject(new Account() { Code = "61070001", Name = "BIAYA KEMASAN", Group = 2, Level = 5, ParentId = BIAYAKEMASAN4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account BIAYAADMINISTRASIUMUM2 = _accountService.CreateObject(new Account() { Code = "62", Name = "BIAYA ADMINISTRASI & UMUM", Group = 2, Level = 2, ParentId = BEBANUSAHA1.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account BIAYATENAGAKERJA3 = _accountService.CreateObject(new Account() { Code = "6201", Name = "BIAYA TENAGA KERJA", Group = 2, Level = 3, ParentId = BIAYAADMINISTRASIUMUM2.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account BIAYATENAGAKERJA4 = _accountService.CreateObject(new Account() { Code = "620101", Name = "BIAYA TENAGA KERJA", Group = 2, Level = 4, ParentId = BIAYATENAGAKERJA3.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account BIAYAGAJIDANUPAH5 = _accountService.CreateObject(new Account() { Code = "62010001", Name = "BIAYA GAJI DAN UPAH", Group = 2, Level = 5, ParentId = BIAYATENAGAKERJA4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account PREMIJAMSOSTEK5 = _accountService.CreateObject(new Account() { Code = "62010002", Name = "PREMI JAMSOSTEK", Group = 2, Level = 5, ParentId = BIAYATENAGAKERJA4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account BIAYAREKRUITMENT5 = _accountService.CreateObject(new Account() { Code = "62010003", Name = "BIAYA REKRUITMENT", Group = 2, Level = 5, ParentId = BIAYATENAGAKERJA4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account BYPENDIDIKANLATIHAN5 = _accountService.CreateObject(new Account() { Code = "62010004", Name = "BY. PENDIDIKAN & LATIHAN", Group = 2, Level = 5, ParentId = BIAYATENAGAKERJA4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account BIAYAPERAWATANKESEHATAN5 = _accountService.CreateObject(new Account() { Code = "62010005", Name = "BIAYA PERAWATAN KESEHATAN", Group = 2, Level = 5, ParentId = BIAYATENAGAKERJA4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account BIAYAOPERASIONALKANTOR3 = _accountService.CreateObject(new Account() { Code = "6202", Name = "BIAYA OPERASIONAL KANTOR", Group = 2, Level = 3, ParentId = BIAYAADMINISTRASIUMUM2.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account BIAYAOPERASIONALKANTOR4 = _accountService.CreateObject(new Account() { Code = "620201", Name = "BIAYA OPERASIONAL KANTOR", Group = 2, Level = 4, ParentId = BIAYAOPERASIONALKANTOR3.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account BIAYACETAKAN5 = _accountService.CreateObject(new Account() { Code = "62020001", Name = "BIAYA CETAKAN", Group = 2, Level = 5, ParentId = BIAYAOPERASIONALKANTOR4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account BIAYAATK5 = _accountService.CreateObject(new Account() { Code = "62020002", Name = "BIAYA ATK", Group = 2, Level = 5, ParentId = BIAYAOPERASIONALKANTOR4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account BIAYATRANSPORTD5 = _accountService.CreateObject(new Account() { Code = "62020003", Name = "BIAYA TRANSPORT (D)", Group = 2, Level = 5, ParentId = BIAYAOPERASIONALKANTOR4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account BIAYAPERIJINAN5 = _accountService.CreateObject(new Account() { Code = "62020005", Name = "BIAYA PERIJINAN", Group = 2, Level = 5, ParentId = BIAYAOPERASIONALKANTOR4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account BIAYAMATERAI5 = _accountService.CreateObject(new Account() { Code = "62020006", Name = "BIAYA MATERAI", Group = 2, Level = 5, ParentId = BIAYAOPERASIONALKANTOR4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account BIAYAFOTOCOPY5 = _accountService.CreateObject(new Account() { Code = "62020007", Name = "BIAYA FOTOCOPY", Group = 2, Level = 5, ParentId = BIAYAOPERASIONALKANTOR4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account BYKEBERSIHANKEAMANAN5 = _accountService.CreateObject(new Account() { Code = "62020008", Name = "BY. KEBERSIHAN & KEAMANAN", Group = 2, Level = 5, ParentId = BIAYAOPERASIONALKANTOR4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account BIAYAPOSDANKURIR5 = _accountService.CreateObject(new Account() { Code = "62020009", Name = "BIAYA POS DAN KURIR", Group = 2, Level = 5, ParentId = BIAYAOPERASIONALKANTOR4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account BIAYAKORANDANMAJALAH5 = _accountService.CreateObject(new Account() { Code = "62020010", Name = "BIAYA KORAN DAN MAJALAH", Group = 2, Level = 5, ParentId = BIAYAOPERASIONALKANTOR4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account BIAYAPAJAKKENDARAAN5 = _accountService.CreateObject(new Account() { Code = "62020011", Name = "BIAYA PAJAK KENDARAAN", Group = 2, Level = 5, ParentId = BIAYAOPERASIONALKANTOR4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account BIAYADENDADANPAJAK5 = _accountService.CreateObject(new Account() { Code = "62020012", Name = "BIAYA DENDA DAN PAJAK", Group = 2, Level = 5, ParentId = BIAYAOPERASIONALKANTOR4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account BIAYAKEPERLUANKANTOR5 = _accountService.CreateObject(new Account() { Code = "62020013", Name = "BIAYA KEPERLUAN KANTOR", Group = 2, Level = 5, ParentId = BIAYAOPERASIONALKANTOR4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account BIAYAPEMBULATAN5 = _accountService.CreateObject(new Account() { Code = "62020014", Name = "BIAYA PEMBULATAN", Group = 2, Level = 5, ParentId = BIAYAOPERASIONALKANTOR4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account BYPEMELIHARAANPERBAIKAN3 = _accountService.CreateObject(new Account() { Code = "6203", Name = "BY. PEMELIHARAAN-PERBAIKAN", Group = 2, Level = 3, ParentId = BIAYAADMINISTRASIUMUM2.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account BYPEMELIHARAANPERBAIKAN4 = _accountService.CreateObject(new Account() { Code = "620301", Name = "BY. PEMELIHARAAN-PERBAIKAN", Group = 2, Level = 4, ParentId = BYPEMELIHARAANPERBAIKAN3.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account BPPGEDUNGKANTOR5 = _accountService.CreateObject(new Account() { Code = "62030001", Name = "BPP-GEDUNG KANTOR", Group = 2, Level = 5, ParentId = BYPEMELIHARAANPERBAIKAN4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account BPPGEDUNGGUDANG5 = _accountService.CreateObject(new Account() { Code = "62030002", Name = "BPP-GEDUNG GUDANG", Group = 2, Level = 5, ParentId = BYPEMELIHARAANPERBAIKAN4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account BPPINVENTARISKANTOR5 = _accountService.CreateObject(new Account() { Code = "62030003", Name = "BPP-INVENTARIS KANTOR", Group = 2, Level = 5, ParentId = BYPEMELIHARAANPERBAIKAN4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account BPPKENDARAAND5 = _accountService.CreateObject(new Account() { Code = "62030004", Name = "BPP-KENDARAAN (D)", Group = 2, Level = 5, ParentId = BYPEMELIHARAANPERBAIKAN4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account BPPMESINDANPERALATAN5 = _accountService.CreateObject(new Account() { Code = "62030006", Name = "BPP-MESIN DAN PERALATAN", Group = 2, Level = 5, ParentId = BYPEMELIHARAANPERBAIKAN4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account BIAYASEWA3 = _accountService.CreateObject(new Account() { Code = "6204", Name = "BIAYA SEWA", Group = 2, Level = 3, ParentId = BIAYAADMINISTRASIUMUM2.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account BIAYASEWA4 = _accountService.CreateObject(new Account() { Code = "620401", Name = "BIAYA SEWA", Group = 2, Level = 4, ParentId = BIAYASEWA3.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account BIAYASEWAKANTOR5 = _accountService.CreateObject(new Account() { Code = "62040001", Name = "BIAYA SEWA KANTOR", Group = 2, Level = 5, ParentId = BIAYASEWA4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account BIAYASEWAGUDANG5 = _accountService.CreateObject(new Account() { Code = "62040002", Name = "BIAYA SEWA GUDANG", Group = 2, Level = 5, ParentId = BIAYASEWA4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account BIAYAUTILITASKOMUNIKASI3 = _accountService.CreateObject(new Account() { Code = "6205", Name = "BIAYA UTILITAS-KOMUNIKASI", Group = 2, Level = 3, ParentId = BIAYAADMINISTRASIUMUM2.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account BIAYAUTILITASKOMUNIKASI4 = _accountService.CreateObject(new Account() { Code = "620501", Name = "BIAYA UTILITAS-KOMUNIKASI", Group = 2, Level = 4, ParentId = BIAYAUTILITASKOMUNIKASI3.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account BIAYALISTRIK5 = _accountService.CreateObject(new Account() { Code = "62050001", Name = "BIAYA LISTRIK", Group = 2, Level = 5, ParentId = BIAYAUTILITASKOMUNIKASI4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account BIAYAPAM5 = _accountService.CreateObject(new Account() { Code = "62050002", Name = "BIAYA PAM", Group = 2, Level = 5, ParentId = BIAYAUTILITASKOMUNIKASI4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account BIAYATELEPON5 = _accountService.CreateObject(new Account() { Code = "62050003", Name = "BIAYA TELEPON", Group = 2, Level = 5, ParentId = BIAYAUTILITASKOMUNIKASI4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account BIAYAFAKSIMILI5 = _accountService.CreateObject(new Account() { Code = "62050004", Name = "BIAYA FAKSIMILI", Group = 2, Level = 5, ParentId = BIAYAUTILITASKOMUNIKASI4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account BIAYAHP5 = _accountService.CreateObject(new Account() { Code = "62050005", Name = "BIAYA HP", Group = 2, Level = 5, ParentId = BIAYAUTILITASKOMUNIKASI4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account BIAYAINTERNET5 = _accountService.CreateObject(new Account() { Code = "62050006", Name = "BIAYA INTERNET", Group = 2, Level = 5, ParentId = BIAYAUTILITASKOMUNIKASI4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account BIAYAJASAOPERASIONAL3 = _accountService.CreateObject(new Account() { Code = "6206", Name = "BIAYA JASA OPERASIONAL", Group = 2, Level = 3, ParentId = BIAYAADMINISTRASIUMUM2.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account BIAYAJASAOPERASIONAL4 = _accountService.CreateObject(new Account() { Code = "620601", Name = "BIAYA JASA OPERASIONAL", Group = 2, Level = 4, ParentId = BIAYAJASAOPERASIONAL3.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account BIAYANOTARIS5 = _accountService.CreateObject(new Account() { Code = "62060001", Name = "BIAYA NOTARIS", Group = 2, Level = 5, ParentId = BIAYAJASAOPERASIONAL4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account BIAYAJASAANGKUTAN5 = _accountService.CreateObject(new Account() { Code = "62060002", Name = "BIAYA JASA ANGKUTAN", Group = 2, Level = 5, ParentId = BIAYAJASAOPERASIONAL4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account BIAYAPENASEHATHUKUM5 = _accountService.CreateObject(new Account() { Code = "62060003", Name = "BIAYA PENASEHAT HUKUM", Group = 2, Level = 5, ParentId = BIAYAJASAOPERASIONAL4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account BIAYAKONSULTANTEKNIK5 = _accountService.CreateObject(new Account() { Code = "62060004", Name = "BIAYA KONSULTAN TEKNIK", Group = 2, Level = 5, ParentId = BIAYAJASAOPERASIONAL4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account BIAYAMANAGEMENTFEE5 = _accountService.CreateObject(new Account() { Code = "62060005", Name = "BIAYA MANAGEMENT FEE", Group = 2, Level = 5, ParentId = BIAYAJASAOPERASIONAL4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account BIAYAROYALTI5 = _accountService.CreateObject(new Account() { Code = "62060007", Name = "BIAYA ROYALTI", Group = 2, Level = 5, ParentId = BIAYAJASAOPERASIONAL4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account BIAYAPENILAIAN5 = _accountService.CreateObject(new Account() { Code = "62060008", Name = "BIAYA PENILAIAN", Group = 2, Level = 5, ParentId = BIAYAJASAOPERASIONAL4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account BIAYASEMINARKHURSUS5 = _accountService.CreateObject(new Account() { Code = "62060009", Name = "BIAYA SEMINAR & KHURSUS", Group = 2, Level = 5, ParentId = BIAYAJASAOPERASIONAL4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account BIAYAASURANSI3 = _accountService.CreateObject(new Account() { Code = "6207", Name = "BIAYA ASURANSI", Group = 2, Level = 3, ParentId = BIAYAADMINISTRASIUMUM2.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account BIAYAASURANSI4 = _accountService.CreateObject(new Account() { Code = "620701", Name = "BIAYA ASURANSI", Group = 2, Level = 4, ParentId = BIAYAASURANSI3.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account BIAYAASURANSIKENDARAAN5 = _accountService.CreateObject(new Account() { Code = "62070001", Name = "BIAYA ASURANSI KENDARAAN ", Group = 2, Level = 5, ParentId = BIAYAASURANSI4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account BIAYAASURANSIGEDUNG5 = _accountService.CreateObject(new Account() { Code = "62070002", Name = "BIAYA ASURANSI GEDUNG", Group = 2, Level = 5, ParentId = BIAYAASURANSI4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account BYASURANSIINVENTARISKANTOR5 = _accountService.CreateObject(new Account() { Code = "62070003", Name = "BY. ASURANSI INVENTARIS KANTOR", Group = 2, Level = 5, ParentId = BIAYAASURANSI4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account BYASSMESINPERALATAN5 = _accountService.CreateObject(new Account() { Code = "62070004", Name = "BY. ASS. MESIN-PERALATAN", Group = 2, Level = 5, ParentId = BIAYAASURANSI4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account BYPENYUSUTANAMORTISASI3 = _accountService.CreateObject(new Account() { Code = "6208", Name = "BY. PENYUSUTAN AMORTISASI", Group = 2, Level = 3, ParentId = BIAYAADMINISTRASIUMUM2.Id, IsLegacy = true, IsLeaf = false, LegacyCode = "X2402" }, _accountService);
                Account BYPENYUSUTANAMORTISASI4 = _accountService.CreateObject(new Account() { Code = "620801", Name = "BY. PENYUSUTAN AMORTISASI", Group = 2, Level = 4, ParentId = BYPENYUSUTANAMORTISASI3.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account BIAYAPENYUSUTANBANGUNAN5 = _accountService.CreateObject(new Account() { Code = "62080001", Name = "BIAYA PENYUSUTAN BANGUNAN", Group = 2, Level = 5, ParentId = BYPENYUSUTANAMORTISASI4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account BIAYAPENYUSUTANKENDARAAND5 = _accountService.CreateObject(new Account() { Code = "62080002", Name = "BIAYA PENYUSUTAN KENDARAAN (D)", Group = 2, Level = 5, ParentId = BYPENYUSUTANAMORTISASI4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account BIAYAPENYUSUTANINVENTARISKANTOR5 = _accountService.CreateObject(new Account() { Code = "62080004", Name = "BIAYA PENYUSUTAN INVENTARIS KANTOR", Group = 2, Level = 5, ParentId = BYPENYUSUTANAMORTISASI4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account BIAYAPENYUSUTANMESINPERALATAN5 = _accountService.CreateObject(new Account() { Code = "62080005", Name = "BIAYA PENYUSUTAN MESIN-PERALATAN", Group = 2, Level = 5, ParentId = BYPENYUSUTANAMORTISASI4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account BIAYAPENYUSUTANINSTALASILISTRIK5 = _accountService.CreateObject(new Account() { Code = "62080007", Name = "BIAYA PENYUSUTAN INSTALASI LISTRIK", Group = 2, Level = 5, ParentId = BYPENYUSUTANAMORTISASI4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account BIAYAPENYUSUTANAKTIVALEASING5 = _accountService.CreateObject(new Account() { Code = "62080008", Name = "BIAYA PENYUSUTAN AKTIVA LEASING", Group = 2, Level = 5, ParentId = BYPENYUSUTANAMORTISASI4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account BIAYAKEUANGAN3 = _accountService.CreateObject(new Account() { Code = "6209", Name = "BIAYA KEUANGAN", Group = 2, Level = 3, ParentId = BIAYAADMINISTRASIUMUM2.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account BIAYAKEUANGAN4 = _accountService.CreateObject(new Account() { Code = "620901", Name = "BIAYA KEUANGAN", Group = 2, Level = 4, ParentId = BIAYAKEUANGAN3.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account BIAYABUNGABANK5 = _accountService.CreateObject(new Account() { Code = "62090001", Name = "BIAYA BUNGA BANK", Group = 2, Level = 5, ParentId = BIAYAKEUANGAN4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "X2403" }, _accountService);
                Account BIAYAADMINISTRASIBANK5 = _accountService.CreateObject(new Account() { Code = "62090002", Name = "BIAYA ADMINISTRASI BANK", Group = 2, Level = 5, ParentId = BIAYAKEUANGAN4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account BIAYAPROVISI5 = _accountService.CreateObject(new Account() { Code = "62090003", Name = "BIAYA PROVISI", Group = 2, Level = 5, ParentId = BIAYAKEUANGAN4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account BEBANLAINLAIN2 = _accountService.CreateObject(new Account() { Code = "72", Name = "BEBAN LAIN-LAIN", Group = 2, Level = 2, ParentId = BEBANUSAHA1.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account BEBANLAINLAIN3 = _accountService.CreateObject(new Account() { Code = "7201", Name = "BEBAN LAIN-LAIN", Group = 2, Level = 3, ParentId = BEBANLAINLAIN2.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account BEBANLAINLAIN4 = _accountService.CreateObject(new Account() { Code = "720101", Name = "BEBAN LAIN-LAIN", Group = 2, Level = 4, ParentId = BEBANLAINLAIN3.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account RUGISELISIHKURS5 = _accountService.CreateObject(new Account() { Code = "72010001", Name = "RUGI SELISIH KURS", Group = 2, Level = 5, ParentId = BEBANLAINLAIN4.Id, IsLegacy = true, IsLeaf = true, LegacyCode = "X25" }, _accountService);
                Account BEBANLAINNYA5 = _accountService.CreateObject(new Account() { Code = "72010002", Name = "BEBAN LAINNYA", Group = 2, Level = 5, ParentId = BEBANLAINLAIN4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "X2302004" }, _accountService);
                Account BEBANNONOPERASIONAL3 = _accountService.CreateObject(new Account() { Code = "7202", Name = "BEBAN NON OPERASIONAL", Group = 2, Level = 3, ParentId = BEBANLAINLAIN2.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account BEBANNONOPERASIONAL4 = _accountService.CreateObject(new Account() { Code = "720201", Name = "BEBAN NON OPERASIONAL", Group = 2, Level = 4, ParentId = BEBANNONOPERASIONAL3.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account LABAPENJUALANAKTIVATETAP5 = _accountService.CreateObject(new Account() { Code = "72020001", Name = "LABA PENJUALAN AKTIVA TETAP", Group = 2, Level = 5, ParentId = BEBANNONOPERASIONAL4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account PASSIVA1 = _accountService.CreateObject(new Account() { Code = "3", Name = "PASSIVA", Group = 3, Level = 1, ParentId = null, IsLegacy = true, IsLeaf = false, LegacyCode = "L3" }, _accountService);
                Account KEWAJIBANLANCAR2 = _accountService.CreateObject(new Account() { Code = "21", Name = "KEWAJIBAN LANCAR", Group = 3, Level = 2, ParentId = PASSIVA1.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account HUTANGBANK3 = _accountService.CreateObject(new Account() { Code = "2101", Name = "HUTANG BANK", Group = 3, Level = 3, ParentId = KEWAJIBANLANCAR2.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account HUTANGBANK4 = _accountService.CreateObject(new Account() { Code = "210101", Name = "HUTANG BANK", Group = 3, Level = 4, ParentId = HUTANGBANK3.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account HUTANGBANK5 = _accountService.CreateObject(new Account() { Code = "21010001", Name = "HUTANG BANK", Group = 3, Level = 5, ParentId = HUTANGBANK4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account HUTANGLEASING5 = _accountService.CreateObject(new Account() { Code = "21010002", Name = "HUTANG LEASING", Group = 3, Level = 5, ParentId = HUTANGBANK4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account HUTANGUSAHA3 = _accountService.CreateObject(new Account() { Code = "2102", Name = "HUTANG USAHA", Group = 3, Level = 3, ParentId = KEWAJIBANLANCAR2.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account HUTANGUSAHA4 = _accountService.CreateObject(new Account() { Code = "210201", Name = "HUTANG USAHA", Group = 3, Level = 4, ParentId = HUTANGUSAHA3.Id, IsLegacy = true, IsLeaf = false, LegacyCode = "L3101" }, _accountService);
                Account HUTANGPEMBELIANLOKAL5 = _accountService.CreateObject(new Account() { Code = "21020001", Name = "HUTANG PEMBELIAN LOKAL", Group = 3, Level = 5, ParentId = HUTANGUSAHA4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account HUTANGPEMBELIANIMPORT5 = _accountService.CreateObject(new Account() { Code = "21020002", Name = "HUTANG PEMBELIAN IMPORT", Group = 3, Level = 5, ParentId = HUTANGUSAHA4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account HUTANGPEMBELIANLAINNYA5 = _accountService.CreateObject(new Account() { Code = "21020003", Name = "HUTANG PEMBELIAN LAINNYA", Group = 3, Level = 5, ParentId = HUTANGUSAHA4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account HUTANGLAINLAIN3 = _accountService.CreateObject(new Account() { Code = "2103", Name = "HUTANG LAIN-LAIN", Group = 3, Level = 3, ParentId = KEWAJIBANLANCAR2.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account HUTANGLAINLAIN4 = _accountService.CreateObject(new Account() { Code = "210301", Name = "HUTANG LAIN-LAIN", Group = 3, Level = 4, ParentId = HUTANGLAINLAIN3.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account HUTANGLAINNYA5 = _accountService.CreateObject(new Account() { Code = "21030002", Name = "HUTANG LAINNYA", Group = 3, Level = 5, ParentId = HUTANGLAINLAIN4.Id, IsLegacy = true, IsLeaf = true, LegacyCode = "L3107" }, _accountService);
                Account HUTANGGBCH = _accountService.CreateObject(new Account() { Code = "210302", Name = "HUTANG GBCH", Group = 3, Level = 4, ParentId = HUTANGLAINLAIN3.Id, IsLegacy = true, IsLeaf = true, LegacyCode = "L3102" }, _accountService);
                Account UANGMUKAPENJUALAN3 = _accountService.CreateObject(new Account() { Code = "2104", Name = "UANG MUKA PENJUALAN", Group = 3, Level = 3, ParentId = KEWAJIBANLANCAR2.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account UANGMUKAPENJUALAN4 = _accountService.CreateObject(new Account() { Code = "210401", Name = "UANG MUKA PENJUALAN", Group = 3, Level = 4, ParentId = UANGMUKAPENJUALAN3.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account UANGMUKAPENJUALAN5 = _accountService.CreateObject(new Account() { Code = "21040001", Name = "UANG MUKA PENJUALAN", Group = 3, Level = 5, ParentId = UANGMUKAPENJUALAN4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account HUTANGPAJAK3 = _accountService.CreateObject(new Account() { Code = "2105", Name = "HUTANG PAJAK", Group = 3, Level = 3, ParentId = KEWAJIBANLANCAR2.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account HUTANGPAJAK4 = _accountService.CreateObject(new Account() { Code = "210501", Name = "HUTANG PAJAK", Group = 3, Level = 4, ParentId = HUTANGPAJAK3.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account HUTANGPPHPS215 = _accountService.CreateObject(new Account() { Code = "21050001", Name = "HUTANG PPH PS 21", Group = 3, Level = 5, ParentId = HUTANGPAJAK4.Id, IsLegacy = true, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account HUTANGPPHPS235 = _accountService.CreateObject(new Account() { Code = "21050002", Name = "HUTANG PPH PS 23", Group = 3, Level = 5, ParentId = HUTANGPAJAK4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account HUTANGPPHPS255 = _accountService.CreateObject(new Account() { Code = "21050003", Name = "HUTANG PPH PS 25", Group = 3, Level = 5, ParentId = HUTANGPAJAK4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account HUTANGPPHPS265 = _accountService.CreateObject(new Account() { Code = "21050004", Name = "HUTANG PPH PS 26", Group = 3, Level = 5, ParentId = HUTANGPAJAK4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account HUTANGPPHPS295 = _accountService.CreateObject(new Account() { Code = "21050005", Name = "HUTANG PPH PS 29", Group = 3, Level = 5, ParentId = HUTANGPAJAK4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account HUTANGPPHPS425 = _accountService.CreateObject(new Account() { Code = "21050006", Name = "HUTANG PPH PS 4:2", Group = 3, Level = 5, ParentId = HUTANGPAJAK4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account PPNKELUARAN5 = _accountService.CreateObject(new Account() { Code = "21050007", Name = "PPN KELUARAN", Group = 3, Level = 5, ParentId = HUTANGPAJAK4.Id, IsLegacy = true, IsLeaf = true, LegacyCode = "L3108" }, _accountService);
                Account BIAYAYMHDIBAYAR3 = _accountService.CreateObject(new Account() { Code = "2106", Name = "BIAYA YMH DIBAYAR", Group = 3, Level = 3, ParentId = KEWAJIBANLANCAR2.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account BIAYAYMHDIBAYAR4 = _accountService.CreateObject(new Account() { Code = "210601", Name = "BIAYA YMH DIBAYAR", Group = 3, Level = 4, ParentId = BIAYAYMHDIBAYAR3.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account BYGAJIYMHDIBAYAR5 = _accountService.CreateObject(new Account() { Code = "21060001", Name = "BY. GAJI YMH DIBAYAR", Group = 3, Level = 5, ParentId = BIAYAYMHDIBAYAR4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account BUNGABANKYMHDIBAYAR5 = _accountService.CreateObject(new Account() { Code = "21060002", Name = "BUNGA BANK YMH DIBAYAR", Group = 3, Level = 5, ParentId = BIAYAYMHDIBAYAR4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account BYUTILITASYMHDIBAYAR5 = _accountService.CreateObject(new Account() { Code = "21060003", Name = "BY. UTILITAS YMH DIBAYAR", Group = 3, Level = 5, ParentId = BIAYAYMHDIBAYAR4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account BYKOMUNIKASIYMHDIBAYAR5 = _accountService.CreateObject(new Account() { Code = "21060004", Name = "BY. KOMUNIKASI YMH DIBAYAR", Group = 3, Level = 5, ParentId = BIAYAYMHDIBAYAR4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account BYLAINNYAYMHDIBAYAR5 = _accountService.CreateObject(new Account() { Code = "21060005", Name = "BY. LAINNYA YMH DIBAYAR", Group = 3, Level = 5, ParentId = BIAYAYMHDIBAYAR4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account BARANGYMHDICLEARANCE3 = _accountService.CreateObject(new Account() { Code = "2107", Name = "BARANG YMH DICLEARANCE", Group = 3, Level = 3, ParentId = KEWAJIBANLANCAR2.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account BARANGYMHDICLEARANCE4 = _accountService.CreateObject(new Account() { Code = "210701", Name = "BARANG YMH DICLEARANCE", Group = 3, Level = 4, ParentId = BARANGYMHDICLEARANCE3.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account BARANGYMHDICLEARANCE5 = _accountService.CreateObject(new Account() { Code = "21070001", Name = "BARANG YMH DICLEARANCE", Group = 3, Level = 5, ParentId = BARANGYMHDICLEARANCE4.Id, IsLegacy = true, IsLeaf = true, LegacyCode = "L3103" }, _accountService);
                Account KEWAJIBANJANGKAPANJANG2 = _accountService.CreateObject(new Account() { Code = "22", Name = "KEWAJIBAN JANGKA PANJANG (KWJP)", Group = 3, Level = 2, ParentId = PASSIVA1.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account KWJPHUTANGBANK3 = _accountService.CreateObject(new Account() { Code = "2201", Name = "KWJP HUTANG BANK", Group = 3, Level = 3, ParentId = KEWAJIBANJANGKAPANJANG2.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account KWJPHUTANGBANK4 = _accountService.CreateObject(new Account() { Code = "220101", Name = "KWJP HUTANG BANK", Group = 3, Level = 4, ParentId = KWJPHUTANGBANK3.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account KWJPHUTANGBANK5 = _accountService.CreateObject(new Account() { Code = "22010001", Name = "KWJP HUTANG BANK", Group = 3, Level = 5, ParentId = KWJPHUTANGBANK4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "X2302001" }, _accountService);
                Account KWJPHUTANGLEASING5 = _accountService.CreateObject(new Account() { Code = "22010002", Name = "HUTANG LEASING", Group = 3, Level = 5, ParentId = KWJPHUTANGBANK4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account HUTANGJKPANJANGLAINNYA3 = _accountService.CreateObject(new Account() { Code = "2202", Name = "HUTANG JK. PANJANG LAINNYA", Group = 3, Level = 3, ParentId = KEWAJIBANJANGKAPANJANG2.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account HUTANGJKPANJANGLAINNYA4 = _accountService.CreateObject(new Account() { Code = "220201", Name = "HUTANG JK. PANJANG LAINNYA", Group = 3, Level = 4, ParentId = HUTANGJKPANJANGLAINNYA3.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account LABASALELEASEYGDITAGIHKAN5 = _accountService.CreateObject(new Account() { Code = "22020002", Name = "LABA SALE & LEASE YG DITAGIHKAN", Group = 3, Level = 5, ParentId = HUTANGJKPANJANGLAINNYA4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account PAJAKPENGHASILAN2 = _accountService.CreateObject(new Account() { Code = "91", Name = "PAJAK PENGHASILAN ", Group = 3, Level = 2, ParentId = PASSIVA1.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account PAJAKPENGHASILAN3 = _accountService.CreateObject(new Account() { Code = "9101", Name = "PAJAK PENGHASILAN ", Group = 3, Level = 3, ParentId = PAJAKPENGHASILAN2.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account PAJAKPENGHASILAN4 = _accountService.CreateObject(new Account() { Code = "910101", Name = "PAJAK PENGHASILAN ", Group = 3, Level = 4, ParentId = PAJAKPENGHASILAN3.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account PAJAKPENGHASILAN5 = _accountService.CreateObject(new Account() { Code = "91010001", Name = "PAJAK PENGHASILAN ", Group = 3, Level = 5, ParentId = PAJAKPENGHASILAN4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account PPHBADAN3 = _accountService.CreateObject(new Account() { Code = "9102", Name = "PPH BADAN", Group = 3, Level = 3, ParentId = PAJAKPENGHASILAN2.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account PPHBADAN4 = _accountService.CreateObject(new Account() { Code = "910201", Name = "PPH BADAN", Group = 3, Level = 4, ParentId = PPHBADAN3.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "X2404" }, _accountService);
                Account BIAYAPAJAKTANGGUHAN5 = _accountService.CreateObject(new Account() { Code = "91020001", Name = "BIAYA PAJAK TANGGUHAN", Group = 3, Level = 5, ParentId = PPHBADAN4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account MODAL1 = _accountService.CreateObject(new Account() { Code = "4", Name = "MODAL", Group = 4, Level = 1, ParentId = null, IsLegacy = true, IsLeaf = false, LegacyCode = "E4" }, _accountService);
                Account MODAL2 = _accountService.CreateObject(new Account() { Code = "31", Name = "MODAL", Group = 4, Level = 2, ParentId = MODAL1.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account MODALDISETOR3 = _accountService.CreateObject(new Account() { Code = "3101", Name = "MODAL DISETOR", Group = 4, Level = 3, ParentId = MODAL2.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account MODALDISETOR4 = _accountService.CreateObject(new Account() { Code = "310101", Name = "MODAL DISETOR", Group = 4, Level = 4, ParentId = MODALDISETOR3.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account MODALDISETOR5 = _accountService.CreateObject(new Account() { Code = "31010001", Name = "MODAL DISETOR", Group = 4, Level = 5, ParentId = MODALDISETOR4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account LABARUGIDITAHAN3 = _accountService.CreateObject(new Account() { Code = "3102", Name = "LABA/RUGI DITAHAN", Group = 4, Level = 3, ParentId = MODAL2.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account LABARUGIDITAHAN4 = _accountService.CreateObject(new Account() { Code = "310201", Name = "LABA/RUGI DITAHAN", Group = 4, Level = 4, ParentId = LABARUGIDITAHAN3.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account LABARUGIDITAHAN5 = _accountService.CreateObject(new Account() { Code = "31020001", Name = "LABA/RUGI DITAHAN", Group = 4, Level = 5, ParentId = LABARUGIDITAHAN4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account LABARUGITAHUNBERJALAN3 = _accountService.CreateObject(new Account() { Code = "3103", Name = "LABA/RUGI TAHUN BERJALAN", Group = 4, Level = 3, ParentId = MODAL2.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account LABARUGITAHUNBERJALAN4 = _accountService.CreateObject(new Account() { Code = "310301", Name = "LABA/RUGI TAHUN BERJALAN", Group = 4, Level = 4, ParentId = LABARUGITAHUNBERJALAN3.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account LABARUGITAHUNBERJALAN5 = _accountService.CreateObject(new Account() { Code = "31030001", Name = "LABA/RUGI TAHUN BERJALAN", Group = 4, Level = 5, ParentId = LABARUGITAHUNBERJALAN4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account LABARUGIBULANBERJALAN3 = _accountService.CreateObject(new Account() { Code = "3104", Name = "LABA/RUGI BULAN BERJALAN", Group = 4, Level = 3, ParentId = MODAL2.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account LABARUGIBULANBERJALAN4 = _accountService.CreateObject(new Account() { Code = "310401", Name = "LABA/RUGI BULAN BERJALAN", Group = 4, Level = 4, ParentId = LABARUGIBULANBERJALAN3.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account LABARUGIBULANBERJALAN5 = _accountService.CreateObject(new Account() { Code = "31040001", Name = "LABA/RUGI BULAN BERJALAN", Group = 4, Level = 5, ParentId = LABARUGIBULANBERJALAN4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account DIVIDEN3 = _accountService.CreateObject(new Account() { Code = "3105", Name = "DIVIDEN", Group = 4, Level = 3, ParentId = MODAL2.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account DIVIDEN4 = _accountService.CreateObject(new Account() { Code = "310501", Name = "DIVIDEN", Group = 4, Level = 4, ParentId = DIVIDEN3.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account DIVIDEN5 = _accountService.CreateObject(new Account() { Code = "31050001", Name = "DIVIDEN", Group = 4, Level = 5, ParentId = DIVIDEN4.Id, IsLegacy = true, IsLeaf = true, LegacyCode = "X2405" }, _accountService);
                Account KOREKSILRDITAHAN3 = _accountService.CreateObject(new Account() { Code = "3106", Name = "KOREKSI L/R DITAHAN", Group = 4, Level = 3, ParentId = MODAL2.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account KOREKSILRDITAHAN4 = _accountService.CreateObject(new Account() { Code = "310601", Name = "KOREKSI L/R DITAHAN", Group = 4, Level = 4, ParentId = KOREKSILRDITAHAN3.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account KOREKSILRDITAHAN5 = _accountService.CreateObject(new Account() { Code = "31060001", Name = "KOREKSI L/R DITAHAN", Group = 4, Level = 5, ParentId = KOREKSILRDITAHAN4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account KOREKSILRTAHUNBERJALAN3 = _accountService.CreateObject(new Account() { Code = "3107", Name = "KOREKSI L/R TAHUN BERJALAN", Group = 4, Level = 3, ParentId = MODAL2.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account KOREKSILRTAHUNBERJALAN4 = _accountService.CreateObject(new Account() { Code = "310701", Name = "KOREKSI L/R TAHUN BERJALAN", Group = 4, Level = 4, ParentId = KOREKSILRTAHUNBERJALAN3.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account KOREKSILRTAHUNBERJALAN5 = _accountService.CreateObject(new Account() { Code = "31070001", Name = "KOREKSI L/R TAHUN BERJALAN", Group = 4, Level = 5, ParentId = KOREKSILRTAHUNBERJALAN4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account KOREKSILRBULANBERJALAN3 = _accountService.CreateObject(new Account() { Code = "3108", Name = "KOREKSI L/R BULAN BERJALAN", Group = 4, Level = 3, ParentId = MODAL2.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account KOREKSILRBULANBERJALAN4 = _accountService.CreateObject(new Account() { Code = "310801", Name = "KOREKSI L/R BULAN BERJALAN", Group = 4, Level = 4, ParentId = KOREKSILRBULANBERJALAN3.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account KOREKSILRBULANBERJALAN5 = _accountService.CreateObject(new Account() { Code = "31080001", Name = "KOREKSI L/R BULAN BERJALAN", Group = 4, Level = 5, ParentId = KOREKSILRBULANBERJALAN4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account PENYESUAIANMODAL3 = _accountService.CreateObject(new Account() { Code = "3109", Name = "PENYESUAIAN MODAL", Group = 4, Level = 3, ParentId = MODAL2.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account PENYESUAIANMODAL4 = _accountService.CreateObject(new Account() { Code = "310901", Name = "PENYESUAIAN MODAL", Group = 4, Level = 4, ParentId = PENYESUAIANMODAL3.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account PENYESUAIANMODAL5 = _accountService.CreateObject(new Account() { Code = "31090001", Name = "PENYESUAIAN MODAL", Group = 4, Level = 5, ParentId = PENYESUAIANMODAL4.Id, IsLegacy = true, IsLeaf = true, LegacyCode = "E4101" }, _accountService);
                Account PENDAPATAN1 = _accountService.CreateObject(new Account() { Code = "5", Name = "PENDAPATAN", Group = 5, Level = 1, ParentId = null, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account PENDAPATAN2 = _accountService.CreateObject(new Account() { Code = "41", Name = "PENDAPATAN", Group = 5, Level = 2, ParentId = PENDAPATAN1.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account PENDAPATANPENJUALAN3 = _accountService.CreateObject(new Account() { Code = "4101", Name = "PENDAPATAN PENJUALAN ", Group = 5, Level = 3, ParentId = PENDAPATAN2.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account PENDAPATANPENJUALAN4 = _accountService.CreateObject(new Account() { Code = "410101", Name = "PENDAPATAN PENJUALAN ", Group = 5, Level = 4, ParentId = PENDAPATANPENJUALAN3.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account PENDAPATANPENJUALAN5 = _accountService.CreateObject(new Account() { Code = "411010001", Name = "PENDAPATAN PENJUALAN ", Group = 5, Level = 5, ParentId = PENDAPATANPENJUALAN4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "R5" }, _accountService);
                Account POTONGANPENJUALAN3 = _accountService.CreateObject(new Account() { Code = "4102", Name = "POTONGAN PENJUALAN", Group = 5, Level = 3, ParentId = PENDAPATAN2.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account POTONGANPENJUALAN4 = _accountService.CreateObject(new Account() { Code = "410201", Name = "POTONGAN PENJUALAN", Group = 5, Level = 4, ParentId = POTONGANPENJUALAN3.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account POTONGANPENJUALAN5 = _accountService.CreateObject(new Account() { Code = "41020001", Name = "POTONGAN PENJUALAN ", Group = 5, Level = 5, ParentId = POTONGANPENJUALAN4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account RETURPENJUALAN3 = _accountService.CreateObject(new Account() { Code = "4103", Name = "RETUR PENJUALAN ", Group = 5, Level = 3, ParentId = PENDAPATAN2.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account RETURPENJUALAN4 = _accountService.CreateObject(new Account() { Code = "410301", Name = "RETUR PENJUALAN ", Group = 5, Level = 4, ParentId = RETURPENJUALAN3.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account RETURPENJUALAN5 = _accountService.CreateObject(new Account() { Code = "41030001", Name = "RETUR PENJUALAN ", Group = 5, Level = 5, ParentId = RETURPENJUALAN4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account HARGAPOKOK2 = _accountService.CreateObject(new Account() { Code = "51", Name = "HARGA POKOK", Group = 2, Level = 2, ParentId = BEBANUSAHA1.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account HARGAPOKOKPENJUALAN3 = _accountService.CreateObject(new Account() { Code = "5101", Name = "HARGA POKOK PENJUALAN", Group = 2, Level = 3, ParentId = HARGAPOKOK2.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account HARGAPOKOKPENJUALAN4 = _accountService.CreateObject(new Account() { Code = "510101", Name = "HARGA POKOK PENJUALAN", Group = 2, Level = 4, ParentId = HARGAPOKOKPENJUALAN3.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account HARGAPOKOKPENJUALAN5 = _accountService.CreateObject(new Account() { Code = "51010001", Name = "HARGA POKOK PENJUALAN", Group = 2, Level = 5, ParentId = HARGAPOKOKPENJUALAN4.Id, IsLegacy = true, IsLeaf = true, LegacyCode = "X21" }, _accountService);
                Account POTONGANPEMBELIAN5 = _accountService.CreateObject(new Account() { Code = "51010002", Name = "POTONGAN PEMBELIAN", Group = 2, Level = 5, ParentId = HARGAPOKOKPENJUALAN4.Id, IsLegacy = true, IsLeaf = true, LegacyCode = "X2302002" }, _accountService);
                Account BIAYAOVERHEADPABRIK3 = _accountService.CreateObject(new Account() { Code = "5102", Name = "BIAYA OVERHEAD PABRIK", Group = 2, Level = 3, ParentId = HARGAPOKOK2.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account BIAYAOVERHEADPABRIK4 = _accountService.CreateObject(new Account() { Code = "510201", Name = "BIAYA OVERHEAD PABRIK", Group = 2, Level = 4, ParentId = BIAYAOVERHEADPABRIK3.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account BIAYAOVERHEADPABRIK5 = _accountService.CreateObject(new Account() { Code = "51020001", Name = "BIAYA OVERHEAD PABRIK", Group = 2, Level = 5, ParentId = BIAYAOVERHEADPABRIK4.Id, IsLegacy = true, IsLeaf = true, LegacyCode = "X2301" }, _accountService);
                Account PENDAPATANLAINLAIN2 = _accountService.CreateObject(new Account() { Code = "71", Name = "PENDAPATAN LAIN-LAIN", Group = 5, Level = 2, ParentId = PENDAPATAN1.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account PENDAPATANBUNGA3 = _accountService.CreateObject(new Account() { Code = "7101", Name = "PENDAPATAN BUNGA", Group = 5, Level = 3, ParentId = PENDAPATANLAINLAIN2.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account PENDAPATANBUNGA4 = _accountService.CreateObject(new Account() { Code = "710101", Name = "PENDAPATAN BUNGA", Group = 5, Level = 4, ParentId = PENDAPATANBUNGA3.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account PENDAPATANJASAGIRO5 = _accountService.CreateObject(new Account() { Code = "71010001", Name = "PENDAPATAN JASA GIRO", Group = 5, Level = 5, ParentId = PENDAPATANBUNGA4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account PENDAPATANBUNGADEPOSITO5 = _accountService.CreateObject(new Account() { Code = "71010002", Name = "PENDAPATAN BUNGA DEPOSITO", Group = 5, Level = 5, ParentId = PENDAPATANBUNGA4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account PENDAPATANLAINNYA3 = _accountService.CreateObject(new Account() { Code = "7102", Name = "PENDAPATAN LAINNYA", Group = 5, Level = 3, ParentId = PENDAPATANLAINLAIN2.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account PENDAPATANLAINNYA4 = _accountService.CreateObject(new Account() { Code = "710201", Name = "PENDAPATAN LAINNYA", Group = 5, Level = 4, ParentId = PENDAPATANLAINNYA3.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account PENDAPATANSELISIHKURS5 = _accountService.CreateObject(new Account() { Code = "71020001", Name = "PENDAPATAN SELISIH KURS", Group = 5, Level = 5, ParentId = PENDAPATANLAINNYA4.Id, IsLegacy = true, IsLeaf = true, LegacyCode = "E42" }, _accountService);
                Account PENDAPATANLAINNYA5 = _accountService.CreateObject(new Account() { Code = "71020002", Name = "PENDAPATAN LAINNYA", Group = 5, Level = 5, ParentId = PENDAPATANLAINNYA4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account PENDAPATANNONOPERASIONAL3 = _accountService.CreateObject(new Account() { Code = "7103", Name = "PENDAPATAN NON OPERASIONAL", Group = 5, Level = 3, ParentId = PENDAPATANLAINLAIN2.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account PENDAPATANNONOPERASIONAL4 = _accountService.CreateObject(new Account() { Code = "710301", Name = "PENDAPATAN NON OPERASIONAL (POP)", Group = 5, Level = 4, ParentId = PENDAPATANNONOPERASIONAL3.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account LABAPENJUALANAKTIVATETAP5POP = _accountService.CreateObject(new Account() { Code = "71030001", Name = "LABA PENJUALAN AKTIVA TETAP (POP)", Group = 5, Level = 5, ParentId = PENDAPATANNONOPERASIONAL4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account LABASALELEASEBACK5 = _accountService.CreateObject(new Account() { Code = "71030002", Name = "LABA SALE & LEASE BACK", Group = 5, Level = 5, ParentId = PENDAPATANNONOPERASIONAL4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
            }

            if (!_currencyService.GetAll().Any())
            {
                currencyIDR = new Currency();
                currencyIDR.IsBase = true;
                currencyIDR.Name = "IDR";
                currencyIDR = _currencyService.CreateObject(currencyIDR, _accountService);
            }
        }

        public void InvoiceMigration()
        {
            Contact customer = _contactService.GetObjectByName("ACEH MEDIA GRAFIKA");
            Currency USD = _currencyService.GetObjectByName("USD");
            Currency IDR = _currencyService.GetObjectByName("Rupiah");

            // For Payable and Receivable
            SalesInvoiceMigration salesInvoiceMigration = new SalesInvoiceMigration()
            {
                NomorSurat = "002.14.87584022",
                CurrencyId = USD.Id,
                AmountReceivable = (decimal) 1451.2,
                DPP = (decimal) 1451.2,
                Tax = (decimal) 0,
                Rate = (decimal) 12148,
                ContactId = customer.Id,
                InvoiceDate = new DateTime(2014, 12, 1),
            };
            salesInvoiceMigration = _salesInvoiceMigrationService.CreateObject(salesInvoiceMigration, _generalLedgerJournalService, _accountService, _gLNonBaseCurrencyService, _currencyService, _receivableService);

            PurchaseInvoiceMigration purchaseInvoiceMigration = new PurchaseInvoiceMigration()
            {
                NomorSurat = "6531",
                CurrencyId = IDR.Id,
                AmountPayable = (decimal) 1788209,
                DPP = (decimal) 1625645,
                Tax = (decimal) 162564,
                InvoiceDate = new DateTime(2014, 12, 1),
                ContactId = customer.Id,
                Rate = 1,
            };
            purchaseInvoiceMigration = _purchaseInvoiceMigrationService.CreateObject(purchaseInvoiceMigration, _generalLedgerJournalService, _accountService, _gLNonBaseCurrencyService, _currencyService, _payableService);
        }

        public void PopulateData()
        {
            PopulateUserRole();
            PopulateWarehouse();
            PopulateItem();
            PopulateSingles();
            PopulateBuilders();
            PopulateBlanket();
            PopulateBlendingRecipe();
            PopulateWarehouseMutationForRollerIdentificationAndRecovery();
            PopulateCoreIdentifications();
            PopulateRecoveryOrders();
            PopulateRecoveryOrders2();
            PopulateStockAdjustment();
            PopulateCustomerStockAdjustment();
            PopulateRecoveryOrders3();
            PopulateCoreIdentifications2();
            PopulateRollerWarehouseMutation();
            PopulateBlanketOrders();
            PopulateBlendingWorkOrders();

            // @SalesBuilder
            PopulateSalesAndDelivery();
            PopulateSalesInvoice();
            PopulateReceiptVoucher();

            // @PurchaseBuilder
            PopulatePurchaseOrderAndPurchaseReceival();
            PopulatePurchaseInvoice();
            PopulatePaymentVoucher();

            PopulateCashBank();
            PopulateSales();
            PopulateVirtualOrder();

            PopulateValidComb();
        }

        public void PopulateDataNoClosing()
        {
            PopulateUserRole();
            PopulateWarehouse();
            PopulateItem();
            PopulateSingles();
            PopulateBuilders();
            PopulateBlanket();
            PopulateBlendingRecipe();
            PopulateWarehouseMutationForRollerIdentificationAndRecovery();
            PopulateCoreIdentifications();
            PopulateRecoveryOrders();
            PopulateRecoveryOrders2();
            PopulateStockAdjustment();
            PopulateCustomerStockAdjustment();
            PopulateRecoveryOrders3();
            PopulateCoreIdentifications2();
            PopulateRollerWarehouseMutation();
            PopulateBlanketOrders();
            PopulateBlendingWorkOrders();

            // @SalesBuilder
            PopulateSalesAndDelivery();
            PopulateSalesInvoice();
            PopulateReceiptVoucher();

            // @PurchaseBuilder
            PopulatePurchaseOrderAndPurchaseReceival();
            PopulatePurchaseInvoice();
            PopulatePaymentVoucher();

            PopulateCashBank();
            PopulateSales();
            PopulateVirtualOrder();
            //PopulateValidComb();
        }

        public void PopulateUserRole()
        {
            user = new UserAccount()
            {
                Username = "admin",
                Password = "123",
                Name = "admin",
                Description = "admin palsu",
            };
            user = _userAccountService.CreateObject(user);

            menudata = _userMenuService.CreateObject(Core.Constants.Constant.MenuName.Contact, Core.Constants.Constant.MenuGroupName.Master);
            menufinance = _userMenuService.CreateObject(Core.Constants.Constant.MenuName.ItemType, Core.Constants.Constant.MenuGroupName.Master);

            admindata = new UserAccess()
            {
                UserAccountId = admin.Id,
                UserMenuId = menudata.Id,
                AllowView = true,
                AllowCreate = true,
                AllowEdit = true,
                AllowConfirm = true,
                AllowUnconfirm = true,
                AllowPaid = true,
                AllowUnpaid = true,
                AllowReconcile = true,
                AllowUnreconcile = true,
                AllowPrint = true,
                AllowDelete = true,
                AllowUndelete = true
            };
            admindata = _userAccessService.CreateObject(admindata, _userAccountService, _userMenuService);

            adminfinance = new UserAccess()
            {
                UserAccountId = admin.Id,
                UserMenuId = menufinance.Id,
                AllowView = true,
                AllowCreate = true,
                AllowEdit = true,
                AllowConfirm = true,
                AllowUnconfirm = true,
                AllowPaid = true,
                AllowUnpaid = true,
                AllowReconcile = true,
                AllowUnreconcile = true,
                AllowPrint = true,
                AllowDelete = true,
                AllowUndelete = true
            };
            adminfinance = _userAccessService.CreateObject(adminfinance, _userAccountService, _userMenuService);
        }

        public void PopulateWarehouse()
        {
            localWarehouse = new Warehouse()
            {
                Name = "Sentral Solusi Data",
                Description = "Kali Besar Jakarta",
                Code = "LCL"
            };
            localWarehouse = _warehouseService.CreateObject(localWarehouse, _warehouseItemService, _itemService);

            movingWarehouse = new Warehouse()
            {
                Name = "Kepala Manufacturing Roland",
                Description = "Mengerjakan Roller dan Blanket",
                Code = "MVG"
            };
            movingWarehouse = _warehouseService.CreateObject(movingWarehouse, _warehouseItemService, _itemService);
        }

        public void PopulateItem()
        {
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

            Bottles = new UoM()
            {
                Name = "Bottles"
            };
            _uomService.CreateObject(Bottles);

            itemBlending = new Item()
            {
                ItemTypeId = _itemTypeService.GetObjectByName("Chemical").Id,
                Name = "Kimia XYZ volume 1 L",
                Sku = "CBI123",
                UoMId = Bottles.Id,
                SellingPrice = 10000,
            };
            _itemService.CreateObject(itemBlending, _uomService, _itemTypeService, _warehouseItemService, _warehouseService, _priceMutationService);

            itemBlendingDet1 = new Item()
            {
                ItemTypeId = _itemTypeService.GetObjectByName("Chemical").Id,
                Name = "Drum Oli volume 5 L",
                Sku = "CBI124",
                UoMId = Bottles.Id,
                SellingPrice = 4000,
            };
            _itemService.CreateObject(itemBlendingDet1, _uomService, _itemTypeService, _warehouseItemService, _warehouseService, _priceMutationService);

            itemBlendingDet2 = new Item()
            {
                ItemTypeId = _itemTypeService.GetObjectByName("Chemical").Id,
                Name = "Botol kosong volume 1L",
                Sku = "CBI125",
                UoMId = Bottles.Id,
                SellingPrice = 2000,
            };
            _itemService.CreateObject(itemBlendingDet2, _uomService, _itemTypeService, _warehouseItemService, _warehouseService, _priceMutationService);

            itemBlendingDet3 = new Item()
            {
                ItemTypeId = _itemTypeService.GetObjectByName("Chemical").Id,
                Name = "Drum Pewarna 5L",
                Sku = "CBI126",
                UoMId = Bottles.Id,
                SellingPrice = 4000,
            };
            _itemService.CreateObject(itemBlendingDet3, _uomService, _itemTypeService, _warehouseItemService, _warehouseService, _priceMutationService);

            itemAdhesiveBlanket = new Item()
            {
                ItemTypeId = _itemTypeService.GetObjectByName("AdhesiveBlanket").Id,
                Name = "Adhesive Blanket",
                Sku = "ADB123",
                UoMId = Tubs.Id
            };
            itemAdhesiveBlanket = _itemService.CreateObject(itemAdhesiveBlanket, _uomService, _itemTypeService, _warehouseItemService, _warehouseService, _priceMutationService);

            itemAdhesiveRoller = new Item()
            {
                ItemTypeId = _itemTypeService.GetObjectByName("AdhesiveRoller").Id,
                Name = "Adhesive Default",
                Sku = "ADR123",
                UoMId = Tubs.Id
            };
            itemAdhesiveRoller = _itemService.CreateObject(itemAdhesiveRoller, _uomService, _itemTypeService, _warehouseItemService, _warehouseService, _priceMutationService);

            itemCompound = new Item()
            {
                ItemTypeId = _itemTypeService.GetObjectByName("Compound").Id,
                Name = "Compound RB else",
                Sku = "CMP123",
                UoMId = Tubs.Id,
                MinimumQuantity = 150000
            };
            itemCompound = _itemService.CreateObject(itemCompound, _uomService, _itemTypeService, _warehouseItemService, _warehouseService, _priceMutationService);

            itemCompound1 = new Item()
            {
                ItemTypeId = _itemTypeService.GetObjectByName("Compound").Id,
                Name = "Compound RB1",
                Sku = "CMP101",
                UoMId = _uomService.GetObjectByName("Tubs").Id
            };
            itemCompound1 = _itemService.CreateObject(itemCompound1, _uomService, _itemTypeService, _warehouseItemService, _warehouseService, _priceMutationService);

            itemCompound2 = new Item()
            {
                ItemTypeId = _itemTypeService.GetObjectByName("Compound").Id,
                Name = "Compound RB2",
                Sku = "CMP102",
                UoMId = Tubs.Id
            };
            itemCompound2 = _itemService.CreateObject(itemCompound2, _uomService, _itemTypeService, _warehouseItemService, _warehouseService, _priceMutationService);

            itemAccessory1 = new Item()
            {
                ItemTypeId = _itemTypeService.GetObjectByName("Accessory").Id,
                Name = "Accessory Sample 1",
                Sku = "ACC001",
                UoMId = Pcs.Id
            };
            itemAccessory1 = _itemService.CreateObject(itemAccessory1, _uomService, _itemTypeService, _warehouseItemService, _warehouseService, _priceMutationService);

            itemAccessory2 = new Item()
            {
                ItemTypeId = _itemTypeService.GetObjectByName("Accessory").Id,
                Name = "Accessory Sample 2",
                Sku = "ACC002",
                UoMId = Pcs.Id
            };
            itemAccessory2 = _itemService.CreateObject(itemAccessory2, _uomService, _itemTypeService, _warehouseItemService, _warehouseService, _priceMutationService);

            StockAdjustment sa = new StockAdjustment()
            {
                AdjustmentDate = DateTime.Today,
                Description = "Compound And Accessories Adjustment",
                WarehouseId = localWarehouse.Id
            };
            _stockAdjustmentService.CreateObject(sa, _warehouseService);

            sadBlendingItem1 = new StockAdjustmentDetail()
            {
                StockAdjustmentId = sa.Id,
                Quantity = 100,
                ItemId = itemBlending.Id,
                Code = "IACBR001",
                Price = 5000
            };
            _stockAdjustmentDetailService.CreateObject(sadBlendingItem1, _stockAdjustmentService, _itemService, _warehouseItemService);

            sadBlendingItem2 = new StockAdjustmentDetail()
            {
                StockAdjustmentId = sa.Id,
                Quantity = 100,
                ItemId = itemBlendingDet1.Id,
                Code = "IACBR002",
                Price = 2000
            };
            _stockAdjustmentDetailService.CreateObject(sadBlendingItem2, _stockAdjustmentService, _itemService, _warehouseItemService);

            sadBlendingItem3 = new StockAdjustmentDetail()
            {
                StockAdjustmentId = sa.Id,
                Quantity = 100,
                ItemId = itemBlendingDet2.Id,
                Code = "IACBR003",
                Price = 1000
            };
            _stockAdjustmentDetailService.CreateObject(sadBlendingItem3, _stockAdjustmentService, _itemService, _warehouseItemService);

            sadBlendingItem4 = new StockAdjustmentDetail()
            {
                StockAdjustmentId = sa.Id,
                Quantity = 100,
                ItemId = itemBlendingDet3.Id,
                Code = "IACBR004",
                Price = 2000
            };
            _stockAdjustmentDetailService.CreateObject(sadBlendingItem4, _stockAdjustmentService, _itemService, _warehouseItemService);

            sadAdhesiveBlanket = new StockAdjustmentDetail()
            {
                StockAdjustmentId = sa.Id,
                Quantity = 100,
                ItemId = itemAdhesiveBlanket.Id,
                Code = "IADB001",
                Price = 3000
            };
            _stockAdjustmentDetailService.CreateObject(sadAdhesiveBlanket, _stockAdjustmentService, _itemService, _warehouseItemService);

            sadAdhesiveRoller = new StockAdjustmentDetail()
            {
                StockAdjustmentId = sa.Id,
                Quantity = 100,
                ItemId = itemAdhesiveRoller.Id,
                Code = "IADR001",
                Price = 3000
            };
            _stockAdjustmentDetailService.CreateObject(sadAdhesiveRoller, _stockAdjustmentService, _itemService, _warehouseItemService);

            sad1 = new StockAdjustmentDetail()
            {
                StockAdjustmentId = sa.Id,
                ItemId = itemCompound.Id,
                Quantity = 200000,
                Code = "ITCM000",
                Price = 50000
            };
            _stockAdjustmentDetailService.CreateObject(sad1, _stockAdjustmentService, _itemService, _warehouseItemService);

            sad2 = new StockAdjustmentDetail()
            {
                StockAdjustmentId = sa.Id,
                ItemId = itemCompound1.Id,
                Quantity = 200000,
                Code = "ITCM001",
                Price = 50000
            };
            _stockAdjustmentDetailService.CreateObject(sad2, _stockAdjustmentService, _itemService, _warehouseItemService);

            sad3 = new StockAdjustmentDetail()
            {
                StockAdjustmentId = sa.Id,
                ItemId = itemCompound2.Id,
                Quantity = 200000,
                Code = "ITCM002",
                Price = 50000
            };
            _stockAdjustmentDetailService.CreateObject(sad3, _stockAdjustmentService, _itemService, _warehouseItemService);

            sad4 = new StockAdjustmentDetail()
            {
                StockAdjustmentId = sa.Id,
                ItemId = itemAccessory1.Id,
                Quantity = 150,
                Code = "ITAC001",
                Price = 50000
            };
            _stockAdjustmentDetailService.CreateObject(sad4, _stockAdjustmentService, _itemService, _warehouseItemService);

            sad5 = new StockAdjustmentDetail()
            {
                StockAdjustmentId = sa.Id,
                ItemId = itemAccessory2.Id,
                Quantity = 100,
                Code = "ITAC002",
                Price = 50000
            };
            _stockAdjustmentDetailService.CreateObject(sad5, _stockAdjustmentService, _itemService, _warehouseItemService);
 
            _stockAdjustmentService.ConfirmObject(sa, DateTime.Today, _stockAdjustmentDetailService, _stockMutationService, _itemService, _blanketService, _warehouseItemService,
                                                  _accountService, _generalLedgerJournalService, _closingService);
        }

        public void PopulateSingles()
        {
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

            machine = new Machine()
            {
                Code = "MX0001",
                Name = "Printing Machine",
                Description = "Generic"
            };
            machine = _machineService.CreateObject(machine);

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
                ExRateDate = DateTime.Today.AddDays(2),
                Rate = 15100
            };
            DayMinusTwoRateEUR = _exchangeRateService.CreateObject(DayMinusTwoRateEUR);

            DayMinusOneRateEUR = new ExchangeRate()
            {
                CurrencyId = currencyEUR.Id,
                ExRateDate = DateTime.Today.AddDays(2),
                Rate = 15050
            };
            DayMinusOneRateEUR = _exchangeRateService.CreateObject(DayMinusOneRateEUR);

            DayRateEUR = new ExchangeRate()
            {
                CurrencyId = currencyEUR.Id,
                ExRateDate = DateTime.Today.AddDays(2),
                Rate = 15000
            };
            DayRateEUR = _exchangeRateService.CreateObject(DayRateEUR);

            DayMinusTwoRateUSD = new ExchangeRate()
            {
                CurrencyId = currencyUSD.Id,
                ExRateDate = DateTime.Today.AddDays(2),
                Rate = 12100
            };
            DayMinusTwoRateUSD = _exchangeRateService.CreateObject(DayMinusTwoRateUSD);

            DayMinusOneRateUSD = new ExchangeRate()
            {
                CurrencyId = currencyUSD.Id,
                ExRateDate = DateTime.Today.AddDays(2),
                Rate = 12150
            };
            DayMinusOneRateUSD = _exchangeRateService.CreateObject(DayMinusOneRateUSD);

            DayRateUSD = new ExchangeRate()
            {
                CurrencyId = currencyUSD.Id,
                ExRateDate = DateTime.Today.AddDays(2),
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

            pettyCash = new CashBank()
            {
                Name = "Petty Cash",
                Description = "Uang kas sementara",
                IsBank = false,
                CurrencyId = currencyIDR.Id
            };
            _cashBankService.CreateObject(pettyCash, _accountService, _currencyService);

            cashBank1 = new CashBank()
            {
                Name = "Kontan",
                IsBank = false,
                Description = "Kontan",
                CurrencyId = currencyIDR.Id
            };
            _cashBankService.CreateObject(cashBank1, _accountService, _currencyService);

            cashBank2 = new CashBank()
            {
                Name = "Bank BCA",
                IsBank = true,
                Description = "Bank BCA",
                CurrencyId = currencyIDR.Id
            };
            _cashBankService.CreateObject(cashBank2, _accountService, _currencyService);

            cashBankAdjustment3 = new CashBankAdjustment()
            {
                CashBankId = cashBank.Id,
                Amount = 1000000000,
                AdjustmentDate = DateTime.Today
            };
            _cashBankAdjustmentService.CreateObject(cashBankAdjustment3, _cashBankService);
            _cashBankAdjustmentService.ConfirmObject(cashBankAdjustment3, DateTime.Now, _cashMutationService, _cashBankService,
                                                     _accountService, _generalLedgerJournalService, _closingService, _currencyService, _exchangeRateService, _gLNonBaseCurrencyService);
        }

        public void PopulateBuilders()
        {
            coreBuilder = new CoreBuilder()
            {
                BaseSku = "CBX",
                SkuNewCore = "CBXN",
                SkuUsedCore = "CBXU",
                Name = "Core X",
                Description = "X",
                UoMId = Pcs.Id,
                MachineId = machine.Id,
                CoreBuilderTypeCase = Core.Constants.Constant.CoreBuilderTypeCase.Hollow
            };
            coreBuilder = _coreBuilderService.CreateObject(coreBuilder, _uomService, _itemService, _itemTypeService, _warehouseItemService,
                                                           _warehouseService, _priceMutationService, _machineService);

            coreBuilder1 = new CoreBuilder()
            {
                BaseSku = "CBA001",
                SkuNewCore = "CB001N",
                SkuUsedCore = "CB001U",
                Name = "Core A 001",
                Description = "A",
                UoMId = Pcs.Id,
                MachineId = machine.Id,
                CoreBuilderTypeCase = Core.Constants.Constant.CoreBuilderTypeCase.Hollow
            };
            coreBuilder1 = _coreBuilderService.CreateObject(coreBuilder1, _uomService, _itemService, _itemTypeService, _warehouseItemService,
                                                            _warehouseService, _priceMutationService, _machineService);

            coreBuilder2 = new CoreBuilder()
            {
                BaseSku = "CBA002",
                SkuNewCore = "CB002N",
                SkuUsedCore = "CB002U",
                Name = "Core A 002",
                Description = "A",
                UoMId = Pcs.Id,
                MachineId = machine.Id,
                CoreBuilderTypeCase = Core.Constants.Constant.CoreBuilderTypeCase.Hollow
            };
            coreBuilder2 = _coreBuilderService.CreateObject(coreBuilder2, _uomService, _itemService, _itemTypeService, _warehouseItemService,
                                                            _warehouseService, _priceMutationService, _machineService);

            coreBuilder3 = new CoreBuilder()
            {
                BaseSku = "CBA003",
                SkuNewCore = "CB003N",
                SkuUsedCore = "CB003U",
                Name = "Core A 003",
                Description = "A",
                UoMId = Pcs.Id,
                MachineId = machine.Id,
                CoreBuilderTypeCase = Core.Constants.Constant.CoreBuilderTypeCase.Hollow
            };
            coreBuilder3 = _coreBuilderService.CreateObject(coreBuilder3, _uomService, _itemService, _itemTypeService, _warehouseItemService,
                                                            _warehouseService, _priceMutationService, _machineService);

            coreBuilder4 = new CoreBuilder()
            {
                BaseSku = "CBA004",
                SkuNewCore = "CB004N",
                SkuUsedCore = "CB004U",
                Name = "Core A 004",
                Description = "A",
                UoMId = Pcs.Id,
                MachineId = machine.Id,
                CoreBuilderTypeCase = Core.Constants.Constant.CoreBuilderTypeCase.Hollow
            };
            coreBuilder4 = _coreBuilderService.CreateObject(coreBuilder4, _uomService, _itemService, _itemTypeService, _warehouseItemService,
                                                            _warehouseService, _priceMutationService, _machineService);

            rollerBuilder = new RollerBuilder()
            {
                BaseSku = "RBX",
                SkuRollerNewCore = "RBXN",
                SkuRollerUsedCore = "RBXU",
                RD = 12,
                CD = 12,
                TL = 12,
                WL = 12,
                RL = 12,
                Name = "Roller X",
                Description = "X",
                CoreBuilderId = coreBuilder.Id,
                CompoundId = itemCompound.Id,
                MachineId = machine.Id,
                RollerTypeId = typeDamp.Id,
                UoMId = Pcs.Id,
                AdhesiveId = itemAdhesiveRoller.Id
            };
            rollerBuilder = _rollerBuilderService.CreateObject(rollerBuilder, _machineService, _uomService, _itemService, _itemTypeService,
                                                               _coreBuilderService, _rollerTypeService, _warehouseItemService, _warehouseService,
                                                               _priceMutationService);

            rollerBuilder1 = new RollerBuilder()
            {
                BaseSku = "RBA001",
                SkuRollerNewCore = "RBA001N",
                SkuRollerUsedCore = "RBA001U",
                RD = 11,
                CD = 11,
                TL = 11,
                WL = 11,
                RL = 11,
                Name = "Roller A001",
                Description = "A",
                CoreBuilderId = coreBuilder1.Id,
                CompoundId = itemCompound1.Id,
                MachineId = machine.Id,
                RollerTypeId = typeFoundDT.Id,
                UoMId = Pcs.Id,
                AdhesiveId = itemAdhesiveRoller.Id
            };
            rollerBuilder1 = _rollerBuilderService.CreateObject(rollerBuilder1, _machineService, _uomService, _itemService, _itemTypeService,
                                                                _coreBuilderService, _rollerTypeService, _warehouseItemService, _warehouseService,
                                                                _priceMutationService);

            rollerBuilder2 = new RollerBuilder()
            {
                BaseSku = "RBA002",
                SkuRollerNewCore = "RBA002N",
                SkuRollerUsedCore = "RBA002U",
                RD = 13,
                CD = 13,
                TL = 13,
                WL = 13,
                RL = 13,
                Name = "Roller A002",
                Description = "A",
                CoreBuilderId = coreBuilder2.Id,
                CompoundId = itemCompound2.Id,
                MachineId = machine.Id,
                RollerTypeId = typeDampFormDQ.Id,
                UoMId = Pcs.Id,
                AdhesiveId = itemAdhesiveRoller.Id
            };
            rollerBuilder2 = _rollerBuilderService.CreateObject(rollerBuilder2, _machineService, _uomService, _itemService, _itemTypeService,
                                                                _coreBuilderService, _rollerTypeService, _warehouseItemService, _warehouseService,
                                                                _priceMutationService);

            rollerBuilder3 = new RollerBuilder()
            {
                BaseSku = "RBA003",
                SkuRollerNewCore = "RBA003N",
                SkuRollerUsedCore = "RBA003U",
                RD = 13,
                CD = 13,
                TL = 13,
                WL = 13,
                RL = 13,
                Name = "Roller A003",
                Description = "A",
                CoreBuilderId = coreBuilder3.Id,
                CompoundId = itemCompound.Id,
                MachineId = machine.Id,
                RollerTypeId = typeInkDistD.Id,
                UoMId = Pcs.Id,
                AdhesiveId = itemAdhesiveRoller.Id
            };
            rollerBuilder3 = _rollerBuilderService.CreateObject(rollerBuilder3, _machineService, _uomService, _itemService, _itemTypeService,
                                                                _coreBuilderService, _rollerTypeService, _warehouseItemService, _warehouseService,
                                                                _priceMutationService);

            rollerBuilder4 = new RollerBuilder()
            {
                BaseSku = "RBA004",
                SkuRollerNewCore = "RBA004N",
                SkuRollerUsedCore = "RBA004U",
                RD = 14,
                CD = 14,
                TL = 14,
                WL = 14,
                RL = 14,
                Name = "Roller X",
                Description = "X",
                CoreBuilderId = coreBuilder4.Id,
                CompoundId = itemCompound.Id,
                MachineId = machine.Id,
                RollerTypeId = typeInkDistH.Id,
                UoMId = Pcs.Id,
                AdhesiveId = itemAdhesiveRoller.Id
            };
            rollerBuilder4 = _rollerBuilderService.CreateObject(rollerBuilder4, _machineService, _uomService, _itemService, _itemTypeService,
                                                                _coreBuilderService, _rollerTypeService, _warehouseItemService, _warehouseService,
                                                                _priceMutationService);

            Item NewCore = _coreBuilderService.GetNewCore(coreBuilder.Id);
            Item NewCore1 = _coreBuilderService.GetNewCore(coreBuilder1.Id);
            Item NewCore2 = _coreBuilderService.GetNewCore(coreBuilder2.Id);
            Item NewCore3 = _coreBuilderService.GetNewCore(coreBuilder3.Id);
            Item NewCore4 = _coreBuilderService.GetNewCore(coreBuilder4.Id);
            Item UsedCore = _coreBuilderService.GetUsedCore(coreBuilder.Id);
            Item UsedCore1 = _coreBuilderService.GetUsedCore(coreBuilder1.Id);
            Item UsedCore2 = _coreBuilderService.GetUsedCore(coreBuilder2.Id);
            Item UsedCore3 = _coreBuilderService.GetUsedCore(coreBuilder3.Id);
            Item UsedCore4 = _coreBuilderService.GetUsedCore(coreBuilder4.Id);
            Item RollerNewCore = _rollerBuilderService.GetRollerNewCore(rollerBuilder.Id);
            Item RollerNewCore1 = _rollerBuilderService.GetRollerNewCore(rollerBuilder1.Id);
            Item RollerNewCore2 = _rollerBuilderService.GetRollerNewCore(rollerBuilder2.Id);
            Item RollerNewCore3 = _rollerBuilderService.GetRollerNewCore(rollerBuilder3.Id);
            Item RollerNewCore4 = _rollerBuilderService.GetRollerNewCore(rollerBuilder4.Id);
            Item RollerUsedCore = _rollerBuilderService.GetRollerUsedCore(rollerBuilder.Id);
            Item RollerUsedCore1 = _rollerBuilderService.GetRollerUsedCore(rollerBuilder1.Id);
            Item RollerUsedCore2 = _rollerBuilderService.GetRollerUsedCore(rollerBuilder2.Id);
            Item RollerUsedCore3 = _rollerBuilderService.GetRollerUsedCore(rollerBuilder3.Id);
            Item RollerUsedCore4 = _rollerBuilderService.GetRollerUsedCore(rollerBuilder4.Id);

            StockAdjustment sa = new StockAdjustment()
            {
                AdjustmentDate = DateTime.Today,
                Description = "Core Adjustment",
                WarehouseId = localWarehouse.Id
            };
            _stockAdjustmentService.CreateObject(sa, _warehouseService);

            StockAdjustmentDetail sadNewCore = new StockAdjustmentDetail() { ItemId = NewCore.Id , Quantity = 7, StockAdjustmentId = sa.Id, Price = 50000};
            _stockAdjustmentDetailService.CreateObject(sadNewCore, _stockAdjustmentService, _itemService, _warehouseItemService);
            StockAdjustmentDetail sadNewCore1 = new StockAdjustmentDetail() { ItemId = NewCore1.Id, Quantity = 7, StockAdjustmentId = sa.Id, Price = 50000 };
            _stockAdjustmentDetailService.CreateObject(sadNewCore1, _stockAdjustmentService, _itemService, _warehouseItemService);
            StockAdjustmentDetail sadNewCore2 = new StockAdjustmentDetail() { ItemId = NewCore2.Id, Quantity = 7, StockAdjustmentId = sa.Id, Price = 50000 };
            _stockAdjustmentDetailService.CreateObject(sadNewCore2, _stockAdjustmentService, _itemService, _warehouseItemService);
            StockAdjustmentDetail sadNewCore3 = new StockAdjustmentDetail() { ItemId = NewCore3.Id, Quantity = 7, StockAdjustmentId = sa.Id, Price = 50000 };
            _stockAdjustmentDetailService.CreateObject(sadNewCore3, _stockAdjustmentService, _itemService, _warehouseItemService);
            StockAdjustmentDetail sadNewCore4 = new StockAdjustmentDetail() { ItemId = NewCore4.Id, Quantity = 7, StockAdjustmentId = sa.Id, Price = 50000 };
            _stockAdjustmentDetailService.CreateObject(sadNewCore4, _stockAdjustmentService, _itemService, _warehouseItemService);

            StockAdjustmentDetail sadUsedCore = new StockAdjustmentDetail() { ItemId = UsedCore.Id, Quantity = 7, StockAdjustmentId = sa.Id, Price = 50000 };
            _stockAdjustmentDetailService.CreateObject(sadUsedCore, _stockAdjustmentService, _itemService, _warehouseItemService);
            StockAdjustmentDetail sadUsedCore1 = new StockAdjustmentDetail() { ItemId = UsedCore1.Id, Quantity = 7, StockAdjustmentId = sa.Id, Price = 50000 };
            _stockAdjustmentDetailService.CreateObject(sadUsedCore1, _stockAdjustmentService, _itemService, _warehouseItemService);
            StockAdjustmentDetail sadUsedCore2 = new StockAdjustmentDetail() { ItemId = UsedCore2.Id, Quantity = 7, StockAdjustmentId = sa.Id, Price = 50000 };
            _stockAdjustmentDetailService.CreateObject(sadUsedCore2, _stockAdjustmentService, _itemService, _warehouseItemService);
            StockAdjustmentDetail sadUsedCore3 = new StockAdjustmentDetail() { ItemId = UsedCore3.Id, Quantity = 7, StockAdjustmentId = sa.Id, Price = 50000 };
            _stockAdjustmentDetailService.CreateObject(sadUsedCore3, _stockAdjustmentService, _itemService, _warehouseItemService);
            StockAdjustmentDetail sadUsedCore4 = new StockAdjustmentDetail() { ItemId = UsedCore4.Id, Quantity = 7, StockAdjustmentId = sa.Id, Price = 50000 };
            _stockAdjustmentDetailService.CreateObject(sadUsedCore4, _stockAdjustmentService, _itemService, _warehouseItemService);

            StockAdjustmentDetail sadRollerNewCore = new StockAdjustmentDetail() { ItemId = RollerNewCore.Id, Quantity = 7, StockAdjustmentId = sa.Id, Price = 50000 };
            _stockAdjustmentDetailService.CreateObject(sadRollerNewCore, _stockAdjustmentService, _itemService, _warehouseItemService);
            StockAdjustmentDetail sadRollerNewCore1 = new StockAdjustmentDetail() { ItemId = RollerNewCore1.Id, Quantity = 7, StockAdjustmentId = sa.Id, Price = 50000 };
            _stockAdjustmentDetailService.CreateObject(sadRollerNewCore1, _stockAdjustmentService, _itemService, _warehouseItemService);
            StockAdjustmentDetail sadRollerNewCore2 = new StockAdjustmentDetail() { ItemId = RollerNewCore2.Id, Quantity = 7, StockAdjustmentId = sa.Id, Price = 50000 };
            _stockAdjustmentDetailService.CreateObject(sadRollerNewCore2, _stockAdjustmentService, _itemService, _warehouseItemService);
            StockAdjustmentDetail sadRollerNewCore3 = new StockAdjustmentDetail() { ItemId = RollerNewCore3.Id, Quantity = 7, StockAdjustmentId = sa.Id, Price = 50000 };
            _stockAdjustmentDetailService.CreateObject(sadRollerNewCore3, _stockAdjustmentService, _itemService, _warehouseItemService);
            StockAdjustmentDetail sadRollerNewCore4 = new StockAdjustmentDetail() { ItemId = RollerNewCore4.Id, Quantity = 7, StockAdjustmentId = sa.Id, Price = 50000 };
            _stockAdjustmentDetailService.CreateObject(sadRollerNewCore4, _stockAdjustmentService, _itemService, _warehouseItemService);

            StockAdjustmentDetail sadRollerUsedCore = new StockAdjustmentDetail() { ItemId = RollerUsedCore.Id, Quantity = 7, StockAdjustmentId = sa.Id, Price = 50000 };
            _stockAdjustmentDetailService.CreateObject(sadRollerUsedCore, _stockAdjustmentService, _itemService, _warehouseItemService);
            StockAdjustmentDetail sadRollerUsedCore1 = new StockAdjustmentDetail() { ItemId = RollerUsedCore1.Id, Quantity = 7, StockAdjustmentId = sa.Id, Price = 50000 };
            _stockAdjustmentDetailService.CreateObject(sadRollerUsedCore1, _stockAdjustmentService, _itemService, _warehouseItemService);
            StockAdjustmentDetail sadRollerUsedCore2 = new StockAdjustmentDetail() { ItemId = RollerUsedCore2.Id, Quantity = 7, StockAdjustmentId = sa.Id, Price = 50000 };
            _stockAdjustmentDetailService.CreateObject(sadRollerUsedCore2, _stockAdjustmentService, _itemService, _warehouseItemService);
            StockAdjustmentDetail sadRollerUsedCore3 = new StockAdjustmentDetail() { ItemId = RollerUsedCore3.Id, Quantity = 7, StockAdjustmentId = sa.Id, Price = 50000 };
            _stockAdjustmentDetailService.CreateObject(sadRollerUsedCore3, _stockAdjustmentService, _itemService, _warehouseItemService);
            StockAdjustmentDetail sadRollerUsedCore4 = new StockAdjustmentDetail() { ItemId = RollerUsedCore4.Id, Quantity = 7, StockAdjustmentId = sa.Id, Price = 50000 };
            _stockAdjustmentDetailService.CreateObject(sadRollerUsedCore4, _stockAdjustmentService, _itemService, _warehouseItemService);

            _stockAdjustmentService.ConfirmObject(sa, DateTime.Today, _stockAdjustmentDetailService, _stockMutationService, _itemService, _blanketService, _warehouseItemService,
                                                  _accountService, _generalLedgerJournalService, _closingService);
        }

        public void PopulateWarehouseMutationForRollerIdentificationAndRecovery()
        {
            warehouseMutation = new WarehouseMutation()
            {
                WarehouseFromId = localWarehouse.Id,
                WarehouseToId = movingWarehouse.Id,
                MutationDate = DateTime.Now
            };
            warehouseMutation = _warehouseMutationService.CreateObject(warehouseMutation, _warehouseService);

            wmoDetail1 = new WarehouseMutationDetail()
            {
                WarehouseMutationId = warehouseMutation.Id,
                ItemId = coreBuilder1.UsedCoreItemId,
                Quantity = 2
            };
            wmoDetail1 = _warehouseMutationDetailService.CreateObject(wmoDetail1, _warehouseMutationService, _itemService, _warehouseItemService, _blanketService);

            wmoDetail2 = new WarehouseMutationDetail()
            {
                WarehouseMutationId = warehouseMutation.Id,
                ItemId = coreBuilder2.UsedCoreItemId,
                Quantity = 2
            };
            wmoDetail2 = _warehouseMutationDetailService.CreateObject(wmoDetail2, _warehouseMutationService, _itemService, _warehouseItemService, _blanketService);

            wmoDetail3 = new WarehouseMutationDetail()
            {
                WarehouseMutationId = warehouseMutation.Id,
                ItemId = coreBuilder3.UsedCoreItemId,
                Quantity = 2
            };
            wmoDetail3 = _warehouseMutationDetailService.CreateObject(wmoDetail3, _warehouseMutationService, _itemService, _warehouseItemService, _blanketService);

            wmoDetail4 = new WarehouseMutationDetail()
            {
                WarehouseMutationId = warehouseMutation.Id,
                ItemId = itemCompound.Id,
                Quantity = 500
            };
            wmoDetail4 = _warehouseMutationDetailService.CreateObject(wmoDetail4, _warehouseMutationService, _itemService, _warehouseItemService, _blanketService);

            wmoDetail5 = new WarehouseMutationDetail()
            {
                WarehouseMutationId = warehouseMutation.Id,
                ItemId = itemCompound1.Id,
                Quantity = 500
            };
            wmoDetail5 = _warehouseMutationDetailService.CreateObject(wmoDetail5, _warehouseMutationService, _itemService, _warehouseItemService, _blanketService);

            wmoDetail6 = new WarehouseMutationDetail()
            {
                WarehouseMutationId = warehouseMutation.Id,
                ItemId = itemCompound2.Id,
                Quantity = 500
            };
            wmoDetail6 = _warehouseMutationDetailService.CreateObject(wmoDetail6, _warehouseMutationService, _itemService, _warehouseItemService, _blanketService);

            wmoDetail7 = new WarehouseMutationDetail()
            {
                WarehouseMutationId = warehouseMutation.Id,
                ItemId = itemAccessory1.Id,
                Quantity = 1
            };
            wmoDetail7 = _warehouseMutationDetailService.CreateObject(wmoDetail7, _warehouseMutationService, _itemService, _warehouseItemService, _blanketService);

            wmoDetail8 = new WarehouseMutationDetail()
            {
                WarehouseMutationId = warehouseMutation.Id,
                ItemId = blanket3.Id,
                Quantity = 1
            };
            wmoDetail8 = _warehouseMutationDetailService.CreateObject(wmoDetail8, _warehouseMutationService, _itemService, _warehouseItemService, _blanketService);

            wmoDetail9 = new WarehouseMutationDetail()
            {
                WarehouseMutationId = warehouseMutation.Id,
                ItemId = coreBuilder4.UsedCoreItemId,
                Quantity = 2
            };
            wmoDetail9 = _warehouseMutationDetailService.CreateObject(wmoDetail9, _warehouseMutationService, _itemService, _warehouseItemService, _blanketService);
        }

        public void PopulateCoreIdentifications()
        {
            warehouseMutation = _warehouseMutationService.ConfirmObject(warehouseMutation, DateTime.Today, _warehouseMutationDetailService, _itemService,
                                                                                  _blanketService, _warehouseItemService, _stockMutationService);
            
            coreIdentification = new CoreIdentification()
            {
                Code = "CI001",
                ContactId = contact.Id,
                IsInHouse = false,
                IdentifiedDate = DateTime.Now,
                Quantity = 1,
                WarehouseId = movingWarehouse.Id
            };
            coreIdentification = _coreIdentificationService.CreateObject(coreIdentification, _contactService);
            coreIdentificationInHouse = new CoreIdentification()
            {
                Code = "CIH001",
                IsInHouse = true,
                IdentifiedDate = DateTime.Now,
                Quantity = 3,
                WarehouseId = movingWarehouse.Id
            };
            coreIdentificationInHouse = _coreIdentificationService.CreateObject(coreIdentificationInHouse, _contactService);
            coreIdentificationContact = new CoreIdentification()
            {
                Code = "CIC001",
                ContactId = contact.Id,
                IsInHouse = false,
                IdentifiedDate = DateTime.Now,
                Quantity = 3,
                WarehouseId = movingWarehouse.Id
            };
            coreIdentificationContact = _coreIdentificationService.CreateObject(coreIdentificationContact, _contactService);

            coreIdentificationDetail = new CoreIdentificationDetail()
            {
                CoreBuilderId = coreBuilder.Id,
                CoreIdentificationId = coreIdentification.Id,
                MachineId = machine.Id,
                DetailId = 1,
                RollerTypeId = typeDamp.Id,
                RD = (decimal) 10.1,
                CD = (decimal) 10.2,
                TL = (decimal) 11.9,
                WL = (decimal) 11.3,
                RL = (decimal) 9.2,
                MaterialCase = Core.Constants.Constant.MaterialCase.Used,
                RepairRequestCase = Core.Constants.Constant.RepairRequestCase.CentreDrill,
            };
            coreIdentificationDetail = _coreIdentificationDetailService.CreateObject(coreIdentificationDetail,
                                       _coreIdentificationService, _coreBuilderService, _rollerTypeService, _machineService, _warehouseItemService);

            coreIDInHouse1 = new CoreIdentificationDetail()
            {
                CoreBuilderId = coreBuilder1.Id,
                CoreIdentificationId = coreIdentificationInHouse.Id,
                MachineId = machine.Id,
                DetailId = 1,
                RollerTypeId = typeFoundDT.Id,
                RD = (decimal)10.1,
                CD = (decimal)10.2,
                TL = (decimal)9.9,
                WL = (decimal)9.3,
                RL = (decimal)9.2,
                MaterialCase = Core.Constants.Constant.MaterialCase.Used,
                RepairRequestCase = Core.Constants.Constant.RepairRequestCase.BearingSeat,
            };
            coreIDInHouse1 = _coreIdentificationDetailService.CreateObject(coreIDInHouse1, _coreIdentificationService,
                             _coreBuilderService, _rollerTypeService, _machineService, _warehouseItemService);

            coreIDInHouse2 = new CoreIdentificationDetail()
            {
                CoreBuilderId = coreBuilder1.Id,
                CoreIdentificationId = coreIdentificationInHouse.Id,
                MachineId = machine.Id,
                DetailId = 2,
                RollerTypeId = typeFoundDT.Id,
                RD = (decimal)10.1,
                CD = (decimal)10.2,
                TL = (decimal)9.9,
                WL = (decimal)9.3,
                RL = (decimal)9.2,
                MaterialCase = Core.Constants.Constant.MaterialCase.Used,
                RepairRequestCase = Core.Constants.Constant.RepairRequestCase.BearingSeat,
            };
            coreIDInHouse2 = _coreIdentificationDetailService.CreateObject(coreIDInHouse2, _coreIdentificationService,
                             _coreBuilderService, _rollerTypeService, _machineService, _warehouseItemService);

            coreIDInHouse3 = new CoreIdentificationDetail()
            {
                CoreBuilderId = coreBuilder2.Id,
                CoreIdentificationId = coreIdentificationInHouse.Id,
                MachineId = machine.Id,
                DetailId = 3,
                RollerTypeId = typeDampFormDQ.Id,
                RD = (decimal)12.1,
                CD = (decimal)10.2,
                TL = (decimal)9.9,
                WL = (decimal)9.3,
                RL = (decimal)12.2,
                MaterialCase = Core.Constants.Constant.MaterialCase.Used,
                RepairRequestCase = Core.Constants.Constant.RepairRequestCase.CentreDrill,
            };
            coreIDInHouse3 = _coreIdentificationDetailService.CreateObject(coreIDInHouse3, _coreIdentificationService,
                             _coreBuilderService, _rollerTypeService, _machineService, _warehouseItemService);

            coreIDContact1 = new CoreIdentificationDetail()
            {
                CoreBuilderId = coreBuilder3.Id,
                CoreIdentificationId = coreIdentificationContact.Id,
                MachineId = machine.Id,
                DetailId = 1,
                RollerTypeId = typeInkDistD.Id,
                RD = (decimal)9.1,
                CD = (decimal)9.2,
                TL = (decimal)9.9,
                WL = (decimal)9.3,
                RL = (decimal)8.2,
                MaterialCase = Core.Constants.Constant.MaterialCase.Used,
                RepairRequestCase = Core.Constants.Constant.RepairRequestCase.BearingSeat,
            };
            coreIDContact1 = _coreIdentificationDetailService.CreateObject(coreIDContact1, _coreIdentificationService,
                             _coreBuilderService, _rollerTypeService, _machineService, _warehouseItemService);

            coreIDContact2 = new CoreIdentificationDetail()
            {
                CoreBuilderId = coreBuilder3.Id,
                CoreIdentificationId = coreIdentificationContact.Id,
                MachineId = machine.Id,
                DetailId = 2,
                RollerTypeId = typeInkDistD.Id,
                RD = (decimal)9.1,
                CD = (decimal)9.2,
                TL = (decimal)9.9,
                WL = (decimal)9.3,
                RL = (decimal)8.2,
                MaterialCase = Core.Constants.Constant.MaterialCase.Used,
                RepairRequestCase = Core.Constants.Constant.RepairRequestCase.BearingSeat,
            };
            coreIDContact2 = _coreIdentificationDetailService.CreateObject(coreIDContact2, _coreIdentificationService,
                             _coreBuilderService, _rollerTypeService, _machineService, _warehouseItemService);

            coreIDContact3 = new CoreIdentificationDetail()
            {
                CoreBuilderId = coreBuilder4.Id,
                CoreIdentificationId = coreIdentificationContact.Id,
                MachineId = machine.Id,
                DetailId = 3,
                RollerTypeId = typeInkDistH.Id,
                RD = (decimal)9.1,
                CD = (decimal)9.2,
                TL = (decimal)9.9,
                WL = (decimal)9.3,
                RL = (decimal)8.2,
                MaterialCase = Core.Constants.Constant.MaterialCase.Used,
                RepairRequestCase = Core.Constants.Constant.RepairRequestCase.BearingSeat,
            };
            coreIDContact3 = _coreIdentificationDetailService.CreateObject(coreIDContact3, _coreIdentificationService,
                             _coreBuilderService, _rollerTypeService, _machineService, _warehouseItemService);
        }
        
        public void PopulateRecoveryOrders()
        {
            coreIdentification = _coreIdentificationService.ConfirmObject(coreIdentification, DateTime.Today, _coreIdentificationDetailService, _stockMutationService, _recoveryOrderService, _recoveryOrderDetailService, _coreBuilderService, _itemService, _warehouseItemService, _blanketService, _customerStockMutationService, _customerItemService);
            coreIdentificationContact = _coreIdentificationService.ConfirmObject(coreIdentificationContact, DateTime.Today, _coreIdentificationDetailService, _stockMutationService, _recoveryOrderService, _recoveryOrderDetailService, _coreBuilderService, _itemService, _warehouseItemService, _blanketService, _customerStockMutationService, _customerItemService);
            coreIdentificationInHouse = _coreIdentificationService.ConfirmObject(coreIdentificationInHouse, DateTime.Today, _coreIdentificationDetailService, _stockMutationService, _recoveryOrderService, _recoveryOrderDetailService, _coreBuilderService, _itemService, _warehouseItemService, _blanketService, _customerStockMutationService, _customerItemService);

            recoveryOrder = new RecoveryOrder()
            {
                Code = "ROX",
                CoreIdentificationId = coreIdentification.Id,
                QuantityReceived = coreIdentification.Quantity,
                WarehouseId = movingWarehouse.Id
            };
            recoveryOrder = _recoveryOrderService.CreateObject(recoveryOrder, _coreIdentificationService);

            recoveryOrderInHouse = new RecoveryOrder()
            {
                Code = "RO001",
                CoreIdentificationId = coreIdentificationInHouse.Id,
                QuantityReceived = coreIdentificationInHouse.Quantity,
                WarehouseId = movingWarehouse.Id
            };
            recoveryOrderInHouse = _recoveryOrderService.CreateObject(recoveryOrderInHouse, _coreIdentificationService);
            
            recoveryOrderContact = new RecoveryOrder()
            {
                Code = "RO002",
                CoreIdentificationId = coreIdentificationContact.Id,
                QuantityReceived = coreIdentificationContact.Quantity,
                WarehouseId = movingWarehouse.Id
            };
            recoveryOrderContact = _recoveryOrderService.CreateObject(recoveryOrderContact, _coreIdentificationService);

            recoveryODInHouse1 = new RecoveryOrderDetail()
            {
                RecoveryOrderId = recoveryOrderInHouse.Id,
                CoreIdentificationDetailId = coreIDInHouse1.Id,
                CoreTypeCase = Core.Constants.Constant.CoreTypeCase.R,
                RollerBuilderId = rollerBuilder1.Id
            };
            recoveryODInHouse1 = _recoveryOrderDetailService.CreateObject(recoveryODInHouse1, _recoveryOrderService, _coreIdentificationDetailService, _rollerBuilderService);

            recoveryODInHouse2 = new RecoveryOrderDetail()
            {
                RecoveryOrderId = recoveryOrderInHouse.Id,
                CoreIdentificationDetailId = coreIDInHouse2.Id,
                CoreTypeCase = Core.Constants.Constant.CoreTypeCase.R,
                RollerBuilderId = rollerBuilder1.Id
            };
            recoveryODInHouse2 = _recoveryOrderDetailService.CreateObject(recoveryODInHouse2, _recoveryOrderService, _coreIdentificationDetailService, _rollerBuilderService);

            recoveryODInHouse3 = new RecoveryOrderDetail()
            {
                RecoveryOrderId = recoveryOrderInHouse.Id,
                CoreIdentificationDetailId = coreIDInHouse3.Id,
                CoreTypeCase = Core.Constants.Constant.CoreTypeCase.R,
                RollerBuilderId = rollerBuilder2.Id
            };
            recoveryODInHouse3 = _recoveryOrderDetailService.CreateObject(recoveryODInHouse3, _recoveryOrderService, _coreIdentificationDetailService, _rollerBuilderService);

            recoveryODContact1 = new RecoveryOrderDetail()
            {
                RecoveryOrderId = recoveryOrderContact.Id,
                CoreIdentificationDetailId = coreIDContact1.Id,
                CoreTypeCase = Core.Constants.Constant.CoreTypeCase.R,
                RollerBuilderId = rollerBuilder3.Id
            };
            recoveryODContact1 = _recoveryOrderDetailService.CreateObject(recoveryODContact1, _recoveryOrderService, _coreIdentificationDetailService, _rollerBuilderService);

            recoveryODContact2 = new RecoveryOrderDetail()
            {
                RecoveryOrderId = recoveryOrderContact.Id,
                CoreIdentificationDetailId = coreIDContact2.Id,
                CoreTypeCase = Core.Constants.Constant.CoreTypeCase.R,
                RollerBuilderId = rollerBuilder3.Id
            };
            recoveryODContact2 = _recoveryOrderDetailService.CreateObject(recoveryODContact2, _recoveryOrderService, _coreIdentificationDetailService, _rollerBuilderService);

            recoveryODContact3 = new RecoveryOrderDetail()
            {
                RecoveryOrderId = recoveryOrderContact.Id,
                CoreIdentificationDetailId = coreIDContact3.Id,
                CoreTypeCase = Core.Constants.Constant.CoreTypeCase.R,
                RollerBuilderId = rollerBuilder4.Id
            };
            recoveryODContact3 = _recoveryOrderDetailService.CreateObject(recoveryODContact3, _recoveryOrderService, _coreIdentificationDetailService, _rollerBuilderService);
        }

        public void PopulateRecoveryOrders2()
        {
            // status after this function
            // recoveryODContact2, recoveryODInHouse3 are rejected.
            // The rest are delivered back to localWarehouse to complete the batch.
            // New recovery orders are created to complete coreIdentificationInHouse and coreIdentificationContact
            _recoveryOrderService.ConfirmObject(recoveryOrderContact, DateTime.Today, _coreIdentificationDetailService, _coreIdentificationService, _recoveryOrderDetailService,
                                      _recoveryAccessoryDetailService, _coreBuilderService, _stockMutationService, _itemService,
                                      _blanketService, _warehouseItemService, _warehouseService);
            _recoveryOrderService.ConfirmObject(recoveryOrderInHouse, DateTime.Today, _coreIdentificationDetailService, _coreIdentificationService, _recoveryOrderDetailService,
                                                _recoveryAccessoryDetailService, _coreBuilderService, _stockMutationService, _itemService,
                                                _blanketService, _warehouseItemService, _warehouseService);
            _recoveryOrderService.ConfirmObject(recoveryOrder, DateTime.Today, _coreIdentificationDetailService, _coreIdentificationService, _recoveryOrderDetailService,
                                                _recoveryAccessoryDetailService, _coreBuilderService, _stockMutationService, _itemService,
                                                _blanketService, _warehouseItemService, _warehouseService);

            _recoveryOrderDetailService.DisassembleObject(recoveryODContact1, _recoveryOrderService);
            _recoveryOrderDetailService.StripAndGlueObject(recoveryODContact1);
            _recoveryOrderDetailService.WrapObject(recoveryODContact1, 20, _recoveryOrderService, _rollerBuilderService, _itemService, _warehouseItemService);
            _recoveryOrderDetailService.VulcanizeObject(recoveryODContact1);
            _recoveryOrderDetailService.FaceOffObject(recoveryODContact1);
            _recoveryOrderDetailService.ConventionalGrindObject(recoveryODContact1);
            _recoveryOrderDetailService.CNCGrindObject(recoveryODContact1);
            _recoveryOrderDetailService.PolishAndQCObject(recoveryODContact1);
            _recoveryOrderDetailService.PackageObject(recoveryODContact1);

            _recoveryOrderDetailService.DisassembleObject(recoveryODContact2, _recoveryOrderService);
            _recoveryOrderDetailService.StripAndGlueObject(recoveryODContact2);
            _recoveryOrderDetailService.WrapObject(recoveryODContact2, 20, _recoveryOrderService, _rollerBuilderService, _itemService, _warehouseItemService);
            _recoveryOrderDetailService.VulcanizeObject(recoveryODContact2);
            _recoveryOrderDetailService.FaceOffObject(recoveryODContact2);
            _recoveryOrderDetailService.ConventionalGrindObject(recoveryODContact2);
            _recoveryOrderDetailService.CNCGrindObject(recoveryODContact2);
            _recoveryOrderDetailService.PolishAndQCObject(recoveryODContact2);
            _recoveryOrderDetailService.PackageObject(recoveryODContact2);

            _recoveryOrderDetailService.DisassembleObject(recoveryODContact3, _recoveryOrderService);
            _recoveryOrderDetailService.StripAndGlueObject(recoveryODContact3);
            _recoveryOrderDetailService.WrapObject(recoveryODContact3, 20, _recoveryOrderService, _rollerBuilderService, _itemService, _warehouseItemService);
            _recoveryOrderDetailService.VulcanizeObject(recoveryODContact3);
            _recoveryOrderDetailService.FaceOffObject(recoveryODContact3);
            _recoveryOrderDetailService.ConventionalGrindObject(recoveryODContact3);
            _recoveryOrderDetailService.CNCGrindObject(recoveryODContact3);
            _recoveryOrderDetailService.PolishAndQCObject(recoveryODContact3);
            _recoveryOrderDetailService.PackageObject(recoveryODContact3);

            _recoveryOrderDetailService.DisassembleObject(recoveryODInHouse1, _recoveryOrderService);
            _recoveryOrderDetailService.StripAndGlueObject(recoveryODInHouse1);
            _recoveryOrderDetailService.WrapObject(recoveryODInHouse1, 20, _recoveryOrderService, _rollerBuilderService, _itemService, _warehouseItemService);
            _recoveryOrderDetailService.VulcanizeObject(recoveryODInHouse1);
            _recoveryOrderDetailService.FaceOffObject(recoveryODInHouse1);
            _recoveryOrderDetailService.ConventionalGrindObject(recoveryODInHouse1);
            _recoveryOrderDetailService.CNCGrindObject(recoveryODInHouse1);
            _recoveryOrderDetailService.PolishAndQCObject(recoveryODInHouse1);
            _recoveryOrderDetailService.PackageObject(recoveryODInHouse1);

            _recoveryOrderDetailService.DisassembleObject(recoveryODInHouse2, _recoveryOrderService);
            _recoveryOrderDetailService.StripAndGlueObject(recoveryODInHouse2);
            _recoveryOrderDetailService.WrapObject(recoveryODInHouse2, 20, _recoveryOrderService, _rollerBuilderService, _itemService, _warehouseItemService);
            _recoveryOrderDetailService.VulcanizeObject(recoveryODInHouse2);
            _recoveryOrderDetailService.FaceOffObject(recoveryODInHouse2);
            _recoveryOrderDetailService.ConventionalGrindObject(recoveryODInHouse2);
            _recoveryOrderDetailService.CNCGrindObject(recoveryODInHouse2);
            _recoveryOrderDetailService.PolishAndQCObject(recoveryODInHouse2);
            _recoveryOrderDetailService.PackageObject(recoveryODInHouse2);

            accessory1 = new RecoveryAccessoryDetail()
            {
                ItemId = itemAccessory1.Id,
                Quantity = 1,
                RecoveryOrderDetailId = recoveryODInHouse2.Id
            };
            _recoveryAccessoryDetailService.CreateObject(accessory1, _recoveryOrderService, _recoveryOrderDetailService,
                                                         _itemService, _itemTypeService, _warehouseItemService);
            _recoveryOrderDetailService.DisassembleObject(recoveryODInHouse3, _recoveryOrderService);

            _recoveryOrderDetailService.FinishObject(recoveryODContact1, DateTime.Today, _coreIdentificationService, _coreIdentificationDetailService,
                                                     _recoveryOrderService, _recoveryAccessoryDetailService, _coreBuilderService, _rollerBuilderService,
                                                     _itemService, _warehouseItemService, _blanketService, _stockMutationService,
                                                     _accountService, _generalLedgerJournalService, _closingService, _serviceCostService, _customerStockMutationService, _customerItemService);
            _recoveryOrderDetailService.RejectObject(recoveryODContact2, DateTime.Today, _coreIdentificationService, _coreIdentificationDetailService, _recoveryOrderService,
                                                     _recoveryAccessoryDetailService, _coreBuilderService, _rollerBuilderService, _itemService,
                                                     _warehouseItemService, _blanketService, _stockMutationService,
                                                     _accountService, _generalLedgerJournalService, _closingService);
            _recoveryOrderDetailService.FinishObject(recoveryODContact3, DateTime.Today, _coreIdentificationService, _coreIdentificationDetailService,
                                                       _recoveryOrderService, _recoveryAccessoryDetailService, _coreBuilderService, _rollerBuilderService,
                                                       _itemService, _warehouseItemService, _blanketService, _stockMutationService,
                                                       _accountService, _generalLedgerJournalService, _closingService, _serviceCostService, _customerStockMutationService, _customerItemService);
            _recoveryOrderDetailService.FinishObject(recoveryODInHouse1, DateTime.Today, _coreIdentificationService, _coreIdentificationDetailService,
                                                     _recoveryOrderService, _recoveryAccessoryDetailService, _coreBuilderService, _rollerBuilderService,
                                                     _itemService, _warehouseItemService, _blanketService, _stockMutationService,
                                                     _accountService, _generalLedgerJournalService, _closingService, _serviceCostService, _customerStockMutationService, _customerItemService);
            _recoveryOrderDetailService.FinishObject(recoveryODInHouse2, DateTime.Today, _coreIdentificationService, _coreIdentificationDetailService,
                                                     _recoveryOrderService, _recoveryAccessoryDetailService, _coreBuilderService, _rollerBuilderService,
                                                     _itemService, _warehouseItemService, _blanketService, _stockMutationService,
                                                     _accountService, _generalLedgerJournalService, _closingService, _serviceCostService, _customerStockMutationService, _customerItemService);
            _recoveryOrderDetailService.RejectObject(recoveryODInHouse3, DateTime.Today, _coreIdentificationService, _coreIdentificationDetailService, _recoveryOrderService,
                                                     _recoveryAccessoryDetailService, _coreBuilderService, _rollerBuilderService, _itemService,
                                                     _warehouseItemService, _blanketService, _stockMutationService,
                                                     _accountService, _generalLedgerJournalService, _closingService);
        }

        public void PopulateStockAdjustment()
        {
            stockAdjustment = new StockAdjustment()
            {
                WarehouseId = movingWarehouse.Id,
                AdjustmentDate = DateTime.Now
            };
            stockAdjustment = _stockAdjustmentService.CreateObject(stockAdjustment, _warehouseService);

            stockAD = new StockAdjustmentDetail()
            {
                ItemId = coreBuilder.UsedCoreItemId,
                Quantity = 3,
                StockAdjustmentId = stockAdjustment.Id,
                Price = 50000
            };
            stockAD = _stockAdjustmentDetailService.CreateObject(stockAD, _stockAdjustmentService, _itemService, _warehouseItemService);
            
            stockAD1 = new StockAdjustmentDetail()
            {
                ItemId = coreBuilder1.UsedCoreItemId,
                Quantity = 3,
                StockAdjustmentId = stockAdjustment.Id,
                Price = 50000
            };
            stockAD1 = _stockAdjustmentDetailService.CreateObject(stockAD1, _stockAdjustmentService, _itemService, _warehouseItemService);

            stockAD2 = new StockAdjustmentDetail()
            {
                ItemId = coreBuilder2.UsedCoreItemId,
                Quantity = 3,
                StockAdjustmentId = stockAdjustment.Id,
                Price = 50000
            };
            stockAD2 = _stockAdjustmentDetailService.CreateObject(stockAD2, _stockAdjustmentService, _itemService, _warehouseItemService);

            stockAD3 = new StockAdjustmentDetail()
            {
                ItemId = coreBuilder3.UsedCoreItemId,
                Quantity = 3,
                StockAdjustmentId = stockAdjustment.Id,
                Price = 50000
            };
            stockAD3 = _stockAdjustmentDetailService.CreateObject(stockAD3, _stockAdjustmentService, _itemService, _warehouseItemService);

            stockAD4 = new StockAdjustmentDetail()
            {
                ItemId = coreBuilder4.UsedCoreItemId,
                Quantity = 3,
                StockAdjustmentId = stockAdjustment.Id,
                Price = 50000
            };
            stockAD4 = _stockAdjustmentDetailService.CreateObject(stockAD4, _stockAdjustmentService, _itemService, _warehouseItemService);
        }

        public void PopulateCustomerStockAdjustment()
        {
            customerStockAdjustment = new CustomerStockAdjustment()
            {
                ContactId = contact.Id,
                WarehouseId = movingWarehouse.Id,
                AdjustmentDate = DateTime.Now
            };
            _customerStockAdjustmentService.CreateObject(customerStockAdjustment, _warehouseService, _contactService);

            cstockAD = new CustomerStockAdjustmentDetail()
            {
                ItemId = coreBuilder.UsedCoreItemId,
                Quantity = 3,
                CustomerStockAdjustmentId = customerStockAdjustment.Id,
                Price = 50000
            };
            _customerStockAdjustmentDetailService.CreateObject(cstockAD, _customerStockAdjustmentService, _itemService, _warehouseItemService, _customerItemService);

            cstockAD1 = new CustomerStockAdjustmentDetail()
            {
                ItemId = coreBuilder1.UsedCoreItemId,
                Quantity = 3,
                CustomerStockAdjustmentId = customerStockAdjustment.Id,
                Price = 50000
            };
            _customerStockAdjustmentDetailService.CreateObject(cstockAD1, _customerStockAdjustmentService, _itemService, _warehouseItemService, _customerItemService);

            cstockAD2 = new CustomerStockAdjustmentDetail()
            {
                ItemId = coreBuilder2.UsedCoreItemId,
                Quantity = 3,
                CustomerStockAdjustmentId = customerStockAdjustment.Id,
                Price = 50000
            };
            _customerStockAdjustmentDetailService.CreateObject(cstockAD2, _customerStockAdjustmentService, _itemService, _warehouseItemService, _customerItemService);

            cstockAD3 = new CustomerStockAdjustmentDetail()
            {
                ItemId = coreBuilder3.UsedCoreItemId,
                Quantity = 3,
                CustomerStockAdjustmentId = customerStockAdjustment.Id,
                Price = 50000
            };
            _customerStockAdjustmentDetailService.CreateObject(cstockAD3, _customerStockAdjustmentService, _itemService, _warehouseItemService, _customerItemService);

            cstockAD4 = new CustomerStockAdjustmentDetail()
            {
                ItemId = coreBuilder4.UsedCoreItemId,
                Quantity = 3,
                CustomerStockAdjustmentId = customerStockAdjustment.Id,
                Price = 50000
            };
            _customerStockAdjustmentDetailService.CreateObject(cstockAD4, _customerStockAdjustmentService, _itemService, _warehouseItemService, _customerItemService);
        }

        public void PopulateRecoveryOrders3()
        {
            _stockAdjustmentService.ConfirmObject(stockAdjustment, DateTime.Today, _stockAdjustmentDetailService, _stockMutationService, _itemService, 
                                                  _blanketService, _warehouseItemService, _accountService, _generalLedgerJournalService, _closingService);
            
            recoveryOrderContact2 = new RecoveryOrder()
            {
                Code = "R002C2",
                CoreIdentificationId = coreIdentificationContact.Id,
                QuantityReceived = 1,
                WarehouseId = movingWarehouse.Id
            };
            _recoveryOrderService.CreateObject(recoveryOrderContact2, _coreIdentificationService);

            recoveryOrderInHouse2 = new RecoveryOrder()
            {
                Code = "R002H2",
                CoreIdentificationId = coreIdentificationInHouse.Id,
                QuantityReceived = 1,
                WarehouseId = movingWarehouse.Id
            };
            _recoveryOrderService.CreateObject(recoveryOrderInHouse2, _coreIdentificationService);

            recoveryODContact2b = new RecoveryOrderDetail()
            {
                RecoveryOrderId = recoveryOrderContact2.Id,
                CoreIdentificationDetailId = coreIDContact2.Id,
                CoreTypeCase = Core.Constants.Constant.CoreTypeCase.R,
                RollerBuilderId = rollerBuilder3.Id
            };
            _recoveryOrderDetailService.CreateObject(recoveryODContact2b, _recoveryOrderService, _coreIdentificationDetailService, _rollerBuilderService);

            recoveryODInHouse3b = new RecoveryOrderDetail()
            {
                RecoveryOrderId = recoveryOrderInHouse2.Id,
                CoreIdentificationDetailId = coreIDInHouse3.Id,
                CoreTypeCase = Core.Constants.Constant.CoreTypeCase.R,
                RollerBuilderId = rollerBuilder2.Id
            };
            _recoveryOrderDetailService.CreateObject(recoveryODInHouse3b, _recoveryOrderService, _coreIdentificationDetailService, _rollerBuilderService);
        }

        public void PopulateCoreIdentifications2()
        {
            _recoveryOrderService.ConfirmObject(recoveryOrderContact2, DateTime.Today, _coreIdentificationDetailService, _coreIdentificationService, _recoveryOrderDetailService, _recoveryAccessoryDetailService,
                                                _coreBuilderService, _stockMutationService, _itemService, _blanketService, _warehouseItemService, _warehouseService);
            _recoveryOrderService.ConfirmObject(recoveryOrderInHouse2, DateTime.Today, _coreIdentificationDetailService, _coreIdentificationService, _recoveryOrderDetailService, _recoveryAccessoryDetailService,
                                                _coreBuilderService, _stockMutationService, _itemService, _blanketService, _warehouseItemService, _warehouseService);

            _recoveryOrderDetailService.DisassembleObject(recoveryODInHouse3b, _recoveryOrderService);
            _recoveryOrderDetailService.StripAndGlueObject(recoveryODInHouse3b);
            _recoveryOrderDetailService.WrapObject(recoveryODInHouse3b, 20, _recoveryOrderService, _rollerBuilderService, _itemService, _warehouseItemService);
            _recoveryOrderDetailService.VulcanizeObject(recoveryODInHouse3b);
            _recoveryOrderDetailService.FaceOffObject(recoveryODInHouse3b);
            _recoveryOrderDetailService.ConventionalGrindObject(recoveryODInHouse3b);
            _recoveryOrderDetailService.CNCGrindObject(recoveryODInHouse3b);
            _recoveryOrderDetailService.PolishAndQCObject(recoveryODInHouse3b);
            _recoveryOrderDetailService.PackageObject(recoveryODInHouse3b);

            _recoveryOrderDetailService.DisassembleObject(recoveryODContact2b, _recoveryOrderService);
            _recoveryOrderDetailService.StripAndGlueObject(recoveryODContact2b);
            _recoveryOrderDetailService.WrapObject(recoveryODContact2b, 20, _recoveryOrderService, _rollerBuilderService, _itemService, _warehouseItemService);
            _recoveryOrderDetailService.VulcanizeObject(recoveryODContact2b);
            _recoveryOrderDetailService.FaceOffObject(recoveryODContact2b);
            _recoveryOrderDetailService.ConventionalGrindObject(recoveryODContact2b);
            _recoveryOrderDetailService.CNCGrindObject(recoveryODContact2b);
            _recoveryOrderDetailService.PolishAndQCObject(recoveryODContact2b);
            _recoveryOrderDetailService.PackageObject(recoveryODContact2b);

            _recoveryOrderDetailService.FinishObject(recoveryODInHouse3b, DateTime.Today, _coreIdentificationService, _coreIdentificationDetailService, _recoveryOrderService,
                                                     _recoveryAccessoryDetailService, _coreBuilderService, _rollerBuilderService, _itemService, _warehouseItemService,
                                                     _blanketService, _stockMutationService, _accountService, _generalLedgerJournalService, _closingService, _serviceCostService,
                                                     _customerStockMutationService, _customerItemService);
            _recoveryOrderDetailService.FinishObject(recoveryODContact2b, DateTime.Today, _coreIdentificationService, _coreIdentificationDetailService, _recoveryOrderService,
                                                     _recoveryAccessoryDetailService, _coreBuilderService, _rollerBuilderService, _itemService, _warehouseItemService,
                                                     _blanketService, _stockMutationService, _accountService, _generalLedgerJournalService, _closingService, _serviceCostService,
                                                     _customerStockMutationService, _customerItemService);
        }

        public void PopulateRollerWarehouseMutation()
        {
            rollerWarehouseMutationContact = new RollerWarehouseMutation()
            {
                RecoveryOrderId = recoveryOrderContact.Id,
                Quantity = 3,
                WarehouseFromId = movingWarehouse.Id,
                WarehouseToId = localWarehouse.Id,
                MutationDate = DateTime.Today
            };
            _rollerWarehouseMutationService.CreateObject(rollerWarehouseMutationContact, _warehouseService, _recoveryOrderService);

            rwmDetailContact1 = new RollerWarehouseMutationDetail()
            {
                RollerWarehouseMutationId = rollerWarehouseMutationContact.Id,
                RecoveryOrderDetailId = recoveryODContact1.Id,
                ItemId = (coreIDContact1.MaterialCase == Core.Constants.Constant.MaterialCase.Used) ?
                         _rollerBuilderService.GetRollerUsedCore(recoveryODContact1.RollerBuilderId).Id :
                          _rollerBuilderService.GetRollerNewCore(recoveryODContact1.RollerBuilderId).Id
            };
            _rollerWarehouseMutationDetailService.CreateObject(rwmDetailContact1, _rollerWarehouseMutationService, _recoveryOrderDetailService,
                                                               _coreIdentificationDetailService, _itemService, _warehouseItemService);
            
            rwmDetailContact2 = new RollerWarehouseMutationDetail()
            {
                RollerWarehouseMutationId = rollerWarehouseMutationContact.Id,
                RecoveryOrderDetailId = recoveryODContact2b.Id,
                ItemId = (coreIDContact2.MaterialCase == Core.Constants.Constant.MaterialCase.Used) ?
                         _rollerBuilderService.GetRollerUsedCore(recoveryODContact2b.RollerBuilderId).Id :
                          _rollerBuilderService.GetRollerNewCore(recoveryODContact2b.RollerBuilderId).Id
            };
            _rollerWarehouseMutationDetailService.CreateObject(rwmDetailContact2, _rollerWarehouseMutationService, _recoveryOrderDetailService,
                                                               _coreIdentificationDetailService, _itemService, _warehouseItemService);

            rwmDetailContact3 = new RollerWarehouseMutationDetail()
            {
                RollerWarehouseMutationId = rollerWarehouseMutationContact.Id,
                RecoveryOrderDetailId = recoveryODContact3.Id,
                ItemId = (coreIDContact3.MaterialCase == Core.Constants.Constant.MaterialCase.Used) ?
                         _rollerBuilderService.GetRollerUsedCore(recoveryODContact3.RollerBuilderId).Id :
                          _rollerBuilderService.GetRollerNewCore(recoveryODContact3.RollerBuilderId).Id
            };

            _rollerWarehouseMutationDetailService.CreateObject(rwmDetailContact3, _rollerWarehouseMutationService, _recoveryOrderDetailService,
                                                               _coreIdentificationDetailService, _itemService, _warehouseItemService);

            _rollerWarehouseMutationService.ConfirmObject(rollerWarehouseMutationContact, DateTime.Today, _rollerWarehouseMutationDetailService, _itemService,
                                                          _blanketService, _warehouseItemService, _stockMutationService, _recoveryOrderDetailService, _recoveryOrderService,
                                                          _coreIdentificationDetailService, _coreIdentificationService, _customerStockMutationService, _customerItemService);
            
            rollerWarehouseMutationInHouse = new RollerWarehouseMutation()
            {
                RecoveryOrderId = recoveryOrderInHouse.Id,
                Quantity = 3,
                WarehouseFromId = movingWarehouse.Id,
                WarehouseToId = localWarehouse.Id,
                MutationDate = DateTime.Today
            };
            _rollerWarehouseMutationService.CreateObject(rollerWarehouseMutationInHouse, _warehouseService, _recoveryOrderService);

            rwmDetailInHouse1 = new RollerWarehouseMutationDetail()
            {
                RollerWarehouseMutationId = rollerWarehouseMutationInHouse.Id,
                RecoveryOrderDetailId = recoveryODInHouse1.Id,
                ItemId = (coreIDInHouse1.MaterialCase == Core.Constants.Constant.MaterialCase.Used) ?
                         _rollerBuilderService.GetRollerUsedCore(recoveryODInHouse1.RollerBuilderId).Id :
                          _rollerBuilderService.GetRollerNewCore(recoveryODInHouse1.RollerBuilderId).Id
            };
            _rollerWarehouseMutationDetailService.CreateObject(rwmDetailInHouse1, _rollerWarehouseMutationService, _recoveryOrderDetailService,
                                                               _coreIdentificationDetailService, _itemService, _warehouseItemService);

            rwmDetailInHouse2 = new RollerWarehouseMutationDetail()
            {
                RollerWarehouseMutationId = rollerWarehouseMutationInHouse.Id,
                RecoveryOrderDetailId = recoveryODInHouse2.Id,
                ItemId = (coreIDInHouse2.MaterialCase == Core.Constants.Constant.MaterialCase.Used) ?
                         _rollerBuilderService.GetRollerUsedCore(recoveryODInHouse2.RollerBuilderId).Id :
                          _rollerBuilderService.GetRollerNewCore(recoveryODInHouse2.RollerBuilderId).Id
            };
            _rollerWarehouseMutationDetailService.CreateObject(rwmDetailInHouse2, _rollerWarehouseMutationService, _recoveryOrderDetailService,
                                                               _coreIdentificationDetailService, _itemService, _warehouseItemService);

            rwmDetailInHouse3 = new RollerWarehouseMutationDetail()
            {
                RollerWarehouseMutationId = rollerWarehouseMutationInHouse.Id,
                RecoveryOrderDetailId = recoveryODInHouse3b.Id,
                ItemId = (coreIDInHouse3.MaterialCase == Core.Constants.Constant.MaterialCase.Used) ?
                         _rollerBuilderService.GetRollerUsedCore(recoveryODInHouse3b.RollerBuilderId).Id :
                          _rollerBuilderService.GetRollerNewCore(recoveryODInHouse3b.RollerBuilderId).Id
            };
            _rollerWarehouseMutationDetailService.CreateObject(rwmDetailInHouse3, _rollerWarehouseMutationService, _recoveryOrderDetailService,
                                                               _coreIdentificationDetailService, _itemService, _warehouseItemService);

            _rollerWarehouseMutationService.ConfirmObject(rollerWarehouseMutationInHouse, DateTime.Today, _rollerWarehouseMutationDetailService, _itemService,
                                                          _blanketService, _warehouseItemService, _stockMutationService, _recoveryOrderDetailService, _recoveryOrderService,
                                                          _coreIdentificationDetailService, _coreIdentificationService, _customerStockMutationService, _customerItemService);
        }

        public void PopulateBlanket()
        {
            bargeneric = new Item()
            {
                ItemTypeId = typeBar.Id,
                Name = "Bar Generic",
                UoMId = Pcs.Id,
                Sku = "BGEN"
            };
            _itemService.CreateObject(bargeneric, _uomService, _itemTypeService, _warehouseItemService, _warehouseService, _priceMutationService);

            barleft1 = new Item()
            {
                ItemTypeId = typeBar.Id,
                Name = "Bar Left 1",
                UoMId = Pcs.Id,
                Sku = "BL1"
            };
            _itemService.CreateObject(barleft1, _uomService, _itemTypeService, _warehouseItemService, _warehouseService, _priceMutationService);

            barleft2 = new Item()
            {
                ItemTypeId = typeBar.Id,
                Name = "Bar Left 2",
                UoMId = Pcs.Id,
                Sku = "BL2"
            };
            _itemService.CreateObject(barleft2, _uomService, _itemTypeService, _warehouseItemService, _warehouseService, _priceMutationService);

            barright1 = new Item()
            {
                ItemTypeId = typeBar.Id,
                Name = "Bar Right 1",
                UoMId = Pcs.Id,
                Sku = "BR1"
            };
            _itemService.CreateObject(barright1, _uomService, _itemTypeService, _warehouseItemService, _warehouseService, _priceMutationService);

            barright2 = new Item()
            {
                ItemTypeId = typeBar.Id,
                Name = "Bar Right 2",
                UoMId = Pcs.Id,
                Sku = "BR2"
            };
            _itemService.CreateObject(barright2, _uomService, _itemTypeService, _warehouseItemService, _warehouseService, _priceMutationService);

            rollBlanket1 = new Item()
            {
                ItemTypeId = typeRollBlanket.Id,
                Name = "RollBlanket1",
                UoMId = Pcs.Id,
                Sku = "BLK1"
            };
            _itemService.CreateObject(rollBlanket1, _uomService, _itemTypeService, _warehouseItemService, _warehouseService, _priceMutationService);

            rollBlanket2 = new Item()
            {
                ItemTypeId = typeRollBlanket.Id,
                Name = "RollBlanket2",
                UoMId = Pcs.Id,
                Sku = "BLK2"
            };
            _itemService.CreateObject(rollBlanket2, _uomService, _itemTypeService, _warehouseItemService, _warehouseService, _priceMutationService);

            rollBlanket3 = new Item()
            {
                ItemTypeId = typeRollBlanket.Id,
                Name = "RollBlanket3",
                UoMId = Pcs.Id,
                Sku = "BLK3"
            };
            _itemService.CreateObject(rollBlanket3, _uomService, _itemTypeService, _warehouseItemService, _warehouseService, _priceMutationService);

            blanket1 = new Blanket()
            {
                ItemTypeId = typeBlanket.Id,
                Name = "Blanket1",
                UoMId = Pcs.Id,
                Sku = "BRG1",
                RollNo = "BRG1_123",
                AC = 10,
                AR = 15,
                IsBarRequired = true,
                RollBlanketItemId = rollBlanket1.Id,
                HasLeftBar = true,
                HasRightBar = true,
                LeftBarItemId = bargeneric.Id,
                RightBarItemId = bargeneric.Id,
                ContactId = contact.Id,
                thickness = 1,
                MachineId = machine.Id,
                AdhesiveId = itemAdhesiveBlanket.Id,
                Adhesive2Id = itemAdhesiveBlanket.Id,
                CroppingType = Core.Constants.Constant.CroppingType.Normal,
                ApplicationCase = Core.Constants.Constant.ApplicationCase.Web
            };
            _blanketService.CreateObject(blanket1, _blanketService, _uomService, _itemService, _itemTypeService, _contactService,
                                         _machineService, _warehouseItemService, _warehouseService, _priceMutationService);

            blanket2 = new Blanket()
            {
                ItemTypeId = typeBlanket.Id,
                Name = "Blanket2",
                UoMId = Pcs.Id,
                Sku = "BRG2",
                RollNo = "BRG2_123",
                AC = 3,
                AR = 5,
                IsBarRequired = true,
                RollBlanketItemId = rollBlanket2.Id,
                HasLeftBar = true,
                HasRightBar = true,
                LeftBarItemId = barleft1.Id,
                RightBarItemId = barright1.Id,
                ContactId = contact.Id,
                thickness = 1,
                MachineId = machine.Id,
                AdhesiveId = itemAdhesiveBlanket.Id,
                Adhesive2Id = itemAdhesiveBlanket.Id,
                CroppingType = Core.Constants.Constant.CroppingType.Normal,
                ApplicationCase = Core.Constants.Constant.ApplicationCase.Web
            };
            _blanketService.CreateObject(blanket2, _blanketService, _uomService, _itemService, _itemTypeService, _contactService,
                                         _machineService, _warehouseItemService, _warehouseService, _priceMutationService);

            blanket3 = new Blanket()
            {
                ItemTypeId = typeBlanket.Id,
                Name = "Blanket3",
                UoMId = Pcs.Id,
                Sku = "BRG3",
                RollNo = "BRG3_123",
                AC = 7,
                AR = 10,
                IsBarRequired = true,
                RollBlanketItemId = rollBlanket3.Id,
                HasLeftBar = true,
                HasRightBar = true,
                LeftBarItemId = barleft2.Id,
                RightBarItemId = barright2.Id,
                ContactId = contact.Id,
                thickness = 1,
                MachineId = machine.Id,
                AdhesiveId = itemAdhesiveBlanket.Id,
                Adhesive2Id = itemAdhesiveBlanket.Id,
                CroppingType = Core.Constants.Constant.CroppingType.Special,
                LeftOverAC = 6,
                LeftOverAR = 9,
                ApplicationCase = Core.Constants.Constant.ApplicationCase.Sheetfed
            };
            _blanketService.CreateObject(blanket3, _blanketService, _uomService, _itemService, _itemTypeService, _contactService,
                                         _machineService, _warehouseItemService, _warehouseService, _priceMutationService);

            StockAdjustment sa = new StockAdjustment() { WarehouseId = localWarehouse.Id, AdjustmentDate = DateTime.Today, Description = "Bar Related Adjustment" };
            _stockAdjustmentService.CreateObject(sa, _warehouseService);
            StockAdjustmentDetail sadBarGeneric = new StockAdjustmentDetail () { ItemId = bargeneric.Id , Quantity = 10, StockAdjustmentId = sa.Id, Price = 50000 };
            _stockAdjustmentDetailService.CreateObject(sadBarGeneric, _stockAdjustmentService, _itemService, _warehouseItemService);
            StockAdjustmentDetail sadBarleft1 = new StockAdjustmentDetail() { ItemId = barleft1.Id, Quantity = 10, StockAdjustmentId = sa.Id, Price = 50000 };
            _stockAdjustmentDetailService.CreateObject(sadBarleft1, _stockAdjustmentService, _itemService, _warehouseItemService);
            StockAdjustmentDetail sadBarleft2 = new StockAdjustmentDetail() { ItemId = barleft1.Id, Quantity = 10, StockAdjustmentId = sa.Id, Price = 50000 };
            _stockAdjustmentDetailService.CreateObject(sadBarleft2, _stockAdjustmentService, _itemService, _warehouseItemService);
            StockAdjustmentDetail sadBarright1 = new StockAdjustmentDetail() { ItemId = barright1.Id, Quantity = 10, StockAdjustmentId = sa.Id, Price = 50000 };
            _stockAdjustmentDetailService.CreateObject(sadBarright1, _stockAdjustmentService, _itemService, _warehouseItemService);
            StockAdjustmentDetail sadBarright2 = new StockAdjustmentDetail() { ItemId = barright2.Id, Quantity = 10, StockAdjustmentId = sa.Id, Price = 50000 };
            _stockAdjustmentDetailService.CreateObject(sadBarright2, _stockAdjustmentService, _itemService, _warehouseItemService);
            StockAdjustmentDetail sadRollBlanket1 = new StockAdjustmentDetail() { ItemId = rollBlanket1.Id, Quantity = 10, StockAdjustmentId = sa.Id, Price = 50000 };
            _stockAdjustmentDetailService.CreateObject(sadRollBlanket1, _stockAdjustmentService, _itemService, _warehouseItemService);
            StockAdjustmentDetail sadRollBlanket2 = new StockAdjustmentDetail() { ItemId = rollBlanket2.Id, Quantity = 20, StockAdjustmentId = sa.Id, Price = 50000 };
            _stockAdjustmentDetailService.CreateObject(sadRollBlanket2, _stockAdjustmentService, _itemService, _warehouseItemService);
            StockAdjustmentDetail sadRollBlanket3 = new StockAdjustmentDetail() { ItemId = rollBlanket3.Id, Quantity = 10, StockAdjustmentId = sa.Id, Price = 50000 };
            _stockAdjustmentDetailService.CreateObject(sadRollBlanket3, _stockAdjustmentService, _itemService, _warehouseItemService);
            StockAdjustmentDetail sadBlanket1 = new StockAdjustmentDetail() { ItemId = blanket1.Id, Quantity = 10, StockAdjustmentId = sa.Id, Price = 50000 };
            _stockAdjustmentDetailService.CreateObject(sadBlanket1, _stockAdjustmentService, _itemService, _warehouseItemService);
            StockAdjustmentDetail sadBlanket2 = new StockAdjustmentDetail() { ItemId = blanket2.Id, Quantity = 10, StockAdjustmentId = sa.Id, Price = 50000 };
            _stockAdjustmentDetailService.CreateObject(sadBlanket2, _stockAdjustmentService, _itemService, _warehouseItemService);
            StockAdjustmentDetail sadBlanket3 = new StockAdjustmentDetail() { ItemId = blanket3.Id, Quantity = 10, StockAdjustmentId = sa.Id, Price = 50000 };
            _stockAdjustmentDetailService.CreateObject(sadBlanket3, _stockAdjustmentService, _itemService, _warehouseItemService);

            _stockAdjustmentService.ConfirmObject(sa, DateTime.Today, _stockAdjustmentDetailService, _stockMutationService, _itemService, _blanketService, _warehouseItemService,
                                                  _accountService, _generalLedgerJournalService, _closingService);
        }

        public void PopulateBlanketOrders()
        {
            blanketOrderContact = new BlanketOrder()
            {
                ContactId = contact.Id,
                QuantityReceived = 4,
                Code = "BO0001",
                WarehouseId = localWarehouse.Id
            };
            _blanketOrderService.CreateObject(blanketOrderContact);

            blanketODContact1 = new BlanketOrderDetail()
            {
                BlanketId = blanket1.Id,
                BlanketOrderId = blanketOrderContact.Id
            };
            _blanketOrderDetailService.CreateObject(blanketODContact1, _blanketOrderService, _blanketService);

            blanketODContact2 = new BlanketOrderDetail()
            {
                BlanketId = blanket1.Id,
                BlanketOrderId = blanketOrderContact.Id
            };
            _blanketOrderDetailService.CreateObject(blanketODContact2, _blanketOrderService, _blanketService);

            blanketODContact3 = new BlanketOrderDetail()
            {
                BlanketId = blanket2.Id,
                BlanketOrderId = blanketOrderContact.Id
            };
            _blanketOrderDetailService.CreateObject(blanketODContact3, _blanketOrderService, _blanketService);

            blanketODContact4 = new BlanketOrderDetail()
            {
                BlanketId = blanket2.Id,
                BlanketOrderId = blanketOrderContact.Id
            };
            _blanketOrderDetailService.CreateObject(blanketODContact4, _blanketOrderService, _blanketService);
        }

        public void PopulateBlendingRecipe()
        {
            blending = new BlendingRecipe()
            {
                Name = "Pencampuran pewarna dan oli",
                TargetItemId = itemBlending.Id,
                TargetQuantity = 10,
            };
            _blendingRecipeService.CreateObject(blending, _itemService, _itemTypeService);

            blendingDet1 = new BlendingRecipeDetail()
            {
                BlendingRecipeId = blending.Id,
                ItemId = itemBlendingDet1.Id,
                Quantity = 1,
            };
            _blendingRecipeDetailService.CreateObject(blendingDet1, _blendingRecipeService, _itemService);

            blendingDet2 = new BlendingRecipeDetail()
            {
                BlendingRecipeId = blending.Id,
                ItemId = itemBlendingDet2.Id,
                Quantity = 10,
            };
            _blendingRecipeDetailService.CreateObject(blendingDet2, _blendingRecipeService, _itemService);

            blendingDet3 = new BlendingRecipeDetail()
            {
                BlendingRecipeId = blending.Id,
                ItemId = itemBlendingDet3.Id,
                Quantity = 1,
            };
            _blendingRecipeDetailService.CreateObject(blendingDet3, _blendingRecipeService, _itemService);
            
        }

        public void PopulateBlendingWorkOrders()
        {
            blendingWorkOrder = new BlendingWorkOrder()
            {
                Code = "CBWO001",
                BlendingDate = DateTime.Now,
                BlendingRecipeId = blending.Id,
                WarehouseId = localWarehouse.Id
            };
            _blendingWorkOrderService.CreateObject(blendingWorkOrder, _blendingRecipeService, _warehouseService);

            
        }

        public void PopulateCashBank()
        {
            cashBankAdjustment = new CashBankAdjustment()
            {
                AdjustmentDate = DateTime.Today,
                Amount = 200000000,
                CashBankId = cashBank1.Id,
            };
            _cashBankAdjustmentService.CreateObject(cashBankAdjustment, _cashBankService);

            _cashBankAdjustmentService.ConfirmObject(cashBankAdjustment, DateTime.Today, _cashMutationService, _cashBankService,
                                                     _accountService, _generalLedgerJournalService, _closingService,_currencyService, _exchangeRateService, _gLNonBaseCurrencyService);

            cashBankAdjustment2 = new CashBankAdjustment()
            {
                AdjustmentDate = DateTime.Today,
                Amount = 50000,
                CashBankId = cashBank1.Id,
            };
            _cashBankAdjustmentService.CreateObject(cashBankAdjustment2, _cashBankService);

            _cashBankAdjustmentService.ConfirmObject(cashBankAdjustment2, DateTime.Today, _cashMutationService, _cashBankService,
                                                     _accountService, _generalLedgerJournalService, _closingService,_currencyService, _exchangeRateService, _gLNonBaseCurrencyService);

            cashBankMutation = new CashBankMutation()
            {
                Amount = 50000000,
                SourceCashBankId = cashBank1.Id,
                TargetCashBankId = cashBank2.Id,
                Code = "CBM0001",
                MutationDate = DateTime.Today
            };
            _cashBankMutationService.CreateObject(cashBankMutation, _cashBankService);

            _cashBankMutationService.ConfirmObject(cashBankMutation, DateTime.Today, _cashMutationService, _cashBankService,
                                                   _accountService, _generalLedgerJournalService, _closingService,_currencyService, _exchangeRateService, _gLNonBaseCurrencyService);

        }

        // @SalesBuilder
        public void PopulateSalesAndDelivery()
        {
            TimeSpan purchaseDate = new TimeSpan(10, 0, 0, 0);
            TimeSpan receivedDate = new TimeSpan(3, 0, 0 ,0);
            TimeSpan lateDeliveryDate = new TimeSpan(2, 0, 0, 0);
            so1 = new SalesOrder()
            {
                SalesDate = DateTime.Today.Subtract(purchaseDate),
                ContactId = contact.Id,
                CurrencyId = currencyIDR.Id
            };
            _salesOrderService.CreateObject(so1, _contactService);

            so2 = new SalesOrder()
            {
                SalesDate = DateTime.Today.Subtract(purchaseDate),
                ContactId = contact.Id,
                CurrencyId = currencyIDR.Id
            };
            _salesOrderService.CreateObject(so2, _contactService);

            so1a = new SalesOrderDetail()
            {
                ItemId = rollBlanket1.Id,
                SalesOrderId = so1.Id,
                Quantity = 300,
                Price = 50000
            };
            _salesOrderDetailService.CreateObject(so1a, _salesOrderService, _itemService);

            so1b = new SalesOrderDetail()
            {
                ItemId = rollBlanket2.Id,
                SalesOrderId = so1.Id,
                Quantity = 250,
                Price = 72000
            };
            _salesOrderDetailService.CreateObject(so1b, _salesOrderService, _itemService);

            so1c = new SalesOrderDetail()
            {
                ItemId = rollBlanket3.Id,
                SalesOrderId = so1.Id,
                Quantity = 100,
                Price = 100000
            };
            _salesOrderDetailService.CreateObject(so1c, _salesOrderService, _itemService);

            so2a = new SalesOrderDetail()
            {
                ItemId = rollBlanket1.Id,
                SalesOrderId = so2.Id,
                Quantity = 300,
                Price = 50000
            };
            _salesOrderDetailService.CreateObject(so2a, _salesOrderService, _itemService);

            so2b = new SalesOrderDetail()
            {
                ItemId = rollBlanket2.Id,
                SalesOrderId = so2.Id,
                Quantity = 250,
                Price = 72000
            };
            _salesOrderDetailService.CreateObject(so2b, _salesOrderService, _itemService);

            _salesOrderService.ConfirmObject(so1, so1.SalesDate, _salesOrderDetailService, _stockMutationService, _itemService, _blanketService, _warehouseItemService);
            _salesOrderService.ConfirmObject(so2, so2.SalesDate, _salesOrderDetailService, _stockMutationService, _itemService, _blanketService, _warehouseItemService);

            do1 = new DeliveryOrder()
            {
                SalesOrderId = so1.Id,
                DeliveryDate = DateTime.Now.Subtract(receivedDate),
                WarehouseId = localWarehouse.Id
            };
            _deliveryOrderService.CreateObject(do1, _salesOrderService, _warehouseService);

            do2 = new DeliveryOrder()
            {
                SalesOrderId = so2.Id,
                DeliveryDate = DateTime.Now.Subtract(receivedDate),
                WarehouseId = localWarehouse.Id
            };
            _deliveryOrderService.CreateObject(do2, _salesOrderService, _warehouseService);

            do1a = new DeliveryOrderDetail()
            {
                SalesOrderDetailId = so1a.Id,
                DeliveryOrderId = do1.Id,
                ItemId = so1a.ItemId,
                Quantity = so1a.Quantity
            };
            _deliveryOrderDetailService.CreateObject(do1a, _deliveryOrderService, _salesOrderDetailService, _salesOrderService, _itemService);

            do1b = new DeliveryOrderDetail()
            {
                SalesOrderDetailId = so1b.Id,
                DeliveryOrderId = do1.Id,
                ItemId = so1b.ItemId,
                Quantity = so1b.Quantity
            };
            _deliveryOrderDetailService.CreateObject(do1b, _deliveryOrderService, _salesOrderDetailService, _salesOrderService, _itemService);

            do2a = new DeliveryOrderDetail()
            {
                SalesOrderDetailId = so2a.Id,
                DeliveryOrderId = do2.Id,
                ItemId = so2a.ItemId,
                Quantity = so2a.Quantity
            };
            _deliveryOrderDetailService.CreateObject(do2a, _deliveryOrderService, _salesOrderDetailService, _salesOrderService, _itemService);

            do2b = new DeliveryOrderDetail()
            {
                SalesOrderDetailId = so2b.Id,
                DeliveryOrderId = do2.Id,
                ItemId = so2b.ItemId,
                Quantity = so2b.Quantity
            };
            _deliveryOrderDetailService.CreateObject(do2b, _deliveryOrderService, _salesOrderDetailService, _salesOrderService, _itemService);

            do3 = new DeliveryOrder()
            {
                SalesOrderId = so1.Id,
                DeliveryDate = DateTime.Now.Subtract(lateDeliveryDate),
                WarehouseId = localWarehouse.Id
            };
            _deliveryOrderService.CreateObject(do3, _salesOrderService, _warehouseService);

            do1c = new DeliveryOrderDetail()
            {
                DeliveryOrderId = do3.Id,
                SalesOrderDetailId = so1c.Id,
                Quantity = so1c.Quantity,
                ItemId = so1c.ItemId
            };
            _deliveryOrderDetailService.CreateObject(do1c, _deliveryOrderService, _salesOrderDetailService, _salesOrderService, _itemService);

            do1a2 = new DeliveryOrderDetail()
            {
                DeliveryOrderId = do3.Id,
                SalesOrderDetailId = so1a.Id,
                Quantity = 100,
                ItemId = so1a.ItemId
            };
            _deliveryOrderDetailService.CreateObject(do1a2, _deliveryOrderService, _salesOrderDetailService, _salesOrderService, _itemService);

        }

        public void PopulateSalesInvoice()
        {
            TimeSpan receivedDate = new TimeSpan(3, 0, 0, 0);
            TimeSpan lateDeliveryDate = new TimeSpan(2, 0, 0, 0);
            _deliveryOrderService.ConfirmObject(do1, DateTime.Now.Subtract(receivedDate), _deliveryOrderDetailService, _salesOrderService, _salesOrderDetailService, _stockMutationService,
                                                _itemService, _blanketService, _warehouseItemService, _accountService, _generalLedgerJournalService, _closingService, _serviceCostService,
                                                _temporaryDeliveryOrderDetailService, _temporaryDeliveryOrderService, _customerStockMutationService, _customerItemService, _currencyService, _exchangeRateService);
            _deliveryOrderService.ConfirmObject(do2, DateTime.Now.Subtract(receivedDate), _deliveryOrderDetailService, _salesOrderService, _salesOrderDetailService, _stockMutationService,
                                                _itemService, _blanketService, _warehouseItemService, _accountService, _generalLedgerJournalService, _closingService, _serviceCostService,
                                                _temporaryDeliveryOrderDetailService, _temporaryDeliveryOrderService, _customerStockMutationService, _customerItemService, _currencyService, _exchangeRateService);
            _deliveryOrderService.ConfirmObject(do3, DateTime.Now.Subtract(receivedDate), _deliveryOrderDetailService, _salesOrderService, _salesOrderDetailService, _stockMutationService, 
                                                _itemService, _blanketService, _warehouseItemService, _accountService, _generalLedgerJournalService, _closingService, _serviceCostService,
                                                _temporaryDeliveryOrderDetailService, _temporaryDeliveryOrderService, _customerStockMutationService, _customerItemService, _currencyService, _exchangeRateService);

            si1 = new SalesInvoice()
            {
                InvoiceDate = DateTime.Today,
                Description = "Penjualan DO1",
                DeliveryOrderId = do1.Id,
                Tax = 10,
                Discount = 0,
                DueDate = DateTime.Today.AddDays(14),
                CurrencyId = currencyIDR.Id
            };
            si1 = _salesInvoiceService.CreateObject(si1, _deliveryOrderService);

            si1a = new SalesInvoiceDetail()
            {
                SalesInvoiceId = si1.Id,
                DeliveryOrderDetailId = do1a.Id,
                Quantity = do1a.Quantity
            };
            si1a = _salesInvoiceDetailService.CreateObject(si1a, _salesInvoiceService, _salesOrderDetailService, _deliveryOrderDetailService);

            si1b = new SalesInvoiceDetail()
            {
                SalesInvoiceId = si1.Id,
                DeliveryOrderDetailId = do1b.Id,
                Quantity = do1b.Quantity
            };
            si1b = _salesInvoiceDetailService.CreateObject(si1b, _salesInvoiceService, _salesOrderDetailService, _deliveryOrderDetailService);

            si2 = new SalesInvoice()
            {
                InvoiceDate = DateTime.Today,
                Description = "Penjualan DO2",
                DeliveryOrderId = do2.Id,
                Tax = 10,
                Discount = 5,
                DueDate = DateTime.Today.AddDays(14),
                CurrencyId = currencyIDR.Id
            };
            si2 = _salesInvoiceService.CreateObject(si2, _deliveryOrderService);

            si2a = new SalesInvoiceDetail()
            {
                SalesInvoiceId = si2.Id,
                DeliveryOrderDetailId = do2a.Id,
                Quantity = do2a.Quantity
            };
            si2a = _salesInvoiceDetailService.CreateObject(si2a, _salesInvoiceService, _salesOrderDetailService, _deliveryOrderDetailService);

            si2b = new SalesInvoiceDetail()
            {
                SalesInvoiceId = si2.Id,
                DeliveryOrderDetailId = do2b.Id,
                Quantity = do2b.Quantity
            };
            si2b = _salesInvoiceDetailService.CreateObject(si2b, _salesInvoiceService, _salesOrderDetailService, _deliveryOrderDetailService);

            si3 = new SalesInvoice()
            {
                InvoiceDate = DateTime.Today,
                Description = "Penjualan DO3",
                DeliveryOrderId = do3.Id,
                Tax = 10,
                Discount = 0,
                DueDate = DateTime.Today.AddDays(14),
                CurrencyId = currencyIDR.Id
            };
            si3 = _salesInvoiceService.CreateObject(si3, _deliveryOrderService);

            si1a2 = new SalesInvoiceDetail()
            {
                SalesInvoiceId = si3.Id,
                DeliveryOrderDetailId = do1a2.Id,
                Quantity = do1a2.Quantity
            };
            si1a2 = _salesInvoiceDetailService.CreateObject(si1a2, _salesInvoiceService, _salesOrderDetailService, _deliveryOrderDetailService);

            si1c = new SalesInvoiceDetail()
            {
                SalesInvoiceId = si3.Id,
                DeliveryOrderDetailId = do1c.Id,
                Quantity = do1c.Quantity
            };
            si1c = _salesInvoiceDetailService.CreateObject(si1c, _salesInvoiceService, _salesOrderDetailService, _deliveryOrderDetailService);

        }

        public void PopulateReceiptVoucher()
        {
            _salesInvoiceService.ConfirmObject(si1, DateTime.Today, _salesInvoiceDetailService, _salesOrderService, _salesOrderDetailService, _deliveryOrderService,
                                               _deliveryOrderDetailService, _receivableService, _accountService, _generalLedgerJournalService, _closingService,
                                               _serviceCostService, _rollerBuilderService, _itemService, _contactService, _exchangeRateService, _currencyService, _gLNonBaseCurrencyService);
            _salesInvoiceService.ConfirmObject(si2, DateTime.Today, _salesInvoiceDetailService, _salesOrderService, _salesOrderDetailService, _deliveryOrderService,
                                               _deliveryOrderDetailService, _receivableService, _accountService, _generalLedgerJournalService, _closingService,
                                               _serviceCostService, _rollerBuilderService, _itemService, _contactService, _exchangeRateService, _currencyService, _gLNonBaseCurrencyService);
            _salesInvoiceService.ConfirmObject(si3, DateTime.Today, _salesInvoiceDetailService, _salesOrderService, _salesOrderDetailService, _deliveryOrderService,
                                               _deliveryOrderDetailService, _receivableService, _accountService, _generalLedgerJournalService, _closingService,
                                               _serviceCostService, _rollerBuilderService, _itemService, _contactService, _exchangeRateService, _currencyService, _gLNonBaseCurrencyService);

            rv = new ReceiptVoucher()
            {
                ContactId = contact.Id,
                CashBankId = cashBank.Id,
                ReceiptDate = DateTime.Today.AddDays(14),
                IsGBCH = true,
                DueDate = DateTime.Today.AddDays(14),
                RateToIDR = 1,                
            };
            _receiptVoucherService.CreateObject(rv, _receiptVoucherDetailService, _receivableService, _contactService, _cashBankService);

            rvd1 = new ReceiptVoucherDetail()
            {
                ReceiptVoucherId = rv.Id,
                ReceivableId = _receivableService.GetObjectBySource(Core.Constants.Constant.ReceivableSource.SalesInvoice, si1.Id).Id,
                Amount = si1.AmountReceivable,
                AmountPaid = si1.AmountReceivable,
                Description = "Receipt buat Sales Invoice 1",
                Rate = 1
            };
            _receiptVoucherDetailService.CreateObject(rvd1, _receiptVoucherService, _cashBankService, _receivableService, _currencyService);

            rvd2 = new ReceiptVoucherDetail()
            {
                ReceiptVoucherId = rv.Id,
                ReceivableId = _receivableService.GetObjectBySource(Core.Constants.Constant.ReceivableSource.SalesInvoice, si2.Id).Id,
                Amount = si2.AmountReceivable,
                AmountPaid = si2.AmountReceivable,
                Description = "Receipt buat Sales Invoice 2",
                Rate = 1
            };
            _receiptVoucherDetailService.CreateObject(rvd2, _receiptVoucherService, _cashBankService, _receivableService, _currencyService);

            rvd3 = new ReceiptVoucherDetail()
            {
                ReceiptVoucherId = rv.Id,
                ReceivableId = _receivableService.GetObjectBySource(Core.Constants.Constant.ReceivableSource.SalesInvoice, si3.Id).Id,
                Amount = si3.AmountReceivable,
                AmountPaid = si3.AmountReceivable,
                Rate = 1,
                Description = "Receipt buat Sales Invoice 3"
            };
            _receiptVoucherDetailService.CreateObject(rvd3, _receiptVoucherService, _cashBankService, _receivableService, _currencyService);

            _receiptVoucherService.ConfirmObject(rv, DateTime.Today, _receiptVoucherDetailService, _cashBankService, _receivableService, _cashMutationService,
                                                 _accountService, _generalLedgerJournalService, _closingService, _currencyService, _exchangeRateService, _salesInvoiceService, _gLNonBaseCurrencyService);

            _receiptVoucherService.ReconcileObject(rv, DateTime.Today.AddDays(10), _receiptVoucherDetailService, _cashMutationService, _cashBankService, _receivableService,
                                                   _accountService, _generalLedgerJournalService, _closingService, _currencyService, _exchangeRateService, _salesInvoiceService, _gLNonBaseCurrencyService);
        }

        // @PurchaseBuilder
        public void PopulatePurchaseOrderAndPurchaseReceival()
        {
            TimeSpan purchaseDate = new TimeSpan(10, 0, 0, 0);
            TimeSpan receivedDate = new TimeSpan(3, 0, 0, 0);
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
                Quantity = po1a.Quantity
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

        public void PopulatePurchaseInvoice()
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

        public void PopulatePaymentVoucher()
        {
            _purchaseInvoiceService.ConfirmObject(pi1, DateTime.Today, _purchaseInvoiceDetailService, _purchaseOrderService, _purchaseReceivalService,
                                                  _purchaseReceivalDetailService, _payableService, _accountService, _generalLedgerJournalService, _closingService, 
                                                  _currencyService, _exchangeRateService, _gLNonBaseCurrencyService);
            _purchaseInvoiceService.ConfirmObject(pi2, DateTime.Today, _purchaseInvoiceDetailService, _purchaseOrderService, _purchaseReceivalService,
                                                  _purchaseReceivalDetailService, _payableService, _accountService, _generalLedgerJournalService, _closingService,
                                                  _currencyService, _exchangeRateService, _gLNonBaseCurrencyService);
            _purchaseInvoiceService.ConfirmObject(pi3, DateTime.Today, _purchaseInvoiceDetailService, _purchaseOrderService, _purchaseReceivalService,
                                                  _purchaseReceivalDetailService, _payableService, _accountService, _generalLedgerJournalService, _closingService,
                                                  _currencyService, _exchangeRateService, _gLNonBaseCurrencyService);

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
                Rate = 1
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
                                                 _accountService, _generalLedgerJournalService, _closingService,_currencyService, _gLNonBaseCurrencyService);

            _paymentVoucherService.ReconcileObject(pv, DateTime.Today.AddDays(10), _paymentVoucherDetailService, _cashMutationService, _cashBankService, _payableService,
                                                   _accountService, _generalLedgerJournalService, _closingService,_currencyService, _gLNonBaseCurrencyService);
        }


        public void PopulateSales()
        {
            salesOrder1 = new SalesOrder()
            {
                SalesDate = DateTime.Today,
                ContactId = contact.Id,
                CurrencyId = currencyIDR.Id
            };
            _salesOrderService.CreateObject(salesOrder1, _contactService);

            salesOrder2 = new SalesOrder()
            {
                SalesDate = DateTime.Today,
                ContactId = contact.Id,
                CurrencyId = currencyIDR.Id

            };
            _salesOrderService.CreateObject(salesOrder2, _contactService);

            salesOrder3 = new SalesOrder()
            {
                SalesDate = DateTime.Today,
                ContactId = contact.Id,
                CurrencyId = currencyIDR.Id

            };
            _salesOrderService.CreateObject(salesOrder3, _contactService);

            salesOD1a = new SalesOrderDetail()
            {
                SalesOrderId = salesOrder1.Id,
                ItemId = itemAccessory1.Id,
                Quantity = 2,
                Price = 52000
            };
            _salesOrderDetailService.CreateObject(salesOD1a, _salesOrderService, _itemService);

            salesOD1b = new SalesOrderDetail()
            {
                SalesOrderId = salesOrder1.Id,
                ItemId = itemAccessory2.Id,
                Quantity = 2,
                Price = 22000
            };
            _salesOrderDetailService.CreateObject(salesOD1b, _salesOrderService, _itemService);

            salesOD2a = new SalesOrderDetail()
            {
                SalesOrderId = salesOrder2.Id,
                ItemId = itemAccessory1.Id,
                Quantity = 2,
                Price = 51000
            };
            _salesOrderDetailService.CreateObject(salesOD2a, _salesOrderService, _itemService);

            salesOD2b = new SalesOrderDetail()
            {
                SalesOrderId = salesOrder2.Id,
                ItemId = itemAccessory2.Id,
                Quantity = 2,
                Price = 21000
            };
            _salesOrderDetailService.CreateObject(salesOD2b, _salesOrderService, _itemService);

            salesOD3a = new SalesOrderDetail()
            {
                SalesOrderId = salesOrder3.Id,
                ItemId = itemAccessory1.Id,
                Quantity = 2,
                Price = 53000
            };
            _salesOrderDetailService.CreateObject(salesOD3a, _salesOrderService, _itemService);

            salesOD3b = new SalesOrderDetail()
            {
                SalesOrderId = salesOrder3.Id,
                ItemId = itemAccessory2.Id,
                Quantity = 2,
                Price = 23000
            };
            _salesOrderDetailService.CreateObject(salesOD3b, _salesOrderService, _itemService);

            _salesOrderService.ConfirmObject(salesOrder1, DateTime.Today, _salesOrderDetailService, _stockMutationService, _itemService, _blanketService, _warehouseItemService);
            _salesOrderService.ConfirmObject(salesOrder2, DateTime.Today, _salesOrderDetailService, _stockMutationService, _itemService, _blanketService, _warehouseItemService);
            _salesOrderService.ConfirmObject(salesOrder3, DateTime.Today, _salesOrderDetailService, _stockMutationService, _itemService, _blanketService, _warehouseItemService);

            deliveryOrder1 = new DeliveryOrder()
            {
                DeliveryDate = DateTime.Today,
                SalesOrderId = salesOrder1.Id,
                WarehouseId = localWarehouse.Id,
            };
            _deliveryOrderService.CreateObject(deliveryOrder1, _salesOrderService, _warehouseService);

            deliveryOrder2 = new DeliveryOrder()
            {
                DeliveryDate = DateTime.Today,
                SalesOrderId = salesOrder2.Id,
                WarehouseId = localWarehouse.Id
            };
            _deliveryOrderService.CreateObject(deliveryOrder2, _salesOrderService, _warehouseService);

            deliveryOrder3 = new DeliveryOrder()
            {
                DeliveryDate = DateTime.Today,
                SalesOrderId = salesOrder3.Id,
                WarehouseId = localWarehouse.Id
            };
            _deliveryOrderService.CreateObject(deliveryOrder3, _salesOrderService, _warehouseService);

            deliveryOD1a = new DeliveryOrderDetail()
            {
                DeliveryOrderId = deliveryOrder1.Id,
                SalesOrderDetailId = salesOD1a.Id,
                ItemId = itemAccessory1.Id,
                Quantity = 2,
            };
            _deliveryOrderDetailService.CreateObject(deliveryOD1a, _deliveryOrderService, _salesOrderDetailService, _salesOrderService, _itemService);

            deliveryOD1b = new DeliveryOrderDetail()
            {
                DeliveryOrderId = deliveryOrder1.Id,
                SalesOrderDetailId = salesOD1b.Id,
                ItemId = itemAccessory2.Id,
                Quantity = 2,
            };
            _deliveryOrderDetailService.CreateObject(deliveryOD1b, _deliveryOrderService, _salesOrderDetailService, _salesOrderService, _itemService);

            deliveryOD2a = new DeliveryOrderDetail()
            {
                DeliveryOrderId = deliveryOrder2.Id,
                SalesOrderDetailId = salesOD2a.Id,
                ItemId = itemAccessory1.Id,
                Quantity = 2,
            };
            _deliveryOrderDetailService.CreateObject(deliveryOD2a, _deliveryOrderService, _salesOrderDetailService, _salesOrderService, _itemService);

            deliveryOD2b = new DeliveryOrderDetail()
            {
                DeliveryOrderId = deliveryOrder2.Id,
                SalesOrderDetailId = salesOD2b.Id,
                ItemId = itemAccessory2.Id,
                Quantity = 2
            };
            _deliveryOrderDetailService.CreateObject(deliveryOD2b, _deliveryOrderService, _salesOrderDetailService, _salesOrderService, _itemService);

            deliveryOD3a = new DeliveryOrderDetail()
            {
                DeliveryOrderId = deliveryOrder3.Id,
                SalesOrderDetailId = salesOD3a.Id,
                ItemId = itemAccessory1.Id,
                Quantity = 2
            };
            _deliveryOrderDetailService.CreateObject(deliveryOD3a, _deliveryOrderService, _salesOrderDetailService, _salesOrderService, _itemService);

            deliveryOD3b = new DeliveryOrderDetail()
            {
                DeliveryOrderId = deliveryOrder3.Id,
                SalesOrderDetailId = salesOD3b.Id,
                ItemId = itemAccessory2.Id,
                Quantity = 2
            };
            _deliveryOrderDetailService.CreateObject(deliveryOD3b, _deliveryOrderService, _salesOrderDetailService, _salesOrderService, _itemService);

            _deliveryOrderService.ConfirmObject(deliveryOrder1, DateTime.Today, _deliveryOrderDetailService, _salesOrderService, _salesOrderDetailService, _stockMutationService,
                                                _itemService, _blanketService, _warehouseItemService, _accountService, _generalLedgerJournalService, _closingService, _serviceCostService,
                                                _temporaryDeliveryOrderDetailService, _temporaryDeliveryOrderService, _customerStockMutationService, _customerItemService, _currencyService, _exchangeRateService);
            _deliveryOrderService.ConfirmObject(deliveryOrder2, DateTime.Today, _deliveryOrderDetailService, _salesOrderService, _salesOrderDetailService, _stockMutationService,
                                                _itemService, _blanketService, _warehouseItemService, _accountService, _generalLedgerJournalService, _closingService, _serviceCostService,
                                                _temporaryDeliveryOrderDetailService, _temporaryDeliveryOrderService, _customerStockMutationService, _customerItemService, _currencyService, _exchangeRateService);
            _deliveryOrderService.ConfirmObject(deliveryOrder3, DateTime.Today, _deliveryOrderDetailService, _salesOrderService, _salesOrderDetailService, _stockMutationService,
                                                _itemService, _blanketService, _warehouseItemService, _accountService, _generalLedgerJournalService, _closingService, _serviceCostService,
                                                _temporaryDeliveryOrderDetailService, _temporaryDeliveryOrderService, _customerStockMutationService, _customerItemService, _currencyService, _exchangeRateService);

            salesInvoice1 = new SalesInvoice()
            {
                DeliveryOrderId = deliveryOrder1.Id,
                InvoiceDate = DateTime.Today,
                DueDate = DateTime.Today.AddDays(7),
                Tax = 0,
                Discount = 0,
                CurrencyId = currencyIDR.Id,
            };
            _salesInvoiceService.CreateObject(salesInvoice1, _deliveryOrderService);

            salesInvoice2 = new SalesInvoice()
            {
                DeliveryOrderId = deliveryOrder2.Id,
                InvoiceDate = DateTime.Today,
                DueDate = DateTime.Today.AddDays(7),
                Tax = 0,
                Discount = 0,
                CurrencyId = currencyIDR.Id,
            };
            _salesInvoiceService.CreateObject(salesInvoice2, _deliveryOrderService);

            salesInvoice3 = new SalesInvoice()
            {
                DeliveryOrderId = deliveryOrder3.Id,
                InvoiceDate = DateTime.Today,
                DueDate = DateTime.Today.AddDays(7),
                Tax = 0,
                Discount = 0,
                CurrencyId = currencyIDR.Id
            };
            _salesInvoiceService.CreateObject(salesInvoice3, _deliveryOrderService);

            salesID1a = new SalesInvoiceDetail()
            {
                SalesInvoiceId = salesInvoice1.Id,
                DeliveryOrderDetailId = deliveryOD1a.Id,
                Quantity = 2,
            };
            _salesInvoiceDetailService.CreateObject(salesID1a, _salesInvoiceService, _salesOrderDetailService, _deliveryOrderDetailService);

            salesID1b = new SalesInvoiceDetail()
            {
                SalesInvoiceId = salesInvoice1.Id,
                DeliveryOrderDetailId = deliveryOD1b.Id,
                Quantity = 2,
            };
            _salesInvoiceDetailService.CreateObject(salesID1b, _salesInvoiceService, _salesOrderDetailService, _deliveryOrderDetailService);

            salesID2a = new SalesInvoiceDetail()
            {
                SalesInvoiceId = salesInvoice2.Id,
                DeliveryOrderDetailId = deliveryOD2a.Id,
                Quantity = 2,
            };
            _salesInvoiceDetailService.CreateObject(salesID2a, _salesInvoiceService, _salesOrderDetailService, _deliveryOrderDetailService);

            salesID2b = new SalesInvoiceDetail()
            {
                SalesInvoiceId = salesInvoice2.Id,
                DeliveryOrderDetailId = deliveryOD2b.Id,
                Quantity = 2,
            };
            _salesInvoiceDetailService.CreateObject(salesID2b, _salesInvoiceService, _salesOrderDetailService, _deliveryOrderDetailService);

            salesID3a = new SalesInvoiceDetail()
            {
                SalesInvoiceId = salesInvoice3.Id,
                DeliveryOrderDetailId = deliveryOD3a.Id,
                Quantity = 2,
            };
            _salesInvoiceDetailService.CreateObject(salesID3a, _salesInvoiceService, _salesOrderDetailService, _deliveryOrderDetailService);

            salesID3b = new SalesInvoiceDetail()
            {
                SalesInvoiceId = salesInvoice3.Id,
                DeliveryOrderDetailId = deliveryOD3b.Id,
                Quantity = 2,
            };
            _salesInvoiceDetailService.CreateObject(salesID3b, _salesInvoiceService, _salesOrderDetailService, _deliveryOrderDetailService);

            _salesInvoiceService.ConfirmObject(salesInvoice1, DateTime.Today, _salesInvoiceDetailService, _salesOrderService, _salesOrderDetailService, _deliveryOrderService,
                                               _deliveryOrderDetailService, _receivableService, _accountService, _generalLedgerJournalService, _closingService,
                                               _serviceCostService, _rollerBuilderService, _itemService, _contactService, _exchangeRateService, _currencyService, _gLNonBaseCurrencyService);
            _salesInvoiceService.ConfirmObject(salesInvoice2, DateTime.Today, _salesInvoiceDetailService, _salesOrderService, _salesOrderDetailService, _deliveryOrderService,
                                               _deliveryOrderDetailService, _receivableService, _accountService, _generalLedgerJournalService, _closingService,
                                               _serviceCostService, _rollerBuilderService, _itemService, _contactService, _exchangeRateService, _currencyService, _gLNonBaseCurrencyService);
            _salesInvoiceService.ConfirmObject(salesInvoice3, DateTime.Today, _salesInvoiceDetailService, _salesOrderService, _salesOrderDetailService, _deliveryOrderService,
                                               _deliveryOrderDetailService, _receivableService, _accountService, _generalLedgerJournalService, _closingService,
                                               _serviceCostService, _rollerBuilderService, _itemService, _contactService, _exchangeRateService, _currencyService, _gLNonBaseCurrencyService);

            receiptVoucher1 = new ReceiptVoucher()
            {
                CashBankId = cashBank1.Id,
                ContactId = contact.Id,
                DueDate = DateTime.Today.AddDays(6),
                IsGBCH = false,
                ReceiptDate = DateTime.Today,
                RateToIDR = 1
            };
            _receiptVoucherService.CreateObject(receiptVoucher1, _receiptVoucherDetailService, _receivableService, _contactService, _cashBankService);

            receiptVoucher2 = new ReceiptVoucher()
            {
                CashBankId = cashBank1.Id,
                ContactId = contact.Id,
                DueDate = DateTime.Today.AddDays(6),
                IsGBCH = false,
                ReceiptDate = DateTime.Today,
                RateToIDR = 1
            };
            _receiptVoucherService.CreateObject(receiptVoucher2, _receiptVoucherDetailService, _receivableService, _contactService, _cashBankService);

            receiptVoucher3 = new ReceiptVoucher()
            {
                CashBankId = cashBank1.Id,
                ContactId = contact.Id,
                DueDate = DateTime.Today.AddDays(6),
                IsGBCH = false,
                ReceiptDate = DateTime.Today,
                RateToIDR = 1
            };
            _receiptVoucherService.CreateObject(receiptVoucher3, _receiptVoucherDetailService, _receivableService, _contactService, _cashBankService);

            receiptVD1a = new ReceiptVoucherDetail()
            {
                ReceiptVoucherId = receiptVoucher1.Id,
                Amount = salesID1a.Amount + salesID1b.Amount,
                AmountPaid = salesID1a.Amount + salesID1b.Amount,
                ReceivableId = _receivableService.GetObjectBySource(Constant.ReceivableSource.SalesInvoice, salesInvoice1.Id).Id,
                Rate = 1
            };
            _receiptVoucherDetailService.CreateObject(receiptVD1a, _receiptVoucherService, _cashBankService, _receivableService, _currencyService);

            receiptVD2a = new ReceiptVoucherDetail()
            {
                ReceiptVoucherId = receiptVoucher2.Id,
                Amount = salesID2a.Amount + salesID2b.Amount,
                AmountPaid = salesID2a.Amount + salesID2b.Amount,
                ReceivableId = _receivableService.GetObjectBySource(Constant.ReceivableSource.SalesInvoice, salesInvoice2.Id).Id,
                Rate = 1
            };
            _receiptVoucherDetailService.CreateObject(receiptVD2a, _receiptVoucherService, _cashBankService, _receivableService, _currencyService);

            receiptVD3a = new ReceiptVoucherDetail()
            {
                ReceiptVoucherId = receiptVoucher3.Id,
                Amount = salesID3a.Amount + salesID3b.Amount,
                AmountPaid = salesID3a.Amount + salesID3b.Amount,
                ReceivableId = _receivableService.GetObjectBySource(Constant.ReceivableSource.SalesInvoice, salesInvoice3.Id).Id,
                Rate = 1
            };
            _receiptVoucherDetailService.CreateObject(receiptVD3a, _receiptVoucherService, _cashBankService, _receivableService, _currencyService);

            _receiptVoucherService.ConfirmObject(receiptVoucher1, DateTime.Now, _receiptVoucherDetailService, _cashBankService, _receivableService,
                                                 _cashMutationService, _accountService, _generalLedgerJournalService, _closingService,_currencyService,
                                                 _exchangeRateService, _salesInvoiceService, _gLNonBaseCurrencyService);
            _receiptVoucherService.ConfirmObject(receiptVoucher2, DateTime.Now, _receiptVoucherDetailService, _cashBankService, _receivableService,
                                                 _cashMutationService, _accountService, _generalLedgerJournalService, _closingService,_currencyService,
                                                 _exchangeRateService, _salesInvoiceService, _gLNonBaseCurrencyService);
            _receiptVoucherService.ConfirmObject(receiptVoucher3, DateTime.Now, _receiptVoucherDetailService, _cashBankService, _receivableService,
                                                 _cashMutationService, _accountService, _generalLedgerJournalService, _closingService,_currencyService,
                                                 _exchangeRateService, _salesInvoiceService, _gLNonBaseCurrencyService);
        }

        public void PopulateVirtualOrder()
        {
            virtualOrder1 = new VirtualOrder()
            {
                Code = "VO001",
                CurrencyId = currencyIDR.Id,
                ContactId = contact.Id,
                OrderType = Constant.OrderTypeCase.Consignment,
                OrderDate = DateTime.Today
            };
            virtualOrder1 = _virtualOrderService.CreateObject(virtualOrder1, _contactService);

            virtualOrder2 = new VirtualOrder()
            {
                Code = "VO002",
                CurrencyId = currencyEUR.Id,
                ContactId = contact.Id,
                OrderType = Constant.OrderTypeCase.TrialOrder,
                OrderDate = DateTime.Today,
            };
            virtualOrder2 = _virtualOrderService.CreateObject(virtualOrder2, _contactService);

            voD1a = new VirtualOrderDetail()
            {
                ItemId = itemAccessory1.Id,
                Quantity = 2,
                VirtualOrderId = virtualOrder1.Id,
                Price = 50
            };
            voD1a = _virtualOrderDetailService.CreateObject(voD1a, _virtualOrderService, _itemService);

            voD1b = new VirtualOrderDetail()
            {
                ItemId = itemAccessory2.Id,
                Quantity = 2,
                VirtualOrderId = virtualOrder1.Id,
                Price = 35
            };
            voD1b = _virtualOrderDetailService.CreateObject(voD1b, _virtualOrderService, _itemService);

            voD2a = new VirtualOrderDetail()
            {
                ItemId = itemAdhesiveBlanket.Id,    
                Quantity = 2,
                VirtualOrderId = virtualOrder2.Id,
                Price = 50
            };
            voD2a = _virtualOrderDetailService.CreateObject(voD2a, _virtualOrderService, _itemService);

            voD2b = new VirtualOrderDetail()
            {
                ItemId = itemAdhesiveRoller.Id,
                Quantity = 3,
                VirtualOrderId = virtualOrder2.Id,
                Price = 35
            };
            voD2b = _virtualOrderDetailService.CreateObject(voD2b, _virtualOrderService, _itemService);

            _virtualOrderService.ConfirmObject(virtualOrder1, DateTime.Now, _virtualOrderDetailService,
                                               _stockMutationService, _itemService, _blanketService, _warehouseItemService);
            _virtualOrderService.ConfirmObject(virtualOrder2, DateTime.Now, _virtualOrderDetailService,
                                               _stockMutationService, _itemService, _blanketService, _warehouseItemService);

            temporaryDeliveryOrder1 = new TemporaryDeliveryOrder()
            {
                VirtualOrderId = virtualOrder1.Id,
                OrderType = virtualOrder1.OrderType,
                WarehouseId = localWarehouse.Id,
                DeliveryDate = DateTime.Today,                
            };
            temporaryDeliveryOrder1 = _temporaryDeliveryOrderService.CreateObject(temporaryDeliveryOrder1, _virtualOrderService, _deliveryOrderService, _warehouseService);

            temporaryDeliveryOrder2 = new TemporaryDeliveryOrder()
            {
                VirtualOrderId = virtualOrder2.Id,
                OrderType = virtualOrder2.OrderType,
                WarehouseId = localWarehouse.Id,
                DeliveryDate = DateTime.Today
            };
            temporaryDeliveryOrder2 = _temporaryDeliveryOrderService.CreateObject(temporaryDeliveryOrder2, _virtualOrderService, _deliveryOrderService, _warehouseService);

            tdoD1a = new TemporaryDeliveryOrderDetail()
            {
                VirtualOrderDetailId = voD1a.Id,
                ItemId = voD1a.ItemId,
                Quantity = voD1a.Quantity,
                TemporaryDeliveryOrderId = temporaryDeliveryOrder1.Id,
            };
            tdoD1a = _temporaryDeliveryOrderDetailService.CreateObject(tdoD1a, _temporaryDeliveryOrderService, _virtualOrderDetailService,
                                                                       _salesOrderDetailService, _deliveryOrderService, _itemService);

            tdoD1b = new TemporaryDeliveryOrderDetail()
            {
                VirtualOrderDetailId = voD1b.Id,
                ItemId = voD1b.ItemId,
                Quantity = voD1b.Quantity,
                TemporaryDeliveryOrderId = temporaryDeliveryOrder1.Id,
            };
            tdoD1b = _temporaryDeliveryOrderDetailService.CreateObject(tdoD1b, _temporaryDeliveryOrderService, _virtualOrderDetailService,
                                                                       _salesOrderDetailService, _deliveryOrderService, _itemService);

            tdoD2a = new TemporaryDeliveryOrderDetail()
            {
                VirtualOrderDetailId = voD2a.Id,
                ItemId = voD2a.ItemId,
                Quantity = voD2a.Quantity,
                TemporaryDeliveryOrderId = temporaryDeliveryOrder2.Id,
            };
            tdoD2a = _temporaryDeliveryOrderDetailService.CreateObject(tdoD2a, _temporaryDeliveryOrderService, _virtualOrderDetailService,
                                                                       _salesOrderDetailService, _deliveryOrderService, _itemService);

            tdoD2b = new TemporaryDeliveryOrderDetail()
            {
                VirtualOrderDetailId = voD2b.Id,
                ItemId = voD2b.ItemId,
                Quantity = voD2b.Quantity,
                TemporaryDeliveryOrderId = temporaryDeliveryOrder2.Id,
            };
            tdoD2b = _temporaryDeliveryOrderDetailService.CreateObject(tdoD2b, _temporaryDeliveryOrderService, _virtualOrderDetailService,
                                                                       _salesOrderDetailService, _deliveryOrderService, _itemService);

            temporaryDeliveryOrder1 = _temporaryDeliveryOrderService.ConfirmObject(temporaryDeliveryOrder1, DateTime.Today, _temporaryDeliveryOrderDetailService,
                                      _virtualOrderService, _virtualOrderDetailService, _deliveryOrderService, _deliveryOrderDetailService, _salesOrderDetailService,
                                      _stockMutationService, _itemService, _blanketService, _warehouseItemService);

            temporaryDeliveryOrder2 = _temporaryDeliveryOrderService.ConfirmObject(temporaryDeliveryOrder2, DateTime.Today, _temporaryDeliveryOrderDetailService,
                                      _virtualOrderService, _virtualOrderDetailService, _deliveryOrderService, _deliveryOrderDetailService, _salesOrderDetailService,
                                      _stockMutationService, _itemService, _blanketService, _warehouseItemService);

            tdoc1 = new TemporaryDeliveryOrderClearance()
            {
                ClearanceDate = DateTime.Today,
                TemporaryDeliveryOrderId = temporaryDeliveryOrder1.Id,
                IsWaste = true,                
            };
            _temporaryDeliveryOrderClearanceService.CreateObject(tdoc1, _temporaryDeliveryOrderService);

            tdoc2 = new TemporaryDeliveryOrderClearance()
            {
                ClearanceDate = DateTime.Today,
                TemporaryDeliveryOrderId = temporaryDeliveryOrder1.Id,
                IsWaste = false,
            };
            _temporaryDeliveryOrderClearanceService.CreateObject(tdoc2, _temporaryDeliveryOrderService);

            tdoc3 = new TemporaryDeliveryOrderClearance()
            {
                ClearanceDate = DateTime.Today,
                TemporaryDeliveryOrderId = temporaryDeliveryOrder2.Id,
                IsWaste = true,
            };
            _temporaryDeliveryOrderClearanceService.CreateObject(tdoc3, _temporaryDeliveryOrderService);

            tdoc4 = new TemporaryDeliveryOrderClearance()
            {
                ClearanceDate = DateTime.Today,
                TemporaryDeliveryOrderId = temporaryDeliveryOrder2.Id,
                IsWaste = false,
            };
            _temporaryDeliveryOrderClearanceService.CreateObject(tdoc4, _temporaryDeliveryOrderService);

            tdoc5 = new TemporaryDeliveryOrderClearance()
            {
                ClearanceDate = DateTime.Today,
                TemporaryDeliveryOrderId = temporaryDeliveryOrder2.Id,
                IsWaste = false,
            };
            _temporaryDeliveryOrderClearanceService.CreateObject(tdoc5, _temporaryDeliveryOrderService);

            tdocd1a = new TemporaryDeliveryOrderClearanceDetail()
            {
                Quantity = 1,
                TemporaryDeliveryOrderClearanceId = tdoc1.Id,
                TemporaryDeliveryOrderDetailId = tdoD1a.Id,
            };
            _temporaryDeliveryOrderClearanceDetailService.CreateObject(tdocd1a, _temporaryDeliveryOrderClearanceService, _temporaryDeliveryOrderDetailService);

            tdocd2a = new TemporaryDeliveryOrderClearanceDetail()
            {
                Quantity = 1,
                TemporaryDeliveryOrderClearanceId = tdoc2.Id,
                TemporaryDeliveryOrderDetailId = tdoD1a.Id,
                SellingPrice = 5000
            };
            _temporaryDeliveryOrderClearanceDetailService.CreateObject(tdocd2a, _temporaryDeliveryOrderClearanceService, _temporaryDeliveryOrderDetailService);

            tdocd2b = new TemporaryDeliveryOrderClearanceDetail()
            {
                Quantity = 2,
                TemporaryDeliveryOrderClearanceId = tdoc2.Id,
                TemporaryDeliveryOrderDetailId = tdoD1b.Id,
                SellingPrice = 6000
            };
            _temporaryDeliveryOrderClearanceDetailService.CreateObject(tdocd2b, _temporaryDeliveryOrderClearanceService, _temporaryDeliveryOrderDetailService);

            tdocd3a = new TemporaryDeliveryOrderClearanceDetail()
            {
                Quantity = 1,
                TemporaryDeliveryOrderClearanceId = tdoc3.Id,
                TemporaryDeliveryOrderDetailId = tdoD2a.Id,
            };
            _temporaryDeliveryOrderClearanceDetailService.CreateObject(tdocd3a, _temporaryDeliveryOrderClearanceService, _temporaryDeliveryOrderDetailService);

            tdocd3b = new TemporaryDeliveryOrderClearanceDetail()
            {
                Quantity = 1,
                TemporaryDeliveryOrderClearanceId = tdoc3.Id,
                TemporaryDeliveryOrderDetailId = tdoD2b.Id,
            };
            _temporaryDeliveryOrderClearanceDetailService.CreateObject(tdocd3b, _temporaryDeliveryOrderClearanceService, _temporaryDeliveryOrderDetailService);

            tdocd4a = new TemporaryDeliveryOrderClearanceDetail()
            {
                Quantity = 1,
                TemporaryDeliveryOrderClearanceId = tdoc4.Id,
                TemporaryDeliveryOrderDetailId = tdoD2a.Id,
                SellingPrice = 25000
            };
            _temporaryDeliveryOrderClearanceDetailService.CreateObject(tdocd4a, _temporaryDeliveryOrderClearanceService, _temporaryDeliveryOrderDetailService);

            tdocd4b = new TemporaryDeliveryOrderClearanceDetail()
            {
                Quantity = 1,
                TemporaryDeliveryOrderClearanceId = tdoc4.Id,
                TemporaryDeliveryOrderDetailId = tdoD2b.Id,
                SellingPrice = 30000
            };
            _temporaryDeliveryOrderClearanceDetailService.CreateObject(tdocd4b, _temporaryDeliveryOrderClearanceService, _temporaryDeliveryOrderDetailService);

            tdocd5b = new TemporaryDeliveryOrderClearanceDetail()
            {
                Quantity = 1,
                TemporaryDeliveryOrderClearanceId = tdoc5.Id,
                TemporaryDeliveryOrderDetailId = tdoD2b.Id,
                SellingPrice = 35000
            };
            _temporaryDeliveryOrderClearanceDetailService.CreateObject(tdocd5b, _temporaryDeliveryOrderClearanceService, _temporaryDeliveryOrderDetailService);

            _temporaryDeliveryOrderClearanceService.ConfirmObject(tdoc1, DateTime.Today, _temporaryDeliveryOrderClearanceDetailService, _stockMutationService, _itemService,
                                            _blanketService, _warehouseItemService, _temporaryDeliveryOrderService, _temporaryDeliveryOrderDetailService, _generalLedgerJournalService, _accountService,
                                            _closingService);

            _temporaryDeliveryOrderClearanceService.ConfirmObject(tdoc2, DateTime.Today, _temporaryDeliveryOrderClearanceDetailService, _stockMutationService, _itemService,
                                            _blanketService, _warehouseItemService, _temporaryDeliveryOrderService, _temporaryDeliveryOrderDetailService, _generalLedgerJournalService, _accountService,
                                            _closingService);

            _temporaryDeliveryOrderClearanceService.ConfirmObject(tdoc3, DateTime.Today, _temporaryDeliveryOrderClearanceDetailService, _stockMutationService, _itemService,
                                            _blanketService, _warehouseItemService, _temporaryDeliveryOrderService, _temporaryDeliveryOrderDetailService, _generalLedgerJournalService, _accountService,
                                            _closingService);

            _temporaryDeliveryOrderClearanceService.ConfirmObject(tdoc4, DateTime.Today, _temporaryDeliveryOrderClearanceDetailService, _stockMutationService, _itemService,
                                            _blanketService, _warehouseItemService, _temporaryDeliveryOrderService, _temporaryDeliveryOrderDetailService, _generalLedgerJournalService, _accountService,
                                            _closingService);

            _temporaryDeliveryOrderClearanceService.ConfirmObject(tdoc5, DateTime.Today, _temporaryDeliveryOrderClearanceDetailService, _stockMutationService, _itemService,
                                            _blanketService, _warehouseItemService, _temporaryDeliveryOrderService, _temporaryDeliveryOrderDetailService, _generalLedgerJournalService, _accountService,
                                            _closingService);

            sales1 = new SalesOrder()
            {
                OrderCode = virtualOrder1.Code,
                OrderType = virtualOrder1.OrderType,
                Code = "SO001",
                ContactId = virtualOrder1.ContactId,
                CurrencyId = virtualOrder1.CurrencyId,
                SalesDate = DateTime.Today,
                NomorSurat = "SO001.12.2014"
            };
            sales1 = _salesOrderService.CreateObject(sales1, _contactService);

            sales2 = new SalesOrder()
            {
                OrderCode = virtualOrder2.Code,
                OrderType = virtualOrder2.OrderType,
                Code = "SO002",
                ContactId = virtualOrder2.ContactId,
                CurrencyId = virtualOrder2.CurrencyId,
                SalesDate = DateTime.Today,
                NomorSurat = "SO002.12.2014"
            };
            sales2 = _salesOrderService.CreateObject(sales2, _contactService);

            salesDetail1a = new SalesOrderDetail()
            {
                SalesOrderId = sales1.Id,
                Code = "SOD001a",
                OrderCode = voD1a.Code,
                ItemId = voD1a.ItemId,
                Quantity = voD1a.Quantity,
                Price = tdocd2a.SellingPrice,
            };
            salesDetail1a = _salesOrderDetailService.CreateObject(salesDetail1a, _salesOrderService, _itemService);

            salesDetail1b = new SalesOrderDetail()
            {
                SalesOrderId = sales1.Id,
                Code = "SOD001b",
                OrderCode = voD1b.Code,
                ItemId = voD1b.ItemId,
                Quantity = voD1b.Quantity,
                Price = tdocd2b.SellingPrice,
            };
            salesDetail1b = _salesOrderDetailService.CreateObject(salesDetail1b, _salesOrderService, _itemService);

            salesDetail2a = new SalesOrderDetail()
            {
                SalesOrderId = sales2.Id,
                Code = "SOD002a",
                OrderCode = voD2a.Code,
                ItemId = voD2a.ItemId,
                Quantity = voD2a.Quantity,
                Price = tdocd4a.SellingPrice,
            };
            salesDetail2a = _salesOrderDetailService.CreateObject(salesDetail2a, _salesOrderService, _itemService);

            salesDetail2b = new SalesOrderDetail()
            {
                SalesOrderId = sales2.Id,
                Code = "SOD002b",
                OrderCode = voD2b.Code,
                ItemId = voD2b.ItemId,
                Quantity = voD2b.Quantity,
                Price = tdocd4b.SellingPrice,
            };
            salesDetail2b = _salesOrderDetailService.CreateObject(salesDetail2b, _salesOrderService, _itemService);

            sales1 = _salesOrderService.ConfirmObject(sales1, DateTime.Today, _salesOrderDetailService, _stockMutationService, _itemService,
                                                      _blanketService, _warehouseItemService);

            sales2 = _salesOrderService.ConfirmObject(sales2, DateTime.Today, _salesOrderDetailService, _stockMutationService, _itemService,
                                                      _blanketService, _warehouseItemService);

            GramSalesOrder1 = new SalesOrder()
            {
                ContactId = contact.Id,
                CurrencyId = currencyIDR.Id,
                OrderType = Constant.OrderTypeCase.SalesOrder,
                Code = "GramSO001",
                SalesDate = DateTime.Today,
                NomorSurat = "SO.G.1.1.2015"
            };
            GramSalesOrder1 = _salesOrderService.CreateObject(GramSalesOrder1, _contactService);

            GramSOD1a = new SalesOrderDetail()
            {
                ItemId = itemAccessory1.Id,
                Code = "GramSO001A",
                Quantity = 100,
                Price = 25000,
                SalesOrderId = GramSalesOrder1.Id
            };
            GramSOD1a = _salesOrderDetailService.CreateObject(GramSOD1a, _salesOrderService, _itemService);

            GramSOD1b = new SalesOrderDetail()
            {
                ItemId = itemAccessory2.Id,
                Code = "GramSO001B",
                Quantity = 80,
                Price = 30000,
                SalesOrderId = GramSalesOrder1.Id,
            };
            GramSOD1b = _salesOrderDetailService.CreateObject(GramSOD1b, _salesOrderService, _itemService);

            GramSalesOrder1 = _salesOrderService.ConfirmObject(GramSalesOrder1, DateTime.Today, _salesOrderDetailService,
                              _stockMutationService, _itemService, _blanketService, _warehouseItemService);

            GramDeliveryOrder1 = new DeliveryOrder()
            {
                NomorSurat = "DO.G.1.1.2015",
                SalesOrderId = GramSalesOrder1.Id,
                Code = "GramDO001",
                WarehouseId = localWarehouse.Id,
                DeliveryDate = DateTime.Today,
            };
            GramDeliveryOrder1 = _deliveryOrderService.CreateObject(GramDeliveryOrder1, _salesOrderService, _warehouseService);

            GramTDO1 = new TemporaryDeliveryOrder()
            {
                DeliveryOrderId = GramDeliveryOrder1.Id,
                OrderType = Constant.OrderTypeCase.PartDeliveryOrder,
                NomorSurat = "DO.G.1.1.2015.T1",
                WarehouseId = GramDeliveryOrder1.WarehouseId,
                Code = "GramDO001_T1",
                DeliveryDate = DateTime.Today,
            };
            GramTDO1 = _temporaryDeliveryOrderService.CreateObject(GramTDO1, _virtualOrderService, _deliveryOrderService, _warehouseService);

            GramTDO2 = new TemporaryDeliveryOrder()
            {
                DeliveryOrderId = GramDeliveryOrder1.Id,
                OrderType = Constant.OrderTypeCase.PartDeliveryOrder,
                NomorSurat = "DO.G.1.1.2015.T2",
                WarehouseId = GramDeliveryOrder1.WarehouseId,
                Code = "GramDO001_T2",
                DeliveryDate = DateTime.Today,
            };
            GramTDO2 = _temporaryDeliveryOrderService.CreateObject(GramTDO2, _virtualOrderService, _deliveryOrderService, _warehouseService);

            GramTDOD1a = new TemporaryDeliveryOrderDetail()
            {
                TemporaryDeliveryOrderId = GramTDO1.Id,
                ItemId = GramSOD1a.ItemId,
                Quantity = GramSOD1a.Quantity - 50,
                SalesOrderDetailId = GramSOD1a.Id,
                Code = "GramTDOD1a",
            };
            GramTDOD1a = _temporaryDeliveryOrderDetailService.CreateObject(GramTDOD1a, _temporaryDeliveryOrderService, _virtualOrderDetailService,
                                                                           _salesOrderDetailService, _deliveryOrderService, _itemService);

            GramTDOD2a = new TemporaryDeliveryOrderDetail()
            {
                TemporaryDeliveryOrderId = GramTDO2.Id,
                ItemId = GramSOD1a.ItemId,
                Quantity = 50,
                SalesOrderDetailId = GramSOD1a.Id,
                Code = "GramTDOD2a",
            };
            GramTDOD2a = _temporaryDeliveryOrderDetailService.CreateObject(GramTDOD2a, _temporaryDeliveryOrderService, _virtualOrderDetailService,
                                                                           _salesOrderDetailService, _deliveryOrderService, _itemService);

            GramTDOD1b = new TemporaryDeliveryOrderDetail()
            {
                TemporaryDeliveryOrderId = GramTDO1.Id,
                ItemId = GramSOD1b.ItemId,
                Quantity = GramSOD1b.Quantity - 50,
                SalesOrderDetailId = GramSOD1b.Id,
                Code = "GramTDOD1b",
            };
            GramTDOD1b = _temporaryDeliveryOrderDetailService.CreateObject(GramTDOD1b, _temporaryDeliveryOrderService, _virtualOrderDetailService,
                                                                           _salesOrderDetailService, _deliveryOrderService, _itemService);

            GramTDOD2b = new TemporaryDeliveryOrderDetail()
            {
                TemporaryDeliveryOrderId = GramTDO2.Id,
                ItemId = GramSOD1b.ItemId,
                Quantity = 50,
                SalesOrderDetailId = GramSOD1b.Id,
                Code = "GramTDOD2b",
            };
            GramTDOD2b = _temporaryDeliveryOrderDetailService.CreateObject(GramTDOD2b, _temporaryDeliveryOrderService, _virtualOrderDetailService,
                                                                           _salesOrderDetailService, _deliveryOrderService, _itemService);

            GramTDO1 = _temporaryDeliveryOrderService.ConfirmObject(GramTDO1, DateTime.Today, _temporaryDeliveryOrderDetailService, _virtualOrderService, _virtualOrderDetailService,
                       _deliveryOrderService, _deliveryOrderDetailService, _salesOrderDetailService, _stockMutationService, _itemService, _blanketService, _warehouseItemService);

            GramTDO2 = _temporaryDeliveryOrderService.ConfirmObject(GramTDO2, DateTime.Today, _temporaryDeliveryOrderDetailService, _virtualOrderService, _virtualOrderDetailService,
                       _deliveryOrderService, _deliveryOrderDetailService, _salesOrderDetailService, _stockMutationService, _itemService, _blanketService, _warehouseItemService);

            GramTDOC1R = new TemporaryDeliveryOrderClearance()
            {
                ClearanceDate = DateTime.Today,
                Code = "GramTDOC1R",
                TemporaryDeliveryOrderId = GramTDO1.Id,
                IsWaste = false
            };
            GramTDOC1R = _temporaryDeliveryOrderClearanceService.CreateObject(GramTDOC1R, _temporaryDeliveryOrderService);

            /*
            GramTDOC1W = new TemporaryDeliveryOrderClearance()
            {
                ClearanceDate = DateTime.Today,
                Code = "GramTDOC1W",
                TemporaryDeliveryOrderId = GramTDO1.Id,
                IsWaste = true
            };
            GramTDOC1W = _temporaryDeliveryOrderClearanceService.CreateObject(GramTDOC1W, _temporaryDeliveryOrderService);
            */

            GramTDOC2R = new TemporaryDeliveryOrderClearance()
            {
                ClearanceDate = DateTime.Today,
                Code = "GramTDOC2R",
                TemporaryDeliveryOrderId = GramTDO2.Id,
                IsWaste = false
            };
            GramTDOC2R = _temporaryDeliveryOrderClearanceService.CreateObject(GramTDOC2R, _temporaryDeliveryOrderService);

            GramTDOC1Ra = new TemporaryDeliveryOrderClearanceDetail()
            {
                TemporaryDeliveryOrderClearanceId = GramTDOC1R.Id,
                TemporaryDeliveryOrderDetailId = GramTDOD1a.Id,
                Quantity = GramTDOD1a.Quantity,
                Code = "GramTDOC1Ra",
            };
            GramTDOC1Ra = _temporaryDeliveryOrderClearanceDetailService.CreateObject(GramTDOC1Ra, _temporaryDeliveryOrderClearanceService, _temporaryDeliveryOrderDetailService);

            GramTDOC1Rb = new TemporaryDeliveryOrderClearanceDetail()
            {
                TemporaryDeliveryOrderClearanceId = GramTDOC1R.Id,
                TemporaryDeliveryOrderDetailId = GramTDOD1b.Id,
                Quantity = GramTDOD1b.Quantity,
                Code = "GramTDOC1Rb",
            };
            GramTDOC1Rb = _temporaryDeliveryOrderClearanceDetailService.CreateObject(GramTDOC1Rb, _temporaryDeliveryOrderClearanceService, _temporaryDeliveryOrderDetailService);

            /*
            GramTDOC1Wa = new TemporaryDeliveryOrderClearanceDetail()
            {
                TemporaryDeliveryOrderClearanceId = GramTDOC1W.Id,
                TemporaryDeliveryOrderDetailId = GramTDOD1a.Id,
                Quantity = 5,
                Code = "GramTDOC1Wa",
            };
            GramTDOC1Wa = _temporaryDeliveryOrderClearanceDetailService.CreateObject(GramTDOC1Wa, _temporaryDeliveryOrderClearanceService, _temporaryDeliveryOrderDetailService);
            */

            GramTDOC2Ra = new TemporaryDeliveryOrderClearanceDetail()
            {
                TemporaryDeliveryOrderClearanceId = GramTDOC2R.Id,
                TemporaryDeliveryOrderDetailId = GramTDOD2a.Id,
                Quantity = GramTDOD2a.Quantity,
                Code = "GramTDOC2Ra",
            };
            GramTDOC2Ra = _temporaryDeliveryOrderClearanceDetailService.CreateObject(GramTDOC2Ra, _temporaryDeliveryOrderClearanceService, _temporaryDeliveryOrderDetailService);

            GramTDOC2Rb = new TemporaryDeliveryOrderClearanceDetail()
            {
                TemporaryDeliveryOrderClearanceId = GramTDOC2R.Id,
                TemporaryDeliveryOrderDetailId = GramTDOD2b.Id,
                Quantity = GramTDOD2b.Quantity,
                Code = "GramTDOC2Rb",
            };
            GramTDOC2Rb = _temporaryDeliveryOrderClearanceDetailService.CreateObject(GramTDOC2Rb, _temporaryDeliveryOrderClearanceService, _temporaryDeliveryOrderDetailService);

            GramTDOC1R = _temporaryDeliveryOrderClearanceService.ConfirmObject(GramTDOC1R, DateTime.Today, _temporaryDeliveryOrderClearanceDetailService, _stockMutationService, _itemService,
                         _blanketService, _warehouseItemService, _temporaryDeliveryOrderService, _temporaryDeliveryOrderDetailService, _generalLedgerJournalService, _accountService, _closingService);

            /* No waste for Part Delivery Order
            GramTDOC1W = _temporaryDeliveryOrderClearanceService.ConfirmObject(GramTDOC1W, DateTime.Today, _temporaryDeliveryOrderClearanceDetailService, _stockMutationService, _itemService,
                         _blanketService, _warehouseItemService, _temporaryDeliveryOrderService, _temporaryDeliveryOrderDetailService, _generalLedgerJournalService, _accountService, _closingService);
            */
            GramTDOC2R = _temporaryDeliveryOrderClearanceService.ConfirmObject(GramTDOC2R, DateTime.Today, _temporaryDeliveryOrderClearanceDetailService, _stockMutationService, _itemService,
                         _blanketService, _warehouseItemService, _temporaryDeliveryOrderService, _temporaryDeliveryOrderDetailService, _generalLedgerJournalService, _accountService, _closingService);

            GramDOD1a = new DeliveryOrderDetail()
            {
                DeliveryOrderId = GramDeliveryOrder1.Id,
                OrderCode = GramTDOD1a.Code + "," + GramTDOD2a.Code,
                OrderType = GramTDO1.OrderType,
                Code = "GramDOD1a",
                ItemId = GramSOD1a.ItemId,
                Quantity = GramSOD1a.Quantity,
                SalesOrderDetailId = GramSOD1a.Id,
            };
            GramDOD1a = _deliveryOrderDetailService.CreateObject(GramDOD1a, _deliveryOrderService, _salesOrderDetailService, _salesOrderService, _itemService);

            GramDOD1b = new DeliveryOrderDetail()
            {
                DeliveryOrderId = GramDeliveryOrder1.Id,
                OrderCode = GramTDOD1b.Code + "," + GramTDOD2b.Code,
                OrderType = GramTDO1.OrderType,
                Code = "GramDOD1b",
                ItemId = GramSOD1b.ItemId,
                Quantity = GramSOD1b.Quantity,
                SalesOrderDetailId = GramSOD1b.Id,
            };
            GramDOD1b = _deliveryOrderDetailService.CreateObject(GramDOD1b, _deliveryOrderService, _salesOrderDetailService, _salesOrderService, _itemService);

            GramDeliveryOrder1 = _deliveryOrderService.ConfirmObject(GramDeliveryOrder1, DateTime.Today, _deliveryOrderDetailService, _salesOrderService, _salesOrderDetailService,
                                                                     _stockMutationService, _itemService, _blanketService, _warehouseItemService, _accountService, _generalLedgerJournalService,
                                                                     _closingService, _serviceCostService, _temporaryDeliveryOrderDetailService, _temporaryDeliveryOrderService,
                                                                     _customerStockMutationService, _customerItemService, _currencyService, _exchangeRateService);
        }

        public void PopulateValidComb()
        {
            thisMonthClosing = new Closing()
            {
                BeginningPeriod = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1),
                EndDatePeriod = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.DaysInMonth(DateTime.Today.Year, DateTime.Today.Month)),
                Period = DateTime.Today.Month,
                YearPeriod = DateTime.Today.Year,
            };
            IList<ExchangeRateClosing> exchangeRateClosing = new List<ExchangeRateClosing>();
            ExchangeRateClosing usd = new ExchangeRateClosing()
            {
                CurrencyId = currencyUSD.Id,
                Rate = 12000
            };
            ExchangeRateClosing eur = new ExchangeRateClosing()
            {
                CurrencyId = currencyEUR.Id,
                Rate = 15000
            };
            exchangeRateClosing.Add(usd);
            exchangeRateClosing.Add(eur);
            _closingService.CreateObject(thisMonthClosing, exchangeRateClosing, _accountService, _validCombService, _exchangeRateClosingService);

            thisMonthClosing.ClosedAt = DateTime.Today;
            _closingService.CloseObject(thisMonthClosing, _accountService, _generalLedgerJournalService, _validCombService, _gLNonBaseCurrencyService, _exchangeRateClosingService, _vCNonBaseCurrencyService, _cashBankService);
        }
    }
}
