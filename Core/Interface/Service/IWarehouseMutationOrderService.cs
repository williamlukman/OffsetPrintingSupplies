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
    public interface IWarehouseMutationOrderService
    {
        IWarehouseMutationOrderValidator GetValidator();
        IList<WarehouseMutationOrder> GetAll();
        WarehouseMutationOrder GetObjectById(int Id);
        WarehouseMutationOrder CreateObject(WarehouseMutationOrder warehouseMutationOrder, IWarehouseService _warehouseService);
        WarehouseMutationOrder UpdateObject(WarehouseMutationOrder warehouseMutationOrder, IWarehouseService _warehouseService);
        WarehouseMutationOrder SoftDeleteObject(WarehouseMutationOrder warehouseMutationOrder, IWarehouseService _warehouseService);
        bool DeleteObject(int Id);
    }
}