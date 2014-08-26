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
    public class CashMutationController : Controller
    {
        private readonly static log4net.ILog LOG = log4net.LogManager.GetLogger("CashMutationController");
        private ICashMutationService _cashMutationService;
        private ICashBankService _cashBankService;

        public CashMutationController()
        {
            _cashMutationService = new CashMutationService(new CashMutationRepository(), new CashMutationValidator());
            _cashBankService = new CashBankService(new CashBankRepository(), new CashBankValidator());
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
            var query = _cashMutationService.GetAll().Where(d => d.IsDeleted == false);

            var list = query as IEnumerable<CashMutation>;

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
                    from cashmutation in list
                    select new
                    {
                        id = cashmutation.Id,
                        cell = new object[] {
                            cashmutation.Id,
                            cashmutation.CashBankId,
                            _cashBankService.GetObjectById(cashmutation.CashBankId).Name,
                            cashmutation.Status == Core.Constants.Constant.MutationStatus.Addition ? cashmutation.Amount : cashmutation.Amount * (-1),
                            cashmutation.SourceDocumentType,
                            cashmutation.SourceDocumentId,
                            cashmutation.CreatedAt,
                            cashmutation.MutationDate
                      }
                    }).ToArray()
            }, JsonRequestBehavior.AllowGet);
        }

        public dynamic GetListByDate(string _search, long nd, int rows, int? page, string sidx, string sord, DateTime startdate, DateTime enddate, string filters = "")
        {
            // Construct where statement

            string strWhere = GeneralFunction.ConstructWhere(filters);

            // Get Data
            var query = _cashMutationService.GetAll().Where(d => d.MutationDate >= startdate && d.MutationDate < enddate.AddDays(1) && d.IsDeleted == false);

            var list = query as IEnumerable<CashMutation>;

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
                    from cashmutation in list
                    select new
                    {
                        id = cashmutation.Id,
                        cell = new object[] {
                            cashmutation.Id,
                            cashmutation.CashBankId,
                            _cashBankService.GetObjectById(cashmutation.CashBankId).Name,
                            cashmutation.Status == Core.Constants.Constant.MutationStatus.Addition ? cashmutation.Amount : cashmutation.Amount * (-1),
                            cashmutation.SourceDocumentType,
                            cashmutation.SourceDocumentId,
                            cashmutation.CreatedAt,
                            cashmutation.MutationDate
                      }
                    }).ToArray()
            }, JsonRequestBehavior.AllowGet);
        }
    }
}