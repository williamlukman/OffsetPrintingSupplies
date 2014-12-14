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

    public class SpecWarehouseMutation: nspec
    {
        DataBuilder d;
        
        void before_each()
        {
            var db = new OffsetPrintingSuppliesEntities();
            using (db)
            {
                db.DeleteAllTables();
                d = new DataBuilder();
                d.PopulateWarehouse();
                d.PopulateItem();
                d.PopulateSingles();
                d.PopulateBuilders();
                d.PopulateBlanket();
                d.PopulateWarehouseMutationForRollerIdentificationAndRecovery();
            }
        }

        void data_validation()
        {
        
            it["validates_data"] = () =>
            {
                d.warehouseMutation.Errors.Count().should_be(0);
                d.wmoDetail1.Errors.Count().should_be(0);
                d.wmoDetail2.Errors.Count().should_be(0);
                d.wmoDetail3.Errors.Count().should_be(0);
                d.wmoDetail4.Errors.Count().should_be(0);
                d.wmoDetail5.Errors.Count().should_be(0);
                d.wmoDetail6.Errors.Count().should_be(0);
            };

            it["deletes_WarehouseMutation"] = () =>
            {
                d.warehouseMutation = d._warehouseMutationService.SoftDeleteObject(d.warehouseMutation);
                d.warehouseMutation.Errors.Count().should_be(0);
            };

            it["deletes_WarehouseMutationdetail"] = () =>
            {
                d.wmoDetail1 = d._warehouseMutationDetailService.SoftDeleteObject(d.wmoDetail1, d._warehouseMutationService, d._warehouseItemService);
                d.wmoDetail1.Errors.Count().should_be(0);
            };

            it["confirms_WarehouseMutation"] = () =>
            {
                d.warehouseMutation = d._warehouseMutationService.ConfirmObject(d.warehouseMutation, DateTime.Now, d._warehouseMutationDetailService, d._itemService, d._blanketService, d._warehouseItemService, d._stockMutationService);
                d.warehouseMutation.IsConfirmed.should_be(true);
                d.warehouseMutation.Errors.Count().should_be(0);
            };

            it["unconfirms_WarehouseMutation"] = () =>
            {
                d.warehouseMutation = d._warehouseMutationService.ConfirmObject(d.warehouseMutation, DateTime.Now, d._warehouseMutationDetailService, d._itemService, d._blanketService, d._warehouseItemService, d._stockMutationService);
                d.warehouseMutation.IsConfirmed.should_be(true);
                d.warehouseMutation = d._warehouseMutationService.UnconfirmObject(d.warehouseMutation, d._warehouseMutationDetailService, d._itemService, d._blanketService, d._warehouseItemService, d._stockMutationService);
                d.warehouseMutation.IsConfirmed.should_be(false);
                d.warehouseMutation.Errors.Count().should_be(0);
            };

            it["finishes_detail"] = () =>
            {
                decimal quantitywarehousefrom = d._warehouseItemService.FindOrCreateObject(d.warehouseMutation.WarehouseFromId, d.wmoDetail1.ItemId).Quantity;
                decimal quantitywarehouseto = d._warehouseItemService.FindOrCreateObject(d.warehouseMutation.WarehouseToId, d.wmoDetail1.ItemId).Quantity;
                d.warehouseMutation = d._warehouseMutationService.ConfirmObject(d.warehouseMutation, DateTime.Today, d._warehouseMutationDetailService, d._itemService,
                                                                                          d._blanketService, d._warehouseItemService, d._stockMutationService);
                d.wmoDetail1.IsConfirmed.should_be(true);
                decimal quantitywarehousefromfinal = d._warehouseItemService.FindOrCreateObject(d.warehouseMutation.WarehouseFromId, d.wmoDetail1.ItemId).Quantity;
                decimal quantitywarehousetofinal = d._warehouseItemService.FindOrCreateObject(d.warehouseMutation.WarehouseToId, d.wmoDetail1.ItemId).Quantity;
                d.wmoDetail1.Errors.Count().should_be(0);
                quantitywarehousefromfinal.should_be(quantitywarehousefrom - d.wmoDetail1.Quantity);
                quantitywarehousetofinal.should_be(quantitywarehouseto + d.wmoDetail1.Quantity);
            };

            it["unconfirm_whendetailisfinished"] = () =>
            {
                d.warehouseMutation = d._warehouseMutationService.ConfirmObject(d.warehouseMutation, DateTime.Today, d._warehouseMutationDetailService, d._itemService,
                                                                                          d._blanketService, d._warehouseItemService, d._stockMutationService);
                d.warehouseMutation.IsConfirmed.should_be(true);
                d.warehouseMutation = d._warehouseMutationService.UnconfirmObject(d.warehouseMutation, d._warehouseMutationDetailService, d._itemService, d._blanketService,
                                                                                            d._warehouseItemService, d._stockMutationService);
                d.warehouseMutation.IsConfirmed.should_be(false);
                d.warehouseMutation.Errors.Count().should_be(0);
            };

            it["deletes_whendetailisfinished"] = () =>
            {
                d.warehouseMutation = d._warehouseMutationService.ConfirmObject(d.warehouseMutation, DateTime.Today, d._warehouseMutationDetailService, d._itemService, d._blanketService, d._warehouseItemService, d._stockMutationService);
                d.warehouseMutation = d._warehouseMutationService.SoftDeleteObject(d.warehouseMutation);
                d.warehouseMutation.IsDeleted.should_be(false);
                d.warehouseMutation.Errors.Count().should_not_be(0);
            };

            context["confirm_orderanddetail"] = () =>
            {
                before = () =>
                {
                    d.warehouseMutation = d._warehouseMutationService.ConfirmObject(d.warehouseMutation, DateTime.Today, d._warehouseMutationDetailService, d._itemService,
                                                                                              d._blanketService, d._warehouseItemService, d._stockMutationService);
                };

                it["validates_confirmeddetails"] = () =>
                {
                    d.warehouseMutation.IsConfirmed.should_be(true);
                    d.warehouseMutation.Errors.Count().should_be(0);
                };
            };
        }
    }
}