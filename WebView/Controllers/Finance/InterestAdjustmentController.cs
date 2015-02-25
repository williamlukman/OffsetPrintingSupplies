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
    public class InterestAdjustmentController : Controller
    {
        private readonly static log4net.ILog LOG = log4net.LogManager.GetLogger("InterestAdjustmentController");
        private ICashBankService _cashBankService;
        private IInterestAdjustmentService _interestAdjustmentService;
        private ICashMutationService _cashMutationService;
        private IAccountService _accountService;
        private IGeneralLedgerJournalService _generalLedgerJournalService;
        private IClosingService _closingService;
        private ICurrencyService _currencyService;
        private IExchangeRateService _exchangeRateService;
        private IGLNonBaseCurrencyService _gLNonBaseCurrencyService;

        public InterestAdjustmentController()
        {
            _interestAdjustmentService = new InterestAdjustmentService(new InterestAdjustmentRepository(), new InterestAdjustmentValidator());
            _cashBankService = new CashBankService(new CashBankRepository(), new CashBankValidator());
            _cashMutationService = new CashMutationService(new CashMutationRepository(), new CashMutationValidator());
            _accountService = new AccountService(new AccountRepository(), new AccountValidator());
            _generalLedgerJournalService = new GeneralLedgerJournalService(new GeneralLedgerJournalRepository(), new GeneralLedgerJournalValidator());
            _closingService = new ClosingService(new ClosingRepository(), new ClosingValidator());
            _currencyService = new CurrencyService(new CurrencyRepository(), new CurrencyValidator());
            _exchangeRateService = new ExchangeRateService(new ExchangeRateRepository(), new ExchangeRateValidator());
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
            var q = _interestAdjustmentService.GetQueryable().Include("CashBank").Where(x => !x.IsDeleted);

            var query = (from model in q
                         select new
                         {
                             model.Id,
                             model.Code,
                             model.NoBukti,
                             model.CashBankId,
                             CashBank = model.CashBank.Name,
                             Amount = model.Amount * (model.IsExpense ? -1 : 1),
                             TaxAmount = model.TaxAmount * (model.IsExpense ? 1 : -1),
                             model.ExchangeRateAmount,
                             model.InterestDate,
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
                            model.NoBukti,
                            model.CashBank,
                            model.Amount,
                            model.TaxAmount,
                            model.ExchangeRateAmount,
                            model.InterestDate,
                            model.ConfirmationDate,
                            model.CreatedAt,
                            model.UpdatedAt,
                      }
                    }).ToArray()
            }, JsonRequestBehavior.AllowGet);
        }

        public dynamic GetInfo(int Id)
        {
            InterestAdjustment model = new InterestAdjustment();
            try
            {
                model = _interestAdjustmentService.GetObjectById(Id);

            }
            catch (Exception ex)
            {
                LOG.Error("GetInfo", ex);
                model.Errors.Add("Generic", "Error" + ex);
            }

            return Json(new
            {
                model.Id,
                model.Code,
                model.NoBukti,
                model.CashBankId,
                CashBank = _cashBankService.GetObjectById(model.CashBankId).Name,
                model.Amount,
                model.TaxAmount,
                model.ExchangeRateAmount,
                model.IsExpense,
                model.InterestDate,
                model.ConfirmationDate,
                model.Errors
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public dynamic Insert(InterestAdjustment model)
        {
            try
            {
               model = _interestAdjustmentService.CreateObject(model, _cashBankService, _currencyService, _exchangeRateService);
            }
            catch (Exception ex)
            {
                LOG.Error("Insert Failed", ex);
            }

            return Json(new
            {
                model.Errors
            });
        }

        [HttpPost]
        public dynamic Update(InterestAdjustment model)
        {
            try
            {
                var data = _interestAdjustmentService.GetObjectById(model.Id);
                data.CashBankId = model.CashBankId;
                data.InterestDate = model.InterestDate;
                data.NoBukti = model.NoBukti;
                data.Amount = model.Amount;
                data.ExchangeRateAmount = model.ExchangeRateAmount;
                data.TaxAmount = model.TaxAmount;
                data.IsExpense = model.IsExpense;
                model = _interestAdjustmentService.UpdateObject(data,_cashBankService, _currencyService, _exchangeRateService);
            }
            catch (Exception ex)
            {
                LOG.Error("Update Failed", ex);
            }

            return Json(new
            {
                model.Errors
            });
        }

        [HttpPost]
        public dynamic Delete(InterestAdjustment model)
        {
            try
            {
               var data = _interestAdjustmentService.GetObjectById(model.Id);
               model = _interestAdjustmentService.SoftDeleteObject(data);
            }
            catch (Exception ex)
            {
                LOG.Error("Delete Failed", ex);
                model.Errors.Add("Generic", "Error" + ex);
            }

            return Json(new
            {
                model.Errors
            });
        }

        [HttpPost]
        public dynamic Confirm(InterestAdjustment model)
        {
            try
            {
                
                var data = _interestAdjustmentService.GetObjectById(model.Id);
                model = _interestAdjustmentService.ConfirmObject(data,model.ConfirmationDate.Value,_cashMutationService,_cashBankService,_accountService
                    ,_generalLedgerJournalService,_closingService,_currencyService,_exchangeRateService,_gLNonBaseCurrencyService);
            }
            catch (Exception ex)
            {
                LOG.Error("Confirm Failed", ex);
                model.Errors.Add("Generic", "Error" + ex);
            }

            return Json(new
            {
                model.Errors
            });
        }

        [HttpPost]
        public dynamic UnConfirm(InterestAdjustment model)
        {
            try
            {
                var data = _interestAdjustmentService.GetObjectById(model.Id);
                model = _interestAdjustmentService.UnconfirmObject(data,_cashMutationService,_cashBankService,_accountService, _generalLedgerJournalService, _closingService,_currencyService,_gLNonBaseCurrencyService);
            }
            catch (Exception ex)
            {
                LOG.Error("Unconfirm Failed", ex);
                model.Errors.Add("Generic", "Error" + ex);
            }

            return Json(new
            {
                model.Errors
            });
        }

    }
}
