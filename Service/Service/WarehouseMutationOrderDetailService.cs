using Core.DomainModel;
using Core.Interface.Repository;
using Core.Interface.Service;
using Core.Interface.Validation;
using Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Service
{
    public class WarehouseMutationOrderDetailService : IWarehouseMutationOrderDetailService
    {
        private IWarehouseMutationOrderDetailRepository _repository;
        private IWarehouseMutationOrderDetailValidator _validator;

        public WarehouseMutationOrderDetailService(IWarehouseMutationOrderDetailRepository _warehouseMutationOrderDetailRepository, IWarehouseMutationOrderDetailValidator _warehouseMutationOrderDetailValidator)
        {
            _repository = _warehouseMutationOrderDetailRepository;
            _validator = _warehouseMutationOrderDetailValidator;
        }

        public IWarehouseMutationOrderDetailValidator GetValidator()
        {
            return _validator;
        }

        public IList<WarehouseMutationOrderDetail> GetAll()
        {
            return _repository.GetAll();
        }

        public IList<WarehouseMutationOrderDetail> GetObjectsByWarehouseMutationOrderId(int warehouseMutationOrderId)
        {
            return _repository.GetObjectsByWarehouseMutationOrderId(warehouseMutationOrderId);
        }

        public WarehouseMutationOrderDetail GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public WarehouseMutationOrderDetail CreateObject(WarehouseMutationOrderDetail warehouseMutationOrderDetail, IWarehouseMutationOrderService _warehouseMutationOrderService,
                                                         IItemService _itemService, IWarehouseItemService _warehouseItemService)
        {
            warehouseMutationOrderDetail.Errors = new Dictionary<String, String>();
            return (warehouseMutationOrderDetail = _validator.ValidCreateObject(warehouseMutationOrderDetail, _warehouseMutationOrderService, this, _itemService, _warehouseItemService) ?
                                                   _repository.CreateObject(warehouseMutationOrderDetail) : warehouseMutationOrderDetail);
        }

        public WarehouseMutationOrderDetail CreateObject(int warehouseMutationOrderId, int itemId, int quantity,
                                                    IWarehouseMutationOrderService _warehouseMutationOrderService, IItemService _itemService, IWarehouseItemService _warehouseItemService)
        {
            WarehouseMutationOrderDetail warehouseMutationOrderDetail = new WarehouseMutationOrderDetail
            {
                WarehouseMutationOrderId = warehouseMutationOrderId,
                ItemId = itemId,
                Quantity = quantity,
                // Price = price
            };
            return this.CreateObject(warehouseMutationOrderDetail, _warehouseMutationOrderService, _itemService, _warehouseItemService);
        }

        public WarehouseMutationOrderDetail UpdateObject(WarehouseMutationOrderDetail warehouseMutationOrderDetail, IWarehouseMutationOrderService _warehouseMutationOrderService, IItemService _itemService, IWarehouseItemService _warehouseItemService)
        {
            return (warehouseMutationOrderDetail = _validator.ValidUpdateObject(warehouseMutationOrderDetail, _warehouseMutationOrderService, this, _itemService, _warehouseItemService) ?
                                                   _repository.UpdateObject(warehouseMutationOrderDetail) : warehouseMutationOrderDetail);
        }

        public WarehouseMutationOrderDetail SoftDeleteObject(WarehouseMutationOrderDetail warehouseMutationOrderDetail, IWarehouseMutationOrderService _warehouseMutationOrderService, IWarehouseItemService _warehouseItemService)
        {
            return (warehouseMutationOrderDetail = _validator.ValidDeleteObject(warehouseMutationOrderDetail) ? _repository.SoftDeleteObject(warehouseMutationOrderDetail) : warehouseMutationOrderDetail);
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }

        public WarehouseMutationOrderDetail FinishObject(WarehouseMutationOrderDetail warehouseMutationOrderDetail, IWarehouseMutationOrderService _warehouseMutationOrderService,
                                                         IItemService _itemService, IBarringService _barringService, IWarehouseItemService _warehouseItemService)
        {
            if (_validator.ValidFinishObject(warehouseMutationOrderDetail, _warehouseMutationOrderService, _itemService, _barringService, _warehouseItemService))
            {
                _repository.FinishObject(warehouseMutationOrderDetail);
            }
            return warehouseMutationOrderDetail;
        }

        public WarehouseMutationOrderDetail UnfinishObject(WarehouseMutationOrderDetail warehouseMutationOrderDetail, IWarehouseMutationOrderService _warehouseMutationOrderService,
                                                            IItemService _itemService, IBarringService _barringService, IWarehouseItemService _warehouseItemService)
        {
            if (_validator.ValidUnfinishObject(warehouseMutationOrderDetail, _warehouseMutationOrderService, _itemService, _barringService, _warehouseItemService))
            {
                _repository.UnfinishObject(warehouseMutationOrderDetail);
            }
            return warehouseMutationOrderDetail;
        }
    }
}