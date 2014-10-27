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
    public class MstContactController : Controller
    {
        private readonly static log4net.ILog LOG = log4net.LogManager.GetLogger("ContactController");
        private IContactService _contactService;
        private IBlanketService _blanketService;
        private ICoreIdentificationService _coreIdentificationService;
        private IPurchaseOrderService _purchaseOrderService;
        private ISalesOrderService _salesOrderService;
        private IContactGroupService _contactGroupService;
        private ISalesQuotationService _salesQuotationService;
        private IVirtualOrderService _virtualOrderService;

        public MstContactController()
        {
            _contactService = new ContactService(new ContactRepository(), new ContactValidator());
            _coreIdentificationService = new CoreIdentificationService(new CoreIdentificationRepository(), new CoreIdentificationValidator());
            _blanketService = new BlanketService(new BlanketRepository(),new BlanketValidator());
            _purchaseOrderService = new PurchaseOrderService(new PurchaseOrderRepository(), new PurchaseOrderValidator());
            _salesOrderService = new SalesOrderService(new SalesOrderRepository(),new SalesOrderValidator());
            _contactGroupService = new ContactGroupService(new ContactGroupRepository(), new ContactGroupValidator());
            _virtualOrderService = new VirtualOrderService(new VirtualOrderRepository(), new VirtualOrderValidator());
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
            var q = _contactService.GetQueryable().Include("ContactGroup").Where(x => !x.IsDeleted);

            var query = (from model in q
                         select new
                         {
                             model.Id,
                             model.Name,
                             model.Address,
                             model.ContactNo,
                             model.PIC,
                             model.PICContactNo,
                             model.Email,
                             ContactGroup = model.ContactGroup.Name,
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
                            model.Address,
                            model.ContactNo,
                            model.PIC,
                            model.PICContactNo,
                            model.Email,
                            model.ContactGroup,
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

             return Json(new
             {
                 model.Id,
                 model.Name,
                 model.Address,
                 model.ContactNo,
                 model.PIC,
                 model.PICContactNo,
                 model.Email,
                 model.ContactGroupId,
                 ContactGroup = _contactGroupService.GetObjectById(model.ContactGroupId).Name,
                 model.Errors
             }, JsonRequestBehavior.AllowGet);
         }

        [HttpPost]
        public dynamic Insert(Contact model)
        {
            try
            {
                model = _contactService.CreateObject(model,_contactGroupService);
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
                data.Address = model.Address;
                data.ContactNo = model.ContactNo;
                data.PIC = model.PIC;
                data.PICContactNo = model.PICContactNo;
                data.Email = model.Email;
                data.ContactGroupId = model.ContactGroupId;
                model = _contactService.UpdateObject(data,_contactGroupService);
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
                model = _contactService.SoftDeleteObject(data, _coreIdentificationService, 
                    _blanketService, _purchaseOrderService, _salesOrderService, _salesQuotationService, _virtualOrderService);
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
