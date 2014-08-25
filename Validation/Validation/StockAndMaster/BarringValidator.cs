using Core.DomainModel;
using Core.Interface.Validation;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Validation.Validation
{
    public class BarringValidator : IBarringValidator
    {
        public Barring VHasItemTypeAndIsLegacy(Barring barring, IItemTypeService _itemTypeService)
        {
            ItemType itemType = _itemTypeService.GetObjectById(barring.ItemTypeId);
            if (itemType == null)
            {
                barring.Errors.Add("ItemTypeId", "Tidak boleh tidak ada");
            }
            else if (!itemType.IsLegacy)
            {
                barring.Errors.Add("ItemTypeId", "Harus berupa legacy item");
            }
            return barring;
        }

        public Barring VHasUniqueSku(Barring barring, IBarringService _barringService)
        {
            if (String.IsNullOrEmpty(barring.Sku) || barring.Sku.Trim() == "")
            {
                barring.Errors.Add("Sku", "Tidak boleh kosong");
            }
            else if (_barringService.IsSkuDuplicated(barring))
            {
                barring.Errors.Add("Sku", "Tidak boleh diduplikasi");
            }
            return barring;
        }

        public Barring VHasName(Barring barring)
        {
            if (String.IsNullOrEmpty(barring.Name) || barring.Name.Trim() == "")
            {
                barring.Errors.Add("Name", "Tidak boleh kosong");
            }
            return barring;
        }

        public Barring VHasCategory(Barring barring)
        {
            if (String.IsNullOrEmpty(barring.Category) || barring.Category.Trim() == "")
            {
                barring.Errors.Add("Category", "Tidak boleh kosong");
            }
            return barring;
        }

        public Barring VHasUoM(Barring barring, IUoMService _uomService)
        {
            UoM uom = _uomService.GetObjectById(barring.UoMId);
            if (uom == null)
            {
                barring.Errors.Add("UoMId", "Tidak terasosiasi dengan Unit of Measurement");
            }
            return barring;
        }

        public Barring VNonNegativeQuantity(Barring barring)
        {
            if (barring.Quantity < 0)
            {
                barring.Errors.Add("Quantity", "Tidak boleh negatif");
            }
            else if (barring.PendingDelivery < 0)
            {
                barring.Errors.Add("Generic", "Pending delivery tidak boleh negatif");
            }
            else if (barring.PendingReceival < 0)
            {
                barring.Errors.Add("Generic", "Pending receival tidak boleh negatif");
            }
            return barring;
        }

        public Barring VWarehouseQuantityMustBeZero(Barring barring, IWarehouseItemService _warehouseItemService)
        {
            IList<WarehouseItem> warehouseItems = _warehouseItemService.GetObjectsByItemId(barring.Id);
            foreach (var warehouseitem in warehouseItems)
            {
                if (warehouseitem.Quantity > 0)
                {
                    barring.Errors.Add("Generic", "Quantity di setiap warehouse harus 0");
                    return barring;
                }
            }
            return barring;
        }

        public Barring VHasBlanket(Barring barring, IItemService _itemService)
        {
            Item blanket = _itemService.GetObjectById(barring.BlanketItemId);
            if (blanket == null)
            {
                barring.Errors.Add("BlanketItemId", "Tidak terasosiasi dengan blanket item");
            }
            return barring;
        }

        public Barring VHasContact(Barring barring, IContactService _contactService)
        {
            Contact contact = _contactService.GetObjectById(barring.ContactId);
            if (contact == null)
            {
                barring.Errors.Add("ContactId", "Tidak terasosiasi dengan contact");
            }
            return barring;
        }

        public Barring VHasMachine(Barring barring, IMachineService _machineService)
        {
            Machine machine = _machineService.GetObjectById(barring.MachineId);
            if (machine == null)
            {
                barring.Errors.Add("MachineId", "Tidak terasosiasi dengan mesin");
            }
            return barring;
        }

        public Barring VHasMeasurement(Barring barring)
        {
            if (barring.AC <= 0)
            {
                barring.Errors.Add("AC", "Tidak boleh negatif atau 0");
            }
            else if (barring.AR <= 0)
            {
                barring.Errors.Add("AR", "Tidak boleh negatif atau 0");
            }
            return barring;
        }

        public Barring VIfIsBarRequiredThenHasAtLeastOneBar(Barring barring)
        {
            if (barring.IsBarRequired)
            {
                if (!barring.HasLeftBar && !barring.HasRightBar)
                {
                    barring.Errors.Add("Generic", "Jika IsBarRequired, maka barring harus memiliki bar");
                }
            }
            return barring;
        }

        public Barring VIfIsBarNotRequiredThenHasNoBars(Barring barring)
        {
            if (!barring.IsBarRequired)
            {
                if (barring.HasLeftBar || barring.HasRightBar)
                {
                    barring.Errors.Add("Generic", "Jika tidak IsBarRequired, maka barring tidak boleh memiliki bar");
                }
            }
            return barring;
        }

        public Barring VIfHasLeftBarThenLeftBarIsValid(Barring barring, IBarringService _barringService)
        {
            if (barring.HasLeftBar)
            {
                Item leftBarItem = _barringService.GetLeftBarItem(barring);
                if (leftBarItem == null)
                {
                    barring.Errors.Add("LeftBarItemId", "Tidak terasosiasi dengan item"); 
                }
            }
            return barring;
        }

        public Barring VIfHasRightBarThenRightBarIsValid(Barring barring, IBarringService _barringService)
        {
            if (barring.HasRightBar)
            {
                Item rightBarItem = _barringService.GetRightBarItem(barring);
                if (rightBarItem == null)
                {
                    barring.Errors.Add("RightBarItemId", "Tidak terasosiasi dengan item");
                }
            }
            return barring;
        }

        public Barring VHasNoStockMutations(Barring barring, IStockMutationService _stockMutationService)
        {
            IList<StockMutation> stockMutations = _stockMutationService.GetObjectsByItemId(barring.Id);
            if (stockMutations.Any())
            {
                barring.Errors.Add("Generic", "Tidak boleh terasosiasi dengan stock mutation");
            }
            return barring;
        }

        public Barring VHasNoPurchaseOrderDetails(Barring barring, IPurchaseOrderDetailService _purchaseOrderDetailService)
        {
            IList<PurchaseOrderDetail> purchaseOrderDetails = _purchaseOrderDetailService.GetObjectsByItemId(barring.Id);
            if (purchaseOrderDetails.Any())
            {
                barring.Errors.Add("Generic", "Tidak boleh terasosiasi dengan purchase order detail");
            }
            return barring;
        }

        public Barring VHasNoStockAdjustmentDetails(Barring barring, IStockAdjustmentDetailService _stockAdjustmentDetailService)
        {
            IList<StockAdjustmentDetail> stockAdjustmentDetails = _stockAdjustmentDetailService.GetObjectsByItemId(barring.Id);
            if (stockAdjustmentDetails.Any())
            {
                barring.Errors.Add("Generic", "Tidak boleh terasosiasi dengan stock adjustment detail");
            }
            return barring;
        }

        public Barring VHasNoSalesOrderDetails(Barring barring, ISalesOrderDetailService _salesOrderDetailService)
        {
            IList<SalesOrderDetail> salesOrderDetails = _salesOrderDetailService.GetObjectsByItemId(barring.Id);
            if (salesOrderDetails.Any())
            {
                barring.Errors.Add("Generic", "Tidak boleh terasosiasi dengan sales order detail");
            }
            return barring;
        }

        public Barring VHasNoBarringOrderDetails(Barring barring, IBarringOrderDetailService _barringOrderDetailService)
        {
            IList<BarringOrderDetail> barringOrderDetails = _barringOrderDetailService.GetObjectsByBarringId(barring.Id);
            if (barringOrderDetails.Any())
            {
                barring.Errors.Add("Generic", "Tidak boleh terasosiasi dengan barring order detail");
            }
            return barring;
        }

        public Barring VCreateObject(Barring barring, IBarringService _barringService, IUoMService _uomService, IItemService _itemService, IItemTypeService _itemTypeService,
                                     IContactService _contactService, IMachineService _machineService)
        {
            // Item Validation

            VHasItemTypeAndIsLegacy(barring, _itemTypeService);
            if (!isValid(barring)) { return barring; }
            VHasUniqueSku(barring, _barringService);
            if (!isValid(barring)) { return barring; }
            VHasName(barring);
            if (!isValid(barring)) { return barring; }
            VHasCategory(barring);
            if (!isValid(barring)) { return barring; }
            VHasUoM(barring, _uomService);
            if (!isValid(barring)) { return barring; }
            VNonNegativeQuantity(barring);
            if (!isValid(barring)) { return barring; }
            
            // Bar Validation
            VHasBlanket(barring, _itemService);
            if (!isValid(barring)) { return barring; }
            VHasContact(barring, _contactService);
            if (!isValid(barring)) { return barring; }
            VHasMachine(barring, _machineService);
            if (!isValid(barring)) { return barring; }
            VHasMeasurement(barring);
            if (!isValid(barring)) { return barring; }
            VIfIsBarNotRequiredThenHasNoBars(barring);
            if (!isValid(barring)) { return barring; }
            VIfIsBarRequiredThenHasAtLeastOneBar(barring);
            if (!isValid(barring)) { return barring; }
            VIfHasLeftBarThenLeftBarIsValid(barring, _barringService);
            if (!isValid(barring)) { return barring; }
            VIfHasRightBarThenRightBarIsValid(barring, _barringService);
            return barring;
        }

        public Barring VUpdateObject(Barring barring, IBarringService _barringService, IUoMService _uomService, IItemService _itemService, IItemTypeService _itemTypeService,
                                     IContactService _contactService, IMachineService _machineService)
        {
            return VCreateObject(barring, _barringService, _uomService, _itemService, _itemTypeService, _contactService, _machineService);
        }

        public Barring VDeleteObject(Barring barring, IItemTypeService _itemTypeService, IWarehouseItemService _warehouseItemService, IPurchaseOrderDetailService _purchaseOrderDetailService,
                                     IStockAdjustmentDetailService _stockAdjustmentDetailService, ISalesOrderDetailService _salesOrderDetailService,
                                     IStockMutationService _stockMutationService, IBarringOrderDetailService _barringOrderDetailService)
        {
            VHasItemTypeAndIsLegacy(barring, _itemTypeService);
            if (!isValid(barring)) { return barring; }
            VWarehouseQuantityMustBeZero(barring, _warehouseItemService);
            if (!isValid(barring)) { return barring; }
            VHasNoStockMutations(barring, _stockMutationService);
            if (!isValid(barring)) { return barring; }
            VHasNoBarringOrderDetails(barring, _barringOrderDetailService);
            if (!isValid(barring)) { return barring; }
            VHasNoPurchaseOrderDetails(barring, _purchaseOrderDetailService);
            if (!isValid(barring)) { return barring; }
            VHasNoStockAdjustmentDetails(barring, _stockAdjustmentDetailService);
            if (!isValid(barring)) { return barring; }
            VHasNoSalesOrderDetails(barring, _salesOrderDetailService);
            return barring;
        }

        public Barring VAdjustQuantity(Barring barring)
        {
            VNonNegativeQuantity(barring);
            return barring;
        }

        public Barring VAdjustPendingReceival(Barring barring)
        {
            VNonNegativeQuantity(barring);
            return barring;
        }

        public Barring VAdjustPendingDelivery(Barring barring)
        {
            VNonNegativeQuantity(barring);
            return barring;
        }

        public bool ValidCreateObject(Barring barring, IBarringService _barringService, IUoMService _uomService, IItemService _itemService, IItemTypeService _itemTypeService,
                                     IContactService _contactService, IMachineService _machineService)
        {
            VCreateObject(barring, _barringService, _uomService, _itemService, _itemTypeService, _contactService, _machineService);
            return isValid(barring);
        }

        public bool ValidUpdateObject(Barring barring, IBarringService _barringService, IUoMService _uomService, IItemService _itemService, IItemTypeService _itemTypeService,
                                     IContactService _contactService, IMachineService _machineService)
        {
            barring.Errors.Clear();
            VUpdateObject(barring, _barringService, _uomService, _itemService, _itemTypeService, _contactService, _machineService);
            return isValid(barring);
        }

        public bool ValidDeleteObject(Barring barring, IItemTypeService _itemTypeService, IWarehouseItemService _warehouseItemService, IPurchaseOrderDetailService _purchaseOrderDetailService,
                                     IStockAdjustmentDetailService _stockAdjustmentDetailService, ISalesOrderDetailService _salesOrderDetailService,
                                     IStockMutationService _stockMutationService, IBarringOrderDetailService _barringOrderDetailService)
        {
            barring.Errors.Clear();
            VDeleteObject(barring, _itemTypeService, _warehouseItemService, _purchaseOrderDetailService, _stockAdjustmentDetailService, _salesOrderDetailService,
                          _stockMutationService, _barringOrderDetailService);
            return isValid(barring);
        }

        public bool ValidAdjustQuantity(Barring barring)
        {
            barring.Errors.Clear();
            VAdjustQuantity(barring);
            return isValid(barring);
        }

        public bool ValidAdjustPendingReceival(Barring barring)
        {
            barring.Errors.Clear();
            VAdjustPendingReceival(barring);
            return isValid(barring);
        }

        public bool ValidAdjustPendingDelivery(Barring barring)
        {
            barring.Errors.Clear();
            VAdjustPendingDelivery(barring);
            return isValid(barring);
        }
        
        public bool isValid(Barring obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public string PrintError(Barring obj)
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
