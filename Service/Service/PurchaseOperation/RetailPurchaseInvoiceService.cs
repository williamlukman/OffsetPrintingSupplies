using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Constants;
using Core.DomainModel;
using Core.Interface.Repository;
using Core.Interface.Service;
using Core.Interface.Validation;

namespace Service.Service
{
    public class RetailPurchaseInvoiceService : IRetailPurchaseInvoiceService
    {
        private IRetailPurchaseInvoiceRepository _repository;
        private IRetailPurchaseInvoiceValidator _validator;
        public RetailPurchaseInvoiceService(IRetailPurchaseInvoiceRepository _retailPurchaseInvoiceRepository, IRetailPurchaseInvoiceValidator _retailPurchaseInvoiceValidator)
        {
            _repository = _retailPurchaseInvoiceRepository;
            _validator = _retailPurchaseInvoiceValidator;
        }

        public IRetailPurchaseInvoiceValidator GetValidator()
        {
            return _validator;
        }

        public IRetailPurchaseInvoiceRepository GetRepository()
        {
            return _repository;
        }

        public IQueryable<RetailPurchaseInvoice> GetQueryable()
        {
            return _repository.GetQueryable();
        }

        public IList<RetailPurchaseInvoice> GetAll()
        {
            return _repository.GetAll();
        }

        public RetailPurchaseInvoice GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public RetailPurchaseInvoice CreateObject(RetailPurchaseInvoice retailPurchaseInvoice, IWarehouseService _warehouseService)
        {
            retailPurchaseInvoice.Errors = new Dictionary<String, String>();
            return (_validator.ValidCreateObject(retailPurchaseInvoice, _warehouseService) ? _repository.CreateObject(retailPurchaseInvoice) : retailPurchaseInvoice);
        }

        public RetailPurchaseInvoice UpdateObject(RetailPurchaseInvoice retailPurchaseInvoice, IRetailPurchaseInvoiceDetailService _retailPurchaseInvoiceDetailService)
        {
            return (retailPurchaseInvoice = _validator.ValidUpdateObject(retailPurchaseInvoice, _retailPurchaseInvoiceDetailService) ? _repository.UpdateObject(retailPurchaseInvoice) : retailPurchaseInvoice);
        }

        public RetailPurchaseInvoice ConfirmObject(RetailPurchaseInvoice retailPurchaseInvoice, DateTime ConfirmationDate, int ContactId, 
                                                IRetailPurchaseInvoiceDetailService _retailPurchaseInvoiceDetailService, IContactService _contactService,
                                                IPriceMutationService _priceMutationService, IPayableService _payableService, 
                                                IRetailPurchaseInvoiceService _retailPurchaseInvoiceService, IWarehouseItemService _warehouseItemService,
                                                IWarehouseService _warehouseService, IItemService _itemService, IBlanketService _blanketService, IStockMutationService _stockMutationService)
        {
            if (retailPurchaseInvoice.IsGroupPricing)
            {
                retailPurchaseInvoice.ContactId = ContactId;
            }
            retailPurchaseInvoice.ConfirmationDate = ConfirmationDate;
            if (_validator.ValidConfirmObject(retailPurchaseInvoice, _retailPurchaseInvoiceDetailService, _retailPurchaseInvoiceService, _warehouseItemService, _contactService))
            {
                IList<RetailPurchaseInvoiceDetail> retailPurchaseInvoiceDetails = _retailPurchaseInvoiceDetailService.GetObjectsByRetailPurchaseInvoiceId(retailPurchaseInvoice.Id);
                retailPurchaseInvoice.Total = 0;
                retailPurchaseInvoice.CoGS = 0;
                foreach (var retailPurchaseInvoiceDetail in retailPurchaseInvoiceDetails)
                {
                    retailPurchaseInvoice.Total += retailPurchaseInvoiceDetail.Amount;
                    retailPurchaseInvoice.CoGS += retailPurchaseInvoiceDetail.CoGS;
                }
                retailPurchaseInvoice.Total = retailPurchaseInvoice.Total - (retailPurchaseInvoice.Total * retailPurchaseInvoice.Discount) + (retailPurchaseInvoice.Total * retailPurchaseInvoice.Tax);
                Payable payable = _payableService.CreateObject(retailPurchaseInvoice.ContactId, Core.Constants.Constant.PayableSource.RetailPurchaseInvoice, retailPurchaseInvoice.Id,retailPurchaseInvoice.CurrencyId, retailPurchaseInvoice.Total, (DateTime)retailPurchaseInvoice.DueDate);
                foreach (var retailPurchaseInvoiceDetail in retailPurchaseInvoiceDetails)
                {
                    retailPurchaseInvoiceDetail.Errors = new Dictionary<string,string>();
                    _retailPurchaseInvoiceDetailService.ConfirmObject(retailPurchaseInvoiceDetail,_retailPurchaseInvoiceService,_warehouseItemService,
                                                                   _warehouseService, _itemService, _blanketService, _stockMutationService);
                }
                retailPurchaseInvoice = _repository.ConfirmObject(retailPurchaseInvoice);
            }
            return retailPurchaseInvoice;
        }

