using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Interface.Validation;
using Core.DomainModel;
using Core.Interface.Service;
using Core.Constants;

namespace Validation.Validation
{
    public class ReceiptRequestDetailValidator : IReceiptRequestDetailValidator
    {
        public ReceiptRequestDetail VHasReceiptRequest(ReceiptRequestDetail receiptRequestDetail, IReceiptRequestService _receiptRequestService)
        {
            ReceiptRequest receiptRequest = _receiptRequestService.GetObjectById(receiptRequestDetail.ReceiptRequestId);
            if (receiptRequest == null)
            {
                receiptRequestDetail.Errors.Add("ReceiptRequestId", "Tidak boleh tidak ada");
            }
            return receiptRequestDetail;
        }

        public ReceiptRequestDetail VHasAccount(ReceiptRequestDetail receiptRequestDetail, IAccountService _accountService)
        {
            Account account = _accountService.GetObjectById(receiptRequestDetail.AccountId);
            if (account == null)
            {
                receiptRequestDetail.Errors.Add("AccountId", "Tidak boleh tidak ada");
            }
            return receiptRequestDetail;
        }

        public ReceiptRequestDetail VHasNotBeenConfirmed(ReceiptRequestDetail receiptRequestDetail)
        {
            if (receiptRequestDetail.IsConfirmed)
            {
                receiptRequestDetail.Errors.Add("Generic", "Sudah dikonfirmasi");
            }
            return receiptRequestDetail;
        }

        public ReceiptRequestDetail VHasBeenConfirmed(ReceiptRequestDetail receiptRequestDetail)
        {
            if (!receiptRequestDetail.IsConfirmed)
            {
                receiptRequestDetail.Errors.Add("Generic", "Belum dikonfirmasi");
            }
            return receiptRequestDetail;
        }

        public ReceiptRequestDetail VHasNotBeenDeleted(ReceiptRequestDetail receiptRequestDetail)
        {
            if (receiptRequestDetail.IsDeleted)
            {
                receiptRequestDetail.Errors.Add("Generic", "Sudah didelete");
            }
            return receiptRequestDetail;
        }

        public ReceiptRequestDetail VAmountIsTheSameWithReceiptRequestAmount(ReceiptRequestDetail receiptRequestDetail, IReceiptRequestService _receiptRequestService)
        {
            ReceiptRequest receiptRequest = _receiptRequestService.GetObjectById(receiptRequestDetail.ReceiptRequestId);
            if (receiptRequestDetail.Amount != receiptRequest.Amount)
            {
                receiptRequest.Errors.Add("Generic", "Account Payable Non Trading tidak memiliki amount yang sama dengan Payment Request");
            }
            return receiptRequestDetail;
        }

        public ReceiptRequestDetail VNonNegativeAmount(ReceiptRequestDetail receiptRequestDetail)
        {
            if (receiptRequestDetail.Amount < 0)
            {
                receiptRequestDetail.Errors.Add("Amount", "Tidak boleh kurang dari 0");
            }
            return receiptRequestDetail;
        }

        public ReceiptRequestDetail VStatusIsCredit(ReceiptRequestDetail receiptRequestDetail)
        {
            if (receiptRequestDetail.Status == Constant.GeneralLedgerStatus.Debit)
            {
                receiptRequestDetail.Errors.Add("Generic", "Sistem mengharapkan posting General Ledger credit");
            }
            return receiptRequestDetail;
        }

        public ReceiptRequestDetail VStatusIsDebit(ReceiptRequestDetail receiptRequestDetail)
        {
            if (receiptRequestDetail.Status == Constant.GeneralLedgerStatus.Credit)
            {
                receiptRequestDetail.Errors.Add("Generic", "Sistem mengharapkan posting General Ledger debit");
            }
            return receiptRequestDetail;
        }

        public ReceiptRequestDetail VNotLegacyObject(ReceiptRequestDetail receiptRequestDetail)
        {
            if (receiptRequestDetail.IsLegacy)
            {
                receiptRequestDetail.Errors.Add("Generic", "Hanya untuk non legacy object");
            }
            return receiptRequestDetail;
        }

