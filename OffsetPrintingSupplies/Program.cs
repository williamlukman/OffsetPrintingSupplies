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

                //PurchaseBuilder p = new PurchaseBuilder();
                //p.PopulateData();

                DataBuilder d = new DataBuilder();
                DataFunction(d);

                Console.WriteLine("Press any key to stop...");
                Console.ReadKey();

            }
        }

        public static void DataFunction(DataBuilder d)
        {
            // d.PopulateData();
            d.PopulateUserRole();
            d.PopulateWarehouse();
            d.PopulateItem();
            d.PopulateSingles();
            d.PopulateCashBank();

            d.PopulateVirtualOrder();

        }
    }
}
