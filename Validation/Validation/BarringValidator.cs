using Core.DomainModel;
using Core.Interface.Validation;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Validation.Validation
{
    public class BarringValidator : IBarringValidator
    {
        public Barring VHasItemType(Barring barring, IItemTypeService _itemTypeService)
        {
            ItemType itemType = _itemTypeService.GetObjectById(barring.ItemTypeId);
            if (itemType == null)
            {
                barring.Errors.Add("itemType", "Tidak boleh tidak ada");
            }
            return barring;
        }

        public Barring VHasUniqueSku(Barring barring, IBarringService _barringService)
        {
            if (String.IsNullOrEmpty(barring.Sku) || barring.Sku.Trim() == "")
            {
                barring.Errors.Add("Sku", "Tidak boleh kosong");
            }
            if (_barringService.IsSkuDuplicated(barring))
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

        public Barring VHasCustomer(Barring barring, ICustomerService _customerService)
        {
            Customer customer = _customerService.GetObjectById(barring.CustomerId);
            if (customer == null)
            {
                barring.Errors.Add("CustomerId", "Tidak terasosiasi dengan customer");
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
            if (barring.AR <= 0)
            {
                barring.Errors.Add("AR", "Tidak boleh negatif atau 0");
            }
            return barring;
        }

        public Barring VHasBar(Barring barring, int itemId, IItemService _itemService)
        {
            if (_itemService.GetObjectById(itemId) == null)
            {
                barring.Errors.Add("Generic", "Bar tidak terasosiasi di dalam system");
            }
            return barring;
        }

        public Barring VHasNoBar(Barring barring, int itemId, IItemService _itemService)
        {
            if (_itemService.GetObjectById(itemId) != null)
            {
                barring.Errors.Add("Generic", "Bar tidak boleh terisi");
            }
            return barring;
        }

        public Barring VCreateObject(Barring barring, IBarringService _barringService, IUoMService _uomService, IItemService _itemService, IItemTypeService _itemTypeService,
                                     ICustomerService _customerService, IMachineService _machineService)
        {
            // Item Validation

            VHasItemType(barring, _itemTypeService);
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
            VHasCustomer(barring, _customerService);
            if (!isValid(barring)) { return barring; }
            VHasMachine(barring, _machineService);
            if (!isValid(barring)) { return barring; }
            VHasMeasurement(barring);
            return barring;
        }

        public Barring VUpdateObject(Barring barring, IBarringService _barringService, IUoMService _uomService, IItemService _itemService, IItemTypeService _itemTypeService,
                                     ICustomerService _customerService, IMachineService _machineService)
        {
            return VCreateObject(barring, _barringService, _uomService, _itemService, _itemTypeService, _customerService, _machineService);
        }

        public Barring VDeleteObject(Barring barring, IWarehouseItemService _warehouseItemService)
        {
            VWarehouseQuantityMustBeZero(barring, _warehouseItemService);
            return barring;
        }

        public Barring VAdjustQuantity(Barring barring)
        {
            VNonNegativeQuantity(barring);
            return barring;
        }

        public Barring VAddLeftBar(Barring barring, IItemService _itemService)
        {
            VHasNoBar(barring, (int) barring.LeftBarItemId, _itemService);
            return barring;
        }

        public Barring VRemoveLeftBar(Barring barring, IItemService _itemService)
        {
            VHasBar(barring, (int) barring.LeftBarItemId, _itemService);
            return barring;
        }

        public Barring VAddRightBar(Barring barring, IItemService _itemService)
        {
            VHasNoBar(barring, (int) barring.RightBarItemId, _itemService);
            return barring;
        }

        public Barring VRemoveRightBar(Barring barring, IItemService _itemService)
        {
            VHasBar(barring, (int) barring.RightBarItemId, _itemService);
            return barring;
        }

        public bool ValidCreateObject(Barring barring, IBarringService _barringService, IUoMService _uomService, IItemService _itemService, IItemTypeService _itemTypeService,
                                     ICustomerService _customerService, IMachineService _machineService)
        {
            VCreateObject(barring, _barringService, _uomService, _itemService, _itemTypeService, _customerService, _machineService);
            return isValid(barring);
        }

        public bool ValidUpdateObject(Barring barring, IBarringService _barringService, IUoMService _uomService, IItemService _itemService, IItemTypeService _itemTypeService,
                                     ICustomerService _customerService, IMachineService _machineService)
        {
            barring.Errors.Clear();
            VUpdateObject(barring, _barringService, _uomService, _itemService, _itemTypeService, _customerService, _machineService);
            return isValid(barring);
        }

        public bool ValidDeleteObject(Barring barring, IWarehouseItemService _warehouseItemService)
        {
            barring.Errors.Clear();
            VDeleteObject(barring, _warehouseItemService);
            return isValid(barring);
        }

        public bool ValidAdjustQuantity(Barring barring)
        {
            barring.Errors.Clear();
            VAdjustQuantity(barring);
            return isValid(barring);
        }

        public bool ValidAddLeftBar(Barring barring, IItemService _itemService)
        {
            barring.Errors.Clear();
            VAddLeftBar(barring, _itemService);
            return isValid(barring);
        }

        public bool ValidRemoveLeftBar(Barring barring, IItemService _itemService)
        {
            barring.Errors.Clear();
            VRemoveLeftBar(barring, _itemService);
            return isValid(barring);
        }

        public bool ValidAddRightBar(Barring barring, IItemService _itemService)
        {
            barring.Errors.Clear();
            VAddRightBar(barring, _itemService);
            return isValid(barring);
        }

        public bool ValidRemoveRightBar(Barring barring, IItemService _itemService)
        {
            barring.Errors.Clear();
            VRemoveRightBar(barring, _itemService);
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
