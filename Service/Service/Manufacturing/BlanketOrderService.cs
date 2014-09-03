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
    public class BlanketOrderService : IBlanketOrderService
    {
        private IBlanketOrderRepository _repository;
        private IBlanketOrderValidator _validator;
        public BlanketOrderService(IBlanketOrderRepository _blanketOrderRepository, IBlanketOrderValidator _blanketOrderValidator)
        {
            _repository = _blanketOrderRepository;
            _validator = _blanketOrderValidator;
        }

        public IBlanketOrderValidator GetValidator()
        {
            return _validator;
        }

        public IQueryable<BlanketOrder> GetQueryable()
        {
            return _repository.GetQueryable();
        }

        public IList<BlanketOrder> GetAll()
        {
            return _repository.GetAll();
        }

        public IList<BlanketOrder> GetAllObjectsByContactId(int ContactId)
        {
            return _repository.GetAllObjectsByContactId(ContactId);
        }

        public IList<BlanketOrder> GetAllObjectsByWarehouseId(int WarehouseId)
        {
            return _repository.GetAllObjectsByWarehouseId(WarehouseId);
        }

        public BlanketOrder GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public BlanketOrder CreateObject(BlanketOrder blanketOrder)
        {
            blanketOrder.Errors = new Dictionary<String, String>();
            return (_validator.ValidCreateObject(blanketOrder, this) ? _repository.CreateObject(blanketOrder) : blanketOrder);
        }

        public BlanketOrder UpdateObject(BlanketOrder blanketOrder, IBlanketOrderDetailService _blanketOrderDetailService)
        {
            return (blanketOrder = _validator.ValidUpdateObject(blanketOrder, _blanketOrderDetailService, this) ? _repository.UpdateObject(blanketOrder) : blanketOrder);
        }

        public BlanketOrder SoftDeleteObject(BlanketOrder blanketOrder, IBlanketOrderDetailService _blanketOrderDetailService)
        {
            if (_validator.ValidDeleteObject(blanketOrder, _blanketOrderDetailService))
            {
                ICollection<BlanketOrderDetail> details = _blanketOrderDetailService.GetObjectsByBlanketOrderId(blanketOrder.Id);
                foreach (var detail in details)
                {
                    // delete details
                    _blanketOrderDetailService.GetRepository().SoftDeleteObject(detail);
                }
                _repository.SoftDeleteObject(blanketOrder);
            }
            return blanketOrder;
        }

        public BlanketOrder ConfirmObject(BlanketOrder blanketOrder, DateTime ConfirmationDate, IBlanketOrderDetailService _blanketOrderDetailService, IBlanketService _blanketService,
                                          IItemService _itemService, IWarehouseItemService _warehouseItemService)
        {
            blanketOrder.ConfirmationDate = ConfirmationDate;
            if (_validator.ValidConfirmObject(blanketOrder, _blanketOrderDetailService, _blanketService, _itemService, _warehouseItemService))
            {
                _repository.ConfirmObject(blanketOrder);
            }
            return blanketOrder;
        }

        public BlanketOrder UnconfirmObject(BlanketOrder blanketOrder, IBlanketOrderDetailService _blanketOrderDetailService, IBlanketService _blanketService, IItemService _itemService, IWarehouseItemService _warehouseItemService)
        {
            if (_validator.ValidUnconfirmObject(blanketOrder, _blanketOrderDetailService))
            {
                _repository.UnconfirmObject(blanketOrder);
            }
            return blanketOrder;
        }

        public BlanketOrder CompleteObject(BlanketOrder blanketOrder, IBlanketOrderDetailService _blanketOrderDetailService, IBlanketService _blanketService, IItemService _itemService, IWarehouseItemService _warehouseItemService)
        {
            if (_validator.ValidCompleteObject(blanketOrder, _blanketOrderDetailService))
            {
                _repository.CompleteObject(blanketOrder);
            }
            return blanketOrder;
        }

        public BlanketOrder AdjustQuantity(BlanketOrder blanketOrder)
        {
            return (blanketOrder = _validator.ValidAdjustQuantity(blanketOrder) ? _repository.AdjustQuantity(blanketOrder) : blanketOrder);
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }

        public bool IsCodeDuplicated(BlanketOrder blanketOrder)
        {
            return _repository.IsCodeDuplicated(blanketOrder);
        }
    }
}