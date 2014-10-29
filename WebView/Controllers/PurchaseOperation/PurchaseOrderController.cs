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
    public class PurchaseOrderController : Controller
    {
       private readonly static log4net.ILog LOG = log4net.LogManager.GetLogger("PurchaseOrderController");
        private IItemService _itemService;
        private IWarehouseItemService _warehouseItemService;
        private IStockMutationService _stockMutationService;
        private IBlanketService _blanketService;
        private IPurchaseOrderService _purchaseOrderService;
        private IPurchaseOrderDetailService _purchaseOrderDetailService;
        private IContactService _contactService;
        private IPurchaseReceivalDetailService _purchaseReceivalDetailService;
        private IPurchaseReceivalService _purchaseReceivalService;

        public PurchaseOrderController()
        {
            _itemService = new ItemService(new ItemRepository(), new ItemValidator());
            _warehouseItemService = new WarehouseItemService(new WarehouseItemRepository(), new WarehouseItemValidator());
            _stockMutationService = new StockMutationService(new StockMutationRepository(), new StockMutationValidator());
            _blanketService = new BlanketService(new BlanketRepository(), new BlanketValidator());
            _purchaseOrderService = new PurchaseOrderService(new PurchaseOrderRepository(), new PurchaseOrderValidator());
            _purchaseOrderDetailService = new PurchaseOrderDetailService(new PurchaseOrderDetailRepository(), new PurchaseOrderDetailValidator());
            _contactService = new ContactService(new ContactRepository(), new ContactValidator());
            _purchaseReceivalService = new PurchaseReceivalService(new PurchaseReceivalRepository(), new PurchaseReceivalValidator());
            _purchaseReceivalDetailService = new PurchaseReceivalDetailService(new PurchaseReceivalDetailRepository(), new PurchaseReceivalDetailValidator());
     
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
            var q = _purchaseOrderService.GetQueryable().Include("Contact").Where(x => !x.IsDeleted);

            var query = (from model in q
                         select new
                         {
                             model.Id,
                             model.Code,
                             model.NomorSurat,
                             model.ContactId,
                             Contact = model.Contact.Name,
                             model.PurchaseDate,
                             model.IsConfirmed,
                             model.ConfirmationDate,
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
                            model.Code,
                            model.NomorSurat,
                            model.ContactId,
                            model.Contact,
                            model.PurchaseDate,
                            model.IsConfirmed,
                            model.ConfirmationDate,
                            model.CreatedAt,
                            model.UpdatedAt,
                      }
                    }).ToArray()
            }, JsonRequestBehavior.AllowGet);
        }

        public dynamic GetListConfirmedNotCompleted(string _search, long nd, int rows, int? page, string sidx, string sord, string filters = "")
        {
            // Construct where statement
            string strWhere = GeneralFunction.ConstructWhere(filters);
            string filter = null;
            GeneralFunction.ConstructWhereInLinq(strWhere, out filter);
            if (filter == "") filter = "true";

            // Get Data
            var q = _purchaseOrderService.GetQueryable().Include("Contact")
                                         .Where(x => x.IsConfirmed && !x.IsReceivalCompleted && !x.IsDeleted);

            var query = (from model in q
                         select new
                         {
                             model.Id,
                             model.Code,
                             model.NomorSurat,
                             model.ContactId,
                             Contact = model.Contact.Name,
                             model.PurchaseDate,
                             model.IsConfirmed,
                             model.ConfirmationDate,
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
                            model.Code,
                            model.NomorSurat,
                            model.ContactId,
                            model.Contact,
                            model.PurchaseDate,
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
            string filter = null;
            GeneralFunction.ConstructWhereInLinq(strWhere, out filter);
            if (filter == "") filter = "true";

            // Get Data
            var q = _purchaseOrderDetailService.GetQueryable().Include("Item")
                                               .Where(x => x.PurchaseOrderId == id && !x.IsDeleted);

            var query = (from model in q
                         select new
                         {
                             model.Id,
                             model.Code,
                             model.ItemId,
                             ItemSku =  model.Item.Sku,
                             Item = model.Item.Name,
                             model.Quantity,
                             model.PendingReceivalQuantity,
                             model.Price
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
                            model.Code,
                            model.ItemId,
                            model.ItemSku,
                            model.Item,
                            model.Quantity,
                            model.PendingReceivalQuantity,
                            model.Price
                      }
                    }).ToArray()
            }, JsonRequestBehavior.AllowGet);
        }

        public dynamic GetInfo(int Id)
        {
            PurchaseOrder model = new PurchaseOrder();
            try
            {
                model = _purchaseOrderService.GetObjectById(Id);
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
                model.NomorSurat,
                model.ContactId,
                Contact = _contactService.GetObjectById(model.ContactId).Name,
                model.PurchaseDate,
                ConfirmationDate = model.ConfirmationDate,
                model.Errors
            }, JsonRequestBehavior.AllowGet);
        }

        public dynamic GetInfoDetail(int Id)
        {
            PurchaseOrderDetail model = new PurchaseOrderDetail();
            try
            {
                model = _purchaseOrderDetailService.GetObjectById(Id);
            
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
                model.Price,
                model.Errors
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public dynamic Insert(PurchaseOrder model)
        {
            try
            {
                model = _purchaseOrderService.CreateObject(model,_contactService);
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
        public dynamic InsertDetail(PurchaseOrderDetail model)
        {
            try
            {
                model = _purchaseOrderDetailService.CreateObject(model,_purchaseOrderService,_itemService);
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
        public dynamic Update(PurchaseOrder model)
        {
            try
            {
                var data = _purchaseOrderService.GetObjectById(model.Id);
                data.ContactId = model.ContactId;
                data.PurchaseDate = model.PurchaseDate;
                data.NomorSurat = model.NomorSurat;
                model = _purchaseOrderService.UpdateObject(data,_contactService);
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
        public dynamic Delete(PurchaseOrder model)
        {
            try
            {
                var data = _purchaseOrderService.GetObjectById(model.Id);
                model = _purchaseOrderService.SoftDeleteObject(data,_purchaseOrderDetailService);
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
        public dynamic DeleteDetail(PurchaseOrderDetail model)
        {
            try
            {
                var data = _purchaseOrderDetailService.GetObjectById(model.Id);
                model = _purchaseOrderDetailService.SoftDeleteObject(data);
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
        public dynamic UpdateDetail(PurchaseOrderDetail model)
        {
            try
            {
                var data = _purchaseOrderDetailService.GetObjectById(model.Id);
                data.ItemId = model.ItemId;
                data.Quantity = model.Quantity;
                data.Price = model.Price;
                model = _purchaseOrderDetailService.UpdateObject(data,_purchaseOrderService,_itemService);
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
        public dynamic Confirm(PurchaseOrder model)
        {
            try
            {
                var data = _purchaseOrderService.GetObjectById(model.Id);
                model = _purchaseOrderService.ConfirmObject(data,model.ConfirmationDate.Value,_purchaseOrderDetailService,_stockMutationService,_itemService,_blanketService,_warehouseItemService);
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
        public dynamic UnConfirm(PurchaseOrder model)
        {
            try
            {

                var data = _purchaseOrderService.GetObjectById(model.Id);
                model = _purchaseOrderService.UnconfirmObject(data,_purchaseOrderDetailService,_purchaseReceivalService,_purchaseReceivalDetailService,_stockMutationService,_itemService,_blanketService,_warehouseItemService);
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
