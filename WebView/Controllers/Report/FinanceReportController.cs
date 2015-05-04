using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Core.Interface.Service;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using Data.Repository;
using Service.Service;
using Validation.Validation;
using System.Linq.Dynamic;
using System.Data.Entity;
using Core.DomainModel;
using Core.Constants;
using Data.Context;
using System.Data.Objects;

namespace WebView.Controllers
{
    public class FinanceReportController : Controller
    {
        //
        // GET: /FinanceReport/
        private readonly static log4net.ILog LOG = log4net.LogManager.GetLogger("FinanceReportController");
        private IAccountService _accountService;
        private IClosingService _closingService;
        private IValidCombService _validCombService;
        private IValidCombIncomeStatementService _validCombIncomeStatementService;
        private IGeneralLedgerJournalService _generalLedgerJournalService;
        private ICompanyService _companyService;

        public class ModelIncomeStatement
        {
            public string CompanyName { get; set; }
            public DateTime BeginningDate { get; set; }
            public DateTime EndDate { get; set; }
            public decimal Revenue { get; set; }
            public decimal COGS { get; set; }
            public decimal OperationalExpenses { get; set; }
            public decimal InterestEarning { get; set; }
            public decimal Depreciation { get; set; }
            public decimal Amortization { get; set; }
            public decimal Tax { get; set; }
            public decimal Divident { get; set; }
        }

        public class ModelBalanceSheet
        {
            public string CompanyName { get; set; }
            public DateTime StartDate { get; set; }
            public DateTime EndDate { get; set; }
            public string DCNote { get; set; }
            public string AccountName { get; set; }
            public string AccountGroup { get; set; }
            public string AccountParent { get; set; }
            public string AccountTitle { get; set; }
            public decimal CurrentAmount { get; set; }
            public decimal PrevAmount { get; set; }
            public string ASSET { get; set; }
            public string AccountCode { get; set; }
            public string AccountParentCode { get; set; }
            public int AccountLevel { get; set; }
            public bool IsLeaf { get; set; }
        }

        public FinanceReportController()
        {
            _accountService = new AccountService(new AccountRepository(), new AccountValidator());
            _closingService = new ClosingService(new ClosingRepository(), new ClosingValidator());
            _validCombService = new ValidCombService(new ValidCombRepository(), new ValidCombValidator());
            _validCombIncomeStatementService = new ValidCombIncomeStatementService(new ValidCombIncomeStatementRepository(), new ValidCombIncomeStatementValidator());            
            _generalLedgerJournalService = new GeneralLedgerJournalService(new GeneralLedgerJournalRepository(), new GeneralLedgerJournalValidator());
            _companyService = new CompanyService(new CompanyRepository(), new CompanyValidator());
        }

        public ActionResult IncomeStatement()
        {
            if (!AuthenticationModel.IsAllowed("View", Constant.MenuName.IncomeStatement, Constant.MenuGroupName.Report))
            {
                return Content(Constant.ControllerOutput.PageViewNotAllowed);
            }

            return View();
        }
         
        public ActionResult TranscationList()
        {
            if (!AuthenticationModel.IsAllowed("View", Constant.MenuName.Finance, Constant.MenuGroupName.Report))
            {
                return Content(Constant.ControllerOutput.PageViewNotAllowed);
            }

            return View();
        } 

