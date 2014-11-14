using Core.DomainModel;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface ICurrencyService
    {
        IQueryable<Currency> GetQueryable();
        ICurrencyValidator GetValidator();
        IList<Currency> GetAll();
        Currency GetObjectById(int Id);
        Currency GetObjectByName(string Name);
        Currency CreateObject(Currency currency);
        Currency UpdateObject(Currency currency);
        Currency SoftDeleteObject(Currency currency);
        bool DeleteObject(int Id);
        bool IsNameDuplicated(Currency currency);
        decimal GetTotalCurrency();
    }
}