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
    public class WarehouseMutationDetailService : IWarehouseMutationDetailService
    {
        private IWarehouseMutationDetailRepository _repository;
        private IWarehouseMutationDetailValidator _validator;

        public WarehouseMutationDetailService(IWarehouseMutationDetailRepository _WarehouseMutationDetailRepository, IWarehouseMutationDetailValidator _WarehouseMutationDetailValidator)
        {
            _repository = _WarehouseMutationDetailRepository;
            _validator = _WarehouseMutationDetailValidator;
        }

        public IWarehouseMutationDetailValidator GetValidator()
        {
            return _validator;
        }

        public IQueryable<WarehouseMutationDetail> GetQueryable()
        {
            return _repository.GetQueryable();
        }

        public IList<WarehouseMutationDetail> GetAll()
        {
            return _repository.GetAll();
        }

        public IList<WarehouseMutationDetail> GetObjectsByWarehouseMutationId(int WarehouseMutationId)
        {
            return _repository.GetObjectsByWarehouseMutationId(WarehouseMutationId);
        }

        public WarehouseMutationDetail GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public WarehouseMutationDetail CreateObject(WarehouseMutationDetail WarehouseMutationDetail, IWarehouseMutationService _WarehouseMutationService,
                                                    IItemService _itemService, IWarehouseItemService _warehouseItemService, IBlanketService _blanketService)
        {
            WarehouseMutationDetail.Errors = new Dictionary<String, String>();
            return (WarehouseMutationDetail = _validator.ValidCreateObject(WarehouseMutationDetail, _WarehouseMutationService, this, _itemService, _warehouseItemService, _blanketService) ?
                                              _repository.CreateObject(WarehouseMutationDetail) : WarehouseMutationDetail);
        }

        public WarehouseMutationDetail UpdateObject(WarehouseMutationDetail WarehouseMutationDetail, IWarehouseMutationService _WarehouseMutationService, IItemService _itemService,
                                                    IWarehouseItemService _warehouseItemService, IBlanketService _blanketService)
        {
            return (WarehouseMutationDetail = _validator.ValidUpdateObject(WarehouseMutationDetail, _WarehouseMutationService, this, _itemService, _warehouseItemService, _blanketService) ?
                                              _repository.UpdateObject(WarehouseMutationDetail) : WarehouseMutationDetail);
        }

        public WarehouseMutationDetail SoftDeleteObject(WarehouseMutationDetail WarehouseMutationDetail, IWarehouseMutationService _WarehouseMutationService, IWarehouseItemService _warehouseItemService)
        {
            return (WarehouseMutationDetail = _validator.ValidDeleteObject(WarehouseMutationDetail) ? _repository.SoftDeleteObject(WarehouseMutationDetail) : WarehouseMutationDetail);
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }

        public WarehouseMutationDetail ConfirmObject(WarehouseMutationDetail WarehouseMutationDetail, DateTime ConfirmationDate, IWarehouseMutationService _WarehouseMutationService,
                                                         IItemService _itemService, IBlanketService _blanketService, IWarehouseItemService _warehouseItemService, IStockMutationService _stockMutationService)
        {
            WarehouseMutationDetail.ConfirmationDate = ConfirmationDate;
            if (_validator.ValidConfirmObject(WarehouseMutationDetail, _WarehouseMutationService, _itemService, _blanketService, _warehouseItemService))
            {
                WarehouseMutation WarehouseMutation = _WarehouseMutationService.GetObjectById(WarehouseMutationDetail.WarehouseMutationId);

                _repository.ConfirmObject(WarehouseMutationDetail);

                // deduce warehouseFrom item
                // add warehouseTo item
                WarehouseItem warehouseItemFrom = _warehouseItemService.FindOrCreateObject(WarehouseMutation.WarehouseFromId, WarehouseMutationDetail.ItemId);
                WarehouseItem warehouseItemTo = _warehouseItemService.FindOrCreateObject(WarehouseMutation.WarehouseToId, WarehouseMutationDetail.ItemId);

                IList<StockMutation> stockMutations = _stockMutationService.CreateStockMutationForWarehouseMutation(WarehouseMutationDetail, warehouseItemFrom, warehouseItemTo);
                foreach (var stockMutation in stockMutations)
                {
                    _stockMutationService.StockMutateObject(stockMutation, _itemService, _blanketService, _warehouseItemService);
                }
            }
            return WarehouseMutationDetail;
        }

        public WarehouseMutationDetail UnconfirmObject(WarehouseMutationDetail WarehouseMutationDetail, IWarehouseMutationService _WarehouseMutationService,
                                                            IItemService _itemService, IBlanketService _blanketService, IWarehouseItemService _warehouseItemService, IStockMutationService _stockMutationService)
        {
            if (_validator.ValidUnconfirmObject(WarehouseMutationDetail, _WarehouseMutationService, _itemService, _blanketService, _warehouseItemService))
            {
                _repository.UnconfirmObject(WarehouseMutationDetail);

                // reverse mutate item in warehousefrom and warehouseto
                WarehouseMutation WarehouseMutation = _WarehouseMutationService.GetObjectById(WarehouseMutationDetail.WarehouseMutationId);
                WarehouseItem warehouseItemFrom = _warehouseItemService.FindOrCreateObject(WarehouseMutation.WarehouseFromId, WarehouseMutationDetail.ItemId);
                WarehouseItem warehouseItemTo = _warehouseItemService.FindOrCreateObject(WarehouseMutation.WarehouseToId, WarehouseMutationDetail.ItemId);

                IList<StockMutation> stockMutations = _stockMutationService.SoftDeleteStockMutationForWarehouseMutation(WarehouseMutationDetail, warehouseItemFrom, warehouseItemTo);
                foreach (var stockMutation in stockMutations)
                {
                    _stockMutationService.ReverseStockMutateObject(stockMutation, _itemService, _blanketService, _warehouseItemService);
                }
            }
            return WarehouseMutationDetail;
        }
    }
}