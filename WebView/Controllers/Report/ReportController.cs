using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Core.Interface.Service;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using Data.Repository;
using Service.Service;
using Validation.Validation;
using System.Linq.Dynamic;
using System.Data.Entity;
using Core.DomainModel;
using Core.Constants;
using Newtonsoft.Json;
using System.Data.Objects.SqlClient;
using System.Data.Objects;
using Data.Context;
using System.Globalization;
using System.Text;

namespace WebView.Controllers
{
    public class ReportController : Controller
    {
        //
        // GET: /Report/
        private readonly static log4net.ILog LOG = log4net.LogManager.GetLogger("ReportController");
        private IItemService _itemService;
        private IItemTypeService _itemTypeService;
        private IUoMService _uoMService;
        private ICompanyService _companyService;
        private ICashBankService _cashBankService;
        private IPayableService _payableService;
        private IReceivableService _receivableService;
        private ISalesQuotationDetailService _salesQuotationDetailService;
        private ISalesQuotationService _salesQuotationService;
        private ISalesOrderDetailService _salesOrderDetailService;
        private ISalesOrderService _salesOrderService;
        private IDeliveryOrderDetailService _deliveryOrderDetailService;
        private IDeliveryOrderService _deliveryOrderService;
        private ISalesInvoiceService _salesInvoiceService;
        private ISalesInvoiceDetailService _salesInvoiceDetailService;
        private IPurchaseOrderService _purchaseOrderService;
        private IPurchaseOrderDetailService _purchaseOrderDetailService;
        private IPurchaseReceivalDetailService _purchaseReceivalDetailService;
        private IPurchaseReceivalService _purchaseReceivalService;
        private IPurchaseInvoiceService _purchaseInvoiceService;
        private IPurchaseInvoiceDetailService _purchaseInvoiceDetailService;
        private IVirtualOrderDetailService _virtualOrderDetailService;
        private IVirtualOrderService _virtualOrderService;
        private ITemporaryDeliveryOrderDetailService _temporaryDeliveryOrderDetailService;
        private ITemporaryDeliveryOrderService _temporaryDeliveryOrderService;
        private IEmployeeService _employeeService;

        public class ModelProfitLoss
        {
            public DateTime StartDate { get; set; }
            public DateTime EndDate { get; set; }
            public decimal TotalSales { get; set; }
            public decimal TotalCoGS { get; set; }
            public decimal TotalSalesReturn { get; set; }
            public decimal TotalExpense { get; set; }
            public string CompanyName { get; set; }
            public string CompanyAddress { get; set; }
            public string CompanyContactNo { get; set; }
        }

        public class ModelFund
        {
            public DateTime FromDueDate { get; set; }
            public DateTime ToDueDate { get; set; }
            public DateTime CurDate { get; set; }
            public decimal cashBank { get; set; }
            public decimal receivable { get; set; }
            public decimal payable { get; set; }
            public decimal dailySalesProjection { get; set; }
            public string CompanyName { get; set; }
            public string CompanyAddress { get; set; }
            public string CompanyContactNo { get; set; }
        }

        public ReportController()
        {
            _itemService = new ItemService(new ItemRepository(), new ItemValidator());
            _itemTypeService = new ItemTypeService(new ItemTypeRepository(), new ItemTypeValidator());
            _uoMService = new UoMService(new UoMRepository(), new UoMValidator());
            _companyService = new CompanyService(new CompanyRepository(), new CompanyValidator());
            _cashBankService = new CashBankService(new CashBankRepository(), new CashBankValidator());
            _payableService = new PayableService(new PayableRepository(), new PayableValidator());
            _receivableService = new ReceivableService(new ReceivableRepository(), new ReceivableValidator());

            _salesQuotationDetailService = new SalesQuotationDetailService(new SalesQuotationDetailRepository(), new SalesQuotationDetailValidator());
            _salesQuotationService = new SalesQuotationService(new SalesQuotationRepository(), new SalesQuotationValidator());
            _salesOrderService = new SalesOrderService(new SalesOrderRepository(), new SalesOrderValidator());
            _salesOrderDetailService = new SalesOrderDetailService(new SalesOrderDetailRepository(), new SalesOrderDetailValidator());
            _deliveryOrderService = new DeliveryOrderService(new DeliveryOrderRepository(), new DeliveryOrderValidator());
            _deliveryOrderDetailService = new DeliveryOrderDetailService(new DeliveryOrderDetailRepository(), new DeliveryOrderDetailValidator());
            _salesInvoiceService = new SalesInvoiceService(new SalesInvoiceRepository(), new SalesInvoiceValidator());
            _salesInvoiceDetailService = new SalesInvoiceDetailService(new SalesInvoiceDetailRepository(), new SalesInvoiceDetailValidator());
            _purchaseOrderService = new PurchaseOrderService(new PurchaseOrderRepository(), new PurchaseOrderValidator());
            _purchaseOrderDetailService = new PurchaseOrderDetailService(new PurchaseOrderDetailRepository(), new PurchaseOrderDetailValidator());
            _purchaseReceivalService = new PurchaseReceivalService(new PurchaseReceivalRepository(), new PurchaseReceivalValidator());
            _purchaseReceivalDetailService = new PurchaseReceivalDetailService(new PurchaseReceivalDetailRepository(), new PurchaseReceivalDetailValidator());
            _purchaseInvoiceService = new PurchaseInvoiceService(new PurchaseInvoiceRepository(), new PurchaseInvoiceValidator());
            _purchaseInvoiceDetailService = new PurchaseInvoiceDetailService(new PurchaseInvoiceDetailRepository(), new PurchaseInvoiceDetailValidator());

            _virtualOrderDetailService = new VirtualOrderDetailService(new VirtualOrderDetailRepository(), new VirtualOrderDetailValidator());
            _virtualOrderService = new VirtualOrderService(new VirtualOrderRepository(), new VirtualOrderValidator());
            _temporaryDeliveryOrderDetailService = new TemporaryDeliveryOrderDetailService(new TemporaryDeliveryOrderDetailRepository(), new TemporaryDeliveryOrderDetailValidator());
            _temporaryDeliveryOrderService = new TemporaryDeliveryOrderService(new TemporaryDeliveryOrderRepository(), new TemporaryDeliveryOrderValidator());
            _employeeService = new EmployeeService(new EmployeeRepository(), new EmployeeValidator());
        }

        #region Item
        public ActionResult Item()
        {
            if (!AuthenticationModel.IsAllowed("View", Constant.MenuName.Item, Constant.MenuGroupName.Report))
            {
                return Content(Constant.ControllerOutput.PageViewNotAllowed);
            }

            return View();
        }

        public ActionResult ReportItem(int itemTypeId = 0)
        {
            var company = _companyService.GetQueryable().FirstOrDefault();
            var q = _itemService.GetQueryable().Include("ItemType").Include("UoM");

            string filter = "true";
            ItemType itemType = _itemTypeService.GetObjectById(itemTypeId);
            if (itemType != null)
            {
                filter = "ItemTypeId == " + itemTypeId.ToString();
            }

            var query = (from model in q
                         select new
                         {
                             model.Id,
                             model.Name,
                             model.ItemTypeId,
                             itemtype = model.ItemType.Name,
                             model.Sku,
                             uom = model.UoM.Name,
                             model.Quantity,
                             model.SellingPrice,
                             model.AvgPrice,
                             Margin = model.AvgPrice == 0 ? 0 : (model.SellingPrice - model.AvgPrice) / model.AvgPrice * 100,
                             model.PendingReceival,
                             model.PendingDelivery,
                             CompanyName = company.Name,
                             CompanyAddress = company.Address,
                             CompanyContactNo = company.ContactNo,
                             cogs = model.AvgPrice * model.Quantity,
                         }).Where(filter).ToList();

            if (!query.Any())
            {
                return Content(Constant.ControllerOutput.ErrorPageRecordNotFound);
            }

            var rd = new ReportDocument();

            //Loading Report
            rd.Load(Server.MapPath("~/") + "Reports/General/Item.rpt");

            // Setting report data source
            rd.SetDataSource(query);

            var stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            return File(stream, "application/pdf");
        }
        #endregion

        #region PurchaseOrder
        public ActionResult PurchaseOrder()
        {
            return View();
        }

        public ActionResult ReportPurchaseOrder(int Id)
        {
            var company = _companyService.GetQueryable().FirstOrDefault();
            var q = _purchaseOrderDetailService.GetQueryable().Include("PurchaseOrder")
                                               .Include("Item").Include("UoM").Include("Contact")
                                               .Where(x => x.PurchaseOrderId == Id && !x.IsDeleted);
            string user = AuthenticationModel.GetUserName();

            var query = (from model in q
                         select new
                         {
                             SKU = model.Item.Sku,
                             Name = model.Item.Name,
                             UoM = model.Item.UoM.Name,
                             model.Quantity,
                             Price = model.Price,
                             Discount = 0,
                             GlobalDiscount = 0,
                             Tax = model.PurchaseOrder.Contact.IsTaxable ?
                                   (model.PurchaseOrder.Contact.TaxCode == "01") ? Constant.TaxValue.Code01 :
                                   (model.PurchaseOrder.Contact.TaxCode == "02") ? Constant.TaxValue.Code02 :
                                   (model.PurchaseOrder.Contact.TaxCode == "03") ? Constant.TaxValue.Code03 :
                                   (model.PurchaseOrder.Contact.TaxCode == "04") ? Constant.TaxValue.Code04 :
                                   (model.PurchaseOrder.Contact.TaxCode == "05") ? Constant.TaxValue.Code05 :
                                   (model.PurchaseOrder.Contact.TaxCode == "06") ? Constant.TaxValue.Code06 :
                                   (model.PurchaseOrder.Contact.TaxCode == "07") ? Constant.TaxValue.Code07 :
                                   (model.PurchaseOrder.Contact.TaxCode == "08") ? Constant.TaxValue.Code08 :
                                   (model.PurchaseOrder.Contact.TaxCode == "09") ? Constant.TaxValue.Code09 : 0 : 0,
                             Allowance = 0,
                             Code = model.PurchaseOrder.NomorSurat,
                             Date = model.PurchaseOrder.ConfirmationDate.Value,
                             Contact = model.PurchaseOrder.Contact.Name,
                             CompanyName = company.Name,
                             CompanyAddress = company.Address,
                             CompanyContactNo = company.ContactNo,
                             User = user,
                             Description = "",
                         }).ToList();

            if (!query.Any())
            {
                return Content(Constant.ControllerOutput.ErrorPageRecordNotFound);
            }

            var rd = new ReportDocument();

            //Loading Report
            rd.Load(Server.MapPath("~/") + "Reports/General/PurchaseOrder.rpt");

            // Setting report data source
            rd.SetDataSource(query);

            var stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            return File(stream, "application/pdf");
        }
        #endregion

        #region PurchaseReceival
        public ActionResult PurchaseReceival()
        {
            return View();
        }

        public ActionResult ReportPurchaseReceival(int Id)
        {
            var company = _companyService.GetQueryable().FirstOrDefault();
            var q = _purchaseReceivalDetailService.GetQueryable().Include("PurchaseReceival")
                                              .Include("PurchaseOrderDetail").Include("Item").Include("UoM").Include("Contact")
                                              .Where(x => x.PurchaseReceivalId == Id && !x.IsDeleted);
            string user = AuthenticationModel.GetUserName();

            var query = (from model in q
                         select new
                         {
                             SKU = model.Item.Sku,
                             Name = model.Item.Name,
                             UoM = model.Item.UoM.Name,
                             model.Quantity,
                             Price = 0,
                             Discount = 0,
                             GlobalDiscount = 0,
                             Tax = 0,
                             Allowance = 0,
                             Code = model.PurchaseReceival.NomorSurat,
                             Date = model.PurchaseReceival.ConfirmationDate.Value,
                             Contact = model.PurchaseReceival.PurchaseOrder.Contact.Name,
                             CompanyName = company.Name,
                             CompanyAddress = company.Address,
                             CompanyContactNo = company.ContactNo,
                             User = user,
                             Description = "",
                         }).ToList();

            if (!query.Any())
            {
                return Content(Constant.ControllerOutput.ErrorPageRecordNotFound);
            }

            var rd = new ReportDocument();

            //Loading Report
            rd.Load(Server.MapPath("~/") + "Reports/General/PurchaseReceival.rpt");

            // Setting report data source
            rd.SetDataSource(query);

            var stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            return File(stream, "application/pdf");
        }
        #endregion

        #region PurchaseInvoice
        public ActionResult PurchaseInvoice()
        {
            return View();
        }

        public ActionResult ReportPurchaseInvoice(int Id)
        {
            var company = _companyService.GetQueryable().FirstOrDefault();
            var q = _purchaseInvoiceDetailService.GetQueryable().Include("PurchaseInvoice").Include("PurchaseReceivalDetail")
                                                 .Include("PurchaseOrderDetail").Include("Item").Include("UoM")
                                                 .Where(x => x.PurchaseInvoiceId == Id && !x.IsDeleted);
            string user = AuthenticationModel.GetUserName();

            var query = (from model in q
                         select new
                         {
                             SKU = model.PurchaseReceivalDetail.Item.Sku,
                             Name = model.PurchaseReceivalDetail.Item.Name,
                             UoM = model.PurchaseReceivalDetail.Item.UoM.Name,
                             model.Quantity,
                             Price = model.Amount / model.Quantity,
                             Discount = 0,
                             GlobalDiscount = model.PurchaseInvoice.Discount,
                             Tax = model.PurchaseInvoice.Tax,
                             Allowance = 0,
                             Code = model.PurchaseInvoice.NomorSurat,
                             Date = model.PurchaseInvoice.ConfirmationDate.Value,
                             Contact = model.PurchaseInvoice.PurchaseReceival.PurchaseOrder.Contact.Name,
                             CompanyName = company.Name,
                             CompanyAddress = company.Address,
                             CompanyContactNo = company.ContactNo,
                             User = user,
                             Description = model.PurchaseInvoice.Description,
                         }).ToList();

            if (!query.Any())
            {
                return Content(Constant.ControllerOutput.ErrorPageRecordNotFound);
            }

            var rd = new ReportDocument();

            //Loading Report
            rd.Load(Server.MapPath("~/") + "Reports/General/PurchaseInvoice.rpt");

            // Setting report data source
            rd.SetDataSource(query);

            var stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            return File(stream, "application/pdf");
        }
        #endregion

        #region VirtualOrder
        public ActionResult VirtualOrder()
        {
            return View();
        }

        public ActionResult ReportVirtualOrder(int Id)
        {
            var company = _companyService.GetQueryable().FirstOrDefault();
            //var salesOrder = _salesOrderService.GetObjectById(Id);
            var q = _virtualOrderDetailService.GetQueryable().Include("VirtualOrder")
                                              .Include("VirtualOrderDetail").Include("Item").Include("UoM").Include("Contact")
                                              .Where(x => x.VirtualOrderId == Id && !x.IsDeleted);
            string user = AuthenticationModel.GetUserName();

            var query = (from model in q
                         select new
                         {
                             OrderType = model.VirtualOrder.OrderType == Constant.OrderTypeCase.SampleOrder ? "SAMPLE ORDER" : "TRIAL ORDER",
                             SKU = model.Item.Sku,
                             Name = model.Item.Name,
                             UoM = model.Item.UoM.Name,
                             model.Quantity,
                             Price = model.Price,
                             Discount = 0,
                             GlobalDiscount = 0,
                             Tax = model.VirtualOrder.Contact.IsTaxable ?
                                   (model.VirtualOrder.Contact.TaxCode == "01") ? Constant.TaxValue.Code01 :
                                   (model.VirtualOrder.Contact.TaxCode == "02") ? Constant.TaxValue.Code02 :
                                   (model.VirtualOrder.Contact.TaxCode == "03") ? Constant.TaxValue.Code03 :
                                   (model.VirtualOrder.Contact.TaxCode == "04") ? Constant.TaxValue.Code04 :
                                   (model.VirtualOrder.Contact.TaxCode == "05") ? Constant.TaxValue.Code05 :
                                   (model.VirtualOrder.Contact.TaxCode == "06") ? Constant.TaxValue.Code06 :
                                   (model.VirtualOrder.Contact.TaxCode == "07") ? Constant.TaxValue.Code07 :
                                   (model.VirtualOrder.Contact.TaxCode == "08") ? Constant.TaxValue.Code08 :
                                   (model.VirtualOrder.Contact.TaxCode == "09") ? Constant.TaxValue.Code09 : 0 : 0,
                             Allowance = 0,
                             Code = model.VirtualOrder.NomorSurat,
                             Date = model.VirtualOrder.ConfirmationDate.Value,
                             Contact = model.VirtualOrder.Contact.Name,
                             CompanyName = company.Name,
                             CompanyAddress = company.Address,
                             CompanyContactNo = company.ContactNo,
                             User = user,
                             Description = "",
                         }).ToList();

            if (!query.Any())
            {
                return Content(Constant.ControllerOutput.ErrorPageRecordNotFound);
            }

            var rd = new ReportDocument();

            //Loading Report
            rd.Load(Server.MapPath("~/") + "Reports/General/VirtualOrder.rpt");

            // Setting report data source
            rd.SetDataSource(query);

            var stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            return File(stream, "application/pdf");
        }
        #endregion

        #region TemporaryDeliveryOrder
        public ActionResult TemporaryDeliveryOrder()
        {
            return View();
        }

        public ActionResult ReportTemporaryDeliveryOrder(int Id)
        {
            var company = _companyService.GetQueryable().FirstOrDefault();
            //var salesInvoice = _salesInvoiceService.GetObjectById(Id);
            var q = _temporaryDeliveryOrderDetailService.GetQueryable().Include("TemporaryDeliveryOrder")
                                              .Include("DeliveryOrder").Include("SalesOrder").Include("Item")
                                              .Include("UoM").Include("Contact")
                                              .Where(x => x.TemporaryDeliveryOrderId == Id && !x.IsDeleted);
            string user = AuthenticationModel.GetUserName();

            var query = (from model in q
                         select new
                         {
                             OrderType = (model.TemporaryDeliveryOrder.OrderType == Constant.OrderTypeCase.PartDeliveryOrder) ? "Part Delivery Order" :
                                         (model.TemporaryDeliveryOrder.OrderType == Constant.OrderTypeCase.TrialOrder) ? "Trial Order" :
                                         (model.TemporaryDeliveryOrder.OrderType == Constant.OrderTypeCase.SampleOrder) ? "Sample Order" :
                                         (model.TemporaryDeliveryOrder.OrderType == Constant.OrderTypeCase.Consignment) ? "Consignment Order" : "",
                             SKU = model.Item.Sku,
                             Name = model.Item.Name,
                             UoM = model.Item.UoM.Name,
                             model.Quantity,
                             Price = 0,
                             Discount = 0,
                             GlobalDiscount = 0,
                             Tax = 0,
                             Allowance = 0,
                             Code = model.TemporaryDeliveryOrder.NomorSurat,
                             Date = model.TemporaryDeliveryOrder.ConfirmationDate.Value,
                             Contact = (model.TemporaryDeliveryOrder.OrderType == Constant.OrderTypeCase.PartDeliveryOrder) ?
                                        model.TemporaryDeliveryOrder.DeliveryOrder.SalesOrder.Contact.Name :
                                        model.TemporaryDeliveryOrder.VirtualOrder.Contact.Name,
                             CompanyName = company.Name,
                             CompanyAddress = company.Address,
                             CompanyContactNo = company.ContactNo,
                             User = user,
                             Description = "",
                         }).ToList();

            if (!query.Any())
            {
                return Content(Constant.ControllerOutput.ErrorPageRecordNotFound);
            }

            var rd = new ReportDocument();

            //Loading Report
            rd.Load(Server.MapPath("~/") + "Reports/General/TemporaryDeliveryOrder.rpt");

            // Setting report data source
            rd.SetDataSource(query);

            var stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            return File(stream, "application/pdf");
        }
        #endregion

