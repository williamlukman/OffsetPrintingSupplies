using Core.DomainModel;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface IReceiptVoucherService
    {
        IReceiptVoucherValidator GetValidator();
        IList<ReceiptVoucher> GetAll();
        ReceiptVoucher GetObjectById(int Id);
        IList<ReceiptVoucher> GetObjectsByCashBankId(int cashBankId);
        IList<ReceiptVoucher> GetObjectsByCustomerId(int customerId);
        ReceiptVoucher CreateObject(ReceiptVoucher receiptVoucher, IReceiptVoucherDetailService _receiptVoucherDetailService, IReceivableService _receivableService, ICustomerService _customerService, ICashBankService _cashBankService);
        ReceiptVoucher CreateObject(int cashBankId, int customerId, DateTime paymentDate, decimal totalAmount, bool IsGBCH, DateTime DueDate, bool IsBank,
                                    IReceiptVoucherDetailService _receiptVoucherDetailService, IReceivableService _receivableService,
                                    ICustomerService _customerService, ICashBankService _cashBankService);
        ReceiptVoucher UpdateAmount(ReceiptVoucher receiptVoucher);
        ReceiptVoucher UpdateObject(ReceiptVoucher receiptVoucher, IReceiptVoucherDetailService _receiptVoucherDetailService, IReceivableService _receivableService, ICustomerService _customerService, ICashBankService _cashBankService);
        ReceiptVoucher SoftDeleteObject(ReceiptVoucher receiptVoucher, IReceiptVoucherDetailService _receiptVoucherDetailService);
        bool DeleteObject(int Id);
        ReceiptVoucher ConfirmObject(ReceiptVoucher receiptVoucher, IReceiptVoucherDetailService _receiptVoucherDetailService, ICashMutationService _cashMutationService, 
                                     ICashBankService _cashBankService, IReceivableService _receivableService, ICustomerService _customerService);
        ReceiptVoucher UnconfirmObject(ReceiptVoucher receiptVoucher, IReceiptVoucherDetailService _receiptVoucherDetailService, ICashMutationService _cashMutationService, 
                                     ICashBankService _cashBankService, IReceivableService _receivableService, ICustomerService _customerService);
        ReceiptVoucher ReconcileObject(ReceiptVoucher receiptVoucher, IReceiptVoucherDetailService _receiptVoucherDetailService, ICashMutationService _cashMutationService, 
                                     ICashBankService _cashBankService, IReceivableService _receivableService, ICustomerService _customerService);
        ReceiptVoucher UnreconcileObject(ReceiptVoucher receiptVoucher, IReceiptVoucherDetailService _receiptVoucherDetailService, ICashMutationService _cashMutationService, 
                                     ICashBankService _cashBankService, IReceivableService _receivableService, ICustomerService _customerService);
    }
}