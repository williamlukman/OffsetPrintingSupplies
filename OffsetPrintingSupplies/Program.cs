using Core.DomainModel;
using Core.Interface.Service;
using Data.Context;
using Data.Repository;
using Service.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                d.PopulateData();
                d.barringOrderCustomer = d._barringOrderService.ConfirmObject(d.barringOrderCustomer, d._barringOrderDetailService, d._barringService, d._itemService);

                d._barringOrderDetailService.CutObject(d.barringODCustomer1);
                d._barringOrderDetailService.CutObject(d.barringODCustomer2);
                d._barringOrderDetailService.CutObject(d.barringODCustomer3);
                d._barringOrderDetailService.CutObject(d.barringODCustomer4);

                d._barringOrderDetailService.SideSealObject(d.barringODCustomer1);
                d._barringOrderDetailService.PrepareObject(d.barringODCustomer1);
                d._barringOrderDetailService.ApplyTapeAdhesiveToObject(d.barringODCustomer1);
                d._barringOrderDetailService.MountObject(d.barringODCustomer1);
                d._barringOrderDetailService.HeatPressObject(d.barringODCustomer1);
                d._barringOrderDetailService.PullOffTestObject(d.barringODCustomer1);
                d._barringOrderDetailService.QCAndMarkObject(d.barringODCustomer1);
                d._barringOrderDetailService.PackageObject(d.barringODCustomer1);
                d._barringOrderDetailService.AddLeftBar(d.barringODCustomer1, d._barringService);
                d._barringOrderDetailService.AddRightBar(d.barringODCustomer1, d._barringService);

                d._barringOrderDetailService.SideSealObject(d.barringODCustomer2);
                d._barringOrderDetailService.PrepareObject(d.barringODCustomer2);
                d._barringOrderDetailService.ApplyTapeAdhesiveToObject(d.barringODCustomer2);
                d._barringOrderDetailService.MountObject(d.barringODCustomer2);
                d._barringOrderDetailService.HeatPressObject(d.barringODCustomer2);
                d._barringOrderDetailService.PullOffTestObject(d.barringODCustomer2);
                d._barringOrderDetailService.QCAndMarkObject(d.barringODCustomer2);
                d._barringOrderDetailService.PackageObject(d.barringODCustomer2);
                d._barringOrderDetailService.AddLeftBar(d.barringODCustomer2, d._barringService);
                d._barringOrderDetailService.AddRightBar(d.barringODCustomer2, d._barringService);

                d._barringOrderDetailService.SideSealObject(d.barringODCustomer3);
                d._barringOrderDetailService.PrepareObject(d.barringODCustomer3);
                d._barringOrderDetailService.ApplyTapeAdhesiveToObject(d.barringODCustomer3);
                d._barringOrderDetailService.MountObject(d.barringODCustomer3);
                d._barringOrderDetailService.HeatPressObject(d.barringODCustomer3);
                d._barringOrderDetailService.PullOffTestObject(d.barringODCustomer3);
                d._barringOrderDetailService.QCAndMarkObject(d.barringODCustomer3);
                d._barringOrderDetailService.PackageObject(d.barringODCustomer3);
                d._barringOrderDetailService.AddLeftBar(d.barringODCustomer3, d._barringService);
                d._barringOrderDetailService.AddRightBar(d.barringODCustomer3, d._barringService);

                d._barringOrderDetailService.SideSealObject(d.barringODCustomer4);
                d._barringOrderDetailService.PrepareObject(d.barringODCustomer4);
                d._barringOrderDetailService.ApplyTapeAdhesiveToObject(d.barringODCustomer4);
                d._barringOrderDetailService.MountObject(d.barringODCustomer4);
                d._barringOrderDetailService.HeatPressObject(d.barringODCustomer4);
                d._barringOrderDetailService.PullOffTestObject(d.barringODCustomer4);
                d._barringOrderDetailService.QCAndMarkObject(d.barringODCustomer4);
                d._barringOrderDetailService.RejectObject(d.barringODCustomer4, d._barringOrderService);

                int barring1quantity = d.barring1.Quantity;
                int barring2quantity = d.barring2.Quantity;
                d.barringOrderCustomer = d._barringOrderService.FinishObject(d.barringOrderCustomer, d._barringOrderDetailService, d._barringService, d._itemService);
                int barring1quantityfinal = d.barring1.Quantity;
                int barring2quantityfinal = d.barring2.Quantity;

                Console.WriteLine("Press any key to stop...");
                Console.ReadKey();
            }

        }
    }
}
