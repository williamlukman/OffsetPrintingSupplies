using Core.DomainModel;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interface.Service
{
    public interface IRecoveryOrderService
    {
        IRecoveryOrderValidator GetValidator();
        IList<RecoveryOrder> GetAll();
        IList<RecoveryOrder> GetAllObjectsInHouse();
        IList<RecoveryOrder> GetAllObjectsByCustomerId(int CustomerId);
        IList<RecoveryOrder> GetObjectsByCoreIdentificationId(int coreIdentificationId);
        RecoveryOrder GetObjectById(int Id);
        RecoveryOrder CreateObject(RecoveryOrder recoveryOrder, ICoreIdentificationService _coreIdentificationService);
        RecoveryOrder UpdateObject(RecoveryOrder recoveryOrder, IRecoveryOrderDetailService _recoveryOrderDetailService, ICoreIdentificationService _coreIdentificationService);
        RecoveryOrder SoftDeleteObject(RecoveryOrder recoveryOrder, IRecoveryOrderDetailService _recoveryOrderDetailService,
                                       IRecoveryAccessoryDetailService _recoveryAccessoryDetailService);
        RecoveryOrder ConfirmObject(RecoveryOrder recoveryOrder, ICoreIdentificationDetailService _coreIdentificationDetailService, IRecoveryOrderDetailService _recoveryOrderDetailService,
                                    IRecoveryAccessoryDetailService _recoveryAccessoryDetailService, ICoreBuilderService _coreBuilderService, IStockMutationService _stockMutationService,
                                    IItemService _itemService, IBarringService _barringService, IWarehouseItemService _warehouseItemService, IWarehouseService _warehouseService);
        RecoveryOrder UnconfirmObject(RecoveryOrder recoveryOrder, ICoreIdentificationDetailService _coreIdentificationDetailService, IRecoveryOrderDetailService _recoveryOrderDetailService,
                                      IRecoveryAccessoryDetailService _recoveryAccessoryDetailService, ICoreBuilderService _coreBuilderService, IStockMutationService _stockMutationService,
                                      IItemService _itemService, IBarringService _barringService, IWarehouseItemService _warehouseItemService, IWarehouseService _warehouseService);
        RecoveryOrder FinishObject(RecoveryOrder recoveryOrder, ICoreIdentificationDetailService _coreIdentificationDetailService, IRecoveryOrderDetailService _recoveryOrderDetailService,
                                   IRecoveryAccessoryDetailService _recoveryAccessoryDetailService, ICoreBuilderService _coreBuilderService, IRollerBuilderService _rollerBuilderService,
                                    IItemService _itemService, IWarehouseItemService _warehouseItemService, IBarringService _barringService, IStockMutationService _stockMutationService);
        RecoveryOrder UnfinishObject(RecoveryOrder recoveryOrder, ICoreIdentificationDetailService _coreIdentificationDetailService, IRecoveryOrderDetailService _recoveryOrderDetailService,
                                   IRecoveryAccessoryDetailService _recoveryAccessoryDetailService, ICoreBuilderService _coreBuilderService, IRollerBuilderService _rollerBuilderService,
                                    IItemService _itemService, IWarehouseItemService _warehouseItemService, IBarringService _barringService, IStockMutationService _stockMutationService);
        void StockMutateObject(StockMutation stockMutation, IItemService _itemService, IBarringService _barringService, IWarehouseItemService _warehouseItemService);
        void ReverseStockMutateObject(StockMutation stockMutation, IItemService _itemService, IBarringService _barringService, IWarehouseItemService _warehouseItemService);
        bool DeleteObject(int Id);
        bool IsCodeDuplicated(RecoveryOrder recoveryOrder);
    }
}