using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                d.PopulateData();
            }
        }

        /*
         * STEPS:
         * 1. Create valid d.item
         * 2. Create invalid d.item with no name
         * 3. Create invalid items with same SKU
         * 4a. Delete d.item
         * 4b. Delete d.item with stock mutations
         */
        void data_validation()
        {
        
            it["validates_data"] = () =>
            {
                d.itemCompound.Errors.Count().should_be(0);
                d.itemCompound1.Errors.Count().should_be(0);
                d.itemCompound2.Errors.Count().should_be(0);
                d.itemAccessory1.Errors.Count().should_be(0);
                d.itemAccessory2.Errors.Count().should_be(0);
                d.customer.Errors.Count().should_be(0);
                d.machine.Errors.Count().should_be(0);
                d.coreBuilder.Errors.Count().should_be(0);
                d.coreBuilder1.Errors.Count().should_be(0);
                d.coreBuilder2.Errors.Count().should_be(0);
                d.coreBuilder3.Errors.Count().should_be(0);
                d.coreBuilder4.Errors.Count().should_be(0);
                d.coreIdentification.Errors.Count().should_be(0);
                d.coreIdentificationCustomer.Errors.Count().should_be(0);
                d.coreIDCustomer1.Errors.Count().should_be(0);
                d.coreIDCustomer2.Errors.Count().should_be(0);
                d.coreIDCustomer3.Errors.Count().should_be(0);
                d.coreIdentificationDetail.Errors.Count().should_be(0);
                d.coreIdentificationInHouse.Errors.Count().should_be(0);
                d.coreIDInHouse1.Errors.Count().should_be(0);
                d.coreIDInHouse2.Errors.Count().should_be(0);
                d.coreIDInHouse3.Errors.Count().should_be(0);
                d.recoveryOrderCustomer.Errors.Count().should_be(0);
                d.recoveryODCustomer1.Errors.Count().should_be(0);
                d.recoveryODCustomer2.Errors.Count().should_be(0);
                d.recoveryODCustomer3.Errors.Count().should_be(0);
                d.recoveryOrderInHouse.Errors.Count().should_be(0);
                d.recoveryODInHouse1.Errors.Count().should_be(0);
                d.recoveryODInHouse2.Errors.Count().should_be(0);
                d.recoveryODInHouse3.Errors.Count().should_be(0);
            };

            it["deletes_recovery_order"] = () =>
            {
                d.recoveryOrderCustomer = d._recoveryOrderService.SoftDeleteObject(d.recoveryOrderCustomer, d._recoveryOrderDetailService, d._recoveryAccessoryDetailService);
                d.recoveryOrderCustomer.Errors.Count().should_be(0);
            };

            it["confirms_recovery_order"] = () =>
            {
                int usedCoreBuilder3Quantity = d._recoveryOrderDetailService.GetCore(d.recoveryODCustomer1, d._coreIdentificationDetailService, d._coreBuilderService, d._itemService).Quantity;
                int usedCoreBuilder4Quantity = d._recoveryOrderDetailService.GetCore(d.recoveryODCustomer3, d._coreIdentificationDetailService, d._coreBuilderService, d._itemService).Quantity;
                d.recoveryOrderCustomer = d._recoveryOrderService.ConfirmObject(d.recoveryOrderCustomer, d._coreIdentificationDetailService, d._recoveryOrderDetailService,
                                                                                d._recoveryAccessoryDetailService, d._coreBuilderService, d._itemService, d._warehouseItemService);
                d.recoveryOrderCustomer.IsConfirmed.should_be(true);
                d.recoveryOrderCustomer.Errors.Count().should_be(0);
                int usedCoreBuilder3AfterConfirmed = d._recoveryOrderDetailService.GetCore(d.recoveryODCustomer1, d._coreIdentificationDetailService, d._coreBuilderService, d._itemService).Quantity;
                int usedCoreBuilder4AfterConfirmed = d._recoveryOrderDetailService.GetCore(d.recoveryODCustomer3, d._coreIdentificationDetailService, d._coreBuilderService, d._itemService).Quantity;
                usedCoreBuilder3AfterConfirmed.should_be(usedCoreBuilder3Quantity - 2);
                usedCoreBuilder4AfterConfirmed.should_be(usedCoreBuilder4Quantity - 1);
            };

            it["finishes_recovery_order"] = () =>
            {
                d.recoveryOrderCustomer = d._recoveryOrderService.ConfirmObject(d.recoveryOrderCustomer, d._coreIdentificationDetailService, d._recoveryOrderDetailService,
                                                                                d._recoveryAccessoryDetailService, d._coreBuilderService, d._itemService, d._warehouseItemService);
                d.recoveryOrderCustomer = d._recoveryOrderService.FinishObject(d.recoveryOrderCustomer, d._coreIdentificationDetailService, d._recoveryOrderDetailService,
                                                                               d._recoveryAccessoryDetailService, d._coreBuilderService, d._rollerBuilderService, d._itemService, d._warehouseItemService);
                d.recoveryOrderCustomer.Errors.Count().should_not_be(0);
            };

            it["unconfirms_recovery_order"] = () =>
            {
                d.recoveryOrderCustomer = d._recoveryOrderService.ConfirmObject(d.recoveryOrderCustomer, d._coreIdentificationDetailService, d._recoveryOrderDetailService,
                                                                                d._recoveryAccessoryDetailService, d._coreBuilderService, d._itemService, d._warehouseItemService);
                d.recoveryOrderCustomer.IsConfirmed.should_be(true);
                d.recoveryOrderCustomer = d._recoveryOrderService.UnconfirmObject(d.recoveryOrderCustomer, d._coreIdentificationDetailService, d._recoveryOrderDetailService,
                                                                                d._recoveryAccessoryDetailService, d._coreBuilderService, d._itemService, d._warehouseItemService);
                d.recoveryOrderCustomer.IsConfirmed.should_be(false);
                d.recoveryOrderCustomer.Errors.Count().should_be(0);
            };

            it["deletes_recoveryorder_with_processed_detail"] = () =>
            {
                d.recoveryOrderCustomer = d._recoveryOrderService.ConfirmObject(d.recoveryOrderCustomer, d._coreIdentificationDetailService, d._recoveryOrderDetailService,
                                                                                d._recoveryAccessoryDetailService, d._coreBuilderService, d._itemService, d._warehouseItemService);
                d.recoveryODCustomer1 = d._recoveryOrderDetailService.DisassembleObject(d.recoveryODCustomer1);
                d.recoveryODCustomer1 = d._recoveryOrderDetailService.StripAndGlueObject(d.recoveryODCustomer1);
                d.recoveryODCustomer1 = d._recoveryOrderDetailService.WrapObject(d.recoveryODCustomer1);

                d.recoveryOrderCustomer = d._recoveryOrderService.SoftDeleteObject(d.recoveryOrderCustomer, d._recoveryOrderDetailService, d._recoveryAccessoryDetailService);
                d.recoveryOrderCustomer.Errors.Count().should_not_be(0);
            };

            context["when recovery order details are disassembled"] = () =>
            {
                before = () =>
                {
                    d._recoveryOrderService.ConfirmObject(d.recoveryOrderCustomer, d._coreIdentificationDetailService, d._recoveryOrderDetailService,
                                                          d._recoveryAccessoryDetailService, d._coreBuilderService, d._itemService, d._warehouseItemService);

                    d._recoveryOrderDetailService.DisassembleObject(d.recoveryODCustomer1);
                    d._recoveryOrderDetailService.DisassembleObject(d.recoveryODCustomer2);
                    d._recoveryOrderDetailService.DisassembleObject(d.recoveryODCustomer3);
                    d._recoveryOrderDetailService.DisassembleObject(d.recoveryODInHouse1);
                    d._recoveryOrderDetailService.DisassembleObject(d.recoveryODInHouse2);
                    d._recoveryOrderDetailService.DisassembleObject(d.recoveryODInHouse3);
                    d._recoveryOrderDetailService.RejectObject(d.recoveryODInHouse3, d._recoveryOrderService);
                };

                it["validates_recoveryorderdetails"] = () =>
                {
                    d.recoveryODCustomer1.Errors.Count().should_be(0);
                    d.recoveryODCustomer2.Errors.Count().should_be(0);
                    d.recoveryODCustomer3.Errors.Count().should_be(0);
                    d.recoveryODInHouse1.Errors.Count().should_be(0);
                    d.recoveryODInHouse2.Errors.Count().should_be(0);
                    d.recoveryODInHouse3.Errors.Count().should_be(0);
                };

                it["validates_unconfirmrecoveryorder"] = () =>
                {
                    d.recoveryOrderCustomer = d._recoveryOrderService.UnconfirmObject(d.recoveryOrderCustomer, d._coreIdentificationDetailService, d._recoveryOrderDetailService,
                                                                                    d._recoveryAccessoryDetailService, d._coreBuilderService, d._itemService, d._warehouseItemService);
                    d.recoveryOrderCustomer.IsConfirmed.should_be(true);
                    d.recoveryOrderCustomer.Errors.Count().should_not_be(0);
                };

                it["validates_finishrecoveryorder"] = () =>
                {
                    d.recoveryOrderCustomer = d._recoveryOrderService.FinishObject(d.recoveryOrderCustomer, d._coreIdentificationDetailService, d._recoveryOrderDetailService,
                                                                                   d._recoveryAccessoryDetailService, d._coreBuilderService, d._rollerBuilderService, d._itemService, d._warehouseItemService);
                    d.recoveryOrderCustomer.IsFinished.should_be(false);
                    d.recoveryOrderCustomer.Errors.Count().should_not_be(0);
                };

                context["when recovery order details are all finished"] = () =>
                {
                    before = () =>
                    {
                        d._recoveryOrderDetailService.StripAndGlueObject(d.recoveryODCustomer1);
                        d._recoveryOrderDetailService.WrapObject(d.recoveryODCustomer1);
                        d._recoveryOrderDetailService.VulcanizeObject(d.recoveryODCustomer1);
                        d._recoveryOrderDetailService.FaceOffObject(d.recoveryODCustomer1);
                        d._recoveryOrderDetailService.ConventionalGrindObject(d.recoveryODCustomer1);
                        d._recoveryOrderDetailService.CWCGrindObject(d.recoveryODCustomer1);
                        d._recoveryOrderDetailService.PolishAndQCObject(d.recoveryODCustomer1);
                        d._recoveryOrderDetailService.PackageObject(d.recoveryODCustomer1);

                        d._recoveryOrderDetailService.StripAndGlueObject(d.recoveryODCustomer2);
                        d._recoveryOrderDetailService.WrapObject(d.recoveryODCustomer2);
                        d._recoveryOrderDetailService.VulcanizeObject(d.recoveryODCustomer2);
                        d._recoveryOrderDetailService.FaceOffObject(d.recoveryODCustomer2);
                        d._recoveryOrderDetailService.ConventionalGrindObject(d.recoveryODCustomer2);
                        d._recoveryOrderDetailService.CWCGrindObject(d.recoveryODCustomer2);
                        d._recoveryOrderDetailService.PolishAndQCObject(d.recoveryODCustomer2);
                        d._recoveryOrderDetailService.PackageObject(d.recoveryODCustomer2);
                        d._recoveryOrderDetailService.RejectObject(d.recoveryODCustomer2, d._recoveryOrderService);

                        d._recoveryOrderDetailService.StripAndGlueObject(d.recoveryODCustomer3);
                        d._recoveryOrderDetailService.WrapObject(d.recoveryODCustomer3);
                        d._recoveryOrderDetailService.VulcanizeObject(d.recoveryODCustomer3);
                        d._recoveryOrderDetailService.FaceOffObject(d.recoveryODCustomer3);
                        d._recoveryOrderDetailService.ConventionalGrindObject(d.recoveryODCustomer3);
                        d._recoveryOrderDetailService.CWCGrindObject(d.recoveryODCustomer3);
                        d._recoveryOrderDetailService.PolishAndQCObject(d.recoveryODCustomer3);
                        d._recoveryOrderDetailService.PackageObject(d.recoveryODCustomer3);

                        d._recoveryOrderDetailService.StripAndGlueObject(d.recoveryODInHouse1);
                        d._recoveryOrderDetailService.WrapObject(d.recoveryODInHouse1);
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
                        d._recoveryAccessoryDetailService.CreateObject(d.accessory1, d._recoveryOrderDetailService, d._itemService, d._itemTypeService);
                        
                        d._recoveryOrderDetailService.StripAndGlueObject(d.recoveryODInHouse2);
                        d._recoveryOrderDetailService.WrapObject(d.recoveryODInHouse2);
                        d._recoveryOrderDetailService.VulcanizeObject(d.recoveryODInHouse2);
                        d._recoveryOrderDetailService.FaceOffObject(d.recoveryODInHouse2);
                        d._recoveryOrderDetailService.ConventionalGrindObject(d.recoveryODInHouse2);
                        d._recoveryOrderDetailService.CWCGrindObject(d.recoveryODInHouse2);
                        d._recoveryOrderDetailService.PolishAndQCObject(d.recoveryODInHouse2);
                        d._recoveryOrderDetailService.PackageObject(d.recoveryODInHouse2);

                    };

                    it["validates_recoveryorderdetails"] = () =>
                    {
                        d.recoveryODCustomer1.Errors.Count().should_be(0);
                        d.recoveryODCustomer2.Errors.Count().should_be(0);
                        d.recoveryODCustomer3.Errors.Count().should_be(0);
                        d.recoveryODInHouse1.Errors.Count().should_be(0);
                        d.recoveryODInHouse2.Errors.Count().should_be(0);
                        d.recoveryODInHouse3.Errors.Count().should_be(0);
                        d.accessory1.Errors.Count().should_be(0);
                    };

                    it["validates_confirm_accessory"] = () =>
                    {
                        int itemquantitybefore = d._itemService.GetObjectById(d.itemAccessory1.Id).Quantity;
                        d._recoveryAccessoryDetailService.ConfirmObject(d.accessory1, d._recoveryOrderService, d._recoveryOrderDetailService, d._itemService, d._warehouseItemService);
                        d.accessory1.Errors.Count().should_be(0);
                        itemquantitybefore.should_be(d._itemService.GetObjectById(d.itemAccessory1.Id).Quantity + 1);
                    };

                    it["validates_finishrecoveryorder"] = () =>
                    {
                        d._recoveryAccessoryDetailService.ConfirmObject(d.accessory1, d._recoveryOrderService, d._recoveryOrderDetailService, d._itemService, d._warehouseItemService);
                        int usedCoreBuilder3Quantity = d._recoveryOrderDetailService.GetCore(d.recoveryODCustomer1, d._coreIdentificationDetailService, d._coreBuilderService, d._itemService).Quantity;
                        int usedCoreBuilder4Quantity = d._recoveryOrderDetailService.GetCore(d.recoveryODCustomer3, d._coreIdentificationDetailService, d._coreBuilderService, d._itemService).Quantity;
                        int usedRollerBuilder3Quantity = d._recoveryOrderDetailService.GetRoller(d.recoveryODCustomer1, d._coreIdentificationDetailService, d._rollerBuilderService, d._itemService).Quantity;
                        int usedRollerBuilder4Quantity = d._recoveryOrderDetailService.GetRoller(d.recoveryODCustomer3, d._coreIdentificationDetailService, d._rollerBuilderService, d._itemService).Quantity;
                        d.recoveryOrderCustomer = d._recoveryOrderService.FinishObject(d.recoveryOrderCustomer, d._coreIdentificationDetailService, d._recoveryOrderDetailService,
                                                                                       d._recoveryAccessoryDetailService, d._coreBuilderService, d._rollerBuilderService, d._itemService, d._warehouseItemService);
                        d.recoveryOrderCustomer.IsFinished.should_be(true);
                        d.recoveryOrderCustomer.Errors.Count().should_be(0);
                        int usedCoreBuilder3Final = d._recoveryOrderDetailService.GetCore(d.recoveryODCustomer1, d._coreIdentificationDetailService, d._coreBuilderService, d._itemService).Quantity;
                        int usedCoreBuilder4Final = d._recoveryOrderDetailService.GetCore(d.recoveryODCustomer3, d._coreIdentificationDetailService, d._coreBuilderService, d._itemService).Quantity;
                        int usedRollerBuilder3Final = d._recoveryOrderDetailService.GetRoller(d.recoveryODCustomer1, d._coreIdentificationDetailService, d._rollerBuilderService, d._itemService).Quantity;
                        int usedRollerBuilder4Final = d._recoveryOrderDetailService.GetRoller(d.recoveryODCustomer3, d._coreIdentificationDetailService, d._rollerBuilderService, d._itemService).Quantity;
                        usedCoreBuilder3Final.should_be(usedCoreBuilder3Quantity);
                        usedRollerBuilder3Final.should_be(usedRollerBuilder3Quantity + 1);
                        usedCoreBuilder4Final.should_be(usedCoreBuilder4Quantity);
                        usedRollerBuilder4Final.should_be(usedRollerBuilder4Quantity + 1);
                    };
                };
            };
        }
    }
}