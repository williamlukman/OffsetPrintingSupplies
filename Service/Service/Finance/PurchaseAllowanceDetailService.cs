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
    public class PurchaseAllowanceDetailService : IPurchaseAllowanceDetailService
    {
        private IPurchaseAllowanceDetailRepository _repository;
        private IPurchaseAllowanceDetailValidator _validator;

        public PurchaseAllowanceDetailService(IPurchaseAllowanceDetailRepository _purchaseAllowanceDetailRepository, IPurchaseAllowanceDetailValidator _purchaseAllowanceDetailValidator)
        {
            _repository = _purchaseAllowanceDetailRepository;
            _validator = _purchaseAllowanceDetailValidator;
        }

        public IPurchaseAllowanceDetailValidator GetValidator()
        {
            return _validator;
        }

        public IQueryable<PurchaseAllowanceDetail> GetQueryable()
        {
            return _repository.GetQueryable();
        }

        public IList<PurchaseAllowanceDetail> GetAll()
        {
            return _repository.GetAll();
        }

        public IList<PurchaseAllowanceDetail> GetObjectsByPurchaseAllowanceId(int purchaseAllowanceId)
        {
            return _repository.GetObjectsByPurchaseAllowanceId(purchaseAllowanceId);
        }

        public IList<PurchaseAllowanceDetail> GetObjectsByPayableId(int payableId)
        {
            return _repository.GetObjectsByPayableId(payableId);
        }

        public PurchaseAllowanceDetail GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public PurchaseAllowanceDetail CreateObject(PurchaseAllowanceDetail purchaseAllowanceDetail, IPurchaseAllowanceService _purchaseAllowanceService,
                                                ICashBankService _cashBankService, IPayableService _payableService)
        {
            purchaseAllowanceDetail.Errors = new Dictionary<String, String>();
            return (_validator.ValidCreateObject(purchaseAllowanceDetail, _purchaseAllowanceService, this, _cashBankService, _payableService) ?
                    _repository.CreateObject(purchaseAllowanceDetail) : purchaseAllowanceDetail);
        }

        public PurchaseAllowanceDetail UpdateObject(PurchaseAllowanceDetail purchaseAllowanceDetail, IPurchaseAllowanceService _purchaseAllowanceService, ICashBankService _cashBankService, IPayableService _payableService)
        {
            return (_validator.ValidUpdateObject(purchaseAllowanceDetail, _purchaseAllowanceService, this, _cashBankService, _payableService) ?
                     _repository.UpdateObject(purchaseAllowanceDetail) : purchaseAllowanceDetail);
        }

        public PurchaseAllowanceDetail SoftDeleteObject(PurchaseAllowanceDetail purchaseAllowanceDetail)
        {
            return (_validator.ValidDeleteObject(purchaseAllowanceDetail) ? _repository.SoftDeleteObject(purchaseAllowanceDetail) : purchaseAllowanceDetail);
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }

        public PurchaseAllowanceDetail ConfirmObject(PurchaseAllowanceDetail purchaseAllowanceDetail, DateTime ConfirmationDate,
                                                  IPurchaseAllowanceService _purchaseAllowanceService, IPayableService _payableService)
        {
            purchaseAllowanceDetail.ConfirmationDate = ConfirmationDate;
            if (_validator.ValidConfirmObject(purchaseAllowanceDetail, _payableService))
            {
                PurchaseAllowance purchaseAllowance = _purchaseAllowanceService.GetObjectById(purchaseAllowanceDetail.PurchaseAllowanceId);
                Payable payable = _payableService.GetObjectById(purchaseAllowanceDetail.PayableId);

                if (purchaseAllowance.IsGBCH) { payable.PendingClearanceAmount += purchaseAllowanceDetail.Amount; }
                payable.RemainingAmount -= purchaseAllowanceDetail.Amount;
                if (payable.RemainingAmount == 0 && payable.PendingClearanceAmount == 0)
                {
                    payable.IsCompleted = true;
                    payable.CompletionDate = DateTime.Now;
                }
                _payableService.UpdateObject(payable);

                purchaseAllowanceDetail = _repository.ConfirmObject(purchaseAllowanceDetail);
            }
            return purchaseAllowanceDetail;
        }

        public PurchaseAllowanceDetail UnconfirmObject(PurchaseAllowanceDetail purchaseAllowanceDetail, IPurchaseAllowanceService _purchaseAllowanceService, IPayableService _payableService)
        {
            if (_validator.ValidUnconfirmObject(purchaseAllowanceDetail))
            {
                PurchaseAllowance purchaseAllowance = _purchaseAllowanceService.GetObjectById(purchaseAllowanceDetail.PurchaseAllowanceId);
                Payable payable = _payableService.GetObjectById(purchaseAllowanceDetail.PayableId);

                if (purchaseAllowance.IsGBCH) { payable.PendingClearanceAmount -= purchaseAllowanceDetail.Amount; }
                payable.RemainingAmount += purchaseAllowanceDetail.Amount;
                if (payable.RemainingAmount != 0 || payable.PendingClearanceAmount != 0)
                {
                    payable.IsCompleted = false;
                    payable.CompletionDate = null;
                }
                _payableService.UpdateObject(payable);

                purchaseAllowanceDetail = _repository.UnconfirmObject(purchaseAllowanceDetail);
            }
            return purchaseAllowanceDetail;
        }
    }
}