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
    public class MstRollerBuilderController : Controller
    {
        private readonly static log4net.ILog LOG = log4net.LogManager.GetLogger("RollerBuilderController");
        private ICoreBuilderService _coreBuilderService;
        private IItemService _itemService;
        private IUoMService _uomService;
        private IWarehouseService _warehouseService;
        private IItemTypeService _itemTypeService;
        private IRollerBuilderService _rollerBuilderService;
        private IRecoveryOrderDetailService _recoveryOrderDetailService;
        private IWarehouseItemService _warehouseItemService;
        private IStockMutationService _stockMutationService;
        private IMachineService _machineService;
        private IRollerTypeService _rollerTypeService;
        private IBlanketService _blanketService;
        private IContactService _contactService;
        private IPriceMutationService _priceMutationService;
        private IPurchaseOrderDetailService _purchaseOrderDetailService;
        private IStockAdjustmentDetailService _stockAdjustmentDetailService;
        private ISalesOrderDetailService _salesOrderDetailService;
        private IBlanketOrderDetailService _blanketOrderDetailService;

        public MstRollerBuilderController()
        {
            _coreBuilderService = new CoreBuilderService(new CoreBuilderRepository(), new CoreBuilderValidator());
            _itemTypeService = new ItemTypeService(new ItemTypeRepository(), new ItemTypeValidator());
            _rollerBuilderService = new RollerBuilderService(new RollerBuilderRepository(), new RollerBuilderValidator());
            _recoveryOrderDetailService = new RecoveryOrderDetailService(new RecoveryOrderDetailRepository(), new RecoveryOrderDetailValidator());
            _warehouseItemService = new WarehouseItemService(new WarehouseItemRepository(), new WarehouseItemValidator());
            _stockMutationService = new StockMutationService(new StockMutationRepository(), new StockMutationValidator());
            _itemService = new ItemService(new ItemRepository(), new ItemValidator());
            _uomService = new UoMService(new UoMRepository(), new UoMValidator());
            _warehouseService = new WarehouseService(new WarehouseRepository(), new WarehouseValidator());
            _machineService = new MachineService(new MachineRepository(), new MachineValidator());
            _rollerTypeService = new RollerTypeService(new RollerTypeRepository(), new RollerTypeValidator());
            _blanketService = new BlanketService(new BlanketRepository(), new BlanketValidator());
            _contactService = new ContactService(new ContactRepository(), new ContactValidator());
            _priceMutationService = new PriceMutationService(new PriceMutationRepository(), new PriceMutationValidator());
            _purchaseOrderDetailService = new PurchaseOrderDetailService(new PurchaseOrderDetailRepository(), new PurchaseOrderDetailValidator());
            _stockAdjustmentDetailService = new StockAdjustmentDetailService(new StockAdjustmentDetailRepository(), new StockAdjustmentDetailValidator());
            _salesOrderDetailService = new SalesOrderDetailService(new SalesOrderDetailRepository(),new SalesOrderDetailValidator());
            _blanketOrderDetailService = new BlanketOrderDetailService(new BlanketOrderDetailRepository(), new BlanketOrderDetailValidator());
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
            var q = _rollerBuilderService.GetQueryable().Include("Item").Include("RollerType")
                                                        .Include("UoM").Include("Machine")
                                                        .Include("CoreBuilder").Where(x => !x.IsDeleted);

            var query = (from model in q
                         select new
                         {
                             model.Id,
                             model.BaseSku,
                             model.Name,
                             RollerType = model.RollerType.Name,
                             model.RD,
                             model.CD,
                             model.RL,
                             model.WL,
                             model.TL,
                             model.SkuRollerUsedCore,
                             RollerUsedCoreItemQuantity = model.RollerUsedCoreItem.Quantity,
                             model.SkuRollerNewCore,
                             RollerNewCoreItemQuantity = model.RollerNewCoreItem.Quantity,
                             UoM = model.UoM.Name,
                             Machine = model.Machine.Name,
                             Compound = model.Compound.Name,
                             Adhesive = model.Adhesive.Name,
                             CoreBuilderBaseSku = model.CoreBuilder.BaseSku,
                             CoreBuilder = model.CoreBuilder.Name,
                             model.Description,
                             model.IsCrowning,
                             model.CrowningSize,
                             model.IsGrooving,
                             model.GroovingWidth,
                             model.GroovingDepth,
                             model.GroovingPosition,
                             model.IsChamfer,
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
                            model.BaseSku,
                            model.Name,
                            model.RollerType,
                            model.RD,
                            model.CD,
                            model.RL,
                            model.WL,
                            model.TL,
                            model.SkuRollerUsedCore,
                            model.RollerUsedCoreItemQuantity,
                            model.UoM,
                            model.SkuRollerNewCore,
                            model.RollerNewCoreItemQuantity,
                            model.UoM,
                            model.Machine,
                            model.Compound,
                            model.Adhesive,
                            model.CoreBuilderBaseSku,
                            model.CoreBuilder,
                            model.Description,
                            model.IsCrowning,
                            model.CrowningSize,
                            model.IsGrooving,
                            model.GroovingWidth,
                            model.GroovingDepth,
                            model.GroovingPosition,
                            model.IsChamfer,
                            model.CreatedAt,
                            model.UpdatedAt,
                      }
                    }).ToArray()
            }, JsonRequestBehavior.AllowGet);
        }

        public dynamic GetListCompound(string _search, long nd, int rows, int? page, string sidx, string sord, string filters = "")
        {
            // Construct where statement
            string strWhere = GeneralFunction.ConstructWhere(filters);
            string filter = null;
            GeneralFunction.ConstructWhereInLinq(strWhere, out filter);
            if (filter == "") filter = "true";

            // Get Data
            var q = _itemService.GetQueryable().Include("ItemType").Where(x => !x.IsDeleted && x.ItemType.Name == Core.Constants.Constant.ItemTypeCase.Compound);

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
                            model.CreatedAt,
                            model.UpdatedAt
                      }
                    }).ToArray()
            }, JsonRequestBehavior.AllowGet);
        }

        public dynamic GetListAdhesiveRoller(string _search, long nd, int rows, int? page, string sidx, string sord, string filters = "")
        {
            // Construct where statement
            string strWhere = GeneralFunction.ConstructWhere(filters);
            string filter = null;
            GeneralFunction.ConstructWhereInLinq(strWhere, out filter);
            if (filter == "") filter = "true";

            // Get Data
            var q = _itemService.GetQueryable().Include("ItemType").Where(x => !x.IsDeleted && x.ItemType.Name == Core.Constants.Constant.ItemTypeCase.AdhesiveRoller);

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
                            model.CreatedAt,
                            model.UpdatedAt
                      }
                    }).ToArray()
            }, JsonRequestBehavior.AllowGet);
        }

        public dynamic GetInfo(int Id)
        {
            RollerBuilder model = new RollerBuilder();
            try
            {
                model = _rollerBuilderService.GetObjectById(Id);
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
                model.UoMId,
                UoM = _uomService.GetObjectById(model.UoMId).Name,
                model.BaseSku,
                model.SkuRollerUsedCore,
                model.SkuRollerNewCore,
                model.MachineId,
                Machine = _machineService.GetObjectById(model.MachineId).Name,
                model.RollerTypeId,
                RollerType = _rollerTypeService.GetObjectById(model.RollerTypeId).Name,
                model.CompoundId,
                Compound = _itemService.GetObjectById(model.CompoundId).Name,
                model.AdhesiveId,
                Adhesive = model.AdhesiveId == null ? "" : _itemService.GetObjectById(model.AdhesiveId.GetValueOrDefault()).Name,
                model.CoreBuilderId,
                CoreBuilder = _coreBuilderService.GetObjectById(model.CoreBuilderId).Name,
                model.RD,
                model.CD,
                model.RL,
                model.WL,
                model.TL,
                RollerUsedCoreQuantity = _itemService.GetObjectById(model.RollerUsedCoreItemId).Quantity,
                RollerNewCoreQuantity =_itemService.GetObjectById(model.RollerNewCoreItemId).Quantity,
                model.IsCrowning,
                model.CrowningSize,
                model.IsGrooving,
                model.GroovingWidth,
                model.GroovingDepth,
                model.GroovingPosition,
                model.IsChamfer,
                model.Errors
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public dynamic Insert(RollerBuilder model)
        {
            try
            {
                model = _rollerBuilderService.CreateObject(model,_machineService,
                    _uomService,_itemService,_itemTypeService,_coreBuilderService,
                    _rollerTypeService,_warehouseItemService,_warehouseService,_priceMutationService);
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
        public dynamic Update(RollerBuilder model)
        {
            try
            {
                var data = _rollerBuilderService.GetObjectById(model.Id);
                data.Name = model.Name;
                data.BaseSku = model.BaseSku;
                data.SkuRollerNewCore = model.SkuRollerNewCore;
                data.SkuRollerUsedCore = model.SkuRollerUsedCore;
                data.MachineId = model.MachineId;
                data.RollerTypeId = model.RollerTypeId;
                data.CompoundId = model.CompoundId;
                data.AdhesiveId = model.AdhesiveId;
                data.UoMId = model.UoMId;
                data.CoreBuilderId = model.CoreBuilderId;
                data.Description = model.Description;
                data.RD = model.RD;
                data.CD = model.CD;
                data.RL = model.RL;
                data.WL = model.WL;
                data.TL = model.TL;
                data.IsCrowning = model.IsCrowning;
                data.CrowningSize = model.CrowningSize;
                data.IsGrooving = model.IsGrooving;
                data.GroovingWidth = model.GroovingWidth;
                data.GroovingDepth = model.GroovingDepth;
                data.GroovingPosition = model.GroovingPosition;
                data.IsChamfer = model.IsChamfer;

                model = _rollerBuilderService.UpdateObject(data, _machineService, _uomService, _itemService,
                    _itemTypeService,_coreBuilderService,_rollerTypeService,_warehouseItemService,_warehouseService,
                    _blanketService,_contactService,_priceMutationService);
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
        public dynamic Delete(RollerBuilder model)
        {
            try
            {
                var data = _rollerBuilderService.GetObjectById(model.Id);
                model = _rollerBuilderService.SoftDeleteObject(data,_itemService,_blanketService,
                    _priceMutationService,_recoveryOrderDetailService,_coreBuilderService,
                    _warehouseItemService,_stockMutationService,_itemTypeService,_purchaseOrderDetailService,
                    _stockAdjustmentDetailService,_salesOrderDetailService, _blanketOrderDetailService);
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
