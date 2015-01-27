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
using Core.Constants;

namespace WebView.Controllers
{
    public class PayableController : Controller
    {
        private readonly static log4net.ILog LOG = log4net.LogManager.GetLogger("PayableController");
        private IPayableService _payableService;
        private IContactService _contactService;
        private IPriceMutationService _priceMutationService;
        private IPaymentRequestService _paymentRequestService;
        private IPurchaseDownPaymentService _purchaseDownPaymentService;
        private IPurchaseInvoiceMigrationService _purchaseInvoiceMigrationService;

        public PayableController()
        {
            _payableService = new PayableService(new PayableRepository(), new PayableValidator());
            _contactService = new ContactService(new ContactRepository(), new ContactValidator());
            _priceMutationService = new PriceMutationService(new PriceMutationRepository(), new PriceMutationValidator());
            _paymentRequestService = new PaymentRequestService(new PaymentRequestRepository(), new PaymentRequestValidator());
            _purchaseDownPaymentService = new PurchaseDownPaymentService(new PurchaseDownPaymentRepository(), new PurchaseDownPaymentValidator());
            _purchaseInvoiceMigrationService = new PurchaseInvoiceMigrationService(new PurchaseInvoiceMigrationRepository());
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

           var q = _payableService.GetQueryable().Include("Contact").Where(x => !x.IsDeleted);
           var query = (from model in q
                         select new
                         {
                             model.Id,
                             model.Code,
                             model.ContactId,
                             Contact = model.Contact.Name,
                             model.PayableSource,
                             model.PayableSourceId,
                             model.Amount,
                             model.RemainingAmount,
                             model.PendingClearanceAmount,
                             Currency = model.Currency.Name,
                             model.Rate,
                             model.DueDate,
                             model.CompletionDate,
                             model.CreatedAt,
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
                    from payable in list
                    select new
                    {
                        id = payable.Id,
                        cell = new object[] {
                            payable.Id,
                            payable.Code,
                            payable.ContactId,
                            payable.Contact,
                            payable.PayableSource,
                            payable.PayableSourceId,
                            payable.Amount,
                            payable.RemainingAmount,
                            payable.PendingClearanceAmount,
                            payable.Currency,
                            payable.Rate,
                            payable.DueDate,
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

            // Get Data
            var q = _payableService.GetQueryable().Include("Contact")
                                   .Where(x => !x.IsDeleted && x.CreatedAt >= startdate && x.CreatedAt < enddate.AddDays(1));

            var query = (from model in q
                         select new
                         {
                             model.Id,
                             model.Code,
                             model.ContactId,
                             Contact = model.Contact.Name,
                             model.PayableSource,
                             model.PayableSourceId,
                             model.Amount,
                             model.RemainingAmount,
                             model.PendingClearanceAmount,
                             Currency = model.Currency.Name,
                             model.Rate,
                             model.DueDate,
                             model.CompletionDate,
                             model.CreatedAt,
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
                    from payable in list
                    select new
                    {
                        id = payable.Id,
                        cell = new object[] {
                            payable.Id,
                            payable.Code,
                            payable.ContactId,
                            payable.Contact,
                            payable.PayableSource,
                            payable.PayableSourceId,
                            payable.Amount,
                            payable.RemainingAmount,
                            payable.PendingClearanceAmount,
                            payable.Currency,
                            payable.Rate,
                            payable.DueDate,
                            payable.CompletionDate,
                            payable.CreatedAt,
                      }
                    }).ToArray()
            }, JsonRequestBehavior.AllowGet);
        }

        public dynamic GetListSalesDownPayment(string _search, long nd, int rows, int? page, string sidx, string sord, string filters = "")
        {
            // Construct where statement
            string strWhere = GeneralFunction.ConstructWhere(filters);
            string filter = null;
            GeneralFunction.ConstructWhereInLinq(strWhere, out filter);
            if (filter == "") filter = "true";

            // Get Data
            var q = _payableService.GetQueryable().Include("Contact").Where(x => x.PayableSource == Constant.PayableSource.SalesDownPayment && !x.IsDeleted);

            var query = (from model in q
                         select new
                         {
                             model.Id,
                             model.Code,
                             model.ContactId,
                             Contact = model.Contact.Name,
                             model.PayableSource,
                             model.PayableSourceId,
                             model.DueDate,
                             model.Amount,
                             model.RemainingAmount,
                             model.PendingClearanceAmount,
                             Currency = model.Currency.Name,
                             model.Rate,
                             model.CompletionDate,
                             model.CreatedAt,
                             model.UpdatedAt
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
                    from payable in list
                    select new
                    {
                        id = payable.Id,
                        cell = new object[] {
                            payable.Code,
                            payable.ContactId,
                            payable.Contact,
                            payable.PayableSource,
                            payable.PayableSourceId,
                            payable.DueDate,
                            payable.Amount,
                            payable.RemainingAmount,
                            payable.PendingClearanceAmount,
                            payable.Currency,
                            payable.Rate,
                            payable.CompletionDate,
                            payable.CreatedAt,
                            payable.UpdatedAt,
                        }
                    }).ToArray()
            }, JsonRequestBehavior.AllowGet);
        }

        public dynamic GetListPurchaseDownPayment(string _search, long nd, int rows, int? page, string sidx, string sord, string filters = "")
        {
            // Construct where statement
            string strWhere = GeneralFunction.ConstructWhere(filters);
            string filter = null;
            GeneralFunction.ConstructWhereInLinq(strWhere, out filter);
            if (filter == "") filter = "true";

            // Get Data
            var q = _payableService.GetQueryable().Include("Contact").Where(x => x.PayableSource == Constant.PayableSource.PurchaseDownPayment && !x.IsDeleted);

            var query = (from model in q
                         select new
                         {
                             model.Id,
                             model.Code,
                             model.ContactId,
                             Contact = model.Contact.Name,
                             model.PayableSource,
                             model.PayableSourceId,
                             model.Amount,
                             model.RemainingAmount,
                             model.PendingClearanceAmount,
                             Currency = model.Currency.Name,
                             model.Rate,
                             model.DueDate,
                             model.CompletionDate,
                             model.CreatedAt,
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
                    from payable in list
                    select new
                    {
                        id = payable.Id,
                        cell = new object[] {
                            payable.Id,
                            payable.Code,
                            payable.ContactId,
                            payable.Contact,
                            payable.PayableSource,
                            payable.PayableSourceId,
                            payable.Amount,
                            payable.RemainingAmount,
                            payable.PendingClearanceAmount,
                            payable.Currency,
                            payable.Rate,
                            payable.DueDate,
                            payable.CompletionDate,
                            payable.CreatedAt,
                      }
                    }).ToArray()
            }, JsonRequestBehavior.AllowGet);
        }

        public dynamic GetListPurchaseInvoice(string _search, long nd, int rows, int? page, string sidx, string sord, string filters = "")
        {
            // Construct where statement
            string strWhere = GeneralFunction.ConstructWhere(filters);
            string filter = null;
            GeneralFunction.ConstructWhereInLinq(strWhere, out filter);
            if (filter == "") filter = "true";

            // Get Data
            var q = _payableService.GetQueryable().Include("Contact").Where(x => x.PayableSource == Constant.PayableSource.PurchaseInvoice && !x.IsDeleted);

            var query = (from model in q
                         select new
                         {
                             model.Id,
                             model.Code,
                             model.ContactId,
                             Contact = model.Contact.Name,
                             model.PayableSource,
                             model.PayableSourceId,
                             model.Amount,
                             model.RemainingAmount,
                             model.PendingClearanceAmount,
                             Currency = model.Currency.Name,
                             model.Rate,
                             model.DueDate,
                             model.CompletionDate,
                             model.CreatedAt,
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
                    from payable in list
                    select new
                    {
                        id = payable.Id,
                        cell = new object[] {
                            payable.Id,
                            payable.Code,
                            payable.ContactId,
                            payable.Contact,
                            payable.PayableSource,
                            payable.PayableSourceId,
                            payable.Amount,
                            payable.RemainingAmount,
                            payable.PendingClearanceAmount,
                            payable.Currency,
                            payable.Rate,
                            payable.DueDate,
                            payable.CompletionDate,
                            payable.CreatedAt,
                      }
                    }).ToArray()
            }, JsonRequestBehavior.AllowGet);
        }
    }
}