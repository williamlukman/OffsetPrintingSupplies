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
    public class RetailSalesInvoiceService : IRetailSalesInvoiceService
    {
        private IRetailSalesInvoiceRepository _repository;
        private IRetailSalesInvoiceValidator _validator;
        public RetailSalesInvoiceService(IRetailSalesInvoiceRepository _retailSalesInvoiceRepository, IRetailSalesInvoiceValidator _retailSalesInvoiceValidator)
        {
            _repository = _retailSalesInvoiceRepository;
            _validator = _retailSalesInvoiceValidator;
        }

        public IRetailSalesInvoiceValidator GetValidator()
        {
            return _validator;
        }

        public IRetailSalesInvoiceRepository GetRepository()
        {
            return _repository;
        }

        public IQueryable<RetailSalesInvoice> GetQueryable()
        {
            return _repository.GetQueryable();
        }

        public IList<RetailSalesInvoice> GetAll()
        {
            return _repository.GetAll();
        }

        public RetailSalesInvoice GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public RetailSalesInvoice CreateObject(RetailSalesInvoice retailSalesInvoice, IWarehouseService _warehouseService)
        {
            retailSalesInvoice.Errors = new Dictionary<String, String>();
            return (_validator.ValidCreateObject(retailSalesInvoice, _warehouseService) ? _repository.CreateObject(retailSalesInvoice) : retailSalesInvoice);
        }

        public RetailSalesInvoice UpdateObject(RetailSalesInvoice retailSalesInvoice, IRetailSalesInvoiceDetailService _retailSalesInvoiceDetailService)
        {
            return (retailSalesInvoice = _validator.ValidUpdateObject(retailSalesInvoice, _retailSalesInvoiceDetailService) ? _repository.UpdateObject(retailSalesInvoice) : retailSalesInvoice);
        }

        public RetailSalesInvoice ConfirmObject(RetailSalesInvoice retailSalesInvoice, DateTime ConfirmationDate, int ContactId, 
                                                IRetailSalesInvoiceDetailService _retailSalesInvoiceDetailService, IContactService _contactService,
                                                IPriceMutationService _priceMutationService, IReceivableService _receivableService, 
                                                IRetailSalesInvoiceService _retailSalesInvoiceService, IWarehouseItemService _warehouseItemService,
                                                IWarehouseService _warehouseService, IItemService _itemService, IBlanketService _blanketService, IStockMutationService _stockMutationService)
        {
            if (retailSalesInvoice.IsGroupPricing)
            {
                retailSalesInvoice.ContactId = ContactId;
            }
            retailSalesInvoice.ConfirmationDate = ConfirmationDate;
            if (_validator.ValidConfirmObject(retailSalesInvoice, _retailSalesInvoiceDetailService, _retailSalesInvoiceService, _warehouseItemService, _contactService))
            {
                IList<RetailSalesInvoiceDetail> retailSalesInvoiceDetails = _retailSalesInvoiceDetailService.GetObjectsByRetailSalesInvoiceId(retailSalesInvoice.Id);
                retailSalesInvoice.Total = 0;
                retailSalesInvoice.CoGS = 0;
                foreach (var retailSalesInvoiceDetail in retailSalesInvoiceDetails)
                {
                    retailSalesInvoice.Total += retailSalesInvoiceDetail.Amount;
                    retailSalesInvoice.CoGS += retailSalesInvoiceDetail.CoGS;
                }
                retailSalesInvoice.Total = retailSalesInvoice.Total - (retailSalesInvoice.Total * retailSalesInvoice.Discount) + (retailSalesInvoice.Total * retailSalesInvoice.Tax);
                Receivable receivable = _receivableService.CreateObject(retailSalesInvoice.ContactId, Core.Constants.Constant.ReceivableSource.RetailSalesInvoice, retailSalesInvoice.Id,retailSalesInvoice.CurrencyId, retailSalesInvoice.Total, (DateTime)retailSalesInvoice.DueDate);
                foreach (var retailSalesInvoiceDetail in retailSalesInvoiceDetails)
                {
                    retailSalesInvoiceDetail.Errors = new Dictionary<string,string>();
                    _retailSalesInvoiceDetailService.ConfirmObject(retailSalesInvoiceDetail,_retailSalesInvoiceService,_warehouseItemService,
                                                                   _warehouseService, _itemService, _blanketService, _stockMutationService);
                }
                retailSalesInvoice = _repository.ConfirmObject(retailSalesInvoice);
            }
            return retailSalesInvoice;
        }

        public RetailSalesInvoice UnconfirmObject(RetailSalesInvoice retailSalesInvoice, IRetailSalesInvoiceDetailService _retailSalesInvoiceDetailService,
                                                  IReceivableService _receivableService, IReceiptVoucherDetailService _receiptVoucherDetailService,
                                                  IWarehouseItemService _warehouseItemService, IWarehouseService _warehouseService, IItemService _itemService, 
                                                  IBlanketService _blanketService, IStockMutationService _stockMutationService)
        {
            if (_validator.ValidUnconfirmObject(retailSalesInvoice, _retailSalesInvoiceDetailService, _receivableService, _receiptVoucherDetailService))
            {
                retailSalesInvoice = _repository.UnconfirmObject(retailSalesInvoice);
                IList<RetailSalesInvoiceDetail> retailSalesInvoiceDetails = _retailSalesInvoiceDetailService.GetObjectsByRetailSalesInvoiceId(retailSalesInvoice.Id);
                foreach (var retailSalesInvoiceDetail in retailSalesInvoiceDetails)
                {
                    retailSalesInvoiceDetail.Errors = new Dictionary<string, string>();
                    _retailSalesInvoiceDetailService.UnconfirmObject(retailSalesInvoiceDetail, _warehouseItemService, _warehouseService, _itemService, _blanketService, _stockMutationService);
                }
                Receivable receivable = _receivableService.GetObjectBySource(Core.Constants.Constant.ReceivableSource.RetailSalesInvoice, retailSalesInvoice.Id);
                _receivableService.SoftDeleteObject(receivable);
            }
            return retailSalesInvoice;
        }

        public RetailSalesInvoice PaidObject(RetailSalesInvoice retailSalesInvoice, ICashBankService _cashBankService, IReceivableService _receivableService, 
                                             IReceiptVoucherService _receiptVoucherService, IReceiptVoucherDetailService _receiptVoucherDetailService, IContactService _contactService)
        {
            if (_validator.ValidPaidObject(retailSalesInvoice, _cashBankService, _receiptVoucherService))
            {
                if (!retailSalesInvoice.IsGBCH)
                {
                    retailSalesInvoice.GBCH_No = null;
                    retailSalesInvoice.Description = null;
                }
                Receivable receivable = _receivableService.GetObjectBySource(Core.Constants.Constant.ReceivableSource.RetailSalesInvoice, retailSalesInvoice.Id);
                if (receivable != null)
                {
                    ReceiptVoucher receiptVoucher = _receiptVoucherService.CreateObject(retailSalesInvoice.CashBankId, retailSalesInvoice.ContactId, DateTime.Now, retailSalesInvoice.Total, 
                                                                            retailSalesInvoice.IsGBCH, (DateTime)retailSalesInvoice.DueDate, retailSalesInvoice.IsBank, _receiptVoucherDetailService, 
                                                                            _receivableService, _contactService, _cashBankService ); 
                    retailSalesInvoice = _repository.PaidObject(retailSalesInvoice);
                }
            }
            return retailSalesInvoice;
        }

        public RetailSalesInvoice UnpaidObject(RetailSalesInvoice retailSalesInvoice, IReceiptVoucherService _receiptVoucherService, IReceiptVoucherDetailService _receiptVoucherDetailService)
        {
            if (_validator.ValidUnpaidObject(retailSalesInvoice))
            {
                IList<ReceiptVoucher> receiptVouchers = _receiptVoucherService.GetObjectsByCashBankId(retailSalesInvoice.CashBankId);
                foreach (var receiptVoucher in receiptVouchers)
                {
                    receiptVoucher.Errors = new Dictionary<string, string>();
                    _receiptVoucherService.SoftDeleteObject(receiptVoucher, _receiptVoucherDetailService);
                }
                retailSalesInvoice = _repository.UnpaidObject(retailSalesInvoice);
            }
            return retailSalesInvoice;
        }

        public RetailSalesInvoice SoftDeleteObject(RetailSalesInvoice retailSalesInvoice, IRetailSalesInvoiceDetailService _retailSalesInvoiceDetailService)
        {
            return (retailSalesInvoice = _validator.ValidDeleteObject(retailSalesInvoice, _retailSalesInvoiceDetailService) ?
                    _repository.SoftDeleteObject(retailSalesInvoice) : retailSalesInvoice);
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }
    }
}
