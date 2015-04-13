using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Repository
{
    public interface IBlanketWarehouseMutationRepository : IRepository<BlanketWarehouseMutation>
    {
        IQueryable<BlanketWarehouseMutation> GetQueryable();
        IList<BlanketWarehouseMutation> GetAll();
        IList<BlanketWarehouseMutation> GetAllByMonthCreated();
        IList<BlanketWarehouseMutation> GetObjectsByBlanketOrderId(int blanketOrderId);
        BlanketWarehouseMutation GetObjectById(int Id);
        Warehouse GetWarehouseFrom(BlanketWarehouseMutation blanketWarehouseMutation);
        Warehouse GetWarehouseTo(BlanketWarehouseMutation blanketWarehouseMutation);
        BlanketWarehouseMutation CreateObject(BlanketWarehouseMutation blanketWarehouseMutation);
        BlanketWarehouseMutation UpdateObject(BlanketWarehouseMutation blanketWarehouseMutation);
        BlanketWarehouseMutation SoftDeleteObject(BlanketWarehouseMutation blanketWarehouseMutation);
        BlanketWarehouseMutation ConfirmObject(BlanketWarehouseMutation blanketWarehouseMutation);
        BlanketWarehouseMutation UnconfirmObject(BlanketWarehouseMutation blanketWarehouseMutation);
        bool DeleteObject(int Id);
        string SetObjectCode();
    }
}