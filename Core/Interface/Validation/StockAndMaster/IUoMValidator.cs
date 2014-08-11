using Core.DomainModel;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Validation
{
    public interface IUoMValidator
    {
        UoM VHasUniqueName(UoM uom, IUoMService _uomService);
        UoM VHasItem(UoM uom, IItemService _itemService);
        UoM VCreateObject(UoM uom, IUoMService _uomService);
        UoM VUpdateObject(UoM uom, IUoMService _uomService);
        UoM VDeleteObject(UoM uom, IItemService _itemService);
        bool ValidCreateObject(UoM uom, IUoMService _uomService);
        bool ValidUpdateObject(UoM uom, IUoMService _uomService);
        bool ValidDeleteObject(UoM uom, IItemService _itemService);
        bool isValid(UoM uom);
        string PrintError(UoM uom);
    }
}