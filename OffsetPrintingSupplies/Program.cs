using Core.Constants;
using Core.DomainModel;
using Core.Interface.Service;
using Data.Context;
using Data.Repository;
using Service.Service;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                ICoreBuilderService coreBuilderService = new CoreBuilderService(new CoreBuilderRepository(), new CoreBuilderValidator());
                ICoreIdentificationDetailService coreIdentificationDetailService = new CoreIdentificationDetailService(new CoreIdentificationDetailRepository(), new CoreIdentificationDetailValidator());
                ICoreIdentificationService coreIdentificationService = new CoreIdentificationService(new CoreIdentificationRepository(), new CoreIdentificationValidator());
                ICustomerService customerService = new CustomerService(new CustomerRepository(), new CustomerValidator());
                IItemService itemService = new ItemService(new ItemRepository(), new ItemValidator());
                IItemTypeService itemTypeService = new ItemTypeService(new ItemTypeRepository(), new ItemTypeValidator());
                IMachineService machineService = new MachineService(new MachineRepository(), new MachineValidator());
                IRecoveryAccessoryDetailService recoveryAccessoryDetailService = new RecoveryAccessoryDetailService(new RecoveryAccessoryDetailRepository(), new RecoveryAccessoryDetailValidator());
                IRecoveryOrderDetailService recoveryOrderDetailService = new RecoveryOrderDetailService(new RecoveryOrderDetailRepository(), new RecoveryOrderDetailValidator());
                IRecoveryOrderService recoveryOrderService = new RecoveryOrderService(new RecoveryOrderRepository(), new RecoveryOrderValidator());
                IRollerBuilderService rollerBuilderService = new RollerBuilderService(new RollerBuilderRepository(), new RollerBuilderValidator());
                IRollerTypeService rollerTypeService = new RollerTypeService(new RollerTypeRepository(), new RollerTypeValidator());

                // Warning: this function will delete all data in the DB. Use with caution!!!
                db.DeleteAllTables();

                Console.WriteLine("Press any key to stop...");
                Console.ReadKey();
            }
        }
    }
}
