﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.DomainModel;
using Core.Interface.Service;

namespace Core.Interface.Validation
{
    public interface IPaymentVoucherValidator
    {
        PaymentVoucher VHasContact(PaymentVoucher paymentVoucher, IContactService _contactService);
        PaymentVoucher VHasCashBank(PaymentVoucher paymentVoucher, ICashBankService _cashBankService);
        PaymentVoucher VHasPaymentDate(PaymentVoucher paymentVoucher);
        PaymentVoucher VNotIsGBCH(PaymentVoucher paymentVoucher);
        PaymentVoucher VIfGBCHThenIsBank(PaymentVoucher paymentVoucher, ICashBankService _cashBankService);
        PaymentVoucher VIfGBCHThenHasDueDate(PaymentVoucher paymentVoucher);
        PaymentVoucher VHasNoPaymentVoucherDetail(PaymentVoucher paymentVoucher, IPaymentVoucherDetailService _paymentVoucherDetailService);
        PaymentVoucher VHasPaymentVoucherDetails(PaymentVoucher paymentVoucher, IPaymentVoucherDetailService _paymentVoucherDetailService);
        PaymentVoucher VTotalAmountIsNotZero(PaymentVoucher paymentVoucher, IPaymentVoucherDetailService _paymentVoucherDetailService);
        PaymentVoucher VHasNotBeenDeleted(PaymentVoucher paymentVoucher);
        PaymentVoucher VHasBeenConfirmed(PaymentVoucher paymentVoucher);
        PaymentVoucher VHasNotBeenConfirmed(PaymentVoucher paymentVoucher);
        PaymentVoucher VAllPaymentVoucherDetailsAreConfirmable(PaymentVoucher paymentVoucher, IPaymentVoucherService _paymetnVoucherService,
                                                               IPaymentVoucherDetailService paymentVoucherDetailService, ICashBankService _cashBankService,
                                                               IPayableService _payableService);
        PaymentVoucher VCashBankIsGreaterThanOrEqualPaymentVoucherDetails(PaymentVoucher paymentVoucher, IPaymentVoucherDetailService _paymentVoucherDetailService,
                                                                   ICashBankService _cashBankService, bool CasePayment);
        PaymentVoucher VHasBeenReconciled(PaymentVoucher paymentVoucher);
        PaymentVoucher VHasNotBeenReconciled(PaymentVoucher paymentVoucher);
        PaymentVoucher VHasReconciliationDate(PaymentVoucher paymentVoucher);
        PaymentVoucher VGeneralLedgerPostingHasNotBeenClosed(PaymentVoucher paymentVoucher, IClosingService _closingService, int CaseConfirmUnconfirm);
        PaymentVoucher VCreateObject(PaymentVoucher paymentVoucher, IPaymentVoucherService _paymentVoucherService, IPaymentVoucherDetailService _paymentVoucherDetailService,
                                     IPayableService _payableService, IContactService _contactService, ICashBankService _cashBankService);
        PaymentVoucher VUpdateObject(PaymentVoucher paymentVoucher, IPaymentVoucherService _paymentVoucherService, IPaymentVoucherDetailService _paymentVoucherDetailService,
                                     IPayableService _payableService, IContactService _contactService, ICashBankService _cashBankService);
        PaymentVoucher VDeleteObject(PaymentVoucher paymentVoucher, IPaymentVoucherDetailService _paymentVoucherDetailService);
        PaymentVoucher VHasConfirmationDate(PaymentVoucher paymentVoucher);
        PaymentVoucher VConfirmObject(PaymentVoucher paymentVoucher, IPaymentVoucherService _paymentVoucherService,
                                       IPaymentVoucherDetailService _paymentVoucherDetailService, ICashBankService _cashBankService,
                                       IPayableService _payableService, IClosingService _closingService);
        PaymentVoucher VUnconfirmObject(PaymentVoucher paymentVoucher, IClosingService _closingService);
        PaymentVoucher VReconcileObject(PaymentVoucher paymentVoucher, IPaymentVoucherDetailService _paymentVoucherDetailService, ICashBankService _cashBankService, IClosingService _closingService);
        PaymentVoucher VUnreconcileObject(PaymentVoucher paymentVoucher, IClosingService _closingService);
        bool ValidCreateObject(PaymentVoucher paymentVoucher, IPaymentVoucherService _paymentVoucherService, IPaymentVoucherDetailService _paymentVoucherDetailService,
                               IPayableService _payableService, IContactService _contactService, ICashBankService _cashBankService);
        bool ValidUpdateObject(PaymentVoucher paymentVoucher, IPaymentVoucherService _paymentVoucherService, IPaymentVoucherDetailService _paymentVoucherDetailService,
                               IPayableService _payableService, IContactService _contactService, ICashBankService _cashBankService);
        bool ValidDeleteObject(PaymentVoucher paymentVoucher, IPaymentVoucherDetailService _paymentVoucherDetailService);
        bool ValidConfirmObject(PaymentVoucher paymentVoucher, IPaymentVoucherService _paymentVoucherService,
                                IPaymentVoucherDetailService _paymentVoucherDetailService, ICashBankService _cashBankService,
                                IPayableService _payableService, IClosingService _closingService);
        bool ValidUnconfirmObject(PaymentVoucher paymentVoucher, IClosingService _closingService);
        bool ValidReconcileObject(PaymentVoucher paymentVoucher, IPaymentVoucherDetailService _paymentVoucherDetailService, ICashBankService _cashBankService, IClosingService _closingService);
        bool ValidUnreconcileObject(PaymentVoucher paymentVoucher, IClosingService _closingService);
        bool isValid(PaymentVoucher paymentVoucher);
        string PrintError(PaymentVoucher paymentVoucher);
    }
}
