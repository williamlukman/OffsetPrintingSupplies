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
        private ICustomerService _customerService;
        private IBarringService _barringService;
        private ICoreIdentificationService _coreIdentificationService;
        public MstContactController()
        {
            _customerService = new CustomerService(new CustomerRepository(), new CustomerValidator());
            _coreIdentificationService = new CoreIdentificationService(new CoreIdentificationRepository(), new CoreIdentificationValidator());
            _barringService = new BarringService(new BarringRepository(),new BarringValidator());
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
            var query = _customerService.GetAll().Where(d => d.IsDeleted == false);
            
            var list = query as IEnumerable<Customer>;

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
                            item.CustomerNo,
                            item.PIC,
                            item.PICCustomerNo,
                            item.Email,
                            item.CreatedAt,
                            item.UpdatedAt,
                      }
                    }).ToArray()
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public dynamic Insert(Customer model)
        {
            try
            {
                model = _customerService.CreateObject(model);
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
        public dynamic Update(Customer model)
        {
            try
            {
                var data = _customerService.GetObjectById(model.Id);
                data.Name = model.Name;
                data.Address = model.Address;
                data.CustomerNo = model.CustomerNo;
                data.PIC = model.PIC;
                data.PICCustomerNo = model.PICCustomerNo;
                data.Email = model.Email;
                model = _customerService.UpdateObject(data);
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
        public dynamic Delete(Customer model)
        {
            try
            {
                var data = _customerService.GetObjectById(model.Id);
                model = _customerService.SoftDeleteObject(data, _coreIdentificationService, _barringService);
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
