using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Service.Service;
using Core.Interface.Service;
using Core.DomainModel;
using Data.Repository;
using Validation.Validation;
using System.Linq.Dynamic;
using System.Data.Entity;
using Core.Constants;

namespace WebView.Controllers
{
    public class ReceiptVoucherController : Controller
    {
        private readonly static log4net.ILog LOG = log4net.LogManager.GetLogger("ReceiptVoucherController");
        private IPurchaseOrderService _purchaseOrderService;
        private IPurchaseOrderDetailService _purchaseOrderDetailService;
        private IPurchaseInvoiceService _purchaseInvoiceService;
        private IPurchaseInvoiceDetailService _purchaseInvoiceDetailService;
        private IPurchaseReceivalService _purchaseReceivalService;
        private IPurchaseReceivalDetailService _purchaseReceivalDetailService;
        private IPaymentVoucherDetailService _paymentVoucherDetailService;
        private IReceivableService _receivableService;
        private IItemService _itemService;
        private IPaymentVoucherService _paymentVoucherService;
        private ICashBankService _cashBankService;
        private ICashMutationService _cashMutationService;
        private IContactService _contactService;
        private IReceiptVoucherService _receiptVoucherService;
        private IReceiptVoucherDetailService _receiptVoucherDetailService;
        private IAccountService _accountService;
        private IGeneralLedgerJournalService _generalLedgerJournalService;
        private IClosingService _closingService;
        public ICurrencyService _currencyService;
        private ISalesInvoiceService _salesInvoiceService;
        private IExchangeRateService _exchangeRateService;
        private IGLNonBaseCurrencyService _gLNonBaseCurrencyService;
        public ReceiptVoucherController()
        {
            _accountService = new AccountService(new AccountRepository(), new AccountValidator());
            _generalLedgerJournalService = new GeneralLedgerJournalService(new GeneralLedgerJournalRepository(), new GeneralLedgerJournalValidator());
            _cashBankService = new CashBankService(new CashBankRepository(),new CashBankValidator());
            _cashMutationService = new CashMutationService(new CashMutationRepository(), new CashMutationValidator());
            _purchaseOrderService = new PurchaseOrderService(new PurchaseOrderRepository(), new PurchaseOrderValidator());
            _purchaseOrderDetailService = new PurchaseOrderDetailService(new PurchaseOrderDetailRepository(), new PurchaseOrderDetailValidator());
            _purchaseInvoiceService = new PurchaseInvoiceService(new PurchaseInvoiceRepository(), new PurchaseInvoiceValidator());
            _purchaseInvoiceDetailService = new PurchaseInvoiceDetailService(new PurchaseInvoiceDetailRepository(), new PurchaseInvoiceDetailValidator());
            _purchaseReceivalService = new PurchaseReceivalService(new PurchaseReceivalRepository(), new PurchaseReceivalValidator());
            _purchaseReceivalDetailService = new PurchaseReceivalDetailService(new PurchaseReceivalDetailRepository(), new PurchaseReceivalDetailValidator());
            _paymentVoucherDetailService = new PaymentVoucherDetailService(new PaymentVoucherDetailRepository(), new PaymentVoucherDetailValidator());
            _receivableService = new ReceivableService(new ReceivableRepository(), new ReceivableValidator());
            _itemService = new ItemService(new ItemRepository(), new ItemValidator());
            _paymentVoucherService = new PaymentVoucherService(new PaymentVoucherRepository(), new PaymentVoucherValidator());
            _contactService = new ContactService(new ContactRepository(), new ContactValidator());
            _receiptVoucherService = new ReceiptVoucherService(new ReceiptVoucherRepository(), new ReceiptVoucherValidator());
            _receiptVoucherDetailService = new ReceiptVoucherDetailService(new ReceiptVoucherDetailRepository(), new ReceiptVoucherDetailValidator());
            _closingService = new ClosingService(new ClosingRepository(), new ClosingValidator());
            _currencyService = new CurrencyService(new CurrencyRepository(), new CurrencyValidator());
            _salesInvoiceService = new SalesInvoiceService(new SalesInvoiceRepository(), new SalesInvoiceValidator());
            _exchangeRateService = new ExchangeRateService(new ExchangeRateRepository(), new ExchangeRateValidator());
            _gLNonBaseCurrencyService = new GLNonBaseCurrencyService(new GLNonBaseCurrencyRepository(), new GLNonBaseCurrencyValidator());
        }


