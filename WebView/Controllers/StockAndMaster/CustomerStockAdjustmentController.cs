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
    public class CustomerStockAdjustmentController : Controller
    {
        private readonly static log4net.ILog LOG = log4net.LogManager.GetLogger("CustomerStockAdjustmentController");
        private ICustomerStockAdjustmentService _customerStockAdjustmentService;
        private ICustomerStockAdjustmentDetailService _customerStockAdjustmentDetailService;
        private IWarehouseService _warehouseService;
        private IContactService _contactService;
        private IItemService _itemService;
        private IUoMService _uomService;
        private IWarehouseItemService _warehouseItemService;
        private ICustomerStockMutationService _customerStockMutationService;
        private ICustomerItemService _customerItemService;
        private IAccountService _accountService;
        private IGeneralLedgerJournalService _generalLedgerJournalService;
        private IClosingService _closingService;

        public CustomerStockAdjustmentController()
        {
            _accountService = new AccountService(new AccountRepository(), new AccountValidator());
            _closingService = new ClosingService(new ClosingRepository(), new ClosingValidator());
            _generalLedgerJournalService = new GeneralLedgerJournalService(new GeneralLedgerJournalRepository(), new GeneralLedgerJournalValidator());
            _customerStockAdjustmentService = new CustomerStockAdjustmentService(new CustomerStockAdjustmentRepository(), new CustomerStockAdjustmentValidator());
            _customerStockAdjustmentDetailService = new CustomerStockAdjustmentDetailService(new CustomerStockAdjustmentDetailRepository(), new CustomerStockAdjustmentDetailValidator());
            _warehouseService = new WarehouseService(new WarehouseRepository(), new WarehouseValidator());
            _itemService = new ItemService(new ItemRepository(), new ItemValidator());
            _uomService = new UoMService(new UoMRepository(), new UoMValidator());
            _warehouseItemService = new WarehouseItemService(new WarehouseItemRepository(), new WarehouseItemValidator());
            _customerStockMutationService = new CustomerStockMutationService(new CustomerStockMutationRepository(), new CustomerStockMutationValidator());
            _customerItemService = new CustomerItemService(new CustomerItemRepository(), new CustomerItemValidator());
            _contactService = new ContactService(new ContactRepository(), new ContactValidator());
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
            var q = _customerStockAdjustmentService.GetQueryable().Include("Warehouse").Include("Contact").Where(x => !x.IsDeleted);

            var query = (from model in q
                         select new
                         {
                             model.Id,
                             model.Code,
                             Contact = model.Contact.Name,
                             Warehouse = model.Warehouse.Name,
                             model.Description,
                             model.AdjustmentDate,
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
                            model.Contact,
                            model.Warehouse,
                            model.Description,
                            model.AdjustmentDate,
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
            var q = _customerStockAdjustmentDetailService.GetQueryable().Include("Item").Include("UoM")
                                                 .Where(x => x.CustomerStockAdjustmentId == id && !x.IsDeleted);

            var query = (from model in q
                         select new
                         {
                             model.Id,
                             model.Code,
                             ItemSku = model.Item.Sku,
                             Item = model.Item.Name,
                             model.Quantity,
                             UoM = model.Item.UoM.Name,
                             model.Price
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
                            model.ItemSku,
                            model.Item,
                            model.Quantity,
                            model.UoM,
                            model.Price
                      }
                    }).ToArray()
            }, JsonRequestBehavior.AllowGet);
        }

        public dynamic GetInfo(int Id)
        {
            CustomerStockAdjustment model = new CustomerStockAdjustment();
            try
            {
                model = _customerStockAdjustmentService.GetObjectById(Id);
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
                Contact = model.Contact.Name,
                model.WarehouseId,
                Warehouse = model.Warehouse.Name,
                model.AdjustmentDate,
                model.Description,
                model.Errors
            }, JsonRequestBehavior.AllowGet);
        }

        public dynamic GetInfoDetail(int Id)
        {
            CustomerStockAdjustmentDetail model = new CustomerStockAdjustmentDetail();
            try
            {
                model = _customerStockAdjustmentDetailService.GetObjectById(Id);
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
                model.Price,
                model.Errors
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public dynamic Insert(CustomerStockAdjustment model)
        {
            try
            {
                model = _customerStockAdjustmentService.CreateObject(model,_warehouseService, _contactService);
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
        public dynamic InsertDetail(CustomerStockAdjustmentDetail model)
        {
            try
            {
                model = _customerStockAdjustmentDetailService.CreateObject(model,_customerStockAdjustmentService,_itemService,_warehouseItemService,_customerItemService);
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
        public dynamic Update(CustomerStockAdjustment model)
        {
            try
            {
                var data = _customerStockAdjustmentService.GetObjectById(model.Id);
                data.AdjustmentDate = model.AdjustmentDate;
                data.Description = model.Description;
                data.WarehouseId = model.WarehouseId;
                data.ContactId = model.ContactId;
                model = _customerStockAdjustmentService.UpdateObject(data,_warehouseService,_contactService);
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
        public dynamic UpdateDetail(CustomerStockAdjustmentDetail model)
        {
            try
            {
                var data = _customerStockAdjustmentDetailService.GetObjectById(model.Id);
                data.ItemId = model.ItemId;
                data.Quantity = model.Quantity;
                data.Price = model.Price;
                model = _customerStockAdjustmentDetailService.UpdateObject(data,_customerStockAdjustmentService,_itemService,_warehouseItemService,_customerItemService);
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
        public dynamic Delete(CustomerStockAdjustment model)
        {
            try
            {
                var data = _customerStockAdjustmentService.GetObjectById(model.Id);
                model = _customerStockAdjustmentService.SoftDeleteObject(data, _customerStockAdjustmentDetailService);
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
        public dynamic DeleteDetail(CustomerStockAdjustmentDetail model)
        {
            try
            {
                var data = _customerStockAdjustmentDetailService.GetObjectById(model.Id);
                model = _customerStockAdjustmentDetailService.SoftDeleteObject(data);
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
        public dynamic Confirm(CustomerStockAdjustment model)
        {
            try
            {
                var data = _customerStockAdjustmentService.GetObjectById(model.Id);
                model = _customerStockAdjustmentService.ConfirmObject(data, model.ConfirmationDate.GetValueOrDefault(), _customerStockAdjustmentDetailService, _customerStockMutationService,
                                                              _itemService, _customerItemService, _warehouseItemService,
                                                              _accountService, _generalLedgerJournalService, _closingService);
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
        public dynamic UnConfirm(CustomerStockAdjustment model)
        {
            try
            {

                var data = _customerStockAdjustmentService.GetObjectById(model.Id);
                model = _customerStockAdjustmentService.UnconfirmObject(data,_customerStockAdjustmentDetailService,_customerStockMutationService,_itemService,
                                                                _customerItemService,_warehouseItemService,_accountService,_generalLedgerJournalService, _closingService);
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

