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
    public class MstUoMController : Controller
    {
        private readonly static log4net.ILog LOG = log4net.LogManager.GetLogger("UoMController");
        private IUoMService _UoMService;
        private IItemService _itemService;

        public MstUoMController()
        {
            _UoMService = new UoMService(new UoMRepository(),new UoMValidator());
            _itemService = new ItemService(new ItemRepository(), new ItemValidator());

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
            var query = _UoMService.GetAll().Where(d => d.IsDeleted == false);

            var list = query as IEnumerable<UoM>;

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
                            item.CreatedAt,
                            item.UpdatedAt,
                      }
                    }).ToArray()
            }, JsonRequestBehavior.AllowGet);
        }

        public dynamic GetInfo(int Id)
        {
            UoM model = new UoM();
            try
            {
                model = _UoMService.GetObjectById(Id);
            }
            catch (Exception ex)
            {
                LOG.Error("GetInfo", ex);
                model.Errors.Add("Error", "Error : " + ex);
            }

            return Json(new
            {
                model.Id,
                model.Name,
                model.Errors
            }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public dynamic Insert(UoM model)
        {
            try
            {
                model = _UoMService.CreateObject(model);
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
        public dynamic Update(UoM model)
        {
            try
            {
                var data = _UoMService.GetObjectById(model.Id);
                data.Name = model.Name;
                model = _UoMService.UpdateObject(data);
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
        public dynamic Delete(UoM model)
        {
            try
            {
                var data = _UoMService.GetObjectById(model.Id);
                model = _UoMService.SoftDeleteObject(data,_itemService);
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
