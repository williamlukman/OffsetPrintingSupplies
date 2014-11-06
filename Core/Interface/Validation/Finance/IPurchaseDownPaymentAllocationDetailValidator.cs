using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.DomainModel;
using Core.Interface.Service;

namespace Core.Interface.Validation
{
    public interface IPurchaseDownPaymentAllocationDetailValidator
    {
        PurchaseDownPaymentAllocationDetail VHasPurchaseDownPaymentAllocation(PurchaseDownPaymentAllocationDetail purchaseDownPaymentAllocationDetail,
                                            IPurchaseDownPaymentAllocationService _purchaseDownPaymentAllocationService);
        PurchaseDownPaymentAllocationDetail VHasPayable(PurchaseDownPaymentAllocationDetail purchaseDownPaymentAllocationDetail, IPayableService _payableService);
        PurchaseDownPaymentAllocationDetail VHasBeenConfirmed(PurchaseDownPaymentAllocationDetail purchaseDownPaymentAllocationDetail);
        PurchaseDownPaymentAllocationDetail VHasNotBeenConfirmed(PurchaseDownPaymentAllocationDetail purchaseDownPaymentAllocationDetail);
        PurchaseDownPaymentAllocationDetail VHasNotBeenDeleted(PurchaseDownPaymentAllocationDetail purchaseDownPaymentAllocationDetail);
        PurchaseDownPaymentAllocationDetail VPayableHasNotBeenCompleted(PurchaseDownPaymentAllocationDetail purchaseDownPaymentAllocationDetail, IPayableService _payableService);
        PurchaseDownPaymentAllocationDetail VNonNegativeAmount(PurchaseDownPaymentAllocationDetail purchaseDownPaymentAllocationDetail);
        PurchaseDownPaymentAllocationDetail VAmountLessOrEqualPayable(PurchaseDownPaymentAllocationDetail purchaseDownPaymentAllocationDetail, IPayableService _payableService);
        PurchaseDownPaymentAllocationDetail VUniquePayableId(PurchaseDownPaymentAllocationDetail purchaseDownPaymentAllocationDetail,
                                            IPurchaseDownPaymentAllocationDetailService _purchaseDownPaymentAllocationDetailService, IPayableService _payableService);
        PurchaseDownPaymentAllocationDetail VDetailsAmountLessOrEqualPurchaseDownPaymentTotal(PurchaseDownPaymentAllocationDetail purchaseDownPaymentAllocationDetail,
                                            IPurchaseDownPaymentAllocationService _purchaseDownPaymentAllocationService,
                                            IPurchaseDownPaymentAllocationDetailService _purchaseDownPaymentAllocationDetailService);
        PurchaseDownPaymentAllocationDetail VCreateObject(PurchaseDownPaymentAllocationDetail purchaseDownPaymentAllocationDetail, IPurchaseDownPaymentAllocationService _purchaseDownPaymentAllocationService,
                                                       IPurchaseDownPaymentAllocationDetailService _purchaseDownPaymentAllocationDetailService, IPurchaseDownPaymentService _purchaseDownPaymentService,
                                                       IPayableService _payableService, IReceivableService _receivableService);
        PurchaseDownPaymentAllocationDetail VUpdateObject(PurchaseDownPaymentAllocationDetail purchaseDownPaymentAllocationDetail, IPurchaseDownPaymentAllocationService _purchaseDownPaymentAllocationService,
                                                       IPurchaseDownPaymentAllocationDetailService _purchaseDownPaymentAllocationDetailService, IPurchaseDownPaymentService _purchaseDownPaymentService,
                                                       IPayableService _payableService, IReceivableService _receivableService);
        PurchaseDownPaymentAllocationDetail VDeleteObject(PurchaseDownPaymentAllocationDetail purchaseDownPaymentAllocationDetail);
        PurchaseDownPaymentAllocationDetail VHasConfirmationDate(PurchaseDownPaymentAllocationDetail purchaseDownPaymentAllocationDetail);
        PurchaseDownPaymentAllocationDetail VConfirmObject(PurchaseDownPaymentAllocationDetail purchaseDownPaymentAllocationDetail,
                                                        IPayableService _payableService, IReceivableService _receivableService);
        PurchaseDownPaymentAllocationDetail VUnconfirmObject(PurchaseDownPaymentAllocationDetail purchaseDownPaymentAllocationDetail,
                                                          IPayableService _payableService, IReceivableService _receivableService);
        bool ValidCreateObject(PurchaseDownPaymentAllocationDetail purchaseDownPaymentAllocationDetail, IPurchaseDownPaymentAllocationService _purchaseDownPaymentAllocationService,
                               IPurchaseDownPaymentAllocationDetailService _purchaseDownPaymentAllocationDetailService, IPurchaseDownPaymentService _purchaseDownPaymentService,
                               IPayableService _payableService, IReceivableService _receivableService);
        bool ValidUpdateObject(PurchaseDownPaymentAllocationDetail purchaseDownPaymentAllocationDetail, IPurchaseDownPaymentAllocationService _purchaseDownPaymentAllocationService,
                               IPurchaseDownPaymentAllocationDetailService _purchaseDownPaymentAllocationDetailService, IPurchaseDownPaymentService _purchaseDownPaymentService,
                               IPayableService _payableService, IReceivableService _receivableService);
        bool ValidDeleteObject(PurchaseDownPaymentAllocationDetail purchaseDownPaymentAllocationDetail);
        bool ValidConfirmObject(PurchaseDownPaymentAllocationDetail purchaseDownPaymentAllocationDetail,
                                IPayableService _payableService, IReceivableService _receivableService);
        bool ValidUnconfirmObject(PurchaseDownPaymentAllocationDetail purchaseDownPaymentAllocationDetail,
                                  IPayableService _payableService, IReceivableService _receivableService);
        bool isValid(PurchaseDownPaymentAllocationDetail purchaseDownPaymentAllocationDetail);
        string PrintError(PurchaseDownPaymentAllocationDetail purchaseDownPaymentAllocationDetail);
    }
}