        #region SalesQuotation
        public ActionResult SalesQuotation()
        {
            return View();
        }

        public ActionResult ReportSalesQuotation(int Id)
        {
            var company = _companyService.GetQueryable().FirstOrDefault();
            //var salesInvoice = _salesInvoiceService.GetObjectById(Id);
            var q = _salesQuotationDetailService.GetQueryable().Include("SalesQuotation")
                                              .Include("SalesQuotationDetail").Include("Item").Include("UoM").Include("Contact")
                                              .Where(x => x.SalesQuotationId == Id && !x.IsDeleted);
            string user = AuthenticationModel.GetUserName();

            var query = (from model in q
                         select new
                         {
                             SKU = model.Item.Sku,
                             Name = model.Item.Name,
                             UoM = model.Item.UoM.Name,
                             model.Quantity,
                             Price = model.QuotationPrice,
                             Discount = 0,
                             GlobalDiscount = 0,
                             Tax = model.SalesQuotation.Contact.IsTaxable ?
                                   (model.SalesQuotation.Contact.TaxCode == "01") ? Constant.TaxValue.Code01 :
                                   (model.SalesQuotation.Contact.TaxCode == "02") ? Constant.TaxValue.Code02 :
                                   (model.SalesQuotation.Contact.TaxCode == "03") ? Constant.TaxValue.Code03 :
                                   (model.SalesQuotation.Contact.TaxCode == "04") ? Constant.TaxValue.Code04 :
                                   (model.SalesQuotation.Contact.TaxCode == "05") ? Constant.TaxValue.Code05 :
                                   (model.SalesQuotation.Contact.TaxCode == "06") ? Constant.TaxValue.Code06 :
                                   (model.SalesQuotation.Contact.TaxCode == "07") ? Constant.TaxValue.Code07 :
                                   (model.SalesQuotation.Contact.TaxCode == "08") ? Constant.TaxValue.Code08 :
                                   (model.SalesQuotation.Contact.TaxCode == "09") ? Constant.TaxValue.Code09 : 0 : 0,
                             Allowance = 0,
                             Code = model.SalesQuotation.NomorSurat,
                             VersionNo = model.SalesQuotation.VersionNo,
                             Date = model.SalesQuotation.ConfirmationDate.Value,
                             Contact = model.SalesQuotation.Contact.Name,
                             CompanyName = company.Name,
                             CompanyAddress = company.Address,
                             CompanyContactNo = company.ContactNo,
                             User = user,
                             Description = "",
                         }).ToList();

            if (!query.Any())
            {
                return Content(Constant.ControllerOutput.ErrorPageRecordNotFound);
            }

            var rd = new ReportDocument();

            //Loading Report
            rd.Load(Server.MapPath("~/") + "Reports/General/SalesQuotation.rpt");

            // Setting report data source
            rd.SetDataSource(query);

            var stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            return File(stream, "application/pdf");
        }
        #endregion

        #region SalesOrder
        public ActionResult SalesOrder()
        {
            return View();
        }

        public ActionResult ReportSalesOrder(int Id)
        {
            var company = _companyService.GetQueryable().FirstOrDefault();
            //var salesOrder = _salesOrderService.GetObjectById(Id);
            var q = _salesOrderDetailService.GetQueryable().Include("SalesOrder")
                                              .Include("SalesOrderDetail").Include("Item").Include("UoM").Include("Contact")
                                              .Where(x => x.SalesOrderId == Id && !x.IsDeleted);
            string user = AuthenticationModel.GetUserName();

            var query = (from model in q
                         select new
                         {
                             SKU = model.Item.Sku,
                             Name = model.Item.Name,
                             UoM = model.Item.UoM.Name,
                             model.Quantity,
                             Price = model.Price,
                             Discount = 0,
                             GlobalDiscount = 0,
                             Tax = model.SalesOrder.Contact.IsTaxable ?
                                   (model.SalesOrder.Contact.TaxCode == "01") ? Constant.TaxValue.Code01 :
                                   (model.SalesOrder.Contact.TaxCode == "02") ? Constant.TaxValue.Code02 :
                                   (model.SalesOrder.Contact.TaxCode == "03") ? Constant.TaxValue.Code03 :
                                   (model.SalesOrder.Contact.TaxCode == "04") ? Constant.TaxValue.Code04 :
                                   (model.SalesOrder.Contact.TaxCode == "05") ? Constant.TaxValue.Code05 :
                                   (model.SalesOrder.Contact.TaxCode == "06") ? Constant.TaxValue.Code06 :
                                   (model.SalesOrder.Contact.TaxCode == "07") ? Constant.TaxValue.Code07 :
                                   (model.SalesOrder.Contact.TaxCode == "08") ? Constant.TaxValue.Code08 :
                                   (model.SalesOrder.Contact.TaxCode == "09") ? Constant.TaxValue.Code09 : 0 : 0,
                             Allowance = 0,
                             Code = model.SalesOrder.NomorSurat,
                             Date = model.SalesOrder.ConfirmationDate.Value,
                             Contact = model.SalesOrder.Contact.Name,
                             CompanyName = company.Name,
                             CompanyAddress = company.Address,
                             CompanyContactNo = company.ContactNo,
                             User = user,
                             Description = "",
                         }).ToList();

            if (!query.Any())
            {
                return Content(Constant.ControllerOutput.ErrorPageRecordNotFound);
            }

            var rd = new ReportDocument();

            //Loading Report
            rd.Load(Server.MapPath("~/") + "Reports/General/SalesOrder.rpt");

            // Setting report data source
            rd.SetDataSource(query);

            var stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            return File(stream, "application/pdf");
        }
        #endregion

        #region DeliveryOrder
        public ActionResult DeliveryOrder()
        {
            return View();
        }

        public ActionResult ReportDeliveryOrder(int Id)
        {
            var company = _companyService.GetQueryable().FirstOrDefault();
            //var salesInvoice = _salesInvoiceService.GetObjectById(Id);
            var q = _deliveryOrderDetailService.GetQueryable().Include("DeliveryOrder")
                                              .Include("SalesOrderDetail").Include("Item").Include("UoM").Include("Contact")
                                              .Where(x => x.DeliveryOrderId == Id && !x.IsDeleted);
            string user = AuthenticationModel.GetUserName();

            var query = (from model in q
                         select new
                         {
                             SKU = model.Item.Sku,
                             Name = model.Item.Name,
                             UoM = model.Item.UoM.Name,
                             model.Quantity,
                             Price = 0,
                             Discount = 0,
                             GlobalDiscount = 0,
                             Tax = 0,
                             Allowance = 0,
                             Code = model.DeliveryOrder.NomorSurat,
                             Date = model.DeliveryOrder.ConfirmationDate.Value,
                             Contact = model.DeliveryOrder.SalesOrder.Contact.Name,
                             CompanyName = company.Name,
                             CompanyAddress = company.Address,
                             CompanyContactNo = company.ContactNo,
                             User = user,
                             Description = "",
                         }).ToList();

            if (!query.Any())
            {
                return Content(Constant.ControllerOutput.ErrorPageRecordNotFound);
            }

            var rd = new ReportDocument();

            //Loading Report
            rd.Load(Server.MapPath("~/") + "Reports/General/DeliveryOrder.rpt");

            // Setting report data source
            rd.SetDataSource(query);

            var stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            return File(stream, "application/pdf");
        }
        #endregion

        #region SalesInvoice
        public ActionResult SalesInvoice()
        {
            return View();
        }

        public ActionResult ReportSalesInvoice(int Id)
        {
            var company = _companyService.GetQueryable().FirstOrDefault();
            //var salesInvoice = _salesInvoiceService.GetObjectById(Id);
            var q = _salesInvoiceDetailService.GetQueryable().Include("SalesInvoice").Include("DeliveryOrderDetail")
                                              .Include("SalesOrderDetail").Include("Item").Include("UoM").Include("Contact")
                                              .Where(x => x.SalesInvoiceId == Id && !x.IsDeleted);
            string user = AuthenticationModel.GetUserName();

            var query = (from model in q
                         select new
                         {
                             SKU = model.DeliveryOrderDetail.Item.Sku,
                             Name = model.DeliveryOrderDetail.Item.Name,
                             UoM = model.DeliveryOrderDetail.Item.UoM.Name,
                             model.Quantity,
                             Price = model.Amount / model.Quantity,
                             Discount = 0,
                             GlobalDiscount = model.SalesInvoice.Discount,
                             Tax = model.SalesInvoice.Tax,
                             Allowance = 0,
                             Code = model.SalesInvoice.NomorSurat,
                             Date = model.SalesInvoice.ConfirmationDate.Value,
                             Contact = model.SalesInvoice.DeliveryOrder.SalesOrder.Contact.Name,
                             CompanyName = company.Name,
                             CompanyAddress = company.Address,
                             CompanyContactNo = company.ContactNo,
                             User = user,
                             Description = model.SalesInvoice.Description,
                         }).ToList();

            if (!query.Any())
            {
                return Content(Constant.ControllerOutput.ErrorPageRecordNotFound);
            }

            var rd = new ReportDocument();

            //Loading Report
            rd.Load(Server.MapPath("~/") + "Reports/General/SalesInvoice.rpt");

            // Setting report data source
            rd.SetDataSource(query);

            var stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            return File(stream, "application/pdf");
        }
        #endregion

        #region CustomerGroupSalesQuarterly
        public ActionResult CustomerGroupSalesQuarterly()
        {
            return View();
        }

        public ActionResult ReportCustomerGroupSalesQuarterly(DateTime startQ1Date, DateTime endQ1Date, DateTime startQ2Date, DateTime endQ2Date, DateTime startQ3Date, DateTime endQ3Date, DateTime startQ4Date, DateTime endQ4Date)
        {
            DateTime endQ1 = endQ1Date.Date.AddDays(1);
            DateTime endQ2 = endQ2Date.Date.AddDays(1);
            DateTime endQ3 = endQ3Date.Date.AddDays(1);
            DateTime endQ4 = endQ4Date.Date.AddDays(1);
            var company = _companyService.GetQueryable().FirstOrDefault();
            //var salesInvoice = _salesInvoiceService.GetObjectById(Id);
            var q = _salesInvoiceDetailService.GetQueryable().Include(x => x.SalesInvoice).Include(x => x.DeliveryOrderDetail)
                                              .Where(x => !x.IsDeleted && (
                                                        (x.SalesInvoice.DeliveryOrder.SalesOrder.SalesDate >= startQ1Date && x.SalesInvoice.DeliveryOrder.SalesOrder.SalesDate < endQ1) ||
                                                        (x.SalesInvoice.DeliveryOrder.SalesOrder.SalesDate >= startQ2Date && x.SalesInvoice.DeliveryOrder.SalesOrder.SalesDate < endQ2) ||
                                                        (x.SalesInvoice.DeliveryOrder.SalesOrder.SalesDate >= startQ3Date && x.SalesInvoice.DeliveryOrder.SalesOrder.SalesDate < endQ3) ||
                                                        (x.SalesInvoice.DeliveryOrder.SalesOrder.SalesDate >= startQ4Date && x.SalesInvoice.DeliveryOrder.SalesOrder.SalesDate < endQ4)
                                                    ));
            string user = AuthenticationModel.GetUserName();

            var query = q.GroupBy(m => new
            {
                CustomerGroup = m.SalesInvoice.DeliveryOrder.SalesOrder.Contact.ContactGroup.Name, //.NamaFakturPajak,
                //SalesDate = m.SalesInvoice.DeliveryOrder.SalesOrder.SalesDate,
                Currency = (m.SalesInvoice.DeliveryOrder.SalesOrder.Currency.Name == "Rupiah") ? "IDR" : m.SalesInvoice.DeliveryOrder.SalesOrder.Currency.Name,
            }).Select(g => new
            {
                CustomerGroup = g.Key.CustomerGroup, //g.FirstOrDefault().SalesInvoice.DeliveryOrder.SalesOrder.Contact.NamaFakturPajak, //g.Key.CustomerGroup,
                Currency = g.Key.Currency, //(g.FirstOrDefault().SalesInvoice.DeliveryOrder.SalesOrder.Currency.Name == "Rupiah") ? "IDR" : g.FirstOrDefault().SalesInvoice.DeliveryOrder.SalesOrder.Currency.Name,
                //SalesDate = g.Key.SalesDate,
                AmountQ1 = g.Where(x => (x.SalesInvoice.DeliveryOrder.SalesOrder.SalesDate >= startQ1Date && x.SalesInvoice.DeliveryOrder.SalesOrder.SalesDate < endQ1)).Sum(x => (Decimal?)x.SalesInvoice.AmountReceivable)??0,
                AmountQ2 = g.Where(x => (x.SalesInvoice.DeliveryOrder.SalesOrder.SalesDate >= startQ2Date && x.SalesInvoice.DeliveryOrder.SalesOrder.SalesDate < endQ2)).Sum(x => (Decimal?)x.SalesInvoice.AmountReceivable)??0,
                AmountQ3 = g.Where(x => (x.SalesInvoice.DeliveryOrder.SalesOrder.SalesDate >= startQ3Date && x.SalesInvoice.DeliveryOrder.SalesOrder.SalesDate < endQ3)).Sum(x => (Decimal?)x.SalesInvoice.AmountReceivable)??0,
                AmountQ4 = g.Where(x => (x.SalesInvoice.DeliveryOrder.SalesOrder.SalesDate >= startQ4Date && x.SalesInvoice.DeliveryOrder.SalesOrder.SalesDate < endQ4)).Sum(x => (Decimal?)x.SalesInvoice.AmountReceivable)??0,
            }).ToList();

            if (!query.Any())
            {
                return Content(Constant.ControllerOutput.ErrorPageRecordNotFound);
            }
                
            var rd = new ReportDocument();

            //Loading Report
            rd.Load(Server.MapPath("~/") + "Reports/General/CustomerGroupSalesQuarterlyReportByValue.rpt");

            // Setting report data source
            rd.SetDataSource(query);

            // Setting subreport data source
            //rd.Subreports["subreport.rpt"].SetDataSource(q2);

            // Set parameters, need to be done after all data sources are set (to prevent reseting parameters)
            rd.SetParameterValue("CompanyName", company.Name);
            rd.SetParameterValue("AsOfDate", DateTime.Today);
            rd.SetParameterValue("Q1", startQ1Date.ToString("MMM") + "'" + startQ1Date.ToString("yy") + "-" + endQ1Date.ToString("MMM") + "'" + endQ1Date.ToString("yy"));
            rd.SetParameterValue("Q2", startQ2Date.ToString("MMM") + "'" + startQ2Date.ToString("yy") + "-" + endQ2Date.ToString("MMM") + "'" + endQ2Date.ToString("yy"));
            rd.SetParameterValue("Q3", startQ3Date.ToString("MMM") + "'" + startQ3Date.ToString("yy") + "-" + endQ3Date.ToString("MMM") + "'" + endQ3Date.ToString("yy"));
            rd.SetParameterValue("Q4", startQ4Date.ToString("MMM") + "'" + startQ4Date.ToString("yy") + "-" + endQ4Date.ToString("MMM") + "'" + endQ4Date.ToString("yy"));
            rd.SetParameterValue("startQ1", startQ1Date.Date); //monthYear.ToString("MMMM yyyy", ci)
            rd.SetParameterValue("endQ1", endQ1.Date);
            rd.SetParameterValue("startQ2", startQ2Date.Date);
            rd.SetParameterValue("endQ2", endQ2.Date);
            rd.SetParameterValue("startQ3", startQ3Date.Date);
            rd.SetParameterValue("endQ3", endQ3.Date);
            rd.SetParameterValue("startQ4", startQ4Date.Date);
            rd.SetParameterValue("endQ4", endQ4.Date);

            var stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            return File(stream, "application/pdf");
        }
        #endregion

        #region CustomerSalesQuarterly
        public ActionResult CustomerSalesQuarterly()
        {
            return View();
        }

