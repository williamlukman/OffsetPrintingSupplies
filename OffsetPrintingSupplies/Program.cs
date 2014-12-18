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

                ZengraBuilder z = new ZengraBuilder();

                z.PopulateMasterData();
                z.PopulateContact();
                z.PopulateSupplier();
                z.PopulateCOA();
                z.PopulateFinance();
                z.PopulateItemSerpong();
                //z.AdjustItemSerpong();
                z.PopulateItemSurabaya();
                //z.AdjustItemSurabaya();
                //z.AdjustItemSemarang();
                z.PopulateBarsForBlanket();
                z.PopulateBlanket();
                //z.StockAdjustBlanket();
                z.PopulateCore();
                //z.AdjustCore();
                z.PopulateRoller();
                //z.AdjustRoller();
                Console.WriteLine("Press any key to stop...");
                Console.ReadKey();

            }
        }
    }
}
