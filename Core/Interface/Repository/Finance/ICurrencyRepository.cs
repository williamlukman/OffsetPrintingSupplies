using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Repository
{
    public interface ICurrencyRepository : IRepository<Currency>
    {
        IQueryable<Currency> GetQueryable();
        IList<Currency> GetAll();
        Currency GetObjectById(int Id);
        Currency GetObjectByName(string Name);
        Currency CreateObject(Currency currency);
        Currency UpdateObject(Currency currency);
        Currency SoftDeleteObject(Currency currency);
        bool DeleteObject(int Id);
    }
}