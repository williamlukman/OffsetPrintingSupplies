using Core.DomainModel;
using Core.Interface.Validation;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Validation.Validation
{
    public class BlanketValidator : IBlanketValidator
    {
        public Blanket VHasItemTypeAndIsLegacy(Blanket blanket, IItemTypeService _itemTypeService)
        {
            ItemType itemType = _itemTypeService.GetObjectById(blanket.ItemTypeId);
            if (itemType == null)
            {
                blanket.Errors.Add("ItemTypeId", "Tidak boleh tidak ada");
            }
            else if (!itemType.IsLegacy)
            {
                blanket.Errors.Add("ItemTypeId", "Harus berupa legacy item");
            }
            return blanket;
        }

        public Blanket VHasUniqueSku(Blanket blanket, IBlanketService _blanketService)
        {
            if (String.IsNullOrEmpty(blanket.Sku) || blanket.Sku.Trim() == "")
            {
                blanket.Errors.Add("Sku", "Tidak boleh kosong");
            }
            else if (_blanketService.IsSkuDuplicated(blanket))
            {
                blanket.Errors.Add("Sku", "Tidak boleh diduplikasi");
            }
            return blanket;
        }

        public Blanket VHasName(Blanket blanket)
        {
            if (String.IsNullOrEmpty(blanket.Name) || blanket.Name.Trim() == "")
            {
                blanket.Errors.Add("Name", "Tidak boleh kosong");
            }
            return blanket;
        }

        public Blanket VHasUoM(Blanket blanket, IUoMService _uomService)
        {
            UoM uom = _uomService.GetObjectById(blanket.UoMId);
            if (uom == null)
            {
                blanket.Errors.Add("UoMId", "Tidak terasosiasi dengan Unit of Measurement");
            }
            return blanket;
        }

        public Blanket VHasApplicationCase(Blanket blanket)
        {
            if (blanket.ApplicationCase != Core.Constants.Constant.ApplicationCase.Sheetfed &&
                blanket.ApplicationCase != Core.Constants.Constant.ApplicationCase.Web &&
                blanket.ApplicationCase != Core.Constants.Constant.ApplicationCase.Both)
            {
                blanket.Errors.Add("ApplicationCase", "Harus Sheetfed atau Web");
            }
            return blanket;
        }

        public Blanket VNonNegativeQuantity(Blanket blanket)
        {
            if (blanket.Quantity < 0)
            {
                blanket.Errors.Add("Quantity", "Tidak boleh negatif");
            }
            else if (blanket.PendingDelivery < 0)
            {
                blanket.Errors.Add("Generic", "Pending delivery tidak boleh negatif");
            }
            else if (blanket.PendingReceival < 0)
            {
                blanket.Errors.Add("Generic", "Pending receival tidak boleh negatif");
            }
            return blanket;
        }

        public Blanket VWarehouseQuantityMustBeZero(Blanket blanket, IWarehouseItemService _warehouseItemService)
        {
            IList<WarehouseItem> warehouseItems = _warehouseItemService.GetObjectsByItemId(blanket.Id);
            foreach (var warehouseitem in warehouseItems)
            {
                if (warehouseitem.Quantity > 0)
                {
                    blanket.Errors.Add("Generic", "Quantity di setiap warehouse harus 0");
                    return blanket;
                }
            }
            return blanket;
        }

        public Blanket VHasRollBlanket(Blanket blanket, IItemService _itemService)
        {
            Item rollBlanket = _itemService.GetObjectById(blanket.RollBlanketItemId);
            if (rollBlanket == null)
            {
                blanket.Errors.Add("RollBlanketItemId", "Tidak terasosiasi dengan rollBlanket item");
            }
            return blanket;
        }

        public Blanket VHasContact(Blanket blanket, IContactService _contactService)
        {
            Contact contact = _contactService.GetObjectById(blanket.ContactId);
            if (contact == null)
            {
                blanket.Errors.Add("ContactId", "Tidak terasosiasi dengan contact");
            }
            return blanket;
        }

        public Blanket VHasMachine(Blanket blanket, IMachineService _machineService)
        {
            Machine machine = _machineService.GetObjectById(blanket.MachineId);
            if (machine == null)
            {
                blanket.Errors.Add("MachineId", "Tidak terasosiasi dengan mesin");
            }
            return blanket;
        }

        public Blanket VHasMeasurement(Blanket blanket)
        {
            if (blanket.AC <= 0)
            {
                blanket.Errors.Add("AC", "Tidak boleh negatif atau 0");
            }
            else if (blanket.AR <= 0)
            {
                blanket.Errors.Add("AR", "Tidak boleh negatif atau 0");
            }
            return blanket;
        }

        public Blanket VIfIsBarRequiredThenHasAtLeastOneBar(Blanket blanket)
        {
            if (blanket.IsBarRequired)
            {
                if (!blanket.HasLeftBar && !blanket.HasRightBar)
                {
                    blanket.Errors.Add("Generic", "Jika IsBarRequired, maka blanket harus memiliki bar");
                }
            }
            return blanket;
        }

        public Blanket VIfIsBarNotRequiredThenHasNoBars(Blanket blanket)
        {
            if (!blanket.IsBarRequired)
            {
                if (blanket.HasLeftBar || blanket.HasRightBar)
                {
                    blanket.Errors.Add("Generic", "Jika tidak IsBarRequired, maka blanket tidak boleh memiliki bar");
                }
            }
            return blanket;
        }

        public Blanket VIfHasLeftBarThenLeftBarIsValid(Blanket blanket, IBlanketService _blanketService)
        {
            if (blanket.HasLeftBar)
            {
                Item leftBarItem = _blanketService.GetLeftBarItem(blanket);
                if (leftBarItem == null)
                {
                    blanket.Errors.Add("LeftBarItemId", "Tidak terasosiasi dengan item"); 
                }
            }
            return blanket;
        }

        public Blanket VIfHasRightBarThenRightBarIsValid(Blanket blanket, IBlanketService _blanketService)
        {
            if (blanket.HasRightBar)
            {
                Item rightBarItem = _blanketService.GetRightBarItem(blanket);
                if (rightBarItem == null)
                {
                    blanket.Errors.Add("RightBarItemId", "Tidak terasosiasi dengan item");
                }
            }
            return blanket;
        }

        public Blanket VHasNoStockMutations(Blanket blanket, IStockMutationService _stockMutationService)
        {
            IList<StockMutation> stockMutations = _stockMutationService.GetObjectsByItemId(blanket.Id);
            if (stockMutations.Any())
            {
                blanket.Errors.Add("Generic", "Tidak boleh terasosiasi dengan stock mutation");
            }
            return blanket;
        }

        public Blanket VHasNoPurchaseOrderDetails(Blanket blanket, IPurchaseOrderDetailService _purchaseOrderDetailService)
        {
            IList<PurchaseOrderDetail> purchaseOrderDetails = _purchaseOrderDetailService.GetObjectsByItemId(blanket.Id);
            if (purchaseOrderDetails.Any())
            {
                blanket.Errors.Add("Generic", "Tidak boleh terasosiasi dengan purchase order detail");
            }
            return blanket;
        }

        public Blanket VHasNoStockAdjustmentDetails(Blanket blanket, IStockAdjustmentDetailService _stockAdjustmentDetailService)
        {
            IList<StockAdjustmentDetail> stockAdjustmentDetails = _stockAdjustmentDetailService.GetObjectsByItemId(blanket.Id);
            if (stockAdjustmentDetails.Any())
            {
                blanket.Errors.Add("Generic", "Tidak boleh terasosiasi dengan stock adjustment detail");
            }
            return blanket;
        }

        public Blanket VHasNoSalesOrderDetails(Blanket blanket, ISalesOrderDetailService _salesOrderDetailService)
        {
            IList<SalesOrderDetail> salesOrderDetails = _salesOrderDetailService.GetObjectsByItemId(blanket.Id);
            if (salesOrderDetails.Any())
            {
                blanket.Errors.Add("Generic", "Tidak boleh terasosiasi dengan sales order detail");
            }
            return blanket;
        }

        public Blanket VHasNoBlanketOrderDetails(Blanket blanket, IBlanketOrderDetailService _blanketOrderDetailService)
        {
            IList<BlanketOrderDetail> blanketOrderDetails = _blanketOrderDetailService.GetObjectsByBlanketId(blanket.Id);
            if (blanketOrderDetails.Any())
            {
                blanket.Errors.Add("Generic", "Tidak boleh terasosiasi dengan blanket order detail");
            }
            return blanket;
        }

        public Blanket VCreateObject(Blanket blanket, IBlanketService _blanketService, IUoMService _uomService, IItemService _itemService, IItemTypeService _itemTypeService,
                                     IContactService _contactService, IMachineService _machineService)
        {
            // Item Validation

            VHasItemTypeAndIsLegacy(blanket, _itemTypeService);
            if (!isValid(blanket)) { return blanket; }
            VHasUniqueSku(blanket, _blanketService);
            if (!isValid(blanket)) { return blanket; }
            VHasName(blanket);
            if (!isValid(blanket)) { return blanket; }
            VHasUoM(blanket, _uomService);
            if (!isValid(blanket)) { return blanket; }
            VHasApplicationCase(blanket);
            if (!isValid(blanket)) { return blanket; }
            VNonNegativeQuantity(blanket);
            if (!isValid(blanket)) { return blanket; }
            
            // Bar Validation
            VHasRollBlanket(blanket, _itemService);
            if (!isValid(blanket)) { return blanket; }
            VHasContact(blanket, _contactService);
            if (!isValid(blanket)) { return blanket; }
            VHasMachine(blanket, _machineService);
            if (!isValid(blanket)) { return blanket; }
            VHasMeasurement(blanket);
            if (!isValid(blanket)) { return blanket; }
            VIfIsBarNotRequiredThenHasNoBars(blanket);
            if (!isValid(blanket)) { return blanket; }
            VIfIsBarRequiredThenHasAtLeastOneBar(blanket);
            if (!isValid(blanket)) { return blanket; }
            VIfHasLeftBarThenLeftBarIsValid(blanket, _blanketService);
            if (!isValid(blanket)) { return blanket; }
            VIfHasRightBarThenRightBarIsValid(blanket, _blanketService);
            return blanket;
        }

        public Blanket VUpdateObject(Blanket blanket, IBlanketService _blanketService, IUoMService _uomService, IItemService _itemService, IItemTypeService _itemTypeService,
                                     IContactService _contactService, IMachineService _machineService)
        {
            return VCreateObject(blanket, _blanketService, _uomService, _itemService, _itemTypeService, _contactService, _machineService);
        }

        public Blanket VDeleteObject(Blanket blanket, IItemTypeService _itemTypeService, IWarehouseItemService _warehouseItemService, IPurchaseOrderDetailService _purchaseOrderDetailService,
                                     IStockAdjustmentDetailService _stockAdjustmentDetailService, ISalesOrderDetailService _salesOrderDetailService,
                                     IStockMutationService _stockMutationService, IBlanketOrderDetailService _blanketOrderDetailService)
        {
            VHasItemTypeAndIsLegacy(blanket, _itemTypeService);
            if (!isValid(blanket)) { return blanket; }
            VWarehouseQuantityMustBeZero(blanket, _warehouseItemService);
            if (!isValid(blanket)) { return blanket; }
            VHasNoStockMutations(blanket, _stockMutationService);
            if (!isValid(blanket)) { return blanket; }
            VHasNoBlanketOrderDetails(blanket, _blanketOrderDetailService);
            if (!isValid(blanket)) { return blanket; }
            VHasNoPurchaseOrderDetails(blanket, _purchaseOrderDetailService);
            if (!isValid(blanket)) { return blanket; }
            VHasNoStockAdjustmentDetails(blanket, _stockAdjustmentDetailService);
            if (!isValid(blanket)) { return blanket; }
            VHasNoSalesOrderDetails(blanket, _salesOrderDetailService);
            return blanket;
        }

        public Blanket VAdjustQuantity(Blanket blanket)
        {
            VNonNegativeQuantity(blanket);
            return blanket;
        }

        public Blanket VAdjustPendingReceival(Blanket blanket)
        {
            VNonNegativeQuantity(blanket);
            return blanket;
        }

        public Blanket VAdjustPendingDelivery(Blanket blanket)
        {
            VNonNegativeQuantity(blanket);
            return blanket;
        }

        public bool ValidCreateObject(Blanket blanket, IBlanketService _blanketService, IUoMService _uomService, IItemService _itemService, IItemTypeService _itemTypeService,
                                     IContactService _contactService, IMachineService _machineService)
        {
            VCreateObject(blanket, _blanketService, _uomService, _itemService, _itemTypeService, _contactService, _machineService);
            return isValid(blanket);
        }

        public bool ValidUpdateObject(Blanket blanket, IBlanketService _blanketService, IUoMService _uomService, IItemService _itemService, IItemTypeService _itemTypeService,
                                     IContactService _contactService, IMachineService _machineService)
        {
            blanket.Errors.Clear();
            VUpdateObject(blanket, _blanketService, _uomService, _itemService, _itemTypeService, _contactService, _machineService);
            return isValid(blanket);
        }

        public bool ValidDeleteObject(Blanket blanket, IItemTypeService _itemTypeService, IWarehouseItemService _warehouseItemService, IPurchaseOrderDetailService _purchaseOrderDetailService,
                                     IStockAdjustmentDetailService _stockAdjustmentDetailService, ISalesOrderDetailService _salesOrderDetailService,
                                     IStockMutationService _stockMutationService, IBlanketOrderDetailService _blanketOrderDetailService)
        {
            blanket.Errors.Clear();
            VDeleteObject(blanket, _itemTypeService, _warehouseItemService, _purchaseOrderDetailService, _stockAdjustmentDetailService, _salesOrderDetailService,
                          _stockMutationService, _blanketOrderDetailService);
            return isValid(blanket);
        }

        public bool ValidAdjustQuantity(Blanket blanket)
        {
            blanket.Errors.Clear();
            VAdjustQuantity(blanket);
            return isValid(blanket);
        }

        public bool ValidAdjustPendingReceival(Blanket blanket)
        {
            blanket.Errors.Clear();
            VAdjustPendingReceival(blanket);
            return isValid(blanket);
        }

        public bool ValidAdjustPendingDelivery(Blanket blanket)
        {
            blanket.Errors.Clear();
            VAdjustPendingDelivery(blanket);
            return isValid(blanket);
        }
        
        public bool isValid(Blanket obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public string PrintError(Blanket obj)
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
