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
using Data.Context;

namespace WebView.Controllers
{
    public class ReceivableController : Controller
    {
        private readonly static log4net.ILog LOG = log4net.LogManager.GetLogger("ReceivableController");
        private IReceivableService _receivableService;
        private IContactService _contactService;
        private IPriceMutationService _priceMutationService;

        public ReceivableController()
        {
            _receivableService = new ReceivableService(new ReceivableRepository(), new ReceivableValidator());
            _contactService = new ContactService(new ContactRepository(), new ContactValidator());
            _priceMutationService = new PriceMutationService(new PriceMutationRepository(), new PriceMutationValidator());
        }

        public ActionResult Index()
        {
            return View();
        }

        /*
        public dynamic GetSum()
        {
            "SELECT SUM(Amount) FROM Receivable WHERE CurrencyId = 1 GROUP BY ContactId HAVING ContactId = 255"
        }
        */

        public dynamic GetList(string _search, long nd, int rows, int? page, string sidx, string sord, string filters = "")
        {
            // Construct where statement
            string strWhere = GeneralFunction.ConstructWhere(filters);
            string filter = null;
            GeneralFunction.ConstructWhereInLinq(strWhere, out filter);
            if (filter == "") filter = "true";

            using (var db = new OffsetPrintingSuppliesEntities())
            {
                // Get Data
                var q = db.Receivables.Include("Contact").Include("Currency").Where(x => !x.IsDeleted);

                var query = (from model in q
                             select new
                             {
                                 model.Id,
                                 model.Code,
                                 NomorSurat = (model.ReceivableSource == Constant.ReceivableSource.SalesInvoice) ? db.SalesInvoices.Where(x => x.Id == model.ReceivableSourceId).FirstOrDefault().NomorSurat :
                                                    (model.ReceivableSource == Constant.ReceivableSource.SalesInvoiceMigration) ? db.SalesInvoiceMigrations.Where(x => x.Id == model.ReceivableSourceId).FirstOrDefault().NomorSurat : "",
                                 model.ContactId,
                                 Contact = model.Contact.Name,
                                 model.ReceivableSource,
                                 model.ReceivableSourceId,
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
                        from receivable in list
                        select new
                        {
                            id = receivable.Id,
                            cell = new object[] {
                            receivable.Id,
                            receivable.Code,
                            receivable.NomorSurat,
                            receivable.ContactId,
                            receivable.Contact,
                            receivable.ReceivableSource,
                            receivable.ReceivableSourceId,
                            receivable.Amount,
                            receivable.RemainingAmount,
                            receivable.PendingClearanceAmount,
                            receivable.Currency,
                            receivable.Rate,
                            receivable.DueDate,
                            receivable.CompletionDate,
                            receivable.CreatedAt,
                      }
                        }).ToArray()
                }, JsonRequestBehavior.AllowGet);
            }
        }