        public ActionResult ReportCustomerSalesQuarterly(DateTime startQ1Date, DateTime endQ1Date, DateTime startQ2Date, DateTime endQ2Date, DateTime startQ3Date, DateTime endQ3Date, DateTime startQ4Date, DateTime endQ4Date)
        {
            DateTime endQ1 = endQ1Date.Date.AddDays(1);
            DateTime endQ2 = endQ2Date.Date.AddDays(1);
            DateTime endQ3 = endQ3Date.Date.AddDays(1);
            DateTime endQ4 = endQ4Date.Date.AddDays(1);
            var company = _companyService.GetQueryable().FirstOrDefault();
            //var salesInvoice = _salesInvoiceService.GetObjectById(Id);
            var q = _salesInvoiceDetailService.GetQueryable().Include(x => x.SalesInvoice).Include(x => x.DeliveryOrderDetail)
                                              .Where(x => !x.IsDeleted && (
                                                        (x.SalesInvoice.DeliveryOrder.SalesOrder.SalesDate >= startQ1Date && x.SalesInvoice.DeliveryOrder.SalesOrder.SalesDate < endQ1) ||
                                                        (x.SalesInvoice.DeliveryOrder.SalesOrder.SalesDate >= startQ2Date && x.SalesInvoice.DeliveryOrder.SalesOrder.SalesDate < endQ2) ||
                                                        (x.SalesInvoice.DeliveryOrder.SalesOrder.SalesDate >= startQ3Date && x.SalesInvoice.DeliveryOrder.SalesOrder.SalesDate < endQ3) ||
                                                        (x.SalesInvoice.DeliveryOrder.SalesOrder.SalesDate >= startQ4Date && x.SalesInvoice.DeliveryOrder.SalesOrder.SalesDate < endQ4)
                                                    ));
            string user = AuthenticationModel.GetUserName();

            var query = q.GroupBy(m => new
            {
                CustomerName = m.SalesInvoice.DeliveryOrder.SalesOrder.Contact.Name,
                //SalesDate = m.SalesInvoice.DeliveryOrder.SalesOrder.SalesDate,
                Currency = (m.SalesInvoice.DeliveryOrder.SalesOrder.Currency.Name == "Rupiah") ? "IDR" : m.SalesInvoice.DeliveryOrder.SalesOrder.Currency.Name,
            }).Select(g => new
            {
                CustomerName = g.Key.CustomerName, //g.FirstOrDefault().SalesInvoice.DeliveryOrder.SalesOrder.Contact.NamaFakturPajak, //g.Key.CustomerGroup,
                Currency = g.Key.Currency, //(g.FirstOrDefault().SalesInvoice.DeliveryOrder.SalesOrder.Currency.Name == "Rupiah") ? "IDR" : g.FirstOrDefault().SalesInvoice.DeliveryOrder.SalesOrder.Currency.Name,
                //SalesDate = g.Key.SalesDate,
                AmountQ1 = g.Where(x => (x.SalesInvoice.DeliveryOrder.SalesOrder.SalesDate >= startQ1Date && x.SalesInvoice.DeliveryOrder.SalesOrder.SalesDate < endQ1)).Sum(x => (Decimal?)x.SalesInvoice.AmountReceivable) ?? 0,
                AmountQ2 = g.Where(x => (x.SalesInvoice.DeliveryOrder.SalesOrder.SalesDate >= startQ2Date && x.SalesInvoice.DeliveryOrder.SalesOrder.SalesDate < endQ2)).Sum(x => (Decimal?)x.SalesInvoice.AmountReceivable) ?? 0,
                AmountQ3 = g.Where(x => (x.SalesInvoice.DeliveryOrder.SalesOrder.SalesDate >= startQ3Date && x.SalesInvoice.DeliveryOrder.SalesOrder.SalesDate < endQ3)).Sum(x => (Decimal?)x.SalesInvoice.AmountReceivable) ?? 0,
                AmountQ4 = g.Where(x => (x.SalesInvoice.DeliveryOrder.SalesOrder.SalesDate >= startQ4Date && x.SalesInvoice.DeliveryOrder.SalesOrder.SalesDate < endQ4)).Sum(x => (Decimal?)x.SalesInvoice.AmountReceivable) ?? 0,
            }).ToList();

            if (!query.Any())
            {
                return Content(Constant.ControllerOutput.ErrorPageRecordNotFound);
            }

            var rd = new ReportDocument();

            //Loading Report
            rd.Load(Server.MapPath("~/") + "Reports/General/CustomerSalesQuarterlyReportByValue.rpt");

            // Setting report data source
            rd.SetDataSource(query);

            // Setting subreport data source
            //rd.Subreports["subreport.rpt"].SetDataSource(q2);

            // Set parameters, need to be done after all data sources are set (to prevent reseting parameters)
            rd.SetParameterValue("CompanyName", company.Name);
            rd.SetParameterValue("AsOfDate", DateTime.Today);
            rd.SetParameterValue("Q1", startQ1Date.ToString("MMM") + "'" + startQ1Date.ToString("yy") + "-" + endQ1Date.ToString("MMM") + "'" + endQ1Date.ToString("yy"));
            rd.SetParameterValue("Q2", startQ2Date.ToString("MMM") + "'" + startQ2Date.ToString("yy") + "-" + endQ2Date.ToString("MMM") + "'" + endQ2Date.ToString("yy"));
            rd.SetParameterValue("Q3", startQ3Date.ToString("MMM") + "'" + startQ3Date.ToString("yy") + "-" + endQ3Date.ToString("MMM") + "'" + endQ3Date.ToString("yy"));
            rd.SetParameterValue("Q4", startQ4Date.ToString("MMM") + "'" + startQ4Date.ToString("yy") + "-" + endQ4Date.ToString("MMM") + "'" + endQ4Date.ToString("yy"));
            rd.SetParameterValue("startQ1", startQ1Date.Date); //monthYear.ToString("MMMM yyyy", ci)
            rd.SetParameterValue("endQ1", endQ1.Date);
            rd.SetParameterValue("startQ2", startQ2Date.Date);
            rd.SetParameterValue("endQ2", endQ2.Date);
            rd.SetParameterValue("startQ3", startQ3Date.Date);
            rd.SetParameterValue("endQ3", endQ3.Date);
            rd.SetParameterValue("startQ4", startQ4Date.Date);
            rd.SetParameterValue("endQ4", endQ4.Date);

            var stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            return File(stream, "application/pdf");
        }
        #endregion

        #region CustomerSalesQuarterlyComparison
        public ActionResult CustomerSalesQuarterlyComparison()
        {
            return View();
        }

        public ActionResult ReportCustomerSalesQuarterlyComparison(DateTime startQ1Date, DateTime endQ1Date, DateTime startQ2Date, DateTime endQ2Date)
        {
            DateTime endQ1 = endQ1Date.Date.AddDays(1);
            DateTime endQ2 = endQ2Date.Date.AddDays(1);
            var company = _companyService.GetQueryable().FirstOrDefault();
            //var salesInvoice = _salesInvoiceService.GetObjectById(Id);
            var q = _salesInvoiceDetailService.GetQueryable().Include(x => x.SalesInvoice).Include(x => x.DeliveryOrderDetail)
                                              .Where(x => !x.IsDeleted && (
                                                        (x.SalesInvoice.DeliveryOrder.SalesOrder.SalesDate >= startQ1Date && x.SalesInvoice.DeliveryOrder.SalesOrder.SalesDate < endQ1) ||
                                                        (x.SalesInvoice.DeliveryOrder.SalesOrder.SalesDate >= startQ2Date && x.SalesInvoice.DeliveryOrder.SalesOrder.SalesDate < endQ2)
                                                    ));
            string user = AuthenticationModel.GetUserName();

            var query = q.GroupBy(m => new
            {
                CustomerName = m.SalesInvoice.DeliveryOrder.SalesOrder.Contact.Name,
                //SalesDate = m.SalesInvoice.DeliveryOrder.SalesOrder.SalesDate,
                Currency = (m.SalesInvoice.DeliveryOrder.SalesOrder.Currency.Name == "Rupiah") ? "IDR" : m.SalesInvoice.DeliveryOrder.SalesOrder.Currency.Name,
            }).Select(g => new
            {
                CustomerName = g.Key.CustomerName, //g.FirstOrDefault().SalesInvoice.DeliveryOrder.SalesOrder.Contact.NamaFakturPajak, //g.Key.CustomerGroup,
                Currency = g.Key.Currency, //(g.FirstOrDefault().SalesInvoice.DeliveryOrder.SalesOrder.Currency.Name == "Rupiah") ? "IDR" : g.FirstOrDefault().SalesInvoice.DeliveryOrder.SalesOrder.Currency.Name,
                //SalesDate = g.Key.SalesDate,
                AmountQ1 = g.Where(x => (x.SalesInvoice.DeliveryOrder.SalesOrder.SalesDate >= startQ1Date && x.SalesInvoice.DeliveryOrder.SalesOrder.SalesDate < endQ1)).Sum(x => (Decimal?)x.SalesInvoice.AmountReceivable) ?? 0,
                AmountQ2 = g.Where(x => (x.SalesInvoice.DeliveryOrder.SalesOrder.SalesDate >= startQ2Date && x.SalesInvoice.DeliveryOrder.SalesOrder.SalesDate < endQ2)).Sum(x => (Decimal?)x.SalesInvoice.AmountReceivable) ?? 0,
            }).ToList();

            if (!query.Any())
            {
                return Content(Constant.ControllerOutput.ErrorPageRecordNotFound);
            }

            var rd = new ReportDocument();

            //Loading Report
            rd.Load(Server.MapPath("~/") + "Reports/General/CustomerSalesQuarterlyReportComparisonByValue.rpt");

            // Setting report data source
            rd.SetDataSource(query);

            // Setting subreport data source
            //rd.Subreports["subreport.rpt"].SetDataSource(q2);

            // Set parameters, need to be done after all data sources are set (to prevent reseting parameters)
            rd.SetParameterValue("CompanyName", company.Name);
            rd.SetParameterValue("AsOfDate", DateTime.Today);
            rd.SetParameterValue("Q1", startQ1Date.ToString("dd MMM yy") + " - " + endQ1Date.ToString("dd MMM yy"));
            rd.SetParameterValue("Q2", startQ2Date.ToString("dd MMM yy") + " - " + endQ2Date.ToString("dd MMM yy"));
            rd.SetParameterValue("startQ1", startQ1Date.Date); //monthYear.ToString("MMMM yyyy", ci)
            rd.SetParameterValue("endQ1", endQ1.Date);
            rd.SetParameterValue("startQ2", startQ2Date.Date);
            rd.SetParameterValue("endQ2", endQ2.Date);

            var stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            return File(stream, "application/pdf");
        }
        #endregion

        #region CustomerSalesYearly
        public ActionResult CustomerSalesYearly()
        {
            return View();
        }

        public ActionResult ReportCustomerSalesYearly(int Y2)
        {
            int Y1 = Y2 - 1;
            var company = _companyService.GetQueryable().FirstOrDefault();
            //var salesInvoice = _salesInvoiceService.GetObjectById(Id);
            var q = _salesInvoiceDetailService.GetQueryable().Include(x => x.SalesInvoice).Include(x => x.DeliveryOrderDetail)
                                              .Where(x => !x.IsDeleted && (
                                                        (x.SalesInvoice.DeliveryOrder.SalesOrder.SalesDate.Year >= Y1 && x.SalesInvoice.DeliveryOrder.SalesOrder.SalesDate.Year <= Y2)
                                                    ));
            string user = AuthenticationModel.GetUserName();

            var query = q.GroupBy(m => new
            {
                CustomerName = m.SalesInvoice.DeliveryOrder.SalesOrder.Contact.Name,
                ItemType = m.DeliveryOrderDetail.SalesOrderDetail.Item.ItemType.Name,
                UoM = m.DeliveryOrderDetail.SalesOrderDetail.Item.UoM.Name,
                //SalesDate = m.SalesInvoice.DeliveryOrder.SalesOrder.SalesDate,
            }).Select(g => new
            {
                CustomerName = g.Key.CustomerName, //g.FirstOrDefault().SalesInvoice.DeliveryOrder.SalesOrder.Contact.NamaFakturPajak, //g.Key.CustomerGroup,
                ItemType = g.Key.ItemType,
                UoM = g.Key.UoM,
                //SalesDate = g.Key.SalesDate,
                AmountY1 = g.Where(x => (x.SalesInvoice.DeliveryOrder.SalesOrder.SalesDate.Year == Y1)).Sum(x => (Decimal?)x.DeliveryOrderDetail.SalesOrderDetail.Quantity) ?? 0,
                AmountY2 = g.Where(x => (x.SalesInvoice.DeliveryOrder.SalesOrder.SalesDate.Year == Y2)).Sum(x => (Decimal?)x.DeliveryOrderDetail.SalesOrderDetail.Quantity) ?? 0,
            }).OrderBy(x => x.CustomerName).ThenBy(x => x.ItemType).ToList();

            if (!query.Any())
            {
                return Content(Constant.ControllerOutput.ErrorPageRecordNotFound);
            }

            var rd = new ReportDocument();

            //Loading Report
            rd.Load(Server.MapPath("~/") + "Reports/General/CustomerSalesYearlyReportByQuantity.rpt");

            // Setting report data source
            rd.SetDataSource(query);

            // Setting subreport data source
            //rd.Subreports["subreport.rpt"].SetDataSource(q2);

            // Set parameters, need to be done after all data sources are set (to prevent reseting parameters)
            rd.SetParameterValue("CompanyName", company.Name);
            rd.SetParameterValue("AsOfDate", DateTime.Today);
            rd.SetParameterValue("Y1", "Year " + Y1.ToString());
            rd.SetParameterValue("Y2", "Year " + Y2.ToString());

            var stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            return File(stream, "application/pdf");
        }
        #endregion

        #region CustomerSalesByItem
        public ActionResult CustomerSalesByItem()
        {
            return View();
        }

        public ActionResult ReportCustomerSalesByItem(DateTime startDate, DateTime endDate, int ContactId = 0)
        {
            DateTime endDay = endDate.AddDays(1);
            var company = _companyService.GetQueryable().FirstOrDefault();
            //var salesInvoice = _salesInvoiceService.GetObjectById(Id);
            var q = _salesInvoiceDetailService.GetQueryable().Include(x => x.SalesInvoice).Include(x => x.DeliveryOrderDetail)
                                              .Where(x => !x.IsDeleted && x.SalesInvoice.DeliveryOrder.SalesOrder.ContactId == ContactId && (
                                                        (x.SalesInvoice.DeliveryOrder.SalesOrder.SalesDate >= startDate && x.SalesInvoice.DeliveryOrder.SalesOrder.SalesDate < endDay)
                                                    ));
            string user = AuthenticationModel.GetUserName();

            var query = q.GroupBy(m => new
            {
                CustomerName = m.SalesInvoice.DeliveryOrder.SalesOrder.Contact.Name,
                Currency = (m.SalesInvoice.DeliveryOrder.SalesOrder.Currency.Name == "Rupiah") ? "IDR" : m.SalesInvoice.DeliveryOrder.SalesOrder.Currency.Name,
                ItemName = m.DeliveryOrderDetail.SalesOrderDetail.Item.Name,
                UoM = m.DeliveryOrderDetail.SalesOrderDetail.Item.UoM.Name,
                SalesDate = EntityFunctions.TruncateTime(m.SalesInvoice.DeliveryOrder.SalesOrder.SalesDate)??DateTime.MinValue,
                Price = m.DeliveryOrderDetail.SalesOrderDetail.Price,
            }).Select(g => new
            {
                CustomerName = g.Key.CustomerName, //g.FirstOrDefault().SalesInvoice.DeliveryOrder.SalesOrder.Contact.NamaFakturPajak, //g.Key.CustomerGroup,
                Currency = g.Key.Currency,
                ItemName = g.Key.ItemName,
                UoM = g.Key.UoM,
                SalesDate = g.Key.SalesDate,
                Price = g.Key.Price,
                Quantity = g.Where(x => (x.SalesInvoice.DeliveryOrder.SalesOrder.SalesDate == g.Key.SalesDate)).Sum(x => (Decimal?)x.DeliveryOrderDetail.SalesOrderDetail.Quantity) ?? 0,
                Discount = g.Where(x => (x.SalesInvoice.DeliveryOrder.SalesOrder.SalesDate == g.Key.SalesDate)).Sum(x => (Decimal?)x.DeliveryOrderDetail.SalesOrderDetail.Item.PriceMutations.Where(y => (y.DeactivatedAt == null || g.Key.SalesDate < y.DeactivatedAt.Value)).OrderByDescending(y => y.DeactivatedAt.Value).FirstOrDefault().Amount) ?? 0, //.Sum(x => (Decimal?)(x.SalesInvoice.Discount * g.Key.Price)/100.0m) ?? 0,
            }).OrderBy(x => x.ItemName).ToList();

            if (!query.Any())
            {
                return Content(Constant.ControllerOutput.ErrorPageRecordNotFound);
            }

            var rd = new ReportDocument();

            //Loading Report
            rd.Load(Server.MapPath("~/") + "Reports/General/CustomerSalesReportByItem.rpt");

            // Setting report data source
            rd.SetDataSource(query);

            // Setting subreport data source
            //rd.Subreports["subreport.rpt"].SetDataSource(q2);

            // Set parameters, need to be done after all data sources are set (to prevent reseting parameters)
            rd.SetParameterValue("CompanyName", company.Name);
            rd.SetParameterValue("AsOfDate", DateTime.Today);
            rd.SetParameterValue("startDate", startDate);
            rd.SetParameterValue("endDate", endDay);

            var stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            return File(stream, "application/pdf");
        }
        #endregion

        #region CustomerSalesComparedLastYear
        public ActionResult CustomerSalesComparedLastYear()
        {
            return View();
        }

        public ActionResult ReportCustomerSalesComparedLastYear(int Y2, int M1, int M2, int M3, int M4)
        {
            int Y1 = Y2-1;
            //int MinM = Math.Min(Math.Min(Math.Min(M1, M2), M3), M4);
            //int MaxM = Math.Max(Math.Max(Math.Max(M1, M2), M3), M4);
            var company = _companyService.GetQueryable().FirstOrDefault();
            //var salesInvoice = _salesInvoiceService.GetObjectById(Id);
            var q = _salesInvoiceDetailService.GetQueryable().Include(x => x.SalesInvoice).Include(x => x.DeliveryOrderDetail)
                                              .Where(x => !x.IsDeleted && (
                                                        (x.SalesInvoice.DeliveryOrder.SalesOrder.SalesDate.Year <= Y2 && x.SalesInvoice.DeliveryOrder.SalesOrder.SalesDate.Year >= Y1) && (
                                                        (x.SalesInvoice.DeliveryOrder.SalesOrder.SalesDate.Month == M1) ||
                                                        (x.SalesInvoice.DeliveryOrder.SalesOrder.SalesDate.Month == M2) ||
                                                        (x.SalesInvoice.DeliveryOrder.SalesOrder.SalesDate.Month == M3) ||
                                                        (x.SalesInvoice.DeliveryOrder.SalesOrder.SalesDate.Month == M4)
                                                    )));
            string user = AuthenticationModel.GetUserName();

            var query = q.GroupBy(m => new
            {
                CustomerName = m.SalesInvoice.DeliveryOrder.SalesOrder.Contact.Name,
                //SalesDate = m.SalesInvoice.DeliveryOrder.SalesOrder.SalesDate,
                Currency = (m.SalesInvoice.DeliveryOrder.SalesOrder.Currency.Name == "Rupiah") ? "IDR" : m.SalesInvoice.DeliveryOrder.SalesOrder.Currency.Name,
            }).Select(g => new
            {
                CustomerName = g.Key.CustomerName, //g.FirstOrDefault().SalesInvoice.DeliveryOrder.SalesOrder.Contact.NamaFakturPajak, //g.Key.CustomerGroup,
                Currency = g.Key.Currency, //(g.FirstOrDefault().SalesInvoice.DeliveryOrder.SalesOrder.Currency.Name == "Rupiah") ? "IDR" : g.FirstOrDefault().SalesInvoice.DeliveryOrder.SalesOrder.Currency.Name,
                //SalesDate = g.Key.SalesDate,
                AmountM1Y1 = g.Where(x => (x.SalesInvoice.DeliveryOrder.SalesOrder.SalesDate.Year == Y1 && x.SalesInvoice.DeliveryOrder.SalesOrder.SalesDate.Month == M1)).Sum(x => (Decimal?)x.SalesInvoice.AmountReceivable) ?? 0,
                AmountM2Y1 = g.Where(x => (x.SalesInvoice.DeliveryOrder.SalesOrder.SalesDate.Year == Y1 && x.SalesInvoice.DeliveryOrder.SalesOrder.SalesDate.Month == M2)).Sum(x => (Decimal?)x.SalesInvoice.AmountReceivable) ?? 0,
                AmountM3Y1 = g.Where(x => (x.SalesInvoice.DeliveryOrder.SalesOrder.SalesDate.Year == Y1 && x.SalesInvoice.DeliveryOrder.SalesOrder.SalesDate.Month == M3)).Sum(x => (Decimal?)x.SalesInvoice.AmountReceivable) ?? 0,
                AmountM4Y1 = g.Where(x => (x.SalesInvoice.DeliveryOrder.SalesOrder.SalesDate.Year == Y1 && x.SalesInvoice.DeliveryOrder.SalesOrder.SalesDate.Month == M4)).Sum(x => (Decimal?)x.SalesInvoice.AmountReceivable) ?? 0,
                AmountM1Y2 = g.Where(x => (x.SalesInvoice.DeliveryOrder.SalesOrder.SalesDate.Year == Y2 && x.SalesInvoice.DeliveryOrder.SalesOrder.SalesDate.Month == M1)).Sum(x => (Decimal?)x.SalesInvoice.AmountReceivable) ?? 0,
                AmountM2Y2 = g.Where(x => (x.SalesInvoice.DeliveryOrder.SalesOrder.SalesDate.Year == Y2 && x.SalesInvoice.DeliveryOrder.SalesOrder.SalesDate.Month == M2)).Sum(x => (Decimal?)x.SalesInvoice.AmountReceivable) ?? 0,
                AmountM3Y2 = g.Where(x => (x.SalesInvoice.DeliveryOrder.SalesOrder.SalesDate.Year == Y2 && x.SalesInvoice.DeliveryOrder.SalesOrder.SalesDate.Month == M3)).Sum(x => (Decimal?)x.SalesInvoice.AmountReceivable) ?? 0,
                AmountM4Y2 = g.Where(x => (x.SalesInvoice.DeliveryOrder.SalesOrder.SalesDate.Year == Y2 && x.SalesInvoice.DeliveryOrder.SalesOrder.SalesDate.Month == M4)).Sum(x => (Decimal?)x.SalesInvoice.AmountReceivable) ?? 0,
            }).ToList();

            if (!query.Any())
            {
                return Content(Constant.ControllerOutput.ErrorPageRecordNotFound);
            }

            var rd = new ReportDocument();

            //Loading Report
            rd.Load(Server.MapPath("~/") + "Reports/General/CustomerSalesReportComparedLastYearByValue.rpt");

            // Setting report data source
            rd.SetDataSource(query);

            // Setting subreport data source
            //rd.Subreports["subreport.rpt"].SetDataSource(q2);

            // Set parameters, need to be done after all data sources are set (to prevent reseting parameters)
            rd.SetParameterValue("CompanyName", company.Name);
            rd.SetParameterValue("AsOfDate", DateTime.Today);
            rd.SetParameterValue("M1Y1", new DateTime(Y1, M1, 1).ToString("MMM-yy")); //monthYear.ToString("MMMM yyyy", ci)
            rd.SetParameterValue("M1Y2", new DateTime(Y2, M1, 1).ToString("MMM yyyy"));
            rd.SetParameterValue("M2Y1", new DateTime(Y1, M2, 1).ToString("MMM-yy"));
            rd.SetParameterValue("M2Y2", new DateTime(Y2, M2, 1).ToString("MMM yyyy"));
            rd.SetParameterValue("M3Y1", new DateTime(Y1, M3, 1).ToString("MMM-yy"));
            rd.SetParameterValue("M3Y2", new DateTime(Y2, M3, 1).ToString("MMM yyyy"));
            rd.SetParameterValue("M4Y1", new DateTime(Y1, M4, 1).ToString("MMM-yy"));
            rd.SetParameterValue("M4Y2", new DateTime(Y2, M4, 1).ToString("MMM yyyy"));
            rd.SetParameterValue("Y1", Y1);
            rd.SetParameterValue("Y2", Y2);
            rd.SetParameterValue("M1", M1);
            rd.SetParameterValue("M2", M2);
            rd.SetParameterValue("M3", M3);
            rd.SetParameterValue("M4", M4);

            var stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            return File(stream, "application/pdf");
        }
        #endregion

