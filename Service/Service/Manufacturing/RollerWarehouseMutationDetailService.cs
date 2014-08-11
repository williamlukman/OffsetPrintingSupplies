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

        public IList<RollerWarehouseMutationDetail> GetAll()
        {
            return _repository.GetAll();
        }

        public IList<RollerWarehouseMutationDetail> GetObjectsByRollerWarehouseMutationId(int rollerWarehouseMutationId)
        {
            return _repository.GetObjectsByRollerWarehouseMutationId(rollerWarehouseMutationId);
        }

        public RollerWarehouseMutationDetail GetObjectByCoreIdentificationDetailId(int coreIdentificationDetailId)
        {
            return _repository.GetObjectByCoreIdentificationDetailId(coreIdentificationDetailId);
        }

        public RollerWarehouseMutationDetail GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public RollerWarehouseMutationDetail CreateObject(RollerWarehouseMutationDetail rollerWarehouseMutationDetail, IRollerWarehouseMutationService _rollerWarehouseMutationService,
                                                          ICoreIdentificationDetailService _coreIdentificationDetailService, IItemService _itemService, IWarehouseItemService _warehouseItemService)
        {
            rollerWarehouseMutationDetail.Errors = new Dictionary<String, String>();
            return (rollerWarehouseMutationDetail = _validator.ValidCreateObject(rollerWarehouseMutationDetail, _rollerWarehouseMutationService, _coreIdentificationDetailService, this, _itemService, _warehouseItemService) ?
                                                   _repository.CreateObject(rollerWarehouseMutationDetail) : rollerWarehouseMutationDetail);
        }

        public RollerWarehouseMutationDetail CreateObject(int rollerWarehouseMutationId, int itemId, int quantity, IRollerWarehouseMutationService _rollerWarehouseMutationService,
                                                          ICoreIdentificationDetailService _coreIdentificationDetailService, IItemService _itemService, IWarehouseItemService _warehouseItemService)
        {
            RollerWarehouseMutationDetail rollerWarehouseMutationDetail = new RollerWarehouseMutationDetail
            {
                RollerWarehouseMutationId = rollerWarehouseMutationId,
                ItemId = itemId,
                // Price = price
            };
            return this.CreateObject(rollerWarehouseMutationDetail, _rollerWarehouseMutationService, _coreIdentificationDetailService, _itemService, _warehouseItemService);
        }

        public RollerWarehouseMutationDetail UpdateObject(RollerWarehouseMutationDetail rollerWarehouseMutationDetail, IRollerWarehouseMutationService _rollerWarehouseMutationService,
                                                          ICoreIdentificationDetailService _coreIdentificationDetailService, IItemService _itemService, IWarehouseItemService _warehouseItemService)
        {
            return (rollerWarehouseMutationDetail = _validator.ValidUpdateObject(rollerWarehouseMutationDetail, _rollerWarehouseMutationService, _coreIdentificationDetailService, this, _itemService, _warehouseItemService) ?
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
                                                         IItemService _itemService, IBarringService _barringService, IWarehouseItemService _warehouseItemService, IStockMutationService _stockMutationService,
                                                         ICoreIdentificationDetailService _coreIdentificationDetailService, ICoreIdentificationService _coreIdentificationService)
        {
            if (_validator.ValidConfirmObject(rollerWarehouseMutationDetail, _rollerWarehouseMutationService, _itemService, _barringService, _warehouseItemService))
            {
                RollerWarehouseMutation rollerWarehouseMutation = _rollerWarehouseMutationService.GetObjectById(rollerWarehouseMutationDetail.RollerWarehouseMutationId);

                rollerWarehouseMutationDetail.ConfirmationDate = ConfirmationDate;
                _repository.ConfirmObject(rollerWarehouseMutationDetail);
                
                // Set IsDelivered = true
                CoreIdentificationDetail coreIdentificationDetail = _coreIdentificationDetailService.GetObjectById(rollerWarehouseMutationDetail.CoreIdentificationDetailId);
                _coreIdentificationDetailService.DeliverObject(coreIdentificationDetail, _coreIdentificationService, this);

                // reduce warehouseFromItem
                // add warehouseToItem

                WarehouseItem warehouseItemFrom = _warehouseItemService.FindOrCreateObject(rollerWarehouseMutation.WarehouseFromId, rollerWarehouseMutationDetail.ItemId);
                WarehouseItem warehouseItemTo = _warehouseItemService.FindOrCreateObject(rollerWarehouseMutation.WarehouseToId, rollerWarehouseMutationDetail.ItemId);

                IList<StockMutation> stockMutations = _stockMutationService.CreateStockMutationForRollerWarehouseMutation(rollerWarehouseMutationDetail, warehouseItemFrom, warehouseItemTo);
                foreach (var stockMutation in stockMutations)
                {
                    _stockMutationService.StockMutateObject(stockMutation, _itemService, _barringService, _warehouseItemService);
                }
            }
            return rollerWarehouseMutationDetail;
        }

        public RollerWarehouseMutationDetail UnconfirmObject(RollerWarehouseMutationDetail rollerWarehouseMutationDetail, IRollerWarehouseMutationService _rollerWarehouseMutationService,
                                                            IItemService _itemService, IBarringService _barringService, IWarehouseItemService _warehouseItemService, IStockMutationService _stockMutationService,
                                                            ICoreIdentificationDetailService _coreIdentificationDetailService, ICoreIdentificationService _coreIdentificationService)
        {
            if (_validator.ValidUnconfirmObject(rollerWarehouseMutationDetail, _rollerWarehouseMutationService, _itemService, _barringService, _warehouseItemService))
            {
                _repository.UnconfirmObject(rollerWarehouseMutationDetail);

                // Set IsDelivered = false
                CoreIdentificationDetail coreIdentificationDetail = _coreIdentificationDetailService.GetObjectById(rollerWarehouseMutationDetail.CoreIdentificationDetailId);
                _coreIdentificationDetailService.UndoDeliverObject(coreIdentificationDetail, _coreIdentificationService, this);

                // reverse stock mutate warehouseFromItem and warehouseToItem
                RollerWarehouseMutation rollerWarehouseMutation = _rollerWarehouseMutationService.GetObjectById(rollerWarehouseMutationDetail.RollerWarehouseMutationId);
                WarehouseItem warehouseItemFrom = _warehouseItemService.FindOrCreateObject(rollerWarehouseMutation.WarehouseFromId, rollerWarehouseMutationDetail.ItemId);
                WarehouseItem warehouseItemTo = _warehouseItemService.FindOrCreateObject(rollerWarehouseMutation.WarehouseToId, rollerWarehouseMutationDetail.ItemId);

                IList<StockMutation> stockMutations = _stockMutationService.SoftDeleteStockMutationForRollerWarehouseMutation(rollerWarehouseMutationDetail, warehouseItemFrom, warehouseItemTo);
                foreach (var stockMutation in stockMutations)
                {
                    _stockMutationService.ReverseStockMutateObject(stockMutation, _itemService, _barringService, _warehouseItemService);
                }

            }
            return rollerWarehouseMutationDetail;
        }
    }
}