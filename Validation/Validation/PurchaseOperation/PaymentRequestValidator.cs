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
    public class PaymentRequestValidator : IPaymentRequestValidator
    {
        public PaymentRequest VHasContact(PaymentRequest paymentRequest, IContactService _contactService)
        {
            Contact contact = _contactService.GetObjectById(paymentRequest.ContactId);
            if (contact == null)
            {
                paymentRequest.Errors.Add("Contact", "Tidak valid");
            }
            return paymentRequest;
        }

        public PaymentRequest VHasRequestedDate(PaymentRequest paymentRequest)
        {
            if (paymentRequest.RequestedDate == null)
            {
                paymentRequest.Errors.Add("RequestedDate", "Tidak boleh kosong");
            }
            return paymentRequest;
        }

        public PaymentRequest VHasDueDate(PaymentRequest paymentRequest)
        {
            if (paymentRequest.DueDate == null)
            {
                paymentRequest.Errors.Add("DueDate", "Tidak boleh kosong");
            }
            return paymentRequest;
        }

        public PaymentRequest VIsValidAmount(PaymentRequest paymentRequest)
        {
            if (paymentRequest.Amount < 0)
            {
                paymentRequest.Errors.Add("Amount", "Harus lebih besar atau sama dengan 0");
            }
            return paymentRequest;
        }

        public PaymentRequest VHasBeenConfirmed(PaymentRequest paymentRequest)
        {
            if (!paymentRequest.IsConfirmed)
            {
                paymentRequest.Errors.Add("Generic", "Sudah dikonfirmasi");
            }
            return paymentRequest;
        }

        public PaymentRequest VHasNotBeenConfirmed(PaymentRequest paymentRequest)
        {
            if (paymentRequest.IsConfirmed)
            {
                paymentRequest.Errors.Add("Generic", "Tidak boleh sudah dikonfirmasi");
            }
            return paymentRequest;
        }

        public PaymentRequest VHasNotBeenDeleted(PaymentRequest paymentRequest)
        {
            if (paymentRequest.IsDeleted)
            {
                paymentRequest.Errors.Add("Generic", "Sudah dihapus");
            }
            return paymentRequest;
        }

        public PaymentRequest VPayableHasNoOtherAssociation(PaymentRequest paymentRequest, IPayableService _payableService, IPaymentVoucherDetailService _paymentVoucherDetailService)
        {
            Payable payable = _payableService.GetObjectBySource(Constant.PayableSource.PaymentRequest, paymentRequest.Id);
            IList<PaymentVoucherDetail> paymentVoucherDetails = _paymentVoucherDetailService.GetObjectsByPayableId(payable.Id);
            if (paymentVoucherDetails.Any())
            {
                paymentRequest.Errors.Add("Generic", "Payable memiliki asosiasi dengan payment voucher detail");
                return paymentRequest;
            }
            return paymentRequest;
        }

        public PaymentRequest VHasConfirmationDate(PaymentRequest obj)
        {
            if (obj.ConfirmationDate == null)
            {
                obj.Errors.Add("ConfirmationDate", "Tidak boleh kosong");
            }
            return obj;
        }

        public PaymentRequest VCreateObject(PaymentRequest paymentRequest, IContactService _contactService)
        {
            VHasContact(paymentRequest, _contactService);
            if (!isValid(paymentRequest)) { return paymentRequest; }
            VHasRequestedDate(paymentRequest);
            if (!isValid(paymentRequest)) { return paymentRequest; }
            VHasDueDate(paymentRequest);
            if (!isValid(paymentRequest)) { return paymentRequest; }
            VIsValidAmount(paymentRequest);
            return paymentRequest;
        }

        public PaymentRequest VUpdateObject(PaymentRequest paymentRequest, IContactService _contactService)
        {
            VHasNotBeenConfirmed(paymentRequest);
            if (!isValid(paymentRequest)) { return paymentRequest; }
            VHasNotBeenDeleted(paymentRequest);
            if (!isValid(paymentRequest)) { return paymentRequest; }
            VCreateObject(paymentRequest, _contactService);
            return paymentRequest;
        }

        public PaymentRequest VDeleteObject(PaymentRequest paymentRequest)
        {
            VHasNotBeenConfirmed(paymentRequest);
            if (!isValid(paymentRequest)) { return paymentRequest; }
            VHasNotBeenDeleted(paymentRequest);
            return paymentRequest;
        }

        

        public PaymentRequest VConfirmObject(PaymentRequest paymentRequest)
        {
            VHasConfirmationDate(paymentRequest);
            if (!isValid(paymentRequest)) { return paymentRequest; }
            VHasNotBeenDeleted(paymentRequest);
            if (!isValid(paymentRequest)) { return paymentRequest; }
            VHasNotBeenConfirmed(paymentRequest);
            return paymentRequest;
        }

        public PaymentRequest VUnconfirmObject(PaymentRequest paymentRequest, IPaymentVoucherDetailService _paymentVoucherDetailService, IPayableService _payableService)
        {
            VHasBeenConfirmed(paymentRequest);
            if (!isValid(paymentRequest)) { return paymentRequest; }
            VHasNotBeenDeleted(paymentRequest);
            if (!isValid(paymentRequest)) { return paymentRequest; }
            VPayableHasNoOtherAssociation(paymentRequest, _payableService, _paymentVoucherDetailService); 
            return paymentRequest;
        }

        public bool ValidCreateObject(PaymentRequest paymentRequest, IContactService _contactService)
        {
            VCreateObject(paymentRequest, _contactService);
            return isValid(paymentRequest);
        }

        public bool ValidUpdateObject(PaymentRequest paymentRequest, IContactService _contactService)
        {
            paymentRequest.Errors.Clear();
            VUpdateObject(paymentRequest, _contactService);
            return isValid(paymentRequest);
        }

        public bool ValidDeleteObject(PaymentRequest paymentRequest)
        {
            paymentRequest.Errors.Clear();
            VDeleteObject(paymentRequest);
            return isValid(paymentRequest);
        }

        public bool ValidConfirmObject(PaymentRequest paymentRequest)
        {
            paymentRequest.Errors.Clear();
            VConfirmObject(paymentRequest);
            return isValid(paymentRequest);
        }

        public bool ValidUnconfirmObject(PaymentRequest paymentRequest, IPaymentVoucherDetailService _paymentVoucherDetailService, IPayableService _payableService)
        {
            paymentRequest.Errors.Clear();
            VUnconfirmObject(paymentRequest, _paymentVoucherDetailService, _payableService);
            return isValid(paymentRequest);
        }

        public bool isValid(PaymentRequest obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public string PrintError(PaymentRequest obj)
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