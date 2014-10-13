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
    public class PurchaseDownPaymentDetailService : IPurchaseDownPaymentDetailService
    {
        private IPurchaseDownPaymentDetailRepository _repository;
        private IPurchaseDownPaymentDetailValidator _validator;

        public PurchaseDownPaymentDetailService(IPurchaseDownPaymentDetailRepository _purchaseDownPaymentDetailRepository, IPurchaseDownPaymentDetailValidator _purchaseDownPaymentDetailValidator)
        {
            _repository = _purchaseDownPaymentDetailRepository;
            _validator = _purchaseDownPaymentDetailValidator;
        }

        public IPurchaseDownPaymentDetailValidator GetValidator()
        {
            return _validator;
        }

        public IQueryable<PurchaseDownPaymentDetail> GetQueryable()
        {
            return _repository.GetQueryable();
        }

        public IList<PurchaseDownPaymentDetail> GetAll()
        {
            return _repository.GetAll();
        }

        public IList<PurchaseDownPaymentDetail> GetObjectsByPurchaseDownPaymentId(int purchaseDownPaymentId)
        {
            return _repository.GetObjectsByPurchaseDownPaymentId(purchaseDownPaymentId);
        }

        public IList<PurchaseDownPaymentDetail> GetObjectsByPayableId(int payableId)
        {
            return _repository.GetObjectsByPayableId(payableId);
        }

        public PurchaseDownPaymentDetail GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public PurchaseDownPaymentDetail CreateObject(PurchaseDownPaymentDetail purchaseDownPaymentDetail, IPurchaseDownPaymentService _purchaseDownPaymentService,
                                                ICashBankService _cashBankService, IPayableService _payableService)
        {
            purchaseDownPaymentDetail.Errors = new Dictionary<String, String>();
            return (_validator.ValidCreateObject(purchaseDownPaymentDetail, _purchaseDownPaymentService, this, _cashBankService, _payableService) ?
                    _repository.CreateObject(purchaseDownPaymentDetail) : purchaseDownPaymentDetail);
        }

        public PurchaseDownPaymentDetail UpdateObject(PurchaseDownPaymentDetail purchaseDownPaymentDetail, IPurchaseDownPaymentService _purchaseDownPaymentService, ICashBankService _cashBankService, IPayableService _payableService)
        {
            return (_validator.ValidUpdateObject(purchaseDownPaymentDetail, _purchaseDownPaymentService, this, _cashBankService, _payableService) ?
                     _repository.UpdateObject(purchaseDownPaymentDetail) : purchaseDownPaymentDetail);
        }

        public PurchaseDownPaymentDetail SoftDeleteObject(PurchaseDownPaymentDetail purchaseDownPaymentDetail)
        {
            return (_validator.ValidDeleteObject(purchaseDownPaymentDetail) ? _repository.SoftDeleteObject(purchaseDownPaymentDetail) : purchaseDownPaymentDetail);
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }

        public PurchaseDownPaymentDetail ConfirmObject(PurchaseDownPaymentDetail purchaseDownPaymentDetail, DateTime ConfirmationDate,
                                                  IPurchaseDownPaymentService _purchaseDownPaymentService, IPayableService _payableService)
        {
            purchaseDownPaymentDetail.ConfirmationDate = ConfirmationDate;
            if (_validator.ValidConfirmObject(purchaseDownPaymentDetail, _payableService))
            {
                PurchaseDownPayment purchaseDownPayment = _purchaseDownPaymentService.GetObjectById(purchaseDownPaymentDetail.PurchaseDownPaymentId);
                Payable payable = _payableService.GetObjectById(purchaseDownPaymentDetail.PayableId);

                if (purchaseDownPayment.IsGBCH) { payable.PendingClearanceAmount += purchaseDownPaymentDetail.Amount; }
                payable.RemainingAmount -= purchaseDownPaymentDetail.Amount;
                if (payable.RemainingAmount == 0 && payable.PendingClearanceAmount == 0)
                {
                    payable.IsCompleted = true;
                    payable.CompletionDate = DateTime.Now;
                }
                _payableService.UpdateObject(payable);

                purchaseDownPaymentDetail = _repository.ConfirmObject(purchaseDownPaymentDetail);
            }
            return purchaseDownPaymentDetail;
        }

        public PurchaseDownPaymentDetail UnconfirmObject(PurchaseDownPaymentDetail purchaseDownPaymentDetail, IPurchaseDownPaymentService _purchaseDownPaymentService, IPayableService _payableService)
        {
            if (_validator.ValidUnconfirmObject(purchaseDownPaymentDetail))
            {
                PurchaseDownPayment purchaseDownPayment = _purchaseDownPaymentService.GetObjectById(purchaseDownPaymentDetail.PurchaseDownPaymentId);
                Payable payable = _payableService.GetObjectById(purchaseDownPaymentDetail.PayableId);

                if (purchaseDownPayment.IsGBCH) { payable.PendingClearanceAmount -= purchaseDownPaymentDetail.Amount; }
                payable.RemainingAmount += purchaseDownPaymentDetail.Amount;
                if (payable.RemainingAmount != 0 || payable.PendingClearanceAmount != 0)
                {
                    payable.IsCompleted = false;
                    payable.CompletionDate = null;
                }
                _payableService.UpdateObject(payable);

                purchaseDownPaymentDetail = _repository.UnconfirmObject(purchaseDownPaymentDetail);
            }
            return purchaseDownPaymentDetail;
        }
    }
}