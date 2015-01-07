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
    public class ReceiptRequestDetailService : IReceiptRequestDetailService
    {
        private IReceiptRequestDetailRepository _repository;
        private IReceiptRequestDetailValidator _validator;

        public ReceiptRequestDetailService(IReceiptRequestDetailRepository _ReceiptRequestDetailRepository, IReceiptRequestDetailValidator _ReceiptRequestDetailValidator)
        {
            _repository = _ReceiptRequestDetailRepository;
            _validator = _ReceiptRequestDetailValidator;
        }

        public IReceiptRequestDetailValidator GetValidator()
        {
            return _validator;
        }

        public IQueryable<ReceiptRequestDetail> GetQueryable()
        {
            return _repository.GetQueryable();
        }

        public IList<ReceiptRequestDetail> GetAll()
        {
            return _repository.GetAll();
        }

        public IList<ReceiptRequestDetail> GetObjectsByReceiptRequestId(int ReceiptRequestId)
        {
            return _repository.GetObjectsByReceiptRequestId(ReceiptRequestId);
        }

        public IList<ReceiptRequestDetail> GetNonLegacyObjectsByReceiptRequestId(int ReceiptRequestId)
        {
            return _repository.GetNonLegacyObjectsByReceiptRequestId(ReceiptRequestId);
        }

        public ReceiptRequestDetail GetLegacyObjectByReceiptRequestId(int ReceiptRequestId)
        {
            return _repository.GetLegacyObjectByReceiptRequestId(ReceiptRequestId);
        }
        
        public ReceiptRequestDetail GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public ReceiptRequestDetail CreateObject(ReceiptRequestDetail ReceiptRequestDetail, IReceiptRequestService _ReceiptRequestService, IAccountService _accountService)
        {
            ReceiptRequestDetail.Errors = new Dictionary<String, String>();
            if (_validator.ValidCreateObject(ReceiptRequestDetail, _ReceiptRequestService, this, _accountService))
            {
                ReceiptRequestDetail =  _repository.CreateObject(ReceiptRequestDetail);
                ReceiptRequest ReceiptRequest = _ReceiptRequestService.GetObjectById(ReceiptRequestDetail.ReceiptRequestId);
                _ReceiptRequestService.CalculateTotalAmount(ReceiptRequest, this);
            }
            return ReceiptRequestDetail;
        }

        public ReceiptRequestDetail UpdateObject(ReceiptRequestDetail ReceiptRequestDetail, IReceiptRequestService _ReceiptRequestService, IAccountService _accountService)
        {
            if (_validator.ValidUpdateObject(ReceiptRequestDetail, _ReceiptRequestService, this, _accountService))
            {
                ReceiptRequestDetail = _repository.UpdateObject(ReceiptRequestDetail);
                ReceiptRequest ReceiptRequest = _ReceiptRequestService.GetObjectById(ReceiptRequestDetail.ReceiptRequestId);
                _ReceiptRequestService.CalculateTotalAmount(ReceiptRequest, this);
            }
            return ReceiptRequestDetail;
        }

        public ReceiptRequestDetail CreateLegacyObject(ReceiptRequestDetail ReceiptRequestDetail, IReceiptRequestService _ReceiptRequestService, IAccountService _accountService)
        {
            ReceiptRequestDetail.Errors = new Dictionary<String, String>();
            return (_validator.ValidCreateLegacyObject(ReceiptRequestDetail, _ReceiptRequestService, this, _accountService) ?
                    _repository.CreateObject(ReceiptRequestDetail) : ReceiptRequestDetail);
        }

        public ReceiptRequestDetail UpdateLegacyObject(ReceiptRequestDetail ReceiptRequestDetail, IReceiptRequestService _ReceiptRequestService, IAccountService _accountService)
        {
            return (_validator.ValidUpdateLegacyObject(ReceiptRequestDetail, _ReceiptRequestService, this, _accountService) ?
                     _repository.UpdateObject(ReceiptRequestDetail) : ReceiptRequestDetail);
        }

        public ReceiptRequestDetail SoftDeleteObject(ReceiptRequestDetail ReceiptRequestDetail,IReceiptRequestService _ReceiptRequestService)
        {
            if (_validator.ValidDeleteObject(ReceiptRequestDetail))
            {
                ReceiptRequestDetail = _repository.SoftDeleteObject(ReceiptRequestDetail);
                ReceiptRequest ReceiptRequest = _ReceiptRequestService.GetObjectById(ReceiptRequestDetail.ReceiptRequestId);
                _ReceiptRequestService.CalculateTotalAmount(ReceiptRequest, this);
            }

            return ReceiptRequestDetail;
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }

        public ReceiptRequestDetail ConfirmObject(ReceiptRequestDetail ReceiptRequestDetail, DateTime ConfirmationDate, IReceiptRequestService _ReceiptRequestService)
        {
            ReceiptRequestDetail.ConfirmationDate = ConfirmationDate;
            if (_validator.ValidConfirmObject(ReceiptRequestDetail))
            {
                ReceiptRequestDetail = _repository.ConfirmObject(ReceiptRequestDetail);
            }
            return ReceiptRequestDetail;
        }

        public ReceiptRequestDetail UnconfirmObject(ReceiptRequestDetail ReceiptRequestDetail, IReceiptRequestService _ReceiptRequestService)
        {
            if (_validator.ValidUnconfirmObject(ReceiptRequestDetail))
            {
                ReceiptRequestDetail = _repository.UnconfirmObject(ReceiptRequestDetail);
            }
            return ReceiptRequestDetail;
        }
    }
}