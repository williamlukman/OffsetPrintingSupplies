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
                db.Configuration.LazyLoadingEnabled = true;
                db.Configuration.ProxyCreationEnabled = true;

                db.DeleteAllTables();

                //PurchaseBuilder p = new PurchaseBuilder();
                //p.PopulateData();

                DataBuilder d = new DataBuilder();
                DataFunction(d);

                Console.WriteLine("Press any key to stop...");
                Console.ReadKey();

            }
        }

        public static void DataFunction(DataBuilder d)
        {
            // d.PopulateData();
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
                Name = "Abbey",
                Address = "1 Abbey St",
                ContactNo = "001234567",
                PIC = "Daddy",
                PICContactNo = "001234888",
                Email = "abbey@abbeyst.com",
                TaxCode = "01"
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
                Price = 50000,
            };
            d._customerStockAdjustmentDetailService.CreateObject(d.cstockAD1, d._customerStockAdjustmentService, d._itemService, d._warehouseItemService, d._customerItemService);

            d._customerStockAdjustmentService.ConfirmObject(d.customerStockAdjustment, DateTime.Today, d._customerStockAdjustmentDetailService, d._customerStockMutationService,
                                                    d._itemService, d._customerItemService, d._warehouseItemService, d._accountService, d._generalLedgerJournalService, d._closingService);

            //d.item.Errors.Count().should_be(0);
            //d.item.CustomerQuantity.should_be(d.cstockAD1.Quantity);
        }
    }
}
