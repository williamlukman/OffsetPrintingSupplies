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

namespace TestValidation
{
    public class DataBuilder
    {
        public IBlanketService _blanketService;
        public IBlanketOrderService _blanketOrderService;
        public IBlanketOrderDetailService _blanketOrderDetailService;
        public ICashBankService _cashBankService;
        public ICashBankAdjustmentService _cashBankAdjustmentService;
        public ICashBankMutationService _cashBankMutationService;
        public ICashMutationService _cashMutationService;
        public ICoreBuilderService _coreBuilderService;
        public ICoreIdentificationService _coreIdentificationService;
        public ICoreIdentificationDetailService _coreIdentificationDetailService;
        public IContactService _contactService;
        public IDeliveryOrderService _deliveryOrderService;
        public IDeliveryOrderDetailService _deliveryOrderDetailService;
        public IItemService _itemService;
        public IItemTypeService _itemTypeService;
        public IMachineService _machineService;
        public IPayableService _payableService;
        public IPaymentVoucherDetailService _paymentVoucherDetailService;
        public IPaymentVoucherService _paymentVoucherService;
        public IPurchaseInvoiceDetailService _purchaseInvoiceDetailService;
        public IPurchaseInvoiceService _purchaseInvoiceService;
        public IPurchaseOrderService _purchaseOrderService;
        public IPurchaseOrderDetailService _purchaseOrderDetailService;
        public IPurchaseReceivalService _purchaseReceivalService;
        public IPurchaseReceivalDetailService _purchaseReceivalDetailService;
        public IReceivableService _receivableService;
        public IReceiptVoucherDetailService _receiptVoucherDetailService;
        public IReceiptVoucherService _receiptVoucherService;
        public ISalesInvoiceDetailService _salesInvoiceDetailService;
        public ISalesInvoiceService _salesInvoiceService;
        public ISalesOrderService _salesOrderService;
        public ISalesOrderDetailService _salesOrderDetailService;
        public IRecoveryAccessoryDetailService _recoveryAccessoryDetailService;
        public IRecoveryOrderDetailService _recoveryOrderDetailService;
        public IRecoveryOrderService _recoveryOrderService;
        public IRollerBuilderService _rollerBuilderService;
        public IRollerTypeService _rollerTypeService;
        public IRollerWarehouseMutationDetailService _rollerWarehouseMutationDetailService;
        public IRollerWarehouseMutationService _rollerWarehouseMutationService;
        public IStockAdjustmentDetailService _stockAdjustmentDetailService;
        public IStockAdjustmentService _stockAdjustmentService;
        public IStockMutationService _stockMutationService;
        public IUoMService _uomService;
        public IWarehouseItemService _warehouseItemService;
        public IWarehouseService _warehouseService;
        public IWarehouseMutationService _warehouseMutationService;
        public IWarehouseMutationDetailService _warehouseMutationDetailService;

        public IPriceMutationService _priceMutationService;
        public IContactGroupService _contactGroupService;

        public CashBank cashBank, pettyCash;
        public CashBankAdjustment cashBankAdjustment;

        public ContactGroup baseGroup;
        public ItemType typeAdhesive, typeAccessory, typeBar, typeBlanket, typeBearing, typeRollBlanket, typeCore, typeCompound, typeChemical,
                        typeConsumable, typeGlue, typeUnderpacking, typeRoller;
        public RollerType typeDamp, typeFoundDT, typeInkFormX, typeInkDistD, typeInkDistM, typeInkDistE,
                        typeInkDuctB, typeInkDistH, typeInkFormW, typeInkDistHQ, typeDampFormDQ, typeInkFormY;
        public UoM Pcs, Boxes, Tubs;
        public Item item, itemAdhesive, itemCompound, itemCompound1, itemCompound2, itemAccessory1, itemAccessory2;
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
        public Blanket blanket1, blanket2, blanket3;
        public BlanketOrder blanketOrderContact;
        public BlanketOrderDetail blanketODContact1, blanketODContact2, blanketODContact3, blanketODContact4; 
        public WarehouseMutation warehouseMutation;
        public WarehouseMutationDetail wmoDetail1, wmoDetail2, wmoDetail3, wmoDetail4, wmoDetail5, wmoDetail6,
                                            wmoDetail7, wmoDetail8, wmoDetail9;
        public RollerWarehouseMutation rollerWarehouseMutationContact, rollerWarehouseMutationInHouse;
        public RollerWarehouseMutationDetail rwmDetailContact1, rwmDetailContact2, rwmDetailContact3,
                                             rwmDetailInHouse1, rwmDetailInHouse2, rwmDetailInHouse3;
        public StockAdjustment stockAdjustment;
        public StockAdjustmentDetail stockAD, stockAD1, stockAD2, stockAD3, stockAD4;

        // extended variable
        public int usedCoreBuilderQuantity, usedCoreBuilder1Quantity, usedCoreBuilder2Quantity, usedCoreBuilder3Quantity, usedCoreBuilder4Quantity;
        public int usedRollerBuilderQuantity, usedRollerBuilder1Quantity, usedRollerBuilder2Quantity, usedRollerBuilder3Quantity, usedRollerBuilder4Quantity;
        public int usedCoreBuilderFinal, usedCoreBuilder1Final, usedCoreBuilder2Final, usedCoreBuilder3Final, usedCoreBuilder4Final;
        public int usedRollerBuilderFinal, usedRollerBuilder1Final, usedRollerBuilder2Final, usedRollerBuilder3Final, usedRollerBuilder4Final;
        public int accessory1quantity;

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

