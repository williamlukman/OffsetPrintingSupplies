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
using Core.Constants;
using System.Data.Objects;

namespace OffsetPrintingSupplies
{
    public class Program
    {

        public static void Main(string[] args)
        {
            var db = new OffsetPrintingSuppliesEntities();
            using (db)
            {
                //db.Configuration.LazyLoadingEnabled = true;
                //db.Configuration.ProxyCreationEnabled = true;
                //db.DeleteAllTables();
                ////DataAwalZentrum();

                //DataBuilder d = new DataBuilder();
                //d.PopulateData();

                var nov1 = new DateTime(2014, 11, 1);
                var nov30 = new DateTime(2014, 11, 30);
                var novrev = db.GeneralLedgerJournals.Include("Account").Where(x => !x.IsDeleted && x.Account.Group == Constant.AccountGroup.Revenue && x.SourceDocument != Constant.GeneralLedgerSource.Closing && EntityFunctions.TruncateTime(x.TransactionDate) <= nov30 && EntityFunctions.TruncateTime(x.TransactionDate) >= nov1).Sum(x => (Decimal?)(x.Status == Constant.GeneralLedgerStatus.Debit ? -x.Amount : x.Amount)) ?? 0;
                var novexp = db.GeneralLedgerJournals.Include("Account").Where(x => !x.IsDeleted && x.Account.Group == Constant.AccountGroup.Expense && x.SourceDocument != Constant.GeneralLedgerSource.Closing && EntityFunctions.TruncateTime(x.TransactionDate) <= nov30 && EntityFunctions.TruncateTime(x.TransactionDate) >= nov1).Sum(x => (Decimal?)(x.Status == Constant.GeneralLedgerStatus.Credit ? -x.Amount : x.Amount)) ?? 0;
                var nov72010001 = db.GeneralLedgerJournals.Include("Account").Where(x => !x.IsDeleted && x.Account.Code == "72010001" && x.SourceDocument != Constant.GeneralLedgerSource.Closing && EntityFunctions.TruncateTime(x.TransactionDate) <= nov30 && EntityFunctions.TruncateTime(x.TransactionDate) >= nov1).Sum(x => (Decimal?)(x.Status == Constant.GeneralLedgerStatus.Credit ? -x.Amount : x.Amount)) ?? 0;
                var nov11013 = db.GeneralLedgerJournals.Include("Account").Where(x => !x.IsDeleted && x.Account.Code == "11013" && x.SourceDocument != Constant.GeneralLedgerSource.Closing && EntityFunctions.TruncateTime(x.TransactionDate) <= nov30 && EntityFunctions.TruncateTime(x.TransactionDate) >= nov1).Sum(x => (Decimal?)(x.Status == Constant.GeneralLedgerStatus.Credit ? -x.Amount : x.Amount)) ?? 0;
                var nov11014 = db.GeneralLedgerJournals.Include("Account").Where(x => !x.IsDeleted && x.Account.Code == "11014" && x.SourceDocument != Constant.GeneralLedgerSource.Closing && EntityFunctions.TruncateTime(x.TransactionDate) <= nov30 && EntityFunctions.TruncateTime(x.TransactionDate) >= nov1).Sum(x => (Decimal?)(x.Status == Constant.GeneralLedgerStatus.Credit ? -x.Amount : x.Amount)) ?? 0;
                var nov11016 = db.GeneralLedgerJournals.Include("Account").Where(x => !x.IsDeleted && x.Account.Code == "11016" && x.SourceDocument != Constant.GeneralLedgerSource.Closing && EntityFunctions.TruncateTime(x.TransactionDate) <= nov30 && EntityFunctions.TruncateTime(x.TransactionDate) >= nov1).Sum(x => (Decimal?)(x.Status == Constant.GeneralLedgerStatus.Credit ? -x.Amount : x.Amount)) ?? 0;
                var nov11017 = db.GeneralLedgerJournals.Include("Account").Where(x => !x.IsDeleted && x.Account.Code == "11017" && x.SourceDocument != Constant.GeneralLedgerSource.Closing && EntityFunctions.TruncateTime(x.TransactionDate) <= nov30 && EntityFunctions.TruncateTime(x.TransactionDate) >= nov1).Sum(x => (Decimal?)(x.Status == Constant.GeneralLedgerStatus.Credit ? -x.Amount : x.Amount)) ?? 0;

                var dec1 = new DateTime(2014, 12, 1);
                var dec31 = new DateTime(2014, 12, 31);
                var decrev = db.GeneralLedgerJournals.Include("Account").Where(x => !x.IsDeleted && x.Account.Group == Constant.AccountGroup.Revenue && x.SourceDocument != Constant.GeneralLedgerSource.Closing && EntityFunctions.TruncateTime(x.TransactionDate) <= dec31 && EntityFunctions.TruncateTime(x.TransactionDate) >= dec1).Sum(x => (Decimal?)(x.Status == Constant.GeneralLedgerStatus.Debit ? -x.Amount : x.Amount)) ?? 0;
                var decexp = db.GeneralLedgerJournals.Include("Account").Where(x => !x.IsDeleted && x.Account.Group == Constant.AccountGroup.Expense && x.SourceDocument != Constant.GeneralLedgerSource.Closing && EntityFunctions.TruncateTime(x.TransactionDate) <= dec31 && EntityFunctions.TruncateTime(x.TransactionDate) >= dec1).Sum(x => (Decimal?)(x.Status == Constant.GeneralLedgerStatus.Credit ? -x.Amount : x.Amount)) ?? 0;
                var dec41010001 = db.GeneralLedgerJournals.Include("Account").Where(x => !x.IsDeleted && x.Account.Code == "41010001" && x.SourceDocument != Constant.GeneralLedgerSource.Closing && EntityFunctions.TruncateTime(x.TransactionDate) <= dec31 && EntityFunctions.TruncateTime(x.TransactionDate) >= dec1).Sum(x => (Decimal?)(x.Status == Constant.GeneralLedgerStatus.Debit ? -x.Amount : x.Amount)) ?? 0;
                var dec72010001 = db.GeneralLedgerJournals.Include("Account").Where(x => !x.IsDeleted && x.Account.Code == "72010001" && x.SourceDocument != Constant.GeneralLedgerSource.Closing && EntityFunctions.TruncateTime(x.TransactionDate) <= dec31 && EntityFunctions.TruncateTime(x.TransactionDate) >= dec1).Sum(x => (Decimal?)(x.Status == Constant.GeneralLedgerStatus.Credit ? -x.Amount : x.Amount)) ?? 0;
                var dec31040001 = db.GeneralLedgerJournals.Include("Account").Where(x => !x.IsDeleted && x.Account.Code == "31040001" && x.SourceDocument != Constant.GeneralLedgerSource.Closing && EntityFunctions.TruncateTime(x.TransactionDate) <= dec31 && EntityFunctions.TruncateTime(x.TransactionDate) >= dec1).Sum(x => (Decimal?)(x.Status == Constant.GeneralLedgerStatus.Debit ? -x.Amount : x.Amount)) ?? 0;
                var dec11016 = db.GeneralLedgerJournals.Include("Account").Where(x => !x.IsDeleted && x.Account.Code == "11016" && x.SourceDocument != Constant.GeneralLedgerSource.Closing && EntityFunctions.TruncateTime(x.TransactionDate) <= dec31 && EntityFunctions.TruncateTime(x.TransactionDate) >= dec1).Sum(x => (Decimal?)(x.Status == Constant.GeneralLedgerStatus.Credit ? -x.Amount : x.Amount)) ?? 0;
                var dec11017 = db.GeneralLedgerJournals.Include("Account").Where(x => !x.IsDeleted && x.Account.Code == "11017" && x.SourceDocument != Constant.GeneralLedgerSource.Closing && EntityFunctions.TruncateTime(x.TransactionDate) <= dec31 && EntityFunctions.TruncateTime(x.TransactionDate) >= dec1).Sum(x => (Decimal?)(x.Status == Constant.GeneralLedgerStatus.Credit ? -x.Amount : x.Amount)) ?? 0;

                Console.WriteLine("Nov:");
                Console.WriteLine("    Rev: " + novrev);
                Console.WriteLine("    Exp: " + novexp);
                Console.WriteLine("    72010001: " + nov72010001);
                Console.WriteLine("    11013: " + nov11013);
                Console.WriteLine("    11014: " + nov11014);
                Console.WriteLine("    11016: " + nov11016);
                Console.WriteLine("    11017: " + nov11017);
                Console.WriteLine();
                Console.WriteLine("Dec:");
                Console.WriteLine("    Rev: " + decrev);
                Console.WriteLine("    Exp: " + decexp);
                Console.WriteLine("    41010001: " + dec41010001);
                Console.WriteLine("    31040001: " + dec31040001);
                Console.WriteLine("    72010001: " + dec72010001);
                Console.WriteLine("    11016: " + dec11016);
                Console.WriteLine("    11017: " + dec11017);

                Console.WriteLine("Press any key to stop..");
                Console.ReadKey();

            }
        }

        public static void DataAwalZentrum()
        {
            ZengraBuilder z = new ZengraBuilder();
            // Warning: it will take around 1 hour to run the whole process
            z.PopulateCOA();
            z.PopulateMasterData();
            z.PopulateContact();
            z.PopulateSupplier();
            z.PopulateFinance();
            z.PopulateWarehouse();
            /*
            z.PopulateItemSerpong();
            z.PopulateItemSurabaya();
            z.PopulateBarsForBlanket();
            z.PopulateBlanket();
            z.PopulateCore();
            z.PopulateRoller();
            z.AdjustCore();
            z.StockAdjustBlanket();
            z.AdjustItemSurabaya();
            z.AdjustItemSemarang();
            z.AdjustItemSerpong();
            z.AdjustRoller();
             */
        }
    }
}
