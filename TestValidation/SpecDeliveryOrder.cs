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

    public class SpecDeliveryOrder : nspec
    {
        Customer customer;
        Item item_batiktulis;
        Item item_busway;
        Item item_botolaqua;
        UoM Pcs;
        ItemType type;
        Warehouse warehouse;
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
        ICustomerService _customerService;
        IItemService _itemService;
        IStockMutationService _stockMutationService;
        IStockAdjustmentService _stockAdjustmentService;
        IStockAdjustmentDetailService _stockAdjustmentDetailService;
        ISalesOrderService _salesOrderService;
        ISalesOrderDetailService _salesOrderDetailService;
        IDeliveryOrderService _deliveryOrderService;
        IDeliveryOrderDetailService _deliveryOrderDetailService;
        IUoMService _uomService;
        IBarringService _barringService;
        IItemTypeService _itemTypeService;
        IWarehouseItemService _warehouseItemService;
        IWarehouseService _warehouseService;

        void before_each()
        {
            var db = new OffsetPrintingSuppliesEntities();
            using (db)
            {
                db.DeleteAllTables();
                _customerService = new CustomerService(new CustomerRepository(), new CustomerValidator());
                _itemService = new ItemService(new ItemRepository(), new ItemValidator());
                _stockMutationService = new StockMutationService(new StockMutationRepository(), new StockMutationValidator());
                _salesOrderService = new SalesOrderService(new SalesOrderRepository(), new SalesOrderValidator());
                _salesOrderDetailService = new SalesOrderDetailService(new SalesOrderDetailRepository(), new SalesOrderDetailValidator());
                _deliveryOrderService = new DeliveryOrderService(new DeliveryOrderRepository(), new DeliveryOrderValidator());
                _deliveryOrderDetailService = new DeliveryOrderDetailService(new DeliveryOrderDetailRepository(), new DeliveryOrderDetailValidator());
                _stockAdjustmentService = new StockAdjustmentService(new StockAdjustmentRepository(), new StockAdjustmentValidator());
                _stockAdjustmentDetailService = new StockAdjustmentDetailService(new StockAdjustmentDetailRepository(), new StockAdjustmentDetailValidator());
                _itemTypeService = new ItemTypeService(new ItemTypeRepository(), new ItemTypeValidator());
                _uomService = new UoMService(new UoMRepository(), new UoMValidator());
                _warehouseItemService = new WarehouseItemService(new WarehouseItemRepository(), new WarehouseItemValidator());
                _warehouseService = new WarehouseService(new WarehouseRepository(), new WarehouseValidator());
                _barringService = new BarringService(new BarringRepository(), new BarringValidator());

                Pcs = new UoM()
                {
                    Name = "Pcs"
                };
                _uomService.CreateObject(Pcs);

                customer = new Customer()
                {
                    Name = "President of Indonesia",
                    Address = "Istana Negara Jl. Veteran No. 16 Jakarta Pusat",
                    CustomerNo = "021 3863777",
                    PIC = "Mr. President",
                    PICCustomerNo = "021 3863777",
                    Email = "random@ri.gov.au"
                };
                customer = _customerService.CreateObject(customer);

                type = _itemTypeService.CreateObject("Item", "Item");

                warehouse = new Warehouse()
                {
                    Name = "Sentral Solusi Data",
                    Description = "Kali Besar Jakarta",
                    IsMovingWarehouse = false,
                    Code = "LCL"
                };
                warehouse = _warehouseService.CreateObject(warehouse, _warehouseItemService, _itemService);

                item_batiktulis = new Item()
                {
                    ItemTypeId = _itemTypeService.GetObjectByName("Item").Id,
                    Name = "Batik Tulis",
                    Category = "Item",
                    Sku = "bt123",
                    UoMId = Pcs.Id
                };

                item_batiktulis = _itemService.CreateObject(item_batiktulis, _uomService, _itemTypeService, _warehouseItemService, _warehouseService);
                _itemService.AdjustQuantity(item_batiktulis, 1000);
                _warehouseItemService.AdjustQuantity(_warehouseItemService.FindOrCreateObject(warehouse.Id, item_batiktulis.Id), 1000);

                item_busway = new Item()
                {
                    ItemTypeId = _itemTypeService.GetObjectByName("Item").Id,
                    Name = "Busway",
                    Category = "Untuk disumbangkan bagi kebutuhan DKI Jakarta",
                    Sku = "DKI002",
                    UoMId = Pcs.Id
                };
                item_busway = _itemService.CreateObject(item_busway, _uomService, _itemTypeService, _warehouseItemService, _warehouseService);
                _itemService.AdjustQuantity(item_busway, 200);
                _warehouseItemService.AdjustQuantity(_warehouseItemService.FindOrCreateObject(warehouse.Id, item_busway.Id), 200);

                item_botolaqua = new Item()
                {
                    ItemTypeId = _itemTypeService.GetObjectByName("Item").Id,
                    Name = "Botol Aqua",
                    Category = "Minuman",
                    Sku = "DKI003",
                    UoMId = Pcs.Id
                };
                item_botolaqua = _itemService.CreateObject(item_botolaqua, _uomService, _itemTypeService, _warehouseItemService, _warehouseService);
                _itemService.AdjustQuantity(item_botolaqua, 20000);
                _warehouseItemService.AdjustQuantity(_warehouseItemService.FindOrCreateObject(warehouse.Id, item_botolaqua.Id), 20000);

                salesOrder1 = _salesOrderService.CreateObject(customer.Id, new DateTime(2014, 07, 09), _customerService);
                salesOrder2 = _salesOrderService.CreateObject(customer.Id, new DateTime(2014, 04, 09), _customerService);
                salesOrderDetail_batiktulis_so1 = _salesOrderDetailService.CreateObject(salesOrder1.Id, item_batiktulis.Id, 500, 2000000, _salesOrderService, _itemService);
                salesOrderDetail_busway_so1 = _salesOrderDetailService.CreateObject(salesOrder1.Id, item_busway.Id, 91, 800000000, _salesOrderService, _itemService);
                salesOrderDetail_botolaqua_so1 = _salesOrderDetailService.CreateObject(salesOrder1.Id, item_botolaqua.Id, 2000, 5000, _salesOrderService, _itemService);
                salesOrderDetail_batiktulis_so2 = _salesOrderDetailService.CreateObject(salesOrder2.Id, item_batiktulis.Id, 40, 2000500, _salesOrderService, _itemService);
                salesOrderDetail_busway_so2 = _salesOrderDetailService.CreateObject(salesOrder2.Id, item_busway.Id, 3, 810000000, _salesOrderService, _itemService);
                salesOrderDetail_botolaqua_so2 = _salesOrderDetailService.CreateObject(salesOrder2.Id, item_botolaqua.Id, 340, 5500, _salesOrderService, _itemService);
                salesOrder1 = _salesOrderService.ConfirmObject(salesOrder1, _salesOrderDetailService, _stockMutationService, _itemService);
                salesOrder2 = _salesOrderService.ConfirmObject(salesOrder2, _salesOrderDetailService, _stockMutationService, _itemService);
            }
        }

        void deliveryorder_validation()
        {
            it["validates_all_variables"] = () =>
            {
                customer.Errors.Count().should_be(0);
                item_batiktulis.Errors.Count().should_be(0);
                item_busway.Errors.Count().should_be(0);
                item_botolaqua.Errors.Count().should_be(0);
                salesOrder1.Errors.Count().should_be(0);
                salesOrder2.Errors.Count().should_be(0);
            };

            it["validates the item pending delivery"] = () =>
            {
                salesOrderDetail_batiktulis_so1 = _salesOrderDetailService.FinishObject(salesOrderDetail_batiktulis_so1, _stockMutationService, _itemService, _barringService, _warehouseItemService);
                salesOrderDetail_busway_so1 = _salesOrderDetailService.FinishObject(salesOrderDetail_busway_so1, _stockMutationService, _itemService, _barringService, _warehouseItemService);
                salesOrderDetail_botolaqua_so1 = _salesOrderDetailService.FinishObject(salesOrderDetail_botolaqua_so1, _stockMutationService, _itemService, _barringService, _warehouseItemService);
                salesOrderDetail_batiktulis_so2 = _salesOrderDetailService.FinishObject(salesOrderDetail_batiktulis_so2, _stockMutationService, _itemService, _barringService, _warehouseItemService);
                salesOrderDetail_busway_so2 = _salesOrderDetailService.FinishObject(salesOrderDetail_busway_so2, _stockMutationService, _itemService, _barringService, _warehouseItemService);
                salesOrderDetail_botolaqua_so2 = _salesOrderDetailService.FinishObject(salesOrderDetail_botolaqua_so2, _stockMutationService, _itemService, _barringService, _warehouseItemService);

                salesOrderDetail_batiktulis_so1.IsFinished.should_be(true);
                salesOrderDetail_busway_so1.IsFinished.should_be(true);
                salesOrderDetail_botolaqua_so1.IsFinished.should_be(true);
                salesOrderDetail_batiktulis_so2.IsFinished.should_be(true);
                salesOrderDetail_busway_so2.IsFinished.should_be(true);
                salesOrderDetail_botolaqua_so2.IsFinished.should_be(true);

                item_batiktulis.PendingDelivery.should_be(salesOrderDetail_batiktulis_so1.Quantity + salesOrderDetail_batiktulis_so2.Quantity);
                item_busway.PendingDelivery.should_be(salesOrderDetail_busway_so1.Quantity + salesOrderDetail_busway_so2.Quantity);
                item_botolaqua.PendingDelivery.should_be(salesOrderDetail_botolaqua_so1.Quantity + salesOrderDetail_botolaqua_so2.Quantity);
            };

            context["when confirming delivery order"] = () =>
            {
                before = () =>
                {
                    salesOrderDetail_batiktulis_so1 = _salesOrderDetailService.FinishObject(salesOrderDetail_batiktulis_so1, _stockMutationService, _itemService, _barringService, _warehouseItemService);
                    salesOrderDetail_busway_so1 = _salesOrderDetailService.FinishObject(salesOrderDetail_busway_so1, _stockMutationService, _itemService, _barringService, _warehouseItemService);
                    salesOrderDetail_botolaqua_so1 = _salesOrderDetailService.FinishObject(salesOrderDetail_botolaqua_so1, _stockMutationService, _itemService, _barringService, _warehouseItemService);
                    salesOrderDetail_batiktulis_so2 = _salesOrderDetailService.FinishObject(salesOrderDetail_batiktulis_so2, _stockMutationService, _itemService, _barringService, _warehouseItemService);
                    salesOrderDetail_busway_so2 = _salesOrderDetailService.FinishObject(salesOrderDetail_busway_so2, _stockMutationService, _itemService, _barringService, _warehouseItemService);
                    salesOrderDetail_botolaqua_so2 = _salesOrderDetailService.FinishObject(salesOrderDetail_botolaqua_so2, _stockMutationService, _itemService, _barringService, _warehouseItemService);

                    deliveryOrder1 = _deliveryOrderService.CreateObject(warehouse.Id, customer.Id, new DateTime(2000, 1, 1), _customerService);
                    deliveryOrder2 = _deliveryOrderService.CreateObject(warehouse.Id, customer.Id, new DateTime(2014, 5, 5), _customerService);
                    deliveryOrderDetail_batiktulis_do1 = _deliveryOrderDetailService.CreateObject(deliveryOrder1.Id, item_batiktulis.Id, 400, salesOrderDetail_batiktulis_so1.Id, _deliveryOrderService,
                                                                                                  _salesOrderDetailService, _salesOrderService, _itemService, _customerService);
                    deliveryOrderDetail_busway_do1 = _deliveryOrderDetailService.CreateObject(deliveryOrder1.Id, item_busway.Id, 91, salesOrderDetail_busway_so1.Id, _deliveryOrderService,
                                                                                                _salesOrderDetailService, _salesOrderService, _itemService, _customerService);
                    deliveryOrderDetail_botolaqua_do1 = _deliveryOrderDetailService.CreateObject(deliveryOrder1.Id, item_botolaqua.Id, 2000, salesOrderDetail_botolaqua_so1.Id,  _deliveryOrderService,
                                                                                                  _salesOrderDetailService, _salesOrderService, _itemService, _customerService);
                    deliveryOrderDetail_batiktulis_do2a = _deliveryOrderDetailService.CreateObject(deliveryOrder2.Id, item_batiktulis.Id, 100, salesOrderDetail_batiktulis_so1.Id, _deliveryOrderService,
                                                                                                                          _salesOrderDetailService, _salesOrderService, _itemService, _customerService);
                    deliveryOrderDetail_batiktulis_do2b = _deliveryOrderDetailService.CreateObject(deliveryOrder2.Id, item_batiktulis.Id, 40, salesOrderDetail_batiktulis_so2.Id, _deliveryOrderService,
                                                                                                                          _salesOrderDetailService, _salesOrderService, _itemService, _customerService);
                    deliveryOrderDetail_busway_do2 = _deliveryOrderDetailService.CreateObject(deliveryOrder2.Id, item_busway.Id, 3, salesOrderDetail_busway_so2.Id, _deliveryOrderService,
                                                                                                                          _salesOrderDetailService, _salesOrderService, _itemService, _customerService);
                    deliveryOrderDetail_botolaqua_do2 = _deliveryOrderDetailService.CreateObject(deliveryOrder2.Id, item_botolaqua.Id, 340, salesOrderDetail_botolaqua_so2.Id, _deliveryOrderService,
                                                                                                                          _salesOrderDetailService, _salesOrderService, _itemService, _customerService);
                    deliveryOrder1 = _deliveryOrderService.ConfirmObject(deliveryOrder1, _deliveryOrderDetailService, _salesOrderDetailService, _stockMutationService, _itemService);
                    deliveryOrder2 = _deliveryOrderService.ConfirmObject(deliveryOrder2, _deliveryOrderDetailService, _salesOrderDetailService, _stockMutationService, _itemService);
                };

                it["validates_deliveryorders"] = () =>
                {
                    deliveryOrder1.Errors.Count().should_be(0);
                    deliveryOrder2.Errors.Count().should_be(0);
                };

                it["deletes confirmed delivery order"] = () =>
                {
                    deliveryOrder1 = _deliveryOrderService.SoftDeleteObject(deliveryOrder1, _deliveryOrderDetailService);
                    deliveryOrder1.Errors.Count().should_not_be(0);
                };

                it["unconfirm delivery order"] = () =>
                {
                    deliveryOrder1 = _deliveryOrderService.UnconfirmObject(deliveryOrder1, _deliveryOrderDetailService, _stockMutationService, _itemService);
                    deliveryOrder1.Errors.Count().should_be(0);
                };

                it["validates item pending delivery"] = () =>
                {
                    deliveryOrderDetail_batiktulis_do1 = _deliveryOrderDetailService.FinishObject(deliveryOrderDetail_batiktulis_do1, _deliveryOrderService, _salesOrderDetailService, _stockMutationService,
                                                                                                  _itemService, _barringService, _warehouseItemService);
                    deliveryOrderDetail_busway_do1 = _deliveryOrderDetailService.FinishObject(deliveryOrderDetail_busway_do1, _deliveryOrderService, _salesOrderDetailService, _stockMutationService,
                                                                                              _itemService, _barringService, _warehouseItemService);
                    deliveryOrderDetail_botolaqua_do1 = _deliveryOrderDetailService.FinishObject(deliveryOrderDetail_botolaqua_do1, _deliveryOrderService, _salesOrderDetailService, _stockMutationService,
                                                                                                 _itemService, _barringService, _warehouseItemService);
                    deliveryOrderDetail_batiktulis_do2a = _deliveryOrderDetailService.FinishObject(deliveryOrderDetail_batiktulis_do2a, _deliveryOrderService, _salesOrderDetailService, _stockMutationService,
                                                                                                   _itemService, _barringService, _warehouseItemService);
                    deliveryOrderDetail_batiktulis_do2b = _deliveryOrderDetailService.FinishObject(deliveryOrderDetail_batiktulis_do2b, _deliveryOrderService, _salesOrderDetailService, _stockMutationService,
                                                                                                  _itemService, _barringService, _warehouseItemService);
                    deliveryOrderDetail_busway_do2 = _deliveryOrderDetailService.FinishObject(deliveryOrderDetail_busway_do2, _deliveryOrderService, _salesOrderDetailService, _stockMutationService,
                                                                                                  _itemService, _barringService, _warehouseItemService);
                    deliveryOrderDetail_botolaqua_do2 = _deliveryOrderDetailService.FinishObject(deliveryOrderDetail_botolaqua_do2, _deliveryOrderService, _salesOrderDetailService, _stockMutationService,
                                                                                                  _itemService, _barringService, _warehouseItemService);
                    item_batiktulis.PendingDelivery.should_be(0);
                    item_busway.PendingDelivery.should_be(0);
                    item_botolaqua.PendingDelivery.should_be(0);
                };
            };
        }
    }
}