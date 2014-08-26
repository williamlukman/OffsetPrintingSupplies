using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Web;
using System.Web.Mvc;
using Service.Service;
using Core.Interface.Service;
using Core.DomainModel;
using Data.Repository;
using Validation.Validation;

namespace WebView.Controllers
{
    public class PayableController : Controller
    {
        private readonly static log4net.ILog LOG = log4net.LogManager.GetLogger("PayableController");
        private IPayableService _payableService;
        private IContactService _contactService;

        public PayableController()
        {
            _payableService = new PayableService(new PayableRepository(), new PayableValidator());
            _contactService = new ContactService(new ContactRepository(), new ContactValidator());
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
            var query = _payableService.GetQueryable().Where(filter);

            var list = query as IEnumerable<Payable>;

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
                    from payable in list
                    select new
                    {
                        id = payable.Id,
                        cell = new object[] {
                            payable.Id,
                            payable.ContactId,
                            _contactService.GetObjectById(payable.ContactId).Name,
                            payable.Code,
                            payable.PayableSource,
                            payable.PayableSourceId,
                            payable.Amount,
                            payable.RemainingAmount,
                            payable.PendingClearanceAmount,
                            payable.AllowanceAmount,
                            payable.DueDate,
                            payable.IsCompleted,
                            payable.CompletionDate,
                            payable.CreatedAt,
                      }
                    }).ToArray()
            }, JsonRequestBehavior.AllowGet);
        }

        public dynamic GetListByDate(string _search, long nd, int rows, int? page, string sidx, string sord, DateTime startdate, DateTime enddate, string filters = "")
        {
            // Construct where statement

            string strWhere = GeneralFunction.ConstructWhere(filters);
            string filter = null;
            GeneralFunction.ConstructWhereInLinq(strWhere, out filter);
            if (filter == "") filter = "true";
            //DateTime dtStart = startdate;
            //DateTime dtEnd = enddate.AddDays(1);
            filter += " AND CreatedAt >= @0 AND CreatedAt < @1";

            // Get Data
            var query = _payableService.GetQueryable().Where(filter, startdate, enddate.AddDays(1) /*d => d.CreatedAt >= startdate && d.CreatedAt < enddate.AddDays(1) && d.IsDeleted == false*/);

            var list = query as IEnumerable<Payable>;

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
                    from payable in list
                    select new
                    {
                        id = payable.Id,
                        cell = new object[] {
                            payable.Id,
                            payable.ContactId,
                            _contactService.GetObjectById(payable.ContactId).Name,
                            payable.Code,
                            payable.PayableSource,
                            payable.PayableSourceId,
                            payable.Amount,
                            payable.RemainingAmount,
                            payable.PendingClearanceAmount,
                            payable.AllowanceAmount,
                            payable.DueDate,
                            payable.IsCompleted,
                            payable.CompletionDate,
                            payable.CreatedAt,
                      }
                    }).ToArray()
            }, JsonRequestBehavior.AllowGet);
        }
    }
}