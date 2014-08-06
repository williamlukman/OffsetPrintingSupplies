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
                
                // d.PopulateData();
                /*
                if (d.itemCompound.Errors.Count() > 0) { Console.WriteLine("Error"); };
                if (d.itemCompound1.Errors.Count() > 0) { Console.WriteLine("Error"); };
                if (d.itemCompound2.Errors.Count() > 0) { Console.WriteLine("Error"); };
                if (d.itemAccessory1.Errors.Count() > 0) { Console.WriteLine("Error"); };
                if (d.itemAccessory2.Errors.Count() > 0) { Console.WriteLine("Error"); };
                if (d.customer.Errors.Count() > 0) { Console.WriteLine("Error"); };
                if (d.machine.Errors.Count() > 0) { Console.WriteLine("Error"); };
                if (d.coreBuilder.Errors.Count() > 0) { Console.WriteLine("Error"); };
                if (d.coreBuilder1.Errors.Count() > 0) { Console.WriteLine("Error"); };
                if (d.coreBuilder2.Errors.Count() > 0) { Console.WriteLine("Error"); };
                if (d.coreBuilder3.Errors.Count() > 0) { Console.WriteLine("Error"); };
                if (d.coreBuilder4.Errors.Count() > 0) { Console.WriteLine("Error"); };
                if (d.coreIdentification.Errors.Count() > 0) { Console.WriteLine("Error"); };
                if (d.coreIdentificationCustomer.Errors.Count() > 0) { Console.WriteLine("Error"); };
                if (d.coreIDCustomer1.Errors.Count() > 0) { Console.WriteLine("Error"); };
                if (d.coreIDCustomer2.Errors.Count() > 0) { Console.WriteLine("Error"); };
                if (d.coreIDCustomer3.Errors.Count() > 0) { Console.WriteLine("Error"); };
                if (d.coreIdentificationDetail.Errors.Count() > 0) { Console.WriteLine("Error"); };
                if (d.coreIdentificationInHouse.Errors.Count() > 0) { Console.WriteLine("Error"); };
                if (d.coreIDInHouse1.Errors.Count() > 0) { Console.WriteLine("Error"); };
                if (d.coreIDInHouse2.Errors.Count() > 0) { Console.WriteLine("Error"); };
                if (d.coreIDInHouse3.Errors.Count() > 0) { Console.WriteLine("Error"); };
                if (d.recoveryOrderCustomer.Errors.Count() > 0) { Console.WriteLine("Error"); };
                if (d.recoveryODCustomer1.Errors.Count() > 0) { Console.WriteLine("Error"); };
                if (d.recoveryODCustomer2.Errors.Count() > 0) { Console.WriteLine("Error"); };
                if (d.recoveryODCustomer3.Errors.Count() > 0) { Console.WriteLine("Error"); };
                if (d.recoveryOrderInHouse.Errors.Count() > 0) { Console.WriteLine("Error"); };
                if (d.recoveryODInHouse1.Errors.Count() > 0) { Console.WriteLine("Error"); };
                if (d.recoveryODInHouse2.Errors.Count() > 0) { Console.WriteLine("Error"); };
                if (d.recoveryODInHouse3.Errors.Count() > 0) { Console.WriteLine("Error"); };
                */
                Customer customer;
                Item item_batiktulis;
                Item item_busway;
                Item item_botolaqua;
                UoM Pcs;
                ItemType type;
                Warehouse warehouse;
                WarehouseItem warehouse_batiktulis;
                WarehouseItem warehouse_busway;
                WarehouseItem warehouse_botolaqua;
                StockAdjustment stockAdjustment;
                StockAdjustmentDetail stockAdjust_batiktulis;
                StockAdjustmentDetail stockAdjust_busway;
                StockAdjustmentDetail stockAdjust_botolaqua;
                SalesOrder salesOrder1;
                SalesOrder salesOrder2;
                SalesOrderDetail salesOrderDetail_batiktulis_so1;
                SalesOrderDetail salesOrderDetail_busway_so1;
                SalesOrderDetail salesOrderDetail_botolaqua_so1;
                SalesOrderDetail salesOrderDetail_batiktulis_so2;
                SalesOrderDetail salesOrderDetail_busway_so2;
                SalesOrderDetail salesOrderDetail_botolaqua_so2;
                DeliveryOrder deliveryOrder1;
                DeliveryOrder deliveryOrder2;
                DeliveryOrderDetail deliveryOrderDetail_batiktulis_do1;
                DeliveryOrderDetail deliveryOrderDetail_busway_do1;
                DeliveryOrderDetail deliveryOrderDetail_botolaqua_do1;
                DeliveryOrderDetail deliveryOrderDetail_batiktulis_do2a;
                DeliveryOrderDetail deliveryOrderDetail_batiktulis_do2b;
                DeliveryOrderDetail deliveryOrderDetail_busway_do2;
                DeliveryOrderDetail deliveryOrderDetail_botolaqua_do2;

                Pcs = new UoM()
                {
                    Name = "Pcs"
                };
                d._uomService.CreateObject(Pcs);

                customer = new Customer()
                {
                    Name = "President of Indonesia",
                    Address = "Istana Negara Jl. Veteran No. 16 Jakarta Pusat",
                    CustomerNo = "021 3863777",
                    PIC = "Mr. President",
                    PICCustomerNo = "021 3863777",
                    Email = "random@ri.gov.au"
                };
                customer = d._customerService.CreateObject(customer);

                type = d._itemTypeService.CreateObject("Item", "Item");

                warehouse = new Warehouse()
                {
                    Name = "Sentral Solusi Data",
                    Description = "Kali Besar Jakarta",
                    IsMovingWarehouse = false,
                    Code = "LCL"
                };
                warehouse = d._warehouseService.CreateObject(warehouse, d._warehouseItemService, d._itemService);

                item_batiktulis = new Item()
                {
                    ItemTypeId = d._itemTypeService.GetObjectByName("Item").Id,
                    Name = "Batik Tulis",
                    Category = "Item",
                    Sku = "bt123",
                    UoMId = Pcs.Id
                };

                item_batiktulis = d._itemService.CreateObject(item_batiktulis, d._uomService, d._itemTypeService, d._warehouseItemService, d._warehouseService);
                d._itemService.AdjustQuantity(item_batiktulis, 1000);
                d._warehouseItemService.AdjustQuantity(d._warehouseItemService.FindOrCreateObject(warehouse.Id, item_batiktulis.Id), 1000);

                item_busway = new Item()
                {
                    ItemTypeId = d._itemTypeService.GetObjectByName("Item").Id,
                    Name = "Busway",
                    Category = "Untuk disumbangkan bagi kebutuhan DKI Jakarta",
                    Sku = "DKI002",
                    UoMId = Pcs.Id
                };
                item_busway = d._itemService.CreateObject(item_busway, d._uomService, d._itemTypeService, d._warehouseItemService, d._warehouseService);
                d._itemService.AdjustQuantity(item_busway, 200);
                d._warehouseItemService.AdjustQuantity(d._warehouseItemService.FindOrCreateObject(warehouse.Id, item_busway.Id), 200);

                item_botolaqua = new Item()
                {
                    ItemTypeId = d._itemTypeService.GetObjectByName("Item").Id,
                    Name = "Botol Aqua",
                    Category = "Minuman",
                    Sku = "DKI003",
                    UoMId = Pcs.Id
                };
                item_botolaqua = d._itemService.CreateObject(item_botolaqua, d._uomService, d._itemTypeService, d._warehouseItemService, d._warehouseService);
                d._itemService.AdjustQuantity(item_botolaqua, 20000);
                d._warehouseItemService.AdjustQuantity(d._warehouseItemService.FindOrCreateObject(warehouse.Id, item_botolaqua.Id), 20000);

                salesOrder1 = d._salesOrderService.CreateObject(customer.Id, new DateTime(2014, 07, 09), d._customerService);
                salesOrder2 = d._salesOrderService.CreateObject(customer.Id, new DateTime(2014, 04, 09), d._customerService);
                salesOrderDetail_batiktulis_so1 = d._salesOrderDetailService.CreateObject(salesOrder1.Id, item_batiktulis.Id, 500, 2000000, d._salesOrderService, d._itemService);
                salesOrderDetail_busway_so1 = d._salesOrderDetailService.CreateObject(salesOrder1.Id, item_busway.Id, 91, 800000000, d._salesOrderService, d._itemService);
                salesOrderDetail_botolaqua_so1 = d._salesOrderDetailService.CreateObject(salesOrder1.Id, item_botolaqua.Id, 2000, 5000, d._salesOrderService, d._itemService);
                salesOrderDetail_batiktulis_so2 = d._salesOrderDetailService.CreateObject(salesOrder2.Id, item_batiktulis.Id, 40, 2000500, d._salesOrderService, d._itemService);
                salesOrderDetail_busway_so2 = d._salesOrderDetailService.CreateObject(salesOrder2.Id, item_busway.Id, 3, 810000000, d._salesOrderService, d._itemService);
                salesOrderDetail_botolaqua_so2 = d._salesOrderDetailService.CreateObject(salesOrder2.Id, item_botolaqua.Id, 340, 5500, d._salesOrderService, d._itemService);
                salesOrder1 = d._salesOrderService.ConfirmObject(salesOrder1, d._salesOrderDetailService, d._stockMutationService, d._itemService);
                salesOrder2 = d._salesOrderService.ConfirmObject(salesOrder2, d._salesOrderDetailService, d._stockMutationService, d._itemService);
                salesOrderDetail_batiktulis_so1 = d._salesOrderDetailService.FinishObject(salesOrderDetail_batiktulis_so1, d._stockMutationService, d._itemService, d._barringService, d._warehouseItemService);
                salesOrderDetail_busway_so1 = d._salesOrderDetailService.FinishObject(salesOrderDetail_busway_so1, d._stockMutationService, d._itemService, d._barringService, d._warehouseItemService);
                salesOrderDetail_botolaqua_so1 = d._salesOrderDetailService.FinishObject(salesOrderDetail_botolaqua_so1, d._stockMutationService, d._itemService, d._barringService, d._warehouseItemService);
                salesOrderDetail_batiktulis_so2 = d._salesOrderDetailService.FinishObject(salesOrderDetail_batiktulis_so2, d._stockMutationService, d._itemService, d._barringService, d._warehouseItemService);
                salesOrderDetail_busway_so2 = d._salesOrderDetailService.FinishObject(salesOrderDetail_busway_so2, d._stockMutationService, d._itemService, d._barringService, d._warehouseItemService);
                salesOrderDetail_botolaqua_so2 = d._salesOrderDetailService.FinishObject(salesOrderDetail_botolaqua_so2, d._stockMutationService, d._itemService, d._barringService, d._warehouseItemService);

                deliveryOrder1 = d._deliveryOrderService.CreateObject(warehouse.Id, customer.Id, new DateTime(2000, 1, 1), d._customerService);
                deliveryOrder2 = d._deliveryOrderService.CreateObject(warehouse.Id, customer.Id, new DateTime(2014, 5, 5), d._customerService);
                deliveryOrderDetail_batiktulis_do1 = d._deliveryOrderDetailService.CreateObject(deliveryOrder1.Id, item_batiktulis.Id, 400, salesOrderDetail_batiktulis_so1.Id, d._deliveryOrderService,
                                                                                              d._salesOrderDetailService, d._salesOrderService, d._itemService, d._customerService);
                deliveryOrderDetail_busway_do1 = d._deliveryOrderDetailService.CreateObject(deliveryOrder1.Id, item_busway.Id, 91, salesOrderDetail_busway_so1.Id, d._deliveryOrderService,
                                                                                            d._salesOrderDetailService, d._salesOrderService, d._itemService, d._customerService);
                deliveryOrderDetail_botolaqua_do1 = d._deliveryOrderDetailService.CreateObject(deliveryOrder1.Id, item_botolaqua.Id, 2000, salesOrderDetail_botolaqua_so1.Id, d._deliveryOrderService,
                                                                                              d._salesOrderDetailService, d._salesOrderService, d._itemService, d._customerService);
                deliveryOrderDetail_batiktulis_do2a = d._deliveryOrderDetailService.CreateObject(deliveryOrder2.Id, item_batiktulis.Id, 100, salesOrderDetail_batiktulis_so1.Id, d._deliveryOrderService,
                                                                                                                      d._salesOrderDetailService, d._salesOrderService, d._itemService, d._customerService);
                deliveryOrderDetail_batiktulis_do2b = d._deliveryOrderDetailService.CreateObject(deliveryOrder2.Id, item_batiktulis.Id, 40, salesOrderDetail_batiktulis_so2.Id, d._deliveryOrderService,
                                                                                                                      d._salesOrderDetailService, d._salesOrderService, d._itemService, d._customerService);
                deliveryOrderDetail_busway_do2 = d._deliveryOrderDetailService.CreateObject(deliveryOrder2.Id, item_busway.Id, 3, salesOrderDetail_busway_so2.Id, d._deliveryOrderService,
                                                                                                                      d._salesOrderDetailService, d._salesOrderService, d._itemService, d._customerService);
                deliveryOrderDetail_botolaqua_do2 = d._deliveryOrderDetailService.CreateObject(deliveryOrder2.Id, item_botolaqua.Id, 340, salesOrderDetail_botolaqua_so2.Id, d._deliveryOrderService,
                                                                                                                      d._salesOrderDetailService, d._salesOrderService, d._itemService, d._customerService);
                deliveryOrder1 = d._deliveryOrderService.ConfirmObject(deliveryOrder1, d._deliveryOrderDetailService, d._salesOrderDetailService, d._stockMutationService, d._itemService);
                deliveryOrder2 = d._deliveryOrderService.ConfirmObject(deliveryOrder2, d._deliveryOrderDetailService, d._salesOrderDetailService, d._stockMutationService, d._itemService);
                int monitor1 = item_batiktulis.Quantity;
                int monitor1pending = item_batiktulis.PendingDelivery;
                deliveryOrderDetail_batiktulis_do1 = d._deliveryOrderDetailService.FinishObject(deliveryOrderDetail_batiktulis_do1, d._deliveryOrderService, d._salesOrderDetailService, d._stockMutationService,
                                                                                              d._itemService, d._barringService, d._warehouseItemService);
                deliveryOrderDetail_busway_do1 = d._deliveryOrderDetailService.FinishObject(deliveryOrderDetail_busway_do1, d._deliveryOrderService, d._salesOrderDetailService, d._stockMutationService,
                                                                                          d._itemService, d._barringService, d._warehouseItemService);
                deliveryOrderDetail_botolaqua_do1 = d._deliveryOrderDetailService.FinishObject(deliveryOrderDetail_botolaqua_do1, d._deliveryOrderService, d._salesOrderDetailService, d._stockMutationService,
                                                                                             d._itemService, d._barringService, d._warehouseItemService);
                int monitor2 = item_batiktulis.Quantity;
                int monitor2pending = item_batiktulis.PendingDelivery;
                deliveryOrderDetail_batiktulis_do2a = d._deliveryOrderDetailService.FinishObject(deliveryOrderDetail_batiktulis_do2a, d._deliveryOrderService, d._salesOrderDetailService, d._stockMutationService,
                                                                                               d._itemService, d._barringService, d._warehouseItemService);
                int monitor3 = item_batiktulis.Quantity;
                int monitor3pending = item_batiktulis.PendingDelivery;
                deliveryOrderDetail_batiktulis_do2b = d._deliveryOrderDetailService.FinishObject(deliveryOrderDetail_batiktulis_do2b, d._deliveryOrderService, d._salesOrderDetailService, d._stockMutationService,
                                                                                              d._itemService, d._barringService, d._warehouseItemService);
                int monitor4 = item_batiktulis.Quantity;
                int monitor4pending = item_batiktulis.PendingDelivery;
                deliveryOrderDetail_busway_do2 = d._deliveryOrderDetailService.FinishObject(deliveryOrderDetail_busway_do2, d._deliveryOrderService, d._salesOrderDetailService, d._stockMutationService,
                                                                                              d._itemService, d._barringService, d._warehouseItemService);
                deliveryOrderDetail_botolaqua_do2 = d._deliveryOrderDetailService.FinishObject(deliveryOrderDetail_botolaqua_do2, d._deliveryOrderService, d._salesOrderDetailService, d._stockMutationService,
                                                                                              d._itemService, d._barringService, d._warehouseItemService);

                Console.WriteLine("Press any key to stop...");
                Console.ReadKey();
            }

        }
    }
}