        #region CustomerGroupSalesLast6Year
        public ActionResult CustomerGroupSalesLast6Year()
        {
            return View();
        }

        public ActionResult ReportCustomerGroupSalesLast6Year(int Y6)
        {
            int Y5 = Y6 - 1;
            int Y4 = Y5 - 1;
            int Y3 = Y4 - 1;
            int Y2 = Y3 - 1;
            int Y1 = Y2 - 1;
            var company = _companyService.GetQueryable().FirstOrDefault();
            //var salesInvoice = _salesInvoiceService.GetObjectById(Id);
            var q = _salesInvoiceDetailService.GetQueryable().Include(x => x.SalesInvoice).Include(x => x.DeliveryOrderDetail)
                                              .Where(x => !x.IsDeleted && (
                                                        (x.SalesInvoice.DeliveryOrder.SalesOrder.SalesDate.Year >= Y1 && x.SalesInvoice.DeliveryOrder.SalesOrder.SalesDate.Year <= Y6)
                                                    ));
            string user = AuthenticationModel.GetUserName();

            var query = q.GroupBy(m => new
            {
                CustomerGroup = m.SalesInvoice.DeliveryOrder.SalesOrder.Contact.ContactGroup.Name, //.NamaFakturPajak,
                //SalesDate = m.SalesInvoice.DeliveryOrder.SalesOrder.SalesDate,
                Currency = (m.SalesInvoice.DeliveryOrder.SalesOrder.Currency.Name == "Rupiah") ? "IDR" : m.SalesInvoice.DeliveryOrder.SalesOrder.Currency.Name,
            }).Select(g => new
            {
                CustomerGroup = g.Key.CustomerGroup, //g.FirstOrDefault().SalesInvoice.DeliveryOrder.SalesOrder.Contact.NamaFakturPajak, //g.Key.CustomerGroup,
                Currency = g.Key.Currency, //(g.FirstOrDefault().SalesInvoice.DeliveryOrder.SalesOrder.Currency.Name == "Rupiah") ? "IDR" : g.FirstOrDefault().SalesInvoice.DeliveryOrder.SalesOrder.Currency.Name,
                //SalesDate = g.Key.SalesDate,
                AmountY1 = g.Where(x => (x.SalesInvoice.DeliveryOrder.SalesOrder.SalesDate.Year == Y1)).Sum(x => (Decimal?)x.SalesInvoice.AmountReceivable) ?? 0,
                AmountY2 = g.Where(x => (x.SalesInvoice.DeliveryOrder.SalesOrder.SalesDate.Year == Y2)).Sum(x => (Decimal?)x.SalesInvoice.AmountReceivable) ?? 0,
                AmountY3 = g.Where(x => (x.SalesInvoice.DeliveryOrder.SalesOrder.SalesDate.Year == Y3)).Sum(x => (Decimal?)x.SalesInvoice.AmountReceivable) ?? 0,
                AmountY4 = g.Where(x => (x.SalesInvoice.DeliveryOrder.SalesOrder.SalesDate.Year == Y4)).Sum(x => (Decimal?)x.SalesInvoice.AmountReceivable) ?? 0,
                AmountY5 = g.Where(x => (x.SalesInvoice.DeliveryOrder.SalesOrder.SalesDate.Year == Y5)).Sum(x => (Decimal?)x.SalesInvoice.AmountReceivable) ?? 0,
                AmountY6 = g.Where(x => (x.SalesInvoice.DeliveryOrder.SalesOrder.SalesDate.Year == Y6)).Sum(x => (Decimal?)x.SalesInvoice.AmountReceivable) ?? 0,
            }).ToList();

            if (!query.Any())
            {
                return Content(Constant.ControllerOutput.ErrorPageRecordNotFound);
            }

            var rd = new ReportDocument();

            //Loading Report
            rd.Load(Server.MapPath("~/") + "Reports/General/CustomerGroupSalesReportLast6YearByValue.rpt");

            // Setting report data source
            rd.SetDataSource(query);

            // Setting subreport data source
            //rd.Subreports["subreport.rpt"].SetDataSource(q2);

            // Set parameters, need to be done after all data sources are set (to prevent reseting parameters)
            rd.SetParameterValue("CompanyName", company.Name);
            rd.SetParameterValue("AsOfDate", DateTime.Today);
            rd.SetParameterValue("Y1", "Year " + Y1.ToString());
            rd.SetParameterValue("Y2", "Year " + Y2.ToString());
            rd.SetParameterValue("Y3", "Year " + Y3.ToString());
            rd.SetParameterValue("Y4", "Year " + Y4.ToString());
            rd.SetParameterValue("Y5", "Year " + Y5.ToString());
            rd.SetParameterValue("Y6", "Year " + Y6.ToString());

            var stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            return File(stream, "application/pdf");
        }
        #endregion

        #region CustomerSalesLast6Year
        public ActionResult CustomerSalesLast6Year()
        {
            return View();
        }

        public ActionResult ReportCustomerSalesLast6Year(int Y6, string contactIDs)
        {
            int Y5 = Y6 - 1;
            int Y4 = Y5 - 1;
            int Y3 = Y4 - 1;
            int Y2 = Y3 - 1;
            int Y1 = Y2 - 1;
            var clist = JsonConvert.DeserializeObject<List<int>>(contactIDs);
            var company = _companyService.GetQueryable().FirstOrDefault();
            //var salesInvoice = _salesInvoiceService.GetObjectById(Id);
            var q = _salesInvoiceDetailService.GetQueryable().Include(x => x.SalesInvoice).Include(x => x.DeliveryOrderDetail)
                                              .Where(x => !x.IsDeleted && (
                                                        (x.SalesInvoice.DeliveryOrder.SalesOrder.SalesDate.Year >= Y1 && x.SalesInvoice.DeliveryOrder.SalesOrder.SalesDate.Year <= Y6)
                                                    ));
            string user = AuthenticationModel.GetUserName();

            var query = q.GroupBy(m => new
            {
                ContactId = m.SalesInvoice.DeliveryOrder.SalesOrder.ContactId,
                CustomerName = m.SalesInvoice.DeliveryOrder.SalesOrder.Contact.Name,
                //SalesDate = m.SalesInvoice.DeliveryOrder.SalesOrder.SalesDate,
                Currency = (m.SalesInvoice.DeliveryOrder.SalesOrder.Currency.Name == "Rupiah") ? "IDR" : m.SalesInvoice.DeliveryOrder.SalesOrder.Currency.Name,
            }).Select(g => new
            {
                ContactId = g.Key.ContactId,
                CustomerName = g.Key.CustomerName, //g.FirstOrDefault().SalesInvoice.DeliveryOrder.SalesOrder.Contact.NamaFakturPajak, //g.Key.CustomerGroup,
                Currency = g.Key.Currency, //(g.FirstOrDefault().SalesInvoice.DeliveryOrder.SalesOrder.Currency.Name == "Rupiah") ? "IDR" : g.FirstOrDefault().SalesInvoice.DeliveryOrder.SalesOrder.Currency.Name,
                //SalesDate = g.Key.SalesDate,
                AmountY1 = g.Where(x => (x.SalesInvoice.DeliveryOrder.SalesOrder.SalesDate.Year == Y1)).Sum(x => (Decimal?)x.SalesInvoice.AmountReceivable) ?? 0,
                AmountY2 = g.Where(x => (x.SalesInvoice.DeliveryOrder.SalesOrder.SalesDate.Year == Y2)).Sum(x => (Decimal?)x.SalesInvoice.AmountReceivable) ?? 0,
                AmountY3 = g.Where(x => (x.SalesInvoice.DeliveryOrder.SalesOrder.SalesDate.Year == Y3)).Sum(x => (Decimal?)x.SalesInvoice.AmountReceivable) ?? 0,
                AmountY4 = g.Where(x => (x.SalesInvoice.DeliveryOrder.SalesOrder.SalesDate.Year == Y4)).Sum(x => (Decimal?)x.SalesInvoice.AmountReceivable) ?? 0,
                AmountY5 = g.Where(x => (x.SalesInvoice.DeliveryOrder.SalesOrder.SalesDate.Year == Y5)).Sum(x => (Decimal?)x.SalesInvoice.AmountReceivable) ?? 0,
                AmountY6 = g.Where(x => (x.SalesInvoice.DeliveryOrder.SalesOrder.SalesDate.Year == Y6)).Sum(x => (Decimal?)x.SalesInvoice.AmountReceivable) ?? 0,
            }).AsEnumerable().Where(x => clist.Contains(x.ContactId)).OrderBy(x => x.CustomerName).ToList();

            if (!query.Any())
            {
                return Content(Constant.ControllerOutput.ErrorPageRecordNotFound);
            }

            var rd = new ReportDocument();

            //Loading Report
            rd.Load(Server.MapPath("~/") + "Reports/General/CustomerSalesReportLast6YearByValue.rpt");

            // Setting report data source
            rd.SetDataSource(query);

            // Setting subreport data source
            //rd.Subreports["subreport.rpt"].SetDataSource(q2);

            // Set parameters, need to be done after all data sources are set (to prevent reseting parameters)
            rd.SetParameterValue("CompanyName", company.Name);
            rd.SetParameterValue("AsOfDate", DateTime.Today);
            rd.SetParameterValue("Y1", "Year " + Y1.ToString());
            rd.SetParameterValue("Y2", "Year " + Y2.ToString());
            rd.SetParameterValue("Y3", "Year " + Y3.ToString());
            rd.SetParameterValue("Y4", "Year " + Y4.ToString());
            rd.SetParameterValue("Y5", "Year " + Y5.ToString());
            rd.SetParameterValue("Y6", "Year " + Y6.ToString());

            var stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            return File(stream, "application/pdf");
        }
        #endregion

        #region SortedCustomerSalesLast6Year
        public ActionResult SortedCustomerSalesLast6Year()
        {
            return View();
        }

        public ActionResult ReportSortedCustomerSalesLast6Year(int Y6)
        {
            int Y5 = Y6 - 1;
            int Y4 = Y5 - 1;
            int Y3 = Y4 - 1;
            int Y2 = Y3 - 1;
            int Y1 = Y2 - 1;
            //var clist = JsonConvert.DeserializeObject<List<int>>(contactIDs);
            var company = _companyService.GetQueryable().FirstOrDefault();
            //var salesInvoice = _salesInvoiceService.GetObjectById(Id);
            var q = _salesInvoiceDetailService.GetQueryable().Include(x => x.SalesInvoice).Include(x => x.DeliveryOrderDetail)
                                              .Where(x => !x.IsDeleted && (
                                                        (x.SalesInvoice.DeliveryOrder.SalesOrder.SalesDate.Year >= Y1 && x.SalesInvoice.DeliveryOrder.SalesOrder.SalesDate.Year <= Y6)
                                                    ));
            string user = AuthenticationModel.GetUserName();

            var query = q.GroupBy(m => new
            {
                ContactId = m.SalesInvoice.DeliveryOrder.SalesOrder.ContactId,
                CustomerName = m.SalesInvoice.DeliveryOrder.SalesOrder.Contact.Name,
                //SalesDate = m.SalesInvoice.DeliveryOrder.SalesOrder.SalesDate,
                Currency = (m.SalesInvoice.DeliveryOrder.SalesOrder.Currency.Name == "Rupiah") ? "IDR" : m.SalesInvoice.DeliveryOrder.SalesOrder.Currency.Name,
            }).Select(g => new
            {
                ContactId = g.Key.ContactId,
                CustomerName = g.Key.CustomerName, //g.FirstOrDefault().SalesInvoice.DeliveryOrder.SalesOrder.Contact.NamaFakturPajak, //g.Key.CustomerGroup,
                Currency = g.Key.Currency, //(g.FirstOrDefault().SalesInvoice.DeliveryOrder.SalesOrder.Currency.Name == "Rupiah") ? "IDR" : g.FirstOrDefault().SalesInvoice.DeliveryOrder.SalesOrder.Currency.Name,
                //SalesDate = g.Key.SalesDate,
                AmountY1 = g.Where(x => (x.SalesInvoice.DeliveryOrder.SalesOrder.SalesDate.Year == Y1)).Sum(x => (Decimal?)x.SalesInvoice.AmountReceivable) ?? 0,
                AmountY2 = g.Where(x => (x.SalesInvoice.DeliveryOrder.SalesOrder.SalesDate.Year == Y2)).Sum(x => (Decimal?)x.SalesInvoice.AmountReceivable) ?? 0,
                AmountY3 = g.Where(x => (x.SalesInvoice.DeliveryOrder.SalesOrder.SalesDate.Year == Y3)).Sum(x => (Decimal?)x.SalesInvoice.AmountReceivable) ?? 0,
                AmountY4 = g.Where(x => (x.SalesInvoice.DeliveryOrder.SalesOrder.SalesDate.Year == Y4)).Sum(x => (Decimal?)x.SalesInvoice.AmountReceivable) ?? 0,
                AmountY5 = g.Where(x => (x.SalesInvoice.DeliveryOrder.SalesOrder.SalesDate.Year == Y5)).Sum(x => (Decimal?)x.SalesInvoice.AmountReceivable) ?? 0,
                AmountY6 = g.Where(x => (x.SalesInvoice.DeliveryOrder.SalesOrder.SalesDate.Year == Y6)).Sum(x => (Decimal?)x.SalesInvoice.AmountReceivable) ?? 0,
            }).AsEnumerable().OrderByDescending(x => (x.AmountY1 + x.AmountY2 + x.AmountY3 + x.AmountY4 + x.AmountY5 + x.AmountY6)).ThenByDescending(x => x.AmountY6).ToList(); //.Where(x => clist.Contains(x.ContactId)).ToList();

            if (!query.Any())
            {
                return Content(Constant.ControllerOutput.ErrorPageRecordNotFound);
            }

            var rd = new ReportDocument();

            //Loading Report
            rd.Load(Server.MapPath("~/") + "Reports/General/SortedCustomerSalesReportLast6YearByValue.rpt");

            // Setting report data source
            rd.SetDataSource(query);

            // Setting subreport data source
            //rd.Subreports["subreport.rpt"].SetDataSource(q2);

            // Set parameters, need to be done after all data sources are set (to prevent reseting parameters)
            rd.SetParameterValue("CompanyName", company.Name);
            rd.SetParameterValue("AsOfDate", DateTime.Today);
            rd.SetParameterValue("Y1", "Year " + Y1.ToString());
            rd.SetParameterValue("Y2", "Year " + Y2.ToString());
            rd.SetParameterValue("Y3", "Year " + Y3.ToString());
            rd.SetParameterValue("Y4", "Year " + Y4.ToString());
            rd.SetParameterValue("Y5", "Year " + Y5.ToString());
            rd.SetParameterValue("Y6", "Year " + Y6.ToString());

            var stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            return File(stream, "application/pdf");
        }
        #endregion

        #region ItemSalesYearlyByValue
        public ActionResult ItemSalesYearlyByValue()
        {
            return View();
        }

