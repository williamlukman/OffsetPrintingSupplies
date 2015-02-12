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

    public class SpecBlendingWorkOrder: nspec
    {
        DataBuilder d;
        
        void before_each()
        {
            var db = new OffsetPrintingSuppliesEntities();
            using (db)
            {
                db.DeleteAllTables();
                d = new DataBuilder();
                d.PopulateDataNoClosing();
                d.PopulateBlendingWorkOrders();
            }
        }

        void data_validation()
        {
        
            it["validates_data"] = () =>
            {
                d.itemBlending.Errors.Count().should_be(0);
                d.itemBlendingDet1.Errors.Count().should_be(0);
                d.itemBlendingDet2.Errors.Count().should_be(0);
                d.itemBlendingDet3.Errors.Count().should_be(0);
                d.blending.Errors.Count().should_be(0);
                d.blendingDet1.Errors.Count().should_be(0);
                d.blendingDet2.Errors.Count().should_be(0);
                d.blendingDet3.Errors.Count().should_be(0);
                d.blendingWorkOrder.Errors.Count().should_be(0);
            };

            it["deletes_blendingworkorder"] = () =>
            {
                d.blendingWorkOrder = d._blendingWorkOrderService.SoftDeleteObject(d.blendingWorkOrder);
                d.blendingWorkOrder.Errors.Count().should_be(0);
            };

            it["confirms_blendingworkorder"] = () =>
            {
                d.blendingWorkOrder = d._blendingWorkOrderService.ConfirmObject(d.blendingWorkOrder, DateTime.Today, d._blendingRecipeService, d._blendingRecipeDetailService, d._stockMutationService,
                                                        d._blanketService, d._itemService, d._itemTypeService, d._warehouseItemService, d._generalLedgerJournalService, d._accountService, d._closingService);
                d.blendingWorkOrder.IsConfirmed.should_be(true);
                d.blendingWorkOrder.Errors.Count().should_be(0);
                
                d.itemBlending.Quantity.should_be(d.sadBlendingItem1.Quantity + d.blending.TargetQuantity);
                d.itemBlendingDet1.Quantity.should_be(d.sadBlendingItem2.Quantity - d.blendingDet1.Quantity);
                d.itemBlendingDet2.Quantity.should_be(d.sadBlendingItem3.Quantity - d.blendingDet2.Quantity);
                d.itemBlendingDet3.Quantity.should_be(d.sadBlendingItem4.Quantity - d.blendingDet3.Quantity);
                
                d.itemBlending.AvgPrice.should_be(((d.sadBlendingItem1.Quantity * d.sadBlendingItem1.Price) + (d.blendingDet1.Quantity * d.sadBlendingItem2.Price + d.blendingDet2.Quantity * d.sadBlendingItem3.Price + d.blendingDet3.Quantity * d.sadBlendingItem4.Price)) / (d.sadBlendingItem1.Quantity + d.blending.TargetQuantity));
            };

            it["unconfirms_blendingworkorder"] = () =>
            {
                d.blendingWorkOrder = d._blendingWorkOrderService.ConfirmObject(d.blendingWorkOrder, DateTime.Today, d._blendingRecipeService, d._blendingRecipeDetailService, d._stockMutationService,
                                                        d._blanketService, d._itemService, d._itemTypeService, d._warehouseItemService, d._generalLedgerJournalService, d._accountService, d._closingService);
                d.blendingWorkOrder.IsConfirmed.should_be(true);
                d.blendingWorkOrder.Errors.Count().should_be(0);
                d.blendingWorkOrder = d._blendingWorkOrderService.UnconfirmObject(d.blendingWorkOrder, d._blendingRecipeService, d._blendingRecipeDetailService, d._stockMutationService,
                                                    d._blanketService, d._itemService, d._itemTypeService, d._warehouseItemService, d._generalLedgerJournalService, d._accountService, d._closingService);
                d.blendingWorkOrder.IsConfirmed.should_be(false);
                d.blendingWorkOrder.Errors.Count().should_be(0);

                d.itemBlending.Quantity.should_be(d.sadBlendingItem1.Quantity);
                d.itemBlendingDet1.Quantity.should_be(d.sadBlendingItem2.Quantity);
                d.itemBlendingDet2.Quantity.should_be(d.sadBlendingItem3.Quantity);
                d.itemBlendingDet3.Quantity.should_be(d.sadBlendingItem4.Quantity);

                d.itemBlending.AvgPrice.should_be(d.sadBlendingItem1.Price);
            };

            
        }
    }
}