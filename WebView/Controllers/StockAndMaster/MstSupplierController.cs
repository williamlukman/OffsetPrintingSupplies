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
    public class MstSupplierController : Controller
    {
        private readonly static log4net.ILog LOG = log4net.LogManager.GetLogger("SupplierController");
        private IContactService _contactService;
        private IBlanketService _blanketService;
        private ICoreIdentificationService _coreIdentificationService;
        private IPurchaseOrderService _purchaseOrderService;
        private ISalesOrderService _salesOrderService;
        private ISalesQuotationService _salesQuotationService;
        private IVirtualOrderService _virtualOrderService;
        private IContactGroupService _contactGroupService;

        public MstSupplierController()
        {
            _contactService = new ContactService(new ContactRepository(), new ContactValidator());
            _coreIdentificationService = new CoreIdentificationService(new CoreIdentificationRepository(), new CoreIdentificationValidator());
            _blanketService = new BlanketService(new BlanketRepository(),new BlanketValidator());
            _purchaseOrderService = new PurchaseOrderService(new PurchaseOrderRepository(), new PurchaseOrderValidator());
            _salesOrderService = new SalesOrderService(new SalesOrderRepository(),new SalesOrderValidator());
            _salesQuotationService = new SalesQuotationService(new SalesQuotationRepository(), new SalesQuotationValidator());
            _virtualOrderService = new VirtualOrderService(new VirtualOrderRepository(), new VirtualOrderValidator());
            _contactGroupService = new ContactGroupService(new ContactGroupRepository(), new ContactGroupValidator());
        }

        public ActionResult Index()
        {
            return View();
        }
    }
}
