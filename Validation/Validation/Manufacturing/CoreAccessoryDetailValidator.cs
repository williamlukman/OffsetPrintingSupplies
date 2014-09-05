using Core.DomainModel;
using Core.Interface.Validation;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Validation.Validation
{
    public class CoreAccessoryDetailValidator : ICoreAccessoryDetailValidator
    {
        public CoreAccessoryDetail VHasCoreIdentificationDetail(CoreAccessoryDetail coreAccessoryDetail, ICoreIdentificationDetailService _coreIdentificationDetailService)
        {
            CoreIdentificationDetail detail = _coreIdentificationDetailService.GetObjectById(coreAccessoryDetail.CoreIdentificationDetailId);
            if (detail == null)
            {
                coreAccessoryDetail.Errors.Add("Generic", "Tidak terasosiasi dengan Core Order Detail");
            }
            return coreAccessoryDetail;
        }

       public CoreAccessoryDetail VIsAccessory(CoreAccessoryDetail coreAccessoryDetail, IItemService _itemService, IItemTypeService _itemTypeService)
        {
            Item item = _itemService.GetObjectById(coreAccessoryDetail.ItemId);
            ItemType itemType = _itemTypeService.GetObjectByName(Core.Constants.Constant.ItemTypeCase.Accessory);
            if (item.ItemTypeId != itemType.Id)
            {
                coreAccessoryDetail.Errors.Add("ItemId", "Bukan sebuah accessory");
            }
            return coreAccessoryDetail;
        }

        public CoreAccessoryDetail VNonNegativeNorZeroQuantity(CoreAccessoryDetail coreAccessoryDetail)
        {
            if (coreAccessoryDetail.Quantity <= 0)
            {
                coreAccessoryDetail.Errors.Add("Quantity", "Tidak boleh 0 atau negatif");
            }
            return coreAccessoryDetail;
        }

        public CoreAccessoryDetail VHasUniqueDetailId(CoreAccessoryDetail coreAccessorrynDetail, ICoreAccessoryDetailService _coreAccessoryDetailService)
        {
            IList<CoreAccessoryDetail> details = _coreAccessoryDetailService.GetObjectsByCoreIdentificationDetailId(coreAccessorrynDetail.CoreIdentificationDetailId);
            foreach (var detail in details)
            {
                if (detail.ItemId == coreAccessorrynDetail.ItemId && detail.Id != coreAccessorrynDetail.Id)
                {
                    coreAccessorrynDetail.Errors.Add("ItemId", "Tidak boleh di duplikasi");
                    return coreAccessorrynDetail;
                }
            }
            return coreAccessorrynDetail;
        }

      

        public CoreAccessoryDetail VCreateObject(CoreAccessoryDetail coreAccessoryDetail, ICoreIdentificationService _coreIdentificationService, ICoreIdentificationDetailService _coreIdentificationDetailService,
                                                     IItemService _itemService, IItemTypeService _itemTypeService, IWarehouseItemService _warehouseItemService, ICoreAccessoryDetailService _coreAccessoryDetailService)
        {
            VHasCoreIdentificationDetail(coreAccessoryDetail, _coreIdentificationDetailService);
            if (!isValid(coreAccessoryDetail)) { return coreAccessoryDetail; }
            VIsAccessory(coreAccessoryDetail, _itemService, _itemTypeService);
            if (!isValid(coreAccessoryDetail)) { return coreAccessoryDetail; }
            VNonNegativeNorZeroQuantity(coreAccessoryDetail);
            if (!isValid(coreAccessoryDetail)) { return coreAccessoryDetail; }
            VHasUniqueDetailId(coreAccessoryDetail, _coreAccessoryDetailService);
            return coreAccessoryDetail;
        }

        public CoreAccessoryDetail VUpdateObject(CoreAccessoryDetail coreAccessoryDetail, ICoreIdentificationService _coreIdentificationService, ICoreIdentificationDetailService _coreIdentificationDetailService,
                                                     IItemService _itemService, IItemTypeService _itemTypeService, IWarehouseItemService _warehouseItemService,ICoreAccessoryDetailService _coreAccessoryDetailService)
        {
            VCreateObject(coreAccessoryDetail, _coreIdentificationService, _coreIdentificationDetailService, _itemService, _itemTypeService, _warehouseItemService, _coreAccessoryDetailService);
            if (!isValid(coreAccessoryDetail)) { return coreAccessoryDetail; }
            return coreAccessoryDetail;
        }

        public CoreAccessoryDetail VDeleteObject(CoreAccessoryDetail coreAccessoryDetail, ICoreIdentificationDetailService _coreIdentificationDetailService)
        {
            return coreAccessoryDetail;
        }

        public bool ValidCreateObject(CoreAccessoryDetail coreAccessoryDetail, ICoreIdentificationService _coreIdentificationService, ICoreIdentificationDetailService _coreIdentificationDetailService,
                                      IItemService _itemService, IItemTypeService _itemTypeService, IWarehouseItemService _warehouseItemService, ICoreAccessoryDetailService _coreAccessoryDetailService)
        {
            VCreateObject(coreAccessoryDetail, _coreIdentificationService, _coreIdentificationDetailService, _itemService, _itemTypeService, _warehouseItemService,_coreAccessoryDetailService);
            return isValid(coreAccessoryDetail);
        }

        public bool ValidUpdateObject(CoreAccessoryDetail coreAccessoryDetail, ICoreIdentificationService _coreIdentificationService, ICoreIdentificationDetailService _coreIdentificationDetailService,
                                      IItemService _itemService, IItemTypeService _itemTypeService, IWarehouseItemService _warehouseItemService, ICoreAccessoryDetailService _coreAccessoryDetailService)
        {
            coreAccessoryDetail.Errors.Clear();
            VUpdateObject(coreAccessoryDetail, _coreIdentificationService, _coreIdentificationDetailService, _itemService, _itemTypeService, _warehouseItemService,_coreAccessoryDetailService);
            return isValid(coreAccessoryDetail);
        }

        public bool ValidDeleteObject(CoreAccessoryDetail coreAccessoryDetail, ICoreIdentificationDetailService _coreIdentificationDetailService)
        {
            coreAccessoryDetail.Errors.Clear();
            VDeleteObject(coreAccessoryDetail, _coreIdentificationDetailService);
            return isValid(coreAccessoryDetail);
        }

        public bool isValid(CoreAccessoryDetail obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public string PrintError(CoreAccessoryDetail obj)
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
