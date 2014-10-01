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
    public class CoreIdentificationController : Controller
    {
        private readonly static log4net.ILog LOG = log4net.LogManager.GetLogger("CoreIdentificationController");
        private ICoreIdentificationService _coreIdentificationService;
        private ICoreIdentificationDetailService _coreIdentificationDetailService;
        private IStockMutationService _stockMutationService;
        private IRecoveryOrderService _recoveryOrderService;
        private ICoreBuilderService _coreBuilderService;
        private IItemService _itemService;
        private IWarehouseItemService _warehouseItemService;
        private IBlanketService _blanketService;
        private IContactService _contactService;
        private IRecoveryOrderDetailService _recoveryOrderDetailService;
        private IRollerWarehouseMutationDetailService _rollerWarehouseMutationDetailService;
        private IRollerTypeService _rollerTypeService;
        private IMachineService _machineService;
        private IWarehouseService _warehouseService;

        public CoreIdentificationController()
        {
            _coreIdentificationService = new CoreIdentificationService(new CoreIdentificationRepository(), new CoreIdentificationValidator());
            _coreIdentificationDetailService = new CoreIdentificationDetailService(new CoreIdentificationDetailRepository(), new CoreIdentificationDetailValidator());
            _stockMutationService = new StockMutationService(new StockMutationRepository(), new StockMutationValidator());
            _recoveryOrderService = new RecoveryOrderService(new RecoveryOrderRepository(), new RecoveryOrderValidator());
            _coreBuilderService = new CoreBuilderService(new CoreBuilderRepository(), new CoreBuilderValidator());
            _itemService = new ItemService(new ItemRepository(), new ItemValidator());
            _warehouseItemService = new WarehouseItemService(new WarehouseItemRepository(), new WarehouseItemValidator());
            _blanketService = new BlanketService(new BlanketRepository(), new BlanketValidator());
            _contactService = new ContactService(new ContactRepository(), new ContactValidator());
            _recoveryOrderDetailService = new RecoveryOrderDetailService(new RecoveryOrderDetailRepository(), new RecoveryOrderDetailValidator());
            _rollerWarehouseMutationDetailService = new RollerWarehouseMutationDetailService(new RollerWarehouseMutationDetailRepository(), new RollerWarehouseMutationDetailValidator());
            _rollerTypeService = new RollerTypeService(new RollerTypeRepository(), new RollerTypeValidator());
            _machineService = new MachineService(new MachineRepository(), new MachineValidator());
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
            string filter = null;
            GeneralFunction.ConstructWhereInLinq(strWhere, out filter);
            if (filter == "") filter = "true";

            // Get Data
            var q = _coreIdentificationService.GetQueryable().Include("Warehouse")
                                              .Include("Contact").Where(x => !x.IsDeleted);
    
            var query = (from model in q
                         select new
                         {
                            model.Id,
                            model.Code,
                            Warehouse = model.Warehouse.Name,
                            model.IsInHouse,
                            Contact = model.Contact.Name,
                            model.Quantity,
                            model.IdentifiedDate,
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
                            model.Warehouse,
                            model.IsInHouse,
                            model.Contact,
                            model.Quantity,
                            model.IdentifiedDate,
                            model.ConfirmationDate,
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
            var q = _coreIdentificationDetailService.GetQueryable().Include("CoreIdentification")
                                              .Include("CoreBuilder").Include("RollerType")
                                              .Include("Machine").Include("Item").Where(x => x.CoreIdentificationId == id && !x.IsDeleted);

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
                            RepairRequestCase = model.RepairRequestCase == 1 ? "BearingSeat":"CentreDrill",
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


        public dynamic GetInfo(int Id)
        {
            CoreIdentification model = new CoreIdentification();
            try
            {
                model = _coreIdentificationService.GetObjectById(Id);
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
                model.WarehouseId,
                WarehouseCode =  _warehouseService.GetObjectById(model.WarehouseId).Code,
                Warehouse = _warehouseService.GetObjectById(model.WarehouseId).Name,
                model.ContactId,
                Contact =  model.ContactId.HasValue ?_contactService.GetObjectById(model.ContactId.Value).Name : "",
                model.IsInHouse,
                model.Quantity,
                model.IdentifiedDate,
                model.IsConfirmed,
                model.ConfirmationDate,
                model.Errors
            }, JsonRequestBehavior.AllowGet);
        }

        public dynamic GetInfoDetail(int Id)
        {
            CoreIdentificationDetail model = new CoreIdentificationDetail();
            try
            {
                model = _coreIdentificationDetailService.GetObjectById(Id);
            }
            catch (Exception ex)
            {
                LOG.Error("GetInfo", ex);
                model.Errors.Add("Generic", "Error : " + ex);
            }

            return Json(new
            {
                model.Id,
                model.DetailId,
                model.CoreIdentificationId,
                model.MaterialCase,
                model.CoreBuilderId,
                CoreBuilderBaseSku = _coreBuilderService.GetObjectById(model.CoreBuilderId).BaseSku,
                CoreBuilder = _coreBuilderService.GetObjectById(model.CoreBuilderId).Name,
                model.RollerTypeId,
                RollerType = _rollerTypeService.GetObjectById(model.RollerTypeId).Name,
                model.MachineId,
                Machine = _machineService.GetObjectById(model.MachineId).Name,
                RepairRequestCase = model.RepairRequestCase == 1 ? "BearingSeat" : "CentreDrill",
                model.RD,
                model.CD,
                model.RL,
                model.WL,
                model.TL,
                model.Errors
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public dynamic Insert(CoreIdentification model)
        {
            try
            {

                model = _coreIdentificationService.CreateObject(model,_contactService);
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
        public dynamic InsertDetail(CoreIdentificationDetail model)
        {
            decimal amount = 0;
            try
            {
                model = _coreIdentificationDetailService.CreateObject(model, _coreIdentificationService,_coreBuilderService,
                    _rollerTypeService,_machineService, _warehouseItemService
                  );
                amount = _coreIdentificationService.GetObjectById(model.CoreIdentificationId).Quantity;
            }
            catch (Exception ex)
            {
                LOG.Error("Insert Failed", ex);
                model.Errors.Add("Generic", "Error : " + ex);

            }

            return Json(new
            {
                model.Errors,
                Quantity = amount
            });
        }

        [HttpPost]
        public dynamic Update(CoreIdentification model)
        {
            try
            {
                var data = _coreIdentificationService.GetObjectById(model.Id);
                data.Code = model.Code;
                data.ContactId = model.ContactId;
                data.IsInHouse = model.IsInHouse;
                data.IdentifiedDate = model.IdentifiedDate;
                data.Quantity = model.Quantity;
                model = _coreIdentificationService.UpdateObject(data, _contactService);
            }
            catch (Exception ex)
            {
                LOG.Error("Update Failed", ex);
                model.Errors.Add("Generic", "Error : " + ex);
            }

            return Json(new
            {
                model.Errors,
            });
        }

        [HttpPost]
        public dynamic Delete(CoreIdentification model)
        {
            try
            {
                var data = _coreIdentificationService.GetObjectById(model.Id);
                model = _coreIdentificationService.SoftDeleteObject(data,_coreIdentificationDetailService,_recoveryOrderService);
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
        public dynamic DeleteDetail(CoreIdentificationDetail model)
        {
            decimal amount = 0;
            try
            {
                var data = _coreIdentificationDetailService.GetObjectById(model.Id);
                model = _coreIdentificationDetailService.SoftDeleteObject(data,_coreIdentificationService,_recoveryOrderDetailService,
                    _rollerWarehouseMutationDetailService);
                amount = _coreIdentificationService.GetObjectById(model.CoreIdentificationId).Quantity;
            }
            catch (Exception ex)
            {
                LOG.Error("Delete Failed", ex);
                model.Errors.Add("Generic", "Error : " + ex);
            }

            return Json(new
            {
                model.Errors,
                Quantity = amount
            });
        }

        [HttpPost]
        public dynamic UpdateDetail(CoreIdentificationDetail model)
        {
            decimal amount = 0;
            try
            {
                var data = _coreIdentificationDetailService.GetObjectById(model.Id);
                data.DetailId = model.DetailId;
                data.MaterialCase = model.MaterialCase;
                data.CoreBuilderId = model.CoreBuilderId;
                data.RollerTypeId = model.RollerTypeId;
                data.MachineId = model.MachineId;
                data.RD = model.RD;
                data.CD = model.CD;
                data.RL = model.RL;
                data.WL = model.WL;
                data.TL = model.TL;
                model = _coreIdentificationDetailService.UpdateObject(data, _coreIdentificationService
                    ,_coreBuilderService,_rollerTypeService,_machineService, _warehouseItemService);
                amount = _coreIdentificationService.GetObjectById(model.CoreIdentificationId).Quantity;
            }
            catch (Exception ex)
            {
                LOG.Error("Update Failed", ex);
                model.Errors.Add("Generic", "Error : " + ex);
            }

            return Json(new
            {
                model.Errors,
                AmountPayable = amount
            });
        }


        [HttpPost]
        public dynamic Confirm(CoreIdentification model)
        {
            try
            {
                var data = _coreIdentificationService.GetObjectById(model.Id);
                model = _coreIdentificationService.ConfirmObject(data, model.ConfirmationDate.Value
                    ,_coreIdentificationDetailService,_stockMutationService,_recoveryOrderService
                    ,_recoveryOrderDetailService,_coreBuilderService,_itemService
                    ,_warehouseItemService,_blanketService);
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
        public dynamic UnConfirm(CoreIdentification model)
        {
            try
            {

                var data = _coreIdentificationService.GetObjectById(model.Id);
                model = _coreIdentificationService.UnconfirmObject(data, _coreIdentificationDetailService
                    ,_stockMutationService,_recoveryOrderService,_coreBuilderService
                    ,_itemService,_warehouseItemService,_blanketService);
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