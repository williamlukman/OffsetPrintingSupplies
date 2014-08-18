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
                //db.DeleteAllTables();

                DataBuilder d = new DataBuilder();
                //PurchaseBuilder p = new PurchaseBuilder();
               // SalesBuilder s = new SalesBuilder();

                 // DataFunction(d);
                //SalesFunction(s);
                //PurchaseFunction(p);
            }
        }

        public static void PurchaseFunction(PurchaseBuilder p)
        {
            p.PopulateData();
        }

        public static void SalesFunction(SalesBuilder s)
        {
            s.PopulateData();
        }

        public static void DataFunction(DataBuilder d)
        {
            /*
            d.PopulateData();
                
            if (d.itemCompound.Errors.Count() > 0) { Console.WriteLine("Error"); };
            if (d.itemCompound1.Errors.Count() > 0) { Console.WriteLine("Error"); };
            if (d.itemCompound2.Errors.Count() > 0) { Console.WriteLine("Error"); };
            if (d.itemAccessory1.Errors.Count() > 0) { Console.WriteLine("Error"); };
            if (d.itemAccessory2.Errors.Count() > 0) { Console.WriteLine("Error"); };
            if (d.contact.Errors.Count() > 0) { Console.WriteLine("Error"); };
            if (d.machine.Errors.Count() > 0) { Console.WriteLine("Error"); };
            if (d.coreBuilder.Errors.Count() > 0) { Console.WriteLine("Error"); };
            if (d.coreBuilder1.Errors.Count() > 0) { Console.WriteLine("Error"); };
            if (d.coreBuilder2.Errors.Count() > 0) { Console.WriteLine("Error"); };
            if (d.coreBuilder3.Errors.Count() > 0) { Console.WriteLine("Error"); };
            if (d.coreBuilder4.Errors.Count() > 0) { Console.WriteLine("Error"); };
            if (d.coreIdentification.Errors.Count() > 0) { Console.WriteLine("Error"); };
            if (d.coreIdentificationContact.Errors.Count() > 0) { Console.WriteLine("Error"); };
            if (d.coreIDContact1.Errors.Count() > 0) { Console.WriteLine("Error"); };
            if (d.coreIDContact2.Errors.Count() > 0) { Console.WriteLine("Error"); };
            if (d.coreIDContact3.Errors.Count() > 0) { Console.WriteLine("Error"); };
            if (d.coreIdentificationDetail.Errors.Count() > 0) { Console.WriteLine("Error"); };
            if (d.coreIdentificationInHouse.Errors.Count() > 0) { Console.WriteLine("Error"); };
            if (d.coreIDInHouse1.Errors.Count() > 0) { Console.WriteLine("Error"); };
            if (d.coreIDInHouse2.Errors.Count() > 0) { Console.WriteLine("Error"); };
            if (d.coreIDInHouse3.Errors.Count() > 0) { Console.WriteLine("Error"); };
            if (d.recoveryOrderContact.Errors.Count() > 0) { Console.WriteLine("Error"); };
            if (d.recoveryODContact1.Errors.Count() > 0) { Console.WriteLine("Error"); };
            if (d.recoveryODContact2.Errors.Count() > 0) { Console.WriteLine("Error"); };
            if (d.recoveryODContact3.Errors.Count() > 0) { Console.WriteLine("Error"); };
            if (d.recoveryOrderInHouse.Errors.Count() > 0) { Console.WriteLine("Error"); };
            if (d.recoveryODInHouse1.Errors.Count() > 0) { Console.WriteLine("Error"); };
            if (d.recoveryODInHouse2.Errors.Count() > 0) { Console.WriteLine("Error"); };
            if (d.recoveryODInHouse3.Errors.Count() > 0) { Console.WriteLine("Error"); };
            */
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
                d.coreIdentificationDetail = d._coreIdentificationDetailService.CreateObject(d.coreIdentificationDetail, d._coreIdentificationService, d._coreBuilderService, d._rollerTypeService, d._machineService);

                d.coreBuilder = d._coreBuilderService.SoftDeleteObject(d.coreBuilder, d._itemService, d._rollerBuilderService, d._coreIdentificationDetailService, d._recoveryOrderDetailService, d._recoveryAccessoryDetailService,
                                                                       d._warehouseItemService, d._stockMutationService, d._itemTypeService, d._purchaseOrderDetailService,
                                                                       d._stockAdjustmentDetailService, d._salesOrderDetailService);
            Console.WriteLine("Press any key to stop...");
            Console.ReadKey();
        }
    }
}