        public ActionResult Index()
        {
            return View(this);
        }

        public dynamic GetList(string _search, long nd, int rows, int? page, string sidx, string sord, string filters = "")
        {
            // Construct where statement
            string strWhere = GeneralFunction.ConstructWhere(filters);
            string filter = null;
            GeneralFunction.ConstructWhereInLinq(strWhere, out filter);
            if (filter == "") filter = "true";

            // Get Data
            var q = _receiptVoucherService.GetQueryable().Include("Contact").Include("CashBank").Include("Currency").Where(x => !x.IsDeleted);

            var query = (from model in q
                         select new
                         {
                             model.Id,
                             model.Code,
                             model.ContactId,
                             Contact = model.Contact.Name,
                             model.CashBankId,
                             CashBank = model.CashBank.Name,
                             model.ReceiptDate,
                             model.IsGBCH,
                             model.DueDate,
                             model.TotalAmount,
                             Currency = model.CashBank.Currency.Name,
                             model.RateToIDR,
                             model.IsReconciled,
                             model.ReconciliationDate,
                             model.IsConfirmed,
                             model.ConfirmationDate,
                             model.NoBukti,
                             model.TotalPPH23,
                             model.BiayaBank,
                             Pembulatan = model.Pembulatan * (model.StatusPembulatan == Constant.GeneralLedgerStatus.Credit ? 1 : -1),
                             model.CreatedAt,
                             model.UpdatedAt,
                         }).Where(filter).OrderBy(sidx + " " + sord); //.ToList();

            var list = query.AsEnumerable();

            var pageIndex = Convert.ToInt32(page) - 1;
            var pageSize = rows;
            var totalRecords = query.Count();
            var totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);
            // default last page
            if (totalPages > 0)
            {
                if (!page.HasValue)
                {
                    pageIndex = totalPages - 1;
                    page = totalPages;
                }
            }

            list = list.Skip(pageIndex * pageSize).Take(pageSize);

            return Json(new
            {
                total = totalPages,
                page = page,
                records = totalRecords,
                rows = (
                    from model in list
                    select new
                    {
                        id = model.Id,
                        cell = new object[] {
                            model.Id,
                            model.Code,
                            model.ContactId,
                            model.Contact,
                            model.CashBankId,
                            model.CashBank,
                            model.ReceiptDate,
                            model.IsGBCH,
                            model.DueDate,
                            model.TotalAmount,
                            model.Currency,
                            model.RateToIDR,
                            model.IsReconciled,
                            model.ReconciliationDate,
                            model.IsConfirmed,
                            model.ConfirmationDate,
                            model.NoBukti,
                            model.TotalPPH23,
                            model.BiayaBank,
                            model.Pembulatan,
                            model.CreatedAt,
                            model.UpdatedAt,
                      }
                    }).ToArray()
            }, JsonRequestBehavior.AllowGet);
        }

