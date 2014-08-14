using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Repository
{
    public interface IWarehouseMutationOrderRepository : IRepository<WarehouseMutationOrder>
    {
        IList<WarehouseMutationOrder> GetAll();
        IList<WarehouseMutationOrder> GetAllByMonthCreated();
        WarehouseMutationOrder GetObjectById(int Id);
        Warehouse GetWarehouseFrom(WarehouseMutationOrder warehouseMutationOrder);
        Warehouse GetWarehouseTo(WarehouseMutationOrder warehouseMutationOrder);
        WarehouseMutationOrder CreateObject(WarehouseMutationOrder warehouseMutationOrder);
        WarehouseMutationOrder UpdateObject(WarehouseMutationOrder warehouseMutationOrder);
        WarehouseMutationOrder SoftDeleteObject(WarehouseMutationOrder warehouseMutationOrder);
        WarehouseMutationOrder ConfirmObject(WarehouseMutationOrder warehouseMutationOrder);
        WarehouseMutationOrder UnconfirmObject(WarehouseMutationOrder warehouseMutationOrder);
        bool DeleteObject(int Id);
        string SetObjectCode();
    }
}