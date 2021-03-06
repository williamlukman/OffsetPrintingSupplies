﻿using Core.DomainModel;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Validation
{
    public interface IRollerWarehouseMutationDetailValidator
    {
        RollerWarehouseMutationDetail VHasRecoveryOrderDetail(RollerWarehouseMutationDetail rollerWarehouseMutationDetail, IRecoveryOrderDetailService _recoveryOrderDetailService);
        RollerWarehouseMutationDetail VCoreIdentificationDetailHasNotBeenDelivered(RollerWarehouseMutationDetail rollerWarehouseMutationDetail, IRecoveryOrderDetailService _recoveryOrderDetailService,
                                                                                   ICoreIdentificationDetailService _coreIdentificationDetailService);
        RollerWarehouseMutationDetail VRecoveryOrderDetailHasBeenFinished(RollerWarehouseMutationDetail rollerWarehouseMutationDetail, IRecoveryOrderDetailService _recoveryOrderDetailService);
        RollerWarehouseMutationDetail VHasRollerWarehouseMutation(RollerWarehouseMutationDetail rollerWarehouseMutationDetail, IRollerWarehouseMutationService _rollerWarehouseMutationService);
        RollerWarehouseMutationDetail VHasWarehouseItemFrom(RollerWarehouseMutationDetail rollerWarehouseMutationDetail, IRollerWarehouseMutationService _rollerWarehouseMutationService, IWarehouseItemService _warehouseItemService);
        RollerWarehouseMutationDetail VHasWarehouseItemTo(RollerWarehouseMutationDetail rollerWarehouseMutationDetail, IRollerWarehouseMutationService _rollerWarehouseMutationService, IWarehouseItemService _warehouseItemService);
        RollerWarehouseMutationDetail VUniqueRecoveryOrderDetail(RollerWarehouseMutationDetail rollerWarehouseMutationDetail, IRollerWarehouseMutationDetailService _rollerWarehouseMutationDetailService);
        RollerWarehouseMutationDetail VNonNegativeStockQuantity(RollerWarehouseMutationDetail rollerWarehouseMutationDetail, IRecoveryOrderService _recoveryOrderService, ICoreIdentificationService _coreIdentificationService,
                                                                IRollerWarehouseMutationService _rollerWarehouseMutationService, IItemService _itemService, IBlanketService _blanketService,
                                                                IWarehouseItemService _warehouseItemService, ICustomerItemService _customerItemService, bool CaseConfirm);
        RollerWarehouseMutationDetail VRollerWarehouseMutationHasBeenConfirmed(RollerWarehouseMutationDetail rollerWarehouseMutationDetail, IRollerWarehouseMutationService _rollerWarehouseMutationService);
        RollerWarehouseMutationDetail VRollerWarehouseMutationHasNotBeenConfirmed(RollerWarehouseMutationDetail rollerWarehouseMutationDetail, IRollerWarehouseMutationService _rollerWarehouseMutationService);
        RollerWarehouseMutationDetail VHasNotBeenConfirmed(RollerWarehouseMutationDetail rollerWarehouseMutationDetail);
        RollerWarehouseMutationDetail VHasBeenConfirmed(RollerWarehouseMutationDetail rollerWarehouseMutationDetail);
        RollerWarehouseMutationDetail VCreateObject(RollerWarehouseMutationDetail rollerWarehouseMutationDetail, IRollerWarehouseMutationService _rollerWarehouseMutationService,
                                                    IRecoveryOrderDetailService _recoveryOrderDetailService, ICoreIdentificationDetailService _coreIdentificationDetailService,
                                                    IRollerWarehouseMutationDetailService _rollerWarehouseMutationDetailService, IItemService _itemService, IWarehouseItemService _warehouseItemService);
        RollerWarehouseMutationDetail VUpdateObject(RollerWarehouseMutationDetail rollerWarehouseMutationDetail, IRollerWarehouseMutationService _rollerWarehouseMutationService,
                                                    IRecoveryOrderDetailService _recoveryOrderDetailService, ICoreIdentificationDetailService _coreIdentificationDetailService,
                                                    IRollerWarehouseMutationDetailService _rollerWarehouseMutationDetailService, IItemService _itemService, IWarehouseItemService _warehouseItemService);
        RollerWarehouseMutationDetail VDeleteObject(RollerWarehouseMutationDetail rollerWarehouseMutationDetail);
        RollerWarehouseMutationDetail VHasConfirmationDate(RollerWarehouseMutationDetail rollerWarehouseMutationDetail);
        RollerWarehouseMutationDetail VConfirmObject(RollerWarehouseMutationDetail rollerWarehouseMutationDetail, IRecoveryOrderService _recoveryOrderService, ICoreIdentificationService _coreIdentificationService, 
                                                     IRollerWarehouseMutationService _rollerWarehouseMutationService, IItemService _itemService, IBlanketService _blanketService,
                                                     IWarehouseItemService _warehouseItemService, ICustomerItemService _customerItemService);
        RollerWarehouseMutationDetail VUnconfirmObject(RollerWarehouseMutationDetail rollerWarehouseMutationDetail, IRecoveryOrderService _recoveryOrderService, ICoreIdentificationService _coreIdentificationService,
                                                       IRollerWarehouseMutationService _rollerWarehouseMutationService, IItemService _itemService, IBlanketService _blanketService,
                                                       IWarehouseItemService _warehouseItemService, ICustomerItemService _customerItemService);
        bool ValidCreateObject(RollerWarehouseMutationDetail rollerWarehouseMutationDetail, IRollerWarehouseMutationService _rollerWarehouseMutationService,
                               IRecoveryOrderDetailService _recoveryOrderDetailService, ICoreIdentificationDetailService _coreIdentificationDetailService,
                               IRollerWarehouseMutationDetailService _rollerWarehouseMutationDetailService, IItemService _itemService, IWarehouseItemService _warehouseItemService);
        bool ValidUpdateObject(RollerWarehouseMutationDetail rollerWarehouseMutationDetail, IRollerWarehouseMutationService _rollerWarehouseMutationService,
                               IRecoveryOrderDetailService _recoveryOrderDetailService, ICoreIdentificationDetailService _coreIdentificationDetailService,
                               IRollerWarehouseMutationDetailService _rollerWarehouseMutationDetailService, IItemService _itemService, IWarehouseItemService _warehouseItemService);
        bool ValidConfirmObject(RollerWarehouseMutationDetail rollerWarehouseMutationDetail, IRecoveryOrderService _recoveryOrderService, ICoreIdentificationService _coreIdentificationService,
                                IRollerWarehouseMutationService _rollerWarehouseMutationService, IItemService _itemService, IBlanketService _blanketService,
                                IWarehouseItemService _warehouseItemService, ICustomerItemService _customerItemService);
        bool ValidUnconfirmObject(RollerWarehouseMutationDetail rollerWarehouseMutationDetail, IRecoveryOrderService _recoveryOrderService, ICoreIdentificationService _coreIdentificationService,
                                  IRollerWarehouseMutationService _rollerWarehouseMutationService, IItemService _itemService, IBlanketService _blanketService,
                                  IWarehouseItemService _warehouseItemService, ICustomerItemService _customerItemService);
        bool ValidDeleteObject(RollerWarehouseMutationDetail rollerWarehouseMutationDetail);
        bool isValid(RollerWarehouseMutationDetail rollerWarehouseMutationDetail);
        string PrintError(RollerWarehouseMutationDetail rollerWarehouseMutationDetail);
    }
}