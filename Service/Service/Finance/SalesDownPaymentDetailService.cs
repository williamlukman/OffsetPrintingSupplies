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
    public class SalesDownPaymentDetailService : ISalesDownPaymentDetailService
    {
        private ISalesDownPaymentDetailRepository _repository;
        private ISalesDownPaymentDetailValidator _validator;

        public SalesDownPaymentDetailService(ISalesDownPaymentDetailRepository _salesDownPaymentDetailRepository, ISalesDownPaymentDetailValidator _salesDownPaymentDetailValidator)
        {
            _repository = _salesDownPaymentDetailRepository;
            _validator = _salesDownPaymentDetailValidator;
        }

        public ISalesDownPaymentDetailValidator GetValidator()
        {
            return _validator;
        }

        public IQueryable<SalesDownPaymentDetail> GetQueryable()
        {
            return _repository.GetQueryable();
        }

        public IList<SalesDownPaymentDetail> GetAll()
        {
            return _repository.GetAll();
        }

        public IList<SalesDownPaymentDetail> GetObjectsBySalesDownPaymentId(int salesDownPaymentId)
        {
            return _repository.GetObjectsBySalesDownPaymentId(salesDownPaymentId);
        }

        public IList<SalesDownPaymentDetail> GetObjectsByReceivableId(int receivableId)
        {
            return _repository.GetObjectsByReceivableId(receivableId);
        }

        public SalesDownPaymentDetail GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public SalesDownPaymentDetail CreateObject(SalesDownPaymentDetail salesDownPaymentDetail, ISalesDownPaymentService _salesDownPaymentService,
                                                ICashBankService _cashBankService, IReceivableService _receivableService)
        {
            salesDownPaymentDetail.Errors = new Dictionary<String, String>();
            return (_validator.ValidCreateObject(salesDownPaymentDetail, _salesDownPaymentService, this, _cashBankService, _receivableService) ?
                    _repository.CreateObject(salesDownPaymentDetail) : salesDownPaymentDetail);
        }

        public SalesDownPaymentDetail UpdateObject(SalesDownPaymentDetail salesDownPaymentDetail, ISalesDownPaymentService _salesDownPaymentService, ICashBankService _cashBankService, IReceivableService _receivableService)
        {
            return (_validator.ValidUpdateObject(salesDownPaymentDetail, _salesDownPaymentService, this, _cashBankService, _receivableService) ?
                     _repository.UpdateObject(salesDownPaymentDetail) : salesDownPaymentDetail);
        }

        public SalesDownPaymentDetail SoftDeleteObject(SalesDownPaymentDetail salesDownPaymentDetail)
        {
            return (_validator.ValidDeleteObject(salesDownPaymentDetail) ? _repository.SoftDeleteObject(salesDownPaymentDetail) : salesDownPaymentDetail);
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }

        public SalesDownPaymentDetail ConfirmObject(SalesDownPaymentDetail salesDownPaymentDetail, DateTime ConfirmationDate, ISalesDownPaymentService _salesDownPaymentService, IReceivableService _receivableService)
        {
            salesDownPaymentDetail.ConfirmationDate = ConfirmationDate;
            if (_validator.ValidConfirmObject(salesDownPaymentDetail, _receivableService))
            {
                SalesDownPayment salesDownPayment = _salesDownPaymentService.GetObjectById(salesDownPaymentDetail.SalesDownPaymentId);
                Receivable receivable = _receivableService.GetObjectById(salesDownPaymentDetail.ReceivableId);

                if (salesDownPayment.IsGBCH) { receivable.PendingClearanceAmount += salesDownPaymentDetail.Amount; }
                receivable.RemainingAmount -= salesDownPaymentDetail.Amount;
                if (receivable.RemainingAmount == 0 && receivable.PendingClearanceAmount == 0)
                {
                    receivable.IsCompleted = true;
                    receivable.CompletionDate = DateTime.Now;
                }
                _receivableService.UpdateObject(receivable);

                salesDownPaymentDetail = _repository.ConfirmObject(salesDownPaymentDetail);
            }
            return salesDownPaymentDetail;
        }

        public SalesDownPaymentDetail UnconfirmObject(SalesDownPaymentDetail salesDownPaymentDetail, ISalesDownPaymentService _salesDownPaymentService, IReceivableService _receivableService)
        {
            if (_validator.ValidUnconfirmObject(salesDownPaymentDetail))
            {
                SalesDownPayment salesDownPayment = _salesDownPaymentService.GetObjectById(salesDownPaymentDetail.SalesDownPaymentId);
                Receivable receivable = _receivableService.GetObjectById(salesDownPaymentDetail.ReceivableId);

                if (salesDownPayment.IsGBCH) { receivable.PendingClearanceAmount -= salesDownPaymentDetail.Amount; }
                receivable.RemainingAmount += salesDownPaymentDetail.Amount;
                if (receivable.RemainingAmount != 0 || receivable.PendingClearanceAmount != 0)
                {
                    receivable.IsCompleted = false;
                    receivable.CompletionDate = null;
                }
                _receivableService.UpdateObject(receivable);

                salesDownPaymentDetail = _repository.UnconfirmObject(salesDownPaymentDetail);
            }
            return salesDownPaymentDetail;
        }
    }
}