        public ActionResult ReportItemSalesYearlyByValue(int Y3, int tipe = 0)
        {
            int Y2 = Y3 - 1;
            int Y1 = Y2 - 1;
            var company = _companyService.GetQueryable().FirstOrDefault();
            //var salesInvoice = _salesInvoiceService.GetObjectById(Id);
            var q = _salesInvoiceDetailService.GetQueryable().Include(x => x.SalesInvoice).Include(x => x.DeliveryOrderDetail)
                                              .Where(x => !x.IsDeleted && (
                                                        (x.SalesInvoice.DeliveryOrder.SalesOrder.SalesDate.Year >= Y1 && x.SalesInvoice.DeliveryOrder.SalesOrder.SalesDate.Year <= Y3)
                                                    ));
            string user = AuthenticationModel.GetUserName();

            var query = q.GroupBy(m => new
            {
                CustomerName = m.SalesInvoice.DeliveryOrder.SalesOrder.Contact.Name,
                Currency = (m.SalesInvoice.DeliveryOrder.SalesOrder.Currency.Name == "Rupiah") ? "IDR" : m.SalesInvoice.DeliveryOrder.SalesOrder.Currency.Name,
                ItemName = m.DeliveryOrderDetail.SalesOrderDetail.Item.Name,
                ItemType = m.DeliveryOrderDetail.SalesOrderDetail.Item.ItemType.Name,
                SKU = m.DeliveryOrderDetail.SalesOrderDetail.Item.Sku,
                UoM = m.DeliveryOrderDetail.SalesOrderDetail.Item.UoM.Name,
                //SalesDate = EntityFunctions.TruncateTime(m.SalesInvoice.DeliveryOrder.SalesOrder.SalesDate) ?? DateTime.MinValue,
            }).Select(g => new
            {
                CustomerName = g.Key.CustomerName, //g.FirstOrDefault().SalesInvoice.DeliveryOrder.SalesOrder.Contact.NamaFakturPajak, //g.Key.CustomerGroup,
                Currency = g.Key.Currency,
                ItemName = g.Key.ItemName,
                ItemType = g.Key.ItemType,
                SKU = g.Key.SKU,
                UoM = g.Key.UoM,
                //SalesDate = g.Key.SalesDate,
                AmountY1 = (tipe == 0) ? g.Where(x => (x.SalesInvoice.DeliveryOrder.SalesOrder.SalesDate.Year == Y1)).Sum(x => (Decimal?)x.SalesInvoice.AmountReceivable) ?? 0 : g.Where(x => (x.SalesInvoice.DeliveryOrder.SalesOrder.SalesDate.Year == Y1)).Sum(x => (Decimal?)x.DeliveryOrderDetail.SalesOrderDetail.Quantity) ?? 0,
                AmountY2 = (tipe == 0) ? g.Where(x => (x.SalesInvoice.DeliveryOrder.SalesOrder.SalesDate.Year == Y2)).Sum(x => (Decimal?)x.SalesInvoice.AmountReceivable) ?? 0 : g.Where(x => (x.SalesInvoice.DeliveryOrder.SalesOrder.SalesDate.Year == Y2)).Sum(x => (Decimal?)x.DeliveryOrderDetail.SalesOrderDetail.Quantity) ?? 0,
                AmountY3 = (tipe == 0) ? g.Where(x => (x.SalesInvoice.DeliveryOrder.SalesOrder.SalesDate.Year == Y3)).Sum(x => (Decimal?)x.SalesInvoice.AmountReceivable) ?? 0 : g.Where(x => (x.SalesInvoice.DeliveryOrder.SalesOrder.SalesDate.Year == Y3)).Sum(x => (Decimal?)x.DeliveryOrderDetail.SalesOrderDetail.Quantity) ?? 0,
            }).AsEnumerable();

            if (!query.Any())
            {
                return Content(Constant.ControllerOutput.ErrorPageRecordNotFound);
            }

            var rd = new ReportDocument();

            //Loading Report
            if (tipe == 0)
            {
                query = query.OrderBy(x => x.SKU).ToList();
                rd.Load(Server.MapPath("~/") + "Reports/General/ItemSalesYearlyReportByValueBySKU.rpt");
            }
            else
            {
                query = query.OrderBy(x => x.ItemType).ToList();
                rd.Load(Server.MapPath("~/") + "Reports/General/ItemSalesYearlyReportByValueByType.rpt");
            }

            // Setting report data source
            rd.SetDataSource(query);

            // Setting subreport data source
            //rd.Subreports["subreport.rpt"].SetDataSource(q2);

            // Set parameters, need to be done after all data sources are set (to prevent reseting parameters)
            rd.SetParameterValue("CompanyName", company.Name);
            rd.SetParameterValue("AsOfDate", DateTime.Today);
            rd.SetParameterValue("Y1", Y1.ToString());
            rd.SetParameterValue("Y2", Y2.ToString());
            rd.SetParameterValue("Y3", Y3.ToString());

            var stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            return File(stream, "application/pdf");
        }
        #endregion

        #region ItemSalesYearlyByQuantity
        public ActionResult ItemSalesYearlyByQuantity()
        {
            return View();
        }

        public ActionResult ReportItemSalesYearlyByQuantity(int Y3)
        {
            int Y2 = Y3 - 1;
            int Y1 = Y2 - 1;
            var company = _companyService.GetQueryable().FirstOrDefault();
            //var salesInvoice = _salesInvoiceService.GetObjectById(Id);
            var q = _salesInvoiceDetailService.GetQueryable().Include(x => x.SalesInvoice).Include(x => x.DeliveryOrderDetail)
                                              .Where(x => !x.IsDeleted && (
                                                        (x.SalesInvoice.DeliveryOrder.SalesOrder.SalesDate.Year >= Y1 && x.SalesInvoice.DeliveryOrder.SalesOrder.SalesDate.Year <= Y3)
                                                    ));
            string user = AuthenticationModel.GetUserName();

            var query = q.GroupBy(m => new
            {
                //CustomerName = m.SalesInvoice.DeliveryOrder.SalesOrder.Contact.Name,
                //Currency = (m.SalesInvoice.DeliveryOrder.SalesOrder.Currency.Name == "Rupiah") ? "IDR" : m.SalesInvoice.DeliveryOrder.SalesOrder.Currency.Name,
                //ItemName = m.DeliveryOrderDetail.SalesOrderDetail.Item.Name,
                ItemType = m.DeliveryOrderDetail.SalesOrderDetail.Item.ItemType.Name,
                //SKU = m.DeliveryOrderDetail.SalesOrderDetail.Item.Sku,
                UoM = m.DeliveryOrderDetail.SalesOrderDetail.Item.UoM.Name,
                //SalesDate = EntityFunctions.TruncateTime(m.SalesInvoice.DeliveryOrder.SalesOrder.SalesDate) ?? DateTime.MinValue,
            }).Select(g => new
            {
                //CustomerName = g.Key.CustomerName, //g.FirstOrDefault().SalesInvoice.DeliveryOrder.SalesOrder.Contact.NamaFakturPajak, //g.Key.CustomerGroup,
                //Currency = g.Key.Currency,
                //ItemName = g.Key.ItemName,
                ItemType = g.Key.ItemType,
                //SKU = g.Key.SKU,
                UoM = g.Key.UoM,
                //SalesDate = g.Key.SalesDate,
                AmountY1 = g.Where(x => (x.SalesInvoice.DeliveryOrder.SalesOrder.SalesDate.Year == Y1)).Sum(x => (Decimal?)x.DeliveryOrderDetail.SalesOrderDetail.Quantity) ?? 0,
                AmountY2 = g.Where(x => (x.SalesInvoice.DeliveryOrder.SalesOrder.SalesDate.Year == Y2)).Sum(x => (Decimal?)x.DeliveryOrderDetail.SalesOrderDetail.Quantity) ?? 0,
                AmountY3 = g.Where(x => (x.SalesInvoice.DeliveryOrder.SalesOrder.SalesDate.Year == Y3)).Sum(x => (Decimal?)x.DeliveryOrderDetail.SalesOrderDetail.Quantity) ?? 0,
            }).AsEnumerable();

            if (!query.Any())
            {
                return Content(Constant.ControllerOutput.ErrorPageRecordNotFound);
            }

            var rd = new ReportDocument();

            //Loading Report
            query = query.OrderBy(x => x.ItemType).ToList();
            rd.Load(Server.MapPath("~/") + "Reports/General/ItemSalesYearlyReportByQuantity.rpt");

            // Setting report data source
            rd.SetDataSource(query);

            // Setting subreport data source
            //rd.Subreports["subreport.rpt"].SetDataSource(q2);

            // Set parameters, need to be done after all data sources are set (to prevent reseting parameters)
            rd.SetParameterValue("CompanyName", company.Name);
            rd.SetParameterValue("AsOfDate", DateTime.Today);
            rd.SetParameterValue("Y1", Y1.ToString());
            rd.SetParameterValue("Y2", Y2.ToString());
            rd.SetParameterValue("Y3", Y3.ToString());

            var stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            return File(stream, "application/pdf");
        }
        #endregion

        #region SalesOrderDailyByDate
        public ActionResult SalesOrderDailyByDate()
        {
            return View();
        }

        public ActionResult ReportSalesOrderDailyByDate(DateTime startDate, DateTime endDate)
        {
            using (var db = new OffsetPrintingSuppliesEntities())
            {
                DateTime endDay = endDate.AddDays(1);
                var company = _companyService.GetQueryable().FirstOrDefault();
                //var salesInvoice = _salesInvoiceService.GetObjectById(Id);
                var q = db.SalesOrderDetails.Include(x => x.SalesOrder)
                                                  .Where(x => !x.IsDeleted && (
                                                            (x.SalesOrder.SalesDate >= startDate && x.SalesOrder.SalesDate < endDay)
                                                        ));
                string user = AuthenticationModel.GetUserName();

                var query = q.GroupBy(m => new
                {
                    CustomerName = m.SalesOrder.Contact.Name,
                    Currency = (m.SalesOrder.Currency.Name == "Rupiah") ? "IDR" : m.SalesOrder.Currency.Name,
                    ItemName = m.Item.Name,
                    SKU = m.Item.Sku,
                    UoM = m.Item.UoM.Name,
                    SalesDate = m.SalesOrder.SalesDate,
                    Price = m.Price, //.Item.PriceList,
                    Rate = db.ExchangeRates.Where(x => x.CurrencyId == m.SalesOrder.CurrencyId && m.SalesOrder.SalesDate >= x.ExRateDate && !x.IsDeleted).OrderByDescending(x => x.ExRateDate).FirstOrDefault().Rate,//m.SalesOrder.ExchangeRateAmount,
                    Discount = 0m, //100m - (m.Price/m.Item.PriceMutations.Where(y => (y.DeactivatedAt == null || m.SalesOrder.SalesDate < y.DeactivatedAt.Value)).OrderByDescending(y => y.DeactivatedAt.Value).FirstOrDefault().Amount)*100m,
                    //Amount = m.DeliveryOrderDetail.SalesOrderDetail.Quantity * m.DeliveryOrderDetail.SalesOrderDetail.Price,
                    Status = m.SalesOrder.IsDeliveryCompleted ? "Sent" : "Queue",
                }).Select(g => new
                {
                    CustomerName = g.Key.CustomerName, //g.FirstOrDefault().SalesInvoice.DeliveryOrder.SalesOrder.Contact.NamaFakturPajak, //g.Key.CustomerGroup,
                    Currency = g.Key.Currency,
                    ItemName = g.Key.ItemName,
                    SKU = g.Key.SKU,
                    UoM = g.Key.UoM,
                    SalesDate = g.Key.SalesDate,
                    Price = g.Key.Price,
                    Rate = g.Key.Rate,
                    Status = g.Key.Status,
                    //Amount = g.Key.Amount,
                    Shipped = g.Where(x => (x.SalesOrder.SalesDate == g.Key.SalesDate)).Sum(x => (Decimal?)x.Quantity - x.PendingDeliveryQuantity) ?? 0,
                    Quantity = g.Where(x => (x.SalesOrder.SalesDate == g.Key.SalesDate)).Sum(x => (Decimal?)x.Quantity) ?? 0,
                    Discount = g.Key.Discount, //g.Where(x => (x.SalesInvoice.DeliveryOrder.SalesOrder.SalesDate == g.Key.SalesDate)).Sum(x => (Decimal?)x.DeliveryOrderDetail.SalesOrderDetail.Item.PriceMutations.Where(y => (y.DeactivatedAt == null || g.Key.SalesDate < y.DeactivatedAt.Value)).OrderByDescending(y => y.DeactivatedAt.Value).FirstOrDefault().Amount) ?? 0, //.Sum(x => (Decimal?)(x.SalesInvoice.Discount * g.Key.Price)/100.0m) ?? 0,
                }).OrderBy(x => x.SalesDate).ThenBy(x => x.CustomerName).ThenBy(x => x.ItemName).ToList();

                if (!query.Any())
                {
                    return Content(Constant.ControllerOutput.ErrorPageRecordNotFound);
                }

                var rd = new ReportDocument();

                //Loading Report
                rd.Load(Server.MapPath("~/") + "Reports/General/SalesOrderDailyReportByDate.rpt");

                // Setting report data source
                rd.SetDataSource(query);

                // Setting subreport data source
                //rd.Subreports["subreport.rpt"].SetDataSource(q2);

                // Set parameters, need to be done after all data sources are set (to prevent reseting parameters)
                rd.SetParameterValue("CompanyName", company.Name);
                rd.SetParameterValue("AsOfDate", DateTime.Today);
                rd.SetParameterValue("startDate", startDate);
                rd.SetParameterValue("endDate", endDay);

                var stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                return File(stream, "application/pdf");
            }
        }
        #endregion

        #region DiscountByDate
        public ActionResult DiscountByDate()
        {
            return View();
        }

        public ActionResult ReportDiscountByDate(DateTime startDate, DateTime endDate)
        {
            using (var db = new OffsetPrintingSuppliesEntities())
            {
                DateTime endDay = endDate.AddDays(1);
                var company = _companyService.GetQueryable().FirstOrDefault();
                //var salesInvoice = _salesInvoiceService.GetObjectById(Id);
                var q = db.DeliveryOrderDetails.Include(x => x.SalesOrderDetail)
                                                  .Where(x => !x.IsDeleted && (
                                                            (x.SalesOrderDetail.SalesOrder.SalesDate >= startDate && x.SalesOrderDetail.SalesOrder.SalesDate < endDay)
                                                        ));
                string user = AuthenticationModel.GetUserName();

                var query = q.GroupBy(m => new
                {
                    CustomerName = m.SalesOrderDetail.SalesOrder.Contact.Name,
                    Code = m.DeliveryOrder.NomorSurat,
                    //Currency = (m.SalesOrder.Currency.Name == "Rupiah") ? "IDR" : m.SalesOrder.Currency.Name,
                    SalesDate = m.SalesOrderDetail.SalesOrder.SalesDate,
                    Item = m.Item,
                    Rate = db.ExchangeRates.Where(x => x.CurrencyId == m.SalesOrderDetail.SalesOrder.CurrencyId && m.SalesOrderDetail.SalesOrder.SalesDate >= x.ExRateDate && !x.IsDeleted).OrderByDescending(x => x.ExRateDate).FirstOrDefault().Rate,//m.SalesOrder.ExchangeRateAmount,
                    RatePL = (decimal?)db.ExchangeRates.Where(x => x.CurrencyId == (m.Item.CurrencyId??0) && m.SalesOrderDetail.SalesOrder.SalesDate >= x.ExRateDate && !x.IsDeleted).OrderByDescending(x => x.ExRateDate).FirstOrDefault().Rate??0,
                    AmountIDR = (m.SalesOrderDetail.SalesOrder.Currency.Name == "Rupiah") ? m.SalesOrderDetail.Price : 0,
                    AmountUSD = (m.SalesOrderDetail.SalesOrder.Currency.Name == "USD") ? m.SalesOrderDetail.Price : 0,
                    AmountEUR = (m.SalesOrderDetail.SalesOrder.Currency.Name == "EUR") ? m.SalesOrderDetail.Price : 0,
                    Quantity = m.Quantity,
                    //Rate = db.ExchangeRates.Where(x => x.CurrencyId == m.SalesOrder.CurrencyId && m.SalesOrder.SalesDate >= x.ExRateDate && !x.IsDeleted).OrderByDescending(x => x.ExRateDate).FirstOrDefault().Rate,//m.SalesOrder.ExchangeRateAmount,
                }).Select(g => new
                {
                    CustomerName = g.Key.CustomerName, //g.FirstOrDefault().SalesInvoice.DeliveryOrder.SalesOrder.Contact.NamaFakturPajak, //g.Key.CustomerGroup,
                    Code = g.Key.Code,
                    SalesDate = g.Key.SalesDate,
                    Price = g.Key.Item.PriceList*g.Key.RatePL,
                    Amount = g.Key.Rate*((g.Key.AmountEUR != 0)?g.Key.AmountEUR:(g.Key.AmountUSD != 0)?g.Key.AmountUSD:g.Key.AmountIDR),
                    ItemName = g.Key.Item.Name,
                    Quantity = g.Key.Quantity,
                    AmountUSD = g.Key.AmountUSD,
                    AmountEUR = g.Key.AmountEUR,
                    AmountIDR = g.Key.AmountIDR,
                }).OrderBy(x => x.SalesDate).ThenBy(x => x.Code).ThenBy(x => x.CustomerName).ThenBy(x => x.ItemName).ToList();

                if (!query.Any())
                {
                    return Content(Constant.ControllerOutput.ErrorPageRecordNotFound);
                }

                var rd = new ReportDocument();

                //Loading Report
                rd.Load(Server.MapPath("~/") + "Reports/General/DiscountReportByDate.rpt");

                // Setting report data source
                rd.SetDataSource(query);

                // Setting subreport data source
                //rd.Subreports["subreport.rpt"].SetDataSource(q2);

                // Set parameters, need to be done after all data sources are set (to prevent reseting parameters)
                rd.SetParameterValue("CompanyName", company.Name);
                rd.SetParameterValue("AsOfDate", DateTime.Today);
                rd.SetParameterValue("startDate", startDate);
                rd.SetParameterValue("endDate", endDay);

                var stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                return File(stream, "application/pdf");
            }
        }
        #endregion


        #region MonthlyRollerCompoundUsage
        public ActionResult MonthlyRollerCompoundUsage()
        {
            return View();
        }

