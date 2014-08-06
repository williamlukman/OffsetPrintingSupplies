using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Repository
{
    public interface ICashBankAdjustmentRepository : IRepository<CashBankAdjustment>
    {
        IList<CashBankAdjustment> GetAll();
        CashBankAdjustment GetObjectById(int Id);
        IList<CashBankAdjustment> GetObjectsByCashBankId(int cashBankId);
        CashBankAdjustment CreateObject(CashBankAdjustment cashBankAdjustment);
        CashBankAdjustment UpdateObject(CashBankAdjustment cashBankAdjustment);
        CashBankAdjustment SoftDeleteObject(CashBankAdjustment cashBankAdjustment);
        bool DeleteObject(int Id);
        CashBankAdjustment ConfirmObject(CashBankAdjustment cashBankAdjustment);
        CashBankAdjustment UnconfirmObject(CashBankAdjustment cashBankAdjustment);
        string SetObjectCode();
    }
}