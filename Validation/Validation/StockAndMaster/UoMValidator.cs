using Core.DomainModel;
using Core.Interface.Validation;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Validation.Validation
{
    public class UoMValidator : IUoMValidator
    {
        public UoM VHasUniqueName(UoM uom, IUoMService _uomService)
        {
            if (String.IsNullOrEmpty(uom.Name) || uom.Name.Trim() == "")
            {
                uom.Errors.Add("Name", "Tidak boleh kosong");
            }
            if (_uomService.IsNameDuplicated(uom))
            {
                uom.Errors.Add("Name", "Tidak boleh diduplikasi");
            }
            return uom;
        }

        public UoM VHasItem(UoM uom, IItemService _itemService)
        {
            IList<Item> list = _itemService.GetObjectsByUoMId(uom.Id);
            if (list.Any())
            {
                uom.Errors.Add("Generic", "Item tidak boleh ada yang terasosiakan dengan uom");
            }
            return uom;
        }

        public UoM VCreateObject(UoM uom, IUoMService _uomService)
        {
            VHasUniqueName(uom, _uomService);
            return uom;
        }

        public UoM VUpdateObject(UoM uom, IUoMService _uomService)
        {
            VHasUniqueName(uom, _uomService);
            return uom;
        }

        public UoM VDeleteObject(UoM uom, IItemService _itemService)
        {
            VHasItem(uom, _itemService);
            return uom;
        }

        public bool ValidCreateObject(UoM uom, IUoMService _uomService)
        {
            VCreateObject(uom, _uomService);
            return isValid(uom);
        }

        public bool ValidUpdateObject(UoM uom, IUoMService _uomService)
        {
            uom.Errors.Clear();
            VUpdateObject(uom, _uomService);
            return isValid(uom);
        }

        public bool ValidDeleteObject(UoM uom, IItemService _itemService)
        {
            uom.Errors.Clear();
            VDeleteObject(uom, _itemService);
            return isValid(uom);
        }

        public bool isValid(UoM obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public string PrintError(UoM obj)
        {
            string erroroutput = "";
            KeyValuePair<string, string> first = obj.Errors.ElementAt(0);
            erroroutput += first.Key + "," + first.Value;
            foreach (KeyValuePair<string, string> pair in obj.Errors.Skip(1))
            {
                erroroutput += Environment.NewLine;
                erroroutput += pair.Key + "," + pair.Value;
            }
            return erroroutput;
        }

    }
}