        public ActionResult ReportMonthlyRollerCompoundUsage(int Y1)
        {
            using (var db = new OffsetPrintingSuppliesEntities())
            {
                var company = _companyService.GetQueryable().FirstOrDefault();
                //var salesInvoice = _salesInvoiceService.GetObjectById(Id);
                var q = db.RecoveryOrderDetails.Include(x => x.RecoveryOrder).Include(x => x.RollerBuilder)
                                                  .Where(x => !x.IsDeleted && !x.RecoveryOrder.IsDeleted && (
                                                            (x.IsRejected && x.RejectedDate.Value.Year == Y1) || (x.IsFinished && x.FinishedDate.Value.Year == Y1)
                                                        ));
                string user = AuthenticationModel.GetUserName();

                var query = q.GroupBy(m => new
                {
                    //CustomerName = m.SalesInvoice.DeliveryOrder.SalesOrder.Contact.Name,
                    //Currency = (m.SalesInvoice.DeliveryOrder.SalesOrder.Currency.Name == "Rupiah") ? "IDR" : m.SalesInvoice.DeliveryOrder.SalesOrder.Currency.Name,
                    ItemType = m.RollerBuilder.Compound.Name,
                    //SKU = m.DeliveryOrderDetail.SalesOrderDetail.Item.Sku,
                    UoM = m.RollerBuilder.Compound.UoM.Name,
                    //ProcessDate = (m.IsRejected ? m.RejectedDate.Value : m.FinishedDate.Value)
                    //SalesDate = EntityFunctions.TruncateTime(m.SalesInvoice.DeliveryOrder.SalesOrder.SalesDate) ?? DateTime.MinValue,
                }).Select(g => new
                {
                    //CustomerName = g.Key.CustomerName, //g.FirstOrDefault().SalesInvoice.DeliveryOrder.SalesOrder.Contact.NamaFakturPajak, //g.Key.CustomerGroup,
                    //Currency = g.Key.Currency,
                    //ItemName = g.Key.ItemName,
                    ItemType = g.Key.ItemType,
                    //SKU = g.Key.SKU,
                    UoM = g.Key.UoM,
                    //SalesDate = g.Key.SalesDate,
                    //ProcessDate = g.Key.ProcessDate,
                    Amount1 = g.Where(x => (x.IsRejected && x.RejectedDate.Value.Month == 1) || (x.IsFinished && x.FinishedDate.Value.Month == 1)).Sum(x => (Decimal?)x.CompoundUsage) ?? 0,
                    Amount2 = g.Where(x => (x.IsRejected && x.RejectedDate.Value.Month == 2) || (x.IsFinished && x.FinishedDate.Value.Month == 2)).Sum(x => (Decimal?)x.CompoundUsage) ?? 0,
                    Amount3 = g.Where(x => (x.IsRejected && x.RejectedDate.Value.Month == 3) || (x.IsFinished && x.FinishedDate.Value.Month == 3)).Sum(x => (Decimal?)x.CompoundUsage) ?? 0,
                    Amount4 = g.Where(x => (x.IsRejected && x.RejectedDate.Value.Month == 4) || (x.IsFinished && x.FinishedDate.Value.Month == 4)).Sum(x => (Decimal?)x.CompoundUsage) ?? 0,
                    Amount5 = g.Where(x => (x.IsRejected && x.RejectedDate.Value.Month == 5) || (x.IsFinished && x.FinishedDate.Value.Month == 5)).Sum(x => (Decimal?)x.CompoundUsage) ?? 0,
                    Amount6 = g.Where(x => (x.IsRejected && x.RejectedDate.Value.Month == 6) || (x.IsFinished && x.FinishedDate.Value.Month == 6)).Sum(x => (Decimal?)x.CompoundUsage) ?? 0,
                    Amount7 = g.Where(x => (x.IsRejected && x.RejectedDate.Value.Month == 7) || (x.IsFinished && x.FinishedDate.Value.Month == 7)).Sum(x => (Decimal?)x.CompoundUsage) ?? 0,
                    Amount8 = g.Where(x => (x.IsRejected && x.RejectedDate.Value.Month == 8) || (x.IsFinished && x.FinishedDate.Value.Month == 8)).Sum(x => (Decimal?)x.CompoundUsage) ?? 0,
                    Amount9 = g.Where(x => (x.IsRejected && x.RejectedDate.Value.Month == 9) || (x.IsFinished && x.FinishedDate.Value.Month == 9)).Sum(x => (Decimal?)x.CompoundUsage) ?? 0,
                    Amount10 = g.Where(x => (x.IsRejected && x.RejectedDate.Value.Month == 10) || (x.IsFinished && x.FinishedDate.Value.Month == 10)).Sum(x => (Decimal?)x.CompoundUsage) ?? 0,
                    Amount11 = g.Where(x => (x.IsRejected && x.RejectedDate.Value.Month == 11) || (x.IsFinished && x.FinishedDate.Value.Month == 11)).Sum(x => (Decimal?)x.CompoundUsage) ?? 0,
                    Amount12 = g.Where(x => (x.IsRejected && x.RejectedDate.Value.Month == 12) || (x.IsFinished && x.FinishedDate.Value.Month == 12)).Sum(x => (Decimal?)x.CompoundUsage) ?? 0,
                }).AsEnumerable();

                if (!query.Any())
                {
                    return Content(Constant.ControllerOutput.ErrorPageRecordNotFound);
                }

                var rd = new ReportDocument();

                //Loading Report
                query = query.OrderBy(x => x.ItemType).ToList();
                rd.Load(Server.MapPath("~/") + "Reports/General/MonthlyRollerCompoundUsage.rpt");

                // Setting report data source
                rd.SetDataSource(query);

                // Setting subreport data source
                //rd.Subreports["subreport.rpt"].SetDataSource(q2);

                // Set parameters, need to be done after all data sources are set (to prevent reseting parameters)
                rd.SetParameterValue("CompanyName", company.Name);
                rd.SetParameterValue("AsOfDate", DateTime.Today);
                rd.SetParameterValue("Y1", Y1.ToString());
                rd.SetParameterValue("M1", new DateTime(Y1, 1, 1).ToString("MMM-yy"));
                rd.SetParameterValue("M2", new DateTime(Y1, 2, 1).ToString("MMM-yy"));
                rd.SetParameterValue("M3", new DateTime(Y1, 3, 1).ToString("MMM-yy"));
                rd.SetParameterValue("M4", new DateTime(Y1, 4, 1).ToString("MMM-yy"));
                rd.SetParameterValue("M5", new DateTime(Y1, 5, 1).ToString("MMM-yy"));
                rd.SetParameterValue("M6", new DateTime(Y1, 6, 1).ToString("MMM-yy"));
                rd.SetParameterValue("M7", new DateTime(Y1, 7, 1).ToString("MMM-yy"));
                rd.SetParameterValue("M8", new DateTime(Y1, 8, 1).ToString("MMM-yy"));
                rd.SetParameterValue("M9", new DateTime(Y1, 9, 1).ToString("MMM-yy"));
                rd.SetParameterValue("M10", new DateTime(Y1, 10, 1).ToString("MMM-yy"));
                rd.SetParameterValue("M11", new DateTime(Y1, 11, 1).ToString("MMM-yy"));
                rd.SetParameterValue("M12", new DateTime(Y1, 12, 1).ToString("MMM-yy"));

                var stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                return File(stream, "application/pdf");
            }
        }
        #endregion

        #region MonthlyBlanketUsage
        public ActionResult MonthlyBlanketUsage()
        {
            return View();
        }

        public ActionResult ReportMonthlyBlanketUsage(int Y1)
        {
            using (var db = new OffsetPrintingSuppliesEntities())
            {
                var company = _companyService.GetQueryable().FirstOrDefault();
                //var salesInvoice = _salesInvoiceService.GetObjectById(Id);
                var q = db.BlanketOrderDetails.Include(x => x.BlanketOrder).Include(x => x.Blanket)
                                                  .Where(x => !x.IsDeleted && !x.BlanketOrder.IsDeleted && (
                                                            (x.IsRejected && x.RejectedDate.Value.Year == Y1) || (x.IsFinished && x.FinishedDate.Value.Year == Y1)
                                                        ));
                string user = AuthenticationModel.GetUserName();

                var query = q.GroupBy(m => new
                {
                    //CustomerName = m.SalesInvoice.DeliveryOrder.SalesOrder.Contact.Name,
                    //Currency = (m.SalesInvoice.DeliveryOrder.SalesOrder.Currency.Name == "Rupiah") ? "IDR" : m.SalesInvoice.DeliveryOrder.SalesOrder.Currency.Name,
                    ItemType = m.Blanket.RollBlanketItem.Name,
                    //SKU = m.DeliveryOrderDetail.SalesOrderDetail.Item.Sku,
                    UoM = m.Blanket.RollBlanketItem.UoM.Name,
                    //SalesDate = EntityFunctions.TruncateTime(m.SalesInvoice.DeliveryOrder.SalesOrder.SalesDate) ?? DateTime.MinValue,
                }).Select(g => new
                {
                    //CustomerName = g.Key.CustomerName, //g.FirstOrDefault().SalesInvoice.DeliveryOrder.SalesOrder.Contact.NamaFakturPajak, //g.Key.CustomerGroup,
                    //Currency = g.Key.Currency,
                    //ItemName = g.Key.ItemName,
                    ItemType = g.Key.ItemType,
                    //SKU = g.Key.SKU,
                    UoM = g.Key.UoM,
                    //SalesDate = g.Key.SalesDate,
                    Amount1 = g.Where(x => (x.IsRejected && x.RejectedDate.Value.Month == 1) || (x.IsFinished && x.FinishedDate.Value.Month == 1)).Sum(x => (Decimal?)x.RollBlanketUsage) ?? 0, // (x.RollBlanketUsage * x.Blanket.AC * x.Blanket.AR)
                    Amount2 = g.Where(x => (x.IsRejected && x.RejectedDate.Value.Month == 2) || (x.IsFinished && x.FinishedDate.Value.Month == 2)).Sum(x => (Decimal?)x.RollBlanketUsage) ?? 0,
                    Amount3 = g.Where(x => (x.IsRejected && x.RejectedDate.Value.Month == 3) || (x.IsFinished && x.FinishedDate.Value.Month == 3)).Sum(x => (Decimal?)x.RollBlanketUsage) ?? 0,
                    Amount4 = g.Where(x => (x.IsRejected && x.RejectedDate.Value.Month == 4) || (x.IsFinished && x.FinishedDate.Value.Month == 4)).Sum(x => (Decimal?)x.RollBlanketUsage) ?? 0,
                    Amount5 = g.Where(x => (x.IsRejected && x.RejectedDate.Value.Month == 5) || (x.IsFinished && x.FinishedDate.Value.Month == 5)).Sum(x => (Decimal?)x.RollBlanketUsage) ?? 0,
                    Amount6 = g.Where(x => (x.IsRejected && x.RejectedDate.Value.Month == 6) || (x.IsFinished && x.FinishedDate.Value.Month == 6)).Sum(x => (Decimal?)x.RollBlanketUsage) ?? 0,
                    Amount7 = g.Where(x => (x.IsRejected && x.RejectedDate.Value.Month == 7) || (x.IsFinished && x.FinishedDate.Value.Month == 7)).Sum(x => (Decimal?)x.RollBlanketUsage) ?? 0,
                    Amount8 = g.Where(x => (x.IsRejected && x.RejectedDate.Value.Month == 8) || (x.IsFinished && x.FinishedDate.Value.Month == 8)).Sum(x => (Decimal?)x.RollBlanketUsage) ?? 0,
                    Amount9 = g.Where(x => (x.IsRejected && x.RejectedDate.Value.Month == 9) || (x.IsFinished && x.FinishedDate.Value.Month == 9)).Sum(x => (Decimal?)x.RollBlanketUsage) ?? 0,
                    Amount10 = g.Where(x => (x.IsRejected && x.RejectedDate.Value.Month == 10) || (x.IsFinished && x.FinishedDate.Value.Month == 10)).Sum(x => (Decimal?)x.RollBlanketUsage) ?? 0,
                    Amount11 = g.Where(x => (x.IsRejected && x.RejectedDate.Value.Month == 11) || (x.IsFinished && x.FinishedDate.Value.Month == 11)).Sum(x => (Decimal?)x.RollBlanketUsage) ?? 0,
                    Amount12 = g.Where(x => (x.IsRejected && x.RejectedDate.Value.Month == 12) || (x.IsFinished && x.FinishedDate.Value.Month == 12)).Sum(x => (Decimal?)x.RollBlanketUsage) ?? 0,
                }).AsEnumerable();

                if (!query.Any())
                {
                    return Content(Constant.ControllerOutput.ErrorPageRecordNotFound);
                }

                var rd = new ReportDocument();

                //Loading Report
                query = query.OrderBy(x => x.ItemType).ToList();
                rd.Load(Server.MapPath("~/") + "Reports/General/MonthlyBlanketUsage.rpt");

                // Setting report data source
                rd.SetDataSource(query);

                // Setting subreport data source
                //rd.Subreports["subreport.rpt"].SetDataSource(q2);

                // Set parameters, need to be done after all data sources are set (to prevent reseting parameters)
                rd.SetParameterValue("CompanyName", company.Name);
                rd.SetParameterValue("AsOfDate", DateTime.Today);
                rd.SetParameterValue("Y1", Y1.ToString());
                rd.SetParameterValue("M1", new DateTime(Y1, 1, 1).ToString("MMM-yy"));
                rd.SetParameterValue("M2", new DateTime(Y1, 2, 1).ToString("MMM-yy"));
                rd.SetParameterValue("M3", new DateTime(Y1, 3, 1).ToString("MMM-yy"));
                rd.SetParameterValue("M4", new DateTime(Y1, 4, 1).ToString("MMM-yy"));
                rd.SetParameterValue("M5", new DateTime(Y1, 5, 1).ToString("MMM-yy"));
                rd.SetParameterValue("M6", new DateTime(Y1, 6, 1).ToString("MMM-yy"));
                rd.SetParameterValue("M7", new DateTime(Y1, 7, 1).ToString("MMM-yy"));
                rd.SetParameterValue("M8", new DateTime(Y1, 8, 1).ToString("MMM-yy"));
                rd.SetParameterValue("M9", new DateTime(Y1, 9, 1).ToString("MMM-yy"));
                rd.SetParameterValue("M10", new DateTime(Y1, 10, 1).ToString("MMM-yy"));
                rd.SetParameterValue("M11", new DateTime(Y1, 11, 1).ToString("MMM-yy"));
                rd.SetParameterValue("M12", new DateTime(Y1, 12, 1).ToString("MMM-yy"));

                var stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                return File(stream, "application/pdf");
            }
        }
        #endregion

        #region VulcanBlanketSales
        public ActionResult VulcanBlanketSales()
        {
            return View();
        }

        public ActionResult ReportVulcanBlanketSales(DateTime startDate, DateTime endDate)
        {
            using (var db = new OffsetPrintingSuppliesEntities())
            {
                DateTime endDay = endDate.AddDays(1);
                var company = _companyService.GetQueryable().FirstOrDefault();
                //var salesInvoice = _salesInvoiceService.GetObjectById(Id);
                var q = db.SalesOrderDetails.Include(x => x.SalesOrder)
                                                  .Where(x => !x.IsDeleted && x.SalesOrder.IsConfirmed && 
                                                            x.Item.ItemType.Name == Constant.ItemTypeCase.Blanket &&
                                                            x.Item.Name.ToLower().Contains("vulcan") && (
                                                            (x.SalesOrder.SalesDate >= startDate && x.SalesOrder.SalesDate < endDay)
                                                        ));
                string user = AuthenticationModel.GetUserName();

                var query = q.GroupBy(m => new
                {
                    CustomerName = m.SalesOrder.Contact.Name,
                    //Currency = (m.SalesOrder.Currency.Name == "Rupiah") ? "IDR" : m.SalesOrder.Currency.Name,
                    ItemName = m.Item.Name,
                    //SKU = m.Item.Sku,
                    //UoM = m.Item.UoM.Name,
                    SalesDate = m.SalesOrder.SalesDate,
                    Code = m.SalesOrder.NomorSurat,
                    AC = db.Blankets.Where(x => x.Id == m.ItemId).FirstOrDefault().AC,
                    AR = db.Blankets.Where(x => x.Id == m.ItemId).FirstOrDefault().AR,
                    //Price = m.Price, //.Item.PriceList,
                    //Amount = m.DeliveryOrderDetail.SalesOrderDetail.Quantity * m.DeliveryOrderDetail.SalesOrderDetail.Price,
                }).Select(g => new
                {
                    CustomerName = g.Key.CustomerName, //g.FirstOrDefault().SalesInvoice.DeliveryOrder.SalesOrder.Contact.NamaFakturPajak, //g.Key.CustomerGroup,
                    ItemName = g.Key.ItemName,
                    //UoM = g.Key.UoM,
                    Code = g.Key.Code,
                    SalesDate = g.Key.SalesDate,
                    AC = g.Key.AC,
                    AR = g.Key.AR,
                    Quantity = g.Where(x => (x.SalesOrder.SalesDate == g.Key.SalesDate)).Sum(x => (Decimal?)x.Quantity) ?? 0,
                }).OrderBy(x => x.SalesDate).ThenBy(x => x.Code).ThenBy(x => x.CustomerName).ThenBy(x => x.ItemName).ToList();

                if (!query.Any())
                {
                    return Content(Constant.ControllerOutput.ErrorPageRecordNotFound);
                }

                var rd = new ReportDocument();

                //Loading Report
                rd.Load(Server.MapPath("~/") + "Reports/General/VulcanBlanketSalesReport.rpt");

                // Setting report data source
                rd.SetDataSource(query);

                // Setting subreport data source
                //rd.Subreports["subreport.rpt"].SetDataSource(q2);

                // Set parameters, need to be done after all data sources are set (to prevent reseting parameters)
                rd.SetParameterValue("CompanyName", company.Name);
                rd.SetParameterValue("AsOfDate", DateTime.Today);
                rd.SetParameterValue("startDate", startDate);
                rd.SetParameterValue("endDate", endDay);

                var stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                return File(stream, "application/pdf");
            }
        }
        #endregion

        #region SalesPersonnelYearly
        public ActionResult SalesPersonnelYearly()
        {
            return View();
        }

        public ActionResult ReportSalesPersonnelYearly(DateTime startDate, DateTime endDate, int EmployeeId = 0)
        {
            using (var db = new OffsetPrintingSuppliesEntities())
            {
                DateTime endDay = endDate.Date.AddDays(1);
                var company = _companyService.GetQueryable().FirstOrDefault();
                var employee = _employeeService.GetObjectById(EmployeeId);
                //var salesInvoice = _salesInvoiceService.GetObjectById(Id);
                var q = db.SalesInvoices.Include(x => x.SalesInvoiceDetails).Include(x => x.DeliveryOrder)
                                                  .Where(x => !x.IsDeleted && x.DeliveryOrder.SalesOrder.EmployeeId == EmployeeId && (
                                                            (x.DeliveryOrder.SalesOrder.SalesDate >= startDate && x.DeliveryOrder.SalesOrder.SalesDate < endDay)
                                                        ));
                string user = AuthenticationModel.GetUserName();

                var query = q.GroupBy(m => new
                {
                    CustomerName = m.DeliveryOrder.SalesOrder.Contact.Name,
                    Currency = (m.DeliveryOrder.SalesOrder.Currency.Name == "Rupiah") ? "IDR" : m.DeliveryOrder.SalesOrder.Currency.Name,
                    //ItemType = m.DeliveryOrderDetail.SalesOrderDetail.Item.ItemType.Name,
                    //UoM = m.DeliveryOrderDetail.SalesOrderDetail.Item.UoM.Name,
                    //SalesDate = m.SalesInvoice.DeliveryOrder.SalesOrder.SalesDate,
                }).Select(g => new
                {
                    CustomerName = g.Key.CustomerName, //g.FirstOrDefault().SalesInvoice.DeliveryOrder.SalesOrder.Contact.NamaFakturPajak, //g.Key.CustomerGroup,
                    //ItemType = g.Key.ItemType,
                    //UoM = g.Key.UoM,
                    //SalesDate = g.Key.SalesDate,
                    Currency = g.Key.Currency,
                    Amount1 = g.Sum(x => (Decimal?)x.AmountReceivable) ?? 0,
                    Amount2 = g.Sum(x => (Decimal?)db.Receivables.Where(y => !y.IsDeleted && y.ReceivableSource == Constant.ReceivableSource.SalesInvoice && y.ReceivableSourceId == x.Id).FirstOrDefault().RemainingAmount) ?? 0,
                }).OrderBy(x => x.Currency).ThenBy(x => x.CustomerName).ToList();

                if (!query.Any())
                {
                    return Content(Constant.ControllerOutput.ErrorPageRecordNotFound);
                }

                var rd = new ReportDocument();

                //Loading Report
                rd.Load(Server.MapPath("~/") + "Reports/General/SalesPersonnelYearlyByValue.rpt");

                // Setting report data source
                rd.SetDataSource(query);

                // Setting subreport data source
                //rd.Subreports["subreport.rpt"].SetDataSource(q2);

                // Set parameters, need to be done after all data sources are set (to prevent reseting parameters)
                rd.SetParameterValue("CompanyName", company.Name);
                rd.SetParameterValue("AsOfDate", DateTime.Today);
                rd.SetParameterValue("startDate", startDate.Date);
                rd.SetParameterValue("endDate", endDay.Date);
                rd.SetParameterValue("Personnel", employee.Name);

                var stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                return File(stream, "application/pdf");
            }
        }
        #endregion

