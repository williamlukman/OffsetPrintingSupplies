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
    public class CashBankAdjustmentController : Controller
    {
        private readonly static log4net.ILog LOG = log4net.LogManager.GetLogger("CashBankAdjustmentController");
        private ICashBankService _cashBankService;
        private ICashBankAdjustmentService _cashBankAdjustmentService;
        private ICashMutationService _cashMutationService;
        private IAccountService _accountService;
        private IGeneralLedgerJournalService _generalLedgerJournalService;
        private IClosingService _closingService;

        public CashBankAdjustmentController()
        {
            _cashBankAdjustmentService = new CashBankAdjustmentService(new CashBankAdjustmentRepository(), new CashBankAdjustmentValidator());
            _cashBankService = new CashBankService(new CashBankRepository(), new CashBankValidator());
            _cashMutationService = new CashMutationService(new CashMutationRepository(), new CashMutationValidator());
            _accountService = new AccountService(new AccountRepository(), new AccountValidator());
            _generalLedgerJournalService = new GeneralLedgerJournalService(new GeneralLedgerJournalRepository(), new GeneralLedgerJournalValidator());
            _closingService = new ClosingService(new ClosingRepository(), new ClosingValidator());
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
            var q = _cashBankAdjustmentService.GetQueryable().Include("CashBank").Where(x => !x.IsDeleted);

            var query = (from model in q
                         select new
                         {
                             model.Id,
                             model.Code,
                             model.CashBankId,
                             CashBank = model.CashBank.Name,
                             model.Amount,
                             model.AdjustmentDate,
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
                            model.CashBank,
                            model.Amount,
                            model.AdjustmentDate,
                            model.ConfirmationDate,
                            model.CreatedAt,
                            model.UpdatedAt,
                      }
                    }).ToArray()
            }, JsonRequestBehavior.AllowGet);
        }

        public dynamic GetInfo(int Id)
        {
            CashBankAdjustment model = new CashBankAdjustment();
            try
            {
                model = _cashBankAdjustmentService.GetObjectById(Id);

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
                model.CashBankId,
                CashBank = _cashBankService.GetObjectById(model.CashBankId).Name,
                model.Amount,
                model.AdjustmentDate,
                model.Errors
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public dynamic Insert(CashBankAdjustment model)
        {
            try
            {
               model = _cashBankAdjustmentService.CreateObject(model, _cashBankService);
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
        public dynamic Update(CashBankAdjustment model)
        {
            try
            {
                var data = _cashBankAdjustmentService.GetObjectById(model.Id);
                data.CashBankId = model.CashBankId;
                data.AdjustmentDate = model.AdjustmentDate;
                data.Amount = model.Amount;
                model = _cashBankAdjustmentService.UpdateObject(data,_cashBankService);
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
        public dynamic Delete(CashBankAdjustment model)
        {
            try
            {
               var data = _cashBankAdjustmentService.GetObjectById(model.Id);
               model = _cashBankAdjustmentService.SoftDeleteObject(data);
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
        public dynamic Confirm(CashBankAdjustment model)
        {
            try
            {
                
                var data = _cashBankAdjustmentService.GetObjectById(model.Id);
                model = _cashBankAdjustmentService.ConfirmObject(data,model.ConfirmationDate.Value,_cashMutationService,_cashBankService,
                                                                 _accountService, _generalLedgerJournalService, _closingService);
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
        public dynamic UnConfirm(CashBankAdjustment model)
        {
            try
            {
                var data = _cashBankAdjustmentService.GetObjectById(model.Id);
                model = _cashBankAdjustmentService.UnconfirmObject(data,_cashMutationService,_cashBankService,_accountService, _generalLedgerJournalService, _closingService);
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
