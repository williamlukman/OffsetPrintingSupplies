using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Repository
{
    public interface ICashMutationRepository : IRepository<CashMutation>
    {
        IQueryable<CashMutation> GetQueryable();
        IList<CashMutation> GetAll();
        CashMutation GetObjectById(int Id);
        IList<CashMutation> GetObjectsByCashBankId(int cashBankId);
        IList<CashMutation> GetObjectsBySourceDocument(int cashBankId, string SourceDocumentType, int SourceDocumentId);
        CashMutation CreateObject(CashMutation cashMutation);
        CashMutation SoftDeleteObject(CashMutation cashMutation);
        bool DeleteObject(int Id);
    }
}