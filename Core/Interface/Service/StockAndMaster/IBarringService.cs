﻿using Core.DomainModel;
using Core.Interface.Repository;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface IBarringService
    {
        IBarringValidator GetValidator();
        IBarringRepository GetRepository();
        IQueryable<Barring> GetQueryable();
        IList<Barring> GetAll();
        IList<Barring> GetObjectsByItemTypeId(int ItemTypeId);
        IList<Barring> GetObjectsByUoMId(int UoMId);
        IList<Barring> GetObjectsByMachineId(int machineId);
        IList<Barring> GetObjectsByContactId(int contactId);
        Barring GetObjectById(int Id);
        Barring GetObjectBySku(string Sku);
        Barring CreateObject(Barring barring, IBarringService _barringService, IUoMService _uomService, IItemService _itemService, IItemTypeService _itemTypeService,
                                    IContactService _contactService, IMachineService _machineService,
                                    IWarehouseItemService _warehouseItemService, IWarehouseService _warehouseService,
                                    IPriceMutationService _priceMutationService, IContactGroupService _contactGroupService);
        Barring UpdateObject(Barring barring, IBarringService _barringService, IUoMService _uomService, IItemService _itemService,
                               IItemTypeService _itemTypeService, IContactService _contactService, IMachineService _machineService,
                               IWarehouseItemService _warehouseItemService, IWarehouseService _warehouseService,
                               IContactGroupService _contactGroupService, IPriceMutationService _priceMutationService);
        Barring SoftDeleteObject(Barring barring, IItemTypeService _itemTypeService, IWarehouseItemService _warehouseItemService, IPriceMutationService _priceMutationService);
        Barring AdjustQuantity(Barring barring, int quantity);
        Barring AdjustPendingReceival(Barring barring, int quantity);
        Barring AdjustPendingDelivery(Barring barring, int quantity);
        bool DeleteObject(int Id);
        bool IsSkuDuplicated(Barring barring);
    }
}