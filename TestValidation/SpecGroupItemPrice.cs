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

    public class SpecGroupItemPrice: nspec
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
                d.item = d._itemService.CreateObject(d.item, d._uomService, d._itemTypeService, d._warehouseItemService, d._warehouseService, d._priceMutationService, d._contactGroupService);

                d.groupItemPrice1 = new GroupItemPrice()
                {
                    Price = 1000,
                    ItemId = d.item.Id,
                    ContactGroupId = d.baseGroup.Id
                };
                d.groupItemPrice1 = d._groupItemPriceService.CreateObject(d.groupItemPrice1, d._contactGroupService, d._itemService, d._priceMutationService);
            }
        }

        void groupitemprice_validation()
        {
        
            it["validates_groupitemprice"] = () =>
            {
                d.groupItemPrice1.Errors.Count().should_be(0);
            };

            it["groupitemprice_with_no_price"] = () =>
            {
                GroupItemPrice groupitemprice2 = new GroupItemPrice()
                {
                    Price = 0,
                    ItemId = d.item.Id,
                    ContactGroupId = d.baseGroup.Id
                };
                groupitemprice2 = d._groupItemPriceService.CreateObject(groupitemprice2, d._contactGroupService, d._itemService, d._priceMutationService);
                groupitemprice2.Errors.Count().should_not_be(0);
            };

            it["groupitemprice_with_no_itemid"] = () =>
            {
                GroupItemPrice groupitemprice2 = new GroupItemPrice()
                {
                    Price = 1000,
                    ContactGroupId = d.baseGroup.Id
                };
                groupitemprice2 = d._groupItemPriceService.CreateObject(groupitemprice2, d._contactGroupService, d._itemService, d._priceMutationService);
                groupitemprice2.Errors.Count().should_not_be(0);
            };

            it["groupitemprice_with_no_groupid"] = () =>
            {
                GroupItemPrice groupitemprice2 = new GroupItemPrice()
                {
                    Price = 1000,
                    ItemId = d.item.Id
                };
                groupitemprice2 = d._groupItemPriceService.CreateObject(groupitemprice2, d._contactGroupService, d._itemService, d._priceMutationService);
                groupitemprice2.Errors.Count().should_not_be(0);
            };

            it["groupitemprice_with_same_idcombination"] = () =>
            {
                GroupItemPrice groupitemprice2 = new GroupItemPrice()
                {
                    Price = 2000,
                    ItemId = d.item.Id,
                    ContactGroupId =d.baseGroup.Id
                };
                groupitemprice2 = d._groupItemPriceService.CreateObject(groupitemprice2, d._contactGroupService, d._itemService, d._priceMutationService);
                groupitemprice2.Errors.Count().should_not_be(0);
            };

            context["when creating groupitemprice"] = () =>
            {
                before = () =>
                {

                };

                it["should create PriceMutation"] = () =>
                {
                    d._priceMutationService.GetObjectById(d.groupItemPrice1.PriceMutationId).should_not_be_null();
                };

                it["should have 1 active PriceMutation"] = () =>
                {
                    d._priceMutationService.GetObjectsByIsActive(true, d.groupItemPrice1.ItemId, d.groupItemPrice1.ContactGroupId, 0).Count().should_be(1);
                };

            };

            it["update_with_zero_price"] = () =>
            {
                d.groupItemPrice1.Price = 0;
                d.groupItemPrice1 = d._groupItemPriceService.UpdateObject(d.groupItemPrice1, d._itemService, d._priceMutationService);
                d.groupItemPrice1.Errors.Count().should_not_be(0);
            };

            it["update_with_active_price"] = () =>
            {
                PriceMutation pricemutation1 = d._priceMutationService.GetObjectsByIsActive(true, d.item.Id, d.baseGroup.Id, 0).FirstOrDefault();
                d.groupItemPrice1.Price = pricemutation1.Amount;
                d.groupItemPrice1 = d._groupItemPriceService.UpdateObject(d.groupItemPrice1, d._itemService, d._priceMutationService);
                d.groupItemPrice1.Errors.Count().should_not_be(0);
            };

            it["update_with_different_itemid"] = () =>
            {
                Item item = new Item()
                {
                    ItemTypeId = d._itemTypeService.GetObjectByName("Accessory").Id,
                    Sku = "ABC1002",
                    Name = "CBA",
                    Category = "ABC123",
                    UoMId = d.Pcs.Id
                };
                item = d._itemService.CreateObject(item, d._uomService, d._itemTypeService, d._warehouseItemService, d._warehouseService, d._priceMutationService, d._contactGroupService);

                d.groupItemPrice1.ItemId = item.Id;
                d.groupItemPrice1 = d._groupItemPriceService.UpdateObject(d.groupItemPrice1, d._itemService, d._priceMutationService);
                d.groupItemPrice1.Errors.Count().should_not_be(0);
            };

            it["update_with_different_groupid"] = () =>
            {
                ContactGroup contactgroup = d._contactGroupService.CreateObject("Admin", "Administrators");

                d.groupItemPrice1.ContactGroupId = contactgroup.Id;
                d.groupItemPrice1 = d._groupItemPriceService.UpdateObject(d.groupItemPrice1, d._itemService, d._priceMutationService);
                d.groupItemPrice1.Errors.Count().should_not_be(0);
            };
        }
    }
}