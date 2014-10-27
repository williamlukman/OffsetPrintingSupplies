using Core.DomainModel;
using Core.Interface.Repository;
using Core.Interface.Service;
using Core.Interface.Validation;
using Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Service.Service
{
    public class SalesQuotationDetailService : ISalesQuotationDetailService
    {
        private ISalesQuotationDetailRepository _repository;
        private ISalesQuotationDetailValidator _validator;

        public SalesQuotationDetailService(ISalesQuotationDetailRepository _salesQuotationDetailRepository, ISalesQuotationDetailValidator _salesQuotationDetailValidator)
        {
            _repository = _salesQuotationDetailRepository;
            _validator = _salesQuotationDetailValidator;
        }

        public ISalesQuotationDetailValidator GetValidator()
        {
            return _validator;
        }

        public IQueryable<SalesQuotationDetail> GetQueryable()
        {
            return _repository.GetQueryable();
        }

        public IList<SalesQuotationDetail> GetAll()
        {
            return _repository.GetAll();
        }

        public IList<SalesQuotationDetail> GetObjectsBySalesQuotationId(int salesQuotationId)
        {
            return _repository.GetObjectsBySalesQuotationId(salesQuotationId);
        }

        public IList<SalesQuotationDetail> GetObjectsByItemId(int itemId)
        {
            return _repository.GetObjectsByItemId(itemId);
        }

        public SalesQuotationDetail GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public SalesQuotationDetail CreateObject(SalesQuotationDetail salesQuotationDetail, ISalesQuotationService _salesQuotationService, IItemService _itemService)
        {
            salesQuotationDetail.Errors = new Dictionary<String, String>();
            if (_validator.ValidCreateObject(salesQuotationDetail, this, _salesQuotationService, _itemService))
            {
                _repository.CreateObject(salesQuotationDetail);
            }
            return salesQuotationDetail;
        }

        public SalesQuotationDetail UpdateObject(SalesQuotationDetail salesQuotationDetail, ISalesQuotationService _salesQuotationService, IItemService _itemService)
        {
            return (_validator.ValidUpdateObject(salesQuotationDetail, this, _salesQuotationService, _itemService) ? _repository.UpdateObject(salesQuotationDetail) : salesQuotationDetail);
        }

        public SalesQuotationDetail SoftDeleteObject(SalesQuotationDetail salesQuotationDetail)
        {
            return (_validator.ValidDeleteObject(salesQuotationDetail) ? _repository.SoftDeleteObject(salesQuotationDetail) : salesQuotationDetail);
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }

        public SalesQuotationDetail ConfirmObject(SalesQuotationDetail salesQuotationDetail, DateTime ConfirmationDate, 
                                                  IItemService _itemService, IWarehouseItemService _warehouseItemService)
        {
            salesQuotationDetail.ConfirmationDate = ConfirmationDate;
            if (_validator.ValidConfirmObject(salesQuotationDetail))
            {
                salesQuotationDetail = _repository.ConfirmObject(salesQuotationDetail);
            }
            return salesQuotationDetail;
        }

        public SalesQuotationDetail UnconfirmObject(SalesQuotationDetail salesQuotationDetail, ISalesQuotationService _salesQuotationService, ISalesOrderDetailService _salesOrderDetailService,
                                                    IItemService _itemService, IWarehouseItemService _warehouseItemService)
        {
            if (_validator.ValidUnconfirmObject(salesQuotationDetail, _salesOrderDetailService, _itemService))
            {
                salesQuotationDetail = _repository.UnconfirmObject(salesQuotationDetail);
            }
            return salesQuotationDetail;
        }
    }
}