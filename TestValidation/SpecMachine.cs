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

    public class SpecMachine: nspec
    {

        DataBuilder d;

        void before_each()
        {
            var db = new OffsetPrintingSuppliesEntities();
            using (db)
            {
                db.DeleteAllTables();
                d = new DataBuilder();

                d.baseGroup = d._contactGroupService.CreateObject(Core.Constants.Constant.GroupType.Base, "Base Group", true);

                d.Pcs = new UoM()
                {
                    Name = "Pcs"
                };
                d._uomService.CreateObject(d.Pcs);

                d.Boxes = new UoM()
                {
                    Name = "Boxes"
                };
                d._uomService.CreateObject(d.Boxes);

                d.Tubs = new UoM()
                {
                    Name = "Tubs"
                };
                d._uomService.CreateObject(d.Tubs);

                d.localWarehouse = new Warehouse()
                {
                    Name = "Sentral Solusi Data",
                    Description = "Kali Besar Jakarta",
                    Code = "LCL"
                };
                d.localWarehouse = d._warehouseService.CreateObject(d.localWarehouse, d._warehouseItemService, d._itemService);

                d.item = new Item()
                {
                    ItemTypeId = d._itemTypeService.GetObjectByName("Accessory").Id,
                    Sku = "ABC1001",
                    Name = "ABC",
                    Description = "ABC123",
                    UoMId = d.Pcs.Id
                };
                d.item = d._itemService.CreateObject(d.item, d._uomService, d._itemTypeService, d._warehouseItemService, d._warehouseService, d._priceMutationService, d._contactGroupService);

                d.machine = new Machine()
                {
                    Code = "M00001",
                    Name = "Machine 00001",
                    Description = "Machine"
                };
                d.machine = d._machineService.CreateObject(d.machine);
            }
        }

        void machine_validation()
        {
        
            it["validates_machine"] = () =>
            {
                d.machine.Errors.Count().should_be(0);
            };

            it["machine_with_no_name"] = () =>
            {
                Machine machine2 = new Machine()
                {
                    Code = "M00002",
                    Name = " ",
                    Description = "Machine"
                };
                machine2 = d._machineService.CreateObject(machine2);
                machine2.Errors.Count().should_not_be(0);
            };

            it["machine_with_same_code"] = () =>
            {
                Machine machine2 = new Machine()
                {
                    Code = "M00001",
                    Name = "Machine 00002",
                    Description = "Machine"
                };
                machine2 = d._machineService.CreateObject(machine2);
                machine2.Errors.Count().should_not_be(0);
            };

            it["delete_machine"] = () =>
            {
                d.machine = d._machineService.SoftDeleteObject(d.machine, d._rollerBuilderService, d._coreIdentificationDetailService, d._blanketService);
                d.machine.Errors.Count().should_be(0);
            };

            it["delete_machine_in_coreidentification"] = () =>
            {
                d.coreBuilder = new CoreBuilder()
                {
                    BaseSku = "CB00001",
                    SkuNewCore = "CB00001N",
                    SkuUsedCore = "CB00001U",
                    Name = "CoreBuilder00001",
                    Description = "X",
                    UoMId = d.Pcs.Id,
                    MachineId = d.machine.Id
                };
                d.coreBuilder = d._coreBuilderService.CreateObject(d.coreBuilder, d._uomService, d._itemService, d._itemTypeService, d._warehouseItemService,
                                                                   d._warehouseService, d._priceMutationService, d._contactGroupService, d._machineService);
                d.coreIdentification = new CoreIdentification()
                {
                    ContactId = null,
                    Code = "CI0001",
                    Quantity = 1,
                    IsInHouse = true,
                    IdentifiedDate = DateTime.Now,
                    WarehouseId = d.localWarehouse.Id
                };
                d.coreIdentification = d._coreIdentificationService.CreateObject(d.coreIdentification, d._contactService);

                d.stockAdjustment = new StockAdjustment()
                {
                    AdjustmentDate = DateTime.Today,
                    WarehouseId = d.localWarehouse.Id,
                    Description = "Add Core Builder"
                };
                d.stockAdjustment = d._stockAdjustmentService.CreateObject(d.stockAdjustment, d._warehouseService);

                d.stockAD1 = new StockAdjustmentDetail()
                {
                    ItemId = d._coreBuilderService.GetNewCore(d.coreBuilder.Id).Id,
                    Quantity = 5,
                    StockAdjustmentId = d.stockAdjustment.Id
                };
                d.stockAD1 = d._stockAdjustmentDetailService.CreateObject(d.stockAD1, d._stockAdjustmentService, d._itemService, d._warehouseItemService);

                d._stockAdjustmentService.ConfirmObject(d.stockAdjustment, DateTime.Today, d._stockAdjustmentDetailService, d._stockMutationService,
                                                        d._itemService, d._blanketService, d._warehouseItemService);

                d.coreIdentificationDetail = new CoreIdentificationDetail()
                {
                    CoreIdentificationId = d.coreIdentification.Id,
                    DetailId = 1,
                    MaterialCase = Core.Constants.Constant.MaterialCase.New,
                    CoreBuilderId = d.coreBuilder.Id,
                    RollerTypeId = d._rollerTypeService.GetObjectByName("Found DT").Id,
                    MachineId = d.machine.Id,
                    RD = 12,
                    CD = 12,
                    RL = 12,
                    WL = 12,
                    TL = 12
                };
                d.coreIdentificationDetail = d._coreIdentificationDetailService.CreateObject(d.coreIdentificationDetail,
                           d._coreIdentificationService, d._coreBuilderService, d._rollerTypeService, d._machineService, d._warehouseItemService);
                d.machine = d._machineService.SoftDeleteObject(d.machine, d._rollerBuilderService, d._coreIdentificationDetailService, d._blanketService);
                d.machine.Errors.Count().should_not_be(0);
            };
        }
    }
}