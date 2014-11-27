using Core.DomainModel;
using Core.Interface.Repository;
using Core.Interface.Service;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Service.Service
{
    public class TemporaryDeliveryOrderClearanceService : ITemporaryDeliveryOrderClearanceService
    {
        private ITemporaryDeliveryOrderClearanceRepository _repository;
        private ITemporaryDeliveryOrderClearanceValidator _validator;

        public TemporaryDeliveryOrderClearanceService(ITemporaryDeliveryOrderClearanceRepository _temporaryDeliveryOrderClearanceRepository, ITemporaryDeliveryOrderClearanceValidator _temporaryDeliveryOrderClearanceValidator)
        {
            _repository = _temporaryDeliveryOrderClearanceRepository;
            _validator = _temporaryDeliveryOrderClearanceValidator;
        }

        public ITemporaryDeliveryOrderClearanceValidator GetValidator()
        {
            return _validator;
        }

        public IQueryable<TemporaryDeliveryOrderClearance> GetQueryable()
        {
            return _repository.GetQueryable();
        }

        public IList<TemporaryDeliveryOrderClearance> GetAll()
        {
            return _repository.GetAll();
        }

        public IList<TemporaryDeliveryOrderClearance> GetConfirmedObjects()
        {
            return _repository.GetConfirmedObjects();
        }

        public TemporaryDeliveryOrderClearance GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public IList<TemporaryDeliveryOrderClearance> GetObjectsByTemporaryDeliveryOrderId(int deliveryOrderId)
        {
            return _repository.GetObjectsByTemporaryDeliveryOrderId(deliveryOrderId);
        }

        public TemporaryDeliveryOrderClearance CreateObject(TemporaryDeliveryOrderClearance temporaryDeliveryOrderClearance, IVirtualOrderService _virtualOrderService, IDeliveryOrderService _deliveryOrderService, IWarehouseService _warehouseService)
        {
            temporaryDeliveryOrderClearance.Errors = new Dictionary<String, String>();
            return (_validator.ValidCreateObject(temporaryDeliveryOrderClearance, this, _virtualOrderService, _deliveryOrderService, _warehouseService) ? _repository.CreateObject(temporaryDeliveryOrderClearance) : temporaryDeliveryOrderClearance);
        }

        public TemporaryDeliveryOrderClearance UpdateObject(TemporaryDeliveryOrderClearance temporaryDeliveryOrderClearance, IVirtualOrderService _virtualOrderService, IDeliveryOrderService _deliveryOrderService, IWarehouseService _warehouseService)
        {
            return (_validator.ValidUpdateObject(temporaryDeliveryOrderClearance, this, _virtualOrderService, _deliveryOrderService, _warehouseService) ? _repository.UpdateObject(temporaryDeliveryOrderClearance) : temporaryDeliveryOrderClearance);
        }

        public TemporaryDeliveryOrderClearance SoftDeleteObject(TemporaryDeliveryOrderClearance temporaryDeliveryOrderClearance, ITemporaryDeliveryOrderClearanceDetailService _temporaryDeliveryOrderClearanceDetailService)
        {
            return (_validator.ValidDeleteObject(temporaryDeliveryOrderClearance, _temporaryDeliveryOrderClearanceDetailService) ? _repository.SoftDeleteObject(temporaryDeliveryOrderClearance) : temporaryDeliveryOrderClearance);
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }

        public TemporaryDeliveryOrderClearance ConfirmObject(TemporaryDeliveryOrderClearance temporaryDeliveryOrderClearance, DateTime ConfirmationDate, ITemporaryDeliveryOrderClearanceDetailService _temporaryDeliveryOrderClearanceDetailService,
                                     IVirtualOrderService _virtualOrderService, IVirtualOrderDetailService _virtualOrderDetailService,
                                     IDeliveryOrderService _deliveryOrderService, IDeliveryOrderDetailService _deliveryOrderDetailService,
                                     ISalesOrderDetailService _salesOrderDetailService, IStockMutationService _stockMutationService, IItemService _itemService,
                                     IBlanketService _blanketService, IWarehouseItemService _warehouseItemService)
        {
            temporaryDeliveryOrderClearance.ConfirmationDate = ConfirmationDate;
            if (_validator.ValidConfirmObject(temporaryDeliveryOrderClearance, _temporaryDeliveryOrderClearanceDetailService))
            {
                IList<TemporaryDeliveryOrderClearanceDetail> temporaryDeliveryOrderClearanceDetails = _temporaryDeliveryOrderClearanceDetailService.GetObjectsByTemporaryDeliveryOrderClearanceId(temporaryDeliveryOrderClearance.Id);
                foreach (var detail in temporaryDeliveryOrderClearanceDetails)
                {
                    detail.Errors = new Dictionary<string, string>();
                    _temporaryDeliveryOrderClearanceDetailService.ConfirmObject(detail, ConfirmationDate, this, _virtualOrderDetailService, _salesOrderDetailService, _stockMutationService, _itemService,
                                                                      _blanketService, _warehouseItemService);
                }
                _repository.ConfirmObject(temporaryDeliveryOrderClearance);
                if (temporaryDeliveryOrderClearance.IsWasted)
                {
                    // cari wasted cogs average cost

                    // Posting GL (Inventory Credit, SalesExpense debit)

                    // ready berkurang sejumlah waste
                    // virtual berkurang sejumlah yang dikembalikan

                }
                else
                {
                    // Virtual berkurang

                }
            }
            return temporaryDeliveryOrderClearance;
        }

        public TemporaryDeliveryOrderClearance UnconfirmObject(TemporaryDeliveryOrderClearance temporaryDeliveryOrderClearance, ITemporaryDeliveryOrderClearanceDetailService _temporaryDeliveryOrderClearanceDetailService,
                                                      IVirtualOrderService _virtualOrderService, IVirtualOrderDetailService _virtualOrderDetailService,
                                                      IDeliveryOrderService _deliveryOrderService, IDeliveryOrderDetailService _deliveryOrderDetailService,
                                                      ISalesOrderService _salesOrderService, ISalesOrderDetailService _salesOrderDetailService,
                                                      IStockMutationService _stockMutationService, IItemService _itemService,
                                                      IBlanketService _blanketService, IWarehouseItemService _warehouseItemService)
        {
            if (_validator.ValidUnconfirmObject(temporaryDeliveryOrderClearance))
            {
                if (temporaryDeliveryOrderClearance.IsWasted)
                {
                    // cari wasted cogs dari average cost

                    // Posting GL (Inventory Debit, SalesExpense Credit)

                    // ready bertambah sejumlah waste
                    // virtual bertambah sejumlah yang dikembalikan

                }
                else
                {
                    // Virtual bertambah

                }

                IList<TemporaryDeliveryOrderClearanceDetail> temporaryDeliveryOrderClearanceDetails = _temporaryDeliveryOrderClearanceDetailService.GetObjectsByTemporaryDeliveryOrderClearanceId(temporaryDeliveryOrderClearance.Id);
                foreach (var detail in temporaryDeliveryOrderClearanceDetails)
                {
                    detail.Errors = new Dictionary<string, string>();
                    _temporaryDeliveryOrderClearanceDetailService.UnconfirmObject(detail, this, _virtualOrderDetailService, _virtualOrderService, _salesOrderService, _salesOrderDetailService,
                                                                         _stockMutationService, _itemService, _blanketService, _warehouseItemService);
                }
                _repository.UnconfirmObject(temporaryDeliveryOrderClearance);
            }
            return temporaryDeliveryOrderClearance;
        }


        
    }
}