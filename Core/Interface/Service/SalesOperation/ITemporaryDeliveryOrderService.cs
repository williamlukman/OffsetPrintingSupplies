using Core.DomainModel;
using Core.Interface.Repository;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface ITemporaryDeliveryOrderService
    {
        ITemporaryDeliveryOrderValidator GetValidator();
        IQueryable<TemporaryDeliveryOrder> GetQueryable();
        IList<TemporaryDeliveryOrder> GetAll();
        TemporaryDeliveryOrder GetObjectById(int Id);
        IList<TemporaryDeliveryOrder> GetObjectsByVirtualOrderId(int virtualOrderId);
        IList<TemporaryDeliveryOrder> GetObjectsByDeliveryOrderId(int deliveryOrderId);
        IList<TemporaryDeliveryOrder> GetConfirmedObjects();
        TemporaryDeliveryOrder CreateObject(TemporaryDeliveryOrder temporaryDeliveryOrder, IVirtualOrderService _virtualOrderService, IDeliveryOrderService _deliveryOrderService, IWarehouseService _warehouseService);
        TemporaryDeliveryOrder UpdateObject(TemporaryDeliveryOrder temporaryDeliveryOrder, IVirtualOrderService _virtualOrderService, IDeliveryOrderService _deliveryOrderService, IWarehouseService _warehouseService);
        TemporaryDeliveryOrder SoftDeleteObject(TemporaryDeliveryOrder temporaryDeliveryOrder, ITemporaryDeliveryOrderDetailService _temporaryDeliveryOrderDetailService);
        bool DeleteObject(int Id);
        TemporaryDeliveryOrder ConfirmObject(TemporaryDeliveryOrder temporaryDeliveryOrder, DateTime ConfirmationDate, ITemporaryDeliveryOrderDetailService _temporaryDeliveryOrderDetailService,
                                             IVirtualOrderService _virtualOrderService, IVirtualOrderDetailService _virtualOrderDetailService,
                                             IDeliveryOrderService _deliveryOrderService, IDeliveryOrderDetailService _deliveryOrderDetailService,
                                             ISalesOrderDetailService _salesOrderDetailService, IStockMutationService _stockMutationService, IItemService _itemService,
                                             IBlanketService _blanketService, IWarehouseItemService _warehouseItemService);
        TemporaryDeliveryOrder UnconfirmObject(TemporaryDeliveryOrder temporaryDeliveryOrder, ITemporaryDeliveryOrderDetailService _temporaryDeliveryOrderDetailService,
                                               IVirtualOrderService _virtualOrderService, IVirtualOrderDetailService _virtualOrderDetailService,
                                               IDeliveryOrderService _deliveryOrderService, IDeliveryOrderDetailService _deliveryOrderDetailService,
                                               ISalesOrderService _salesOrderService, ISalesOrderDetailService _salesOrderDetailService,
                                               IStockMutationService _stockMutationService, IItemService _itemService,
                                               IBlanketService _blanketService, IWarehouseItemService _warehouseItemService);
        TemporaryDeliveryOrder SetReconcileComplete(TemporaryDeliveryOrder temporaryDeliveryOrder, ITemporaryDeliveryOrderDetailService _temporaryDeliveryOrderDetailService);
        TemporaryDeliveryOrder UnsetReconcileComplete(TemporaryDeliveryOrder temporaryDeliveryOrder);
    }
}