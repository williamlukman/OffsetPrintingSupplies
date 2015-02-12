using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.DomainModel;
using Core.Interface.Service;

namespace Core.Interface.Validation
{
    public interface IReceiptRequestValidator
    {
        ReceiptRequest VHasContact(ReceiptRequest ReceiptRequest, IContactService _contatService);
        ReceiptRequest VIsValidAmount(ReceiptRequest ReceiptRequest);
        ReceiptRequest VDebitEqualCreditEqualAmount(ReceiptRequest ReceiptRequest, IReceiptRequestDetailService _ReceiptRequestDetailService);
        ReceiptRequest VHasNotBeenConfirmed(ReceiptRequest ReceiptRequest);
        ReceiptRequest VHasBeenConfirmed(ReceiptRequest ReceiptRequest);
        ReceiptRequest VHasNotBeenDeleted(ReceiptRequest ReceiptRequest);
        ReceiptRequest VHasConfirmationDate(ReceiptRequest ReceiptRequest);
        ReceiptRequest VGeneralLedgerPostingHasNotBeenClosed(ReceiptRequest ReceiptRequest, IClosingService _closingService);
        ReceiptRequest VCreateObject(ReceiptRequest ReceiptRequest, IContactService _contactService);
        ReceiptRequest VUpdateObject(ReceiptRequest ReceiptRequest, IContactService _contactService);
        ReceiptRequest VDeleteObject(ReceiptRequest ReceiptRequest);
        ReceiptRequest VConfirmObject(ReceiptRequest ReceiptRequest, IReceiptRequestDetailService _ReceiptRequestDetailService, IClosingService _closingService);
        ReceiptRequest VUnconfirmObject(ReceiptRequest ReceiptRequest, IReceiptRequestDetailService _ReceiptRequestDetailService, IClosingService _closingService);
        bool ValidCreateObject(ReceiptRequest ReceiptRequest, IContactService _contactService);
        bool ValidUpdateObject(ReceiptRequest ReceiptRequest, IContactService _contactService);
        bool ValidDeleteObject(ReceiptRequest ReceiptRequest);
        bool ValidConfirmObject(ReceiptRequest ReceiptRequest, IReceiptRequestDetailService _ReceiptRequestDetailService, IClosingService _closingService);
        bool ValidUnconfirmObject(ReceiptRequest ReceiptRequest, IReceiptRequestDetailService _ReceiptRequestDetailService, IClosingService _closingService);
        bool isValid(ReceiptRequest ReceiptRequest);
        string PrintError(ReceiptRequest ReceiptRequest);
    }
}
