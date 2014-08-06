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
                d.Pcs = new UoM()
                {
                    Name = "Pcs"
                };
                d._uomService.CreateObject(d.Pcs);

                d.Boxes = new UoM()
                {
                    Name = "Boxes"
                };
                d._uomService.CreateObject(d.Boxes);

                d.Tubs = new UoM()
                {
                    Name = "Tubs"
                };
                d._uomService.CreateObject(d.Tubs);

                d.item = new Item()
                {
                    ItemTypeId = d._itemTypeService.GetObjectByName("Accessory").Id,
                    Sku = "ABC1001",
                    Name = "ABC",
                    Category = "ABC123",
                    UoMId = d.Pcs.Id
                };
                d.item = d._itemService.CreateObject(d.item, d._uomService, d._itemTypeService, d._warehouseItemService, d._warehouseService);

                d.localWarehouse = new Warehouse()
                {
                    Name = "Sentral Solusi Data",
                    Description = "Kali Besar Jakarta",
                    IsMovingWarehouse = false,
                    Code = "LCL"
                };
                d.localWarehouse = d._warehouseService.CreateObject(d.localWarehouse, d._warehouseItemService, d._itemService);

                d.customer = d._customerService.CreateObject("Abbey", "1 Abbey St", "001234567", "Daddy", "001234888", "abbey@abbeyst.com");
            }
        }

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
                    CustomerNo = "0000123",
                    PIC = "Who are you?",
                    PICCustomerNo = "001234",
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
                    CustomerNo = "0000123",
                    PIC = "Who are you?",
                    PICCustomerNo = "001234",
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
                    CustomerNo = "0000123",
                    PIC = "Who are you?",
                    PICCustomerNo = "001234",
                    Email = "empty@noname.com"
                };
                emptyaddress = d._customerService.CreateObject(emptyaddress);
                emptyaddress.Errors.Count().should_not_be(0);
            };

            it["withemptycustomer"] = () =>
            {
                Customer emptycustomer = new Customer()
                {
                    Name = "Abbey123",
                    Address = "Ada isi",
                    CustomerNo = "   ",
                    PIC = "Who are you?",
                    PICCustomerNo = "001234",
                    Email = "empty@noname.com"
                };
                emptycustomer = d._customerService.CreateObject(emptycustomer);
                emptycustomer.Errors.Count().should_not_be(0);
            };

            it["update_with_empty_pic"] = () =>
            {
                d.customer.PIC = "   ";
                d.customer = d._customerService.UpdateObject(d.customer);
                d.customer.Errors.Count().should_not_be(0);
            };

            it["update_with_empty_pic_customerno"] = () =>
            {
                d.customer.PICCustomerNo = "   ";
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
                d.customer = d._customerService.SoftDeleteObject(d.customer, d._coreIdentificationService, d._barringService);
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
                    Category = "X",
                    UoMId = d.Pcs.Id
                };
                d.coreBuilder = d._coreBuilderService.CreateObject(d.coreBuilder, d._uomService, d._itemService, d._itemTypeService, d._warehouseItemService, d._warehouseService);
                d.coreIdentification = new CoreIdentification()
                {
                    CustomerId = d.customer.Id,
                    Code = "CI0001",
                    Quantity = 1,
                    IdentifiedDate = DateTime.Now,
                    WarehouseId = d.localWarehouse.Id
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
                d.customer = d._customerService.SoftDeleteObject(d.customer, d._coreIdentificationService, d._barringService);
                d.customer.Errors.Count().should_not_be(0);
            };
        }
    }
}