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
    public class WarehouseMutationOrderController : Controller
    {
      private readonly static log4net.ILog LOG = log4net.LogManager.GetLogger("WarehouseMutationOrderController");
        private IWarehouseService _warehouseService;
        private IItemService _itemService;
        private IWarehouseItemService _warehouseItemService;
        private IStockMutationService _stockMutationService;
        private IBarringService _barringService;
        private IWarehouseMutationOrderService _warehouseMutationOrderService;
        private IWarehouseMutationOrderDetailService _warehouseMutationOrderDetailService;

        public WarehouseMutationOrderController()
        {
            _warehouseService = new WarehouseService(new WarehouseRepository(), new WarehouseValidator());
            _itemService = new ItemService(new ItemRepository(), new ItemValidator());
            _warehouseItemService = new WarehouseItemService(new WarehouseItemRepository(), new WarehouseItemValidator());
            _stockMutationService = new StockMutationService(new StockMutationRepository(), new StockMutationValidator());
            _barringService = new BarringService(new BarringRepository(), new BarringValidator());
            _warehouseMutationOrderService = new WarehouseMutationOrderService(new WarehouseMutationOrderRepository(), new WarehouseMutationOrderValidator());
            _warehouseMutationOrderDetailService = new WarehouseMutationOrderDetailService(new WarehouseMutationOrderDetailRepository(), new WarehouseMutationOrderDetailValidator());
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
            var query =  _warehouseMutationOrderService.GetAll().Where(d => d.IsDeleted == false);

            var list = query as IEnumerable<WarehouseMutationOrder>;

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
                            model.WarehouseFromId,
                            _warehouseService.GetObjectById(model.WarehouseFromId).Name,
                            model.WarehouseToId,
                            _warehouseService.GetObjectById(model.WarehouseToId).Name,
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
            var query =  _warehouseMutationOrderDetailService.GetObjectsByWarehouseMutationOrderId(id).Where(d => d.IsDeleted == false);

            var list = query as IEnumerable<WarehouseMutationOrderDetail>;

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
            WarehouseMutationOrder model = new WarehouseMutationOrder();
            try
            {
                model = _warehouseMutationOrderService.GetObjectById(Id);
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
                model.Errors
            }, JsonRequestBehavior.AllowGet);
        }

        public dynamic GetInfoDetail(int Id)
        {
            WarehouseMutationOrderDetail model = new WarehouseMutationOrderDetail();
            try
            {
                model = _warehouseMutationOrderDetailService.GetObjectById(Id);
            
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
        public dynamic Insert(WarehouseMutationOrder model)
        {
            try
            {
                model = _warehouseMutationOrderService.CreateObject(model,_warehouseService);
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
        public dynamic InsertDetail(WarehouseMutationOrderDetail model)
        {
            try
            {
                model = _warehouseMutationOrderDetailService.CreateObject(model,_warehouseMutationOrderService,_itemService,_warehouseItemService);
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
        public dynamic Update(WarehouseMutationOrder model)
        {
            try
            {
                var data = _warehouseMutationOrderService.GetObjectById(model.Id);
                data.WarehouseFromId = model.WarehouseFromId;
                data.WarehouseToId = model.WarehouseToId;
                model = _warehouseMutationOrderService.UpdateObject(data,_warehouseService);
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
        public dynamic Delete(WarehouseMutationOrder model)
        {
            try
            {
                var data = _warehouseMutationOrderService.GetObjectById(model.Id);
                model = _warehouseMutationOrderService.SoftDeleteObject(data);
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
        public dynamic DeleteDetail(WarehouseMutationOrderDetail model)
        {
            try
            {
                var data = _warehouseMutationOrderDetailService.GetObjectById(model.Id);
                model = _warehouseMutationOrderDetailService.SoftDeleteObject(data,_warehouseMutationOrderService,_warehouseItemService);
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
        public dynamic UpdateDetail(WarehouseMutationOrderDetail model)
        {
            try
            {
                var data = _warehouseMutationOrderDetailService.GetObjectById(model.Id);
                data.ItemId = model.ItemId;
                data.Quantity = model.Quantity;
                model = _warehouseMutationOrderDetailService.UpdateObject(data,_warehouseMutationOrderService,_itemService,_warehouseItemService);
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
        public dynamic Confirm(WarehouseMutationOrder model)
        {
            try
            {
                var data = _warehouseMutationOrderService.GetObjectById(model.Id);
                model = _warehouseMutationOrderService.ConfirmObject(data,model.ConfirmationDate.Value,_warehouseMutationOrderDetailService,_itemService,_barringService,_warehouseItemService,_stockMutationService);
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
        public dynamic UnConfirm(WarehouseMutationOrder model)
        {
            try
            {

                var data = _warehouseMutationOrderService.GetObjectById(model.Id);
                model = _warehouseMutationOrderService.UnconfirmObject(data,_warehouseMutationOrderDetailService,_itemService,_barringService,_warehouseItemService,_stockMutationService);
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
