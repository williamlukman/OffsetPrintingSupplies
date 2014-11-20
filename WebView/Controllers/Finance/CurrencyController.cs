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
    public class CurrencyController : Controller
    {
        private readonly static log4net.ILog LOG = log4net.LogManager.GetLogger("CurrencyController");
        private ICashMutationService _cashMutationService;
        private IAccountService _accountService;
        public ICurrencyService _currencyService;
        private ICashBankService _cashBankService;
        private IPaymentRequestService _paymentRequestService;
        private IPurchaseOrderService  _purchaseOrderService;
        private ISalesOrderService _salesOrderService;
        private ISalesInvoiceService _salesInvoiceService;
        private IPurchaseInvoiceService _purchaseInvoiceService;
        private IPayableService  _payableService;
        private IReceivableService _receivableService;
        private IPaymentVoucherService _paymentVoucherService;
        private IReceiptVoucherService _receiptVoucherService;
        public CurrencyController()
        {
            _cashMutationService = new CashMutationService(new CashMutationRepository(), new CashMutationValidator());
            _accountService = new AccountService(new AccountRepository(), new AccountValidator());
            _currencyService = new CurrencyService(new CurrencyRepository(), new CurrencyValidator());
            _cashBankService = new CashBankService(new CashBankRepository(), new CashBankValidator());
            _paymentRequestService = new PaymentRequestService(new PaymentRequestRepository(), new PaymentRequestValidator());
            _purchaseOrderService = new PurchaseOrderService(new PurchaseOrderRepository(), new PurchaseOrderValidator());
            _salesOrderService = new SalesOrderService(new SalesOrderRepository(), new SalesOrderValidator());
            _salesInvoiceService = new SalesInvoiceService(new SalesInvoiceRepository(), new SalesInvoiceValidator());
            _purchaseInvoiceService = new PurchaseInvoiceService(new PurchaseInvoiceRepository(), new PurchaseInvoiceValidator());
            _payableService = new PayableService(new PayableRepository(), new PayableValidator());
            _receivableService = new ReceivableService(new ReceivableRepository(), new ReceivableValidator());
            _paymentVoucherService = new PaymentVoucherService(new PaymentVoucherRepository(), new PaymentVoucherValidator());
            _receiptVoucherService = new ReceiptVoucherService(new ReceiptVoucherRepository(), new ReceiptVoucherValidator());
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
            var q = _currencyService.GetQueryable().Where(x => !x.IsDeleted);

            var query = (from model in q
                         select new
                         {
                             model.Id,
                             model.Name,
                             model.Description,
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
                    from item in list
                    select new
                    {
                        id = item.Id,
                        cell = new object[] {
                            item.Id,
                            item.Name,
                            item.Description,
                            item.CreatedAt,
                            item.UpdatedAt,
                      }
                    }).ToArray()
            }, JsonRequestBehavior.AllowGet);
        }

        public dynamic GetInfo(int Id)
        {
            Currency model = new Currency();
            try
            {
                model = _currencyService.GetObjectById(Id);

            }
            catch (Exception ex)
            {
                LOG.Error("GetInfo", ex);
                model.Errors.Add("Generic", "Error" + ex);
            }

            return Json(new
            {
                model.Id,
                model.Name,
                model.Description,
                model.Errors
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public dynamic Insert(Currency model)
        {
            try
            {
                model = _currencyService.CreateObject(model);
            }
            catch (Exception ex)
            {
                LOG.Error("Insert Failed", ex);
                model.Errors.Add("Insert Failed", "Error : " + ex);
            }

            return Json(new
            {
                model.Errors
            });
        }

        [HttpPost]
        public dynamic Update(Currency model)
        {
            try
            {
                var data = _currencyService.GetObjectById(model.Id);
                data.Name = model.Name;
                data.Description = model.Description;
                model = _currencyService.UpdateObject(data,_cashBankService,_paymentRequestService,_purchaseOrderService,
                    _salesOrderService,_salesInvoiceService,_purchaseInvoiceService,_payableService,_receivableService,
                    _paymentVoucherService,_receiptVoucherService);
            }
            catch (Exception ex)
            {
                LOG.Error("Update Failed", ex);
                model.Errors.Add("Update Failed", "Error : " + ex);
            }

            return Json(new
            {
                model.Errors
            });
        }

        [HttpPost]
        public dynamic Delete(Currency model)
        {
            try
            {
                var data = _currencyService.GetObjectById(model.Id);
                model = _currencyService.SoftDeleteObject(data,_cashBankService, _paymentRequestService, _purchaseOrderService,
                    _salesOrderService, _salesInvoiceService, _purchaseInvoiceService, _payableService, _receivableService,
                    _paymentVoucherService, _receiptVoucherService);
            }
            catch (Exception ex)
            {
                LOG.Error("Delete Failed", ex);
                model.Errors.Add("Delete Failed", "Error : " + ex);
            }

            return Json(new
            {
                model.Errors
            });
        }
    }
}
