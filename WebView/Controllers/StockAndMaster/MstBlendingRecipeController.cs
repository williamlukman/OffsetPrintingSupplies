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
using Core.Constants;

namespace WebView.Controllers
{
    public class MstBlendingRecipeController : Controller
    {
        private readonly static log4net.ILog LOG = log4net.LogManager.GetLogger("BlendingRecipeController");
        private IItemService _itemService;
        private IUoMService _uomService;
        private IWarehouseService _warehouseService;
        private IItemTypeService _itemTypeService;
        private IMachineService _machineService;
        private IWarehouseItemService _warehouseItemService;
        private IBlendingRecipeService _blendingRecipeService;
        private IContactService _contactService;
        private IPriceMutationService _priceMutationService;
        private IPurchaseOrderDetailService _purchaseOrderDetailService;
        private IStockAdjustmentDetailService _stockAdjustmentDetailService;
        private ISalesOrderDetailService _salesOrderDetailService;
        private IStockMutationService _stockMutationService;
        private IBlendingRecipeDetailService _blendingRecipeDetailService;
        private IBlendingWorkOrderService _blendingWorkOrderService;

        public MstBlendingRecipeController()
        {
            _itemTypeService = new ItemTypeService(new ItemTypeRepository(), new ItemTypeValidator());
            _warehouseItemService = new WarehouseItemService(new WarehouseItemRepository(), new WarehouseItemValidator());
            _itemService = new ItemService(new ItemRepository(), new ItemValidator());
            _uomService = new UoMService(new UoMRepository(), new UoMValidator());
            _warehouseService = new WarehouseService(new WarehouseRepository(), new WarehouseValidator());
             _machineService = new MachineService(new MachineRepository(), new MachineValidator());
            _blendingRecipeService = new BlendingRecipeService(new BlendingRecipeRepository(), new BlendingRecipeValidator());
            _contactService = new ContactService(new ContactRepository(),new ContactValidator());
            _priceMutationService = new PriceMutationService(new PriceMutationRepository(), new PriceMutationValidator());
            _purchaseOrderDetailService = new PurchaseOrderDetailService(new PurchaseOrderDetailRepository(), new PurchaseOrderDetailValidator());
            _stockAdjustmentDetailService = new StockAdjustmentDetailService(new StockAdjustmentDetailRepository(), new StockAdjustmentDetailValidator());
            _salesOrderDetailService = new SalesOrderDetailService(new SalesOrderDetailRepository(), new SalesOrderDetailValidator());
            _stockMutationService = new StockMutationService(new StockMutationRepository(), new StockMutationValidator());
            _blendingRecipeDetailService = new BlendingRecipeDetailService(new BlendingRecipeDetailRepository(), new BlendingRecipeDetailValidator());
            _blendingWorkOrderService = new BlendingWorkOrderService(new BlendingWorkOrderRepository(), new BlendingWorkOrderValidator());
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
            var q = _blendingRecipeService.GetQueryable().Where(x => !x.IsDeleted);

            var query = (from model in q
                         select new
                         {
                            model.Id,
                            model.Name,
                            model.Description,
                            model.TargetItemId,
                            TargetItemSku = model.TargetItem.Sku,
                            TargetItem = model.TargetItem.Name,
                            model.TargetQuantity,
                            UoM = model.TargetItem.UoM.Name,
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
                            model.Name,
                            model.Description,
                            model.TargetItemId,
                            model.TargetItemSku,
                            model.TargetItem,
                            model.TargetQuantity,
                            model.UoM,
                            model.CreatedAt,
                            model.UpdatedAt,
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
            var q = _blendingRecipeDetailService.GetQueryable().Where(x => !x.IsDeleted && x.BlendingRecipeId == id);

            var query = (from model in q
                         select new
                         {
                             model.Id,
                             model.ItemId,
                             ItemSku = model.Item.Sku,
                             ItemName = model.Item.Name,
                             model.Quantity,
                             UoM = model.Item.UoM.Name,
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
                             model.ItemId,
                             model.ItemSku,
                             model.ItemName,
                             model.Quantity,
                             model.UoM,
                             model.CreatedAt,
                             model.UpdatedAt,
                      }
                    }).ToArray()
            }, JsonRequestBehavior.AllowGet);
        }

