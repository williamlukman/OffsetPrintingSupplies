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

        private Company baseCompany;
        public Currency currencyIDR;

        public ItemType typeAdhesiveBlanket, typeAdhesiveRoller, typeAccessory, typeBar, typeBlanket, typeBearing, typeRollBlanket, typeCore, typeCompound, typeChemical,
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
            _itemTypeService = new ItemTypeService(new ItemTypeRepository(), new ItemTypeValidator());
            _rollerTypeService = new RollerTypeService(new RollerTypeRepository(), new RollerTypeValidator());
            _userMenuService = new UserMenuService(new UserMenuRepository(), new UserMenuValidator());
            _userAccountService = new UserAccountService(new UserAccountRepository(), new UserAccountValidator());
            _userAccessService = new UserAccessService(new UserAccessRepository(), new UserAccessValidator());
            _companyService = new CompanyService(new CompanyRepository(), new CompanyValidator());
            _currencyService = new CurrencyService(new CurrencyRepository(), new CurrencyValidator());

            baseCompany = _companyService.GetQueryable().FirstOrDefault();
          
            if (baseCompany == null)
            {
                baseCompany = _companyService.CreateObject("PT Zentrum Graphics Asia", "Jl. Raya Serpong KM 7, Komplek Multiguna A1 / 1, Serpong Tangerang", "+62 21 5312 3222", "", "zga@zengra.com");
            }

            if (!_itemTypeService.GetAll().Any())
            {
                typeAdhesiveBlanket = _itemTypeService.CreateObject("AdhesiveBlanket", "AdhesiveBlanket");
                typeAdhesiveRoller = _itemTypeService.CreateObject("AdhesiveRoller", "AdhesiveRoller");
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
                Account AKTIVA1 = _accountService.CreateObject(new Account() { Code = "1", Name = "AKTIVA", Group = 1, Level = 1, ParentId = null, IsLegacy = true, IsLeaf = false, LegacyCode = "A1" }, _accountService);
                Account AKTIVALANCAR2 = _accountService.CreateObject(new Account() { Code = "11", Name = "AKTIVA LANCAR ", Group = 1, Level = 2, ParentId = AKTIVA1.Id, IsLegacy = true, IsLeaf = false, LegacyCode = "A11" }, _accountService);
                Account KASDANSETARAKAS3 = _accountService.CreateObject(new Account() { Code = "1101", Name = "KAS DAN SETARA KAS ", Group = 1, Level = 3, ParentId = AKTIVALANCAR2.Id, IsLegacy = true, IsLeaf = false, LegacyCode = "A1101" }, _accountService);
                Account KASDANBANK4 = _accountService.CreateObject(new Account() { Code = "110101", Name = "KAS DAN BANK", Group = 1, Level = 4, ParentId = KASDANSETARAKAS3.Id, IsLegacy = true, IsLeaf = false, LegacyCode = "A110101" }, _accountService);
                Account DEPOSITOBERJANGKA3 = _accountService.CreateObject(new Account() { Code = "1102", Name = "DEPOSITO BERJANGKA", Group = 1, Level = 3, ParentId = AKTIVALANCAR2.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account DEPOSITOBERJANGKA4 = _accountService.CreateObject(new Account() { Code = "110201", Name = "DEPOSITO BERJANGKA", Group = 1, Level = 4, ParentId = DEPOSITOBERJANGKA3.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account PIUTANGUSAHA3 = _accountService.CreateObject(new Account() { Code = "1103", Name = "PIUTANG USAHA", Group = 1, Level = 3, ParentId = AKTIVALANCAR2.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account PIUTANGUSAHA4 = _accountService.CreateObject(new Account() { Code = "110301", Name = "PIUTANG USAHA", Group = 1, Level = 4, ParentId = PIUTANGUSAHA3.Id, IsLegacy = true, IsLeaf = false, LegacyCode = "A1102" }, _accountService);
                Account PIUTANGLAINLAIN3 = _accountService.CreateObject(new Account() { Code = "1104", Name = "PIUTANG LAIN-LAIN", Group = 1, Level = 3, ParentId = AKTIVALANCAR2.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account PIUTANGLAINLAIN4 = _accountService.CreateObject(new Account() { Code = "110401", Name = "PIUTANG LAIN-LAIN", Group = 1, Level = 4, ParentId = PIUTANGLAINLAIN3.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account PIUTANGLAINNYA5 = _accountService.CreateObject(new Account() { Code = "11040103", Name = "PIUTANG LAINNYA", Group = 1, Level = 5, ParentId = PIUTANGLAINLAIN4.Id, IsLegacy = true, IsLeaf = true, LegacyCode = "A1106" }, _accountService);
                Account PIUTANGGBCH4 = _accountService.CreateObject(new Account() { Code = "110402", Name = "PIUTANG GBCH", Group = 1, Level = 4, ParentId = PIUTANGLAINLAIN3.Id, IsLegacy = true, IsLeaf = false, LegacyCode = "A1103" }, _accountService);
                Account PERSEDIAANBARANG3 = _accountService.CreateObject(new Account() { Code = "1105", Name = "PERSEDIAAN BARANG", Group = 1, Level = 3, ParentId = AKTIVALANCAR2.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account PERSEDIAANBARANG4 = _accountService.CreateObject(new Account() { Code = "110501", Name = "PERSEDIAAN BARANG", Group = 1, Level = 4, ParentId = PERSEDIAANBARANG3.Id, IsLegacy = true, IsLeaf = false, LegacyCode = "A1104" }, _accountService);
                Account PERSEDPRINTINGCHEMICALS5 = _accountService.CreateObject(new Account() { Code = "11050001", Name = "PERSED. PRINTING CHEMICALS", Group = 1, Level = 5, ParentId = PERSEDIAANBARANG4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account PERSEDPRINTINGBLANKET5 = _accountService.CreateObject(new Account() { Code = "11050002", Name = "PERSED. PRINTING BLANKET", Group = 1, Level = 5, ParentId = PERSEDIAANBARANG4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account PERSEDPRINTINGROLLERS5 = _accountService.CreateObject(new Account() { Code = "11050003", Name = "PERSED. PRINTING ROLLERS", Group = 1, Level = 5, ParentId = PERSEDIAANBARANG4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account PERSEDBARANGLAINNYA5 = _accountService.CreateObject(new Account() { Code = "11050004", Name = "PERSED. BARANG LAINNYA", Group = 1, Level = 5, ParentId = PERSEDIAANBARANG4.Id, IsLegacy = true, IsLeaf = true, LegacyCode = "A1104002" }, _accountService);
                Account PERSEDBAHANPEMBANTU5 = _accountService.CreateObject(new Account() { Code = "11050005", Name = "PERSED. BAHAN PEMBANTU", Group = 1, Level = 5, ParentId = PERSEDIAANBARANG4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account BAHANBAKUCHEMICALS5 = _accountService.CreateObject(new Account() { Code = "11050101", Name = "BAHAN BAKU CHEMICALS", Group = 1, Level = 5, ParentId = PERSEDIAANBARANG4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account BAHANBAKUBLANKET5 = _accountService.CreateObject(new Account() { Code = "11050102", Name = "BAHAN BAKU BLANKET", Group = 1, Level = 5, ParentId = PERSEDIAANBARANG4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account BAHANBAKUROLLERS5 = _accountService.CreateObject(new Account() { Code = "11050103", Name = "BAHAN BAKU ROLLERS", Group = 1, Level = 5, ParentId = PERSEDIAANBARANG4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account BAHANBAKUOTHER5 = _accountService.CreateObject(new Account() { Code = "11050104", Name = "BAHAN BAKU OTHER", Group = 1, Level = 5, ParentId = PERSEDIAANBARANG4.Id, IsLegacy = true, IsLeaf = true, LegacyCode = "A1104001" }, _accountService);
                Account UANGMUKAPEMBELIAN3 = _accountService.CreateObject(new Account() { Code = "1106", Name = "UANG MUKA PEMBELIAN", Group = 1, Level = 3, ParentId = AKTIVALANCAR2.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account UANGMUKAPEMBELIAN4 = _accountService.CreateObject(new Account() { Code = "110601", Name = "UANG MUKA PEMBELIAN", Group = 1, Level = 4, ParentId = UANGMUKAPEMBELIAN3.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account UANGMUKAPEMBELIANLOKAL5 = _accountService.CreateObject(new Account() { Code = "11060001", Name = "UANG MUKA PEMBELIAN LOKAL", Group = 1, Level = 5, ParentId = UANGMUKAPEMBELIAN4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account UANGMUKAPEMBELIANIMPORT5 = _accountService.CreateObject(new Account() { Code = "11060002", Name = "UANG MUKA PEMBELIAN IMPORT", Group = 1, Level = 5, ParentId = UANGMUKAPEMBELIAN4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account UANGMUKALAINNYA5 = _accountService.CreateObject(new Account() { Code = "11060003", Name = "UANG MUKA LAINNYA", Group = 1, Level = 5, ParentId = UANGMUKAPEMBELIAN4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account PAJAKDIBAYARDIMUKA3 = _accountService.CreateObject(new Account() { Code = "1107", Name = "PAJAK DIBAYAR DI MUKA", Group = 1, Level = 3, ParentId = AKTIVALANCAR2.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account PAJAKDIBAYARDIMUKA4 = _accountService.CreateObject(new Account() { Code = "110701", Name = "PAJAK DIBAYAR DI MUKA", Group = 1, Level = 4, ParentId = PAJAKDIBAYARDIMUKA3.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account PPHPS225 = _accountService.CreateObject(new Account() { Code = "11070001", Name = "PPH PS 22", Group = 1, Level = 5, ParentId = PAJAKDIBAYARDIMUKA4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account PPHPS235 = _accountService.CreateObject(new Account() { Code = "11070002", Name = "PPH PS 23", Group = 1, Level = 5, ParentId = PAJAKDIBAYARDIMUKA4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account PPHPS255 = _accountService.CreateObject(new Account() { Code = "11070003", Name = "PPH PS 25", Group = 1, Level = 5, ParentId = PAJAKDIBAYARDIMUKA4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account PPNMASUKAN5 = _accountService.CreateObject(new Account() { Code = "11070004", Name = "PPN MASUKAN", Group = 1, Level = 5, ParentId = PAJAKDIBAYARDIMUKA4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account PPHPS245 = _accountService.CreateObject(new Account() { Code = "11070005", Name = "PPH PS 24", Group = 1, Level = 5, ParentId = PAJAKDIBAYARDIMUKA4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account BIAYADIBAYARDIMUKA3 = _accountService.CreateObject(new Account() { Code = "1108", Name = "BIAYA DIBAYAR DIMUKA", Group = 1, Level = 3, ParentId = AKTIVALANCAR2.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account BIAYADIBAYARDIMUKA4 = _accountService.CreateObject(new Account() { Code = "110801", Name = "BIAYA DIBAYAR DIMUKA", Group = 1, Level = 4, ParentId = BIAYADIBAYARDIMUKA3.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account BDDASURANSIKENDARAAN5 = _accountService.CreateObject(new Account() { Code = "11080001", Name = "BDD-ASURANSI KENDARAAN ", Group = 1, Level = 5, ParentId = BIAYADIBAYARDIMUKA4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account BDDASURANSIGEDUNG5 = _accountService.CreateObject(new Account() { Code = "11080002", Name = "BDD-ASURANSI GEDUNG", Group = 1, Level = 5, ParentId = BIAYADIBAYARDIMUKA4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account BDDASSINVENTARISKANTOR5 = _accountService.CreateObject(new Account() { Code = "11080003", Name = "BDD-ASS. INVENTARIS KANTOR", Group = 1, Level = 5, ParentId = BIAYADIBAYARDIMUKA4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account BDDASSMESINPERALATAN5 = _accountService.CreateObject(new Account() { Code = "11080004", Name = "BDD-ASS. MESIN-PERALATAN", Group = 1, Level = 5, ParentId = BIAYADIBAYARDIMUKA4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account BDDSEWAGEDUNGKANTOR5 = _accountService.CreateObject(new Account() { Code = "11080006", Name = "BDD-SEWA GEDUNG/KANTOR", Group = 1, Level = 5, ParentId = BIAYADIBAYARDIMUKA4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account AKTIVATETAP2 = _accountService.CreateObject(new Account() { Code = "14", Name = "AKTIVA TETAP", Group = 1, Level = 2, ParentId = AKTIVA1.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account TANAH3 = _accountService.CreateObject(new Account() { Code = "1401", Name = "TANAH", Group = 1, Level = 3, ParentId = AKTIVATETAP2.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account TANAH4 = _accountService.CreateObject(new Account() { Code = "140101", Name = "TANAH", Group = 1, Level = 4, ParentId = TANAH3.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account TANAH5 = _accountService.CreateObject(new Account() { Code = "140101001", Name = "TANAH", Group = 1, Level = 5, ParentId = TANAH4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account BANGUNAN3 = _accountService.CreateObject(new Account() { Code = "1402", Name = "BANGUNAN", Group = 1, Level = 3, ParentId = AKTIVATETAP2.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account BANGUNAN4 = _accountService.CreateObject(new Account() { Code = "140201", Name = "BANGUNAN", Group = 1, Level = 4, ParentId = BANGUNAN3.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account BANGUNAN5 = _accountService.CreateObject(new Account() { Code = "14020001", Name = "BANGUNAN", Group = 1, Level = 5, ParentId = BANGUNAN4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account KENDARAANBERMOTORD3 = _accountService.CreateObject(new Account() { Code = "1403", Name = "KENDARAAN BERMOTOR (D)", Group = 1, Level = 3, ParentId = AKTIVATETAP2.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account KENDARAANBERMOTORD4 = _accountService.CreateObject(new Account() { Code = "140301", Name = "KENDARAAN BERMOTOR (D)", Group = 1, Level = 4, ParentId = KENDARAANBERMOTORD3.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account KENDARAANBERMOTORD5 = _accountService.CreateObject(new Account() { Code = "14030001", Name = "KENDARAAN BERMOTOR (D)", Group = 1, Level = 5, ParentId = KENDARAANBERMOTORD4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account INVENTARISKANTOR3 = _accountService.CreateObject(new Account() { Code = "1405", Name = "INVENTARIS KANTOR", Group = 1, Level = 3, ParentId = AKTIVATETAP2.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account INVENTARISKANTOR4 = _accountService.CreateObject(new Account() { Code = "140501", Name = "INVENTARIS KANTOR", Group = 1, Level = 4, ParentId = INVENTARISKANTOR3.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account INVENTARISKANTOR5 = _accountService.CreateObject(new Account() { Code = "14050001", Name = "INVENTARIS KANTOR", Group = 1, Level = 5, ParentId = INVENTARISKANTOR4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account MESINDANPERALATAN3 = _accountService.CreateObject(new Account() { Code = "1406", Name = "MESIN DAN PERALATAN", Group = 1, Level = 3, ParentId = AKTIVATETAP2.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account MESINDANPERALATAN4 = _accountService.CreateObject(new Account() { Code = "140601", Name = "MESIN DAN PERALATAN", Group = 1, Level = 4, ParentId = MESINDANPERALATAN3.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account MESINDANPERALATAN5 = _accountService.CreateObject(new Account() { Code = "14060001", Name = "MESIN DAN PERALATAN", Group = 1, Level = 5, ParentId = MESINDANPERALATAN4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account INSTALASILISTRIK3 = _accountService.CreateObject(new Account() { Code = "1407", Name = "INSTALASI LISTRIK", Group = 1, Level = 3, ParentId = AKTIVATETAP2.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account INSTALASILISTRIK4 = _accountService.CreateObject(new Account() { Code = "140701", Name = "INSTALASI LISTRIK", Group = 1, Level = 4, ParentId = INSTALASILISTRIK3.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account INSTALASILISTRIK5 = _accountService.CreateObject(new Account() { Code = "14070001", Name = "INSTALASI LISTRIK", Group = 1, Level = 5, ParentId = INSTALASILISTRIK4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account AKTIVALEASING5 = _accountService.CreateObject(new Account() { Code = "14070002", Name = "AKTIVA LEASING", Group = 1, Level = 5, ParentId = INSTALASILISTRIK4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account AKUMULASIPENYUSUTAN3 = _accountService.CreateObject(new Account() { Code = "1408", Name = "AKUMULASI PENYUSUTAN", Group = 1, Level = 3, ParentId = AKTIVATETAP2.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "X2401" }, _accountService);
                Account AKUMULASIPENYUSUTAN4 = _accountService.CreateObject(new Account() { Code = "140801", Name = "AKUMULASI PENYUSUTAN", Group = 1, Level = 4, ParentId = AKUMULASIPENYUSUTAN3.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account AKUMPENYBANGUNAN5 = _accountService.CreateObject(new Account() { Code = "14080001", Name = "AKUM. PENY. BANGUNAN", Group = 1, Level = 5, ParentId = AKUMULASIPENYUSUTAN4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account AKUMPENYKENDBERMOTORD5 = _accountService.CreateObject(new Account() { Code = "14080002", Name = "AKUM. PENY. KEND. BERMOTOR-D", Group = 1, Level = 5, ParentId = AKUMULASIPENYUSUTAN4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account AKUMPENYINVENTARISKANTOR5 = _accountService.CreateObject(new Account() { Code = "14080004", Name = "AKUM. PENY. INVENTARIS KANTOR", Group = 1, Level = 5, ParentId = AKUMULASIPENYUSUTAN4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account AKUMPENYMESINPERALATAN5 = _accountService.CreateObject(new Account() { Code = "14080005", Name = "AKUM. PENY. MESIN-PERALATAN", Group = 1, Level = 5, ParentId = AKUMULASIPENYUSUTAN4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account AKUMPENYINSTALASILISTRIK5 = _accountService.CreateObject(new Account() { Code = "14080006", Name = "AKUM. PENY. INSTALASI LISTRIK", Group = 1, Level = 5, ParentId = AKUMULASIPENYUSUTAN4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account AKUMPENYAKTIVALEASING5 = _accountService.CreateObject(new Account() { Code = "14080007", Name = "AKUM. PENY. AKTIVA LEASING", Group = 1, Level = 5, ParentId = AKUMULASIPENYUSUTAN4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account AKTIVALAINLAIN2 = _accountService.CreateObject(new Account() { Code = "15", Name = "AKTIVA LAIN-LAIN", Group = 1, Level = 2, ParentId = AKTIVA1.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account UANGJAMINAN3 = _accountService.CreateObject(new Account() { Code = "1501", Name = "UANG JAMINAN", Group = 1, Level = 3, ParentId = AKTIVALAINLAIN2.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account UANGJAMINAN4 = _accountService.CreateObject(new Account() { Code = "150101", Name = "UANG JAMINAN", Group = 1, Level = 4, ParentId = UANGJAMINAN3.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account UANGJAMINAN5 = _accountService.CreateObject(new Account() { Code = "15010001", Name = "UANG JAMINAN", Group = 1, Level = 5, ParentId = UANGJAMINAN4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account BEBANUSAHA1 = _accountService.CreateObject(new Account() { Code = "2", Name = "BEBAN USAHA", Group = 2, Level = 1, ParentId = null, IsLegacy = true, IsLeaf = false, LegacyCode = "X2" }, _accountService);
                Account BEBANPEMASARAN2 = _accountService.CreateObject(new Account() { Code = "61", Name = "BEBAN PEMASARAN", Group = 2, Level = 2, ParentId = BEBANUSAHA1.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account BIAYAIKLANDANREKLAME3 = _accountService.CreateObject(new Account() { Code = "6101", Name = "BIAYA IKLAN DAN REKLAME", Group = 2, Level = 3, ParentId = BEBANPEMASARAN2.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account BIAYAIKLANDANREKLAME4 = _accountService.CreateObject(new Account() { Code = "610101", Name = "BIAYA IKLAN DAN REKLAME", Group = 2, Level = 4, ParentId = BIAYAIKLANDANREKLAME3.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account BIAYAIKLANDANREKLAME5 = _accountService.CreateObject(new Account() { Code = "61010001", Name = "BIAYA IKLAN DAN REKLAME", Group = 2, Level = 5, ParentId = BIAYAIKLANDANREKLAME4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account BIAYAPAMERAN3 = _accountService.CreateObject(new Account() { Code = "6102", Name = "BIAYA PAMERAN", Group = 2, Level = 3, ParentId = BEBANPEMASARAN2.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account BIAYAPAMERAN4 = _accountService.CreateObject(new Account() { Code = "610201", Name = "BIAYA PAMERAN", Group = 2, Level = 4, ParentId = BIAYAPAMERAN3.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account BIAYAPAMERAN5 = _accountService.CreateObject(new Account() { Code = "61020001", Name = "BIAYA PAMERAN", Group = 2, Level = 5, ParentId = BIAYAPAMERAN4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account BIAYAPROMOSI3 = _accountService.CreateObject(new Account() { Code = "6103", Name = "BIAYA PROMOSI", Group = 2, Level = 3, ParentId = BEBANPEMASARAN2.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account BIAYAPROMOSI4 = _accountService.CreateObject(new Account() { Code = "610301", Name = "BIAYA PROMOSI", Group = 2, Level = 4, ParentId = BIAYAPROMOSI3.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account BIAYAPROMOSI5 = _accountService.CreateObject(new Account() { Code = "61030001", Name = "BIAYA PROMOSI", Group = 2, Level = 5, ParentId = BIAYAPROMOSI4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account BIAYAPERJALANANDINAS3 = _accountService.CreateObject(new Account() { Code = "6105", Name = "BIAYA PERJALANAN DINAS", Group = 2, Level = 3, ParentId = BEBANPEMASARAN2.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account BIAYAPERJALANANDINAS4 = _accountService.CreateObject(new Account() { Code = "610501", Name = "BIAYA PERJALANAN DINAS", Group = 2, Level = 4, ParentId = BIAYAPERJALANANDINAS3.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account BIAYAPERJALANANDINAS5 = _accountService.CreateObject(new Account() { Code = "61050001", Name = "BIAYA PERJALANAN DINAS", Group = 2, Level = 5, ParentId = BIAYAPERJALANANDINAS4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account BIAYAPENGIRIMAN3 = _accountService.CreateObject(new Account() { Code = "6106", Name = "BIAYA PENGIRIMAN", Group = 2, Level = 3, ParentId = BEBANPEMASARAN2.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account BIAYAPENGIRIMAN4 = _accountService.CreateObject(new Account() { Code = "610601", Name = "BIAYA PENGIRIMAN", Group = 2, Level = 4, ParentId = BIAYAPENGIRIMAN3.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account BIAYAPENGIRIMAN5 = _accountService.CreateObject(new Account() { Code = "61060001", Name = "BIAYA PENGIRIMAN", Group = 2, Level = 5, ParentId = BIAYAPENGIRIMAN4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account BIAYAKEMASAN3 = _accountService.CreateObject(new Account() { Code = "6107", Name = "BIAYA KEMASAN", Group = 2, Level = 3, ParentId = BEBANPEMASARAN2.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account BIAYAKEMASAN4 = _accountService.CreateObject(new Account() { Code = "610701", Name = "BIAYA KEMASAN", Group = 2, Level = 4, ParentId = BIAYAKEMASAN3.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account BIAYAKEMASAN5 = _accountService.CreateObject(new Account() { Code = "61070001", Name = "BIAYA KEMASAN", Group = 2, Level = 5, ParentId = BIAYAKEMASAN4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account BIAYAADMINISTRASIUMUM2 = _accountService.CreateObject(new Account() { Code = "62", Name = "BIAYA ADMINISTRASI & UMUM", Group = 2, Level = 2, ParentId = BEBANUSAHA1.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account BIAYATENAGAKERJA3 = _accountService.CreateObject(new Account() { Code = "6201", Name = "BIAYA TENAGA KERJA", Group = 2, Level = 3, ParentId = BIAYAADMINISTRASIUMUM2.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account BIAYATENAGAKERJA4 = _accountService.CreateObject(new Account() { Code = "620101", Name = "BIAYA TENAGA KERJA", Group = 2, Level = 4, ParentId = BIAYATENAGAKERJA3.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account BIAYAGAJIDANUPAH5 = _accountService.CreateObject(new Account() { Code = "62010001", Name = "BIAYA GAJI DAN UPAH", Group = 2, Level = 5, ParentId = BIAYATENAGAKERJA4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account PREMIJAMSOSTEK5 = _accountService.CreateObject(new Account() { Code = "62010002", Name = "PREMI JAMSOSTEK", Group = 2, Level = 5, ParentId = BIAYATENAGAKERJA4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account BIAYAREKRUITMENT5 = _accountService.CreateObject(new Account() { Code = "62010003", Name = "BIAYA REKRUITMENT", Group = 2, Level = 5, ParentId = BIAYATENAGAKERJA4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account BYPENDIDIKANLATIHAN5 = _accountService.CreateObject(new Account() { Code = "62010004", Name = "BY. PENDIDIKAN & LATIHAN", Group = 2, Level = 5, ParentId = BIAYATENAGAKERJA4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account BIAYAPERAWATANKESEHATAN5 = _accountService.CreateObject(new Account() { Code = "62010005", Name = "BIAYA PERAWATAN KESEHATAN", Group = 2, Level = 5, ParentId = BIAYATENAGAKERJA4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account BIAYAOPERASIONALKANTOR3 = _accountService.CreateObject(new Account() { Code = "6202", Name = "BIAYA OPERASIONAL KANTOR", Group = 2, Level = 3, ParentId = BIAYAADMINISTRASIUMUM2.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account BIAYAOPERASIONALKANTOR4 = _accountService.CreateObject(new Account() { Code = "620201", Name = "BIAYA OPERASIONAL KANTOR", Group = 2, Level = 4, ParentId = BIAYAOPERASIONALKANTOR3.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account BIAYACETAKAN5 = _accountService.CreateObject(new Account() { Code = "62020001", Name = "BIAYA CETAKAN", Group = 2, Level = 5, ParentId = BIAYAOPERASIONALKANTOR4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account BIAYAATK5 = _accountService.CreateObject(new Account() { Code = "62020002", Name = "BIAYA ATK", Group = 2, Level = 5, ParentId = BIAYAOPERASIONALKANTOR4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account BIAYATRANSPORTD5 = _accountService.CreateObject(new Account() { Code = "62020003", Name = "BIAYA TRANSPORT (D)", Group = 2, Level = 5, ParentId = BIAYAOPERASIONALKANTOR4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account BIAYAPERIJINAN5 = _accountService.CreateObject(new Account() { Code = "62020005", Name = "BIAYA PERIJINAN", Group = 2, Level = 5, ParentId = BIAYAOPERASIONALKANTOR4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account BIAYAMATERAI5 = _accountService.CreateObject(new Account() { Code = "62020006", Name = "BIAYA MATERAI", Group = 2, Level = 5, ParentId = BIAYAOPERASIONALKANTOR4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account BIAYAFOTOCOPY5 = _accountService.CreateObject(new Account() { Code = "62020007", Name = "BIAYA FOTOCOPY", Group = 2, Level = 5, ParentId = BIAYAOPERASIONALKANTOR4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account BYKEBERSIHANKEAMANAN5 = _accountService.CreateObject(new Account() { Code = "62020008", Name = "BY. KEBERSIHAN & KEAMANAN", Group = 2, Level = 5, ParentId = BIAYAOPERASIONALKANTOR4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account BIAYAPOSDANKURIR5 = _accountService.CreateObject(new Account() { Code = "62020009", Name = "BIAYA POS DAN KURIR", Group = 2, Level = 5, ParentId = BIAYAOPERASIONALKANTOR4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account BIAYAKORANDANMAJALAH5 = _accountService.CreateObject(new Account() { Code = "62020010", Name = "BIAYA KORAN DAN MAJALAH", Group = 2, Level = 5, ParentId = BIAYAOPERASIONALKANTOR4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account BIAYAPAJAKKENDARAAN5 = _accountService.CreateObject(new Account() { Code = "62020011", Name = "BIAYA PAJAK KENDARAAN", Group = 2, Level = 5, ParentId = BIAYAOPERASIONALKANTOR4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account BIAYADENDADANPAJAK5 = _accountService.CreateObject(new Account() { Code = "62020012", Name = "BIAYA DENDA DAN PAJAK", Group = 2, Level = 5, ParentId = BIAYAOPERASIONALKANTOR4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account BIAYAKEPERLUANKANTOR5 = _accountService.CreateObject(new Account() { Code = "62020013", Name = "BIAYA KEPERLUAN KANTOR", Group = 2, Level = 5, ParentId = BIAYAOPERASIONALKANTOR4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account BIAYAPEMBULATAN5 = _accountService.CreateObject(new Account() { Code = "62020014", Name = "BIAYA PEMBULATAN", Group = 2, Level = 5, ParentId = BIAYAOPERASIONALKANTOR4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account BYPEMELIHARAANPERBAIKAN3 = _accountService.CreateObject(new Account() { Code = "6203", Name = "BY. PEMELIHARAAN-PERBAIKAN", Group = 2, Level = 3, ParentId = BIAYAADMINISTRASIUMUM2.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account BYPEMELIHARAANPERBAIKAN4 = _accountService.CreateObject(new Account() { Code = "620301", Name = "BY. PEMELIHARAAN-PERBAIKAN", Group = 2, Level = 4, ParentId = BYPEMELIHARAANPERBAIKAN3.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account BPPGEDUNGKANTOR5 = _accountService.CreateObject(new Account() { Code = "62030001", Name = "BPP-GEDUNG KANTOR", Group = 2, Level = 5, ParentId = BYPEMELIHARAANPERBAIKAN4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account BPPGEDUNGGUDANG5 = _accountService.CreateObject(new Account() { Code = "62030002", Name = "BPP-GEDUNG GUDANG", Group = 2, Level = 5, ParentId = BYPEMELIHARAANPERBAIKAN4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account BPPINVENTARISKANTOR5 = _accountService.CreateObject(new Account() { Code = "62030003", Name = "BPP-INVENTARIS KANTOR", Group = 2, Level = 5, ParentId = BYPEMELIHARAANPERBAIKAN4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account BPPKENDARAAND5 = _accountService.CreateObject(new Account() { Code = "62030004", Name = "BPP-KENDARAAN (D)", Group = 2, Level = 5, ParentId = BYPEMELIHARAANPERBAIKAN4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account BPPMESINDANPERALATAN5 = _accountService.CreateObject(new Account() { Code = "62030006", Name = "BPP-MESIN DAN PERALATAN", Group = 2, Level = 5, ParentId = BYPEMELIHARAANPERBAIKAN4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account BIAYASEWA3 = _accountService.CreateObject(new Account() { Code = "6204", Name = "BIAYA SEWA", Group = 2, Level = 3, ParentId = BIAYAADMINISTRASIUMUM2.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account BIAYASEWA4 = _accountService.CreateObject(new Account() { Code = "620401", Name = "BIAYA SEWA", Group = 2, Level = 4, ParentId = BIAYASEWA3.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account BIAYASEWAKANTOR5 = _accountService.CreateObject(new Account() { Code = "62040001", Name = "BIAYA SEWA KANTOR", Group = 2, Level = 5, ParentId = BIAYASEWA4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account BIAYASEWAGUDANG5 = _accountService.CreateObject(new Account() { Code = "62040002", Name = "BIAYA SEWA GUDANG", Group = 2, Level = 5, ParentId = BIAYASEWA4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account BIAYAUTILITASKOMUNIKASI3 = _accountService.CreateObject(new Account() { Code = "6205", Name = "BIAYA UTILITAS-KOMUNIKASI", Group = 2, Level = 3, ParentId = BIAYAADMINISTRASIUMUM2.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account BIAYAUTILITASKOMUNIKASI4 = _accountService.CreateObject(new Account() { Code = "620501", Name = "BIAYA UTILITAS-KOMUNIKASI", Group = 2, Level = 4, ParentId = BIAYAUTILITASKOMUNIKASI3.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account BIAYALISTRIK5 = _accountService.CreateObject(new Account() { Code = "62050001", Name = "BIAYA LISTRIK", Group = 2, Level = 5, ParentId = BIAYAUTILITASKOMUNIKASI4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account BIAYAPAM5 = _accountService.CreateObject(new Account() { Code = "62050002", Name = "BIAYA PAM", Group = 2, Level = 5, ParentId = BIAYAUTILITASKOMUNIKASI4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account BIAYATELEPON5 = _accountService.CreateObject(new Account() { Code = "62050003", Name = "BIAYA TELEPON", Group = 2, Level = 5, ParentId = BIAYAUTILITASKOMUNIKASI4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account BIAYAFAKSIMILI5 = _accountService.CreateObject(new Account() { Code = "62050004", Name = "BIAYA FAKSIMILI", Group = 2, Level = 5, ParentId = BIAYAUTILITASKOMUNIKASI4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account BIAYAHP5 = _accountService.CreateObject(new Account() { Code = "62050005", Name = "BIAYA HP", Group = 2, Level = 5, ParentId = BIAYAUTILITASKOMUNIKASI4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account BIAYAINTERNET5 = _accountService.CreateObject(new Account() { Code = "62050006", Name = "BIAYA INTERNET", Group = 2, Level = 5, ParentId = BIAYAUTILITASKOMUNIKASI4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account BIAYAJASAOPERASIONAL3 = _accountService.CreateObject(new Account() { Code = "6206", Name = "BIAYA JASA OPERASIONAL", Group = 2, Level = 3, ParentId = BIAYAADMINISTRASIUMUM2.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account BIAYAJASAOPERASIONAL4 = _accountService.CreateObject(new Account() { Code = "620601", Name = "BIAYA JASA OPERASIONAL", Group = 2, Level = 4, ParentId = BIAYAJASAOPERASIONAL3.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account BIAYANOTARIS5 = _accountService.CreateObject(new Account() { Code = "62060001", Name = "BIAYA NOTARIS", Group = 2, Level = 5, ParentId = BIAYAJASAOPERASIONAL4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account BIAYAJASAANGKUTAN5 = _accountService.CreateObject(new Account() { Code = "62060002", Name = "BIAYA JASA ANGKUTAN", Group = 2, Level = 5, ParentId = BIAYAJASAOPERASIONAL4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account BIAYAPENASEHATHUKUM5 = _accountService.CreateObject(new Account() { Code = "62060003", Name = "BIAYA PENASEHAT HUKUM", Group = 2, Level = 5, ParentId = BIAYAJASAOPERASIONAL4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account BIAYAKONSULTANTEKNIK5 = _accountService.CreateObject(new Account() { Code = "62060004", Name = "BIAYA KONSULTAN TEKNIK", Group = 2, Level = 5, ParentId = BIAYAJASAOPERASIONAL4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account BIAYAMANAGEMENTFEE5 = _accountService.CreateObject(new Account() { Code = "62060005", Name = "BIAYA MANAGEMENT FEE", Group = 2, Level = 5, ParentId = BIAYAJASAOPERASIONAL4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account BIAYAROYALTI5 = _accountService.CreateObject(new Account() { Code = "62060007", Name = "BIAYA ROYALTI", Group = 2, Level = 5, ParentId = BIAYAJASAOPERASIONAL4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account BIAYAPENILAIAN5 = _accountService.CreateObject(new Account() { Code = "62060008", Name = "BIAYA PENILAIAN", Group = 2, Level = 5, ParentId = BIAYAJASAOPERASIONAL4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account BIAYASEMINARKHURSUS5 = _accountService.CreateObject(new Account() { Code = "62060009", Name = "BIAYA SEMINAR & KHURSUS", Group = 2, Level = 5, ParentId = BIAYAJASAOPERASIONAL4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account BIAYAASURANSI3 = _accountService.CreateObject(new Account() { Code = "6207", Name = "BIAYA ASURANSI", Group = 2, Level = 3, ParentId = BIAYAADMINISTRASIUMUM2.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account BIAYAASURANSI4 = _accountService.CreateObject(new Account() { Code = "620701", Name = "BIAYA ASURANSI", Group = 2, Level = 4, ParentId = BIAYAASURANSI3.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account BIAYAASURANSIKENDARAAN5 = _accountService.CreateObject(new Account() { Code = "62070001", Name = "BIAYA ASURANSI KENDARAAN ", Group = 2, Level = 5, ParentId = BIAYAASURANSI4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account BIAYAASURANSIGEDUNG5 = _accountService.CreateObject(new Account() { Code = "62070002", Name = "BIAYA ASURANSI GEDUNG", Group = 2, Level = 5, ParentId = BIAYAASURANSI4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account BYASURANSIINVENTARISKANTOR5 = _accountService.CreateObject(new Account() { Code = "62070003", Name = "BY. ASURANSI INVENTARIS KANTOR", Group = 2, Level = 5, ParentId = BIAYAASURANSI4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account BYASSMESINPERALATAN5 = _accountService.CreateObject(new Account() { Code = "62070004", Name = "BY. ASS. MESIN-PERALATAN", Group = 2, Level = 5, ParentId = BIAYAASURANSI4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account BYPENYUSUTANAMORTISASI3 = _accountService.CreateObject(new Account() { Code = "6208", Name = "BY. PENYUSUTAN AMORTISASI", Group = 2, Level = 3, ParentId = BIAYAADMINISTRASIUMUM2.Id, IsLegacy = true, IsLeaf = false, LegacyCode = "X2402" }, _accountService);
                Account BYPENYUSUTANAMORTISASI4 = _accountService.CreateObject(new Account() { Code = "620801", Name = "BY. PENYUSUTAN AMORTISASI", Group = 2, Level = 4, ParentId = BYPENYUSUTANAMORTISASI3.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account BIAYAPENYUSUTANBANGUNAN5 = _accountService.CreateObject(new Account() { Code = "62080001", Name = "BIAYA PENYUSUTAN BANGUNAN", Group = 2, Level = 5, ParentId = BYPENYUSUTANAMORTISASI4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account BIAYAPENYUSUTANKENDARAAND5 = _accountService.CreateObject(new Account() { Code = "62080002", Name = "BIAYA PENYUSUTAN KENDARAAN (D)", Group = 2, Level = 5, ParentId = BYPENYUSUTANAMORTISASI4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account BIAYAPENYUSUTANINVENTARISKANTOR5 = _accountService.CreateObject(new Account() { Code = "62080004", Name = "BIAYA PENYUSUTAN INVENTARIS KANTOR", Group = 2, Level = 5, ParentId = BYPENYUSUTANAMORTISASI4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account BIAYAPENYUSUTANMESINPERALATAN5 = _accountService.CreateObject(new Account() { Code = "62080005", Name = "BIAYA PENYUSUTAN MESIN-PERALATAN", Group = 2, Level = 5, ParentId = BYPENYUSUTANAMORTISASI4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account BIAYAPENYUSUTANINSTALASILISTRIK5 = _accountService.CreateObject(new Account() { Code = "62080007", Name = "BIAYA PENYUSUTAN INSTALASI LISTRIK", Group = 2, Level = 5, ParentId = BYPENYUSUTANAMORTISASI4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account BIAYAPENYUSUTANAKTIVALEASING5 = _accountService.CreateObject(new Account() { Code = "62080008", Name = "BIAYA PENYUSUTAN AKTIVA LEASING", Group = 2, Level = 5, ParentId = BYPENYUSUTANAMORTISASI4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account BIAYAKEUANGAN3 = _accountService.CreateObject(new Account() { Code = "6209", Name = "BIAYA KEUANGAN", Group = 2, Level = 3, ParentId = BIAYAADMINISTRASIUMUM2.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account BIAYAKEUANGAN4 = _accountService.CreateObject(new Account() { Code = "620901", Name = "BIAYA KEUANGAN", Group = 2, Level = 4, ParentId = BIAYAKEUANGAN3.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account BIAYABUNGABANK5 = _accountService.CreateObject(new Account() { Code = "62090001", Name = "BIAYA BUNGA BANK", Group = 2, Level = 5, ParentId = BIAYAKEUANGAN4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "X2403" }, _accountService);
                Account BIAYAADMINISTRASIBANK5 = _accountService.CreateObject(new Account() { Code = "62090002", Name = "BIAYA ADMINISTRASI BANK", Group = 2, Level = 5, ParentId = BIAYAKEUANGAN4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account BIAYAPROVISI5 = _accountService.CreateObject(new Account() { Code = "62090003", Name = "BIAYA PROVISI", Group = 2, Level = 5, ParentId = BIAYAKEUANGAN4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account BEBANLAINLAIN2 = _accountService.CreateObject(new Account() { Code = "72", Name = "BEBAN LAIN-LAIN", Group = 2, Level = 2, ParentId = BEBANUSAHA1.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account BEBANLAINLAIN3 = _accountService.CreateObject(new Account() { Code = "7201", Name = "BEBAN LAIN-LAIN", Group = 2, Level = 3, ParentId = BEBANLAINLAIN2.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account BEBANLAINLAIN4 = _accountService.CreateObject(new Account() { Code = "720101", Name = "BEBAN LAIN-LAIN", Group = 2, Level = 4, ParentId = BEBANLAINLAIN3.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account RUGISELISIHKURS5 = _accountService.CreateObject(new Account() { Code = "72010001", Name = "RUGI SELISIH KURS", Group = 2, Level = 5, ParentId = BEBANLAINLAIN4.Id, IsLegacy = true, IsLeaf = true, LegacyCode = "X25" }, _accountService);
                Account BEBANLAINNYA5 = _accountService.CreateObject(new Account() { Code = "72010002", Name = "BEBAN LAINNYA", Group = 2, Level = 5, ParentId = BEBANLAINLAIN4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "X2302004" }, _accountService);
                Account BEBANNONOPERASIONAL3 = _accountService.CreateObject(new Account() { Code = "7202", Name = "BEBAN NON OPERASIONAL", Group = 2, Level = 3, ParentId = BEBANLAINLAIN2.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account BEBANNONOPERASIONAL4 = _accountService.CreateObject(new Account() { Code = "720201", Name = "BEBAN NON OPERASIONAL", Group = 2, Level = 4, ParentId = BEBANNONOPERASIONAL3.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account LABAPENJUALANAKTIVATETAP5 = _accountService.CreateObject(new Account() { Code = "72020001", Name = "LABA PENJUALAN AKTIVA TETAP", Group = 2, Level = 5, ParentId = BEBANNONOPERASIONAL4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account PASSIVA1 = _accountService.CreateObject(new Account() { Code = "3", Name = "PASSIVA", Group = 3, Level = 1, ParentId = null, IsLegacy = true, IsLeaf = false, LegacyCode = "L3" }, _accountService);
                Account KEWAJIBANLANCAR2 = _accountService.CreateObject(new Account() { Code = "21", Name = "KEWAJIBAN LANCAR", Group = 3, Level = 2, ParentId = PASSIVA1.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account HUTANGBANK3 = _accountService.CreateObject(new Account() { Code = "2101", Name = "HUTANG BANK", Group = 3, Level = 3, ParentId = KEWAJIBANLANCAR2.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account HUTANGBANK4 = _accountService.CreateObject(new Account() { Code = "210101", Name = "HUTANG BANK", Group = 3, Level = 4, ParentId = HUTANGBANK3.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account HUTANGBANK5 = _accountService.CreateObject(new Account() { Code = "21010001", Name = "HUTANG BANK", Group = 3, Level = 5, ParentId = HUTANGBANK4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account HUTANGLEASING5 = _accountService.CreateObject(new Account() { Code = "21010002", Name = "HUTANG LEASING", Group = 3, Level = 5, ParentId = HUTANGBANK4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account HUTANGUSAHA3 = _accountService.CreateObject(new Account() { Code = "2102", Name = "HUTANG USAHA", Group = 3, Level = 3, ParentId = KEWAJIBANLANCAR2.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account HUTANGUSAHA4 = _accountService.CreateObject(new Account() { Code = "210201", Name = "HUTANG USAHA", Group = 3, Level = 4, ParentId = HUTANGUSAHA3.Id, IsLegacy = true, IsLeaf = false, LegacyCode = "L3101" }, _accountService);
                Account HUTANGPEMBELIANLOKAL5 = _accountService.CreateObject(new Account() { Code = "21020001", Name = "HUTANG PEMBELIAN LOKAL", Group = 3, Level = 5, ParentId = HUTANGUSAHA4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account HUTANGPEMBELIANIMPORT5 = _accountService.CreateObject(new Account() { Code = "21020002", Name = "HUTANG PEMBELIAN IMPORT", Group = 3, Level = 5, ParentId = HUTANGUSAHA4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account HUTANGPEMBELIANLAINNYA5 = _accountService.CreateObject(new Account() { Code = "21020003", Name = "HUTANG PEMBELIAN LAINNYA", Group = 3, Level = 5, ParentId = HUTANGUSAHA4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account HUTANGLAINLAIN3 = _accountService.CreateObject(new Account() { Code = "2103", Name = "HUTANG LAIN-LAIN", Group = 3, Level = 3, ParentId = KEWAJIBANLANCAR2.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account HUTANGLAINLAIN4 = _accountService.CreateObject(new Account() { Code = "210301", Name = "HUTANG LAIN-LAIN", Group = 3, Level = 4, ParentId = HUTANGLAINLAIN3.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account HUTANGLAINNYA5 = _accountService.CreateObject(new Account() { Code = "21030002", Name = "HUTANG LAINNYA", Group = 3, Level = 5, ParentId = HUTANGLAINLAIN4.Id, IsLegacy = true, IsLeaf = true, LegacyCode = "L3107" }, _accountService);
                Account HUTANGGBCH = _accountService.CreateObject(new Account() { Code = "210302", Name = "HUTANG GBCH", Group = 3, Level = 4, ParentId = HUTANGLAINLAIN3.Id, IsLegacy = true, IsLeaf = true, LegacyCode = "L3102" }, _accountService);
                Account UANGMUKAPENJUALAN3 = _accountService.CreateObject(new Account() { Code = "2104", Name = "UANG MUKA PENJUALAN", Group = 3, Level = 3, ParentId = KEWAJIBANLANCAR2.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account UANGMUKAPENJUALAN4 = _accountService.CreateObject(new Account() { Code = "210401", Name = "UANG MUKA PENJUALAN", Group = 3, Level = 4, ParentId = UANGMUKAPENJUALAN3.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account UANGMUKAPENJUALAN5 = _accountService.CreateObject(new Account() { Code = "21040001", Name = "UANG MUKA PENJUALAN", Group = 3, Level = 5, ParentId = UANGMUKAPENJUALAN4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account HUTANGPAJAK3 = _accountService.CreateObject(new Account() { Code = "2105", Name = "HUTANG PAJAK", Group = 3, Level = 3, ParentId = KEWAJIBANLANCAR2.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account HUTANGPAJAK4 = _accountService.CreateObject(new Account() { Code = "210501", Name = "HUTANG PAJAK", Group = 3, Level = 4, ParentId = HUTANGPAJAK3.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account HUTANGPPHPS215 = _accountService.CreateObject(new Account() { Code = "21050001", Name = "HUTANG PPH PS 21", Group = 3, Level = 5, ParentId = HUTANGPAJAK4.Id, IsLegacy = true, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account HUTANGPPHPS235 = _accountService.CreateObject(new Account() { Code = "21050002", Name = "HUTANG PPH PS 23", Group = 3, Level = 5, ParentId = HUTANGPAJAK4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account HUTANGPPHPS255 = _accountService.CreateObject(new Account() { Code = "21050003", Name = "HUTANG PPH PS 25", Group = 3, Level = 5, ParentId = HUTANGPAJAK4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account HUTANGPPHPS265 = _accountService.CreateObject(new Account() { Code = "21050004", Name = "HUTANG PPH PS 26", Group = 3, Level = 5, ParentId = HUTANGPAJAK4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account HUTANGPPHPS295 = _accountService.CreateObject(new Account() { Code = "21050005", Name = "HUTANG PPH PS 29", Group = 3, Level = 5, ParentId = HUTANGPAJAK4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account HUTANGPPHPS425 = _accountService.CreateObject(new Account() { Code = "21050006", Name = "HUTANG PPH PS 4:2", Group = 3, Level = 5, ParentId = HUTANGPAJAK4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account PPNKELUARAN5 = _accountService.CreateObject(new Account() { Code = "21050007", Name = "PPN KELUARAN", Group = 3, Level = 5, ParentId = HUTANGPAJAK4.Id, IsLegacy = true, IsLeaf = true, LegacyCode = "L3108" }, _accountService);
                Account BIAYAYMHDIBAYAR3 = _accountService.CreateObject(new Account() { Code = "2106", Name = "BIAYA YMH DIBAYAR", Group = 3, Level = 3, ParentId = KEWAJIBANLANCAR2.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account BIAYAYMHDIBAYAR4 = _accountService.CreateObject(new Account() { Code = "210601", Name = "BIAYA YMH DIBAYAR", Group = 3, Level = 4, ParentId = BIAYAYMHDIBAYAR3.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account BYGAJIYMHDIBAYAR5 = _accountService.CreateObject(new Account() { Code = "21060001", Name = "BY. GAJI YMH DIBAYAR", Group = 3, Level = 5, ParentId = BIAYAYMHDIBAYAR4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account BUNGABANKYMHDIBAYAR5 = _accountService.CreateObject(new Account() { Code = "21060002", Name = "BUNGA BANK YMH DIBAYAR", Group = 3, Level = 5, ParentId = BIAYAYMHDIBAYAR4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account BYUTILITASYMHDIBAYAR5 = _accountService.CreateObject(new Account() { Code = "21060003", Name = "BY. UTILITAS YMH DIBAYAR", Group = 3, Level = 5, ParentId = BIAYAYMHDIBAYAR4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account BYKOMUNIKASIYMHDIBAYAR5 = _accountService.CreateObject(new Account() { Code = "21060004", Name = "BY. KOMUNIKASI YMH DIBAYAR", Group = 3, Level = 5, ParentId = BIAYAYMHDIBAYAR4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account BYLAINNYAYMHDIBAYAR5 = _accountService.CreateObject(new Account() { Code = "21060005", Name = "BY. LAINNYA YMH DIBAYAR", Group = 3, Level = 5, ParentId = BIAYAYMHDIBAYAR4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account BARANGYMHDICLEARANCE3 = _accountService.CreateObject(new Account() { Code = "2107", Name = "BARANG YMH DICLEARANCE", Group = 3, Level = 3, ParentId = KEWAJIBANLANCAR2.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account BARANGYMHDICLEARANCE4 = _accountService.CreateObject(new Account() { Code = "210701", Name = "BARANG YMH DICLEARANCE", Group = 3, Level = 4, ParentId = BARANGYMHDICLEARANCE3.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account BARANGYMHDICLEARANCE5 = _accountService.CreateObject(new Account() { Code = "21070001", Name = "BARANG YMH DICLEARANCE", Group = 3, Level = 5, ParentId = BARANGYMHDICLEARANCE4.Id, IsLegacy = true, IsLeaf = true, LegacyCode = "L3103" }, _accountService);                
                Account KEWAJIBANJANGKAPANJANG2 = _accountService.CreateObject(new Account() { Code = "22", Name = "KEWAJIBAN JANGKA PANJANG (KWJP)", Group = 3, Level = 2, ParentId = PASSIVA1.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account KWJPHUTANGBANK3 = _accountService.CreateObject(new Account() { Code = "2201", Name = "KWJP HUTANG BANK", Group = 3, Level = 3, ParentId = KEWAJIBANJANGKAPANJANG2.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account KWJPHUTANGBANK4 = _accountService.CreateObject(new Account() { Code = "220101", Name = "KWJP HUTANG BANK", Group = 3, Level = 4, ParentId = KWJPHUTANGBANK3.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account KWJPHUTANGBANK5 = _accountService.CreateObject(new Account() { Code = "22010001", Name = "KWJP HUTANG BANK", Group = 3, Level = 5, ParentId = KWJPHUTANGBANK4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "X2302001" }, _accountService);
                Account KWJPHUTANGLEASING5 = _accountService.CreateObject(new Account() { Code = "22010002", Name = "HUTANG LEASING", Group = 3, Level = 5, ParentId = KWJPHUTANGBANK4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account HUTANGJKPANJANGLAINNYA3 = _accountService.CreateObject(new Account() { Code = "2202", Name = "HUTANG JK. PANJANG LAINNYA", Group = 3, Level = 3, ParentId = KEWAJIBANJANGKAPANJANG2.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account HUTANGJKPANJANGLAINNYA4 = _accountService.CreateObject(new Account() { Code = "220201", Name = "HUTANG JK. PANJANG LAINNYA", Group = 3, Level = 4, ParentId = HUTANGJKPANJANGLAINNYA3.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account LABASALELEASEYGDITAGIHKAN5 = _accountService.CreateObject(new Account() { Code = "22020002", Name = "LABA SALE & LEASE YG DITAGIHKAN", Group = 3, Level = 5, ParentId = HUTANGJKPANJANGLAINNYA4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account PAJAKPENGHASILAN2 = _accountService.CreateObject(new Account() { Code = "91", Name = "PAJAK PENGHASILAN ", Group = 3, Level = 2, ParentId = PASSIVA1.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account PAJAKPENGHASILAN3 = _accountService.CreateObject(new Account() { Code = "9101", Name = "PAJAK PENGHASILAN ", Group = 3, Level = 3, ParentId = PAJAKPENGHASILAN2.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account PAJAKPENGHASILAN4 = _accountService.CreateObject(new Account() { Code = "910101", Name = "PAJAK PENGHASILAN ", Group = 3, Level = 4, ParentId = PAJAKPENGHASILAN3.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account PAJAKPENGHASILAN5 = _accountService.CreateObject(new Account() { Code = "91010001", Name = "PAJAK PENGHASILAN ", Group = 3, Level = 5, ParentId = PAJAKPENGHASILAN4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account PPHBADAN3 = _accountService.CreateObject(new Account() { Code = "9102", Name = "PPH BADAN", Group = 3, Level = 3, ParentId = PAJAKPENGHASILAN2.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account PPHBADAN4 = _accountService.CreateObject(new Account() { Code = "910201", Name = "PPH BADAN", Group = 3, Level = 4, ParentId = PPHBADAN3.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account BIAYAPAJAKTANGGUHAN5 = _accountService.CreateObject(new Account() { Code = "91020001", Name = "BIAYA PAJAK TANGGUHAN", Group = 3, Level = 5, ParentId = PPHBADAN4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account MODAL1 = _accountService.CreateObject(new Account() { Code = "4", Name = "MODAL", Group = 4, Level = 1, ParentId = null, IsLegacy = true, IsLeaf = false, LegacyCode = "E4" }, _accountService);
                Account MODAL2 = _accountService.CreateObject(new Account() { Code = "31", Name = "MODAL", Group = 4, Level = 2, ParentId = MODAL1.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account MODALDISETOR3 = _accountService.CreateObject(new Account() { Code = "3101", Name = "MODAL DISETOR", Group = 4, Level = 3, ParentId = MODAL2.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account MODALDISETOR4 = _accountService.CreateObject(new Account() { Code = "310101", Name = "MODAL DISETOR", Group = 4, Level = 4, ParentId = MODALDISETOR3.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account MODALDISETOR5 = _accountService.CreateObject(new Account() { Code = "31010001", Name = "MODAL DISETOR", Group = 4, Level = 5, ParentId = MODALDISETOR4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account LABARUGIDITAHAN3 = _accountService.CreateObject(new Account() { Code = "3102", Name = "LABA/RUGI DITAHAN", Group = 4, Level = 3, ParentId = MODAL2.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account LABARUGIDITAHAN4 = _accountService.CreateObject(new Account() { Code = "310201", Name = "LABA/RUGI DITAHAN", Group = 4, Level = 4, ParentId = LABARUGIDITAHAN3.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account LABARUGIDITAHAN5 = _accountService.CreateObject(new Account() { Code = "31020001", Name = "LABA/RUGI DITAHAN", Group = 4, Level = 5, ParentId = LABARUGIDITAHAN4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account LABARUGITAHUNBERJALAN3 = _accountService.CreateObject(new Account() { Code = "3103", Name = "LABA/RUGI TAHUN BERJALAN", Group = 4, Level = 3, ParentId = MODAL2.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account LABARUGITAHUNBERJALAN4 = _accountService.CreateObject(new Account() { Code = "310301", Name = "LABA/RUGI TAHUN BERJALAN", Group = 4, Level = 4, ParentId = LABARUGITAHUNBERJALAN3.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account LABARUGITAHUNBERJALAN5 = _accountService.CreateObject(new Account() { Code = "31030001", Name = "LABA/RUGI TAHUN BERJALAN", Group = 4, Level = 5, ParentId = LABARUGITAHUNBERJALAN4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account LABARUGIBULANBERJALAN3 = _accountService.CreateObject(new Account() { Code = "3104", Name = "LABA/RUGI BULAN BERJALAN", Group = 4, Level = 3, ParentId = MODAL2.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account LABARUGIBULANBERJALAN4 = _accountService.CreateObject(new Account() { Code = "310401", Name = "LABA/RUGI BULAN BERJALAN", Group = 4, Level = 4, ParentId = LABARUGIBULANBERJALAN3.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account LABARUGIBULANBERJALAN5 = _accountService.CreateObject(new Account() { Code = "31040001", Name = "LABA/RUGI BULAN BERJALAN", Group = 4, Level = 5, ParentId = LABARUGIBULANBERJALAN4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account DIVIDEN3 = _accountService.CreateObject(new Account() { Code = "3105", Name = "DIVIDEN", Group = 4, Level = 3, ParentId = MODAL2.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account DIVIDEN4 = _accountService.CreateObject(new Account() { Code = "310501", Name = "DIVIDEN", Group = 4, Level = 4, ParentId = DIVIDEN3.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account DIVIDEN5 = _accountService.CreateObject(new Account() { Code = "31050001", Name = "DIVIDEN", Group = 4, Level = 5, ParentId = DIVIDEN4.Id, IsLegacy = true, IsLeaf = true, LegacyCode = "X2405" }, _accountService);
                Account KOREKSILRDITAHAN3 = _accountService.CreateObject(new Account() { Code = "3106", Name = "KOREKSI L/R DITAHAN", Group = 4, Level = 3, ParentId = MODAL2.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account KOREKSILRDITAHAN4 = _accountService.CreateObject(new Account() { Code = "310601", Name = "KOREKSI L/R DITAHAN", Group = 4, Level = 4, ParentId = KOREKSILRDITAHAN3.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account KOREKSILRDITAHAN5 = _accountService.CreateObject(new Account() { Code = "31060001", Name = "KOREKSI L/R DITAHAN", Group = 4, Level = 5, ParentId = KOREKSILRDITAHAN4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account KOREKSILRTAHUNBERJALAN3 = _accountService.CreateObject(new Account() { Code = "3107", Name = "KOREKSI L/R TAHUN BERJALAN", Group = 4, Level = 3, ParentId = MODAL2.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account KOREKSILRTAHUNBERJALAN4 = _accountService.CreateObject(new Account() { Code = "310701", Name = "KOREKSI L/R TAHUN BERJALAN", Group = 4, Level = 4, ParentId = KOREKSILRTAHUNBERJALAN3.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account KOREKSILRTAHUNBERJALAN5 = _accountService.CreateObject(new Account() { Code = "31070001", Name = "KOREKSI L/R TAHUN BERJALAN", Group = 4, Level = 5, ParentId = KOREKSILRTAHUNBERJALAN4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account KOREKSILRBULANBERJALAN3 = _accountService.CreateObject(new Account() { Code = "3108", Name = "KOREKSI L/R BULAN BERJALAN", Group = 4, Level = 3, ParentId = MODAL2.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account KOREKSILRBULANBERJALAN4 = _accountService.CreateObject(new Account() { Code = "310801", Name = "KOREKSI L/R BULAN BERJALAN", Group = 4, Level = 4, ParentId = KOREKSILRBULANBERJALAN3.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account KOREKSILRBULANBERJALAN5 = _accountService.CreateObject(new Account() { Code = "31080001", Name = "KOREKSI L/R BULAN BERJALAN", Group = 4, Level = 5, ParentId = KOREKSILRBULANBERJALAN4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account PENYESUAIANMODAL3 = _accountService.CreateObject(new Account() { Code = "3109", Name = "PENYESUAIAN MODAL", Group = 4, Level = 3, ParentId = MODAL2.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account PENYESUAIANMODAL4 = _accountService.CreateObject(new Account() { Code = "310901", Name = "PENYESUAIAN MODAL", Group = 4, Level = 4, ParentId = PENYESUAIANMODAL3.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account PENYESUAIANMODAL5 = _accountService.CreateObject(new Account() { Code = "31090001", Name = "PENYESUAIAN MODAL", Group = 4, Level = 5, ParentId = PENYESUAIANMODAL4.Id, IsLegacy = true, IsLeaf = true, LegacyCode = "E4101" }, _accountService);
                Account PENDAPATAN1 = _accountService.CreateObject(new Account() { Code = "5", Name = "PENDAPATAN", Group = 5, Level = 1, ParentId = null, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account PENDAPATAN2 = _accountService.CreateObject(new Account() { Code = "41", Name = "PENDAPATAN", Group = 5, Level = 2, ParentId = PENDAPATAN1.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account PENDAPATANPENJUALAN3 = _accountService.CreateObject(new Account() { Code = "4101", Name = "PENDAPATAN PENJUALAN ", Group = 5, Level = 3, ParentId = PENDAPATAN2.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account PENDAPATANPENJUALAN4 = _accountService.CreateObject(new Account() { Code = "410101", Name = "PENDAPATAN PENJUALAN ", Group = 5, Level = 4, ParentId = PENDAPATANPENJUALAN3.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account PENDAPATANPENJUALAN5 = _accountService.CreateObject(new Account() { Code = "411010001", Name = "PENDAPATAN PENJUALAN ", Group = 5, Level = 5, ParentId = PENDAPATANPENJUALAN4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "R5" }, _accountService);
                Account POTONGANPENJUALAN3 = _accountService.CreateObject(new Account() { Code = "4102", Name = "POTONGAN PENJUALAN", Group = 5, Level = 3, ParentId = PENDAPATAN2.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account POTONGANPENJUALAN4 = _accountService.CreateObject(new Account() { Code = "410201", Name = "POTONGAN PENJUALAN", Group = 5, Level = 4, ParentId = POTONGANPENJUALAN3.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account POTONGANPENJUALAN5 = _accountService.CreateObject(new Account() { Code = "41020001", Name = "POTONGAN PENJUALAN ", Group = 5, Level = 5, ParentId = POTONGANPENJUALAN4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account RETURPENJUALAN3 = _accountService.CreateObject(new Account() { Code = "4103", Name = "RETUR PENJUALAN ", Group = 5, Level = 3, ParentId = PENDAPATAN2.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account RETURPENJUALAN4 = _accountService.CreateObject(new Account() { Code = "410301", Name = "RETUR PENJUALAN ", Group = 5, Level = 4, ParentId = RETURPENJUALAN3.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account RETURPENJUALAN5 = _accountService.CreateObject(new Account() { Code = "41030001", Name = "RETUR PENJUALAN ", Group = 5, Level = 5, ParentId = RETURPENJUALAN4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account HARGAPOKOK2 = _accountService.CreateObject(new Account() { Code = "51", Name = "HARGA POKOK", Group = 2, Level = 2, ParentId = BEBANUSAHA1.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account HARGAPOKOKPENJUALAN3 = _accountService.CreateObject(new Account() { Code = "5101", Name = "HARGA POKOK PENJUALAN", Group = 2, Level = 3, ParentId = HARGAPOKOK2.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account HARGAPOKOKPENJUALAN4 = _accountService.CreateObject(new Account() { Code = "510101", Name = "HARGA POKOK PENJUALAN", Group = 2, Level = 4, ParentId = HARGAPOKOKPENJUALAN3.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account HARGAPOKOKPENJUALAN5 = _accountService.CreateObject(new Account() { Code = "51010001", Name = "HARGA POKOK PENJUALAN", Group = 2, Level = 5, ParentId = HARGAPOKOKPENJUALAN4.Id, IsLegacy = true, IsLeaf = true, LegacyCode = "X21" }, _accountService);
                Account POTONGANPEMBELIAN5 = _accountService.CreateObject(new Account() { Code = "51010002", Name = "POTONGAN PEMBELIAN", Group = 2, Level = 5, ParentId = HARGAPOKOKPENJUALAN4.Id, IsLegacy = true, IsLeaf = true, LegacyCode = "X2302002" }, _accountService);
                Account BIAYAOVERHEADPABRIK3 = _accountService.CreateObject(new Account() { Code = "5102", Name = "BIAYA OVERHEAD PABRIK", Group = 2, Level = 3, ParentId = HARGAPOKOK2.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account BIAYAOVERHEADPABRIK4 = _accountService.CreateObject(new Account() { Code = "510201", Name = "BIAYA OVERHEAD PABRIK", Group = 2, Level = 4, ParentId = BIAYAOVERHEADPABRIK3.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account BIAYAOVERHEADPABRIK5 = _accountService.CreateObject(new Account() { Code = "51020001", Name = "BIAYA OVERHEAD PABRIK", Group = 2, Level = 5, ParentId = BIAYAOVERHEADPABRIK4.Id, IsLegacy = true, IsLeaf = true, LegacyCode = "X2301" }, _accountService);
                Account PENDAPATANLAINLAIN2 = _accountService.CreateObject(new Account() { Code = "71", Name = "PENDAPATAN LAIN-LAIN", Group = 5, Level = 2, ParentId = PENDAPATAN1.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account PENDAPATANBUNGA3 = _accountService.CreateObject(new Account() { Code = "7101", Name = "PENDAPATAN BUNGA", Group = 5, Level = 3, ParentId = PENDAPATANLAINLAIN2.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account PENDAPATANBUNGA4 = _accountService.CreateObject(new Account() { Code = "710101", Name = "PENDAPATAN BUNGA", Group = 5, Level = 4, ParentId = PENDAPATANBUNGA3.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account PENDAPATANJASAGIRO5 = _accountService.CreateObject(new Account() { Code = "71010001", Name = "PENDAPATAN JASA GIRO", Group = 5, Level = 5, ParentId = PENDAPATANBUNGA4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account PENDAPATANBUNGADEPOSITO5 = _accountService.CreateObject(new Account() { Code = "71010002", Name = "PENDAPATAN BUNGA DEPOSITO", Group = 5, Level = 5, ParentId = PENDAPATANBUNGA4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account PENDAPATANLAINNYA3 = _accountService.CreateObject(new Account() { Code = "7102", Name = "PENDAPATAN LAINNYA", Group = 5, Level = 3, ParentId = PENDAPATANLAINLAIN2.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account PENDAPATANLAINNYA4 = _accountService.CreateObject(new Account() { Code = "710201", Name = "PENDAPATAN LAINNYA", Group = 5, Level = 4, ParentId = PENDAPATANLAINNYA3.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account PENDAPATANSELISIHKURS5 = _accountService.CreateObject(new Account() { Code = "71020001", Name = "PENDAPATAN SELISIH KURS", Group = 5, Level = 5, ParentId = PENDAPATANLAINNYA4.Id, IsLegacy = true, IsLeaf = true, LegacyCode = "E42" }, _accountService);
                Account PENDAPATANLAINNYA5 = _accountService.CreateObject(new Account() { Code = "71020002", Name = "PENDAPATAN LAINNYA", Group = 5, Level = 5, ParentId = PENDAPATANLAINNYA4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account PENDAPATANNONOPERASIONAL3 = _accountService.CreateObject(new Account() { Code = "7103", Name = "PENDAPATAN NON OPERASIONAL", Group = 5, Level = 3, ParentId = PENDAPATANLAINLAIN2.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account PENDAPATANNONOPERASIONAL4 = _accountService.CreateObject(new Account() { Code = "710301", Name = "PENDAPATAN NON OPERASIONAL (POP)", Group = 5, Level = 4, ParentId = PENDAPATANNONOPERASIONAL3.Id, IsLegacy = false, IsLeaf = false, LegacyCode = "" }, _accountService);
                Account LABAPENJUALANAKTIVATETAP5POP = _accountService.CreateObject(new Account() { Code = "71030001", Name = "LABA PENJUALAN AKTIVA TETAP (POP)", Group = 5, Level = 5, ParentId = PENDAPATANNONOPERASIONAL4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
                Account LABASALELEASEBACK5 = _accountService.CreateObject(new Account() { Code = "71030002", Name = "LABA SALE & LEASE BACK", Group = 5, Level = 5, ParentId = PENDAPATANNONOPERASIONAL4.Id, IsLegacy = false, IsLeaf = true, LegacyCode = "" }, _accountService);
            }

            if (!_currencyService.GetAll().Any())
            {
                currencyIDR = new Currency();
                currencyIDR.IsBase = true;
                currencyIDR.Name = "IDR";
                currencyIDR = _currencyService.CreateObject(currencyIDR, _accountService);
            }

            
            CreateUserMenus();
            CreateSysAdmin();     
   
        }

        public void CreateUserMenus()
        {
            _userMenuService.CreateObject(Constant.MenuName.Contact, Constant.MenuGroupName.Master);
            _userMenuService.CreateObject(Constant.MenuName.ItemType, Constant.MenuGroupName.Master);
            _userMenuService.CreateObject(Constant.MenuName.UoM, Constant.MenuGroupName.Master);
            _userMenuService.CreateObject(Constant.MenuName.Machine, Constant.MenuGroupName.Master);
            _userMenuService.CreateObject(Constant.MenuName.RollerType, Constant.MenuGroupName.Master);

            _userMenuService.CreateObject(Constant.MenuName.CashBank, Constant.MenuGroupName.Master);
            _userMenuService.CreateObject(Constant.MenuName.CashMutation, Constant.MenuGroupName.Master);
            _userMenuService.CreateObject(Constant.MenuName.CashBankAdjustment, Constant.MenuGroupName.Master);
            _userMenuService.CreateObject(Constant.MenuName.CashBankMutation, Constant.MenuGroupName.Master);
            _userMenuService.CreateObject(Constant.MenuName.PaymentRequest, Constant.MenuGroupName.Master);
            _userMenuService.CreateObject(Constant.MenuName.Memorial, Constant.MenuGroupName.Master);

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