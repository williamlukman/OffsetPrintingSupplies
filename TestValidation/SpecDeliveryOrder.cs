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

    public class SpecDeliveryOrder : nspec
    {
        ContactGroup baseGroup;
        Contact contact;
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
        DeliveryOrder deliveryOrder3;
        DeliveryOrderDetail deliveryOrderDetail_batiktulis_do1;
        DeliveryOrderDetail deliveryOrderDetail_busway_do1;
        DeliveryOrderDetail deliveryOrderDetail_botolaqua_do1;
        DeliveryOrderDetail deliveryOrderDetail_batiktulis_do2a;
        DeliveryOrderDetail deliveryOrderDetail_batiktulis_do2b;
        DeliveryOrderDetail deliveryOrderDetail_busway_do2;
        DeliveryOrderDetail deliveryOrderDetail_botolaqua_do2;
        IContactService _contactService;
        IItemService _itemService;
        IStockMutationService _stockMutationService;
        IStockAdjustmentService _stockAdjustmentService;
        IStockAdjustmentDetailService _stockAdjustmentDetailService;
        ISalesOrderService _salesOrderService;
        ISalesOrderDetailService _salesOrderDetailService;
        ISalesInvoiceDetailService _salesInvoiceDetailService;
        ISalesInvoiceService _salesInvoiceService;
        IDeliveryOrderService _deliveryOrderService;
        IDeliveryOrderDetailService _deliveryOrderDetailService;
        IUoMService _uomService;
        IBlanketService _blanketService;
        IItemTypeService _itemTypeService;
        IWarehouseItemService _warehouseItemService;
        IWarehouseService _warehouseService;

        IPriceMutationService _priceMutationService;
        IContactGroupService _contactGroupService;

        void before_each()
        {
            var db = new OffsetPrintingSuppliesEntities();
            using (db)
            {
                db.DeleteAllTables();
                _contactService = new ContactService(new ContactRepository(), new ContactValidator());
                _itemService = new ItemService(new ItemRepository(), new ItemValidator());
                _stockMutationService = new StockMutationService(new StockMutationRepository(), new StockMutationValidator());
                _salesOrderService = new SalesOrderService(new SalesOrderRepository(), new SalesOrderValidator());
                _salesOrderDetailService = new SalesOrderDetailService(new SalesOrderDetailRepository(), new SalesOrderDetailValidator());
                _salesInvoiceService = new SalesInvoiceService(new SalesInvoiceRepository(), new SalesInvoiceValidator());
                _salesInvoiceDetailService = new SalesInvoiceDetailService(new SalesInvoiceDetailRepository(), new SalesInvoiceDetailValidator());
                _deliveryOrderService = new DeliveryOrderService(new DeliveryOrderRepository(), new DeliveryOrderValidator());
                _deliveryOrderDetailService = new DeliveryOrderDetailService(new DeliveryOrderDetailRepository(), new DeliveryOrderDetailValidator());
                _stockAdjustmentService = new StockAdjustmentService(new StockAdjustmentRepository(), new StockAdjustmentValidator());
                _stockAdjustmentDetailService = new StockAdjustmentDetailService(new StockAdjustmentDetailRepository(), new StockAdjustmentDetailValidator());
                _itemTypeService = new ItemTypeService(new ItemTypeRepository(), new ItemTypeValidator());
                _uomService = new UoMService(new UoMRepository(), new UoMValidator());
                _warehouseItemService = new WarehouseItemService(new WarehouseItemRepository(), new WarehouseItemValidator());
                _warehouseService = new WarehouseService(new WarehouseRepository(), new WarehouseValidator());
                _blanketService = new BlanketService(new BlanketRepository(), new BlanketValidator());

                _priceMutationService = new PriceMutationService(new PriceMutationRepository(), new PriceMutationValidator());
                _contactGroupService = new ContactGroupService(new ContactGroupRepository(), new ContactGroupValidator());

                baseGroup = _contactGroupService.CreateObject(Core.Constants.Constant.GroupType.Base, "Base Group", true);

                Pcs = new UoM()
                {
                    Name = "Pcs"
                };
                _uomService.CreateObject(Pcs);

                contact = new Contact()
                {
                    Name = "President of Indonesia",
                    Address = "Istana Negara Jl. Veteran No. 16 Jakarta Pusat",
                    ContactNo = "021 3863777",
                    PIC = "Mr. President",
                    PICContactNo = "021 3863777",
                    Email = "random@ri.gov.au"
                };
                contact = _contactService.CreateObject(contact, _contactGroupService);

                type = _itemTypeService.CreateObject("Item", "Item");

                warehouse = new Warehouse()
                {
                    Name = "Sentral Solusi Data",
                    Description = "Kali Besar Jakarta",
                    Code = "LCL"
                };
                warehouse = _warehouseService.CreateObject(warehouse, _warehouseItemService, _itemService);

                item_batiktulis = new Item()
                {
                    ItemTypeId = _itemTypeService.GetObjectByName("Item").Id,
                    Name = "Batik Tulis",
                    Description = "Item",
                    Sku = "bt123",
                    UoMId = Pcs.Id
                };

                item_batiktulis = _itemService.CreateObject(item_batiktulis, _uomService, _itemTypeService, _warehouseItemService, _warehouseService, _priceMutationService, _contactGroupService);

                item_busway = new Item()
                {
                    ItemTypeId = _itemTypeService.GetObjectByName("Item").Id,
                    Name = "Busway",
                    Description = "Untuk disumbangkan bagi kebutuhan DKI Jakarta",
                    Sku = "DKI002",
                    UoMId = Pcs.Id
                };
                item_busway = _itemService.CreateObject(item_busway, _uomService, _itemTypeService, _warehouseItemService, _warehouseService, _priceMutationService, _contactGroupService);

                item_botolaqua = new Item()
                {
                    ItemTypeId = _itemTypeService.GetObjectByName("Item").Id,
                    Name = "Botol Aqua",
                    Description = "Minuman",
                    Sku = "DKI003",
                    UoMId = Pcs.Id
                };
                item_botolaqua = _itemService.CreateObject(item_botolaqua, _uomService, _itemTypeService, _warehouseItemService, _warehouseService, _priceMutationService, _contactGroupService);

                StockAdjustment sa = new StockAdjustment() { AdjustmentDate = DateTime.Today, WarehouseId = warehouse.Id, Description = "item adjustment" };
                _stockAdjustmentService.CreateObject(sa, _warehouseService);
                StockAdjustmentDetail sadBatikTulis = new StockAdjustmentDetail()
                {
                    ItemId = item_batiktulis.Id,
                    Quantity = 1000,
                    StockAdjustmentId = sa.Id
                };
                _stockAdjustmentDetailService.CreateObject(sadBatikTulis, _stockAdjustmentService, _itemService, _warehouseItemService);

                StockAdjustmentDetail sadBusWay = new StockAdjustmentDetail()
                {
                    ItemId = item_busway.Id,
                    Quantity = 200,
                    StockAdjustmentId = sa.Id
                };
                _stockAdjustmentDetailService.CreateObject(sadBusWay, _stockAdjustmentService, _itemService, _warehouseItemService);

                StockAdjustmentDetail sadBotolAqua = new StockAdjustmentDetail()
                {
                    ItemId = item_botolaqua.Id,
                    Quantity = 20000,
                    StockAdjustmentId = sa.Id
                };
                _stockAdjustmentDetailService.CreateObject(sadBotolAqua, _stockAdjustmentService, _itemService, _warehouseItemService);

                _stockAdjustmentService.ConfirmObject(sa, DateTime.Today, _stockAdjustmentDetailService, _stockMutationService,
                                                      _itemService, _blanketService, _warehouseItemService);

                salesOrder1 = _salesOrderService.CreateObject(contact.Id, new DateTime(2014, 07, 09), _contactService);
                salesOrder2 = _salesOrderService.CreateObject(contact.Id, new DateTime(2014, 04, 09), _contactService);
                salesOrderDetail_batiktulis_so1 = _salesOrderDetailService.CreateObject(salesOrder1.Id, item_batiktulis.Id, 500, 2000000, _salesOrderService, _itemService);
                salesOrderDetail_busway_so1 = _salesOrderDetailService.CreateObject(salesOrder1.Id, item_busway.Id, 91, 800000000, _salesOrderService, _itemService);
                salesOrderDetail_botolaqua_so1 = _salesOrderDetailService.CreateObject(salesOrder1.Id, item_botolaqua.Id, 2000, 5000, _salesOrderService, _itemService);
                salesOrderDetail_batiktulis_so2 = _salesOrderDetailService.CreateObject(salesOrder2.Id, item_batiktulis.Id, 40, 2000500, _salesOrderService, _itemService);
                salesOrderDetail_busway_so2 = _salesOrderDetailService.CreateObject(salesOrder2.Id, item_busway.Id, 3, 810000000, _salesOrderService, _itemService);
                salesOrderDetail_botolaqua_so2 = _salesOrderDetailService.CreateObject(salesOrder2.Id, item_botolaqua.Id, 340, 5500, _salesOrderService, _itemService);
                salesOrder1 = _salesOrderService.ConfirmObject(salesOrder1, DateTime.Today, _salesOrderDetailService, _stockMutationService, _itemService, _blanketService, _warehouseItemService);
                salesOrder2 = _salesOrderService.ConfirmObject(salesOrder2, DateTime.Today, _salesOrderDetailService, _stockMutationService, _itemService, _blanketService, _warehouseItemService);
            }
        }

        void deliveryorder_validation()
        {
            it["validates_all_variables"] = () =>
            {
                contact.Errors.Count().should_be(0);
                item_batiktulis.Errors.Count().should_be(0);
                item_busway.Errors.Count().should_be(0);
                item_botolaqua.Errors.Count().should_be(0);
                salesOrder1.Errors.Count().should_be(0);
                salesOrder2.Errors.Count().should_be(0);
            };

            it["validates the item pending delivery"] = () =>
            {
                salesOrderDetail_batiktulis_so1.IsConfirmed.should_be(true);
                salesOrderDetail_busway_so1.IsConfirmed.should_be(true);
                salesOrderDetail_botolaqua_so1.IsConfirmed.should_be(true);
                salesOrderDetail_batiktulis_so2.IsConfirmed.should_be(true);
                salesOrderDetail_busway_so2.IsConfirmed.should_be(true);
                salesOrderDetail_botolaqua_so2.IsConfirmed.should_be(true);

                item_batiktulis.PendingDelivery.should_be(salesOrderDetail_batiktulis_so1.Quantity + salesOrderDetail_batiktulis_so2.Quantity);
                item_busway.PendingDelivery.should_be(salesOrderDetail_busway_so1.Quantity + salesOrderDetail_busway_so2.Quantity);
                item_botolaqua.PendingDelivery.should_be(salesOrderDetail_botolaqua_so1.Quantity + salesOrderDetail_botolaqua_so2.Quantity);
            };

            context["when confirming delivery order"] = () =>
            {
                before = () =>
                {
                    deliveryOrder1 = _deliveryOrderService.CreateObject(warehouse.Id, salesOrder1.Id, new DateTime(2000, 1, 1), _salesOrderService, _warehouseService);
                    deliveryOrder2 = _deliveryOrderService.CreateObject(warehouse.Id, salesOrder2.Id, new DateTime(2014, 5, 5), _salesOrderService, _warehouseService);
                    deliveryOrder3 = _deliveryOrderService.CreateObject(warehouse.Id, salesOrder1.Id, new DateTime(2014, 5, 5), _salesOrderService, _warehouseService);
                    deliveryOrderDetail_batiktulis_do1 = _deliveryOrderDetailService.CreateObject(deliveryOrder1.Id, item_batiktulis.Id, 400, salesOrderDetail_batiktulis_so1.Id, _deliveryOrderService,
                                                                                                  _salesOrderDetailService, _salesOrderService, _itemService);
                    deliveryOrderDetail_busway_do1 = _deliveryOrderDetailService.CreateObject(deliveryOrder1.Id, item_busway.Id, 91, salesOrderDetail_busway_so1.Id, _deliveryOrderService,
                                                                                                _salesOrderDetailService, _salesOrderService, _itemService);
                    deliveryOrderDetail_botolaqua_do1 = _deliveryOrderDetailService.CreateObject(deliveryOrder1.Id, item_botolaqua.Id, 2000, salesOrderDetail_botolaqua_so1.Id,  _deliveryOrderService,
                                                                                                  _salesOrderDetailService, _salesOrderService, _itemService);
                    deliveryOrderDetail_batiktulis_do2b = _deliveryOrderDetailService.CreateObject(deliveryOrder2.Id, item_batiktulis.Id, 40, salesOrderDetail_batiktulis_so2.Id, _deliveryOrderService,
                                                                                                                          _salesOrderDetailService, _salesOrderService, _itemService);
                    deliveryOrderDetail_busway_do2 = _deliveryOrderDetailService.CreateObject(deliveryOrder2.Id, item_busway.Id, 3, salesOrderDetail_busway_so2.Id, _deliveryOrderService,
                                                                                                                          _salesOrderDetailService, _salesOrderService, _itemService);
                    deliveryOrderDetail_botolaqua_do2 = _deliveryOrderDetailService.CreateObject(deliveryOrder2.Id, item_botolaqua.Id, 340, salesOrderDetail_botolaqua_so2.Id, _deliveryOrderService,
                                                                                                                          _salesOrderDetailService, _salesOrderService, _itemService);
                    deliveryOrderDetail_batiktulis_do2a = _deliveryOrderDetailService.CreateObject(deliveryOrder3.Id, item_batiktulis.Id, 100, salesOrderDetail_batiktulis_so1.Id, _deliveryOrderService,
                                                                                                                          _salesOrderDetailService, _salesOrderService, _itemService);
                    deliveryOrder1 = _deliveryOrderService.ConfirmObject(deliveryOrder1, DateTime.Today, _deliveryOrderDetailService, _salesOrderService, _salesOrderDetailService, _stockMutationService, _itemService,
                                                                         _blanketService, _warehouseItemService);
                    deliveryOrder2 = _deliveryOrderService.ConfirmObject(deliveryOrder2, DateTime.Today, _deliveryOrderDetailService, _salesOrderService, _salesOrderDetailService, _stockMutationService, _itemService,
                                                                         _blanketService, _warehouseItemService);
                    deliveryOrder3 = _deliveryOrderService.ConfirmObject(deliveryOrder3, DateTime.Today, _deliveryOrderDetailService, _salesOrderService, _salesOrderDetailService, _stockMutationService, _itemService,
                                                                         _blanketService, _warehouseItemService);
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
                    deliveryOrder1 = _deliveryOrderService.UnconfirmObject(deliveryOrder1, _deliveryOrderDetailService,
                                                                           _salesInvoiceService, _salesInvoiceDetailService, _salesOrderService,
                                                                           _salesOrderDetailService, _stockMutationService, _itemService,
                                                                           _blanketService, _warehouseItemService);
                    deliveryOrder1.Errors.Count().should_be(0);
                };

                it["validates item pending delivery"] = () =>
                {
                    item_batiktulis.PendingDelivery.should_be(0);
                    item_busway.PendingDelivery.should_be(0);
                    item_botolaqua.PendingDelivery.should_be(0);
                };
            };
        }
    }
}