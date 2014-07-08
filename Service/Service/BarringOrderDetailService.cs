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
    public class BarringOrderDetailService : IBarringOrderDetailService
    {
        private IBarringOrderDetailRepository _repository;
        private IBarringOrderDetailValidator _validator;
        public BarringOrderDetailService(IBarringOrderDetailRepository _barringOrderDetailRepository, IBarringOrderDetailValidator _barringOrderDetailValidator)
        {
            _repository = _barringOrderDetailRepository;
            _validator = _barringOrderDetailValidator;
        }

        public IBarringOrderDetailValidator GetValidator()
        {
            return _validator;
        }

        public IBarringOrderDetailRepository GetRepository()
        {
            return _repository;
        }

        public IList<BarringOrderDetail> GetAll()
        {
            return _repository.GetAll();
        }

        public IList<BarringOrderDetail> GetObjectsByBarringOrderId(int barringOrderId)
        {
            return _repository.GetObjectsByBarringOrderId(barringOrderId);
        }

        public BarringOrderDetail GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public Barring GetBarring(BarringOrderDetail barringOrderDetail, IBarringService _barringService)
        {
            return _barringService.GetObjectById(barringOrderDetail.BarringId);
        }
        
        public BarringOrderDetail CreateObject(BarringOrderDetail barringOrderDetail, IBarringOrderService _barringOrderService, IBarringService _barringService)
        {
            barringOrderDetail.Errors = new Dictionary<String, String>();
            return (barringOrderDetail = _validator.ValidCreateObject(barringOrderDetail, _barringOrderService, _barringService) ?
                                          _repository.CreateObject(barringOrderDetail) : barringOrderDetail);
        }

        public BarringOrderDetail UpdateObject(BarringOrderDetail barringOrderDetail, IBarringOrderService _barringOrderService, IBarringService _barringService)
        {
            return (barringOrderDetail = _validator.ValidUpdateObject(barringOrderDetail, _barringOrderService, _barringService) ?
                                          _repository.UpdateObject(barringOrderDetail) : barringOrderDetail);
        }

        public BarringOrderDetail SoftDeleteObject(BarringOrderDetail barringOrderDetail, IBarringOrderService _barringOrderService)
        {
            return (barringOrderDetail = _validator.ValidDeleteObject(barringOrderDetail, _barringOrderService) ?
                                          _repository.SoftDeleteObject(barringOrderDetail) : barringOrderDetail);
        }

        public BarringOrderDetail AddLeftBar(BarringOrderDetail barringOrderDetail, IBarringService _barringService)
        {
            return (barringOrderDetail = _validator.ValidAddLeftBar(barringOrderDetail) ?
                                         _repository.AddLeftBar(barringOrderDetail) : barringOrderDetail);
        }

        public BarringOrderDetail RemoveLeftBar(BarringOrderDetail barringOrderDetail, IBarringService _barringService)
        {
            return (barringOrderDetail = _validator.ValidRemoveLeftBar(barringOrderDetail) ?
                                         _repository.RemoveLeftBar(barringOrderDetail) : barringOrderDetail);
        }

        public BarringOrderDetail AddRightBar(BarringOrderDetail barringOrderDetail, IBarringService _barringService)
        {
            return (barringOrderDetail = _validator.ValidAddRightBar(barringOrderDetail) ?
                                         _repository.AddRightBar(barringOrderDetail) : barringOrderDetail);
        }

        public BarringOrderDetail RemoveRightBar(BarringOrderDetail barringOrderDetail, IBarringService _barringService)
        {
            return (barringOrderDetail = _validator.ValidRemoveRightBar(barringOrderDetail) ?
                                         _repository.RemoveRightBar(barringOrderDetail) : barringOrderDetail);
        }

        public BarringOrderDetail CutObject(BarringOrderDetail barringOrderDetail)
        {
            return (barringOrderDetail = _validator.ValidCutObject(barringOrderDetail) ? _repository.CutObject(barringOrderDetail) : barringOrderDetail);
        }

        public BarringOrderDetail SideSealObject(BarringOrderDetail barringOrderDetail)
        {
            return (barringOrderDetail = _validator.ValidSideSealObject(barringOrderDetail) ? _repository.SideSealObject(barringOrderDetail) : barringOrderDetail);
        }

        public BarringOrderDetail PrepareObject(BarringOrderDetail barringOrderDetail)
        {
            return (barringOrderDetail = _validator.ValidPrepareObject(barringOrderDetail) ? _repository.PrepareObject(barringOrderDetail) : barringOrderDetail);
        }

        public BarringOrderDetail ApplyTapeAdhesiveToObject(BarringOrderDetail barringOrderDetail)
        {
            return (barringOrderDetail = _validator.ValidApplyTapeAdhesiveToObject(barringOrderDetail) ? _repository.ApplyTapeAdhesiveToObject(barringOrderDetail) : barringOrderDetail);
        }

        public BarringOrderDetail MountObject(BarringOrderDetail barringOrderDetail)
        {
            return (barringOrderDetail = _validator.ValidMountObject(barringOrderDetail) ? _repository.MountObject(barringOrderDetail) : barringOrderDetail);
        }

        public BarringOrderDetail HeatPressObject(BarringOrderDetail barringOrderDetail)
        {
            return (barringOrderDetail = _validator.ValidHeatPressObject(barringOrderDetail) ? _repository.HeatPressObject(barringOrderDetail) : barringOrderDetail);
        }

        public BarringOrderDetail PullOffTestObject(BarringOrderDetail barringOrderDetail)
        {
            return (barringOrderDetail = _validator.ValidPullOffTestObject(barringOrderDetail) ? _repository.PullOffTestObject(barringOrderDetail) : barringOrderDetail);
        }

        public BarringOrderDetail QCAndMarkObject(BarringOrderDetail barringOrderDetail)
        {
            return (barringOrderDetail = _validator.ValidQCAndMarkObject(barringOrderDetail) ? _repository.QCAndMarkObject(barringOrderDetail) : barringOrderDetail);
        }

        public BarringOrderDetail PackageObject(BarringOrderDetail barringOrderDetail)
        {
            return (barringOrderDetail = _validator.ValidPackageObject(barringOrderDetail) ? _repository.PackageObject(barringOrderDetail) : barringOrderDetail);
        }

        public BarringOrderDetail RejectObject(BarringOrderDetail barringOrderDetail, IBarringOrderService _barringOrderService)
        {
            return (barringOrderDetail = _validator.ValidRejectObject(barringOrderDetail, _barringOrderService) ?
                                          _repository.RejectObject(barringOrderDetail) : barringOrderDetail);
        }

        public BarringOrderDetail UndoRejectObject(BarringOrderDetail barringOrderDetail, IBarringOrderService _barringOrderService)
        {
            return (barringOrderDetail = _validator.ValidUndoRejectObject(barringOrderDetail, _barringOrderService) ?
                                          _repository.UndoRejectObject(barringOrderDetail) : barringOrderDetail);
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }

    }
}