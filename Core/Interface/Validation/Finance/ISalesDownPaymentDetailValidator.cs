using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.DomainModel;
using Core.Interface.Service;

namespace Core.Interface.Validation
{
    public interface ISalesDownPaymentDetailValidator
    {
        SalesDownPaymentDetail VHasSalesDownPayment(SalesDownPaymentDetail salesDownPaymentDetail, ISalesDownPaymentService _salesDownPaymentService);
        SalesDownPaymentDetail VHasReceivable(SalesDownPaymentDetail salesDownPaymentDetail, IReceivableService _receivableService);
        SalesDownPaymentDetail VHasBeenConfirmed(SalesDownPaymentDetail salesDownPaymentDetail);
        SalesDownPaymentDetail VHasNotBeenConfirmed(SalesDownPaymentDetail salesDownPaymentDetail);
        SalesDownPaymentDetail VHasNotBeenDeleted(SalesDownPaymentDetail salesDownPaymentDetail);
        SalesDownPaymentDetail VReceivableHasNotBeenCompleted(SalesDownPaymentDetail salesDownPaymentDetail, IReceivableService _receivableService);
        SalesDownPaymentDetail VNonNegativeAmount(SalesDownPaymentDetail salesDownPaymentDetail);
        SalesDownPaymentDetail VAmountLessOrEqualReceivable(SalesDownPaymentDetail salesDownPaymentDetail, IReceivableService _receivableService);
        SalesDownPaymentDetail VUniqueReceivableId(SalesDownPaymentDetail salesDownPaymentDetail, ISalesDownPaymentDetailService _salesDownPaymentDetailService, IReceivableService _receivableService);
        SalesDownPaymentDetail VDetailsAmountLessOrEqualSalesDownPaymentTotal(SalesDownPaymentDetail salesDownPaymentDetail, ISalesDownPaymentService _salesDownPaymentService,
                                                                          ISalesDownPaymentDetailService _salesDownPaymentDetailService);

        SalesDownPaymentDetail VCreateObject(SalesDownPaymentDetail salesDownPaymentDetail, ISalesDownPaymentService _salesDownPaymentService, ISalesDownPaymentDetailService _salesDownPaymentDetailService, ICashBankService _cashBankService, IReceivableService _receivableService);
        SalesDownPaymentDetail VUpdateObject(SalesDownPaymentDetail salesDownPaymentDetail, ISalesDownPaymentService _salesDownPaymentService, ISalesDownPaymentDetailService _salesDownPaymentDetailService, ICashBankService _cashBankService, IReceivableService _receivableService);
        SalesDownPaymentDetail VDeleteObject(SalesDownPaymentDetail salesDownPaymentDetail);
        SalesDownPaymentDetail VHasConfirmationDate(SalesDownPaymentDetail salesDownPaymentDetail);
        SalesDownPaymentDetail VConfirmObject(SalesDownPaymentDetail salesDownPaymentDetail, IReceivableService _receivableService);
        SalesDownPaymentDetail VUnconfirmObject(SalesDownPaymentDetail salesDownPaymentDetail);
        bool ValidCreateObject(SalesDownPaymentDetail salesDownPaymentDetail, ISalesDownPaymentService _salesDownPaymentService, ISalesDownPaymentDetailService _salesDownPaymentDetailService, ICashBankService _cashBankService, IReceivableService _receivableService);
        bool ValidUpdateObject(SalesDownPaymentDetail salesDownPaymentDetail, ISalesDownPaymentService _salesDownPaymentService, ISalesDownPaymentDetailService _salesDownPaymentDetailService, ICashBankService _cashBankService, IReceivableService _receivableService);
        bool ValidDeleteObject(SalesDownPaymentDetail salesDownPaymentDetail);
        bool ValidConfirmObject(SalesDownPaymentDetail salesDownPaymentDetail, IReceivableService _receivableService);
        bool ValidUnconfirmObject(SalesDownPaymentDetail salesDownPaymentDetail);
        bool isValid(SalesDownPaymentDetail salesDownPaymentDetail);
        string PrintError(SalesDownPaymentDetail salesDownPaymentDetail);
    }
}
