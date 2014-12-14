using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.DomainModel;
using Core.Interface.Service;

namespace Core.Interface.Validation
{
    public interface ISalesDownPaymentAllocationDetailValidator
    {
        SalesDownPaymentAllocationDetail VHasSalesDownPaymentAllocation(SalesDownPaymentAllocationDetail salesDownPaymentAllocationDetail,
                                            ISalesDownPaymentAllocationService _salesDownPaymentAllocationService);
        SalesDownPaymentAllocationDetail VHasReceivable(SalesDownPaymentAllocationDetail salesDownPaymentAllocationDetail, IReceivableService _receivableService);
        SalesDownPaymentAllocationDetail VHasBeenConfirmed(SalesDownPaymentAllocationDetail salesDownPaymentAllocationDetail);
        SalesDownPaymentAllocationDetail VHasNotBeenConfirmed(SalesDownPaymentAllocationDetail salesDownPaymentAllocationDetail);
        SalesDownPaymentAllocationDetail VHasNotBeenDeleted(SalesDownPaymentAllocationDetail salesDownPaymentAllocationDetail);
        SalesDownPaymentAllocationDetail VReceivableHasNotBeenCompleted(SalesDownPaymentAllocationDetail salesDownPaymentAllocationDetail, IReceivableService _receivableService);
        SalesDownPaymentAllocationDetail VNonNegativeAmount(SalesDownPaymentAllocationDetail salesDownPaymentAllocationDetail);
        SalesDownPaymentAllocationDetail VAmountLessOrEqualReceivable(SalesDownPaymentAllocationDetail salesDownPaymentAllocationDetail, IReceivableService _receivableService);
        SalesDownPaymentAllocationDetail VUniqueReceivableId(SalesDownPaymentAllocationDetail salesDownPaymentAllocationDetail,
                                            ISalesDownPaymentAllocationDetailService _salesDownPaymentAllocationDetailService, IReceivableService _receivableService);
        SalesDownPaymentAllocationDetail VDetailsAmountLessOrEqualSalesDownPaymentTotal(SalesDownPaymentAllocationDetail salesDownPaymentAllocationDetail,
                                         ISalesDownPaymentAllocationService _salesDownPaymentAllocationService, ISalesDownPaymentAllocationDetailService _salesDownPaymentAllocationDetailService,
                                         IPayableService _payableService);
        SalesDownPaymentAllocationDetail VCreateObject(SalesDownPaymentAllocationDetail salesDownPaymentAllocationDetail, ISalesDownPaymentAllocationService _salesDownPaymentAllocationService,
                                                       ISalesDownPaymentAllocationDetailService _salesDownPaymentAllocationDetailService, ISalesDownPaymentService _salesDownPaymentService,
                                                       IReceivableService _receivableService, IPayableService _payableService);
        SalesDownPaymentAllocationDetail VUpdateObject(SalesDownPaymentAllocationDetail salesDownPaymentAllocationDetail, ISalesDownPaymentAllocationService _salesDownPaymentAllocationService,
                                                       ISalesDownPaymentAllocationDetailService _salesDownPaymentAllocationDetailService, ISalesDownPaymentService _salesDownPaymentService,
                                                       IReceivableService _receivableService, IPayableService _payableService);
        SalesDownPaymentAllocationDetail VDeleteObject(SalesDownPaymentAllocationDetail salesDownPaymentAllocationDetail);
        SalesDownPaymentAllocationDetail VHasConfirmationDate(SalesDownPaymentAllocationDetail salesDownPaymentAllocationDetail);
        SalesDownPaymentAllocationDetail VConfirmObject(SalesDownPaymentAllocationDetail salesDownPaymentAllocationDetail,
                                                        IReceivableService _receivableService, IPayableService _payableService);
        SalesDownPaymentAllocationDetail VUnconfirmObject(SalesDownPaymentAllocationDetail salesDownPaymentAllocationDetail,
                                                          IReceivableService _receivableService, IPayableService _payableService);
        bool ValidCreateObject(SalesDownPaymentAllocationDetail salesDownPaymentAllocationDetail, ISalesDownPaymentAllocationService _salesDownPaymentAllocationService,
                               ISalesDownPaymentAllocationDetailService _salesDownPaymentAllocationDetailService, ISalesDownPaymentService _salesDownPaymentService,
                               IReceivableService _receivableService, IPayableService _payableService);
        bool ValidUpdateObject(SalesDownPaymentAllocationDetail salesDownPaymentAllocationDetail, ISalesDownPaymentAllocationService _salesDownPaymentAllocationService,
                               ISalesDownPaymentAllocationDetailService _salesDownPaymentAllocationDetailService, ISalesDownPaymentService _salesDownPaymentService,
                               IReceivableService _receivableService, IPayableService _payableService);
        bool ValidDeleteObject(SalesDownPaymentAllocationDetail salesDownPaymentAllocationDetail);
        bool ValidConfirmObject(SalesDownPaymentAllocationDetail salesDownPaymentAllocationDetail,
                                IReceivableService _receivableService, IPayableService _payableService);
        bool ValidUnconfirmObject(SalesDownPaymentAllocationDetail salesDownPaymentAllocationDetail,
                                  IReceivableService _receivableService, IPayableService _payableService);
        bool isValid(SalesDownPaymentAllocationDetail salesDownPaymentAllocationDetail);
        string PrintError(SalesDownPaymentAllocationDetail salesDownPaymentAllocationDetail);
    }
}
