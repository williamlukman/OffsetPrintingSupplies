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

    public class SpecItem: nspec
    {
        Item item;
        
        ICoreBuilderService _coreBuilderService;
        ICoreIdentificationService _coreIdentificationService;
        ICoreIdentificationDetailService _coreIdentificationDetailService;
        ICustomerService _customerService;
        IItemService _itemService;
        IItemTypeService _itemTypeService;
        IMachineService _machineService;
        IRecoveryAccessoryDetailService _recoveryAccessoryDetailService;
        IRecoveryOrderDetailService _recoveryOrderDetailService;
        IRecoveryOrderService _recoveryOrderService;
        IRollerBuilderService _rollerBuilderService;
        IRollerTypeService _rollerTypeService;
        
        void before_each()
        {
            var db = new OffsetPrintingSuppliesEntities();
            using (db)
            {
                db.DeleteAllTables();
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

                _itemTypeService.CreateObject("Accessory", "Accessory");
                _itemTypeService.CreateObject("Bearing", "Bearing");
                _itemTypeService.CreateObject("Blanket", "Blanket");
                _itemTypeService.CreateObject("Chemical", "Chemical");
                _itemTypeService.CreateObject("Compound", "Compound");
                _itemTypeService.CreateObject("Consumable", "Consumable");
                _itemTypeService.CreateObject("Core", "Core");
                _itemTypeService.CreateObject("Glue", "Glue");
                _itemTypeService.CreateObject("Underpacking", "Underpacking");
                _itemTypeService.CreateObject("Roller", "Roller");

                _rollerTypeService.CreateObject("Damp", "Damp");
                _rollerTypeService.CreateObject("Found DT", "Found DT");
                _rollerTypeService.CreateObject("Ink Form X", "Ink Form X");
                _rollerTypeService.CreateObject("Ink Dist D", "Ink Dist D");
                _rollerTypeService.CreateObject("Ink Dist M", "Ink Dist M");
                _rollerTypeService.CreateObject("Ink Dist E", "Ink Dist E");
                _rollerTypeService.CreateObject("Ink Duct B", "Ink Duct B");
                _rollerTypeService.CreateObject("Ink Dist H", "Ink Dist H");
                _rollerTypeService.CreateObject("Ink Form W", "Ink Form W");
                _rollerTypeService.CreateObject("Ink Dist HQ", "Ink Dist HQ");
                _rollerTypeService.CreateObject("Damp Form DQ", "Damp Form DQ");
                _rollerTypeService.CreateObject("Ink Form Y", "Ink Form Y");

                item = new Item()
                {
                    ItemTypeId = _itemTypeService.GetObjectByName("Accessory").Id,
                    Sku = "ABC1001",
                    Name = "ABC",
                    Category = "ABC123",
                    UoM = "Pcs",
                    Quantity = 0
                };
                item = _itemService.CreateObject(item, _itemTypeService);
            }
        }

        /*
         * STEPS:
         * 1. Create valid item
         * 2. Create invalid item with no name
         * 3. Create invalid items with same SKU
         * 4a. Delete item
         * 4b. Delete item with stock mutations
         */
        void item_validation()
        {
        
            it["validates_item"] = () =>
            {
                item.Errors.Count().should_be(0);
            };

            it["item_with_no_name"] = () =>
            {
                Item nonameitem = new Item()
                {
                    ItemTypeId = _itemTypeService.GetObjectByName("Accessory").Id,
                    Sku = "ABC1002",
                    Name = "     ",
                    Category = "ABC222",
                    UoM = "Pcs",
                    Quantity = 0
                };
                nonameitem = _itemService.CreateObject(nonameitem, _itemTypeService);
                nonameitem.Errors.Count().should_not_be(0);
            };

            it["item_with_same_sku"] = () =>
            {
                Item sameskuitem = new Item()
                {
                    ItemTypeId = _itemTypeService.GetObjectByName("Accessory").Id,
                    Sku = "ABC1001",
                    Name = "BBC",
                    Category = "ABC222",
                    UoM = "Pcs",
                    Quantity = 0
                };
                sameskuitem = _itemService.CreateObject(sameskuitem, _itemTypeService);
                sameskuitem.Errors.Count().should_not_be(0);
            };

            it["adjust_quantity_valid"] = () =>
            {
                item = _itemService.AdjustQuantity(item, 10);
                item.Errors.Count().should_be(0);
            };

            it["adjust_quantity_invalid"] = () =>
            {
                item = _itemService.AdjustQuantity(item, -10);
                item.Errors.Count().should_not_be(0);
            };

            it["delete_item"] = () =>
            {
                item = _itemService.SoftDeleteObject(item, _recoveryOrderDetailService, _recoveryAccessoryDetailService, _rollerBuilderService);
                item.Errors.Count().should_be(0);
            };

            it["delete_item_with_compound_inrollerbuilder"] = () =>
            {
                // TODO
                Machine machine = new Machine()
                {
                    Code = "M00001",
                    Name = "Machine 00001",
                    Description = "Machine"
                };
                machine = _machineService.CreateObject(machine);
                CoreBuilder coreBuilder = new CoreBuilder()
                {
                    BaseSku = "CB00001",
                    SkuNewCore = "CB00001N",
                    SkuUsedCore = "CB00001U",
                    Name = "CoreBuilder00001",
                    Category = "X"
                };
                coreBuilder = _coreBuilderService.CreateObject(coreBuilder, _itemService, _itemTypeService);
                CoreIdentification coreIdentification = new CoreIdentification()
                {
                    Code = "CI0001",
                    Quantity = 1,
                    IdentifiedDate = DateTime.Now
                };
                coreIdentification = _coreIdentificationService.CreateObject(coreIdentification, _customerService);
                CoreIdentificationDetail coreIdentificationDetail = new CoreIdentificationDetail()
                {
                    CoreIdentificationId = coreIdentification.Id,
                    DetailId = 1,
                    MaterialCase = 2,
                    CoreBuilderId = coreBuilder.Id,
                    RollerTypeId = _rollerTypeService.GetObjectByName("Found DT").Id,
                    MachineId = machine.Id,
                    RD = 12,
                    CD = 12,
                    RL = 12,
                    WL = 12,
                    TL = 12
                };
                coreIdentificationDetail = _coreIdentificationDetailService.CreateObject(coreIdentificationDetail, _coreIdentificationService, _coreBuilderService, _rollerTypeService, _machineService);
                Item compound = new Item()
                {
                    ItemTypeId = _itemTypeService.GetObjectByName("Compound").Id,
                    Name = "Compound",
                    Category = "Compound",
                    Quantity = 2,
                    Sku = "CMP0001",
                    UoM = "Pcs"
                };
                compound = _itemService.CreateObject(compound, _itemTypeService);
                RollerBuilder rollerBuilder = new RollerBuilder()
                {
                    CoreBuilderId = coreBuilder.Id,
                    RollerTypeId = _rollerTypeService.GetObjectByName("Found DT").Id,
                    MachineId = machine.Id,
                    RD = 13,
                    CD = 13,
                    RL = 13,
                    WL = 13,
                    TL = 13,
                    BaseSku = "RB0001",
                    SkuUsedRoller = "RB0001U",
                    SkuNewRoller = "RB0001N",
                    Name = "Roller Builder",
                    Category = "RB",
                    CompoundId = compound.Id
                };
                rollerBuilder = _rollerBuilderService.CreateObject(rollerBuilder, _machineService, _itemService, _itemTypeService, _coreBuilderService, _rollerTypeService);

                compound = _itemService.SoftDeleteObject(compound, _recoveryOrderDetailService, _recoveryAccessoryDetailService, _rollerBuilderService);
                compound.Errors.Count().should_not_be(0);
            };
        }
    }
}