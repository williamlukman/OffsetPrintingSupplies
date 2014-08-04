using Core.DomainModel;
using Core.Interface.Repository;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interface.Service
{
    public interface IDeliveryOrderDetailService
    {
        IDeliveryOrderDetailValidator GetValidator();
        IList<DeliveryOrderDetail> GetObjectsByDeliveryOrderId(int deliveryOrderId);
        DeliveryOrderDetail GetObjectById(int Id);
        DeliveryOrderDetail GetObjectBySalesOrderDetailId(int salesOrderDetailId);
        DeliveryOrderDetail CreateObject(DeliveryOrderDetail deliveryOrderDetail,
                                         IDeliveryOrderService _deliveryOrderService, ISalesOrderDetailService _salesOrderDetailService,
                                         ISalesOrderService _salesOrderService, IItemService _itemService, ICustomerService _customerService);
        DeliveryOrderDetail CreateObject(int deliveryOrderId, int itemId, int quantity, int salesOrderDetailId,
                                         IDeliveryOrderService _deliveryOrderService, ISalesOrderDetailService _salesOrderDetailService,
                                         ISalesOrderService _salesOrderService, IItemService _itemService, ICustomerService _customerService);
        DeliveryOrderDetail UpdateObject(DeliveryOrderDetail deliveryOrderDetail,
                                         IDeliveryOrderService _deliveryOrderService, ISalesOrderDetailService _salesOrderDetailService,
                                         ISalesOrderService _salesOrderService, IItemService _itemService, ICustomerService _customerService);
        DeliveryOrderDetail SoftDeleteObject(DeliveryOrderDetail deliveryOrderDetail);
        bool DeleteObject(int Id);
        DeliveryOrderDetail FinishObject(DeliveryOrderDetail deliveryOrderDetail, IDeliveryOrderService _deliveryOrderService, ISalesOrderDetailService _salesOrderDetailService,
                                                IStockMutationService _stockMutationService, IItemService _itemService, IBarringService _barringService, IWarehouseItemService _warehouseItemService);
        DeliveryOrderDetail UnfinishObject(DeliveryOrderDetail deliveryOrderDetail, IDeliveryOrderService _deliveryOrderService, IStockMutationService _stockMutationService,
                                           IItemService _itemService, IBarringService _barringService, IWarehouseItemService _warehouseItemService);
    }
}