        #region SalesPersonnelComparisonYearlyByValue
        public ActionResult SalesPersonnelComparisonYearlyByValue()
        {
            return View();
        }

        public ActionResult ReportSalesPersonnelComparisonYearlyByValue(DateTime startDate, DateTime endDate)
        {
            using (var db = new OffsetPrintingSuppliesEntities())
            {
                DateTime endDay = endDate.Date.AddDays(1);
                var company = _companyService.GetQueryable().FirstOrDefault();
                //var salesInvoice = _salesInvoiceService.GetObjectById(Id);
                var q = db.SalesInvoices.Include(x => x.SalesInvoiceDetails).Include(x => x.DeliveryOrder)
                                                  .Where(x => !x.IsDeleted && (
                                                            (x.DeliveryOrder.SalesOrder.SalesDate >= startDate && x.DeliveryOrder.SalesOrder.SalesDate < endDay)
                                                        ));
                string user = AuthenticationModel.GetUserName();

                var query = q.GroupBy(m => new
                {
                    PersonnelName = m.DeliveryOrder.SalesOrder.Employee.Name,
                    Currency = (m.DeliveryOrder.SalesOrder.Currency.Name == "Rupiah") ? "IDR" : m.DeliveryOrder.SalesOrder.Currency.Name,
                    //ItemType = m.DeliveryOrderDetail.SalesOrderDetail.Item.ItemType.Name,
                    //UoM = m.DeliveryOrderDetail.SalesOrderDetail.Item.UoM.Name,
                    //SalesDate = m.SalesInvoice.DeliveryOrder.SalesOrder.SalesDate,
                }).Select(g => new
                {
                    PersonnelName = g.Key.PersonnelName, //g.FirstOrDefault().SalesInvoice.DeliveryOrder.SalesOrder.Contact.NamaFakturPajak, //g.Key.CustomerGroup,
                    //ItemType = g.Key.ItemType,
                    //UoM = g.Key.UoM,
                    //SalesDate = g.Key.SalesDate,
                    Currency = g.Key.Currency,
                    Amount1 = g.Sum(x => (Decimal?)x.AmountReceivable) ?? 0,
                    Amount2 = g.Sum(x => (Decimal?)db.Receivables.Where(y => !y.IsDeleted && y.ReceivableSource == Constant.ReceivableSource.SalesInvoice && y.ReceivableSourceId == x.Id).FirstOrDefault().RemainingAmount) ?? 0,
                }).OrderBy(x => x.Currency).ThenBy(x => x.PersonnelName).ToList();

                if (!query.Any())
                {
                    return Content(Constant.ControllerOutput.ErrorPageRecordNotFound);
                }

                var rd = new ReportDocument();

                //Loading Report
                rd.Load(Server.MapPath("~/") + "Reports/General/SalesPersonnelComparisonYearlyByValue.rpt");

                // Setting report data source
                rd.SetDataSource(query);

                // Setting subreport data source
                //rd.Subreports["subreport.rpt"].SetDataSource(q2);

                // Set parameters, need to be done after all data sources are set (to prevent reseting parameters)
                rd.SetParameterValue("CompanyName", company.Name);
                rd.SetParameterValue("AsOfDate", DateTime.Today);
                rd.SetParameterValue("startDate", startDate.Date);
                rd.SetParameterValue("endDate", endDay.Date);

                var stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                return File(stream, "application/pdf");
            }
        }
        #endregion

        #region SalesPersonnelComparisonYearlyByQty
        public ActionResult SalesPersonnelComparisonYearlyByQty()
        {
            return View();
        }

        public ActionResult ReportSalesPersonnelComparisonYearlyByQty(DateTime start1Date, DateTime end1Date, DateTime start2Date, DateTime end2Date, DateTime start3Date, DateTime end3Date)
        {
            using (var db = new OffsetPrintingSuppliesEntities())
            {
                DateTime startP1, endP1, startP2, endP2, startP3, endP3;
                string P1, P2, P3;
                if (start1Date.Month == end1Date.Month && start1Date.Year == end1Date.Year)
                {
                    startP1 = new DateTime(start1Date.Year, start1Date.Month, 1);
                    endP1 = startP1.AddMonths(1);//.AddDays(-1);
                    P1 = startP1.ToString("MMM-yy");
                }
                else
                {
                    startP1 = new DateTime(start1Date.Year, 1, 1);
                    endP1 = new DateTime(start1Date.Year + 1, 1, 1);//.AddDays(-1);
                    P1 = "Year " + startP1.ToString("yyyy");
                }
                if (start2Date.Month == end2Date.Month && start2Date.Year == end2Date.Year)
                {
                    startP2 = new DateTime(start2Date.Year, start2Date.Month, 1);
                    endP2 = startP2.AddMonths(1);//.AddDays(-1);
                    P2 = startP2.ToString("MMM-yy");
                }
                else
                {
                    startP2 = new DateTime(start2Date.Year, 1, 1);
                    endP2 = new DateTime(start2Date.Year + 1, 1, 1);//.AddDays(-1);
                    P2 = "Year " + startP2.ToString("yyyy");
                } 
                if (start3Date.Month == end3Date.Month && start3Date.Year == end3Date.Year)
                {
                    startP3 = new DateTime(start3Date.Year, start3Date.Month, 1);
                    endP3 = startP3.AddMonths(1);//.AddDays(-1);
                    P3 = startP3.ToString("MMM-yy");
                }
                else
                {
                    startP3 = new DateTime(start3Date.Year, 1, 1);
                    endP3 = new DateTime(start3Date.Year + 1, 1, 1);//.AddDays(-1);
                    P3 = "Year " + startP3.ToString("yyyy");
                }
                var company = _companyService.GetQueryable().FirstOrDefault();
                //var salesInvoice = _salesInvoiceService.GetObjectById(Id);
                var q = db.SalesInvoiceDetails.Include(x => x.SalesInvoice).Include(x => x.DeliveryOrderDetail)
                                                  .Where(x => !x.IsDeleted && !x.SalesInvoice.IsDeleted && (
                                                            (x.SalesInvoice.DeliveryOrder.SalesOrder.SalesDate >= startP1 && x.SalesInvoice.DeliveryOrder.SalesOrder.SalesDate < endP1) ||
                                                            (x.SalesInvoice.DeliveryOrder.SalesOrder.SalesDate >= startP2 && x.SalesInvoice.DeliveryOrder.SalesOrder.SalesDate < endP2) ||
                                                            (x.SalesInvoice.DeliveryOrder.SalesOrder.SalesDate >= startP3 && x.SalesInvoice.DeliveryOrder.SalesOrder.SalesDate < endP3)
                                                        ));
                string user = AuthenticationModel.GetUserName();

                var query = q.GroupBy(m => new
                {
                    PersonnelName = m.SalesInvoice.DeliveryOrder.SalesOrder.Employee.Name,
                    //Currency = (m.DeliveryOrder.SalesOrder.Currency.Name == "Rupiah") ? "IDR" : m.DeliveryOrder.SalesOrder.Currency.Name,
                    ItemType = m.DeliveryOrderDetail.SalesOrderDetail.Item.ItemType.Name,
                    UoM = m.DeliveryOrderDetail.SalesOrderDetail.Item.UoM.Name,
                    //SalesDate = m.SalesInvoice.DeliveryOrder.SalesOrder.SalesDate,
                }).Select(g => new
                {
                    PersonnelName = g.Key.PersonnelName, //g.FirstOrDefault().SalesInvoice.DeliveryOrder.SalesOrder.Contact.NamaFakturPajak, //g.Key.CustomerGroup,
                    ItemType = g.Key.ItemType,
                    UoM = g.Key.UoM,
                    //SalesDate = g.Key.SalesDate,
                    //Currency = g.Key.Currency,
                    Amount1 = g.Where(x => x.SalesInvoice.DeliveryOrder.SalesOrder.SalesDate >= startP1 && x.SalesInvoice.DeliveryOrder.SalesOrder.SalesDate < endP1).Sum(x => (Decimal?)x.Quantity) ?? 0,
                    Amount2 = g.Where(x => x.SalesInvoice.DeliveryOrder.SalesOrder.SalesDate >= startP2 && x.SalesInvoice.DeliveryOrder.SalesOrder.SalesDate < endP2).Sum(x => (Decimal?)x.Quantity) ?? 0,
                    Amount3 = g.Where(x => x.SalesInvoice.DeliveryOrder.SalesOrder.SalesDate >= startP3 && x.SalesInvoice.DeliveryOrder.SalesOrder.SalesDate < endP3).Sum(x => (Decimal?)x.Quantity) ?? 0,
                }).OrderBy(x => x.PersonnelName).ThenBy(x => x.ItemType).ThenBy(x => x.UoM).ToList();

                if (!query.Any())
                {
                    return Content(Constant.ControllerOutput.ErrorPageRecordNotFound);
                }

                var rd = new ReportDocument();

                //Loading Report
                rd.Load(Server.MapPath("~/") + "Reports/General/SalesPersonnelComparisonYearlyByQty.rpt");

                // Setting report data source
                rd.SetDataSource(query);

                // Setting subreport data source
                //rd.Subreports["subreport.rpt"].SetDataSource(q2);

                // Set parameters, need to be done after all data sources are set (to prevent reseting parameters)
                rd.SetParameterValue("CompanyName", company.Name);
                rd.SetParameterValue("AsOfDate", DateTime.Today);
                rd.SetParameterValue("P1", P1);
                rd.SetParameterValue("P2", P2);
                rd.SetParameterValue("P3", P3);

                var stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                return File(stream, "application/pdf");
            }
        }
        #endregion

        #region DailyByQuantity
        public ActionResult DailyByQuantity()
        {
            return View();
        }

        public ActionResult ReportDailyByQuantity(DateTime startDate, DateTime endDate)
        {
            using (var db = new OffsetPrintingSuppliesEntities())
            {
                DateTime endDay = endDate.AddDays(1);
                //DateTime firstMonday = new DateTime(startDate.Year, startDate.Month, 1);
                var company = _companyService.GetQueryable().FirstOrDefault();
                //var salesInvoice = _salesInvoiceService.GetObjectById(Id);
                var q = db.SalesOrderDetails.Include(x => x.SalesOrder)
                                                  .Where(x => !x.IsDeleted && (
                                                            (x.SalesOrder.SalesDate >= startDate && x.SalesOrder.SalesDate < endDay) && (
                                                            (x.Item.ItemType.Name == Constant.ItemTypeCase.Roller) ||
                                                            (x.Item.ItemType.Name == Constant.ItemTypeCase.Blanket) ||
                                                            (x.Item.ItemType.Name == Constant.ItemTypeCase.Chemical) ||
                                                            (x.Item.ItemType.Name == Constant.ItemTypeCase.Underpacking)
                                                        ))).ToList();
                string user = AuthenticationModel.GetUserName();

                var query = q.GroupBy(m => new
                {
                    //CustomerName = m.SalesOrder.Contact.Name,
                    //Currency = (m.SalesOrder.Currency.Name == "Rupiah") ? "IDR" : m.SalesOrder.Currency.Name,
                    //ItemType = m.Item.ItemType.Name,
                    //SKU = m.Item.Sku,
                    //UoM = m.Item.UoM.Name,
                    Day = m.SalesOrder.SalesDate.DayOfWeek.ToString(),
                    Week = m.SalesOrder.SalesDate.GetWeekOfMonth(),
                    CurDate = m.SalesOrder.SalesDate,
                    //Price = m.Price, //.Item.PriceList,
                    //Rate = db.ExchangeRates.Where(x => x.CurrencyId == m.SalesOrder.CurrencyId && m.SalesOrder.SalesDate >= x.ExRateDate && !x.IsDeleted).OrderByDescending(x => x.ExRateDate).FirstOrDefault().Rate,//m.SalesOrder.ExchangeRateAmount,
                    //Discount = 0m, //100m - (m.Price/m.Item.PriceMutations.Where(y => (y.DeactivatedAt == null || m.SalesOrder.SalesDate < y.DeactivatedAt.Value)).OrderByDescending(y => y.DeactivatedAt.Value).FirstOrDefault().Amount)*100m,
                    //Amount = m.DeliveryOrderDetail.SalesOrderDetail.Quantity * m.DeliveryOrderDetail.SalesOrderDetail.Price,
                }).Select(g => new
                {
                    //CustomerName = g.Key.CustomerName, //g.FirstOrDefault().SalesInvoice.DeliveryOrder.SalesOrder.Contact.NamaFakturPajak, //g.Key.CustomerGroup,
                    //Currency = g.Key.Currency,
                    //ItemType = g.Key.ItemType,
                    //SKU = g.Key.SKU,
                    //UoM = g.Key.UoM,
                    Day = g.Key.Day,
                    Week = g.Key.Week,
                    CurDate = g.Key.CurDate,
                    //Price = g.Key.Price,
                    //Rate = g.Key.Rate,
                    //Amount = g.Key.Amount,
                    Amount1 = g.Where(x => (x.SalesOrder.SalesDate == g.Key.CurDate && x.Item.ItemType.Name == Constant.ItemTypeCase.Roller)).Sum(x => (Decimal?)x.Quantity) ?? 0,
                    Amount2 = g.Where(x => (x.SalesOrder.SalesDate == g.Key.CurDate && x.Item.ItemType.Name == Constant.ItemTypeCase.Blanket)).Sum(x => (Decimal?)x.Quantity) ?? 0,
                    Amount3 = g.Where(x => (x.SalesOrder.SalesDate == g.Key.CurDate && x.Item.ItemType.Name == Constant.ItemTypeCase.Chemical && x.Item.Description.ToLower().Contains("fount"))).Sum(x => (Decimal?)x.Quantity) ?? 0,
                    Amount4 = g.Where(x => (x.SalesOrder.SalesDate == g.Key.CurDate && x.Item.ItemType.Name == Constant.ItemTypeCase.Chemical && x.Item.Name.ToLower().Contains("wash"))).Sum(x => (Decimal?)x.Quantity) ?? 0,
                    Amount5 = g.Where(x => (x.SalesOrder.SalesDate == g.Key.CurDate && x.Item.ItemType.Name == Constant.ItemTypeCase.Chemical && x.Item.Name.Contains("IPA"))).Sum(x => (Decimal?)x.Quantity) ?? 0,
                    Amount6 = g.Where(x => (x.SalesOrder.SalesDate == g.Key.CurDate && x.Item.ItemType.Name == Constant.ItemTypeCase.Chemical && (!x.Item.Name.Contains("IPA") && !x.Item.Name.ToLower().Contains("wash") && !x.Item.Name.ToLower().Contains("wash") && !x.Item.Description.ToLower().Contains("fount")))).Sum(x => (Decimal?)x.Quantity * x.Price * db.ExchangeRates.Where(y => y.CurrencyId == x.SalesOrder.CurrencyId && x.SalesOrder.SalesDate >= y.ExRateDate && !y.IsDeleted).OrderByDescending(y => y.ExRateDate).FirstOrDefault().Rate) ?? 0, ////x.SalesOrder.ExchangeRateAmount,
                    Amount7 = g.Where(x => (x.SalesOrder.SalesDate == g.Key.CurDate && x.Item.ItemType.Name == Constant.ItemTypeCase.Chemical && x.Item.Description.ToLower().Contains("powder"))).Sum(x => (Decimal?)x.Quantity) ?? 0,
                    Amount8 = g.Where(x => (x.SalesOrder.SalesDate == g.Key.CurDate && x.Item.ItemType.Name == Constant.ItemTypeCase.Underpacking)).Sum(x => (Decimal?)x.Quantity) ?? 0,
                }).ToList();//.OrderBy(x => x.CurDate).ToList();

                for (DateTime curDay = startDate; curDay < endDay; curDay = curDay.AddDays(1))
                {
                    if ((curDay.DayOfWeek != DayOfWeek.Saturday && curDay.DayOfWeek != DayOfWeek.Sunday) && (query.Where(x => x.CurDate.Date == curDay.Date).FirstOrDefault() == null))
                    {
                        query.Add(new { Day = curDay.DayOfWeek.ToString(), Week = curDay.GetWeekOfMonth(), CurDate = curDay, Amount1 = 0m, Amount2 = 0m, Amount3 = 0m, Amount4 = 0m, Amount5 = 0m, Amount6 = 0m, Amount7 = 0m, Amount8 = 0m });
                    }
                }

                query = query.OrderBy(x => x.CurDate).ToList();

                if (!query.Any())
                {
                    return Content(Constant.ControllerOutput.ErrorPageRecordNotFound);
                }

                var rd = new ReportDocument();

                //Loading Report
                rd.Load(Server.MapPath("~/") + "Reports/General/DailyReportByQuantity.rpt");

                // Setting report data source
                rd.SetDataSource(query);

                // Setting subreport data source
                //rd.Subreports["subreport.rpt"].SetDataSource(q2);

                // Set parameters, need to be done after all data sources are set (to prevent reseting parameters)
                rd.SetParameterValue("CompanyName", company.Name);
                rd.SetParameterValue("AsOfDate", DateTime.Today);
                rd.SetParameterValue("startDate", startDate);
                rd.SetParameterValue("endDate", endDay);

                var stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                return File(stream, "application/pdf");
            }
        }
        #endregion

        #region PenawaranHarga
        public ActionResult PenawaranHarga()
        {
            return View();
        }

        public ActionResult PrintoutPenawaranHarga(string By = "", string ContactPerson = "", int Id = 0)
        {
            using (var db = new OffsetPrintingSuppliesEntities())
            {
                var company = _companyService.GetQueryable().FirstOrDefault();
                string user = AuthenticationModel.GetUserName();
                string ContactNames = Encoding.UTF8.GetString(Convert.FromBase64String(ContactPerson)); //System.Text.Encoding.Default.GetString(Convert.FromBase64String(ContactPerson));
                var q = db.SalesQuotationDetails.Include(x => x.SalesQuotation).Include(x => x.Item)
                                                              .Where(x => !x.IsDeleted && !x.SalesQuotation.IsDeleted && x.SalesQuotationId == Id).ToList();


                var query = q.Select(g => new
                {
                    NamaProduk = g.Item.Name, 
                    ItemType = g.Item.ItemType.Name,
                    UoM = g.Item.UoM.Name,
                    //SalesDate = g.Key.SalesDate,
                    Jml = g.Quantity,
                    HargaSatuan = g.QuotationPrice,
                    Currency = (g.Item.Currency != null ? g.Item.Currency.Name : "Rupiah"),
                    Catatan = g.SalesQuotation.Catatan,// ?? "",
                    Tgl = company.City + (g.SalesQuotation.IsConfirmed ? g.SalesQuotation.ConfirmationDate.GetValueOrDefault() : g.SalesQuotation.QuotationDate).ToString(", d MMMM yyyy", new CultureInfo("id-ID")),
                    NoSurat = g.SalesQuotation.NomorSurat,
                    ContactName = g.SalesQuotation.Contact.Name,
                    ContactAddr = g.SalesQuotation.Contact.DeliveryAddress,
                    ContactPerson = ContactNames, //g.SalesQuotation.Contact.ContactDetails.FirstOrDefault().Name,
                    Personnel = user,
                    Hal = g.SalesQuotation.IsConfirmed ? "Konfirmasi Harga" : "Penawaran Harga",
                    HalDesc = "Perkenankan kami menyampaikan " + (g.SalesQuotation.IsConfirmed ? "konfirmasi harga" : "penawaran harga") + " untuk produk berikut :",
                }).OrderBy(x => x.NamaProduk).ToList(); //.AsEnumerable();

                if (!query.Any())
                {
                    return Content(Constant.ControllerOutput.ErrorPageRecordNotFound);
                }

                var rd = new ReportDocument();

                //Loading Report
                rd.Load(Server.MapPath("~/") + "Reports/Printout/PenawaranHarga.rpt");

                // Setting report data source
                rd.SetDataSource(query);

                // Setting subreport data source
                rd.Subreports["PHSimple"].SetDataSource(query);

                // Set parameters, need to be done after all data sources are set (to prevent reseting parameters)
                rd.SetParameterValue("CompanyName", company.Name);
                rd.SetParameterValue("Tgl", DateTime.Today);
                rd.SetParameterValue("By", "BY " + By);
                rd.SetParameterValue("CompanyAddr", company.Address);

                var stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                return File(stream, "application/pdf");
            }
        }
        #endregion

