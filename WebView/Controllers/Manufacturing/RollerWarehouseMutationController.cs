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
    public class RollerWarehouseMutationController : Controller
    {
        private readonly static log4net.ILog LOG = log4net.LogManager.GetLogger("RollerWarehouseMutationController");
        private IItemService _itemService;
        private IWarehouseItemService _warehouseItemService;
        private IStockMutationService _stockMutationService;
        private IBlanketService _blanketService;
        private IRollerWarehouseMutationService _rollerWarehouseMutationService;
        private IRollerWarehouseMutationDetailService _rollerWarehouseMutationDetailService;
        private IContactService _contactService;
        private IPurchaseReceivalDetailService _purchaseReceivalDetailService;
        private IPurchaseReceivalService _purchaseReceivalService;
        private ICoreIdentificationDetailService _coreIdentificationDetailService;
        private ICoreIdentificationService _coreIdentificationService;
        private IRecoveryOrderDetailService _recoveryOrderDetailService;
        private IRecoveryOrderService _recoveryOrderService;
        private IWarehouseService _warehouseService;
        private ICoreBuilderService _coreBuilderService;
        private IRollerBuilderService _rollerBuilderService;
        private ICustomerStockMutationService _customerStockMutationService;
        private ICustomerItemService _customerItemService;

        public RollerWarehouseMutationController()
        {
            _itemService = new ItemService(new ItemRepository(), new ItemValidator());
            _warehouseItemService = new WarehouseItemService(new WarehouseItemRepository(), new WarehouseItemValidator());
            _stockMutationService = new StockMutationService(new StockMutationRepository(), new StockMutationValidator());
            _blanketService = new BlanketService(new BlanketRepository(), new BlanketValidator());
            _rollerWarehouseMutationService = new RollerWarehouseMutationService(new RollerWarehouseMutationRepository(), new RollerWarehouseMutationValidator());
            _rollerWarehouseMutationDetailService = new RollerWarehouseMutationDetailService(new RollerWarehouseMutationDetailRepository(), new RollerWarehouseMutationDetailValidator());
            _contactService = new ContactService(new ContactRepository(), new ContactValidator());
            _purchaseReceivalService = new PurchaseReceivalService(new PurchaseReceivalRepository(), new PurchaseReceivalValidator());
            _purchaseReceivalDetailService = new PurchaseReceivalDetailService(new PurchaseReceivalDetailRepository(), new PurchaseReceivalDetailValidator());
            _coreIdentificationService = new CoreIdentificationService(new CoreIdentificationRepository(), new CoreIdentificationValidator());
            _coreIdentificationDetailService = new CoreIdentificationDetailService(new CoreIdentificationDetailRepository(), new CoreIdentificationDetailValidator());
            _warehouseService = new WarehouseService(new WarehouseRepository(), new WarehouseValidator());
            _coreBuilderService = new CoreBuilderService(new CoreBuilderRepository(), new CoreBuilderValidator());
            _rollerBuilderService = new RollerBuilderService(new RollerBuilderRepository(), new RollerBuilderValidator());
            _recoveryOrderDetailService = new RecoveryOrderDetailService(new RecoveryOrderDetailRepository(), new RecoveryOrderDetailValidator());
            _recoveryOrderService = new RecoveryOrderService(new RecoveryOrderRepository(), new RecoveryOrderValidator());
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
            var q = _rollerWarehouseMutationService.GetQueryable().Include("RecoveryOrder").Include("Warehouse")
                                                   .Where(x => !x.IsDeleted);

            var query = (from model in q
                         select new
                         {
                             model.Id,
                             model.Code,
                             model.RecoveryOrderId,
                             RecoveryOrderCode = model.RecoveryOrder.Code,
                             model.WarehouseFromId,
                             WarehouseFromCode = model.WarehouseFrom.Code,
                             WarehouseFrom = model.WarehouseFrom.Name,
                             model.WarehouseToId,
                             WarehouseToCode = model.WarehouseTo.Code,
                             WarehouseTo = model.WarehouseTo.Name,
                             model.Quantity,
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
                            model.RecoveryOrderId,
                            model.RecoveryOrderCode,
                            model.WarehouseFromId,
                            model.WarehouseFromCode,
                            model.WarehouseFrom,
                            model.WarehouseToId,
                            model.WarehouseToCode,
                            model.WarehouseTo,
                            model.Quantity,
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
            var q = _rollerWarehouseMutationDetailService.GetQueryable().Include("RecoveryOrderDetail")
                                                         .Include("CoreIdentificationDetail").Include("Item")
                                                         .Where(x => x.RollerWarehouseMutationId == id && !x.IsDeleted);

            var query = (from model in q
                         select new
                         {
                             model.Id,
                             DetailId = model.RecoveryOrderDetail.CoreIdentificationDetail.DetailId,
                             model.Code,
                             model.RollerWarehouseMutationId,
                             model.RecoveryOrderDetailId,
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
                            model.DetailId,
                            model.Code,
                            model.RollerWarehouseMutationId,
                            model.RecoveryOrderDetailId,
                            model.ItemId,
                            model.ItemSku,
                            model.Item,
                      }
                    }).ToArray()
            }, JsonRequestBehavior.AllowGet);
        }


        public dynamic GetInfo(int Id)
        {
            RollerWarehouseMutation model = new RollerWarehouseMutation();
            try
            {
                model = _rollerWarehouseMutationService.GetObjectById(Id);
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
                model.RecoveryOrderId,
                RecoveryOrder = _recoveryOrderService.GetObjectById(model.RecoveryOrderId).Code,
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
            RollerWarehouseMutationDetail model = new RollerWarehouseMutationDetail();
            try
            {
                model = _rollerWarehouseMutationDetailService.GetObjectById(Id);
            
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
                model.RollerWarehouseMutationId,
                model.RecoveryOrderDetailId, 
                model.ItemId,
                ItemSku = _itemService.GetObjectById(model.ItemId).Sku,
                Item = _itemService.GetObjectById(model.ItemId).Name,
                RIFID = _coreIdentificationDetailService.GetObjectById(_recoveryOrderDetailService.GetObjectById
                                (model.RecoveryOrderDetailId).CoreIdentificationDetailId).DetailId,
                model.Errors
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public dynamic Insert(RollerWarehouseMutation model)
        {
            try
            {
                model = _rollerWarehouseMutationService.CreateObject(model,_warehouseService,_recoveryOrderService);
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
        public dynamic InsertDetail(RollerWarehouseMutationDetail model)
        {
            try
            {
                var data = _recoveryOrderDetailService.GetObjectById(model.RecoveryOrderDetailId);
                var MaterialCase = _coreIdentificationDetailService.GetObjectById(_recoveryOrderDetailService.GetObjectById(model.RecoveryOrderDetailId).CoreIdentificationDetailId).MaterialCase;
                var item = MaterialCase == Core.Constants.Constant.MaterialCase.New ? _rollerBuilderService.GetRollerNewCore(data.RollerBuilderId) :
                            _rollerBuilderService.GetRollerUsedCore(data.RollerBuilderId);
                model.ItemId = item.Id;
                model = _rollerWarehouseMutationDetailService.CreateObject(model, _rollerWarehouseMutationService,
                    _recoveryOrderDetailService, _coreIdentificationDetailService, _itemService, _warehouseItemService);
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
        public dynamic Update(RollerWarehouseMutation model)
        {
            try
            {
                var data = _rollerWarehouseMutationService.GetObjectById(model.Id);
                data.RecoveryOrderId = model.RecoveryOrderId;
                data.WarehouseFromId = model.WarehouseFromId;
                data.WarehouseToId = model.WarehouseToId;
                data.Quantity = model.Quantity;
                data.MutationDate = model.MutationDate;
                model = _rollerWarehouseMutationService.UpdateObject(data,_warehouseService, _recoveryOrderService);
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
        public dynamic Delete(RollerWarehouseMutation model)
        {
            try
            {
                var data = _rollerWarehouseMutationService.GetObjectById(model.Id);
                model = _rollerWarehouseMutationService.SoftDeleteObject(data);
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
        public dynamic DeleteDetail(RollerWarehouseMutationDetail model)
        {
            try
            {
                var data = _rollerWarehouseMutationDetailService.GetObjectById(model.Id);
                model = _rollerWarehouseMutationDetailService.SoftDeleteObject(data,_rollerWarehouseMutationService,_warehouseItemService);
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
        public dynamic UpdateDetail(RollerWarehouseMutationDetail model)
        {
            try
            {
                var data2 = _rollerWarehouseMutationDetailService.GetObjectById(model.Id);
                data2.RecoveryOrderDetailId = model.RecoveryOrderDetailId;
                model = _rollerWarehouseMutationDetailService.UpdateObject(data2,_rollerWarehouseMutationService,
                    _recoveryOrderDetailService,_coreIdentificationDetailService,_itemService,_warehouseItemService);
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
        public dynamic Confirm(RollerWarehouseMutation model)
        {
            try
            {
                var data = _rollerWarehouseMutationService.GetObjectById(model.Id);
                model = _rollerWarehouseMutationService.ConfirmObject(data,model.ConfirmationDate.Value
                    ,_rollerWarehouseMutationDetailService,_itemService,_blanketService
                    ,_warehouseItemService,_stockMutationService,_recoveryOrderDetailService, _coreIdentificationDetailService,_coreIdentificationService
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
        public dynamic UnConfirm(RollerWarehouseMutation model)
        {
            try
            {

                var data = _rollerWarehouseMutationService.GetObjectById(model.Id);
                model = _rollerWarehouseMutationService.UnconfirmObject(data,_rollerWarehouseMutationDetailService,
                    _itemService,_blanketService,_warehouseItemService,_stockMutationService,_recoveryOrderDetailService,
                    _coreIdentificationDetailService,_coreIdentificationService,
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
