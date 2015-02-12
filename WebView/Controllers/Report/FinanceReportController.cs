﻿using System;
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
            public string AccountTitle { get; set; }
            public decimal CurrentAmount { get; set; }
            public decimal PrevAmount { get; set; }
            public string ASSET { get; set; }
            public string AccountCode { get; set; }
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
                                                        )).OrderBy(x =>x.Id);
                string user = AuthenticationModel.GetUserName();

                var query = q.GroupBy(m => new 
                {
                    TransactionDate = EntityFunctions.TruncateTime(m.TransactionDate).Value,
                    SourceDocument =m.SourceDocument,
                    SourceDocumentId =m.SourceDocumentId,
                    AccountCode = m.Account.Code,
                   
                }).
                Select(g => new
                {
                    TransactionDate = g.Key.TransactionDate,
                    SourceDocument = g.Key.SourceDocument,
                    SourceDocumentId = g.Key.SourceDocumentId,
                    DocumentId = g.Key.SourceDocumentId,
                    AccountName = g.FirstOrDefault().Account.Name,
                    AccountCode = g.Key.AccountCode,
                    Debet = db.GeneralLedgerJournals.Where(x=>x.IsDeleted == false && 
                                                              x.TransactionDate == g.Key.TransactionDate && 
                                                              x.SourceDocument == g.Key.SourceDocument &&
                                                              x.SourceDocumentId == g.Key.SourceDocumentId &&
                                                              x.Account.Code == g.Key.AccountCode).
                                                              Sum(x=>((Decimal?)(x.Status == Constant.GeneralLedgerStatus.Credit ? -x.Amount : x.Amount)) ?? 0)
                }).ToList();

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


        // Revenue - Expense - TaxExpense - Divident = NetEarnings
        public ActionResult ReportIncomeStatement(int period, int yearPeriod)
        {
            var company = _companyService.GetQueryable().FirstOrDefault();
            Closing closing = _closingService.GetObjectByPeriodAndYear(period, yearPeriod);

            if (closing == null) { return Content(Constant.ControllerOutput.ErrorPageHasNoClosingDate); }

            ValidCombIncomeStatement Revenue = _validCombIncomeStatementService.FindOrCreateObjectByAccountAndClosing(_accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.Revenue).Id, closing.Id);
            ValidCombIncomeStatement COGSExpense = _validCombIncomeStatementService.FindOrCreateObjectByAccountAndClosing(_accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.COGSExpense).Id, closing.Id);
            ValidCombIncomeStatement SellingGeneralAndAdministrationExpense = _validCombIncomeStatementService.FindOrCreateObjectByAccountAndClosing(_accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.SellingGeneralAndAdministrationExpense).Id, closing.Id);
            ValidCombIncomeStatement NonOperationalExpense = _validCombIncomeStatementService.FindOrCreateObjectByAccountAndClosing(_accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.NonOperationalExpense).Id, closing.Id);
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
    }
}
