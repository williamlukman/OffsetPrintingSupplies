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
                                               .Where(x => x.PurchaseOrderId == Id);
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
                                              .Where(x => x.PurchaseReceivalId == Id);
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
                                                 .Where(x => x.PurchaseInvoiceId == Id);
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
                                              .Where(x => x.VirtualOrderId == Id);
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
                                              .Where(x => x.TemporaryDeliveryOrderId == Id);
            string user = AuthenticationModel.GetUserName();

            var query = (from model in q
                         select new
                         {
                             OrderType = (model.TemporaryDeliveryOrder.OrderType == Constant.OrderTypeCase.PartDeliveryOrder) ? "Part Delivery Order" :
                                         (model.TemporaryDeliveryOrder.OrderType == Constant.OrderTypeCase.TrialOrder) ? "Trial Order" :
                                         (model.TemporaryDeliveryOrder.OrderType == Constant.OrderTypeCase.SampleOrder) ? "Sample Order" : "",
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
                                              .Where(x => x.SalesQuotationId == Id);
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
                                              .Where(x => x.SalesOrderId == Id);
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
                                              .Where(x => x.DeliveryOrderId == Id);
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
                                              .Where(x => x.SalesInvoiceId == Id);
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

            var rd = new ReportDocument();

            //Loading Report
            rd.Load(Server.MapPath("~/") + "Reports/General/SalesInvoice.rpt");

            // Setting report data source
            rd.SetDataSource(query);

            var stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            return File(stream, "application/pdf");
        }
        #endregion
    }
}