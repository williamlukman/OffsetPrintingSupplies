using Core.DomainModel;
using Core.Interface.Repository;
using Core.Interface.Service;
using Core.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Interface.Validation;

namespace Service.Service
{
    public class CashMutationService : ICashMutationService
    {
        private ICashMutationRepository _repository;
        private ICashMutationValidator _validator;
        public CashMutationService(ICashMutationRepository _cashMutationRepository, ICashMutationValidator _cashMutationValidator)
        {
            _repository = _cashMutationRepository;
            _validator = _cashMutationValidator;
        }

        public ICashMutationValidator GetValidator()
        {
            return _validator;
        }

        public IQueryable<CashMutation> GetQueryable()
        {
            return _repository.GetQueryable();
        }

        public IList<CashMutation> GetAll()
        {
            return _repository.GetAll();
        }

        public IList<CashMutation> GetObjectsByCashBankId(int cashBankId)
        {
            return _repository.GetObjectsByCashBankId(cashBankId);
        }

        public CashMutation GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public IList<CashMutation> GetObjectsBySourceDocument(int cashBankId, string SourceDocumentType, int SourceDocumentId)
        {
            return _repository.GetObjectsBySourceDocument(cashBankId, SourceDocumentType, SourceDocumentId);
        }

        public CashMutation CreateObject(CashMutation cashMutation, ICashBankService _cashBankService)
        {
            cashMutation.Errors = new Dictionary<String, String>();
            return (_validator.ValidCreateObject(cashMutation, _cashBankService) ? _repository.CreateObject(cashMutation) : cashMutation);
        }

        public CashMutation SoftDeleteObject(CashMutation cashMutation)
        {
            return (_validator.ValidDeleteObject(cashMutation) ? _repository.SoftDeleteObject(cashMutation) : cashMutation);
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }

        #region PaymentVoucher
        public CashMutation CreateCashMutationForPaymentVoucher(PaymentVoucher paymentVoucher, CashBank cashBank)
        {
            decimal Total = paymentVoucher.TotalAmount - (paymentVoucher.TotalPPH21 + paymentVoucher.TotalPPH23 - paymentVoucher.BiayaBank + (paymentVoucher.StatusPembulatan == Constant.GeneralLedgerStatus.Credit ? paymentVoucher.Pembulatan : -paymentVoucher.Pembulatan));
            CashMutation cashMutation = new CashMutation();
            cashMutation.CashBankId = cashBank.Id;
            cashMutation.Amount = Math.Abs(Total);
            cashMutation.MutationDate = paymentVoucher.IsGBCH ? (DateTime) paymentVoucher.ReconciliationDate.GetValueOrDefault() : (DateTime) paymentVoucher.ConfirmationDate.GetValueOrDefault();
            cashMutation.SourceDocumentType = Constant.SourceDocumentType.PaymentVoucher;
            cashMutation.SourceDocumentId = paymentVoucher.Id;
            cashMutation.SourceDocumentCode = paymentVoucher.Code;
            cashMutation.Status = Constant.MutationStatus.Deduction;
            return _repository.CreateObject(cashMutation);
        }

        public IList<CashMutation> SoftDeleteCashMutationForPaymentVoucher(PaymentVoucher paymentVoucher, CashBank cashBank)
        {
            IList<CashMutation> cashMutations = _repository.GetObjectsBySourceDocument(cashBank.Id, Constant.SourceDocumentType.PaymentVoucher, paymentVoucher.Id);
            foreach (var cashMutation in cashMutations)
            {
                _repository.Delete(cashMutation);
            }
            return cashMutations;
        }
        #endregion

        #region Purchase Allowance
        public CashMutation CreateCashMutationForPurchaseAllowance(PurchaseAllowance purchaseAllowance, CashBank cashBank)
        {
            CashMutation cashMutation = new CashMutation();
            cashMutation.CashBankId = cashBank.Id;
            cashMutation.Amount = Math.Abs(purchaseAllowance.TotalAmount);
            cashMutation.MutationDate = purchaseAllowance.IsGBCH ? (DateTime)purchaseAllowance.ReconciliationDate.GetValueOrDefault() : (DateTime)purchaseAllowance.ConfirmationDate.GetValueOrDefault();
            cashMutation.SourceDocumentType = Constant.SourceDocumentType.PurchaseAllowance;
            cashMutation.SourceDocumentId = purchaseAllowance.Id;
            cashMutation.SourceDocumentCode = purchaseAllowance.Code;
            cashMutation.Status = Constant.MutationStatus.Deduction;
            return _repository.CreateObject(cashMutation);
        }

        public IList<CashMutation> SoftDeleteCashMutationForPurchaseAllowance(PurchaseAllowance purchaseAllowance, CashBank cashBank)
        {
            IList<CashMutation> cashMutations = _repository.GetObjectsBySourceDocument(cashBank.Id, Constant.SourceDocumentType.PurchaseAllowance, purchaseAllowance.Id);
            foreach (var cashMutation in cashMutations)
            {
                _repository.Delete(cashMutation);
            }
            return cashMutations;
        }
        #endregion

