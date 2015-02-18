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
    public class PaymentRequestDetailService : IPaymentRequestDetailService
    {
        private IPaymentRequestDetailRepository _repository;
        private IPaymentRequestDetailValidator _validator;

        public PaymentRequestDetailService(IPaymentRequestDetailRepository _paymentRequestDetailRepository, IPaymentRequestDetailValidator _paymentRequestDetailValidator)
        {
            _repository = _paymentRequestDetailRepository;
            _validator = _paymentRequestDetailValidator;
        }

        public IPaymentRequestDetailValidator GetValidator()
        {
            return _validator;
        }

        public IQueryable<PaymentRequestDetail> GetQueryable()
        {
            return _repository.GetQueryable();
        }

        public IList<PaymentRequestDetail> GetAll()
        {
            return _repository.GetAll();
        }

        public IList<PaymentRequestDetail> GetObjectsByPaymentRequestId(int paymentRequestId)
        {
            return _repository.GetObjectsByPaymentRequestId(paymentRequestId);
        }

        public IList<PaymentRequestDetail> GetNonLegacyObjectsByPaymentRequestId(int paymentRequestId)
        {
            return _repository.GetNonLegacyObjectsByPaymentRequestId(paymentRequestId);
        }

        public PaymentRequestDetail GetLegacyObjectByPaymentRequestId(int paymentRequestId)
        {
            return _repository.GetLegacyObjectByPaymentRequestId(paymentRequestId);
        }
        
        public PaymentRequestDetail GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public PaymentRequestDetail CreateObject(PaymentRequestDetail paymentRequestDetail, IPaymentRequestService _paymentRequestService, IAccountService _accountService)
        {
            paymentRequestDetail.Errors = new Dictionary<String, String>();
            if (_validator.ValidCreateObject(paymentRequestDetail, _paymentRequestService, this, _accountService))
            {
                paymentRequestDetail =  _repository.CreateObject(paymentRequestDetail);
                PaymentRequest paymentRequest = _paymentRequestService.GetObjectById(paymentRequestDetail.PaymentRequestId);
                _paymentRequestService.CalculateTotalAmount(paymentRequest, this);
            }
            return paymentRequestDetail;
        }

        public PaymentRequestDetail UpdateObject(PaymentRequestDetail paymentRequestDetail, IPaymentRequestService _paymentRequestService, IAccountService _accountService)
        {
            if (_validator.ValidUpdateObject(paymentRequestDetail, _paymentRequestService, this, _accountService))
            {
                paymentRequestDetail = _repository.UpdateObject(paymentRequestDetail);
                PaymentRequest paymentRequest = _paymentRequestService.GetObjectById(paymentRequestDetail.PaymentRequestId);
                _paymentRequestService.CalculateTotalAmount(paymentRequest, this);
            }
            return paymentRequestDetail;
        }

        public PaymentRequestDetail CreateLegacyObject(PaymentRequestDetail paymentRequestDetail, IPaymentRequestService _paymentRequestService, IAccountService _accountService)
        {
            paymentRequestDetail.Errors = new Dictionary<String, String>();
            return (_validator.ValidCreateLegacyObject(paymentRequestDetail, _paymentRequestService, this, _accountService) ?
                    _repository.CreateObject(paymentRequestDetail) : paymentRequestDetail);
        }

        public PaymentRequestDetail UpdateLegacyObject(PaymentRequestDetail paymentRequestDetail, IPaymentRequestService _paymentRequestService, IAccountService _accountService)
        {
            return (_validator.ValidUpdateLegacyObject(paymentRequestDetail, _paymentRequestService, this, _accountService) ?
                     _repository.UpdateObject(paymentRequestDetail) : paymentRequestDetail);
        }

        public PaymentRequestDetail SoftDeleteObject(PaymentRequestDetail paymentRequestDetail,IPaymentRequestService _paymentRequestService)
        {
            if (_validator.ValidDeleteObject(paymentRequestDetail, _paymentRequestService))
            {
                paymentRequestDetail = _repository.SoftDeleteObject(paymentRequestDetail);
                PaymentRequest paymentRequest = _paymentRequestService.GetObjectById(paymentRequestDetail.PaymentRequestId);
                _paymentRequestService.CalculateTotalAmount(paymentRequest, this);
            }

            return paymentRequestDetail;
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }

        public PaymentRequestDetail ConfirmObject(PaymentRequestDetail paymentRequestDetail, DateTime ConfirmationDate, IPaymentRequestService _paymentRequestService)
        {
            paymentRequestDetail.ConfirmationDate = ConfirmationDate;
            if (_validator.ValidConfirmObject(paymentRequestDetail, _paymentRequestService))
            {
                paymentRequestDetail = _repository.ConfirmObject(paymentRequestDetail);
            }
            return paymentRequestDetail;
        }

        public PaymentRequestDetail UnconfirmObject(PaymentRequestDetail paymentRequestDetail, IPaymentRequestService _paymentRequestService)
        {
            if (_validator.ValidUnconfirmObject(paymentRequestDetail, _paymentRequestService))
            {
                paymentRequestDetail = _repository.UnconfirmObject(paymentRequestDetail);
            }
            return paymentRequestDetail;
        }
    }
}