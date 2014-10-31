using Core.DomainModel;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface ISalesDownPaymentAllocationDetailService
    {
        ISalesDownPaymentAllocationDetailValidator GetValidator();
        IQueryable<SalesDownPaymentAllocationDetail> GetQueryable();
        IList<SalesDownPaymentAllocationDetail> GetAll();
        IList<SalesDownPaymentAllocationDetail> GetObjectsBySalesDownPaymentAllocationId(int salesDownPaymentAllocationId);
        IList<SalesDownPaymentAllocationDetail> GetObjectsByReceivableId(int receivableId);
        SalesDownPaymentAllocationDetail GetObjectById(int Id);
        SalesDownPaymentAllocationDetail CreateObject(SalesDownPaymentAllocationDetail salesDownPaymentAllocationDetail, ISalesDownPaymentAllocationService _salesDownPaymentAllocationService,
                                                      ISalesDownPaymentService _salesDownPaymentService, IReceivableService _receivableService, IPayableService _payableService);
        SalesDownPaymentAllocationDetail UpdateObject(SalesDownPaymentAllocationDetail salesDownPaymentAllocationDetail, ISalesDownPaymentAllocationService _salesDownPaymentAllocationService,
                                                      ISalesDownPaymentService _salesDownPaymentService, IReceivableService _receivableService, IPayableService _payableService);
        SalesDownPaymentAllocationDetail SoftDeleteObject(SalesDownPaymentAllocationDetail salesDownPaymentAllocationDetail);
        bool DeleteObject(int Id);
        SalesDownPaymentAllocationDetail ConfirmObject(SalesDownPaymentAllocationDetail salesDownPaymentAllocationDetail, DateTime ConfirmationDate,
                                                       ISalesDownPaymentAllocationService _salesDownPaymentAllocationService, ISalesDownPaymentService _salesDownPaymentService,
                                                       IReceivableService _receivableService, IPayableService _payableService);
        SalesDownPaymentAllocationDetail UnconfirmObject(SalesDownPaymentAllocationDetail salesDownPaymentAllocationDetail,
                                                         ISalesDownPaymentAllocationService _salesDownPaymentAllocationService, ISalesDownPaymentService _salesDownPaymentService,
                                                         IReceivableService _receivableService, IPayableService _payableService);
    }
}