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
    public class BlanketWarehouseMutationController : Controller
    {
        private readonly static log4net.ILog LOG = log4net.LogManager.GetLogger("BlanketWarehouseMutationController");
        private IItemService _itemService;
        private IWarehouseItemService _warehouseItemService;
        private IStockMutationService _stockMutationService;
        private IBlanketService _blanketService;
        private IBlanketWarehouseMutationService _blanketWarehouseMutationService;
        private IBlanketWarehouseMutationDetailService _blanketWarehouseMutationDetailService;
        private IContactService _contactService;
        private IPurchaseReceivalDetailService _purchaseReceivalDetailService;
        private IPurchaseReceivalService _purchaseReceivalService;
        private ICoreIdentificationDetailService _coreIdentificationDetailService;
        private ICoreIdentificationService _coreIdentificationService;
        private IBlanketOrderDetailService _blanketOrderDetailService;
        private IBlanketOrderService _blanketOrderService;
        private IWarehouseService _warehouseService;
        private ICoreBuilderService _coreBuilderService;
        private IRollerBuilderService _rollerBuilderService;
        private ICustomerStockMutationService _customerStockMutationService;
        private ICustomerItemService _customerItemService;

        public BlanketWarehouseMutationController()
        {
            _itemService = new ItemService(new ItemRepository(), new ItemValidator());
            _warehouseItemService = new WarehouseItemService(new WarehouseItemRepository(), new WarehouseItemValidator());
            _stockMutationService = new StockMutationService(new StockMutationRepository(), new StockMutationValidator());
            _blanketService = new BlanketService(new BlanketRepository(), new BlanketValidator());
            _blanketWarehouseMutationService = new BlanketWarehouseMutationService(new BlanketWarehouseMutationRepository(), new BlanketWarehouseMutationValidator());
            _blanketWarehouseMutationDetailService = new BlanketWarehouseMutationDetailService(new BlanketWarehouseMutationDetailRepository(), new BlanketWarehouseMutationDetailValidator());
            _contactService = new ContactService(new ContactRepository(), new ContactValidator());
            _purchaseReceivalService = new PurchaseReceivalService(new PurchaseReceivalRepository(), new PurchaseReceivalValidator());
            _purchaseReceivalDetailService = new PurchaseReceivalDetailService(new PurchaseReceivalDetailRepository(), new PurchaseReceivalDetailValidator());
            _coreIdentificationService = new CoreIdentificationService(new CoreIdentificationRepository(), new CoreIdentificationValidator());
            _coreIdentificationDetailService = new CoreIdentificationDetailService(new CoreIdentificationDetailRepository(), new CoreIdentificationDetailValidator());
            _warehouseService = new WarehouseService(new WarehouseRepository(), new WarehouseValidator());
            _coreBuilderService = new CoreBuilderService(new CoreBuilderRepository(), new CoreBuilderValidator());
            _rollerBuilderService = new RollerBuilderService(new RollerBuilderRepository(), new RollerBuilderValidator());
            _blanketOrderDetailService = new BlanketOrderDetailService(new BlanketOrderDetailRepository(), new BlanketOrderDetailValidator());
            _blanketOrderService = new BlanketOrderService(new BlanketOrderRepository(), new BlanketOrderValidator());
            _customerStockMutationService = new CustomerStockMutationService(new CustomerStockMutationRepository(), new CustomerStockMutationValidator());
            _customerItemService = new CustomerItemService(new CustomerItemRepository(), new CustomerItemValidator());
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
            var q = _blanketWarehouseMutationService.GetQueryable().Include("BlanketOrder").Include("Warehouse")
                                                   .Where(x => !x.IsDeleted);

            var query = (from model in q
                         select new
                         {
                             model.Id,
                             model.Code,
                             model.BlanketOrderId,
                             BlanketOrderCode = model.BlanketOrder.Code,
                             model.WarehouseFromId,
                             WarehouseFromCode = model.WarehouseFrom.Code,
                             WarehouseFrom = model.WarehouseFrom.Name,
                             model.WarehouseToId,
                             WarehouseToCode = model.WarehouseTo.Code,
                             WarehouseTo = model.WarehouseTo.Name,
                             model.Quantity,
                             model.MutationDate,
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
                            model.BlanketOrderId,
                            model.BlanketOrderCode,
                            model.WarehouseFromId,
                            model.WarehouseFromCode,
                            model.WarehouseFrom,
                            model.WarehouseToId,
                            model.WarehouseToCode,
                            model.WarehouseTo,
                            model.Quantity,
                            model.MutationDate,
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
            var q = _blanketWarehouseMutationDetailService.GetQueryable().Include("BlanketOrderDetail")
                                                         .Include("Item")//.Include("CoreIdentificationDetail")
                                                         .Where(x => x.BlanketWarehouseMutationId == id && !x.IsDeleted);

            var query = (from model in q
                         select new
                         {
                             model.Id,
                             //DetailId = model.BlanketOrderDetail.CoreIdentificationDetail.DetailId,
                             model.Code,
                             model.BlanketWarehouseMutationId,
                             model.BlanketOrderDetailId,
                             model.ItemId,
                             ItemSku = model.Item.Sku,
                             Item = model.Item.Name,
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
                            //model.DetailId,
                            model.Code,
                            model.BlanketWarehouseMutationId,
                            model.BlanketOrderDetailId,
                            model.ItemId,
                            model.ItemSku,
                            model.Item,
                      }
                    }).ToArray()
            }, JsonRequestBehavior.AllowGet);
        }


        public dynamic GetInfo(int Id)
        {
            BlanketWarehouseMutation model = new BlanketWarehouseMutation();
            try
            {
                model = _blanketWarehouseMutationService.GetObjectById(Id);
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
                model.BlanketOrderId,
                BlanketOrder = _blanketOrderService.GetObjectById(model.BlanketOrderId).Code,
                model.WarehouseFromId,
                WarehouseFromCode = _warehouseService.GetObjectById(model.WarehouseFromId).Code,
                WarehouseFrom = _warehouseService.GetObjectById(model.WarehouseFromId).Name,
                model.WarehouseToId,
                model.MutationDate,
                WarehouseToCode = _warehouseService.GetObjectById(model.WarehouseToId).Code,
                WarehouseTo = _warehouseService.GetObjectById(model.WarehouseToId).Name,
                model.Quantity,
                model.Errors
            }, JsonRequestBehavior.AllowGet);
        }

        public dynamic GetInfoDetail(int Id)
        {
            BlanketWarehouseMutationDetail model = new BlanketWarehouseMutationDetail();
            try
            {
                model = _blanketWarehouseMutationDetailService.GetObjectById(Id);
            
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
                model.BlanketWarehouseMutationId,
                model.BlanketOrderDetailId, 
                model.ItemId,
                ItemSku = _itemService.GetObjectById(model.ItemId).Sku,
                Item = _itemService.GetObjectById(model.ItemId).Name,
                //RIFID = _coreIdentificationDetailService.GetObjectById(_blanketOrderDetailService.GetObjectById(model.BlanketOrderDetailId).CoreIdentificationDetailId).DetailId,
                model.Errors
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public dynamic Insert(BlanketWarehouseMutation model)
        {
            try
            {
                model = _blanketWarehouseMutationService.CreateObject(model,_warehouseService,_blanketOrderService);
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
        public dynamic InsertDetail(BlanketWarehouseMutationDetail model)
        {
            try
            {
                var data = _blanketOrderDetailService.GetObjectById(model.BlanketOrderDetailId);
                model.ItemId = data.BlanketId;
                //var MaterialCase = _coreIdentificationDetailService.GetObjectById(_blanketOrderDetailService.GetObjectById(model.BlanketOrderDetailId).CoreIdentificationDetailId).MaterialCase;
                //var item = MaterialCase == Core.Constants.Constant.MaterialCase.New ? _rollerBuilderService.GetRollerNewCore(data.RollerBuilderId) :
                //            _rollerBuilderService.GetRollerUsedCore(data.RollerBuilderId);
                //model.ItemId = item.Id;
                model = _blanketWarehouseMutationDetailService.CreateObject(model, _blanketWarehouseMutationService,
                    _blanketOrderDetailService, _coreIdentificationDetailService, _itemService, _warehouseItemService);
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
        public dynamic Update(BlanketWarehouseMutation model)
        {
            try
            {
                var data = _blanketWarehouseMutationService.GetObjectById(model.Id);
                data.BlanketOrderId = model.BlanketOrderId;
                data.WarehouseFromId = model.WarehouseFromId;
                data.WarehouseToId = model.WarehouseToId;
                data.Quantity = model.Quantity;
                data.MutationDate = model.MutationDate;
                model = _blanketWarehouseMutationService.UpdateObject(data,_warehouseService, _blanketOrderService);
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
        public dynamic Delete(BlanketWarehouseMutation model)
        {
            try
            {
                var data = _blanketWarehouseMutationService.GetObjectById(model.Id);
                model = _blanketWarehouseMutationService.SoftDeleteObject(data);
            }
            catch (Exception ex)
            {
                LOG.Error("Delete Failed", ex);
                if (!model.Errors.ContainsKey("Generic")) { model.Errors.Add("Generic", "Error : " + ex); }
            }

            return Json(new
            {
                model.Errors
            });
        }

        [HttpPost]
        public dynamic DeleteDetail(BlanketWarehouseMutationDetail model)
        {
            try
            {
                var data = _blanketWarehouseMutationDetailService.GetObjectById(model.Id);
                model = _blanketWarehouseMutationDetailService.SoftDeleteObject(data,_blanketWarehouseMutationService,_warehouseItemService);
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
        public dynamic UpdateDetail(BlanketWarehouseMutationDetail model)
        {
            try
            {
                var data2 = _blanketWarehouseMutationDetailService.GetObjectById(model.Id);
                data2.BlanketOrderDetailId = model.BlanketOrderDetailId;
                model = _blanketWarehouseMutationDetailService.UpdateObject(data2,_blanketWarehouseMutationService,
                    _blanketOrderDetailService,_coreIdentificationDetailService,_itemService,_warehouseItemService);
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
        public dynamic Confirm(BlanketWarehouseMutation model)
        {
            try
            {
                var data = _blanketWarehouseMutationService.GetObjectById(model.Id);
                model = _blanketWarehouseMutationService.ConfirmObject(data,model.ConfirmationDate.Value
                    ,_blanketWarehouseMutationDetailService,_itemService,_blanketService
                    ,_warehouseItemService,_stockMutationService,_blanketOrderDetailService, _blanketOrderService,_coreIdentificationDetailService,_coreIdentificationService
                    ,_customerStockMutationService, _customerItemService);
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
        public dynamic UnConfirm(BlanketWarehouseMutation model)
        {
            try
            {

                var data = _blanketWarehouseMutationService.GetObjectById(model.Id);
                model = _blanketWarehouseMutationService.UnconfirmObject(data,_blanketWarehouseMutationDetailService,
                    _itemService,_blanketService,_warehouseItemService,_stockMutationService,_blanketOrderDetailService,
                    _blanketOrderService,_coreIdentificationDetailService,_coreIdentificationService,
                    _customerStockMutationService, _customerItemService);
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
