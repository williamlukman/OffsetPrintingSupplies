﻿using System;
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

        public ItemType typeAccessory, typeBearing, typeBlanket, typeCore, typeCompound, typeChemical,
                        typeConsumable, typeGlue, typeUnderpacking, typeRoller;
        public RollerType typeDamp, typeFoundDT, typeInkFormX, typeInkDistD, typeInkDistM, typeInkDistE,
                        typeInkDuctB, typeInkDistH, typeInkFormW, typeInkDistHQ, typeDampFormDQ, typeInkFormY;
        public Item item, itemCompound, itemCompound1, itemCompound2, itemAccessory1, itemAccessory2;
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

        public DataBuilder()
        {
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

            typeAccessory = _itemTypeService.CreateObject("Accessory", "Accessory");
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
            PopulateItem();
            PopulateSingles();
            PopulateBuilders();
            PopulateCoreIdentifications();
            PopulateRecoveryOrders();
        }

        public void PopulateItem()
        {
            itemCompound = new Item()
            {
                ItemTypeId = _itemTypeService.GetObjectByName("Compound").Id,
                Name = "Compound RB else",
                Category = "Compound",
                Sku = "CMP123",
                UoM = "Tub",
                Quantity = 5
            };
            itemCompound = _itemService.CreateObject(itemCompound, _itemTypeService);

            itemCompound1 = new Item()
            {
                ItemTypeId = _itemTypeService.GetObjectByName("Compound").Id,
                Name = "Compound RB1",
                Category = "Compound",
                Sku = "CMP101",
                UoM = "Tub",
                Quantity = 2
            };
            itemCompound1 = _itemService.CreateObject(itemCompound1, _itemTypeService);

            itemCompound2 = new Item()
            {
                ItemTypeId = _itemTypeService.GetObjectByName("Compound").Id,
                Name = "Compound RB2",
                Category = "Compound",
                Sku = "CMP102",
                UoM = "Tub",
                Quantity = 2
            };
            itemCompound2 = _itemService.CreateObject(itemCompound2, _itemTypeService);

            itemAccessory1 = new Item()
            {
                ItemTypeId = _itemTypeService.GetObjectByName("Accessory").Id,
                Name = "Accessory Sample 1",
                Category = "Accessory",
                Sku = "ACC001",
                UoM = "Tub",
                Quantity = 5
            };
            itemAccessory1 = _itemService.CreateObject(itemAccessory1, _itemTypeService);

            itemAccessory2 = new Item()
            {
                ItemTypeId = _itemTypeService.GetObjectByName("Accessory").Id,
                Name = "Accessory Sample 2",
                Category = "Accessory",
                Sku = "ACC002",
                UoM = "Tub",
                Quantity = 5
            };
            itemAccessory2 = _itemService.CreateObject(itemAccessory2, _itemTypeService);
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
                Category = "X"
            };
            coreBuilder = _coreBuilderService.CreateObject(coreBuilder, _itemService, _itemTypeService);

            coreBuilder1 = new CoreBuilder()
            {
                BaseSku = "CBA001",
                SkuNewCore = "CB001N",
                SkuUsedCore = "CB001U",
                Name = "Core A 001",
                Category = "A"
            };
            coreBuilder1 = _coreBuilderService.CreateObject(coreBuilder1, _itemService, _itemTypeService);

            coreBuilder2 = new CoreBuilder()
            {
                BaseSku = "CBA002",
                SkuNewCore = "CB002N",
                SkuUsedCore = "CB002U",
                Name = "Core A 002",
                Category = "A"
            };
            coreBuilder2 = _coreBuilderService.CreateObject(coreBuilder2, _itemService, _itemTypeService);

            coreBuilder3 = new CoreBuilder()
            {
                BaseSku = "CBA003",
                SkuNewCore = "CB003N",
                SkuUsedCore = "CB003U",
                Name = "Core A 003",
                Category = "A"
            };
            coreBuilder3 = _coreBuilderService.CreateObject(coreBuilder3, _itemService, _itemTypeService);

            coreBuilder4 = new CoreBuilder()
            {
                BaseSku = "CBA004",
                SkuNewCore = "CB004N",
                SkuUsedCore = "CB004U",
                Name = "Core A 004",
                Category = "A"
            };
            coreBuilder4 = _coreBuilderService.CreateObject(coreBuilder4, _itemService, _itemTypeService);

            rollerBuilder = new RollerBuilder()
            {
                BaseSku = "RBX",
                SkuNewRoller = "RBXN",
                SkuUsedRoller = "RBXU",
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
                RollerTypeId = typeDamp.Id
            };
            rollerBuilder = _rollerBuilderService.CreateObject(rollerBuilder, _machineService,
                                                               _itemService, _itemTypeService, _coreBuilderService, _rollerTypeService);

            rollerBuilder1 = new RollerBuilder()
            {
                BaseSku = "RBA001",
                SkuNewRoller = "RBA001N",
                SkuUsedRoller = "RBA001U",
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
                RollerTypeId = typeFoundDT.Id
            };
            rollerBuilder1 = _rollerBuilderService.CreateObject(rollerBuilder1, _machineService,
                                                               _itemService, _itemTypeService, _coreBuilderService, _rollerTypeService);

            rollerBuilder2 = new RollerBuilder()
            {
                BaseSku = "RBA002",
                SkuNewRoller = "RBA002N",
                SkuUsedRoller = "RBA002U",
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
                RollerTypeId = typeDampFormDQ.Id
            };
            rollerBuilder2 = _rollerBuilderService.CreateObject(rollerBuilder2, _machineService,
                                                               _itemService, _itemTypeService, _coreBuilderService, _rollerTypeService);

            rollerBuilder3 = new RollerBuilder()
            {
                BaseSku = "RBA003",
                SkuNewRoller = "RBA003N",
                SkuUsedRoller = "RBA003U",
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
                RollerTypeId = typeInkDistD.Id
            };
            rollerBuilder3 = _rollerBuilderService.CreateObject(rollerBuilder3, _machineService,
                                                               _itemService, _itemTypeService, _coreBuilderService, _rollerTypeService);

            rollerBuilder4 = new RollerBuilder()
            {
                BaseSku = "RBA004",
                SkuNewRoller = "RBA004N",
                SkuUsedRoller = "RBA004U",
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
                RollerTypeId = typeInkDistH.Id
            };
            rollerBuilder4 = _rollerBuilderService.CreateObject(rollerBuilder4, _machineService,
                                                               _itemService, _itemTypeService, _coreBuilderService, _rollerTypeService);

            _itemService.AdjustQuantity(_coreBuilderService.GetNewCore(coreBuilder.Id), 5);
            _itemService.AdjustQuantity(_coreBuilderService.GetNewCore(coreBuilder1.Id), 5);
            _itemService.AdjustQuantity(_coreBuilderService.GetNewCore(coreBuilder2.Id), 5);
            _itemService.AdjustQuantity(_coreBuilderService.GetNewCore(coreBuilder3.Id), 5);
            _itemService.AdjustQuantity(_coreBuilderService.GetNewCore(coreBuilder4.Id), 5);

            _itemService.AdjustQuantity(_coreBuilderService.GetUsedCore(coreBuilder.Id), 5);
            _itemService.AdjustQuantity(_coreBuilderService.GetUsedCore(coreBuilder1.Id), 5);
            _itemService.AdjustQuantity(_coreBuilderService.GetUsedCore(coreBuilder2.Id), 5);
            _itemService.AdjustQuantity(_coreBuilderService.GetUsedCore(coreBuilder3.Id), 5);
            _itemService.AdjustQuantity(_coreBuilderService.GetUsedCore(coreBuilder4.Id), 5);

            _itemService.AdjustQuantity(_rollerBuilderService.GetNewRoller(rollerBuilder.Id), 5);
            _itemService.AdjustQuantity(_rollerBuilderService.GetNewRoller(rollerBuilder1.Id), 5);
            _itemService.AdjustQuantity(_rollerBuilderService.GetNewRoller(rollerBuilder2.Id), 5);
            _itemService.AdjustQuantity(_rollerBuilderService.GetNewRoller(rollerBuilder3.Id), 5);
            _itemService.AdjustQuantity(_rollerBuilderService.GetNewRoller(rollerBuilder4.Id), 5);

            _itemService.AdjustQuantity(_rollerBuilderService.GetUsedRoller(rollerBuilder.Id), 5);
            _itemService.AdjustQuantity(_rollerBuilderService.GetUsedRoller(rollerBuilder1.Id), 5);
            _itemService.AdjustQuantity(_rollerBuilderService.GetUsedRoller(rollerBuilder2.Id), 5);
            _itemService.AdjustQuantity(_rollerBuilderService.GetUsedRoller(rollerBuilder3.Id), 5);
            _itemService.AdjustQuantity(_rollerBuilderService.GetUsedRoller(rollerBuilder4.Id), 5);
        }

        public void PopulateCoreIdentifications()
        {
            coreIdentification = new CoreIdentification()
            {
                Code = "CI001",
                CustomerId = customer.Id,
                IsInHouse = false,
                IdentifiedDate = DateTime.Now,
                Quantity = 1
            };
            coreIdentification = _coreIdentificationService.CreateObject(coreIdentification, _customerService);
            coreIdentificationInHouse = new CoreIdentification()
            {
                Code = "CIH001",
                IsInHouse = true,
                IdentifiedDate = DateTime.Now,
                Quantity = 3
            };
            coreIdentificationInHouse = _coreIdentificationService.CreateObject(coreIdentificationInHouse, _customerService);
            coreIdentificationCustomer = new CoreIdentification()
            {
                Code = "CIC001",
                CustomerId = customer.Id,
                IsInHouse = false,
                IdentifiedDate = DateTime.Now,
                Quantity = 3
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
            coreIdentification = _coreIdentificationService.ConfirmObject(coreIdentification, _coreIdentificationDetailService, _recoveryOrderService, _recoveryOrderDetailService, _coreBuilderService, _itemService);
            coreIdentificationCustomer = _coreIdentificationService.ConfirmObject(coreIdentificationCustomer, _coreIdentificationDetailService, _recoveryOrderService, _recoveryOrderDetailService, _coreBuilderService, _itemService);
            coreIdentificationInHouse = _coreIdentificationService.ConfirmObject(coreIdentificationInHouse, _coreIdentificationDetailService, _recoveryOrderService, _recoveryOrderDetailService, _coreBuilderService, _itemService); 

            recoveryOrder = new RecoveryOrder()
            {
                Code = "ROX",
                CoreIdentificationId = coreIdentification.Id,
                QuantityReceived = coreIdentification.Quantity
            };
            recoveryOrder = _recoveryOrderService.CreateObject(recoveryOrder, _coreIdentificationService);

            recoveryOrderInHouse = new RecoveryOrder()
            {
                Code = "RO001",
                CoreIdentificationId = coreIdentificationInHouse.Id,
                QuantityReceived = coreIdentificationInHouse.Quantity
            };
            recoveryOrderInHouse = _recoveryOrderService.CreateObject(recoveryOrderInHouse, _coreIdentificationService);
            
            recoveryOrderCustomer = new RecoveryOrder()
            {
                Code = "RO002",
                CoreIdentificationId = coreIdentificationCustomer.Id,
                QuantityReceived = coreIdentificationCustomer.Quantity,                
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
    }
}