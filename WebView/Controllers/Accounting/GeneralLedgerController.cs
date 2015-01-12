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
        private IBlanketOrderService _blanketOrderService;
        private IBlanketOrderDetailService _blanketOrderDetailService;
        private IBlendingWorkOrderService _blendingWorkOrderService;
        private IValidCombService _validCombService;

        private IRepackingService _repackingService;
        private IStockAdjustmentService _stockAdjustmentService;
        private ICustomerStockAdjustmentService _customerStockAdjustmentService;
        private IReceiptRequestService _receiptRequestService;

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
            _validCombService = new ValidCombService(new ValidCombRepository(), new ValidCombValidator());
            _purchaseReceivalService = new PurchaseReceivalService(new PurchaseReceivalRepository(), new PurchaseReceivalValidator());
            _purchaseInvoiceService = new PurchaseInvoiceService(new PurchaseInvoiceRepository(), new PurchaseInvoiceValidator());
            _purchaseInvoiceMigrationService = new PurchaseInvoiceMigrationService(new PurchaseInvoiceMigrationRepository());
            _receiptVoucherService = new ReceiptVoucherService(new ReceiptVoucherRepository(), new ReceiptVoucherValidator());
            _deliveryOrderService = new DeliveryOrderService(new DeliveryOrderRepository(), new DeliveryOrderValidator());
            _salesDownPaymentService = new SalesDownPaymentService(new SalesDownPaymentRepository(), new SalesDownPaymentValidator());
            _salesDownPaymentAllocationService = new SalesDownPaymentAllocationService(new SalesDownPaymentAllocationRepository(), new SalesDownPaymentAllocationValidator());
            _salesAllowanceService = new SalesAllowanceService(new SalesAllowanceRepository(), new SalesAllowanceValidator());
            _salesInvoiceService = new SalesInvoiceService(new SalesInvoiceRepository(), new SalesInvoiceValidator());
            _salesInvoiceMigrationService = new SalesInvoiceMigrationService(new SalesInvoiceMigrationRepository());
            _recoveryOrderDetailService = new RecoveryOrderDetailService(new RecoveryOrderDetailRepository(), new RecoveryOrderDetailValidator());
            _recoveryAccessoryDetailService = new RecoveryAccessoryDetailService(new RecoveryAccessoryDetailRepository(), new RecoveryAccessoryDetailValidator());
            _blanketOrderService = new BlanketOrderService(new BlanketOrderRepository(), new BlanketOrderValidator());
            _blanketOrderDetailService = new BlanketOrderDetailService(new BlanketOrderDetailRepository(), new BlanketOrderDetailValidator());
            _blendingWorkOrderService = new BlendingWorkOrderService(new BlendingWorkOrderRepository(), new BlendingWorkOrderValidator());
            _repackingService = new RepackingService(new RepackingRepository(), new RepackingValidator());
            _stockAdjustmentService = new StockAdjustmentService(new StockAdjustmentRepository(), new StockAdjustmentValidator());
            _customerStockAdjustmentService = new CustomerStockAdjustmentService(new CustomerStockAdjustmentRepository(), new CustomerStockAdjustmentValidator());
            _receiptRequestService = new ReceiptRequestService(new ReceiptRequestRepository(), new ReceiptRequestValidator());
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
                            DebitAmount = model.Status == Constant.GeneralLedgerStatus.Debit ? model.Amount : 0,
                            CreditAmount = model.Status == Constant.GeneralLedgerStatus.Credit ? model.Amount : 0,
                            model.SourceDocument,
                            model.SourceDocumentId,
                            NomorSurat = model.SourceDocumentId,                                                                                        
                            model.AccountId,
                         }).Where(filter).OrderBy(sidx + " " + sord); //.ToList();

