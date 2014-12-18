using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Interface.Validation;

namespace Core.Interface.Service
{
    public interface IPayableService
    {
        IQueryable<Payable> GetQueryable();
        IPayableValidator GetValidator();
        IList<Payable> GetAll();
        IList<Payable> GetObjectsByContactId(int contactId);
        Payable GetObjectBySource(string PayableSource, int PayableSourceId); 
        Payable GetObjectById(int Id);
        Payable CreateObject(Payable payable);
        Payable CreateObject(int contactId, string payableSource, int payableSourceId,int currencyId, decimal amount, decimal rate,DateTime dueDate);
        Payable CreateObject(int contactId, string payableSource, int payableSourceId, int currencyId, decimal amount, decimal rate); // for initial data only
        Payable UpdateObject(Payable payable);
        Payable SoftDeleteObject(Payable payable);
        bool DeleteObject(int Id);
    }
}