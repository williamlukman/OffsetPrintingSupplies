﻿using Core.DomainModel;
using Core.Interface.Repository;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interface.Service
{
    public interface IRecoveryAccessoryDetailService
    {
        IRecoveryAccessoryDetailValidator GetValidator();
        IRecoveryAccessoryDetailRepository GetRepository();
        IList<RecoveryAccessoryDetail> GetAll();
        IList<RecoveryAccessoryDetail> GetObjectsByRecoveryOrderDetailId(int recoveryOrderDetailId);
        IList<RecoveryAccessoryDetail> GetObjectsByItemId(int ItemId);
        RecoveryAccessoryDetail GetObjectById(int Id);
        RecoveryAccessoryDetail CreateObject(RecoveryAccessoryDetail recoveryAccessoryDetail, IRecoveryOrderDetailService _recoveryOrderDetailService,
                                             IItemService _itemService, IItemTypeService _itemTypeService);
        RecoveryAccessoryDetail CreateObject(int RecoveryOrderDetailId, int ItemId, int Quantity, IRecoveryOrderDetailService _recoveryOrderDetailService,
                                             IItemService _itemService, IItemTypeService _itemTypeService);
        RecoveryAccessoryDetail UpdateObject(RecoveryAccessoryDetail recoveryAccessoryDetail, IRecoveryOrderDetailService _recoveryOrderDetailService,
                                             IItemService _itemService, IItemTypeService _itemTypeService);
        RecoveryAccessoryDetail SoftDeleteObject(RecoveryAccessoryDetail recoveryAccessoryDetail);
        RecoveryAccessoryDetail ConfirmObject(RecoveryAccessoryDetail recoveryAccessoryDetail, IRecoveryOrderService _recoveryOrderService,
                                              IRecoveryOrderDetailService _recoveryOrderDetailService, IItemService _itemService);
        RecoveryAccessoryDetail UnconfirmObject(RecoveryAccessoryDetail recoveryAccessoryDetail, IRecoveryOrderService _recoveryOrderService,
                                                IRecoveryOrderDetailService _recoveryOrderDetailService, IItemService _itemService);
        bool DeleteObject(int Id);
    }
}