        public DataBuilder()
        {
            _blanketService = new BlanketService(new BlanketRepository(), new BlanketValidator());
            _blanketOrderService = new BlanketOrderService(new BlanketOrderRepository(), new BlanketOrderValidator());
            _blanketOrderDetailService = new BlanketOrderDetailService(new BlanketOrderDetailRepository(), new BlanketOrderDetailValidator());
            _cashBankAdjustmentService = new CashBankAdjustmentService(new CashBankAdjustmentRepository(), new CashBankAdjustmentValidator());
            _cashBankMutationService = new CashBankMutationService(new CashBankMutationRepository(), new CashBankMutationValidator());
            _cashBankService = new CashBankService(new CashBankRepository(), new CashBankValidator());
            _cashMutationService = new CashMutationService(new CashMutationRepository(), new CashMutationValidator());
            _coreBuilderService = new CoreBuilderService(new CoreBuilderRepository(), new CoreBuilderValidator());
            _coreIdentificationDetailService = new CoreIdentificationDetailService(new CoreIdentificationDetailRepository(), new CoreIdentificationDetailValidator());
            _coreIdentificationService = new CoreIdentificationService(new CoreIdentificationRepository(), new CoreIdentificationValidator());
            _contactService = new ContactService(new ContactRepository(), new ContactValidator());
            _deliveryOrderService = new DeliveryOrderService(new DeliveryOrderRepository(), new DeliveryOrderValidator());
            _deliveryOrderDetailService = new DeliveryOrderDetailService(new DeliveryOrderDetailRepository(), new DeliveryOrderDetailValidator());
            _itemService = new ItemService(new ItemRepository(), new ItemValidator());
            _itemTypeService = new ItemTypeService(new ItemTypeRepository(), new ItemTypeValidator());
            _machineService = new MachineService(new MachineRepository(), new MachineValidator());
            _payableService = new PayableService(new PayableRepository(), new PayableValidator());
            _paymentVoucherDetailService = new PaymentVoucherDetailService(new PaymentVoucherDetailRepository(), new PaymentVoucherDetailValidator());
            _paymentVoucherService = new PaymentVoucherService(new PaymentVoucherRepository(), new PaymentVoucherValidator());
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
            _stockAdjustmentDetailService = new StockAdjustmentDetailService(new StockAdjustmentDetailRepository(), new StockAdjustmentDetailValidator());
            _stockAdjustmentService = new StockAdjustmentService(new StockAdjustmentRepository(), new StockAdjustmentValidator());
            _stockMutationService = new StockMutationService(new StockMutationRepository(), new StockMutationValidator());
            _uomService = new UoMService(new UoMRepository(), new UoMValidator());
            _warehouseItemService = new WarehouseItemService(new WarehouseItemRepository(), new WarehouseItemValidator());
            _warehouseService = new WarehouseService(new WarehouseRepository(), new WarehouseValidator());
            _warehouseMutationService = new WarehouseMutationService(new WarehouseMutationRepository(), new WarehouseMutationValidator());
            _warehouseMutationDetailService = new WarehouseMutationDetailService(new WarehouseMutationDetailRepository(), new WarehouseMutationDetailValidator());

            _priceMutationService = new PriceMutationService(new PriceMutationRepository(), new PriceMutationValidator());
            _contactGroupService = new ContactGroupService(new ContactGroupRepository(), new ContactGroupValidator());

            typeAdhesive = _itemTypeService.CreateObject("Adhesive", "Adhesive");
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

            baseGroup = _contactGroupService.CreateObject(Core.Constants.Constant.GroupType.Base, "Base Group", true);
        }

        public void PopulateData()
        {
            PopulateWarehouse();
            PopulateItem();
            PopulateSingles();
            PopulateBuilders();
            PopulateBlanket();
            PopulateWarehouseMutationForRollerIdentificationAndRecovery();
            PopulateCoreIdentifications();
            PopulateRecoveryOrders();
            PopulateRecoveryOrders2();
            PopulateStockAdjustment();
            PopulateRecoveryOrders3();
            PopulateCoreIdentifications2();
            PopulateRollerWarehouseMutation();
            PopulateBlanketOrders();

            // @SalesBuilder
            PopulateSalesAndDelivery();
            PopulateSalesInvoice();
            PopulateReceiptVoucher();

            // @PurchaseBuilder
            PopulatePurchaseOrderAndPurchaseReceival();
            PopulatePurchaseInvoice();
            PopulatePaymentVoucher();
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

            itemAdhesive= new Item()
            {
                ItemTypeId = _itemTypeService.GetObjectByName("Adhesive").Id,
                Name = "Adhesive Default",
                Category = "Adhesive",
                Sku = "ADD123",
                UoMId = Tubs.Id
            };
            itemAdhesive = _itemService.CreateObject(itemAdhesive, _uomService, _itemTypeService, _warehouseItemService, _warehouseService, _priceMutationService, _contactGroupService);

            itemCompound = new Item()
            {
                ItemTypeId = _itemTypeService.GetObjectByName("Compound").Id,
                Name = "Compound RB else",
                Category = "Compound",
                Sku = "CMP123",
                UoMId = Tubs.Id,
                MinimumQuantity = 150000
            };
            itemCompound = _itemService.CreateObject(itemCompound, _uomService, _itemTypeService, _warehouseItemService, _warehouseService, _priceMutationService, _contactGroupService);

            itemCompound1 = new Item()
            {
                ItemTypeId = _itemTypeService.GetObjectByName("Compound").Id,
                Name = "Compound RB1",
                Category = "Compound",
                Sku = "CMP101",
                UoMId = _uomService.GetObjectByName("Tubs").Id
            };
            itemCompound1 = _itemService.CreateObject(itemCompound1, _uomService, _itemTypeService, _warehouseItemService, _warehouseService, _priceMutationService, _contactGroupService);

            itemCompound2 = new Item()
            {
                ItemTypeId = _itemTypeService.GetObjectByName("Compound").Id,
                Name = "Compound RB2",
                Category = "Compound",
                Sku = "CMP102",
                UoMId = Tubs.Id
            };
            itemCompound2 = _itemService.CreateObject(itemCompound2, _uomService, _itemTypeService, _warehouseItemService, _warehouseService, _priceMutationService, _contactGroupService);

            itemAccessory1 = new Item()
            {
                ItemTypeId = _itemTypeService.GetObjectByName("Accessory").Id,
                Name = "Accessory Sample 1",
                Category = "Accessory",
                Sku = "ACC001",
                UoMId = Pcs.Id
            };
            itemAccessory1 = _itemService.CreateObject(itemAccessory1, _uomService, _itemTypeService, _warehouseItemService, _warehouseService, _priceMutationService, _contactGroupService);

            itemAccessory2 = new Item()
            {
                ItemTypeId = _itemTypeService.GetObjectByName("Accessory").Id,
                Name = "Accessory Sample 2",
                Category = "Accessory",
                Sku = "ACC002",
                UoMId = Pcs.Id
            };
            itemAccessory2 = _itemService.CreateObject(itemAccessory2, _uomService, _itemTypeService, _warehouseItemService, _warehouseService, _priceMutationService, _contactGroupService);

            StockAdjustment sa = new StockAdjustment()
            {
                AdjustmentDate = DateTime.Today,
                Description = "Compound And Accessories Adjustment",
                WarehouseId = localWarehouse.Id
            };
            _stockAdjustmentService.CreateObject(sa, _warehouseService);
            StockAdjustmentDetail sadAdhesive = new StockAdjustmentDetail()
            {
                StockAdjustmentId = sa.Id,
                Quantity = 100,
                ItemId = itemAdhesive.Id
            };
            _stockAdjustmentDetailService.CreateObject(sadAdhesive, _stockAdjustmentService, _itemService, _warehouseItemService);
            StockAdjustmentDetail sadCompound = new StockAdjustmentDetail()
            {
                StockAdjustmentId = sa.Id,
                Quantity = 100000,
                ItemId = itemCompound.Id
            };
            _stockAdjustmentDetailService.CreateObject(sadCompound, _stockAdjustmentService, _itemService, _warehouseItemService);
            StockAdjustmentDetail sadCompound1 = new StockAdjustmentDetail()
            {
                StockAdjustmentId = sa.Id,
                Quantity = 200000,
                ItemId = itemCompound1.Id
            };
            _stockAdjustmentDetailService.CreateObject(sadCompound1, _stockAdjustmentService, _itemService, _warehouseItemService);
            StockAdjustmentDetail sadCompound2 = new StockAdjustmentDetail()
            {
                StockAdjustmentId = sa.Id,
                Quantity = 200000,
                ItemId = itemCompound2.Id
            };
            _stockAdjustmentDetailService.CreateObject(sadCompound2, _stockAdjustmentService, _itemService, _warehouseItemService);
            StockAdjustmentDetail sadAccessory1 = new StockAdjustmentDetail()
            {
                StockAdjustmentId = sa.Id,
                Quantity = 5,
                ItemId = itemAccessory1.Id
            };
            _stockAdjustmentDetailService.CreateObject(sadAccessory1, _stockAdjustmentService, _itemService, _warehouseItemService);
            StockAdjustmentDetail sadAccessory2 = new StockAdjustmentDetail()
            {
                StockAdjustmentId = sa.Id,
                Quantity = 5,
                ItemId = itemAccessory2.Id
            };
            _stockAdjustmentDetailService.CreateObject(sadAccessory2, _stockAdjustmentService, _itemService, _warehouseItemService);
            _stockAdjustmentService.ConfirmObject(sa, DateTime.Today, _stockAdjustmentDetailService, _stockMutationService, _itemService, _blanketService, _warehouseItemService);
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
                Email = "random@ri.gov.au"
            };
            contact = _contactService.CreateObject(contact, _contactGroupService);

