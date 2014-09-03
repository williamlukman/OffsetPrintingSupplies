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
    public class RecoveryAccessoryDetailService : IRecoveryAccessoryDetailService
    {
        private IRecoveryAccessoryDetailRepository _repository;
        private IRecoveryAccessoryDetailValidator _validator;
        public RecoveryAccessoryDetailService(IRecoveryAccessoryDetailRepository _recoveryAccessoryDetailRepository, IRecoveryAccessoryDetailValidator _recoveryAccessoryDetailValidator)
        {
            _repository = _recoveryAccessoryDetailRepository;
            _validator = _recoveryAccessoryDetailValidator;
        }

        public IRecoveryAccessoryDetailValidator GetValidator()
        {
            return _validator;
        }

        public IRecoveryAccessoryDetailRepository GetRepository()
        {
            return _repository;
        }

        public IQueryable<RecoveryAccessoryDetail> GetQueryable()
        {
            return _repository.GetQueryable();
        }

        public IList<RecoveryAccessoryDetail> GetAll()
        {
            return _repository.GetAll();
        }

        public IList<RecoveryAccessoryDetail> GetObjectsByRecoveryOrderDetailId(int recoveryOrderDetailId)
        {
            return _repository.GetObjectsByRecoveryOrderDetailId(recoveryOrderDetailId);
        }

        public IList<RecoveryAccessoryDetail> GetObjectsByItemId(int ItemId)
        {
            return _repository.GetObjectsByItemId(ItemId);
        }

        public RecoveryAccessoryDetail GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public RecoveryAccessoryDetail CreateObject(int RecoveryOrderDetailId, int ItemId, int Quantity,
                                                    IRecoveryOrderService _recoveryOrderService, IRecoveryOrderDetailService _recoveryOrderDetailService, 
                                                    IItemService _itemService, IItemTypeService _itemTypeService, IWarehouseItemService _warehouseItemService)
        {
            RecoveryAccessoryDetail recoveryAccessoryDetail = new RecoveryAccessoryDetail
            {
                RecoveryOrderDetailId = RecoveryOrderDetailId,
                ItemId = ItemId,
                Quantity = Quantity
            };
            return this.CreateObject(recoveryAccessoryDetail, _recoveryOrderService, _recoveryOrderDetailService, _itemService, _itemTypeService, _warehouseItemService);
        }

        public RecoveryAccessoryDetail CreateObject(RecoveryAccessoryDetail recoveryAccessoryDetail, IRecoveryOrderService _recoveryOrderService, IRecoveryOrderDetailService _recoveryOrderDetailService,
                                                    IItemService _itemService, IItemTypeService _itemTypeService, IWarehouseItemService _warehouseItemService)
        {
            recoveryAccessoryDetail.Errors = new Dictionary<String, String>();
            return (recoveryAccessoryDetail = _validator.ValidCreateObject(recoveryAccessoryDetail, _recoveryOrderService, _recoveryOrderDetailService, _itemService, _itemTypeService, _warehouseItemService) ?
                                              _repository.CreateObject(recoveryAccessoryDetail) : recoveryAccessoryDetail);
        }

        public RecoveryAccessoryDetail UpdateObject(RecoveryAccessoryDetail recoveryAccessoryDetail, IRecoveryOrderService _recoveryOrderService, IRecoveryOrderDetailService _recoveryOrderDetailService,
                                                    IItemService _itemService, IItemTypeService _itemTypeService, IWarehouseItemService _warehouseItemService)
        {
            return (recoveryAccessoryDetail = _validator.ValidUpdateObject(recoveryAccessoryDetail, _recoveryOrderService, _recoveryOrderDetailService, _itemService, _itemTypeService, _warehouseItemService) ?
                                              _repository.UpdateObject(recoveryAccessoryDetail) : recoveryAccessoryDetail);
        }

        public RecoveryAccessoryDetail SoftDeleteObject(RecoveryAccessoryDetail recoveryAccessoryDetail, IRecoveryOrderDetailService _recoveryOrderDetailService)
        {
            return (recoveryAccessoryDetail = _validator.ValidDeleteObject(recoveryAccessoryDetail, _recoveryOrderDetailService) ? _repository.SoftDeleteObject(recoveryAccessoryDetail) : recoveryAccessoryDetail);
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }
    }
}