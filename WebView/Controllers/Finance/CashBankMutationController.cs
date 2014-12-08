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
    public class CashBankMutationController : Controller
    {
        private readonly static log4net.ILog LOG = log4net.LogManager.GetLogger("CashBankMutationController");
        private ICashBankService _cashBankService;
        private ICashBankAdjustmentService _cashBankAdjustmentService;
        private ICashMutationService _cashMutationService;
        private ICashBankMutationService _cashBankMutationService;
        private IAccountService _accountService;
        private IGeneralLedgerJournalService _generalLedgerJournalService;
        private IClosingService _closingService;
        private ICurrencyService _currencyService;
        private IExchangeRateService _exchangeRateService;

        public CashBankMutationController()
        {
            _cashBankAdjustmentService = new CashBankAdjustmentService(new CashBankAdjustmentRepository(), new CashBankAdjustmentValidator());
            _cashBankService = new CashBankService(new CashBankRepository(), new CashBankValidator());
            _cashMutationService = new CashMutationService(new CashMutationRepository(), new CashMutationValidator());
            _cashBankMutationService = new CashBankMutationService(new CashBankMutationRepository(), new CashBankMutationValidator());
            _accountService = new AccountService(new AccountRepository(), new AccountValidator());
            _generalLedgerJournalService = new GeneralLedgerJournalService(new GeneralLedgerJournalRepository(), new GeneralLedgerJournalValidator());
            _closingService = new ClosingService(new ClosingRepository(), new ClosingValidator());
            _currencyService = new CurrencyService(new CurrencyRepository(), new CurrencyValidator());
            _exchangeRateService = new ExchangeRateService(new ExchangeRateRepository(), new ExchangeRateValidator());
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
            var q = _cashBankMutationService.GetQueryable().Include("CashBank").Where(x => !x.IsDeleted);

            var query = (from model in q
                         select new
                         {
                             model.Id,
                             model.Code,
                             SourceCashBank = model.SourceCashBank.Name,
                             TargetCashBank = model.TargetCashBank.Name,
                             model.MutationDate,
                             model.Amount,
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
                            model.SourceCashBank,
                            model.TargetCashBank,
                            model.MutationDate,
                            model.Amount,
                            model.ConfirmationDate,
                            model.CreatedAt,
                            model.UpdatedAt,
                      }
                    }).ToArray()
            }, JsonRequestBehavior.AllowGet);
        }

        public dynamic GetInfo(int Id)
        {
            CashBankMutation model = new CashBankMutation();
            try
            {
                model = _cashBankMutationService.GetObjectById(Id);

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
                model.Amount,
                model.SourceCashBankId,
                SourceCashBank = _cashBankService.GetObjectById(model.SourceCashBankId).Name,
                model.TargetCashBankId,
                TargetCashBank = _cashBankService.GetObjectById(model.TargetCashBankId).Name,
                model.MutationDate,
                model.IsConfirmed,
                model.ConfirmationDate,
                model.Errors
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public dynamic Insert(CashBankMutation model)
        {
            try
            {
               model = _cashBankMutationService.CreateObject(model,_cashBankService);
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
        public dynamic Update(CashBankMutation model)
        {
            try
            {
                var data = _cashBankMutationService.GetObjectById(model.Id);
                data.SourceCashBankId = model.SourceCashBankId;
                data.TargetCashBankId = model.TargetCashBankId;
                data.Amount = model.Amount;
                data.MutationDate = model.MutationDate;
                model = _cashBankMutationService.UpdateObject(data,_cashBankService);
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
        public dynamic Delete(CashBankMutation model)
        {
            try
            {
               var data = _cashBankMutationService.GetObjectById(model.Id);
               model = _cashBankMutationService.SoftDeleteObject(data);
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
        public dynamic Confirm(CashBankMutation model)
        {
            try
            {
                
                var data = _cashBankMutationService.GetObjectById(model.Id);
                model = _cashBankMutationService.ConfirmObject(data,model.ConfirmationDate.Value,_cashMutationService,_cashBankService,
                                                               _accountService,_generalLedgerJournalService, _closingService,_currencyService, _exchangeRateService);
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
        public dynamic UnConfirm(CashBankMutation model)
        {
            try
            {
                var data = _cashBankMutationService.GetObjectById(model.Id);
                model = _cashBankMutationService.UnconfirmObject(data,_cashMutationService,_cashBankService,_accountService,_generalLedgerJournalService, _closingService,_currencyService);
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