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
using System.Data.Objects;
using System.Data.Objects.SqlClient;

namespace WebView.Controllers
{
    public class MstContactController : Controller
    {
        private readonly static log4net.ILog LOG = log4net.LogManager.GetLogger("ContactController");
        private IContactService _contactService;
        private IBlanketService _blanketService;
        private ICoreIdentificationService _coreIdentificationService;
        private IPurchaseOrderService _purchaseOrderService;
        private ISalesOrderService _salesOrderService;
        private ISalesQuotationService _salesQuotationService;
        private IVirtualOrderService _virtualOrderService;
        private IContactGroupService _contactGroupService;

        public MstContactController()
        {
            _contactService = new ContactService(new ContactRepository(), new ContactValidator());
            _coreIdentificationService = new CoreIdentificationService(new CoreIdentificationRepository(), new CoreIdentificationValidator());
            _blanketService = new BlanketService(new BlanketRepository(),new BlanketValidator());
            _purchaseOrderService = new PurchaseOrderService(new PurchaseOrderRepository(), new PurchaseOrderValidator());
            _salesOrderService = new SalesOrderService(new SalesOrderRepository(),new SalesOrderValidator());
            _salesQuotationService = new SalesQuotationService(new SalesQuotationRepository(), new SalesQuotationValidator());
            _virtualOrderService = new VirtualOrderService(new VirtualOrderRepository(), new VirtualOrderValidator());
            _contactGroupService = new ContactGroupService(new ContactGroupRepository(), new ContactGroupValidator());
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
            var q = _contactService.GetQueryable().Where(x => !x.IsDeleted);

            var query = (from model in q
                         select new
                         {
                             model.Id,
                             model.Name,
                             model.NamaFakturPajak,
                             model.Address,
                             model.DeliveryAddress,
                             //model.DefaultPaymentTerm,
                             model.Description,
                             model.ContactNo,
                             model.PIC,
                             model.PICContactNo,
                             model.Email,
                             model.TaxCode,
                             model.IsTaxable,
                             model.ContactGroupId,
                             ContactGroup = model.ContactGroup != null ? model.ContactGroup.Name : "",
                             model.ContactType,
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
                            model.Name,
                            model.NamaFakturPajak,
                            model.Address,
                            model.DeliveryAddress,
                            //model.DefaultPaymentTerm,
                            model.Description,
                            model.ContactNo,
                            model.PIC,
                            model.PICContactNo,
                            model.Email,
                            model.TaxCode,
                            model.IsTaxable,
                            model.ContactGroupId,
                            model.ContactGroup,
                            model.ContactType,
                            model.CreatedAt,
                            model.UpdatedAt,
                      }
                    }).ToArray()
            }, JsonRequestBehavior.AllowGet);
        }

        public dynamic GetShortList(string _search, long nd, int rows, int? page, string sidx, string sord, string filters = "")
        {
            // Construct where statement
            string strWhere = GeneralFunction.ConstructWhere(filters);
            string filter = null;
            GeneralFunction.ConstructWhereInLinq(strWhere, out filter);
            if (filter == "") filter = "true";

            // Get Data
            var q = _contactService.GetQueryable().Where(x => !x.IsDeleted);

            var query = (from model in q
                         select new
                         {
                             model.Id,
                             model.Name,
                             model.NamaFakturPajak,
                             model.ContactGroupId,
                             ContactGroup = model.ContactGroup != null ? model.ContactGroup.Name : "",
                             model.ContactType,
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
                            model.Name,
                            model.NamaFakturPajak,
                            model.ContactGroupId,
                            model.ContactGroup,
                            model.ContactType,
                      }
                    }).ToArray()
            }, JsonRequestBehavior.AllowGet);
        }

        public dynamic GetListCustomer(string _search, long nd, int rows, int? page, string sidx, string sord, string filters = "")
        {
            // Construct where statement
            string strWhere = GeneralFunction.ConstructWhere(filters);
            string filter = null;
            GeneralFunction.ConstructWhereInLinq(strWhere, out filter);
            if (filter == "") filter = "true";

            // Get Data
            var q = _contactService.GetQueryable().Where(x => !x.IsDeleted && x.ContactType.ToLower() != "supplier");

            var query = (from model in q
                         select new
                         {
                             model.Id,
                             model.Name,
                             model.NamaFakturPajak,
                             model.Address,
                             model.DeliveryAddress,
                             model.DefaultPaymentTerm,
                             model.Description,
                             model.NPWP,
                             model.ContactNo,
                             model.PIC,
                             model.PICContactNo,
                             model.Email,
                             model.TaxCode,
                             model.IsTaxable,
                             model.ContactGroupId,
                             ContactGroup = model.ContactGroup != null ? model.ContactGroup.Name : "",
                             model.ContactType,
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
                            model.Name,
                            model.NamaFakturPajak,
                            model.Address,
                            model.DeliveryAddress,
                            model.DefaultPaymentTerm,
                            model.Description,
                            model.NPWP,
                            model.ContactNo,
                            model.PIC,
                            model.PICContactNo,
                            model.Email,
                            model.TaxCode,
                            model.IsTaxable,
                            model.ContactGroupId,
                            model.ContactGroup,
                            model.ContactType,
                            model.CreatedAt,
                            model.UpdatedAt,
                      }
                    }).ToArray()
            }, JsonRequestBehavior.AllowGet);
        }

        public dynamic GetListSupplier(string _search, long nd, int rows, int? page, string sidx, string sord, string filters = "")
        {
            // Construct where statement
            string strWhere = GeneralFunction.ConstructWhere(filters);
            string filter = null;
            GeneralFunction.ConstructWhereInLinq(strWhere, out filter);
            if (filter == "") filter = "true";

            // Get Data
            var q = _contactService.GetQueryable().Where(x => !x.IsDeleted && x.ContactType.ToLower() != "customer");

            var query = (from model in q
                         select new
                         {
                             model.Id,
                             model.Name,
                             model.NamaFakturPajak,
                             model.Address,
                             model.DeliveryAddress,
                             model.Description,
                             model.NPWP,
                             model.ContactNo,
                             model.PIC,
                             model.PICContactNo,
                             model.Email,
                             model.TaxCode,
                             model.IsTaxable,
                             model.ContactGroupId,
                             ContactGroup = model.ContactGroup != null ? model.ContactGroup.Name : "",
                             model.ContactType,
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
                    from model in list
                    select new
                    {
                        id = model.Id,
                        cell = new object[] {
                            model.Id,
                            model.Name,
                            model.NamaFakturPajak,
                            model.Address,
                            model.DeliveryAddress,
                            model.Description,
                            model.NPWP,
                            model.ContactNo,
                            model.PIC,
                            model.PICContactNo,
                            model.Email,
                            model.TaxCode,
                            model.IsTaxable,
                            model.ContactGroupId,
                            model.ContactGroup,
                            model.ContactType,
                            model.CreatedAt,
                            model.UpdatedAt,
                      }
                    }).ToArray()
            }, JsonRequestBehavior.AllowGet);
        }

         public dynamic GetInfo(int Id)
         {
             Contact model = new Contact();
             try
             {
                 model = _contactService.GetObjectById(Id);
             }
             catch (Exception ex)
             {
                 LOG.Error("GetInfo", ex);
                 model.Errors.Add("Generic", "Error : " + ex);
             }

             var contactgrp = _contactGroupService.GetObjectById(model.ContactGroupId.GetValueOrDefault());

             return Json(new
             {
                 model.Id,
                 model.Name,
                 model.NamaFakturPajak,
                 model.Address,
                 model.DeliveryAddress,
                 model.DefaultPaymentTerm,
                 model.Description,
                 model.NPWP,
                 model.ContactNo,
                 model.PIC,
                 model.PICContactNo,
                 model.Email,
                 model.TaxCode,
                 model.IsTaxable,
                 model.ContactGroupId,
                 ContactGroup = contactgrp != null ? contactgrp.Name : "",
                 model.ContactType,
                 model.Errors
             }, JsonRequestBehavior.AllowGet);
         }

        [HttpPost]
        public dynamic Insert(Contact model)
        {
            try
            {
                model = _contactService.CreateObject(model);
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
        public dynamic Update(Contact model)
        {
            try
            {
                var data = _contactService.GetObjectById(model.Id);
                data.Name = model.Name;
                data.NamaFakturPajak = model.NamaFakturPajak;
                data.Address = model.Address;
                data.DeliveryAddress = model.DeliveryAddress;
                data.DefaultPaymentTerm = model.DefaultPaymentTerm;
                data.Description = model.Description;
                data.NPWP = model.NPWP;
                data.ContactNo = model.ContactNo;
                data.PIC = model.PIC;
                data.PICContactNo = model.PICContactNo;
                data.Email = model.Email;
                data.TaxCode = model.TaxCode;
                data.IsTaxable = model.IsTaxable;
                data.ContactGroupId = model.ContactGroupId;
                model = _contactService.UpdateObject(data);
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
        public dynamic Delete(Contact model)
        {
            try
            {
                var data = _contactService.GetObjectById(model.Id);
                model = _contactService.SoftDeleteObject(data, _coreIdentificationService, _blanketService, _purchaseOrderService,
                                                         _salesOrderService, _salesQuotationService, _virtualOrderService);
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