        public ActionResult ReportTranscationList(DateTime startDate, DateTime endDate)
        {
            using (var db = new OffsetPrintingSuppliesEntities())
            {
                DateTime endDay = endDate.Date.AddDays(1);
                var company = _companyService.GetQueryable().FirstOrDefault();
                //var salesInvoice = _salesInvoiceService.GetObjectById(Id);
                var q = db.GeneralLedgerJournals.Include(x => x.Account)
                                                    .Where(x => !x.IsDeleted && (
                                                            (x.TransactionDate >= startDate && x.TransactionDate < endDay)
                                                        )).ToList();
                string user = AuthenticationModel.GetUserName();

                var query = q.GroupBy(m => new 
                {
                    TransactionDate = m.TransactionDate.Date, //EntityFunctions.TruncateTime(m.TransactionDate).Value,
                    SourceDocument = m.SourceDocument,
                    SourceDocumentId = m.SourceDocumentId,
                    AccountCode = m.Account.Code,
                }).
                Select(g => new
                {
                    TransactionDate = g.Key.TransactionDate,
                    SourceDocument = g.Key.SourceDocument,
                    SourceDocumentId = g.Key.SourceDocumentId,
                    AccountName = g.FirstOrDefault().Account.Name,
                    AccountCode = g.Key.AccountCode,
                    NoBukti = db.GetNoBukti(g.Key.SourceDocument, g.Key.SourceDocumentId),
                    Amount = db.GeneralLedgerJournals.Where(x=>x.IsDeleted == false && 
                                                              x.TransactionDate == g.Key.TransactionDate && 
                                                              x.SourceDocument == g.Key.SourceDocument &&
                                                              x.SourceDocumentId == g.Key.SourceDocumentId &&
                                                              x.Account.Code == g.Key.AccountCode).
                                                              Sum(x => (Decimal?)(x.Status == Constant.GeneralLedgerStatus.Credit ? -x.Amount : x.Amount)) ?? 0
                }).Where(x => x.Amount != 0).OrderBy(x => x.TransactionDate).ThenBy(x => x.Amount > 0 ? "D" : "K").ThenBy(x => x.NoBukti).ToList();

                if (!query.Any())
                {
                    return Content(Constant.ControllerOutput.ErrorPageRecordNotFound);
                }

                var rd = new ReportDocument();

                //Loading Report
                rd.Load(Server.MapPath("~/") + "Reports/Finance/DaftarTransaksi.rpt");

                // Setting report data source
                rd.SetDataSource(query);

                // Setting subreport data source
                //rd.Subreports["subreport.rpt"].SetDataSource(q2);

                // Set parameters, need to be done after all data sources are set (to prevent reseting parameters)
                rd.SetParameterValue("CompanyName", company.Name);
                //rd.SetParameterValue("AsOfDate", DateTime.Today);
                rd.SetParameterValue("startDate", startDate.Date);
                rd.SetParameterValue("endDate", endDate.Date);

                var stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                return File(stream, "application/pdf");
            }
        }

        public ActionResult KartuBukuBesar()
        {
            if (!AuthenticationModel.IsAllowed("View", Constant.MenuName.Finance, Constant.MenuGroupName.Report))
            {
                return Content(Constant.ControllerOutput.PageViewNotAllowed);
            }

            return View();
        }

