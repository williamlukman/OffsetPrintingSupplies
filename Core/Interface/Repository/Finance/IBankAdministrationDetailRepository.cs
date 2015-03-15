using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Repository
{
    public interface IBankAdministrationDetailRepository : IRepository<BankAdministrationDetail>
    {
        IQueryable<BankAdministrationDetail> GetQueryable();
        IList<BankAdministrationDetail> GetAll();
        IList<BankAdministrationDetail> GetAllByMonthCreated();
        IList<BankAdministrationDetail> GetObjectsByBankAdministrationId(int bankAdministrationId);
        IList<BankAdministrationDetail> GetNonLegacyObjectsByBankAdministrationId(int bankAdministrationId);
        BankAdministrationDetail GetLegacyObjectByBankAdministrationId(int bankAdministrationId);
        BankAdministrationDetail GetObjectById(int Id);
        BankAdministrationDetail CreateObject(BankAdministrationDetail bankAdministrationDetail);
        BankAdministrationDetail UpdateObject(BankAdministrationDetail bankAdministrationDetail);
        BankAdministrationDetail SoftDeleteObject(BankAdministrationDetail bankAdministrationDetail);
        bool DeleteObject(int Id);
        BankAdministrationDetail ConfirmObject(BankAdministrationDetail bankAdministrationDetail);
        BankAdministrationDetail UnconfirmObject(BankAdministrationDetail bankAdministrationDetail);
        string SetObjectCode(string ParentCode);
    }
}