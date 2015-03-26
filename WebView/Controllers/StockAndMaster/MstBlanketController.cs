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
    public class MstBlanketController : Controller
    {
        private readonly static log4net.ILog LOG = log4net.LogManager.GetLogger("BlanketController");
        private IItemService _itemService;
        private IUoMService _uomService;
        private IWarehouseService _warehouseService;
        private IItemTypeService _itemTypeService;
        private IMachineService _machineService;
        private IWarehouseItemService _warehouseItemService;
        private IBlanketService _blanketService;
        private IContactService _contactService;
        private IPriceMutationService _priceMutationService;
        private IPurchaseOrderDetailService _purchaseOrderDetailService;
        private IStockAdjustmentDetailService _stockAdjustmentDetailService;
        private ISalesOrderDetailService _salesOrderDetailService;
        private IStockMutationService _stockMutationService;
        private IBlanketOrderDetailService _blanketOrderDetailService;

        public MstBlanketController()
        {
            _itemTypeService = new ItemTypeService(new ItemTypeRepository(), new ItemTypeValidator());
            _warehouseItemService = new WarehouseItemService(new WarehouseItemRepository(), new WarehouseItemValidator());
            _itemService = new ItemService(new ItemRepository(), new ItemValidator());
            _uomService = new UoMService(new UoMRepository(), new UoMValidator());
            _warehouseService = new WarehouseService(new WarehouseRepository(), new WarehouseValidator());
             _machineService = new MachineService(new MachineRepository(), new MachineValidator());
            _blanketService = new BlanketService(new BlanketRepository(), new BlanketValidator());
            _contactService = new ContactService(new ContactRepository(),new ContactValidator());
            _priceMutationService = new PriceMutationService(new PriceMutationRepository(), new PriceMutationValidator());
            _purchaseOrderDetailService = new PurchaseOrderDetailService(new PurchaseOrderDetailRepository(), new PurchaseOrderDetailValidator());
            _stockAdjustmentDetailService = new StockAdjustmentDetailService(new StockAdjustmentDetailRepository(), new StockAdjustmentDetailValidator());
            _salesOrderDetailService = new SalesOrderDetailService(new SalesOrderDetailRepository(), new SalesOrderDetailValidator());
            _stockMutationService = new StockMutationService(new StockMutationRepository(), new StockMutationValidator());
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
            var q = _blanketService.GetQueryable().Include("UoM").Include("ItemType").Include("Machine")
                                                  .Include("Item").Include("Contact").Where(x => !x.IsDeleted);

            var query = (from model in q
                         select new
                         {
                            model.Id,
                            model.Sku,
                            model.Name,
                            model.RollNo,
                            model.Quantity,
                            UoM = model.UoM.Name,
                            model.AC,
                            model.AR,
                            model.thickness,
                            model.KS,
                            model.Description,
                            ItemType = model.ItemType.Name,
                            Machine = model.Machine.Name,
                            Adhesive = model.Adhesive.Name,
                            Adhesive2 = model.Adhesive2.Name,
                            RollBlanketItem = model.RollBlanketItem.Name,
                            LeftBarItem = model.LeftBarItem.Name,
                            RightBarItem = model.RightBarItem.Name,
                            Contact = model.Contact.Name,
                            model.ApplicationCase,
                            model.CroppingType,
                            model.LeftOverAC,
                            model.LeftOverAR,
                            model.Special,
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
                            model.Sku,
                            model.Name,
                            model.RollNo,
                            model.Quantity,
                            model.UoM,
                            model.AC,
                            model.AR,
                            model.thickness,
                            model.KS,
                            model.Description,
                            model.ItemType,
                            model.Machine,
                            model.Adhesive,
                            model.Adhesive2,
                            model.RollBlanketItem,
                            model.LeftBarItem,
                            model.RightBarItem,
                            model.Contact,
                            model.ApplicationCase,
                            model.CroppingType,
                            model.LeftOverAC,
                            model.LeftOverAR,
                            model.Special,
                            model.CreatedAt,
                            model.UpdatedAt,
                      }
                    }).ToArray()
            }, JsonRequestBehavior.AllowGet);
        }

        public dynamic GetListLookUpItems(string _search, long nd, int rows, int? page, string sidx, string sord, string filters = "")
        {
            // Construct where statement
            string strWhere = GeneralFunction.ConstructWhere(filters);
            string filter = null;
            GeneralFunction.ConstructWhereInLinq(strWhere, out filter);
            if (filter == "") filter = "true";

            // Get Data
            var q = _blanketService.GetQueryable().Include("UoM").Include("ItemType").Include("Machine")
                                                  .Include("Item").Include("Contact").Where(x => !x.IsDeleted);

            var query = (from model in q
                         select new
                         {
                             model.Id,
                             model.Sku,
                             model.Name,
                             RollBlanketItemSku = model.RollBlanketItem.Sku,
                             RollBlanketItem = model.RollBlanketItem.Name,
                             LeftBarItemSku = model.LeftBarItem.Sku,
                             LeftBarItem = model.LeftBarItem.Name,
                             RightBarItemSku = model.RightBarItem.Sku,
                             RightBarItem = model.RightBarItem.Name,
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
                            model.RollBlanketItemSku,
                            model.RollBlanketItem,
                            model.LeftBarItemSku,
                            model.LeftBarItem,
                            model.RightBarItemSku,
                            model.RightBarItem
                      }
                    }).ToArray()
            }, JsonRequestBehavior.AllowGet);
        }

        public dynamic GetListRollBlanket(string _search, long nd, int rows, int? page, string sidx, string sord, string filters = "")
        {
            // Construct where statement
            string strWhere = GeneralFunction.ConstructWhere(filters);
            string filter = null;
            GeneralFunction.ConstructWhereInLinq(strWhere, out filter);
            if (filter == "") filter = "true";

            // Get Data
            var q = _itemService.GetQueryable().Include("ItemType").Where(x => !x.IsDeleted && x.ItemType.Name == Core.Constants.Constant.ItemTypeCase.RollBlanket);

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

        public dynamic GetListBar(string _search, long nd, int rows, int? page, string sidx, string sord, string filters = "")
        {
            // Construct where statement
            string strWhere = GeneralFunction.ConstructWhere(filters);
            string filter = null;
            GeneralFunction.ConstructWhereInLinq(strWhere, out filter);
            if (filter == "") filter = "true";

            // Get Data
            var q = _itemService.GetQueryable().Include("ItemType").Where(x => !x.IsDeleted && x.ItemType.Name == Core.Constants.Constant.ItemTypeCase.Bar);

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

        public dynamic GetListAdhesiveBlanket(string _search, long nd, int rows, int? page, string sidx, string sord, string filters = "")
        {
            // Construct where statement
            string strWhere = GeneralFunction.ConstructWhere(filters);
            string filter = null;
            GeneralFunction.ConstructWhereInLinq(strWhere, out filter);
            if (filter == "") filter = "true";

            // Get Data
            var q = _itemService.GetQueryable().Include("ItemType").Where(x => !x.IsDeleted && x.ItemType.Name == Core.Constants.Constant.ItemTypeCase.AdhesiveBlanket);

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
            Blanket model = new Blanket();
            try
            {
                model = _blanketService.GetObjectById(Id);
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
                model.ItemTypeId,
                ItemType = _itemTypeService.GetObjectById(model.ItemTypeId).Name,
                model.UoMId,
                UoM = _uomService.GetObjectById(model.UoMId).Name,
                model.Sku,
                model.RollNo,
                model.ContactId,
                Contact =_contactService.GetObjectById(model.ContactId).Name,
                model.MachineId,
                Machine = _machineService.GetObjectById(model.MachineId).Name,
                model.AdhesiveId,
                Adhesive = model.AdhesiveId == null ? "" : _itemService.GetObjectById((int)model.AdhesiveId).Name,
                model.Adhesive2Id,
                Adhesive2 = model.Adhesive2Id == null ? "" : _itemService.GetObjectById((int)model.Adhesive2Id).Name,
                model.RollBlanketItemId,
                RollBlanketItem = _itemService.GetObjectById(model.RollBlanketItemId).Name,
                model.IsBarRequired,
                model.HasLeftBar,
                model.HasRightBar,
                LeftBarItemId = model.LeftBarItemId.HasValue ? model.LeftBarItemId.Value : model.LeftBarItemId = null,
                LeftBarItem = model.LeftBarItemId.HasValue ? _itemService.GetObjectById(model.LeftBarItemId.Value).Name : "",
                RightBarItemId = model.RightBarItemId.HasValue ? model.RightBarItemId.Value : model.RightBarItemId = null,
                RightBarItem = model.RightBarItemId.HasValue ? _itemService.GetObjectById(model.RightBarItemId.Value).Name : "",
                model.AC,
                model.AR, 
                model.thickness,
                model.KS,
                model.Quantity,
                model.ApplicationCase,
                model.CroppingType,
                model.LeftOverAC,
                model.LeftOverAR,
                model.Special,
                model.CreatedAt,
                model.UpdatedAt,
                model.Errors
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public dynamic Insert(Blanket model)
        {
            try
            {
                model = _blanketService.CreateObject(model,_blanketService,_uomService,
                    _itemService,_itemTypeService,_contactService,_machineService,_warehouseItemService,
                    _warehouseService,_priceMutationService);
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
        public dynamic Update(Blanket model)
        {
            try
            {
                var data = _blanketService.GetObjectById(model.Id);
                data.Name = model.Name;
                data.RollNo = model.RollNo;
                data.UoMId = model.UoMId;
                data.AC = model.AC;
                data.AR = model.AR;
                data.thickness = model.thickness;
                data.KS = model.KS;
                data.Description = model.Description;
                data.MachineId = model.MachineId;
                data.AdhesiveId = model.AdhesiveId;
                data.Adhesive2Id = model.Adhesive2Id;
                data.RollBlanketItemId = model.RollBlanketItemId;
                data.IsBarRequired = model.IsBarRequired;
                data.HasLeftBar = model.HasLeftBar;
                data.LeftBarItemId = model.LeftBarItemId;
                data.HasRightBar = model.HasRightBar;
                data.RightBarItemId = model.RightBarItemId;
                data.ContactId = model.ContactId;
                data.ApplicationCase = model.ApplicationCase;
                data.CroppingType = model.CroppingType;
                data.LeftOverAC = model.LeftOverAC;
                data.LeftOverAR = model.LeftOverAR;
                data.Special = model.Special;
                model = _blanketService.UpdateObject(data,_blanketService,_uomService,_itemService,_itemTypeService,_contactService,_machineService,_warehouseItemService,_warehouseService,_priceMutationService);
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
        public dynamic Delete(Blanket model)
        {
            try
            {
                var data = _blanketService.GetObjectById(model.Id);
                model = _blanketService.SoftDeleteObject(data,_itemTypeService,_warehouseItemService,_priceMutationService, _purchaseOrderDetailService,
                                                        _stockAdjustmentDetailService, _salesOrderDetailService,_stockMutationService, _blanketOrderDetailService);
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
