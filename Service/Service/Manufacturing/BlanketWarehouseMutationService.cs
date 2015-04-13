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
    public class BlanketWarehouseMutationService : IBlanketWarehouseMutationService
    {
        private IBlanketWarehouseMutationRepository _repository;
        private IBlanketWarehouseMutationValidator _validator;
        public BlanketWarehouseMutationService(IBlanketWarehouseMutationRepository _blanketWarehouseMutationRepository, IBlanketWarehouseMutationValidator _blanketWarehouseMutationValidator)
        {
            _repository = _blanketWarehouseMutationRepository;
            _validator = _blanketWarehouseMutationValidator;
        }

        public IBlanketWarehouseMutationValidator GetValidator()
        {
            return _validator;
        }

        public IBlanketWarehouseMutationRepository GetRepository()
        {
            return _repository;
        }

        public IQueryable<BlanketWarehouseMutation> GetQueryable()
        {
            return _repository.GetQueryable();
        }

        public IList<BlanketWarehouseMutation> GetAll()
        {
            return _repository.GetAll();
        }

        public IList<BlanketWarehouseMutation> GetObjectsByBlanketOrderId(int blanketOrderId)
        {
            return _repository.GetObjectsByBlanketOrderId(blanketOrderId);
        }

        public Warehouse GetWarehouseFrom(BlanketWarehouseMutation blanketWarehouseMutation)
        {
            return _repository.GetWarehouseFrom(blanketWarehouseMutation);
        }

        public Warehouse GetWarehouseTo(BlanketWarehouseMutation blanketWarehouseMutation)
        {
            return _repository.GetWarehouseFrom(blanketWarehouseMutation);
        }

        public BlanketWarehouseMutation GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public BlanketWarehouseMutation CreateObject(BlanketWarehouseMutation blanketWarehouseMutation, IWarehouseService _warehouseService, IBlanketOrderService _blanketOrderService)
        {
            blanketWarehouseMutation.Errors = new Dictionary<String, String>();
            return (_validator.ValidCreateObject(blanketWarehouseMutation, _warehouseService, _blanketOrderService) ? _repository.CreateObject(blanketWarehouseMutation) : blanketWarehouseMutation);
        }

        public BlanketWarehouseMutation UpdateObject(BlanketWarehouseMutation blanketWarehouseMutation, IWarehouseService _warehouseService, IBlanketOrderService _blanketOrderService)
        {
            return (blanketWarehouseMutation = _validator.ValidUpdateObject(blanketWarehouseMutation, _warehouseService, _blanketOrderService) ? _repository.UpdateObject(blanketWarehouseMutation) : blanketWarehouseMutation);
        }

        public BlanketWarehouseMutation SoftDeleteObject(BlanketWarehouseMutation blanketWarehouseMutation)
        {
            return (blanketWarehouseMutation = _validator.ValidDeleteObject(blanketWarehouseMutation) ? _repository.SoftDeleteObject(blanketWarehouseMutation) : blanketWarehouseMutation);
        }

        public BlanketWarehouseMutation ConfirmObject(BlanketWarehouseMutation blanketWarehouseMutation, DateTime ConfirmationDate, IBlanketWarehouseMutationDetailService _blanketWarehouseMutationDetailService,
                                                     IItemService _itemService, IBlanketService _blanketService, IWarehouseItemService _warehouseItemService,
                                                     IStockMutationService _stockMutationService, IBlanketOrderDetailService _blanketOrderDetailService, IBlanketOrderService _blanketOrderService,
                                                     ICoreIdentificationDetailService _coreIdentificationDetailService, ICoreIdentificationService _coreIdentificationService,
                                                     ICustomerStockMutationService _customerStockMutationService, ICustomerItemService _customerItemService)
        {
            blanketWarehouseMutation.ConfirmationDate = ConfirmationDate;
            if (_validator.ValidConfirmObject(blanketWarehouseMutation, this, _blanketWarehouseMutationDetailService,
                                              _itemService, _blanketService, _warehouseItemService, _customerItemService,
                                              _blanketOrderService, _coreIdentificationService))
            {
                IList<BlanketWarehouseMutationDetail> blanketWarehouseMutationDetails = _blanketWarehouseMutationDetailService.GetObjectsByBlanketWarehouseMutationId(blanketWarehouseMutation.Id);
                foreach (var detail in blanketWarehouseMutationDetails)
                {
                    detail.Errors = new Dictionary<string, string>();
                    _blanketWarehouseMutationDetailService.ConfirmObject(detail, ConfirmationDate, this, _itemService, _blanketService,
                                                                        _warehouseItemService, _stockMutationService, _blanketOrderDetailService, _blanketOrderService,
                                                                        _coreIdentificationDetailService, _coreIdentificationService, _customerStockMutationService, _customerItemService);
                }
                _repository.ConfirmObject(blanketWarehouseMutation);
            }
            return blanketWarehouseMutation;
        }

        public BlanketWarehouseMutation UnconfirmObject(BlanketWarehouseMutation blanketWarehouseMutation, IBlanketWarehouseMutationDetailService _blanketWarehouseMutationDetailService,
                                                      IItemService _itemService, IBlanketService _blanketService, IWarehouseItemService _warehouseItemService,
                                                      IStockMutationService _stockMutationService, IBlanketOrderDetailService _blanketOrderDetailService, IBlanketOrderService _blanketOrderService,
                                                      ICoreIdentificationDetailService _coreIdentificationDetailService, ICoreIdentificationService _coreIdentificationService,
                                                      ICustomerStockMutationService _customerStockMutationService, ICustomerItemService _customerItemService)
        {
            if (_validator.ValidUnconfirmObject(blanketWarehouseMutation, _blanketOrderService, _coreIdentificationService, this, _blanketWarehouseMutationDetailService,
                                                _itemService, _blanketService, _warehouseItemService, _customerItemService))
            {
                IList<BlanketWarehouseMutationDetail> blanketWarehouseMutationDetails = _blanketWarehouseMutationDetailService.GetObjectsByBlanketWarehouseMutationId(blanketWarehouseMutation.Id);
                foreach (var detail in blanketWarehouseMutationDetails)
                {
                    detail.Errors = new Dictionary<string, string>();
                    _blanketWarehouseMutationDetailService.UnconfirmObject(detail, this, _itemService, _blanketService, _warehouseItemService,
                                                                          _stockMutationService, _blanketOrderDetailService, _blanketOrderService, _coreIdentificationDetailService,
                                                                          _coreIdentificationService, _customerStockMutationService, _customerItemService);
                }
                _repository.UnconfirmObject(blanketWarehouseMutation);
            }
            return blanketWarehouseMutation;
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }
    }
}