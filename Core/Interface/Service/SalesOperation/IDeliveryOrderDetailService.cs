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
        IQueryable<DeliveryOrderDetail> GetQueryable();
        IList<DeliveryOrderDetail> GetAll();
        IList<DeliveryOrderDetail> GetObjectsByDeliveryOrderId(int deliveryOrderId);
        DeliveryOrderDetail GetObjectById(int Id);
        IList<DeliveryOrderDetail> GetObjectsBySalesOrderDetailId(int salesOrderDetailId);
        DeliveryOrderDetail CreateObject(DeliveryOrderDetail deliveryOrderDetail,
                                         IDeliveryOrderService _deliveryOrderService, ISalesOrderDetailService _salesOrderDetailService,
                                         ISalesOrderService _salesOrderService, IItemService _itemService);
        DeliveryOrderDetail UpdateObject(DeliveryOrderDetail deliveryOrderDetail,
                                         IDeliveryOrderService _deliveryOrderService, ISalesOrderDetailService _salesOrderDetailService,
                                         ISalesOrderService _salesOrderService, IItemService _itemService);
        DeliveryOrderDetail SoftDeleteObject(DeliveryOrderDetail deliveryOrderDetail);
        bool DeleteObject(int Id);
        DeliveryOrderDetail ConfirmObject(DeliveryOrderDetail deliveryOrderDetail, DateTime ConfirmationDate, IDeliveryOrderService _deliveryOrderService, ISalesOrderDetailService _salesOrderDetailService,
                                          IStockMutationService _stockMutationService, IItemService _itemService, IBlanketService _blanketService, IWarehouseItemService _warehouseItemService,
                                          IServiceCostService _serviceCostService, ICustomerStockMutationService _customerStockMutationService, ICustomerItemService _customerItemService);
        DeliveryOrderDetail UnconfirmObject(DeliveryOrderDetail deliveryOrderDetail, IDeliveryOrderService _deliveryOrderService, ISalesOrderService _salesOrderService,
                                            ISalesOrderDetailService _salesOrderDetailService, ISalesInvoiceDetailService _salesInvoiceDetailService, IStockMutationService _stockMutationService,
                                            IItemService _itemService, IBlanketService _blanketService, IWarehouseItemService _warehouseItemService, ICustomerStockMutationService _customerStockMutationService, ICustomerItemService _customerItemService);
        DeliveryOrderDetail InvoiceObject(DeliveryOrderDetail deliveryOrderDetail, int Quantity);
        DeliveryOrderDetail UndoInvoiceObject(DeliveryOrderDetail deliveryOrderDetail, int Quantity, IDeliveryOrderService _deliveryOrderService);
    }
}