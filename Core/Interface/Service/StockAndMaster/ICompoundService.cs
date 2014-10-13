using Core.DomainModel;
using Core.Interface.Repository;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface ICompoundService
    {
        ICompoundValidator GetValidator();
        ICompoundRepository GetRepository();
        IQueryable<Compound> GetQueryable();
        IList<Compound> GetAll();
        IList<Compound> GetObjectsByItemTypeId(int ItemTypeId);
        IList<Compound> GetObjectsByUoMId(int UoMId);
        Compound GetObjectById(int Id);
        Compound GetObjectBySku(string Sku);
        Compound CreateObject(Compound compound, ICompoundService _compoundService, IUoMService _uomService, IItemService _itemService, IItemTypeService _itemTypeService,
                              IWarehouseItemService _warehouseItemService, IWarehouseService _warehouseService,
                              IPriceMutationService _priceMutationService, IContactGroupService _contactGroupService);
        Compound UpdateObject(Compound compound, IUoMService _uomService, IItemTypeService _itemTypeService, IItemService _itemService,
                              IPriceMutationService _priceMutationService, IContactGroupService _contactGroupService);
        Compound SoftDeleteObject(Compound compound, IItemTypeService _itemTypeService, IWarehouseItemService _warehouseItemService,
                                  IPriceMutationService _priceMutationService, IPurchaseOrderDetailService _purchaseOrderDetailService,
                                  IStockAdjustmentDetailService _stockAdjustmentDetailService, ISalesOrderDetailService _salesOrderDetailService,
                                  IStockMutationService _stockMutationService);
        Compound AdjustQuantity(Compound compound, int quantity);
        Compound AdjustPendingReceival(Compound compound, int quantity);
        Compound AdjustPendingDelivery(Compound compound, int quantity);
        bool DeleteObject(int Id);
        bool IsSkuDuplicated(Compound compound);
    }
}