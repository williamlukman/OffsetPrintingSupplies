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
    public class VirtualOrderDetailService : IVirtualOrderDetailService
    {
        private IVirtualOrderDetailRepository _repository;
        private IVirtualOrderDetailValidator _validator;

        public VirtualOrderDetailService(IVirtualOrderDetailRepository _virtualOrderDetailRepository, IVirtualOrderDetailValidator _virtualOrderDetailValidator)
        {
            _repository = _virtualOrderDetailRepository;
            _validator = _virtualOrderDetailValidator;
        }

        public IVirtualOrderDetailValidator GetValidator()
        {
            return _validator;
        }

        public IQueryable<VirtualOrderDetail> GetQueryable()
        {
            return _repository.GetQueryable();
        }

        public IList<VirtualOrderDetail> GetAll()
        {
            return _repository.GetAll();
        }

        public IList<VirtualOrderDetail> GetObjectsByVirtualOrderId(int virtualOrderId)
        {
            return _repository.GetObjectsByVirtualOrderId(virtualOrderId);
        }

        public IList<VirtualOrderDetail> GetObjectsByItemId(int itemId)
        {
            return _repository.GetObjectsByItemId(itemId);
        }

        public VirtualOrderDetail GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public VirtualOrderDetail CreateObject(VirtualOrderDetail virtualOrderDetail, IVirtualOrderService _virtualOrderService, IItemService _itemService)
        {
            virtualOrderDetail.Errors = new Dictionary<String, String>();
            if (_validator.ValidCreateObject(virtualOrderDetail, this, _virtualOrderService, _itemService))
            {
                _repository.CreateObject(virtualOrderDetail);
            }
            return virtualOrderDetail;
        }

        public VirtualOrderDetail UpdateObject(VirtualOrderDetail virtualOrderDetail, IVirtualOrderService _virtualOrderService, IItemService _itemService)
        {
            return (_validator.ValidUpdateObject(virtualOrderDetail, this, _virtualOrderService, _itemService) ? _repository.UpdateObject(virtualOrderDetail) : virtualOrderDetail);
        }

        public VirtualOrderDetail SoftDeleteObject(VirtualOrderDetail virtualOrderDetail)
        {
            return (_validator.ValidDeleteObject(virtualOrderDetail) ? _repository.SoftDeleteObject(virtualOrderDetail) : virtualOrderDetail);
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }

        public VirtualOrderDetail ConfirmObject(VirtualOrderDetail virtualOrderDetail, DateTime ConfirmationDate, IStockMutationService _stockMutationService,
                                             IItemService _itemService, IBlanketService _blanketService, IWarehouseItemService _warehouseItemService)
        {
            virtualOrderDetail.ConfirmationDate = ConfirmationDate;
            if (_validator.ValidConfirmObject(virtualOrderDetail))
            {
                virtualOrderDetail = _repository.ConfirmObject(virtualOrderDetail);

                Item item = _itemService.GetObjectById(virtualOrderDetail.ItemId);
                StockMutation stockMutation = _stockMutationService.CreateStockMutationForVirtualOrder(virtualOrderDetail, item);
                _stockMutationService.StockMutateObject(stockMutation, _itemService, _blanketService, _warehouseItemService);
                // item.PendingDelivery += virtualOrderDetail.Quantity;
            }
            return virtualOrderDetail;
        }

        public VirtualOrderDetail UnconfirmObject(VirtualOrderDetail virtualOrderDetail, IVirtualOrderService _virtualOrderService, ITemporaryDeliveryOrderDetailService _temporaryDeliveryOrderDetailService,
                                               IStockMutationService _stockMutationService, IItemService _itemService, IBlanketService _blanketService, IWarehouseItemService _warehouseItemService)
        {
            if (_validator.ValidUnconfirmObject(virtualOrderDetail, _temporaryDeliveryOrderDetailService, _itemService))
            {
                virtualOrderDetail = _repository.UnconfirmObject(virtualOrderDetail);
                Item item = _itemService.GetObjectById(virtualOrderDetail.ItemId);
                IList<StockMutation> stockMutations = _stockMutationService.SoftDeleteStockMutationForVirtualOrder(virtualOrderDetail, item);
                foreach (var stockMutation in stockMutations)
                {
                    _stockMutationService.ReverseStockMutateObject(stockMutation, _itemService, _blanketService, _warehouseItemService);
                    //item.PendingDelivery -= virtualOrderDetail.Quantity;
                }
            }
            return virtualOrderDetail;
        }

        public VirtualOrderDetail SetDeliveryComplete(VirtualOrderDetail virtualOrderDetail, int Quantity)
        {
            virtualOrderDetail.PendingDeliveryQuantity -= Quantity;
            if (virtualOrderDetail.PendingDeliveryQuantity == 0) { virtualOrderDetail.IsAllDelivered = true; }
            _repository.UpdateObject(virtualOrderDetail);
            return virtualOrderDetail;
        }

        public VirtualOrderDetail UnsetDeliveryComplete(VirtualOrderDetail virtualOrderDetail, int Quantity, IVirtualOrderService _virtualOrderService)
        {
            VirtualOrder virtualOrder = _virtualOrderService.GetObjectById(virtualOrderDetail.VirtualOrderId);
            _virtualOrderService.UnsetDeliveryComplete(virtualOrder);

            virtualOrderDetail.IsAllDelivered = false;
            virtualOrderDetail.PendingDeliveryQuantity += Quantity;
            _repository.UpdateObject(virtualOrderDetail);
            return virtualOrderDetail;
        }
    }
}