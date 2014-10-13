using Core.DomainModel;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface ISalesDownPaymentDetailService
    {
        ISalesDownPaymentDetailValidator GetValidator();
        IQueryable<SalesDownPaymentDetail> GetQueryable();
        IList<SalesDownPaymentDetail> GetAll();
        IList<SalesDownPaymentDetail> GetObjectsBySalesDownPaymentId(int salesDownPaymentId);
        IList<SalesDownPaymentDetail> GetObjectsByReceivableId(int receivableId);
        SalesDownPaymentDetail GetObjectById(int Id);        
        SalesDownPaymentDetail CreateObject(SalesDownPaymentDetail salesDownPaymentDetail, ISalesDownPaymentService _salesDownPaymentService,
                                          ICashBankService _cashBankService, IReceivableService _receivableService);
        SalesDownPaymentDetail UpdateObject(SalesDownPaymentDetail salesDownPaymentDetail, ISalesDownPaymentService _salesDownPaymentService,
                                          ICashBankService _cashBankService, IReceivableService _receivableService);
        SalesDownPaymentDetail SoftDeleteObject(SalesDownPaymentDetail salesDownPaymentDetail);
        bool DeleteObject(int Id);
        SalesDownPaymentDetail ConfirmObject(SalesDownPaymentDetail salesDownPaymentDetail, DateTime ConfirmationDate, ISalesDownPaymentService _salesDownPaymentService, IReceivableService _receivableService);
        SalesDownPaymentDetail UnconfirmObject(SalesDownPaymentDetail salesDownPaymentDetail, ISalesDownPaymentService _salesDownPaymentService, IReceivableService _receivableService);
    }
}