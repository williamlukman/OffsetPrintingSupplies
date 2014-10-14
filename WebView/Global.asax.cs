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
        private IContactGroupService _contactGroupService;
        private IContactService _contactService;
        private IItemTypeService _itemTypeService;
        private IRollerTypeService _rollerTypeService;
        private IUserMenuService _userMenuService;
        private IUserAccountService _userAccountService;
        private IUserAccessService _userAccessService;
        private ICompanyService _companyService;

        private Company baseCompany;
        private ContactGroup baseContactGroup;
        private Account Asset, CurrentAsset, CashBank, AccountReceivable, GBCHReceivable, Inventory, Raw, FinishedGoods, PrepaidExpense, NonCurrentAsset;
        private Account Expense, COGS, COS, OperationalExpense, ManufacturingExpense, RecoveryExpense, ConversionExpense;
        private Account SellingGeneralAndAdministrationExpense, CashBankAdjustmentExpense, Discount, SalesAllowance, StockAdjustmentExpense;
        private Account NonOperationalExpense, DepreciationExpense, Amortization, InterestExpense, TaxExpense, DividentExpense;
        private Account Liability, CurrentLiability, AccountPayable, GBCHPayable, GoodsPendingClearance, PurchaseAllowance, UnearnedRevenue, NonCurrentLiability;
        private Account Equity, OwnersEquity, EquityAdjustment;
        private Account Revenue;

        public ItemType typeAdhesive, typeAccessory, typeBar, typeBlanket, typeBearing, typeRollBlanket, typeCore, typeCompound, typeChemical,
                        typeConsumable, typeGlue, typeUnderpacking, typeRoller;
        public RollerType typeDamp, typeFoundDT, typeInkFormX, typeInkDistD, typeInkDistM, typeInkDistE,
                          typeInkDuctB, typeInkDistH, typeInkFormW, typeInkDistHQ, typeDampFormDQ, typeInkFormY;

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
            _contactGroupService = new ContactGroupService(new ContactGroupRepository(), new ContactGroupValidator());
            _itemTypeService = new ItemTypeService(new ItemTypeRepository(), new ItemTypeValidator());
            _rollerTypeService = new RollerTypeService(new RollerTypeRepository(), new RollerTypeValidator());
            _userMenuService = new UserMenuService(new UserMenuRepository(), new UserMenuValidator());
            _userAccountService = new UserAccountService(new UserAccountRepository(), new UserAccountValidator());
            _userAccessService = new UserAccessService(new UserAccessRepository(), new UserAccessValidator());
            _companyService = new CompanyService(new CompanyRepository(), new CompanyValidator());

            baseCompany = _companyService.GetQueryable().FirstOrDefault();
            if (baseCompany == null)
            {
                baseCompany = _companyService.CreateObject("PT Zentrum Graphics Asia", "Jl. Raya Serpong KM 7, Komplek Multiguna A1 / 1, Serpong Tangerang", "+62 21 5312 3222", "", "zga@zengra.com");
            }

            if (!_contactGroupService.GetAll().Any())
            {
                baseContactGroup = _contactGroupService.CreateObject(Constant.GroupType.Base, "Base Group", true);
            }

            if (!_itemTypeService.GetAll().Any())
            {
                typeAdhesive = _itemTypeService.CreateObject("Adhesive", "Adhesive");
                typeAccessory = _itemTypeService.CreateObject("Accessory", "Accessory");
                typeBar = _itemTypeService.CreateObject("Bar", "Bar");
                typeBlanket = _itemTypeService.CreateObject("Blanket", "Blanket", true);
                typeBearing = _itemTypeService.CreateObject("Bearing", "Bearing");
                typeRollBlanket = _itemTypeService.CreateObject("RollBlanket", "RollBlanket");
                typeChemical = _itemTypeService.CreateObject("Chemical", "Chemical");
                typeCompound = _itemTypeService.CreateObject("Compound", "Compound");
                typeConsumable = _itemTypeService.CreateObject("Consumable", "Consumable");
                typeCore = _itemTypeService.CreateObject("Core", "Core", true);
                typeGlue = _itemTypeService.CreateObject("Glue", "Glue");
                typeUnderpacking = _itemTypeService.CreateObject("Underpacking", "Underpacking");
                typeRoller = _itemTypeService.CreateObject("Roller", "Roller", true);
            }

            if (!_rollerTypeService.GetAll().Any())
            {
                typeDamp = _rollerTypeService.CreateObject("Damp", "Damp");
                typeFoundDT = _rollerTypeService.CreateObject("Found DT", "Found DT");
                typeInkFormX = _rollerTypeService.CreateObject("Ink Form X", "Ink Form X");
                typeInkDistD = _rollerTypeService.CreateObject("Ink Dist D", "Ink Dist D");
                typeInkDistM = _rollerTypeService.CreateObject("Ink Dist M", "Ink Dist M");
                typeInkDistE = _rollerTypeService.CreateObject("Ink Dist E", "Ink Dist E");
                typeInkDuctB = _rollerTypeService.CreateObject("Ink Duct B", "Ink Duct B");
                typeInkDistH = _rollerTypeService.CreateObject("Ink Dist H", "Ink Dist H");
                typeInkFormW = _rollerTypeService.CreateObject("Ink Form W", "Ink Form W");
                typeInkDistHQ = _rollerTypeService.CreateObject("Ink Dist HQ", "Ink Dist HQ");
                typeDampFormDQ = _rollerTypeService.CreateObject("Damp Form DQ", "Damp Form DQ");
                typeInkFormY = _rollerTypeService.CreateObject("Ink Form Y", "Ink Form Y");
            }

            if (!_accountService.GetLegacyObjects().Any())
            {
                Asset = _accountService.CreateLegacyObject(new Account() { Level = 1, Name = "Asset", Code = Constant.AccountCode.Asset, LegacyCode = Constant.AccountLegacyCode.Asset, Group = Constant.AccountGroup.Asset, IsLegacy = true }, _accountService);
                  CurrentAsset = _accountService.CreateLegacyObject(new Account() { Level = 2, Name = "Current Asset", Code = Constant.AccountCode.CurrentAsset, LegacyCode = Constant.AccountLegacyCode.CurrentAsset, Group = Constant.AccountGroup.Asset, ParentId = Asset.Id, IsLegacy = true }, _accountService);
                    CashBank = _accountService.CreateLegacyObject(new Account() { Level = 3, Name = "Cash & Bank", Code = Constant.AccountCode.CashBank, LegacyCode = Constant.AccountLegacyCode.CashBank, Group = Constant.AccountGroup.Asset, ParentId = CurrentAsset.Id, IsLegacy = true }, _accountService);
                    AccountReceivable = _accountService.CreateLegacyObject(new Account() { Level = 3, IsLeaf = true, Name = "Account Receivable", Code = Constant.AccountCode.AccountReceivable, LegacyCode = Constant.AccountLegacyCode.AccountReceivable, Group = Constant.AccountGroup.Asset, ParentId = CurrentAsset.Id, IsLegacy = true }, _accountService);
                    GBCHReceivable = _accountService.CreateLegacyObject(new Account() { Level = 3, IsLeaf = true, Name = "GBCH Receivable", Code = Constant.AccountCode.GBCHReceivable, LegacyCode = Constant.AccountLegacyCode.GBCHReceivable, Group = Constant.AccountGroup.Asset, ParentId = CurrentAsset.Id, IsLegacy = true }, _accountService);
                    Inventory = _accountService.CreateLegacyObject(new Account() { Level = 3, Name = "Inventory", Code = Constant.AccountCode.Inventory, LegacyCode = Constant.AccountLegacyCode.Inventory, Group = Constant.AccountGroup.Asset, ParentId = CurrentAsset.Id, IsLegacy = true }, _accountService);
                      Raw = _accountService.CreateLegacyObject(new Account() { Level = 4, IsLeaf = true, Name = "Raw Material", Code = Constant.AccountCode.Raw, LegacyCode = Constant.AccountLegacyCode.Raw, Group = Constant.AccountGroup.Asset, ParentId = Inventory.Id, IsLegacy = true }, _accountService);
                      FinishedGoods = _accountService.CreateLegacyObject(new Account() { Level = 4, IsLeaf = true, Name = "Finished Goods", Code = Constant.AccountCode.FinishedGoods, LegacyCode = Constant.AccountLegacyCode.FinishedGoods, Group = Constant.AccountGroup.Asset, ParentId = Inventory.Id, IsLegacy = true }, _accountService);
                    PrepaidExpense = _accountService.CreateLegacyObject(new Account() { Level = 3, IsLeaf = true, Name = "Prepaid Expense (Asset)", Code = Constant.AccountCode.PrepaidExpense, LegacyCode = Constant.AccountLegacyCode.PrepaidExpense, Group = Constant.AccountGroup.Asset, ParentId = CurrentAsset.Id, IsLegacy = true }, _accountService);
                  NonCurrentAsset = _accountService.CreateLegacyObject(new Account() { Level = 2, Name = "Noncurrent Asset", Code = Constant.AccountCode.NonCurrentAsset, LegacyCode = Constant.AccountLegacyCode.NonCurrentAsset, Group = Constant.AccountGroup.Asset, ParentId = Asset.Id, IsLegacy = true}, _accountService);

                Expense = _accountService.CreateLegacyObject(new Account() { Level = 1, Name = "Expense", Code = Constant.AccountCode.Expense, LegacyCode = Constant.AccountLegacyCode.Expense, Group = Constant.AccountGroup.Expense, IsLegacy = true }, _accountService);
                  COGS = _accountService.CreateLegacyObject(new Account() { Level = 2, IsLeaf = true, Name = "Cost Of Goods Sold", Code = Constant.AccountCode.COGS, LegacyCode = Constant.AccountLegacyCode.COGS, Group = Constant.AccountGroup.Expense, ParentId = Expense.Id, IsLegacy = true }, _accountService);
                  COS = _accountService.CreateLegacyObject(new Account() { Level = 2, IsLeaf = true, Name = "Cost of Services", Code = Constant.AccountCode.COS, LegacyCode = Constant.AccountLegacyCode.COS, Group = Constant.AccountGroup.Expense, ParentId = Expense.Id, IsLegacy = true }, _accountService);
                  OperationalExpense = _accountService.CreateLegacyObject(new Account() { Level = 2, Name = "Operational Expense", Code = Constant.AccountCode.OperationalExpense, LegacyCode = Constant.AccountLegacyCode.OperationalExpense, Group = Constant.AccountGroup.Expense, ParentId = Expense.Id, IsLegacy = true }, _accountService);
                    ManufacturingExpense = _accountService.CreateLegacyObject(new Account() { Level = 3, Name = "Manufacturing Expense", Code = Constant.AccountCode.ManufacturingExpense, LegacyCode = Constant.AccountLegacyCode.ManufacturingExpense, Group = Constant.AccountGroup.Expense, ParentId = OperationalExpense.Id, IsLegacy = true }, _accountService);
                      RecoveryExpense = _accountService.CreateLegacyObject(new Account() { Level = 4, IsLeaf = true, Name ="Roller Recovery Expense", Code = Constant.AccountCode.RecoveryExpense, LegacyCode = Constant.AccountLegacyCode.RecoveryExpense, Group = Constant.AccountGroup.Expense, ParentId = ManufacturingExpense.Id, IsLegacy = true }, _accountService);
                      ConversionExpense = _accountService.CreateLegacyObject(new Account() { Level = 4, IsLeaf = true, Name = "Blanket Conversion Expense", Code = Constant.AccountCode.ConversionExpense, LegacyCode = Constant.AccountLegacyCode.ConversionExpense, Group = Constant.AccountGroup.Expense, ParentId = ManufacturingExpense.Id, IsLegacy = true }, _accountService);
                    SellingGeneralAndAdministrationExpense = _accountService.CreateLegacyObject(new Account() { Level = 3, Name = "Selling, General, and Administration Expense", Code = Constant.AccountCode.SellingGeneralAndAdministrationExpense, LegacyCode = Constant.AccountLegacyCode.SellingGeneralAndAdministrationExpense, Group = Constant.AccountGroup.Expense, ParentId = OperationalExpense.Id, IsLegacy = true }, _accountService);
                      CashBankAdjustmentExpense = _accountService.CreateLegacyObject(new Account() { Level = 4, IsLeaf = true, Name = "CashBank Adjustment Expense", Code = Constant.AccountCode.CashBankAdjustmentExpense, LegacyCode = Constant.AccountLegacyCode.CashBankAdjustmentExpense, Group = Constant.AccountGroup.Expense, ParentId = SellingGeneralAndAdministrationExpense.Id, IsLegacy = true }, _accountService);
                      Discount = _accountService.CreateLegacyObject(new Account() { Level = 4, IsLeaf = true, Name = "Discount", Code = Constant.AccountCode.Discount, LegacyCode = Constant.AccountLegacyCode.Discount, Group = Constant.AccountGroup.Expense, ParentId = SellingGeneralAndAdministrationExpense.Id, IsLegacy = true }, _accountService);
                      SalesAllowance = _accountService.CreateLegacyObject(new Account() { Level = 4, IsLeaf = true, Name = "Sales Allowance", Code = Constant.AccountCode.SalesAllowance, LegacyCode = Constant.AccountLegacyCode.SalesAllowance, Group = Constant.AccountGroup.Expense, ParentId = SellingGeneralAndAdministrationExpense.Id, IsLegacy = true }, _accountService);
                      StockAdjustmentExpense = _accountService.CreateLegacyObject(new Account() { Level = 4, IsLeaf = true, Name = "Stock Adjustment Expense", Code = Constant.AccountCode.StockAdjustmentExpense, LegacyCode = Constant.AccountLegacyCode.StockAdjustmentExpense, Group = Constant.AccountGroup.Expense, ParentId = SellingGeneralAndAdministrationExpense.Id, IsLegacy = true }, _accountService);
                  NonOperationalExpense = _accountService.CreateObject(new Account() { Level = 2, Name = "Non Operational Expense", Code = Constant.AccountCode.NonOperationalExpense, LegacyCode = Constant.AccountLegacyCode.NonOperationalExpense, Group = Constant.AccountGroup.Expense, ParentId = Expense.Id, IsLegacy = true }, _accountService);
                    DepreciationExpense = _accountService.CreateObject(new Account() { Level = 3, IsLeaf = true, Name = "Depreciation Expense", Code = Constant.AccountCode.DepreciationExpense, LegacyCode = Constant.AccountLegacyCode.DepreciationExpense, Group = Constant.AccountGroup.Expense, ParentId = NonOperationalExpense.Id, IsLegacy = true}, _accountService);
                    Amortization = _accountService.CreateObject(new Account() { Level = 3, IsLeaf = true, Name = "Amortization", Code = Constant.AccountCode.Amortization, LegacyCode = Constant.AccountLegacyCode.Amortization, Group = Constant.AccountGroup.Expense, ParentId = NonOperationalExpense.Id, IsLegacy = true }, _accountService);
                    InterestExpense = _accountService.CreateObject(new Account() { Level = 3, IsLeaf = true, Name = "Interest Expense", Code = Constant.AccountCode.InterestExpense, LegacyCode = Constant.AccountLegacyCode.InterestExpense, Group = Constant.AccountGroup.Expense, ParentId = NonOperationalExpense.Id, IsLegacy = true }, _accountService);
                    TaxExpense = _accountService.CreateObject(new Account() { Level = 3, IsLeaf = true, Name = "Tax Expense", Code = Constant.AccountCode.TaxExpense, LegacyCode = Constant.AccountLegacyCode.TaxExpense, Group = Constant.AccountGroup.Expense, ParentId = NonOperationalExpense.Id, IsLegacy = true }, _accountService);
                    DividentExpense = _accountService.CreateObject(new Account() { Level = 3, IsLeaf = true, Name = "Divident Expense", Code = Constant.AccountCode.DividentExpense, LegacyCode = Constant.AccountLegacyCode.DividentExpense, Group = Constant.AccountGroup.Expense, ParentId = NonOperationalExpense.Id, IsLegacy = true }, _accountService);

                Liability = _accountService.CreateLegacyObject(new Account() { Level = 1, Name = "Liability", Code = Constant.AccountCode.Liability, LegacyCode = Constant.AccountLegacyCode.Liability, Group = Constant.AccountGroup.Liability, IsLegacy = true }, _accountService);
                  CurrentLiability = _accountService.CreateLegacyObject(new Account() { Level = 2, Name = "Current Liability", Code = Constant.AccountCode.CurrentLiability, LegacyCode = Constant.AccountLegacyCode.CurrentLiability, Group = Constant.AccountGroup.Liability, ParentId = Liability.Id, IsLegacy = true }, _accountService);
                    AccountPayable = _accountService.CreateLegacyObject(new Account() { Level = 3, IsLeaf = true, Name = "Account Payable", Code = Constant.AccountCode.AccountPayable, LegacyCode = Constant.AccountLegacyCode.AccountPayable, Group = Constant.AccountGroup.Liability, ParentId = CurrentLiability.Id, IsLegacy = true }, _accountService);
                    GBCHPayable = _accountService.CreateLegacyObject(new Account() { Level = 3, IsLeaf = true,  Name = "GBCH Payable", Code = Constant.AccountCode.GBCHPayable, LegacyCode = Constant.AccountLegacyCode.GBCHPayable, Group = Constant.AccountGroup.Liability, ParentId = CurrentLiability.Id, IsLegacy = true }, _accountService);
                    GoodsPendingClearance = _accountService.CreateLegacyObject(new Account() { Level = 3, IsLeaf = true,  Name = "Goods Pending Clearance", Code = Constant.AccountCode.GoodsPendingClearance, LegacyCode = Constant.AccountLegacyCode.GoodsPendingClearance, Group = Constant.AccountGroup.Liability, ParentId = CurrentLiability.Id, IsLegacy = true }, _accountService);
                    UnearnedRevenue = _accountService.CreateLegacyObject(new Account() { Level = 3, IsLeaf = true, Name = "Unearned Revenue", Code = Constant.AccountCode.UnearnedRevenue, LegacyCode = Constant.AccountLegacyCode.UnearnedRevenue, Group = Constant.AccountGroup.Liability, ParentId = CurrentLiability.Id, IsLegacy = true }, _accountService);
                    PurchaseAllowance = _accountService.CreateLegacyObject(new Account() { Level = 3, IsLeaf = true, Name = "Purchase Allowance", Code = Constant.AccountCode.PurchaseAllowance, LegacyCode = Constant.AccountLegacyCode.PurchaseAllowance, Group = Constant.AccountGroup.Liability, ParentId = CurrentLiability.Id, IsLegacy = true }, _accountService);
                  NonCurrentLiability = _accountService.CreateLegacyObject(new Account() { Level = 2, Name = "Noncurrent Liability", Code = Constant.AccountCode.NonCurrentLiability, LegacyCode = Constant.AccountLegacyCode.NonCurrentLiability, Group = Constant.AccountGroup.Liability, ParentId = Liability.Id, IsLegacy = true }, _accountService); 

                Equity = _accountService.CreateLegacyObject(new Account() { Level = 1, Name = "Equity", Code = Constant.AccountCode.Equity, LegacyCode = Constant.AccountLegacyCode.Equity, Group = Constant.AccountGroup.Equity, IsLegacy = true }, _accountService);
                  OwnersEquity = _accountService.CreateLegacyObject(new Account() { Level = 2, Name = "Owners Equity", Code = Constant.AccountCode.OwnersEquity, LegacyCode = Constant.AccountLegacyCode.OwnersEquity, Group = Constant.AccountGroup.Equity, ParentId = Equity.Id, IsLegacy = true }, _accountService);
                    EquityAdjustment = _accountService.CreateLegacyObject(new Account() { Level = 3, IsLeaf = true, Name = "Equity Adjustment", Code = Constant.AccountCode.EquityAdjustment, LegacyCode = Constant.AccountLegacyCode.EquityAdjustment, Group = Constant.AccountGroup.Equity, ParentId = OwnersEquity.Id, IsLegacy = true }, _accountService);

                Revenue = _accountService.CreateLegacyObject(new Account() { Level = 1, IsLeaf = true, Name = "Revenue", Code = Constant.AccountCode.Revenue, LegacyCode = Constant.AccountLegacyCode.Revenue, Group = Constant.AccountGroup.Revenue, IsLegacy = true }, _accountService);
            }
            CreateUserMenus();
            CreateSysAdmin();        
        }

        public void CreateUserMenus()
        {
            _userMenuService.CreateObject(Constant.MenuName.Contact, Constant.MenuGroupName.Master);
            _userMenuService.CreateObject(Constant.MenuName.ContactGroup, Constant.MenuGroupName.Master);
            _userMenuService.CreateObject(Constant.MenuName.ItemType, Constant.MenuGroupName.Master);
            _userMenuService.CreateObject(Constant.MenuName.UoM, Constant.MenuGroupName.Master);
            _userMenuService.CreateObject(Constant.MenuName.Machine, Constant.MenuGroupName.Master);
            _userMenuService.CreateObject(Constant.MenuName.RollerType, Constant.MenuGroupName.Master);

            _userMenuService.CreateObject(Constant.MenuName.CashBank, Constant.MenuGroupName.Master);
            _userMenuService.CreateObject(Constant.MenuName.CashMutation, Constant.MenuGroupName.Master);
            _userMenuService.CreateObject(Constant.MenuName.CashBankAdjustment, Constant.MenuGroupName.Master);
            _userMenuService.CreateObject(Constant.MenuName.CashBankMutation, Constant.MenuGroupName.Master);
            _userMenuService.CreateObject(Constant.MenuName.PaymentRequest, Constant.MenuGroupName.Master);

            _userMenuService.CreateObject(Constant.MenuName.Blanket, Constant.MenuGroupName.Master);
            _userMenuService.CreateObject(Constant.MenuName.CoreBuilder, Constant.MenuGroupName.Master);
            _userMenuService.CreateObject(Constant.MenuName.Item, Constant.MenuGroupName.Master);
            _userMenuService.CreateObject(Constant.MenuName.RollerBuilder, Constant.MenuGroupName.Master);
            _userMenuService.CreateObject(Constant.MenuName.StockAdjustment, Constant.MenuGroupName.Master);
            _userMenuService.CreateObject(Constant.MenuName.StockMutation, Constant.MenuGroupName.Master);
            _userMenuService.CreateObject(Constant.MenuName.Warehouse, Constant.MenuGroupName.Master);
            _userMenuService.CreateObject(Constant.MenuName.WarehouseItem, Constant.MenuGroupName.Master);
            _userMenuService.CreateObject(Constant.MenuName.WarehouseMutation, Constant.MenuGroupName.Master);

            _userMenuService.CreateObject(Constant.MenuName.BlanketWorkOrder, Constant.MenuGroupName.Manufacturing);
            _userMenuService.CreateObject(Constant.MenuName.BlanketWorkProcess, Constant.MenuGroupName.Manufacturing);
            _userMenuService.CreateObject(Constant.MenuName.RollerIdentification, Constant.MenuGroupName.Manufacturing);
            _userMenuService.CreateObject(Constant.MenuName.RollerAccessoryDetail, Constant.MenuGroupName.Manufacturing);
            _userMenuService.CreateObject(Constant.MenuName.RollerWarehouseMutation, Constant.MenuGroupName.Manufacturing);
            _userMenuService.CreateObject(Constant.MenuName.RecoveryWorkOrder, Constant.MenuGroupName.Manufacturing);
            _userMenuService.CreateObject(Constant.MenuName.RecoveryWorkProcess, Constant.MenuGroupName.Manufacturing);

            _userMenuService.CreateObject(Constant.MenuName.PurchaseOrder, Constant.MenuGroupName.Transaction);
            _userMenuService.CreateObject(Constant.MenuName.PurchaseReceival, Constant.MenuGroupName.Transaction);
            _userMenuService.CreateObject(Constant.MenuName.PurchaseInvoice, Constant.MenuGroupName.Transaction);
            //_userMenuService.CreateObject(Constant.MenuName.CustomPurchaseInvoice, Constant.MenuGroupName.Transaction);
            _userMenuService.CreateObject(Constant.MenuName.PaymentVoucher, Constant.MenuGroupName.Transaction);
            _userMenuService.CreateObject(Constant.MenuName.Payable, Constant.MenuGroupName.Transaction);

            _userMenuService.CreateObject(Constant.MenuName.SalesOrder, Constant.MenuGroupName.Transaction);            
            _userMenuService.CreateObject(Constant.MenuName.DeliveryOrder, Constant.MenuGroupName.Transaction);
            _userMenuService.CreateObject(Constant.MenuName.VirtualOrder, Constant.MenuGroupName.Transaction);
            _userMenuService.CreateObject(Constant.MenuName.TemporaryDeliveryOrder, Constant.MenuGroupName.Transaction);
            _userMenuService.CreateObject(Constant.MenuName.SalesInvoice, Constant.MenuGroupName.Transaction);
            //_userMenuService.CreateObject(Constant.MenuName.RetailSalesInvoice, Constant.MenuGroupName.Transaction);
            //_userMenuService.CreateObject(Constant.MenuName.CashSalesInvoice, Constant.MenuGroupName.Transaction);
            //_userMenuService.CreateObject(Constant.MenuName.CashSalesReturn, Constant.MenuGroupName.Transaction)
            _userMenuService.CreateObject(Constant.MenuName.ReceiptVoucher, Constant.MenuGroupName.Transaction);
            _userMenuService.CreateObject(Constant.MenuName.Receivable, Constant.MenuGroupName.Transaction);

            //_userMenuService.CreateObject(Constant.MenuName.Item, Constant.MenuGroupName.Report);
            //_userMenuService.CreateObject(Constant.MenuName.Sales, Constant.MenuGroupName.Report);
            //_userMenuService.CreateObject(Constant.MenuName.TopSales, Constant.MenuGroupName.Report);
            //_userMenuService.CreateObject(Constant.MenuName.ProfitLoss, Constant.MenuGroupName.Report);
            _userMenuService.CreateObject(Constant.MenuName.Account, Constant.MenuGroupName.Report);
            _userMenuService.CreateObject(Constant.MenuName.Closing, Constant.MenuGroupName.Report);
            _userMenuService.CreateObject(Constant.MenuName.GeneralLedger, Constant.MenuGroupName.Report);
            _userMenuService.CreateObject(Constant.MenuName.ValidComb, Constant.MenuGroupName.Report);
            _userMenuService.CreateObject(Constant.MenuName.BalanceSheet, Constant.MenuGroupName.Report);
            _userMenuService.CreateObject(Constant.MenuName.IncomeStatement, Constant.MenuGroupName.Report);

            _userMenuService.CreateObject(Constant.MenuName.User, Constant.MenuGroupName.Setting);
            _userMenuService.CreateObject(Constant.MenuName.UserAccessRight, Constant.MenuGroupName.Setting);
            _userMenuService.CreateObject(Constant.MenuName.CompanyInfo, Constant.MenuGroupName.Setting);
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