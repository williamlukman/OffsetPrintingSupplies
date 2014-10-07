using Core.DomainModel;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface IServiceCostService
    {
        IServiceCostValidator GetValidator();
        IQueryable<ServiceCost> GetQueryable();
        IList<ServiceCost> GetAll();
        ServiceCost FindOrCreateObject(int rollerBuilderId);
        ServiceCost GetObjectById(int Id);
        ServiceCost GetObjectByItemId(int itemId);
        ServiceCost GetObjectByRollerBuilderId(int rollerBuilderId);
        ServiceCost CreateObject(ServiceCost serviceCost, IRollerBuilderService _rollerBuilderService, IItemService _itemService);
        ServiceCost UpdateObject(ServiceCost serviceCost, IRollerBuilderService _rollerBuilderService, IItemService _itemService);
        ServiceCost CalculateAndUpdateAvgPrice(ServiceCost serviceCost, int AdditionalQuantity, decimal AdditionalCost);
    }
}