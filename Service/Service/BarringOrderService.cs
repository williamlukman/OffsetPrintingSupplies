using Core.DomainModel;
using Core.Interface.Repository;
using Core.Interface.Service;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Service
{
    public class BarringOrderService : IBarringOrderService
    {
        private IBarringOrderRepository _repository;
        private IBarringOrderValidator _validator;
        public BarringOrderService(IBarringOrderRepository _barringOrderRepository, IBarringOrderValidator _barringOrderValidator)
        {
            _repository = _barringOrderRepository;
            _validator = _barringOrderValidator;
        }

        public IBarringOrderValidator GetValidator()
        {
            return _validator;
        }

        public IList<BarringOrder> GetAll()
        {
            return _repository.GetAll();
        }

        public IList<BarringOrder> GetAllObjectsByCustomerId(int CustomerId)
        {
            return _repository.GetAllObjectsByCustomerId(CustomerId);
        }

        public BarringOrder GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public BarringOrder CreateObject(BarringOrder barringOrder)
        {
            barringOrder.Errors = new Dictionary<String, String>();
            return (_validator.ValidCreateObject(barringOrder, this) ? _repository.CreateObject(barringOrder) : barringOrder);
        }

        public BarringOrder UpdateObject(BarringOrder barringOrder, IBarringOrderDetailService _barringOrderDetailService)
        {
            return (barringOrder = _validator.ValidUpdateObject(barringOrder, _barringOrderDetailService, this) ? _repository.UpdateObject(barringOrder) : barringOrder);
        }

        public BarringOrder SoftDeleteObject(BarringOrder barringOrder, IBarringOrderDetailService _barringOrderDetailService)
        {
            if (_validator.ValidDeleteObject(barringOrder, _barringOrderDetailService))
            {
                ICollection<BarringOrderDetail> details = _barringOrderDetailService.GetObjectsByBarringOrderId(barringOrder.Id);
                foreach (var detail in details)
                {
                    // delete details
                    _barringOrderDetailService.GetRepository().SoftDeleteObject(detail);
                }
                _repository.SoftDeleteObject(barringOrder);
            }
            return barringOrder;
        }

        public BarringOrder ConfirmObject(BarringOrder barringOrder, IBarringOrderDetailService _barringOrderDetailService, IBarringService _barringService,
                                          IItemService _itemService, IWarehouseItemService _warehouseItemService)
        {
            if (_validator.ValidConfirmObject(barringOrder, _barringOrderDetailService, _barringService, _itemService, _warehouseItemService))
            {
                IList<BarringOrderDetail> details = _barringOrderDetailService.GetObjectsByBarringOrderId(barringOrder.Id);
                // itemId contains Id of the blanket, leftbar, and rightbar
                IDictionary<int, int> ValuePairItemIdQuantity = new Dictionary<int, int>();
                foreach (var detail in details)
                {
                    Barring barring = _barringService.GetObjectById(detail.BarringId);
                    // blanket
                    if (ValuePairItemIdQuantity.ContainsKey(barring.BlanketItemId))
                    { ValuePairItemIdQuantity[barring.BlanketItemId] -= 1; }
                    else
                    { ValuePairItemIdQuantity.Add(barring.BlanketItemId, -1); }

                    // leftbar
                    if (barring.LeftBarItemId != null)
                    {
                        if (ValuePairItemIdQuantity.ContainsKey((int)barring.LeftBarItemId))
                        { ValuePairItemIdQuantity[(int)barring.LeftBarItemId] -= 1; }
                        else
                        { ValuePairItemIdQuantity.Add((int)barring.LeftBarItemId, -1); }
                    }

                    // rightbar
                    if (barring.RightBarItemId != null)
                    {
                        if (ValuePairItemIdQuantity.ContainsKey((int)barring.RightBarItemId))
                        { ValuePairItemIdQuantity[(int)barring.RightBarItemId] -= 1; }
                        else
                        { ValuePairItemIdQuantity.Add((int)barring.RightBarItemId, -1); }
                    }
                }

                foreach (var ValuePair in ValuePairItemIdQuantity)
                {
                    WarehouseItem warehouseItem = _warehouseItemService.GetObjectByWarehouseAndItem(barringOrder.WarehouseId, ValuePair.Key);
                    _warehouseItemService.AdjustQuantity(warehouseItem, ValuePair.Value);
                    Item item = _itemService.GetObjectById(ValuePair.Key);
                    _itemService.AdjustQuantity(item, ValuePair.Value);
                } 
                _repository.ConfirmObject(barringOrder);
            }
            return barringOrder;
        }

        public BarringOrder UnconfirmObject(BarringOrder barringOrder, IBarringOrderDetailService _barringOrderDetailService, IBarringService _barringService, IItemService _itemService, IWarehouseItemService _warehouseItemService)
        {
            if (_validator.ValidUnconfirmObject(barringOrder, _barringOrderDetailService))
            {
                IList<BarringOrderDetail> details = _barringOrderDetailService.GetObjectsByBarringOrderId(barringOrder.Id);
                // itemId contains Id of the blanket, leftbar, and rightbar
                IDictionary<int, int> ValuePairItemIdQuantity = new Dictionary<int, int>();
                foreach (var detail in details)
                {
                    Barring barring = _barringService.GetObjectById(detail.BarringId);
                    // blanket
                    if (ValuePairItemIdQuantity.ContainsKey(barring.BlanketItemId))
                    { ValuePairItemIdQuantity[barring.BlanketItemId] += 1; }
                    else
                    { ValuePairItemIdQuantity.Add(barring.BlanketItemId, 1); }

                    // leftbar
                    if (barring.LeftBarItemId != null)
                    {
                        if (ValuePairItemIdQuantity.ContainsKey((int)barring.LeftBarItemId))
                        { ValuePairItemIdQuantity[(int)barring.LeftBarItemId] += 1; }
                        else
                        { ValuePairItemIdQuantity.Add((int)barring.LeftBarItemId, 1); }
                    }

                    // rightbar
                    if (barring.RightBarItemId != null)
                    {
                        if (ValuePairItemIdQuantity.ContainsKey((int)barring.RightBarItemId))
                        { ValuePairItemIdQuantity[(int)barring.RightBarItemId] += 1; }
                        else
                        { ValuePairItemIdQuantity.Add((int)barring.RightBarItemId, 1); }
                    }
                }

                foreach (var ValuePair in ValuePairItemIdQuantity)
                {
                    WarehouseItem warehouseItem = _warehouseItemService.GetObjectByWarehouseAndItem(barringOrder.WarehouseId, ValuePair.Key);
                    _warehouseItemService.AdjustQuantity(warehouseItem, ValuePair.Value);
                    Item item = _itemService.GetObjectById(ValuePair.Key);
                    _itemService.AdjustQuantity(item, ValuePair.Value);
                }
                _repository.UnconfirmObject(barringOrder);
            }
            return barringOrder;
        }

        public BarringOrder FinishObject(BarringOrder barringOrder, IBarringOrderDetailService _barringOrderDetailService, IBarringService _barringService, IItemService _itemService, IWarehouseItemService _warehouseItemService)
        {
            if (_validator.ValidFinishObject(barringOrder, _barringOrderDetailService, _itemService))
            {
                IList<BarringOrderDetail> details = _barringOrderDetailService.GetObjectsByBarringOrderId(barringOrder.Id);
                IDictionary<int, int> ValuePairRejectedItemIdQuantity = new Dictionary<int, int>();
                IDictionary<int, int> ValuePairPackagedItemIdQuantity = new Dictionary<int, int>();
                int QuantityRejected = 0;
                int QuantityFinal = 0;
                foreach (var detail in details)
                {
                    QuantityRejected += detail.IsRejected ? 1 : 0;
                    QuantityFinal += (detail.IsPackaged && !detail.IsRejected) ? 1 : 0;
                    if (detail.IsRejected)
                    {
                        // Barring quantity increases
                        if (ValuePairRejectedItemIdQuantity.ContainsKey(detail.BarringId))
                        {
                            ValuePairRejectedItemIdQuantity[detail.BarringId] += 1;
                        }
                        else
                        {
                            ValuePairRejectedItemIdQuantity.Add(detail.BarringId, 1);
                        }

                    }
                    else if (detail.IsPackaged && !detail.IsRejected)
                    {
                        if (ValuePairPackagedItemIdQuantity.ContainsKey(detail.BarringId))
                        {
                            ValuePairPackagedItemIdQuantity[detail.BarringId] += 1;
                        }
                        else
                        {
                            ValuePairPackagedItemIdQuantity.Add(detail.BarringId, 1);
                        }
                    }
                }

                // do nothing for rejected data

                foreach (var ValuePairPackaged in ValuePairPackagedItemIdQuantity)
                {
                    WarehouseItem warehouseItem = _warehouseItemService.GetObjectByWarehouseAndItem(barringOrder.WarehouseId, ValuePairPackaged.Key);
                    _warehouseItemService.AdjustQuantity(warehouseItem, ValuePairPackaged.Value);
                    Barring barring = _barringService.GetObjectById(ValuePairPackaged.Key);
                    _barringService.AdjustQuantity(barring, ValuePairPackaged.Value);
                }
                barringOrder.QuantityRejected = QuantityRejected;
                barringOrder.QuantityFinal = QuantityFinal;
                _repository.FinishObject(barringOrder);
            }
            return barringOrder;
        }

        public BarringOrder UnfinishObject(BarringOrder barringOrder, IBarringOrderDetailService _barringOrderDetailService, IBarringService _barringService, IWarehouseItemService _warehouseItemService)
        {
            if (_validator.ValidUnfinishObject(barringOrder))
            {
                IList<BarringOrderDetail> details = _barringOrderDetailService.GetObjectsByBarringOrderId(barringOrder.Id);
                IDictionary<int, int> ValuePairRejectedItemIdQuantity = new Dictionary<int, int>();
                IDictionary<int, int> ValuePairPackagedItemIdQuantity = new Dictionary<int, int>();
                foreach (var detail in details)
                {
                    if (detail.IsRejected)
                    {
                        if (ValuePairRejectedItemIdQuantity.ContainsKey(detail.BarringId))
                        {
                            ValuePairRejectedItemIdQuantity[detail.BarringId] -= 1;
                        }
                        else
                        {
                            ValuePairRejectedItemIdQuantity.Add(detail.BarringId, -1);
                        }

                    }
                    else if (detail.IsPackaged && !detail.IsRejected)
                    {
                        // Roller quantity increases
                        if (ValuePairPackagedItemIdQuantity.ContainsKey(detail.BarringId))
                        {
                            ValuePairPackagedItemIdQuantity[detail.BarringId] -= 1;
                        }
                        else
                        {
                            ValuePairPackagedItemIdQuantity.Add(detail.BarringId, -1);
                        }
                    }
                }

                // do nothing for rejected data

                foreach (var ValuePairPackaged in ValuePairPackagedItemIdQuantity)
                {
                    WarehouseItem warehouseItem = _warehouseItemService.GetObjectByWarehouseAndItem(barringOrder.WarehouseId, ValuePairPackaged.Key);
                    _warehouseItemService.AdjustQuantity(warehouseItem, ValuePairPackaged.Value);
                    Barring barring = _barringService.GetObjectById(ValuePairPackaged.Key);
                    _barringService.AdjustQuantity(barring, ValuePairPackaged.Value);
                }
                barringOrder.QuantityRejected = 0;
                barringOrder.QuantityFinal = 0;
                _repository.UnfinishObject(barringOrder);
            }
            return barringOrder;
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }

        public bool IsCodeDuplicated(BarringOrder barringOrder)
        {
            return _repository.IsCodeDuplicated(barringOrder);
        }
    }
}