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
                db.DeleteAllTables();

                DataBuilder d = new DataBuilder();
                
                d.PopulateUserRole();
                d.PopulateWarehouse();
                d.PopulateItem(); // 1. Stock Adjustment
                d.PopulateSingles();
                d.PopulateCashBank(); // 2. CashBankAdjustment, 3. CashBankMutation, 4. CashBankAdjustment (Negative)
                d.PopulateSales(); // 5. 3x Cash Invoice

                d.PopulateValidComb(); // 7. Closing

                Account Asset, CashBankAccount, AccountReceivableIDR, GBCHReceivable, Inventory;

                Inventory = d._accountService.GetObjectByLegacyCode(Core.Constants.Constant.AccountLegacyCode.Inventory);
                GBCHReceivable = d._accountService.GetObjectByLegacyCode(Core.Constants.Constant.AccountLegacyCode.GBCHReceivable);
                
                AccountReceivableIDR = d._accountService.GetObjectByLegacyCode(Core.Constants.Constant.AccountLegacyCode.AccountReceivable + d.currencyIDR.Id);
                
                Account kontan = d._accountService.GetObjectByLegacyCode(Core.Constants.Constant.AccountLegacyCode.CashBank + d.cashBank1.Id);
                Account bca = d._accountService.GetObjectByLegacyCode(Core.Constants.Constant.AccountLegacyCode.CashBank + d.cashBank2.Id);
                Account cashBank = d._accountService.GetObjectByLegacyCode(Core.Constants.Constant.AccountLegacyCode.CashBank + d.cashBank.Id);
                Account pettyCash = d._accountService.GetObjectByLegacyCode(Core.Constants.Constant.AccountLegacyCode.CashBank + d.pettyCash.Id);
                CashBankAccount = d._accountService.GetObjectByLegacyCode(Core.Constants.Constant.AccountLegacyCode.CashBank);
                Asset = d._accountService.GetObjectByLegacyCode(Core.Constants.Constant.AccountLegacyCode.Asset);

                decimal InventoryAmount = (d.sad1.Price * d.sad1.Quantity) + (d.sad2.Price * d.sad2.Quantity) +
                                          (d.sad3.Price * d.sad3.Quantity) + (d.sad4.Price * d.sad4.Quantity) +
                                          (d.sad5.Price * d.sad5.Quantity) + (d.sadAdhesiveBlanket.Price * d.sadAdhesiveBlanket.Quantity) +
                                          (d.sadAdhesiveRoller.Price * d.sadAdhesiveRoller.Quantity)
                                          - d.deliveryOrder1.TotalCOGS - d.deliveryOrder2.TotalCOGS - d.deliveryOrder3.TotalCOGS;
                decimal GBCHReceivableAmount = 0;
                decimal ReceivableAmount = (d.receiptVoucher1.TotalAmount + d.receiptVoucher2.TotalAmount + d.receiptVoucher3.TotalAmount)
                                           - (d.salesInvoice1.AmountReceivable + d.salesInvoice2.AmountReceivable + d.salesInvoice3.AmountReceivable);
                decimal KontanAmount = d.receiptVoucher1.TotalAmount + d.receiptVoucher2.TotalAmount + d.receiptVoucher3.TotalAmount +
                                       d.cashBankAdjustment.Amount + d.cashBankAdjustment2.Amount - d.cashBankMutation.Amount;
                decimal cashBank0Amount = d.cashBankAdjustment3.Amount;
                decimal pettyCashAmount = (decimal)0;
                decimal bcaAmount = d.cashBankMutation.Amount;
                decimal cashBankAmount = KontanAmount + bcaAmount + cashBank0Amount + pettyCashAmount;
                decimal AssetAmount = InventoryAmount + ReceivableAmount + GBCHReceivableAmount + cashBankAmount;

                // Expense
                Account Expense, CashBankAdjustmentExpense, COGS, Discount, SalesAllowance, StockAdjustmentExpense;

                StockAdjustmentExpense = d._accountService.GetObjectByLegacyCode(Core.Constants.Constant.AccountLegacyCode.StockAdjustmentExpense);
                SalesAllowance = d._accountService.GetObjectByLegacyCode(Core.Constants.Constant.AccountLegacyCode.SalesAllowance);
                Discount = d._accountService.GetObjectByLegacyCode(Core.Constants.Constant.AccountLegacyCode.Discount);
                COGS = d._accountService.GetObjectByLegacyCode(Core.Constants.Constant.AccountLegacyCode.COGS);
                CashBankAdjustmentExpense = d._accountService.GetObjectByLegacyCode(Core.Constants.Constant.AccountLegacyCode.CashBankAdjustmentExpense);
                Expense = d._accountService.GetObjectByLegacyCode(Core.Constants.Constant.AccountLegacyCode.Expense);

                decimal StockAdjustmentExpenseAmount = 0;
                decimal SalesAllowanceAmount = 0;
                decimal DiscountAmount = 0;
                decimal COGSAmount = 0;
                // Right Hand Side
                Account Liability, AccountPayable, GBCHPayable, GoodsPendingClearance;
                Account Equity, OwnersEquity, EquityAdjustment;
                Account Revenue;

                EquityAdjustment = d._accountService.GetObjectByLegacyCode(Core.Constants.Constant.AccountLegacyCode.EquityAdjustment);

                OwnersEquity = d._accountService.GetObjectByLegacyCode(Core.Constants.Constant.AccountLegacyCode.OwnersEquity);
                Equity = d._accountService.GetObjectByLegacyCode(Core.Constants.Constant.AccountLegacyCode.Equity);
                
                decimal CBAdjustmentExpenseAmount = Math.Abs(d.cashBankAdjustment2.Amount);
                decimal ExpenseAmount = CBAdjustmentExpenseAmount + COGSAmount + DiscountAmount + SalesAllowanceAmount +
                                        StockAdjustmentExpenseAmount;
                decimal EquityAdjustmentAmount = (d.sad1.Price * d.sad1.Quantity) + (d.sad2.Price * d.sad2.Quantity) +
                                                 (d.sad3.Price * d.sad3.Quantity) + (d.sad4.Price * d.sad4.Quantity) +
                                                 (d.sad5.Price * d.sad5.Quantity) + d.cashBankAdjustment.Amount +
                                                 (d.sadAdhesiveBlanket.Price * d.sadAdhesiveBlanket.Quantity) +
                                                 (d.sadAdhesiveRoller.Price * d.sadAdhesiveRoller.Quantity) +
                                                 d.cashBankAdjustment3.Amount;

                decimal InventoryA = d._validCombService.FindOrCreateObjectByAccountAndClosing(Inventory.Id, d.thisMonthClosing.Id).Amount;
                decimal kontanA = d._validCombService.FindOrCreateObjectByAccountAndClosing(kontan.Id, d.thisMonthClosing.Id).Amount;
                decimal bcaA = d._validCombService.FindOrCreateObjectByAccountAndClosing(bca.Id, d.thisMonthClosing.Id).Amount;
                decimal CashBankA = d._validCombService.FindOrCreateObjectByAccountAndClosing(CashBankAccount.Id, d.thisMonthClosing.Id).Amount;
                decimal ARIDR = d._validCombService.FindOrCreateObjectByAccountAndClosing(AccountReceivableIDR.Id, d.thisMonthClosing.Id).Amount;
                Account AccountReceivable = d._accountService.GetObjectByLegacyCode(Core.Constants.Constant.AccountLegacyCode.AccountReceivable + d.currencyIDR.Id);
                decimal AR = d._validCombService.FindOrCreateObjectByAccountAndClosing(AccountReceivableIDR.Id, d.thisMonthClosing.Id).Amount;
                decimal A = d._validCombService.FindOrCreateObjectByAccountAndClosing(Asset.Id, d.thisMonthClosing.Id).Amount;

                decimal EquityAdjustmentA = d._validCombService.FindOrCreateObjectByAccountAndClosing(EquityAdjustment.Id, d.thisMonthClosing.Id).Amount;
                decimal OwnersEA = d._validCombService.FindOrCreateObjectByAccountAndClosing(OwnersEquity.Id, d.thisMonthClosing.Id).Amount;
                decimal EquityA = d._validCombService.FindOrCreateObjectByAccountAndClosing(Equity.Id, d.thisMonthClosing.Id).Amount;

                // DataFunction(d);

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
