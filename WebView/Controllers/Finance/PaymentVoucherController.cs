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
    public class PaymentVoucherController : Controller
    {
        private readonly static log4net.ILog LOG = log4net.LogManager.GetLogger("PaymentVoucherController");
        private IPurchaseOrderService _purchaseOrderService;
        private IPurchaseOrderDetailService _purchaseOrderDetailService;
        private IPurchaseInvoiceService _purchaseInvoiceService;
        private IPurchaseInvoiceMigrationService _purchaseInvoiceMigrationService;
        private IPurchaseInvoiceDetailService _purchaseInvoiceDetailService;
        private IPurchaseReceivalService _purchaseReceivalService;
        private IPurchaseReceivalDetailService _purchaseReceivalDetailService;
        private IPaymentVoucherDetailService _paymentVoucherDetailService;
        private IPayableService _payableService;
        private IItemService _itemService;
        private IPaymentVoucherService _paymentVoucherService;
        private IPaymentRequestService _paymentRequestService;
        private ICashBankService _cashBankService;
        private ICashMutationService _cashMutationService;
        private IContactService _contactService;
        private IAccountService _accountService;
        private IGeneralLedgerJournalService _generalLedgerJournalService;
        private IClosingService _closingService;
        private ICurrencyService _currencyService;
        private IGLNonBaseCurrencyService _gLNonBaseCurrencyService;
        public PaymentVoucherController()
        {
            _accountService = new AccountService(new AccountRepository(), new AccountValidator());
            _generalLedgerJournalService = new GeneralLedgerJournalService(new GeneralLedgerJournalRepository(), new GeneralLedgerJournalValidator());
            _cashBankService = new CashBankService(new CashBankRepository(),new CashBankValidator());
            _cashMutationService = new CashMutationService(new CashMutationRepository(), new CashMutationValidator());
            _purchaseOrderService = new PurchaseOrderService(new PurchaseOrderRepository(), new PurchaseOrderValidator());
            _purchaseOrderDetailService = new PurchaseOrderDetailService(new PurchaseOrderDetailRepository(), new PurchaseOrderDetailValidator());
            _purchaseInvoiceService = new PurchaseInvoiceService(new PurchaseInvoiceRepository(), new PurchaseInvoiceValidator());
            _purchaseInvoiceMigrationService = new PurchaseInvoiceMigrationService(new PurchaseInvoiceMigrationRepository());
            _purchaseInvoiceDetailService = new PurchaseInvoiceDetailService(new PurchaseInvoiceDetailRepository(), new PurchaseInvoiceDetailValidator());
            _purchaseReceivalService = new PurchaseReceivalService(new PurchaseReceivalRepository(), new PurchaseReceivalValidator());
            _purchaseReceivalDetailService = new PurchaseReceivalDetailService(new PurchaseReceivalDetailRepository(), new PurchaseReceivalDetailValidator());
            _paymentVoucherDetailService = new PaymentVoucherDetailService(new PaymentVoucherDetailRepository(), new PaymentVoucherDetailValidator());
            _payableService = new PayableService(new PayableRepository(), new PayableValidator());
            _itemService = new ItemService(new ItemRepository(), new ItemValidator());
            _paymentVoucherService = new PaymentVoucherService(new PaymentVoucherRepository(), new PaymentVoucherValidator());
            _paymentRequestService = new PaymentRequestService(new PaymentRequestRepository(), new PaymentRequestValidator());
            _contactService = new ContactService(new ContactRepository(), new ContactValidator());
            _closingService = new ClosingService(new ClosingRepository(), new ClosingValidator());
            _currencyService = new CurrencyService(new CurrencyRepository(), new CurrencyValidator());
            _gLNonBaseCurrencyService = new GLNonBaseCurrencyService(new GLNonBaseCurrencyRepository(), new GLNonBaseCurrencyValidator());
        }


        public ActionResult Index()
        {
            return View();
        }

        public dynamic GetList(string _search, long nd, int rows, int? page, string sidx, string sord, string filters = "")
        {
            // Construct where statement
            string strWhere = GeneralFunction.ConstructWhere(filters);
            string filter = null;
            GeneralFunction.ConstructWhereInLinq(strWhere, out filter);
            if (filter == "") filter = "true";

            // Get Data
            var q = _paymentVoucherService.GetQueryable().Include("CashBank").Include("Contact").Where(x => !x.IsDeleted);

            var query = (from model in q
                         select new
                         {
                             model.Id,
                             model.Code,
                             model.ContactId,
                             Contact = model.Contact.Name,
                             model.CashBankId,
                             CashBank = model.CashBank.Name,
                             model.PaymentDate,
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
                             model.TotalPPH21,
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
                            model.PaymentDate,
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
                            model.TotalPPH21,
                            model.BiayaBank,
                            model.Pembulatan,
                            model.CreatedAt,
                            model.UpdatedAt,
                      }
                    }).ToArray()
            }, JsonRequestBehavior.AllowGet);
        }

        public dynamic GetListPayable(string _search, long nd, int rows, int? page, string sidx, string sord, string filters = "")
        {
            // Construct where statement
            string strWhere = GeneralFunction.ConstructWhere(filters);
            string filter = null;
            GeneralFunction.ConstructWhereInLinq(strWhere, out filter);
            if (filter == "") filter = "true";

            // Get Data
            var q = _payableService.GetQueryable().Include("Contact").Where(x => !x.IsDeleted && x.RemainingAmount > 0);

            var query = (from model in q
                         select new
                         {
                             model.Id,
                             model.Code,
                             model.ContactId,
                             Contact = model.Contact.Name,
                             model.PayableSource,
                             model.PayableSourceId,
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
                            model.PayableSource,
                            model.PayableSourceId,
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

        public dynamic GetListPayableNonDP(string _search, long nd, int rows, int? page, string sidx, string sord, int contactid, string filters = "")
        {
            // Construct where statement
            string strWhere = GeneralFunction.ConstructWhere(filters);
            string filter = null;
            GeneralFunction.ConstructWhereInLinq(strWhere, out filter);
            if (filter == "") filter = "true";

            // Get Data
            var q = _payableService.GetQueryable().Include("Contact").Where(x => !x.IsDeleted && x.ContactId == contactid && x.RemainingAmount > 0 &&
                                                   x.PayableSource != Constant.PayableSource.SalesDownPayment).ToList();

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
                             model.PayableSource,
                             model.PayableSourceId,
                             NomorSurat = (model.PayableSource == Constant.PayableSource.PurchaseInvoice) ? _purchaseInvoiceService.GetObjectById(model.PayableSourceId).NomorSurat :
                                                (model.PayableSource == Constant.PayableSource.PurchaseInvoiceMigration) ? _purchaseInvoiceMigrationService.GetObjectById(model.PayableSourceId).NomorSurat :
                                                (model.PayableSource == Constant.PayableSource.PaymentRequest) ? _paymentRequestService.GetObjectById(model.PayableSourceId).NoBukti : "",
                             model.CreatedAt,
                             model.UpdatedAt,
                         }).AsQueryable().Where(filter).OrderBy(sidx + " " + sord); //.ToList();

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
                            model.PayableSource,
                            model.PayableSourceId,
                            model.NomorSurat,
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
            var q = _paymentVoucherDetailService.GetQueryable().Include("PaymentVoucher").Include("Payable")
                                                .Where(x => x.PaymentVoucherId == id && !x.IsDeleted).ToList();

            var query = (from model in q
                         select new
                         {
                            model.Id,
                            model.Code,
                            Currency = model.Payable.Currency.Name,
                            model.PayableId,
                            PayableCode = model.Payable.Code,
                            model.AmountPaid,
                            model.Rate,
                            model.Amount,
                            model.PPH23,
                            model.PPH21,
                            NomorSurat = (model.Payable.PayableSource == Constant.PayableSource.PurchaseInvoice) ? _purchaseInvoiceService.GetObjectById(model.Payable.PayableSourceId).NomorSurat :
                                                (model.Payable.PayableSource == Constant.PayableSource.PurchaseInvoiceMigration) ? _purchaseInvoiceMigrationService.GetObjectById(model.Payable.PayableSourceId).NomorSurat :
                                                (model.Payable.PayableSource == Constant.PayableSource.PaymentRequest) ? _paymentRequestService.GetObjectById(model.Payable.PayableSourceId).NoBukti : "",
                            model.Description,
                         }).AsQueryable().Where(filter).OrderBy(sidx + " " + sord); //.ToList();

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
                            model.PayableId,
                            model.PayableCode,
                            model.AmountPaid,
                            model.Rate,
                            model.Amount,
                            model.PPH23,
                            model.PPH21,
                            model.NomorSurat,
                            model.Description,
                      }
                    }).ToArray()
            }, JsonRequestBehavior.AllowGet);
        }

      
        public dynamic GetInfo(int Id)
        {
            PaymentVoucher model = new PaymentVoucher();
            try
            {
                model = _paymentVoucherService.GetObjectById(Id);
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
                Contact = _contactService.GetObjectById(model.ContactId).Name,
                model.CashBankId,
                CashBank = _cashBankService.GetObjectById(model.CashBankId).Name,
                Currency = model.CashBank.Currency.Name,
                model.PaymentDate,
                model.IsGBCH,
                model.DueDate,
                model.TotalAmount,
                model.RateToIDR,
                model.NoBukti,
                model.TotalPPH23,
                model.TotalPPH21,
                model.BiayaBank,
                model.Pembulatan,
                model.StatusPembulatan,
                model.Errors
            }, JsonRequestBehavior.AllowGet);
        }

        public dynamic GetInfoDetail(int Id)
        {
            PaymentVoucherDetail model = new PaymentVoucherDetail();
            try
            {
                model = _paymentVoucherDetailService.GetObjectById(Id);
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
                model.PayableId,
                Payable =_payableService.GetObjectById(model.PayableId).Code,
                model.AmountPaid,
                model.Rate,
                model.Amount,
                Remaining = model.Payable.RemainingAmount,
                Currency = model.Payable.Currency.Name,
                model.Description,
                model.PPH23,
                model.PPH21,
                model.Errors
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public dynamic Insert(PaymentVoucher model)
        {
            try
            {
             
                model = _paymentVoucherService.CreateObject(model,_paymentVoucherDetailService,_payableService
                    , _contactService, _cashBankService, _currencyService);
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
        public dynamic InsertDetail(PaymentVoucherDetail model)
        {
            decimal totalamount = 0;
            decimal totalpph23 = 0;
            decimal totalpph21 = 0;
            try
            {
                model = _paymentVoucherDetailService.CreateObject(model,_paymentVoucherService,_cashBankService,_payableService);
                var obj = _paymentVoucherService.GetObjectById(model.PaymentVoucherId);
                totalamount = obj.TotalAmount;
                totalpph23 = obj.TotalPPH23;
                totalpph21 = obj.TotalPPH21;
            }
            catch (Exception ex)
            {
                LOG.Error("Insert Failed", ex);
                model.Errors.Add("Generic", "Error : " + ex);
               
            }

            return Json(new
            {
                model.Errors,
                totalamount,
                totalpph23,
                totalpph21,
            });
        }

        [HttpPost]
        public dynamic Update(PaymentVoucher model)
        {
            try
            {
                var data = _paymentVoucherService.GetObjectById(model.Id);
                bool PembulatanOnly = true;
                if (data.ContactId != model.ContactId) PembulatanOnly = false;
                if (data.CashBankId != model.CashBankId) PembulatanOnly = false;
                if (data.PaymentDate != model.PaymentDate) PembulatanOnly = false;
                if (data.IsGBCH != model.IsGBCH) PembulatanOnly = false;
                if (data.DueDate != model.DueDate) PembulatanOnly = false;
                if (data.NoBukti != model.NoBukti) PembulatanOnly = false;
                if (data.RateToIDR != model.RateToIDR) PembulatanOnly = false;
                //if (data.BiayaBank != model.BiayaBank) PembulatanOnly = false;

                data.ContactId = model.ContactId;
                data.CashBankId = model.CashBankId;
                data.PaymentDate = model.PaymentDate;
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
                    model = _paymentVoucherService.CalculateTotalAmount(data, _paymentVoucherDetailService);
                }
                else
                {
                    model = _paymentVoucherService.UpdateObject(data, _paymentVoucherDetailService, _payableService,
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
        public dynamic Delete(PaymentVoucher model)
        {
            try
            {
                var data = _paymentVoucherService.GetObjectById(model.Id);
                model = _paymentVoucherService.SoftDeleteObject(data,_paymentVoucherDetailService);
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
        public dynamic DeleteDetail(PaymentVoucherDetail model)
        {
            decimal totalamount = 0;
            decimal totalpph23 = 0;
            decimal totalpph21 = 0;
            try
            {
                var data = _paymentVoucherDetailService.GetObjectById(model.Id);
                model = _paymentVoucherDetailService.SoftDeleteObject(data,_paymentVoucherService);
                var obj = _paymentVoucherService.GetObjectById(model.PaymentVoucherId);
                totalamount = obj.TotalAmount;
                totalpph23 = obj.TotalPPH23;
                totalpph21 = obj.TotalPPH21;
            }
            catch (Exception ex)
            {
                LOG.Error("Delete Failed", ex);
                model.Errors.Add("Generic", "Error : " + ex);
            }

            return Json(new
            {
                model.Errors,
                totalamount,
                totalpph23,
                totalpph21,
            });
        }

        [HttpPost]
        public dynamic UpdateDetail(PaymentVoucherDetail model)
        {
            decimal totalamount = 0;
            decimal totalpph23 = 0;
            decimal totalpph21 = 0;
            try
            {
                var data = _paymentVoucherDetailService.GetObjectById(model.Id);
                data.PayableId = model.PayableId;
                data.Amount = model.Amount;
                data.AmountPaid = model.AmountPaid;
                data.Rate = model.Rate;
                data.Description = model.Description;
                data.PPH23 = model.PPH23;
                data.PPH21 = model.PPH21;
                model = _paymentVoucherDetailService.UpdateObject(data,_paymentVoucherService,_cashBankService,_payableService);
                var obj = _paymentVoucherService.GetObjectById(model.PaymentVoucherId);
                totalamount = obj.TotalAmount;
                totalpph23 = obj.TotalPPH23;
                totalpph21 = obj.TotalPPH21;
            }
            catch (Exception ex)
            {
                LOG.Error("Update Failed", ex);
                model.Errors.Add("Generic", "Error : " + ex);
            }

            return Json(new
            {
                model.Errors,
                totalamount,
                totalpph23,
                totalpph21,
            });
        }


        [HttpPost]
        public dynamic Confirm(PaymentVoucher model)
        {
            try
            {
                var data = _paymentVoucherService.GetObjectById(model.Id);
                model = _paymentVoucherService.ConfirmObject(data,model.ConfirmationDate.Value,
                    _paymentVoucherDetailService,_cashBankService,_payableService,_cashMutationService,
                    _accountService,_generalLedgerJournalService, _closingService,_currencyService,_gLNonBaseCurrencyService);
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
        public dynamic UnConfirm(PaymentVoucher model)
        {
            try
            {

                var data = _paymentVoucherService.GetObjectById(model.Id);
                model = _paymentVoucherService.UnconfirmObject(data,_paymentVoucherDetailService,_cashBankService,
                    _payableService,_cashMutationService,_accountService, _generalLedgerJournalService, _closingService,_currencyService,
                    _gLNonBaseCurrencyService);
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
        public dynamic Reconcile(PaymentVoucher model)
        {
            try
            {
                var data = _paymentVoucherService.GetObjectById(model.Id);
                model = _paymentVoucherService.ReconcileObject(data,model.ReconciliationDate.Value,_paymentVoucherDetailService,
                    _cashMutationService,_cashBankService,_payableService,_accountService, _generalLedgerJournalService, _closingService,_currencyService,_gLNonBaseCurrencyService);
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
        public dynamic UnReconcile(PaymentVoucher model)
        {
            try
            {
                var data = _paymentVoucherService.GetObjectById(model.Id);
                model = _paymentVoucherService.UnreconcileObject(data,_paymentVoucherDetailService,_cashMutationService,
                    _cashBankService,_payableService, _accountService, _generalLedgerJournalService, _closingService,_currencyService,_gLNonBaseCurrencyService
                    );
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


