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
                
                d.PopulateData();

                if (d.itemCompound.Errors.Count() > 0) { Console.WriteLine("Error"); };
                if (d.itemCompound1.Errors.Count() > 0) { Console.WriteLine("Error"); };
                if (d.itemCompound2.Errors.Count() > 0) { Console.WriteLine("Error"); };
                if (d.itemAccessory1.Errors.Count() > 0) { Console.WriteLine("Error"); };
                if (d.itemAccessory2.Errors.Count() > 0) { Console.WriteLine("Error"); };
                if (d.customer.Errors.Count() > 0) { Console.WriteLine("Error"); };
                if (d.machine.Errors.Count() > 0) { Console.WriteLine("Error"); };
                if (d.coreBuilder.Errors.Count() > 0) { Console.WriteLine("Error"); };
                if (d.coreBuilder1.Errors.Count() > 0) { Console.WriteLine("Error"); };
                if (d.coreBuilder2.Errors.Count() > 0) { Console.WriteLine("Error"); };
                if (d.coreBuilder3.Errors.Count() > 0) { Console.WriteLine("Error"); };
                if (d.coreBuilder4.Errors.Count() > 0) { Console.WriteLine("Error"); };
                if (d.coreIdentification.Errors.Count() > 0) { Console.WriteLine("Error"); };
                if (d.coreIdentificationCustomer.Errors.Count() > 0) { Console.WriteLine("Error"); };
                if (d.coreIDCustomer1.Errors.Count() > 0) { Console.WriteLine("Error"); };
                if (d.coreIDCustomer2.Errors.Count() > 0) { Console.WriteLine("Error"); };
                if (d.coreIDCustomer3.Errors.Count() > 0) { Console.WriteLine("Error"); };
                if (d.coreIdentificationDetail.Errors.Count() > 0) { Console.WriteLine("Error"); };
                if (d.coreIdentificationInHouse.Errors.Count() > 0) { Console.WriteLine("Error"); };
                if (d.coreIDInHouse1.Errors.Count() > 0) { Console.WriteLine("Error"); };
                if (d.coreIDInHouse2.Errors.Count() > 0) { Console.WriteLine("Error"); };
                if (d.coreIDInHouse3.Errors.Count() > 0) { Console.WriteLine("Error"); };
                if (d.recoveryOrderCustomer.Errors.Count() > 0) { Console.WriteLine("Error"); };
                if (d.recoveryODCustomer1.Errors.Count() > 0) { Console.WriteLine("Error"); };
                if (d.recoveryODCustomer2.Errors.Count() > 0) { Console.WriteLine("Error"); };
                if (d.recoveryODCustomer3.Errors.Count() > 0) { Console.WriteLine("Error"); };
                if (d.recoveryOrderInHouse.Errors.Count() > 0) { Console.WriteLine("Error"); };
                if (d.recoveryODInHouse1.Errors.Count() > 0) { Console.WriteLine("Error"); };
                if (d.recoveryODInHouse2.Errors.Count() > 0) { Console.WriteLine("Error"); };
                if (d.recoveryODInHouse3.Errors.Count() > 0) { Console.WriteLine("Error"); };
                

                Console.WriteLine("Press any key to stop...");
                Console.ReadKey();
            }

        }
    }
}
