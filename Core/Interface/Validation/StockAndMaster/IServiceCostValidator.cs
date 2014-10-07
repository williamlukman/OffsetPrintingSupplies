using Core.DomainModel;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Validation
{
    public interface IServiceCostValidator
    {
        ServiceCost VHasItem(ServiceCost serviceCost, IItemService _itemService);
        ServiceCost VHasRollerBuilder(ServiceCost serviceCost, IRollerBuilderService _rollerBuilderService);
        ServiceCost VCreateObject(ServiceCost serviceCost, IRollerBuilderService _rollerBuilderService, IItemService _itemService);
        ServiceCost VUpdateObject(ServiceCost serviceCost, IRollerBuilderService _rollerBuilderService, IItemService _itemService);
        bool ValidCreateObject(ServiceCost serviceCost, IRollerBuilderService _rollerBuilderService, IItemService _itemService);
        bool ValidUpdateObject(ServiceCost serviceCost, IRollerBuilderService _rollerBuilderService, IItemService _itemService);
        bool isValid(ServiceCost serviceCost);
        string PrintError(ServiceCost serviceCost);
    }
}