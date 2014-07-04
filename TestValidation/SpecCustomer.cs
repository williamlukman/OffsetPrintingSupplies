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

    public class SpecCustomer: nspec
    {

        DataBuilder d;
        void before_each()
        {
            var db = new OffsetPrintingSuppliesEntities();
            using (db)
            {
                db.DeleteAllTables();
                d = new DataBuilder();

                d.item = new Item()
                {
                    ItemTypeId = d._itemTypeService.GetObjectByName("Accessory").Id,
                    Sku = "ABC1001",
                    Name = "ABC",
                    Category = "ABC123",
                    UoM = "Pcs",
                    Quantity = 0
                };
                d.item = d._itemService.CreateObject(d.item, d._itemTypeService);
                d.customer = d._customerService.CreateObject("Abbey", "1 Abbey St", "001234567", "Daddy", "001234888", "abbey@abbeyst.com");
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
        void customer_validation()
        {
        
            it["validates_customer"] = () =>
            {
                d.customer.Errors.Count().should_be(0);
            };

            it["customer_with_no_name"] = () =>
            {
                Customer noname = new Customer()
                {
                    Name = "     ",
                    Address = "I have no name",
                    ContactNo = "0000123",
                    PIC = "Who are you?",
                    PICContactNo = "001234",
                    Email = "empty@noname.com"
                };
                noname = d._customerService.CreateObject(noname);
                noname.Errors.Count().should_not_be(0);
            };

            it["item_with_same_name"] = () =>
            {
                Customer samename = new Customer()
                {
                    Name = "Abbey",
                    Address = "I am a copy",
                    ContactNo = "0000123",
                    PIC = "Who are you?",
                    PICContactNo = "001234",
                    Email = "empty@noname.com"
                }; 
                samename = d._customerService.CreateObject(samename);
                samename.Errors.Count().should_not_be(0);
            };

            it["withemptyaddress"] = () =>
            {
                Customer emptyaddress = new Customer()
                {
                    Name = "Abbey12",
                    Address = " ",
                    ContactNo = "0000123",
                    PIC = "Who are you?",
                    PICContactNo = "001234",
                    Email = "empty@noname.com"
                };
                emptyaddress = d._customerService.CreateObject(emptyaddress);
                emptyaddress.Errors.Count().should_not_be(0);
            };

            it["withemptycontact"] = () =>
            {
                Customer emptycontact = new Customer()
                {
                    Name = "Abbey123",
                    Address = "Ada isi",
                    ContactNo = "   ",
                    PIC = "Who are you?",
                    PICContactNo = "001234",
                    Email = "empty@noname.com"
                };
                emptycontact = d._customerService.CreateObject(emptycontact);
                emptycontact.Errors.Count().should_not_be(0);
            };

            it["update_with_empty_pic"] = () =>
            {
                d.customer.PIC = "   ";
                d.customer = d._customerService.UpdateObject(d.customer);
                d.customer.Errors.Count().should_not_be(0);
            };

            it["update_with_empty_pic_contactno"] = () =>
            {
                d.customer.PICContactNo = "   ";
                d.customer = d._customerService.UpdateObject(d.customer);
                d.customer.Errors.Count().should_not_be(0);
            };

            it["update_with_empty_email"] = () =>
            {
                d.customer.Email = "   ";
                d.customer = d._customerService.UpdateObject(d.customer);
                d.customer.Errors.Count().should_not_be(0);
            };

            it["delete_customer"] = () =>
            {
                d.customer = d._customerService.SoftDeleteObject(d.customer, d._coreIdentificationService);
                d.customer.Errors.Count().should_be(0);
            };

            it["delete_customer_with_core_identification"] = () =>
            {
                d.machine = new Machine()
                {
                    Code = "M00001",
                    Name = "Machine 00001",
                    Description = "Machine"
                };
                d.machine = d._machineService.CreateObject(d.machine);
                d.coreBuilder = new CoreBuilder()
                {
                    BaseSku = "CB00001",
                    SkuNewCore = "CB00001N",
                    SkuUsedCore = "CB00001U",
                    Name = "CoreBuilder00001",
                    Category = "X"
                };
                d.coreBuilder = d._coreBuilderService.CreateObject(d.coreBuilder, d._itemService, d._itemTypeService);
                d.coreIdentification = new CoreIdentification()
                {
                    CustomerId = d.customer.Id,
                    Code = "CI0001",
                    Quantity = 1,
                    IdentifiedDate = DateTime.Now
                };
                d.coreIdentification = d._coreIdentificationService.CreateObject(d.coreIdentification, d._customerService);
                d.coreIdentificationDetail = new CoreIdentificationDetail()
                {
                    CoreIdentificationId = d.coreIdentification.Id,
                    DetailId = 1,
                    MaterialCase = 2,
                    CoreBuilderId = d.coreBuilder.Id,
                    RollerTypeId = d._rollerTypeService.GetObjectByName("Found DT").Id,
                    MachineId = d.machine.Id,
                    RD = 12,
                    CD = 12,
                    RL = 12,
                    WL = 12,
                    TL = 12
                };
                d.coreIdentificationDetail = d._coreIdentificationDetailService.CreateObject(d.coreIdentificationDetail, d._coreIdentificationService, d._coreBuilderService, d._rollerTypeService, d._machineService);
                d.customer = d._customerService.SoftDeleteObject(d.customer, d._coreIdentificationService);
                d.customer.Errors.Count().should_not_be(0);
            };
        }
    }
}