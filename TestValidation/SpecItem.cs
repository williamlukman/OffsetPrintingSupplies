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

    public class SpecItem: nspec
    {
        Item item;
        ItemType itemType;
        IItemService _itemService;
        IItemTypeService _itemTypeService;
        IRecoveryOrderDetailService _recoveryOrderDetailService;
        IRecoveryAccessoryDetailService _recoveryAccessoryDetailService;
        ICoreBuilderService _coreBuilderService;
        IRollerBuilderService _rollerBuilderService;

        void before_each()
        {
            var db = new OffsetPrintingSuppliesEntities();
            using (db)
            {
                db.DeleteAllTables();
                _itemService = new ItemService(new ItemRepository(), new ItemValidator());
                _itemTypeService = new ItemTypeService(new ItemTypeRepository(), new ItemTypeValidator());
                itemType = _itemTypeService.CreateObject("Accessory", "Accessory");
                _recoveryOrderDetailService = new RecoveryOrderDetailService(new RecoveryOrderDetailRepository(), new RecoveryOrderDetailValidator());
                _recoveryAccessoryDetailService = new RecoveryAccessoryDetailService(new RecoveryAccessoryDetailRepository(), new RecoveryAccessoryDetailValidator());
                _coreBuilderService = new CoreBuilderService(new CoreBuilderRepository(), new CoreBuilderValidator());
                _rollerBuilderService = new RollerBuilderService(new RollerBuilderRepository(), new RollerBuilderValidator());
                item = new Item()
                {
                    ItemTypeId = itemType.Id,
                    Sku = "ABC1001",
                    Name = "ABC",
                    Category = "ABC123",
                    UoM = "Pcs",
                    Quantity = 0
                };
                item = _itemService.CreateObject(item, _itemTypeService);
            }
        }

        /*
         * STEPS:
         * 1. Create valid item
         * 2. Create invalid item with no name
         * 3. Create invalid items with same SKU
         * 4a. Delete item
         * 4b. Delete item with stock mutations
         */
        void contact_validation()
        {
        
            it["validates_item"] = () =>
            {
                item.Errors.Count().should_be(0);
            };

            it["delete_item"] = () =>
            {
                item = _itemService.SoftDeleteObject(item, _recoveryOrderDetailService, _recoveryAccessoryDetailService, _coreBuilderService, _rollerBuilderService);
                item.Errors.Count().should_be(0);
            };
        }
    }
}