        #region Receipt Voucher
        public CashMutation CreateCashMutationForReceiptVoucher(ReceiptVoucher receiptVoucher, CashBank cashBank)
        {
            decimal Total = receiptVoucher.TotalAmount - (receiptVoucher.TotalPPH23 + receiptVoucher.BiayaBank + (receiptVoucher.StatusPembulatan == Constant.GeneralLedgerStatus.Credit ? -receiptVoucher.Pembulatan : receiptVoucher.Pembulatan));
            CashMutation cashMutation = new CashMutation();
            cashMutation.CashBankId = cashBank.Id;
            cashMutation.Amount = Math.Abs(Total);
            cashMutation.MutationDate = receiptVoucher.IsGBCH ? (DateTime) receiptVoucher.ReconciliationDate.GetValueOrDefault() : (DateTime) receiptVoucher.ConfirmationDate.GetValueOrDefault();
            cashMutation.SourceDocumentType = Constant.SourceDocumentType.ReceiptVoucher;
            cashMutation.SourceDocumentId = receiptVoucher.Id;
            cashMutation.SourceDocumentCode = receiptVoucher.Code;
            cashMutation.Status = Constant.MutationStatus.Addition;
            return _repository.CreateObject(cashMutation);
        }

        public IList<CashMutation> SoftDeleteCashMutationForReceiptVoucher(ReceiptVoucher receiptVoucher, CashBank cashBank)
        {
            IList<CashMutation> cashMutations = _repository.GetObjectsBySourceDocument(cashBank.Id, Constant.SourceDocumentType.ReceiptVoucher, receiptVoucher.Id);
            foreach (var cashMutation in cashMutations)
            {
                _repository.Delete(cashMutation);
            }
            return cashMutations;
        }
        #endregion

        #region Sales Allowance
        public CashMutation CreateCashMutationForSalesAllowance(SalesAllowance salesAllowance, CashBank cashBank)
        {
            CashMutation cashMutation = new CashMutation();
            cashMutation.CashBankId = cashBank.Id;
            cashMutation.Amount = Math.Abs(salesAllowance.TotalAmount);
            cashMutation.MutationDate = salesAllowance.IsGBCH ? (DateTime)salesAllowance.ReconciliationDate.GetValueOrDefault() : (DateTime)salesAllowance.ConfirmationDate.GetValueOrDefault();
            cashMutation.SourceDocumentType = Constant.SourceDocumentType.SalesAllowance;
            cashMutation.SourceDocumentId = salesAllowance.Id;
            cashMutation.SourceDocumentCode = salesAllowance.Code;
            cashMutation.Status = Constant.MutationStatus.Addition;
            return _repository.CreateObject(cashMutation);
        }

        public IList<CashMutation> SoftDeleteCashMutationForSalesAllowance(SalesAllowance salesAllowance, CashBank cashBank)
        {
            IList<CashMutation> cashMutations = _repository.GetObjectsBySourceDocument(cashBank.Id, Constant.SourceDocumentType.SalesAllowance, salesAllowance.Id);
            foreach (var cashMutation in cashMutations)
            {
                _repository.Delete(cashMutation);
            }
            return cashMutations;
        }
        #endregion

        #region Cash Bank Adjustment
        public CashMutation CreateCashMutationForCashBankAdjustment(CashBankAdjustment cashBankAdjustment, CashBank cashBank)
        {
            CashMutation cashMutation = new CashMutation();
            cashMutation.CashBankId = cashBank.Id;
            cashMutation.Amount = Math.Abs(cashBankAdjustment.Amount);
            cashMutation.MutationDate = (DateTime) cashBankAdjustment.ConfirmationDate.GetValueOrDefault();
            cashMutation.SourceDocumentType = Constant.SourceDocumentType.CashBankAdjustment;
            cashMutation.SourceDocumentId = cashBankAdjustment.Id;
            cashMutation.SourceDocumentCode = cashBankAdjustment.Code;
            cashMutation.Status = (cashBankAdjustment.Amount >= 0) ? Constant.MutationStatus.Addition : Constant.MutationStatus.Deduction;
            return _repository.CreateObject(cashMutation);
        }

        public IList<CashMutation> SoftDeleteCashMutationForCashBankAdjustment(CashBankAdjustment cashBankAdjustment, CashBank cashBank)
        {
            IList<CashMutation> cashMutations = _repository.GetObjectsBySourceDocument(cashBank.Id, Constant.SourceDocumentType.CashBankAdjustment, cashBankAdjustment.Id);
            foreach (var cashMutation in cashMutations)
            {
                _repository.Delete(cashMutation);
            }
            return cashMutations;
        }
        #endregion

