using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.DomainModel;
using Core.Interface.Service;

namespace Core.Interface.Validation
{
    public interface IPaymentRequestValidator
    {
        PaymentRequest VHasContact(PaymentRequest paymentRequest, IContactService _contactService);
        PaymentRequest VHasRequestedDate(PaymentRequest paymentRequest);
        PaymentRequest VHasDueDate(PaymentRequest paymentRequest);
        PaymentRequest VIsValidAmount(PaymentRequest paymentRequest);
        PaymentRequest VHasNotBeenConfirmed(PaymentRequest paymentRequest);
        PaymentRequest VHasBeenConfirmed(PaymentRequest paymentRequest);
        PaymentRequest VHasNotBeenDeleted(PaymentRequest paymentRequest);
        PaymentRequest VPayableHasNoOtherAssociation(PaymentRequest paymentRequest, IPayableService _payableService, IPaymentVoucherDetailService _paymentVoucherDetailService);
        PaymentRequest VHasConfirmationDate(PaymentRequest paymentRequest);

        PaymentRequest VCreateObject(PaymentRequest paymentRequest, IContactService _contactService);
        PaymentRequest VUpdateObject(PaymentRequest paymentRequest, IContactService _contactService);
        PaymentRequest VDeleteObject(PaymentRequest paymentRequest);
        PaymentRequest VConfirmObject(PaymentRequest paymentRequest);
        PaymentRequest VUnconfirmObject(PaymentRequest paymentRequest, IPaymentVoucherDetailService _paymentVoucherDetailService, IPayableService _payableService);
        bool ValidCreateObject(PaymentRequest paymentRequest, IContactService _contactService);
        bool ValidUpdateObject(PaymentRequest paymentRequest, IContactService _contactService);
        bool ValidDeleteObject(PaymentRequest paymentRequest);
        bool ValidConfirmObject(PaymentRequest paymentRequest);
        bool ValidUnconfirmObject(PaymentRequest paymentRequest, IPaymentVoucherDetailService _paymentVoucherDetailService, IPayableService _payableService);
        bool isValid(PaymentRequest paymentRequest);
        string PrintError(PaymentRequest paymentRequest);
    }
}
