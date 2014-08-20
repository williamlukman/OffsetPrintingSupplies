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
    public class ReceiptVoucherDetailService : IReceiptVoucherDetailService
    {
        private IReceiptVoucherDetailRepository _repository;
        private IReceiptVoucherDetailValidator _validator;

        public ReceiptVoucherDetailService(IReceiptVoucherDetailRepository _receiptVoucherDetailRepository, IReceiptVoucherDetailValidator _receiptVoucherDetailValidator)
        {
            _repository = _receiptVoucherDetailRepository;
            _validator = _receiptVoucherDetailValidator;
        }

        public IReceiptVoucherDetailValidator GetValidator()
        {
            return _validator;
        }

        public IQueryable<ReceiptVoucherDetail> GetQueryable()
        {
            return _repository.GetQueryable();
        }

        public IList<ReceiptVoucherDetail> GetAll()
        {
            return _repository.GetAll();
        }

        public IList<ReceiptVoucherDetail> GetObjectsByReceiptVoucherId(int receiptVoucherId)
        {
            return _repository.GetObjectsByReceiptVoucherId(receiptVoucherId);
        }

        public IList<ReceiptVoucherDetail> GetObjectsByReceivableId(int receivableId)
        {
            return _repository.GetObjectsByReceivableId(receivableId);
        }

        public ReceiptVoucherDetail GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public ReceiptVoucherDetail CreateObject(ReceiptVoucherDetail receiptVoucherDetail, IReceiptVoucherService _receiptVoucherService,
                                                ICashBankService _cashBankService, IReceivableService _receivableService)
        {
            receiptVoucherDetail.Errors = new Dictionary<String, String>();
            return (_validator.ValidCreateObject(receiptVoucherDetail, _receiptVoucherService, this, _cashBankService, _receivableService) ?
                    _repository.CreateObject(receiptVoucherDetail) : receiptVoucherDetail);
        }

        public ReceiptVoucherDetail CreateObject(int receiptVoucherId, int receivableId, decimal amount, string description, 
                                         IReceiptVoucherService _receiptVoucherService, ICashBankService _cashBankService,
                                         IReceivableService _receivableService)
        {
            ReceiptVoucherDetail receiptVoucherDetail = new ReceiptVoucherDetail
            {
                ReceiptVoucherId = receiptVoucherId,
                ReceivableId = receivableId,
                Amount = amount,
                Description = description,
            };
            return this.CreateObject(receiptVoucherDetail, _receiptVoucherService, _cashBankService, _receivableService);
        }

        public ReceiptVoucherDetail UpdateObject(ReceiptVoucherDetail receiptVoucherDetail, IReceiptVoucherService _receiptVoucherService, ICashBankService _cashBankService, IReceivableService _receivableService)
        {
            return (_validator.ValidUpdateObject(receiptVoucherDetail, _receiptVoucherService, this, _cashBankService, _receivableService) ?
                     _repository.UpdateObject(receiptVoucherDetail) : receiptVoucherDetail);
        }

        public ReceiptVoucherDetail SoftDeleteObject(ReceiptVoucherDetail receiptVoucherDetail)
        {
            return (_validator.ValidDeleteObject(receiptVoucherDetail) ? _repository.SoftDeleteObject(receiptVoucherDetail) : receiptVoucherDetail);
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }

        public ReceiptVoucherDetail ConfirmObject(ReceiptVoucherDetail receiptVoucherDetail, DateTime ConfirmationDate, IReceiptVoucherService _receiptVoucherService, IReceivableService _receivableService)
        {
            receiptVoucherDetail.ConfirmationDate = ConfirmationDate;
            if (_validator.ValidConfirmObject(receiptVoucherDetail, _receivableService))
            {
                ReceiptVoucher receiptVoucher = _receiptVoucherService.GetObjectById(receiptVoucherDetail.ReceiptVoucherId);
                Receivable receivable = _receivableService.GetObjectById(receiptVoucherDetail.ReceivableId);

                if (receiptVoucher.IsGBCH) { receivable.PendingClearanceAmount += receiptVoucherDetail.Amount; }
                receivable.RemainingAmount -= receiptVoucherDetail.Amount;
                if (receivable.RemainingAmount == 0 && receivable.PendingClearanceAmount == 0)
                {
                    receivable.IsCompleted = true;
                    receivable.CompletionDate = DateTime.Now;
                }
                _receivableService.UpdateObject(receivable);

                receiptVoucherDetail = _repository.ConfirmObject(receiptVoucherDetail);
            }
            return receiptVoucherDetail;
        }

        public ReceiptVoucherDetail UnconfirmObject(ReceiptVoucherDetail receiptVoucherDetail, IReceiptVoucherService _receiptVoucherService, IReceivableService _receivableService)
        {
            if (_validator.ValidUnconfirmObject(receiptVoucherDetail))
            {
                ReceiptVoucher receiptVoucher = _receiptVoucherService.GetObjectById(receiptVoucherDetail.ReceiptVoucherId);
                Receivable receivable = _receivableService.GetObjectById(receiptVoucherDetail.ReceivableId);

                if (receiptVoucher.IsGBCH) { receivable.PendingClearanceAmount -= receiptVoucherDetail.Amount; }
                receivable.RemainingAmount += receiptVoucherDetail.Amount;
                if (receivable.RemainingAmount != 0 || receivable.PendingClearanceAmount != 0)
                {
                    receivable.IsCompleted = false;
                    receivable.CompletionDate = null;
                }
                _receivableService.UpdateObject(receivable);

                receiptVoucherDetail = _repository.UnconfirmObject(receiptVoucherDetail);
            }
            return receiptVoucherDetail;
        }
    }
}