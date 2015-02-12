using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Repository
{
    public interface IReceiptRequestRepository : IRepository<ReceiptRequest>
    {
        IQueryable<ReceiptRequest> GetQueryable();
        IList<ReceiptRequest> GetAll();
        IList<ReceiptRequest> GetAllByMonthCreated();
        ReceiptRequest GetObjectById(int Id);
        IList<ReceiptRequest> GetObjectsByContactId(int contactId);
        ReceiptRequest CreateObject(ReceiptRequest ReceiptRequest);
        ReceiptRequest UpdateObject(ReceiptRequest ReceiptRequest);
        ReceiptRequest SoftDeleteObject(ReceiptRequest ReceiptRequest);
        bool DeleteObject(int Id);
        ReceiptRequest ConfirmObject(ReceiptRequest ReceiptRequest);
        ReceiptRequest UnconfirmObject(ReceiptRequest ReceiptRequest);
        string SetObjectCode();
    }
}