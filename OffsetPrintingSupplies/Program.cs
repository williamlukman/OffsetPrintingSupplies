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
                //PurchaseBuilder p = new PurchaseBuilder();
               // SalesBuilder s = new SalesBuilder();

                  DataFunction(d);
                //SalesFunction(s);
                //PurchaseFunction(p);
            }
        }

        //public static void PurchaseFunction(PurchaseBuilder p)
        //{
        //    p.PopulateData();
        //}

        //public static void SalesFunction(SalesBuilder s)
        //{
        //    s.PopulateData();
        //}

        public static void DataFunction(DataBuilder d)
        {
            d.PopulateData();

            d.barringOrderContact = d._barringOrderService.ConfirmObject(d.barringOrderContact, DateTime.Today, d._barringOrderDetailService, d._barringService, d._itemService, d._warehouseItemService);

            Console.WriteLine("Press any key to stop...");
            Console.ReadKey();
        }
    }
}