        public ReceiptRequestDetail VIsLegacyObject(ReceiptRequestDetail receiptRequestDetail)
        {
            if (!receiptRequestDetail.IsLegacy)
            {
                receiptRequestDetail.Errors.Add("Generic", "Hanya untuk legacy object");
            }
            return receiptRequestDetail;
        }

        public ReceiptRequestDetail VCreateObject(ReceiptRequestDetail receiptRequestDetail, IReceiptRequestService _receiptRequestService, IReceiptRequestDetailService _receiptRequestDetailService, IAccountService _accountService)
        {
            VHasReceiptRequest(receiptRequestDetail, _receiptRequestService);
            if (!isValid(receiptRequestDetail)) { return receiptRequestDetail; }
            VHasAccount(receiptRequestDetail, _accountService);
            if (!isValid(receiptRequestDetail)) { return receiptRequestDetail; }
            VHasNotBeenConfirmed(receiptRequestDetail);
            if (!isValid(receiptRequestDetail)) { return receiptRequestDetail; }
            VHasNotBeenDeleted(receiptRequestDetail);
            if (!isValid(receiptRequestDetail)) { return receiptRequestDetail; }
            VNotLegacyObject(receiptRequestDetail);
            if (!isValid(receiptRequestDetail)) { return receiptRequestDetail; }
            VNonNegativeAmount(receiptRequestDetail);
            if (!isValid(receiptRequestDetail)) { return receiptRequestDetail; }
            VStatusIsDebit(receiptRequestDetail);
            return receiptRequestDetail;
        }

        public ReceiptRequestDetail VUpdateObject(ReceiptRequestDetail receiptRequestDetail, IReceiptRequestService _receiptRequestService, IReceiptRequestDetailService _receiptRequestDetailService, IAccountService _accountService)
        {
            VHasNotBeenConfirmed(receiptRequestDetail);
            if (!isValid(receiptRequestDetail)) { return receiptRequestDetail; }
            VCreateObject(receiptRequestDetail, _receiptRequestService, _receiptRequestDetailService, _accountService);
            return receiptRequestDetail;    
        }

        public ReceiptRequestDetail VCreateLegacyObject(ReceiptRequestDetail receiptRequestDetail, IReceiptRequestService _receiptRequestService, IReceiptRequestDetailService _receiptRequestDetailService, IAccountService _accountService)
        {
            VHasReceiptRequest(receiptRequestDetail, _receiptRequestService);
            if (!isValid(receiptRequestDetail)) { return receiptRequestDetail; }
            VHasAccount(receiptRequestDetail, _accountService);
            if (!isValid(receiptRequestDetail)) { return receiptRequestDetail; }
            VHasNotBeenConfirmed(receiptRequestDetail);
            if (!isValid(receiptRequestDetail)) { return receiptRequestDetail; }
            VHasNotBeenDeleted(receiptRequestDetail);
            if (!isValid(receiptRequestDetail)) { return receiptRequestDetail; }
            VIsLegacyObject(receiptRequestDetail);
            if (!isValid(receiptRequestDetail)) { return receiptRequestDetail; }
            VNonNegativeAmount(receiptRequestDetail);
            if (!isValid(receiptRequestDetail)) { return receiptRequestDetail; }
            VAmountIsTheSameWithReceiptRequestAmount(receiptRequestDetail, _receiptRequestService);
            if (!isValid(receiptRequestDetail)) { return receiptRequestDetail; }
            VStatusIsCredit(receiptRequestDetail);
            return receiptRequestDetail;
        }

        public ReceiptRequestDetail VUpdateLegacyObject(ReceiptRequestDetail receiptRequestDetail, IReceiptRequestService _receiptRequestService, IReceiptRequestDetailService _receiptRequestDetailService, IAccountService _accountService)
        {
            VHasNotBeenConfirmed(receiptRequestDetail);
            if (!isValid(receiptRequestDetail)) { return receiptRequestDetail; }
            VCreateLegacyObject(receiptRequestDetail, _receiptRequestService, _receiptRequestDetailService, _accountService);
            return receiptRequestDetail;
        }
        public ReceiptRequestDetail VDeleteObject(ReceiptRequestDetail receiptRequestDetail)
        {
            VHasNotBeenConfirmed(receiptRequestDetail);
            if (!isValid(receiptRequestDetail)) { return receiptRequestDetail; }
            VHasNotBeenDeleted(receiptRequestDetail);
            if (!isValid(receiptRequestDetail)) { return receiptRequestDetail; }
            VNotLegacyObject(receiptRequestDetail);
            return receiptRequestDetail;
        }

