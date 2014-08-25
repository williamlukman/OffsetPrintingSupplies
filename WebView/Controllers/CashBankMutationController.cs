﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Service.Service;
using Core.Interface.Service;
using Core.DomainModel;
using Data.Repository;
using Validation.Validation;

namespace WebView.Controllers
{
    public class CashBankMutationController : Controller
    {
        private readonly static log4net.ILog LOG = log4net.LogManager.GetLogger("CashBankMutationController");
        private ICashBankService _cashBankService;
        private ICashBankAdjustmentService _cashBankAdjustmentService;
        private ICashMutationService _cashMutationService;
        private ICashBankMutationService _cashBankMutationService;

        public CashBankMutationController()
        {
            _cashBankAdjustmentService = new CashBankAdjustmentService(new CashBankAdjustmentRepository(), new CashBankAdjustmentValidator());
            _cashBankService = new CashBankService(new CashBankRepository(), new CashBankValidator());
            _cashMutationService = new CashMutationService(new CashMutationRepository(), new CashMutationValidator());
            _cashBankMutationService = new CashBankMutationService(new CashBankMutationRepository(), new CashBankMutationValidator());
        }

        public ActionResult Index()
        {
            return View();
        }

        public dynamic GetList(string _search, long nd, int rows, int? page, string sidx, string sord, string filters = "")
        {
            // Construct where statement

            string strWhere = GeneralFunction.ConstructWhere(filters);

            // Get Data
            var query = _cashBankMutationService.GetAll().Where(d => d.IsDeleted == false);

            var list = query as IEnumerable<CashBankMutation>;

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
                            _cashBankService.GetObjectById(model.SourceCashBankId).Name,
                            _cashBankService.GetObjectById(model.TargetCashBankId).Name,
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
                model = _cashBankMutationService.ConfirmObject(data,model.ConfirmationDate.Value,_cashMutationService,_cashBankService);
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
                model = _cashBankMutationService.UnconfirmObject(data,_cashMutationService,_cashBankService);
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