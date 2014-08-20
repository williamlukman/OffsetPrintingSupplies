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
    public class CustomPurchaseInvoiceService : ICustomPurchaseInvoiceService
    {
        private ICustomPurchaseInvoiceRepository _repository;
        private ICustomPurchaseInvoiceValidator _validator;
        public CustomPurchaseInvoiceService(ICustomPurchaseInvoiceRepository _customPurchaseInvoiceRepository, ICustomPurchaseInvoiceValidator _customPurchaseInvoiceValidator)
        {
            _repository = _customPurchaseInvoiceRepository;
            _validator = _customPurchaseInvoiceValidator;
        }

        public ICustomPurchaseInvoiceValidator GetValidator()
        {
            return _validator;
        }

        public ICustomPurchaseInvoiceRepository GetRepository()
        {
            return _repository;
        }

        public IList<CustomPurchaseInvoice> GetAll()
        {
            return _repository.GetAll();
        }

        public CustomPurchaseInvoice GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public CustomPurchaseInvoice CreateObject(CustomPurchaseInvoice customPurchaseInvoice, IWarehouseService _warehouseService, IContactService _contactService)
        {
            customPurchaseInvoice.Errors = new Dictionary<String, String>();
            return (_validator.ValidCreateObject(customPurchaseInvoice, _warehouseService, _contactService) ? _repository.CreateObject(customPurchaseInvoice) : customPurchaseInvoice);
        }

        public CustomPurchaseInvoice UpdateObject(CustomPurchaseInvoice customPurchaseInvoice, ICustomPurchaseInvoiceDetailService _customPurchaseInvoiceDetailService,
                                                  IWarehouseService _warehouseService, IContactService _contactService)
        {
            return (customPurchaseInvoice = _validator.ValidUpdateObject(customPurchaseInvoice, _customPurchaseInvoiceDetailService, _warehouseService, _contactService) ? _repository.UpdateObject(customPurchaseInvoice) : customPurchaseInvoice);
        }

        public CustomPurchaseInvoice ConfirmObject(CustomPurchaseInvoice customPurchaseInvoice, DateTime ConfirmationDate, 
                                                ICustomPurchaseInvoiceDetailService _customPurchaseInvoiceDetailService, IContactService _contactService,
                                                IPriceMutationService _priceMutationService, IPayableService _payableService, 
                                                ICustomPurchaseInvoiceService _customPurchaseInvoiceService, IWarehouseItemService _warehouseItemService,
                                                IWarehouseService _warehouseService, IItemService _itemService, IBarringService _barringService, IStockMutationService _stockMutationService)
        {
            customPurchaseInvoice.ConfirmationDate = ConfirmationDate;
            if (_validator.ValidConfirmObject(customPurchaseInvoice, _customPurchaseInvoiceDetailService, _customPurchaseInvoiceService, _warehouseItemService, _contactService))
            {
                IList<CustomPurchaseInvoiceDetail> customPurchaseInvoiceDetails = _customPurchaseInvoiceDetailService.GetObjectsByCustomPurchaseInvoiceId(customPurchaseInvoice.Id);
                customPurchaseInvoice.Total = 0;
                customPurchaseInvoice.CoGS = 0;
                foreach (var customPurchaseInvoiceDetail in customPurchaseInvoiceDetails)
                {
                    customPurchaseInvoiceDetail.Errors = new Dictionary<string, string>();
                    _customPurchaseInvoiceDetailService.ConfirmObject(customPurchaseInvoiceDetail, _customPurchaseInvoiceService, _warehouseItemService,
                                                                   _warehouseService, _itemService, _barringService, _stockMutationService);
                    customPurchaseInvoice.Total += customPurchaseInvoiceDetail.Amount;
                    customPurchaseInvoice.CoGS += customPurchaseInvoiceDetail.CoGS;
                }
                customPurchaseInvoice.Total = customPurchaseInvoice.Total - (customPurchaseInvoice.Total * customPurchaseInvoice.Discount / 100) + (customPurchaseInvoice.Total * customPurchaseInvoice.Tax / 100);
                Payable payable = _payableService.CreateObject(customPurchaseInvoice.ContactId, Core.Constants.Constant.PayableSource.CustomPurchaseInvoice, customPurchaseInvoice.Id, customPurchaseInvoice.Total, (DateTime)customPurchaseInvoice.DueDate);
                customPurchaseInvoice = _repository.ConfirmObject(customPurchaseInvoice);
            }
            else
            {
                customPurchaseInvoice.ConfirmationDate = null;
            }
            return customPurchaseInvoice;
        }

        public CustomPurchaseInvoice UnconfirmObject(CustomPurchaseInvoice customPurchaseInvoice, ICustomPurchaseInvoiceDetailService _customPurchaseInvoiceDetailService,
                                                  IPayableService _payableService, IPaymentVoucherDetailService _paymentVoucherDetailService,
                                                  IWarehouseItemService _warehouseItemService, IWarehouseService _warehouseService, IItemService _itemService, 
                                                  IBarringService _barringService, IStockMutationService _stockMutationService)
        {
            if (_validator.ValidUnconfirmObject(customPurchaseInvoice, _customPurchaseInvoiceDetailService, _payableService, _paymentVoucherDetailService))
            {
                customPurchaseInvoice = _repository.UnconfirmObject(customPurchaseInvoice);
                IList<CustomPurchaseInvoiceDetail> customPurchaseInvoiceDetails = _customPurchaseInvoiceDetailService.GetObjectsByCustomPurchaseInvoiceId(customPurchaseInvoice.Id);
                foreach (var customPurchaseInvoiceDetail in customPurchaseInvoiceDetails)
                {
                    customPurchaseInvoiceDetail.Errors = new Dictionary<string, string>();
                    _customPurchaseInvoiceDetailService.UnconfirmObject(customPurchaseInvoiceDetail, _warehouseItemService, _warehouseService, _itemService, _barringService, _stockMutationService);
                }
                Payable payable = _payableService.GetObjectBySource(Core.Constants.Constant.PayableSource.CustomPurchaseInvoice, customPurchaseInvoice.Id);
                _payableService.SoftDeleteObject(payable);
            }
            return customPurchaseInvoice;
        }

        public CustomPurchaseInvoice PaidObject(CustomPurchaseInvoice customPurchaseInvoice, decimal AmountPaid, ICashBankService _cashBankService, IPayableService _payableService, 
                                             IPaymentVoucherService _paymentVoucherService, IPaymentVoucherDetailService _paymentVoucherDetailService, IContactService _contactService, ICashMutationService _cashMutationService)
        {
            if (_validator.ValidPaidObject(customPurchaseInvoice, _cashBankService, _paymentVoucherService))
            {
                CashBank cashBank = _cashBankService.GetObjectById((int)customPurchaseInvoice.CashBankId);
                customPurchaseInvoice.IsBank = cashBank.IsBank;
                customPurchaseInvoice.AmountPaid = AmountPaid;
                if (!customPurchaseInvoice.IsGBCH)
                {
                    customPurchaseInvoice.GBCH_No = null;
                    customPurchaseInvoice.Description = null;
                }
                if (customPurchaseInvoice.AmountPaid == customPurchaseInvoice.Total)
                {
                    customPurchaseInvoice.IsFullPayment = true;
                }
                Payable payable = _payableService.GetObjectBySource(Core.Constants.Constant.PayableSource.CustomPurchaseInvoice, customPurchaseInvoice.Id);
                payable.RemainingAmount = payable.Amount - customPurchaseInvoice.Allowance;
                _payableService.UpdateObject(payable);
                PaymentVoucher paymentVoucher = _paymentVoucherService.CreateObject((int)customPurchaseInvoice.CashBankId, customPurchaseInvoice.ContactId, DateTime.Now, customPurchaseInvoice.Total - customPurchaseInvoice.Allowance,
                                                                            customPurchaseInvoice.IsGBCH, (DateTime)customPurchaseInvoice.DueDate, customPurchaseInvoice.IsBank, _paymentVoucherDetailService,
                                                                            _payableService, _contactService, _cashBankService);
                PaymentVoucherDetail paymentVoucherDetail = _paymentVoucherDetailService.CreateObject(paymentVoucher.Id, payable.Id, (decimal)customPurchaseInvoice.AmountPaid, 
                                                                            "Automatic Payment", _paymentVoucherService, _cashBankService, _payableService);
                customPurchaseInvoice = _repository.PaidObject(customPurchaseInvoice);
                _paymentVoucherService.ConfirmObject(paymentVoucher, (DateTime)customPurchaseInvoice.ConfirmationDate, _paymentVoucherDetailService, _cashBankService, _payableService, _cashMutationService);
            }
            return customPurchaseInvoice;
        }

        public CustomPurchaseInvoice UnpaidObject(CustomPurchaseInvoice customPurchaseInvoice, IPaymentVoucherService _paymentVoucherService, IPaymentVoucherDetailService _paymentVoucherDetailService, ICashBankService _cashBankService, IPayableService _payableService, ICashMutationService _cashMutationService)
        {
            if (_validator.ValidUnpaidObject(customPurchaseInvoice))
            {
                Payable payable = _payableService.GetObjectBySource(Core.Constants.Constant.PayableSource.CustomPurchaseInvoice, customPurchaseInvoice.Id);
                IList<PaymentVoucher> paymentVouchers = _paymentVoucherService.GetObjectsByCashBankId((int)customPurchaseInvoice.CashBankId);
                foreach (var paymentVoucher in paymentVouchers)
                {
                    if (paymentVoucher.ContactId == customPurchaseInvoice.ContactId)
                    {
                        paymentVoucher.Errors = new Dictionary<string, string>();
                        _paymentVoucherService.UnconfirmObject(paymentVoucher, _paymentVoucherDetailService, _cashBankService, _payableService, _cashMutationService);

                        IList<PaymentVoucherDetail> paymentVoucherDetails = _paymentVoucherDetailService.GetObjectsByPaymentVoucherId(paymentVoucher.Id);
                        foreach (var paymentVoucherDetail in paymentVoucherDetails)
                        {
                            _paymentVoucherDetailService.SoftDeleteObject(paymentVoucherDetail);
                        }
                        _paymentVoucherService.SoftDeleteObject(paymentVoucher, _paymentVoucherDetailService);
                    }
                }
                customPurchaseInvoice.AmountPaid = 0;
                customPurchaseInvoice.IsFullPayment = false;
                customPurchaseInvoice = _repository.UnpaidObject(customPurchaseInvoice);
            }
            return customPurchaseInvoice;
        }

        public CustomPurchaseInvoice SoftDeleteObject(CustomPurchaseInvoice customPurchaseInvoice, ICustomPurchaseInvoiceDetailService _customPurchaseInvoiceDetailService)
        {
            return (customPurchaseInvoice = _validator.ValidDeleteObject(customPurchaseInvoice, _customPurchaseInvoiceDetailService) ?
                    _repository.SoftDeleteObject(customPurchaseInvoice) : customPurchaseInvoice);
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }
    }
}
