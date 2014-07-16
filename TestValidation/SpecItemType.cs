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

    public class SpecItemType: nspec
    {

        DataBuilder d;

        void before_each()
        {
            var db = new OffsetPrintingSuppliesEntities();
            using (db)
            {
                db.DeleteAllTables();

                d = new DataBuilder();

                d.localWarehouse = new Warehouse()
                {
                    Name = "Sentral Solusi Data",
                    Description = "Kali Besar Jakarta",
                    IsMovingWarehouse = false,
                    Code = "LCL"
                };
                d.localWarehouse = d._warehouseService.CreateObject(d.localWarehouse, d._warehouseItemService, d._itemService);

                d.item = new Item()
                {
                    ItemTypeId = d._itemTypeService.GetObjectByName("Accessory").Id,
                    Sku = "ABC1001",
                    Name = "ABC",
                    Category = "ABC123",
                    UoM = "Pcs",
                    Quantity = 0
                };
                d.item = d._itemService.CreateObject(d.item, d._itemTypeService, d._warehouseItemService, d._warehouseService);
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
        void itemtype_validation()
        {
        
            it["validates_itemtypes"] = () =>
            {
                d.typeAccessory.Errors.Count().should_be(0);
                d.typeBearing.Errors.Count().should_be(0);
                d.typeBlanket.Errors.Count().should_be(0);
                d.typeCore.Errors.Count().should_be(0);
                d.typeConsumable.Errors.Count().should_be(0);
                d.typeChemical.Errors.Count().should_be(0);
                d.typeCompound.Errors.Count().should_be(0);
                d.typeGlue.Errors.Count.should_be(0);
                d.typeUnderpacking.Errors.Count().should_be(0);
                d.typeRoller.Errors.Count().should_be(0);
            };

            it["itemtype_with_no_name"] = () =>
            {
                ItemType nonameitemtype = new ItemType()
                {
                    Name = "     ",
                    Description = "Empty"
                };
                nonameitemtype = d._itemTypeService.CreateObject(nonameitemtype);
                nonameitemtype.Errors.Count().should_not_be(0);
            };

            it["delete_itemtype"] = () =>
            {
                d.typeGlue = d._itemTypeService.SoftDeleteObject(d.typeGlue, d._itemService);
                d.typeGlue.Errors.Count().should_be(0);
            };

            it["delete_itemtype_with_item"] = () =>
            {
                Item glue101 = new Item()
                {
                    ItemTypeId = d.typeGlue.Id,
                    Name = "Glue101",
                    Category = "Glue",
                    Sku = "G101",
                    UoM = "Pcs"
                };
                glue101 = d._itemService.CreateObject(glue101, d._itemTypeService, d._warehouseItemService, d._warehouseService);
                d._itemService.AdjustQuantity(glue101, 100);
                d._warehouseItemService.AdjustQuantity(d._warehouseItemService.GetObjectByWarehouseAndItem(d.localWarehouse.Id, glue101.Id), 100);
                d.typeGlue = d._itemTypeService.SoftDeleteObject(d.typeGlue, d._itemService);
                d.typeGlue.Errors.Count().should_not_be(0);
            };
        }
    }
}