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
                PurchaseBuilder p = new PurchaseBuilder();
                SalesBuilder s = new SalesBuilder();

                SalesFunction(s);
                // PurchaseFunction(p);
            }
        }

        public static void PurchaseFunction(PurchaseBuilder p)
        {
            p.PopulateData();
        }

        public static void SalesFunction(SalesBuilder s)
        {
            s.PopulateData();
        }

        public static void DataFunction(DataBuilder d)
        {
            d.PopulateData();
                
            if (d.itemCompound.Errors.Count() > 0) { Console.WriteLine("Error"); };
            if (d.itemCompound1.Errors.Count() > 0) { Console.WriteLine("Error"); };
            if (d.itemCompound2.Errors.Count() > 0) { Console.WriteLine("Error"); };
            if (d.itemAccessory1.Errors.Count() > 0) { Console.WriteLine("Error"); };
            if (d.itemAccessory2.Errors.Count() > 0) { Console.WriteLine("Error"); };
            if (d.contact.Errors.Count() > 0) { Console.WriteLine("Error"); };
            if (d.machine.Errors.Count() > 0) { Console.WriteLine("Error"); };
            if (d.coreBuilder.Errors.Count() > 0) { Console.WriteLine("Error"); };
            if (d.coreBuilder1.Errors.Count() > 0) { Console.WriteLine("Error"); };
            if (d.coreBuilder2.Errors.Count() > 0) { Console.WriteLine("Error"); };
            if (d.coreBuilder3.Errors.Count() > 0) { Console.WriteLine("Error"); };
            if (d.coreBuilder4.Errors.Count() > 0) { Console.WriteLine("Error"); };
            if (d.coreIdentification.Errors.Count() > 0) { Console.WriteLine("Error"); };
            if (d.coreIdentificationContact.Errors.Count() > 0) { Console.WriteLine("Error"); };
            if (d.coreIDContact1.Errors.Count() > 0) { Console.WriteLine("Error"); };
            if (d.coreIDContact2.Errors.Count() > 0) { Console.WriteLine("Error"); };
            if (d.coreIDContact3.Errors.Count() > 0) { Console.WriteLine("Error"); };
            if (d.coreIdentificationDetail.Errors.Count() > 0) { Console.WriteLine("Error"); };
            if (d.coreIdentificationInHouse.Errors.Count() > 0) { Console.WriteLine("Error"); };
            if (d.coreIDInHouse1.Errors.Count() > 0) { Console.WriteLine("Error"); };
            if (d.coreIDInHouse2.Errors.Count() > 0) { Console.WriteLine("Error"); };
            if (d.coreIDInHouse3.Errors.Count() > 0) { Console.WriteLine("Error"); };
            if (d.recoveryOrderContact.Errors.Count() > 0) { Console.WriteLine("Error"); };
            if (d.recoveryODContact1.Errors.Count() > 0) { Console.WriteLine("Error"); };
            if (d.recoveryODContact2.Errors.Count() > 0) { Console.WriteLine("Error"); };
            if (d.recoveryODContact3.Errors.Count() > 0) { Console.WriteLine("Error"); };
            if (d.recoveryOrderInHouse.Errors.Count() > 0) { Console.WriteLine("Error"); };
            if (d.recoveryODInHouse1.Errors.Count() > 0) { Console.WriteLine("Error"); };
            if (d.recoveryODInHouse2.Errors.Count() > 0) { Console.WriteLine("Error"); };
            if (d.recoveryODInHouse3.Errors.Count() > 0) { Console.WriteLine("Error"); };

            int blanket1quantityPRE = d.blanket1.Quantity;
            int blanket2quantityPRE = d.blanket2.Quantity; // 5?
            int bargenericquantityPRE = d.bargeneric.Quantity;
            int barleft1quantityPRE = d.barleft1.Quantity;
            int barright1quantityPRE = d.barright1.Quantity;
            int barring1quantityPRE = d.barring1.Quantity;
            int barring2quantityPRE = d.barring2.Quantity;

            d.barringOrderContact = d._barringOrderService.ConfirmObject(d.barringOrderContact, DateTime.Today, d._barringOrderDetailService, d._barringService, d._itemService, d._warehouseItemService);

            d._barringOrderDetailService.CutObject(d.barringODContact1);
            d._barringOrderDetailService.CutObject(d.barringODContact2);
            d._barringOrderDetailService.CutObject(d.barringODContact3);
            d._barringOrderDetailService.CutObject(d.barringODContact4);
            d._barringOrderDetailService.SideSealObject(d.barringODContact1);
            d._barringOrderDetailService.PrepareObject(d.barringODContact1);
            d._barringOrderDetailService.ApplyTapeAdhesiveToObject(d.barringODContact1);
            d._barringOrderDetailService.MountObject(d.barringODContact1);
            d._barringOrderDetailService.HeatPressObject(d.barringODContact1);
            d._barringOrderDetailService.PullOffTestObject(d.barringODContact1);
            d._barringOrderDetailService.QCAndMarkObject(d.barringODContact1);
            d._barringOrderDetailService.PackageObject(d.barringODContact1);
            d._barringOrderDetailService.AddLeftBar(d.barringODContact1, d._barringService);
            d._barringOrderDetailService.AddRightBar(d.barringODContact1, d._barringService);

            d._barringOrderDetailService.SideSealObject(d.barringODContact2);
            d._barringOrderDetailService.PrepareObject(d.barringODContact2);
            d._barringOrderDetailService.ApplyTapeAdhesiveToObject(d.barringODContact2);
            d._barringOrderDetailService.MountObject(d.barringODContact2);
            d._barringOrderDetailService.HeatPressObject(d.barringODContact2);
            d._barringOrderDetailService.PullOffTestObject(d.barringODContact2);
            d._barringOrderDetailService.QCAndMarkObject(d.barringODContact2);
            d._barringOrderDetailService.PackageObject(d.barringODContact2);
            d._barringOrderDetailService.AddLeftBar(d.barringODContact2, d._barringService);
            d._barringOrderDetailService.AddRightBar(d.barringODContact2, d._barringService);

            d._barringOrderDetailService.SideSealObject(d.barringODContact3);
            d._barringOrderDetailService.PrepareObject(d.barringODContact3);
            d._barringOrderDetailService.ApplyTapeAdhesiveToObject(d.barringODContact3);
            d._barringOrderDetailService.MountObject(d.barringODContact3);
            d._barringOrderDetailService.HeatPressObject(d.barringODContact3);
            d._barringOrderDetailService.PullOffTestObject(d.barringODContact3);
            d._barringOrderDetailService.QCAndMarkObject(d.barringODContact3);
            d._barringOrderDetailService.PackageObject(d.barringODContact3);
            d._barringOrderDetailService.AddLeftBar(d.barringODContact3, d._barringService);
            d._barringOrderDetailService.AddRightBar(d.barringODContact3, d._barringService);

            d._barringOrderDetailService.SideSealObject(d.barringODContact4);
            d._barringOrderDetailService.PrepareObject(d.barringODContact4);
            d._barringOrderDetailService.ApplyTapeAdhesiveToObject(d.barringODContact4);
            d._barringOrderDetailService.MountObject(d.barringODContact4);
            d._barringOrderDetailService.HeatPressObject(d.barringODContact4);
            d._barringOrderDetailService.PullOffTestObject(d.barringODContact4);
            d._barringOrderDetailService.QCAndMarkObject(d.barringODContact4);
            d._barringOrderDetailService.AddLeftBar(d.barringODContact4, d._barringService);

            int blanket1quantity = d.blanket1.Quantity;
            int blanket2quantity = d.blanket2.Quantity; // 5?
            int bargenericquantity = d.bargeneric.Quantity;
            int barleft1quantity = d.barleft1.Quantity;
            int barright1quantity = d.barright1.Quantity;
            int barring1quantity = d.barring1.Quantity;
            int barring2quantity = d.barring2.Quantity;
            int barring1warehousequantity = d._warehouseItemService.FindOrCreateObject(d.localWarehouse.Id, d.barring1.Id).Quantity;
            int barring2warehousequantity = d._warehouseItemService.FindOrCreateObject(d.localWarehouse.Id, d.barring2.Id).Quantity;
            d._barringOrderDetailService.FinishObject(d.barringODContact1, DateTime.Today, d._barringOrderService, d._stockMutationService, d._barringService, d._itemService, d._warehouseItemService);
            d._barringOrderDetailService.FinishObject(d.barringODContact2, DateTime.Today, d._barringOrderService, d._stockMutationService, d._barringService, d._itemService, d._warehouseItemService);
            d._barringOrderDetailService.FinishObject(d.barringODContact3, DateTime.Today, d._barringOrderService, d._stockMutationService, d._barringService, d._itemService, d._warehouseItemService);
            d._barringOrderDetailService.RejectObject(d.barringODContact4, DateTime.Today, d._barringOrderService, d._stockMutationService, d._barringService, d._itemService, d._warehouseItemService);
            int blanket1quantityfinal = d.blanket1.Quantity;
            int blanket2quantityfinal = d.blanket2.Quantity; // 3 ?
            int bargenericquantityfinal = d.bargeneric.Quantity;
            int barleft1quantityfinal = d.barleft1.Quantity;
            int barright1quantityfinal = d.barright1.Quantity;

            Console.WriteLine("Press any key to stop...");
            Console.ReadKey();
        }
    }
}