//             (model.SourceDocument == Constant.GeneralLedgerSource.BlanketOrderDetail) ? _blanketOrderDetailService.get .GetObjectById(model.SourceDocumentId).Code :
//                                         (model.SourceDocument == Constant.GeneralLedgerSource.StockAdjustment) ? _stockAdjustmentService.GetObjectById(model.SourceDocumentId).Code :
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
                            model.DebitAmount,
                            model.CreditAmount,
                            model.SourceDocument,
                            model.SourceDocumentId,
                            model.NomorSurat,
                            model.AccountId
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
                             DebitAmount = model.Status == Constant.GeneralLedgerStatus.Debit ? model.Amount : 0,
                             CreditAmount = model.Status == Constant.GeneralLedgerStatus.Credit ? model.Amount : 0,
                             model.SourceDocument,
                             model.SourceDocumentId,
                             NomorSurat = model.SourceDocumentId,
                             model.AccountId,
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
                            model.DebitAmount,
                            model.CreditAmount,
                            model.SourceDocument,
                            model.SourceDocumentId,
                            model.NomorSurat,
                            model.AccountId
                         }
                    }).ToArray()
            }, JsonRequestBehavior.AllowGet);
        }

        public dynamic GetInfoSaldo(int AccountId, DateTime StartDate, DateTime EndDate)
        {
            string filter = "AccountId = @0 AND TransactionDate >= @1 AND TransactionDate < @2";

            //var debit = _generalLedgerJournalService.GetQueryable().Include("Account").GroupBy(glj => glj.Account)
            //                                    .Select(g => new { TotalDebit = g.Sum(glj => glj.Amount) })
            //                                    .Where(filter + " AND STATUS = 1");
            //var credit = _generalLedgerJournalService.GetQueryable().Include("Account").GroupBy(glj => glj.Account)
            //                                    .Select(g => new { TotalDebit = g.Sum(glj => glj.Amount) })
            //                                    .Where(filter + " AND STATUS = 2");
            var q = _generalLedgerJournalService.GetQueryable().Include("Account");

            /*
            var results = (from ssi in fooList
                           // here I choose each field I want to group by
                           group ssi by new { ssi.Code, ssi.StatusId } into g
                           select new { AgencyCode = g.Key.Code, Status = g.Key.StatusId, Count = g.Count() }).ToList();
            */
            var query = (from model in q
                         select new
                         {
                             model.Id,
                             model.TransactionDate,
                             model.Status,
                             AccountCode = model.Account.Code,
                             Account = model.Account.Name,
                             DebitAmount = model.Status == Constant.GeneralLedgerStatus.Debit ? model.Amount : 0,
                             CreditAmount = model.Status == Constant.GeneralLedgerStatus.Credit ? model.Amount : 0,
                             model.SourceDocument,
                             model.SourceDocumentId,
                             NomorSurat = model.SourceDocumentId,
                             model.AccountId
                         }).Where(filter, AccountId, StartDate, EndDate.AddDays(1));

            var list = query.AsEnumerable();

            // (Debit) Asset, (Credit) Liability, Equity
            var journal = list.ElementAtOrDefault(0);
            Account account = (journal == null) ? null : _accountService.GetObjectById(list.ElementAtOrDefault(0).AccountId);
            Closing closing = _closingService.GetObjectByPeriodAndYear(StartDate.AddDays(-1).Month, StartDate.AddDays(-1).Year);
            ValidComb validComb = (closing == null) ? null : _validCombService.FindOrCreateObjectByAccountAndClosing(account.Id, closing.Id);
            decimal saldoawal = (validComb == null) ? 0 : validComb.Amount;
            decimal saldoakhir = saldoawal;

            foreach (var model in list)
            {
                if (account.Group == Constant.AccountGroup.Asset)
                {
                    saldoakhir += model.DebitAmount;
                    saldoakhir -= model.CreditAmount;
                }
                else if (account.Group == Constant.AccountGroup.Liability || account.Group == Constant.AccountGroup.Equity)
                {
                    saldoakhir += model.CreditAmount;
                    saldoakhir -= model.DebitAmount;
                }
            }

            return Json(new
            {
                SaldoAwal = saldoawal,
                saldoakhir = saldoakhir,
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
                             DebitAmount = model.Status == Constant.GeneralLedgerStatus.Debit ? model.Amount : 0,
                             CreditAmount = model.Status == Constant.GeneralLedgerStatus.Credit ? model.Amount : 0,
                             model.SourceDocument,
                             model.SourceDocumentId,
                             NomorSurat = model.SourceDocumentId,
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
                            model.DebitAmount,
                            model.CreditAmount,
                            model.SourceDocument,
                            model.SourceDocumentId,
                            model.NomorSurat,
                            model.AccountId
                         }
                    }).ToArray()
            }, JsonRequestBehavior.AllowGet);
        }
    }
}