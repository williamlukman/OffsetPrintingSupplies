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
    public class WarehouseMutationService : IWarehouseMutationService
    {
        private IWarehouseMutationRepository _repository;
        private IWarehouseMutationValidator _validator;
        public WarehouseMutationService(IWarehouseMutationRepository _warehouseMutationRepository, IWarehouseMutationValidator _warehouseMutationValidator)
        {
            _repository = _warehouseMutationRepository;
            _validator = _warehouseMutationValidator;
        }

        public IWarehouseMutationValidator GetValidator()
        {
            return _validator;
        }

        public IWarehouseMutationRepository GetRepository()
        {
            return _repository;
        }

        public IQueryable<WarehouseMutation> GetQueryable()
        {
            return _repository.GetQueryable();
        }

        public IList<WarehouseMutation> GetAll()
        {
            return _repository.GetAll();
        }

        public Warehouse GetWarehouseFrom(WarehouseMutation warehouseMutation)
        {
            return _repository.GetWarehouseFrom(warehouseMutation);
        }

        public Warehouse GetWarehouseTo(WarehouseMutation warehouseMutation)
        {
            return _repository.GetWarehouseFrom(warehouseMutation);
        }

        public WarehouseMutation GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public WarehouseMutation CreateObject(WarehouseMutation warehouseMutation, IWarehouseService _warehouseService)
        {
            warehouseMutation.Errors = new Dictionary<String, String>();
            return (_validator.ValidCreateObject(warehouseMutation, _warehouseService) ? _repository.CreateObject(warehouseMutation) : warehouseMutation);
        }

        public WarehouseMutation UpdateObject(WarehouseMutation warehouseMutation, IWarehouseService _warehouseService)
        {
            return (warehouseMutation = _validator.ValidUpdateObject(warehouseMutation, _warehouseService) ? _repository.UpdateObject(warehouseMutation) : warehouseMutation);
        }

        public WarehouseMutation SoftDeleteObject(WarehouseMutation warehouseMutation)
        {
            return (warehouseMutation = _validator.ValidDeleteObject(warehouseMutation) ? _repository.SoftDeleteObject(warehouseMutation) : warehouseMutation);
        }

        public WarehouseMutation ConfirmObject(WarehouseMutation warehouseMutation, DateTime ConfirmationDate, IWarehouseMutationDetailService _warehouseMutationDetailService,
                                                    IItemService _itemService, IBlanketService _blanketService, IWarehouseItemService _warehouseItemService, IStockMutationService _stockMutationService)
        {
            warehouseMutation.ConfirmationDate = ConfirmationDate;
            if (_validator.ValidConfirmObject(warehouseMutation, this, _warehouseMutationDetailService,
                                              _itemService, _blanketService, _warehouseItemService))
            {
                IList<WarehouseMutationDetail> warehouseMutationDetails = _warehouseMutationDetailService.GetObjectsByWarehouseMutationId(warehouseMutation.Id);
                foreach (var detail in warehouseMutationDetails)
                {
                    detail.Errors = new Dictionary<string, string>();
                    _warehouseMutationDetailService.ConfirmObject(detail, ConfirmationDate, this, _itemService, _blanketService, _warehouseItemService, _stockMutationService);
                }
                _repository.ConfirmObject(warehouseMutation);
            }
            return warehouseMutation;
        }

        public WarehouseMutation UnconfirmObject(WarehouseMutation warehouseMutation, IWarehouseMutationDetailService _warehouseMutationDetailService,
                                                      IItemService _itemService, IBlanketService _blanketService, IWarehouseItemService _warehouseItemService,
                                                      IStockMutationService _stockMutationService)
        {
            if (_validator.ValidUnconfirmObject(warehouseMutation, this, _warehouseMutationDetailService,
                                                _itemService, _blanketService, _warehouseItemService))
            {
                IList<WarehouseMutationDetail> warehouseMutationDetails = _warehouseMutationDetailService.GetObjectsByWarehouseMutationId(warehouseMutation.Id);
                foreach (var detail in warehouseMutationDetails)
                {
                    detail.Errors = new Dictionary<string, string>();
                    _warehouseMutationDetailService.UnconfirmObject(detail, this, _itemService, _blanketService, _warehouseItemService, _stockMutationService);
                }
                _repository.UnconfirmObject(warehouseMutation);
            }
            return warehouseMutation;
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }
    }
}