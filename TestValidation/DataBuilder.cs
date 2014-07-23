using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public IBarringService _barringService;
        public IBarringOrderService _barringOrderService;
        public IBarringOrderDetailService _barringOrderDetailService;
        public ICoreBuilderService _coreBuilderService;
        public ICoreIdentificationService _coreIdentificationService;
        public ICoreIdentificationDetailService _coreIdentificationDetailService;
        public ICustomerService _customerService;
        public IItemService _itemService;
        public IItemTypeService _itemTypeService;
        public IMachineService _machineService;
        public IRecoveryAccessoryDetailService _recoveryAccessoryDetailService;
        public IRecoveryOrderDetailService _recoveryOrderDetailService;
        public IRecoveryOrderService _recoveryOrderService;
        public IRollerBuilderService _rollerBuilderService;
        public IRollerTypeService _rollerTypeService;
        public IStockMutationService _stockMutationService;
        public IUoMService _uomService;
        public IWarehouseItemService _warehouseItemService;
        public IWarehouseService _warehouseService;
        public IWarehouseMutationOrderService _warehouseMutationOrderService;
        public IWarehouseMutationOrderDetailService _warehouseMutationOrderDetailService;
        
        public ItemType typeAccessory, typeBar, typeBarring, typeBearing, typeBlanket, typeCore, typeCompound, typeChemical,
                        typeConsumable, typeGlue, typeUnderpacking, typeRoller;
        public RollerType typeDamp, typeFoundDT, typeInkFormX, typeInkDistD, typeInkDistM, typeInkDistE,
                        typeInkDuctB, typeInkDistH, typeInkFormW, typeInkDistHQ, typeDampFormDQ, typeInkFormY;
        public UoM Pcs, Boxes, Tubs;
        public Item item, itemCompound, itemCompound1, itemCompound2, itemAccessory1, itemAccessory2;
        public Warehouse localWarehouse, movingWarehouse;
        public Customer customer;
        public Machine machine;
        public CoreBuilder coreBuilder, coreBuilder1, coreBuilder2, coreBuilder3, coreBuilder4;
        public CoreIdentification coreIdentification, coreIdentificationInHouse, coreIdentificationCustomer;
        public CoreIdentificationDetail coreIdentificationDetail, coreIDInHouse1, coreIDInHouse2, coreIDInHouse3,
                                        coreIDCustomer1, coreIDCustomer2, coreIDCustomer3;
        public RollerBuilder rollerBuilder, rollerBuilder1, rollerBuilder2, rollerBuilder3, rollerBuilder4;
        public RecoveryOrder recoveryOrder, recoveryOrderCustomer, recoveryOrderInHouse;
        public RecoveryOrderDetail recoveryODCustomer1, recoveryODCustomer2, recoveryODCustomer3,
                                   recoveryODInHouse1, recoveryODInHouse2, recoveryODInHouse3;
        public RecoveryAccessoryDetail accessory1, accessory2, accessory3, accessory4;
        public Item bargeneric, barleft1, barleft2, barright1, barright2;
        public Item blanket1, blanket2, blanket3;
        public Barring barring1, barring2, barring3;
        public BarringOrder barringOrderCustomer;
        public BarringOrderDetail barringODCustomer1, barringODCustomer2, barringODCustomer3, barringODCustomer4; 

        // extended variable
        public int usedCoreBuilderQuantity, usedCoreBuilder1Quantity, usedCoreBuilder2Quantity, usedCoreBuilder3Quantity, usedCoreBuilder4Quantity;
        public int usedRollerBuilderQuantity, usedRollerBuilder1Quantity, usedRollerBuilder2Quantity, usedRollerBuilder3Quantity, usedRollerBuilder4Quantity;
        public int usedCoreBuilderFinal, usedCoreBuilder1Final, usedCoreBuilder2Final, usedCoreBuilder3Final, usedCoreBuilder4Final;
        public int usedRollerBuilderFinal, usedRollerBuilder1Final, usedRollerBuilder2Final, usedRollerBuilder3Final, usedRollerBuilder4Final;
        public int accessory1quantity;

        public DataBuilder()
        {
            _barringService = new BarringService(new BarringRepository(), new BarringValidator());
            _barringOrderService = new BarringOrderService(new BarringOrderRepository(), new BarringOrderValidator());
            _barringOrderDetailService = new BarringOrderDetailService(new BarringOrderDetailRepository(), new BarringOrderDetailValidator());
            _coreBuilderService = new CoreBuilderService(new CoreBuilderRepository(), new CoreBuilderValidator());
            _coreIdentificationDetailService = new CoreIdentificationDetailService(new CoreIdentificationDetailRepository(), new CoreIdentificationDetailValidator());
            _coreIdentificationService = new CoreIdentificationService(new CoreIdentificationRepository(), new CoreIdentificationValidator());
            _customerService = new CustomerService(new CustomerRepository(), new CustomerValidator());
            _itemService = new ItemService(new ItemRepository(), new ItemValidator());
            _itemTypeService = new ItemTypeService(new ItemTypeRepository(), new ItemTypeValidator());
            _machineService = new MachineService(new MachineRepository(), new MachineValidator());
            _recoveryOrderDetailService = new RecoveryOrderDetailService(new RecoveryOrderDetailRepository(), new RecoveryOrderDetailValidator());
            _recoveryOrderService = new RecoveryOrderService(new RecoveryOrderRepository(), new RecoveryOrderValidator());
            _recoveryAccessoryDetailService = new RecoveryAccessoryDetailService(new RecoveryAccessoryDetailRepository(), new RecoveryAccessoryDetailValidator());
            _rollerBuilderService = new RollerBuilderService(new RollerBuilderRepository(), new RollerBuilderValidator());
            _rollerTypeService = new RollerTypeService(new RollerTypeRepository(), new RollerTypeValidator());
            _stockMutationService = new StockMutationService(new StockMutationRepository(), new StockMutationValidator());
            _uomService = new UoMService(new UoMRepository(), new UoMValidator());
            _warehouseItemService = new WarehouseItemService(new WarehouseItemRepository(), new WarehouseItemValidator());
            _warehouseService = new WarehouseService(new WarehouseRepository(), new WarehouseValidator());
            //_warehouseMutationOrderService = new _warehouseMutationOrderService(new WarehouseMutationOrderRepository(), new WarehouseMutationOrderValidator());
            //_warehouseMutationOrderDetailService = new _warehouseMutationOrderDetailService(new WarehouseMutationOrderDetailRepository(), new WarehouseMutationOrderDetailValidator());

            typeAccessory = _itemTypeService.CreateObject("Accessory", "Accessory");
            typeBar = _itemTypeService.CreateObject("Bar", "Bar");
            typeBarring = _itemTypeService.CreateObject("Barring", "Barring");
            typeBearing = _itemTypeService.CreateObject("Bearing", "Bearing");
            typeBlanket = _itemTypeService.CreateObject("Blanket", "Blanket");
            typeChemical = _itemTypeService.CreateObject("Chemical", "Chemical");
            typeCompound = _itemTypeService.CreateObject("Compound", "Compound");
            typeConsumable = _itemTypeService.CreateObject("Consumable", "Consumable");
            typeCore = _itemTypeService.CreateObject("Core", "Core");
            typeGlue = _itemTypeService.CreateObject("Glue", "Glue");
            typeUnderpacking = _itemTypeService.CreateObject("Underpacking", "Underpacking");
            typeRoller = _itemTypeService.CreateObject("Roller", "Roller");

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
        }

        public void PopulateData()
        {
            PopulateWarehouse();
            PopulateItem();
            PopulateSingles();
            PopulateBuilders();
            PopulateCoreIdentifications();
            PopulateRecoveryOrders();
            PopulateBarringOrders();
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

            itemCompound = new Item()
            {
                ItemTypeId = _itemTypeService.GetObjectByName("Compound").Id,
                Name = "Compound RB else",
                Category = "Compound",
                Sku = "CMP123",
                UoMId = Tubs.Id
            };

            itemCompound = _itemService.CreateObject(itemCompound, _uomService, _itemTypeService, _warehouseItemService, _warehouseService);
            _itemService.AdjustQuantity(itemCompound, 5);
            _warehouseItemService.AdjustQuantity(_warehouseItemService.FindOrCreateObject(localWarehouse.Id, itemCompound.Id), 5);

            itemCompound1 = new Item()
            {
                ItemTypeId = _itemTypeService.GetObjectByName("Compound").Id,
                Name = "Compound RB1",
                Category = "Compound",
                Sku = "CMP101",
                UoMId = _uomService.GetObjectByName("Tubs").Id
            };
            itemCompound1 = _itemService.CreateObject(itemCompound1, _uomService, _itemTypeService, _warehouseItemService, _warehouseService);
            _itemService.AdjustQuantity(itemCompound1, 2);
            _warehouseItemService.AdjustQuantity(_warehouseItemService.FindOrCreateObject(localWarehouse.Id, itemCompound1.Id), 2);

            itemCompound2 = new Item()
            {
                ItemTypeId = _itemTypeService.GetObjectByName("Compound").Id,
                Name = "Compound RB2",
                Category = "Compound",
                Sku = "CMP102",
                UoMId = Tubs.Id
            };
            itemCompound2 = _itemService.CreateObject(itemCompound2, _uomService, _itemTypeService, _warehouseItemService, _warehouseService);
            _itemService.AdjustQuantity(itemCompound2, 2);
            _warehouseItemService.AdjustQuantity(_warehouseItemService.FindOrCreateObject(localWarehouse.Id, itemCompound2.Id), 2);

            itemAccessory1 = new Item()
            {
                ItemTypeId = _itemTypeService.GetObjectByName("Accessory").Id,
                Name = "Accessory Sample 1",
                Category = "Accessory",
                Sku = "ACC001",
                UoMId = Pcs.Id
            };
            itemAccessory1 = _itemService.CreateObject(itemAccessory1, _uomService, _itemTypeService, _warehouseItemService, _warehouseService);
            _itemService.AdjustQuantity(itemAccessory1, 5);
            _warehouseItemService.AdjustQuantity(_warehouseItemService.FindOrCreateObject(localWarehouse.Id, itemAccessory1.Id), 5);

            itemAccessory2 = new Item()
            {
                ItemTypeId = _itemTypeService.GetObjectByName("Accessory").Id,
                Name = "Accessory Sample 2",
                Category = "Accessory",
                Sku = "ACC002",
                UoMId = Pcs.Id
            };
            itemAccessory2 = _itemService.CreateObject(itemAccessory2, _uomService, _itemTypeService, _warehouseItemService, _warehouseService);
            _itemService.AdjustQuantity(itemAccessory2, 5);
            _warehouseItemService.AdjustQuantity(_warehouseItemService.FindOrCreateObject(localWarehouse.Id, itemAccessory2.Id), 5);
        }

        public void PopulateWarehouse()
        {
            localWarehouse = new Warehouse()
            {
                Name = "Sentral Solusi Data",
                Description = "Kali Besar Jakarta",
                IsMovingWarehouse = false,
                Code = "LCL"
            };
            localWarehouse = _warehouseService.CreateObject(localWarehouse, _warehouseItemService, _itemService);

            movingWarehouse = new Warehouse()
            {
                Name = "Kepala Manufacturing Roland",
                Description = "Mengerjakan Roller dan Barring",
                IsMovingWarehouse = true,
                Code = "MVG"
            };
            movingWarehouse = _warehouseService.CreateObject(movingWarehouse, _warehouseItemService, _itemService);
        }

        public void PopulateSingles()
        {
            customer = new Customer()
            {
                Name = "President of Indonesia",
                Address = "Istana Negara Jl. Veteran No. 16 Jakarta Pusat",
                ContactNo = "021 3863777",
                PIC = "Mr. President",
                PICContactNo = "021 3863777",
                Email = "random@ri.gov.au"
            };
            customer = _customerService.CreateObject(customer);

            machine = new Machine()
            {
                Code = "MX0001",
                Name = "Printing Machine",
                Description = "Generic"
            };
            machine = _machineService.CreateObject(machine);
        }

        public void PopulateBuilders()
        {
            coreBuilder = new CoreBuilder()
            {
                BaseSku = "CBX",
                SkuNewCore = "CBXN",
                SkuUsedCore = "CBXU",
                Name = "Core X",
                Category = "X",
                UoMId = Pcs.Id
            };
            coreBuilder = _coreBuilderService.CreateObject(coreBuilder, _uomService, _itemService, _itemTypeService, _warehouseItemService, _warehouseService);

            coreBuilder1 = new CoreBuilder()
            {
                BaseSku = "CBA001",
                SkuNewCore = "CB001N",
                SkuUsedCore = "CB001U",
                Name = "Core A 001",
                Category = "A",
                UoMId = Pcs.Id
            };
            coreBuilder1 = _coreBuilderService.CreateObject(coreBuilder1, _uomService, _itemService, _itemTypeService, _warehouseItemService, _warehouseService);

            coreBuilder2 = new CoreBuilder()
            {
                BaseSku = "CBA002",
                SkuNewCore = "CB002N",
                SkuUsedCore = "CB002U",
                Name = "Core A 002",
                Category = "A",
                UoMId = Pcs.Id
            };
            coreBuilder2 = _coreBuilderService.CreateObject(coreBuilder2, _uomService, _itemService, _itemTypeService, _warehouseItemService, _warehouseService);

            coreBuilder3 = new CoreBuilder()
            {
                BaseSku = "CBA003",
                SkuNewCore = "CB003N",
                SkuUsedCore = "CB003U",
                Name = "Core A 003",
                Category = "A",
                UoMId = Pcs.Id
            };
            coreBuilder3 = _coreBuilderService.CreateObject(coreBuilder3, _uomService, _itemService, _itemTypeService, _warehouseItemService, _warehouseService);

            coreBuilder4 = new CoreBuilder()
            {
                BaseSku = "CBA004",
                SkuNewCore = "CB004N",
                SkuUsedCore = "CB004U",
                Name = "Core A 004",
                Category = "A",
                UoMId = Pcs.Id
            };
            coreBuilder4 = _coreBuilderService.CreateObject(coreBuilder4, _uomService, _itemService, _itemTypeService, _warehouseItemService, _warehouseService);

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
                Category = "X",
                CoreBuilderId = coreBuilder.Id,
                CompoundId = itemCompound.Id,
                MachineId = machine.Id,
                RollerTypeId = typeDamp.Id,
                UoMId = Pcs.Id
            };
            rollerBuilder = _rollerBuilderService.CreateObject(rollerBuilder, _machineService, _uomService, _itemService, _itemTypeService,
                                                               _coreBuilderService, _rollerTypeService, _warehouseItemService, _warehouseService);

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
                Category = "A",
                CoreBuilderId = coreBuilder1.Id,
                CompoundId = itemCompound1.Id,
                MachineId = machine.Id,
                RollerTypeId = typeFoundDT.Id,
                UoMId = Pcs.Id
            };
            rollerBuilder1 = _rollerBuilderService.CreateObject(rollerBuilder1, _machineService, _uomService, _itemService, _itemTypeService,
                                                                _coreBuilderService, _rollerTypeService, _warehouseItemService, _warehouseService);

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
                Category = "A",
                CoreBuilderId = coreBuilder2.Id,
                CompoundId = itemCompound2.Id,
                MachineId = machine.Id,
                RollerTypeId = typeDampFormDQ.Id,
                UoMId = Pcs.Id
            };
            rollerBuilder2 = _rollerBuilderService.CreateObject(rollerBuilder2, _machineService, _uomService, _itemService, _itemTypeService,
                                                                _coreBuilderService, _rollerTypeService, _warehouseItemService, _warehouseService);

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
                Category = "A",
                CoreBuilderId = coreBuilder3.Id,
                CompoundId = itemCompound.Id,
                MachineId = machine.Id,
                RollerTypeId = typeInkDistD.Id,
                UoMId = Pcs.Id
            };
            rollerBuilder3 = _rollerBuilderService.CreateObject(rollerBuilder3, _machineService, _uomService, _itemService, _itemTypeService,
                                                                _coreBuilderService, _rollerTypeService, _warehouseItemService, _warehouseService);

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
                Category = "X",
                CoreBuilderId = coreBuilder4.Id,
                CompoundId = itemCompound.Id,
                MachineId = machine.Id,
                RollerTypeId = typeInkDistH.Id,
                UoMId = Pcs.Id
            };
            rollerBuilder4 = _rollerBuilderService.CreateObject(rollerBuilder4, _machineService, _uomService, _itemService, _itemTypeService,
                                                                _coreBuilderService, _rollerTypeService, _warehouseItemService, _warehouseService);

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

            _itemService.AdjustQuantity(NewCore, 5);
            _itemService.AdjustQuantity(NewCore1, 5);
            _itemService.AdjustQuantity(NewCore2, 5);
            _itemService.AdjustQuantity(NewCore3, 5);
            _itemService.AdjustQuantity(NewCore4, 5);

            _itemService.AdjustQuantity(UsedCore, 5);
            _itemService.AdjustQuantity(UsedCore1, 5);
            _itemService.AdjustQuantity(UsedCore2, 5);
            _itemService.AdjustQuantity(UsedCore3, 5);
            _itemService.AdjustQuantity(UsedCore4, 5);

            _itemService.AdjustQuantity(RollerUsedCore, 5);
            _itemService.AdjustQuantity(RollerUsedCore1, 5);
            _itemService.AdjustQuantity(RollerUsedCore2, 5);
            _itemService.AdjustQuantity(RollerUsedCore3, 5);
            _itemService.AdjustQuantity(RollerUsedCore4, 5);

            _itemService.AdjustQuantity(RollerNewCore, 5);
            _itemService.AdjustQuantity(RollerNewCore1, 5);
            _itemService.AdjustQuantity(RollerNewCore2, 5);
            _itemService.AdjustQuantity(RollerNewCore3, 5);
            _itemService.AdjustQuantity(RollerNewCore4, 5);

            _warehouseItemService.AdjustQuantity(_warehouseItemService.FindOrCreateObject(localWarehouse.Id, NewCore.Id), 5);
            _warehouseItemService.AdjustQuantity(_warehouseItemService.FindOrCreateObject(localWarehouse.Id, NewCore1.Id), 5);
            _warehouseItemService.AdjustQuantity(_warehouseItemService.FindOrCreateObject(localWarehouse.Id, NewCore2.Id), 5);
            _warehouseItemService.AdjustQuantity(_warehouseItemService.FindOrCreateObject(localWarehouse.Id, NewCore3.Id), 5);
            _warehouseItemService.AdjustQuantity(_warehouseItemService.FindOrCreateObject(localWarehouse.Id, NewCore4.Id), 5);

            _warehouseItemService.AdjustQuantity(_warehouseItemService.FindOrCreateObject(localWarehouse.Id, UsedCore.Id), 5);
            _warehouseItemService.AdjustQuantity(_warehouseItemService.FindOrCreateObject(localWarehouse.Id, UsedCore1.Id), 5);
            _warehouseItemService.AdjustQuantity(_warehouseItemService.FindOrCreateObject(localWarehouse.Id, UsedCore2.Id), 5);
            _warehouseItemService.AdjustQuantity(_warehouseItemService.FindOrCreateObject(localWarehouse.Id, UsedCore3.Id), 5);
            _warehouseItemService.AdjustQuantity(_warehouseItemService.FindOrCreateObject(localWarehouse.Id, UsedCore4.Id), 5);

            _warehouseItemService.AdjustQuantity(_warehouseItemService.FindOrCreateObject(localWarehouse.Id, RollerNewCore.Id), 5);
            _warehouseItemService.AdjustQuantity(_warehouseItemService.FindOrCreateObject(localWarehouse.Id, RollerNewCore1.Id), 5);
            _warehouseItemService.AdjustQuantity(_warehouseItemService.FindOrCreateObject(localWarehouse.Id, RollerNewCore2.Id), 5);
            _warehouseItemService.AdjustQuantity(_warehouseItemService.FindOrCreateObject(localWarehouse.Id, RollerNewCore3.Id), 5);
            _warehouseItemService.AdjustQuantity(_warehouseItemService.FindOrCreateObject(localWarehouse.Id, RollerNewCore4.Id), 5);

            _warehouseItemService.AdjustQuantity(_warehouseItemService.FindOrCreateObject(localWarehouse.Id, RollerUsedCore.Id), 5);
            _warehouseItemService.AdjustQuantity(_warehouseItemService.FindOrCreateObject(localWarehouse.Id, RollerUsedCore1.Id), 5);
            _warehouseItemService.AdjustQuantity(_warehouseItemService.FindOrCreateObject(localWarehouse.Id, RollerUsedCore2.Id), 5);
            _warehouseItemService.AdjustQuantity(_warehouseItemService.FindOrCreateObject(localWarehouse.Id, RollerUsedCore3.Id), 5);
            _warehouseItemService.AdjustQuantity(_warehouseItemService.FindOrCreateObject(localWarehouse.Id, RollerUsedCore4.Id), 5);
        }

        public void PopulateCoreIdentifications()
        {
            coreIdentification = new CoreIdentification()
            {
                Code = "CI001",
                CustomerId = customer.Id,
                IsInHouse = false,
                IdentifiedDate = DateTime.Now,
                Quantity = 1,
                WarehouseId = localWarehouse.Id
            };
            coreIdentification = _coreIdentificationService.CreateObject(coreIdentification, _customerService);
            coreIdentificationInHouse = new CoreIdentification()
            {
                Code = "CIH001",
                IsInHouse = true,
                IdentifiedDate = DateTime.Now,
                Quantity = 3,
                WarehouseId = localWarehouse.Id
            };
            coreIdentificationInHouse = _coreIdentificationService.CreateObject(coreIdentificationInHouse, _customerService);
            coreIdentificationCustomer = new CoreIdentification()
            {
                Code = "CIC001",
                CustomerId = customer.Id,
                IsInHouse = false,
                IdentifiedDate = DateTime.Now,
                Quantity = 3,
                WarehouseId = localWarehouse.Id
            };
            coreIdentificationCustomer = _coreIdentificationService.CreateObject(coreIdentificationCustomer, _customerService);

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
                MaterialCase = Core.Constants.Constant.MaterialCase.Used
            };
            coreIdentificationDetail = _coreIdentificationDetailService.CreateObject(coreIdentificationDetail, _coreIdentificationService, _coreBuilderService, _rollerTypeService, _machineService);

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
                MaterialCase = Core.Constants.Constant.MaterialCase.Used
            };
            coreIDInHouse1 = _coreIdentificationDetailService.CreateObject(coreIDInHouse1, _coreIdentificationService, _coreBuilderService, _rollerTypeService, _machineService);

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
                MaterialCase = Core.Constants.Constant.MaterialCase.Used
            };
            coreIDInHouse2 = _coreIdentificationDetailService.CreateObject(coreIDInHouse2, _coreIdentificationService, _coreBuilderService, _rollerTypeService, _machineService);

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
                MaterialCase = Core.Constants.Constant.MaterialCase.Used
            };
            coreIDInHouse3 = _coreIdentificationDetailService.CreateObject(coreIDInHouse3, _coreIdentificationService, _coreBuilderService, _rollerTypeService, _machineService);

            coreIDCustomer1 = new CoreIdentificationDetail()
            {
                CoreBuilderId = coreBuilder3.Id,
                CoreIdentificationId = coreIdentificationCustomer.Id,
                MachineId = machine.Id,
                DetailId = 1,
                RollerTypeId = typeInkDistD.Id,
                RD = (decimal)9.1,
                CD = (decimal)9.2,
                TL = (decimal)9.9,
                WL = (decimal)9.3,
                RL = (decimal)8.2,
                MaterialCase = Core.Constants.Constant.MaterialCase.Used
            };
            coreIDCustomer1 = _coreIdentificationDetailService.CreateObject(coreIDCustomer1, _coreIdentificationService, _coreBuilderService, _rollerTypeService, _machineService);

            coreIDCustomer2 = new CoreIdentificationDetail()
            {
                CoreBuilderId = coreBuilder3.Id,
                CoreIdentificationId = coreIdentificationCustomer.Id,
                MachineId = machine.Id,
                DetailId = 2,
                RollerTypeId = typeInkDistD.Id,
                RD = (decimal)9.1,
                CD = (decimal)9.2,
                TL = (decimal)9.9,
                WL = (decimal)9.3,
                RL = (decimal)8.2,
                MaterialCase = Core.Constants.Constant.MaterialCase.Used
            };
            coreIDCustomer2 = _coreIdentificationDetailService.CreateObject(coreIDCustomer2, _coreIdentificationService, _coreBuilderService, _rollerTypeService, _machineService);

            coreIDCustomer3 = new CoreIdentificationDetail()
            {
                CoreBuilderId = coreBuilder4.Id,
                CoreIdentificationId = coreIdentificationCustomer.Id,
                MachineId = machine.Id,
                DetailId = 3,
                RollerTypeId = typeInkDistH.Id,
                RD = (decimal)9.1,
                CD = (decimal)9.2,
                TL = (decimal)9.9,
                WL = (decimal)9.3,
                RL = (decimal)8.2,
                MaterialCase = Core.Constants.Constant.MaterialCase.Used
            };
            coreIDCustomer3 = _coreIdentificationDetailService.CreateObject(coreIDCustomer3, _coreIdentificationService, _coreBuilderService, _rollerTypeService, _machineService);
        }
        
        public void PopulateRecoveryOrders()
        {
            coreIdentification = _coreIdentificationService.ConfirmObject(coreIdentification, _coreIdentificationDetailService, _stockMutationService, _recoveryOrderService, _recoveryOrderDetailService, _coreBuilderService, _itemService, _warehouseItemService, _barringService);
            coreIdentificationCustomer = _coreIdentificationService.ConfirmObject(coreIdentificationCustomer, _coreIdentificationDetailService, _stockMutationService, _recoveryOrderService, _recoveryOrderDetailService, _coreBuilderService, _itemService, _warehouseItemService, _barringService);
            coreIdentificationInHouse = _coreIdentificationService.ConfirmObject(coreIdentificationInHouse, _coreIdentificationDetailService, _stockMutationService, _recoveryOrderService, _recoveryOrderDetailService, _coreBuilderService, _itemService, _warehouseItemService, _barringService);

            recoveryOrder = new RecoveryOrder()
            {
                Code = "ROX",
                CoreIdentificationId = coreIdentification.Id,
                QuantityReceived = coreIdentification.Quantity,
                WarehouseId = localWarehouse.Id
            };
            recoveryOrder = _recoveryOrderService.CreateObject(recoveryOrder, _coreIdentificationService);

            recoveryOrderInHouse = new RecoveryOrder()
            {
                Code = "RO001",
                CoreIdentificationId = coreIdentificationInHouse.Id,
                QuantityReceived = coreIdentificationInHouse.Quantity,
                WarehouseId = localWarehouse.Id
            };
            recoveryOrderInHouse = _recoveryOrderService.CreateObject(recoveryOrderInHouse, _coreIdentificationService);
            
            recoveryOrderCustomer = new RecoveryOrder()
            {
                Code = "RO002",
                CoreIdentificationId = coreIdentificationCustomer.Id,
                QuantityReceived = coreIdentificationCustomer.Quantity,
                WarehouseId = localWarehouse.Id
            };
            recoveryOrderCustomer = _recoveryOrderService.CreateObject(recoveryOrderCustomer, _coreIdentificationService);

            recoveryODInHouse1 = new RecoveryOrderDetail()
            {
                RecoveryOrderId = recoveryOrderInHouse.Id,
                CoreIdentificationDetailId = coreIDInHouse1.Id,
                Acc = "Y",
                CoreTypeCase = Core.Constants.Constant.CoreTypeCase.R,
                RepairRequestCase = Core.Constants.Constant.RepairRequestCase.BearingSeat,
                RollerBuilderId = rollerBuilder1.Id,
            };
            recoveryODInHouse1 = _recoveryOrderDetailService.CreateObject(recoveryODInHouse1, _recoveryOrderService, _coreIdentificationDetailService, _rollerBuilderService);

            recoveryODInHouse2 = new RecoveryOrderDetail()
            {
                RecoveryOrderId = recoveryOrderInHouse.Id,
                CoreIdentificationDetailId = coreIDInHouse2.Id,
                Acc = "Y",
                CoreTypeCase = Core.Constants.Constant.CoreTypeCase.R,
                RepairRequestCase = Core.Constants.Constant.RepairRequestCase.BearingSeat,
                RollerBuilderId = rollerBuilder1.Id,
            };
            recoveryODInHouse2 = _recoveryOrderDetailService.CreateObject(recoveryODInHouse2, _recoveryOrderService, _coreIdentificationDetailService, _rollerBuilderService);

            recoveryODInHouse3 = new RecoveryOrderDetail()
            {
                RecoveryOrderId = recoveryOrderInHouse.Id,
                CoreIdentificationDetailId = coreIDInHouse3.Id,
                Acc = "Y",
                CoreTypeCase = Core.Constants.Constant.CoreTypeCase.R,
                RepairRequestCase = Core.Constants.Constant.RepairRequestCase.BearingSeat,
                RollerBuilderId = rollerBuilder2.Id,
            };
            recoveryODInHouse3 = _recoveryOrderDetailService.CreateObject(recoveryODInHouse3, _recoveryOrderService, _coreIdentificationDetailService, _rollerBuilderService);

            recoveryODCustomer1 = new RecoveryOrderDetail()
            {
                RecoveryOrderId = recoveryOrderCustomer.Id,
                CoreIdentificationDetailId = coreIDCustomer1.Id,
                Acc = "Y",
                CoreTypeCase = Core.Constants.Constant.CoreTypeCase.R,
                RepairRequestCase = Core.Constants.Constant.RepairRequestCase.BearingSeat,
                RollerBuilderId = rollerBuilder3.Id,
            };
            recoveryODCustomer1 = _recoveryOrderDetailService.CreateObject(recoveryODCustomer1, _recoveryOrderService, _coreIdentificationDetailService, _rollerBuilderService);

            recoveryODCustomer2 = new RecoveryOrderDetail()
            {
                RecoveryOrderId = recoveryOrderCustomer.Id,
                CoreIdentificationDetailId = coreIDCustomer2.Id,
                Acc = "Y",
                CoreTypeCase = Core.Constants.Constant.CoreTypeCase.R,
                RepairRequestCase = Core.Constants.Constant.RepairRequestCase.BearingSeat,
                RollerBuilderId = rollerBuilder3.Id,
            };
            recoveryODCustomer2 = _recoveryOrderDetailService.CreateObject(recoveryODCustomer2, _recoveryOrderService, _coreIdentificationDetailService, _rollerBuilderService);

            recoveryODCustomer3 = new RecoveryOrderDetail()
            {
                RecoveryOrderId = recoveryOrderCustomer.Id,
                CoreIdentificationDetailId = coreIDCustomer3.Id,
                Acc = "Y",
                CoreTypeCase = Core.Constants.Constant.CoreTypeCase.R,
                RepairRequestCase = Core.Constants.Constant.RepairRequestCase.BearingSeat,
                RollerBuilderId = rollerBuilder4.Id,
            };
            recoveryODCustomer3 = _recoveryOrderDetailService.CreateObject(recoveryODCustomer3, _recoveryOrderService, _coreIdentificationDetailService, _rollerBuilderService);

        }

        public void PopulateBarringOrders()
        {
            bargeneric = new Item()
            {
                ItemTypeId = typeBar.Id,
                Category = "bar",
                Name = "Bar Generic",
                UoMId = Pcs.Id,
                Sku = "BGEN"
            };
            _itemService.CreateObject(bargeneric, _uomService, _itemTypeService, _warehouseItemService, _warehouseService);
            _itemService.AdjustQuantity(bargeneric, 5);
            _warehouseItemService.AdjustQuantity(_warehouseItemService.FindOrCreateObject(localWarehouse.Id, bargeneric.Id), 5);

            barleft1 = new Item()
            {
                ItemTypeId = typeBar.Id,
                Category = "bar",
                Name = "Bar Left 1",
                UoMId = Pcs.Id,
                Sku = "BL1"
            };
            _itemService.CreateObject(barleft1, _uomService, _itemTypeService, _warehouseItemService, _warehouseService);
            _itemService.AdjustQuantity(barleft1, 2);
            _warehouseItemService.AdjustQuantity(_warehouseItemService.FindOrCreateObject(localWarehouse.Id, barleft1.Id), 2);

            barleft2 = new Item()
            {
                ItemTypeId = typeBar.Id,
                Category = "bar",
                Name = "Bar Left 2",
                UoMId = Pcs.Id,
                Sku = "BL2"
            };
            _itemService.CreateObject(barleft2, _uomService, _itemTypeService, _warehouseItemService, _warehouseService);
            _itemService.AdjustQuantity(barleft2, 5);
            _warehouseItemService.AdjustQuantity(_warehouseItemService.FindOrCreateObject(localWarehouse.Id, barleft2.Id), 5);

            barright1 = new Item()
            {
                ItemTypeId = typeBar.Id,
                Category = "bar",
                Name = "Bar Right 1",
                UoMId = Pcs.Id,
                Sku = "BR1"
            };
            _itemService.CreateObject(barright1, _uomService, _itemTypeService, _warehouseItemService, _warehouseService);
            _itemService.AdjustQuantity(barright1, 2);
            _warehouseItemService.AdjustQuantity(_warehouseItemService.FindOrCreateObject(localWarehouse.Id, barright1.Id), 2);

            barright2 = new Item()
            {
                ItemTypeId = typeBar.Id,
                Category = "bar",
                Name = "Bar Right 2",
                UoMId = Pcs.Id,
                Sku = "BR2"
            };
            _itemService.CreateObject(barright2, _uomService, _itemTypeService, _warehouseItemService, _warehouseService);
            _itemService.AdjustQuantity(barright2, 2);
            _warehouseItemService.AdjustQuantity(_warehouseItemService.FindOrCreateObject(localWarehouse.Id, barright2.Id), 2);

            blanket1 = new Item()
            {
                ItemTypeId = typeBlanket.Id,
                Category = "Blanket",
                Name = "Blanket1",
                UoMId = Pcs.Id,
                Sku = "BLK1"
            };
            _itemService.CreateObject(blanket1, _uomService, _itemTypeService, _warehouseItemService, _warehouseService);
            _itemService.AdjustQuantity(blanket1, 10);
            _warehouseItemService.AdjustQuantity(_warehouseItemService.FindOrCreateObject(localWarehouse.Id, blanket1.Id), 10);

            blanket2 = new Item()
            {
                ItemTypeId = typeBlanket.Id,
                Category = "Blanket",
                Name = "Blanket2",
                UoMId = Pcs.Id,
                Sku = "BLK2"
            };
            _itemService.CreateObject(blanket2, _uomService, _itemTypeService, _warehouseItemService, _warehouseService);
            _itemService.AdjustQuantity(blanket2, 4);
            _warehouseItemService.AdjustQuantity(_warehouseItemService.FindOrCreateObject(localWarehouse.Id, blanket2.Id), 4);

            blanket3 = new Item()
            {
                ItemTypeId = typeBlanket.Id,
                Category = "Blanket",
                Name = "Blanket3",
                UoMId = Pcs.Id,
                Sku = "BLK3"
            };
            _itemService.CreateObject(blanket3, _uomService, _itemTypeService, _warehouseItemService, _warehouseService);
            _itemService.AdjustQuantity(blanket3, 3);
            _warehouseItemService.AdjustQuantity(_warehouseItemService.FindOrCreateObject(localWarehouse.Id, blanket3.Id), 3);

            barring1 = new Barring()
            {
                ItemTypeId = typeBarring.Id,
                Category = "Barring",
                Name = "Barring1",
                UoMId = Pcs.Id,
                Sku = "BRG1",
                RollNo = "BRG1_123",
                AC = 10,
                AR = 15,
                BlanketItemId = blanket1.Id,
                LeftBarItemId = bargeneric.Id,
                RightBarItemId = bargeneric.Id,
                CustomerId = customer.Id,
                KS = 1,
                thickness = 1,
                MachineId = machine.Id
            };
            _barringService.CreateObject(barring1, _barringService, _uomService,_itemService, _itemTypeService, _customerService, _machineService, _warehouseItemService, _warehouseService);
            _barringService.AdjustQuantity(barring1, 2);
            _warehouseItemService.AdjustQuantity(_warehouseItemService.FindOrCreateObject(localWarehouse.Id, barring1.Id), 2);

            barring2 = new Barring()
            {
                ItemTypeId = typeBarring.Id,
                Category = "Barring",
                Name = "Barring2",
                UoMId = Pcs.Id,
                Sku = "BRG2",
                RollNo = "BRG2_123",
                AC = 3,
                AR = 5,
                BlanketItemId = blanket2.Id,
                LeftBarItemId = barleft1.Id,
                RightBarItemId = barright1.Id,
                CustomerId = customer.Id,
                KS = 1,
                thickness = 1,
                MachineId = machine.Id
            };
            _barringService.CreateObject(barring2, _barringService, _uomService, _itemService, _itemTypeService, _customerService, _machineService, _warehouseItemService, _warehouseService);
            _barringService.AdjustQuantity(barring2, 2);
            _warehouseItemService.AdjustQuantity(_warehouseItemService.FindOrCreateObject(localWarehouse.Id, barring2.Id), 2);
            
            barring3 = new Barring()
            {
                ItemTypeId = typeBarring.Id,
                Category = "Barring",
                Name = "Barring3",
                UoMId = Pcs.Id,
                Sku = "BRG3",
                RollNo = "BRG3_123",
                AC = 7,
                AR = 10,
                BlanketItemId = blanket3.Id,
                LeftBarItemId = barleft2.Id,
                RightBarItemId = barright2.Id,
                CustomerId = customer.Id,
                KS = 1,
                thickness = 1,
                MachineId = machine.Id
            };
            _barringService.CreateObject(barring3, _barringService, _uomService, _itemService, _itemTypeService, _customerService, _machineService, _warehouseItemService, _warehouseService);
            _barringService.AdjustQuantity(barring3, 3);
            _warehouseItemService.AdjustQuantity(_warehouseItemService.FindOrCreateObject(localWarehouse.Id, barring3.Id), 3);

            barringOrderCustomer = new BarringOrder()
            {
                CustomerId = customer.Id,
                QuantityReceived = 4,
                Code = "BO0001",
                WarehouseId = localWarehouse.Id
            };
            _barringOrderService.CreateObject(barringOrderCustomer);

            barringODCustomer1 = new BarringOrderDetail()
            {
                BarringId = barring1.Id,
                BarringOrderId = barringOrderCustomer.Id,
                IsBarRequired = true
            };
            _barringOrderDetailService.CreateObject(barringODCustomer1, _barringOrderService, _barringService);

            barringODCustomer2 = new BarringOrderDetail()
            {
                BarringId = barring1.Id,
                BarringOrderId = barringOrderCustomer.Id,
                IsBarRequired = true
            };
            _barringOrderDetailService.CreateObject(barringODCustomer2, _barringOrderService, _barringService);

            barringODCustomer3 = new BarringOrderDetail()
            {
                BarringId = barring2.Id,
                BarringOrderId = barringOrderCustomer.Id,
                IsBarRequired = true
            };
            _barringOrderDetailService.CreateObject(barringODCustomer3, _barringOrderService, _barringService);

            barringODCustomer4 = new BarringOrderDetail()
            {
                BarringId = barring2.Id,
                BarringOrderId = barringOrderCustomer.Id,
                IsBarRequired = true
            };
            _barringOrderDetailService.CreateObject(barringODCustomer4, _barringOrderService, _barringService);
        }
    }
}