        public dynamic GetListByDate(string _search, long nd, int rows, int? page, string sidx, string sord, DateTime startdate, DateTime enddate, string filters = "")
        {
            DateTime endDay = enddate.AddDays(1);
            // Construct where statement
            string strWhere = GeneralFunction.ConstructWhere(filters);
            string filter = null;
            GeneralFunction.ConstructWhereInLinq(strWhere, out filter);
            if (filter == "") filter = "true";

            using (var db = new OffsetPrintingSuppliesEntities())
            {
                // Get Data
                var q = db.Receivables.Include("Contact")
                                          .Where(x => x.CreatedAt >= startdate && x.CreatedAt < endDay && !x.IsDeleted);

                var query = (from model in q
                             select new
                             {
                                 model.Id,
                                 model.Code,
                                 NomorSurat = (model.ReceivableSource == Constant.ReceivableSource.SalesInvoice) ? db.SalesInvoices.Where(x => x.Id == model.ReceivableSourceId).FirstOrDefault().NomorSurat :
                                                        (model.ReceivableSource == Constant.ReceivableSource.SalesInvoiceMigration) ? db.SalesInvoiceMigrations.Where(x => x.Id == model.ReceivableSourceId).FirstOrDefault().NomorSurat : "",
                                 model.ContactId,
                                 Contact = model.Contact.Name,
                                 model.ReceivableSource,
                                 model.ReceivableSourceId,
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
                        from receivable in list
                        select new
                        {
                            id = receivable.Id,
                            cell = new object[] {
                            receivable.Id,
                            receivable.Code,
                            receivable.NomorSurat,
                            receivable.ContactId,
                            receivable.Contact,
                            receivable.ReceivableSource,
                            receivable.ReceivableSourceId,
                            receivable.Amount,
                            receivable.RemainingAmount,
                            receivable.PendingClearanceAmount,
                            receivable.Currency,
                            receivable.Rate,
                            receivable.DueDate,
                            receivable.CompletionDate,
                            receivable.CreatedAt,
                      }
                        }).ToArray()
                }, JsonRequestBehavior.AllowGet);
            }
        }

        public dynamic GetListPurchaseDownPayment(string _search, long nd, int rows, int? page, string sidx, string sord, string filters = "")
        {
            // Construct where statement
            string strWhere = GeneralFunction.ConstructWhere(filters);
            string filter = null;
            GeneralFunction.ConstructWhereInLinq(strWhere, out filter);
            if (filter == "") filter = "true";

            // Get Data
            var q = _receivableService.GetQueryable().Include("Contact").Where(x => x.ReceivableSource == Constant.ReceivableSource.PurchaseDownPayment && !x.IsDeleted);

            var query = (from model in q
                         select new
                         {
                             model.Id,
                             model.Code,
                             model.ContactId,
                             Contact = model.Contact.Name,
                             model.ReceivableSource,
                             model.ReceivableSourceId,
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
                    from receivable in list
                    select new
                    {
                        id = receivable.Id,
                        cell = new object[] {
                            receivable.Id,
                            receivable.Code,
                            receivable.ContactId,
                            receivable.Contact,
                            receivable.ReceivableSource,
                            receivable.ReceivableSourceId,
                            receivable.Amount,
                            receivable.RemainingAmount,
                            receivable.PendingClearanceAmount,
                            receivable.Currency,
                            receivable.Rate,
                            receivable.DueDate,
                            receivable.CompletionDate,
                            receivable.CreatedAt,
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
            var q = _receivableService.GetQueryable().Include("Contact").Where(x => x.ReceivableSource == Constant.ReceivableSource.SalesDownPayment && !x.IsDeleted);

            var query = (from model in q
                         select new
                         {
                             model.Id,
                             model.Code,
                             model.ContactId,
                             Contact = model.Contact.Name,
                             model.ReceivableSource,
                             model.ReceivableSourceId,
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
                    from receivable in list
                    select new
                    {
                        id = receivable.Id,
                        cell = new object[] {
                            receivable.Id,
                            receivable.Code,
                            receivable.ContactId,
                            receivable.Contact,
                            receivable.ReceivableSource,
                            receivable.ReceivableSourceId,
                            receivable.Amount,
                            receivable.RemainingAmount,
                            receivable.PendingClearanceAmount,
                            receivable.Currency,
                            receivable.Rate,
                            receivable.DueDate,
                            receivable.CompletionDate,
                            receivable.CreatedAt,
                      }
                    }).ToArray()
            }, JsonRequestBehavior.AllowGet);
        }

        public dynamic GetListSalesInvoice(string _search, long nd, int rows, int? page, string sidx, string sord, string filters = "")
        {
            // Construct where statement
            string strWhere = GeneralFunction.ConstructWhere(filters);
            string filter = null;
            GeneralFunction.ConstructWhereInLinq(strWhere, out filter);
            if (filter == "") filter = "true";

            // Get Data
            var q = _receivableService.GetQueryable().Include("Contact").Where(x => x.ReceivableSource == Constant.ReceivableSource.SalesInvoice && !x.IsDeleted);

            var query = (from model in q
                         select new
                         {
                             model.Id,
                             model.Code,
                             model.ContactId,
                             Contact = model.Contact.Name,
                             model.ReceivableSource,
                             model.ReceivableSourceId,
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
                    from model in list
                    select new
                    {
                        id = model.Id,
                        cell = new object[] {
                            model.Id,
                            model.Code,
                            model.ContactId,
                            model.Contact,
                            model.ReceivableSource,
                            model.ReceivableSourceId,
                            model.Amount,
                            model.RemainingAmount,
                            model.PendingClearanceAmount,
                            model.Currency,
                            model.Rate,
                            model.DueDate,
                            model.CompletionDate,
                            model.CreatedAt,
                      }
                    }).ToArray()
            }, JsonRequestBehavior.AllowGet);
        }
    }
}