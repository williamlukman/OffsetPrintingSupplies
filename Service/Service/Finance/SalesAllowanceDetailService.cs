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
    public class SalesAllowanceDetailService : ISalesAllowanceDetailService
    {
        private ISalesAllowanceDetailRepository _repository;
        private ISalesAllowanceDetailValidator _validator;

        public SalesAllowanceDetailService(ISalesAllowanceDetailRepository _salesAllowanceDetailRepository, ISalesAllowanceDetailValidator _salesAllowanceDetailValidator)
        {
            _repository = _salesAllowanceDetailRepository;
            _validator = _salesAllowanceDetailValidator;
        }

        public ISalesAllowanceDetailValidator GetValidator()
        {
            return _validator;
        }

        public IQueryable<SalesAllowanceDetail> GetQueryable()
        {
            return _repository.GetQueryable();
        }

        public IList<SalesAllowanceDetail> GetAll()
        {
            return _repository.GetAll();
        }

        public IList<SalesAllowanceDetail> GetObjectsBySalesAllowanceId(int salesAllowanceId)
        {
            return _repository.GetObjectsBySalesAllowanceId(salesAllowanceId);
        }

        public IList<SalesAllowanceDetail> GetObjectsByReceivableId(int receivableId)
        {
            return _repository.GetObjectsByReceivableId(receivableId);
        }

        public SalesAllowanceDetail GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public SalesAllowanceDetail CreateObject(SalesAllowanceDetail salesAllowanceDetail, ISalesAllowanceService _salesAllowanceService,
                                                ICashBankService _cashBankService, IReceivableService _receivableService)
        {
            salesAllowanceDetail.Errors = new Dictionary<String, String>();
            return (_validator.ValidCreateObject(salesAllowanceDetail, _salesAllowanceService, this, _cashBankService, _receivableService) ?
                    _repository.CreateObject(salesAllowanceDetail) : salesAllowanceDetail);
        }

        public SalesAllowanceDetail UpdateObject(SalesAllowanceDetail salesAllowanceDetail, ISalesAllowanceService _salesAllowanceService, ICashBankService _cashBankService, IReceivableService _receivableService)
        {
            return (_validator.ValidUpdateObject(salesAllowanceDetail, _salesAllowanceService, this, _cashBankService, _receivableService) ?
                     _repository.UpdateObject(salesAllowanceDetail) : salesAllowanceDetail);
        }

        public SalesAllowanceDetail SoftDeleteObject(SalesAllowanceDetail salesAllowanceDetail)
        {
            return (_validator.ValidDeleteObject(salesAllowanceDetail) ? _repository.SoftDeleteObject(salesAllowanceDetail) : salesAllowanceDetail);
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }

        public SalesAllowanceDetail ConfirmObject(SalesAllowanceDetail salesAllowanceDetail, DateTime ConfirmationDate, ISalesAllowanceService _salesAllowanceService, IReceivableService _receivableService)
        {
            salesAllowanceDetail.ConfirmationDate = ConfirmationDate;
            if (_validator.ValidConfirmObject(salesAllowanceDetail, _receivableService))
            {
                SalesAllowance salesAllowance = _salesAllowanceService.GetObjectById(salesAllowanceDetail.SalesAllowanceId);
                Receivable receivable = _receivableService.GetObjectById(salesAllowanceDetail.ReceivableId);

                if (salesAllowance.IsGBCH) { receivable.PendingClearanceAmount += salesAllowanceDetail.Amount; }
                receivable.RemainingAmount -= salesAllowanceDetail.Amount;
                if (receivable.RemainingAmount == 0 && receivable.PendingClearanceAmount == 0)
                {
                    receivable.IsCompleted = true;
                    receivable.CompletionDate = DateTime.Now;
                }
                _receivableService.UpdateObject(receivable);

                salesAllowanceDetail = _repository.ConfirmObject(salesAllowanceDetail);
            }
            return salesAllowanceDetail;
        }

        public SalesAllowanceDetail UnconfirmObject(SalesAllowanceDetail salesAllowanceDetail, ISalesAllowanceService _salesAllowanceService, IReceivableService _receivableService)
        {
            if (_validator.ValidUnconfirmObject(salesAllowanceDetail))
            {
                SalesAllowance salesAllowance = _salesAllowanceService.GetObjectById(salesAllowanceDetail.SalesAllowanceId);
                Receivable receivable = _receivableService.GetObjectById(salesAllowanceDetail.ReceivableId);

                if (salesAllowance.IsGBCH) { receivable.PendingClearanceAmount -= salesAllowanceDetail.Amount; }
                receivable.RemainingAmount += salesAllowanceDetail.Amount;
                if (receivable.RemainingAmount != 0 || receivable.PendingClearanceAmount != 0)
                {
                    receivable.IsCompleted = false;
                    receivable.CompletionDate = null;
                }
                _receivableService.UpdateObject(receivable);

                salesAllowanceDetail = _repository.UnconfirmObject(salesAllowanceDetail);
            }
            return salesAllowanceDetail;
        }
    }
}