using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Repository
{
    public interface IReceiptRequestDetailRepository : IRepository<ReceiptRequestDetail>
    {
        IQueryable<ReceiptRequestDetail> GetQueryable();
        IList<ReceiptRequestDetail> GetAll();
        IList<ReceiptRequestDetail> GetAllByMonthCreated();
        IList<ReceiptRequestDetail> GetObjectsByReceiptRequestId(int ReceiptRequestId);
        IList<ReceiptRequestDetail> GetNonLegacyObjectsByReceiptRequestId(int ReceiptRequestId);
        ReceiptRequestDetail GetLegacyObjectByReceiptRequestId(int ReceiptRequestId);
        ReceiptRequestDetail GetObjectById(int Id);
        ReceiptRequestDetail CreateObject(ReceiptRequestDetail ReceiptRequestDetail);
        ReceiptRequestDetail UpdateObject(ReceiptRequestDetail ReceiptRequestDetail);
        ReceiptRequestDetail SoftDeleteObject(ReceiptRequestDetail ReceiptRequestDetail);
        bool DeleteObject(int Id);
        ReceiptRequestDetail ConfirmObject(ReceiptRequestDetail ReceiptRequestDetail);
        ReceiptRequestDetail UnconfirmObject(ReceiptRequestDetail ReceiptRequestDetail);
        string SetObjectCode(string ParentCode);
    }
}