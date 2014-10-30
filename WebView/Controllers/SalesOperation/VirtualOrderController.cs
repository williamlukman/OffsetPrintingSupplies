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
    public class VirtualOrderController : Controller
    {
        private readonly static log4net.ILog LOG = log4net.LogManager.GetLogger("VirtualOrderController");
        private IItemService _itemService;
        private IWarehouseItemService _warehouseItemService;
        private IStockMutationService _stockMutationService;
        private IBlanketService _blanketService;
        private IVirtualOrderService _virtualOrderService;
        private IVirtualOrderDetailService _virtualOrderDetailService;
        private IContactService _contactService;
        private ITemporaryDeliveryOrderDetailService _temporaryDeliveryOrderDetailService;
        private ITemporaryDeliveryOrderService _temporaryDeliveryOrderService;

        public VirtualOrderController()
        {
            _itemService = new ItemService(new ItemRepository(), new ItemValidator());
            _warehouseItemService = new WarehouseItemService(new WarehouseItemRepository(), new WarehouseItemValidator());
            _stockMutationService = new StockMutationService(new StockMutationRepository(), new StockMutationValidator());
            _blanketService = new BlanketService(new BlanketRepository(), new BlanketValidator());
            _virtualOrderService = new VirtualOrderService(new VirtualOrderRepository(), new VirtualOrderValidator());
            _virtualOrderDetailService = new VirtualOrderDetailService(new VirtualOrderDetailRepository(), new VirtualOrderDetailValidator());
            _contactService = new ContactService(new ContactRepository(), new ContactValidator());
            _temporaryDeliveryOrderService = new TemporaryDeliveryOrderService(new TemporaryDeliveryOrderRepository(), new TemporaryDeliveryOrderValidator());
            _temporaryDeliveryOrderDetailService = new TemporaryDeliveryOrderDetailService(new TemporaryDeliveryOrderDetailRepository(), new TemporaryDeliveryOrderDetailValidator());

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
            var q = _virtualOrderService.GetQueryable().Include("Contact").Where(x => !x.IsDeleted);

            var query = (from model in q
                         select new
                         {
                             model.Id,
                             model.Code,
                             model.NomorSurat,
                             model.ContactId,
                             Contact = model.Contact.Name,
                             model.OrderType,
                             model.OrderDate,
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
                            model.OrderType,
                            model.OrderDate,
                            model.IsConfirmed,
                            model.ConfirmationDate,
                            model.CreatedAt,
                            model.UpdatedAt,
                      }
                    }).ToArray()
            }, JsonRequestBehavior.AllowGet);
        }

        public dynamic GetSampleListConfirmedNotCompleted(string _search, long nd, int rows, int? page, string sidx, string sord, string filters = "")
        {
            // Construct where statement
            string strWhere = GeneralFunction.ConstructWhere(filters);
            string filter = null;
            GeneralFunction.ConstructWhereInLinq(strWhere, out filter);
            if (filter == "") filter = "true";

            // Get Data
            var q = _virtualOrderService.GetQueryable().Include("Contact")
                                      .Where(x => x.IsConfirmed && !x.IsDeliveryCompleted && !x.IsDeleted && x.OrderType == Core.Constants.Constant.OrderTypeCase.SampleOrder);

            var query = (from model in q
                         select new
                         {
                             model.Id,
                             model.Code,
                             model.NomorSurat,
                             model.ContactId,
                             Contact = model.Contact.Name,
                             model.OrderDate,
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
                            model.OrderDate,
                            model.IsConfirmed,
                            model.ConfirmationDate,
                            model.CreatedAt,
                            model.UpdatedAt,
                            0,
                            ""
                      }
                    }).ToArray()
            }, JsonRequestBehavior.AllowGet);
        }

        public dynamic GetTrialListConfirmedNotCompleted(string _search, long nd, int rows, int? page, string sidx, string sord, string filters = "")
        {
            // Construct where statement
            string strWhere = GeneralFunction.ConstructWhere(filters);
            string filter = null;
            GeneralFunction.ConstructWhereInLinq(strWhere, out filter);
            if (filter == "") filter = "true";

            // Get Data
            var q = _virtualOrderService.GetQueryable().Include("Contact")
                                      .Where(x => x.IsConfirmed && !x.IsDeliveryCompleted && !x.IsDeleted && x.OrderType == Core.Constants.Constant.OrderTypeCase.TrialOrder);

            var query = (from model in q
                         select new
                         {
                             model.Id,
                             model.Code,
                             model.NomorSurat,
                             model.ContactId,
                             Contact = model.Contact.Name,
                             model.OrderType,
                             model.OrderDate,
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
                            model.OrderDate,
                            model.IsConfirmed,
                            model.ConfirmationDate,
                            model.CreatedAt,
                            model.UpdatedAt,
                            0,
                            ""
                      }
                    }).ToArray()
            }, JsonRequestBehavior.AllowGet);
        }

        public dynamic GetListDetail(string _search, long nd, int rows, int? page, string sidx, string sord, int id, string filters = "")
        {
            // Construct where statement
            string strWhere = GeneralFunction.ConstructWhere(filters);
            string filter = null;
            GeneralFunction.ConstructWhereInLinq(strWhere, out filter);
            if (filter == "") filter = "true";

            // Get Data
            var q = _virtualOrderDetailService.GetQueryable().Include("Item").Where(x => x.VirtualOrderId == id && !x.IsDeleted);

            var query = (from model in q
                         select new
                         {
                             model.Id,
                             model.Code,
                             model.ItemId,
                             ItemSku = model.Item.Sku,
                             Item = model.Item.Name,
                             model.Quantity,
                             model.PendingDeliveryQuantity,
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
                            model.PendingDeliveryQuantity,
                            model.Price
                      }
                    }).ToArray()
            }, JsonRequestBehavior.AllowGet);
        }


        public dynamic GetInfo(int Id)
        {
            VirtualOrder model = new VirtualOrder();
            try
            {
                model = _virtualOrderService.GetObjectById(Id);
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
                model.OrderType,
                model.OrderDate,
                ConfirmationDate = model.ConfirmationDate,
                model.Errors
            }, JsonRequestBehavior.AllowGet);
        }

        public dynamic GetInfoDetail(int Id)
        {
            VirtualOrderDetail model = new VirtualOrderDetail();
            try
            {
                model = _virtualOrderDetailService.GetObjectById(Id);

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
        public dynamic Insert(VirtualOrder model)
        {
            try
            {
                model = _virtualOrderService.CreateObject(model, _contactService);
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
        public dynamic InsertDetail(VirtualOrderDetail model)
        {
            try
            {
                model = _virtualOrderDetailService.CreateObject(model, _virtualOrderService, _itemService);
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
        public dynamic Update(VirtualOrder model)
        {
            try
            {
                var data = _virtualOrderService.GetObjectById(model.Id);
                data.ContactId = model.ContactId;
                data.OrderType = model.OrderType;
                data.OrderDate = model.OrderDate;
                data.NomorSurat = model.NomorSurat;
                model = _virtualOrderService.UpdateObject(data, _contactService);
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
        public dynamic Delete(VirtualOrder model)
        {
            try
            {
                var data = _virtualOrderService.GetObjectById(model.Id);
                model = _virtualOrderService.SoftDeleteObject(data, _virtualOrderDetailService);
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
        public dynamic DeleteDetail(VirtualOrderDetail model)
        {
            try
            {
                var data = _virtualOrderDetailService.GetObjectById(model.Id);
                model = _virtualOrderDetailService.SoftDeleteObject(data);
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
        public dynamic UpdateDetail(VirtualOrderDetail model)
        {
            try
            {
                var data = _virtualOrderDetailService.GetObjectById(model.Id);
                data.ItemId = model.ItemId;
                data.Quantity = model.Quantity;
                data.Price = model.Price;
                model = _virtualOrderDetailService.UpdateObject(data, _virtualOrderService, _itemService);
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
        public dynamic Confirm(VirtualOrder model)
        {
            try
            {
                var data = _virtualOrderService.GetObjectById(model.Id);
                model = _virtualOrderService.ConfirmObject(data, model.ConfirmationDate.Value, _virtualOrderDetailService, _stockMutationService, _itemService, _blanketService, _warehouseItemService);
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
        public dynamic UnConfirm(VirtualOrder model)
        {
            try
            {
                var data = _virtualOrderService.GetObjectById(model.Id);
                model = _virtualOrderService.UnconfirmObject(data, _virtualOrderDetailService, _temporaryDeliveryOrderService, _temporaryDeliveryOrderDetailService, _stockMutationService, _itemService, _blanketService, _warehouseItemService);
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