        public dynamic GetItemListChemical(string _search, long nd, int rows, int? page, string sidx, string sord, string filters = "")
        {
            // Construct where statement
            string strWhere = GeneralFunction.ConstructWhere(filters);
            string filter = null;
            GeneralFunction.ConstructWhereInLinq(strWhere, out filter);
            if (filter == "") filter = "true";

            // Get Data
            var q = _itemService.GetQueryable().Where(x => !x.IsDeleted && x.ItemType.Name == Constant.ItemTypeCase.Chemical);

            var query = (from model in q
                         select new
                         {
                             model.Id,
                             model.Sku,
                             model.Name,
                             model.Description,
                             model.Quantity,
                             model.PendingReceival,
                             model.PendingDelivery,
                             model.UoMId,
                             UoM = model.UoM.Name,
                             ItemType = model.ItemType.Name,
                             model.CreatedAt,
                             model.UpdatedAt
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
                            model.Sku,
                            model.Name,
                            model.Description,
                            model.Quantity,
                            model.PendingReceival,
                            model.PendingDelivery,
                            model.UoMId,
                            model.UoM,
                            model.ItemType,
                            model.CreatedAt,
                            model.UpdatedAt
                      }
                    }).ToArray()
            }, JsonRequestBehavior.AllowGet);
        }

        public dynamic GetInfo(int Id)
        {
            BlendingRecipe model = new BlendingRecipe();
            try
            {
                model = _blendingRecipeService.GetObjectById(Id);
            }
            catch (Exception ex)
            {
                LOG.Error("GetInfo", ex);
                model.Errors.Add("Generic", "Error : " + ex);
            }

            return Json(new
            {
                model.Id,
                model.Name,
                model.Description,
                model.TargetItemId,
                model.TargetItem.Sku,
                TargetItem = model.TargetItem.Name,
                model.TargetQuantity,
                model.TargetItem.UoMId,
                UoM = model.TargetItem.UoM.Name,
                model.CreatedAt,
                model.UpdatedAt,
                model.Errors
            }, JsonRequestBehavior.AllowGet);
        }

        public dynamic GetInfoDetail(int Id)
        {
            BlendingRecipeDetail model = new BlendingRecipeDetail();
            try
            {
                model = _blendingRecipeDetailService.GetObjectById(Id);
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
                model.Item.Sku,
                Item = model.Item.Name,
                model.Quantity,
                model.Item.UoMId,
                UoM = model.Item.UoM.Name,
                model.CreatedAt,
                model.UpdatedAt,
                model.Errors
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public dynamic Insert(BlendingRecipe model)
        {
            try
            {
                model = _blendingRecipeService.CreateObject(model, _itemService);
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
        public dynamic InsertDetail(BlendingRecipeDetail model)
        {
            try
            {
                model = _blendingRecipeDetailService.CreateObject(model, _blendingRecipeService, _itemService);
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
        public dynamic Update(BlendingRecipe model)
        {
            try
            {
                var data = _blendingRecipeService.GetObjectById(model.Id);
                data.Name = model.Name;
                data.Description = model.Description;
                data.TargetItemId = model.TargetItemId;
                data.TargetQuantity = model.TargetQuantity;
                model = _blendingRecipeService.UpdateObject(data, _itemService);
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
        public dynamic UpdateDetail(BlendingRecipeDetail model)
        {
            try
            {
                var data = _blendingRecipeDetailService.GetObjectById(model.Id);
                //data.BlendingRecipeId = model.BlendingRecipeId;
                data.ItemId = model.ItemId;
                data.Quantity = model.Quantity;
                model = _blendingRecipeDetailService.UpdateObject(data, _blendingRecipeService, _itemService);
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
        public dynamic Delete(BlendingRecipe model)
        {
            try
            {
                var data = _blendingRecipeService.GetObjectById(model.Id);
                model = _blendingRecipeService.SoftDeleteObject(data, _blendingRecipeDetailService, _blendingWorkOrderService);
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
        public dynamic DeleteDetail(BlendingRecipeDetail model)
        {
            try
            {
                var data = _blendingRecipeDetailService.GetObjectById(model.Id);
                model = _blendingRecipeDetailService.SoftDeleteObject(data);
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

    }
}
