using Core.DomainModel;
using Core.Interface.Repository;
using Core.Interface.Service;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Service.Service
{
    public class BarringOrderService : IBarringOrderService
    {
        private IBarringOrderRepository _repository;
        private IBarringOrderValidator _validator;
        public BarringOrderService(IBarringOrderRepository _barringOrderRepository, IBarringOrderValidator _barringOrderValidator)
        {
            _repository = _barringOrderRepository;
            _validator = _barringOrderValidator;
        }

        public IBarringOrderValidator GetValidator()
        {
            return _validator;
        }

        public IList<BarringOrder> GetAll()
        {
            return _repository.GetAll();
        }

        public IList<BarringOrder> GetAllObjectsByContactId(int ContactId)
        {
            return _repository.GetAllObjectsByContactId(ContactId);
        }

        public IList<BarringOrder> GetAllObjectsByWarehouseId(int WarehouseId)
        {
            return _repository.GetAllObjectsByWarehouseId(WarehouseId);
        }

        public BarringOrder GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public BarringOrder CreateObject(BarringOrder barringOrder)
        {
            barringOrder.Errors = new Dictionary<String, String>();
            return (_validator.ValidCreateObject(barringOrder, this) ? _repository.CreateObject(barringOrder) : barringOrder);
        }

        public BarringOrder UpdateObject(BarringOrder barringOrder, IBarringOrderDetailService _barringOrderDetailService)
        {
            return (barringOrder = _validator.ValidUpdateObject(barringOrder, _barringOrderDetailService, this) ? _repository.UpdateObject(barringOrder) : barringOrder);
        }

        public BarringOrder SoftDeleteObject(BarringOrder barringOrder, IBarringOrderDetailService _barringOrderDetailService)
        {
            if (_validator.ValidDeleteObject(barringOrder, _barringOrderDetailService))
            {
                ICollection<BarringOrderDetail> details = _barringOrderDetailService.GetObjectsByBarringOrderId(barringOrder.Id);
                foreach (var detail in details)
                {
                    // delete details
                    _barringOrderDetailService.GetRepository().SoftDeleteObject(detail);
                }
                _repository.SoftDeleteObject(barringOrder);
            }
            return barringOrder;
        }

        public BarringOrder ConfirmObject(BarringOrder barringOrder, DateTime ConfirmationDate, IBarringOrderDetailService _barringOrderDetailService, IBarringService _barringService,
                                          IItemService _itemService, IWarehouseItemService _warehouseItemService)
        {
            if (_validator.ValidConfirmObject(barringOrder, _barringOrderDetailService, _barringService, _itemService, _warehouseItemService))
            {
                barringOrder.ConfirmationDate = ConfirmationDate;
                _repository.ConfirmObject(barringOrder);
            }
            return barringOrder;
        }

        public BarringOrder UnconfirmObject(BarringOrder barringOrder, IBarringOrderDetailService _barringOrderDetailService, IBarringService _barringService, IItemService _itemService, IWarehouseItemService _warehouseItemService)
        {
            if (_validator.ValidUnconfirmObject(barringOrder, _barringOrderDetailService))
            {
                _repository.UnconfirmObject(barringOrder);
            }
            return barringOrder;
        }

        public BarringOrder CompleteObject(BarringOrder barringOrder, IBarringOrderDetailService _barringOrderDetailService, IBarringService _barringService, IItemService _itemService, IWarehouseItemService _warehouseItemService)
        {
            if (_validator.ValidCompleteObject(barringOrder, _barringOrderDetailService))
            {
                _repository.CompleteObject(barringOrder);
            }
            return barringOrder;
        }

        public BarringOrder AdjustQuantity(BarringOrder barringOrder)
        {
            return (barringOrder = _validator.ValidAdjustQuantity(barringOrder) ? _repository.AdjustQuantity(barringOrder) : barringOrder);
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }

        public bool IsCodeDuplicated(BarringOrder barringOrder)
        {
            return _repository.IsCodeDuplicated(barringOrder);
        }
    }
}