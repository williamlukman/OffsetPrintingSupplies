using Core.DomainModel;
using Core.Interface.Repository;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface ITemporaryDeliveryOrderClearanceService
    {
        ITemporaryDeliveryOrderClearanceValidator GetValidator();
        IQueryable<TemporaryDeliveryOrderClearance> GetQueryable();
        IList<TemporaryDeliveryOrderClearance> GetAll();
        TemporaryDeliveryOrderClearance GetObjectById(int Id);
        IList<TemporaryDeliveryOrderClearance> GetObjectsByTemporaryDeliveryOrderId(int deliveryOrderId);
        IList<TemporaryDeliveryOrderClearance> GetConfirmedObjects();
        TemporaryDeliveryOrderClearance CreateObject(TemporaryDeliveryOrderClearance temporaryDeliveryOrderClearance, IVirtualOrderService _virtualOrderService, IDeliveryOrderService _deliveryOrderService, IWarehouseService _warehouseService);
        TemporaryDeliveryOrderClearance UpdateObject(TemporaryDeliveryOrderClearance temporaryDeliveryOrderClearance, IVirtualOrderService _virtualOrderService, IDeliveryOrderService _deliveryOrderService, IWarehouseService _warehouseService);
        TemporaryDeliveryOrderClearance SoftDeleteObject(TemporaryDeliveryOrderClearance temporaryDeliveryOrderClearance, ITemporaryDeliveryOrderClearanceDetailService _temporaryDeliveryOrderClearanceDetailService);
        bool DeleteObject(int Id);
        TemporaryDeliveryOrderClearance ConfirmObject(TemporaryDeliveryOrderClearance temporaryDeliveryOrderClearance, DateTime ConfirmationDate, ITemporaryDeliveryOrderClearanceDetailService _temporaryDeliveryOrderClearanceDetailService,
                                             IVirtualOrderService _virtualOrderService, IVirtualOrderDetailService _virtualOrderDetailService,
                                             IDeliveryOrderService _deliveryOrderService, IDeliveryOrderDetailService _deliveryOrderDetailService,
                                             ISalesOrderDetailService _salesOrderDetailService, IStockMutationService _stockMutationService, IItemService _itemService,
                                             IBlanketService _blanketService, IWarehouseItemService _warehouseItemService);
        TemporaryDeliveryOrderClearance UnconfirmObject(TemporaryDeliveryOrderClearance temporaryDeliveryOrderClearance, ITemporaryDeliveryOrderClearanceDetailService _temporaryDeliveryOrderClearanceDetailService,
                                               IVirtualOrderService _virtualOrderService, IVirtualOrderDetailService _virtualOrderDetailService,
                                               IDeliveryOrderService _deliveryOrderService, IDeliveryOrderDetailService _deliveryOrderDetailService,
                                               ISalesOrderService _salesOrderService, ISalesOrderDetailService _salesOrderDetailService,
                                               IStockMutationService _stockMutationService, IItemService _itemService,
                                               IBlanketService _blanketService, IWarehouseItemService _warehouseItemService);
        
    }
}