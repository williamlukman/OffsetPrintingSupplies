using Core.DomainModel;
using Core.Interface.Repository;
using Core.Interface.Service;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Service.Service
{
    public class ServiceCostService : IServiceCostService
    {
        private IServiceCostRepository _repository;
        private IServiceCostValidator _validator;
        public ServiceCostService(IServiceCostRepository _ServiceCostRepository, IServiceCostValidator _ServiceCostValidator)
        {
            _repository = _ServiceCostRepository;
            _validator = _ServiceCostValidator;
        }

        public IServiceCostValidator GetValidator()
        {
            return _validator;
        }

        public IQueryable<ServiceCost> GetQueryable()
        {
            return _repository.GetQueryable();
        }

        public IList<ServiceCost> GetAll()
        {
            return _repository.GetAll();
        }

        public ServiceCost FindOrCreateObject(int rollerBuilderId)
        {
            return _repository.FindOrCreateObject(rollerBuilderId);
        }

        public ServiceCost GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public ServiceCost GetObjectByItemId(int ItemId)
        {
            return _repository.GetObjectByItemId(ItemId);
        }

        public ServiceCost GetObjectByRollerBuilderId(int RollerBuilderId)
        {
            return _repository.GetObjectByRollerBuilderId(RollerBuilderId);
        }

        public ServiceCost CreateObject(ServiceCost serviceCost, IRollerBuilderService _rollerBuilderService, IItemService _itemService)
        {
            serviceCost.Errors = new Dictionary<String, String>();
            return (_validator.ValidCreateObject(serviceCost, _rollerBuilderService, _itemService) ? _repository.CreateObject(serviceCost) : serviceCost);
        }

        public ServiceCost UpdateObject(ServiceCost serviceCost, IRollerBuilderService _rollerBuilderService, IItemService _itemService)
        {
            return (serviceCost = _validator.ValidUpdateObject(serviceCost, _rollerBuilderService, _itemService) ? _repository.UpdateObject(serviceCost) : serviceCost);
        }

        public ServiceCost CalculateAndUpdateAvgPrice(ServiceCost serviceCost, int AdditionalQuantity, decimal AdditionalCost)
        {
            decimal NewQuantity = serviceCost.Quantity + AdditionalQuantity;
            decimal NewAvgPrice = (serviceCost.Quantity + AdditionalQuantity == 0) ? 0 :
                ((serviceCost.Quantity * serviceCost.AvgPrice) + (AdditionalQuantity * AdditionalCost) / NewQuantity);
            serviceCost.Quantity = NewQuantity;
            serviceCost.AvgPrice = NewAvgPrice;
            _repository.UpdateObject(serviceCost);
            return serviceCost;
        }
    }
}