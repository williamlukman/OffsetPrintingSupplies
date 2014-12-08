using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Interface.Validation;
using Core.DomainModel;
using Core.Interface.Service;

namespace Validation.Validation
{
    public class CustomerStockAdjustmentDetailValidator : ICustomerStockAdjustmentDetailValidator
    {
        public CustomerStockAdjustmentDetail VHasCustomerStockAdjustment(CustomerStockAdjustmentDetail customerStockAdjustmentDetail, ICustomerStockAdjustmentService _customerStockAdjustmentService)
        {
            CustomerStockAdjustment customerStockAdjustment = _customerStockAdjustmentService.GetObjectById(customerStockAdjustmentDetail.CustomerStockAdjustmentId);
            if (customerStockAdjustment == null)
            {
                customerStockAdjustmentDetail.Errors.Add("CustomerStockAdjustmentId", "Tidak terasosiasi dengan Customer Stock Adjustment");
            }
            return customerStockAdjustmentDetail;
        }

        public CustomerStockAdjustmentDetail VHasItem(CustomerStockAdjustmentDetail customerStockAdjustmentDetail, IItemService _itemService)
        {
            Item item = _itemService.GetObjectById(customerStockAdjustmentDetail.ItemId);
            if (item == null)
            {
                customerStockAdjustmentDetail.Errors.Add("ItemId", "Tidak terasosiasi dengan item");
            }
            return customerStockAdjustmentDetail;
        }

        public CustomerStockAdjustmentDetail VHasCustomerWarehouseItem(CustomerStockAdjustmentDetail customerStockAdjustmentDetail, ICustomerStockAdjustmentService _customerStockAdjustmentService, IWarehouseItemService _warehouseItemService, ICustomerItemService _customerItemService)
        {
            CustomerStockAdjustment customerStockAdjustment = _customerStockAdjustmentService.GetObjectById(customerStockAdjustmentDetail.CustomerStockAdjustmentId);
            WarehouseItem warehouseItem = _warehouseItemService.FindOrCreateObject(customerStockAdjustment.WarehouseId, customerStockAdjustmentDetail.ItemId);
            if (warehouseItem == null)
            {
                customerStockAdjustmentDetail.Errors.Add("Generic", "Tidak terasosiasi dengan warehouse");
            }
            else
            {
                CustomerItem customerItem = _customerItemService.FindOrCreateObject(customerStockAdjustment.ContactId, warehouseItem.Id);
                if (customerItem == null)
                {
                    customerStockAdjustmentDetail.Errors.Add("Generic", "Tidak terasosiasi dengan contact");
                }
            }
            return customerStockAdjustmentDetail;
        }

        public CustomerStockAdjustmentDetail VNonZeroQuantity(CustomerStockAdjustmentDetail customerStockAdjustmentDetail)
        {
            if (customerStockAdjustmentDetail.Quantity == 0)
            {
                customerStockAdjustmentDetail.Errors.Add("Quantity", "Tidak boleh 0");
            }
            return customerStockAdjustmentDetail;
        }

        public CustomerStockAdjustmentDetail VNonZeroNorNegativePrice(CustomerStockAdjustmentDetail customerStockAdjustmentDetail)
        {
            if (customerStockAdjustmentDetail.Price <= 0)
            {
                customerStockAdjustmentDetail.Errors.Add("Price", "Harus lebih besar dari 0");
            }
            return customerStockAdjustmentDetail;
        }

        public CustomerStockAdjustmentDetail VUniqueItem(CustomerStockAdjustmentDetail customerStockAdjustmentDetail, ICustomerStockAdjustmentDetailService _customerStockAdjustmentDetailService, IItemService _itemService)
        {
            IList<CustomerStockAdjustmentDetail> details = _customerStockAdjustmentDetailService.GetObjectsByCustomerStockAdjustmentId(customerStockAdjustmentDetail.CustomerStockAdjustmentId);
            foreach (var detail in details)
            {
                if (detail.ItemId == customerStockAdjustmentDetail.ItemId && detail.Id != customerStockAdjustmentDetail.Id)
                {
                     customerStockAdjustmentDetail.Errors.Add("Generic", "Tidak boleh ada duplikasi item dalam 1 Customer Stock Adjustment");
                }
            }
            return customerStockAdjustmentDetail;
        }

