using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using Core.Interface.Validation;
using Core.DomainModel;
using Core.Interface.Service;
using Core.Constants;

namespace Validation.Validation
{
    public class StockMutationValidator : IStockMutationValidator
    {
        
        public StockMutation VItemCase(StockMutation sm)
        {
            if (!sm.ItemCase.Equals (Constant.StockMutationItemCase.Ready) &&
                !sm.ItemCase.Equals (Constant.StockMutationItemCase.PendingDelivery) &&
                !sm.ItemCase.Equals (Constant.StockMutationItemCase.PendingReceival))
            {
                sm.Errors.Add("ItemCase", "Harus merupakan bagian dari Constant.StockMutationItemCase");
            }
            return sm;
        }

        public StockMutation VStatus(StockMutation sm)
        {
            if (!sm.Status.Equals(Constant.StockMutationStatus.Addition) &&
                !sm.Status.Equals(Constant.StockMutationStatus.Deduction))
            {
                sm.Errors.Add("Status", "Harus merupakan bagian dari Constant.StockMutationStatus");
            }
            return sm;
        }

        public StockMutation VSourceDocumentType(StockMutation sm)
        {
            if (!sm.SourceDocumentType.Equals(Constant.SourceDocumentType.PurchaseOrder) &&
                !sm.SourceDocumentType.Equals(Constant.SourceDocumentType.PurchaseReceival) &&
                !sm.SourceDocumentType.Equals(Constant.SourceDocumentType.SalesOrder) &&
                !sm.SourceDocumentType.Equals(Constant.SourceDocumentType.DeliveryOrder))
            {
                sm.Errors.Add("SourceDocumentType", "Harus merupakan bagian dari Constant.SourceDocumentType");
            }
            return sm;
        }

        public StockMutation VSourceDocumentDetailType(StockMutation sm)
        {
            if (!sm.SourceDocumentDetailType.Equals(Constant.SourceDocumentDetailType.PurchaseOrderDetail) &&
                !sm.SourceDocumentDetailType.Equals(Constant.SourceDocumentDetailType.PurchaseReceivalDetail) &&
                !sm.SourceDocumentDetailType.Equals(Constant.SourceDocumentDetailType.SalesOrderDetail) &&
                !sm.SourceDocumentDetailType.Equals(Constant.SourceDocumentDetailType.DeliveryOrderDetail))
            {
                sm.Errors.Add("SourceDocumentDetailType", "Harus merupakan bagian dari Constant.SourceDocumentDetailType");
            }
            return sm;
        }

        public StockMutation VQuantity(StockMutation sm)
        {
            /*
             * value never reach null.
            if (sm.Quantity == null)
            {
                sm.Errors.Add("Quantity", "Tidak boleh tidak ada");
            }
            */
            return sm;
        }

        public StockMutation VCreateObject(StockMutation sm)
        {
            VItemCase(sm);
            if (!isValid(sm)) { return sm; }
            VStatus(sm);
            if (!isValid(sm)) { return sm; }
            VSourceDocumentType(sm);
            if (!isValid(sm)) { return sm; }
            VSourceDocumentDetailType(sm);
            if (!isValid(sm)) { return sm; }
            VQuantity(sm);
            return sm;
        }

        public StockMutation VDeleteObject(StockMutation sm)
        {
            return sm;
        }

        public bool ValidCreateObject(StockMutation sm)
        {
            VCreateObject(sm);
            return isValid(sm);
        }

        public bool ValidDeleteObject(StockMutation sm)
        {
            sm.Errors.Clear();
            VDeleteObject(sm);
            return isValid(sm);
        }

        public bool isValid(StockMutation obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public string PrintError(StockMutation obj)
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
