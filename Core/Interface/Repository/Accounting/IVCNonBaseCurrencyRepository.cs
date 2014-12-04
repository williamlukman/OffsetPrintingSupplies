using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Repository
{
    public interface IVCNonBaseCurrencyRepository : IRepository<VCNonBaseCurrency>
    {
        IQueryable<VCNonBaseCurrency> GetQueryable();
        IList<VCNonBaseCurrency> GetAll();
        VCNonBaseCurrency GetObjectById(int Id);
        VCNonBaseCurrency CreateObject(VCNonBaseCurrency validComb);
        VCNonBaseCurrency UpdateObject(VCNonBaseCurrency validComb);
        //VCNonBaseCurrency SoftDeleteObject(VCNonBaseCurrency validComb);
        bool DeleteObject(int Id);
    }
}