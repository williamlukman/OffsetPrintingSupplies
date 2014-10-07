using Core.DomainModel;
using Core.Interface.Validation;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Validation.Validation
{
    public class ServiceCostValidator : IServiceCostValidator
    {
        public ServiceCost VHasItem(ServiceCost serviceCost, IItemService _itemService)
        {
            Item item = _itemService.GetObjectById(serviceCost.ItemId);
            if (item == null)
            {
                serviceCost.Errors.Add("Generic", "Tidak terasosiasi dengan item");
            }
            return serviceCost;
        }

        public ServiceCost VHasRollerBuilder(ServiceCost serviceCost, IRollerBuilderService _rollerBuilderService)
        {
            RollerBuilder rollerBuilder = _rollerBuilderService.GetObjectById(serviceCost.RollerBuilderId);
            if (rollerBuilder == null)
            {
                serviceCost.Errors.Add("Generic", "Tidak terasosiasi dengan roller builder");
            }
            return serviceCost;
        }

        public ServiceCost VCreateObject(ServiceCost serviceCost, IRollerBuilderService _rollerBuilderService, IItemService _itemService)
        {
            return serviceCost;
        }

        public ServiceCost VUpdateObject(ServiceCost serviceCost, IRollerBuilderService _rollerBuilderService, IItemService _itemService)
        {
            return serviceCost;
        }

        public bool ValidCreateObject(ServiceCost serviceCost, IRollerBuilderService _rollerBuilderService, IItemService _itemService)
        {
            VCreateObject(serviceCost, _rollerBuilderService, _itemService);
            return isValid(serviceCost);
        }

        public bool ValidUpdateObject(ServiceCost serviceCost, IRollerBuilderService _rollerBuilderService, IItemService _itemService)
        {
            serviceCost.Errors.Clear();
            VUpdateObject(serviceCost, _rollerBuilderService, _itemService);
            return isValid(serviceCost);
        }

        public bool isValid(ServiceCost obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public string PrintError(ServiceCost obj)
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
