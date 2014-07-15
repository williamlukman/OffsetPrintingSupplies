using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interface.Repository
{
    public interface IBarringOrderRepository : IRepository<BarringOrder>
    {
        IList<BarringOrder> GetAll();
        IList<BarringOrder> GetAllObjectsByCustomerId(int CustomerId);
        IList<BarringOrder> GetAllObjectsByWarehouseId(int WarehouseId);
        BarringOrder GetObjectById(int Id);
        BarringOrder CreateObject(BarringOrder barringOrder);
        BarringOrder UpdateObject(BarringOrder barringOrder);
        BarringOrder SoftDeleteObject(BarringOrder barringOrder);
        BarringOrder ConfirmObject(BarringOrder barringOrder);
        BarringOrder UnconfirmObject(BarringOrder barringOrder);
        BarringOrder FinishObject(BarringOrder barringOrder);
        BarringOrder UnfinishObject(BarringOrder barringOrder);
        bool DeleteObject(int Id);
        bool IsCodeDuplicated(BarringOrder barringOrder);
    }
}