        public CustomerStockAdjustmentDetail VHasNotBeenConfirmed(CustomerStockAdjustmentDetail customerStockAdjustmentDetail)
        {
            if (customerStockAdjustmentDetail.IsConfirmed)
            {
                customerStockAdjustmentDetail.Errors.Add("Generic", "Tidak boleh sudah terkonfirmasi");
            }
            return customerStockAdjustmentDetail;
        }

        public CustomerStockAdjustmentDetail VHasBeenConfirmed(CustomerStockAdjustmentDetail customerStockAdjustmentDetail)
        {
            if (!customerStockAdjustmentDetail.IsConfirmed)
            {
                customerStockAdjustmentDetail.Errors.Add("Generic", "Belum dikonfirmasi");
            }
            return customerStockAdjustmentDetail;
        }

        public CustomerStockAdjustmentDetail VCustomerStockAdjustmentHasBeenConfirmed(CustomerStockAdjustmentDetail customerStockAdjustmentDetail, ICustomerStockAdjustmentService _customerStockAdjustmentService)
        {
            CustomerStockAdjustment customerStockAdjustment = _customerStockAdjustmentService.GetObjectById(customerStockAdjustmentDetail.CustomerStockAdjustmentId);
            if (!customerStockAdjustment.IsConfirmed)
            {
                customerStockAdjustmentDetail.Errors.Add("Generic", "Customer Stock adjustment belum dikonfirmasi");
                return customerStockAdjustmentDetail;
            }
            return customerStockAdjustmentDetail;
        }

        public CustomerStockAdjustmentDetail VNonNegativeStockQuantity(CustomerStockAdjustmentDetail customerStockAdjustmentDetail, ICustomerStockAdjustmentService _customerStockAdjustmentService,
                                                               IItemService _itemService, ICustomerItemService _customerItemService, IWarehouseItemService _warehouseItemService, bool ToConfirm)
        {
            int customerStockAdjustmentDetailQuantity = ToConfirm ? customerStockAdjustmentDetail.Quantity : ((-1) * customerStockAdjustmentDetail.Quantity);
            //decimal customerStockAdjustmentDetailPrice = ToConfirm ? customerStockAdjustmentDetail.Price : ((-1) * customerStockAdjustmentDetail.Price);
            Item item = _itemService.GetObjectById(customerStockAdjustmentDetail.ItemId);
            CustomerStockAdjustment customerStockAdjustment = _customerStockAdjustmentService.GetObjectById(customerStockAdjustmentDetail.CustomerStockAdjustmentId);
            WarehouseItem warehouseItem = _warehouseItemService.FindOrCreateObject(customerStockAdjustment.WarehouseId, item.Id);
            CustomerItem customerItem = _customerItemService.FindOrCreateObject(customerStockAdjustment.ContactId, warehouseItem.Id);
            if (item.CustomerQuantity + customerStockAdjustmentDetailQuantity < 0)
            {
                customerStockAdjustmentDetail.Errors.Add("Quantity", "Stock barang tidak boleh menjadi kurang dari 0");
                return customerStockAdjustmentDetail;
            }
            if (warehouseItem.CustomerQuantity + customerStockAdjustmentDetailQuantity < 0)
            {
                customerStockAdjustmentDetail.Errors.Add("Quantity", "Stock item customer di dalam warehouse tidak boleh kurang dari 0");
                return customerStockAdjustmentDetail;
            }
            if (customerItem.Quantity + customerStockAdjustmentDetailQuantity < 0)
            {
                customerStockAdjustmentDetail.Errors.Add("Quantity", "Stock item customer tidak boleh kurang dari 0");
                return customerStockAdjustmentDetail;
            }
            /*
            if (_itemService.CalculateAvgCost(item, customerStockAdjustmentDetail.Quantity, customerStockAdjustmentDetailPrice) < 0)
            {
                customerStockAdjustmentDetail.Errors.Add("AvgCost", "Tidak boleh kurang dari 0");
            }
            */
            return customerStockAdjustmentDetail;
        }

