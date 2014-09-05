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
    public class CoreAccessoryDetailService : ICoreAccessoryDetailService
    {
        private ICoreAccessoryDetailRepository _repository;
        private ICoreAccessoryDetailValidator _validator;

        public CoreAccessoryDetailService(ICoreAccessoryDetailRepository _coreAccessoryDetailRepository, ICoreAccessoryDetailValidator _coreAccessoryDetailValidator)
        {
            _repository = _coreAccessoryDetailRepository;
            _validator = _coreAccessoryDetailValidator;
        }

        public ICoreAccessoryDetailValidator GetValidator()
        {
            return _validator;
        }

        public ICoreAccessoryDetailRepository GetRepository()
        {
            return _repository;
        }

        public IQueryable<CoreAccessoryDetail> GetQueryable()
        {
            return _repository.GetQueryable();
        }

        public IList<CoreAccessoryDetail> GetAll()
        {
            return _repository.GetAll();
        }

        public IList<CoreAccessoryDetail> GetObjectsByCoreIdentificationDetailId(int coreIdentificationDetailId)
        {
            return _repository.GetObjectsByCoreIdentificationDetailId(coreIdentificationDetailId);
        }

        public IList<CoreAccessoryDetail> GetObjectsByItemId(int ItemId)
        {
            return _repository.GetObjectsByItemId(ItemId);
        }

        public CoreAccessoryDetail GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public CoreAccessoryDetail CreateObject(int CoreIdentificationDetailId, int ItemId, int Quantity,
                                                    ICoreIdentificationService _coreIdentificationService, ICoreIdentificationDetailService _coreIdentificationDetailService,
                                                    IItemService _itemService, IItemTypeService _itemTypeService, IWarehouseItemService _warehouseItemService, ICoreAccessoryDetailService _coreAccessoryDetailService)
        {
            CoreAccessoryDetail coreAccessoryDetail = new CoreAccessoryDetail
            {
                CoreIdentificationDetailId = CoreIdentificationDetailId,
                ItemId = ItemId,
                Quantity = Quantity
            };
            return this.CreateObject(coreAccessoryDetail, _coreIdentificationService, _coreIdentificationDetailService, _itemService, _itemTypeService, _warehouseItemService, _coreAccessoryDetailService);
        }

        public CoreAccessoryDetail CreateObject(CoreAccessoryDetail coreAccessoryDetail, ICoreIdentificationService _coreIdentificationService, ICoreIdentificationDetailService _coreIdentificationDetailService,
                                                    IItemService _itemService, IItemTypeService _itemTypeService, IWarehouseItemService _warehouseItemService,ICoreAccessoryDetailService _coreAccessoryDetailService)
        {
            coreAccessoryDetail.Errors = new Dictionary<String, String>();
            return (coreAccessoryDetail = _validator.ValidCreateObject(coreAccessoryDetail, _coreIdentificationService, _coreIdentificationDetailService, _itemService, _itemTypeService, _warehouseItemService,_coreAccessoryDetailService) ?
                                              _repository.CreateObject(coreAccessoryDetail) : coreAccessoryDetail);
        }

        public CoreAccessoryDetail UpdateObject(CoreAccessoryDetail coreAccessoryDetail, ICoreIdentificationService _coreIdentificationService, ICoreIdentificationDetailService _coreIdentificationDetailService,
                                                    IItemService _itemService, IItemTypeService _itemTypeService, IWarehouseItemService _warehouseItemService, ICoreAccessoryDetailService _coreAccessoryDetailService)
        {
            return (coreAccessoryDetail = _validator.ValidUpdateObject(coreAccessoryDetail, _coreIdentificationService, _coreIdentificationDetailService, _itemService, _itemTypeService, _warehouseItemService,_coreAccessoryDetailService) ?
                                              _repository.UpdateObject(coreAccessoryDetail) : coreAccessoryDetail);
        }

        public CoreAccessoryDetail SoftDeleteObject(CoreAccessoryDetail coreAccessoryDetail, ICoreIdentificationDetailService _coreIdentificationDetailService)
        {
            return (coreAccessoryDetail = _validator.ValidDeleteObject(coreAccessoryDetail, _coreIdentificationDetailService) ? _repository.SoftDeleteObject(coreAccessoryDetail) : coreAccessoryDetail);
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }
    }
}