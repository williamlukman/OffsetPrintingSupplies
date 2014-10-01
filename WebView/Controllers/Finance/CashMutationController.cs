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
using System.Linq.Dynamic;
using System.Data.Entity;

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
            string filter = null;
            GeneralFunction.ConstructWhereInLinq(strWhere, out filter);
            if (filter == "") filter = "true";

            // Get Data
            var q = _cashMutationService.GetQueryable().Include("CashBank").Where(x => !x.IsDeleted);

            var query = (from model in q
                         select new
                         {
                            model.Id,
                            model.CashBankId,
                            CashBank = model.CashBank.Name,
                            RealAmount = (model.Status == Core.Constants.Constant.MutationStatus.Addition) ? model.Amount : model.Amount * (-1),
                            model.SourceDocumentType,
                            model.SourceDocumentId,
                            model.CreatedAt,
                            model.MutationDate
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
                            model.CashBankId,
                            model.CashBank,
                            model.RealAmount,
                            model.SourceDocumentType,
                            model.SourceDocumentId,
                            model.CreatedAt,
                            model.MutationDate
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
            var q = _cashMutationService.GetQueryable().Include("CashBank").Where(x => x.MutationDate >= startdate && x.MutationDate < enddate.AddDays(1) && !x.IsDeleted);

            var query = (from model in q
                         select new
                         {
                            model.Id,
                            model.CashBankId,
                            CashBank = model.CashBank.Name,
                            RealAmount = (model.Status == Core.Constants.Constant.MutationStatus.Addition) ? model.Amount : model.Amount * (-1),
                            model.SourceDocumentType,
                            model.SourceDocumentId,
                            model.CreatedAt,
                            model.MutationDate
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
                            model.CashBankId,
                            model.CashBank,
                            model.RealAmount,
                            model.SourceDocumentType,
                            model.SourceDocumentId,
                            model.CreatedAt,
                            model.MutationDate
                      }
                    }).ToArray()
            }, JsonRequestBehavior.AllowGet);
        }
    }
}