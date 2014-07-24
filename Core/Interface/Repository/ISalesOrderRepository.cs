using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interface.Repository
{
    public interface ISalesOrderRepository : IRepository<SalesOrder>
    {
        IList<SalesOrder> GetAll();
        SalesOrder GetObjectById(int Id);
        IList<SalesOrder> GetObjectsByCustomerId(int customerId);
        SalesOrder CreateObject(SalesOrder salesOrder);
        SalesOrder UpdateObject(SalesOrder salesOrder);
        SalesOrder SoftDeleteObject(SalesOrder salesOrder);
        bool DeleteObject(int Id);
        SalesOrder ConfirmObject(SalesOrder salesOrder);
        SalesOrder UnconfirmObject(SalesOrder salesOrder);
        SalesOrder CompleteObject(SalesOrder salesOrder);
        string SetObjectCode();
    }
}