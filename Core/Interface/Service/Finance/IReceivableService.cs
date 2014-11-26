using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Interface.Validation;

namespace Core.Interface.Service
{
    public interface IReceivableService
    {
        IReceivableValidator GetValidator();
        IQueryable<Receivable> GetQueryable();
        IList<Receivable> GetAll();
        IList<Receivable> GetObjectsByContactId(int contactId);
        Receivable GetObjectBySource(string ReceivableSource, int ReceivableSourceId); 
        Receivable GetObjectById(int Id);
        Receivable CreateObject(Receivable receivable); 
        Receivable CreateObject(int contactId, string receivableSource, int receivableSourceId, int currencyId, decimal amount, decimal Rate,DateTime dueDate);
        Receivable UpdateObject(Receivable receivable);
        Receivable SoftDeleteObject(Receivable receivable);
        bool DeleteObject(int Id);
    }
}