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

    public class SpecContact: nspec
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
                    UoMId = d.Pcs.Id
                };
                d.item = d._itemService.CreateObject(d.item, d._uomService, d._itemTypeService, d._warehouseItemService, d._warehouseService, d._priceMutationService);

                d.localWarehouse = new Warehouse()
                {
                    Name = "Sentral Solusi Data",
                    Description = "Kali Besar Jakarta",
                    Code = "LCL"
                };
                d.localWarehouse = d._warehouseService.CreateObject(d.localWarehouse, d._warehouseItemService, d._itemService);

                d.contact = new Contact()
                {
                    Name = "Abbey",
                    Address = "1 Abbey St",
                    ContactNo = "001234567",
                    PIC = "Daddy",
                    PICContactNo = "001234888",
                    Email = "abbey@abbeyst.com",
                    TaxCode = "01",
                    ContactType = "CUSTOMER"
                };
                d.contact = d._contactService.CreateObject(d.contact);
            }
        }

        void contact_validation()
        {
        
            it["validates_contact"] = () =>
            {
                d.contact.Errors.Count().should_be(0);
            };

            it["contact_with_no_name"] = () =>
            {
                Contact noname = new Contact()
                {
                    Name = "     ",
                    Address = "I have no name",
                    ContactNo = "0000123",
                    PIC = "Who are you?",
                    PICContactNo = "001234",
                    Email = "empty@noname.com",
                    TaxCode = "01",
                    ContactType = "CUSTOMER"
                };
                noname = d._contactService.CreateObject(noname);
                noname.Errors.Count().should_not_be(0);
            };

            it["item_with_same_name_same_contacttype"] = () =>
            {
                Contact samename = new Contact()
                {
                    Name = "Abbey",
                    Address = "I am a copy",
                    ContactNo = "0000123",
                    PIC = "Who are you?",
                    PICContactNo = "001234",
                    Email = "empty@noname.com",
                    TaxCode = "01",
                    ContactType = "CUSTOMER"
                };
                samename = d._contactService.CreateObject(samename);
                samename.Errors.Count().should_not_be(0);
            };

            it["withemptyaddress"] = () =>
            {
                Contact emptyaddress = new Contact()
                {
                    Name = "Abbey12",
                    Address = " ",
                    ContactNo = "0000123",
                    PIC = "Who are you?",
                    PICContactNo = "001234",
                    Email = "empty@noname.com",
                    TaxCode = "01",
                    ContactType = "CUSTOMER"
                };
                emptyaddress = d._contactService.CreateObject(emptyaddress);
                emptyaddress.Errors.Count().should_be(0);
            };

            it["withemptycontact"] = () =>
            {
                Contact emptycontact = new Contact()
                {
                    Name = "Abbey123",
                    Address = "Ada isi",
                    ContactNo = "   ",
                    PIC = "Who are you?",
                    PICContactNo = "001234",
                    Email = "empty@noname.com",
                    TaxCode = "01",
                    ContactType = "CUSTOMER"
                };
                emptycontact = d._contactService.CreateObject(emptycontact);
                emptycontact.Errors.Count().should_be(0);
            };

            it["update_with_empty_pic"] = () =>
            {
                d.contact.PIC = "   ";
                d.contact = d._contactService.UpdateObject(d.contact);
                d.contact.Errors.Count().should_be(0);
            };

            it["update_with_empty_pic_contactno"] = () =>
            {
                d.contact.PICContactNo = "   ";
                d.contact = d._contactService.UpdateObject(d.contact);
                d.contact.Errors.Count().should_be(0);
            };

            it["update_with_empty_email"] = () =>
            {
                d.contact.Email = "   ";
                d.contact = d._contactService.UpdateObject(d.contact);
                d.contact.Errors.Count().should_be(0);
            };

            it["delete_contact"] = () =>
            {
                d.contact = d._contactService.SoftDeleteObject(d.contact, d._coreIdentificationService, d._blanketService, d._purchaseOrderService,
                                                               d._salesOrderService, d._salesQuotationService, d._virtualOrderService);
                d.contact.Errors.Count().should_be(0);
            };

            it["delete_contact_with_core_identification"] = () =>
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
                    Description = "X",
                    UoMId = d.Pcs.Id,
                    MachineId = d.machine.Id,
                    CoreBuilderTypeCase = Core.Constants.Constant.CoreBuilderTypeCase.Hollow
                };
                d.coreBuilder = d._coreBuilderService.CreateObject(d.coreBuilder, d._uomService, d._itemService, d._itemTypeService, d._warehouseItemService,
                                                                   d._warehouseService, d._priceMutationService, d._machineService);
                d.coreIdentification = new CoreIdentification()
                {
                    ContactId = d.contact.Id,
                    Code = "CI0001",
                    Quantity = 1,
                    IdentifiedDate = DateTime.Now,
                    WarehouseId = d.localWarehouse.Id
                };
                d.coreIdentification = d._coreIdentificationService.CreateObject(d.coreIdentification, d._contactService);
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
                    TL = 12,
                    RepairRequestCase = Core.Constants.Constant.RepairRequestCase.BearingSeat
                };
                d.coreIdentificationDetail = d._coreIdentificationDetailService.CreateObject(d.coreIdentificationDetail, d._coreIdentificationService, d._coreBuilderService,
                                             d._rollerTypeService, d._machineService, d._warehouseItemService);
                d.contact = d._contactService.SoftDeleteObject(d.contact, d._coreIdentificationService, d._blanketService,
                                                               d._purchaseOrderService, d._salesOrderService, d._salesQuotationService, d._virtualOrderService);
                d.contact.Errors.Count().should_not_be(0);
            };
        }
    }
}