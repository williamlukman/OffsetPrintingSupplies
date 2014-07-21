using Core.Constants;
using Core.DomainModel;
using Core.Interface.Repository;
using Core.Interface.Service;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Service
{
    public class CoreIdentificationService : ICoreIdentificationService
    {
        private ICoreIdentificationRepository _repository;
        private ICoreIdentificationValidator _validator;
        public CoreIdentificationService(ICoreIdentificationRepository _coreIdentificationRepository, ICoreIdentificationValidator _coreIdentificationValidator)
        {
            _repository = _coreIdentificationRepository;
            _validator = _coreIdentificationValidator;
        }

        public ICoreIdentificationValidator GetValidator()
        {
            return _validator;
        }

        public IList<CoreIdentification> GetAll()
        {
            return _repository.GetAll();
        }

        public IList<CoreIdentification> GetAllObjectsInHouse()
        {
            return _repository.GetAllObjectsInHouse();
        }

        public IList<CoreIdentification> GetAllObjectsByCustomerId(int CustomerId)
        {
            return _repository.GetAllObjectsByCustomerId(CustomerId);
        }

        public IList<CoreIdentification> GetAllObjectsByWarehouseId(int WarehouseId)
        {
            return _repository.GetAllObjectsByWarehouseId(WarehouseId);
        }

        public CoreIdentification GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public CoreIdentification CreateObjectForCustomer(int CustomerId, string Code, int Quantity, DateTime IdentifiedDate, int WarehouseId, ICustomerService _customerService)
        {
            CoreIdentification coreIdentification = new CoreIdentification
            {
                CustomerId = CustomerId,
                IsInHouse = false,
                Code = Code,
                Quantity = Quantity,
                IdentifiedDate = IdentifiedDate,
                WarehouseId = WarehouseId
            };
            return this.CreateObject(coreIdentification, _customerService);
        }

        public CoreIdentification CreateObjectForInHouse(string Code, int Quantity, DateTime IdentifiedDate, int WarehouseId, ICustomerService _customerService)
        {
            CoreIdentification coreIdentification = new CoreIdentification
            {
                CustomerId = null,
                IsInHouse = true,
                Code = Code,
                Quantity = Quantity,
                IdentifiedDate = IdentifiedDate,
                WarehouseId = WarehouseId
            };
            return this.CreateObject(coreIdentification, _customerService);
        }

        public CoreIdentification CreateObject(CoreIdentification coreIdentification, ICustomerService _customerService)
        {
            coreIdentification.Errors = new Dictionary<String, String>();
            return (coreIdentification = _validator.ValidCreateObject(coreIdentification, this, _customerService) ? _repository.CreateObject(coreIdentification) : coreIdentification);
        }

        public CoreIdentification UpdateObject(CoreIdentification coreIdentification, ICustomerService _customerService)
        {
            return (coreIdentification = _validator.ValidUpdateObject(coreIdentification, this, _customerService) ? _repository.UpdateObject(coreIdentification) : coreIdentification);
        }

        public CoreIdentification SoftDeleteObject(CoreIdentification coreIdentification, ICoreIdentificationDetailService _coreIdentificationDetailService, IRecoveryOrderService _recoveryOrderService)
        {
            if (_validator.ValidDeleteObject(coreIdentification, _recoveryOrderService))
            {
                IList<CoreIdentificationDetail> details = _coreIdentificationDetailService.GetObjectsByCoreIdentificationId(coreIdentification.Id);
                foreach (var detail in details)
                {
                    _coreIdentificationDetailService.GetRepository().SoftDeleteObject(detail);
                }
                _repository.SoftDeleteObject(coreIdentification);
            }
            return coreIdentification;
        }

        public CoreIdentification ConfirmObject(CoreIdentification coreIdentification, ICoreIdentificationDetailService _coreIdentificationDetailService, IStockMutationService _stockMutationService,
                                                IRecoveryOrderService _recoveryOrderService, IRecoveryOrderDetailService _recoveryOrderDetailService, ICoreBuilderService _coreBuilderService,
                                                IItemService _itemService, IWarehouseItemService _warehouseItemService, IBarringService _barringService)
        {
            if (_validator.ValidConfirmObject(coreIdentification, _coreIdentificationDetailService, _coreBuilderService, _warehouseItemService))
            {
                if (coreIdentification.CustomerId != null)
                {
                    IList<CoreIdentificationDetail> details = _coreIdentificationDetailService.GetObjectsByCoreIdentificationId(coreIdentification.Id);
                    foreach (var detail in details)
                    {
                        // add customer core
                        int MaterialCase = detail.MaterialCase;
                        Item item = (MaterialCase == Core.Constants.Constant.MaterialCase.New ?
                                        _coreBuilderService.GetNewCore(detail.CoreBuilderId) :
                                        _coreBuilderService.GetUsedCore(detail.CoreBuilderId));
                        WarehouseItem warehouseItem = _warehouseItemService.GetObjectByWarehouseAndItem(coreIdentification.WarehouseId, item.Id);
                        StockMutation stockMutation = _stockMutationService.CreateStockMutationForCoreIdentification(detail, warehouseItem);
                        StockMutateObject(stockMutation, _itemService, _barringService, _warehouseItemService);
                    }
                }
                _repository.ConfirmObject(coreIdentification);
            }
            return coreIdentification;
        }

        public CoreIdentification UnconfirmObject(CoreIdentification coreIdentification, ICoreIdentificationDetailService _coreIdentificationDetailService, IStockMutationService _stockMutationService,
                                                  IRecoveryOrderService _recoveryOrderService, ICoreBuilderService _coreBuilderService, IItemService _itemService,
                                                  IWarehouseItemService _warehouseItemService, IBarringService _barringService)
        {
            if (_validator.ValidUnconfirmObject(coreIdentification, _recoveryOrderService))
            {
                if (coreIdentification.CustomerId != null)
                {
                    IList<CoreIdentificationDetail> details = _coreIdentificationDetailService.GetObjectsByCoreIdentificationId(coreIdentification.Id);
                    foreach (var detail in details)
                    {
                        // reduce customer core
                        int MaterialCase = detail.MaterialCase;
                        Item item = (MaterialCase == Core.Constants.Constant.MaterialCase.New ?
                                        _coreBuilderService.GetNewCore(detail.CoreBuilderId) :
                                        _coreBuilderService.GetUsedCore(detail.CoreBuilderId));
                        WarehouseItem warehouseItem = _warehouseItemService.GetObjectByWarehouseAndItem(coreIdentification.WarehouseId, item.Id);
                        IList<StockMutation> stockMutations = _stockMutationService.SoftDeleteStockMutationForCoreIdentification(detail, warehouseItem);
                        foreach (var stockMutation in stockMutations)
                        {
                            ReverseStockMutateObject(stockMutation, _itemService, _barringService, _warehouseItemService);
                        }
                    }
                }
                _repository.UnconfirmObject(coreIdentification);
            }
            return coreIdentification;
        }

        public CoreIdentification CompleteObject(CoreIdentification coreIdentification, ICoreIdentificationDetailService _coreIdentificationDetailService)
        {
            if (_validator.ValidCompleteObject(coreIdentification, _coreIdentificationDetailService))
            {
                _repository.CompleteObject(coreIdentification);
            }
            return coreIdentification;
        }
        
        public void StockMutateObject(StockMutation stockMutation, IItemService _itemService, IBarringService _barringService, IWarehouseItemService _warehouseItemService)
        {
            int Quantity = (stockMutation.Status == Constant.StockMutationStatus.Addition) ? stockMutation.Quantity : (-1) * stockMutation.Quantity;
            WarehouseItem warehouseItem = _warehouseItemService.GetObjectById(stockMutation.WarehouseItemId);
            Item item = _itemService.GetObjectById(warehouseItem.ItemId);
            _itemService.AdjustQuantity(item, Quantity);
            _warehouseItemService.AdjustQuantity(warehouseItem, Quantity);
        }

        public void ReverseStockMutateObject(StockMutation stockMutation, IItemService _itemService, IBarringService _barringService, IWarehouseItemService _warehouseItemService)
        {
            int reverseQuantity = (stockMutation.Status == Constant.StockMutationStatus.Deduction) ? stockMutation.Quantity : (-1) * stockMutation.Quantity;
            WarehouseItem warehouseItem = _warehouseItemService.GetObjectById(stockMutation.WarehouseItemId);
            Item item = _itemService.GetObjectById(warehouseItem.ItemId);
            _itemService.AdjustQuantity(item, reverseQuantity);
            _warehouseItemService.AdjustQuantity(warehouseItem, reverseQuantity);
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }

        public bool IsCodeDuplicated(CoreIdentification coreIdentification)
        {
            IQueryable<CoreIdentification> coreIdentifications = _repository.FindAll(x => x.Code == coreIdentification.Code && !x.IsDeleted && x.Id != coreIdentification.Id);
            return (coreIdentifications.Count() > 0 ? true : false);
        }
    }
}