        #region Interest Income
        public CashMutation CreateCashMutationForBankAdministration(BankAdministration bankAdministration, CashBank cashBank)
        {
            //decimal Amount = (bankAdministration.PendapatanJasaAmount + bankAdministration.PendapatanBungaAmount + bankAdministration.PengembalianPiutangAmount) -
            //                 (bankAdministration.BiayaAdminAmount + bankAdministration.BiayaBungaAmount);
            decimal Amount = bankAdministration.Amount;
            CashMutation cashMutation = new CashMutation();
            cashMutation.CashBankId = cashBank.Id;
            cashMutation.Amount = Math.Abs(Amount);
            cashMutation.MutationDate = (DateTime)bankAdministration.ConfirmationDate.GetValueOrDefault();
            cashMutation.SourceDocumentType = Constant.SourceDocumentType.BankAdministration;
            cashMutation.SourceDocumentId = bankAdministration.Id;
            cashMutation.SourceDocumentCode = bankAdministration.Code;
            cashMutation.Status = (Amount < 0) ? Constant.MutationStatus.Deduction : Constant.MutationStatus.Addition;
            return _repository.CreateObject(cashMutation);
        }

        public IList<CashMutation> SoftDeleteCashMutationForBankAdministration(BankAdministration bankAdministration, CashBank cashBank)
        {
            IList<CashMutation> cashMutations = _repository.GetObjectsBySourceDocument(cashBank.Id, Constant.SourceDocumentType.BankAdministration, bankAdministration.Id);
            foreach (var cashMutation in cashMutations)
            {
                _repository.Delete(cashMutation);
            }
            return cashMutations;
        }
        #endregion

        #region Cash Bank Mutation
        public IList<CashMutation> CreateCashMutationForCashBankMutation(CashBankMutation cashBankMutation, CashBank sourceCashBank, CashBank targetCashBank)
        {
            IList<CashMutation> results = new List<CashMutation>();

            CashMutation sourceCashMutation = new CashMutation();
            sourceCashMutation.CashBankId = sourceCashBank.Id;
            sourceCashMutation.Amount = Math.Abs(cashBankMutation.Amount);
            sourceCashMutation.MutationDate = (DateTime) cashBankMutation.ConfirmationDate.GetValueOrDefault();
            sourceCashMutation.SourceDocumentType = Constant.SourceDocumentType.CashBankMutation;
            sourceCashMutation.SourceDocumentId = cashBankMutation.Id;
            sourceCashMutation.SourceDocumentCode = cashBankMutation.Code;
            sourceCashMutation.Status = Constant.MutationStatus.Deduction;
            _repository.CreateObject(sourceCashMutation);

            CashMutation targetCashMutation = new CashMutation();
            targetCashMutation.CashBankId = targetCashBank.Id;
            targetCashMutation.Amount = Math.Abs(cashBankMutation.Amount);
            targetCashMutation.MutationDate = (DateTime)cashBankMutation.ConfirmationDate.GetValueOrDefault();
            targetCashMutation.SourceDocumentType = Constant.SourceDocumentType.CashBankMutation;
            targetCashMutation.SourceDocumentId = cashBankMutation.Id;
            targetCashMutation.SourceDocumentCode = cashBankMutation.Code;
            targetCashMutation.Status = Constant.MutationStatus.Addition;
            _repository.CreateObject(targetCashMutation);

            results.Add(sourceCashMutation);
            results.Add(targetCashMutation);
            return results;
        }

        public IList<CashMutation> SoftDeleteCashMutationForCashBankMutation(CashBankMutation cashBankMutation, CashBank sourceCashBank, CashBank targetCashBank)
        {
            IList<CashMutation> cashMutations = new List<CashMutation>();

            IList<CashMutation> sourceCashMutations = _repository.GetObjectsBySourceDocument(sourceCashBank.Id, Constant.SourceDocumentType.CashBankMutation, cashBankMutation.Id);
            sourceCashMutations.ToList().ForEach(x => cashMutations.Add(x));
            foreach (var sourceCashMutation in sourceCashMutations)
            {
                _repository.Delete(sourceCashMutation);
            }

            IList<CashMutation> targetCashMutations = _repository.GetObjectsBySourceDocument(targetCashBank.Id, Constant.SourceDocumentType.CashBankMutation, cashBankMutation.Id);
            targetCashMutations.ToList().ForEach(x => cashMutations.Add(x));
            foreach (var targetCashMutation in targetCashMutations)
            {
                _repository.Delete(targetCashMutation);
            }

            return cashMutations;
        }
        #endregion

        public void CashMutateObject(CashMutation cashMutation, ICashBankService _cashBankService, ICurrencyService _currencyService)
        {
            decimal Amount = (cashMutation.Status == Constant.MutationStatus.Addition) ? cashMutation.Amount : (-1) * cashMutation.Amount;
            CashBank cashBank = _cashBankService.GetObjectById(cashMutation.CashBankId);
            cashBank.Amount += Amount;
            _cashBankService.UpdateObject(cashBank,_currencyService);
        }

        public void ReverseCashMutateObject(CashMutation cashMutation, ICashBankService _cashBankService, ICurrencyService _currencyService)
        {
            decimal Amount = (cashMutation.Status == Constant.MutationStatus.Deduction) ? cashMutation.Amount : (-1) * cashMutation.Amount;
            CashBank cashBank = _cashBankService.GetObjectById(cashMutation.CashBankId);
            cashBank.Amount += Amount;
            _cashBankService.UpdateObject(cashBank,_currencyService);
        }
    }
}
