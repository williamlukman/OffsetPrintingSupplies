using Core.DomainModel;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface IReceiptRequestDetailService
    {
        IReceiptRequestDetailValidator GetValidator();
        IQueryable<ReceiptRequestDetail> GetQueryable();
        IList<ReceiptRequestDetail> GetAll();
        IList<ReceiptRequestDetail> GetObjectsByReceiptRequestId(int ReceiptRequestId);
        IList<ReceiptRequestDetail> GetNonLegacyObjectsByReceiptRequestId(int ReceiptRequestId);
        ReceiptRequestDetail GetLegacyObjectByReceiptRequestId(int ReceiptRequestId);
        ReceiptRequestDetail GetObjectById(int Id);
        ReceiptRequestDetail CreateObject(ReceiptRequestDetail ReceiptRequestDetail, IReceiptRequestService _ReceiptRequestService, IAccountService _accountService);
        ReceiptRequestDetail UpdateObject(ReceiptRequestDetail ReceiptRequestDetail, IReceiptRequestService _ReceiptRequestService, IAccountService _accountService);
        ReceiptRequestDetail SoftDeleteObject(ReceiptRequestDetail ReceiptRequestDetail, IReceiptRequestService _ReceiptRequestService);
        bool DeleteObject(int Id);
        ReceiptRequestDetail CreateLegacyObject(ReceiptRequestDetail ReceiptRequestDetail, IReceiptRequestService _ReceiptRequestService, IAccountService _accountService);
        ReceiptRequestDetail UpdateLegacyObject(ReceiptRequestDetail ReceiptRequestDetail, IReceiptRequestService _ReceiptRequestService, IAccountService _accountService);
        ReceiptRequestDetail ConfirmObject(ReceiptRequestDetail ReceiptRequestDetail, DateTime ConfirmationDate, IReceiptRequestService _ReceiptRequestService);
        ReceiptRequestDetail UnconfirmObject(ReceiptRequestDetail ReceiptRequestDetail, IReceiptRequestService _ReceiptRequestService);
    }
}