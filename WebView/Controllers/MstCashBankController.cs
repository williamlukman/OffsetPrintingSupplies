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

namespace WebView.Controllers
{
    public class MstCashBankController : Controller
    {
        private readonly static log4net.ILog LOG = log4net.LogManager.GetLogger("CashBankController");
        private ICashBankService _cashBankService;
        private ICashMutationService _cashMutationService;
        public MstCashBankController()
        {
            _cashBankService = new CashBankService(new CashBankRepository(),new CashBankValidator());
            _cashMutationService = new CashMutationService(new CashMutationRepository(), new CashMutationValidator());
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
            var query = _cashBankService.GetAll().Where(d => d.IsDeleted == false);

            var list = query as IEnumerable<CashBank>;

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
                    from item in list
                    select new
                    {
                        id = item.Id,
                        cell = new object[] {
                            item.Id,
                            item.Name,
                            item.Description,
                            item.Amount,
                            item.CreatedAt,
                            item.UpdatedAt,
                      }
                    }).ToArray()
            }, JsonRequestBehavior.AllowGet);
        }

        public dynamic GetInfo(int Id)
        {
            CashBank model = new CashBank();
            try
            {
                model = _cashBankService.GetObjectById(Id);

            }
            catch (Exception ex)
            {
                LOG.Error("GetInfo", ex);
                model.Errors.Add("Generic", "Error" + ex);
            }

            return Json(new
            {
                model
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public dynamic Insert(CashBank model)
        {
            try
            {
                model = _cashBankService.CreateObject(model);
            }
            catch (Exception ex)
            {
                LOG.Error("Insert Failed", ex);
                model.Errors.Add("Insert Failed", "Error : " + ex);
            }

            return Json(new
            {
                model.Errors
            });
        }

        [HttpPost]
        public dynamic Update(CashBank model)
        {
            try
            {
                var data = _cashBankService.GetObjectById(model.Id);
                data.Name = model.Name;
                data.Description = model.Description;
                model = _cashBankService.UpdateObject(data);
            }
            catch (Exception ex)
            {
                LOG.Error("Update Failed", ex);
                model.Errors.Add("Update Failed", "Error : " + ex);
            }

            return Json(new
            {
                model.Errors
            });
        }

        [HttpPost]
        public dynamic Delete(CashBank model)
        {
            try
            {
                var data = _cashBankService.GetObjectById(model.Id);
                model = _cashBankService.SoftDeleteObject(data,_cashMutationService);
            }
            catch (Exception ex)
            {
                LOG.Error("Delete Failed", ex);
                model.Errors.Add("Delete Failed", "Error : " + ex);
            }

            return Json(new
            {
                model.Errors
            });
        }
    }
}