        public ActionResult ReportKartuBukuBesar(DateTime startDate, DateTime endDate, int AccountId = 0)
        {
            using (var db = new OffsetPrintingSuppliesEntities())
            {
                DateTime endDay = endDate.Date.AddDays(1);
                var company = _companyService.GetQueryable().FirstOrDefault();
                //var salesInvoice = _salesInvoiceService.GetObjectById(Id);
                var q = db.GeneralLedgerJournals.Include(x => x.Account)
                                                    .Where(x => !x.IsDeleted && (
                                                            (x.AccountId == AccountId && x.TransactionDate >= startDate && x.TransactionDate < endDay)
                                                        )).ToList();
                string user = AuthenticationModel.GetUserName();

                var obj = q.FirstOrDefault();

                var query = q.GroupBy(m => new
                {
                    TransactionDate = m.TransactionDate.Date, //EntityFunctions.TruncateTime(m.TransactionDate).Value,
                    SourceDocument = m.SourceDocument,
                    SourceDocumentId = m.SourceDocumentId,
                    //AccountCode = m.Account.Code,
                }).
                Select(g => new
                {
                    TransactionDate = g.Key.TransactionDate,
                    SourceDocument = g.Key.SourceDocument,
                    SourceDocumentId = g.Key.SourceDocumentId,
                    //AccountName = g.FirstOrDefault().Account.Name,
                    //AccountCode = g.Key.AccountCode,
                    NoBukti = db.GetNoBukti(g.Key.SourceDocument, g.Key.SourceDocumentId),
                    Amount = db.GeneralLedgerJournals.Where(x => x.IsDeleted == false &&
                                                              x.TransactionDate == g.Key.TransactionDate &&
                                                              x.SourceDocument == g.Key.SourceDocument &&
                                                              x.SourceDocumentId == g.Key.SourceDocumentId &&
                                                              x.AccountId == AccountId).
                                                              Sum(x => (Decimal?)(x.Status == Constant.GeneralLedgerStatus.Credit ? -x.Amount : x.Amount)) ?? 0
                }).Where(x => x.Amount != 0).OrderBy(x => x.TransactionDate).ThenBy(x => x.Amount > 0 ? "D" : "K").ThenBy(x => x.NoBukti).ToList();

                if (!query.Any())
                {
                    return Content(Constant.ControllerOutput.ErrorPageRecordNotFound);
                }

                var rd = new ReportDocument();

                //Loading Report
                rd.Load(Server.MapPath("~/") + "Reports/Finance/KartuBukuBesar.rpt");

                // Setting report data source
                rd.SetDataSource(query);

                // Setting subreport data source
                //rd.Subreports["subreport.rpt"].SetDataSource(q2);

                // Set parameters, need to be done after all data sources are set (to prevent reseting parameters)
                DateTime startDay = startDate.Date;
                decimal SaldoAwal = db.GeneralLedgerJournals.Where(x => !x.IsDeleted && x.AccountId == AccountId && x.TransactionDate < startDate).Sum(x => (Decimal?)(x.Status == Constant.GeneralLedgerStatus.Credit ? -x.Amount : x.Amount)) ?? 0;
                rd.SetParameterValue("CompanyName", company.Name);
                //rd.SetParameterValue("AsOfDate", DateTime.Today);
                rd.SetParameterValue("startDate", startDate.Date);
                rd.SetParameterValue("endDate", endDate.Date);
                rd.SetParameterValue("Code", obj.Account.Code);
                rd.SetParameterValue("Name", obj.Account.Name);
                rd.SetParameterValue("SaldoAwal", SaldoAwal);
                rd.SetParameterValue("SaldoAkhir", SaldoAwal + query.Sum(x => x.Amount));

                var stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                return File(stream, "application/pdf");
            }
        }

        public ActionResult NeracaSaldo()
        {
            if (!AuthenticationModel.IsAllowed("View", Constant.MenuName.Finance, Constant.MenuGroupName.Report))
            {
                return Content(Constant.ControllerOutput.PageViewNotAllowed);
            }

            return View();
        }

        public ActionResult ReportNeracaSaldo(DateTime Tgl)
        {
            using (var db = new OffsetPrintingSuppliesEntities())
            {
                DateTime startDay = Tgl.Date;
                var company = _companyService.GetQueryable().FirstOrDefault();
                //var salesInvoice = _salesInvoiceService.GetObjectById(Id);
                var acl = db.Accounts.Where(x => !x.IsDeleted && x.IsLeaf && x.Level == 5); //.OrderBy(x => x.Code);
                var q = db.GeneralLedgerJournals.Include(x => x.Account)
                                                    .Where(x => !x.IsDeleted && (
                                                            (EntityFunctions.TruncateTime(x.TransactionDate) == startDay)
                                                        ));
                string user = AuthenticationModel.GetUserName();

                var obj = q.FirstOrDefault();

                var query = acl.GroupJoin(q, outer => outer.Id, inner => inner.AccountId, (outer, inner) => new
                {
                    AccountId = outer.Id,
                    AccountCode = outer.Code,
                    AccountName = outer.Name,
                    Amount = inner.Sum(x => (Decimal?)(x.Status == Constant.GeneralLedgerStatus.Credit ? -x.Amount : x.Amount)) ?? 0,
                    SaldoAwal = db.GeneralLedgerJournals.Where(x => !x.IsDeleted && x.AccountId == outer.Id && EntityFunctions.TruncateTime(x.TransactionDate) < startDay).Sum(x => (Decimal?)(x.Status == Constant.GeneralLedgerStatus.Credit ? -x.Amount : x.Amount)) ?? 0,
                }).Select(m => new {
                    AccountId = m.AccountId,
                    AccountCode = m.AccountCode,
                    AccountName = m.AccountName,
                    Amount = m.Amount,
                    SaldoAwal = m.SaldoAwal,
                    SaldoAkhir = m.SaldoAwal + m.Amount,
                }).OrderBy(x => x.AccountCode).ToList();

                if (!query.Any())
                {
                    return Content(Constant.ControllerOutput.ErrorPageRecordNotFound);
                }

                var rd = new ReportDocument();

                //Loading Report
                rd.Load(Server.MapPath("~/") + "Reports/Finance/NeracaSaldo.rpt");

                // Setting report data source
                rd.SetDataSource(query);

                // Setting subreport data source
                //rd.Subreports["subreport.rpt"].SetDataSource(q2);

                // Set parameters, need to be done after all data sources are set (to prevent reseting parameters)
                rd.SetParameterValue("CompanyName", company.Name);
                rd.SetParameterValue("Tgl", Tgl);

                var stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                return File(stream, "application/pdf");
            }
        }

