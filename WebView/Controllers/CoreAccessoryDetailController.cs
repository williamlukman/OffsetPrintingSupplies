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
    public class CoreAccessoryDetailController : Controller
    {
        private readonly static log4net.ILog LOG = log4net.LogManager.GetLogger("CoreAccessoryDetailController");
        private IItemService _itemService;
        private IWarehouseItemService _warehouseItemService;
        private IStockMutationService _stockMutationService;
        private IBlanketService _blanketService;
        private IWarehouseService _warehouseService;
        private IBlanketOrderService _blanketOrderService;
        private IBlanketOrderDetailService _blanketOrderDetailService;
        private IContactService _contactService;
        private IRecoveryOrderService _recoveryOrderService;
        private IRecoveryOrderDetailService _recoveryOrderDetailService;
        private IRecoveryAccessoryDetailService _recoveryAccessoryDetailService;
        private ICoreIdentificationService _coreIdentificationService;
        private ICoreIdentificationDetailService _coreIdentificationDetailService;
        private IRollerBuilderService _rollerBuilderService;
        private ICoreBuilderService _coreBuilderService;
        private IItemTypeService _itemTypeService;
        private ICoreAccessoryDetailService _coreAccessoryDetailService;

        public CoreAccessoryDetailController()
        {
            _itemService = new ItemService(new ItemRepository(), new ItemValidator());
            _warehouseItemService = new WarehouseItemService(new WarehouseItemRepository(), new WarehouseItemValidator());
            _stockMutationService = new StockMutationService(new StockMutationRepository(), new StockMutationValidator());
            _blanketService = new BlanketService(new BlanketRepository(), new BlanketValidator());
            _contactService = new ContactService(new ContactRepository(), new ContactValidator());
            _warehouseService = new WarehouseService(new WarehouseRepository(), new WarehouseValidator());
            _blanketOrderService = new BlanketOrderService(new BlanketOrderRepository(), new BlanketOrderValidator());
            _blanketOrderDetailService = new BlanketOrderDetailService(new BlanketOrderDetailRepository(), new BlanketOrderDetailValidator());
            _recoveryOrderService = new RecoveryOrderService(new RecoveryOrderRepository(), new RecoveryOrderValidator());
            _recoveryOrderDetailService = new RecoveryOrderDetailService(new RecoveryOrderDetailRepository(), new RecoveryOrderDetailValidator());
            _recoveryAccessoryDetailService = new RecoveryAccessoryDetailService(new RecoveryAccessoryDetailRepository(), new RecoveryAccessoryDetailValidator());
            _coreIdentificationService = new CoreIdentificationService(new CoreIdentificationRepository(), new CoreIdentificationValidator());
            _coreIdentificationDetailService = new CoreIdentificationDetailService(new CoreIdentificationDetailRepository(), new CoreIdentificationDetailValidator());
            _rollerBuilderService = new RollerBuilderService(new RollerBuilderRepository(), new RollerBuilderValidator());
            _coreBuilderService = new CoreBuilderService(new CoreBuilderRepository(), new CoreBuilderValidator());
            _itemTypeService = new ItemTypeService(new ItemTypeRepository(), new ItemTypeValidator());
            _coreAccessoryDetailService = new CoreAccessoryDetailService(new CoreAccessoryDetailRepository(), new CoreAccessoryDetailValidator());
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
            var q = _coreIdentificationDetailService.GetQueryable().Include("CoreIdentification")
                                             .Include("CoreBuilder").Include("RollerType")
                                             .Include("Machine").Include("Item").Where(x =>!x.IsDeleted && !x.CoreIdentification.IsInHouse);

            var query = (from model in q
                         select new
                         {
                             model.Id,
                             model.DetailId,
                             model.CoreIdentificationId,
                             MaterialCase = model.MaterialCase == 1 ? "New" : "Used",
                             model.CoreBuilderId,
                             CoreBuilderBaseSku = model.CoreBuilder.BaseSku,
                             CoreBuilder = model.CoreBuilder.Name,
                             model.RollerTypeId,
                             RollerType = model.RollerType.Name,
                             model.MachineId,
                             Machine = model.Machine.Name,
                             RepairRequestCase = model.RepairRequestCase == 1 ? "BearingSeat" : "CentreDrill",
                             model.RD,
                             model.CD,
                             model.RL,
                             model.WL,
                             model.TL,
                             model.IsJobScheduled,
                             model.IsRollerBuilt,
                             model.IsDelivered
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
                            model.DetailId,
                            model.CoreIdentificationId,
                            model.MaterialCase,
                            model.CoreBuilderId,
                            model.CoreBuilderBaseSku,
                            model.CoreBuilder,
                            model.RollerTypeId,
                            model.RollerType,
                            model.MachineId,
                            model.Machine,
                            model.RepairRequestCase,
                            model.RD,
                            model.CD,
                            model.RL,
                            model.WL,
                            model.TL,
                            model.IsJobScheduled,
                            model.IsRollerBuilt,
                            model.IsDelivered
                      }
                    }).ToArray()
            }, JsonRequestBehavior.AllowGet);
        }


        public dynamic GetListAccessory(string _search, long nd, int rows, int? page, string sidx, string sord, int id, string filters = "")
        {
            // Construct where statement
            string strWhere = GeneralFunction.ConstructWhere(filters);
            string filter = null;
            GeneralFunction.ConstructWhereInLinq(strWhere, out filter);
            if (filter == "") filter = "true";

            // Get Data
            var q = _coreAccessoryDetailService.GetQueryable().Include("Item")
                                                   .Where(x => x.CoreIdentificationDetailId == id && !x.IsDeleted);

            var query = (from model in q
                         select new
                         {
                             model.Id,
                             model.ItemId,
                             ItemSku = model.Item.Sku,
                             Item = model.Item.Name,
                             model.Quantity
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
                            model.ItemId,
                            model.ItemSku,
                            model.Item,
                            model.Quantity
                        }
                    }).ToArray()
            }, JsonRequestBehavior.AllowGet);
        }


        public dynamic GetInfo(int Id)
        {
            RecoveryOrderDetail model = new RecoveryOrderDetail();
            try
            {
                model = _recoveryOrderDetailService.GetObjectById(Id);
            }
            catch (Exception ex)
            {
                LOG.Error("GetInfo", ex);
                model.Errors.Add("Generic", "Error : " + ex);
            }

            return Json(new
            {
                model.Id,
                model.RecoveryOrderId,
                CoreIdentificationDetail =  _coreIdentificationDetailService.GetObjectById(model.CoreIdentificationDetailId).DetailId,
                model.CoreIdentificationDetailId,
                MaterialCase = _coreIdentificationDetailService.GetObjectById(model.CoreIdentificationDetailId).MaterialCase == Core.Constants.Constant.MaterialCase.New ? "New" : "Used", 
                model.RollerBuilderId,
                RollerBuilderSku = _rollerBuilderService.GetObjectById(model.RollerBuilderId).BaseSku,
                RollerBuilder = _rollerBuilderService.GetObjectById(model.RollerBuilderId).Name,
                model.CoreTypeCase,
                model.IsDisassembled,
                model.IsStrippedAndGlued,
                model.IsWrapped,
                model.CompoundUsage,
                model.IsVulcanized,
                model.IsFacedOff,
                model.IsConventionalGrinded,
                model.IsCNCGrinded,
                model.IsPolishedAndQC,
                model.IsPackaged,
                model.RejectedDate,
                model.FinishedDate,
                model.CreatedAt,
                model.UpdatedAt,
                model.Errors
            }, JsonRequestBehavior.AllowGet);
        }

        public dynamic GetInfoDetail(int Id)
        {
            CoreAccessoryDetail model = new CoreAccessoryDetail();
            try
            {
                model = _coreAccessoryDetailService.GetObjectById(Id);
            
            }
            catch (Exception ex)
            {
                LOG.Error("GetInfo", ex);
                model.Errors.Add("Generic", "Error : " + ex);
            }

            return Json(new
            {
                model.Id,
                model.CoreIdentificationDetailId,
                DetailId = _coreIdentificationDetailService.GetObjectById(model.CoreIdentificationDetailId).DetailId,
                CoreBuilderName = _coreBuilderService.GetObjectById(_coreIdentificationDetailService.GetObjectById(
                                   model.CoreIdentificationDetailId).CoreBuilderId).Name,
                model.Errors
            }, JsonRequestBehavior.AllowGet);
        }

        public dynamic GetInfoAccessory(int Id)
        {
            CoreAccessoryDetail model = new CoreAccessoryDetail();
            try
            {
                model =_coreAccessoryDetailService.GetObjectById(Id);

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
        public dynamic InsertAccessory(CoreAccessoryDetail model)
        {
            try
            {
                model = _coreAccessoryDetailService.CreateObject(model,_coreIdentificationService,_coreIdentificationDetailService,
                    _itemService,_itemTypeService,_warehouseItemService);
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
        public dynamic DeleteAccessory(CoreAccessoryDetail model)
        {
            try
            {
                var data = _coreAccessoryDetailService.GetObjectById(model.Id);
                model = _coreAccessoryDetailService.SoftDeleteObject(data,_coreIdentificationDetailService);

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
        public dynamic UpdateAccessory(CoreAccessoryDetail model)
        {
            try
            {
                var data = _coreAccessoryDetailService.GetObjectById(model.Id);
                data.ItemId = model.ItemId;
                data.Quantity = model.Quantity;
                model = _coreAccessoryDetailService.UpdateObject(data,_coreIdentificationService,
                    _coreIdentificationDetailService,_itemService,_itemTypeService,_warehouseItemService
                    );
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

    }
}
