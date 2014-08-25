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
    public class RollerWarehouseMutationService : IRollerWarehouseMutationService
    {
        private IRollerWarehouseMutationRepository _repository;
        private IRollerWarehouseMutationValidator _validator;
        public RollerWarehouseMutationService(IRollerWarehouseMutationRepository _rollerWarehouseMutationRepository, IRollerWarehouseMutationValidator _rollerWarehouseMutationValidator)
        {
            _repository = _rollerWarehouseMutationRepository;
            _validator = _rollerWarehouseMutationValidator;
        }

        public IRollerWarehouseMutationValidator GetValidator()
        {
            return _validator;
        }

        public IRollerWarehouseMutationRepository GetRepository()
        {
            return _repository;
        }

        public IQueryable<RollerWarehouseMutation> GetQueryable()
        {
            return _repository.GetQueryable();
        }

        public IList<RollerWarehouseMutation> GetAll()
        {
            return _repository.GetAll();
        }

        public IList<RollerWarehouseMutation> GetObjectsByRecoveryOrderId(int recoveryOrderId)
        {
            return _repository.GetObjectsByRecoveryOrderId(recoveryOrderId);
        }

        public Warehouse GetWarehouseFrom(RollerWarehouseMutation rollerWarehouseMutation)
        {
            return _repository.GetWarehouseFrom(rollerWarehouseMutation);
        }

        public Warehouse GetWarehouseTo(RollerWarehouseMutation rollerWarehouseMutation)
        {
            return _repository.GetWarehouseFrom(rollerWarehouseMutation);
        }

        public RollerWarehouseMutation GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public RollerWarehouseMutation CreateObject(RollerWarehouseMutation rollerWarehouseMutation, IWarehouseService _warehouseService, IRecoveryOrderService _recoveryOrderService)
        {
            rollerWarehouseMutation.Errors = new Dictionary<String, String>();
            return (_validator.ValidCreateObject(rollerWarehouseMutation, _warehouseService, _recoveryOrderService) ? _repository.CreateObject(rollerWarehouseMutation) : rollerWarehouseMutation);
        }

        public RollerWarehouseMutation UpdateObject(RollerWarehouseMutation rollerWarehouseMutation, IWarehouseService _warehouseService, IRecoveryOrderService _recoveryOrderService)
        {
            return (rollerWarehouseMutation = _validator.ValidUpdateObject(rollerWarehouseMutation, _warehouseService, _recoveryOrderService) ? _repository.UpdateObject(rollerWarehouseMutation) : rollerWarehouseMutation);
        }

        public RollerWarehouseMutation SoftDeleteObject(RollerWarehouseMutation rollerWarehouseMutation)
        {
            return (rollerWarehouseMutation = _validator.ValidDeleteObject(rollerWarehouseMutation) ? _repository.SoftDeleteObject(rollerWarehouseMutation) : rollerWarehouseMutation);
        }

        public RollerWarehouseMutation ConfirmObject(RollerWarehouseMutation rollerWarehouseMutation, DateTime ConfirmationDate, IRollerWarehouseMutationDetailService _rollerWarehouseMutationDetailService,
                                                     IItemService _itemService, IBarringService _barringService, IWarehouseItemService _warehouseItemService,
                                                     IStockMutationService _stockMutationService, IRecoveryOrderDetailService _recoveryOrderDetailService, 
                                                     ICoreIdentificationDetailService _coreIdentificationDetailService, ICoreIdentificationService _coreIdentificationService)
        {
            rollerWarehouseMutation.ConfirmationDate = ConfirmationDate;
            if (_validator.ValidConfirmObject(rollerWarehouseMutation, this, _rollerWarehouseMutationDetailService,
                                              _itemService, _barringService, _warehouseItemService))
            {
                IList<RollerWarehouseMutationDetail> rollerWarehouseMutationDetails = _rollerWarehouseMutationDetailService.GetObjectsByRollerWarehouseMutationId(rollerWarehouseMutation.Id);
                foreach (var detail in rollerWarehouseMutationDetails)
                {
                    detail.Errors = new Dictionary<string, string>();
                    _rollerWarehouseMutationDetailService.ConfirmObject(detail, ConfirmationDate, this, _itemService, _barringService,
                                                                        _warehouseItemService, _stockMutationService, _recoveryOrderDetailService,
                                                                        _coreIdentificationDetailService, _coreIdentificationService);
                }
                _repository.ConfirmObject(rollerWarehouseMutation);
            }
            return rollerWarehouseMutation;
        }

        public RollerWarehouseMutation UnconfirmObject(RollerWarehouseMutation rollerWarehouseMutation, IRollerWarehouseMutationDetailService _rollerWarehouseMutationDetailService,
                                                      IItemService _itemService, IBarringService _barringService, IWarehouseItemService _warehouseItemService,
                                                      IStockMutationService _stockMutationService, IRecoveryOrderDetailService _recoveryOrderDetailService, 
                                                      ICoreIdentificationDetailService _coreIdentificationDetailService, ICoreIdentificationService _coreIdentificationService)
        {
            if (_validator.ValidUnconfirmObject(rollerWarehouseMutation, this, _rollerWarehouseMutationDetailService,
                                                _itemService, _barringService, _warehouseItemService))
            {
                IList<RollerWarehouseMutationDetail> rollerWarehouseMutationDetails = _rollerWarehouseMutationDetailService.GetObjectsByRollerWarehouseMutationId(rollerWarehouseMutation.Id);
                foreach (var detail in rollerWarehouseMutationDetails)
                {
                    detail.Errors = new Dictionary<string, string>();
                    _rollerWarehouseMutationDetailService.UnconfirmObject(detail, this, _itemService, _barringService, _warehouseItemService,
                                                                          _stockMutationService, _recoveryOrderDetailService, _coreIdentificationDetailService,
                                                                          _coreIdentificationService);
                }
                _repository.UnconfirmObject(rollerWarehouseMutation);
            }
            return rollerWarehouseMutation;
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }
    }
}