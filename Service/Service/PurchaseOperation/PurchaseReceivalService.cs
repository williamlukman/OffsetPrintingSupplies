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
    public class PurchaseReceivalService : IPurchaseReceivalService
    {
        private IPurchaseReceivalRepository _repository;
        private IPurchaseReceivalValidator _validator;

        public PurchaseReceivalService(IPurchaseReceivalRepository _purchaseReceivalRepository, IPurchaseReceivalValidator _purchaseReceivalValidator)
        {
            _repository = _purchaseReceivalRepository;
            _validator = _purchaseReceivalValidator;
        }

        public IPurchaseReceivalValidator GetValidator()
        {
            return _validator;
        }

        public IList<PurchaseReceival> GetAll()
        {
            return _repository.GetAll();
        }

        public PurchaseReceival GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public IList<PurchaseReceival> GetObjectsByPurchaseOrderId(int purchaseOrderId)
        {
            return _repository.GetObjectsByPurchaseOrderId(purchaseOrderId);
        }
        
        public PurchaseReceival CreateObject(PurchaseReceival purchaseReceival, IPurchaseOrderService _purchaseOrderService)
        {
            purchaseReceival.Errors = new Dictionary<String, String>();
            return (_validator.ValidCreateObject(purchaseReceival, _purchaseOrderService) ? _repository.CreateObject(purchaseReceival) : purchaseReceival);
        }

        public PurchaseReceival CreateObject(int warehouseId, int purchaseOrderId, DateTime receivalDate, IPurchaseOrderService _purchaseOrderService)
        {
            PurchaseReceival purchaseReceival = new PurchaseReceival
            {
                PurchaseOrderId = purchaseOrderId,
                ReceivalDate = receivalDate,
                WarehouseId = warehouseId
            };
            return this.CreateObject(purchaseReceival, _purchaseOrderService);
        }

        public PurchaseReceival UpdateObject(PurchaseReceival purchaseReceival, IPurchaseOrderService _purchaseOrderService)
        {
            return (_validator.ValidUpdateObject(purchaseReceival, _purchaseOrderService) ? _repository.UpdateObject(purchaseReceival) : purchaseReceival);
        }

        public PurchaseReceival SoftDeleteObject(PurchaseReceival purchaseReceival, IPurchaseReceivalDetailService _purchaseReceivalDetailService)
        {
            return (_validator.ValidDeleteObject(purchaseReceival, _purchaseReceivalDetailService) ? _repository.SoftDeleteObject(purchaseReceival) : purchaseReceival);
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }

        public PurchaseReceival ConfirmObject(PurchaseReceival purchaseReceival, DateTime ConfirmationDate, IPurchaseReceivalDetailService _purchaseReceivalDetailService,
                                              IPurchaseOrderDetailService _purchaseOrderDetailService, IStockMutationService _stockMutationService, IItemService _itemService,
                                              IBarringService _barringService, IWarehouseItemService _warehouseItemService)
        {
            if (_validator.ValidConfirmObject(purchaseReceival, _purchaseReceivalDetailService))
            {
                IList<PurchaseReceivalDetail> purchaseReceivalDetails = _purchaseReceivalDetailService.GetObjectsByPurchaseReceivalId(purchaseReceival.Id);
                foreach (var detail in purchaseReceivalDetails)
                {
                    _purchaseReceivalDetailService.ConfirmObject(detail, ConfirmationDate, this, _purchaseOrderDetailService, _stockMutationService, _itemService, _barringService, _warehouseItemService);
                }
                purchaseReceival.ConfirmationDate = ConfirmationDate;
                _repository.ConfirmObject(purchaseReceival);
            }
            return purchaseReceival;
        }

        public PurchaseReceival UnconfirmObject(PurchaseReceival purchaseReceival, IPurchaseReceivalDetailService _purchaseReceivalDetailService,
                                                IPurchaseInvoiceService _purchaseInvoiceService, IPurchaseInvoiceDetailService _purchaseInvoiceDetailService,
                                                IPurchaseOrderDetailService _purchaseOrderDetailService, IStockMutationService _stockMutationService, IItemService _itemService,
                                                IBarringService _barringService, IWarehouseItemService _warehouseItemService)
        {
            if (_validator.ValidUnconfirmObject(purchaseReceival, _purchaseInvoiceService))
            {
                IList<PurchaseReceivalDetail> purchaseReceivalDetails = _purchaseReceivalDetailService.GetObjectsByPurchaseReceivalId(purchaseReceival.Id);
                foreach (var detail in purchaseReceivalDetails)
                {
                    _purchaseReceivalDetailService.UnconfirmObject(detail, this, _purchaseInvoiceDetailService, _stockMutationService,
                                                                   _itemService, _barringService, _warehouseItemService);
                }
                _repository.UnconfirmObject(purchaseReceival);
            }
            return purchaseReceival;
        }

        public PurchaseReceival CheckAndSetInvoiceComplete(PurchaseReceival purchaseReceival, IPurchaseReceivalDetailService _purchaseReceivalDetailService)
        {
            IList<PurchaseReceivalDetail> details = _purchaseReceivalDetailService.GetObjectsByPurchaseReceivalId(purchaseReceival.Id);

            foreach (var detail in details)
            {
                if (!detail.IsAllInvoiced)
                {
                    return purchaseReceival;
                }
            }
            return _repository.SetInvoiceComplete(purchaseReceival);
        }

        public PurchaseReceival UnsetInvoiceComplete(PurchaseReceival purchaseReceival)
        {
            _repository.UnsetInvoiceComplete(purchaseReceival);
            return purchaseReceival;
        }
    }
}