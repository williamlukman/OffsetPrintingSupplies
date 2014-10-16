using Core.Constants;
using Core.DomainModel;
using Core.Interface.Repository;
using Core.Interface.Service;
using Core.Interface.Validation;
using Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Service.Service
{
    public class RollerWarehouseMutationDetailService : IRollerWarehouseMutationDetailService
    {

        private IRollerWarehouseMutationDetailRepository _repository;
        private IRollerWarehouseMutationDetailValidator _validator;

        public RollerWarehouseMutationDetailService(IRollerWarehouseMutationDetailRepository _rollerWarehouseMutationDetailRepository, IRollerWarehouseMutationDetailValidator _rollerWarehouseMutationDetailValidator)
        {
            _repository = _rollerWarehouseMutationDetailRepository;
            _validator = _rollerWarehouseMutationDetailValidator;
        }

        public IRollerWarehouseMutationDetailValidator GetValidator()
        {
            return _validator;
        }

        public IQueryable<RollerWarehouseMutationDetail> GetQueryable()
        {
            return _repository.GetQueryable();
        }

        public IList<RollerWarehouseMutationDetail> GetAll()
        {
            return _repository.GetAll();
        }

        public IList<RollerWarehouseMutationDetail> GetObjectsByRollerWarehouseMutationId(int rollerWarehouseMutationId)
        {
            return _repository.GetObjectsByRollerWarehouseMutationId(rollerWarehouseMutationId);
        }

        public RollerWarehouseMutationDetail GetObjectByRecoveryOrderDetailId(int recoveryOrderDetailId)
        {
            return _repository.GetObjectByRecoveryOrderDetailId(recoveryOrderDetailId);
        }

        public RollerWarehouseMutationDetail GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public RollerWarehouseMutationDetail CreateObject(RollerWarehouseMutationDetail rollerWarehouseMutationDetail, IRollerWarehouseMutationService _rollerWarehouseMutationService,
                                                          IRecoveryOrderDetailService _recoveryOrderDetailService, ICoreIdentificationDetailService _coreIdentificationDetailService,
                                                          IItemService _itemService, IWarehouseItemService _warehouseItemService)
        {
            rollerWarehouseMutationDetail.Errors = new Dictionary<String, String>();
            return (rollerWarehouseMutationDetail = _validator.ValidCreateObject(rollerWarehouseMutationDetail, _rollerWarehouseMutationService,
                                                    _recoveryOrderDetailService, _coreIdentificationDetailService, this, _itemService, _warehouseItemService) ?
                                                    _repository.CreateObject(rollerWarehouseMutationDetail) : rollerWarehouseMutationDetail);
        }

        public RollerWarehouseMutationDetail CreateObject(int rollerWarehouseMutationId, int itemId, int quantity, IRollerWarehouseMutationService _rollerWarehouseMutationService,
                                                          IRecoveryOrderDetailService _recoveryOrderDetailService, ICoreIdentificationDetailService _coreIdentificationDetailService,
                                                          IItemService _itemService, IWarehouseItemService _warehouseItemService)
        {
            RollerWarehouseMutationDetail rollerWarehouseMutationDetail = new RollerWarehouseMutationDetail
            {
                RollerWarehouseMutationId = rollerWarehouseMutationId,
                ItemId = itemId,
                // Price = price
            };
            return this.CreateObject(rollerWarehouseMutationDetail, _rollerWarehouseMutationService, _recoveryOrderDetailService,
                                     _coreIdentificationDetailService, _itemService, _warehouseItemService);
        }

        public RollerWarehouseMutationDetail UpdateObject(RollerWarehouseMutationDetail rollerWarehouseMutationDetail, IRollerWarehouseMutationService _rollerWarehouseMutationService,
                                                          IRecoveryOrderDetailService _recoveryOrderDetailService, ICoreIdentificationDetailService _coreIdentificationDetailService,
                                                          IItemService _itemService, IWarehouseItemService _warehouseItemService)
        {
            return (rollerWarehouseMutationDetail = _validator.ValidUpdateObject(rollerWarehouseMutationDetail, _rollerWarehouseMutationService,
                                                    _recoveryOrderDetailService, _coreIdentificationDetailService, this, _itemService, _warehouseItemService) ?
                                                   _repository.UpdateObject(rollerWarehouseMutationDetail) : rollerWarehouseMutationDetail);
        }

        public RollerWarehouseMutationDetail SoftDeleteObject(RollerWarehouseMutationDetail rollerWarehouseMutationDetail, IRollerWarehouseMutationService _rollerWarehouseMutationService, IWarehouseItemService _warehouseItemService)
        {
            return (rollerWarehouseMutationDetail = _validator.ValidDeleteObject(rollerWarehouseMutationDetail) ? _repository.SoftDeleteObject(rollerWarehouseMutationDetail) : rollerWarehouseMutationDetail);
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }

        public RollerWarehouseMutationDetail ConfirmObject(RollerWarehouseMutationDetail rollerWarehouseMutationDetail, DateTime ConfirmationDate, IRollerWarehouseMutationService _rollerWarehouseMutationService,
                                                         IItemService _itemService, IBlanketService _blanketService, IWarehouseItemService _warehouseItemService, IStockMutationService _stockMutationService,
                                                         IRecoveryOrderDetailService _recoveryOrderDetailService, ICoreIdentificationDetailService _coreIdentificationDetailService,
                                                         ICoreIdentificationService _coreIdentificationService)
        {
            rollerWarehouseMutationDetail.ConfirmationDate = ConfirmationDate;
            if (_validator.ValidConfirmObject(rollerWarehouseMutationDetail, _rollerWarehouseMutationService, _itemService, _blanketService, _warehouseItemService))
            {
                _repository.ConfirmObject(rollerWarehouseMutationDetail);
                
                // Set IsDelivered = true
                RecoveryOrderDetail recoveryOrderDetail = _recoveryOrderDetailService.GetObjectById(rollerWarehouseMutationDetail.RecoveryOrderDetailId);
                CoreIdentificationDetail coreIdentificationDetail = _coreIdentificationDetailService.GetObjectById(recoveryOrderDetail.CoreIdentificationDetailId);
                _coreIdentificationDetailService.DeliverObject(coreIdentificationDetail, _coreIdentificationService, this);

                // reduce warehouseFromItem
                // add warehouseToItem

                RollerWarehouseMutation rollerWarehouseMutation = _rollerWarehouseMutationService.GetObjectById(rollerWarehouseMutationDetail.RollerWarehouseMutationId);
                WarehouseItem warehouseItemFrom = _warehouseItemService.FindOrCreateObject(rollerWarehouseMutation.WarehouseFromId, rollerWarehouseMutationDetail.ItemId);
                WarehouseItem warehouseItemTo = _warehouseItemService.FindOrCreateObject(rollerWarehouseMutation.WarehouseToId, rollerWarehouseMutationDetail.ItemId);

                IList<StockMutation> stockMutations = _stockMutationService.CreateStockMutationForRollerWarehouseMutation(rollerWarehouseMutationDetail, warehouseItemFrom, warehouseItemTo);
                foreach (var stockMutation in stockMutations)
                {
                    _stockMutationService.StockMutateObject(stockMutation, _itemService, _blanketService, _warehouseItemService);
                }
            }
            return rollerWarehouseMutationDetail;
        }

        public RollerWarehouseMutationDetail UnconfirmObject(RollerWarehouseMutationDetail rollerWarehouseMutationDetail, IRollerWarehouseMutationService _rollerWarehouseMutationService,
                                                            IItemService _itemService, IBlanketService _blanketService, IWarehouseItemService _warehouseItemService, IStockMutationService _stockMutationService,
                                                            IRecoveryOrderDetailService _recoveryOrderDetailService, ICoreIdentificationDetailService _coreIdentificationDetailService,
                                                            ICoreIdentificationService _coreIdentificationService)
        {
            if (_validator.ValidUnconfirmObject(rollerWarehouseMutationDetail, _rollerWarehouseMutationService, _itemService, _blanketService, _warehouseItemService))
            {
                _repository.UnconfirmObject(rollerWarehouseMutationDetail);

                // Set IsDelivered = false
                RecoveryOrderDetail recoveryOrderDetail = _recoveryOrderDetailService.GetObjectById(rollerWarehouseMutationDetail.RecoveryOrderDetailId);
                CoreIdentificationDetail coreIdentificationDetail = _coreIdentificationDetailService.GetObjectById(recoveryOrderDetail.CoreIdentificationDetailId);
                _coreIdentificationDetailService.UndoDeliverObject(coreIdentificationDetail, _coreIdentificationService, this);

                // reverse stock mutate warehouseFromItem and warehouseToItem
                RollerWarehouseMutation rollerWarehouseMutation = _rollerWarehouseMutationService.GetObjectById(rollerWarehouseMutationDetail.RollerWarehouseMutationId);
                WarehouseItem warehouseItemFrom = _warehouseItemService.FindOrCreateObject(rollerWarehouseMutation.WarehouseFromId, rollerWarehouseMutationDetail.ItemId);
                WarehouseItem warehouseItemTo = _warehouseItemService.FindOrCreateObject(rollerWarehouseMutation.WarehouseToId, rollerWarehouseMutationDetail.ItemId);

                IList<StockMutation> stockMutations = _stockMutationService.DeleteStockMutationForRollerWarehouseMutation(rollerWarehouseMutationDetail, warehouseItemFrom, warehouseItemTo);
                foreach (var stockMutation in stockMutations)
                {
                    _stockMutationService.ReverseStockMutateObject(stockMutation, _itemService, _blanketService, _warehouseItemService);
                }

            }
            return rollerWarehouseMutationDetail;
        }
    }
}