        public dynamic GetListReceivable(string _search, long nd, int rows, int? page, string sidx, string sord, string filters = "")
        {
            // Construct where statement
            string strWhere = GeneralFunction.ConstructWhere(filters);
            string filter = null;
            GeneralFunction.ConstructWhereInLinq(strWhere, out filter);
            if (filter == "") filter = "true";

            // Get Data
            var q = _receivableService.GetQueryable().Include("Contact").Where(x => !x.IsDeleted && x.RemainingAmount > 0);

            var query = (from model in q
                         select new
                         {
                             model.Id,
                             model.Code,
                             model.ContactId,
                             Contact = model.Contact.Name,
                             model.ReceivableSource,
                             model.ReceivableSourceId,
                             model.DueDate,
                             model.Amount,
                             model.RemainingAmount,
                             model.PendingClearanceAmount,
                             Currency = model.Currency.Name,
                             model.Rate,
                             model.CreatedAt,
                             model.UpdatedAt,
                         }).Where(filter).OrderBy(sidx + " " + sord); //.ToList();

            var list = query.AsEnumerable();

            var pageIndex = Convert.ToInt32(page) - 1;
            var pageSize = rows;
            var totalRecords = query.Count();
            var totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);
            // default last page
            if (totalPages > 0)
            {
                if (!page.HasValue)
                {
                    pageIndex = totalPages - 1;
                    page = totalPages;
                }
            }

            list = list.Skip(pageIndex * pageSize).Take(pageSize);

