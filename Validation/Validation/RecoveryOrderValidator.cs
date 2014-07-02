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
    public class RecoveryOrderValidator : IRecoveryOrderValidator
    {
        public RecoveryOrder VHasUniqueCode(RecoveryOrder recoveryOrder, IRecoveryOrderService _recoveryOrderService)
        {
            if (String.IsNullOrEmpty(recoveryOrder.Code) || recoveryOrder.Code.Trim() == "")
            {
                recoveryOrder.Errors.Add("Code", "Tidak boleh kosong");
            }
            if (_recoveryOrderService.IsCodeDuplicated(recoveryOrder))
            {
                recoveryOrder.Errors.Add("Code", "Tidak boleh ada duplikasi");
            }
            return recoveryOrder;
        }

        public RecoveryOrder VHasRecoveryOrder(RecoveryOrder recoveryOrder, IRecoveryOrderService _recoveryOrderService)
        {
            IList<RecoveryOrder> recoveryOrders = _recoveryOrderService.GetAllObjectsByRecoveryOrderId(recoveryOrder.Id).ToList();
            if (recoveryOrders.Any())
            {
                recoveryOrder.Errors.Add("Generic", "RecoveryOrder masih memiliki Core Identification");
            }
            return recoveryOrder;
        }

        public RecoveryOrder VInHouseOrHasCustomer(RecoveryOrder recoveryOrder, ICustomerService _customerService)
        {
            if ((recoveryOrder.IsInHouse && recoveryOrder.CustomerId != null) ||
                (!recoveryOrder.IsInHouse && recoveryOrder.CustomerId == null))
            {
                recoveryOrder.Errors.Add("Generic", "Core Identification harus memilih InHouse atau menyertakan informasi Customer");
            }
            else if (!recoveryOrder.IsInHouse && recoveryOrder.CustomerId != null)
            {
                Customer customer = _customerService.GetObjectById((int)recoveryOrder.CustomerId);
                if (customer == null)
                {
                    recoveryOrder.Errors.Add("CustomerId", "Tidak terasosiasi dengan customer");
                }
            }
            return recoveryOrder;
        }

        public RecoveryOrder VQuantity(RecoveryOrder recoveryOrder)
        {
            if (recoveryOrder.Quantity <= 0)
            {
                recoveryOrder.Errors.Add("Quantity", "Tidak boleh 0 atau negatif");
            }
            return recoveryOrder;
        }

        public RecoveryOrder VHasIdentifiedDate(RecoveryOrder recoveryOrder)
        {
            // will always be true since date is not null
            return recoveryOrder;
        }

        public RecoveryOrder VHasBeenConfirmed(RecoveryOrder recoveryOrder)
        {
            if (!recoveryOrder.IsConfirmed)
            {
                recoveryOrder.Errors.Add("Generic", "Belum dikonfirmasi");
            }
            return recoveryOrder;
        }

        public RecoveryOrder VHasNotBeenConfirmed(RecoveryOrder recoveryOrder)
        {
            if (recoveryOrder.IsConfirmed)
            {
                recoveryOrder.Errors.Add("Generic", "Telah dikonfirmasi");
            }
            return recoveryOrder;
        }

        public RecoveryOrder VHasRecoveryOrderDetails(RecoveryOrder recoveryOrder, IRecoveryOrderDetailService _recoveryOrderDetailService)
        {
            IList<RecoveryOrderDetail> details = _recoveryOrderDetailService.GetObjectsByRecoveryOrderId(recoveryOrder.Id);
            if (!details.Any())
            {
                recoveryOrder.Errors.Add("Generic", "Harus membuat core identification detail dahulu");
            }
            return recoveryOrder;
        }

        public RecoveryOrder VIsInRecoveryOrder(RecoveryOrder recoveryOrder, IRecoveryOrderService _recoveryOrderService)
        {
            IList<RecoveryOrder> recoveryOrders = _recoveryOrderService.GetObjectsByRecoveryOrderId(recoveryOrder.Id);
            if (recoveryOrders.Any())
            {
                recoveryOrder.Errors.Add("Generic", "Tidak boleh ada asosiasi dengan Recovery Order");
            }
            return recoveryOrder;
        }

        public RecoveryOrder VQuantityEqualNumberOfDetails(RecoveryOrder recoveryOrder, IRecoveryOrderDetailService _recoveryOrderDetailService)
        {
            IList<RecoveryOrderDetail> details = _recoveryOrderDetailService.GetObjectsByRecoveryOrderId(recoveryOrder.Id);
            if (recoveryOrder.Quantity != details.Count())
            {
                recoveryOrder.Errors.Add("Quantity", "Tidak sama dengan jumlah Core Identification Detail");
            }
            return recoveryOrder;
        }

        // Hanya untuk InHouse Production
        public RecoveryOrder VQuantityIsInStock(RecoveryOrder recoveryOrder, IRecoveryOrderDetailService _recoveryOrderDetailService,
                                                     ICoreBuilderService _coreBuilderService)
        {
            if (!recoveryOrder.IsInHouse) { return recoveryOrder; }

            IList<RecoveryOrderDetail> details = _recoveryOrderDetailService.GetObjectsByRecoveryOrderId(recoveryOrder.Id);
            IDictionary<int, int> temporaryItemQuantity = new Dictionary<int, int>();
            
            foreach (var detail in details)
            {
                CoreBuilder coreBuilder = _coreBuilderService.GetObjectById(detail.CoreBuilderId);
                Item item = (detail.MaterialCase == Core.Constants.Constant.MaterialCase.New) ?
                            _coreBuilderService.GetNewCore(coreBuilder.Id) : _coreBuilderService.GetUsedCore(coreBuilder.Id);
                if (temporaryItemQuantity.ContainsKey(item.Id))
                {
                    temporaryItemQuantity[item.Id] -= 1;
                }
                else
                {
                    temporaryItemQuantity.Add(item.Id, item.Quantity - 1);
                }
            }

            foreach (var temp in temporaryItemQuantity)
            {
                if (temp.Value < 0)
                {
                    recoveryOrder.Errors.Add("Generic", "Stock item tidak mencukupi untuk melakukan Core Identification");
                    return recoveryOrder;
                }
            }
            return recoveryOrder;
        }

        public RecoveryOrder VCreateObject(RecoveryOrder recoveryOrder, IRecoveryOrderService _recoveryOrderService, ICustomerService _customerService)
        {
            VHasUniqueCode(recoveryOrder, _recoveryOrderService);
            if (!isValid(recoveryOrder)) { return recoveryOrder; }
            VInHouseOrHasCustomer(recoveryOrder, _customerService);
            if (!isValid(recoveryOrder)) { return recoveryOrder; }
            VQuantity(recoveryOrder);
            if (!isValid(recoveryOrder)) { return recoveryOrder; }
            VHasIdentifiedDate(recoveryOrder);
            return recoveryOrder;
        }

        public RecoveryOrder VUpdateObject(RecoveryOrder recoveryOrder, IRecoveryOrderService _recoveryOrderService, ICustomerService _customerService)
        {
            VCreateObject(recoveryOrder, _recoveryOrderService, _customerService);
            return recoveryOrder;
        }

        public RecoveryOrder VDeleteObject(RecoveryOrder recoveryOrder, IRecoveryOrderService _recoveryOrderService)
        {
            VHasNotBeenConfirmed(recoveryOrder);
            if (!isValid(recoveryOrder)) { return recoveryOrder; }
            VIsInRecoveryOrder(recoveryOrder, _recoveryOrderService);
            return recoveryOrder;
        }

        public RecoveryOrder VConfirmObject(RecoveryOrder recoveryOrder, IRecoveryOrderDetailService _recoveryOrderDetailService,
                                                 ICoreBuilderService _coreBuilderService)
        {
            VHasRecoveryOrderDetails(recoveryOrder, _recoveryOrderDetailService);
            if (!isValid(recoveryOrder)) { return recoveryOrder; }
            VQuantityEqualNumberOfDetails(recoveryOrder, _recoveryOrderDetailService);
            if (!isValid(recoveryOrder)) { return recoveryOrder; }
            VHasNotBeenConfirmed(recoveryOrder);
            if (!isValid(recoveryOrder)) { return recoveryOrder; }
            VQuantityIsInStock(recoveryOrder, _recoveryOrderDetailService, _coreBuilderService);
            return recoveryOrder;
        }

        public RecoveryOrder VUnconfirmObject(RecoveryOrder recoveryOrder, IRecoveryOrderService _recoveryOrderService)
        {
            VHasBeenConfirmed(recoveryOrder);
            if (!isValid(recoveryOrder)) { return recoveryOrder; }
            VIsInRecoveryOrder(recoveryOrder, _recoveryOrderService);
            return recoveryOrder;
        }
        
        public bool ValidCreateObject(RecoveryOrder recoveryOrder, IRecoveryOrderService _recoveryOrderService, ICustomerService _customerService)
        {
            VCreateObject(recoveryOrder, _recoveryOrderService, _customerService);
            return isValid(recoveryOrder);
        }

        public bool ValidUpdateObject(RecoveryOrder recoveryOrder, IRecoveryOrderService _recoveryOrderService, ICustomerService _customerService)
        {
            recoveryOrder.Errors.Clear();
            VUpdateObject(recoveryOrder, _recoveryOrderService, _customerService);
            return isValid(recoveryOrder);
        }

        public bool ValidDeleteObject(RecoveryOrder recoveryOrder, IRecoveryOrderService _recoveryOrderService)
        {
            recoveryOrder.Errors.Clear();
            VDeleteObject(recoveryOrder, _recoveryOrderService);
            return isValid(recoveryOrder);
        }

        public bool ValidConfirmObject(RecoveryOrder recoveryOrder, IRecoveryOrderDetailService _recoveryOrderDetailService, ICoreBuilderService _coreBuilderService)
        {
            recoveryOrder.Errors.Clear();
            VConfirmObject(recoveryOrder, _recoveryOrderDetailService, _coreBuilderService);
            return isValid(recoveryOrder);
        }

        public bool ValidUnconfirmObject(RecoveryOrder recoveryOrder, IRecoveryOrderService _recoveryOrderService)
        {
            recoveryOrder.Errors.Clear();
            VUnconfirmObject(recoveryOrder, _recoveryOrderService);
            return isValid(recoveryOrder);
        }
        
        public bool isValid(RecoveryOrder obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public string PrintError(RecoveryOrder obj)
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
