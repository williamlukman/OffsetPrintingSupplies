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

    public class SpecRollerBuilder: nspec
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
                d.item = d._itemService.CreateObject(d.item, d._itemTypeService);
                d.itemCompound = new Item()
                {
                    ItemTypeId = d._itemTypeService.GetObjectByName("Compound").Id,
                    Sku = "Cmp10001",
                    Name = "Cmp 10001",
                    Category = "cmp",
                    UoM = "Pcs",
                    Quantity = 2
                };
                d.itemCompound = d._itemService.CreateObject(d.itemCompound, d._itemTypeService);

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
                d.coreBuilder = d._coreBuilderService.CreateObject(d.coreBuilder, d._itemService, d._itemTypeService);
                d.coreIdentification = new CoreIdentification()
                {
                    CustomerId = d.customer.Id,
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

                d.rollerBuilder = new RollerBuilder()
                {
                    BaseSku = "RB0001",
                    SkuNewRoller = "RB0001N",
                    SkuUsedRoller = "RB0001U",
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
                d.rollerBuilder = d._rollerBuilderService.CreateObject(d.rollerBuilder, d._machineService, d._itemService, d._itemTypeService, d._coreBuilderService, d._rollerTypeService);
            }
        }

        /*
         * STEPS:
         * 1. Create valid d.item
         * 2. Create invalid d.item with no name
         * 3. Create invalid items with same SKU
         * 4a. Delete d.item
         * 4b. Delete d.item with stock mutations
         */
        void rollerbuilder_validation()
        {
        
            it["validates_rollerbuilder"] = () =>
            {
                d.item.Errors.Count().should_be(0);
                d.itemCompound.Errors.Count().should_be(0);
                d.machine.Errors.Count().should_be(0);
                d.coreBuilder.Errors.Count().should_be(0);
                d.coreIdentification.Errors.Count().should_be(0);
                d.coreIdentificationDetail.Errors.Count().should_be(0);
                d.rollerBuilder.Errors.Count().should_be(0);
            };

            it["delete_rollerbuilder"] = () =>
            {
                d.rollerBuilder = d._rollerBuilderService.SoftDeleteObject(d.rollerBuilder, d._itemService, d._recoveryOrderDetailService, d._recoveryAccessoryDetailService, d._coreBuilderService);
                d.rollerBuilder.Errors.Count().should_be(0);
            };

            it["delete_rollerbuilder_with_recoveryorderdetail"] = () =>
            {
                d._itemService.AdjustQuantity(d._coreBuilderService.GetNewCore(d.coreBuilder.Id), 1);
                d._itemService.AdjustQuantity(d._coreBuilderService.GetUsedCore(d.coreBuilder.Id), 1);

                d.coreIdentification = d._coreIdentificationService.ConfirmObject(d.coreIdentification, d._coreIdentificationDetailService, d._recoveryOrderService, d._recoveryOrderDetailService, d._coreBuilderService);
                d.coreIdentification.Errors.Count().should_be(0);

                RecoveryOrder recoveryOrder = new RecoveryOrder()
                {
                    Code = "RO0001",
                    CoreIdentificationId = d.coreIdentification.Id,
                    QuantityReceived = 1,
                };
                recoveryOrder = d._recoveryOrderService.CreateObject(recoveryOrder, d._coreIdentificationService);
                recoveryOrder.Errors.Count().should_be(0);

                RecoveryOrderDetail recoveryOrderDetail = new RecoveryOrderDetail()
                {
                    RecoveryOrderId = recoveryOrder.Id,
                    CoreIdentificationDetailId = d.coreIdentificationDetail.Id,
                    RollerBuilderId = d.rollerBuilder.Id,
                    CoreTypeCase = Core.Constants.Constant.CoreTypeCase.R,
                    RepairRequestCase = Core.Constants.Constant.RepairRequestCase.BearingSeat,
                    Acc = "Y",
                };
                recoveryOrderDetail = d._recoveryOrderDetailService.CreateObject(recoveryOrderDetail, d._recoveryOrderService, d._coreIdentificationDetailService, d._rollerBuilderService);
                recoveryOrderDetail.Errors.Count().should_be(0);

                d.rollerBuilder = d._rollerBuilderService.SoftDeleteObject(d.rollerBuilder, d._itemService, d._recoveryOrderDetailService, d._recoveryAccessoryDetailService, d._coreBuilderService);
                d.rollerBuilder.Errors.Count().should_not_be(0);
            };
        }
    }
}