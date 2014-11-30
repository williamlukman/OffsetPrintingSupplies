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
    public class TemporaryDeliveryOrderClearanceValidator : ITemporaryDeliveryOrderClearanceValidator
    {
        public TemporaryDeliveryOrderClearance VHasUniqueCode(TemporaryDeliveryOrderClearance temporaryDeliveryOrderClearance, ITemporaryDeliveryOrderClearanceService _temporaryDeliveryOrderClearanceService)
        {
            IQueryable<TemporaryDeliveryOrderClearance> duplicates = _temporaryDeliveryOrderClearanceService.GetQueryable().Where(x => x.Code == temporaryDeliveryOrderClearance.Code && x.Id != temporaryDeliveryOrderClearance.Id && !x.IsDeleted);
            if (duplicates.Any())
            {
                temporaryDeliveryOrderClearance.Errors.Add("Code", "Tidak boleh merupakan duplikasi");
            }
            return temporaryDeliveryOrderClearance;
        }

        public TemporaryDeliveryOrderClearance VHasTemporaryDeliveryOrder(TemporaryDeliveryOrderClearance temporaryDeliveryOrderClearance, ITemporaryDeliveryOrderService _temporaryDeliveryOrderService)
        {
            TemporaryDeliveryOrder temporaryDeliveryOrder = _temporaryDeliveryOrderService.GetObjectById(temporaryDeliveryOrderClearance.TemporaryDeliveryOrderId.GetValueOrDefault());
            if (temporaryDeliveryOrder == null)
            {
                temporaryDeliveryOrderClearance.Errors.Add("PreviousOrderId", "Tidak terasosiasi dengan Temporary Delivery Order");
            }
            return temporaryDeliveryOrderClearance;
        }

        public TemporaryDeliveryOrderClearance VHasClearanceDate(TemporaryDeliveryOrderClearance temporaryDeliveryOrderClearance)
        {
            if (temporaryDeliveryOrderClearance.ClearanceDate == null)
            {
                temporaryDeliveryOrderClearance.Errors.Add("ClearanceDate", "Tidak boleh kosong");
            }
            return temporaryDeliveryOrderClearance;
        }

        public TemporaryDeliveryOrderClearance VHasBeenConfirmed(TemporaryDeliveryOrderClearance temporaryDeliveryOrderClearance)
        {
            if (!temporaryDeliveryOrderClearance.IsConfirmed)
            {
                temporaryDeliveryOrderClearance.Errors.Add("Generic", "Belum dikonfirmasi");
            }
            return temporaryDeliveryOrderClearance;
        }

        public TemporaryDeliveryOrderClearance VHasNotBeenConfirmed(TemporaryDeliveryOrderClearance temporaryDeliveryOrderClearance)
        {
            if (temporaryDeliveryOrderClearance.IsConfirmed)
            {
                temporaryDeliveryOrderClearance.Errors.Add("Generic", "Tidak boleh sudah dikonfirmasi");
            }
            return temporaryDeliveryOrderClearance;
        }

        public TemporaryDeliveryOrderClearance VHasTemporaryDeliveryOrderClearanceDetails(TemporaryDeliveryOrderClearance temporaryDeliveryOrderClearance, ITemporaryDeliveryOrderClearanceDetailService _temporaryDeliveryOrderClearanceDetailService)
        {
            IList<TemporaryDeliveryOrderClearanceDetail> details = _temporaryDeliveryOrderClearanceDetailService.GetObjectsByTemporaryDeliveryOrderClearanceId(temporaryDeliveryOrderClearance.Id);
            if (!details.Any())
            {
                temporaryDeliveryOrderClearance.Errors.Add("Generic", "Tidak memiliki details");
            }
            return temporaryDeliveryOrderClearance;
        }

        public TemporaryDeliveryOrderClearance VHasNoTemporaryDeliveryOrderClearanceDetail(TemporaryDeliveryOrderClearance temporaryDeliveryOrderClearance, ITemporaryDeliveryOrderClearanceDetailService _temporaryDeliveryOrderClearanceDetailService)
        {
            IList<TemporaryDeliveryOrderClearanceDetail> details = _temporaryDeliveryOrderClearanceDetailService.GetObjectsByTemporaryDeliveryOrderClearanceId(temporaryDeliveryOrderClearance.Id);
            if (details.Any())
            {
                temporaryDeliveryOrderClearance.Errors.Add("Generic", "Masih memiliki details");
            }
            return temporaryDeliveryOrderClearance;
        }

        public TemporaryDeliveryOrderClearance VTemporaryDeliveryOrderHasBeenConfirmed(TemporaryDeliveryOrderClearance temporaryDeliveryOrderClearance, ITemporaryDeliveryOrderService _temporaryDeliveryOrderService)
        {
            TemporaryDeliveryOrder temporaryDeliveryOrder = _temporaryDeliveryOrderService.GetObjectById(temporaryDeliveryOrderClearance.TemporaryDeliveryOrderId.GetValueOrDefault());
            if (!temporaryDeliveryOrder.IsConfirmed)
            {
                temporaryDeliveryOrderClearance.Errors.Add("Generic", "Temporary Delivery Order belum dikonfirmasi");
            }
            return temporaryDeliveryOrderClearance;
        }

        public TemporaryDeliveryOrderClearance VHasConfirmationDate(TemporaryDeliveryOrderClearance obj)
        {
            if (obj.ConfirmationDate == null)
            {
                obj.Errors.Add("ConfirmationDate", "Tidak boleh kosong");
            }
            return obj;
        }


        public TemporaryDeliveryOrderClearance VGeneralLedgerPostingHasNotBeenClosed(TemporaryDeliveryOrderClearance temporaryDeliveryOrderClearance, IClosingService _closingService, int CaseConfirmUnconfirm)
        {
            switch (CaseConfirmUnconfirm)
            {
                case (1): // Confirm
                    {
                        if (_closingService.IsDateClosed(temporaryDeliveryOrderClearance.ConfirmationDate.GetValueOrDefault()))
                        {
                            temporaryDeliveryOrderClearance.Errors.Add("Generic", "Ledger sudah tutup buku");
                        }
                        break;
                    }
                case (2): // Unconfirm
                    {
                        if (_closingService.IsDateClosed(DateTime.Now))
                        {
                            temporaryDeliveryOrderClearance.Errors.Add("Generic", "Ledger sudah tutup buku");
                        }
                        break;
                    }
            }
            return temporaryDeliveryOrderClearance;
        }

        public TemporaryDeliveryOrderClearance VAllQuantitiesGreaterOrEqualWasteAndRestock(TemporaryDeliveryOrderClearance temporaryDeliveryOrderClearance, ITemporaryDeliveryOrderClearanceDetailService _temporaryDeliveryOrderClearanceDetailService)
        {
            IList<TemporaryDeliveryOrderClearanceDetail> temporaryDeliveryOrderClearanceDetails = _temporaryDeliveryOrderClearanceDetailService.GetObjectsByTemporaryDeliveryOrderClearanceId(temporaryDeliveryOrderClearance.Id);
            foreach (var detail in temporaryDeliveryOrderClearanceDetails)
            {
                if (detail.TemporaryDeliveryOrderDetail.WasteQuantity + detail.TemporaryDeliveryOrderDetail.RestockQuantity > detail.TemporaryDeliveryOrderDetail.Quantity)
                {
                    temporaryDeliveryOrderClearance.Errors.Add("Generic", "WasteQuantity + RestockQuantity harus kurang dari atau sama dengan Quantity");
                    return temporaryDeliveryOrderClearance;
                }
            }
            return temporaryDeliveryOrderClearance;
        }

        public TemporaryDeliveryOrderClearance VCreateObject(TemporaryDeliveryOrderClearance temporaryDeliveryOrderClearance, ITemporaryDeliveryOrderClearanceService _temporaryDeliveryOrderClearanceService, ITemporaryDeliveryOrderService _temporaryDeliveryOrderService)
        {
            VHasUniqueCode(temporaryDeliveryOrderClearance, _temporaryDeliveryOrderClearanceService);
            if (!isValid(temporaryDeliveryOrderClearance)) { return temporaryDeliveryOrderClearance; }
            VHasTemporaryDeliveryOrder(temporaryDeliveryOrderClearance, _temporaryDeliveryOrderService);
            if (!isValid(temporaryDeliveryOrderClearance)) { return temporaryDeliveryOrderClearance; }
            VTemporaryDeliveryOrderHasBeenConfirmed(temporaryDeliveryOrderClearance, _temporaryDeliveryOrderService);
            if (!isValid(temporaryDeliveryOrderClearance)) { return temporaryDeliveryOrderClearance; }
            VHasClearanceDate(temporaryDeliveryOrderClearance);
            return temporaryDeliveryOrderClearance;
        }

        public TemporaryDeliveryOrderClearance VUpdateObject(TemporaryDeliveryOrderClearance temporaryDeliveryOrderClearance, ITemporaryDeliveryOrderClearanceService _temporaryDeliveryOrderClearanceService, ITemporaryDeliveryOrderService _temporaryDeliveryOrderService)
        {
            VHasNotBeenConfirmed(temporaryDeliveryOrderClearance);
            if (!isValid(temporaryDeliveryOrderClearance)) { return temporaryDeliveryOrderClearance; }
            VCreateObject(temporaryDeliveryOrderClearance, _temporaryDeliveryOrderClearanceService, _temporaryDeliveryOrderService);
            return temporaryDeliveryOrderClearance;
        }

        public TemporaryDeliveryOrderClearance VDeleteObject(TemporaryDeliveryOrderClearance temporaryDeliveryOrderClearance, ITemporaryDeliveryOrderClearanceDetailService _temporaryDeliveryOrderClearanceDetailService)
        {
            VHasNotBeenConfirmed(temporaryDeliveryOrderClearance);
            if (!isValid(temporaryDeliveryOrderClearance)) { return temporaryDeliveryOrderClearance; }
            VHasNoTemporaryDeliveryOrderClearanceDetail(temporaryDeliveryOrderClearance, _temporaryDeliveryOrderClearanceDetailService);
            return temporaryDeliveryOrderClearance;
        }

        public TemporaryDeliveryOrderClearance VConfirmObject(TemporaryDeliveryOrderClearance temporaryDeliveryOrderClearance, ITemporaryDeliveryOrderClearanceDetailService _temporaryDeliveryOrderClearanceDetailService, IClosingService _closingService)
        {
            VHasConfirmationDate(temporaryDeliveryOrderClearance);
            if (!isValid(temporaryDeliveryOrderClearance)) { return temporaryDeliveryOrderClearance; }
            VHasNotBeenConfirmed(temporaryDeliveryOrderClearance);
            if (!isValid(temporaryDeliveryOrderClearance)) { return temporaryDeliveryOrderClearance; }
            VHasTemporaryDeliveryOrderClearanceDetails(temporaryDeliveryOrderClearance, _temporaryDeliveryOrderClearanceDetailService);
            if (!isValid(temporaryDeliveryOrderClearance)) { return temporaryDeliveryOrderClearance; }
            VGeneralLedgerPostingHasNotBeenClosed(temporaryDeliveryOrderClearance, _closingService, 1);
            return temporaryDeliveryOrderClearance;
        }

        public TemporaryDeliveryOrderClearance VUnconfirmObject(TemporaryDeliveryOrderClearance temporaryDeliveryOrderClearance, IClosingService _closingService)
        {
            VHasBeenConfirmed(temporaryDeliveryOrderClearance);
            if (!isValid(temporaryDeliveryOrderClearance)) { return temporaryDeliveryOrderClearance; }
            VGeneralLedgerPostingHasNotBeenClosed(temporaryDeliveryOrderClearance, _closingService, 2);
            return temporaryDeliveryOrderClearance;
        }

        public bool ValidCreateObject(TemporaryDeliveryOrderClearance temporaryDeliveryOrderClearance, ITemporaryDeliveryOrderClearanceService _temporaryDeliveryOrderClearanceService, ITemporaryDeliveryOrderService _temporaryDeliveryOrderService)
        {
            VCreateObject(temporaryDeliveryOrderClearance, _temporaryDeliveryOrderClearanceService, _temporaryDeliveryOrderService);
            return isValid(temporaryDeliveryOrderClearance);
        }

        public bool ValidUpdateObject(TemporaryDeliveryOrderClearance temporaryDeliveryOrderClearance, ITemporaryDeliveryOrderClearanceService _temporaryDeliveryOrderClearanceService, ITemporaryDeliveryOrderService _temporaryDeliveryOrderService)
        {
            temporaryDeliveryOrderClearance.Errors.Clear();
            VUpdateObject(temporaryDeliveryOrderClearance, _temporaryDeliveryOrderClearanceService, _temporaryDeliveryOrderService);
            return isValid(temporaryDeliveryOrderClearance);
        }

        public bool ValidDeleteObject(TemporaryDeliveryOrderClearance temporaryDeliveryOrderClearance, ITemporaryDeliveryOrderClearanceDetailService _temporaryDeliveryOrderClearanceDetailService)
        {
            temporaryDeliveryOrderClearance.Errors.Clear();
            VDeleteObject(temporaryDeliveryOrderClearance, _temporaryDeliveryOrderClearanceDetailService);
            return isValid(temporaryDeliveryOrderClearance);
        }

        public bool ValidConfirmObject(TemporaryDeliveryOrderClearance temporaryDeliveryOrderClearance, ITemporaryDeliveryOrderClearanceDetailService _temporaryDeliveryOrderClearanceDetailService, IClosingService _closingService)
        {
            temporaryDeliveryOrderClearance.Errors.Clear();
            VConfirmObject(temporaryDeliveryOrderClearance, _temporaryDeliveryOrderClearanceDetailService, _closingService);
            return isValid(temporaryDeliveryOrderClearance);
        }

        public bool ValidUnconfirmObject(TemporaryDeliveryOrderClearance temporaryDeliveryOrderClearance, IClosingService _closingService)
        {
            temporaryDeliveryOrderClearance.Errors.Clear();
            VUnconfirmObject(temporaryDeliveryOrderClearance, _closingService);
            return isValid(temporaryDeliveryOrderClearance);
        }

        public bool isValid(TemporaryDeliveryOrderClearance obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public string PrintError(TemporaryDeliveryOrderClearance obj)
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