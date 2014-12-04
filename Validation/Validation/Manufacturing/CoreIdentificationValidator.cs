using Core.DomainModel;
using Core.Interface.Validation;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Validation.Validation
{
    public class CoreIdentificationValidator : ICoreIdentificationValidator
    {
        public CoreIdentification VHasUniqueCode(CoreIdentification coreIdentification, ICoreIdentificationService _coreIdentificationService)
        {
            if (String.IsNullOrEmpty(coreIdentification.Code) || coreIdentification.Code.Trim() == "")
            {
                coreIdentification.Errors.Add("Code", "Tidak boleh kosong");
            }
            if (_coreIdentificationService.IsCodeDuplicated(coreIdentification))
            {
                coreIdentification.Errors.Add("Code", "Tidak boleh ada duplikasi");
            }
            return coreIdentification;
        }

        public CoreIdentification VInHouseOrHasContact(CoreIdentification coreIdentification, IContactService _contactService)
        {
            if (/*(coreIdentification.IsInHouse && coreIdentification.ContactId != null) ||*/
                (!coreIdentification.IsInHouse && coreIdentification.ContactId == null))
            {
                coreIdentification.Errors.Add("Generic", "Core Identification harus memilih InHouse atau menyertakan informasi Contact");
            }
            else if (!coreIdentification.IsInHouse && coreIdentification.ContactId != null)
            {
                Contact contact = _contactService.GetObjectById((int)coreIdentification.ContactId);
                if (contact == null)
                {
                    coreIdentification.Errors.Add("ContactId", "Tidak terasosiasi dengan contact");
                }
            }
            return coreIdentification;
        }

        public CoreIdentification VQuantity(CoreIdentification coreIdentification)
        {
            if (coreIdentification.Quantity <= 0)
            {
                coreIdentification.Errors.Add("Quantity", "Tidak boleh 0 atau negatif");
            }
            return coreIdentification;
        }

        public CoreIdentification VHasIdentifiedDate(CoreIdentification coreIdentification)
        {
            // will always be true since date is not null
            if (coreIdentification.IdentifiedDate == null)
            {
                coreIdentification.Errors.Add("IdentifiedDate", "Tidak boleh kosong");
            }
            return coreIdentification;
        }

        public CoreIdentification VHasConfirmationDate(CoreIdentification coreIdentification)
        {
            if (coreIdentification.ConfirmationDate == null)
            {
                coreIdentification.Errors.Add("ConfirmationDate", "Tidak boleh kosong");
            }
            return coreIdentification;
        }

        public CoreIdentification VHasBeenConfirmed(CoreIdentification coreIdentification)
        {
            if (!coreIdentification.IsConfirmed)
            {
                coreIdentification.Errors.Add("Generic", "Belum dikonfirmasi");
            }
            return coreIdentification;
        }

        public CoreIdentification VHasNotBeenConfirmed(CoreIdentification coreIdentification)
        {
            if (coreIdentification.IsConfirmed)
            {
                coreIdentification.Errors.Add("Generic", "Telah dikonfirmasi");
            }
            return coreIdentification;
        }

        public CoreIdentification VHasCoreIdentificationDetails(CoreIdentification coreIdentification, ICoreIdentificationDetailService _coreIdentificationDetailService)
        {
            IList<CoreIdentificationDetail> details = _coreIdentificationDetailService.GetObjectsByCoreIdentificationId(coreIdentification.Id);
            if (!details.Any())
            {
                coreIdentification.Errors.Add("Generic", "Harus membuat core identification detail dahulu");
            }
            return coreIdentification;
        }

        public CoreIdentification VIsInRecoveryOrder(CoreIdentification coreIdentification, IRecoveryOrderService _recoveryOrderService)
        {
            IList<RecoveryOrder> recoveryOrders = _recoveryOrderService.GetObjectsByCoreIdentificationId(coreIdentification.Id);
            if (recoveryOrders.Any())
            {
                coreIdentification.Errors.Add("Generic", "Tidak boleh ada asosiasi dengan Recovery Order");
            }
            return coreIdentification;
        }

        public CoreIdentification VAllDetailsHaveBeenDelivered(CoreIdentification coreIdentification, ICoreIdentificationDetailService _coreIdentificationDetailService)
        {
            IList<CoreIdentificationDetail> details = _coreIdentificationDetailService.GetObjectsByCoreIdentificationId(coreIdentification.Id);
            foreach (var detail in details)
            {
                if (!detail.IsDelivered)
                {
                    coreIdentification.Errors.Add("Generic", "Semua detail harus telah terkirim");
                    return coreIdentification;
                }
            }
            return coreIdentification;
        }

        public CoreIdentification VAllDetailsAreConfirmable(CoreIdentification coreIdentification, ICoreIdentificationDetailService _coreIdentificationDetailService, ICoreIdentificationService _coreIdentificationService, ICoreBuilderService _coreBuilderService, IWarehouseItemService _warehouseItemService)
        {
            IList<CoreIdentificationDetail> details = _coreIdentificationDetailService.GetObjectsByCoreIdentificationId(coreIdentification.Id);
            foreach (var detail in details)
            {
                detail.Errors = new Dictionary<string, string>();
                detail.ConfirmationDate = coreIdentification.ConfirmationDate;
                if (!_coreIdentificationDetailService.GetValidator().ValidConfirmObject(detail, _coreIdentificationService, _coreIdentificationDetailService, _coreBuilderService, _warehouseItemService))
                {
                    coreIdentification.Errors.Add("Generic", detail.Errors.FirstOrDefault().Key + " " + detail.Errors.FirstOrDefault().Value);
                    return coreIdentification;
                }
            }
            return coreIdentification;
        }

        public CoreIdentification VAllDetailsAreUnconfirmable(CoreIdentification coreIdentification, ICoreIdentificationDetailService _coreIdentificationDetailService, ICoreIdentificationService _coreIdentificationService, ICoreBuilderService _coreBuilderService, IWarehouseItemService _warehouseItemService, ICustomerItemService _customerItemService)
        {
            IList<CoreIdentificationDetail> details = _coreIdentificationDetailService.GetObjectsByCoreIdentificationId(coreIdentification.Id);
            foreach (var detail in details)
            {
                detail.Errors = new Dictionary<string, string>();
                if (!_coreIdentificationDetailService.GetValidator().ValidUnconfirmObject(detail, _coreIdentificationService, _coreIdentificationDetailService, _coreBuilderService, _warehouseItemService, _customerItemService))
                {
                    coreIdentification.Errors.Add("Generic", detail.Errors.FirstOrDefault().Key + " " + detail.Errors.FirstOrDefault().Value);
                    return coreIdentification;
                }
            }
            return coreIdentification;
        }
        
        public CoreIdentification VQuantityEqualNumberOfDetails(CoreIdentification coreIdentification, ICoreIdentificationDetailService _coreIdentificationDetailService)
        {
            IList<CoreIdentificationDetail> details = _coreIdentificationDetailService.GetObjectsByCoreIdentificationId(coreIdentification.Id);
            if (coreIdentification.Quantity != details.Count())
            {
                coreIdentification.Errors.Add("Generic", "Quantity tidak sama dengan jumlah Core Identification Detail " + details.Count());
            }
            return coreIdentification;
        }

        // Hanya untuk InHouse Production
        public CoreIdentification VQuantityIsInStock(CoreIdentification coreIdentification, ICoreIdentificationDetailService _coreIdentificationDetailService,
                                                     ICoreBuilderService _coreBuilderService, IItemService _itemService, IWarehouseItemService _warehouseItemService)
        {
            if (!coreIdentification.IsInHouse) { return coreIdentification; }

            IList<CoreIdentificationDetail> details = _coreIdentificationDetailService.GetObjectsByCoreIdentificationId(coreIdentification.Id);
            IDictionary<int, int> ValuePairWarehouseItemIdQuantity = new Dictionary<int, int>();
            
            foreach (var detail in details)
            {
                CoreBuilder coreBuilder = _coreBuilderService.GetObjectById(detail.CoreBuilderId);
                Item item = (detail.MaterialCase == Core.Constants.Constant.MaterialCase.New) ?
                            _itemService.GetObjectById(coreBuilder.NewCoreItemId) : _itemService.GetObjectById(coreBuilder.UsedCoreItemId);
                WarehouseItem warehouseItem = _warehouseItemService.FindOrCreateObject(coreIdentification.WarehouseId, item.Id);
                if (ValuePairWarehouseItemIdQuantity.ContainsKey(warehouseItem.Id))
                {
                    ValuePairWarehouseItemIdQuantity[warehouseItem.Id] += 1;
                }
                else
                {
                    ValuePairWarehouseItemIdQuantity.Add(warehouseItem.Id, 1);
                }
            }

            foreach (var ValuePair in ValuePairWarehouseItemIdQuantity)
            {
                WarehouseItem warehouseItem = _warehouseItemService.GetObjectById(ValuePair.Key);
                if (warehouseItem.Quantity < ValuePair.Value)
                {
                    coreIdentification.Errors.Add("Generic", "Stock di warehouseId: " + warehouseItem.WarehouseId + ", itemId: " + warehouseItem.ItemId + " jumlah: " + warehouseItem.Quantity + " tidak mencukupi untuk melakukan Core Identification");
                    return coreIdentification;
                }
            }
            return coreIdentification;
        }

        public CoreIdentification VCreateObject(CoreIdentification coreIdentification, ICoreIdentificationService _coreIdentificationService, IContactService _contactService)
        {
            VHasUniqueCode(coreIdentification, _coreIdentificationService);
            if (!isValid(coreIdentification)) { return coreIdentification; }
            VInHouseOrHasContact(coreIdentification, _contactService);
            if (!isValid(coreIdentification)) { return coreIdentification; }
            VQuantity(coreIdentification);
            if (!isValid(coreIdentification)) { return coreIdentification; }
            VHasIdentifiedDate(coreIdentification);
            return coreIdentification;
        }

        public CoreIdentification VUpdateObject(CoreIdentification coreIdentification, ICoreIdentificationService _coreIdentificationService, IContactService _contactService)
        {
            VCreateObject(coreIdentification, _coreIdentificationService, _contactService);
            if (!isValid(coreIdentification)) { return coreIdentification; }
            VHasNotBeenConfirmed(coreIdentification);
            return coreIdentification;
        }

        public CoreIdentification VDeleteObject(CoreIdentification coreIdentification, IRecoveryOrderService _recoveryOrderService)
        {
            VHasNotBeenConfirmed(coreIdentification);
            if (!isValid(coreIdentification)) { return coreIdentification; }
            VIsInRecoveryOrder(coreIdentification, _recoveryOrderService);
            return coreIdentification;
        }

        public CoreIdentification VConfirmObject(CoreIdentification coreIdentification, ICoreIdentificationDetailService _coreIdentificationDetailService,
                                                 ICoreBuilderService _coreBuilderService, IItemService _itemService, IWarehouseItemService _warehouseItemService, ICoreIdentificationService _coreIdentificationService)
        {
            VHasConfirmationDate(coreIdentification);
            if (!isValid(coreIdentification)) { return coreIdentification; }
            VHasCoreIdentificationDetails(coreIdentification, _coreIdentificationDetailService);
            if (!isValid(coreIdentification)) { return coreIdentification; }
            VQuantityEqualNumberOfDetails(coreIdentification, _coreIdentificationDetailService);
            if (!isValid(coreIdentification)) { return coreIdentification; }
            VHasNotBeenConfirmed(coreIdentification);
            if (!isValid(coreIdentification)) { return coreIdentification; }
            VQuantityIsInStock(coreIdentification, _coreIdentificationDetailService, _coreBuilderService, _itemService, _warehouseItemService);
            if (!isValid(coreIdentification)) { return coreIdentification; }
            VAllDetailsAreConfirmable(coreIdentification, _coreIdentificationDetailService, _coreIdentificationService, _coreBuilderService, _warehouseItemService);
            return coreIdentification;
        }

        public CoreIdentification VUnconfirmObject(CoreIdentification coreIdentification, IRecoveryOrderService _recoveryOrderService, ICoreIdentificationDetailService _coreIdentificationDetailService,
                                                 ICoreBuilderService _coreBuilderService, IWarehouseItemService _warehouseItemService, ICoreIdentificationService _coreIdentificationService, ICustomerItemService _customerItemService)
        {
            VHasBeenConfirmed(coreIdentification);
            if (!isValid(coreIdentification)) { return coreIdentification; }
            VIsInRecoveryOrder(coreIdentification, _recoveryOrderService);
            if (!isValid(coreIdentification)) { return coreIdentification; }
            VAllDetailsAreUnconfirmable(coreIdentification, _coreIdentificationDetailService, _coreIdentificationService, _coreBuilderService, _warehouseItemService, _customerItemService);
            return coreIdentification;
        }

        public CoreIdentification VCompleteObject(CoreIdentification coreIdentification, ICoreIdentificationDetailService _coreIdentificationDetailService)
        {
            VAllDetailsHaveBeenDelivered(coreIdentification, _coreIdentificationDetailService);
            return coreIdentification;
        }
        public bool ValidCreateObject(CoreIdentification coreIdentification, ICoreIdentificationService _coreIdentificationService, IContactService _contactService)
        {
            VCreateObject(coreIdentification, _coreIdentificationService, _contactService);
            return isValid(coreIdentification);
        }

        public bool ValidUpdateObject(CoreIdentification coreIdentification, ICoreIdentificationService _coreIdentificationService, IContactService _contactService)
        {
            coreIdentification.Errors.Clear();
            VUpdateObject(coreIdentification, _coreIdentificationService, _contactService);
            return isValid(coreIdentification);
        }

        public bool ValidDeleteObject(CoreIdentification coreIdentification, IRecoveryOrderService _recoveryOrderService)
        {
            coreIdentification.Errors.Clear();
            VDeleteObject(coreIdentification, _recoveryOrderService);
            return isValid(coreIdentification);
        }

        public bool ValidConfirmObject(CoreIdentification coreIdentification, ICoreIdentificationDetailService _coreIdentificationDetailService,
                                       ICoreBuilderService _coreBuilderService, IItemService _itemService, IWarehouseItemService _warehouseItemService, ICoreIdentificationService _coreIdentificationService)
        {
            coreIdentification.Errors.Clear();
            VConfirmObject(coreIdentification, _coreIdentificationDetailService, _coreBuilderService, _itemService, _warehouseItemService, _coreIdentificationService);
            return isValid(coreIdentification);
        }

        public bool ValidUnconfirmObject(CoreIdentification coreIdentification, IRecoveryOrderService _recoveryOrderService, ICoreIdentificationDetailService _coreIdentificationDetailService,
                                                 ICoreBuilderService _coreBuilderService, IWarehouseItemService _warehouseItemService, ICoreIdentificationService _coreIdentificationService, ICustomerItemService _customerItemService)
        {
            coreIdentification.Errors.Clear();
            VUnconfirmObject(coreIdentification, _recoveryOrderService, _coreIdentificationDetailService, _coreBuilderService, _warehouseItemService, _coreIdentificationService, _customerItemService);
            return isValid(coreIdentification);
        }

        public bool ValidCompleteObject(CoreIdentification coreIdentification, ICoreIdentificationDetailService _coreIdentificationDetailService)
        {
            coreIdentification.Errors.Clear();
            VCompleteObject(coreIdentification, _coreIdentificationDetailService);
            return isValid(coreIdentification);
        }

        public bool isValid(CoreIdentification obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public string PrintError(CoreIdentification obj)
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
