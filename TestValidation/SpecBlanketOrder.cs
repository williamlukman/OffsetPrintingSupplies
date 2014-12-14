using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.DomainModel;
using NSpec;
using Service.Service;
using Core.Interface.Service;
using Data.Context;
using System.Data.Entity;
using Data.Repository;
using Validation.Validation;

namespace TestValidation
{

    public class SpecBlanketOrder: nspec
    {
        DataBuilder d;
        
        void before_each()
        {
            var db = new OffsetPrintingSuppliesEntities();
            using (db)
            {
                db.DeleteAllTables();
                d = new DataBuilder();
                d.PopulateData();
            }
        }

        void data_validation()
        {
        
            it["validates_data"] = () =>
            {
                d.bargeneric.Errors.Count().should_be(0);
                d.barleft1.Errors.Count().should_be(0);
                d.barleft2.Errors.Count().should_be(0);
                d.barright1.Errors.Count().should_be(0);
                d.barright2.Errors.Count().should_be(0);
                d.rollBlanket1.Errors.Count().should_be(0);
                d.rollBlanket2.Errors.Count().should_be(0);
                d.rollBlanket3.Errors.Count().should_be(0);
                d.blanketOrderContact.Errors.Count().should_be(0);
                d.blanketODContact1.Errors.Count().should_be(0);
                d.blanketODContact2.Errors.Count().should_be(0);
                d.blanketODContact3.Errors.Count().should_be(0);
                d.blanketODContact4.Errors.Count().should_be(0);
            };

            it["deletes_blanketorder"] = () =>
            {
                d.blanketOrderContact = d._blanketOrderService.SoftDeleteObject(d.blanketOrderContact, d._blanketOrderDetailService);
                d.blanketOrderContact.Errors.Count().should_be(0);
            };

            it["confirms_blanketorder"] = () =>
            {
                d.blanketOrderContact = d._blanketOrderService.ConfirmObject(d.blanketOrderContact, DateTime.Today, d._blanketOrderDetailService, d._blanketService, d._itemService, d._warehouseItemService);
                d.blanketOrderContact.IsConfirmed.should_be(true);
                d.blanketOrderContact.Errors.Count().should_be(0);
            };

            it["unconfirms_blanket_order"] = () =>
            {
                d.blanketOrderContact = d._blanketOrderService.ConfirmObject(d.blanketOrderContact, DateTime.Today, d._blanketOrderDetailService, d._blanketService, d._itemService, d._warehouseItemService);
                d.blanketOrderContact.IsConfirmed.should_be(true);
                d.blanketOrderContact = d._blanketOrderService.UnconfirmObject(d.blanketOrderContact, d._blanketOrderDetailService, d._blanketService, d._itemService, d._warehouseItemService);
                d.blanketOrderContact.IsConfirmed.should_be(false);
                d.blanketOrderContact.Errors.Count().should_be(0);
            };

            it["deletes_blanketorder_with_processed_detail"] = () =>
            {
                d.blanketOrderContact = d._blanketOrderService.ConfirmObject(d.blanketOrderContact, DateTime.Today, d._blanketOrderDetailService, d._blanketService, d._itemService, d._warehouseItemService);
                d.blanketODContact1 = d._blanketOrderDetailService.CutObject(d.blanketODContact1, d._blanketOrderService);
                d.blanketODContact1 = d._blanketOrderDetailService.SideSealObject(d.blanketODContact1);

                d.blanketOrderContact = d._blanketOrderService.SoftDeleteObject(d.blanketOrderContact, d._blanketOrderDetailService);
                d.blanketOrderContact.Errors.Count().should_not_be(0);
            };

            context["when blanket order details are cut"] = () =>
            {
                before = () =>
                {
                    d.blanketOrderContact = d._blanketOrderService.ConfirmObject(d.blanketOrderContact, DateTime.Today, d._blanketOrderDetailService, d._blanketService, d._itemService, d._warehouseItemService);

                    d._blanketOrderDetailService.CutObject(d.blanketODContact1, d._blanketOrderService);
                    d._blanketOrderDetailService.CutObject(d.blanketODContact2, d._blanketOrderService);
                    d._blanketOrderDetailService.CutObject(d.blanketODContact3, d._blanketOrderService);
                    d._blanketOrderDetailService.CutObject(d.blanketODContact4, d._blanketOrderService);
                };

                it["validates_blanketorderdetails"] = () =>
                {
                    d.blanketODContact1.Errors.Count().should_be(0);
                    d.blanketODContact2.Errors.Count().should_be(0);
                    d.blanketODContact3.Errors.Count().should_be(0);
                    d.blanketODContact4.Errors.Count().should_be(0);
                };

                it["validates_unconfirmblanketorder"] = () =>
                {
                    d.blanketOrderContact = d._blanketOrderService.UnconfirmObject(d.blanketOrderContact, d._blanketOrderDetailService, d._blanketService, d._itemService, d._warehouseItemService);
                    d.blanketOrderContact.IsConfirmed.should_be(true);
                    d.blanketOrderContact.Errors.Count().should_not_be(0);
                };

                context["when blanket order details are all finished"] = () =>
                {
                    before = () =>
                    {
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
                    };

                    it["validates_blanketorderdetails"] = () =>
                    {
                        d.blanketODContact1.Errors.Count().should_be(0);
                        d.blanketODContact2.Errors.Count().should_be(0);
                        d.blanketODContact3.Errors.Count().should_be(0);
                        d.blanketODContact4.Errors.Count().should_be(0);
                    };

                    it["validates_finishblanketorder"] = () =>
                    {
                        decimal rollBlanket1quantity = d.rollBlanket1.Quantity;
                        decimal rollBlanket2quantity = d.rollBlanket2.Quantity;
                        decimal bargenericquantity = d.bargeneric.Quantity;
                        decimal barleft1quantity = d.barleft1.Quantity;
                        decimal barright1quantity = d.barright1.Quantity;
                        decimal blanket1quantity = d.blanket1.Quantity;
                        decimal blanket2quantity = d.blanket2.Quantity;
                        decimal blanket1warehousequantity = d._warehouseItemService.FindOrCreateObject(d.localWarehouse.Id, d.blanket1.Id).Quantity;
                        decimal blanket2warehousequantity = d._warehouseItemService.FindOrCreateObject(d.localWarehouse.Id, d.blanket2.Id).Quantity;
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
                        decimal rollBlanket1quantityfinal = d.rollBlanket1.Quantity;
                        decimal rollBlanket2quantityfinal = d.rollBlanket2.Quantity;
                        decimal bargenericquantityfinal = d.bargeneric.Quantity;
                        decimal barleft1quantityfinal = d.barleft1.Quantity;
                        decimal barright1quantityfinal = d.barright1.Quantity;
                        rollBlanket1quantityfinal.should_be(rollBlanket1quantity - 2);
                        rollBlanket2quantityfinal.should_be(rollBlanket2quantity - 2);
                        bargenericquantityfinal.should_be(bargenericquantity - 4);
                        barleft1quantityfinal.should_be(barleft1quantity - 2);
                        barright1quantityfinal.should_be(barright1quantity - 2);
                        d.blanketOrderContact.IsCompleted.should_be(true);
                        decimal blanket1quantityfinal = d.blanket1.Quantity;
                        decimal blanket2quantityfinal = d.blanket2.Quantity;
                        decimal blanket1warehousequantityfinal = d._warehouseItemService.FindOrCreateObject(d.localWarehouse.Id, d.blanket1.Id).Quantity;
                        decimal blanket2warehousequantityfinal = d._warehouseItemService.FindOrCreateObject(d.localWarehouse.Id, d.blanket2.Id).Quantity;
                        blanket1quantityfinal.should_be(blanket1quantity + 2);
                        blanket2quantityfinal.should_be(blanket2quantity + 1);
                        blanket1warehousequantityfinal.should_be(blanket1warehousequantity + 2);
                        blanket2warehousequantityfinal.should_be(blanket2warehousequantity + 1);
                        d.blanketOrderContact.QuantityFinal.should_be(d.blanketOrderContact.QuantityReceived - d.blanketOrderContact.QuantityRejected);
                    };
                };
            };
        }
    }
}