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

namespace WebView.Controllers
{
    public class PurchaseInvoiceController : Controller
    {
        private readonly static log4net.ILog LOG = log4net.LogManager.GetLogger("PurchaseInvoiceController");
        private IPurchaseOrderService _purchaseOrderService;
        private IPurchaseOrderDetailService _purchaseOrderDetailService;
        private IPurchaseInvoiceService _purchaseInvoiceService;
        private IPurchaseInvoiceDetailService _purchaseInvoiceDetailService;
        private IPurchaseReceivalService _purchaseReceivalService;
        private IPurchaseReceivalDetailService _purchaseReceivalDetailService;
        private IPaymentVoucherDetailService _paymentVoucherDetailService;
        private IPayableService _payableService;
        private IItemService _itemService;
        private IAccountService _accountService;
        private IGeneralLedgerJournalService _generalLedgerJournalService;
        private IClosingService _closingService;
        private IExchangeRateService _exchangeRateService;
        private ICurrencyService _currencyService;
        private IGLNonBaseCurrencyService _gLNonBaseCurrencyService;

        public PurchaseInvoiceController()
        {
            _purchaseOrderService = new PurchaseOrderService(new PurchaseOrderRepository(), new PurchaseOrderValidator());
            _purchaseOrderDetailService = new PurchaseOrderDetailService(new PurchaseOrderDetailRepository(), new PurchaseOrderDetailValidator());
            _purchaseInvoiceService = new PurchaseInvoiceService(new PurchaseInvoiceRepository(), new PurchaseInvoiceValidator());
            _purchaseInvoiceDetailService = new PurchaseInvoiceDetailService(new PurchaseInvoiceDetailRepository(), new PurchaseInvoiceDetailValidator());
            _purchaseReceivalService = new PurchaseReceivalService(new PurchaseReceivalRepository(), new PurchaseReceivalValidator());
            _purchaseReceivalDetailService = new PurchaseReceivalDetailService(new PurchaseReceivalDetailRepository(), new PurchaseReceivalDetailValidator());
            _paymentVoucherDetailService = new PaymentVoucherDetailService(new PaymentVoucherDetailRepository(), new PaymentVoucherDetailValidator());
            _payableService = new PayableService(new PayableRepository(), new PayableValidator());
            _itemService = new ItemService(new ItemRepository(), new ItemValidator());
            _accountService = new AccountService(new AccountRepository(), new AccountValidator());
            _generalLedgerJournalService = new GeneralLedgerJournalService(new GeneralLedgerJournalRepository(), new GeneralLedgerJournalValidator());
            _closingService = new ClosingService(new ClosingRepository(), new ClosingValidator());
            _exchangeRateService = new ExchangeRateService(new ExchangeRateRepository(), new ExchangeRateValidator());
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
            var q = _purchaseInvoiceService.GetQueryable().Include("PurchaseReceival").Include("PurchaseOrder").Include("Contact").Where(x => !x.IsDeleted);

            var query = (from model in q
                         select new
                         {
                             model.Id,
                             model.Code,
                             Contact = model.PurchaseReceival.PurchaseOrder.Contact.Name,
                             model.NomorSurat,
                             model.PurchaseReceivalId,
                             PurchaseReceivalCode = model.PurchaseReceival.Code,
                             model.Description,
                             model.Discount,
                             model.Tax,
                             model.InvoiceDate,
                             model.DueDate,
                             model.AmountPayable,
                             model.IsConfirmed,
                             model.ConfirmationDate,
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
                            model.Contact,
                            model.NomorSurat,
                            model.PurchaseReceivalId,
                            model.PurchaseReceivalCode,
                            model.Description,
                            model.Discount,
                            model.Tax,
                            model.InvoiceDate,
                            model.DueDate,
                            model.AmountPayable,
                            model.IsConfirmed,
                            model.ConfirmationDate,
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
            var q = _purchaseInvoiceDetailService.GetQueryable().Include("PurchaseRceivalDetail").Include("Item")
                                                 .Where(x => x.PurchaseInvoiceId == id && !x.IsDeleted);

            var query = (from model in q
                         select new
                         {
                             model.Id,
                             model.Code,
                             model.PurchaseReceivalDetailId,
                             PurchaseReceivalDetailCode = model.PurchaseReceivalDetail.Code,
                             ItemId = model.PurchaseReceivalDetail.ItemId,
                             ItemSku = model.PurchaseReceivalDetail.Item.Sku,
                             Item = model.PurchaseReceivalDetail.Item.Name,
                             model.Quantity,
                             model.Amount,
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
                            model.PurchaseReceivalDetailId,
                            model.PurchaseReceivalDetailCode,
                            model.ItemId,
                            model.ItemSku,
                            model.Item,
                            model.Quantity,
                            model.Amount,
                      }
                    }).ToArray()
            }, JsonRequestBehavior.AllowGet);
        }
      
