using Core.Constants;
using Core.DomainModel;
using Core.Interface.Repository;
using Core.Interface.Service;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Service.Service
{
    public class PaymentVoucherService : IPaymentVoucherService
    {
        private IPaymentVoucherRepository _repository;
        private IPaymentVoucherValidator _validator;

        public PaymentVoucherService(IPaymentVoucherRepository _paymentVoucherRepository, IPaymentVoucherValidator _paymentVoucherValidator)
        {
            _repository = _paymentVoucherRepository;
            _validator = _paymentVoucherValidator;
        }

        public IPaymentVoucherValidator GetValidator()
        {
            return _validator;
        }

        public IList<PaymentVoucher> GetAll()
        {
            return _repository.GetAll();
        }

        public IList<PaymentVoucher> GetObjectsByCashBankId(int cashBankId)
        {
            return _repository.GetObjectsByCashBankId(cashBankId);
        }

        public PaymentVoucher GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public IList<PaymentVoucher> GetObjectsByCustomerId(int customerId)
        {
            return _repository.GetObjectsByCustomerId(customerId);
        }

        public PaymentVoucher CreateObject(PaymentVoucher paymentVoucher, IPaymentVoucherDetailService _paymentVoucherDetailService,
                                            IPayableService _payableService, ICustomerService _customerService, ICashBankService _cashBankService)
        {
            paymentVoucher.Errors = new Dictionary<String, String>();
            return (_validator.ValidCreateObject(paymentVoucher, this, _paymentVoucherDetailService, _payableService, _customerService, _cashBankService) ?
                    _repository.CreateObject(paymentVoucher) : paymentVoucher);
        }

        public PaymentVoucher CreateObject(int cashBankId, int customerId, DateTime paymentDate, decimal totalAmount, bool IsGBCH, DateTime DueDate, bool IsBank,
                                    IPaymentVoucherDetailService _paymentVoucherDetailService, IPayableService _payableService,
                                    ICustomerService _customerService, ICashBankService _cashBankService)
        {
            PaymentVoucher paymentVoucher = new PaymentVoucher
            {
                CashBankId = cashBankId,
                CustomerId = customerId,
                PaymentDate = paymentDate,
                TotalAmount = totalAmount,
                IsGBCH = IsGBCH,
                DueDate = DueDate,
                IsBank = IsBank
            };
            return this.CreateObject(paymentVoucher, _paymentVoucherDetailService, _payableService, _customerService, _cashBankService);
        }

        public PaymentVoucher UpdateObject(PaymentVoucher paymentVoucher, IPaymentVoucherDetailService _paymentVoucherDetailService, IPayableService _payableService, ICustomerService _customerService, ICashBankService _cashBankService)
        {
            return (_validator.ValidUpdateObject(paymentVoucher, this, _paymentVoucherDetailService, _payableService, _customerService, _cashBankService) ? _repository.UpdateObject(paymentVoucher) : paymentVoucher);
        }

        public PaymentVoucher UpdateAmount(PaymentVoucher paymentVoucher)
        {
            return _repository.UpdateObject(paymentVoucher);
        }

        public PaymentVoucher SoftDeleteObject(PaymentVoucher paymentVoucher, IPaymentVoucherDetailService _paymentVoucherDetailService)
        {
            return (_validator.ValidDeleteObject(paymentVoucher, _paymentVoucherDetailService) ? _repository.SoftDeleteObject(paymentVoucher) : paymentVoucher);
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }

        public PaymentVoucher ConfirmObject(PaymentVoucher paymentVoucher, IPaymentVoucherDetailService _paymentVoucherDetailService,
                                            ICashBankService _cashBankService, IPayableService _payableService, ICashMutationService _cashMutationService)
        {
            if (_validator.ValidConfirmObject(paymentVoucher, this, _paymentVoucherDetailService, _cashBankService, _payableService))
            {
                IList<PaymentVoucherDetail> details = _paymentVoucherDetailService.GetObjectsByPaymentVoucherId(paymentVoucher.Id);
                foreach (var detail in details)
                {
                    _paymentVoucherDetailService.ConfirmObject(detail, this, _payableService);
                }
                _repository.ConfirmObject(paymentVoucher);

                if (!paymentVoucher.IsGBCH)
                {
                    CashBank cashBank = _cashBankService.GetObjectById(paymentVoucher.CashBankId);
                    CashMutation cashMutation = _cashMutationService.CreateCashMutationForPaymentVoucher(paymentVoucher, cashBank);
                    _cashMutationService.CashMutateObject(cashMutation, _cashBankService);
                }
            }
            return paymentVoucher;
        }

        public PaymentVoucher UnconfirmObject(PaymentVoucher paymentVoucher, IPaymentVoucherDetailService _paymentVoucherDetailService,
                                            ICashBankService _cashBankService, IPayableService _payableService, ICashMutationService _cashMutationService)
        {
            if (_validator.ValidUnconfirmObject(paymentVoucher))
            {
                IList<PaymentVoucherDetail> details = _paymentVoucherDetailService.GetObjectsByPaymentVoucherId(paymentVoucher.Id);
                foreach (var detail in details)
                {
                    _paymentVoucherDetailService.UnconfirmObject(detail, this, _payableService);
                }
                _repository.UnconfirmObject(paymentVoucher);

                if (!paymentVoucher.IsGBCH)
                {
                    CashBank cashBank = _cashBankService.GetObjectById(paymentVoucher.CashBankId);
                    IList<CashMutation> cashMutations = _cashMutationService.SoftDeleteCashMutationForPaymentVoucher(paymentVoucher, cashBank);
                    foreach (var cashMutation in cashMutations)
                    {
                        _cashMutationService.ReverseCashMutateObject(cashMutation, _cashBankService);
                    }
                }
            }
            return paymentVoucher;
        }

        public PaymentVoucher ReconcileObject(PaymentVoucher paymentVoucher, DateTime ReconciliationDate,
                                              IPaymentVoucherDetailService _paymentVoucherDetailService, ICashMutationService _cashMutationService,
                                              ICashBankService _cashBankService, IPayableService _payableService)
        {
            if (_validator.ValidReconcileObject(paymentVoucher))
            {
                paymentVoucher.ReconciliationDate = ReconciliationDate;
                _repository.ReconcileObject(paymentVoucher);

                CashBank cashBank = _cashBankService.GetObjectById(paymentVoucher.CashBankId);
                // TODO Check the cashMutation input
                CashMutation cashMutation = _cashMutationService.CreateCashMutationForPaymentVoucher(paymentVoucher, cashBank);
                _cashMutationService.CashMutateObject(cashMutation, _cashBankService);

                IList<PaymentVoucherDetail> paymentVoucherDetails = _paymentVoucherDetailService.GetObjectsByPaymentVoucherId(paymentVoucher.Id);
                foreach(var paymentVoucherDetail in paymentVoucherDetails)
                {
                    Payable payable = _payableService.GetObjectById(paymentVoucherDetail.PayableId);
                    payable.PendingClearanceAmount -= paymentVoucherDetail.Amount;
                    if (payable.PendingClearanceAmount == 0 && payable.RemainingAmount == 0)
                    {
                        payable.IsCompleted = true;
                        payable.CompletionDate = DateTime.Now;
                    }
                    _payableService.UpdateObject(payable);
                }
            }
            return paymentVoucher;
        }

        public PaymentVoucher UnreconcileObject(PaymentVoucher paymentVoucher, IPaymentVoucherDetailService _paymentVoucherDetailService,
                                                ICashMutationService _cashMutationService, ICashBankService _cashBankService, IPayableService _payableService)
        {
            if (_validator.ValidUnreconcileObject(paymentVoucher))
            {
                _repository.UnreconcileObject(paymentVoucher);

                CashBank cashBank = _cashBankService.GetObjectById(paymentVoucher.CashBankId);
                IList<CashMutation> cashMutations = _cashMutationService.SoftDeleteCashMutationForPaymentVoucher(paymentVoucher, cashBank);
                foreach (var cashMutation in cashMutations)
                {
                    _cashMutationService.ReverseCashMutateObject(cashMutation, _cashBankService);
                }

                IList<PaymentVoucherDetail> paymentVoucherDetails = _paymentVoucherDetailService.GetObjectsByPaymentVoucherId(paymentVoucher.Id);
                foreach (var paymentVoucherDetail in paymentVoucherDetails)
                {
                    Payable payable = _payableService.GetObjectById(paymentVoucherDetail.PayableId);
                    payable.PendingClearanceAmount += paymentVoucherDetail.Amount;
                    if (payable.PendingClearanceAmount != 0 || payable.RemainingAmount != 0)
                    {
                        payable.IsCompleted = false;
                        payable.CompletionDate = null;
                    }
                    _payableService.UpdateObject(payable);
                }
            }
            return paymentVoucher;
        }
    }
}