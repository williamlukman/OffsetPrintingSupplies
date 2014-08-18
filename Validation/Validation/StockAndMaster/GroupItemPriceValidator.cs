using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.DomainModel;
using Core.Interface.Service;
using Core.Interface.Validation;

namespace Validation.Validation
{
    public class GroupItemPriceValidator : IGroupItemPriceValidator
    {
        public GroupItemPrice VNonNegativePrice(GroupItemPrice groupItemPrice)
        {
            if (groupItemPrice.Price < 0)
            {
                groupItemPrice.Errors.Add("Price", "Tidak boleh negative");
            }
            return groupItemPrice;
        }

        public GroupItemPrice VHasDifferentPrice(GroupItemPrice groupItemPrice, IPriceMutationService _priceMutationService)
        {
            IList<PriceMutation> priceMutations = _priceMutationService.GetObjectsByIsActive(true, groupItemPrice.ItemId/*, groupItemPrice.ContactGroupId*/, 0);
            foreach (var priceMutation in priceMutations)
            {
                if (groupItemPrice.Price == priceMutation.Amount)
                {
                    groupItemPrice.Errors.Add("Price", "Tidak boleh sama dengan current active price");
                    return groupItemPrice;
                }
            }
            return groupItemPrice;
        }

        public GroupItemPrice VSameGroupId(GroupItemPrice groupItemPrice, int oldGroupId)
        {
            if (groupItemPrice.ContactGroupId != oldGroupId)
            {
                groupItemPrice.Errors.Add("ContactGroupId", "Tidak boleh beda");
            }
            return groupItemPrice;
        }

        public GroupItemPrice VSameItemId(GroupItemPrice groupItemPrice, int oldItemId)
        {
            if (groupItemPrice.ItemId != oldItemId)
            {
                groupItemPrice.Errors.Add("ItemId", "Tidak boleh beda");
            }
            return groupItemPrice;
        }

        public GroupItemPrice VHasUniqueIdCombination(GroupItemPrice groupItemPrice, IGroupItemPriceService _groupItemPriceService)
        {
            if (_groupItemPriceService.IsGroupItemPriceDuplicated(groupItemPrice))
            {
                groupItemPrice.Errors.Add("Generic", "Kombinasi ItemId & GroupId Tidak boleh ada duplikasi");
            }
            return groupItemPrice;
        }

        public GroupItemPrice VIsValidGroup(GroupItemPrice groupItemPrice, IContactGroupService _contactGroupService)
        {
            ContactGroup contactGroup = _contactGroupService.GetObjectById(groupItemPrice.ContactGroupId);
            if (contactGroup == null)
            {
                groupItemPrice.Errors.Add("ContactGroupId", "Tidak valid");
            }
            return groupItemPrice;
        }

        public GroupItemPrice VIsValidItem(GroupItemPrice groupItemPrice, IItemService _itemService)
        {
            Item item = _itemService.GetObjectById(groupItemPrice.ItemId);
            if (item == null)
            {
                groupItemPrice.Errors.Add("ItemId", "Tidak valid");
            }
            return groupItemPrice;
        }

        public GroupItemPrice VCreateObject(GroupItemPrice groupItemPrice, IGroupItemPriceService _groupItemPriceService, IContactGroupService _contactGroupService, IItemService _itemService)
        {
            VNonNegativePrice(groupItemPrice);
            if (!isValid(groupItemPrice)) { return groupItemPrice; }
            VHasUniqueIdCombination(groupItemPrice, _groupItemPriceService);
            if (!isValid(groupItemPrice)) { return groupItemPrice; }
            VIsValidGroup(groupItemPrice, _contactGroupService);
            if (!isValid(groupItemPrice)) { return groupItemPrice; }
            VIsValidItem(groupItemPrice, _itemService);
            return groupItemPrice;
        }

        public GroupItemPrice VUpdateObject(GroupItemPrice groupItemPrice, int oldGroupId, int oldItemId, IGroupItemPriceService _groupItemPriceService, IPriceMutationService _priceMutationService)
        {
            // TODO Bugfix: oldgroupItemPrice seems to have the same content with groupItemPrice causing new itemid/groupid looks the same with old id
            //GroupItemPrice oldgroupItemPrice = _groupItemPriceService.GetObjectById(groupItemPrice.Id);
            VSameGroupId(groupItemPrice, oldGroupId);
            if (!isValid(groupItemPrice)) { return groupItemPrice; }
            VSameItemId(groupItemPrice, oldItemId);
            if (!isValid(groupItemPrice)) { return groupItemPrice; }
            VHasDifferentPrice(groupItemPrice, _priceMutationService);
            if (!isValid(groupItemPrice)) { return groupItemPrice; }
            VNonNegativePrice(groupItemPrice);
            return groupItemPrice;
        }

        public GroupItemPrice VDeleteObject(GroupItemPrice groupItemPrice)
        {
            
            return groupItemPrice;
        }

        public bool ValidCreateObject(GroupItemPrice groupItemPrice, IGroupItemPriceService _groupItemPriceService, IContactGroupService _contactGroupService, IItemService _itemService)
        {
            VCreateObject(groupItemPrice, _groupItemPriceService, _contactGroupService, _itemService);
            return isValid(groupItemPrice);
        }

        public bool ValidUpdateObject(GroupItemPrice groupItemPrice, int oldGroupId, int oldItemId, IGroupItemPriceService _groupItemPriceService, IPriceMutationService _priceMutationService)
        {
            groupItemPrice.Errors.Clear();
            VUpdateObject(groupItemPrice, oldGroupId, oldItemId, _groupItemPriceService, _priceMutationService);
            return isValid(groupItemPrice);
        }

        public bool ValidDeleteObject(GroupItemPrice groupItemPrice)
        {
            groupItemPrice.Errors.Clear();
            VDeleteObject(groupItemPrice);
            return isValid(groupItemPrice);
        }

        public bool isValid(GroupItemPrice obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public string PrintError(GroupItemPrice obj)
        {
            string erroroutput = "";
            KeyValuePair<string, string> first = obj.Errors.ElementAt(0);
            erroroutput += first.Key + "," + first.Value;
            foreach (KeyValuePair<string, string> pair in obj.Errors.Skip(1))
            {
                erroroutput += Environment.NewLine;
                erroroutput += pair.Key + "," + pair.Value;
            }
            return erroroutput;
        }

    }
}
