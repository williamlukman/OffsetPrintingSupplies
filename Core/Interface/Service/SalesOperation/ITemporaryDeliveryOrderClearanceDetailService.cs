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
                                                  ITemporaryDeliveryOrderDetailService _temporaryDeliveryOrderDetailService);
        TemporaryDeliveryOrderClearanceDetail UpdateObject(TemporaryDeliveryOrderClearanceDetail temporaryDeliveryOrderClearanceDetail, ITemporaryDeliveryOrderClearanceService _temporaryDeliveryOrderClearanceService,
                                                  ITemporaryDeliveryOrderDetailService _temporaryDeliveryOrderDetailService);
        TemporaryDeliveryOrderClearanceDetail SoftDeleteObject(TemporaryDeliveryOrderClearanceDetail temporaryDeliveryOrderClearanceDetail);
        bool DeleteObject(int Id);
        TemporaryDeliveryOrderClearanceDetail ConfirmObject(TemporaryDeliveryOrderClearanceDetail temporaryDeliveryOrderClearanceDetail, DateTime ConfirmationDate, ITemporaryDeliveryOrderClearanceService _temporaryDeliveryOrderClearanceService,
                                                  IStockMutationService _stockMutationService, IItemService _itemService, IBlanketService _blanketService, IWarehouseItemService _warehouseItemService, ITemporaryDeliveryOrderDetailService _temporaryDeliveryOrderDetailService);
        TemporaryDeliveryOrderClearanceDetail UnconfirmObject(TemporaryDeliveryOrderClearanceDetail temporaryDeliveryOrderClearanceDetail, ITemporaryDeliveryOrderClearanceService _temporaryDeliveryOrderClearanceService,
                                                  IStockMutationService _stockMutationService, IItemService _itemService, IBlanketService _blanketService, IWarehouseItemService _warehouseItemService, ITemporaryDeliveryOrderDetailService _temporaryDeliveryOrderDetailService);
        TemporaryDeliveryOrderClearanceDetail ProcessObject(TemporaryDeliveryOrderClearanceDetail temporaryDeliveryOrderClearanceDetail);
    }
}