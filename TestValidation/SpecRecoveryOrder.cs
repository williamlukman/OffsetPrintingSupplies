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
                d.recoveryOrderCustomer = d._recoveryOrderService.ConfirmObject(d.recoveryOrderCustomer, d._coreIdentificationDetailService, d._recoveryOrderDetailService,
                                                                                d._recoveryAccessoryDetailService, d._coreBuilderService, d._itemService);
                d.recoveryOrderCustomer.IsConfirmed.should_be(true);
                d.recoveryOrderCustomer.Errors.Count().should_be(0);
            };

            it["finishes_recovery_order"] = () =>
            {
                d.recoveryOrderCustomer = d._recoveryOrderService.ConfirmObject(d.recoveryOrderCustomer, d._coreIdentificationDetailService, d._recoveryOrderDetailService,
                                                                                d._recoveryAccessoryDetailService, d._coreBuilderService, d._itemService);
                d.recoveryOrderCustomer = d._recoveryOrderService.FinishObject(d.recoveryOrderCustomer, d._coreIdentificationDetailService, d._recoveryOrderDetailService,
                                                                               d._recoveryAccessoryDetailService, d._coreBuilderService, d._rollerBuilderService, d._itemService);
                d.recoveryOrderCustomer.Errors.Count().should_not_be(0);
            };

            it["unconfirms_recovery_order"] = () =>
            {
                d.recoveryOrderCustomer = d._recoveryOrderService.ConfirmObject(d.recoveryOrderCustomer, d._coreIdentificationDetailService, d._recoveryOrderDetailService,
                                                                                d._recoveryAccessoryDetailService, d._coreBuilderService, d._itemService);
                d.recoveryOrderCustomer.IsConfirmed.should_be(true);
                d.recoveryOrderCustomer = d._recoveryOrderService.UnconfirmObject(d.recoveryOrderCustomer, d._coreIdentificationDetailService, d._recoveryOrderDetailService,
                                                                                d._recoveryAccessoryDetailService, d._coreBuilderService, d._itemService);
                d.recoveryOrderCustomer.IsConfirmed.should_be(false);
                d.recoveryOrderCustomer.Errors.Count().should_be(0);
            };

            it["deletes_recoveryorder_with_processed_detail"] = () =>
            {
                d.recoveryOrderCustomer = d._recoveryOrderService.ConfirmObject(d.recoveryOrderCustomer, d._coreIdentificationDetailService, d._recoveryOrderDetailService,
                                                                                d._recoveryAccessoryDetailService, d._coreBuilderService, d._itemService);
                d.recoveryODCustomer1 = d._recoveryOrderDetailService.DisassembleObject(d.recoveryODCustomer1);
                d.recoveryODCustomer1 = d._recoveryOrderDetailService.StripAndGlueObject(d.recoveryODCustomer1);
                d.recoveryODCustomer1 = d._recoveryOrderDetailService.WrapObject(d.recoveryODCustomer1);

                d.recoveryOrderCustomer = d._recoveryOrderService.SoftDeleteObject(d.recoveryOrderCustomer, d._recoveryOrderDetailService, d._recoveryAccessoryDetailService);
                d.recoveryOrderCustomer.Errors.Count().should_not_be(0);
            };
        }
    }
}