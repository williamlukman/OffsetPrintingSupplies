using Core.DomainModel;
using Core.Interface.Repository;
using Core.Interface.Service;
using Core.Interface.Validation;
using Data.Repository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
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
                                                ICashBankService _cashBankService, IReceivableService _receivableService,ICurrencyService _currencyService)
        {
            receiptVoucherDetail.Errors = new Dictionary<String, String>();
            if (_validator.ValidCreateObject(receiptVoucherDetail, _receiptVoucherService, this, _cashBankService, _receivableService,_currencyService))
            {
                ReceiptVoucher rv = _receiptVoucherService.GetQueryable().Where(x => x.Id == receiptVoucherDetail.ReceiptVoucherId).FirstOrDefault();
                Receivable r = _receivableService.GetObjectById(receiptVoucherDetail.ReceivableId);           
                CashBank cashBank = _cashBankService.GetObjectById(rv.CashBankId);
                Currency currency = _currencyService.GetObjectById(cashBank.CurrencyId);
                if (currency.Id == r.CurrencyId)
                {
                    receiptVoucherDetail.Amount = receiptVoucherDetail.AmountPaid;
                }
                else
                {
                    receiptVoucherDetail.Amount = receiptVoucherDetail.AmountPaid / receiptVoucherDetail.Rate;
                }
                rv.TotalAmount = rv.TotalAmount + receiptVoucherDetail.AmountPaid;
                receiptVoucherDetail = _repository.CreateObject(receiptVoucherDetail);
                _receiptVoucherService.UpdateAmount(rv);
            }
            return receiptVoucherDetail;
        }

        public ReceiptVoucherDetail CreateObject(int receiptVoucherId, int receivableId, decimal amount, string description, 
                                         IReceiptVoucherService _receiptVoucherService, ICashBankService _cashBankService,
                                         IReceivableService _receivableService,ICurrencyService _currencyService)
        {
            ReceiptVoucherDetail receiptVoucherDetail = new ReceiptVoucherDetail
            {
                ReceiptVoucherId = receiptVoucherId,
                ReceivableId = receivableId,
                Amount = amount,
                Description = description,
            };
            return this.CreateObject(receiptVoucherDetail, _receiptVoucherService, _cashBankService, _receivableService,_currencyService);
        }

        public ReceiptVoucherDetail UpdateObject(ReceiptVoucherDetail receiptVoucherDetail, IReceiptVoucherService _receiptVoucherService, ICashBankService _cashBankService, IReceivableService _receivableService,ICurrencyService _currencyService)
        {
            if (_validator.ValidUpdateObject(receiptVoucherDetail, _receiptVoucherService, this, _cashBankService, _receivableService,_currencyService))
            {
                ReceiptVoucher rv = _receiptVoucherService.GetQueryable()
                                .Where(x => x.Id == receiptVoucherDetail.ReceiptVoucherId).FirstOrDefault();
                if (rv.CashBank.CurrencyId == receiptVoucherDetail.Receivable.CurrencyId)
                {
                    receiptVoucherDetail.Amount = receiptVoucherDetail.AmountPaid;
                }
                else
                {
                    receiptVoucherDetail.Amount = receiptVoucherDetail.AmountPaid / receiptVoucherDetail.Rate;
                }
                receiptVoucherDetail = _repository.UpdateObject(receiptVoucherDetail) ;
                _receiptVoucherService.CalculateTotalAmount(rv,this);
            }
            return receiptVoucherDetail;
        }



        public ReceiptVoucherDetail SoftDeleteObject(ReceiptVoucherDetail receiptVoucherDetail,IReceiptVoucherService _receiptVoucherService)
        {
            if (_validator.ValidDeleteObject(receiptVoucherDetail))
            {
                receiptVoucherDetail = _repository.SoftDeleteObject(receiptVoucherDetail);
                ReceiptVoucher rv = _receiptVoucherService.GetObjectById(receiptVoucherDetail.ReceiptVoucherId);
                _receiptVoucherService.CalculateTotalAmount(rv, this);
            }
            return receiptVoucherDetail;
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
                receivable = _receivableService.UpdateObject(receivable);
                receiptVoucherDetail = _repository.ConfirmObject(receiptVoucherDetail);
                //receiptVoucherDetail.Receivable = new Receivable();
                //receiptVoucherDetail.Receivable = receivable;
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