        public CustomerStockAdjustmentDetail VCreateObject(CustomerStockAdjustmentDetail customerStockAdjustmentDetail, ICustomerStockAdjustmentDetailService _customerStockAdjustmentDetailService,
                                                   ICustomerStockAdjustmentService _customerStockAdjustmentService, IItemService _itemService, IWarehouseItemService _warehouseItemService, ICustomerItemService _customerItemService)
        {
            VHasCustomerStockAdjustment(customerStockAdjustmentDetail, _customerStockAdjustmentService);
            if (!isValid(customerStockAdjustmentDetail)) { return customerStockAdjustmentDetail; }
            VHasItem(customerStockAdjustmentDetail, _itemService);
            if (!isValid(customerStockAdjustmentDetail)) { return customerStockAdjustmentDetail; }
            VHasCustomerWarehouseItem(customerStockAdjustmentDetail, _customerStockAdjustmentService, _warehouseItemService, _customerItemService);
            if (!isValid(customerStockAdjustmentDetail)) { return customerStockAdjustmentDetail; }
            VNonZeroQuantity(customerStockAdjustmentDetail);
            if (!isValid(customerStockAdjustmentDetail)) { return customerStockAdjustmentDetail; }
            VNonZeroNorNegativePrice(customerStockAdjustmentDetail);
            if (!isValid(customerStockAdjustmentDetail)) { return customerStockAdjustmentDetail; }
            VUniqueItem(customerStockAdjustmentDetail, _customerStockAdjustmentDetailService, _itemService);
            return customerStockAdjustmentDetail;
        }

        public CustomerStockAdjustmentDetail VUpdateObject(CustomerStockAdjustmentDetail customerStockAdjustmentDetail, ICustomerStockAdjustmentDetailService _customerStockAdjustmentDetailService,
                                                   ICustomerStockAdjustmentService _customerStockAdjustmentService, IItemService _itemService, IWarehouseItemService _warehouseItemService, ICustomerItemService _customerItemService)
        {
            VHasNotBeenConfirmed(customerStockAdjustmentDetail);
            if (!isValid(customerStockAdjustmentDetail)) { return customerStockAdjustmentDetail; }
            VCreateObject(customerStockAdjustmentDetail, _customerStockAdjustmentDetailService, _customerStockAdjustmentService, _itemService, _warehouseItemService, _customerItemService);
            return customerStockAdjustmentDetail;
        }

        public CustomerStockAdjustmentDetail VDeleteObject(CustomerStockAdjustmentDetail customerStockAdjustmentDetail)
        {
            VHasNotBeenConfirmed(customerStockAdjustmentDetail);
            return customerStockAdjustmentDetail;
        }

        public CustomerStockAdjustmentDetail VHasConfirmationDate(CustomerStockAdjustmentDetail customerStockAdjustmentDetail)
        {
            if (customerStockAdjustmentDetail.ConfirmationDate == null)
            {
                customerStockAdjustmentDetail.Errors.Add("ConfirmationDate", "Tidak boleh kosong");
            }
            return customerStockAdjustmentDetail;
        }

        public CustomerStockAdjustmentDetail VConfirmObject(CustomerStockAdjustmentDetail customerStockAdjustmentDetail, ICustomerStockAdjustmentService _customerStockAdjustmentService,
                                                    IItemService _itemService, ICustomerItemService _customerItemService, IWarehouseItemService _warehouseItemService)
        {
            VHasConfirmationDate(customerStockAdjustmentDetail);
            if (!isValid(customerStockAdjustmentDetail)) { return customerStockAdjustmentDetail; }
            VNonNegativeStockQuantity(customerStockAdjustmentDetail, _customerStockAdjustmentService, _itemService, _customerItemService, _warehouseItemService, true);
            return customerStockAdjustmentDetail;
        }

