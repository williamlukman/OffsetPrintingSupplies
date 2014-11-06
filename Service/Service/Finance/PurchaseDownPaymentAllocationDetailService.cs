using Core.Constants;
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
    public class PurchaseDownPaymentAllocationDetailService : IPurchaseDownPaymentAllocationDetailService
    {
        private IPurchaseDownPaymentAllocationDetailRepository _repository;
        private IPurchaseDownPaymentAllocationDetailValidator _validator;

        public PurchaseDownPaymentAllocationDetailService(IPurchaseDownPaymentAllocationDetailRepository _purchaseDownPaymentAllocationDetailRepository, IPurchaseDownPaymentAllocationDetailValidator _purchaseDownPaymentAllocationDetailValidator)
        {
            _repository = _purchaseDownPaymentAllocationDetailRepository;
            _validator = _purchaseDownPaymentAllocationDetailValidator;
        }

        public IPurchaseDownPaymentAllocationDetailValidator GetValidator()
        {
            return _validator;
        }

        public IQueryable<PurchaseDownPaymentAllocationDetail> GetQueryable()
        {
            return _repository.GetQueryable();
        }

        public IList<PurchaseDownPaymentAllocationDetail> GetAll()
        {
            return _repository.GetAll();
        }

        public IList<PurchaseDownPaymentAllocationDetail> GetObjectsByPurchaseDownPaymentAllocationId(int purchaseDownPaymentAllocationId)
        {
            return _repository.GetObjectsByPurchaseDownPaymentAllocationId(purchaseDownPaymentAllocationId);
        }

        public IList<PurchaseDownPaymentAllocationDetail> GetObjectsByPayableId(int payableId)
        {
            return _repository.GetObjectsByPayableId(payableId);
        }

        public PurchaseDownPaymentAllocationDetail GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public PurchaseDownPaymentAllocationDetail CreateObject(PurchaseDownPaymentAllocationDetail purchaseDownPaymentAllocationDetail,
                                                             IPurchaseDownPaymentAllocationService _purchaseDownPaymentAllocationService,
                                                             IPurchaseDownPaymentService _purchaseDownPaymentService, IPayableService _payableService,
                                                             IReceivableService _receivableService)
        {
            purchaseDownPaymentAllocationDetail.Errors = new Dictionary<String, String>();
            if (_validator.ValidCreateObject(purchaseDownPaymentAllocationDetail, _purchaseDownPaymentAllocationService, this, _purchaseDownPaymentService, _payableService, _receivableService))
            {
                _repository.CreateObject(purchaseDownPaymentAllocationDetail);
            }
            return purchaseDownPaymentAllocationDetail;
        }

        public PurchaseDownPaymentAllocationDetail UpdateObject(PurchaseDownPaymentAllocationDetail purchaseDownPaymentAllocationDetail, IPurchaseDownPaymentAllocationService _purchaseDownPaymentAllocationService,
                                                             IPurchaseDownPaymentService _purchaseDownPaymentService, IPayableService _payableService, IReceivableService _receivableService)
        {
            if (_validator.ValidUpdateObject(purchaseDownPaymentAllocationDetail, _purchaseDownPaymentAllocationService, this, _purchaseDownPaymentService, _payableService, _receivableService))
            {
                _repository.UpdateObject(purchaseDownPaymentAllocationDetail);
            }
            return purchaseDownPaymentAllocationDetail;
        }

        public PurchaseDownPaymentAllocationDetail SoftDeleteObject(PurchaseDownPaymentAllocationDetail purchaseDownPaymentAllocationDetail)
        {
            if (_validator.ValidDeleteObject(purchaseDownPaymentAllocationDetail))
            {
                _repository.SoftDeleteObject(purchaseDownPaymentAllocationDetail);
            }
            return purchaseDownPaymentAllocationDetail;
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }

        public PurchaseDownPaymentAllocationDetail ConfirmObject(PurchaseDownPaymentAllocationDetail purchaseDownPaymentAllocationDetail, DateTime ConfirmationDate,
                                                IPurchaseDownPaymentAllocationService _purchaseDownPaymentAllocationService, IPurchaseDownPaymentService _purchaseDownPaymentService,
                                                IPayableService _payableService, IReceivableService _receivableService)
        {
            purchaseDownPaymentAllocationDetail.ConfirmationDate = ConfirmationDate;
            if (_validator.ValidConfirmObject(purchaseDownPaymentAllocationDetail, _payableService, _receivableService))
            {
                PurchaseDownPaymentAllocation purchaseDownPaymentAllocation = _purchaseDownPaymentAllocationService.GetObjectById(purchaseDownPaymentAllocationDetail.PurchaseDownPaymentAllocationId);

                Receivable receivable = _receivableService.GetObjectById(purchaseDownPaymentAllocation.ReceivableId);
                receivable.RemainingAmount -= purchaseDownPaymentAllocationDetail.Amount;
                _receivableService.UpdateObject(receivable);

                Payable payable = _payableService.GetObjectById(purchaseDownPaymentAllocationDetail.PayableId);
                payable.RemainingAmount -= purchaseDownPaymentAllocationDetail.Amount;
                _payableService.UpdateObject(payable);

                purchaseDownPaymentAllocationDetail = _repository.ConfirmObject(purchaseDownPaymentAllocationDetail);
            }
            return purchaseDownPaymentAllocationDetail;
        }

        public PurchaseDownPaymentAllocationDetail UnconfirmObject(PurchaseDownPaymentAllocationDetail purchaseDownPaymentAllocationDetail,
                                                IPurchaseDownPaymentAllocationService _purchaseDownPaymentAllocationService, IPurchaseDownPaymentService _purchaseDownPaymentService,
                                                IPayableService _payableService, IReceivableService _receivableService)
        {
            if (_validator.ValidUnconfirmObject(purchaseDownPaymentAllocationDetail, _payableService, _receivableService))
            {
                PurchaseDownPaymentAllocation purchaseDownPaymentAllocation = _purchaseDownPaymentAllocationService.GetObjectById(purchaseDownPaymentAllocationDetail.PurchaseDownPaymentAllocationId);

                Receivable receivable = _receivableService.GetObjectById(purchaseDownPaymentAllocation.ReceivableId);
                receivable.RemainingAmount += purchaseDownPaymentAllocationDetail.Amount;
                _receivableService.UpdateObject(receivable);

                Payable payable = _payableService.GetObjectById(purchaseDownPaymentAllocationDetail.PayableId);
                payable.RemainingAmount += purchaseDownPaymentAllocationDetail.Amount;
                _payableService.UpdateObject(payable);

                purchaseDownPaymentAllocationDetail = _repository.UnconfirmObject(purchaseDownPaymentAllocationDetail);
            }
            return purchaseDownPaymentAllocationDetail;
        }
    }
}