        public dynamic GetInfo(int Id)
        {
            PurchaseInvoice model = new PurchaseInvoice();
            try
            {
                model = _purchaseInvoiceService.GetObjectById(Id);
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
                model.NomorSurat,
                model.PurchaseReceivalId,
                PurchaseReceival = model.PurchaseReceival.Code,
                model.Discount,
                model.Description,
                model.Tax,
                model.InvoiceDate,
                model.DueDate,
                model.AmountPayable,
                ConfirmationDate = model.ConfirmationDate,
                currency = model.Currency.Name,
                model.CurrencyId,
                model.ExchangeRateAmount,
                model.Errors
            }, JsonRequestBehavior.AllowGet);
        }

        public dynamic GetInfoDetail(int Id)
        {
            PurchaseInvoiceDetail model = new PurchaseInvoiceDetail();
            try
            {
                model = _purchaseInvoiceDetailService.GetObjectById(Id);
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
                model.PurchaseReceivalDetailId,
                PurchaseReceival = _purchaseReceivalDetailService.GetObjectById(model.PurchaseReceivalDetailId).Code,
                ItemId = _purchaseReceivalDetailService.GetObjectById(model.PurchaseReceivalDetailId).ItemId,
                Item = _itemService.GetObjectById(_purchaseReceivalDetailService.GetObjectById(model.PurchaseReceivalDetailId).ItemId).Name,
                model.Quantity,
                model.Amount,
                model.Errors
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public dynamic Insert(PurchaseInvoice model)
        {
            try
            {
             
                model = _purchaseInvoiceService.CreateObject(model,_purchaseReceivalService);
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
        public dynamic InsertDetail(PurchaseInvoiceDetail model)
        {
            decimal amount = 0;
            try
            {
                model = _purchaseInvoiceDetailService.CreateObject(model,_purchaseInvoiceService,_purchaseOrderDetailService,_purchaseReceivalDetailService);
                amount = _purchaseInvoiceService.GetObjectById(model.PurchaseInvoiceId).AmountPayable;
            }
            catch (Exception ex)
            {
                LOG.Error("Insert Failed", ex);
                model.Errors.Add("Generic", "Error : " + ex);
               
            }

            return Json(new
            {
                model.Errors,
                AmountPayable = amount
            });
        }

        [HttpPost]
        public dynamic Update(PurchaseInvoice model)
        {
            try
            {
                var data = _purchaseInvoiceService.GetObjectById(model.Id);
                data.PurchaseReceivalId = model.PurchaseReceivalId;
                data.Description = model.Description;
                data.Discount = model.Discount;
                data.Tax = model.Tax;
                data.InvoiceDate = model.InvoiceDate;
                data.DueDate = model.DueDate;
                data.NomorSurat = model.NomorSurat;
                model = _purchaseInvoiceService.UpdateObject(data,_purchaseReceivalService,_purchaseInvoiceDetailService);
            }
            catch (Exception ex)
            {
                LOG.Error("Update Failed", ex);
                model.Errors.Add("Generic", "Error : " + ex);
            }

            return Json(new
            {
                model.Errors,
                model.AmountPayable
            });
        }

        [HttpPost]
        public dynamic Delete(PurchaseInvoice model)
        {
            try
            {
                var data = _purchaseInvoiceService.GetObjectById(model.Id);
                model = _purchaseInvoiceService.SoftDeleteObject(data,_purchaseInvoiceDetailService);
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
        public dynamic DeleteDetail(PurchaseInvoiceDetail model)
        {
            decimal amount = 0;
            try
            {
                var data = _purchaseInvoiceDetailService.GetObjectById(model.Id);
                model = _purchaseInvoiceDetailService.SoftDeleteObject(data,_purchaseInvoiceService);
                amount = _purchaseInvoiceService.GetObjectById(model.PurchaseInvoiceId).AmountPayable;
            }
            catch (Exception ex)
            {
                LOG.Error("Delete Failed", ex);
                model.Errors.Add("Generic", "Error : " + ex);
            }

            return Json(new
            {
                model.Errors,
                AmountPayable = amount
            });
        }

        [HttpPost]
        public dynamic UpdateDetail(PurchaseInvoiceDetail model)
        {
            decimal amount = 0;
            try
            {
                var data = _purchaseInvoiceDetailService.GetObjectById(model.Id);
                data.PurchaseReceivalDetailId = model.PurchaseReceivalDetailId;
                data.Quantity = model.Quantity;
                model = _purchaseInvoiceDetailService.UpdateObject(data,_purchaseInvoiceService,_purchaseOrderDetailService,_purchaseReceivalDetailService);
                amount = _purchaseInvoiceService.GetObjectById(model.PurchaseInvoiceId).AmountPayable;
            }
            catch (Exception ex)
            {
                LOG.Error("Update Failed", ex);
                model.Errors.Add("Generic", "Error : " + ex);
            }

            return Json(new
            {
                model.Errors,
                AmountPayable = amount
            });
        }


        [HttpPost]
        public dynamic Confirm(PurchaseInvoice model)
        {
            try
            {
                var data = _purchaseInvoiceService.GetObjectById(model.Id);
                model = _purchaseInvoiceService.ConfirmObject(data, model.ConfirmationDate.Value, _purchaseInvoiceDetailService, _purchaseOrderService,
                        _purchaseReceivalService, _purchaseReceivalDetailService, _payableService, _accountService, _generalLedgerJournalService, 
                        _closingService,_currencyService,_exchangeRateService,_gLNonBaseCurrencyService);
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
        public dynamic UnConfirm(PurchaseInvoice model)
        {
            try
            {

                var data = _purchaseInvoiceService.GetObjectById(model.Id);
                model = _purchaseInvoiceService.UnconfirmObject(data,_purchaseInvoiceDetailService,
                        _purchaseReceivalService,_purchaseReceivalDetailService,_paymentVoucherDetailService,_payableService,
                        _accountService,_generalLedgerJournalService,_closingService,_gLNonBaseCurrencyService, _currencyService);
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
    }
}

