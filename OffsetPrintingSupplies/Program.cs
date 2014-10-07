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

                db.DeleteAllTables();
                d = new DataBuilder();

                DataFunction(d);

                Console.WriteLine("Press any key to stop...");
                Console.ReadKey();

            }
        }

        public static void DataFunction(DataBuilder d)
        {
           d.PopulateData();

           d.blanketOrderContact = d._blanketOrderService.ConfirmObject(d.blanketOrderContact, DateTime.Today, d._blanketOrderDetailService, d._blanketService, d._itemService, d._warehouseItemService);

           d._blanketOrderDetailService.CutObject(d.blanketODContact1, d._blanketOrderService);
           d._blanketOrderDetailService.CutObject(d.blanketODContact2, d._blanketOrderService);
           d._blanketOrderDetailService.CutObject(d.blanketODContact3, d._blanketOrderService);
           d._blanketOrderDetailService.CutObject(d.blanketODContact4, d._blanketOrderService);

           d._blanketOrderDetailService.SideSealObject(d.blanketODContact1);
           d._blanketOrderDetailService.PrepareObject(d.blanketODContact1, d._blanketService);
           d._blanketOrderDetailService.ApplyTapeAdhesiveToObject(d.blanketODContact1, 180, d._blanketService);
           d._blanketOrderDetailService.MountObject(d.blanketODContact1, d._blanketService);
           d._blanketOrderDetailService.HeatPressObject(d.blanketODContact1, d._blanketService);
           d._blanketOrderDetailService.PullOffTestObject(d.blanketODContact1, d._blanketService);
           d._blanketOrderDetailService.QCAndMarkObject(d.blanketODContact1, d._blanketService);
           d._blanketOrderDetailService.PackageObject(d.blanketODContact1);

           d._blanketOrderDetailService.SideSealObject(d.blanketODContact2);
           d._blanketOrderDetailService.PrepareObject(d.blanketODContact2, d._blanketService);
           d._blanketOrderDetailService.ApplyTapeAdhesiveToObject(d.blanketODContact2, 180, d._blanketService);
           d._blanketOrderDetailService.MountObject(d.blanketODContact2, d._blanketService);
           d._blanketOrderDetailService.HeatPressObject(d.blanketODContact2, d._blanketService);
           d._blanketOrderDetailService.PullOffTestObject(d.blanketODContact2, d._blanketService);
           d._blanketOrderDetailService.QCAndMarkObject(d.blanketODContact2, d._blanketService);
           d._blanketOrderDetailService.PackageObject(d.blanketODContact2);

           d._blanketOrderDetailService.SideSealObject(d.blanketODContact3);
           d._blanketOrderDetailService.PrepareObject(d.blanketODContact3, d._blanketService);
           d._blanketOrderDetailService.ApplyTapeAdhesiveToObject(d.blanketODContact3, 180, d._blanketService);
           d._blanketOrderDetailService.MountObject(d.blanketODContact3, d._blanketService);
           d._blanketOrderDetailService.HeatPressObject(d.blanketODContact3, d._blanketService);
           d._blanketOrderDetailService.PullOffTestObject(d.blanketODContact3, d._blanketService);
           d._blanketOrderDetailService.QCAndMarkObject(d.blanketODContact3, d._blanketService);
           d._blanketOrderDetailService.PackageObject(d.blanketODContact3);

           d._blanketOrderDetailService.SideSealObject(d.blanketODContact4);
           d._blanketOrderDetailService.PrepareObject(d.blanketODContact4, d._blanketService);
           d._blanketOrderDetailService.ApplyTapeAdhesiveToObject(d.blanketODContact4, 180, d._blanketService);
           d._blanketOrderDetailService.MountObject(d.blanketODContact4, d._blanketService);
           d._blanketOrderDetailService.HeatPressObject(d.blanketODContact4, d._blanketService);
           d._blanketOrderDetailService.PullOffTestObject(d.blanketODContact4, d._blanketService);
           d._blanketOrderDetailService.QCAndMarkObject(d.blanketODContact4, d._blanketService);

           int rollBlanket1quantity = d.rollBlanket1.Quantity;
           int rollBlanket2quantity = d.rollBlanket2.Quantity;
           int bargenericquantity = d.bargeneric.Quantity;
           int barleft1quantity = d.barleft1.Quantity;
           int barright1quantity = d.barright1.Quantity;
           int blanket1quantity = d.blanket1.Quantity;
           int blanket2quantity = d.blanket2.Quantity;
           int blanket1warehousequantity = d._warehouseItemService.FindOrCreateObject(d.localWarehouse.Id, d.blanket1.Id).Quantity;
           int blanket2warehousequantity = d._warehouseItemService.FindOrCreateObject(d.localWarehouse.Id, d.blanket2.Id).Quantity;
           d._blanketOrderDetailService.FinishObject(d.blanketODContact1, DateTime.Today, d._blanketOrderService, d._stockMutationService,
                                                     d._blanketService, d._itemService, d._warehouseItemService, d._accountService,
                                                     d._generalLedgerJournalService, d._closingService);
           d._blanketOrderDetailService.FinishObject(d.blanketODContact2, DateTime.Today, d._blanketOrderService, d._stockMutationService,
                                                     d._blanketService, d._itemService, d._warehouseItemService, d._accountService,
                                                     d._generalLedgerJournalService, d._closingService);
           d._blanketOrderDetailService.FinishObject(d.blanketODContact3, DateTime.Today, d._blanketOrderService, d._stockMutationService,
                                                     d._blanketService, d._itemService, d._warehouseItemService, d._accountService,
                                                     d._generalLedgerJournalService, d._closingService);
           d._blanketOrderDetailService.RejectObject(d.blanketODContact4, DateTime.Today, d._blanketOrderService, d._stockMutationService,
                                                     d._blanketService, d._itemService, d._warehouseItemService, d._accountService,
                                                     d._generalLedgerJournalService, d._closingService);

            int rollBlanket1quantityfinal = d.rollBlanket1.Quantity;
           int rollBlanket2quantityfinal = d.rollBlanket2.Quantity;
           int bargenericquantityfinal = d.bargeneric.Quantity;
           int barleft1quantityfinal = d.barleft1.Quantity;
           int barright1quantityfinal = d.barright1.Quantity;

           int blanket1quantityfinal = d.blanket1.Quantity;
           int blanket2quantityfinal = d.blanket2.Quantity;
           int blanket1warehousequantityfinal = d._warehouseItemService.FindOrCreateObject(d.localWarehouse.Id, d.blanket1.Id).Quantity;
           int blanket2warehousequantityfinal = d._warehouseItemService.FindOrCreateObject(d.localWarehouse.Id, d.blanket2.Id).Quantity;

        }
    }
}
