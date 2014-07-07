using Core.DomainModel;
using Core.Interface.Service;
using Data.Context;
using Data.Repository;
using Service.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                d.PopulateItem();
                d.PopulateSingles();
                d.PopulateBuilders();
                d.PopulateCoreIdentifications();

                int usedCoreBuilder3Quantity = d._coreIdentificationDetailService.GetCore(d.coreIDCustomer1, d._coreBuilderService).Quantity;
                int usedCoreBuilder4Quantity = d._coreIdentificationDetailService.GetCore(d.coreIDCustomer3, d._coreBuilderService).Quantity;
                d.coreIdentificationCustomer = d._coreIdentificationService.ConfirmObject(d.coreIdentificationCustomer, d._coreIdentificationDetailService, d._recoveryOrderService,
                                               d._recoveryOrderDetailService, d._coreBuilderService, d._itemService);
                int usedCoreBuilder3AfterConfirmed = d._coreIdentificationDetailService.GetCore(d.coreIDCustomer1, d._coreBuilderService).Quantity;
                int usedCoreBuilder4AfterConfirmed = d._coreIdentificationDetailService.GetCore(d.coreIDCustomer3, d._coreBuilderService).Quantity;

                d.coreIdentificationCustomer = d._coreIdentificationService.UnconfirmObject(d.coreIdentificationCustomer, d._coreIdentificationDetailService,
                               d._recoveryOrderService, d._coreBuilderService, d._itemService);
                int usedCoreBuilder3AfterUnconfirmed = d._coreIdentificationDetailService.GetCore(d.coreIDCustomer1, d._coreBuilderService).Quantity;
                int usedCoreBuilder4AfterUnconfirmed = d._coreIdentificationDetailService.GetCore(d.coreIDCustomer3, d._coreBuilderService).Quantity;

                int usedCoreBuilder3IHQuantity = d._coreIdentificationDetailService.GetCore(d.coreIDInHouse1, d._coreBuilderService).Quantity;
                int usedCoreBuilder4IHQuantity = d._coreIdentificationDetailService.GetCore(d.coreIDInHouse3, d._coreBuilderService).Quantity;

                d.coreIdentificationInHouse = d._coreIdentificationService.ConfirmObject(d.coreIdentificationInHouse, d._coreIdentificationDetailService, d._recoveryOrderService,
                               d._recoveryOrderDetailService, d._coreBuilderService, d._itemService);

                int usedCoreBuilder3IHAfterConfirmed = d._coreIdentificationDetailService.GetCore(d.coreIDInHouse1, d._coreBuilderService).Quantity;
                int usedCoreBuilder4IHAfterConfirmed = d._coreIdentificationDetailService.GetCore(d.coreIDInHouse3, d._coreBuilderService).Quantity;
            
                d.coreIdentificationInHouse = d._coreIdentificationService.UnconfirmObject(d.coreIdentificationInHouse, d._coreIdentificationDetailService,
                               d._recoveryOrderService, d._coreBuilderService, d._itemService);

                int usedCoreBuilder3IHAfterUnconfirmed = d._coreIdentificationDetailService.GetCore(d.coreIDInHouse1, d._coreBuilderService).Quantity;
                int usedCoreBuilder4IHAfterUnconfirmed = d._coreIdentificationDetailService.GetCore(d.coreIDInHouse3, d._coreBuilderService).Quantity;
            
                Console.WriteLine("Press any key to stop...");
                Console.ReadKey();
            }

        }
    }
}
