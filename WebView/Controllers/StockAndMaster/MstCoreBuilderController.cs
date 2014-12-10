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
    public class MstCoreBuilderController : Controller
    {
        private readonly static log4net.ILog LOG = log4net.LogManager.GetLogger("ContactController");
        private ICoreBuilderService _coreBuilderService;
        private IItemService _itemService;
        private IUoMService _uomService;
        private IWarehouseService _warehouseService;
        private IItemTypeService _itemTypeService;
        private IRollerBuilderService _rollerBuilderService;
        private ICoreIdentificationDetailService _coreIdentificationDetailService;
        private IRecoveryOrderDetailService _recoveryOrderDetailService;
        private IRecoveryAccessoryDetailService _recoveryAccessoryDetailService;
        private IWarehouseItemService _warehouseItemService;
        private IStockMutationService _stockMutationService;
        private IBlanketService _blanketService;
        private IContactService _contactService;
        private IPriceMutationService _priceMutationService;
        private IPurchaseOrderDetailService _purchaseOrderDetailService;
        private IStockAdjustmentDetailService _stockAdjustmentDetailService;
        private ISalesOrderDetailService _salesOrderDetailService;
        private IMachineService _machineService;
        private IBlanketOrderDetailService _blanketOrderDetailService;

        private IRollerTypeService _rollerTypeService;

        public MstCoreBuilderController()
        {
            _coreBuilderService = new CoreBuilderService(new CoreBuilderRepository(), new CoreBuilderValidator());
            _itemTypeService = new ItemTypeService(new ItemTypeRepository(), new ItemTypeValidator());
            _rollerBuilderService = new RollerBuilderService(new RollerBuilderRepository(), new RollerBuilderValidator());
            _coreIdentificationDetailService = new CoreIdentificationDetailService(new CoreIdentificationDetailRepository(), new CoreIdentificationDetailValidator());
            _recoveryOrderDetailService = new RecoveryOrderDetailService(new RecoveryOrderDetailRepository(), new RecoveryOrderDetailValidator());
            _recoveryAccessoryDetailService = new RecoveryAccessoryDetailService(new RecoveryAccessoryDetailRepository(), new RecoveryAccessoryDetailValidator());
            _warehouseItemService = new WarehouseItemService(new WarehouseItemRepository(), new WarehouseItemValidator());
            _stockMutationService = new StockMutationService(new StockMutationRepository(), new StockMutationValidator());
            _itemService = new ItemService(new ItemRepository(), new ItemValidator());
            _uomService = new UoMService(new UoMRepository(), new UoMValidator());
            _warehouseService = new WarehouseService(new WarehouseRepository(), new WarehouseValidator());
            _blanketService = new BlanketService(new BlanketRepository(), new BlanketValidator());
            _contactService = new ContactService(new ContactRepository(), new ContactValidator());
            _priceMutationService = new PriceMutationService(new PriceMutationRepository(), new PriceMutationValidator());
            _purchaseOrderDetailService = new PurchaseOrderDetailService(new PurchaseOrderDetailRepository(), new PurchaseOrderDetailValidator());
            _stockAdjustmentDetailService = new StockAdjustmentDetailService(new StockAdjustmentDetailRepository(), new StockAdjustmentDetailValidator());
            _salesOrderDetailService = new SalesOrderDetailService(new SalesOrderDetailRepository(),new SalesOrderDetailValidator());
            _machineService = new MachineService(new MachineRepository(),new MachineValidator());
            _rollerTypeService = new RollerTypeService(new RollerTypeRepository(), new RollerTypeValidator());
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
            var q = _coreBuilderService.GetQueryable().Include("Machine").Include("Item")
                                                      .Include("UoM").Where(x => !x.IsDeleted);

            var query = (from model in q
                         select new
                         {
                             model.Id,
                             model.BaseSku,
                             model.Name,
                             model.Description,
                             Machine = model.Machine.Name,
                             model.CoreBuilderTypeCase,
                             model.SkuUsedCore,
                             UsedCoreItemQuantity = model.UsedCoreItem.Quantity,
                             model.SkuNewCore,
                             NewCoreItemQuantity = model.NewCoreItem.Quantity,
                             UoM = model.UoM.Name,
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
                            model.Description,
                            model.Machine,
                            model.CoreBuilderTypeCase,
                            model.SkuUsedCore, 
                            model.UsedCoreItemQuantity,
                            model.UoM,
                            model.SkuNewCore,
                            model.NewCoreItemQuantity,
                            model.UoM,
                            model.CreatedAt,
                            model.UpdatedAt,
                      }
                    }).ToArray()
            }, JsonRequestBehavior.AllowGet);
        }


        public dynamic GetInfo(int Id)
        {
            CoreBuilder model = new CoreBuilder();
            try 
            {
                model = _coreBuilderService.GetObjectById(Id);
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
                model.MachineId,
                Machine = _machineService.GetObjectById(model.MachineId).Name,
                model.CoreBuilderTypeCase,
                model.UoMId,
                UoM = _uomService.GetObjectById(model.UoMId).Name,
                model.BaseSku,
                model.SkuUsedCore,
                model.SkuNewCore,
                UsedCoreQuantity = _itemService.GetObjectById(model.UsedCoreItemId).Quantity,
                NewCoreQuantity = _itemService.GetObjectById(model.NewCoreItemId).Quantity,
                model.Errors
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public dynamic Insert(CoreBuilder model)
        {
            try
            {
                model = _coreBuilderService.CreateObject(model,_uomService,_itemService,_itemTypeService,
                    _warehouseItemService,_warehouseService,_priceMutationService,_machineService);
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
        public dynamic Update(CoreBuilder model)
        {
            try
            {
                var data = _coreBuilderService.GetObjectById(model.Id);
                data.Name = model.Name;
                data.BaseSku = model.BaseSku;
                data.SkuNewCore = model.SkuNewCore;
                data.SkuUsedCore = model.SkuUsedCore;
                data.MachineId = model.MachineId;
                data.Description = model.Description;
                data.UoMId = model.UoMId;
                data.CoreBuilderTypeCase = model.CoreBuilderTypeCase;
                model = _coreBuilderService.UpdateObject(data,_uomService,_itemService,_itemTypeService,_warehouseItemService
                    ,_warehouseService,_blanketService,_contactService,_machineService,_priceMutationService);
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
        public dynamic Delete(CoreBuilder model)
        {
            try
            {
                var data = _coreBuilderService.GetObjectById(model.Id);
                model = _coreBuilderService.SoftDeleteObject(data, _itemService, _rollerBuilderService, 
                    _coreIdentificationDetailService, _recoveryOrderDetailService, _recoveryAccessoryDetailService, 
                    _warehouseItemService, _stockMutationService, _itemTypeService,_blanketService,_purchaseOrderDetailService
                    ,_stockAdjustmentDetailService,_salesOrderDetailService,_priceMutationService, _blanketOrderDetailService);
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
