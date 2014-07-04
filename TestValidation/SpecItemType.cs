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

    public class SpecItemType: nspec
    {
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
        Item item;
        ItemType typeAccessory;
        ItemType typeBearing;
        ItemType typeBlanket;
        ItemType typeCore;
        ItemType typeCompound;
        ItemType typeChemical;
        ItemType typeConsumable;
        ItemType typeGlue;
        ItemType typeUnderpacking;
        ItemType typeRoller;
        
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
        void itemtype_validation()
        {
        
            it["validates_itemtypes"] = () =>
            {
                typeAccessory.Errors.Count().should_be(0);
                typeBearing.Errors.Count().should_be(0);
                typeBlanket.Errors.Count().should_be(0);
                typeCore.Errors.Count().should_be(0);
                typeConsumable.Errors.Count().should_be(0);
                typeChemical.Errors.Count().should_be(0);
                typeCompound.Errors.Count().should_be(0);
                typeGlue.Errors.Count.should_be(0);
                typeUnderpacking.Errors.Count().should_be(0);
                typeRoller.Errors.Count().should_be(0);
            };

            it["itemtype_with_no_name"] = () =>
            {
                ItemType nonameitemtype = new ItemType()
                {
                    Name = "     ",
                    Description = "Empty"
                };
                nonameitemtype = _itemTypeService.CreateObject(nonameitemtype);
                nonameitemtype.Errors.Count().should_not_be(0);
            };

            it["delete_itemtype"] = () =>
            {
                typeGlue = _itemTypeService.SoftDeleteObject(typeGlue, _itemService);
                typeGlue.Errors.Count().should_be(0);
            };

            it["delete_itemtype_with_item"] = () =>
            {
                Item glue101 = new Item()
                {
                    ItemTypeId = typeGlue.Id,
                    Name = "Glue101",
                    Category = "Glue",
                    Quantity = 100,
                    Sku = "G101",
                    UoM = "Pcs"
                };
                glue101 = _itemService.CreateObject(glue101, _itemTypeService);
                typeGlue = _itemTypeService.SoftDeleteObject(typeGlue, _itemService);
                typeGlue.Errors.Count().should_not_be(0);
            };
        }
    }
}