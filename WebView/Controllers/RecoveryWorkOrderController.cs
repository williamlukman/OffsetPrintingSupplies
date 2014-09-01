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
    public class RecoveryWorkOrderController : Controller
    {
        private readonly static log4net.ILog LOG = log4net.LogManager.GetLogger("RecoveryWorkOrderController");
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

        public RecoveryWorkOrderController()
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
            var query =  _recoveryOrderService.GetAll().Where(d => d.IsDeleted == false);

            var list = query as IEnumerable<RecoveryOrder>;

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
                            model.CoreIdentificationId,
                            model.WarehouseId,
                            _warehouseService.GetObjectById(model.WarehouseId).Code,
                            _warehouseService.GetObjectById(model.WarehouseId).Name,
                            model.QuantityReceived,
                            model.QuantityFinal,
                            model.QuantityRejected,
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
            var query = _recoveryOrderDetailService.GetObjectsByRecoveryOrderId(id).Where(d => d.IsDeleted == false);

            var list = query as IEnumerable<RecoveryOrderDetail>;

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
                            model.CoreIdentificationDetailId,
                            model.RollerBuilderId,
                            _rollerBuilderService.GetObjectById(model.RollerBuilderId).Name,
                            model.CoreTypeCase,
                            model.Acc,
                            model.RepairRequestCase == 1 ? "BearingSeat":"CentreDrill",
                            model.CompoundUsage,
                            model.IsRejected,
                            model.RejectedDate,
                            model.IsFinished,
                            model.FinishedDate
                        }
                    }).ToArray()
            }, JsonRequestBehavior.AllowGet);
        }

        public dynamic GetListDetailFinished(string _search, long nd, int rows, int? page, string sidx, string sord, int id, string filters = "")
        {
            // Construct where statement

            string strWhere = GeneralFunction.ConstructWhere(filters);

            // Get Data
            var query = _recoveryOrderDetailService.GetObjectsByRecoveryOrderId(id).Where(d => d.IsDeleted == false);

            var list = query as IEnumerable<RecoveryOrderDetail>;

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
                            _coreIdentificationDetailService.GetObjectById(model.CoreIdentificationDetailId).DetailId,
                            model.CoreIdentificationDetailId,
                            _coreIdentificationDetailService.GetObjectById(model.CoreIdentificationDetailId).MaterialCase == Core.Constants.Constant.MaterialCase.New ? "New" : "Used", 
                            model.RollerBuilderId,
                            _rollerBuilderService.GetObjectById(model.RollerBuilderId).BaseSku,
                            _rollerBuilderService.GetObjectById(model.RollerBuilderId).Name,
                            model.CoreTypeCase,
                            model.Acc,
                            model.RepairRequestCase == 1 ? "BearingSeat":"CentreDrill",
                            model.IsDisassembled,
                            model.IsStrippedAndGlued,
                            model.IsWrapped,
                            model.CompoundUsage,
                            model.IsVulcanized,
                            model.IsFacedOff,
                            model.IsConventionalGrinded,
                            model.IsCWCGrinded,
                            model.IsPolishedAndQC,
                            model.IsPackaged,
                            model.IsRejected,
                            model.RejectedDate,
                            model.IsFinished,
                            model.FinishedDate
                        }
                    }).ToArray()
            }, JsonRequestBehavior.AllowGet);
        }

    

        public dynamic GetInfo(int Id)
        {
            RecoveryOrder model = new RecoveryOrder();
            try
            {
                model = _recoveryOrderService.GetObjectById(Id);
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
                model.CoreIdentificationId,
                CoreIdentification =_coreIdentificationService.GetObjectById(model.CoreIdentificationId).Code,
                WarehouseCode = _warehouseService.GetObjectById(model.WarehouseId).Code,
                Warehouse = _warehouseService.GetObjectById(model.WarehouseId).Name,
                model.QuantityReceived,
                model.Errors
            }, JsonRequestBehavior.AllowGet);
        }

        public dynamic GetInfoDetail(int Id)
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
                model.CoreIdentificationDetailId,
                DetailId = _coreIdentificationDetailService.GetObjectById(model.CoreIdentificationDetailId).DetailId,
                CoreBuilderName = _coreBuilderService.GetObjectById(_coreIdentificationDetailService.GetObjectById(
                                   model.CoreIdentificationDetailId).CoreBuilderId).Name,
                model.RollerBuilderId,
                RollerBuilder = _rollerBuilderService.GetObjectById(model.RollerBuilderId).Name,
                model.CoreTypeCase,
                model.Acc,
                RepairRequestCase = model.RepairRequestCase == 1 ? "BearingSeat":"CentreDrill",
                model.IsDisassembled,
                model.IsStrippedAndGlued,
                model.IsWrapped,
                model.IsVulcanized,
                model.IsFacedOff,
                model.IsConventionalGrinded,
                model.IsCWCGrinded,
                model.IsPolishedAndQC,
                model.IsPackaged,
                model.Errors
            }, JsonRequestBehavior.AllowGet);
        }

       
        [HttpPost]
        public dynamic Insert(RecoveryOrder model)
        {
            try
            {
                model = _recoveryOrderService.CreateObject(model,_coreIdentificationService);
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
        public dynamic InsertDetail(RecoveryOrderDetail model)
        {
            try
            {
                model = _recoveryOrderDetailService.CreateObject(model,_recoveryOrderService,_coreIdentificationDetailService,_rollerBuilderService);
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
        public dynamic Update(RecoveryOrder model)
        {
            try
            {
                var data = _recoveryOrderService.GetObjectById(model.Id);
                data.CoreIdentificationId = model.CoreIdentificationId;
                data.WarehouseId = model.WarehouseId;
                data.Code = model.Code;
                data.QuantityReceived = model.QuantityReceived;
                model = _recoveryOrderService.UpdateObject(data,_recoveryOrderDetailService,_coreIdentificationService);
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
        public dynamic Delete(RecoveryOrder model)
        {
            try
            {
                var data = _recoveryOrderService.GetObjectById(model.Id);
                model = _recoveryOrderService.SoftDeleteObject(model,_recoveryOrderDetailService,_recoveryAccessoryDetailService);
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
        public dynamic DeleteDetail(RecoveryOrderDetail model)
        {
            try
            {
                var data = _recoveryOrderDetailService.GetObjectById(model.Id);
                model = _recoveryOrderDetailService.SoftDeleteObject(data,_recoveryOrderService,_recoveryAccessoryDetailService);
                 
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
        public dynamic UpdateDetail(RecoveryOrderDetail model)
        {
            try
            {
                var data = _recoveryOrderDetailService.GetObjectById(model.Id);
                data.CoreIdentificationDetailId = model.CoreIdentificationDetailId;
                data.RollerBuilderId = model.RollerBuilderId;
                data.CoreTypeCase = model.CoreTypeCase;
                data.Acc = model.Acc;
                data.RepairRequestCase = model.RepairRequestCase;
                model = _recoveryOrderDetailService.UpdateObject(data,_recoveryOrderService,_coreIdentificationDetailService,_rollerBuilderService);
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
        public dynamic Confirm(RecoveryOrder model)
        {
            try
            {
                var data = _recoveryOrderService.GetObjectById(model.Id);
                model = _recoveryOrderService.ConfirmObject(data,model.ConfirmationDate.Value
                   ,_coreIdentificationDetailService,_recoveryOrderDetailService,_recoveryAccessoryDetailService,
                   _coreBuilderService,_stockMutationService,_itemService,_blanketService,_warehouseItemService,_warehouseService);
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
        public dynamic UnConfirm(RecoveryOrder model)
        {
            try
            {

                var data = _recoveryOrderService.GetObjectById(model.Id);
                model = _recoveryOrderService.UnconfirmObject(data,_coreIdentificationDetailService,_recoveryOrderDetailService,_recoveryAccessoryDetailService,
                    _coreBuilderService,_stockMutationService,_itemService,_blanketService,_warehouseItemService,_warehouseService);
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
