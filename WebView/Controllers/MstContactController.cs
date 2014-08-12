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
    public class MstContactController : Controller
    {
        private readonly static log4net.ILog LOG = log4net.LogManager.GetLogger("ContactController");
        private IContactService _contactService;
        private IBarringService _barringService;
        private ICoreIdentificationService _coreIdentificationService;
        private IPurchaseOrderService _purchaseOrderService;
        private ISalesOrderService _salesOrderService;
        public MstContactController()
        {
            _contactService = new ContactService(new ContactRepository(), new ContactValidator());
            _coreIdentificationService = new CoreIdentificationService(new CoreIdentificationRepository(), new CoreIdentificationValidator());
            _barringService = new BarringService(new BarringRepository(),new BarringValidator());
            _purchaseOrderService = new PurchaseOrderService(new PurchaseOrderRepository(), new PurchaseOrderValidator());
            _salesOrderService = new SalesOrderService(new SalesOrderRepository(),new SalesOrderValidator());
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
            var query = _contactService.GetAll().Where(d => d.IsDeleted == false);
            
            var list = query as IEnumerable<Contact>;

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
                            item.Address,
                            item.ContactNo,
                            item.PIC,
                            item.PICContactNo,
                            item.Email,
                            item.CreatedAt,
                            item.UpdatedAt,
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
                 model.Errors.Add("Generic", "Error" + ex);
             }

             return Json(new
             {
                 model
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
            }

            return Json(new
            {
                model
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
                model = _contactService.UpdateObject(data);
            }
            catch (Exception ex)
            {
                LOG.Error("Update Failed", ex);
            }

            return Json(new
            {
                model
            });
        }

        [HttpPost]
        public dynamic Delete(Contact model)
        {
            try
            {
                var data = _contactService.GetObjectById(model.Id);
                model = _contactService.SoftDeleteObject(data, _coreIdentificationService, _barringService, _purchaseOrderService, _salesOrderService);
            }
            catch (Exception ex)
            {
                LOG.Error("Delete Failed", ex);
            }

            return Json(new
            {
                model
            });
        }
    }
}
