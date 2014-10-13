using Core.DomainModel;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface IPaymentRequestService
    {
        IQueryable<PaymentRequest> GetQueryable();
        IPaymentRequestValidator GetValidator();
        IList<PaymentRequest> GetAll();
        PaymentRequest GetObjectById(int Id);
        IList<PaymentRequest> GetObjectsByContactId(int contactId);
        PaymentRequest CreateObject(PaymentRequest paymentRequest, IContactService _contactService);
        PaymentRequest CreateObject(int contactId, string description, decimal amount, DateTime requestedDate, DateTime dueDate, IContactService _contactService);
        PaymentRequest UpdateObject(PaymentRequest paymentRequest, IContactService _contactService);
        PaymentRequest SoftDeleteObject(PaymentRequest paymentRequest);
        bool DeleteObject(int Id);
        PaymentRequest ConfirmObject(PaymentRequest paymentRequest, DateTime ConfirmationDate, IPayableService _payableService);
        PaymentRequest UnconfirmObject(PaymentRequest paymentRequest, IPaymentVoucherDetailService _paymentVoucherDetailService, IPayableService _payableService);
    }
}