using Core.DomainModel;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interface.Validation
{
    public interface IUoMValidator
    {
        UoM VHasUniqueName(UoM unitOfMeasurement, IUoMService _unitOfMeasurementService);
        UoM VHasItem(UoM unitOfMeasurement, IItemService _itemService);
        UoM VCreateObject(UoM unitOfMeasurement, IUoMService _unitOfMeasurementService);
        UoM VUpdateObject(UoM unitOfMeasurement, IUoMService _unitOfMeasurementService);
        UoM VDeleteObject(UoM unitOfMeasurement, IItemService _itemService);
        bool ValidCreateObject(UoM unitOfMeasurement, IUoMService _unitOfMeasurementService);
        bool ValidUpdateObject(UoM unitOfMeasurement, IUoMService _unitOfMeasurementService);
        bool ValidDeleteObject(UoM unitOfMeasurement, IItemService _itemService);
        bool isValid(UoM unitOfMeasurement);
        string PrintError(UoM unitOfMeasurement);
    }
}