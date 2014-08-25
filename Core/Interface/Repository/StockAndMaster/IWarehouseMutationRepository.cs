using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Repository
{
    public interface IWarehouseMutationRepository : IRepository<WarehouseMutation>
    {
        IQueryable<WarehouseMutation> GetQueryable();
        IList<WarehouseMutation> GetAll();
        IList<WarehouseMutation> GetAllByMonthCreated();
        WarehouseMutation GetObjectById(int Id);
        Warehouse GetWarehouseFrom(WarehouseMutation warehouseMutation);
        Warehouse GetWarehouseTo(WarehouseMutation warehouseMutation);
        WarehouseMutation CreateObject(WarehouseMutation warehouseMutation);
        WarehouseMutation UpdateObject(WarehouseMutation warehouseMutation);
        WarehouseMutation SoftDeleteObject(WarehouseMutation warehouseMutation);
        WarehouseMutation ConfirmObject(WarehouseMutation warehouseMutation);
        WarehouseMutation UnconfirmObject(WarehouseMutation warehouseMutation);
        bool DeleteObject(int Id);
        string SetObjectCode();
    }
}