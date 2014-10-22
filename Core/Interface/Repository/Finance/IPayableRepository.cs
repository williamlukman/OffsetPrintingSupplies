using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Repository
{
    public interface IPayableRepository : IRepository<Payable>
    {
        IQueryable<Payable> GetQueryable();
        IList<Payable> GetAll();
        IList<Payable> GetAllByMonthCreated();
        IList<Payable> GetObjectsByContactId(int contactId);
        Payable GetObjectBySource(string PayableSource, int PayableSourceId); 
        Payable GetObjectById(int Id);
        Payable CreateObject(Payable payable);
        Payable UpdateObject(Payable payable);
        Payable SoftDeleteObject(Payable payable);
        bool DeleteObject(int Id);
        string SetObjectCode();
    }
}