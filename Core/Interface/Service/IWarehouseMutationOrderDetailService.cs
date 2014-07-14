using Core.DomainModel;
using Core.Interface.Repository;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interface.Service
{
    public interface IWarehouseMutationOrderDetailService
    {
        IWarehouseMutationOrderDetailValidator GetValidator();
        IList<WarehouseMutationOrderDetail> GetAll();
        IList<WarehouseMutationOrderDetail> GetObjectsByWarehouseMutationOrderId(int warehouseMutationOrderId);
        WarehouseMutationOrderDetail GetObjectById(int Id);
        WarehouseMutationOrderDetail CreateObject(WarehouseMutationOrderDetail abstractItem, IItemTypeService _itemTypeService);
        WarehouseMutationOrderDetail UpdateObject(WarehouseMutationOrderDetail abstractItem, IItemTypeService _itemTypeService);
        WarehouseMutationOrderDetail SoftDeleteObject(WarehouseMutationOrderDetail abstractItem);
        WarehouseMutationOrderDetail AdjustQuantity(WarehouseMutationOrderDetail abstractItem, int Quantity);
        bool DeleteObject(int Id);
    }
}