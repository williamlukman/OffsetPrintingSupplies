using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.DomainModel;
using Core.Interface.Service;

namespace Core.Interface.Validation
{
    public interface IPaymentVoucherDetailValidator
    {
        PaymentVoucherDetail VHasPaymentVoucher(PaymentVoucherDetail paymentVoucherDetail, IPaymentVoucherService _paymentVoucherService);
        PaymentVoucherDetail VHasPayable(PaymentVoucherDetail paymentVoucherDetail, IPayableService _payableService);
        PaymentVoucherDetail VPayableContactIsTheSame(PaymentVoucherDetail paymentVoucherDetail, IPaymentVoucherService _paymentVoucherService, IPayableService _payableService);
        PaymentVoucherDetail VAmountLessThanCashBank(PaymentVoucherDetail paymentVoucherDetail, IPaymentVoucherService _paymentVoucherService, ICashBankService _cashBankService);
        PaymentVoucherDetail VCorrectInstantClearance(PaymentVoucherDetail paymentVoucherDetail, IPaymentVoucherService _paymentVoucherService, ICashBankService _cashBankService);
        PaymentVoucherDetail VUniqueCashBankId(PaymentVoucherDetail paymentVoucherDetail, IPaymentVoucherService _paymentVoucherService, ICashBankService _cashBankService);
        PaymentVoucherDetail VNonNegativeOrZeroAmount(PaymentVoucherDetail paymentVoucherDetail);
        PaymentVoucherDetail VHasBeenConfirmed(PaymentVoucherDetail paymentVoucherDetail);
        PaymentVoucherDetail VHasNotBeenConfirmed(PaymentVoucherDetail paymentVoucherDetail);
        PaymentVoucherDetail VUpdateContactOrPayable(PaymentVoucherDetail paymentVoucherDetail, IPaymentVoucherDetailService _paymentVoucherDetailService);
        PaymentVoucherDetail VRemainingAmount(PaymentVoucherDetail paymentVoucherDetail, IPayableService _payableService);

        PaymentVoucherDetail VCreateObject(PaymentVoucherDetail paymentVoucherDetail, IPaymentVoucherService _paymentVoucherService, IPaymentVoucherDetailService _paymentVoucherDetailService, ICashBankService _cashBankService, IPayableService _payableService, ICustomerService _customerService);
        PaymentVoucherDetail VUpdateObject(PaymentVoucherDetail paymentVoucherDetail, IPaymentVoucherService _paymentVoucherService, IPaymentVoucherDetailService _paymentVoucherDetailService, ICashBankService _cashBankService, IPayableService _payableService, ICustomerService _customerService);
        PaymentVoucherDetail VDeleteObject(PaymentVoucherDetail paymentVoucherDetail);
        PaymentVoucherDetail VConfirmObject(PaymentVoucherDetail paymentVoucherDetail, IPaymentVoucherService _paymentVoucherService, IPaymentVoucherDetailService _paymentVoucherDetailService, ICashBankService _cashBankService, IPayableService _payableService);
        PaymentVoucherDetail VUnconfirmObject(PaymentVoucherDetail paymentVoucherDetail, IPaymentVoucherService _paymentVoucherService, IPaymentVoucherDetailService _paymentVoucherDetailService, ICashBankService _cashBankService, IPayableService _payableService);
        bool ValidCreateObject(PaymentVoucherDetail paymentVoucherDetail, IPaymentVoucherService _paymentVoucherService, IPaymentVoucherDetailService _paymentVoucherDetailService, ICashBankService _cashBankService, IPayableService _payableService, ICustomerService _customerService);
        bool ValidUpdateObject(PaymentVoucherDetail paymentVoucherDetail, IPaymentVoucherService _paymentVoucherService, IPaymentVoucherDetailService _paymentVoucherDetailService, ICashBankService _cashBankService, IPayableService _payableService, ICustomerService _customerService);
        bool ValidDeleteObject(PaymentVoucherDetail paymentVoucherDetail);
        bool ValidConfirmObject(PaymentVoucherDetail paymentVoucherDetail, IPaymentVoucherService _paymentVoucherService, IPaymentVoucherDetailService _paymentVoucherDetailService, ICashBankService _cashBankService, IPayableService _payableService);
        bool ValidUnconfirmObject(PaymentVoucherDetail paymentVoucherDetail, IPaymentVoucherService _paymentVoucherService, IPaymentVoucherDetailService _paymentVoucherDetailService, ICashBankService _cashBankService, IPayableService _payableService);
        bool isValid(PaymentVoucherDetail paymentVoucherDetail);
        string PrintError(PaymentVoucherDetail paymentVoucherDetail);
    }
}
