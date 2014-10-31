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
    public class SalesDownPaymentAllocationValidator : ISalesDownPaymentAllocationValidator
    {
        public SalesDownPaymentAllocation VHasPayable(SalesDownPaymentAllocation salesDownPaymentAllocation, IPayableService _payableService)
        {
            Payable payable = _payableService.GetObjectById(salesDownPaymentAllocation.PayableId);
            if (payable == null)
            {
                salesDownPaymentAllocation.Errors.Add("PayableId", "Tidak boleh tidak ada");
            }
            return salesDownPaymentAllocation;
        }

        public SalesDownPaymentAllocation VHasContact(SalesDownPaymentAllocation salesDownPaymentAllocation, IContactService _contactService)
        {
            Contact contact = _contactService.GetObjectById(salesDownPaymentAllocation.ContactId);
            if (contact == null)
            {
                salesDownPaymentAllocation.Errors.Add("ContactId", "Tidak boleh tidak ada");
            }
            return salesDownPaymentAllocation;
        }

        public SalesDownPaymentAllocation VHasSalesDownPayment(SalesDownPaymentAllocation salesDownPaymentAllocation, ISalesDownPaymentService _salesDownPaymentService)
        {
            SalesDownPayment salesDownPayment = _salesDownPaymentService.GetObjectById(salesDownPaymentAllocation.SalesDownPaymentId);
            if (salesDownPayment == null)
            {
                salesDownPaymentAllocation.Errors.Add("SalesDownPaymentId", "Tidak boleh tidak ada");
            }
            return salesDownPaymentAllocation;
        }

        public SalesDownPaymentAllocation VHasAllocationDate(SalesDownPaymentAllocation salesDownPaymentAllocation)
        {
            if (salesDownPaymentAllocation.AllocationDate == null)
            {
                salesDownPaymentAllocation.Errors.Add("AllocationDate", "Tidak boleh kosong");
            }
            return salesDownPaymentAllocation;
        }

        public SalesDownPaymentAllocation VHasSalesDownPaymentAllocationDetails(SalesDownPaymentAllocation salesDownPaymentAllocation, ISalesDownPaymentAllocationDetailService _salesDownPaymentAllocationDetailService)
        {
            IList<SalesDownPaymentAllocationDetail> details = _salesDownPaymentAllocationDetailService.GetObjectsBySalesDownPaymentAllocationId(salesDownPaymentAllocation.Id);
            if (!details.Any())
            {
                salesDownPaymentAllocation.Errors.Add("Generic", "Harus memiliki payment voucher details");
            }
            return salesDownPaymentAllocation;
        }

        public SalesDownPaymentAllocation VHasNoSalesDownPaymentAllocationDetail(SalesDownPaymentAllocation salesDownPaymentAllocation, ISalesDownPaymentAllocationDetailService _salesDownPaymentAllocationDetailService)
        {
            IList<SalesDownPaymentAllocationDetail> details = _salesDownPaymentAllocationDetailService.GetObjectsBySalesDownPaymentAllocationId(salesDownPaymentAllocation.Id);
            if (details.Any())
            {
                salesDownPaymentAllocation.Errors.Add("Generic", "Tidak boleh ada payment voucher details");
            }
            return salesDownPaymentAllocation;
        }

        public SalesDownPaymentAllocation VTotalAmountNotNegativeNorZero(SalesDownPaymentAllocation salesDownPaymentAllocation)
        {
            if (salesDownPaymentAllocation.TotalAmount <= 0)
            {
                salesDownPaymentAllocation.Errors.Add("Generic", "Total Amount tidak boleh <= 0");
            }
            return salesDownPaymentAllocation;
        }

        public SalesDownPaymentAllocation VHasNotBeenDeleted(SalesDownPaymentAllocation salesDownPaymentAllocation)
        {
            if (salesDownPaymentAllocation.IsDeleted)
            {
                salesDownPaymentAllocation.Errors.Add("Generic", "Tidak boleh sudah di deleted");
            }
            return salesDownPaymentAllocation;
        }
        
        public SalesDownPaymentAllocation VHasNotBeenConfirmed(SalesDownPaymentAllocation salesDownPaymentAllocation)
        {
            if (salesDownPaymentAllocation.IsConfirmed)
            {
                salesDownPaymentAllocation.Errors.Add("Generic", "Tidak boleh sudah dikonfirmasi");
            }
            return salesDownPaymentAllocation;
        }

        public SalesDownPaymentAllocation VHasBeenConfirmed(SalesDownPaymentAllocation salesDownPaymentAllocation)
        {
            if (!salesDownPaymentAllocation.IsConfirmed)
            {
                salesDownPaymentAllocation.Errors.Add("Generic", "Harus sudah dikonfirmasi");
            }
            return salesDownPaymentAllocation;
        }

        public SalesDownPaymentAllocation VTotalAmountEqualDetailsAmount(SalesDownPaymentAllocation salesDownPaymentAllocation, ISalesDownPaymentAllocationDetailService _salesDownPaymentAllocationDetailService)
        {
            IList<SalesDownPaymentAllocationDetail> details = _salesDownPaymentAllocationDetailService.GetObjectsBySalesDownPaymentAllocationId(salesDownPaymentAllocation.Id);
            decimal detailsamount = 0;
            foreach (var detail in details)
            {
                detailsamount += detail.Amount;
            }
            if (detailsamount != salesDownPaymentAllocation.TotalAmount)
            {
                salesDownPaymentAllocation.Errors.Add("Generic", "Jumlah amount di details harus sama dengan totalamount");
            }
            return salesDownPaymentAllocation;
        }

        public SalesDownPaymentAllocation VAllSalesDownPaymentAllocationDetailsAreConfirmable(SalesDownPaymentAllocation salesDownPaymentAllocation,
                                          ISalesDownPaymentAllocationDetailService _salesDownPaymentAllocationDetailService, IReceivableService _receivableService, IPayableService _payableService)
        {
            IList<SalesDownPaymentAllocationDetail> details = _salesDownPaymentAllocationDetailService.GetObjectsBySalesDownPaymentAllocationId(salesDownPaymentAllocation.Id);
            foreach (var detail in details)
            {
                detail.Errors = new Dictionary<string, string>();
                detail.ConfirmationDate = salesDownPaymentAllocation.ConfirmationDate;
                detail.Errors = new Dictionary<string, string>();
                if (!_salesDownPaymentAllocationDetailService.GetValidator().ValidConfirmObject(detail, _receivableService, _payableService))
                {
                    foreach (var error in detail.Errors)
                    {
                        salesDownPaymentAllocation.Errors.Add(error.Key, error.Value);
                    }
                    if (salesDownPaymentAllocation.Errors.Any()) { return salesDownPaymentAllocation; }
                }
            }
            return salesDownPaymentAllocation;
        }

        public SalesDownPaymentAllocation VGeneralLedgerPostingHasNotBeenClosed(SalesDownPaymentAllocation salesDownPaymentAllocation, IClosingService _closingService, int CaseConfirmUnconfirmReconcileUnreconcile)
        {
            switch(CaseConfirmUnconfirmReconcileUnreconcile)
            {
                case(1): // Confirm
                {
                    if (_closingService.IsDateClosed(salesDownPaymentAllocation.ConfirmationDate.GetValueOrDefault()))
                    {
                        salesDownPaymentAllocation.Errors.Add("Generic", "Ledger sudah tutup buku");
                    }
                    break;
                }
                case(2): // Unconfirm
                {
                    if (_closingService.IsDateClosed(DateTime.Now))
                    {
                        salesDownPaymentAllocation.Errors.Add("Generic", "Ledger sudah tutup buku");
                    }
                    break;
                }
            }
            return salesDownPaymentAllocation;
        }

        public SalesDownPaymentAllocation VCreateObject(SalesDownPaymentAllocation salesDownPaymentAllocation, ISalesDownPaymentAllocationService _salesDownPaymentAllocationService,
                                                        ISalesDownPaymentAllocationDetailService _salesDownPaymentAllocationDetailService, ISalesDownPaymentService _salesDownPaymentService,
                                                        IContactService _contactService, IPayableService _payableService)
        {
            if (!isValid(salesDownPaymentAllocation)) { return salesDownPaymentAllocation; }
            VHasContact(salesDownPaymentAllocation, _contactService);
            if (!isValid(salesDownPaymentAllocation)) { return salesDownPaymentAllocation; }
            VHasSalesDownPayment(salesDownPaymentAllocation, _salesDownPaymentService);
            if (!isValid(salesDownPaymentAllocation)) { return salesDownPaymentAllocation; }
            VHasAllocationDate(salesDownPaymentAllocation);
            return salesDownPaymentAllocation;
        }

        public SalesDownPaymentAllocation VUpdateObject(SalesDownPaymentAllocation salesDownPaymentAllocation, ISalesDownPaymentAllocationService _salesDownPaymentAllocationService,
                                          ISalesDownPaymentAllocationDetailService _salesDownPaymentAllocationDetailService, ISalesDownPaymentService _salesDownPaymentService,
                                          IContactService _contactService, IPayableService _payableService)
        {
            VHasPayable(salesDownPaymentAllocation, _payableService);
            if (!isValid(salesDownPaymentAllocation)) { return salesDownPaymentAllocation; }
            VHasNotBeenConfirmed(salesDownPaymentAllocation);
            if (!isValid(salesDownPaymentAllocation)) { return salesDownPaymentAllocation; }
            VHasNoSalesDownPaymentAllocationDetail(salesDownPaymentAllocation, _salesDownPaymentAllocationDetailService);
            if (!isValid(salesDownPaymentAllocation)) { return salesDownPaymentAllocation; }
            VCreateObject(salesDownPaymentAllocation, _salesDownPaymentAllocationService, _salesDownPaymentAllocationDetailService, _salesDownPaymentService, _contactService, _payableService);
            return salesDownPaymentAllocation;
        }

        public SalesDownPaymentAllocation VDeleteObject(SalesDownPaymentAllocation salesDownPaymentAllocation, ISalesDownPaymentAllocationDetailService _salesDownPaymentAllocationDetailService)
        {
            VHasNotBeenDeleted(salesDownPaymentAllocation);
            if (!isValid(salesDownPaymentAllocation)) { return salesDownPaymentAllocation; }
            VHasNoSalesDownPaymentAllocationDetail(salesDownPaymentAllocation, _salesDownPaymentAllocationDetailService);
            if (!isValid(salesDownPaymentAllocation)) { return salesDownPaymentAllocation; }
            VHasNotBeenConfirmed(salesDownPaymentAllocation);
            return salesDownPaymentAllocation;
        }

        public SalesDownPaymentAllocation VHasConfirmationDate(SalesDownPaymentAllocation obj)
        {
            if (obj.ConfirmationDate == null)
            {
                obj.Errors.Add("ConfirmationDate", "Tidak boleh kosong");
            }
            return obj;
        }

        public SalesDownPaymentAllocation VConfirmObject(SalesDownPaymentAllocation salesDownPaymentAllocation,
                                          ISalesDownPaymentAllocationDetailService _salesDownPaymentAllocationDetailService,
                                          ISalesDownPaymentService _salesDownPaymentService, IReceivableService _receivableService, IPayableService _payableService, 
                                          IAccountService _accountService, IGeneralLedgerJournalService _generalLedgerJournalService, IClosingService _closingService)
        {
            VHasConfirmationDate(salesDownPaymentAllocation);
            if (!isValid(salesDownPaymentAllocation)) { return salesDownPaymentAllocation; }
            VHasSalesDownPaymentAllocationDetails(salesDownPaymentAllocation, _salesDownPaymentAllocationDetailService);
            if (!isValid(salesDownPaymentAllocation)) { return salesDownPaymentAllocation; }
            VTotalAmountNotNegativeNorZero(salesDownPaymentAllocation);
            if (!isValid(salesDownPaymentAllocation)) { return salesDownPaymentAllocation; }
            VHasNotBeenConfirmed(salesDownPaymentAllocation);
            if (!isValid(salesDownPaymentAllocation)) { return salesDownPaymentAllocation; }
            VHasNotBeenDeleted(salesDownPaymentAllocation);
            if (!isValid(salesDownPaymentAllocation)) { return salesDownPaymentAllocation; }
            VTotalAmountEqualDetailsAmount(salesDownPaymentAllocation, _salesDownPaymentAllocationDetailService);
            if (!isValid(salesDownPaymentAllocation)) { return salesDownPaymentAllocation; }
            VAllSalesDownPaymentAllocationDetailsAreConfirmable(salesDownPaymentAllocation, _salesDownPaymentAllocationDetailService, 
                                                                _receivableService, _payableService);
            if (!isValid(salesDownPaymentAllocation)) { return salesDownPaymentAllocation; }
            VGeneralLedgerPostingHasNotBeenClosed(salesDownPaymentAllocation, _closingService, 1);
            return salesDownPaymentAllocation;
        }

        public SalesDownPaymentAllocation VUnconfirmObject(SalesDownPaymentAllocation salesDownPaymentAllocation, ISalesDownPaymentAllocationDetailService _salesDownPaymentAllocationDetailService,
                                          IReceivableService _receivableService, IPayableService _payableService, IAccountService _accountService,
                                          IGeneralLedgerJournalService _generalLedgerJournalService, IClosingService _closingService)
        {
            VHasBeenConfirmed(salesDownPaymentAllocation);
            if (!isValid(salesDownPaymentAllocation)) { return salesDownPaymentAllocation; }
            VGeneralLedgerPostingHasNotBeenClosed(salesDownPaymentAllocation, _closingService, 2);
            return salesDownPaymentAllocation;
        }

        public bool ValidCreateObject(SalesDownPaymentAllocation salesDownPaymentAllocation, ISalesDownPaymentAllocationService _salesDownPaymentAllocationService,
                                      ISalesDownPaymentAllocationDetailService _salesDownPaymentAllocationDetailService, ISalesDownPaymentService _salesDownPaymentService,
                                      IContactService _contactService, IPayableService _payableService)
        {
            VCreateObject(salesDownPaymentAllocation, _salesDownPaymentAllocationService, _salesDownPaymentAllocationDetailService, _salesDownPaymentService, _contactService,
                          _payableService);
            return isValid(salesDownPaymentAllocation);
        }

        public bool ValidUpdateObject(SalesDownPaymentAllocation salesDownPaymentAllocation, ISalesDownPaymentAllocationService _salesDownPaymentAllocationService,
                                      ISalesDownPaymentAllocationDetailService _salesDownPaymentAllocationDetailService, ISalesDownPaymentService _salesDownPaymentService,
                                      IContactService _contactService, IPayableService _payableService)
        {
            salesDownPaymentAllocation.Errors.Clear();
            VUpdateObject(salesDownPaymentAllocation, _salesDownPaymentAllocationService, _salesDownPaymentAllocationDetailService, _salesDownPaymentService, _contactService,
                          _payableService);
            return isValid(salesDownPaymentAllocation);
        }

        public bool ValidDeleteObject(SalesDownPaymentAllocation salesDownPaymentAllocation, ISalesDownPaymentAllocationDetailService _salesDownPaymentAllocationDetailService)
        {
            salesDownPaymentAllocation.Errors.Clear();
            VDeleteObject(salesDownPaymentAllocation, _salesDownPaymentAllocationDetailService);
            return isValid(salesDownPaymentAllocation);
        }

        public bool ValidConfirmObject(SalesDownPaymentAllocation salesDownPaymentAllocation, ISalesDownPaymentAllocationDetailService _salesDownPaymentAllocationDetailService,
                    ISalesDownPaymentService _salesDownPaymentService, IReceivableService _receivableService, IPayableService _payableService,
                    IAccountService _accountService, IGeneralLedgerJournalService _generalLedgerJournalService, IClosingService _closingService)
        {
            salesDownPaymentAllocation.Errors.Clear();
            VConfirmObject(salesDownPaymentAllocation, _salesDownPaymentAllocationDetailService, _salesDownPaymentService,
                           _receivableService, _payableService, _accountService, _generalLedgerJournalService, _closingService);
            return isValid(salesDownPaymentAllocation);
        }

        public bool ValidUnconfirmObject(SalesDownPaymentAllocation salesDownPaymentAllocation, ISalesDownPaymentAllocationDetailService _salesDownPaymentAllocationDetailService,
                    IReceivableService _receivableService, IPayableService _payableService, IAccountService _accountService,
                    IGeneralLedgerJournalService _generalLedgerJournalService, IClosingService _closingService)
        {
            salesDownPaymentAllocation.Errors.Clear();
            VUnconfirmObject(salesDownPaymentAllocation, _salesDownPaymentAllocationDetailService, _receivableService, _payableService,
                             _accountService, _generalLedgerJournalService, _closingService);
            return isValid(salesDownPaymentAllocation);
        }

        public bool isValid(SalesDownPaymentAllocation obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public string PrintError(SalesDownPaymentAllocation obj)
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