        #region RollerCollectionNote
        public ActionResult RollerCollectionNote()
        {
            return View();
        }

        public ActionResult PrintoutRollerCollectionNote(int Id = 0)
        {
            using (var db = new OffsetPrintingSuppliesEntities())
            {
                var company = _companyService.GetQueryable().FirstOrDefault();
                string user = AuthenticationModel.GetUserName();
                //string ContactNames = Encoding.UTF8.GetString(Convert.FromBase64String(ContactPerson)); //System.Text.Encoding.Default.GetString(Convert.FromBase64String(ContactPerson));
                var q = db.RecoveryOrderDetails.Include(x => x.RecoveryOrder).Include(x => x.RollerBuilder).Include(x => x.CoreIdentificationDetail).Include(x => x.CompoundUnderLayer).Include(x => x.RecoveryAccessoryDetails)
                                                              .Where(x => !x.IsDeleted && !x.RecoveryOrder.IsDeleted && x.RecoveryOrderId == Id).ToList();

                var obj = q.FirstOrDefault();

                //var query = q.GroupBy(m => new
                //{
                //    RollSKu = m.RollerBuilder.BaseSku,
                //}).Select(g => new
                //{
                //    NoIdent = string.Join(",",g.Select(i => i.CoreIdentificationDetailId.ToString())),
                //    Qty = g.Count(),
                //    Pos = "",
                //    RD = g.FirstOrDefault().RollerBuilder.RD.ToString(),
                //    CD = g.FirstOrDefault().RollerBuilder.CD.ToString(),
                //    RL = g.FirstOrDefault().RollerBuilder.RL.ToString(),
                //    WL = g.FirstOrDefault().RollerBuilder.WL.ToString(),
                //    TL = g.FirstOrDefault().RollerBuilder.TL.ToString(),
                //    Compound = g.FirstOrDefault().RollerBuilder.Compound.Name,
                //    Acc = string.Join(",", g.Select(i => (i.RecoveryAccessoryDetails.Where(y => !y.IsDeleted).Count() > 0) ? "Y" : "N")),
                //    RollerType = g.FirstOrDefault().RollerBuilder.Name,
                //    ItemNo = g.FirstOrDefault().CoreIdentificationDetail.RollerNo,
                //    Comment = g.FirstOrDefault().RollerBuilder.Description,
                //}).ToList();

                var query = q.Select(g => new
                {
                    NoIdent = g.CoreIdentificationDetailId.ToString(),
                    Qty = 1,
                    Pos = "",
                    RD = g.RollerBuilder.RD.ToString(),
                    CD = g.RollerBuilder.CD.ToString(),
                    RL = g.RollerBuilder.RL.ToString(),
                    WL = g.RollerBuilder.WL.ToString(),
                    TL = g.RollerBuilder.TL.ToString(),
                    Compound = g.RollerBuilder.Compound.Name,
                    Acc = (g.RecoveryAccessoryDetails.Where(y => !y.IsDeleted).Count() > 0) ? "Y" : "N",
                    RollerType = g.RollerBuilder.Name,
                    ItemNo = g.CoreIdentificationDetail.RollerNo,
                    Comment = g.RollerBuilder.Description,
                }).OrderBy(x => x.RollerType).ThenBy(x => x.ItemNo).ToList();

                if (!query.Any())
                {
                    return Content(Constant.ControllerOutput.ErrorPageRecordNotFound);
                }

                var rd = new ReportDocument();

                //Loading Report
                rd.Load(Server.MapPath("~/") + "Reports/Printout/RCN.rpt");

                // Setting report data source
                rd.SetDataSource(query);

                // Setting subreport data source
                //rd.Subreports["PHSimple"].SetDataSource(query);

                // Set parameters, need to be done after all data sources are set (to prevent reseting parameters)
                rd.SetParameterValue("CompanyName", company.Name);
                rd.SetParameterValue("Tgl", DateTime.Today);
                rd.SetParameterValue("POno", obj.RecoveryOrder.Code);
                rd.SetParameterValue("Personnel", user); //obj.RecoveryOrder.Employee.Name
                rd.SetParameterValue("Customer", obj.CoreIdentificationDetail.CoreIdentification.Contact.Name);
                rd.SetParameterValue("ContactPerson", ""); //obj.CoreIdentificationDetail.CoreIdentification.Contact.ContactDetails.FirstOrDefault().Name
                rd.SetParameterValue("Addr", obj.CoreIdentificationDetail.CoreIdentification.Contact.DeliveryAddress);

                var stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                return File(stream, "application/pdf");
            }
        }
        #endregion

        #region SalesOrderConfirm
        public ActionResult SalesOrderConfirm()
        {
            return View();
        }

        public ActionResult PrintoutSalesOrderConfirm(int Id = 0, decimal Rate1 = 1, decimal Rate2 = 1, decimal Rate = 1)
        {
            using (var db = new OffsetPrintingSuppliesEntities())
            {
                var company = _companyService.GetQueryable().FirstOrDefault();
                string user = AuthenticationModel.GetUserName();
                //string ContactNames = Encoding.UTF8.GetString(Convert.FromBase64String(ContactPerson)); //System.Text.Encoding.Default.GetString(Convert.FromBase64String(ContactPerson));
                var q = db.SalesOrderDetails.Include(x => x.SalesOrder).Include(x => x.Item)
                                                              .Where(x => !x.IsDeleted && !x.SalesOrder.IsDeleted && x.SalesOrderId == Id).ToList();

                var obj = q.FirstOrDefault();

                var query = q.Select(g => new
                {
                    Code = g.Item.Sku, //g.OrderCode,
                    Name = g.Item.Name,
                    Qty = g.Quantity,
                    UoM = g.Item.UoM.Name,
                    Amount = g.Price * Rate, // * db.ExchangeRates.Where(y => y.CurrencyId == x.SalesOrder.CurrencyId && x.SalesOrder.SalesDate >= y.ExRateDate && !y.IsDeleted).OrderByDescending(y => y.ExRateDate).FirstOrDefault().Rate
                }).OrderBy(x => x.Code).ToList();

                if (!query.Any())
                {
                    return Content(Constant.ControllerOutput.ErrorPageRecordNotFound);
                }

                var rd = new ReportDocument();

                //Loading Report
                rd.Load(Server.MapPath("~/") + "Reports/Printout/OrderConfirmation.rpt");

                // Setting report data source
                rd.SetDataSource(query);

                // Setting subreport data source
                //rd.Subreports["PHSimple"].SetDataSource(query);

                // Set parameters, need to be done after all data sources are set (to prevent reseting parameters)
                rd.SetParameterValue("CompanyName", company.Name ?? "");
                rd.SetParameterValue("Tgl", obj.SalesOrder.SalesDate);
                rd.SetParameterValue("OrderNo", obj.SalesOrder.NomorSurat ?? ""); // obj.SalesOrder.OrderCode ?? ""
                rd.SetParameterValue("Personnel", user ?? ""); //obj.RecoveryOrder.Employee.Name
                rd.SetParameterValue("Customer", obj.SalesOrder.Contact.Name ?? "");
                rd.SetParameterValue("ContactPerson", ""); //obj.SalesOrder.Contact.ContactDetails.FirstOrDefault().Name
                rd.SetParameterValue("Addr", obj.SalesOrder.Contact.DeliveryAddress ?? "");
                rd.SetParameterValue("Currency", obj.SalesOrder.Currency.Name ?? "");
                rd.SetParameterValue("Rate1", Rate1);
                rd.SetParameterValue("Rate2", Rate2);

                var stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                return File(stream, "application/pdf");
            }
        }
        #endregion

        #region PurchaseOrderConfirm
        public ActionResult PurchaseOrderConfirm()
        {
            return View();
        }

        public ActionResult PrintoutPurchaseOrderConfirm(int Id = 0, decimal Rate1 = 1, decimal Rate2 = 1, decimal Rate = 1)
        {
            using (var db = new OffsetPrintingSuppliesEntities())
            {
                var company = _companyService.GetQueryable().FirstOrDefault();
                string user = AuthenticationModel.GetUserName();
                //string ContactNames = Encoding.UTF8.GetString(Convert.FromBase64String(ContactPerson)); //System.Text.Encoding.Default.GetString(Convert.FromBase64String(ContactPerson));
                var q = db.PurchaseOrderDetails.Include(x => x.PurchaseOrder).Include(x => x.Item)
                                                              .Where(x => !x.IsDeleted && !x.PurchaseOrder.IsDeleted && x.PurchaseOrderId == Id).ToList();

                var obj = q.FirstOrDefault();

                var query = q.Select(g => new
                {
                    Code = g.Item.Sku, //g.OrderCode,
                    Name = g.Item.Name,
                    Qty = g.Quantity,
                    UoM = g.Item.UoM.Name,
                    Amount = g.Price * Rate, // * db.ExchangeRates.Where(y => y.CurrencyId == x.SalesOrder.CurrencyId && x.SalesOrder.SalesDate >= y.ExRateDate && !y.IsDeleted).OrderByDescending(y => y.ExRateDate).FirstOrDefault().Rate
                }).OrderBy(x => x.Code).ToList();

                if (!query.Any())
                {
                    return Content(Constant.ControllerOutput.ErrorPageRecordNotFound);
                }

                var rd = new ReportDocument();

                //Loading Report
                rd.Load(Server.MapPath("~/") + "Reports/Printout/OrderConfirmation.rpt");

                // Setting report data source
                rd.SetDataSource(query);

                // Setting subreport data source
                //rd.Subreports["PHSimple"].SetDataSource(query);

                // Set parameters, need to be done after all data sources are set (to prevent reseting parameters)
                rd.SetParameterValue("CompanyName", company.Name ?? "");
                rd.SetParameterValue("Tgl", obj.PurchaseOrder.PurchaseDate);
                rd.SetParameterValue("OrderNo", obj.PurchaseOrder.NomorSurat ?? ""); // obj.SalesOrder.OrderCode ?? ""
                rd.SetParameterValue("Personnel", user ?? ""); //obj.RecoveryOrder.Employee.Name
                rd.SetParameterValue("Customer", obj.PurchaseOrder.Contact.Name ?? "");
                rd.SetParameterValue("ContactPerson", ""); //obj.SalesOrder.Contact.ContactDetails.FirstOrDefault().Name
                rd.SetParameterValue("Addr", obj.PurchaseOrder.Contact.DeliveryAddress ?? "");
                rd.SetParameterValue("Currency", obj.PurchaseOrder.Currency.Name ?? "");
                rd.SetParameterValue("Rate1", Rate1);
                rd.SetParameterValue("Rate2", Rate2);

                var stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                return File(stream, "application/pdf");
            }
        }
        #endregion

        #region DeliveryOrderConfirm
        public ActionResult DeliveryOrderConfirm()
        {
            return View();
        }

        public ActionResult PrintoutDeliveryOrderConfirm(int Id = 0)
        {
            using (var db = new OffsetPrintingSuppliesEntities())
            {
                var company = _companyService.GetQueryable().FirstOrDefault();
                string user = AuthenticationModel.GetUserName();
                //string ContactNames = Encoding.UTF8.GetString(Convert.FromBase64String(ContactPerson)); //System.Text.Encoding.Default.GetString(Convert.FromBase64String(ContactPerson));
                var q = db.DeliveryOrderDetails.Include(x => x.DeliveryOrder).Include(x => x.Item)
                                                              .Where(x => !x.IsDeleted && !x.DeliveryOrder.IsDeleted && x.DeliveryOrderId == Id).ToList();

                var obj = q.FirstOrDefault();

                var query = q.Select(g => new
                {
                    Code = g.Item.Sku, //g.OrderCode,
                    Name = g.Item.Name,
                    Qty = g.Quantity,
                    UoM = g.Item.UoM.Name,
                    //Amount = g.Price * Rate, // * db.ExchangeRates.Where(y => y.CurrencyId == x.SalesOrder.CurrencyId && x.SalesOrder.SalesDate >= y.ExRateDate && !y.IsDeleted).OrderByDescending(y => y.ExRateDate).FirstOrDefault().Rate
                }).OrderBy(x => x.Code).ToList();

                if (!query.Any())
                {
                    return Content(Constant.ControllerOutput.ErrorPageRecordNotFound);
                }

                var rd = new ReportDocument();

                //Loading Report
                rd.Load(Server.MapPath("~/") + "Reports/Printout/DO.rpt");

                // Setting report data source
                rd.SetDataSource(query);

                // Setting subreport data source
                //rd.Subreports["PHSimple"].SetDataSource(query);

                // Set parameters, need to be done after all data sources are set (to prevent reseting parameters)
                rd.SetParameterValue("CompanyName", company.Name ?? "");
                rd.SetParameterValue("Tgl", obj.DeliveryOrder.DeliveryDate);
                rd.SetParameterValue("OrderNo", obj.DeliveryOrder.NomorSurat ?? ""); // obj.SalesOrder.OrderCode ?? ""
                rd.SetParameterValue("Personnel", user ?? ""); //obj.RecoveryOrder.Employee.Name
                rd.SetParameterValue("Customer", obj.DeliveryOrder.SalesOrder.Contact.Name ?? "");
                rd.SetParameterValue("ContactPerson", ""); //obj.SalesOrder.Contact.ContactDetails.FirstOrDefault().Name
                rd.SetParameterValue("Addr", obj.DeliveryOrder.SalesOrder.Contact.DeliveryAddress ?? "");
                rd.SetParameterValue("Currency", obj.DeliveryOrder.SalesOrder.Currency.Name ?? "");
                rd.SetParameterValue("Remark", obj.DeliveryOrder.Remark ?? "");

                var stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                return File(stream, "application/pdf");
            }
        }
        #endregion

        #region SalesInvoiceConfirm
        public ActionResult SalesInvoiceConfirm()
        {
            return View();
        }

        public ActionResult PrintoutSalesInvoiceConfirm(int Id = 0)
        {
            using (var db = new OffsetPrintingSuppliesEntities())
            {
                var company = _companyService.GetQueryable().FirstOrDefault();
                string user = AuthenticationModel.GetUserName();
                //string ContactNames = Encoding.UTF8.GetString(Convert.FromBase64String(ContactPerson)); //System.Text.Encoding.Default.GetString(Convert.FromBase64String(ContactPerson));
                var banklist = db.CashBanks.Where(x => !x.IsDeleted && x.IsBank).Include(x => x.Currency).OrderBy(x => x.Currency.Name).ThenBy(x => x.Name)
                    .Select(m => new
                    {
                        Currency = (m.Currency.Name == "Rupiah") ? "IDR" : (m.Currency.Name == "Euro") ? "EUR" : m.Currency.Name ?? "",
                        Name = m.Name,
                        Desc = m.Description,
                    }).ToList();
                
                var q = db.SalesInvoiceDetails.Include(x => x.SalesInvoice).Include(x => x.DeliveryOrderDetail)
                                                              .Where(x => !x.IsDeleted && !x.SalesInvoice.IsDeleted && x.SalesInvoiceId == Id).ToList();

                var obj = q.FirstOrDefault();

                var query = q.Select(g => new
                {
                    Code = g.DeliveryOrderDetail.Item.Sku, //g.OrderCode,
                    Name = g.DeliveryOrderDetail.Item.Name,
                    Qty = g.Quantity,
                    UoM = g.DeliveryOrderDetail.Item.UoM.Name,
                    Amount = g.Amount / g.Quantity, // * db.ExchangeRates.Where(y => y.CurrencyId == x.SalesOrder.CurrencyId && x.SalesOrder.SalesDate >= y.ExRateDate && !y.IsDeleted).OrderByDescending(y => y.ExRateDate).FirstOrDefault().Rate
                }).OrderBy(x => x.Code).ToList();

                if (!query.Any())
                {
                    return Content(Constant.ControllerOutput.ErrorPageRecordNotFound);
                }

                var rd = new ReportDocument();

                //Loading Report
                rd.Load(Server.MapPath("~/") + "Reports/Printout/Invoice.rpt");

                // Setting report data source
                rd.SetDataSource(query);

                // Setting subreport data source
                rd.Subreports["CashBankList"].SetDataSource(banklist);

                // Set parameters, need to be done after all data sources are set (to prevent reseting parameters)
                rd.SetParameterValue("Customer", obj.SalesInvoice.DeliveryOrder.SalesOrder.Contact.Name ?? "");
                rd.SetParameterValue("Personnel", user ?? ""); //obj.RecoveryOrder.Employee.Name
                rd.SetParameterValue("Tgl", obj.SalesInvoice.InvoiceDate);
                rd.SetParameterValue("Addr", obj.SalesInvoice.DeliveryOrder.SalesOrder.Contact.DeliveryAddress ?? "");
                rd.SetParameterValue("CompanyName", company.Name ?? "");
                rd.SetParameterValue("ContactPerson", ""); //obj.SalesOrder.Contact.ContactDetails.FirstOrDefault().Name
                rd.SetParameterValue("Currency", obj.SalesInvoice.Currency.Name == "Rupiah" ? "Rp." : (obj.SalesInvoice.Currency.Name == "Euro") ? "EUR" : obj.SalesInvoice.Currency.Name ?? "");
                rd.SetParameterValue("OrderNo", obj.SalesInvoice.NomorSurat ?? ""); // obj.SalesOrder.OrderCode ?? ""
                rd.SetParameterValue("Remark", "");
                rd.SetParameterValue("Terbilang", GeneralFunction.changeCurrencyToWords(obj.SalesInvoice.AmountReceivable, true, obj.SalesInvoice.Currency.Name, ""));
                rd.SetParameterValue("Term", obj.SalesInvoice.DeliveryOrder.SalesOrder.Contact.DefaultPaymentTerm);
                rd.SetParameterValue("DOno", obj.DeliveryOrderDetail.DeliveryOrder.NomorSurat);
                rd.SetParameterValue("OurRef", "");
                rd.SetParameterValue("YourRef", "");
                rd.SetParameterValue("Discount", obj.SalesInvoice.Discount);
                rd.SetParameterValue("Tax", obj.SalesInvoice.Tax);

                var stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                return File(stream, "application/pdf");
            }
        }
        #endregion


    }
}