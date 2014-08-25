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
    public class WarehouseMutationController : Controller
    {
      private readonly static log4net.ILog LOG = log4net.LogManager.GetLogger("WarehouseMutationController");
        private IWarehouseService _warehouseService;
        private IItemService _itemService;
        private IWarehouseItemService _warehouseItemService;
        private IStockMutationService _stockMutationService;
        private IBarringService _barringService;
        private IWarehouseMutationService _warehouseMutationService;
        private IWarehouseMutationDetailService _warehouseMutationDetailService;
        private IUoMService _uomService;

        public WarehouseMutationController()
        {
            _warehouseService = new WarehouseService(new WarehouseRepository(), new WarehouseValidator());
            _itemService = new ItemService(new ItemRepository(), new ItemValidator());
            _warehouseItemService = new WarehouseItemService(new WarehouseItemRepository(), new WarehouseItemValidator());
            _stockMutationService = new StockMutationService(new StockMutationRepository(), new StockMutationValidator());
            _barringService = new BarringService(new BarringRepository(), new BarringValidator());
            _warehouseMutationService = new WarehouseMutationService(new WarehouseMutationRepository(), new WarehouseMutationValidator());
            _warehouseMutationDetailService = new WarehouseMutationDetailService(new WarehouseMutationDetailRepository(), new WarehouseMutationDetailValidator());
            _uomService = new UoMService(new UoMRepository(), new UoMValidator());
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
            var query =  _warehouseMutationService.GetAll().Where(d => d.IsDeleted == false);

            var list = query as IEnumerable<WarehouseMutation>;

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
                            _warehouseService.GetObjectById(model.WarehouseFromId).Name,
                            _warehouseService.GetObjectById(model.WarehouseToId).Name,
                            model.MutationDate,
                            model.ConfirmationDate,
                            model.CreatedAt,
                            model.UpdatedAt
                      }
                    }).ToArray()
            }, JsonRequestBehavior.AllowGet);
        }

        public dynamic GetListDetail(string _search, long nd, int rows, int? page, string sidx, string sord, int id,string filters = "")
        {
            // Construct where statement

            string strWhere = GeneralFunction.ConstructWhere(filters);

            // Get Data
            var query =  _warehouseMutationDetailService.GetObjectsByWarehouseMutationId(id).Where(d => d.IsDeleted == false);

            var list = query as IEnumerable<WarehouseMutationDetail>;

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
                            model.Code,
                            _itemService.GetObjectById(model.ItemId).Sku,
                            _itemService.GetObjectById(model.ItemId).Name,
                            model.Quantity,
                            _uomService.GetObjectById(_itemService.GetObjectById(model.ItemId).UoMId).Name
                      }
                    }).ToArray()
            }, JsonRequestBehavior.AllowGet);
        }

        public dynamic GetInfo(int Id)
        {
            WarehouseMutation model = new WarehouseMutation();
            try
            {
                model = _warehouseMutationService.GetObjectById(Id);
            }
            catch (Exception ex)
            {
                LOG.Error("GetInfo", ex);
                model.Errors.Add("Generic", "Error : " + ex);
            }

            return Json(new
            {
                model.Id,
                model.Code,
                model.WarehouseFromId,
                WarehouseFrom = _warehouseService.GetObjectById(model.WarehouseFromId).Name,
                model.WarehouseToId,
                WarehouseTo = _warehouseService.GetObjectById(model.WarehouseToId).Name,
                model.MutationDate,
                model.Errors
            }, JsonRequestBehavior.AllowGet);
        }

        public dynamic GetInfoDetail(int Id)
        {
            WarehouseMutationDetail model = new WarehouseMutationDetail();
            try
            {
                model = _warehouseMutationDetailService.GetObjectById(Id);
            
            }
            catch (Exception ex)
            {
                LOG.Error("GetInfo", ex);
                model.Errors.Add("Generic", "Error : " + ex);
            }

            return Json(new
            {
                model.Id,
                model.ItemId,
                Item = _itemService.GetObjectById(model.ItemId).Name,
                model.Quantity,
                model.Errors
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public dynamic Insert(WarehouseMutation model)
        {
            try
            {
                model = _warehouseMutationService.CreateObject(model,_warehouseService);
            }
            catch (Exception ex)
            {
                LOG.Error("Insert Failed", ex);
                model.Errors.Add("Generic", "Error : " + ex);
            }

            return Json(new
            {
                model.Errors
            });
        }

        [HttpPost]
        public dynamic InsertDetail(WarehouseMutationDetail model)
        {
            try
            {
                model = _warehouseMutationDetailService.CreateObject(model,_warehouseMutationService,_itemService,_warehouseItemService);
            }
            catch (Exception ex)
            {
                LOG.Error("Insert Failed", ex);
                model.Errors.Add("Generic", "Error : " + ex);
            }

            return Json(new
            {
                model.Errors
            });
        }

        [HttpPost]
        public dynamic Update(WarehouseMutation model)
        {
            try
            {
                var data = _warehouseMutationService.GetObjectById(model.Id);
                data.WarehouseFromId = model.WarehouseFromId;
                data.WarehouseToId = model.WarehouseToId;
                data.MutationDate = model.MutationDate;
                model = _warehouseMutationService.UpdateObject(data,_warehouseService);
            }
            catch (Exception ex)
            {
                LOG.Error("Update Failed", ex);
                model.Errors.Add("Generic", "Error : " + ex);
            }

            return Json(new
            {
                model.Errors
            });
        }

        [HttpPost]
        public dynamic Delete(WarehouseMutation model)
        {
            try
            {
                var data = _warehouseMutationService.GetObjectById(model.Id);
                model = _warehouseMutationService.SoftDeleteObject(data);
            }
            catch (Exception ex)
            {
                LOG.Error("Delete Failed", ex);
                model.Errors.Add("Generic", "Error : " + ex);
            }

            return Json(new
            {
                model.Errors
            });
        }

        [HttpPost]
        public dynamic DeleteDetail(WarehouseMutationDetail model)
        {
            try
            {
                var data = _warehouseMutationDetailService.GetObjectById(model.Id);
                model = _warehouseMutationDetailService.SoftDeleteObject(data,_warehouseMutationService,_warehouseItemService);
            }
            catch (Exception ex)
            {
                LOG.Error("Delete Failed", ex);
                model.Errors.Add("Generic", "Error : " + ex);
            }

            return Json(new
            {
                model.Errors
            });
        }

        [HttpPost]
        public dynamic UpdateDetail(WarehouseMutationDetail model)
        {
            try
            {
                var data = _warehouseMutationDetailService.GetObjectById(model.Id);
                data.ItemId = model.ItemId;
                data.Quantity = model.Quantity;
                model = _warehouseMutationDetailService.UpdateObject(data,_warehouseMutationService,_itemService,_warehouseItemService);
            }
            catch (Exception ex)
            {
                LOG.Error("Update Failed", ex);
                model.Errors.Add("Generic", "Error : " + ex);
            }

            return Json(new
            {
                model.Errors
            });
        }


        [HttpPost]
        public dynamic Confirm(WarehouseMutation model)
        {
            try
            {
                var data = _warehouseMutationService.GetObjectById(model.Id);
                model = _warehouseMutationService.ConfirmObject(data,model.ConfirmationDate.Value,_warehouseMutationDetailService,_itemService,_barringService,_warehouseItemService,_stockMutationService);
            }
            catch (Exception ex)
            {
                LOG.Error("Confirm Failed", ex);
                model.Errors.Add("Generic", "Error : " + ex);
            }

            return Json(new
            {
                model.Errors
            });
        }

        [HttpPost]
        public dynamic UnConfirm(WarehouseMutation model)
        {
            try
            {

                var data = _warehouseMutationService.GetObjectById(model.Id);
                model = _warehouseMutationService.UnconfirmObject(data,_warehouseMutationDetailService,_itemService,_barringService,_warehouseItemService,_stockMutationService);
            }
            catch (Exception ex)
            {
                LOG.Error("Unconfirm Failed", ex);
                model.Errors.Add("Generic", "Error : " + ex);
            }

            return Json(new
            {
                model.Errors
            });
        }


    }
}
