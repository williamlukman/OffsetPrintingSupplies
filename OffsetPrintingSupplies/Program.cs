using Core.DomainModel;
using Core.Interface.Service;
using Data.Context;
using Data.Repository;
using Service.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestValidation;
using Validation.Validation;

namespace OffsetPrintingSupplies
{
    public class Program
    {

        public static void Main(string[] args)
        {
            var db = new OffsetPrintingSuppliesEntities();
            using (db)
            {
                db.DeleteAllTables();

                DataBuilder d = new DataBuilder();

                db.DeleteAllTables();
                d = new DataBuilder();

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

                d.itemCompound = new Item()
                {
                    ItemTypeId = d._itemTypeService.GetObjectByName("Compound").Id,
                    Sku = "Cmp10001",
                    Name = "Cmp 10001",
                    Category = "cmp",
                    UoMId = d.Pcs.Id
                };
                d.itemCompound = d._itemService.CreateObject(d.itemCompound, d._uomService, d._itemTypeService, d._warehouseItemService, d._warehouseService, d._priceMutationService, d._contactGroupService);

                d.stockAdjustment = new StockAdjustment()
                {
                    AdjustmentDate = DateTime.Today,
                    WarehouseId = d.localWarehouse.Id
                };
                d._stockAdjustmentService.CreateObject(d.stockAdjustment, d._warehouseService);
                d.stockAD1 = new StockAdjustmentDetail()
                {
                    StockAdjustmentId = d.stockAdjustment.Id,
                    Quantity = 2,
                    ItemId = d.itemCompound.Id
                };
                d._stockAdjustmentDetailService.CreateObject(d.stockAD1, d._stockAdjustmentService, d._itemService, d._warehouseItemService);

                d._stockAdjustmentService.ConfirmObject(d.stockAdjustment, DateTime.Today, d._stockAdjustmentDetailService, d._stockMutationService,
                                                        d._itemService, d._blanketService, d._warehouseItemService, d._accountService, d._generalLedgerJournalService);

                d.contact = d._contactService.CreateObject("Abbey", "1 Abbey St", "001234567", "Daddy", "001234888", "abbey@abbeyst.com", d._contactGroupService);

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
                    Description = "X",
                    UoMId = d.Pcs.Id,
                    MachineId = d.machine.Id,
                    CoreBuilderTypeCase = Core.Constants.Constant.CoreBuilderTypeCase.Hollow
                };
               d.coreBuilder = d._coreBuilderService.CreateObject(d.coreBuilder, d._uomService, d._itemService, d._itemTypeService, d._warehouseItemService,
                                                                   d._warehouseService, d._priceMutationService, d._contactGroupService, d._machineService);
                d.coreIdentification = new CoreIdentification()
                {
                    ContactId = d.contact.Id,
                    Code = "CI0001",
                    Quantity = 1,
                    IdentifiedDate = DateTime.Now,
                    WarehouseId = d.localWarehouse.Id
                };
                d.coreIdentification = d._coreIdentificationService.CreateObject(d.coreIdentification, d._contactService);

                d.rollerBuilder = new RollerBuilder()
                {
                    BaseSku = "RB0001",
                    SkuRollerNewCore = "RB0001N",
                    SkuRollerUsedCore = "RB0001U",
                    Name = "Roller 0001",
                    Description = "0001",
                    RD = 12,
                    CD = 13,
                    RL = 14,
                    TL = 15,
                    WL = 16,
                    CompoundId = d.itemCompound.Id,
                    CoreBuilderId = d.coreBuilder.Id,
                    MachineId = d.machine.Id,
                    RollerTypeId = d._rollerTypeService.GetObjectByName("Damp").Id,
                    UoMId = d.Pcs.Id,
                    AdhesiveId = d.itemAdhesive.Id
                };
                d.rollerBuilder = d._rollerBuilderService.CreateObject(d.rollerBuilder, d._machineService, d._uomService, d._itemService, d._itemTypeService,
                                                                       d._coreBuilderService, d._rollerTypeService, d._warehouseItemService, d._warehouseService,
                                                                       d._priceMutationService, d._contactGroupService);
                d.typeDamp = d._rollerTypeService.SoftDeleteObject(d.typeDamp, d._rollerBuilderService, d._coreIdentificationDetailService);

                //                DataFunction(d);

                Console.WriteLine("Press any key to stop...");
                Console.ReadKey();

            }
        }

        public static void DataFunction(DataBuilder d)
        {
           d.PopulateData();
        }
    }
}
