using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Repository
{
    public interface IGLNonBaseCurrencyRepository : IRepository<GLNonBaseCurrency>
    {
        IQueryable<GLNonBaseCurrency> GetQueryable();
        IList<GLNonBaseCurrency> GetAll();
        GLNonBaseCurrency GetObjectById(int Id);
        GLNonBaseCurrency CreateObject(GLNonBaseCurrency generalLedgerJournal);
        GLNonBaseCurrency SoftDeleteObject(GLNonBaseCurrency generalLedgerJournal);
        bool DeleteObject(int Id);
    }
}