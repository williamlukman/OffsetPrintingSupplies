using Core.Constants;
using Core.DomainModel;
using Core.Interface.Service;
using Data.Repository;
using Service.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Validation.Validation;

namespace WebView
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        private IAccountService _accountService;
        private IItemTypeService _itemTypeService;
        private IRollerTypeService _rollerTypeService;
        private IUserMenuService _userMenuService;
        private IUserAccountService _userAccountService;
        private IUserAccessService _userAccessService;
        private ICompanyService _companyService;
        private ICurrencyService _currencyService;
        private IUoMService _uomService;
        private IMachineService _machineService;
        private IWarehouseService _warehouseService;
        private IItemService _itemService;
        private IBlanketService _blanketService;
        private ICoreBuilderService _coreBuilderService;
        private IRollerBuilderService _rollerBuilderService;
        private IWarehouseItemService _warehouseItemService;
        private IContactService _contactService;
        private IPriceMutationService _priceMutationService;
        private IStockAdjustmentService _stockAdjustmentService;
        private IStockAdjustmentDetailService _stockAdjustmentDetailService;
        private IGeneralLedgerJournalService _generalLedgerJournalService;
        private IClosingService _closingService;
        private IStockMutationService _stockMutationService;
        private IExchangeRateService _exchangeRateService;
        
        private Company baseCompany;

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            PopulateData();
        }

        public void PopulateData()
        {
            _accountService = new AccountService(new AccountRepository(), new AccountValidator());
            _itemTypeService = new ItemTypeService(new ItemTypeRepository(), new ItemTypeValidator());
            _rollerTypeService = new RollerTypeService(new RollerTypeRepository(), new RollerTypeValidator());
            _userMenuService = new UserMenuService(new UserMenuRepository(), new UserMenuValidator());
            _userAccountService = new UserAccountService(new UserAccountRepository(), new UserAccountValidator());
            _userAccessService = new UserAccessService(new UserAccessRepository(), new UserAccessValidator());
            _companyService = new CompanyService(new CompanyRepository(), new CompanyValidator());
            _currencyService = new CurrencyService(new CurrencyRepository(), new CurrencyValidator());
            _uomService = new UoMService(new UoMRepository(), new UoMValidator());
            _machineService = new MachineService(new MachineRepository(), new MachineValidator());
            _warehouseService = new WarehouseService(new WarehouseRepository(), new WarehouseValidator());
            _itemService = new ItemService(new ItemRepository(), new ItemValidator());
            _blanketService = new BlanketService(new BlanketRepository(), new BlanketValidator());
            _coreBuilderService = new CoreBuilderService(new CoreBuilderRepository(), new CoreBuilderValidator());
            _rollerBuilderService = new RollerBuilderService(new RollerBuilderRepository(), new RollerBuilderValidator());
            _warehouseItemService = new WarehouseItemService(new WarehouseItemRepository(), new WarehouseItemValidator());
            _contactService = new ContactService(new ContactRepository(), new ContactValidator());
            _priceMutationService = new PriceMutationService(new PriceMutationRepository(), new PriceMutationValidator());
            _stockAdjustmentService = new StockAdjustmentService(new StockAdjustmentRepository(), new StockAdjustmentValidator());
            _stockAdjustmentDetailService = new StockAdjustmentDetailService(new StockAdjustmentDetailRepository(), new StockAdjustmentDetailValidator());
            _generalLedgerJournalService = new GeneralLedgerJournalService(new GeneralLedgerJournalRepository(), new GeneralLedgerJournalValidator());
            _closingService = new ClosingService(new ClosingRepository(), new ClosingValidator());
            _stockMutationService = new StockMutationService(new StockMutationRepository(), new StockMutationValidator());
            _exchangeRateService = new ExchangeRateService(new ExchangeRateRepository(), new ExchangeRateValidator());
            baseCompany = _companyService.GetQueryable().FirstOrDefault();
          
            if (baseCompany == null)
            {
                baseCompany = _companyService.CreateObject("PT Zentrum Graphics Asia", "Jl. Raya Serpong KM 7, Komplek Multiguna A1 / 1, Serpong Tangerang", "+62 21 5312 3222", "", "zga@zengra.com");
            }

            CreateUserMenus();
            CreateSysAdmin();        
        }

        public void CreateUserMenus()
        {
            _userMenuService.CreateObject(Constant.MenuName.CompanyInfo, Constant.MenuGroupName.Master);
            _userMenuService.CreateObject(Constant.MenuName.Contact, Constant.MenuGroupName.Master);
            _userMenuService.CreateObject(Constant.MenuName.Supplier, Constant.MenuGroupName.Master);
            _userMenuService.CreateObject(Constant.MenuName.ItemType, Constant.MenuGroupName.Master);
            _userMenuService.CreateObject(Constant.MenuName.UoM, Constant.MenuGroupName.Master);
            _userMenuService.CreateObject(Constant.MenuName.Machine, Constant.MenuGroupName.Master);
            _userMenuService.CreateObject(Constant.MenuName.RollerType, Constant.MenuGroupName.Master);

            _userMenuService.CreateObject(Constant.MenuName.Blanket, Constant.MenuGroupName.Master);
            _userMenuService.CreateObject(Constant.MenuName.BlendingRecipe, Constant.MenuGroupName.Master);
            _userMenuService.CreateObject(Constant.MenuName.CoreBuilder, Constant.MenuGroupName.Master);
            _userMenuService.CreateObject(Constant.MenuName.Item, Constant.MenuGroupName.Master);
            _userMenuService.CreateObject(Constant.MenuName.RollerBuilder, Constant.MenuGroupName.Master);
            _userMenuService.CreateObject(Constant.MenuName.StockAdjustment, Constant.MenuGroupName.Master);
            _userMenuService.CreateObject(Constant.MenuName.StockMutation, Constant.MenuGroupName.Master);
            _userMenuService.CreateObject(Constant.MenuName.CustomerStockAdjustment, Constant.MenuGroupName.Master);
            _userMenuService.CreateObject(Constant.MenuName.CustomerStockMutation, Constant.MenuGroupName.Master);
            _userMenuService.CreateObject(Constant.MenuName.Warehouse, Constant.MenuGroupName.Master);
            _userMenuService.CreateObject(Constant.MenuName.WarehouseItem, Constant.MenuGroupName.Master);
            _userMenuService.CreateObject(Constant.MenuName.CustomerItem, Constant.MenuGroupName.Master);
            _userMenuService.CreateObject(Constant.MenuName.WarehouseMutation, Constant.MenuGroupName.Master);

            _userMenuService.CreateObject(Constant.MenuName.BlanketWorkOrder, Constant.MenuGroupName.Manufacturing);
            _userMenuService.CreateObject(Constant.MenuName.BlanketWorkProcess, Constant.MenuGroupName.Manufacturing);
            _userMenuService.CreateObject(Constant.MenuName.BlendingWorkOrder, Constant.MenuGroupName.Manufacturing);
            _userMenuService.CreateObject(Constant.MenuName.RollerIdentification, Constant.MenuGroupName.Manufacturing);
            _userMenuService.CreateObject(Constant.MenuName.RollerAccessoryDetail, Constant.MenuGroupName.Manufacturing);
            _userMenuService.CreateObject(Constant.MenuName.RollerWarehouseMutation, Constant.MenuGroupName.Manufacturing);
            _userMenuService.CreateObject(Constant.MenuName.RecoveryWorkOrder, Constant.MenuGroupName.Manufacturing);
            _userMenuService.CreateObject(Constant.MenuName.RecoveryWorkProcess, Constant.MenuGroupName.Manufacturing);

            _userMenuService.CreateObject(Constant.MenuName.CashBank, Constant.MenuGroupName.Transaction);
            _userMenuService.CreateObject(Constant.MenuName.CashMutation, Constant.MenuGroupName.Transaction);
            _userMenuService.CreateObject(Constant.MenuName.CashBankAdjustment, Constant.MenuGroupName.Transaction);
            _userMenuService.CreateObject(Constant.MenuName.CashBankMutation, Constant.MenuGroupName.Transaction);
            _userMenuService.CreateObject(Constant.MenuName.Currency, Constant.MenuGroupName.Transaction);
            _userMenuService.CreateObject(Constant.MenuName.ExchangeRate, Constant.MenuGroupName.Transaction);

            _userMenuService.CreateObject(Constant.MenuName.PaymentRequest, Constant.MenuGroupName.Transaction);
            _userMenuService.CreateObject(Constant.MenuName.PurchaseOrder, Constant.MenuGroupName.Transaction);
            _userMenuService.CreateObject(Constant.MenuName.PurchaseReceival, Constant.MenuGroupName.Transaction);
            _userMenuService.CreateObject(Constant.MenuName.PurchaseInvoice, Constant.MenuGroupName.Transaction);
            _userMenuService.CreateObject(Constant.MenuName.PurchaseDownPayment, Constant.MenuGroupName.Transaction);
            _userMenuService.CreateObject(Constant.MenuName.PurchaseDPAllocation, Constant.MenuGroupName.Transaction);
            //_userMenuService.CreateObject(Constant.MenuName.CustomPurchaseInvoice, Constant.MenuGroupName.Transaction);
            _userMenuService.CreateObject(Constant.MenuName.PaymentVoucher, Constant.MenuGroupName.Transaction);
            _userMenuService.CreateObject(Constant.MenuName.Payable, Constant.MenuGroupName.Transaction);

            _userMenuService.CreateObject(Constant.MenuName.SalesQuotation, Constant.MenuGroupName.Transaction);
            _userMenuService.CreateObject(Constant.MenuName.SalesOrder, Constant.MenuGroupName.Transaction);
            _userMenuService.CreateObject(Constant.MenuName.SalesDownPayment, Constant.MenuGroupName.Transaction);
            _userMenuService.CreateObject(Constant.MenuName.SalesDPAllocation, Constant.MenuGroupName.Transaction);
            _userMenuService.CreateObject(Constant.MenuName.DeliveryOrder, Constant.MenuGroupName.Transaction);
            _userMenuService.CreateObject(Constant.MenuName.VirtualOrder, Constant.MenuGroupName.Transaction);
            _userMenuService.CreateObject(Constant.MenuName.TemporaryDeliveryOrder, Constant.MenuGroupName.Transaction);
            _userMenuService.CreateObject(Constant.MenuName.TemporaryDeliveryOrderClearance, Constant.MenuGroupName.Transaction);
            _userMenuService.CreateObject(Constant.MenuName.SalesInvoice, Constant.MenuGroupName.Transaction);
            //_userMenuService.CreateObject(Constant.MenuName.RetailSalesInvoice, Constant.MenuGroupName.Transaction);
            //_userMenuService.CreateObject(Constant.MenuName.CashSalesInvoice, Constant.MenuGroupName.Transaction);
            //_userMenuService.CreateObject(Constant.MenuName.CashSalesReturn, Constant.MenuGroupName.Transaction)
            _userMenuService.CreateObject(Constant.MenuName.ReceiptVoucher, Constant.MenuGroupName.Transaction);
            _userMenuService.CreateObject(Constant.MenuName.Receivable, Constant.MenuGroupName.Transaction);
            _userMenuService.CreateObject(Constant.MenuName.Memorial, Constant.MenuGroupName.Transaction);
            _userMenuService.CreateObject(Constant.MenuName.Account, Constant.MenuGroupName.Transaction);
            _userMenuService.CreateObject(Constant.MenuName.Closing, Constant.MenuGroupName.Transaction);
            _userMenuService.CreateObject(Constant.MenuName.GeneralLedger, Constant.MenuGroupName.Transaction);
            _userMenuService.CreateObject(Constant.MenuName.ValidComb, Constant.MenuGroupName.Transaction);

            _userMenuService.CreateObject(Constant.MenuName.Stock, Constant.MenuGroupName.Report);
            _userMenuService.CreateObject(Constant.MenuName.Purchases, Constant.MenuGroupName.Report);
            _userMenuService.CreateObject(Constant.MenuName.Sales, Constant.MenuGroupName.Report);
            _userMenuService.CreateObject(Constant.MenuName.Manufacturing, Constant.MenuGroupName.Report);
            //_userMenuService.CreateObject(Constant.MenuName.TopSales, Constant.MenuGroupName.Report);
            //_userMenuService.CreateObject(Constant.MenuName.ProfitLoss, Constant.MenuGroupName.Report);
            _userMenuService.CreateObject(Constant.MenuName.BalanceSheet, Constant.MenuGroupName.Report);
            _userMenuService.CreateObject(Constant.MenuName.IncomeStatement, Constant.MenuGroupName.Report);
            _userMenuService.CreateObject(Constant.MenuName.Finance, Constant.MenuGroupName.Report);

            _userMenuService.CreateObject(Constant.MenuName.User, Constant.MenuGroupName.Setting);
            _userMenuService.CreateObject(Constant.MenuName.UserAccessRight, Constant.MenuGroupName.Setting);
            
        }

        public void CreateSysAdmin()
        {
            UserAccount userAccount = _userAccountService.GetObjectByUsername(Constant.UserType.Admin);
            if (userAccount == null)
            {
                userAccount = _userAccountService.CreateObject(Constant.UserType.Admin, "sysadmin", "Administrator", "Administrator", true);
            }
            _userAccessService.CreateDefaultAccess(userAccount.Id, _userMenuService, _userAccountService);

        }
    }
}