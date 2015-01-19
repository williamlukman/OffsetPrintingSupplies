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
    public class SupplierDetailController : Controller
    {
        private readonly static log4net.ILog LOG = log4net.LogManager.GetLogger("SupplierDetailController");
        private IContactDetailService _contactDetailService;
        private IContactService _contactService;

        public SupplierDetailController()
        {
            _contactDetailService = new ContactDetailService(new ContactDetailRepository(), new ContactDetailValidator());
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
            var q = _contactDetailService.GetQueryable().Where(x => !x.IsDeleted && x.Contact.ContactType == "Supplier");

            var query = (from model in q
                         select new
                         {
                             model.Id,
                             model.ContactId,
                             Contact = model.Contact.Name,
                             model.Name,
                             model.ContactNo,
                             model.Address,
                             model.Email,
                             model.Description,
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
                            model.ContactId,
                            model.Contact,
                            model.Name,
                            model.ContactNo,
                            model.Email,
                            model.Address,
                            model.Description,
                            model.CreatedAt,
                            model.UpdatedAt,
                      }
                    }).ToArray()
            }, JsonRequestBehavior.AllowGet);
        }

         public dynamic GetInfo(int Id)
         {
             ContactDetail model = new ContactDetail();
             try
             {
                 model = _contactDetailService.GetObjectById(Id);
             }
             catch (Exception ex)
             {
                 LOG.Error("GetInfo", ex);
                 model.Errors.Add("Generic", "Error : " + ex);
             }

             return Json(new
             {
                 model.Id,
                 model.ContactId,
                 Contact = _contactService.GetObjectById(model.ContactId).Name,
                 model.Name,
                 model.ContactNo,
                 model.Email,
                 model.Address,
                 model.Description,
                 model.Errors
             }, JsonRequestBehavior.AllowGet);
         }

        [HttpPost]
        public dynamic Insert(ContactDetail model)
        {
            try
            {
                model = _contactDetailService.CreateObject(model, _contactService);
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
        public dynamic Update(ContactDetail model)
        {
            try
            {
                var data = _contactDetailService.GetObjectById(model.Id);
                data.Name = model.Name;
                data.ContactId = model.ContactId;
                data.ContactNo = model.ContactNo;
                data.Address = model.Address;
                data.Description = model.Description;
                data.Email = model.Email;
                model = _contactDetailService.UpdateObject(data, _contactService);
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
        public dynamic Delete(ContactDetail model)
        {
            try
            {
                var data = _contactDetailService.GetObjectById(model.Id);
                model = _contactDetailService.SoftDeleteObject(data);
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
