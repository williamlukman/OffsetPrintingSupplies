﻿using Core.Constants;
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

        private ContactGroup baseContactGroup;
        private Account Asset, CashBank, AccountReceivable, GBCHReceivable, Inventory;
        private Account Expense, CashBankAdjustmentExpense, COGS, Discount, SalesAllowance, StockAdjustmentExpense;
        private Account Liability, AccountPayable, GBCHPayable, GoodsPendingClearance;
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
                Asset = _accountService.CreateLegacyObject(new Account() { Name = "Asset", Code = Constant.AccountCode.Asset, LegacyCode = Constant.AccountLegacyCode.Asset, Level = 1, Group = Constant.AccountGroup.Asset, IsLegacy = true }, _accountService);
                CashBank = _accountService.CreateLegacyObject(new Account() { Name = "CashBank", IsLeaf = true, Code = Constant.AccountCode.CashBank, LegacyCode = Constant.AccountLegacyCode.CashBank, Level = 2, Group = Constant.AccountGroup.Asset, ParentId = Asset.Id, IsLegacy = true }, _accountService);
                AccountReceivable = _accountService.CreateLegacyObject(new Account() { Name = "Account Receivable", IsLeaf = true, Code = Constant.AccountCode.AccountReceivable, LegacyCode = Constant.AccountLegacyCode.AccountReceivable, Level = 2, Group = Constant.AccountGroup.Asset, ParentId = Asset.Id, IsLegacy = true }, _accountService);
                GBCHReceivable = _accountService.CreateLegacyObject(new Account() { Name = "GBCH Receivable", IsLeaf = true, Code = Constant.AccountCode.GBCHReceivable, LegacyCode = Constant.AccountLegacyCode.GBCHReceivable, Level = 2, Group = Constant.AccountGroup.Asset, ParentId = Asset.Id, IsLegacy = true }, _accountService);
                Inventory = _accountService.CreateLegacyObject(new Account() { Name = "Inventory", IsLeaf = true, Code = Constant.AccountCode.Inventory, LegacyCode = Constant.AccountLegacyCode.Inventory, Level = 2, Group = Constant.AccountGroup.Asset, ParentId = Asset.Id, IsLegacy = true }, _accountService);

                Expense = _accountService.CreateLegacyObject(new Account() { Name = "Expense", Code = Constant.AccountCode.Expense, LegacyCode = Constant.AccountLegacyCode.Expense, Level = 1, Group = Constant.AccountGroup.Expense, IsLegacy = true }, _accountService);
                CashBankAdjustmentExpense = _accountService.CreateLegacyObject(new Account() { Name = "CashBank Adjustment Expense", IsLeaf = true, Code = Constant.AccountCode.CashBankAdjustmentExpense, LegacyCode = Constant.AccountLegacyCode.CashBankAdjustmentExpense, Level = 2, Group = Constant.AccountGroup.Expense, ParentId = Expense.Id, IsLegacy = true }, _accountService);
                COGS = _accountService.CreateLegacyObject(new Account() { Name = "Cost Of Goods Sold", IsLeaf = true, Code = Constant.AccountCode.COGS, LegacyCode = Constant.AccountLegacyCode.COGS, Level = 2, Group = Constant.AccountGroup.Expense, ParentId = Expense.Id, IsLegacy = true }, _accountService);
                Discount = _accountService.CreateLegacyObject(new Account() { Name = "Discount", IsLeaf = true, Code = Constant.AccountCode.Discount, LegacyCode = Constant.AccountLegacyCode.Discount, Level = 2, Group = Constant.AccountGroup.Expense, ParentId = Expense.Id, IsLegacy = true }, _accountService);
                SalesAllowance = _accountService.CreateLegacyObject(new Account() { Name = "Sales Allowance", IsLeaf = true, Code = Constant.AccountCode.SalesAllowance, LegacyCode = Constant.AccountLegacyCode.SalesAllowance, Level = 2, Group = Constant.AccountGroup.Expense, ParentId = Expense.Id, IsLegacy = true }, _accountService);
                StockAdjustmentExpense = _accountService.CreateLegacyObject(new Account() { Name = "Stock Adjustment Expense", IsLeaf = true, Code = Constant.AccountCode.StockAdjustmentExpense, LegacyCode = Constant.AccountLegacyCode.StockAdjustmentExpense, Level = 2, Group = Constant.AccountGroup.Expense, ParentId = Expense.Id, IsLegacy = true }, _accountService);

                Liability = _accountService.CreateLegacyObject(new Account() { Name = "Liability", Code = Constant.AccountCode.Liability, LegacyCode = Constant.AccountLegacyCode.Liability, Level = 1, Group = Constant.AccountGroup.Liability, IsLegacy = true }, _accountService);
                AccountPayable = _accountService.CreateLegacyObject(new Account() { Name = "Account Payable", IsLeaf = true, Code = Constant.AccountCode.AccountPayable, LegacyCode = Constant.AccountLegacyCode.AccountPayable, Level = 2, Group = Constant.AccountGroup.Liability, ParentId = Liability.Id, IsLegacy = true }, _accountService);
                GBCHPayable = _accountService.CreateLegacyObject(new Account() { Name = "GBCH Payable", IsLeaf = true, Code = Constant.AccountCode.GBCHPayable, LegacyCode = Constant.AccountLegacyCode.GBCHPayable, Level = 2, Group = Constant.AccountGroup.Liability, ParentId = Liability.Id, IsLegacy = true }, _accountService);
                GoodsPendingClearance = _accountService.CreateLegacyObject(new Account() { Name = "Goods Pending Clearance", IsLeaf = true, Code = Constant.AccountCode.GoodsPendingClearance, LegacyCode = Constant.AccountLegacyCode.GoodsPendingClearance, Level = 2, Group = Constant.AccountGroup.Liability, ParentId = Liability.Id, IsLegacy = true }, _accountService);

                Equity = _accountService.CreateLegacyObject(new Account() { Name = "Equity", Code = Constant.AccountCode.Equity, LegacyCode = Constant.AccountLegacyCode.Equity, Level = 1, Group = Constant.AccountGroup.Equity, IsLegacy = true }, _accountService);
                OwnersEquity = _accountService.CreateLegacyObject(new Account() { Name = "Owners Equity", Code = Constant.AccountCode.OwnersEquity, LegacyCode = Constant.AccountLegacyCode.OwnersEquity, Level = 2, Group = Constant.AccountGroup.Equity, ParentId = Equity.Id, IsLegacy = true }, _accountService);
                EquityAdjustment = _accountService.CreateLegacyObject(new Account() { Name = "Equity Adjustment", IsLeaf = true, Code = Constant.AccountCode.EquityAdjustment, LegacyCode = Constant.AccountLegacyCode.EquityAdjustment, Level = 3, Group = Constant.AccountGroup.Equity, ParentId = OwnersEquity.Id, IsLegacy = true }, _accountService);

                Revenue = _accountService.CreateLegacyObject(new Account() { Name = "Revenue", IsLeaf = true, Code = Constant.AccountCode.Revenue, LegacyCode = Constant.AccountLegacyCode.Revenue, Level = 1, Group = Constant.AccountGroup.Revenue, IsLegacy = true }, _accountService);
            }
        }
    }
}