using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.DomainModel;
using Core.Interface.Service;

namespace Core.Interface.Validation
{
    public interface ISalesAllowanceDetailValidator
    {
        SalesAllowanceDetail VHasSalesAllowance(SalesAllowanceDetail salesAllowanceDetail, ISalesAllowanceService _salesAllowanceService);
        SalesAllowanceDetail VHasReceivable(SalesAllowanceDetail salesAllowanceDetail, IReceivableService _receivableService);
        SalesAllowanceDetail VHasBeenConfirmed(SalesAllowanceDetail salesAllowanceDetail);
        SalesAllowanceDetail VHasNotBeenConfirmed(SalesAllowanceDetail salesAllowanceDetail);
        SalesAllowanceDetail VHasNotBeenDeleted(SalesAllowanceDetail salesAllowanceDetail);
        SalesAllowanceDetail VReceivableHasNotBeenCompleted(SalesAllowanceDetail salesAllowanceDetail, IReceivableService _receivableService);
        SalesAllowanceDetail VNonNegativeAmount(SalesAllowanceDetail salesAllowanceDetail);
        SalesAllowanceDetail VAmountLessOrEqualReceivable(SalesAllowanceDetail salesAllowanceDetail, IReceivableService _receivableService);
        SalesAllowanceDetail VUniqueReceivableId(SalesAllowanceDetail salesAllowanceDetail, ISalesAllowanceDetailService _salesAllowanceDetailService, IReceivableService _receivableService);
        SalesAllowanceDetail VDetailsAmountLessOrEqualSalesAllowanceTotal(SalesAllowanceDetail salesAllowanceDetail, ISalesAllowanceService _salesAllowanceService,
                                                                          ISalesAllowanceDetailService _salesAllowanceDetailService);

        SalesAllowanceDetail VCreateObject(SalesAllowanceDetail salesAllowanceDetail, ISalesAllowanceService _salesAllowanceService, ISalesAllowanceDetailService _salesAllowanceDetailService, ICashBankService _cashBankService, IReceivableService _receivableService);
        SalesAllowanceDetail VUpdateObject(SalesAllowanceDetail salesAllowanceDetail, ISalesAllowanceService _salesAllowanceService, ISalesAllowanceDetailService _salesAllowanceDetailService, ICashBankService _cashBankService, IReceivableService _receivableService);
        SalesAllowanceDetail VDeleteObject(SalesAllowanceDetail salesAllowanceDetail);
        SalesAllowanceDetail VHasConfirmationDate(SalesAllowanceDetail salesAllowanceDetail);
        SalesAllowanceDetail VConfirmObject(SalesAllowanceDetail salesAllowanceDetail, IReceivableService _receivableService);
        SalesAllowanceDetail VUnconfirmObject(SalesAllowanceDetail salesAllowanceDetail);
        bool ValidCreateObject(SalesAllowanceDetail salesAllowanceDetail, ISalesAllowanceService _salesAllowanceService, ISalesAllowanceDetailService _salesAllowanceDetailService, ICashBankService _cashBankService, IReceivableService _receivableService);
        bool ValidUpdateObject(SalesAllowanceDetail salesAllowanceDetail, ISalesAllowanceService _salesAllowanceService, ISalesAllowanceDetailService _salesAllowanceDetailService, ICashBankService _cashBankService, IReceivableService _receivableService);
        bool ValidDeleteObject(SalesAllowanceDetail salesAllowanceDetail);
        bool ValidConfirmObject(SalesAllowanceDetail salesAllowanceDetail, IReceivableService _receivableService);
        bool ValidUnconfirmObject(SalesAllowanceDetail salesAllowanceDetail);
        bool isValid(SalesAllowanceDetail salesAllowanceDetail);
        string PrintError(SalesAllowanceDetail salesAllowanceDetail);
    }
}
