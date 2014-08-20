﻿using Core.DomainModel;
using Core.Interface.Repository;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface IRollerWarehouseMutationService
    {
        IQueryable<RollerWarehouseMutation> GetQueryable();
        IRollerWarehouseMutationValidator GetValidator();
        IList<RollerWarehouseMutation> GetAll();
        IList<RollerWarehouseMutation> GetObjectsByCoreIdentificationId(int coreIdentificationId);
        RollerWarehouseMutation GetObjectById(int Id);
        Warehouse GetWarehouseFrom(RollerWarehouseMutation rollerWarehouseMutation);
        Warehouse GetWarehouseTo(RollerWarehouseMutation rollerWarehouseMutation);
        RollerWarehouseMutation CreateObject(RollerWarehouseMutation rollerWarehouseMutation, IWarehouseService _warehouseService, ICoreIdentificationService _coreIdentificationService);
        RollerWarehouseMutation UpdateObject(RollerWarehouseMutation rollerWarehouseMutation, IWarehouseService _warehouseService, ICoreIdentificationService _coreIdentificationService);
        RollerWarehouseMutation SoftDeleteObject(RollerWarehouseMutation rollerWarehouseMutation);
        bool DeleteObject(int Id);
        RollerWarehouseMutation ConfirmObject(RollerWarehouseMutation rollerWarehouseMutation, DateTime ConfirmationDate, IRollerWarehouseMutationDetailService _rollerWarehouseMutationDetailService,
                                              IItemService _itemService, IBarringService _barringService, IWarehouseItemService _warehouseItemService, IStockMutationService _stockMutationService,
                                              ICoreIdentificationDetailService _coreIdentificationDetailService, ICoreIdentificationService _coreIdentificationService);
        RollerWarehouseMutation UnconfirmObject(RollerWarehouseMutation rollerWarehouseMutation, IRollerWarehouseMutationDetailService _rollerWarehouseMutationDetailService,
                                               IItemService _itemService, IBarringService _barringService, IWarehouseItemService _warehouseItemService, IStockMutationService _stockMutationService,
                                               ICoreIdentificationDetailService _coreIdentificationDetailService, ICoreIdentificationService _coreIdentificationService);
    }
}