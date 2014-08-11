using Core.DomainModel;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface IUoMService
    {
        IUoMValidator GetValidator();
        IList<UoM> GetAll();
        UoM GetObjectById(int Id);
        UoM GetObjectByName(string Name);
        UoM CreateObject(UoM unitOfMeasurement);
        UoM CreateObject(string Name);
        UoM UpdateObject(UoM unitOfMeasurement);
        UoM SoftDeleteObject(UoM unitOfMeasurement, IItemService _itemService);
        bool DeleteObject(int Id);
        bool IsNameDuplicated(UoM unitOfMeasurement);
    }
}