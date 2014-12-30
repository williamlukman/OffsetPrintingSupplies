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
                //DataAwalZentrum();

                DataBuilder d = new DataBuilder();
                d.PopulateData();
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
