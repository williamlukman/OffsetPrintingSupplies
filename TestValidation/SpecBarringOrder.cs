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

    public class SpecBarringOrder: nspec
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
                d.blanket1.Errors.Count().should_be(0);
                d.blanket2.Errors.Count().should_be(0);
                d.blanket3.Errors.Count().should_be(0);
                d.barringOrderContact.Errors.Count().should_be(0);
                d.barringODContact1.Errors.Count().should_be(0);
                d.barringODContact2.Errors.Count().should_be(0);
                d.barringODContact3.Errors.Count().should_be(0);
                d.barringODContact4.Errors.Count().should_be(0);
            };

            it["deletes_barringorder"] = () =>
            {
                d.barringOrderContact = d._barringOrderService.SoftDeleteObject(d.barringOrderContact, d._barringOrderDetailService);
                d.barringOrderContact.Errors.Count().should_be(0);
            };

            it["confirms_barringorder"] = () =>
            {
                d.barringOrderContact = d._barringOrderService.ConfirmObject(d.barringOrderContact, DateTime.Today, d._barringOrderDetailService, d._barringService, d._itemService, d._warehouseItemService);
                d.barringOrderContact.IsConfirmed.should_be(true);
                d.barringOrderContact.Errors.Count().should_be(0);
            };

            it["unconfirms_barring_order"] = () =>
            {
                d.barringOrderContact = d._barringOrderService.ConfirmObject(d.barringOrderContact, DateTime.Today, d._barringOrderDetailService, d._barringService, d._itemService, d._warehouseItemService);
                d.barringOrderContact.IsConfirmed.should_be(true);
                d.barringOrderContact = d._barringOrderService.UnconfirmObject(d.barringOrderContact, d._barringOrderDetailService, d._barringService, d._itemService, d._warehouseItemService);
                d.barringOrderContact.IsConfirmed.should_be(false);
                d.barringOrderContact.Errors.Count().should_be(0);
            };

            it["deletes_barringorder_with_processed_detail"] = () =>
            {
                d.barringOrderContact = d._barringOrderService.ConfirmObject(d.barringOrderContact, DateTime.Today, d._barringOrderDetailService, d._barringService, d._itemService, d._warehouseItemService);
                d.barringODContact1 = d._barringOrderDetailService.CutObject(d.barringODContact1, d._barringOrderService);
                d.barringODContact1 = d._barringOrderDetailService.SideSealObject(d.barringODContact1);

                d.barringOrderContact = d._barringOrderService.SoftDeleteObject(d.barringOrderContact, d._barringOrderDetailService);
                d.barringOrderContact.Errors.Count().should_not_be(0);
            };

            context["when barring order details are cut"] = () =>
            {
                before = () =>
                {
                    d.barringOrderContact = d._barringOrderService.ConfirmObject(d.barringOrderContact, DateTime.Today, d._barringOrderDetailService, d._barringService, d._itemService, d._warehouseItemService);

                    d._barringOrderDetailService.CutObject(d.barringODContact1, d._barringOrderService);
                    d._barringOrderDetailService.CutObject(d.barringODContact2, d._barringOrderService);
                    d._barringOrderDetailService.CutObject(d.barringODContact3, d._barringOrderService);
                    d._barringOrderDetailService.CutObject(d.barringODContact4, d._barringOrderService);
                };

                it["validates_barringorderdetails"] = () =>
                {
                    d.barringODContact1.Errors.Count().should_be(0);
                    d.barringODContact2.Errors.Count().should_be(0);
                    d.barringODContact3.Errors.Count().should_be(0);
                    d.barringODContact4.Errors.Count().should_be(0);
                };

                it["validates_unconfirmbarringorder"] = () =>
                {
                    d.barringOrderContact = d._barringOrderService.UnconfirmObject(d.barringOrderContact, d._barringOrderDetailService, d._barringService, d._itemService, d._warehouseItemService);
                    d.barringOrderContact.IsConfirmed.should_be(true);
                    d.barringOrderContact.Errors.Count().should_not_be(0);
                };

                context["when barring order details are all finished"] = () =>
                {
                    before = () =>
                    {
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
                    };

                    it["validates_barringorderdetails"] = () =>
                    {
                        d.barringODContact1.Errors.Count().should_be(0);
                        d.barringODContact2.Errors.Count().should_be(0);
                        d.barringODContact3.Errors.Count().should_be(0);
                        d.barringODContact4.Errors.Count().should_be(0);
                    };

                    it["validates_finishbarringorder"] = () =>
                    {
                        int blanket1quantity = d.blanket1.Quantity;
                        int blanket2quantity = d.blanket2.Quantity;
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
                        int blanket2quantityfinal = d.blanket2.Quantity;
                        int bargenericquantityfinal = d.bargeneric.Quantity;
                        int barleft1quantityfinal = d.barleft1.Quantity;
                        int barright1quantityfinal = d.barright1.Quantity;
                        blanket1quantityfinal.should_be(blanket1quantity - 2);
                        blanket2quantityfinal.should_be(blanket2quantity - 2);
                        bargenericquantityfinal.should_be(bargenericquantity - 4);
                        barleft1quantityfinal.should_be(barleft1quantity - 2);
                        barright1quantityfinal.should_be(barright1quantity - 1);
                        d.barringOrderContact.IsCompleted.should_be(true);
                        int barring1quantityfinal = d.barring1.Quantity;
                        int barring2quantityfinal = d.barring2.Quantity;
                        int barring1warehousequantityfinal = d._warehouseItemService.FindOrCreateObject(d.localWarehouse.Id, d.barring1.Id).Quantity;
                        int barring2warehousequantityfinal = d._warehouseItemService.FindOrCreateObject(d.localWarehouse.Id, d.barring2.Id).Quantity;
                        barring1quantityfinal.should_be(barring1quantity + 2);
                        barring2quantityfinal.should_be(barring2quantity + 1);
                        barring1warehousequantityfinal.should_be(barring1warehousequantity + 2);
                        barring2warehousequantityfinal.should_be(barring2warehousequantity + 1);
                        d.barringOrderContact.QuantityFinal.should_be(d.barringOrderContact.QuantityReceived - d.barringOrderContact.QuantityRejected);
                    };
                };
            };
        }
    }
}