        public RetailPurchaseInvoice UnconfirmObject(RetailPurchaseInvoice retailPurchaseInvoice, IRetailPurchaseInvoiceDetailService _retailPurchaseInvoiceDetailService,
                                                  IPayableService _payableService, IPaymentVoucherDetailService _paymentVoucherDetailService,
                                                  IWarehouseItemService _warehouseItemService, IWarehouseService _warehouseService, IItemService _itemService, 
                                                  IBlanketService _blanketService, IStockMutationService _stockMutationService)
        {
            if (_validator.ValidUnconfirmObject(retailPurchaseInvoice, _retailPurchaseInvoiceDetailService, _payableService, _paymentVoucherDetailService))
            {
                retailPurchaseInvoice = _repository.UnconfirmObject(retailPurchaseInvoice);
                IList<RetailPurchaseInvoiceDetail> retailPurchaseInvoiceDetails = _retailPurchaseInvoiceDetailService.GetObjectsByRetailPurchaseInvoiceId(retailPurchaseInvoice.Id);
                foreach (var retailPurchaseInvoiceDetail in retailPurchaseInvoiceDetails)
                {
                    retailPurchaseInvoiceDetail.Errors = new Dictionary<string, string>();
                    _retailPurchaseInvoiceDetailService.UnconfirmObject(retailPurchaseInvoiceDetail, _warehouseItemService, _warehouseService, _itemService, _blanketService, _stockMutationService);
                }
                Payable payable = _payableService.GetObjectBySource(Core.Constants.Constant.PayableSource.RetailPurchaseInvoice, retailPurchaseInvoice.Id);
                _payableService.SoftDeleteObject(payable);
            }
            return retailPurchaseInvoice;
        }

        public RetailPurchaseInvoice PaidObject(RetailPurchaseInvoice retailPurchaseInvoice, ICashBankService _cashBankService, IPayableService _payableService, 
                                             IPaymentVoucherService _paymentVoucherService, IPaymentVoucherDetailService _paymentVoucherDetailService, IContactService _contactService)
        {
            if (_validator.ValidPaidObject(retailPurchaseInvoice, _cashBankService, _paymentVoucherService))
            {
                if (!retailPurchaseInvoice.IsGBCH)
                {
                    retailPurchaseInvoice.GBCH_No = null;
                    retailPurchaseInvoice.Description = null;
                }
                Payable payable = _payableService.GetObjectBySource(Core.Constants.Constant.PayableSource.RetailPurchaseInvoice, retailPurchaseInvoice.Id);
                if (payable != null)
                {
                    PaymentVoucher paymentVoucher = new PaymentVoucher()
                    {
                        CashBankId = retailPurchaseInvoice.CashBankId,
                        ContactId = retailPurchaseInvoice.ContactId,
                        PaymentDate = DateTime.Now,
                        TotalAmount = retailPurchaseInvoice.Total,
                        IsGBCH = retailPurchaseInvoice.IsGBCH,
                        DueDate = (DateTime) retailPurchaseInvoice.DueDate,
                    };
                    paymentVoucher = _paymentVoucherService.CreateObject(paymentVoucher, _paymentVoucherDetailService, _payableService, _contactService, _cashBankService); 
                    retailPurchaseInvoice = _repository.PaidObject(retailPurchaseInvoice);
                }
            }
            return retailPurchaseInvoice;
        }

        public RetailPurchaseInvoice UnpaidObject(RetailPurchaseInvoice retailPurchaseInvoice, IPaymentVoucherService _paymentVoucherService, IPaymentVoucherDetailService _paymentVoucherDetailService)
        {
            if (_validator.ValidUnpaidObject(retailPurchaseInvoice))
            {
                IList<PaymentVoucher> paymentVouchers = _paymentVoucherService.GetObjectsByCashBankId(retailPurchaseInvoice.CashBankId);
                foreach (var paymentVoucher in paymentVouchers)
                {
                    paymentVoucher.Errors = new Dictionary<string, string>();
                    _paymentVoucherService.SoftDeleteObject(paymentVoucher, _paymentVoucherDetailService);
                }
                retailPurchaseInvoice = _repository.UnpaidObject(retailPurchaseInvoice);
            }
            return retailPurchaseInvoice;
        }

        public RetailPurchaseInvoice SoftDeleteObject(RetailPurchaseInvoice retailPurchaseInvoice, IRetailPurchaseInvoiceDetailService _retailPurchaseInvoiceDetailService)
        {
            return (retailPurchaseInvoice = _validator.ValidDeleteObject(retailPurchaseInvoice, _retailPurchaseInvoiceDetailService) ?
                    _repository.SoftDeleteObject(retailPurchaseInvoice) : retailPurchaseInvoice);
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }
    }
}
