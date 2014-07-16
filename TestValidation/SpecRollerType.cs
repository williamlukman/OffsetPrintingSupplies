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

    public class SpecRollerType: nspec
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
                    UoM = "Pcs"
                };
                d.item = d._itemService.CreateObject(d.item, d._itemTypeService, d._warehouseItemService, d._warehouseService);

                d.localWarehouse = new Warehouse()
                {
                    Name = "Sentral Solusi Data",
                    Description = "Kali Besar Jakarta",
                    IsMovingWarehouse = false,
                    Code = "LCL"
                };
                d.localWarehouse = d._warehouseService.CreateObject(d.localWarehouse, d._warehouseItemService, d._itemService);

            }
        }

        void rollertype_validation()
        {
        
            it["validates_rollertypes"] = () =>
            {
                d.typeDamp.Errors.Count().should_be(0);
                d.typeFoundDT.Errors.Count().should_be(0);
                d.typeInkFormX.Errors.Count().should_be(0);
                d.typeInkDistD.Errors.Count().should_be(0);
                d.typeInkDistM.Errors.Count().should_be(0);
                d.typeInkDistE.Errors.Count().should_be(0);
                d.typeInkDuctB.Errors.Count().should_be(0);
                d.typeInkDistH.Errors.Count().should_be(0);
                d.typeInkFormW.Errors.Count().should_be(0);
                d.typeInkDistHQ.Errors.Count().should_be(0);
                d.typeDampFormDQ.Errors.Count().should_be(0);
                d.typeInkFormY.Errors.Count().should_be(0);
            };

            it["rollertype_with_existingname"] = () =>
            {
                RollerType typecopyDamp = d._rollerTypeService.CreateObject("Damp", "Damp");
                typecopyDamp.Errors.Count().should_not_be(0);
            };

            it["delete_rollertype"] = () =>
            {
                d.typeDamp = d._rollerTypeService.SoftDeleteObject(d.typeDamp, d._rollerBuilderService, d._coreIdentificationDetailService);
                d.typeDamp.Errors.Count().should_be(0);
            };

            context["when_deleting_rollertype"] = () =>
            {
                before = () =>
                {
                    d.itemCompound = new Item()
                    {
                        ItemTypeId = d._itemTypeService.GetObjectByName("Compound").Id,
                        Sku = "Cmp10001",
                        Name = "Cmp 10001",
                        Category = "cmp",
                        UoM = "Pcs"
                    };
                    d.itemCompound = d._itemService.CreateObject(d.itemCompound, d._itemTypeService, d._warehouseItemService, d._warehouseService);
                    d._itemService.AdjustQuantity(d.itemCompound, 2);
                    d._warehouseItemService.AdjustQuantity(d._warehouseItemService.GetObjectByWarehouseAndItem(d.localWarehouse.Id, d.itemCompound.Id), 2);

                    d.customer = d._customerService.CreateObject("Abbey", "1 Abbey St", "001234567", "Daddy", "001234888", "abbey@abbeyst.com");

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
                        CustomerId = d.customer.Id,
                        Code = "CI0001",
                        Quantity = 1,
                        IdentifiedDate = DateTime.Now,
                        WarehouseId = d.localWarehouse.Id
                    };
                    d.coreIdentification = d._coreIdentificationService.CreateObject(d.coreIdentification, d._customerService);
                };

                it["delete_rollertype_with_coreidentificationdetail"] = () =>
                {
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
                    d.coreIdentificationDetail = d._coreIdentificationDetailService.CreateObject(d.coreIdentificationDetail, d._coreIdentificationService,
                                                                                                 d._coreBuilderService, d._rollerTypeService, d._machineService);
                    d.typeFoundDT = d._rollerTypeService.SoftDeleteObject(d.typeFoundDT, d._rollerBuilderService, d._coreIdentificationDetailService);
                    d.typeFoundDT.Errors.Count().should_not_be(0);
                };

                it["delete_rollertype_with_rollerbuilder"] = () =>
                {
                    d.rollerBuilder = new RollerBuilder()
                    {
                        BaseSku = "RB0001",
                        SkuRollerNewCore = "RB0001N",
                        SkuRollerUsedCore = "RB0001U",
                        Name = "Roller 0001",
                        Category = "0001",
                        RD = 12,
                        CD = 13,
                        RL = 14,
                        TL = 15,
                        WL = 16,
                        CompoundId = d.itemCompound.Id,
                        CoreBuilderId = d.coreBuilder.Id,
                        MachineId = d.machine.Id,
                        RollerTypeId = d._rollerTypeService.GetObjectByName("Damp").Id
                    };
                    d.rollerBuilder = d._rollerBuilderService.CreateObject(d.rollerBuilder, d._machineService, d._itemService, d._itemTypeService,
                                                                           d._coreBuilderService, d._rollerTypeService, d._warehouseItemService, d._warehouseService);
                    d.typeDamp = d._rollerTypeService.SoftDeleteObject(d.typeDamp, d._rollerBuilderService, d._coreIdentificationDetailService);
                    d.typeDamp.Errors.Count().should_not_be(0);
                };
            };
        }
    }
}