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
        SalesDownPaymentAllocationDetail GetObjectByReceiptVoucherDetailId(int receiptVoucherDetailId);
        SalesDownPaymentAllocationDetail GetObjectById(int Id);
        SalesDownPaymentAllocationDetail CreateObject(SalesDownPaymentAllocationDetail salesDownPaymentAllocationDetail, ISalesDownPaymentAllocationService _salesDownPaymentAllocationService,
                                                      ISalesDownPaymentService _salesDownPaymentService, IReceiptVoucherDetailService _receiptVoucherDetailService, IReceivableService _receivableService,
                                                      IReceiptVoucherService _receiptVoucherService, ICashBankService _cashBankService);
        SalesDownPaymentAllocationDetail UpdateObject(SalesDownPaymentAllocationDetail salesDownPaymentAllocationDetail, ISalesDownPaymentAllocationService _salesDownPaymentAllocationService,
                                                      ISalesDownPaymentService _salesDownPaymentService, IReceiptVoucherDetailService _receiptVoucherDetailService, IReceivableService _receivableService,
                                                      IReceiptVoucherService _receiptVoucherService, ICashBankService _cashBankService);
        SalesDownPaymentAllocationDetail SoftDeleteObject(SalesDownPaymentAllocationDetail salesDownPaymentAllocationDetail,
                                                          IReceiptVoucherDetailService _receiptVoucherDetailService);
        bool DeleteObject(int Id);
        SalesDownPaymentAllocationDetail ConfirmObject(SalesDownPaymentAllocationDetail salesDownPaymentAllocationDetail, DateTime ConfirmationDate,
                                                       ISalesDownPaymentAllocationService _salesDownPaymentAllocationService, ISalesDownPaymentService _salesDownPaymentService,
                                                       IReceiptVoucherDetailService _receiptVoucherDetailService, IReceivableService _receivableService);
        SalesDownPaymentAllocationDetail UnconfirmObject(SalesDownPaymentAllocationDetail salesDownPaymentAllocationDetail,
                                                         ISalesDownPaymentAllocationService _salesDownPaymentAllocationService, ISalesDownPaymentService _salesDownPaymentService,
                                                         IReceiptVoucherDetailService _receiptVoucherDetailService, IReceivableService _receivableService);
    }
}