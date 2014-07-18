using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interface.Repository
{
    public interface IUoMRepository : IRepository<UoM>
    {
        IList<UoM> GetAll();
        UoM GetObjectById(int Id);
        UoM CreateObject(UoM unitOfMeasurement);
        UoM UpdateObject(UoM unitOfMeasurement);
        UoM SoftDeleteObject(UoM unitOfMeasurement);
        bool DeleteObject(int Id);
    }
}