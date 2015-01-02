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
using System.Data.Objects;
using Core.Constants;

namespace WebView.Controllers
{
    public class GeneralLedgerController : Controller
    {
        private readonly static log4net.ILog LOG = log4net.LogManager.GetLogger("GeneralLedgerController");

        private IGeneralLedgerJournalService _generalLedgerJournalService;
        private IAccountService _accountService;
        private ISalesOrderService _salesOrderService;
        private ICashBankAdjustmentService _cashBankAdjustmentService;
        private ICashBankMutationService _cashBankMutationService;
        private IClosingService _closingService;
        private IMemorialService _memorialService;
        private IPaymentRequestService _paymentRequestService;
        private IPaymentVoucherService _paymentVoucherService;
        private IPurchaseDownPaymentService _purchaseDownPaymentService;
        private IPurchaseDownPaymentAllocationService _purchaseDownPaymentAllocationService;
        private IPurchaseAllowanceService _purchaseAllowanceService;
        private IPurchaseReceivalService _purchaseReceivalService;
        private IPurchaseInvoiceService _purchaseInvoiceService;
        private IPurchaseInvoiceMigrationService _purchaseInvoiceMigrationService;
        private IReceiptVoucherService _receiptVoucherService;
        private IDeliveryOrderService _deliveryOrderService;
        private ISalesDownPaymentService _salesDownPaymentService;
        private ISalesDownPaymentAllocationService _salesDownPaymentAllocationService;
        private ISalesAllowanceService _salesAllowanceService;
        private ISalesInvoiceService _salesInvoiceService;
        private ISalesInvoiceMigrationService _salesInvoiceMigrationService;
        private IRecoveryOrderDetailService _recoveryOrderDetailService;
        private IRecoveryAccessoryDetailService _recoveryAccessoryDetailService;
        private IBlanketOrderDetailService _blanketOrderDetailService;
        private IBlendingWorkOrderService _blendingWorkOrderService;
     
        public GeneralLedgerController()
        {
            _accountService = new AccountService(new AccountRepository(), new AccountValidator());
            _generalLedgerJournalService = new GeneralLedgerJournalService(new GeneralLedgerJournalRepository(), new GeneralLedgerJournalValidator());
            _cashBankAdjustmentService = new CashBankAdjustmentService(new CashBankAdjustmentRepository(), new CashBankAdjustmentValidator());
            _cashBankMutationService = new CashBankMutationService(new CashBankMutationRepository(), new CashBankMutationValidator());
            _closingService = new ClosingService(new ClosingRepository(), new ClosingValidator());
            _memorialService = new MemorialService(new MemorialRepository(), new MemorialValidator());
            _paymentRequestService = new PaymentRequestService(new PaymentRequestRepository(), new PaymentRequestValidator());
            _paymentVoucherService = new PaymentVoucherService(new PaymentVoucherRepository(), new PaymentVoucherValidator());
            _purchaseDownPaymentService = new PurchaseDownPaymentService(new PurchaseDownPaymentRepository(), new PurchaseDownPaymentValidator());
            _purchaseDownPaymentAllocationService = new PurchaseDownPaymentAllocationService(new PurchaseDownPaymentAllocationRepository(), new PurchaseDownPaymentAllocationValidator());
            _purchaseAllowanceService = new PurchaseAllowanceService(new PurchaseAllowanceRepository(), new PurchaseAllowanceValidator());

        }

        public ActionResult Index()
        {
            if (!AuthenticationModel.IsAllowed("View", Core.Constants.Constant.MenuName.GeneralLedger, Core.Constants.Constant.MenuGroupName.Report))
            {
                return Content("You are not allowed to View this Page.");
            }

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
            var q = _generalLedgerJournalService.GetQueryable().Include("Account");

            var query = (from model in q
                         select new
                         {
                            model.Id,
                            model.TransactionDate,
                            model.Status,
                            AccountCode = model.Account.Code,
                            Account = model.Account.Name,
                            model.Amount,
                            model.SourceDocument,
                            model.SourceDocumentId
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
                            model.TransactionDate,
                            model.Status,
                            model.AccountCode,
                            model.Account,
                            model.Amount,
                            model.SourceDocument,
                            model.SourceDocumentId
                      }
                    }).ToArray()
            }, JsonRequestBehavior.AllowGet);
        }

        public dynamic GetListByDate(string _search, long nd, int rows, int? page, string sidx, string sord, DateTime? startdate, DateTime? enddate, string filters = "")
        {
            // Construct where statement
            string strWhere = GeneralFunction.ConstructWhere(filters);
            string filter = null;
            GeneralFunction.ConstructWhereInLinq(strWhere, out filter);
            if (filter == "") filter = "true";

            if (startdate.HasValue && enddate.HasValue)
            {
                filter = "(" + filter + ") AND TransactionDate >= @0 AND TransactionDate < @1";
            }

            // Get Data
            var q = _generalLedgerJournalService.GetQueryable().Include("Account");

            var query = (from model in q
                         select new
                         {
                             model.Id,
                             model.TransactionDate,
                             model.Status,
                             AccountCode = model.Account.Code,
                             Account = model.Account.Name,
                             model.Amount,
                             model.SourceDocument,
                             model.SourceDocumentId
                         }).Where(filter, startdate.GetValueOrDefault().Date, enddate.GetValueOrDefault().AddDays(1).Date).OrderBy(sidx + " " + sord); //.ToList();

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
                            model.TransactionDate,
                            model.Status,
                            model.AccountCode,
                            model.Account,
                            model.Amount,
                            model.SourceDocument,
                            model.SourceDocumentId
                         }
                    }).ToArray()
            }, JsonRequestBehavior.AllowGet);
        }

        public dynamic GetListByAccountAndDate(string _search, long nd, int rows, int? page, string sidx, string sord, int? accountid, DateTime? startdate, DateTime? enddate, string filters = "")
        {
            // Construct where statement
            string strWhere = GeneralFunction.ConstructWhere(filters);
            string filter = null;
            GeneralFunction.ConstructWhereInLinq(strWhere, out filter);
            if (filter == "") filter = "true";

            if (accountid.HasValue && startdate.HasValue && enddate.HasValue)
            {
                filter = "(" + filter + ") AND AccountId = @0 AND TransactionDate >= @1 AND TransactionDate < @2";
            }

            // Get Data
            var q = _generalLedgerJournalService.GetQueryable().Include("Account");

            var query = (from model in q
                         select new
                         {
                             model.Id,
                             model.TransactionDate,
                             model.Status,
                             AccountCode = model.Account.Code,
                             Account = model.Account.Name,
                             model.Amount,
                             model.SourceDocument,
                             model.SourceDocumentId,
                             model.AccountId
                         }).Where(filter, accountid.GetValueOrDefault(), startdate.GetValueOrDefault().Date, enddate.GetValueOrDefault().AddDays(1).Date).OrderBy(sidx + " " + sord); //.ToList();

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
                            model.TransactionDate,
                            model.Status,
                            model.AccountCode,
                            model.Account,
                            model.Amount,
                            model.SourceDocument,
                            model.SourceDocumentId,
                            model.AccountId
                         }
                    }).ToArray()
            }, JsonRequestBehavior.AllowGet);
        }
    }
}