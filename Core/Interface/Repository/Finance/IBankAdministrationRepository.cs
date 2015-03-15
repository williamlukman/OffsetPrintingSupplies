using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Repository
{
    public interface IBankAdministrationRepository : IRepository<BankAdministration>
    {
        IQueryable<BankAdministration> GetQueryable();
        IList<BankAdministration> GetAll();
        IList<BankAdministration> GetAllByMonthCreated();
        IList<BankAdministration> GetObjectsByCashBankId(int cashBankId);
        BankAdministration GetObjectById(int Id);
        BankAdministration CreateObject(BankAdministration bankAdministration);
        BankAdministration UpdateObject(BankAdministration bankAdministration);
        BankAdministration SoftDeleteObject(BankAdministration bankAdministration);
        bool DeleteObject(int Id);
        BankAdministration ConfirmObject(BankAdministration bankAdministration);
        BankAdministration UnconfirmObject(BankAdministration bankAdministration);
        string SetObjectCode();
    }
}