using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interface.Repository
{
    public interface IWarehouseMutationOrderDetailRepository : IRepository<WarehouseMutationOrderDetail>
    {
        IList<WarehouseMutationOrderDetail> GetAll();
        IList<WarehouseMutationOrderDetail> GetObjectsByWarehouseMutationOrderId(int warehouseMutationOrderId);
        WarehouseMutationOrderDetail GetObjectById(int Id);
        WarehouseMutationOrderDetail CreateObject(WarehouseMutationOrderDetail warehouseMutationOrderDetail);
        WarehouseMutationOrderDetail UpdateObject(WarehouseMutationOrderDetail warehouseMutationOrderDetail);
        WarehouseMutationOrderDetail SoftDeleteObject(WarehouseMutationOrderDetail warehouseMutationOrderDetail);
        bool DeleteObject(int Id);
    }
}