using Core.DomainModel;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface IPurchaseAllowanceDetailService
    {
        IPurchaseAllowanceDetailValidator GetValidator();
        IQueryable<PurchaseAllowanceDetail> GetQueryable();
        IList<PurchaseAllowanceDetail> GetAll();
        IList<PurchaseAllowanceDetail> GetObjectsByPurchaseAllowanceId(int purchaseAllowanceId);
        IList<PurchaseAllowanceDetail> GetObjectsByPayableId(int payableId);
        PurchaseAllowanceDetail GetObjectById(int Id);        
        PurchaseAllowanceDetail CreateObject(PurchaseAllowanceDetail purchaseAllowanceDetail, IPurchaseAllowanceService _purchaseAllowanceService,
                                          ICashBankService _cashBankService, IPayableService _payableService);
        PurchaseAllowanceDetail UpdateObject(PurchaseAllowanceDetail purchaseAllowanceDetail, IPurchaseAllowanceService _purchaseAllowanceService,
                                          ICashBankService _cashBankService, IPayableService _payableService);
        PurchaseAllowanceDetail SoftDeleteObject(PurchaseAllowanceDetail purchaseAllowanceDetail);
        bool DeleteObject(int Id);
        PurchaseAllowanceDetail ConfirmObject(PurchaseAllowanceDetail purchaseAllowanceDetail, DateTime ConfirmationDate, IPurchaseAllowanceService _purchaseAllowanceService, IPayableService _payableService);
        PurchaseAllowanceDetail UnconfirmObject(PurchaseAllowanceDetail purchaseAllowanceDetail, IPurchaseAllowanceService _purchaseAllowanceService, IPayableService _payableService);
    }
}