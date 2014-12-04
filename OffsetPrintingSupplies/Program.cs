using Core.DomainModel;
using Core.Interface.Service;
using Data.Context;
using Data.Repository;
using Service.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestValidation;
using Validation.Validation;

namespace OffsetPrintingSupplies
{
    public class Program
    {

        public static void Main(string[] args)
        {
            var db = new OffsetPrintingSuppliesEntities();
            using (db)
            {
                db.Configuration.LazyLoadingEnabled = true;
                db.Configuration.ProxyCreationEnabled = true;

                db.DeleteAllTables();

                DataBuilder d = new DataBuilder();
                
                //DataFunction(d);
                d.PopulateDataNoClosing();

                d._blendingWorkOrderService.ConfirmObject(d.blendingWorkOrder, DateTime.Today, d._blendingRecipeService, d._blendingRecipeDetailService, d._stockMutationService,
                                                        d._blanketService, d._itemService, d._warehouseItemService, d._generalLedgerJournalService, d._accountService, d._closingService);

                d._blendingWorkOrderService.UnconfirmObject(d.blendingWorkOrder, d._blendingRecipeService, d._blendingRecipeDetailService, d._stockMutationService,
                                                    d._blanketService, d._itemService, d._warehouseItemService, d._generalLedgerJournalService, d._accountService, d._closingService);

                d._blendingWorkOrderService.SoftDeleteObject(d.blendingWorkOrder);

                //d.PopulateUserRole();
                //d.PopulateWarehouse();
                //d.PopulateItem(); // 1. Stock Adjustment
                //d.PopulateSingles();
                //d.PopulateCashBank(); // 2. CashBankAdjustment, 3. CashBankMutation, 4. CashBankAdjustment (Negative)

                //d.PopulateSales(); // 5. 3x Cash Invoice
                //d.PopulateValidComb(); // 7. Closing
                //// salesinvoice 148000 + salesinvoice 144000 + salesinvoice 152000
                //// receiptvoucher 221 148000, 222 144000, 223 152000
                //decimal ReceivableAmount = (d.receiptVoucher1.TotalAmount + d.receiptVoucher2.TotalAmount + d.receiptVoucher3.TotalAmount)
                //           - (d.salesInvoice1.AmountReceivable + d.salesInvoice2.AmountReceivable + d.salesInvoice3.AmountReceivable);
                //decimal KontanAmount = d.receiptVoucher1.TotalAmount + d.receiptVoucher2.TotalAmount + d.receiptVoucher3.TotalAmount +
                //                       d.cashBankAdjustment.Amount + d.cashBankAdjustment2.Amount - d.cashBankMutation.Amount;

                Console.WriteLine("Press any key to stop...");
                Console.ReadKey();

            }
        }

        public static void DataFunction(DataBuilder d)
        {
            d.PopulateData();
        }
    }
}
