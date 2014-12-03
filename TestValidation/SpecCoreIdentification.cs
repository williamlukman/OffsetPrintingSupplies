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

    public class SpecCoreIdentification: nspec
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
                d.PopulateCoreIdentifications();
            }
        }

        void data_validation()
        {
        
            it["validates_data"] = () =>
            {
                d.itemCompound.Errors.Count().should_be(0);
                d.itemCompound1.Errors.Count().should_be(0);
                d.itemCompound2.Errors.Count().should_be(0);
                d.itemAccessory1.Errors.Count().should_be(0);
                d.itemAccessory2.Errors.Count().should_be(0);
                d.contact.Errors.Count().should_be(0);
                d.machine.Errors.Count().should_be(0);
                d.coreBuilder.Errors.Count().should_be(0);
                d.coreBuilder1.Errors.Count().should_be(0);
                d.coreBuilder2.Errors.Count().should_be(0);
                d.coreBuilder3.Errors.Count().should_be(0);
                d.coreBuilder4.Errors.Count().should_be(0);
                d.coreIdentification.Errors.Count().should_be(0);
                d.coreIdentificationContact.Errors.Count().should_be(0);
                d.coreIDContact1.Errors.Count().should_be(0);
                d.coreIDContact2.Errors.Count().should_be(0);
                d.coreIDContact3.Errors.Count().should_be(0);
                d.coreIdentificationDetail.Errors.Count().should_be(0);
                d.coreIdentificationInHouse.Errors.Count().should_be(0);
                d.coreIDInHouse1.Errors.Count().should_be(0);
                d.coreIDInHouse2.Errors.Count().should_be(0);
                d.coreIDInHouse3.Errors.Count().should_be(0);
            };

            it["deletes_coreidentification"] = () =>
            {
                d.coreIdentificationContact = d._coreIdentificationService.SoftDeleteObject(d.coreIdentificationContact, d._coreIdentificationDetailService, d._recoveryOrderService);
                d.coreIdentificationContact.Errors.Count().should_be(0);
            };

            it["confirms_coreidentificationcontact"] = () =>
            {
                d.coreIdentificationContact = d._coreIdentificationService.ConfirmObject(d.coreIdentificationContact, DateTime.Today, d._coreIdentificationDetailService, d._stockMutationService, d._recoveryOrderService,
                                               d._recoveryOrderDetailService, d._coreBuilderService, d._itemService, d._warehouseItemService, d._blanketService, d._customerStockMutationService, d._customerItemService);
                d.coreIdentificationContact.IsConfirmed.should_be(true);
                d.coreIdentificationContact.Errors.Count().should_be(0);
            };

            it["unconfirms_coreidentificationcontact"] = () =>
            {
                d.coreIdentificationContact = d._coreIdentificationService.ConfirmObject(d.coreIdentificationContact, DateTime.Today, d._coreIdentificationDetailService, d._stockMutationService,
                                               d._recoveryOrderService, d._recoveryOrderDetailService, d._coreBuilderService, d._itemService, d._warehouseItemService, d._blanketService, d._customerStockMutationService, d._customerItemService);
                d.coreIdentificationContact.IsConfirmed.should_be(true);
                d.coreIdentificationContact = d._coreIdentificationService.UnconfirmObject(d.coreIdentificationContact, d._coreIdentificationDetailService, d._stockMutationService,
                                               d._recoveryOrderService, d._coreBuilderService, d._itemService, d._warehouseItemService, d._blanketService, d._customerStockMutationService, d._customerItemService);
                d.coreIdentificationContact.IsConfirmed.should_be(false);
                d.coreIdentificationContact.Errors.Count().should_be(0);
            };

            it["confirms_coreidentificationinhouse"] = () =>
            {
                d.coreIdentificationInHouse = d._coreIdentificationService.ConfirmObject(d.coreIdentificationInHouse, DateTime.Today, d._coreIdentificationDetailService, d._stockMutationService,
                                                                                         d._recoveryOrderService, d._recoveryOrderDetailService, d._coreBuilderService, d._itemService,
                                                                                         d._warehouseItemService, d._blanketService, d._customerStockMutationService, d._customerItemService);
                d.coreIdentificationInHouse.IsConfirmed.should_be(true);
                d.coreIdentificationInHouse.Errors.Count().should_be(0);
            };

            it["unconfirms_coreidentificationinhouse"] = () =>
            {
                d.coreIdentificationInHouse = d._coreIdentificationService.ConfirmObject(d.coreIdentificationInHouse, DateTime.Today, d._coreIdentificationDetailService, d._stockMutationService,
                                                                                         d._recoveryOrderService, d._recoveryOrderDetailService, d._coreBuilderService, d._itemService,
                                                                                         d._warehouseItemService, d._blanketService, d._customerStockMutationService, d._customerItemService);
                d.coreIdentificationInHouse.IsConfirmed.should_be(true);
                d.coreIdentificationInHouse = d._coreIdentificationService.UnconfirmObject(d.coreIdentificationInHouse, d._coreIdentificationDetailService, d._stockMutationService,
                                                                                           d._recoveryOrderService, d._coreBuilderService, d._itemService, d._warehouseItemService,
                                                                                           d._blanketService, d._customerStockMutationService, d._customerItemService);
                d.coreIdentificationInHouse.IsConfirmed.should_be(false);
                d.coreIdentificationInHouse.Errors.Count().should_be(0);
            };

            it["deletes_coreidentificationcontact_with_recoveryorder"] = () =>
            {
                d.PopulateRecoveryOrders();
                d.coreIdentificationContact = d._coreIdentificationService.SoftDeleteObject(d.coreIdentificationContact, d._coreIdentificationDetailService, d._recoveryOrderService);
                d.coreIdentificationContact.Errors.Count().should_not_be(0);
            };

            context["finishes_detail"] = () =>
            {
                before = () =>
                {
                    // contact core identification

                    Item usedCoreBuilder, usedCoreBuilder1, usedCoreBuilder2, usedCoreBuilder3, usedCoreBuilder4;
                    usedCoreBuilder = d._coreIdentificationDetailService.GetCore(d.coreIdentificationDetail, d._coreBuilderService);
                    usedCoreBuilder3 = d._coreIdentificationDetailService.GetCore(d.coreIDContact1, d._coreBuilderService);
                    usedCoreBuilder4 = d._coreIdentificationDetailService.GetCore(d.coreIDContact3, d._coreBuilderService);
                    usedCoreBuilder1 = d._coreIdentificationDetailService.GetCore(d.coreIDInHouse1, d._coreBuilderService);
                    usedCoreBuilder2 = d._coreIdentificationDetailService.GetCore(d.coreIDInHouse3, d._coreBuilderService);

                    d.usedCoreBuilderQuantity = usedCoreBuilder.Quantity + usedCoreBuilder.CustomerQuantity;
                    d.usedCoreBuilder3Quantity = usedCoreBuilder3.Quantity + usedCoreBuilder3.CustomerQuantity;
                    d.usedCoreBuilder4Quantity = usedCoreBuilder4.Quantity + usedCoreBuilder4.CustomerQuantity;
                    d.usedCoreBuilder1Quantity = usedCoreBuilder1.Quantity + usedCoreBuilder1.CustomerQuantity;
                    d.usedCoreBuilder2Quantity = usedCoreBuilder2.Quantity + usedCoreBuilder2.CustomerQuantity;

                    d.coreIdentification = d._coreIdentificationService.ConfirmObject(d.coreIdentification, DateTime.Today, d._coreIdentificationDetailService, d._stockMutationService,
                                                                         d._recoveryOrderService, d._recoveryOrderDetailService, d._coreBuilderService, d._itemService,
                                                                         d._warehouseItemService, d._blanketService, d._customerStockMutationService, d._customerItemService);
                    d.coreIdentificationContact = d._coreIdentificationService.ConfirmObject(d.coreIdentificationContact, DateTime.Today, d._coreIdentificationDetailService, d._stockMutationService,
                                                                         d._recoveryOrderService, d._recoveryOrderDetailService, d._coreBuilderService, d._itemService,
                                                                         d._warehouseItemService, d._blanketService, d._customerStockMutationService, d._customerItemService);
                    d.coreIdentificationInHouse = d._coreIdentificationService.ConfirmObject(d.coreIdentificationInHouse, DateTime.Today, d._coreIdentificationDetailService, d._stockMutationService,
                                                                         d._recoveryOrderService, d._recoveryOrderDetailService, d._coreBuilderService, d._itemService,
                                                                         d._warehouseItemService, d._blanketService, d._customerStockMutationService, d._customerItemService);

                    Item usedCoreBuilderFinal, usedCoreBuilder1Final, usedCoreBuilder2Final, usedCoreBuilder3Final, usedCoreBuilder4Final;
                    usedCoreBuilderFinal = d._coreIdentificationDetailService.GetCore(d.coreIdentificationDetail, d._coreBuilderService);
                    usedCoreBuilder3Final = d._coreIdentificationDetailService.GetCore(d.coreIDContact1, d._coreBuilderService);
                    usedCoreBuilder4Final = d._coreIdentificationDetailService.GetCore(d.coreIDContact3, d._coreBuilderService);
                    usedCoreBuilder1Final = d._coreIdentificationDetailService.GetCore(d.coreIDInHouse1, d._coreBuilderService);
                    usedCoreBuilder2Final = d._coreIdentificationDetailService.GetCore(d.coreIDInHouse3, d._coreBuilderService);

                    // customer core identification
                    d.usedCoreBuilderFinal = usedCoreBuilderFinal.Quantity + usedCoreBuilderFinal.CustomerQuantity;
                    d.usedCoreBuilder3Final = usedCoreBuilder3Final.Quantity + usedCoreBuilder3Final.CustomerQuantity;
                    d.usedCoreBuilder4Final = usedCoreBuilder4Final.Quantity + usedCoreBuilder4Final.CustomerQuantity;

                    // in house core identification
                    d.usedCoreBuilder1Final = usedCoreBuilder1Final.Quantity + usedCoreBuilder1Final.CustomerQuantity;
                    d.usedCoreBuilder2Final = usedCoreBuilder2Final.Quantity + usedCoreBuilder2Final.CustomerQuantity;
                };

                it["validates_finish_details"] = () =>
                {
                    d.usedCoreBuilderFinal.should_be(d.usedCoreBuilderQuantity + 1);
                    d.usedCoreBuilder3Final.should_be(d.usedCoreBuilder3Quantity + 2);
                    d.usedCoreBuilder4Final.should_be(d.usedCoreBuilder4Quantity + 1);
                    d.usedCoreBuilder1Final.should_be(d.usedCoreBuilder1Quantity);
                    d.usedCoreBuilder2Final.should_be(d.usedCoreBuilder2Quantity);
                };

                it["validates_nondelivereddetails"] = () =>
                {
                    d.coreIdentification.IsCompleted.should_be(false);
                    d.coreIdentificationContact.IsCompleted.should_be(false);
                    d.coreIdentificationInHouse.IsCompleted.should_be(false);
                };
            };
        }
    }
}