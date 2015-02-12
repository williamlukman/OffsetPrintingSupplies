using Core.DomainModel;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface IPurchaseDownPaymentAllocationDetailService
    {
        IPurchaseDownPaymentAllocationDetailValidator GetValidator();
        IQueryable<PurchaseDownPaymentAllocationDetail> GetQueryable();
        IList<PurchaseDownPaymentAllocationDetail> GetAll();
        IList<PurchaseDownPaymentAllocationDetail> GetObjectsByPurchaseDownPaymentAllocationId(int purchaseDownPaymentAllocationId);
        IList<PurchaseDownPaymentAllocationDetail> GetObjectsByPayableId(int payableId);
        PurchaseDownPaymentAllocationDetail GetObjectById(int Id);
        PurchaseDownPaymentAllocationDetail CreateObject(PurchaseDownPaymentAllocationDetail purchaseDownPaymentAllocationDetail, IPurchaseDownPaymentAllocationService _purchaseDownPaymentAllocationService,
                                                         IPurchaseDownPaymentService _purchaseDownPaymentService, IPayableService _payableService, IReceivableService _receivableService);
        PurchaseDownPaymentAllocationDetail UpdateObject(PurchaseDownPaymentAllocationDetail purchaseDownPaymentAllocationDetail, IPurchaseDownPaymentAllocationService _purchaseDownPaymentAllocationService,
                                                         IPurchaseDownPaymentService _purchaseDownPaymentService, IPayableService _payableService, IReceivableService _receivableService);
        PurchaseDownPaymentAllocationDetail SoftDeleteObject(PurchaseDownPaymentAllocationDetail purchaseDownPaymentAllocationDetail, IPurchaseDownPaymentAllocationService _purchaseDownPaymentAllocationService);
        bool DeleteObject(int Id);
        PurchaseDownPaymentAllocationDetail ConfirmObject(PurchaseDownPaymentAllocationDetail purchaseDownPaymentAllocationDetail, DateTime ConfirmationDate,
                                                          IPurchaseDownPaymentAllocationService _purchaseDownPaymentAllocationService, IPurchaseDownPaymentService _purchaseDownPaymentService,
                                                          IPayableService _payableService, IReceivableService _receivableService);
        PurchaseDownPaymentAllocationDetail UnconfirmObject(PurchaseDownPaymentAllocationDetail purchaseDownPaymentAllocationDetail, IPurchaseDownPaymentAllocationService _purchaseDownPaymentAllocationService,
                                                            IPurchaseDownPaymentService _purchaseDownPaymentService, IPayableService _payableService, IReceivableService _receivableService);
    }
}