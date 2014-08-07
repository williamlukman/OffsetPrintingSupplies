using Core.DomainModel;
using Core.Interface.Repository;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface IDeliveryOrderDetailService
    {
        IDeliveryOrderDetailValidator GetValidator();
        IList<DeliveryOrderDetail> GetObjectsByDeliveryOrderId(int deliveryOrderId);
        DeliveryOrderDetail GetObjectById(int Id);
        IList<DeliveryOrderDetail> GetObjectsBySalesOrderDetailId(int salesOrderDetailId);
        DeliveryOrderDetail CreateObject(DeliveryOrderDetail deliveryOrderDetail,
                                         IDeliveryOrderService _deliveryOrderService, ISalesOrderDetailService _salesOrderDetailService,
                                         ISalesOrderService _salesOrderService, IItemService _itemService, IContactService _contactService);
        DeliveryOrderDetail CreateObject(int deliveryOrderId, int itemId, int quantity, int salesOrderDetailId,
                                         IDeliveryOrderService _deliveryOrderService, ISalesOrderDetailService _salesOrderDetailService,
                                         ISalesOrderService _salesOrderService, IItemService _itemService, IContactService _contactService);
        DeliveryOrderDetail UpdateObject(DeliveryOrderDetail deliveryOrderDetail,
                                         IDeliveryOrderService _deliveryOrderService, ISalesOrderDetailService _salesOrderDetailService,
                                         ISalesOrderService _salesOrderService, IItemService _itemService, IContactService _contactService);
        DeliveryOrderDetail SoftDeleteObject(DeliveryOrderDetail deliveryOrderDetail);
        bool DeleteObject(int Id);
        DeliveryOrderDetail FinishObject(DeliveryOrderDetail deliveryOrderDetail, IDeliveryOrderService _deliveryOrderService, ISalesOrderDetailService _salesOrderDetailService,
                                                IStockMutationService _stockMutationService, IItemService _itemService, IBarringService _barringService, IWarehouseItemService _warehouseItemService);
        DeliveryOrderDetail UnfinishObject(DeliveryOrderDetail deliveryOrderDetail, IDeliveryOrderService _deliveryOrderService, IStockMutationService _stockMutationService,
                                           IItemService _itemService, IBarringService _barringService, IWarehouseItemService _warehouseItemService);
        DeliveryOrderDetail InvoiceObject(DeliveryOrderDetail deliveryOrderDetail, int Quantity);
        DeliveryOrderDetail UndoInvoiceObject(DeliveryOrderDetail deliveryOrderDetail, int Quantity, IDeliveryOrderService _deliveryOrderService);
    }
}