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
    public class SalesInvoiceDetailService : ISalesInvoiceDetailService
    {
        private ISalesInvoiceDetailRepository _repository;
        private ISalesInvoiceDetailValidator _validator;

        public SalesInvoiceDetailService(ISalesInvoiceDetailRepository _salesInvoiceDetailRepository, ISalesInvoiceDetailValidator _salesInvoiceDetailValidator)
        {
            _repository = _salesInvoiceDetailRepository;
            _validator = _salesInvoiceDetailValidator;
        }

        public ISalesInvoiceDetailValidator GetValidator()
        {
            return _validator;
        }

        public IList<SalesInvoiceDetail> GetObjectsBySalesInvoiceId(int salesInvoiceId)
        {
            return _repository.GetObjectsBySalesInvoiceId(salesInvoiceId);
        }

        public IList<SalesInvoiceDetail> GetObjectsByDeliveryOrderDetailId(int deliveryOrderDetailId)
        {
            return _repository.GetObjectsByDeliveryOrderDetailId(deliveryOrderDetailId);
        }

        public SalesInvoiceDetail GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public SalesInvoiceDetail CreateObject(SalesInvoiceDetail salesInvoiceDetail, ISalesInvoiceService _salesInvoiceService,
                                                  ISalesOrderDetailService _salesOrderDetailService, IDeliveryOrderDetailService _deliveryOrderDetailService)
        {
            salesInvoiceDetail.Errors = new Dictionary<String, String>();
            if (_validator.ValidCreateObject(salesInvoiceDetail, _salesInvoiceService, this, _deliveryOrderDetailService))
            {
                DeliveryOrderDetail deliveryOrderDetail = _deliveryOrderDetailService.GetObjectById(salesInvoiceDetail.DeliveryOrderDetailId);
                SalesOrderDetail salesOrderDetail = _salesOrderDetailService.GetObjectById(deliveryOrderDetail.SalesOrderDetailId);
                salesInvoiceDetail.Amount = salesInvoiceDetail.Quantity * salesOrderDetail.Price;
                salesInvoiceDetail = _repository.CreateObject(salesInvoiceDetail);
                SalesInvoice salesInvoice = _salesInvoiceService.GetObjectById(salesInvoiceDetail.SalesInvoiceId);
                _salesInvoiceService.CalculateAmountReceivable(salesInvoice, this);
            }
            return salesInvoiceDetail;
        }

        public SalesInvoiceDetail CreateObject(int salesInvoiceId, int deliveryOrderDetailId, int quantity, decimal amount,
                                                  ISalesInvoiceService _salesInvoiceService, ISalesOrderDetailService _salesOrderDetailService,
                                                  IDeliveryOrderDetailService _deliveryOrderDetailService)
        {
            SalesInvoiceDetail salesInvoiceDetail = new SalesInvoiceDetail
            {
                SalesInvoiceId = salesInvoiceId,
                DeliveryOrderDetailId = deliveryOrderDetailId,
                Quantity = quantity
            };
            return this.CreateObject(salesInvoiceDetail, _salesInvoiceService, _salesOrderDetailService, _deliveryOrderDetailService);
        }

        public SalesInvoiceDetail UpdateObject(SalesInvoiceDetail salesInvoiceDetail, ISalesInvoiceService _salesInvoiceService,
                                                  ISalesOrderDetailService _salesOrderDetailService, IDeliveryOrderDetailService _deliveryOrderDetailService)
        {
            if (_validator.ValidUpdateObject(salesInvoiceDetail, _salesInvoiceService, this, _deliveryOrderDetailService))
            {
                DeliveryOrderDetail deliveryOrderDetail = _deliveryOrderDetailService.GetObjectById(salesInvoiceDetail.DeliveryOrderDetailId);
                SalesOrderDetail salesOrderDetail = _salesOrderDetailService.GetObjectById(deliveryOrderDetail.SalesOrderDetailId);
                salesInvoiceDetail.Amount = salesInvoiceDetail.Quantity * salesOrderDetail.Price;
                _repository.UpdateObject(salesInvoiceDetail);
                SalesInvoice salesInvoice = _salesInvoiceService.GetObjectById(salesInvoiceDetail.SalesInvoiceId);
                _salesInvoiceService.CalculateAmountReceivable(salesInvoice, this);
            }
            return salesInvoiceDetail;
        }

        public SalesInvoiceDetail SoftDeleteObject(SalesInvoiceDetail salesInvoiceDetail, ISalesInvoiceService _salesInvoiceService)
        {
            if (_validator.ValidDeleteObject(salesInvoiceDetail))
            {
                SalesInvoice salesInvoice = _salesInvoiceService.GetObjectById(salesInvoiceDetail.SalesInvoiceId);
                _repository.SoftDeleteObject(salesInvoiceDetail);
                _salesInvoiceService.CalculateAmountReceivable(salesInvoice, this);
            }
            return salesInvoiceDetail;
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }

        public SalesInvoiceDetail ConfirmObject(SalesInvoiceDetail salesInvoiceDetail, DateTime ConfirmationDate, IDeliveryOrderDetailService _deliveryOrderDetailService)
        {
            salesInvoiceDetail.ConfirmationDate = ConfirmationDate;
            if (_validator.ValidConfirmObject(salesInvoiceDetail, this, _deliveryOrderDetailService))
            {
                salesInvoiceDetail = _repository.ConfirmObject(salesInvoiceDetail);
                // update sales receival detail PendingInvoiceQuantity
                DeliveryOrderDetail deliveryOrderDetail = _deliveryOrderDetailService.GetObjectById(salesInvoiceDetail.DeliveryOrderDetailId);
                _deliveryOrderDetailService.InvoiceObject(deliveryOrderDetail, salesInvoiceDetail.Quantity);
            }
            return salesInvoiceDetail;
        }

        public SalesInvoiceDetail UnconfirmObject(SalesInvoiceDetail salesInvoiceDetail, IDeliveryOrderService _deliveryOrderService,
                                                  IDeliveryOrderDetailService _deliveryOrderDetailService)
        {
            if (_validator.ValidUnconfirmObject(salesInvoiceDetail))
            {
                salesInvoiceDetail = _repository.UnconfirmObject(salesInvoiceDetail);
                // reverse sales receival detail PendingInvoiceQuantity
                DeliveryOrderDetail deliveryOrderDetail = _deliveryOrderDetailService.GetObjectById(salesInvoiceDetail.DeliveryOrderDetailId);
                _deliveryOrderDetailService.UndoInvoiceObject(deliveryOrderDetail, salesInvoiceDetail.Quantity, _deliveryOrderService);
            }
            return salesInvoiceDetail;
        }
    }
}