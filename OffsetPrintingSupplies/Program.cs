﻿using Core.DomainModel;
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
                db.Configuration.LazyLoadingEnabled = true;
                db.Configuration.ProxyCreationEnabled = true;
                db.DeleteAllTables();
                //DataAwalZentrum();

                db.DeleteAllTables();
                DataBuilder d = new DataBuilder();

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
                    UoMId = d.Pcs.Id
                };
                d.item = d._itemService.CreateObject(d.item, d._uomService, d._itemTypeService, d._warehouseItemService, d._warehouseService, d._priceMutationService);

                d.localWarehouse = new Warehouse()
                {
                    Name = "Sentral Solusi Data",
                    Description = "Kali Besar Jakarta",
                    Code = "LCL"
                };
                d.localWarehouse = d._warehouseService.CreateObject(d.localWarehouse, d._warehouseItemService, d._itemService);

                d.contact = new Contact()
                {
                    Name = "President",
                    IsTaxable = true,
                    TaxCode = "01",
                };
                d.contact = d._contactService.CreateObject(d.contact);

                d.customerStockAdjustment = new CustomerStockAdjustment()
                {
                    ContactId = d.contact.Id,
                    WarehouseId = d.localWarehouse.Id,
                    AdjustmentDate = DateTime.Today,
                    Description = "Customer Stock Adjust Positive"
                };
                d._customerStockAdjustmentService.CreateObject(d.customerStockAdjustment, d._warehouseService, d._contactService);

                d.cstockAD1 = new CustomerStockAdjustmentDetail()
                {
                    ItemId = d.item.Id,
                    Quantity = 10,
                    CustomerStockAdjustmentId = d.customerStockAdjustment.Id,
                    Price = 50000
                };
                d._customerStockAdjustmentDetailService.CreateObject(d.cstockAD1, d._customerStockAdjustmentService, d._itemService, d._warehouseItemService, d._customerItemService);

                d._customerStockAdjustmentService.ConfirmObject(d.customerStockAdjustment, DateTime.Today, d._customerStockAdjustmentDetailService, d._customerStockMutationService,
                                                        d._itemService, d._customerItemService, d._warehouseItemService, d._accountService, d._generalLedgerJournalService, d._closingService);
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
                                                                   d._warehouseService, d._priceMutationService, d._machineService);
                d.coreIdentification = new CoreIdentification()
                {
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
                    TL = 12,
                    RepairRequestCase = Core.Constants.Constant.RepairRequestCase.BearingSeat
                };
                d.coreIdentificationDetail = d._coreIdentificationDetailService.CreateObject(d.coreIdentificationDetail,
                                             d._coreIdentificationService, d._coreBuilderService, d._rollerTypeService, d._machineService, d._warehouseItemService);
                Item compound = new Item()
                {
                    ItemTypeId = d._itemTypeService.GetObjectByName("Compound").Id,
                    Name = "Compound",
                    Sku = "CMP0001",
                    UoMId = d.Pcs.Id
                };

                compound = d._itemService.CreateObject(compound, d._uomService, d._itemTypeService, d._warehouseItemService, d._warehouseService, d._priceMutationService);

                d.stockAdjustment = new StockAdjustment()
                {
                    WarehouseId = d.localWarehouse.Id,
                    AdjustmentDate = DateTime.Today,
                    Description = "Compound"
                };
                d._stockAdjustmentService.CreateObject(d.stockAdjustment, d._warehouseService);

                d.stockAD1 = new StockAdjustmentDetail()
                {
                    StockAdjustmentId = d.stockAdjustment.Id,
                    Quantity = 2,
                    ItemId = compound.Id,
                    Price = 5000
                };
                d._stockAdjustmentDetailService.CreateObject(d.stockAD1, d._stockAdjustmentService, d._itemService, d._warehouseItemService);

                d._stockAdjustmentService.ConfirmObject(d.stockAdjustment, DateTime.Today, d._stockAdjustmentDetailService, d._stockMutationService,
                                                        d._itemService, d._blanketService, d._warehouseItemService, d._accountService, d._generalLedgerJournalService, d._closingService);

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
                    Description = "RB",
                    CompoundId = compound.Id,
                    UoMId = d.Pcs.Id
                };
                d.rollerBuilder = d._rollerBuilderService.CreateObject(d.rollerBuilder, d._machineService, d._uomService, d._itemService, d._itemTypeService, d._coreBuilderService, d._rollerTypeService, d._warehouseItemService, d._warehouseService, d._priceMutationService);

                compound = d._itemService.SoftDeleteObject(compound, d._stockMutationService, d._itemTypeService, d._warehouseItemService, d._blanketService,
                                                           d._purchaseOrderDetailService, d._stockAdjustmentDetailService, d._salesOrderDetailService, d._priceMutationService);

                Console.WriteLine("Press any key to stop...");
                Console.ReadKey();

            }
        }

        public static void DataAwalZentrum()
        {
            ZengraBuilder z = new ZengraBuilder();
            // Warning: it will take around 1 hour to run the whole process
            z.PopulateMasterData();
            z.PopulateContact();
            z.PopulateSupplier();
            z.PopulateCOA();
            z.PopulateFinance();
            z.PopulateItemSerpong();
            z.PopulateItemSurabaya();
            z.PopulateBarsForBlanket();
            z.PopulateBlanket();
            z.PopulateCore();
            z.PopulateRoller();
            z.PopulateWarehouse();
            z.AdjustCore();
            z.StockAdjustBlanket();
            z.AdjustItemSurabaya();
            z.AdjustItemSemarang();
            z.AdjustItemSerpong();
            z.AdjustRoller();
        }
    }
}
