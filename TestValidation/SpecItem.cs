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
        DataBuilder d;
        void before_each()
        {
            var db = new OffsetPrintingSuppliesEntities();
            using (db)
            {
                db.DeleteAllTables();
                d = new DataBuilder();
                d.item = new Item()
                {
                    ItemTypeId = d._itemTypeService.GetObjectByName("Accessory").Id,
                    Sku = "ABC1001",
                    Name = "ABC",
                    Category = "ABC123",
                    UoM = "Pcs",
                    Quantity = 0
                };
                d.item = d._itemService.CreateObject(d.item, d._itemTypeService, d._warehouseItemService, d._warehouseService);
            }
        }

        void item_validation()
        {
        
            it["validates_item"] = () =>
            {
                d.item.Errors.Count().should_be(0);
            };

            it["item_with_no_name"] = () =>
            {
                Item nonameitem = new Item()
                {
                    ItemTypeId = d._itemTypeService.GetObjectByName("Accessory").Id,
                    Sku = "ABC1002",
                    Name = "     ",
                    Category = "ABC222",
                    UoM = "Pcs",
                    Quantity = 0
                };
                nonameitem = d._itemService.CreateObject(nonameitem, d._itemTypeService, d._warehouseItemService, d._warehouseService);
                nonameitem.Errors.Count().should_not_be(0);
            };

            it["item_with_same_sku"] = () =>
            {
                Item sameskuitem = new Item()
                {
                    ItemTypeId = d._itemTypeService.GetObjectByName("Accessory").Id,
                    Sku = "ABC1001",
                    Name = "BBC",
                    Category = "ABC222",
                    UoM = "Pcs",
                    Quantity = 0
                };
                sameskuitem = d._itemService.CreateObject(sameskuitem, d._itemTypeService, d._warehouseItemService, d._warehouseService);
                sameskuitem.Errors.Count().should_not_be(0);
            };

            it["adjust_quantity_valid"] = () =>
            {
                d.item = d._itemService.AdjustQuantity(d.item, 10);
                d.item.Errors.Count().should_be(0);
            };

            it["adjust_quantity_invalid"] = () =>
            {
                d.item = d._itemService.AdjustQuantity(d.item, -10);
                d.item.Errors.Count().should_not_be(0);
            };

            it["delete_item"] = () =>
            {
                d.item = d._itemService.SoftDeleteObject(d.item, d._recoveryAccessoryDetailService, d._itemTypeService, d._warehouseItemService, d._barringService);
                d.item.Errors.Count().should_be(0);
            };

            it["delete_item_with_compound_inrollerbuilder"] = () =>
            {
                d.machine = new Machine()
                {
                    Code = "M00001",
                    Name = "Machine 00001",
                    Description = "Machine"
                };
                d.machine = d._machineService.CreateObject(d.machine);
                d.coreBuilder = new CoreBuilder()
                {
                    BaseSku = "CB00001",
                    SkuNewCore = "CB00001N",
                    SkuUsedCore = "CB00001U",
                    Name = "CoreBuilder00001",
                    Category = "X"
                };
                d.coreBuilder = d._coreBuilderService.CreateObject(d.coreBuilder, d._itemService, d._itemTypeService, d._warehouseItemService, d._warehouseService);
                d.coreIdentification = new CoreIdentification()
                {
                    Code = "CI0001",
                    Quantity = 1,
                    IdentifiedDate = DateTime.Now
                };
                d.coreIdentification = d._coreIdentificationService.CreateObject(d.coreIdentification, d._customerService);
                d.coreIdentificationDetail = new CoreIdentificationDetail()
                {
                    CoreIdentificationId = d.coreIdentification.Id,
                    DetailId = 1,
                    MaterialCase = 2,
                    CoreBuilderId = d.coreBuilder.Id,
                    RollerTypeId = d._rollerTypeService.GetObjectByName("Found DT").Id,
                    MachineId = d.machine.Id,
                    RD = 12,
                    CD = 12,
                    RL = 12,
                    WL = 12,
                    TL = 12
                };
                d.coreIdentificationDetail = d._coreIdentificationDetailService.CreateObject(d.coreIdentificationDetail, d._coreIdentificationService, d._coreBuilderService, d._rollerTypeService, d._machineService);
                Item compound = new Item()
                {
                    ItemTypeId = d._itemTypeService.GetObjectByName("Compound").Id,
                    Name = "Compound",
                    Category = "Compound",
                    Quantity = 2,
                    Sku = "CMP0001",
                    UoM = "Pcs"
                };
                compound = d._itemService.CreateObject(compound, d._itemTypeService, d._warehouseItemService, d._warehouseService);
                d.rollerBuilder = new RollerBuilder()
                {
                    CoreBuilderId = d.coreBuilder.Id,
                    RollerTypeId = d._rollerTypeService.GetObjectByName("Found DT").Id,
                    MachineId = d.machine.Id,
                    RD = 13,
                    CD = 13,
                    RL = 13,
                    WL = 13,
                    TL = 13,
                    BaseSku = "RB0001",
                    SkuRollerUsedCore = "RB0001U",
                    SkuRollerNewCore = "RB0001N",
                    Name = "Roller Builder",
                    Category = "RB",
                    CompoundId = compound.Id
                };
                d.rollerBuilder = d._rollerBuilderService.CreateObject(d.rollerBuilder, d._machineService, d._itemService, d._itemTypeService, d._coreBuilderService, d._rollerTypeService, d._warehouseItemService, d._warehouseService);

                compound = d._itemService.SoftDeleteObject(compound, d._recoveryAccessoryDetailService, d._itemTypeService, d._warehouseItemService, d._barringService);
                compound.Errors.Count().should_not_be(0);
            };
        }
    }
}