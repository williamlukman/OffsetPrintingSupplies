using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Repository
{
    public interface IUoMRepository : IRepository<UoM>
    {
        IQueryable<UoM> GetQueryable();
        IList<UoM> GetAll();
        UoM GetObjectById(int Id);
        UoM GetObjectByName(string Name);
        UoM CreateObject(UoM unitOfMeasurement);
        UoM UpdateObject(UoM unitOfMeasurement);
        UoM SoftDeleteObject(UoM unitOfMeasurement);
        bool DeleteObject(int Id);
    }
}