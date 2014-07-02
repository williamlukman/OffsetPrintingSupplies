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
        RecoveryOrder SoftDeleteObject(RecoveryOrder recoveryOrder, IRecoveryOrderDetailService _recoveryOrderDetailService, IRecoveryAccessoryDetailService _recoveryAccessoryDetailService,
                                       ICoreIdentificationDetailService _coreIdentificationDetailService);
        RecoveryOrder ConfirmObject(RecoveryOrder recoveryOrder, IRecoveryOrderDetailService _recoveryOrderDetailService,
                                    IRecoveryAccessoryDetailService _recoveryAccessoryDetailService, IItemService _itemService);
        RecoveryOrder UnconfirmObject(RecoveryOrder recoveryOrder, IRecoveryOrderDetailService _recoveryOrderDetailService,
                                    IRecoveryAccessoryDetailService _recoveryAccessoryDetailService, IItemService _itemService);
        RecoveryOrder FinishObject(RecoveryOrder recoveryOrder, IRecoveryOrderDetailService _recoveryOrderDetailService,
                                   IRecoveryAccessoryDetailService _recoveryAccessoryDetailService, IItemService _itemService);
        RecoveryOrder UnfinishObject(RecoveryOrder recoveryOrder, IRecoveryOrderDetailService _recoveryOrderDetailService,
                                   IRecoveryAccessoryDetailService _recoveryAccessoryDetailService, IItemService _itemService);
        bool DeleteObject(int Id);
        bool IsCodeDuplicated(RecoveryOrder recoveryOrder);
    }
}