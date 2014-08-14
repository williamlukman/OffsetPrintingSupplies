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
    public class StockAdjustmentController : Controller
    {
        private readonly static log4net.ILog LOG = log4net.LogManager.GetLogger("StockAdjustmentController");
        private IStockAdjustmentService _stockAdjustmentService;
        private IStockAdjustmentDetailService _stockAdjustmentDetailService;
        private IWarehouseService _warehouseService;
        private IItemService _itemService;
        private IWarehouseItemService _warehouseItemService;
        private IStockMutationService _stockMutationService;
        private IBarringService _barringService;

        public StockAdjustmentController()
        {
            _stockAdjustmentService = new StockAdjustmentService(new StockAdjustmentRepository(), new StockAdjustmentValidator());
            _stockAdjustmentDetailService = new StockAdjustmentDetailService(new StockAdjustmentDetailRepository(), new StockAdjustmentDetailValidator());
            _warehouseService = new WarehouseService(new WarehouseRepository(), new WarehouseValidator());
            _itemService = new ItemService(new ItemRepository(), new ItemValidator());
            _warehouseItemService = new WarehouseItemService(new WarehouseItemRepository(), new WarehouseItemValidator());
            _stockMutationService = new StockMutationService(new StockMutationRepository(), new StockMutationValidator());
            _barringService = new BarringService(new BarringRepository(), new BarringValidator());
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
            var query =  _stockAdjustmentService.GetAll().Where(d => d.IsDeleted == false);

            var list = query as IEnumerable<StockAdjustment>;

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
                            model.WarehouseId,
                            _warehouseService.GetObjectById(model.WarehouseId).Name,
                            model.AdjustmentDate,
                            model.Description,
                            model.IsConfirmed,
                            model.ConfirmationDate,
                            model.CreatedAt,
                            model.UpdatedAt,
                      }
                    }).ToArray()
            }, JsonRequestBehavior.AllowGet);
        }

        public dynamic GetListDetail(string _search, long nd, int rows, int? page, string sidx, string sord, int id,string filters = "")
        {
            // Construct where statement

            string strWhere = GeneralFunction.ConstructWhere(filters);

            // Get Data
            var query =  _stockAdjustmentDetailService.GetObjectsByStockAdjustmentId(id).Where(d => d.IsDeleted == false);

            var list = query as IEnumerable<StockAdjustmentDetail>;

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
                            model.ItemId,
                            _itemService.GetObjectById(model.ItemId).Name,
                            model.Quantity
                      }
                    }).ToArray()
            }, JsonRequestBehavior.AllowGet);
        }

        public dynamic GetInfo(int Id)
        {
            StockAdjustment model = new StockAdjustment();
            try
            {
                model = _stockAdjustmentService.GetObjectById(Id);
            }
            catch (Exception ex)
            {
                LOG.Error("GetInfo", ex);
                model.Errors.Add("Generic", "Error : " + ex);
            }

            return Json(new
            {
                model.Id,
                model.WarehouseId,
                Warehouse = _warehouseService.GetObjectById(model.WarehouseId).Name,
                model.AdjustmentDate,
                model.Description,
                model.Code,
                model.Errors
            }, JsonRequestBehavior.AllowGet);
        }

        public dynamic GetInfoDetail(int Id)
        {
            StockAdjustmentDetail model = new StockAdjustmentDetail();
            try
            {
                model = _stockAdjustmentDetailService.GetObjectById(Id);
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
        public dynamic Insert(StockAdjustment model)
        {
            try
            {
                model = _stockAdjustmentService.CreateObject(model,_warehouseService);
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
        public dynamic InsertDetail(StockAdjustmentDetail model)
        {
            try
            {
                model = _stockAdjustmentDetailService.CreateObject(model,_stockAdjustmentService,_itemService,_warehouseItemService);
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
        public dynamic Update(StockAdjustment model)
        {
            try
            {
                var data = _stockAdjustmentService.GetObjectById(model.Id);
                data.AdjustmentDate = model.AdjustmentDate;
                data.Description = model.Description;
                model = _stockAdjustmentService.UpdateObject(data,_warehouseService);
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
        public dynamic UpdateDetail(StockAdjustmentDetail model)
        {
            try
            {
                var data = _stockAdjustmentDetailService.GetObjectById(model.Id);
                data.ItemId = model.ItemId;
                data.Quantity = model.Quantity;
                model = _stockAdjustmentDetailService.UpdateObject(data,_stockAdjustmentService,_itemService,_warehouseItemService);
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
        public dynamic Delete(StockAdjustment model)
        {
            try
            {
                var data = _stockAdjustmentService.GetObjectById(model.Id);
                model = _stockAdjustmentService.SoftDeleteObject(data, _stockAdjustmentDetailService);
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
        public dynamic DeleteDetail(StockAdjustmentDetail model)
        {
            try
            {
                var data = _stockAdjustmentDetailService.GetObjectById(model.Id);
                model = _stockAdjustmentDetailService.SoftDeleteObject(data);
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
        public dynamic Confirm(StockAdjustment model)
        {
            try
            {
                var data = _stockAdjustmentService.GetObjectById(model.Id);
                model = _stockAdjustmentService.ConfirmObject(data, model.ConfirmationDate.Value, _stockAdjustmentDetailService, _stockMutationService, _itemService, _barringService, _warehouseItemService);
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
        public dynamic UnConfirm(StockAdjustment model)
        {
            try
            {

                var data = _stockAdjustmentService.GetObjectById(model.Id);
                model = _stockAdjustmentService.UnconfirmObject(data,_stockAdjustmentDetailService,_stockMutationService,_itemService,_barringService,_warehouseItemService);
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

