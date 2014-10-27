using Core.DomainModel;
using Core.Interface.Validation;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Validation.Validation
{
    public class CompoundValidator : ICompoundValidator
    {
        public Compound VHasExpiryDate(Compound compound)
        {
            if (compound.ExpiryDate == null)
            {
                compound.Errors.Add("ExpiryDate", "Tidak boleh kosong");
            }
            return compound;
        }

        public Compound VHasItemTypeAndIsLegacy(Compound compound, IItemTypeService _itemTypeService)
        {
            ItemType itemType = _itemTypeService.GetObjectById(compound.ItemTypeId);
            if (itemType == null)
            {
                compound.Errors.Add("ItemTypeId", "Tidak boleh tidak ada");
            }
            else if (!itemType.IsLegacy)
            {
                compound.Errors.Add("ItemTypeId", "Harus berupa legacy item");
            }
            return compound;
        }

        public Compound VHasUniqueSku(Compound compound, ICompoundService _compoundService)
        {
            if (String.IsNullOrEmpty(compound.Sku) || compound.Sku.Trim() == "")
            {
                compound.Errors.Add("Sku", "Tidak boleh kosong");
            }
            else if (_compoundService.IsSkuDuplicated(compound))
            {
                compound.Errors.Add("Sku", "Tidak boleh diduplikasi");
            }
            return compound;
        }

        public Compound VHasName(Compound compound)
        {
            if (String.IsNullOrEmpty(compound.Name) || compound.Name.Trim() == "")
            {
                compound.Errors.Add("Name", "Tidak boleh kosong");
            }
            return compound;
        }

        public Compound VHasUoM(Compound compound, IUoMService _uomService)
        {
            UoM uom = _uomService.GetObjectById(compound.UoMId);
            if (uom == null)
            {
                compound.Errors.Add("UoMId", "Tidak terasosiasi dengan Unit of Measurement");
            }
            return compound;
        }

        public Compound VNonNegativeQuantity(Compound compound)
        {
            if (compound.Quantity < 0)
            {
                compound.Errors.Add("Quantity", "Tidak boleh negatif");
            }
            else if (compound.PendingDelivery < 0)
            {
                compound.Errors.Add("Generic", "Pending delivery tidak boleh negatif");
            }
            else if (compound.PendingReceival < 0)
            {
                compound.Errors.Add("Generic", "Pending receival tidak boleh negatif");
            }
            return compound;
        }

        public Compound VWarehouseQuantityMustBeZero(Compound compound, IWarehouseItemService _warehouseItemService)
        {
            IList<WarehouseItem> warehouseItems = _warehouseItemService.GetObjectsByItemId(compound.Id);
            foreach (var warehouseitem in warehouseItems)
            {
                if (warehouseitem.Quantity > 0)
                {
                    compound.Errors.Add("Generic", "Quantity di setiap warehouse harus 0");
                    return compound;
                }
            }
            return compound;
        }

        public Compound VHasNoStockMutations(Compound compound, IStockMutationService _stockMutationService)
        {
            IList<StockMutation> stockMutations = _stockMutationService.GetObjectsByItemId(compound.Id);
            if (stockMutations.Any())
            {
                compound.Errors.Add("Generic", "Tidak boleh terasosiasi dengan stock mutation");
            }
            return compound;
        }

        public Compound VHasNoPurchaseOrderDetails(Compound compound, IPurchaseOrderDetailService _purchaseOrderDetailService)
        {
            IList<PurchaseOrderDetail> purchaseOrderDetails = _purchaseOrderDetailService.GetObjectsByItemId(compound.Id);
            if (purchaseOrderDetails.Any())
            {
                compound.Errors.Add("Generic", "Tidak boleh terasosiasi dengan purchase order detail");
            }
            return compound;
        }

        public Compound VHasNoStockAdjustmentDetails(Compound compound, IStockAdjustmentDetailService _stockAdjustmentDetailService)
        {
            IList<StockAdjustmentDetail> stockAdjustmentDetails = _stockAdjustmentDetailService.GetObjectsByItemId(compound.Id);
            if (stockAdjustmentDetails.Any())
            {
                compound.Errors.Add("Generic", "Tidak boleh terasosiasi dengan stock adjustment detail");
            }
            return compound;
        }

        public Compound VHasNoSalesOrderDetails(Compound compound, ISalesOrderDetailService _salesOrderDetailService)
        {
            IList<SalesOrderDetail> salesOrderDetails = _salesOrderDetailService.GetObjectsByItemId(compound.Id);
            if (salesOrderDetails.Any())
            {
                compound.Errors.Add("Generic", "Tidak boleh terasosiasi dengan sales order detail");
            }
            return compound;
        }

        public Compound VCreateObject(Compound compound, ICompoundService _compoundService, IUoMService _uomService, IItemService _itemService, IItemTypeService _itemTypeService)
        {
            // Item Validation

            VHasItemTypeAndIsLegacy(compound, _itemTypeService);
            if (!isValid(compound)) { return compound; }
            VHasUniqueSku(compound, _compoundService);
            if (!isValid(compound)) { return compound; }
            VHasName(compound);
            if (!isValid(compound)) { return compound; }
            VHasUoM(compound, _uomService);
            if (!isValid(compound)) { return compound; }
            VNonNegativeQuantity(compound);
            if (!isValid(compound)) { return compound; }
            VHasExpiryDate(compound);            
            return compound;
        }

        public Compound VUpdateObject(Compound compound, ICompoundService _compoundService, IUoMService _uomService, IItemService _itemService, IItemTypeService _itemTypeService)
        {
            return VCreateObject(compound, _compoundService, _uomService, _itemService, _itemTypeService);
        }

        public Compound VDeleteObject(Compound compound, IItemTypeService _itemTypeService, IWarehouseItemService _warehouseItemService, IPurchaseOrderDetailService _purchaseOrderDetailService,
                                     IStockAdjustmentDetailService _stockAdjustmentDetailService, ISalesOrderDetailService _salesOrderDetailService,
                                     IStockMutationService _stockMutationService)
        {
            VHasItemTypeAndIsLegacy(compound, _itemTypeService);
            if (!isValid(compound)) { return compound; }
            VWarehouseQuantityMustBeZero(compound, _warehouseItemService);
            if (!isValid(compound)) { return compound; }
            VHasNoStockMutations(compound, _stockMutationService);
            if (!isValid(compound)) { return compound; }
            VHasNoPurchaseOrderDetails(compound, _purchaseOrderDetailService);
            if (!isValid(compound)) { return compound; }
            VHasNoStockAdjustmentDetails(compound, _stockAdjustmentDetailService);
            if (!isValid(compound)) { return compound; }
            VHasNoSalesOrderDetails(compound, _salesOrderDetailService);
            return compound;
        }

        public Compound VAdjustQuantity(Compound compound)
        {
            VNonNegativeQuantity(compound);
            return compound;
        }

        public Compound VAdjustPendingReceival(Compound compound)
        {
            VNonNegativeQuantity(compound);
            return compound;
        }

        public Compound VAdjustPendingDelivery(Compound compound)
        {
            VNonNegativeQuantity(compound);
            return compound;
        }

        public bool ValidCreateObject(Compound compound, ICompoundService _compoundService, IUoMService _uomService, IItemService _itemService, IItemTypeService _itemTypeService)
        {
            VCreateObject(compound, _compoundService, _uomService, _itemService, _itemTypeService);
            return isValid(compound);
        }

        public bool ValidUpdateObject(Compound compound, ICompoundService _compoundService, IUoMService _uomService, IItemService _itemService, IItemTypeService _itemTypeService)
        {
            compound.Errors.Clear();
            VUpdateObject(compound, _compoundService, _uomService, _itemService, _itemTypeService);
            return isValid(compound);
        }

        public bool ValidDeleteObject(Compound compound, IItemTypeService _itemTypeService, IWarehouseItemService _warehouseItemService, IPurchaseOrderDetailService _purchaseOrderDetailService,
                                     IStockAdjustmentDetailService _stockAdjustmentDetailService, ISalesOrderDetailService _salesOrderDetailService,
                                     IStockMutationService _stockMutationService)
        {
            compound.Errors.Clear();
            VDeleteObject(compound, _itemTypeService, _warehouseItemService, _purchaseOrderDetailService, _stockAdjustmentDetailService, _salesOrderDetailService,
                          _stockMutationService);
            return isValid(compound);
        }

        public bool ValidAdjustQuantity(Compound compound)
        {
            compound.Errors.Clear();
            VAdjustQuantity(compound);
            return isValid(compound);
        }

        public bool ValidAdjustPendingReceival(Compound compound)
        {
            compound.Errors.Clear();
            VAdjustPendingReceival(compound);
            return isValid(compound);
        }

        public bool ValidAdjustPendingDelivery(Compound compound)
        {
            compound.Errors.Clear();
            VAdjustPendingDelivery(compound);
            return isValid(compound);
        }
        
        public bool isValid(Compound obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public string PrintError(Compound obj)
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
