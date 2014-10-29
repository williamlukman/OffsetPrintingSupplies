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
    public class SalesQuotationService : ISalesQuotationService
    {
        private ISalesQuotationRepository _repository;
        private ISalesQuotationValidator _validator;

        public SalesQuotationService(ISalesQuotationRepository _salesQuotationRepository, ISalesQuotationValidator _salesQuotationValidator)
        {
            _repository = _salesQuotationRepository;
            _validator = _salesQuotationValidator;
        }

        public ISalesQuotationValidator GetValidator()
        {
            return _validator;
        }

        public IQueryable<SalesQuotation> GetQueryable()
        {
            return _repository.GetQueryable();
        }

        public IList<SalesQuotation> GetAll()
        {
            return _repository.GetAll();
        }

        public IList<SalesQuotation> GetApprovedObjects()
        {
            return _repository.GetApprovedObjects();
        }

        public SalesQuotation GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public IList<SalesQuotation> GetObjectsByContactId(int contactId)
        {
            return _repository.GetObjectsByContactId(contactId);
        }
        
        public SalesQuotation CreateObject(SalesQuotation salesQuotation, IContactService _contactService)
        {
            salesQuotation.Errors = new Dictionary<String, String>();
            return (_validator.ValidCreateObject(salesQuotation, this, _contactService) ? _repository.CreateObject(salesQuotation) : salesQuotation);
        }

        public SalesQuotation UpdateObject(SalesQuotation salesQuotation, IContactService _contactService)
        {
            return (_validator.ValidUpdateObject(salesQuotation, this, _contactService) ? _repository.UpdateObject(salesQuotation) : salesQuotation);
        }

        public SalesQuotation SoftDeleteObject(SalesQuotation salesQuotation, ISalesQuotationDetailService _salesQuotationDetailService)
        {
            return (_validator.ValidDeleteObject(salesQuotation, _salesQuotationDetailService) ? _repository.SoftDeleteObject(salesQuotation) : salesQuotation);
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }

        public SalesQuotation ConfirmObject(SalesQuotation salesQuotation, DateTime ConfirmationDate, ISalesQuotationDetailService _salesQuotationDetailService,
                                            IItemService _itemService, IWarehouseItemService _warehouseItemService)
        {
            salesQuotation.ConfirmationDate = ConfirmationDate;
            if (_validator.ValidConfirmObject(salesQuotation, _salesQuotationDetailService))
            {
                IList<SalesQuotationDetail> salesQuotationDetails = _salesQuotationDetailService.GetObjectsBySalesQuotationId(salesQuotation.Id);
                decimal TotalQuotedAmount = 0;
                decimal TotalRRP = 0;
                decimal CostSaved = 0;
                decimal PercentageSaved = 0;
                foreach (var detail in salesQuotationDetails)
                {
                    detail.Errors = new Dictionary<string, string>();
                    _salesQuotationDetailService.ConfirmObject(detail, ConfirmationDate, _itemService, _warehouseItemService);
                    TotalQuotedAmount += (detail.Quantity * detail.QuotationPrice);
                    TotalRRP += (detail.Quantity * detail.RRP);
                }
                CostSaved = TotalRRP - TotalQuotedAmount;
                PercentageSaved = ( CostSaved == 0 || TotalRRP == 0) ? 0 : CostSaved / TotalRRP * 100;
                salesQuotation.TotalQuotedAmount = TotalQuotedAmount;
                salesQuotation.TotalRRPAmount = TotalRRP;
                salesQuotation.CostSaved = CostSaved;
                salesQuotation.PercentageSaved = PercentageSaved;
                _repository.ConfirmObject(salesQuotation);
            }
            return salesQuotation;
        }

        public SalesQuotation UnconfirmObject(SalesQuotation salesQuotation, ISalesQuotationDetailService _salesQuotationDetailService,
                                              IItemService _itemService, IWarehouseItemService _warehouseItemService,
                                              ISalesOrderService _salesOrderService, ISalesOrderDetailService _salesOrderDetailService)
        {
            if (_validator.ValidUnconfirmObject(salesQuotation, _salesOrderService))
            {
                IList<SalesQuotationDetail> salesQuotationDetails = _salesQuotationDetailService.GetObjectsBySalesQuotationId(salesQuotation.Id);
                foreach (var detail in salesQuotationDetails)
                {
                    detail.Errors = new Dictionary<string, string>();
                    _salesQuotationDetailService.UnconfirmObject(detail, this, _salesOrderDetailService, _itemService, _warehouseItemService);
                }
                salesQuotation.TotalQuotedAmount = 0;
                salesQuotation.TotalRRPAmount = 0;
                salesQuotation.CostSaved = 0;
                salesQuotation.PercentageSaved = 0;
                _repository.UnconfirmObject(salesQuotation);
            }
            return salesQuotation;
        }

        public SalesQuotation ApproveObject(SalesQuotation salesQuotation)
        {
            if (_validator.ValidApproveObject(salesQuotation))
            {
                _repository.ApproveObject(salesQuotation);
            }
            return salesQuotation;
        }

        public SalesQuotation RejectObject(SalesQuotation salesQuotation)
        {
            if (_validator.ValidRejectObject(salesQuotation))
            {
                _repository.RejectObject(salesQuotation);
            }
            return salesQuotation;
        }
    }
}