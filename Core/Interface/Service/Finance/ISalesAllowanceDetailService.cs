using Core.DomainModel;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface ISalesAllowanceDetailService
    {
        ISalesAllowanceDetailValidator GetValidator();
        IQueryable<SalesAllowanceDetail> GetQueryable();
        IList<SalesAllowanceDetail> GetAll();
        IList<SalesAllowanceDetail> GetObjectsBySalesAllowanceId(int salesAllowanceId);
        IList<SalesAllowanceDetail> GetObjectsByReceivableId(int receivableId);
        SalesAllowanceDetail GetObjectById(int Id);        
        SalesAllowanceDetail CreateObject(SalesAllowanceDetail salesAllowanceDetail, ISalesAllowanceService _salesAllowanceService,
                                          ICashBankService _cashBankService, IReceivableService _receivableService);
        SalesAllowanceDetail UpdateObject(SalesAllowanceDetail salesAllowanceDetail, ISalesAllowanceService _salesAllowanceService,
                                          ICashBankService _cashBankService, IReceivableService _receivableService);
        SalesAllowanceDetail SoftDeleteObject(SalesAllowanceDetail salesAllowanceDetail);
        bool DeleteObject(int Id);
        SalesAllowanceDetail ConfirmObject(SalesAllowanceDetail salesAllowanceDetail, DateTime ConfirmationDate, ISalesAllowanceService _salesAllowanceService, IReceivableService _receivableService);
        SalesAllowanceDetail UnconfirmObject(SalesAllowanceDetail salesAllowanceDetail, ISalesAllowanceService _salesAllowanceService, IReceivableService _receivableService);
    }
}