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
    public class BlendingWorkOrderController : Controller
    {
        private readonly static log4net.ILog LOG = log4net.LogManager.GetLogger("BlendingWorkOrderController");
        private IItemService _itemService;
        private IItemTypeService _itemTypeService;
        private IWarehouseItemService _warehouseItemService;
        private IStockMutationService _stockMutationService;
        private IBlanketService _blanketService;
        private IWarehouseService _warehouseService;
        private IBlendingWorkOrderService _blendingWorkOrderService;
        private IBlendingRecipeService _blendingRecipeService;
        private IBlendingRecipeDetailService _blendingRecipeDetailService;
        private IContactService _contactService;
        private IAccountService _accountService;
        private IGeneralLedgerJournalService _generalLedgerJournalService;
        private IClosingService _closingService;

        public BlendingWorkOrderController()
        {
            _itemService = new ItemService(new ItemRepository(), new ItemValidator());
            _itemTypeService = new ItemTypeService(new ItemTypeRepository(), new ItemTypeValidator());
            _warehouseItemService = new WarehouseItemService(new WarehouseItemRepository(), new WarehouseItemValidator());
            _stockMutationService = new StockMutationService(new StockMutationRepository(), new StockMutationValidator());
            _blanketService = new BlanketService(new BlanketRepository(), new BlanketValidator());
            _contactService = new ContactService(new ContactRepository(), new ContactValidator());
            _warehouseService = new WarehouseService(new WarehouseRepository(), new WarehouseValidator());
            _blendingWorkOrderService = new BlendingWorkOrderService(new BlendingWorkOrderRepository(), new BlendingWorkOrderValidator());
            _blendingRecipeService = new BlendingRecipeService(new BlendingRecipeRepository(), new BlendingRecipeValidator());
            _blendingRecipeDetailService = new BlendingRecipeDetailService(new BlendingRecipeDetailRepository(), new BlendingRecipeDetailValidator());
            _accountService = new AccountService(new AccountRepository(), new AccountValidator());
            _generalLedgerJournalService = new GeneralLedgerJournalService(new GeneralLedgerJournalRepository(), new GeneralLedgerJournalValidator());
            _closingService = new ClosingService(new ClosingRepository(), new ClosingValidator());
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
            var q = _blendingWorkOrderService.GetQueryable().Where(x => !x.IsDeleted);

            var query = (from model in q
                         select new
                         {
                             model.Id,
                             model.Code,
                             model.BlendingRecipeId,
                             BlendingRecipe = model.BlendingRecipe.Name,
                             TargetItemSku = model.BlendingRecipe.TargetItem.Sku,
                             TargetItem = model.BlendingRecipe.TargetItem.Name,
                             model.BlendingRecipe.TargetQuantity,
                             UoM = model.BlendingRecipe.TargetItem.UoM.Name,
                             model.WarehouseId,
                             Warehouse = model.Warehouse.Name,
                             model.Description,
                             model.ConfirmationDate,
                             model.BlendingDate,
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
                             model.BlendingRecipeId,
                             model.BlendingRecipe,
                             model.TargetItemSku,
                             model.TargetItem,
                             model.TargetQuantity,
                             model.UoM,
                             model.WarehouseId,
                             model.Warehouse,
                             model.Description,
                             model.ConfirmationDate,
                             model.BlendingDate,
                             model.CreatedAt,
                             model.UpdatedAt,
                      }
                    }).ToArray()
            }, JsonRequestBehavior.AllowGet);
        }

        public dynamic GetListNonConfirmed(string _search, long nd, int rows, int? page, string sidx, string sord, string filters = "")
        {
            // Construct where statement
            string strWhere = GeneralFunction.ConstructWhere(filters);
            string filter = null;
            GeneralFunction.ConstructWhereInLinq(strWhere, out filter);
            if (filter == "") filter = "true";

            // Get Data
            var q = _blendingWorkOrderService.GetQueryable().Where(x => !x.IsDeleted && !x.IsConfirmed);

            var query = (from model in q
                         select new
                         {
                             model.Id,
                             model.Code,
                             model.BlendingRecipeId,
                             BlendingRecipe = model.BlendingRecipe.Name,
                             TargetItemSku = model.BlendingRecipe.TargetItem.Sku,
                             TargetItem = model.BlendingRecipe.TargetItem.Name,
                             model.BlendingRecipe.TargetQuantity,
                             UoM = model.BlendingRecipe.TargetItem.UoM.Name,
                             model.WarehouseId,
                             Warehouse = model.Warehouse.Name,
                             model.Description,
                             model.ConfirmationDate,
                             model.BlendingDate,
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
                             model.BlendingRecipeId,
                             model.BlendingRecipe,
                             model.TargetItemSku,
                             model.TargetItem,
                             model.TargetQuantity,
                             model.UoM,
                             model.WarehouseId,
                             model.Warehouse,
                             model.Description,
                             model.ConfirmationDate,
                             model.BlendingDate,
                             model.CreatedAt,
                             model.UpdatedAt,
                      }
                    }).ToArray()
            }, JsonRequestBehavior.AllowGet);
        }

        public dynamic GetInfo(int Id)
        {
            BlendingWorkOrder model = new BlendingWorkOrder();
            try
            {
                model = _blendingWorkOrderService.GetObjectById(Id);
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
                model.BlendingRecipeId,
                BlendingRecipe = model.BlendingRecipe.Name,
                TargetItem = model.BlendingRecipe.TargetItem.Name,
                model.BlendingRecipe.TargetQuantity,
                UoM = model.BlendingRecipe.TargetItem.UoM.Name,
                model.WarehouseId,
                Warehouse = model.Warehouse.Name,
                model.Description,
                model.ConfirmationDate,
                model.BlendingDate,
                model.CreatedAt,
                model.UpdatedAt,
                model.Errors
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public dynamic Insert(BlendingWorkOrder model)
        {
            try
            {
                model = _blendingWorkOrderService.CreateObject(model, _blendingRecipeService, _warehouseService);
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
        public dynamic Update(BlendingWorkOrder model)
        {
            try
            {
                var data = _blendingWorkOrderService.GetObjectById(model.Id);
                data.BlendingRecipeId = model.BlendingRecipeId;
                data.WarehouseId = model.WarehouseId;
                data.Code = model.Code;
                data.Description = model.Description;
                data.BlendingDate = model.BlendingDate;
                model = _blendingWorkOrderService.UpdateObject(data, _blendingRecipeService, _warehouseService);
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
        public dynamic Delete(BlendingWorkOrder model)
        {
            try
            {
                var data = _blendingWorkOrderService.GetObjectById(model.Id);
                model = _blendingWorkOrderService.SoftDeleteObject(data);
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
        public dynamic Confirm(BlendingWorkOrder model)
        {
            try
            {
                var data = _blendingWorkOrderService.GetObjectById(model.Id);
                model = _blendingWorkOrderService.ConfirmObject(data, model.ConfirmationDate.Value, _blendingRecipeService, _blendingRecipeDetailService,
                            _stockMutationService,_blanketService,_itemService, _itemTypeService, _warehouseItemService,
                            _generalLedgerJournalService, _accountService, _closingService);
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
        public dynamic UnConfirm(BlendingWorkOrder model)
        {
            try
            {

                var data = _blendingWorkOrderService.GetObjectById(model.Id);
                model = _blendingWorkOrderService.UnconfirmObject(data, _blendingRecipeService, _blendingRecipeDetailService, 
                            _stockMutationService, _blanketService, _itemService, _itemTypeService, _warehouseItemService,
                            _generalLedgerJournalService, _accountService, _closingService);
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