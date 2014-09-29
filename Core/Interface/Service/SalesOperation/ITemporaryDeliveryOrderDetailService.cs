using Core.DomainModel;
using Core.Interface.Repository;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface ITemporaryDeliveryOrderDetailService
    {
        ITemporaryDeliveryOrderDetailValidator GetValidator();
        IQueryable<TemporaryDeliveryOrderDetail> GetQueryable();
        IList<TemporaryDeliveryOrderDetail> GetAll();
        IList<TemporaryDeliveryOrderDetail> GetObjectsByTemporaryDeliveryOrderId(int temporaryDeliveryOrderId);
        TemporaryDeliveryOrderDetail GetObjectById(int Id);
        IList<TemporaryDeliveryOrderDetail> GetObjectsBySalesOrderDetailId(int salesOrderDetailId);
        IList<TemporaryDeliveryOrderDetail> GetObjectsByVirtualOrderDetailId(int virtualOrderDetailId);
        TemporaryDeliveryOrderDetail CreateObject(TemporaryDeliveryOrderDetail temporaryDeliveryOrderDetail, ITemporaryDeliveryOrderService _temporaryDeliveryOrderService, 
                                                  IVirtualOrderDetailService _virtualOrderDetailService, ISalesOrderDetailService _salesOrderDetailService,
                                                  IDeliveryOrderService _deliveryOrderService, IItemService _itemService);
        TemporaryDeliveryOrderDetail UpdateObject(TemporaryDeliveryOrderDetail temporaryDeliveryOrderDetail, ITemporaryDeliveryOrderService _temporaryDeliveryOrderService,
                                                  IVirtualOrderDetailService _virtualOrderDetailService, ISalesOrderDetailService _salesOrderDetailService,
                                                  IDeliveryOrderService _deliveryOrderService, IItemService _itemService);
        TemporaryDeliveryOrderDetail SoftDeleteObject(TemporaryDeliveryOrderDetail temporaryDeliveryOrderDetail);
        bool DeleteObject(int Id);
        TemporaryDeliveryOrderDetail ConfirmObject(TemporaryDeliveryOrderDetail temporaryDeliveryOrderDetail, DateTime ConfirmationDate, ITemporaryDeliveryOrderService _temporaryDeliveryOrderService,
                                                   IVirtualOrderDetailService _virtualOrderDetailService, ISalesOrderDetailService _salesOrderDetailService, IStockMutationService _stockMutationService,
                                                   IItemService _itemService, IBlanketService _blanketService, IWarehouseItemService _warehouseItemService);
        TemporaryDeliveryOrderDetail UnconfirmObject(TemporaryDeliveryOrderDetail temporaryDeliveryOrderDetail, ITemporaryDeliveryOrderService _temporaryDeliveryOrderService,
                                                    IVirtualOrderDetailService _virtualOrderDetailService, IVirtualOrderService _virtualOrderService,
                                                    ISalesOrderService _salesOrderService, ISalesOrderDetailService _salesOrderDetailService, IStockMutationService _stockMutationService,
                                                    IItemService _itemService, IBlanketService _blanketService, IWarehouseItemService _warehouseItemService);
        TemporaryDeliveryOrderDetail ReconcileObject(TemporaryDeliveryOrderDetail temporaryDeliveryOrderDetail, int wasteQuantity, int restockQuantity);
        TemporaryDeliveryOrderDetail UnreconcileObject(TemporaryDeliveryOrderDetail temporaryDeliveryOrderDetail, int wasteQuantity, int restockQuantity, ITemporaryDeliveryOrderService _temporaryDeliveryOrderService);
    }
}