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
            };

            it["deletes_coreidentification"] = () =>
            {
                d.coreIdentificationCustomer = d._coreIdentificationService.SoftDeleteObject(d.coreIdentificationCustomer, d._coreIdentificationDetailService, d._recoveryOrderService);
                d.coreIdentificationCustomer.Errors.Count().should_be(0);
            };

            it["confirms_coreidentificationcustomer"] = () =>
            {
                int usedCoreBuilder3Quantity = d._coreIdentificationDetailService.GetCore(d.coreIDCustomer1, d._coreBuilderService).Quantity;
                int usedCoreBuilder4Quantity = d._coreIdentificationDetailService.GetCore(d.coreIDCustomer3, d._coreBuilderService).Quantity;
                d.coreIdentificationCustomer = d._coreIdentificationService.ConfirmObject(d.coreIdentificationCustomer, d._coreIdentificationDetailService, d._stockMutationService, d._recoveryOrderService,
                                               d._recoveryOrderDetailService, d._coreBuilderService, d._itemService, d._warehouseItemService, d._barringService);
                d.coreIdentificationCustomer.IsConfirmed.should_be(true);
                d.coreIdentificationCustomer.Errors.Count().should_be(0);
                int usedCoreBuilder3AfterConfirmed = d._coreIdentificationDetailService.GetCore(d.coreIDCustomer1, d._coreBuilderService).Quantity;
                int usedCoreBuilder4AfterConfirmed = d._coreIdentificationDetailService.GetCore(d.coreIDCustomer3, d._coreBuilderService).Quantity;
                usedCoreBuilder3AfterConfirmed.should_be(usedCoreBuilder3Quantity + 2);
                usedCoreBuilder4AfterConfirmed.should_be(usedCoreBuilder4Quantity + 1);
            };

            it["unconfirms_coreidentificationcustomer"] = () =>
            {
                d.coreIdentificationCustomer = d._coreIdentificationService.ConfirmObject(d.coreIdentificationCustomer, d._coreIdentificationDetailService, d._stockMutationService,
                                               d._recoveryOrderService, d._recoveryOrderDetailService, d._coreBuilderService, d._itemService, d._warehouseItemService, d._barringService);
                int usedCoreBuilder3Quantity = d._coreIdentificationDetailService.GetCore(d.coreIDCustomer1, d._coreBuilderService).Quantity;
                int usedCoreBuilder4Quantity = d._coreIdentificationDetailService.GetCore(d.coreIDCustomer3, d._coreBuilderService).Quantity;
                d.coreIdentificationCustomer = d._coreIdentificationService.UnconfirmObject(d.coreIdentificationCustomer, d._coreIdentificationDetailService, d._stockMutationService,
                                               d._recoveryOrderService, d._coreBuilderService, d._itemService, d._warehouseItemService, d._barringService);
                int usedCoreBuilder3AfterUnconfirmed = d._coreIdentificationDetailService.GetCore(d.coreIDCustomer1, d._coreBuilderService).Quantity;
                int usedCoreBuilder4AfterUnconfirmed = d._coreIdentificationDetailService.GetCore(d.coreIDCustomer3, d._coreBuilderService).Quantity;
                d.coreIdentificationCustomer.IsConfirmed.should_be(false);
                d.coreIdentificationCustomer.Errors.Count().should_be(0);
                usedCoreBuilder3AfterUnconfirmed.should_be(usedCoreBuilder3Quantity - 2);
                usedCoreBuilder4AfterUnconfirmed.should_be(usedCoreBuilder4Quantity - 1);
            };

            it["confirms_coreidentificationinhouse"] = () =>
            {
                int usedCoreBuilder3Quantity = d._coreIdentificationDetailService.GetCore(d.coreIDInHouse1, d._coreBuilderService).Quantity;
                int usedCoreBuilder4Quantity = d._coreIdentificationDetailService.GetCore(d.coreIDInHouse3, d._coreBuilderService).Quantity;
                d.coreIdentificationInHouse = d._coreIdentificationService.ConfirmObject(d.coreIdentificationInHouse, d._coreIdentificationDetailService, d._stockMutationService,
                                                                                         d._recoveryOrderService, d._recoveryOrderDetailService, d._coreBuilderService, d._itemService,
                                                                                         d._warehouseItemService, d._barringService);
                d.coreIdentificationInHouse.IsConfirmed.should_be(true);
                d.coreIdentificationInHouse.Errors.Count().should_be(0);
                int usedCoreBuilder3AfterConfirmed = d._coreIdentificationDetailService.GetCore(d.coreIDInHouse1, d._coreBuilderService).Quantity;
                int usedCoreBuilder4AfterConfirmed = d._coreIdentificationDetailService.GetCore(d.coreIDInHouse3, d._coreBuilderService).Quantity;
                usedCoreBuilder3AfterConfirmed.should_be(usedCoreBuilder3Quantity);
                usedCoreBuilder4AfterConfirmed.should_be(usedCoreBuilder4Quantity);
            };

            it["unconfirms_coreidentificationinhouse"] = () =>
            {
                d.coreIdentificationInHouse = d._coreIdentificationService.ConfirmObject(d.coreIdentificationInHouse, d._coreIdentificationDetailService, d._stockMutationService,
                                                                                         d._recoveryOrderService, d._recoveryOrderDetailService, d._coreBuilderService, d._itemService,
                                                                                         d._warehouseItemService, d._barringService);
                int usedCoreBuilder3Quantity = d._coreIdentificationDetailService.GetCore(d.coreIDInHouse1, d._coreBuilderService).Quantity;
                int usedCoreBuilder4Quantity = d._coreIdentificationDetailService.GetCore(d.coreIDInHouse3, d._coreBuilderService).Quantity;
                d.coreIdentificationInHouse = d._coreIdentificationService.UnconfirmObject(d.coreIdentificationInHouse, d._coreIdentificationDetailService, d._stockMutationService,
                                                                                           d._recoveryOrderService, d._coreBuilderService, d._itemService, d._warehouseItemService, d._barringService);
                int usedCoreBuilder3AfterUnconfirmed = d._coreIdentificationDetailService.GetCore(d.coreIDInHouse1, d._coreBuilderService).Quantity;
                int usedCoreBuilder4AfterUnconfirmed = d._coreIdentificationDetailService.GetCore(d.coreIDInHouse3, d._coreBuilderService).Quantity;
                d.coreIdentificationInHouse.IsConfirmed.should_be(false);
                d.coreIdentificationInHouse.Errors.Count().should_be(0);
                usedCoreBuilder3AfterUnconfirmed.should_be(usedCoreBuilder3Quantity);
                usedCoreBuilder4AfterUnconfirmed.should_be(usedCoreBuilder4Quantity);
            };

            it["deletes_coreidentificationcustomer_with_recoveryorder"] = () =>
            {
                d.PopulateRecoveryOrders();
                d.coreIdentificationCustomer = d._coreIdentificationService.SoftDeleteObject(d.coreIdentificationCustomer, d._coreIdentificationDetailService, d._recoveryOrderService);
                d.coreIdentificationCustomer.Errors.Count().should_not_be(0);
            };
        }
    }
}