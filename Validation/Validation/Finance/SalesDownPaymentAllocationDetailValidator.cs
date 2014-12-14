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
    public class SalesDownPaymentAllocationDetailValidator : ISalesDownPaymentAllocationDetailValidator
    {
        public SalesDownPaymentAllocationDetail VHasSalesDownPaymentAllocation(SalesDownPaymentAllocationDetail salesDownPaymentAllocationDetail, ISalesDownPaymentAllocationService _salesDownPaymentAllocationService)
        {
            SalesDownPaymentAllocation salesDownPaymentAllocation = _salesDownPaymentAllocationService.GetObjectById(salesDownPaymentAllocationDetail.SalesDownPaymentAllocationId);
            if (salesDownPaymentAllocation == null)
            {
                salesDownPaymentAllocationDetail.Errors.Add("SalesDownPaymentAllocationId", "Tidak boleh tidak ada");
            }
            return salesDownPaymentAllocationDetail;
        }

        public SalesDownPaymentAllocationDetail VHasReceivable(SalesDownPaymentAllocationDetail salesDownPaymentAllocationDetail, IReceivableService _receivableService)
        {
            Receivable receivable = _receivableService.GetObjectById(salesDownPaymentAllocationDetail.ReceivableId);
            if (receivable == null)
            {
                salesDownPaymentAllocationDetail.Errors.Add("ReceivableId", "Tidak boleh tidak ada");
            }
            return salesDownPaymentAllocationDetail;
        }

        public SalesDownPaymentAllocationDetail VHasNotBeenConfirmed(SalesDownPaymentAllocationDetail salesDownPaymentAllocationDetail)
        {
            if (salesDownPaymentAllocationDetail.IsConfirmed)
            {
                salesDownPaymentAllocationDetail.Errors.Add("Generic", "Sudah dikonfirmasi");
            }
            return salesDownPaymentAllocationDetail;
        }

        public SalesDownPaymentAllocationDetail VHasBeenConfirmed(SalesDownPaymentAllocationDetail salesDownPaymentAllocationDetail)
        {
            if (!salesDownPaymentAllocationDetail.IsConfirmed)
            {
                salesDownPaymentAllocationDetail.Errors.Add("Generic", "Belum dikonfirmasi");
            }
            return salesDownPaymentAllocationDetail;
        }

        public SalesDownPaymentAllocationDetail VHasNotBeenDeleted(SalesDownPaymentAllocationDetail salesDownPaymentAllocationDetail)
        {
            if (salesDownPaymentAllocationDetail.IsDeleted)
            {
                salesDownPaymentAllocationDetail.Errors.Add("Generic", "Sudah didelete");
            }
            return salesDownPaymentAllocationDetail;
        }

        public SalesDownPaymentAllocationDetail VReceivableHasNotBeenCompleted(SalesDownPaymentAllocationDetail salesDownPaymentAllocationDetail, IReceivableService _receivableService)
        {
            Receivable receivable = _receivableService.GetObjectById(salesDownPaymentAllocationDetail.ReceivableId);
            if (receivable.IsCompleted)
            {
                salesDownPaymentAllocationDetail.Errors.Add("Generic", "Receivable sudah complete");
            }
            return salesDownPaymentAllocationDetail;
        }
        
        public SalesDownPaymentAllocationDetail VNonNegativeAmount(SalesDownPaymentAllocationDetail salesDownPaymentAllocationDetail)
        {
            if (salesDownPaymentAllocationDetail.Amount < 0)
            {
                salesDownPaymentAllocationDetail.Errors.Add("Amount", "Tidak boleh kurang dari 0");
            }
            return salesDownPaymentAllocationDetail;
        }

        public SalesDownPaymentAllocationDetail VAmountLessOrEqualReceivable(SalesDownPaymentAllocationDetail salesDownPaymentAllocationDetail, IReceivableService _receivableService)
        {
            Receivable receivable = _receivableService.GetObjectById(salesDownPaymentAllocationDetail.ReceivableId);
            if (salesDownPaymentAllocationDetail.Amount > receivable.Amount)
            {
                salesDownPaymentAllocationDetail.Errors.Add("Amount", "Tidak boleh lebih dari receivable");
            }
            return salesDownPaymentAllocationDetail;
        }

        public SalesDownPaymentAllocationDetail VUniqueReceivableId(SalesDownPaymentAllocationDetail salesDownPaymentAllocationDetail, ISalesDownPaymentAllocationDetailService _salesDownPaymentAllocationDetailService,
                                                   IReceivableService _receivableService)
        {
            IList<SalesDownPaymentAllocationDetail> salesDownPaymentAllocationDetails = _salesDownPaymentAllocationDetailService.GetObjectsBySalesDownPaymentAllocationId(salesDownPaymentAllocationDetail.SalesDownPaymentAllocationId);
            Receivable receivable = _receivableService.GetObjectById(salesDownPaymentAllocationDetail.ReceivableId);
            foreach (var detail in salesDownPaymentAllocationDetails)
            {
                if (detail.ReceivableId == salesDownPaymentAllocationDetail.ReceivableId && detail.Id != salesDownPaymentAllocationDetail.Id)
                {
                    salesDownPaymentAllocationDetail.Errors.Add("Generic", "ReceivableId harus unique dibandingkan payment voucher details di dalam satu payment voucher");
                    return salesDownPaymentAllocationDetail;
                }
            }
            return salesDownPaymentAllocationDetail;
        }

        public SalesDownPaymentAllocationDetail VDetailsAmountLessOrEqualSalesDownPaymentTotal(SalesDownPaymentAllocationDetail salesDownPaymentAllocationDetail, ISalesDownPaymentAllocationService _salesDownPaymentAllocationService,
                                                                                 ISalesDownPaymentAllocationDetailService _salesDownPaymentAllocationDetailService)
        {
            IList<SalesDownPaymentAllocationDetail> salesDownPaymentAllocationDetails = _salesDownPaymentAllocationDetailService.GetObjectsBySalesDownPaymentAllocationId(salesDownPaymentAllocationDetail.SalesDownPaymentAllocationId);
            decimal TotalSalesDownPaymentAllocationDetails = 0;
            foreach (var detail in salesDownPaymentAllocationDetails)
            {
                TotalSalesDownPaymentAllocationDetails += detail.AmountPaid;
            }
            SalesDownPaymentAllocation salesDownPaymentAllocation = _salesDownPaymentAllocationService.GetObjectById(salesDownPaymentAllocationDetail.SalesDownPaymentAllocationId);
            if (salesDownPaymentAllocation.Payable.RemainingAmount < TotalSalesDownPaymentAllocationDetails)
            {
                decimal sisa = salesDownPaymentAllocation.TotalAmount - TotalSalesDownPaymentAllocationDetails + salesDownPaymentAllocationDetail.AmountPaid;
                salesDownPaymentAllocationDetail.Errors.Add("Generic", "Down Payment Voucher hanya menyediakan sisa dana sebesar " + sisa);
            }
            return salesDownPaymentAllocationDetail;
        }

        public SalesDownPaymentAllocationDetail VCreateObject(SalesDownPaymentAllocationDetail salesDownPaymentAllocationDetail, ISalesDownPaymentAllocationService _salesDownPaymentAllocationService,
                                                   ISalesDownPaymentAllocationDetailService _salesDownPaymentAllocationDetailService, ISalesDownPaymentService _salesDownPaymentService,
                                                   IReceivableService _receivableService, IPayableService _payableService)
        {
            VHasSalesDownPaymentAllocation(salesDownPaymentAllocationDetail, _salesDownPaymentAllocationService);
            if (!isValid(salesDownPaymentAllocationDetail)) { return salesDownPaymentAllocationDetail; }
            VHasNotBeenConfirmed(salesDownPaymentAllocationDetail);
            if (!isValid(salesDownPaymentAllocationDetail)) { return salesDownPaymentAllocationDetail; }
            VHasNotBeenDeleted(salesDownPaymentAllocationDetail);
            if (!isValid(salesDownPaymentAllocationDetail)) { return salesDownPaymentAllocationDetail; }
            VHasReceivable(salesDownPaymentAllocationDetail, _receivableService);
            if (!isValid(salesDownPaymentAllocationDetail)) { return salesDownPaymentAllocationDetail; }
            VReceivableHasNotBeenCompleted(salesDownPaymentAllocationDetail, _receivableService);
            if (!isValid(salesDownPaymentAllocationDetail)) { return salesDownPaymentAllocationDetail; }
            VAmountLessOrEqualReceivable(salesDownPaymentAllocationDetail, _receivableService);
            if (!isValid(salesDownPaymentAllocationDetail)) { return salesDownPaymentAllocationDetail; }
            VUniqueReceivableId(salesDownPaymentAllocationDetail, _salesDownPaymentAllocationDetailService, _receivableService);
            if (!isValid(salesDownPaymentAllocationDetail)) { return salesDownPaymentAllocationDetail; }
            VDetailsAmountLessOrEqualSalesDownPaymentTotal(salesDownPaymentAllocationDetail, _salesDownPaymentAllocationService, _salesDownPaymentAllocationDetailService);
            return salesDownPaymentAllocationDetail;
        }

        public SalesDownPaymentAllocationDetail VUpdateObject(SalesDownPaymentAllocationDetail salesDownPaymentAllocationDetail, ISalesDownPaymentAllocationService _salesDownPaymentAllocationService,
                                                   ISalesDownPaymentAllocationDetailService _salesDownPaymentAllocationDetailService, ISalesDownPaymentService _salesDownPaymentService,
                                                   IReceivableService _receivableService, IPayableService _payableService)
        {
            VHasNotBeenConfirmed(salesDownPaymentAllocationDetail);
            if (!isValid(salesDownPaymentAllocationDetail)) { return salesDownPaymentAllocationDetail; }
            VCreateObject(salesDownPaymentAllocationDetail, _salesDownPaymentAllocationService, _salesDownPaymentAllocationDetailService, _salesDownPaymentService, _receivableService, _payableService);
            return salesDownPaymentAllocationDetail;    
        }

        public SalesDownPaymentAllocationDetail VDeleteObject(SalesDownPaymentAllocationDetail salesDownPaymentAllocationDetail)
        {
            VHasNotBeenConfirmed(salesDownPaymentAllocationDetail);
            if (!isValid(salesDownPaymentAllocationDetail)) { return salesDownPaymentAllocationDetail; }
            VHasNotBeenDeleted(salesDownPaymentAllocationDetail);
            return salesDownPaymentAllocationDetail;
        }

        public SalesDownPaymentAllocationDetail VHasConfirmationDate(SalesDownPaymentAllocationDetail obj)
        {
            if (obj.ConfirmationDate == null)
            {
                obj.Errors.Add("ConfirmationDate", "Tidak boleh kosong");
            }
            return obj;
        }

        public SalesDownPaymentAllocationDetail VConfirmObject(SalesDownPaymentAllocationDetail salesDownPaymentAllocationDetail, IReceivableService _receivableService, IPayableService _payableService)
        {
            VHasConfirmationDate(salesDownPaymentAllocationDetail);
            if (!isValid(salesDownPaymentAllocationDetail)) { return salesDownPaymentAllocationDetail; }
            VAmountLessOrEqualReceivable(salesDownPaymentAllocationDetail, _receivableService);
            return salesDownPaymentAllocationDetail;
        }

        public SalesDownPaymentAllocationDetail VUnconfirmObject(SalesDownPaymentAllocationDetail salesDownPaymentAllocationDetail,
                                                                 IReceivableService _receivableService, IPayableService _payableService)
        {
            VHasBeenConfirmed(salesDownPaymentAllocationDetail);
            return salesDownPaymentAllocationDetail;
        }

        public bool ValidCreateObject(SalesDownPaymentAllocationDetail salesDownPaymentAllocationDetail, ISalesDownPaymentAllocationService _salesDownPaymentAllocationService,
                                      ISalesDownPaymentAllocationDetailService _salesDownPaymentAllocationDetailService, ISalesDownPaymentService _salesDownPaymentService,
                                      IReceivableService _receivableService, IPayableService _payableService)
        {
            VCreateObject(salesDownPaymentAllocationDetail, _salesDownPaymentAllocationService, _salesDownPaymentAllocationDetailService, _salesDownPaymentService,
                          _receivableService, _payableService);
            return isValid(salesDownPaymentAllocationDetail);
        }

        public bool ValidUpdateObject(SalesDownPaymentAllocationDetail salesDownPaymentAllocationDetail, ISalesDownPaymentAllocationService _salesDownPaymentAllocationService,
                                      ISalesDownPaymentAllocationDetailService _salesDownPaymentAllocationDetailService, ISalesDownPaymentService _salesDownPaymentService,
                                      IReceivableService _receivableService, IPayableService _payableService)
        {
            VUpdateObject(salesDownPaymentAllocationDetail, _salesDownPaymentAllocationService, _salesDownPaymentAllocationDetailService, _salesDownPaymentService,
                          _receivableService, _payableService);
            return isValid(salesDownPaymentAllocationDetail);
        }

        public bool ValidDeleteObject(SalesDownPaymentAllocationDetail salesDownPaymentAllocationDetail)
        {
            VDeleteObject(salesDownPaymentAllocationDetail);
            return isValid(salesDownPaymentAllocationDetail);
        }

        public bool ValidConfirmObject(SalesDownPaymentAllocationDetail salesDownPaymentAllocationDetail, IReceivableService _receivableService, IPayableService _payableService)
        {
            VConfirmObject(salesDownPaymentAllocationDetail, _receivableService, _payableService);
            return isValid(salesDownPaymentAllocationDetail);
        }

        public bool ValidUnconfirmObject(SalesDownPaymentAllocationDetail salesDownPaymentAllocationDetail, IReceivableService _receivableService, IPayableService _payableService)
        {
            VUnconfirmObject(salesDownPaymentAllocationDetail, _receivableService, _payableService);
            return isValid(salesDownPaymentAllocationDetail);
        }

        public bool isValid(SalesDownPaymentAllocationDetail obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public string PrintError(SalesDownPaymentAllocationDetail obj)
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