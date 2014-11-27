using Core.DomainModel;
using Core.Interface.Repository;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface ITemporaryDeliveryOrderClearanceDetailService
    {
        ITemporaryDeliveryOrderClearanceDetailValidator GetValidator();
        IQueryable<TemporaryDeliveryOrderClearanceDetail> GetQueryable();
        IList<TemporaryDeliveryOrderClearanceDetail> GetAll();
        IList<TemporaryDeliveryOrderClearanceDetail> GetObjectsByTemporaryDeliveryOrderClearanceId(int temporaryDeliveryOrderClearanceId);
        TemporaryDeliveryOrderClearanceDetail GetObjectById(int Id);
        TemporaryDeliveryOrderClearanceDetail GetObjectByCode(string Code);
        IList<TemporaryDeliveryOrderClearanceDetail> GetObjectsByTemporaryDeliveryOrderDetailId(int temporaryDeliveryOrderDetailId);
        TemporaryDeliveryOrderClearanceDetail CreateObject(TemporaryDeliveryOrderClearanceDetail temporaryDeliveryOrderClearanceDetail, ITemporaryDeliveryOrderClearanceService _temporaryDeliveryOrderClearanceService, 
                                                  IVirtualOrderDetailService _virtualOrderDetailService, ISalesOrderDetailService _salesOrderDetailService,
                                                  IDeliveryOrderService _deliveryOrderService, IItemService _itemService);
        TemporaryDeliveryOrderClearanceDetail UpdateObject(TemporaryDeliveryOrderClearanceDetail temporaryDeliveryOrderClearanceDetail, ITemporaryDeliveryOrderClearanceService _temporaryDeliveryOrderClearanceService,
                                                  IVirtualOrderDetailService _virtualOrderDetailService, ISalesOrderDetailService _salesOrderDetailService,
                                                  IDeliveryOrderService _deliveryOrderService, IItemService _itemService);
        TemporaryDeliveryOrderClearanceDetail SoftDeleteObject(TemporaryDeliveryOrderClearanceDetail temporaryDeliveryOrderClearanceDetail);
        bool DeleteObject(int Id);
        TemporaryDeliveryOrderClearanceDetail ConfirmObject(TemporaryDeliveryOrderClearanceDetail temporaryDeliveryOrderClearanceDetail, DateTime ConfirmationDate, ITemporaryDeliveryOrderClearanceService _temporaryDeliveryOrderClearanceService,
                                                  IVirtualOrderDetailService _virtualOrderDetailService, ISalesOrderDetailService _salesOrderDetailService, IStockMutationService _stockMutationService,
                                                  IItemService _itemService, IBlanketService _blanketService, IWarehouseItemService _warehouseItemService);
        TemporaryDeliveryOrderClearanceDetail UnconfirmObject(TemporaryDeliveryOrderClearanceDetail temporaryDeliveryOrderClearanceDetail, ITemporaryDeliveryOrderClearanceService _temporaryDeliveryOrderClearanceService,
                                                  IVirtualOrderDetailService _virtualOrderDetailService, IVirtualOrderService _virtualOrderService,
                                                  ISalesOrderService _salesOrderService, ISalesOrderDetailService _salesOrderDetailService, IStockMutationService _stockMutationService,
                                                  IItemService _itemService, IBlanketService _blanketService, IWarehouseItemService _warehouseItemService);
        TemporaryDeliveryOrderClearanceDetail ProcessObject(TemporaryDeliveryOrderClearanceDetail temporaryDeliveryOrderClearanceDetail);
    }
}