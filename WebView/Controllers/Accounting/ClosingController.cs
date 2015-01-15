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
    public class ClosingController : Controller
    {
        private readonly static log4net.ILog LOG = log4net.LogManager.GetLogger("ClosingController");

        private IAccountService _accountService;
        private IClosingService _closingService;
        private IGeneralLedgerJournalService _generalLedgerJournalService;
        private IValidCombService _validCombService;
        public ICurrencyService _currencyService ;
        private IExchangeRateClosingService _exchangeRateClosingService;
        private IGLNonBaseCurrencyService _gLNonBaseCurrencyService;
        private IVCNonBaseCurrencyService _vCNonBaseCurrencyService;
        private ICashBankService _cashBankService;
        public ClosingController()
        {
            _accountService = new AccountService(new AccountRepository(), new AccountValidator());
            _closingService = new ClosingService(new ClosingRepository(), new ClosingValidator());
            _generalLedgerJournalService = new GeneralLedgerJournalService(new GeneralLedgerJournalRepository(), new GeneralLedgerJournalValidator());
            _validCombService = new ValidCombService(new ValidCombRepository(), new ValidCombValidator());
            _currencyService = new CurrencyService(new CurrencyRepository(), new CurrencyValidator());
            _exchangeRateClosingService = new ExchangeRateClosingService(new ExchangeRateClosingRepository(), new ExchangeRateClosingValidator());
            _gLNonBaseCurrencyService = new GLNonBaseCurrencyService(new GLNonBaseCurrencyRepository(), new GLNonBaseCurrencyValidator());
            _vCNonBaseCurrencyService = new VCNonBaseCurrencyService(new VCNonBaseCurrencyRepository(), new VCNonBaseCurrencyValidator());
            _cashBankService = new CashBankService(new CashBankRepository(), new CashBankValidator());
        }

        public ActionResult Index()
        {
            if (!AuthenticationModel.IsAllowed("View", Core.Constants.Constant.MenuName.Closing, Core.Constants.Constant.MenuGroupName.Report))
            {
                return Content("You are not allowed to View this Page.");
            }

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
            var q = _closingService.GetQueryable().Include("Account");

            var query = (from model in q
                         select new
                         {
                            model.Id,
                            model.Period,
                            model.YearPeriod,
                            model.BeginningPeriod,
                            model.EndDatePeriod,
                            model.IsClosed,
                            model.IsYear,
                            model.ClosedAt
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
                            model.Period,
                            model.YearPeriod,
                            model.BeginningPeriod,
                            model.EndDatePeriod,
                            model.IsYear,
                            model.IsClosed,
                            model.ClosedAt
                      }
                    }).ToArray()
            }, JsonRequestBehavior.AllowGet);
        }

        public dynamic GetClosedList(string _search, long nd, int rows, int? page, string sidx, string sord, string filters = "")
        {
            // Construct where statement
            string strWhere = GeneralFunction.ConstructWhere(filters);
            string filter = null;
            GeneralFunction.ConstructWhereInLinq(strWhere, out filter);
            if (filter == "") filter = "true";

            // Get Data
            var q = _closingService.GetQueryable().Include("Account").Where(x => x.IsClosed);

            var query = (from model in q
                         select new
                         {
                             model.Id,
                             model.Period,
                             model.YearPeriod,
                             model.BeginningPeriod,
                             model.EndDatePeriod,
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
                            model.Period,
                            model.YearPeriod,
                            model.BeginningPeriod,
                            model.EndDatePeriod,
                      }
                    }).ToArray()
            }, JsonRequestBehavior.AllowGet);
        }

        public dynamic GetInfo(int Id)
        {
            Closing model = new Closing();
            try
            {
               
                model = _closingService.GetObjectById(Id);
            }
            catch (Exception ex)
            {
                LOG.Error("GetInfo", ex);
                model.Errors.Add("Generic", "Error : " + ex);
            }

            return Json(new
            {
                model.Id,
                model.Period,
                model.YearPeriod,
                model.BeginningPeriod,
                model.EndDatePeriod,
                model.IsYear,
                model.Errors, 
                exchangeRateClosings = ( 
                    from detail in model.ExchangeRateClosings
                    select new
                    {
                        detail.CurrencyId,
                        detail.Currency.Name,
                        detail.Rate
                    })
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public dynamic Insert(Closing model,IList<ExchangeRateClosing> exchangeRateClosing)
        {
            try
            {
                if (!AuthenticationModel.IsAllowed("Create", Core.Constants.Constant.MenuName.Closing, Core.Constants.Constant.MenuGroupName.Report))
                {
                    Dictionary<string, string> Errors = new Dictionary<string, string>();
                    Errors.Add("Generic", "You are Not Allowed to Confirm Record");

                    return Json(new
                    {
                        Errors
                    }, JsonRequestBehavior.AllowGet);
                }
                if (exchangeRateClosing == null) exchangeRateClosing = new List<ExchangeRateClosing>();
                model = _closingService.CreateObject(model,exchangeRateClosing, _accountService, _validCombService,_exchangeRateClosingService);
            }
            catch (Exception ex)
            {
                LOG.Error("Confirm Failed", ex);
                Dictionary<string, string> Errors = new Dictionary<string, string>();
                Errors.Add("Generic", "Error " + ex);

                return Json(new
                {
                    Errors
                }, JsonRequestBehavior.AllowGet);
            }

            return Json(new
            {
                model.Errors
            });
        }

        [HttpPost]
        public dynamic Close(Closing model)
        {
            try
            {
                if (!AuthenticationModel.IsAllowed("Confirm", Core.Constants.Constant.MenuName.Closing, Core.Constants.Constant.MenuGroupName.Report))
                {
                    Dictionary<string, string> Errors = new Dictionary<string, string>();
                    Errors.Add("Generic", "You are Not Allowed to Confirm Record");

                    return Json(new
                    {
                        Errors
                    }, JsonRequestBehavior.AllowGet);
                }

                var data = _closingService.GetObjectById(model.Id);
                data.ClosedAt = model.ClosedAt;
                model = _closingService.CloseObject(data, _accountService, _generalLedgerJournalService, _validCombService,_gLNonBaseCurrencyService,_exchangeRateClosingService,_vCNonBaseCurrencyService,_cashBankService);
            }
            catch (Exception ex)
            {
                LOG.Error("Confirm Failed", ex);
                Dictionary<string, string> Errors = new Dictionary<string, string>();
                Errors.Add("Generic", "Error " + ex);

                return Json(new
                {
                    Errors
                }, JsonRequestBehavior.AllowGet);
            }

            return Json(new
            {
                model.Errors
            });
        }

        [HttpPost]
        public dynamic Open(Closing model)
        {
            try
            {
                if (!AuthenticationModel.IsAllowed("Confirm", Core.Constants.Constant.MenuName.Closing, Core.Constants.Constant.MenuGroupName.Report))
                {
                    Dictionary<string, string> Errors = new Dictionary<string, string>();
                    Errors.Add("Generic", "You are Not Allowed to Confirm Record");

                    return Json(new
                    {
                        Errors
                    }, JsonRequestBehavior.AllowGet);
                }

                var data = _closingService.GetObjectById(model.Id);
                model = _closingService.OpenObject(data, _accountService, _validCombService,_vCNonBaseCurrencyService,_generalLedgerJournalService,_exchangeRateClosingService);
            }
            catch (Exception ex)
            {
                LOG.Error("Confirm Failed", ex);
                Dictionary<string, string> Errors = new Dictionary<string, string>();
                Errors.Add("Generic", "Error " + ex);

                return Json(new
                {
                    Errors
                }, JsonRequestBehavior.AllowGet);
            }

            return Json(new
            {
                model.Errors
            });
        }

        [HttpPost]
        public dynamic Delete(Closing model)
        {
            try
            {
                Dictionary<string, string> Errors = new Dictionary<string, string>();
                if (!AuthenticationModel.IsAllowed("Delete", Core.Constants.Constant.MenuName.Closing, Core.Constants.Constant.MenuGroupName.Report))
                {
                    Errors.Add("Generic", "You are Not Allowed to Confirm Record");
                    return Json(new
                    {
                        Errors
                    }, JsonRequestBehavior.AllowGet);
                }

                var data = _closingService.GetObjectById(model.Id);
                model = _closingService.DeleteObject(data, _accountService, _validCombService, _vCNonBaseCurrencyService, _generalLedgerJournalService);
                return Json(new
                {
                    model.Errors
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                LOG.Error("Confirm Failed", ex);
                Dictionary<string, string> Errors = new Dictionary<string, string>();
                Errors.Add("Generic", "Error " + ex);

                return Json(new
                {
                    Errors
                }, JsonRequestBehavior.AllowGet);
            }

            return Json(new
            {
                model.Errors
            });
        }
    }
}