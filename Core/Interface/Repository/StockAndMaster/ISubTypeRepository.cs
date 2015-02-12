using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Repository
{
    public interface ISubTypeRepository : IRepository<SubType>
    {
        IQueryable<SubType> GetQueryable();
        IList<SubType> GetAll();
        SubType GetObjectById(int Id);
        SubType GetObjectByName(string Name);
        SubType CreateObject(SubType subType);
        SubType UpdateObject(SubType subType);
        SubType SoftDeleteObject(SubType subType);
        bool DeleteObject(int Id);
    }
}