            machine = new Machine()
            {
                Code = "MX0001",
                Name = "Printing Machine",
                Description = "Generic"
            };
            machine = _machineService.CreateObject(machine);

            cashBank = new CashBank()
            {
                Name = "Rekening BRI",
                Description = "Untuk cashflow",
                IsBank = true
            };
            _cashBankService.CreateObject(cashBank);

            pettyCash = new CashBank()
            {
                Name = "Petty Cash",
                Description = "Uang kas sementara",
                IsBank = false
            };
            _cashBankService.CreateObject(pettyCash);

            cashBankAdjustment = new CashBankAdjustment()
            {
                CashBankId = cashBank.Id,
                Amount = 1000000000,
                AdjustmentDate = DateTime.Today
            };
            _cashBankAdjustmentService.CreateObject(cashBankAdjustment, _cashBankService);
            _cashBankAdjustmentService.ConfirmObject(cashBankAdjustment, DateTime.Now, _cashMutationService, _cashBankService);
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
                                                           _warehouseService, _priceMutationService, _contactGroupService, _machineService);

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
                                                            _warehouseService, _priceMutationService, _contactGroupService, _machineService);

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
                                                            _warehouseService, _priceMutationService, _contactGroupService, _machineService);

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
                                                            _warehouseService, _priceMutationService, _contactGroupService, _machineService);

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
                                                            _warehouseService, _priceMutationService, _contactGroupService, _machineService);

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
                AdhesiveId = itemAdhesive.Id
            };
            rollerBuilder = _rollerBuilderService.CreateObject(rollerBuilder, _machineService, _uomService, _itemService, _itemTypeService,
                                                               _coreBuilderService, _rollerTypeService, _warehouseItemService, _warehouseService,
                                                               _priceMutationService, _contactGroupService);

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
                AdhesiveId = itemAdhesive.Id
            };
            rollerBuilder1 = _rollerBuilderService.CreateObject(rollerBuilder1, _machineService, _uomService, _itemService, _itemTypeService,
                                                                _coreBuilderService, _rollerTypeService, _warehouseItemService, _warehouseService,
                                                                _priceMutationService, _contactGroupService);

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
                AdhesiveId = itemAdhesive.Id
            };
            rollerBuilder2 = _rollerBuilderService.CreateObject(rollerBuilder2, _machineService, _uomService, _itemService, _itemTypeService,
                                                                _coreBuilderService, _rollerTypeService, _warehouseItemService, _warehouseService,
                                                                _priceMutationService, _contactGroupService);

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
                AdhesiveId = itemAdhesive.Id
            };
            rollerBuilder3 = _rollerBuilderService.CreateObject(rollerBuilder3, _machineService, _uomService, _itemService, _itemTypeService,
                                                                _coreBuilderService, _rollerTypeService, _warehouseItemService, _warehouseService,
                                                                _priceMutationService, _contactGroupService);

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
                AdhesiveId = itemAdhesive.Id
            };
            rollerBuilder4 = _rollerBuilderService.CreateObject(rollerBuilder4, _machineService, _uomService, _itemService, _itemTypeService,
                                                                _coreBuilderService, _rollerTypeService, _warehouseItemService, _warehouseService,
                                                                _priceMutationService, _contactGroupService);

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
            StockAdjustmentDetail sadNewCore = new StockAdjustmentDetail() { ItemId = NewCore.Id , Quantity = 7, StockAdjustmentId = sa.Id};
            _stockAdjustmentDetailService.CreateObject(sadNewCore, _stockAdjustmentService, _itemService, _warehouseItemService);
            StockAdjustmentDetail sadNewCore1 = new StockAdjustmentDetail() { ItemId = NewCore1.Id, Quantity = 7, StockAdjustmentId = sa.Id };
            _stockAdjustmentDetailService.CreateObject(sadNewCore1, _stockAdjustmentService, _itemService, _warehouseItemService);
            StockAdjustmentDetail sadNewCore2 = new StockAdjustmentDetail() { ItemId = NewCore2.Id, Quantity = 7, StockAdjustmentId = sa.Id };
            _stockAdjustmentDetailService.CreateObject(sadNewCore2, _stockAdjustmentService, _itemService, _warehouseItemService);
            StockAdjustmentDetail sadNewCore3 = new StockAdjustmentDetail() { ItemId = NewCore3.Id, Quantity = 7, StockAdjustmentId = sa.Id };
            _stockAdjustmentDetailService.CreateObject(sadNewCore3, _stockAdjustmentService, _itemService, _warehouseItemService);
            StockAdjustmentDetail sadNewCore4 = new StockAdjustmentDetail() { ItemId = NewCore4.Id, Quantity = 7, StockAdjustmentId = sa.Id };
            _stockAdjustmentDetailService.CreateObject(sadNewCore4, _stockAdjustmentService, _itemService, _warehouseItemService);

            StockAdjustmentDetail sadUsedCore = new StockAdjustmentDetail() { ItemId = UsedCore.Id, Quantity = 7, StockAdjustmentId = sa.Id };
            _stockAdjustmentDetailService.CreateObject(sadUsedCore, _stockAdjustmentService, _itemService, _warehouseItemService);
            StockAdjustmentDetail sadUsedCore1 = new StockAdjustmentDetail() { ItemId = UsedCore1.Id, Quantity = 7, StockAdjustmentId = sa.Id };
            _stockAdjustmentDetailService.CreateObject(sadUsedCore1, _stockAdjustmentService, _itemService, _warehouseItemService);
            StockAdjustmentDetail sadUsedCore2 = new StockAdjustmentDetail() { ItemId = UsedCore2.Id, Quantity = 7, StockAdjustmentId = sa.Id };
            _stockAdjustmentDetailService.CreateObject(sadUsedCore2, _stockAdjustmentService, _itemService, _warehouseItemService);
            StockAdjustmentDetail sadUsedCore3 = new StockAdjustmentDetail() { ItemId = UsedCore3.Id, Quantity = 7, StockAdjustmentId = sa.Id };
            _stockAdjustmentDetailService.CreateObject(sadUsedCore3, _stockAdjustmentService, _itemService, _warehouseItemService);
            StockAdjustmentDetail sadUsedCore4 = new StockAdjustmentDetail() { ItemId = UsedCore4.Id, Quantity = 7, StockAdjustmentId = sa.Id };
            _stockAdjustmentDetailService.CreateObject(sadUsedCore4, _stockAdjustmentService, _itemService, _warehouseItemService);

            StockAdjustmentDetail sadRollerNewCore = new StockAdjustmentDetail() { ItemId = RollerNewCore.Id, Quantity = 7, StockAdjustmentId = sa.Id };
            _stockAdjustmentDetailService.CreateObject(sadRollerNewCore, _stockAdjustmentService, _itemService, _warehouseItemService);
            StockAdjustmentDetail sadRollerNewCore1 = new StockAdjustmentDetail() { ItemId = RollerNewCore1.Id, Quantity = 7, StockAdjustmentId = sa.Id };
            _stockAdjustmentDetailService.CreateObject(sadRollerNewCore1, _stockAdjustmentService, _itemService, _warehouseItemService);
            StockAdjustmentDetail sadRollerNewCore2 = new StockAdjustmentDetail() { ItemId = RollerNewCore2.Id, Quantity = 7, StockAdjustmentId = sa.Id };
            _stockAdjustmentDetailService.CreateObject(sadRollerNewCore2, _stockAdjustmentService, _itemService, _warehouseItemService);
            StockAdjustmentDetail sadRollerNewCore3 = new StockAdjustmentDetail() { ItemId = RollerNewCore3.Id, Quantity = 7, StockAdjustmentId = sa.Id };
            _stockAdjustmentDetailService.CreateObject(sadRollerNewCore3, _stockAdjustmentService, _itemService, _warehouseItemService);
            StockAdjustmentDetail sadRollerNewCore4 = new StockAdjustmentDetail() { ItemId = RollerNewCore4.Id, Quantity = 7, StockAdjustmentId = sa.Id };
            _stockAdjustmentDetailService.CreateObject(sadRollerNewCore4, _stockAdjustmentService, _itemService, _warehouseItemService);

            StockAdjustmentDetail sadRollerUsedCore = new StockAdjustmentDetail() { ItemId = RollerUsedCore.Id, Quantity = 7, StockAdjustmentId = sa.Id };
            _stockAdjustmentDetailService.CreateObject(sadRollerUsedCore, _stockAdjustmentService, _itemService, _warehouseItemService);
            StockAdjustmentDetail sadRollerUsedCore1 = new StockAdjustmentDetail() { ItemId = RollerUsedCore1.Id, Quantity = 7, StockAdjustmentId = sa.Id };
            _stockAdjustmentDetailService.CreateObject(sadRollerUsedCore1, _stockAdjustmentService, _itemService, _warehouseItemService);
            StockAdjustmentDetail sadRollerUsedCore2 = new StockAdjustmentDetail() { ItemId = RollerUsedCore2.Id, Quantity = 7, StockAdjustmentId = sa.Id };
            _stockAdjustmentDetailService.CreateObject(sadRollerUsedCore2, _stockAdjustmentService, _itemService, _warehouseItemService);
            StockAdjustmentDetail sadRollerUsedCore3 = new StockAdjustmentDetail() { ItemId = RollerUsedCore3.Id, Quantity = 7, StockAdjustmentId = sa.Id };
            _stockAdjustmentDetailService.CreateObject(sadRollerUsedCore3, _stockAdjustmentService, _itemService, _warehouseItemService);
            StockAdjustmentDetail sadRollerUsedCore4 = new StockAdjustmentDetail() { ItemId = RollerUsedCore4.Id, Quantity = 7, StockAdjustmentId = sa.Id };
            _stockAdjustmentDetailService.CreateObject(sadRollerUsedCore4, _stockAdjustmentService, _itemService, _warehouseItemService);

            _stockAdjustmentService.ConfirmObject(sa, DateTime.Today, _stockAdjustmentDetailService, _stockMutationService, _itemService, _blanketService, _warehouseItemService);
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
            coreIdentification = _coreIdentificationService.ConfirmObject(coreIdentification, DateTime.Today, _coreIdentificationDetailService, _stockMutationService, _recoveryOrderService, _recoveryOrderDetailService, _coreBuilderService, _itemService, _warehouseItemService, _blanketService);
            coreIdentificationContact = _coreIdentificationService.ConfirmObject(coreIdentificationContact, DateTime.Today, _coreIdentificationDetailService, _stockMutationService, _recoveryOrderService, _recoveryOrderDetailService, _coreBuilderService, _itemService, _warehouseItemService, _blanketService);
            coreIdentificationInHouse = _coreIdentificationService.ConfirmObject(coreIdentificationInHouse, DateTime.Today, _coreIdentificationDetailService, _stockMutationService, _recoveryOrderService, _recoveryOrderDetailService, _coreBuilderService, _itemService, _warehouseItemService, _blanketService);

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
            _recoveryOrderService.ConfirmObject(recoveryOrderContact, DateTime.Today, _coreIdentificationDetailService, _recoveryOrderDetailService,
                                      _recoveryAccessoryDetailService, _coreBuilderService, _stockMutationService, _itemService,
                                      _blanketService, _warehouseItemService, _warehouseService);
            _recoveryOrderService.ConfirmObject(recoveryOrderInHouse, DateTime.Today, _coreIdentificationDetailService, _recoveryOrderDetailService,
                                                _recoveryAccessoryDetailService, _coreBuilderService, _stockMutationService, _itemService,
                                                _blanketService, _warehouseItemService, _warehouseService);
            _recoveryOrderService.ConfirmObject(recoveryOrder, DateTime.Today, _coreIdentificationDetailService, _recoveryOrderDetailService,
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
                                                       _itemService, _warehouseItemService, _blanketService, _stockMutationService);
            _recoveryOrderDetailService.RejectObject(recoveryODContact2, DateTime.Today, _coreIdentificationService, _coreIdentificationDetailService, _recoveryOrderService,
                                                       _recoveryAccessoryDetailService, _coreBuilderService, _rollerBuilderService, _itemService,
                                                       _warehouseItemService, _blanketService, _stockMutationService);
            _recoveryOrderDetailService.FinishObject(recoveryODContact3, DateTime.Today, _coreIdentificationService, _coreIdentificationDetailService,
                                                       _recoveryOrderService, _recoveryAccessoryDetailService, _coreBuilderService, _rollerBuilderService,
                                                       _itemService, _warehouseItemService, _blanketService, _stockMutationService);
            _recoveryOrderDetailService.FinishObject(recoveryODInHouse1, DateTime.Today, _coreIdentificationService, _coreIdentificationDetailService,
                                                       _recoveryOrderService, _recoveryAccessoryDetailService, _coreBuilderService, _rollerBuilderService,
                                                       _itemService, _warehouseItemService, _blanketService, _stockMutationService);
            _recoveryOrderDetailService.FinishObject(recoveryODInHouse2, DateTime.Today, _coreIdentificationService, _coreIdentificationDetailService,
                                                       _recoveryOrderService, _recoveryAccessoryDetailService, _coreBuilderService, _rollerBuilderService,
                                                       _itemService, _warehouseItemService, _blanketService, _stockMutationService);
            _recoveryOrderDetailService.RejectObject(recoveryODInHouse3, DateTime.Today, _coreIdentificationService, _coreIdentificationDetailService, _recoveryOrderService,
                                                       _recoveryAccessoryDetailService, _coreBuilderService, _rollerBuilderService, _itemService,
                                                       _warehouseItemService, _blanketService, _stockMutationService);
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
                StockAdjustmentId = stockAdjustment.Id
            };
            stockAD = _stockAdjustmentDetailService.CreateObject(stockAD, _stockAdjustmentService, _itemService, _warehouseItemService);
            
            stockAD1 = new StockAdjustmentDetail()
            {
                ItemId = coreBuilder1.UsedCoreItemId,
                Quantity = 3,
                StockAdjustmentId = stockAdjustment.Id
            };
            stockAD1 = _stockAdjustmentDetailService.CreateObject(stockAD1, _stockAdjustmentService, _itemService, _warehouseItemService);

            stockAD2 = new StockAdjustmentDetail()
            {
                ItemId = coreBuilder2.UsedCoreItemId,
                Quantity = 3,
                StockAdjustmentId = stockAdjustment.Id
            };
            stockAD2 = _stockAdjustmentDetailService.CreateObject(stockAD2, _stockAdjustmentService, _itemService, _warehouseItemService);

            stockAD3 = new StockAdjustmentDetail()
            {
                ItemId = coreBuilder3.UsedCoreItemId,
                Quantity = 3,
                StockAdjustmentId = stockAdjustment.Id
            };
            stockAD3 = _stockAdjustmentDetailService.CreateObject(stockAD3, _stockAdjustmentService, _itemService, _warehouseItemService);

            stockAD4 = new StockAdjustmentDetail()
            {
                ItemId = coreBuilder4.UsedCoreItemId,
                Quantity = 3,
                StockAdjustmentId = stockAdjustment.Id
            };
            stockAD1 = _stockAdjustmentDetailService.CreateObject(stockAD4, _stockAdjustmentService, _itemService, _warehouseItemService);
        }

        public void PopulateRecoveryOrders3()
        {
            _stockAdjustmentService.ConfirmObject(stockAdjustment, DateTime.Today, _stockAdjustmentDetailService, _stockMutationService, _itemService, _blanketService, _warehouseItemService);
            
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
            _recoveryOrderService.ConfirmObject(recoveryOrderContact2, DateTime.Today, _coreIdentificationDetailService, _recoveryOrderDetailService, _recoveryAccessoryDetailService,
                                                _coreBuilderService, _stockMutationService, _itemService, _blanketService, _warehouseItemService, _warehouseService);
            _recoveryOrderService.ConfirmObject(recoveryOrderInHouse2, DateTime.Today, _coreIdentificationDetailService, _recoveryOrderDetailService, _recoveryAccessoryDetailService,
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
                                                     _blanketService, _stockMutationService);
            _recoveryOrderDetailService.FinishObject(recoveryODContact2b, DateTime.Today, _coreIdentificationService, _coreIdentificationDetailService, _recoveryOrderService,
                                                     _recoveryAccessoryDetailService, _coreBuilderService, _rollerBuilderService, _itemService, _warehouseItemService,
                                                     _blanketService, _stockMutationService);
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
                                                          _blanketService, _warehouseItemService, _stockMutationService, _recoveryOrderDetailService, _coreIdentificationDetailService, _coreIdentificationService);
            
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
                                                          _blanketService, _warehouseItemService, _stockMutationService, _recoveryOrderDetailService, _coreIdentificationDetailService, _coreIdentificationService);
        }

        public void PopulateBlanket()
        {
            bargeneric = new Item()
            {
                ItemTypeId = typeBar.Id,
                Category = "bar",
                Name = "Bar Generic",
                UoMId = Pcs.Id,
                Sku = "BGEN"
            };
            _itemService.CreateObject(bargeneric, _uomService, _itemTypeService, _warehouseItemService, _warehouseService, _priceMutationService, _contactGroupService);

            barleft1 = new Item()
            {
                ItemTypeId = typeBar.Id,
                Category = "bar",
                Name = "Bar Left 1",
                UoMId = Pcs.Id,
                Sku = "BL1"
            };
            _itemService.CreateObject(barleft1, _uomService, _itemTypeService, _warehouseItemService, _warehouseService, _priceMutationService, _contactGroupService);

            barleft2 = new Item()
            {
                ItemTypeId = typeBar.Id,
                Category = "bar",
                Name = "Bar Left 2",
                UoMId = Pcs.Id,
                Sku = "BL2"
            };
            _itemService.CreateObject(barleft2, _uomService, _itemTypeService, _warehouseItemService, _warehouseService, _priceMutationService, _contactGroupService);

            barright1 = new Item()
            {
                ItemTypeId = typeBar.Id,
                Category = "bar",
                Name = "Bar Right 1",
                UoMId = Pcs.Id,
                Sku = "BR1"
            };
            _itemService.CreateObject(barright1, _uomService, _itemTypeService, _warehouseItemService, _warehouseService, _priceMutationService, _contactGroupService);

            barright2 = new Item()
            {
                ItemTypeId = typeBar.Id,
                Category = "bar",
                Name = "Bar Right 2",
                UoMId = Pcs.Id,
                Sku = "BR2"
            };
            _itemService.CreateObject(barright2, _uomService, _itemTypeService, _warehouseItemService, _warehouseService, _priceMutationService, _contactGroupService);

            rollBlanket1 = new Item()
            {
                ItemTypeId = typeRollBlanket.Id,
                Category = "RollBlanket",
                Name = "RollBlanket1",
                UoMId = Pcs.Id,
                Sku = "BLK1"
            };
            _itemService.CreateObject(rollBlanket1, _uomService, _itemTypeService, _warehouseItemService, _warehouseService, _priceMutationService, _contactGroupService);

            rollBlanket2 = new Item()
            {
                ItemTypeId = typeRollBlanket.Id,
                Category = "RollBlanket",
                Name = "RollBlanket2",
                UoMId = Pcs.Id,
                Sku = "BLK2"
            };
            _itemService.CreateObject(rollBlanket2, _uomService, _itemTypeService, _warehouseItemService, _warehouseService, _priceMutationService, _contactGroupService);

            rollBlanket3 = new Item()
            {
                ItemTypeId = typeRollBlanket.Id,
                Category = "RollBlanket",
                Name = "RollBlanket3",
                UoMId = Pcs.Id,
                Sku = "BLK3"
            };
            _itemService.CreateObject(rollBlanket3, _uomService, _itemTypeService, _warehouseItemService, _warehouseService, _priceMutationService, _contactGroupService);

            blanket1 = new Blanket()
            {
                ItemTypeId = typeBlanket.Id,
                Category = "Blanket",
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
                AdhesiveId = itemAdhesive.Id,
                CroppingType = Core.Constants.Constant.CroppingType.Normal,
                ApplicationCase = Core.Constants.Constant.ApplicationCase.Web
            };
            _blanketService.CreateObject(blanket1, _blanketService, _uomService, _itemService, _itemTypeService, _contactService,
                                         _machineService, _warehouseItemService, _warehouseService, _priceMutationService, _contactGroupService);

            blanket2 = new Blanket()
            {
                ItemTypeId = typeBlanket.Id,
                Category = "Blanket",
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
                AdhesiveId = itemAdhesive.Id,
                CroppingType = Core.Constants.Constant.CroppingType.Normal,
                ApplicationCase = Core.Constants.Constant.ApplicationCase.Web
            };
            _blanketService.CreateObject(blanket2, _blanketService, _uomService, _itemService, _itemTypeService, _contactService,
                                         _machineService, _warehouseItemService, _warehouseService, _priceMutationService, _contactGroupService);

            blanket3 = new Blanket()
            {
                ItemTypeId = typeBlanket.Id,
                Category = "Blanket",
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
                AdhesiveId = itemAdhesive.Id,
                CroppingType = Core.Constants.Constant.CroppingType.Special,
                LeftOverAC = 6,
                LeftOverAR = 9,
                ApplicationCase = Core.Constants.Constant.ApplicationCase.Sheetfed
            };
            _blanketService.CreateObject(blanket3, _blanketService, _uomService, _itemService, _itemTypeService, _contactService,
                                         _machineService, _warehouseItemService, _warehouseService, _priceMutationService, _contactGroupService);

            StockAdjustment sa = new StockAdjustment() { WarehouseId = localWarehouse.Id, AdjustmentDate = DateTime.Today, Description = "Bar Related Adjustment" };
            _stockAdjustmentService.CreateObject(sa, _warehouseService);
            StockAdjustmentDetail sadBarGeneric = new StockAdjustmentDetail () { ItemId = bargeneric.Id , Quantity = 10, StockAdjustmentId = sa.Id };
            _stockAdjustmentDetailService.CreateObject(sadBarGeneric, _stockAdjustmentService, _itemService, _warehouseItemService);
            StockAdjustmentDetail sadBarleft1= new StockAdjustmentDetail () { ItemId = barleft1.Id , Quantity = 10, StockAdjustmentId = sa.Id };
            _stockAdjustmentDetailService.CreateObject(sadBarleft1, _stockAdjustmentService, _itemService, _warehouseItemService);
            StockAdjustmentDetail sadBarleft2 = new StockAdjustmentDetail () { ItemId = barleft1.Id , Quantity = 10, StockAdjustmentId = sa.Id };
            _stockAdjustmentDetailService.CreateObject(sadBarleft2, _stockAdjustmentService, _itemService, _warehouseItemService);
            StockAdjustmentDetail sadBarright1 = new StockAdjustmentDetail () { ItemId = barright1.Id , Quantity = 10, StockAdjustmentId = sa.Id };
            _stockAdjustmentDetailService.CreateObject(sadBarright1, _stockAdjustmentService, _itemService, _warehouseItemService);
            StockAdjustmentDetail sadBarright2 = new StockAdjustmentDetail () { ItemId = barright2.Id , Quantity = 10, StockAdjustmentId = sa.Id };
            _stockAdjustmentDetailService.CreateObject(sadBarright2, _stockAdjustmentService, _itemService, _warehouseItemService);
            StockAdjustmentDetail sadRollBlanket1 = new StockAdjustmentDetail () { ItemId = rollBlanket1.Id , Quantity = 10, StockAdjustmentId = sa.Id };
            _stockAdjustmentDetailService.CreateObject(sadRollBlanket1, _stockAdjustmentService, _itemService, _warehouseItemService);
            StockAdjustmentDetail sadRollBlanket2 = new StockAdjustmentDetail () { ItemId = rollBlanket2.Id , Quantity = 20, StockAdjustmentId = sa.Id };
            _stockAdjustmentDetailService.CreateObject(sadRollBlanket2, _stockAdjustmentService, _itemService, _warehouseItemService);
            StockAdjustmentDetail sadRollBlanket3 = new StockAdjustmentDetail () { ItemId = rollBlanket3.Id , Quantity = 10, StockAdjustmentId = sa.Id };
            _stockAdjustmentDetailService.CreateObject(sadRollBlanket3, _stockAdjustmentService, _itemService, _warehouseItemService);
            StockAdjustmentDetail sadBlanket1 = new StockAdjustmentDetail () { ItemId = blanket1.Id , Quantity = 10, StockAdjustmentId = sa.Id };
            _stockAdjustmentDetailService.CreateObject(sadBlanket1, _stockAdjustmentService, _itemService, _warehouseItemService);
            StockAdjustmentDetail sadBlanket2 = new StockAdjustmentDetail () { ItemId = blanket2.Id , Quantity = 10, StockAdjustmentId = sa.Id };
            _stockAdjustmentDetailService.CreateObject(sadBlanket2, _stockAdjustmentService, _itemService, _warehouseItemService);
            StockAdjustmentDetail sadBlanket3 = new StockAdjustmentDetail () { ItemId = blanket3.Id , Quantity = 10, StockAdjustmentId = sa.Id };
            _stockAdjustmentDetailService.CreateObject(sadBlanket3, _stockAdjustmentService, _itemService, _warehouseItemService);

            _stockAdjustmentService.ConfirmObject(sa, DateTime.Today, _stockAdjustmentDetailService, _stockMutationService, _itemService, _blanketService, _warehouseItemService);
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

        // @SalesBuilder
        public void PopulateSalesAndDelivery()
        {
            TimeSpan purchaseDate = new TimeSpan(10, 0, 0, 0);
            TimeSpan receivedDate = new TimeSpan(3, 0, 0 ,0);
            TimeSpan lateDeliveryDate = new TimeSpan(2, 0, 0, 0);
            so1 = new SalesOrder()
            {
                SalesDate = DateTime.Today.Subtract(purchaseDate),
                ContactId = contact.Id
            };
            _salesOrderService.CreateObject(so1, _contactService);

            so2 = new SalesOrder()
            {
                SalesDate = DateTime.Today.Subtract(purchaseDate),
                ContactId = contact.Id
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
                Quantity = so1a.Quantity - 100
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
                                       _itemService, _blanketService, _warehouseItemService);
            _deliveryOrderService.ConfirmObject(do2, DateTime.Now.Subtract(receivedDate), _deliveryOrderDetailService, _salesOrderService, _salesOrderDetailService, _stockMutationService,
                                                   _itemService, _blanketService, _warehouseItemService);
            _deliveryOrderService.ConfirmObject(do3, DateTime.Now.Subtract(receivedDate), _deliveryOrderDetailService, _salesOrderService, _salesOrderDetailService,
                                                   _stockMutationService, _itemService, _blanketService, _warehouseItemService);

            si1 = new SalesInvoice()
            {
                InvoiceDate = DateTime.Today,
                Description = "Penjualan DO1",
                DeliveryOrderId = do1.Id,
                IsTaxable = true,
                Discount = 0,
                DueDate = DateTime.Today.AddDays(14)
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
                IsTaxable = true,
                Discount = 5,
                DueDate = DateTime.Today.AddDays(14)
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
                IsTaxable = true,
                Discount = 0,
                DueDate = DateTime.Today.AddDays(14)
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
            _salesInvoiceService.ConfirmObject(si1, DateTime.Today, _salesInvoiceDetailService, _salesOrderService, _deliveryOrderService,
                                                  _deliveryOrderDetailService, _receivableService);
            _salesInvoiceService.ConfirmObject(si2, DateTime.Today, _salesInvoiceDetailService, _salesOrderService, _deliveryOrderService,
                                                  _deliveryOrderDetailService, _receivableService);
            _salesInvoiceService.ConfirmObject(si3, DateTime.Today, _salesInvoiceDetailService, _salesOrderService, _deliveryOrderService,
                                                  _deliveryOrderDetailService, _receivableService);

            rv = new ReceiptVoucher()
            {
                ContactId = contact.Id,
                CashBankId = cashBank.Id,
                ReceiptDate = DateTime.Today.AddDays(14),
                IsGBCH = true,
                DueDate = DateTime.Today.AddDays(14),
                TotalAmount = si1.AmountReceivable + si2.AmountReceivable + si3.AmountReceivable
            };
            _receiptVoucherService.CreateObject(rv, _receiptVoucherDetailService, _receivableService, _contactService, _cashBankService);

            rvd1 = new ReceiptVoucherDetail()
            {
                ReceiptVoucherId = rv.Id,
                ReceivableId = _receivableService.GetObjectBySource(Core.Constants.Constant.ReceivableSource.SalesInvoice, si1.Id).Id,
                Amount = si1.AmountReceivable,
                Description = "Receipt buat Sales Invoice 1"
            };
            _receiptVoucherDetailService.CreateObject(rvd1, _receiptVoucherService, _cashBankService, _receivableService);

            rvd2 = new ReceiptVoucherDetail()
            {
                ReceiptVoucherId = rv.Id,
                ReceivableId = _receivableService.GetObjectBySource(Core.Constants.Constant.ReceivableSource.SalesInvoice, si2.Id).Id,
                Amount = si2.AmountReceivable,
                Description = "Receipt buat Sales Invoice 2"
            };
            _receiptVoucherDetailService.CreateObject(rvd2, _receiptVoucherService, _cashBankService, _receivableService);

            rvd3 = new ReceiptVoucherDetail()
            {
                ReceiptVoucherId = rv.Id,
                ReceivableId = _receivableService.GetObjectBySource(Core.Constants.Constant.ReceivableSource.SalesInvoice, si3.Id).Id,
                Amount = si3.AmountReceivable,
                Description = "Receipt buat Sales Invoice 3"
            };
            _receiptVoucherDetailService.CreateObject(rvd3, _receiptVoucherService, _cashBankService, _receivableService);

            _receiptVoucherService.ConfirmObject(rv, DateTime.Today, _receiptVoucherDetailService, _cashBankService, _receivableService, _cashMutationService);

            _receiptVoucherService.ReconcileObject(rv, DateTime.Today.AddDays(10), _receiptVoucherDetailService, _cashMutationService, _cashBankService, _receivableService);
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
                ContactId = contact.Id
            };
            _purchaseOrderService.CreateObject(po1, _contactService);

            po2 = new PurchaseOrder()
            {
                PurchaseDate = DateTime.Today.Subtract(purchaseDate),
                ContactId = contact.Id
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

        public void PopulatePurchaseInvoice()
        {
            TimeSpan receivedDate = new TimeSpan(3, 0, 0, 0);
            TimeSpan lateReceivedDate = new TimeSpan(2, 0, 0, 0);
            _purchaseReceivalService.ConfirmObject(pr1, DateTime.Now.Subtract(receivedDate), _purchaseReceivalDetailService, _purchaseOrderService, _purchaseOrderDetailService, _stockMutationService,
                                       _itemService, _blanketService, _warehouseItemService);
            _purchaseReceivalService.ConfirmObject(pr2, DateTime.Now.Subtract(receivedDate), _purchaseReceivalDetailService, _purchaseOrderService, _purchaseOrderDetailService, _stockMutationService,
                                                   _itemService, _blanketService, _warehouseItemService);
            _purchaseReceivalService.ConfirmObject(pr3, DateTime.Now.Subtract(receivedDate), _purchaseReceivalDetailService, _purchaseOrderService, _purchaseOrderDetailService,
                                                   _stockMutationService, _itemService, _blanketService, _warehouseItemService);

            pi1 = new PurchaseInvoice()
            {
                InvoiceDate = DateTime.Today,
                Description = "Pembayaran PR1",
                PurchaseReceivalId = pr1.Id,
                IsTaxable = true,
                Discount = 0,
                DueDate = DateTime.Today.AddDays(14)
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
                IsTaxable = true,
                Discount = 5,
                DueDate = DateTime.Today.AddDays(14)
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
                IsTaxable = true,
                Discount = 0,
                DueDate = DateTime.Today.AddDays(14)
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
                                                  _purchaseReceivalDetailService, _payableService);
            _purchaseInvoiceService.ConfirmObject(pi2, DateTime.Today, _purchaseInvoiceDetailService, _purchaseOrderService, _purchaseReceivalService,
                                                  _purchaseReceivalDetailService, _payableService);
            _purchaseInvoiceService.ConfirmObject(pi3, DateTime.Today, _purchaseInvoiceDetailService, _purchaseOrderService, _purchaseReceivalService,
                                                  _purchaseReceivalDetailService, _payableService);

            pv = new PaymentVoucher()
            {
                ContactId = contact.Id,
                CashBankId = cashBank.Id,
                PaymentDate = DateTime.Today.AddDays(14),
                IsGBCH = true,
                DueDate = DateTime.Today.AddDays(14),
                TotalAmount = pi1.AmountPayable + pi2.AmountPayable + pi3.AmountPayable
            };
            _paymentVoucherService.CreateObject(pv, _paymentVoucherDetailService, _payableService, _contactService, _cashBankService);

            pvd1 = new PaymentVoucherDetail()
            {
                PaymentVoucherId = pv.Id,
                PayableId = _payableService.GetObjectBySource(Core.Constants.Constant.PayableSource.PurchaseInvoice, pi1.Id).Id,
                Amount = pi1.AmountPayable,
                Description = "Payment buat Purchase Invoice 1"
            };
            _paymentVoucherDetailService.CreateObject(pvd1, _paymentVoucherService, _cashBankService, _payableService);

            pvd2 = new PaymentVoucherDetail()
            {
                PaymentVoucherId = pv.Id,
                PayableId = _payableService.GetObjectBySource(Core.Constants.Constant.PayableSource.PurchaseInvoice, pi2.Id).Id,
                Amount = pi2.AmountPayable,
                Description = "Payment buat Purchase Invoice 2"
            };
            _paymentVoucherDetailService.CreateObject(pvd2, _paymentVoucherService, _cashBankService, _payableService);

            pvd3 = new PaymentVoucherDetail()
            {
                PaymentVoucherId = pv.Id,
                PayableId = _payableService.GetObjectBySource(Core.Constants.Constant.PayableSource.PurchaseInvoice, pi3.Id).Id,
                Amount = pi3.AmountPayable,
                Description = "Payment buat Purchase Invoice 3"
            };
            _paymentVoucherDetailService.CreateObject(pvd3, _paymentVoucherService, _cashBankService, _payableService);

            _paymentVoucherService.ConfirmObject(pv, DateTime.Today, _paymentVoucherDetailService, _cashBankService, _payableService, _cashMutationService);

            _paymentVoucherService.ReconcileObject(pv, DateTime.Today.AddDays(10), _paymentVoucherDetailService, _cashMutationService, _cashBankService, _payableService);
        }
    }
}
