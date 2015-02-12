using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.DomainModel;
using Core.Interface.Service;

namespace Core.Interface.Validation
{
    public interface IReceiptRequestDetailValidator
    {
        ReceiptRequestDetail VHasReceiptRequest(ReceiptRequestDetail ReceiptRequestDetail, IReceiptRequestService _ReceiptRequestService);
        ReceiptRequestDetail VHasAccount(ReceiptRequestDetail ReceiptRequestDetail, IAccountService _accountService);
        ReceiptRequestDetail VHasBeenConfirmed(ReceiptRequestDetail ReceiptRequestDetail);
        ReceiptRequestDetail VHasNotBeenConfirmed(ReceiptRequestDetail ReceiptRequestDetail);
        ReceiptRequestDetail VHasNotBeenDeleted(ReceiptRequestDetail ReceiptRequestDetail);
        ReceiptRequestDetail VAmountIsTheSameWithReceiptRequestAmount(ReceiptRequestDetail ReceiptRequestDetail, IReceiptRequestService _ReceiptRequestService);
        ReceiptRequestDetail VNonNegativeAmount(ReceiptRequestDetail ReceiptRequestDetail);
        ReceiptRequestDetail VStatusIsCredit(ReceiptRequestDetail ReceiptRequestDetail);
        ReceiptRequestDetail VStatusIsDebit(ReceiptRequestDetail ReceiptRequestDetail);
        ReceiptRequestDetail VNotLegacyObject(ReceiptRequestDetail ReceiptRequestDetail);
        ReceiptRequestDetail VCreateObject(ReceiptRequestDetail ReceiptRequestDetail, IReceiptRequestService _ReceiptRequestService, IReceiptRequestDetailService _ReceiptRequestDetailService, IAccountService _accountService);
        ReceiptRequestDetail VUpdateObject(ReceiptRequestDetail ReceiptRequestDetail, IReceiptRequestService _ReceiptRequestService, IReceiptRequestDetailService _ReceiptRequestDetailService, IAccountService _accountService);
        ReceiptRequestDetail VDeleteObject(ReceiptRequestDetail ReceiptRequestDetail);
        ReceiptRequestDetail VCreateLegacyObject(ReceiptRequestDetail ReceiptRequestDetail, IReceiptRequestService _ReceiptRequestService, IReceiptRequestDetailService _ReceiptRequestDetailService, IAccountService _accountService);
        ReceiptRequestDetail VUpdateLegacyObject(ReceiptRequestDetail ReceiptRequestDetail, IReceiptRequestService _ReceiptRequestService, IReceiptRequestDetailService _ReceiptRequestDetailService, IAccountService _accountService);
        ReceiptRequestDetail VHasConfirmationDate(ReceiptRequestDetail ReceiptRequestDetail);
        ReceiptRequestDetail VConfirmObject(ReceiptRequestDetail ReceiptRequestDetail);
        ReceiptRequestDetail VUnconfirmObject(ReceiptRequestDetail ReceiptRequestDetail);
        bool ValidCreateObject(ReceiptRequestDetail ReceiptRequestDetail, IReceiptRequestService _ReceiptRequestService, IReceiptRequestDetailService _ReceiptRequestDetailService, IAccountService _accountService);
        bool ValidUpdateObject(ReceiptRequestDetail ReceiptRequestDetail, IReceiptRequestService _ReceiptRequestService, IReceiptRequestDetailService _ReceiptRequestDetailService, IAccountService _accountService);
        bool ValidDeleteObject(ReceiptRequestDetail ReceiptRequestDetail);
        bool ValidCreateLegacyObject(ReceiptRequestDetail ReceiptRequestDetail, IReceiptRequestService _ReceiptRequestService, IReceiptRequestDetailService _ReceiptRequestDetailService, IAccountService _accountService);
        bool ValidUpdateLegacyObject(ReceiptRequestDetail ReceiptRequestDetail, IReceiptRequestService _ReceiptRequestService, IReceiptRequestDetailService _ReceiptRequestDetailService, IAccountService _accountService);
        bool ValidConfirmObject(ReceiptRequestDetail ReceiptRequestDetail);
        bool ValidUnconfirmObject(ReceiptRequestDetail ReceiptRequestDetail);
        bool isValid(ReceiptRequestDetail ReceiptRequestDetail);
        string PrintError(ReceiptRequestDetail ReceiptRequestDetail);
    }
}