            return Json(new
            {
                total = totalPages,
                page = page,
                records = totalRecords,
                rows = (
                    from model in list
                    select new
                    {
                        id = model.Id,
                        cell = new object[] {
                            model.Code,
                            model.ContactId,
                            model.Contact,
                            model.ReceivableSource,
                            model.ReceivableSourceId,
                            model.DueDate,
                            model.Amount,
                            model.RemainingAmount,
                            model.PendingClearanceAmount,
                            model.Currency,
                            model.Rate,
                            model.CreatedAt,
                            model.UpdatedAt,
                      }
                    }).ToArray()
            }, JsonRequestBehavior.AllowGet);
        }

        public dynamic GetListReceivableNonDP(string _search, long nd, int rows, int? page, string sidx, string sord, int contactid, string filters = "")
        {
            // Construct where statement
            string strWhere = GeneralFunction.ConstructWhere(filters);
            string filter = null;
            GeneralFunction.ConstructWhereInLinq(strWhere, out filter);
            if (filter == "") filter = "true";

            // Get Data
            //var q = _receivableService.GetQueryable().Include("Contact").Include("Currency").Where(x => !x.IsDeleted && x.RemainingAmount > 0 &&
            //                           x.ReceivableSource != Constant.ReceivableSource.PurchaseDownPayment);
            var q = _receivableService.GetQueryable().Where(x => !x.IsDeleted && x.RemainingAmount > 0 && x.ContactId == contactid &&
                                       x.ReceivableSource != Constant.ReceivableSource.PurchaseDownPayment);

            var query = (from model in q
                         select new
                         {
                             model.Id,
                             model.Code,
                             model.ContactId,
                             Contact = model.Contact.Name,
                             model.DueDate,
                             model.Amount,
                             model.RemainingAmount,
                             model.PendingClearanceAmount,
                             Currency = model.Currency.Name,
                             model.Rate,
                             model.ReceivableSource,
                             model.ReceivableSourceId,
                             model.CreatedAt,
                             model.UpdatedAt,
                         }).Where(filter).OrderBy(sidx + " " + sord); //.ToList();

            var list = query.AsEnumerable();

            var pageIndex = Convert.ToInt32(page) - 1;
            var pageSize = rows;
            var totalRecords = query.Count();
            var totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);
            // default last page
            if (totalPages > 0)
            {
                if (!page.HasValue)
                {
                    pageIndex = totalPages - 1;
                    page = totalPages;
                }
            }

            list = list.Skip(pageIndex * pageSize).Take(pageSize);

            return Json(new
            {
                total = totalPages,
                page = page,
                records = totalRecords,
                rows = (
                    from model in list
                    select new
                    {
                        id = model.Id,
                        cell = new object[] {
                            model.Code,
                            model.ContactId,
                            model.Contact,
                            model.DueDate,
                            model.Amount,
                            model.RemainingAmount,
                            model.PendingClearanceAmount,
                            model.Currency,
                            model.Rate,
                            model.ReceivableSource,
                            model.ReceivableSourceId,
                            model.CreatedAt,
                            model.UpdatedAt,
                      }
                    }).ToArray()
            }, JsonRequestBehavior.AllowGet);
        }

        public dynamic GetListDetail(string _search, long nd, int rows, int? page, string sidx, string sord, int id,string filters = "")
        {
            // Construct where statement
            string strWhere = GeneralFunction.ConstructWhere(filters);
            string filter = null;
            GeneralFunction.ConstructWhereInLinq(strWhere, out filter);
            if (filter == "") filter = "true";

            // Get Data
            var q = _receiptVoucherDetailService.GetQueryable().Where(x => x.ReceiptVoucherId == id && !x.IsDeleted);

            var query = (from model in q
                         select new
                         {
                             model.Id,
                             model.Code,
                             Currency = model.Receivable.Currency.Name,
                             model.ReceivableId,
                             ReceivableCode = model.Receivable.Code,
                             model.AmountPaid,
                             model.Rate,
                             model.Amount,
                             model.PPH23,
                             model.Description,
                         }).Where(filter).OrderBy(sidx + " " + sord); //.ToList();

            var list = query.AsEnumerable();

            var pageIndex = Convert.ToInt32(page) - 1;
            var pageSize = rows;
            var totalRecords = query.Count();
            var totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);
            // default last page
            if (totalPages > 0)
            {
                if (!page.HasValue)
                {
                    pageIndex = totalPages - 1;
                    page = totalPages;
                }
            }

            list = list.Skip(pageIndex * pageSize).Take(pageSize);

            return Json(new
            {
                total = totalPages,
                page = page,
                records = totalRecords,
                rows = (
                    from model in list
                    select new
                    {
                        id = model.Id,
                        cell = new object[] {
                            model.Id,
                            model.Code,
                            model.Currency,
                            model.ReceivableId,
                            model.ReceivableCode,
                            model.AmountPaid,
                            model.Rate,
                            model.Amount,
                            model.PPH23,
                            model.Description,
                      }
                    }).ToArray()
            }, JsonRequestBehavior.AllowGet);
        }

      
        public dynamic GetInfo(int Id)
        {
            ReceiptVoucher model = new ReceiptVoucher();
            try
            {
                model = _receiptVoucherService.GetObjectById(Id);
            }
            catch (Exception ex)
            {
                LOG.Error("GetInfo", ex);
                model.Errors.Add("Generic", "Error : " + ex);
            }

            return Json(new
            {
                model.Id,
                model.Code,
                model.ContactId,
                Contact = model.Contact.Name,
                model.CashBankId,
                CashBank = model.CashBank.Name,
                Currency = model.CashBank.Currency.Name,
                model.ReceiptDate,
                model.RateToIDR,
                model.IsGBCH,
                model.DueDate,
                model.TotalAmount,
                model.NoBukti,
                model.TotalPPH23,
                model.BiayaBank,
                model.Pembulatan,
                model.StatusPembulatan,
                model.Errors
            }, JsonRequestBehavior.AllowGet);
        }

        public dynamic GetInfoDetail(int Id)
        {
            ReceiptVoucherDetail model = new ReceiptVoucherDetail();
            try
            {
                model = _receiptVoucherDetailService.GetObjectById(Id);
            }
            catch (Exception ex)
            {
                LOG.Error("GetInfo", ex);
                model.Errors.Add("Generic", "Error : " + ex);
            }

            return Json(new
            {
                model.Id,
                model.Code,
                model.ReceivableId,
                Receivable = _receivableService.GetObjectById(model.ReceivableId).Code,
                model.AmountPaid,
                model.Rate,
                model.Amount,
                Remaining = model.Receivable.RemainingAmount,
                Currency = model.Receivable.Currency.Name,
                model.Description,
                model.PPH23,
                model.Errors
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public dynamic Insert(ReceiptVoucher model)
        {
            try
            {
                model = _receiptVoucherService.CreateObject(model,_receiptVoucherDetailService,_receivableService
                    ,_contactService,_cashBankService, _currencyService);
            }
            catch (Exception ex)
            {
                LOG.Error("Insert Failed", ex);
                model.Errors.Add("Generic", "Error : " + ex);
            }

            return Json(new
            {
                model.Errors
            });
        }

        [HttpPost]
        public dynamic InsertDetail(ReceiptVoucherDetail model)
        {
            decimal totalamount = 0;
            try
            {
                model = _receiptVoucherDetailService.CreateObject(model,_receiptVoucherService,_cashBankService,_receivableService,_currencyService);
                totalamount = _receiptVoucherService.GetObjectById(model.ReceiptVoucherId).TotalAmount;
            }
            catch (Exception ex)
            {
                LOG.Error("Insert Failed", ex);
                model.Errors.Add("Generic", "Error : " + ex);
               
            }

            return Json(new
            {
                model.Errors,
                totalamount
            });
        }

        [HttpPost]
        public dynamic Update(ReceiptVoucher model)
        {
            try
            {
                var data = _receiptVoucherService.GetObjectById(model.Id);
                bool PembulatanOnly = true;
                if (data.ContactId != model.ContactId) PembulatanOnly = false;
                if (data.CashBankId != model.CashBankId) PembulatanOnly = false;
                if (data.ReceiptDate != model.ReceiptDate) PembulatanOnly = false;
                if (data.IsGBCH != model.IsGBCH) PembulatanOnly = false;
                if (data.DueDate != model.DueDate) PembulatanOnly = false;
                if (data.NoBukti != model.NoBukti) PembulatanOnly = false;
                if (data.RateToIDR != model.RateToIDR) PembulatanOnly = false;
                //if (data.BiayaBank != model.BiayaBank) PembulatanOnly = false;

                data.ContactId = model.ContactId;
                data.CashBankId = model.CashBankId;
                data.ReceiptDate = model.ReceiptDate;
                data.IsGBCH = model.IsGBCH;
                data.DueDate = model.DueDate;
                //data.TotalAmount = model.TotalAmount;
                data.NoBukti = model.NoBukti;
                data.RateToIDR = model.RateToIDR;
                data.BiayaBank = model.BiayaBank;
                data.Pembulatan = model.Pembulatan;
                data.StatusPembulatan = model.StatusPembulatan;
                if (PembulatanOnly && !data.IsConfirmed)
                {
                    model = _receiptVoucherService.CalculateTotalAmount(data, _receiptVoucherDetailService);
                }
                else
                {
                    model = _receiptVoucherService.UpdateObject(data, _receiptVoucherDetailService, _receivableService,
                        _contactService, _cashBankService);
                }
            }
            catch (Exception ex)
            {
                LOG.Error("Update Failed", ex);
                model.Errors.Add("Generic", "Error : " + ex);
            }

            return Json(new
            {
                model.Errors,
            });
        }

        [HttpPost]
        public dynamic Delete(ReceiptVoucher model)
        {
            try
            {
                var data = _receiptVoucherService.GetObjectById(model.Id);
                model = _receiptVoucherService.SoftDeleteObject(data,_receiptVoucherDetailService);
            }
            catch (Exception ex)
            {
                LOG.Error("Delete Failed", ex);
                model.Errors.Add("Generic", "Error : " + ex);
            }

            return Json(new
            {
                model.Errors
            });
        }

        [HttpPost]
        public dynamic DeleteDetail(ReceiptVoucherDetail model)
        {
            decimal totalamount = 0;
            try
            {
                var data = _receiptVoucherDetailService.GetObjectById(model.Id);
                model = _receiptVoucherDetailService.SoftDeleteObject(data,_receiptVoucherService);
                totalamount = _receiptVoucherService.GetObjectById(model.ReceiptVoucherId).TotalAmount;
            }
            catch (Exception ex)
            {
                LOG.Error("Delete Failed", ex);
                model.Errors.Add("Generic", "Error : " + ex);
            }

            return Json(new
            {
                model.Errors,
                totalamount
            });
        }

        [HttpPost]
        public dynamic UpdateDetail(ReceiptVoucherDetail model)
        {
            decimal totalamount = 0;
            try
            {
                var data = _receiptVoucherDetailService.GetObjectById(model.Id);
                data.ReceivableId = model.ReceivableId;
                data.Amount = model.Amount;
                data.AmountPaid = model.AmountPaid;
                data.Rate = model.Rate;
                data.Description = model.Description;
                data.PPH23 = model.PPH23;
                model = _receiptVoucherDetailService.UpdateObject(data,_receiptVoucherService,_cashBankService,_receivableService,_currencyService);
                totalamount = _receiptVoucherService.GetObjectById(model.ReceiptVoucherId).TotalAmount;
            }
            catch (Exception ex)
            {
                LOG.Error("Update Failed", ex);
                model.Errors.Add("Generic", "Error : " + ex);
            }

            return Json(new
            {
                model.Errors,
                totalamount
            });
        }


        [HttpPost]
        public dynamic Confirm(ReceiptVoucher model)
        {
            try
            {
                var data = _receiptVoucherService.GetObjectById(model.Id);
                model = _receiptVoucherService.ConfirmObject(data,model.ConfirmationDate.Value,
                    _receiptVoucherDetailService,_cashBankService,_receivableService,_cashMutationService,
                    _accountService, _generalLedgerJournalService, _closingService, _currencyService, _exchangeRateService, _salesInvoiceService,_gLNonBaseCurrencyService);
            }
            catch (Exception ex)
            {
                LOG.Error("Confirm Failed", ex);
                model.Errors.Add("Generic", "Error : " + ex);
            }

            return Json(new
            {
                model.Errors
            });
        }

        [HttpPost]
        public dynamic UnConfirm(ReceiptVoucher model)
        {
            try
            {

                var data = _receiptVoucherService.GetObjectById(model.Id);
                model = _receiptVoucherService.UnconfirmObject(data,_receiptVoucherDetailService,_cashBankService,
                    _receivableService,_cashMutationService,_accountService, _generalLedgerJournalService, _closingService,
                    _currencyService,_exchangeRateService,_gLNonBaseCurrencyService);
            }
            catch (Exception ex)
            {
                LOG.Error("Unconfirm Failed", ex);
                model.Errors.Add("Generic", "Error : " + ex);
            }

            return Json(new
            {
                model.Errors
            });
        }

        [HttpPost]
        public dynamic Reconcile(ReceiptVoucher model)
        {
            try
            {
                var data = _receiptVoucherService.GetObjectById(model.Id);
                model = _receiptVoucherService.ReconcileObject(data,model.ReconciliationDate.Value,_receiptVoucherDetailService,
                    _cashMutationService, _cashBankService, _receivableService,_accountService, _generalLedgerJournalService, _closingService,
                    _currencyService,_exchangeRateService,_salesInvoiceService,_gLNonBaseCurrencyService);
            }
            catch (Exception ex)
            {
                LOG.Error("Reconcile Failed", ex);
                model.Errors.Add("Generic", "Error : " + ex);
            }

            return Json(new
            {
                model.Errors
            });
        }

        [HttpPost]
        public dynamic UnReconcile(ReceiptVoucher model)
        {
            try
            {
                var data = _receiptVoucherService.GetObjectById(model.Id);
                model = _receiptVoucherService.UnreconcileObject(data,_receiptVoucherDetailService,_cashMutationService,
                                                                 _cashBankService, _receivableService, _accountService, _generalLedgerJournalService, _closingService,
                                                                 _currencyService,_exchangeRateService,_gLNonBaseCurrencyService);
            }
            catch (Exception ex)
            {
                LOG.Error("UnReconcile Failed", ex);
                model.Errors.Add("Generic", "Error : " + ex);
            }

            return Json(new
            {
                model.Errors
            });
        }
    }
}


