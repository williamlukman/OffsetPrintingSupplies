using Core.DomainModel;
using Core.Interface.Repository;
using Core.Interface.Service;
using Core.Interface.Validation;
using Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Service
{
    public class PurchaseInvoiceDetailService : IPurchaseInvoiceDetailService
    {
        private IPurchaseInvoiceDetailRepository _repository;
        private IPurchaseInvoiceDetailValidator _validator;

        public PurchaseInvoiceDetailService(IPurchaseInvoiceDetailRepository _purchaseInvoiceDetailRepository, IPurchaseInvoiceDetailValidator _purchaseInvoiceDetailValidator)
        {
            _repository = _purchaseInvoiceDetailRepository;
            _validator = _purchaseInvoiceDetailValidator;
        }

        public IPurchaseInvoiceDetailValidator GetValidator()
        {
            return _validator;
        }

        public IList<PurchaseInvoiceDetail> GetObjectsByPurchaseInvoiceId(int purchaseInvoiceId)
        {
            return _repository.GetObjectsByPurchaseInvoiceId(purchaseInvoiceId);
        }

        public IList<PurchaseInvoiceDetail> GetObjectsByPurchaseReceivalDetailId(int purchaseReceivalDetailId)
        {
            return _repository.GetObjectsByPurchaseReceivalDetailId(purchaseReceivalDetailId);
        }

        public PurchaseInvoiceDetail GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public PurchaseInvoiceDetail CreateObject(PurchaseInvoiceDetail purchaseInvoiceDetail, IPurchaseInvoiceService _purchaseInvoiceService,
                                                  IPurchaseOrderDetailService _purchaseOrderDetailService, IPurchaseReceivalDetailService _purchaseReceivalDetailService)
        {
            purchaseInvoiceDetail.Errors = new Dictionary<String, String>();
            if (_validator.ValidCreateObject(purchaseInvoiceDetail, _purchaseInvoiceService, this, _purchaseReceivalDetailService))
            {
                PurchaseReceivalDetail purchaseReceivalDetail = _purchaseReceivalDetailService.GetObjectById(purchaseInvoiceDetail.PurchaseReceivalDetailId);
                PurchaseOrderDetail purchaseOrderDetail = _purchaseOrderDetailService.GetObjectById(purchaseReceivalDetail.PurchaseOrderDetailId);
                purchaseInvoiceDetail.Amount = purchaseInvoiceDetail.Quantity * purchaseOrderDetail.Price;
                purchaseInvoiceDetail = _repository.CreateObject(purchaseInvoiceDetail);
                PurchaseInvoice purchaseInvoice = _purchaseInvoiceService.GetObjectById(purchaseInvoiceDetail.PurchaseInvoiceId);
                _purchaseInvoiceService.CalculateAmountPayable(purchaseInvoice, this);
            }
            return purchaseInvoiceDetail;
        }

        public PurchaseInvoiceDetail CreateObject(int purchaseInvoiceId, int purchaseReceivalDetailId, int quantity, decimal amount,
                                                  IPurchaseInvoiceService _purchaseInvoiceService, IPurchaseOrderDetailService _purchaseOrderDetailService,
                                                  IPurchaseReceivalDetailService _purchaseReceivalDetailService)
        {
            PurchaseInvoiceDetail purchaseInvoiceDetail = new PurchaseInvoiceDetail
            {
                PurchaseInvoiceId = purchaseInvoiceId,
                PurchaseReceivalDetailId = purchaseReceivalDetailId,
                Quantity = quantity
            };
            return this.CreateObject(purchaseInvoiceDetail, _purchaseInvoiceService, _purchaseOrderDetailService, _purchaseReceivalDetailService);
        }

        public PurchaseInvoiceDetail UpdateObject(PurchaseInvoiceDetail purchaseInvoiceDetail, IPurchaseInvoiceService _purchaseInvoiceService,
                                                  IPurchaseOrderDetailService _purchaseOrderDetailService, IPurchaseReceivalDetailService _purchaseReceivalDetailService)
        {
            if (_validator.ValidUpdateObject(purchaseInvoiceDetail, _purchaseInvoiceService, this, _purchaseReceivalDetailService))
            {
                PurchaseReceivalDetail purchaseReceivalDetail = _purchaseReceivalDetailService.GetObjectById(purchaseInvoiceDetail.PurchaseReceivalDetailId);
                PurchaseOrderDetail purchaseOrderDetail = _purchaseOrderDetailService.GetObjectById(purchaseReceivalDetail.PurchaseOrderDetailId);
                purchaseInvoiceDetail.Amount = purchaseInvoiceDetail.Quantity * purchaseOrderDetail.Price;
                _repository.UpdateObject(purchaseInvoiceDetail);
                PurchaseInvoice purchaseInvoice = _purchaseInvoiceService.GetObjectById(purchaseInvoiceDetail.PurchaseInvoiceId);
                _purchaseInvoiceService.CalculateAmountPayable(purchaseInvoice, this);
            }
            return purchaseInvoiceDetail;
        }

        public PurchaseInvoiceDetail SoftDeleteObject(PurchaseInvoiceDetail purchaseInvoiceDetail, IPurchaseInvoiceService _purchaseInvoiceService)
        {
            // TODO : Hard Delete
            if (_validator.ValidDeleteObject(purchaseInvoiceDetail))
            {
                PurchaseInvoice purchaseInvoice = _purchaseInvoiceService.GetObjectById(purchaseInvoiceDetail.PurchaseInvoiceId);
                _repository.SoftDeleteObject(purchaseInvoiceDetail);
                _purchaseInvoiceService.CalculateAmountPayable(purchaseInvoice, this);
            }
            return purchaseInvoiceDetail;
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }

        public PurchaseInvoiceDetail ConfirmObject(PurchaseInvoiceDetail purchaseInvoiceDetail, IPurchaseReceivalDetailService _purchaseReceivalDetailService)
        {
            if (_validator.ValidConfirmObject(purchaseInvoiceDetail, this, _purchaseReceivalDetailService))
            {
                purchaseInvoiceDetail = _repository.ConfirmObject(purchaseInvoiceDetail);
                // update purchase receival detail PendingInvoiceQuantity
                PurchaseReceivalDetail purchaseReceivalDetail = _purchaseReceivalDetailService.GetObjectById(purchaseInvoiceDetail.PurchaseReceivalDetailId);
                _purchaseReceivalDetailService.InvoiceObject(purchaseReceivalDetail, purchaseInvoiceDetail.Quantity);
            }
            return purchaseInvoiceDetail;
        }

        public PurchaseInvoiceDetail UnconfirmObject(PurchaseInvoiceDetail purchaseInvoiceDetail, IPurchaseReceivalService _purchaseReceivalService,
                                                     IPurchaseReceivalDetailService _purchaseReceivalDetailService)
        {
            if (_validator.ValidUnconfirmObject(purchaseInvoiceDetail))
            {
                purchaseInvoiceDetail = _repository.UnconfirmObject(purchaseInvoiceDetail);
                // reverse purchase receival detail PendingInvoiceQuantity
                PurchaseReceivalDetail purchaseReceivalDetail = _purchaseReceivalDetailService.GetObjectById(purchaseInvoiceDetail.PurchaseReceivalDetailId);
                _purchaseReceivalDetailService.UndoInvoiceObject(purchaseReceivalDetail, purchaseInvoiceDetail.Quantity, _purchaseReceivalService);
            }
            return purchaseInvoiceDetail;
        }
    }
}