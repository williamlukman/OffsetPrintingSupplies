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

    public class SpecRecoveryOrder: nspec
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
                d.PopulateBarring();
                d.PopulateWarehouseMutationForRollerIdentificationAndRecovery();
                d.PopulateCoreIdentifications();
                d.PopulateRecoveryOrders();
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
                d.recoveryOrderContact.Errors.Count().should_be(0);
                d.recoveryODContact1.Errors.Count().should_be(0);
                d.recoveryODContact2.Errors.Count().should_be(0);
                d.recoveryODContact3.Errors.Count().should_be(0);
                d.recoveryOrderInHouse.Errors.Count().should_be(0);
                d.recoveryODInHouse1.Errors.Count().should_be(0);
                d.recoveryODInHouse2.Errors.Count().should_be(0);
                d.recoveryODInHouse3.Errors.Count().should_be(0);
            };

            it["deletes_recovery_order"] = () =>
            {
                d.recoveryOrderContact = d._recoveryOrderService.SoftDeleteObject(d.recoveryOrderContact, d._recoveryOrderDetailService, d._recoveryAccessoryDetailService);
                d.recoveryOrderContact.Errors.Count().should_not_be(0);
            };

            it["confirms_recovery_order"] = () =>
            {
                d.recoveryOrderContact = d._recoveryOrderService.ConfirmObject(d.recoveryOrderContact, DateTime.Today, d._coreIdentificationDetailService, d._recoveryOrderDetailService,
                                                                                d._recoveryAccessoryDetailService, d._coreBuilderService, d._stockMutationService, d._itemService, d._barringService,
                                                                                d._warehouseItemService, d._warehouseService);
                d.recoveryOrderContact.IsConfirmed.should_be(true);
                d.recoveryOrderContact.Errors.Count().should_be(0);
            };

            it["unconfirms_recovery_order"] = () =>
            {
                d.recoveryOrderContact = d._recoveryOrderService.ConfirmObject(d.recoveryOrderContact, DateTime.Today, d._coreIdentificationDetailService, d._recoveryOrderDetailService,
                                                                                d._recoveryAccessoryDetailService, d._coreBuilderService, d._stockMutationService, d._itemService,
                                                                                d._barringService, d._warehouseItemService, d._warehouseService);
                d.recoveryOrderContact.IsConfirmed.should_be(true);
                d.recoveryOrderContact = d._recoveryOrderService.UnconfirmObject(d.recoveryOrderContact, d._coreIdentificationDetailService, d._recoveryOrderDetailService,
                                                                                  d._recoveryAccessoryDetailService, d._coreBuilderService, d._stockMutationService, d._itemService,
                                                                                  d._barringService, d._warehouseItemService, d._warehouseService);
                d.recoveryOrderContact.IsConfirmed.should_be(false);
                d.recoveryOrderContact.Errors.Count().should_be(0);
            };

            it["deletes_recoveryorder_with_processed_detail"] = () =>
            {
                d.recoveryOrderContact = d._recoveryOrderService.ConfirmObject(d.recoveryOrderContact, DateTime.Today, d._coreIdentificationDetailService, d._recoveryOrderDetailService,
                                                                                d._recoveryAccessoryDetailService, d._coreBuilderService, d._stockMutationService, d._itemService,
                                                                                d._barringService, d._warehouseItemService, d._warehouseService);
                d.recoveryODContact1 = d._recoveryOrderDetailService.DisassembleObject(d.recoveryODContact1, d._recoveryOrderService);
                d.recoveryODContact1 = d._recoveryOrderDetailService.StripAndGlueObject(d.recoveryODContact1);
                d.recoveryODContact1 = d._recoveryOrderDetailService.WrapObject(d.recoveryODContact1, 20, d._recoveryOrderService, d._rollerBuilderService, d._itemService, d._warehouseItemService);

                d.recoveryOrderContact = d._recoveryOrderService.SoftDeleteObject(d.recoveryOrderContact, d._recoveryOrderDetailService, d._recoveryAccessoryDetailService);
                d.recoveryOrderContact.Errors.Count().should_not_be(0);
            };

            context["when recovery order details are disassembled"] = () =>
            {
                before = () =>
                {
                    d._recoveryOrderService.ConfirmObject(d.recoveryOrderContact, DateTime.Today, d._coreIdentificationDetailService, d._recoveryOrderDetailService,
                                                          d._recoveryAccessoryDetailService, d._coreBuilderService, d._stockMutationService, d._itemService,
                                                          d._barringService, d._warehouseItemService, d._warehouseService);
                    d._recoveryOrderService.ConfirmObject(d.recoveryOrderInHouse, DateTime.Today, d._coreIdentificationDetailService, d._recoveryOrderDetailService,
                                                          d._recoveryAccessoryDetailService, d._coreBuilderService, d._stockMutationService, d._itemService,
                                                          d._barringService, d._warehouseItemService, d._warehouseService);
                    d._recoveryOrderDetailService.DisassembleObject(d.recoveryODContact1, d._recoveryOrderService);
                    d._recoveryOrderDetailService.DisassembleObject(d.recoveryODContact2, d._recoveryOrderService);
                    d._recoveryOrderDetailService.DisassembleObject(d.recoveryODContact3, d._recoveryOrderService);
                    d._recoveryOrderDetailService.DisassembleObject(d.recoveryODInHouse1, d._recoveryOrderService);
                    d._recoveryOrderDetailService.DisassembleObject(d.recoveryODInHouse2, d._recoveryOrderService);
                    d._recoveryOrderDetailService.DisassembleObject(d.recoveryODInHouse3, d._recoveryOrderService);
                };

                it["validates_recoveryorderdetails"] = () =>
                {
                    d.recoveryODContact1.Errors.Count().should_be(0);
                    d.recoveryODContact2.Errors.Count().should_be(0);
                    d.recoveryODContact3.Errors.Count().should_be(0);
                    d.recoveryODInHouse1.Errors.Count().should_be(0);
                    d.recoveryODInHouse2.Errors.Count().should_be(0);
                    d.recoveryODInHouse3.Errors.Count().should_be(0);
                };

                it["validates_unconfirmrecoveryorder"] = () =>
                {
                    d.recoveryOrderContact = d._recoveryOrderService.UnconfirmObject(d.recoveryOrderContact, d._coreIdentificationDetailService, d._recoveryOrderDetailService,
                                                                                      d._recoveryAccessoryDetailService, d._coreBuilderService, d._stockMutationService, d._itemService, d._barringService,
                                                                                      d._warehouseItemService, d._warehouseService);
                    d.recoveryOrderContact.IsConfirmed.should_be(true);
                    d.recoveryOrderContact.Errors.Count().should_not_be(0);
                };

                context["when recovery order details are all finished"] = () =>
                {
                    before = () =>
                    {
                        d._recoveryOrderDetailService.StripAndGlueObject(d.recoveryODContact1);
                        d._recoveryOrderDetailService.WrapObject(d.recoveryODContact1, 20, d._recoveryOrderService, d._rollerBuilderService, d._itemService, d._warehouseItemService);
                        d._recoveryOrderDetailService.VulcanizeObject(d.recoveryODContact1);
                        d._recoveryOrderDetailService.FaceOffObject(d.recoveryODContact1);
                        d._recoveryOrderDetailService.ConventionalGrindObject(d.recoveryODContact1);
                        d._recoveryOrderDetailService.CWCGrindObject(d.recoveryODContact1);
                        d._recoveryOrderDetailService.PolishAndQCObject(d.recoveryODContact1);
                        d._recoveryOrderDetailService.PackageObject(d.recoveryODContact1);

                        d._recoveryOrderDetailService.StripAndGlueObject(d.recoveryODContact2);
                        d._recoveryOrderDetailService.WrapObject(d.recoveryODContact2, 20, d._recoveryOrderService, d._rollerBuilderService, d._itemService, d._warehouseItemService);
                        d._recoveryOrderDetailService.VulcanizeObject(d.recoveryODContact2);
                        d._recoveryOrderDetailService.FaceOffObject(d.recoveryODContact2);
                        d._recoveryOrderDetailService.ConventionalGrindObject(d.recoveryODContact2);
                        d._recoveryOrderDetailService.CWCGrindObject(d.recoveryODContact2);
                        d._recoveryOrderDetailService.PolishAndQCObject(d.recoveryODContact2);
                        d._recoveryOrderDetailService.PackageObject(d.recoveryODContact2);

                        d._recoveryOrderDetailService.StripAndGlueObject(d.recoveryODContact3);
                        d._recoveryOrderDetailService.WrapObject(d.recoveryODContact3, 20, d._recoveryOrderService, d._rollerBuilderService, d._itemService, d._warehouseItemService);
                        d._recoveryOrderDetailService.VulcanizeObject(d.recoveryODContact3);
                        d._recoveryOrderDetailService.FaceOffObject(d.recoveryODContact3);
                        d._recoveryOrderDetailService.ConventionalGrindObject(d.recoveryODContact3);
                        d._recoveryOrderDetailService.CWCGrindObject(d.recoveryODContact3);
                        d._recoveryOrderDetailService.PolishAndQCObject(d.recoveryODContact3);
                        d._recoveryOrderDetailService.PackageObject(d.recoveryODContact3);

                        d._recoveryOrderDetailService.StripAndGlueObject(d.recoveryODInHouse1);
                        d._recoveryOrderDetailService.WrapObject(d.recoveryODInHouse1, 20, d._recoveryOrderService, d._rollerBuilderService, d._itemService, d._warehouseItemService);
                        d._recoveryOrderDetailService.VulcanizeObject(d.recoveryODInHouse1);
                        d._recoveryOrderDetailService.FaceOffObject(d.recoveryODInHouse1);
                        d._recoveryOrderDetailService.ConventionalGrindObject(d.recoveryODInHouse1);
                        d._recoveryOrderDetailService.CWCGrindObject(d.recoveryODInHouse1);
                        d._recoveryOrderDetailService.PolishAndQCObject(d.recoveryODInHouse1);
                        d._recoveryOrderDetailService.PackageObject(d.recoveryODInHouse1);
                        
                        d.accessory1 = new RecoveryAccessoryDetail()
                        {
                            ItemId = d.itemAccessory1.Id,
                            Quantity = 1,
                            RecoveryOrderDetailId = d.recoveryODInHouse2.Id
                        };
                        d._recoveryAccessoryDetailService.CreateObject(d.accessory1, d._recoveryOrderService, d._recoveryOrderDetailService,
                                                                       d._itemService, d._itemTypeService, d._warehouseItemService);
                        
                        d._recoveryOrderDetailService.StripAndGlueObject(d.recoveryODInHouse2);
                        d._recoveryOrderDetailService.WrapObject(d.recoveryODInHouse2, 20, d._recoveryOrderService, d._rollerBuilderService, d._itemService, d._warehouseItemService);
                        d._recoveryOrderDetailService.VulcanizeObject(d.recoveryODInHouse2);
                        d._recoveryOrderDetailService.FaceOffObject(d.recoveryODInHouse2);
                        d._recoveryOrderDetailService.ConventionalGrindObject(d.recoveryODInHouse2);
                        d._recoveryOrderDetailService.CWCGrindObject(d.recoveryODInHouse2);
                        d._recoveryOrderDetailService.PolishAndQCObject(d.recoveryODInHouse2);
                        d._recoveryOrderDetailService.PackageObject(d.recoveryODInHouse2);

                    };

                    it["validates_recoveryorderdetails"] = () =>
                    {
                        d.recoveryODContact1.Errors.Count().should_be(0);
                        d.recoveryODContact2.Errors.Count().should_be(0);
                        d.recoveryODContact3.Errors.Count().should_be(0);
                        d.recoveryODInHouse1.Errors.Count().should_be(0);
                        d.recoveryODInHouse2.Errors.Count().should_be(0);
                        d.recoveryODInHouse3.Errors.Count().should_be(0);
                        d.accessory1.Errors.Count().should_be(0);
                    };

                    context["validates_finishdetails"] = () =>
                    {
                        before = () =>
                        {
                            d.usedCoreBuilder1Quantity = d._recoveryOrderDetailService.GetCore(d.recoveryODInHouse1, d._coreIdentificationDetailService, d._coreBuilderService, d._itemService).Quantity;
                            d.usedCoreBuilder2Quantity = d._recoveryOrderDetailService.GetCore(d.recoveryODInHouse3, d._coreIdentificationDetailService, d._coreBuilderService, d._itemService).Quantity;
                            d.usedCoreBuilder3Quantity = d._recoveryOrderDetailService.GetCore(d.recoveryODContact1, d._coreIdentificationDetailService, d._coreBuilderService, d._itemService).Quantity;
                            d.usedCoreBuilder4Quantity = d._recoveryOrderDetailService.GetCore(d.recoveryODContact3, d._coreIdentificationDetailService, d._coreBuilderService, d._itemService).Quantity;
                            d.usedRollerBuilder1Quantity = d._recoveryOrderDetailService.GetRoller(d.recoveryODInHouse1, d._coreIdentificationDetailService, d._rollerBuilderService, d._itemService).Quantity;
                            d.usedRollerBuilder2Quantity = d._recoveryOrderDetailService.GetRoller(d.recoveryODInHouse3, d._coreIdentificationDetailService, d._rollerBuilderService, d._itemService).Quantity;
                            d.usedRollerBuilder3Quantity = d._recoveryOrderDetailService.GetRoller(d.recoveryODContact1, d._coreIdentificationDetailService, d._rollerBuilderService, d._itemService).Quantity;
                            d.usedRollerBuilder4Quantity = d._recoveryOrderDetailService.GetRoller(d.recoveryODContact3, d._coreIdentificationDetailService, d._rollerBuilderService, d._itemService).Quantity;
                            d.accessory1quantity = d._itemService.GetObjectById(d.itemAccessory1.Id).Quantity;
                            
                            d._recoveryOrderDetailService.FinishObject(d.recoveryODContact1, DateTime.Today, d._coreIdentificationService, d._coreIdentificationDetailService,
                                                   d._recoveryOrderService, d._recoveryAccessoryDetailService, d._coreBuilderService, d._rollerBuilderService,
                                                   d._itemService, d._warehouseItemService, d._barringService, d._stockMutationService);
                            d._recoveryOrderDetailService.RejectObject(d.recoveryODContact2, DateTime.Today, d._coreIdentificationService, d._coreIdentificationDetailService, d._recoveryOrderService,
                                                   d._recoveryAccessoryDetailService, d._coreBuilderService, d._rollerBuilderService, d._itemService,
                                                   d._warehouseItemService, d._barringService, d._stockMutationService);
                            d._recoveryOrderDetailService.FinishObject(d.recoveryODContact3, DateTime.Today, d._coreIdentificationService, d._coreIdentificationDetailService,
                                                   d._recoveryOrderService, d._recoveryAccessoryDetailService, d._coreBuilderService, d._rollerBuilderService,
                                                   d._itemService, d._warehouseItemService, d._barringService, d._stockMutationService);
                            d._recoveryOrderDetailService.FinishObject(d.recoveryODInHouse1, DateTime.Today, d._coreIdentificationService, d._coreIdentificationDetailService,
                                                                       d._recoveryOrderService, d._recoveryAccessoryDetailService, d._coreBuilderService, d._rollerBuilderService,
                                                                       d._itemService, d._warehouseItemService, d._barringService, d._stockMutationService);
                            d._recoveryOrderDetailService.FinishObject(d.recoveryODInHouse2, DateTime.Today, d._coreIdentificationService, d._coreIdentificationDetailService,
                                                                       d._recoveryOrderService, d._recoveryAccessoryDetailService, d._coreBuilderService, d._rollerBuilderService,
                                                                       d._itemService, d._warehouseItemService, d._barringService, d._stockMutationService);
                            d._recoveryOrderDetailService.RejectObject(d.recoveryODInHouse3, DateTime.Today, d._coreIdentificationService, d._coreIdentificationDetailService, d._recoveryOrderService,
                                                                       d._recoveryAccessoryDetailService, d._coreBuilderService, d._rollerBuilderService, d._itemService,
                                                                       d._warehouseItemService, d._barringService, d._stockMutationService);

                            d.usedCoreBuilder1Final = d._recoveryOrderDetailService.GetCore(d.recoveryODInHouse1, d._coreIdentificationDetailService, d._coreBuilderService, d._itemService).Quantity;
                            d.usedCoreBuilder2Final = d._recoveryOrderDetailService.GetCore(d.recoveryODInHouse3, d._coreIdentificationDetailService, d._coreBuilderService, d._itemService).Quantity;
                            d.usedCoreBuilder3Final = d._recoveryOrderDetailService.GetCore(d.recoveryODContact1, d._coreIdentificationDetailService, d._coreBuilderService, d._itemService).Quantity;
                            d.usedCoreBuilder4Final = d._recoveryOrderDetailService.GetCore(d.recoveryODContact3, d._coreIdentificationDetailService, d._coreBuilderService, d._itemService).Quantity;
                            d.usedRollerBuilder1Final = d._recoveryOrderDetailService.GetRoller(d.recoveryODInHouse1, d._coreIdentificationDetailService, d._rollerBuilderService, d._itemService).Quantity;
                            d.usedRollerBuilder2Final = d._recoveryOrderDetailService.GetRoller(d.recoveryODInHouse3, d._coreIdentificationDetailService, d._rollerBuilderService, d._itemService).Quantity;
                            d.usedRollerBuilder3Final = d._recoveryOrderDetailService.GetRoller(d.recoveryODContact1, d._coreIdentificationDetailService, d._rollerBuilderService, d._itemService).Quantity;
                            d.usedRollerBuilder4Final = d._recoveryOrderDetailService.GetRoller(d.recoveryODContact3, d._coreIdentificationDetailService, d._rollerBuilderService, d._itemService).Quantity;
                        };
                        
                        it["validates_finish"] = () =>
                        {
                            d.usedCoreBuilder1Final.should_be(d.usedCoreBuilder1Quantity - 2);
                            d.usedRollerBuilder1Final.should_be(d.usedRollerBuilder1Quantity + 2);
                            d.usedCoreBuilder2Final.should_be(d.usedCoreBuilder2Quantity - 1);
                            d.usedRollerBuilder2Final.should_be(d.usedRollerBuilder2Quantity); // ODInHouse3 rejected
                            d.usedCoreBuilder3Final.should_be(d.usedCoreBuilder3Quantity - 2);
                            d.usedRollerBuilder3Final.should_be(d.usedRollerBuilder3Quantity + 1); // ODContact2 rejected
                            d.usedCoreBuilder4Final.should_be(d.usedCoreBuilder4Quantity - 1);
                            d.usedRollerBuilder4Final.should_be(d.usedRollerBuilder4Quantity + 1);
                            d.accessory1quantity.should_be(d._itemService.GetObjectById(d.itemAccessory1.Id).Quantity + 1);
                        };

                        it["validates_completerecoveryorder"] = () =>
                        {
                            d.recoveryOrderContact.IsCompleted.should_be(true);
                            d.recoveryOrderContact.Errors.Count().should_be(0);
                        };
                    };
                };
            };
        }
    }
}