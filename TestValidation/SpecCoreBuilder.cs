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

    public class SpecCoreBuilder: nspec
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

                d.item = new Item()
                {
                    ItemTypeId = d._itemTypeService.GetObjectByName("Accessory").Id,
                    Sku = "ABC1001",
                    Name = "ABC",
                    Category = "ABC123",
                    UoMId = d.Pcs.Id
                };
                d.item = d._itemService.CreateObject(d.item, d._uomService, d._itemTypeService, d._warehouseItemService, d._warehouseService, d._priceMutationService, d._contactGroupService);

                d.localWarehouse = new Warehouse()
                {
                    Name = "Sentral Solusi Data",
                    Description = "Kali Besar Jakarta",
                    Code = "LCL"
                };
                d.localWarehouse = d._warehouseService.CreateObject(d.localWarehouse, d._warehouseItemService, d._itemService);

                d.contact = d._contactService.CreateObject("Abbey", "1 Abbey St", "001234567", "Daddy", "001234888", "abbey@abbeyst.com", d._contactGroupService);

                d.coreBuilder = new CoreBuilder()
                {
                    BaseSku = "CORE1001",
                    SkuNewCore = "CORE1001N",
                    SkuUsedCore = "CORE1001U",
                    Name = "Core X 1001",
                    Category = "X 1001",
                    UoMId = d.Pcs.Id
                };
                d.coreBuilder = d._coreBuilderService.CreateObject(d.coreBuilder, d._uomService, d._itemService, d._itemTypeService, d._warehouseItemService, d._warehouseService, d._priceMutationService, d._contactGroupService);
            }
        }

        void corebuilder_validation()
        {
        
            it["validates_corebuilder"] = () =>
            {
                d.coreBuilder.Errors.Count().should_be(0);
            };

            it["corebuilder_with_samesku"] = () =>
            {
                CoreBuilder coreBuilderCopySku = new CoreBuilder()
                {
                    BaseSku = "CORE1001",
                    SkuNewCore = "123N",
                    SkuUsedCore = "123U",
                    Name = "Core X 1001",
                    Category = "X 1001",
                    UoMId = d.Pcs.Id
                };
                coreBuilderCopySku = d._coreBuilderService.CreateObject(coreBuilderCopySku, d._uomService, d._itemService, d._itemTypeService, d._warehouseItemService, d._warehouseService, d._priceMutationService, d._contactGroupService);
                coreBuilderCopySku.Errors.Count().should_not_be(0);
            };

            it["corebuilder_with_sameskuitemnew"] = () =>
            {
                CoreBuilder coreBuilderCopySkuNew = new CoreBuilder()
                {
                    BaseSku = "CORE1002",
                    SkuNewCore = "CORE1001N",
                    SkuUsedCore = "123U",
                    Name = "Core X 1001",
                    Category = "X 1001",
                    UoMId = d.Pcs.Id
                };
                coreBuilderCopySkuNew = d._coreBuilderService.CreateObject(coreBuilderCopySkuNew, d._uomService, d._itemService, d._itemTypeService, d._warehouseItemService , d._warehouseService, d._priceMutationService, d._contactGroupService);
                coreBuilderCopySkuNew.Errors.Count().should_not_be(0);
            };


            it["corebuilder_with_sameskuitemused"] = () =>
            {
                CoreBuilder coreBuilderCopySkuUsed = new CoreBuilder()
                {
                    BaseSku = "CORE1002",
                    SkuNewCore = "123N",
                    SkuUsedCore = "CORE1001U",
                    Name = "Core X 1001",
                    Category = "X 1001",
                    UoMId = d.Pcs.Id
                };
                coreBuilderCopySkuUsed = d._coreBuilderService.CreateObject(coreBuilderCopySkuUsed, d._uomService, d._itemService, d._itemTypeService, d._warehouseItemService, d._warehouseService, d._priceMutationService, d._contactGroupService);
                coreBuilderCopySkuUsed.Errors.Count().should_not_be(0);
            };

            it["delete_corebuilder"] = () =>
            {
                d.coreBuilder = d._coreBuilderService.SoftDeleteObject(d.coreBuilder, d._itemService, d._rollerBuilderService, d._coreIdentificationDetailService,
                                                                       d._recoveryOrderDetailService, d._recoveryAccessoryDetailService, d._warehouseItemService,
                                                                       d._stockMutationService, d._itemTypeService, d._barringService, d._purchaseOrderDetailService, 
                                                                       d._stockAdjustmentDetailService, d._salesOrderDetailService, d._priceMutationService, d._barringOrderDetailService);
                d.coreBuilder.Errors.Count().should_be(0);
            };

            it["delete_corebuilder_with_coreidentificationdetail"] = () =>
            {
                d.machine = new Machine()
                {
                    Code = "M00001",
                    Name = "Machine 00001",
                    Description = "Machine"
                };
                d.machine = d._machineService.CreateObject(d.machine);
                d.coreIdentification = new CoreIdentification()
                {
                    ContactId = d.contact.Id,
                    Code = "CI0001",
                    Quantity = 1,
                    IdentifiedDate = DateTime.Now,
                    WarehouseId = d.localWarehouse.Id
                };
                d.coreIdentification = d._coreIdentificationService.CreateObject(d.coreIdentification, d._contactService);
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
                d.coreIdentificationDetail = d._coreIdentificationDetailService.CreateObject(d.coreIdentificationDetail, d._coreIdentificationService, d._coreBuilderService,
                                             d._rollerTypeService, d._machineService, d._warehouseItemService);

                d.coreBuilder = d._coreBuilderService.SoftDeleteObject(d.coreBuilder, d._itemService, d._rollerBuilderService, d._coreIdentificationDetailService, d._recoveryOrderDetailService, d._recoveryAccessoryDetailService,
                                                                       d._warehouseItemService, d._stockMutationService, d._itemTypeService, d._barringService, d._purchaseOrderDetailService,
                                                                       d._stockAdjustmentDetailService, d._salesOrderDetailService, d._priceMutationService, d._barringOrderDetailService);
                d.coreBuilder.Errors.Count().should_not_be(0);
            };
        }
    }
}