        public ReceiptRequestDetail VHasConfirmationDate(ReceiptRequestDetail obj)
        {
            if (obj.ConfirmationDate == null)
            {
                obj.Errors.Add("ConfirmationDate", "Tidak boleh kosong");
            }
            return obj;
        }

        public ReceiptRequestDetail VConfirmObject(ReceiptRequestDetail receiptRequestDetail)
        {
            VHasConfirmationDate(receiptRequestDetail);
            if (!isValid(receiptRequestDetail)) { return receiptRequestDetail; }
            VHasNotBeenConfirmed(receiptRequestDetail);
            return receiptRequestDetail;
        }

        public ReceiptRequestDetail VUnconfirmObject(ReceiptRequestDetail receiptRequestDetail)
        {
            VHasBeenConfirmed(receiptRequestDetail);
            return receiptRequestDetail;
        }

        public bool ValidCreateObject(ReceiptRequestDetail receiptRequestDetail, IReceiptRequestService _receiptRequestService, IReceiptRequestDetailService _receiptRequestDetailService, IAccountService _accountService)
        {
            VCreateObject(receiptRequestDetail, _receiptRequestService, _receiptRequestDetailService, _accountService);
            return isValid(receiptRequestDetail);
        }

        public bool ValidUpdateObject(ReceiptRequestDetail receiptRequestDetail, IReceiptRequestService _receiptRequestService, IReceiptRequestDetailService _receiptRequestDetailService, IAccountService _accountService)
        {
            VUpdateObject(receiptRequestDetail, _receiptRequestService, _receiptRequestDetailService, _accountService);
            return isValid(receiptRequestDetail);
        }

        public bool ValidCreateLegacyObject(ReceiptRequestDetail receiptRequestDetail, IReceiptRequestService _receiptRequestService, IReceiptRequestDetailService _receiptRequestDetailService, IAccountService _accountService)
        {
            VCreateLegacyObject(receiptRequestDetail, _receiptRequestService, _receiptRequestDetailService, _accountService);
            return isValid(receiptRequestDetail);
        }

        public bool ValidUpdateLegacyObject(ReceiptRequestDetail receiptRequestDetail, IReceiptRequestService _receiptRequestService, IReceiptRequestDetailService _receiptRequestDetailService, IAccountService _accountService)
        {
            VUpdateLegacyObject(receiptRequestDetail, _receiptRequestService, _receiptRequestDetailService, _accountService);
            return isValid(receiptRequestDetail);
        }

        public bool ValidDeleteObject(ReceiptRequestDetail receiptRequestDetail)
        {
            VDeleteObject(receiptRequestDetail);
            return isValid(receiptRequestDetail);
        }

        public bool ValidConfirmObject(ReceiptRequestDetail receiptRequestDetail)
        {
            VConfirmObject(receiptRequestDetail);
            return isValid(receiptRequestDetail);
        }

        public bool ValidUnconfirmObject(ReceiptRequestDetail receiptRequestDetail)
        {
            VUnconfirmObject(receiptRequestDetail);
            return isValid(receiptRequestDetail);
        }

        public bool isValid(ReceiptRequestDetail obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public string PrintError(ReceiptRequestDetail obj)
        {
            string erroroutput = "";
            KeyValuePair<string, string> first = obj.Errors.ElementAt(0);
            erroroutput += first.Key + "," + first.Value;
            foreach (KeyValuePair<string, string> pair in obj.Errors.Skip(1))
            {
                erroroutput += Environment.NewLine;
                erroroutput += pair.Key + "," + pair.Value;
            }
            return erroroutput;
        }
    }
}