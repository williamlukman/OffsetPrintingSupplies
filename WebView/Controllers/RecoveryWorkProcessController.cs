﻿using System;
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
    public class RecoveryWorkProcessController : Controller
    {
        private readonly static log4net.ILog LOG = log4net.LogManager.GetLogger("RecoveryWorkProcessController");
        private IItemService _itemService;
        private IWarehouseItemService _warehouseItemService;
        private IStockMutationService _stockMutationService;
        private IBarringService _barringService;
        private IWarehouseService _warehouseService;
        private IBarringOrderService _barringOrderService;
        private IBarringOrderDetailService _barringOrderDetailService;
        private IContactService _contactService;
        private IRecoveryOrderService _recoveryOrderService;
        private IRecoveryOrderDetailService _recoveryOrderDetailService;
        private IRecoveryAccessoryDetailService _recoveryAccessoryDetailService;
        private ICoreIdentificationService _coreIdentificationService;
        private ICoreIdentificationDetailService _coreIdentificationDetailService;
        private IRollerBuilderService _rollerBuilderService;
        private ICoreBuilderService _coreBuilderService;
        private IItemTypeService _itemTypeService;

        public RecoveryWorkProcessController()
        {
            _itemService = new ItemService(new ItemRepository(), new ItemValidator());
            _warehouseItemService = new WarehouseItemService(new WarehouseItemRepository(), new WarehouseItemValidator());
            _stockMutationService = new StockMutationService(new StockMutationRepository(), new StockMutationValidator());
            _barringService = new BarringService(new BarringRepository(), new BarringValidator());
            _contactService = new ContactService(new ContactRepository(), new ContactValidator());
            _warehouseService = new WarehouseService(new WarehouseRepository(), new WarehouseValidator());
            _barringOrderService = new BarringOrderService(new BarringOrderRepository(), new BarringOrderValidator());
            _barringOrderDetailService = new BarringOrderDetailService(new BarringOrderDetailRepository(), new BarringOrderDetailValidator());
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
            var query =  _recoveryOrderDetailService.GetAll().Where(d => d.IsDeleted == false);

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
                            model.RecoveryOrderId,
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
                            model.RejectedDate,
                            model.FinishedDate,
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

  

        public dynamic GetListAccessory(string _search, long nd, int rows, int? page, string sidx, string sord, int id, string filters = "")
        {
            // Construct where statement

            string strWhere = GeneralFunction.ConstructWhere(filters);

            // Get Data
            var query = _recoveryAccessoryDetailService.GetObjectsByRecoveryOrderDetailId(id).Where(d => d.IsDeleted == false);

            var list = query as IEnumerable<RecoveryAccessoryDetail>;

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
                            _itemService.GetObjectById(model.ItemId).Name,
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
                model.Acc,
                RepairRequestCase =  model.RepairRequestCase == 1 ? "BearingSeat":"CentreDrill",
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
                model.RejectedDate,
                model.FinishedDate,
                model.CreatedAt,
                model.UpdatedAt,
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

        public dynamic GetInfoAccessory(int Id)
        {
            RecoveryAccessoryDetail model = new RecoveryAccessoryDetail();
            try
            {
                model = _recoveryAccessoryDetailService.GetObjectById(Id);

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
        public dynamic InsertAccessory(RecoveryAccessoryDetail model)
        {
            try
            {
                model = _recoveryAccessoryDetailService.CreateObject(model,_recoveryOrderService,_recoveryOrderDetailService
                    ,_itemService,_itemTypeService,_warehouseItemService);
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
        public dynamic DeleteAccessory(RecoveryAccessoryDetail model)
        {
            try
            {
                var data = _recoveryAccessoryDetailService.GetObjectById(model.Id);
                model = _recoveryAccessoryDetailService.SoftDeleteObject(data,_recoveryOrderDetailService);

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
        public dynamic UpdateAccessory(RecoveryAccessoryDetail model)
        {
            try
            {
                var data = _recoveryAccessoryDetailService.GetObjectById(model.Id);
                data.ItemId = model.ItemId;
                data.Quantity = model.Quantity;
                model = _recoveryAccessoryDetailService.UpdateObject(data,_recoveryOrderService,_recoveryOrderDetailService
                    ,_itemService,_itemTypeService,_warehouseItemService
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

        [HttpPost]
        public dynamic ProgressDetail(RecoveryOrderDetail model)
        {
            var models = new RecoveryOrderDetail();
            try
            {
                var data = _recoveryOrderDetailService.GetObjectById(model.Id);
                if (model.IsDisassembled == true) { models = _recoveryOrderDetailService.DisassembleObject(data, _recoveryOrderService); }
                if (model.IsStrippedAndGlued == true) { models = _recoveryOrderDetailService.StripAndGlueObject(data); }
                if (model.IsWrapped == true) { models = _recoveryOrderDetailService.WrapObject(data, model.CompoundUsage,
                                                            _recoveryOrderService, _rollerBuilderService, _itemService, _warehouseItemService); }
                if (model.IsVulcanized == true) { models = _recoveryOrderDetailService.VulcanizeObject(data); }
                if (model.IsFacedOff == true) { models = _recoveryOrderDetailService.FaceOffObject(data); }
                if (model.IsConventionalGrinded == true) { models = _recoveryOrderDetailService.ConventionalGrindObject(data); }
                if (model.IsCWCGrinded == true) { models = _recoveryOrderDetailService.CWCGrindObject(data); }
                if (model.IsPolishedAndQC == true) { models = _recoveryOrderDetailService.PolishAndQCObject(data); }
                if (model.IsPackaged == true) { models = _recoveryOrderDetailService.PackageObject(data); }
            }
            catch (Exception ex)
            {
                LOG.Error("Update Failed", ex);
                models.Errors.Add("Generic", "Error : " + ex);
            }

            return Json(new
            {
                models.Errors
            });
        }


       

        [HttpPost]
        public dynamic Finish(RecoveryOrderDetail model)
        {
            try
            {
                var data = _recoveryOrderDetailService.GetObjectById(model.Id);
                model = _recoveryOrderDetailService.FinishObject(data, model.FinishedDate.Value,_coreIdentificationService,
                    _coreIdentificationDetailService,_recoveryOrderService,_recoveryAccessoryDetailService,
                    _coreBuilderService,_rollerBuilderService,_itemService,_warehouseItemService,
                    _barringService,_stockMutationService);
            }
            catch (Exception ex)
            {
                LOG.Error("Finish Failed", ex);
                model.Errors.Add("Generic", "Error : " + ex);
            }

            return Json(new
            {
                model.Errors
            });
        }

        [HttpPost]
        public dynamic UnFinish(RecoveryOrderDetail model)
        {
            try
            {

                var data = _recoveryOrderDetailService.GetObjectById(model.Id);
                model = _recoveryOrderDetailService.UnfinishObject(data,_coreIdentificationService,
                    _coreIdentificationDetailService,_recoveryOrderService,_recoveryAccessoryDetailService
                    ,_coreBuilderService,_rollerBuilderService,_itemService,_warehouseItemService,_barringService
                    ,_stockMutationService);
            }
            catch (Exception ex)
            {
                LOG.Error("Unfinish Failed", ex);
                model.Errors.Add("Generic", "Error : " + ex);
            }

            return Json(new
            {
                model.Errors
            });
        }

        [HttpPost]
        public dynamic Reject(RecoveryOrderDetail model)
        {
            try
            {
                var data = _recoveryOrderDetailService.GetObjectById(model.Id);
                model = _recoveryOrderDetailService.RejectObject(data,model.RejectedDate.Value,_coreIdentificationService,
                    _coreIdentificationDetailService,_recoveryOrderService,_recoveryAccessoryDetailService,_coreBuilderService
                    ,_rollerBuilderService,_itemService,_warehouseItemService,_barringService,_stockMutationService);
            }
            catch (Exception ex)
            {
                LOG.Error("Reject Failed", ex);
                model.Errors.Add("Generic", "Error : " + ex);
            }

            return Json(new
            {
                model.Errors
            });
        }

        [HttpPost]
        public dynamic UnReject(RecoveryOrderDetail model)
        {
            try
            {

                var data = _recoveryOrderDetailService.GetObjectById(model.Id);
                model = _recoveryOrderDetailService.UndoRejectObject(data,_coreIdentificationService,
                    _coreIdentificationDetailService,_recoveryOrderService,_recoveryAccessoryDetailService,
                    _coreBuilderService,_rollerBuilderService,_itemService,_warehouseItemService,_barringService,_stockMutationService);
            }
            catch (Exception ex)
            {
                LOG.Error("Unreject Failed", ex);
                model.Errors.Add("Generic", "Error : " + ex);
            }

            return Json(new
            {
                model.Errors
            });
        }

    }
}
