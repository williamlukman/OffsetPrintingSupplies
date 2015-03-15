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
    public class BankAdministrationController : Controller
    {
        private readonly static log4net.ILog LOG = log4net.LogManager.GetLogger("BankAdministrationController");
        private ICashBankService _cashBankService;
        private IBankAdministrationService _bankAdministrationService;
        private IBankAdministrationDetailService _bankAdministrationDetailService;
        private ICashMutationService _cashMutationService;
        private IAccountService _accountService;
        private IGeneralLedgerJournalService _generalLedgerJournalService;
        private IClosingService _closingService;
        private ICurrencyService _currencyService;
        private IExchangeRateService _exchangeRateService;
        private IGLNonBaseCurrencyService _gLNonBaseCurrencyService;

        public BankAdministrationController()
        {
            _bankAdministrationService = new BankAdministrationService(new BankAdministrationRepository(), new BankAdministrationValidator());
            _bankAdministrationDetailService = new BankAdministrationDetailService(new BankAdministrationDetailRepository(), new BankAdministrationDetailValidator());
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
            var q = _bankAdministrationService.GetQueryable().Include("CashBank").Where(x => !x.IsDeleted);

            var query = (from model in q
                         select new
                         {
                             model.Id,
                             model.Code,
                             model.NoBukti,
                             model.CashBankId,
                             CashBank = model.CashBank.Name,
                             model.ExchangeRateAmount,
                             model.Amount,
                             //model.BiayaAdminAmount,
                             //model.BiayaBungaAmount,
                             //model.PendapatanJasaAmount,
                             //model.PendapatanBungaAmount,
                             //model.PengembalianPiutangAmount,
                             model.Description,
                             model.AdministrationDate,
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
                            model.ExchangeRateAmount,
                            model.Amount,
                            //model.BiayaAdminAmount,
                            //model.BiayaBungaAmount,
                            //model.PendapatanJasaAmount,
                            //model.PendapatanBungaAmount,
                            //model.PengembalianPiutangAmount,
                            model.Description,
                            model.AdministrationDate,
                            model.ConfirmationDate,
                            model.CreatedAt,
                            model.UpdatedAt,
                      }
                    }).ToArray()
            }, JsonRequestBehavior.AllowGet);
        }

        public dynamic GetInfo(int Id)
        {
            BankAdministration model = new BankAdministration();
            try
            {
                model = _bankAdministrationService.GetObjectById(Id);

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
                model.ExchangeRateAmount,
                model.Amount,
                //model.BiayaAdminAmount,
                //model.BiayaBungaAmount,
                //model.PendapatanJasaAmount,
                //model.PendapatanBungaAmount,
                //model.PengembalianPiutangAmount,
                model.Description,
                //model.IsExpense,
                model.AdministrationDate,
                model.ConfirmationDate,
                model.Errors
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public dynamic Insert(BankAdministration model)
        {
            try
            {
               model = _bankAdministrationService.CreateObject(model, _cashBankService, _currencyService, _exchangeRateService);
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
        public dynamic Update(BankAdministration model)
        {
            try
            {
                var data = _bankAdministrationService.GetObjectById(model.Id);
                data.CashBankId = model.CashBankId;
                data.AdministrationDate = model.AdministrationDate;
                data.NoBukti = model.NoBukti;
                data.ExchangeRateAmount = model.ExchangeRateAmount;
                //data.BiayaAdminAmount = model.BiayaAdminAmount;
                //data.BiayaBungaAmount = model.BiayaBungaAmount;
                //data.PendapatanJasaAmount = model.PendapatanJasaAmount;
                //data.PendapatanBungaAmount = model.PendapatanBungaAmount;
                //data.PengembalianPiutangAmount = model.PengembalianPiutangAmount;
                data.Description = model.Description;
                //data.IsExpense = model.IsExpense;
                model = _bankAdministrationService.UpdateObject(data,_cashBankService, _currencyService, _exchangeRateService);
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
        public dynamic Delete(BankAdministration model)
        {
            try
            {
               var data = _bankAdministrationService.GetObjectById(model.Id);
               model = _bankAdministrationService.SoftDeleteObject(data);
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
        public dynamic Confirm(BankAdministration model)
        {
            try
            {
                
                var data = _bankAdministrationService.GetObjectById(model.Id);
                model = _bankAdministrationService.ConfirmObject(data,model.ConfirmationDate.Value,_cashMutationService,_cashBankService,_accountService
                    ,_generalLedgerJournalService,_closingService,_currencyService,_exchangeRateService,_gLNonBaseCurrencyService, _bankAdministrationDetailService);
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
        public dynamic UnConfirm(BankAdministration model)
        {
            try
            {
                var data = _bankAdministrationService.GetObjectById(model.Id);
                model = _bankAdministrationService.UnconfirmObject(data,_cashMutationService,_cashBankService,
                            _accountService, _generalLedgerJournalService, _closingService, _currencyService, _gLNonBaseCurrencyService, _bankAdministrationDetailService);
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

        public dynamic GetListDetail(string _search, long nd, int rows, int? page, string sidx, string sord, int id, string filters = "")
        {
            // Construct where statement
            string strWhere = GeneralFunction.ConstructWhere(filters);
            string filter = null;
            GeneralFunction.ConstructWhereInLinq(strWhere, out filter);
            if (filter == "") filter = "true";

            // Get Data
            var q = _bankAdministrationDetailService.GetQueryable().Include("BankAdministration").Include("Account")
                                                .Where(x => x.BankAdministrationId == id && !x.IsDeleted);

            var query = (from model in q
                         select new
                         {
                             model.Id,
                             model.Code,
                             model.AccountId,
                             AccountCode = model.Account.Code,
                             Account = model.Account.Name,
                             model.Status,
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
                            model.AccountId,
                            model.AccountCode,
                            model.Account,
                            model.Status,
                            model.Amount
                      }
                    }).ToArray()
            }, JsonRequestBehavior.AllowGet);
        }

        public dynamic GetInfoDetail(int Id)
        {
            BankAdministrationDetail model = new BankAdministrationDetail();
            try
            {
                model = _bankAdministrationDetailService.GetObjectById(Id);
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
                model.AccountId,
                AccountCode = model.Account.Code,
                Account = model.Account.Name,
                model.Status,
                model.Amount,
                model.Errors
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public dynamic InsertDetail(BankAdministrationDetail model)
        {
            decimal totalAmount = 0;
            try
            {
                model = _bankAdministrationDetailService.CreateObject(model, _bankAdministrationService, _accountService);
                totalAmount = _bankAdministrationService.GetObjectById(model.BankAdministrationId).Amount;
            }
            catch (Exception ex)
            {
                LOG.Error("Insert Failed", ex);
                model.Errors.Add("Generic", "Error : " + ex);

            }

            return Json(new
            {
                model.Errors,
                totalAmount
            });
        }

        [HttpPost]
        public dynamic UpdateDetail(BankAdministrationDetail model)
        {
            decimal totalAmount = 0;
            try
            {
                var data = _bankAdministrationDetailService.GetObjectById(model.Id);
                data.AccountId = model.AccountId;
                data.Status = model.Status;
                data.Amount = model.Amount;
                model = _bankAdministrationDetailService.UpdateObject(data, _bankAdministrationService, _accountService);
                totalAmount = _bankAdministrationService.GetObjectById(model.BankAdministrationId).Amount;
            }
            catch (Exception ex)
            {
                LOG.Error("Update Failed", ex);
                model.Errors.Add("Generic", "Error : " + ex);
            }

            return Json(new
            {
                model.Errors,
                totalAmount
            });
        }

        [HttpPost]
        public dynamic DeleteDetail(BankAdministrationDetail model)
        {
            decimal totalAmount = 0;
            try
            {
                var data = _bankAdministrationDetailService.GetObjectById(model.Id);
                model = _bankAdministrationDetailService.SoftDeleteObject(data, _bankAdministrationService);
                totalAmount = _bankAdministrationService.GetObjectById(data.BankAdministrationId).Amount;
            }
            catch (Exception ex)
            {
                LOG.Error("Delete Failed", ex);
                model.Errors.Add("Generic", "Error : " + ex);
            }

            return Json(new
            {
                model.Errors,
                totalAmount
            });
        }

    }
}