        // Revenue - Expense - TaxExpense - Divident = NetEarnings
        public ActionResult ReportIncomeStatement(int period, int yearPeriod)
        {
            var company = _companyService.GetQueryable().FirstOrDefault();
            Closing closing = _closingService.GetObjectByPeriodAndYear(period, yearPeriod);

            if (closing == null) { return Content(Constant.ControllerOutput.ErrorPageHasNoClosingDate); }

            ValidCombIncomeStatement Revenue = _validCombIncomeStatementService.FindOrCreateObjectByAccountAndClosing(_accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.Revenue).Id, closing.Id);
            ValidCombIncomeStatement COGSExpense = _validCombIncomeStatementService.FindOrCreateObjectByAccountAndClosing(_accountService.GetObjectByCode(Constant.AccountCode.COGSExpense).Id, closing.Id); // Constant.AccountLegacyCode.COGS ? 
            ValidCombIncomeStatement SellingGeneralAndAdministrationExpense = _validCombIncomeStatementService.FindOrCreateObjectByAccountAndClosing(_accountService.GetObjectByCode(Constant.AccountCode.SellingGeneralAndAdministrationExpense).Id, closing.Id);
            ValidCombIncomeStatement NonOperationalExpense = _validCombIncomeStatementService.FindOrCreateObjectByAccountAndClosing(_accountService.GetObjectByCode(Constant.AccountCode.NonOperationalExpense).Id, closing.Id);
            ValidCombIncomeStatement DepreciationExpense = _validCombIncomeStatementService.FindOrCreateObjectByAccountAndClosing(_accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.DepreciationExpense).Id, closing.Id);
            ValidCombIncomeStatement AmortizationExpense = _validCombIncomeStatementService.FindOrCreateObjectByAccountAndClosing(_accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.Amortization).Id, closing.Id);
            ValidCombIncomeStatement InterestExpense = _validCombIncomeStatementService.FindOrCreateObjectByAccountAndClosing(_accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.InterestExpense).Id, closing.Id);
            ValidCombIncomeStatement TaxExpense = _validCombIncomeStatementService.FindOrCreateObjectByAccountAndClosing(_accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.TaxExpense).Id, closing.Id);
            ValidCombIncomeStatement DividentExpense = _validCombIncomeStatementService.FindOrCreateObjectByAccountAndClosing(_accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.DividentExpense).Id, closing.Id);

            ModelIncomeStatement model = new ModelIncomeStatement()
            {
                CompanyName = company.Name,
                BeginningDate = closing.BeginningPeriod.Date,
                EndDate = closing.EndDatePeriod.Date,
                Revenue = Revenue.Amount,
                COGS = COGSExpense.Amount,
                OperationalExpenses = SellingGeneralAndAdministrationExpense.Amount + NonOperationalExpense.Amount,
                InterestEarning = InterestExpense.Amount,
                Depreciation = DepreciationExpense.Amount,
                Amortization = AmortizationExpense.Amount,
                Tax = TaxExpense.Amount,
                Divident = DividentExpense.Amount
            };

            List<ModelIncomeStatement> list = new List<ModelIncomeStatement>();
            list.Add(model);

            var rd = new ReportDocument();

            //Loading Report
            rd.Load(Server.MapPath("~/") + "Reports/Finance/IncomeStatement.rpt");

            // Setting report data source
            rd.SetDataSource(list);

            var stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            return File(stream, "application/pdf");
        }

        public ActionResult BalanceSheet()
        {
            if (!AuthenticationModel.IsAllowed("View", Constant.MenuName.BalanceSheet, Constant.MenuGroupName.Report))
            {
                return Content(Constant.ControllerOutput.PageViewNotAllowed);
            }

            return View();
        }

        public ActionResult ReportBalanceSheet(int closingId)
        {
            var company = _companyService.GetQueryable().FirstOrDefault();
            Closing closing = _closingService.GetObjectById(closingId);

            if (closing == null) { return Content(Constant.ControllerOutput.ErrorPageHasNoClosingDate); }

            var balanceValidComb = _validCombService.GetQueryable().Include("Account").Include("Closing")
                                                    .Where(x => x.ClosingId == closing.Id &&
                                                           (x.Account.Group == Constant.AccountGroup.Asset ||
                                                            x.Account.Group == Constant.AccountGroup.Liability ||
                                                            x.Account.Group == Constant.AccountGroup.Equity) && 
                                                           (x.Account.Level == 2 || (x.Account.Level == 1 && x.Account.IsLeaf)));

            List<ModelBalanceSheet> query = new List<ModelBalanceSheet>();
            query = (from obj in balanceValidComb
                     select new ModelBalanceSheet()
                     {
                         CompanyName = company.Name,
                         StartDate = closing.BeginningPeriod,
                         EndDate = closing.EndDatePeriod,
                         DCNote = (obj.Account.Group == Constant.AccountGroup.Asset ||
                                  obj.Account.Group == Constant.AccountGroup.Expense) ? "D" : "C",
                         AccountName = obj.Account.Code.Substring(0, 1),
                         AccountGroup = (obj.Account.Group == Constant.AccountGroup.Asset) ? "Asset" :
                                        (obj.Account.Group == Constant.AccountGroup.Expense) ? "Expense" :
                                        (obj.Account.Group == Constant.AccountGroup.Liability) ? "Liability" :
                                        (obj.Account.Group == Constant.AccountGroup.Equity) ? "Equity" :
                                        (obj.Account.Group == Constant.AccountGroup.Revenue) ? "Revenue" : "",
                        AccountTitle = obj.Account.Name,
                        CurrentAmount = obj.Amount,
                        PrevAmount = obj.Amount,
                        ASSET = "NonASSET", // untuk Fix Asset ? "ASSET" : "nonASSET",
                        AccountCode = obj.Account.Code
                     }).OrderBy(x => x.AccountCode).ToList();

            var rd = new ReportDocument();

            //Loading Report
            rd.Load(Server.MapPath("~/") + "Reports/Finance/BalanceSheet.rpt");

            // Setting report data source
            rd.SetDataSource(query);

            var stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            return File(stream, "application/pdf");
        }

        public ActionResult BalanceSheetDetail()
        {
            if (!AuthenticationModel.IsAllowed("View", Constant.MenuName.BalanceSheet, Constant.MenuGroupName.Report))
            {
                return Content(Constant.ControllerOutput.PageViewNotAllowed);
            }

            return View();
        }

        public ActionResult ReportBalanceSheetDetail(Nullable<int> closingId)
        {
            var company = _companyService.GetQueryable().FirstOrDefault();
            Closing closing = _closingService.GetObjectById(closingId.GetValueOrDefault());

            if (closing == null) return Content(Constant.ControllerOutput.ErrorPageRecordNotFound);

            var balanceValidComb = _validCombService.GetQueryable().Include("Account").Include("Closing")
                                                    .Where(x => x.ClosingId == closing.Id & x.Account.Level >= 2
                                                    && x.Account.Group != (int)Constant.AccountGroup.Expense && x.Account.Group != (int)Constant.AccountGroup.Revenue
                                                    );

            //List<ModelBalanceSheet> query = new List<ModelBalanceSheet>();
            var query = (from obj in balanceValidComb
                         select new ModelBalanceSheet()
                         {
                             CompanyName = company.Name,
                             StartDate = closing.BeginningPeriod.Date,
                             EndDate = closing.EndDatePeriod.Date,
                             DCNote = (obj.Account.Group == (int)Constant.AccountGroup.Asset ||
                                      obj.Account.Group == (int)Constant.AccountGroup.Expense) ? "D" : "C",
                             AccountName = obj.Account.Code.Substring(0, 1),
                             AccountGroup = (obj.Account.Group == (int)Constant.AccountGroup.Asset) ? "Asset" :
                                            (obj.Account.Group == (int)Constant.AccountGroup.Expense) ? "Expense" :
                                            (obj.Account.Group == (int)Constant.AccountGroup.Liability) ? "Liability" :
                                            (obj.Account.Group == (int)Constant.AccountGroup.Equity) ? "Equity" :
                                            (obj.Account.Group == (int)Constant.AccountGroup.Revenue) ? "Revenue" : "",
                             AccountTitle = obj.Account.Name,
                             AccountParent = obj.Account.Parent.Name,
                             CurrentAmount = obj.Amount,
                             PrevAmount = obj.Amount,
                             ASSET = "nonASSET", // untuk Fix Asset ? "ASSET" : "nonASSET",
                             AccountCode = obj.Account.Code,
                             AccountParentCode = obj.Account.Parent.Code,
                             AccountLevel = obj.Account.Level,
                             IsLeaf = obj.Account.IsLeaf,
                         }).OrderBy(x => x.AccountCode);

            var query1 = query.Where(x => x.AccountGroup == "Asset" || x.AccountGroup == "Expense").ToList();
            var query2 = query.Where(x => x.AccountGroup != "Asset" && x.AccountGroup != "Expense").ToList();

            var rd = new ReportDocument();

            //Loading Report
            rd.Load(Server.MapPath("~/") + "Reports/Finance/BalanceSheetDetail.rpt");

            // Setting report data source
            rd.SetDataSource(new List<object>());

            // Setting subreport data source
            rd.Subreports["SubBalanceSheetDetail1.rpt"].SetDataSource(query1);
            rd.Subreports["SubBalanceSheetDetail2.rpt"].SetDataSource(query2);

            // Set parameters, need to be done after all data sources are set (to prevent reseting parameters)
            rd.SetParameterValue("CompanyName", company.Name);
            rd.SetParameterValue("StartDate", closing.BeginningPeriod.Date);
            rd.SetParameterValue("EndDate", closing.EndDatePeriod.Date);

            var stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            return File(stream, "application/pdf");
        }

        #region ARCustomerPayment
        public ActionResult ARCustomerPayment()
        {
            if (!AuthenticationModel.IsAllowed("View", Constant.MenuName.Finance, Constant.MenuGroupName.Report))
            {
                return Content(Constant.ControllerOutput.PageViewNotAllowed);
            }
            return View(); 
        }

        public ActionResult ReportARCustomerPayment(DateTime startDate, DateTime endDate)
        {
            using (var db = new OffsetPrintingSuppliesEntities())
            {
                DateTime endDay = endDate.AddDays(1);
                var company = _companyService.GetQueryable().FirstOrDefault();
                //var salesInvoice = _salesInvoiceService.GetObjectById(Id);
                var q = db.ReceiptVoucherDetails.Include(x => x.ReceiptVoucher).Include(x => x.Receivable)
                                                  .Where(x => !x.IsDeleted && !x.ReceiptVoucher.IsDeleted && (
                                                            (x.ReceiptVoucher.ReceiptDate >= startDate && x.ReceiptVoucher.ReceiptDate < endDay)
                                                        ) && (
                                                            (x.Receivable.ReceivableSource == Constant.ReceivableSource.SalesInvoice) ||
                                                            (x.Receivable.ReceivableSource == Constant.ReceivableSource.SalesInvoiceMigration)
                                                        ));
                string user = AuthenticationModel.GetUserName();

                var query = q.Select(g => new
                {
                    CustomerName = g.ReceiptVoucher.Contact.Name??"", //g.FirstOrDefault().SalesInvoice.DeliveryOrder.SalesOrder.Contact.NamaFakturPajak, //g.Key.CustomerGroup,
                    Currency = (g.Receivable.Currency.Name == "Rupiah") ? "IDR" : (g.Receivable.Currency.Name == "Euro") ? "EUR" : g.Receivable.Currency.Name,
                    RefNo = g.ReceiptVoucher.NoBukti,
                    PaymentDate = g.ReceiptVoucher.ReceiptDate,
                    Amount = g.Amount,
                    //InvoiceId = g.Receivable.ReceivableSourceId,
                    InvoiceCode = (g.Receivable.ReceivableSource == Constant.ReceivableSource.SalesInvoice) ? db.SalesInvoices.Where(x => x.Id == g.Receivable.ReceivableSourceId).FirstOrDefault().NomorSurat ?? "" : db.SalesInvoiceMigrations.Where(x => x.Id == g.Receivable.ReceivableSourceId).FirstOrDefault().NomorSurat ?? "",
                    InvoiceDate = (g.Receivable.ReceivableSource == Constant.ReceivableSource.SalesInvoice) ? db.SalesInvoices.Where(x => x.Id == g.Receivable.ReceivableSourceId).FirstOrDefault().InvoiceDate : db.SalesInvoiceMigrations.Where(x => x.Id == g.Receivable.ReceivableSourceId).FirstOrDefault().InvoiceDate,
                    Discount = (g.Receivable.ReceivableSource == Constant.ReceivableSource.SalesInvoice) ? db.SalesInvoices.Where(x => x.Id == g.Receivable.ReceivableSourceId).FirstOrDefault().Discount : 0, //g.Where(x => (x.SalesInvoice.DeliveryOrder.SalesOrder.SalesDate == g.Key.SalesDate)).Sum(x => (Decimal?)x.DeliveryOrderDetail.SalesOrderDetail.Item.PriceMutations.Where(y => (y.DeactivatedAt == null || g.Key.SalesDate < y.DeactivatedAt.Value)).OrderByDescending(y => y.DeactivatedAt.Value).FirstOrDefault().Amount) ?? 0, //.Sum(x => (Decimal?)(x.SalesInvoice.Discount * g.Key.Price)/100.0m) ?? 0,
                }).OrderBy(x => x.PaymentDate).ThenBy(x => x.CustomerName).ThenBy(x => x.InvoiceDate).ToList();

                if (!query.Any())
                {
                    return Content(Constant.ControllerOutput.ErrorPageRecordNotFound);
                }

                var rd = new ReportDocument();

                //Loading Report
                rd.Load(Server.MapPath("~/") + "Reports/Finance/ARCustomerPayment.rpt");

                // Setting report data source
                rd.SetDataSource(query);

                // Setting subreport data source
                //rd.Subreports["subreport.rpt"].SetDataSource(q2);

                // Set parameters, need to be done after all data sources are set (to prevent reseting parameters)
                rd.SetParameterValue("CompanyName", company.Name);
                rd.SetParameterValue("AsOfDate", DateTime.Today);
                rd.SetParameterValue("startDate", startDate);
                rd.SetParameterValue("endDate", endDay);

                var stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                return File(stream, "application/pdf");
            }
        }
        #endregion


    }
}
