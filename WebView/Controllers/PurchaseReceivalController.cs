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
    public class PurchaseReceivalController : Controller
    {
       private readonly static log4net.ILog LOG = log4net.LogManager.GetLogger("PurchaseReceivalController");
        private IItemService _itemService;
        private IWarehouseItemService _warehouseItemService;
        private IStockMutationService _stockMutationService;
        private IBarringService _barringService;
        private IPurchaseOrderService _purchaseOrderService;
        private IPurchaseOrderDetailService _purchaseOrderDetailService;
        private IPurchaseReceivalDetailService _purchaseReceivalDetailService;
        private IPurchaseReceivalService _purchaseReceivalService;
        private IPurchaseInvoiceService _purchaseInvoiceService;
        private IPurchaseInvoiceDetailService _purchaseInvoiceDetailService;
        private IWarehouseService _warehouseService;

        public PurchaseReceivalController()
        {
            _itemService = new ItemService(new ItemRepository(), new ItemValidator());
            _warehouseItemService = new WarehouseItemService(new WarehouseItemRepository(), new WarehouseItemValidator());
            _stockMutationService = new StockMutationService(new StockMutationRepository(), new StockMutationValidator());
            _barringService = new BarringService(new BarringRepository(), new BarringValidator());
            _purchaseOrderService = new PurchaseOrderService(new PurchaseOrderRepository(), new PurchaseOrderValidator());
            _purchaseOrderDetailService = new PurchaseOrderDetailService(new PurchaseOrderDetailRepository(), new PurchaseOrderDetailValidator());
            _purchaseReceivalService = new PurchaseReceivalService(new PurchaseReceivalRepository(), new PurchaseReceivalValidator());
            _purchaseReceivalDetailService = new PurchaseReceivalDetailService(new PurchaseReceivalDetailRepository(), new PurchaseReceivalDetailValidator());
            _purchaseInvoiceService = new PurchaseInvoiceService(new PurchaseInvoiceRepository(), new PurchaseInvoiceValidator());
            _purchaseInvoiceDetailService = new PurchaseInvoiceDetailService(new PurchaseInvoiceDetailRepository(), new PurchaseInvoiceDetailValidator());
            _warehouseService = new WarehouseService(new WarehouseRepository(), new WarehouseValidator());
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
            var query =  _purchaseReceivalService.GetAll().Where(d => d.IsDeleted == false);

            var list = query as IEnumerable<PurchaseReceival>;

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
                            model.PurchaseOrderId,
                            _purchaseOrderService.GetObjectById(model.PurchaseOrderId).Code,
                            model.ReceivalDate,
                            model.WarehouseId,
                            _warehouseService.GetObjectById(model.WarehouseId).Name,
                            model.IsConfirmed,
                            model.ConfirmationDate,
                            model.CreatedAt,
                            model.UpdatedAt,
                      }
                    }).ToArray()
            }, JsonRequestBehavior.AllowGet);
        }

        public dynamic GetListConfirmed(string _search, long nd, int rows, int? page, string sidx, string sord, string filters = "")
        {
            // Construct where statement

            string strWhere = GeneralFunction.ConstructWhere(filters);

            // Get Data
            var query = _purchaseReceivalService.GetConfirmedObjects().Where(d => d.IsDeleted == false);

            var list = query as IEnumerable<PurchaseReceival>;

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
                            model.PurchaseOrderId,
                            _purchaseOrderService.GetObjectById(model.PurchaseOrderId).Code,
                            model.ReceivalDate,
                            model.WarehouseId,
                            _warehouseService.GetObjectById(model.WarehouseId).Name,
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
            var query =  _purchaseReceivalDetailService.GetObjectsByPurchaseReceivalId(id).Where(d => d.IsDeleted == false);

            var list = query as IEnumerable<PurchaseReceivalDetail>;

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
                            model.PurchaseOrderDetailId,
                            _purchaseOrderDetailService.GetObjectById(model.PurchaseOrderDetailId).Code,
                            model.ItemId,
                            _itemService.GetObjectById(model.ItemId).Name,
                            model.Quantity,
                      }
                    }).ToArray()
            }, JsonRequestBehavior.AllowGet);
        }

      
        public dynamic GetInfo(int Id)
        {
            PurchaseReceival model = new PurchaseReceival();
            try
            {
                model = _purchaseReceivalService.GetObjectById(Id);
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
                model.PurchaseOrderId,
                PurchaseOrder = _purchaseOrderService.GetObjectById(model.PurchaseOrderId).Code,
                model.ReceivalDate, 
                model.WarehouseId, 
                Warehouse =_warehouseService.GetObjectById(model.WarehouseId).Name,
                model.Errors
            }, JsonRequestBehavior.AllowGet);
        }

        public dynamic GetInfoDetail(int Id)
        {
            PurchaseReceivalDetail model = new PurchaseReceivalDetail();
            try
            {
                model = _purchaseReceivalDetailService.GetObjectById(Id);
            
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
                model.PurchaseOrderDetailId,
                PurchaseOrderDetail = _purchaseOrderDetailService.GetObjectById(model.PurchaseOrderDetailId).Code,
                model.ItemId,
                Item = _itemService.GetObjectById(model.ItemId).Name,
                model.Errors
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public dynamic Insert(PurchaseReceival model)
        {
            try
            {
             
                model = _purchaseReceivalService.CreateObject(model,_purchaseOrderService,_warehouseService);
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
        public dynamic InsertDetail(PurchaseReceivalDetail model)
        {
            try
            {
                model = _purchaseReceivalDetailService.CreateObject(model,_purchaseReceivalService,_purchaseOrderDetailService,_purchaseOrderService,_itemService);
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
        public dynamic Update(PurchaseReceival model)
        {
            try
            {
                var data = _purchaseReceivalService.GetObjectById(model.Id);
                data.PurchaseOrderId = model.PurchaseOrderId;
                data.ReceivalDate = model.ReceivalDate;
                data.WarehouseId = model.WarehouseId;
                model = _purchaseReceivalService.UpdateObject(data,_purchaseOrderService,_warehouseService);
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
        public dynamic Delete(PurchaseReceival model)
        {
            try
            {
                var data = _purchaseReceivalService.GetObjectById(model.Id);
                model = _purchaseReceivalService.SoftDeleteObject(data,_purchaseReceivalDetailService);
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
        public dynamic DeleteDetail(PurchaseReceivalDetail model)
        {
            try
            {
                var data = _purchaseReceivalDetailService.GetObjectById(model.Id);
                model = _purchaseReceivalDetailService.SoftDeleteObject(data);
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
        public dynamic UpdateDetail(PurchaseReceivalDetail model)
        {
            try
            {
                var data = _purchaseReceivalDetailService.GetObjectById(model.Id);
                data.PurchaseOrderDetailId = model.PurchaseOrderDetailId;
                data.ItemId = model.ItemId;
                data.Quantity = model.Quantity;
                model = _purchaseReceivalDetailService.UpdateObject(data,_purchaseReceivalService,_purchaseOrderDetailService,_purchaseOrderService,_itemService);
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
        public dynamic Confirm(PurchaseReceival model)
        {
            try
            {
                var data = _purchaseReceivalService.GetObjectById(model.Id);
                model = _purchaseReceivalService.ConfirmObject(data,model.ConfirmationDate.Value,_purchaseReceivalDetailService,_purchaseOrderService,_purchaseOrderDetailService,_stockMutationService,_itemService,_barringService,_warehouseItemService);
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
        public dynamic UnConfirm(PurchaseReceival model)
        {
            try
            {

                var data = _purchaseReceivalService.GetObjectById(model.Id);
                model = _purchaseReceivalService.UnconfirmObject(data,_purchaseReceivalDetailService,_purchaseInvoiceService,_purchaseInvoiceDetailService,_purchaseOrderService,_purchaseOrderDetailService,_stockMutationService,_itemService,_barringService,_warehouseItemService);
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