        public CustomerStockAdjustmentDetail VUnconfirmObject(CustomerStockAdjustmentDetail customerStockAdjustmentDetail, ICustomerStockAdjustmentService _customerStockAdjustmentService,
                                                      IItemService _itemService, ICustomerItemService _customerItemService, IWarehouseItemService _warehouseItemService)
        {
            VCustomerStockAdjustmentHasBeenConfirmed(customerStockAdjustmentDetail, _customerStockAdjustmentService);
            if (!isValid(customerStockAdjustmentDetail)) { return customerStockAdjustmentDetail; }
            VHasBeenConfirmed(customerStockAdjustmentDetail);
            if (!isValid(customerStockAdjustmentDetail)) { return customerStockAdjustmentDetail; }
            VNonNegativeStockQuantity(customerStockAdjustmentDetail, _customerStockAdjustmentService, _itemService, _customerItemService, _warehouseItemService, false);
            return customerStockAdjustmentDetail;
        }

        public bool ValidCreateObject(CustomerStockAdjustmentDetail customerStockAdjustmentDetail, ICustomerStockAdjustmentDetailService _customerStockAdjustmentDetailService,
                                      ICustomerStockAdjustmentService _customerStockAdjustmentService, IItemService _itemService, IWarehouseItemService _warehouseItemService, ICustomerItemService _customerItemService)
        {
            VCreateObject(customerStockAdjustmentDetail, _customerStockAdjustmentDetailService, _customerStockAdjustmentService, _itemService, _warehouseItemService, _customerItemService);
            return isValid(customerStockAdjustmentDetail);
        }

        public bool ValidUpdateObject(CustomerStockAdjustmentDetail customerStockAdjustmentDetail, ICustomerStockAdjustmentDetailService _customerStockAdjustmentDetailService,
                                      ICustomerStockAdjustmentService _customerStockAdjustmentService, IItemService _itemService, IWarehouseItemService _warehouseItemService, ICustomerItemService _customerItemService)
        {
            customerStockAdjustmentDetail.Errors.Clear();
            VUpdateObject(customerStockAdjustmentDetail, _customerStockAdjustmentDetailService, _customerStockAdjustmentService, _itemService, _warehouseItemService, _customerItemService);
            return isValid(customerStockAdjustmentDetail);
        }

        public bool ValidDeleteObject(CustomerStockAdjustmentDetail customerStockAdjustmentDetail)
        {
            customerStockAdjustmentDetail.Errors.Clear();
            VDeleteObject(customerStockAdjustmentDetail);
            return isValid(customerStockAdjustmentDetail);
        }

        public bool ValidConfirmObject(CustomerStockAdjustmentDetail customerStockAdjustmentDetail, ICustomerStockAdjustmentService _customerStockAdjustmentService,
                                       IItemService _itemService, ICustomerItemService _customerItemService, IWarehouseItemService _warehouseItemService)
        {
            customerStockAdjustmentDetail.Errors.Clear();
            VConfirmObject(customerStockAdjustmentDetail, _customerStockAdjustmentService, _itemService, _customerItemService, _warehouseItemService);
            return isValid(customerStockAdjustmentDetail);
        }

        public bool ValidUnconfirmObject(CustomerStockAdjustmentDetail customerStockAdjustmentDetail, ICustomerStockAdjustmentService _customerStockAdjustmentService,
                                         IItemService _itemService, ICustomerItemService _customerItemService, IWarehouseItemService _warehouseItemService)
        {
            customerStockAdjustmentDetail.Errors.Clear();
            VUnconfirmObject(customerStockAdjustmentDetail, _customerStockAdjustmentService, _itemService, _customerItemService, _warehouseItemService);
            return isValid(customerStockAdjustmentDetail);
        }

        public bool isValid(CustomerStockAdjustmentDetail obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public string PrintError(CustomerStockAdjustmentDetail obj)
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