using Core.DomainModel;
using Core.Interface.Repository;
using Core.Interface.Service;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Service
{
    public class RecoveryOrderDetailService : IRecoveryOrderDetailService
    {
        private IRecoveryOrderDetailRepository _repository;
        private IRecoveryOrderDetailValidator _validator;
        public RecoveryOrderDetailService(IRecoveryOrderDetailRepository _recoveryOrderDetailRepository, IRecoveryOrderDetailValidator _recoveryOrderDetailValidator)
        {
            _repository = _recoveryOrderDetailRepository;
            _validator = _recoveryOrderDetailValidator;
        }

        public IRecoveryOrderDetailValidator GetValidator()
        {
            return _validator;
        }

        public IRecoveryOrderDetailRepository GetRepository()
        {
            return _repository;
        }

        public IList<RecoveryOrderDetail> GetAll()
        {
            return _repository.GetAll();
        }

        public IList<RecoveryOrderDetail> GetObjectsByRecoveryOrderId(int recoveryOrderId)
        {
            return _repository.GetObjectsByRecoveryOrderId(recoveryOrderId);
        }

        public IList<RecoveryOrderDetail> GetObjectsByCoreIdentificationDetailId(int coreIdentificationDetailId)
        {
            return _repository.GetObjectsByCoreIdentificationDetailId(coreIdentificationDetailId);
        }

        public IList<RecoveryOrderDetail> GetObjectsByRollerBuilderId(int rollerBuilderId)
        {
            return _repository.GetObjectsByRollerBuilderId(rollerBuilderId);
        }

        public RecoveryOrderDetail GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public Item GetCore(RecoveryOrderDetail recoveryOrderDetail, ICoreIdentificationDetailService _coreIdentificationDetailService,
                            ICoreBuilderService _coreBuilderService, IItemService _itemService)
        {
            CoreIdentificationDetail coreIdentificationDetail = _coreIdentificationDetailService.GetObjectById(recoveryOrderDetail.CoreIdentificationDetailId);
            Item core = (coreIdentificationDetail.MaterialCase == Core.Constants.Constant.MaterialCase.New) ?
                          _coreBuilderService.GetNewCore(coreIdentificationDetail.CoreBuilderId) : _coreBuilderService.GetUsedCore(coreIdentificationDetail.CoreBuilderId);
            return core;
        }

        public Item GetRoller(RecoveryOrderDetail recoveryOrderDetail, ICoreIdentificationDetailService _coreIdentificationDetailService,
                              IRollerBuilderService _rollerBuilderService, IItemService _itemService)
        {
            CoreIdentificationDetail coreIdentificationDetail = _coreIdentificationDetailService.GetObjectById(recoveryOrderDetail.CoreIdentificationDetailId);
            Item roller = (coreIdentificationDetail.MaterialCase == Core.Constants.Constant.MaterialCase.New) ?
                          _rollerBuilderService.GetNewRoller(recoveryOrderDetail.RollerBuilderId) : _rollerBuilderService.GetUsedRoller(recoveryOrderDetail.RollerBuilderId);
            return roller;
        }

        public RecoveryOrderDetail CreateObject(int CoreIdentificationDetailId, int RollerBuilderId, string CoreTypeCase, string Acc, int RepairRequestCase,
                                                IRecoveryOrderService _recoveryOrderService, ICoreIdentificationDetailService _coreIdentificationDetailService,
                                                IRollerBuilderService _rollerBuilderService)
        {
            RecoveryOrderDetail recoveryOrderDetail = new RecoveryOrderDetail
            {
                CoreIdentificationDetailId = CoreIdentificationDetailId,
                RollerBuilderId = RollerBuilderId,
                CoreTypeCase = CoreTypeCase,
                Acc = Acc,
                RepairRequestCase = RepairRequestCase
            };
            return this.CreateObject(recoveryOrderDetail, _recoveryOrderService, _coreIdentificationDetailService, _rollerBuilderService);
        }

        public RecoveryOrderDetail CreateObject(RecoveryOrderDetail recoveryOrderDetail, IRecoveryOrderService _recoveryOrderService,
                                                ICoreIdentificationDetailService _coreIdentificationDetailService, IRollerBuilderService _rollerBuilderService)
        {
            recoveryOrderDetail.Errors = new Dictionary<String, String>();
            return (recoveryOrderDetail = _validator.ValidCreateObject(recoveryOrderDetail, _recoveryOrderService, _coreIdentificationDetailService, _rollerBuilderService) ?
                                          _repository.CreateObject(recoveryOrderDetail) : recoveryOrderDetail);
        }

        public RecoveryOrderDetail UpdateObject(RecoveryOrderDetail recoveryOrderDetail, IRecoveryOrderService _recoveryOrderService,
                                                ICoreIdentificationDetailService _coreIdentificationDetailService, IRollerBuilderService _rollerBuilderService)
        {
            return (recoveryOrderDetail = _validator.ValidUpdateObject(recoveryOrderDetail, _recoveryOrderService, _coreIdentificationDetailService, _rollerBuilderService) ?
                                          _repository.UpdateObject(recoveryOrderDetail) : recoveryOrderDetail);
        }

        public RecoveryOrderDetail SoftDeleteObject(RecoveryOrderDetail recoveryOrderDetail, IRecoveryOrderService _recoveryOrderService, IRecoveryAccessoryDetailService _recoveryAccessoryDetailService)
        {
            return (recoveryOrderDetail = _validator.ValidDeleteObject(recoveryOrderDetail, _recoveryOrderService, _recoveryAccessoryDetailService) ?
                                          _repository.SoftDeleteObject(recoveryOrderDetail) : recoveryOrderDetail);
        }

        public RecoveryOrderDetail AddAccessory(RecoveryOrderDetail recoveryOrderDetail, IRecoveryAccessoryDetailService _recoveryAccessoryDetailService)
        {
            return (recoveryOrderDetail = _validator.ValidAddAccessory(recoveryOrderDetail, _recoveryAccessoryDetailService) ? _repository.AddAccessory(recoveryOrderDetail) : recoveryOrderDetail);
        }

        public RecoveryOrderDetail RemoveAccessory(RecoveryOrderDetail recoveryOrderDetail, IRecoveryAccessoryDetailService _recoveryAccessoryDetailService)
        {
            return (recoveryOrderDetail = _validator.ValidRemoveAccessory(recoveryOrderDetail, _recoveryAccessoryDetailService) ? _repository.RemoveAccessory(recoveryOrderDetail) : recoveryOrderDetail);
        }

        public RecoveryOrderDetail DisassembleObject(RecoveryOrderDetail recoveryOrderDetail)
        {
            return (recoveryOrderDetail = _validator.ValidDisassembleObject(recoveryOrderDetail) ? _repository.DisassembleObject(recoveryOrderDetail) : recoveryOrderDetail);
        }

        public RecoveryOrderDetail StripAndGlueObject(RecoveryOrderDetail recoveryOrderDetail)
        {
            return (recoveryOrderDetail = _validator.ValidStripAndGlueObject(recoveryOrderDetail) ? _repository.StripAndGlueObject(recoveryOrderDetail) : recoveryOrderDetail);
        }

        public RecoveryOrderDetail WrapObject(RecoveryOrderDetail recoveryOrderDetail)
        {
            return (recoveryOrderDetail = _validator.ValidWrapObject(recoveryOrderDetail) ? _repository.WrapObject(recoveryOrderDetail) : recoveryOrderDetail);
        }

        public RecoveryOrderDetail VulcanizeObject(RecoveryOrderDetail recoveryOrderDetail)
        {
            return (recoveryOrderDetail = _validator.ValidVulcanizeObject(recoveryOrderDetail) ? _repository.VulcanizeObject(recoveryOrderDetail) : recoveryOrderDetail);
        }

        public RecoveryOrderDetail FaceOffObject(RecoveryOrderDetail recoveryOrderDetail)
        {
            return (recoveryOrderDetail = _validator.ValidFaceOffObject(recoveryOrderDetail) ? _repository.FaceOffObject(recoveryOrderDetail) : recoveryOrderDetail);
        }

        public RecoveryOrderDetail ConventionalGrindObject(RecoveryOrderDetail recoveryOrderDetail)
        {
            return (recoveryOrderDetail = _validator.ValidConventionalGrindObject(recoveryOrderDetail) ? _repository.ConventionalGrindObject(recoveryOrderDetail) : recoveryOrderDetail);
        }

        public RecoveryOrderDetail CWCGrindObject(RecoveryOrderDetail recoveryOrderDetail)
        {
            return (recoveryOrderDetail = _validator.ValidCWCGrindObject(recoveryOrderDetail) ? _repository.CWCGrindObject(recoveryOrderDetail) : recoveryOrderDetail);
        }

        public RecoveryOrderDetail PolishAndQCObject(RecoveryOrderDetail recoveryOrderDetail)
        {
            return (recoveryOrderDetail = _validator.ValidPolishAndQCObject(recoveryOrderDetail) ? _repository.PolishAndQCObject(recoveryOrderDetail) : recoveryOrderDetail);
        }

        public RecoveryOrderDetail PackageObject(RecoveryOrderDetail recoveryOrderDetail)
        {
            return (recoveryOrderDetail = _validator.ValidPackageObject(recoveryOrderDetail) ? _repository.PackageObject(recoveryOrderDetail) : recoveryOrderDetail);
        }

        public RecoveryOrderDetail RejectObject(RecoveryOrderDetail recoveryOrderDetail, IRecoveryOrderService _recoveryOrderService)
        {
            return (recoveryOrderDetail = _validator.ValidRejectObject(recoveryOrderDetail, _recoveryOrderService) ?
                                          _repository.RejectObject(recoveryOrderDetail) : recoveryOrderDetail);
        }

        public RecoveryOrderDetail UndoRejectObject(RecoveryOrderDetail recoveryOrderDetail, IRecoveryOrderService _recoveryOrderService)
        {
            return (recoveryOrderDetail = _validator.ValidUndoRejectObject(recoveryOrderDetail, _recoveryOrderService) ?
                                          _repository.UndoRejectObject(recoveryOrderDetail) : recoveryOrderDetail);
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }

    }
}