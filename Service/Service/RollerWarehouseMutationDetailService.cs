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

        public RollerWarehouseMutationDetail FinishObject(RollerWarehouseMutationDetail rollerWarehouseMutationDetail, IRollerWarehouseMutationService _rollerWarehouseMutationService,
                                                         IItemService _itemService, IBarringService _barringService, IWarehouseItemService _warehouseItemService)
        {
            if (_validator.ValidFinishObject(rollerWarehouseMutationDetail, _rollerWarehouseMutationService, _itemService, _barringService, _warehouseItemService))
            {
                _repository.FinishObject(rollerWarehouseMutationDetail);
            }
            return rollerWarehouseMutationDetail;
        }

        public RollerWarehouseMutationDetail UnfinishObject(RollerWarehouseMutationDetail rollerWarehouseMutationDetail, IRollerWarehouseMutationService _rollerWarehouseMutationService,
                                                            IItemService _itemService, IBarringService _barringService, IWarehouseItemService _warehouseItemService)
        {
            if (_validator.ValidUnfinishObject(rollerWarehouseMutationDetail, _rollerWarehouseMutationService, _itemService, _barringService, _warehouseItemService))
            {
                _repository.UnfinishObject(rollerWarehouseMutationDetail);
            }
            return rollerWarehouseMutationDetail;
        }
    }
}