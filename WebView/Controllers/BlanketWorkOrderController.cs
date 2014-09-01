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
    public class BlanketWorkOrderController : Controller
    {
      private readonly static log4net.ILog LOG = log4net.LogManager.GetLogger("BlanketWorkOrderController");
        private IItemService _itemService;
        private IWarehouseItemService _warehouseItemService;
        private IStockMutationService _stockMutationService;
        private IBlanketService _blanketService;
        private IWarehouseService _warehouseService;
        private IBlanketOrderService _blanketOrderService;
        private IBlanketOrderDetailService _blanketOrderDetailService;
        private IContactService _contactService;

        public BlanketWorkOrderController()
        {
            _itemService = new ItemService(new ItemRepository(), new ItemValidator());
            _warehouseItemService = new WarehouseItemService(new WarehouseItemRepository(), new WarehouseItemValidator());
            _stockMutationService = new StockMutationService(new StockMutationRepository(), new StockMutationValidator());
            _blanketService = new BlanketService(new BlanketRepository(), new BlanketValidator());
            _contactService = new ContactService(new ContactRepository(), new ContactValidator());
            _warehouseService = new WarehouseService(new WarehouseRepository(), new WarehouseValidator());
            _blanketOrderService = new BlanketOrderService(new BlanketOrderRepository(), new BlanketOrderValidator());
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

            // Get Data
            var query =  _blanketOrderService.GetAll().Where(d => d.IsDeleted == false);

            var list = query as IEnumerable<BlanketOrder>;

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
                           _contactService.GetObjectById(model.ContactId).Name,
                            _warehouseService.GetObjectById(model.WarehouseId).Name,
                            model.QuantityReceived,
                            model.QuantityFinal,
                            model.QuantityRejected,
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
            var query = _blanketOrderDetailService.GetObjectsByBlanketOrderId(id).Where(d => d.IsDeleted == false);

            var list = query as IEnumerable<BlanketOrderDetail>;

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
                            _blanketService.GetObjectById(model.BlanketId).Sku,
                            _blanketService.GetObjectById(model.BlanketId).Name,
                            _blanketService.GetRollBlanketItem(_blanketService.GetObjectById(model.BlanketId)).Sku,
                            _blanketService.GetRollBlanketItem(_blanketService.GetObjectById(model.BlanketId)).Name,
                            _blanketService.GetLeftBarItem(_blanketService.GetObjectById(model.BlanketId)).Sku,
                            _blanketService.GetLeftBarItem(_blanketService.GetObjectById(model.BlanketId)).Name,
                            _blanketService.GetRightBarItem(_blanketService.GetObjectById(model.BlanketId)).Sku,
                            _blanketService.GetRightBarItem(_blanketService.GetObjectById(model.BlanketId)).Name,
                            model.RejectedDate,
                            model.FinishedDate
                        }
                    }).ToArray()
            }, JsonRequestBehavior.AllowGet);
        }


        public dynamic GetInfo(int Id)
        {
            BlanketOrder model = new BlanketOrder();
            try
            {
                model = _blanketOrderService.GetObjectById(Id);
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
                model.ContactId,
                Contact = _contactService.GetObjectById(model.ContactId).Name,
                model.WarehouseId,
                WarehouseCode = _warehouseService.GetObjectById(model.WarehouseId).Code,
                Warehouse = _warehouseService.GetObjectById(model.WarehouseId).Name,
                model.QuantityReceived,
                model.Errors
            }, JsonRequestBehavior.AllowGet);
        }

        public dynamic GetInfoDetail(int Id)
        {
            BlanketOrderDetail model = new BlanketOrderDetail();
            try
            {
                model = _blanketOrderDetailService.GetObjectById(Id);
            
            }
            catch (Exception ex)
            {
                LOG.Error("GetInfo", ex);
                model.Errors.Add("Generic", "Error : " + ex);
            }

            return Json(new
            {
                model.Id,
                BlanketSku = _blanketService.GetObjectById(model.BlanketId).Sku,
                Blanket = _blanketService.GetObjectById(model.BlanketId).Name,
                RollBlanketSku = _blanketService.GetLeftBarItem(_blanketService.GetObjectById(model.BlanketId)).Sku,
                RollBlanket = _blanketService.GetRollBlanketItem(_blanketService.GetObjectById(model.BlanketId)).Name,
                BlanketLeftBarSku = _blanketService.GetLeftBarItem(_blanketService.GetObjectById(model.BlanketId)).Sku,
                BlanketLeftBar = _blanketService.GetLeftBarItem(_blanketService.GetObjectById(model.BlanketId)).Name,
                BlanketRightBarSku = _blanketService.GetRightBarItem(_blanketService.GetObjectById(model.BlanketId)).Sku,
                BlanketRightBar = _blanketService.GetLeftBarItem(_blanketService.GetObjectById(model.BlanketId)).Name,
                model.Errors
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public dynamic Insert(BlanketOrder model)
        {
            try
            {
                model = _blanketOrderService.CreateObject(model);
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
        public dynamic InsertDetail(BlanketOrderDetail model)
        {
            try
            {
                model = _blanketOrderDetailService.CreateObject(model,_blanketOrderService,_blanketService);
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
        public dynamic Update(BlanketOrder model)
        {
            try
            {
                var data = _blanketOrderService.GetObjectById(model.Id);
                data.ContactId = model.ContactId;
                data.WarehouseId = model.WarehouseId;
                data.Code = model.Code;
                data.QuantityReceived = model.QuantityReceived;
                model = _blanketOrderService.UpdateObject(data,_blanketOrderDetailService);
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
        public dynamic Delete(BlanketOrder model)
        {
            try
            {
                var data = _blanketOrderService.GetObjectById(model.Id);
                model = _blanketOrderService.SoftDeleteObject(model, _blanketOrderDetailService);
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
        public dynamic DeleteDetail(BlanketOrderDetail model)
        {
            try
            {
                var data = _blanketOrderDetailService.GetObjectById(model.Id);
                model = _blanketOrderDetailService.SoftDeleteObject(data,_blanketOrderService);
                 
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
        public dynamic UpdateDetail(BlanketOrderDetail model)
        {
            try
            {
                var data = _blanketOrderDetailService.GetObjectById(model.Id);
                data.BlanketId = model.BlanketId;
                model = _blanketOrderDetailService.UpdateObject(data,_blanketOrderService,_blanketService);
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
        public dynamic ProgressDetail(int Id,string Progress)
        {
            var model = new BlanketOrderDetail();
            try
            {
                var data = _blanketOrderDetailService.GetObjectById(Id);
                if (Progress == "IsCut") { model = _blanketOrderDetailService.CutObject(data, _blanketOrderService); }
                else if (Progress == "IsSideSealed") { model = _blanketOrderDetailService.SideSealObject(data); }
                else if (Progress == "IsBarPrepared") { model = _blanketOrderDetailService.PrepareObject(data); }
                else if (Progress == "IsAdhesiveTapeApplied") { model = _blanketOrderDetailService.ApplyTapeAdhesiveToObject(data); }
                else if (Progress == "IsBarMounted") { model = _blanketOrderDetailService.MountObject(data); }
                else if (Progress == "IsBarHeatPressed") { model = _blanketOrderDetailService.HeatPressObject(data); }
                else if (Progress == "IsBarPullOffTested") { model = _blanketOrderDetailService.PullOffTestObject(data); }
                else if (Progress == "IsQCAndMarked") { model = _blanketOrderDetailService.QCAndMarkObject(data); }
                else if (Progress == "IsPackaged") { model = _blanketOrderDetailService.PackageObject(data); }
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
        public dynamic Confirm(BlanketOrder model)
        {
            try
            {
                var data = _blanketOrderService.GetObjectById(model.Id);
                model = _blanketOrderService.ConfirmObject(data,model.ConfirmationDate.Value
                   ,_blanketOrderDetailService,_blanketService,_itemService,_warehouseItemService);
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
        public dynamic UnConfirm(BlanketOrder model)
        {
            try
            {

                var data = _blanketOrderService.GetObjectById(model.Id);
                model = _blanketOrderService.UnconfirmObject(data,_blanketOrderDetailService,_blanketService,
                    _itemService,_warehouseItemService);
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

        [HttpPost]
        public dynamic Finish(BlanketOrderDetail model)
        {
            try
            {
                var data = _blanketOrderDetailService.GetObjectById(model.Id);
                model = _blanketOrderDetailService.FinishObject(data, model.FinishedDate.Value, _blanketOrderService, _stockMutationService
                    , _blanketService, _itemService, _warehouseItemService);
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
        public dynamic UnFinish(BlanketOrderDetail model)
        {
            try
            {

                var data = _blanketOrderDetailService.GetObjectById(model.Id);
                model = _blanketOrderDetailService.UnfinishObject(data, _blanketOrderService, _stockMutationService
                    , _blanketService, _itemService, _warehouseItemService);
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
        public dynamic Reject(BlanketOrderDetail model)
        {
            try
            {
                var data = _blanketOrderDetailService.GetObjectById(model.Id);
                model = _blanketOrderDetailService.RejectObject(data,model.RejectedDate.Value,_blanketOrderService,_stockMutationService
                    ,_blanketService,_itemService,_warehouseItemService);
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
        public dynamic UnReject(BlanketOrderDetail model)
        {
            try
            {

                var data = _blanketOrderDetailService.GetObjectById(model.Id);
                model = _blanketOrderDetailService.UndoRejectObject(data,_blanketOrderService,_stockMutationService
                    ,_blanketService,_itemService,_warehouseItemService);
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