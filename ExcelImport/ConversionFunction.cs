using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Data.Context;
using Core.Constants;
using Core.DomainModel;
using Core.Interface.Service;
using Service.Service;
using Data.Repository;
using Validation.Validation;
using System.Text.RegularExpressions;
using System.Globalization;

namespace ExcelImport
{
    // connectionString info at http://connectionstrings.com/excel-2007
    public class ConversionFunction
    {
        //public ICompanyInfoService _companyInfoService = new CompanyInfoService(new CompanyInfoRepository(), new CompanyInfoValidator());
        public IItemTypeService _itemTypeService = new ItemTypeService(new ItemTypeRepository(), new ItemTypeValidator());
        public IUoMService _uomService = new UoMService(new UoMRepository(), new UoMValidator());
        public IRollerTypeService _rollerTypeService = new RollerTypeService(new RollerTypeRepository(), new RollerTypeValidator());
        public IMachineService _machineService = new MachineService(new MachineRepository(), new MachineValidator());
        public IWarehouseService _warehouseService = new WarehouseService(new WarehouseRepository(), new WarehouseValidator());
        public IWarehouseItemService _warehouseItemService = new WarehouseItemService(new WarehouseItemRepository(), new WarehouseItemValidator());
        public IItemService _itemService = new ItemService(new ItemRepository(), new ItemValidator());
        public IAccountService _accountService = new AccountService(new AccountRepository(), new AccountValidator());
        public IGeneralLedgerJournalService _generalLedgerJournalService = new GeneralLedgerJournalService(new GeneralLedgerJournalRepository(), new GeneralLedgerJournalValidator());
        public IClosingService _closingService = new ClosingService(new ClosingRepository(), new ClosingValidator());
        public ICurrencyService _currencyService = new CurrencyService(new CurrencyRepository(), new CurrencyValidator());
        public IExchangeRateService _exchangeRateService = new ExchangeRateService(new ExchangeRateRepository(), new ExchangeRateValidator());
        public IContactService _contactService = new ContactService(new ContactRepository(), new ContactValidator());
        public IPriceMutationService _priceMutationService = new PriceMutationService(new PriceMutationRepository(), new PriceMutationValidator());
        public IStockAdjustmentService _stockAdjustmentService = new StockAdjustmentService(new StockAdjustmentRepository(), new StockAdjustmentValidator());
        public IStockAdjustmentDetailService _stockAdjustmentDetailService = new StockAdjustmentDetailService(new StockAdjustmentDetailRepository(), new StockAdjustmentDetailValidator());
        public IStockMutationService _stockMutationService = new StockMutationService(new StockMutationRepository(), new StockMutationValidator());
        public IBlanketService _blanketService = new BlanketService(new BlanketRepository(), new BlanketValidator());
        public ICoreBuilderService _coreBuilderService = new CoreBuilderService(new CoreBuilderRepository(), new CoreBuilderValidator());
        public IRollerBuilderService _rollerBuilderService = new RollerBuilderService(new RollerBuilderRepository(), new RollerBuilderValidator());
        public ISalesInvoiceMigrationService _salesInvoiceMigrationService = new SalesInvoiceMigrationService(new SalesInvoiceMigrationRepository());
        public IPurchaseInvoiceMigrationService _purchaseInvoiceMigrationService = new PurchaseInvoiceMigrationService(new PurchaseInvoiceMigrationRepository());
        public IGLNonBaseCurrencyService _gLNonBaseCurrencyService = new GLNonBaseCurrencyService(new GLNonBaseCurrencyRepository(), new GLNonBaseCurrencyValidator());
        public IReceivableService _receivableService = new ReceivableService(new ReceivableRepository(), new ReceivableValidator());
        public IPayableService _payableService = new PayableService(new PayableRepository(), new PayableValidator());

        public ConversionFunction()
        {
            //_companyInfoService = new CompanyInfoService(new CompanyInfoRepository(), new CompanyInfoValidator());
            
        }

        public void Log(dynamic model, string sheet, int row)
        {
            foreach (var error in model.Errors)
            {
                Console.WriteLine(model.GetType().Name + "[ID:" + model.Id + ", SHEET:"+ sheet + ", ROW:" + row + "] : " + error.Key + " " + error.Value);
            }
        }

        public string GetValidNumber(string str)
        {
            Regex re = new Regex(@"([\d|\.]+)");
            Match result = re.Match(str);
            string ret = result.Groups[1].Value;
            if (ret == null || ret.Trim() == "") ret = "0";
            return ret;
        }

        public void DoItemType(DataRow row, int rowidx, string sheetname)
        {
            var name = row[0].ToString();
            var desc = row[1].ToString();
            var obj = _itemTypeService.GetQueryable().Where(x => x.Name == name && !x.IsDeleted).FirstOrDefault();
            if (obj == null)
            {
                obj = _itemTypeService.CreateObject(name, desc);
                Log(obj, sheetname, rowidx + 2);
            }
        }

        public void DoUoM(DataRow row, int rowidx, string sheetname)
        {
            var name = row[1].ToString();
            var obj = _uomService.GetQueryable().Where(x => x.Name == name && !x.IsDeleted).FirstOrDefault();
            if (obj == null)
            {
                obj = _uomService.CreateObject(name);
                Log(obj, sheetname, rowidx + 2);
            }
        }

        public void DoRollerType(DataRow row, int rowidx, string sheetname)
        {
            var name = row[1].ToString();
            var desc = row[2].ToString();
            var obj = _rollerTypeService.GetQueryable().Where(x => x.Name == name && !x.IsDeleted).FirstOrDefault();
            if (obj == null)
            {
                obj = _rollerTypeService.CreateObject(name, desc);
                Log(obj, sheetname, rowidx + 2);
            }
        }

        public void DoMachine(DataRow row, int rowidx, string sheetname)
        {
            var name = row[0].ToString();
            var desc = row[1].ToString();
            var code = name.Replace(" ", "").Replace("/", "").Replace("-", "").Replace("(", "").Replace(")", "");
            var obj = _machineService.GetQueryable().Where(x => x.Code == code && !x.IsDeleted).FirstOrDefault();
            if (obj == null)
            {
                obj = new Machine()
                {
                    Code = code,
                    Name = name,
                    Description = desc,
                };
                obj = _machineService.CreateObject(obj);
                Log(obj, sheetname, rowidx + 2);
            }
        }

        public void DoWarehouse(DataRow row, int rowidx, string sheetname)
        {
            var code = row[0].ToString();
            var name = row[1].ToString();
            var desc = row[2].ToString();
            var obj = _warehouseService.GetQueryable().Where(x => x.Code == code && !x.IsDeleted).FirstOrDefault();
            if (obj == null)
            {
                obj = new Warehouse()
                {
                    Code = code,
                    Name = name,
                    Description = desc,
                };
                obj = _warehouseService.CreateObject(obj, _warehouseItemService, _itemService);
                Log(obj, sheetname, rowidx + 2);
            }
        }

        public void DoCurrency(DataRow row, int rowidx, string sheetname)
        {
            var name = row[0].ToString();
            var isbase = Convert.ToBoolean(row[1].ToString()); //bool.Parse(row[1].ToString());
            var rate = decimal.Parse(row[2].ToString());
            var obj = _currencyService.GetQueryable().Where(x => x.Name == name && !x.IsDeleted).FirstOrDefault();
            if (obj == null)
            {
                obj = new Currency()
                {
                    Name = name,
                    Description = name,
                    IsBase = isbase,
                };
                obj = _currencyService.CreateObject(obj, _accountService);
                Log(obj, sheetname, rowidx + 2);
            }
            var date = new DateTime(2014, 12, 1);
            var obj2 = _exchangeRateService.GetQueryable().Where(x => x.CurrencyId == obj.Id && x.ExRateDate == date && !x.IsDeleted).FirstOrDefault();
            if (obj2 == null)
            {
                obj2 = new ExchangeRate()
                {
                    CurrencyId = obj.Id,
                    Rate = rate,
                    ExRateDate = date,
                };
                obj2 = _exchangeRateService.CreateObject(obj2);
                Log(obj2, sheetname, rowidx + 2);
            }
        }

        public void DoCustomer(DataRow row, int rowidx, string sheetname)
        {
            var desc = row[0].ToString();
            var name = row[1].ToString();
            var addr = row[2].ToString();
            var defaddr = row[3].ToString();
            var defterm = Convert.ToInt32(GetValidNumber(row[4].ToString())); //int.Parse(row[4].ToString());
            var phone = row[5].ToString();
            var npwp = row[7].ToString();
            var tax = row[8].ToString();
            var email = row[10].ToString();
            var status = row[15].ToString();
            var obj = _contactService.GetQueryable().Where(x => x.Name == name /*&& x.Description == desc && x.NPWP == npwp*/ && !x.IsDeleted).FirstOrDefault();
            if (obj == null)
            {
                obj = new Contact()
                {
                    Name = name, //(desc.Length > name.Length) ? desc : name,
                    Description = desc, //(desc.Length > name.Length) ? name : desc,
                    Address = (addr == null || addr.Trim() == "") ? "-" : addr,
                    DeliveryAddress = defaddr,
                    DefaultPaymentTerm = defterm,
                    ContactNo = (phone == null || phone.Trim() == "") ? "-" : phone,
                    NPWP = npwp,
                    TaxCode = (tax == null || tax.Trim() == "") ? "01" : tax,
                    IsTaxable = (tax != null && tax.Trim() != ""),
                    Email = email,
                    ContactType = "CUSTOMER", //status,
                    
                };
                obj = _contactService.CreateObject(obj);
                Log(obj, sheetname + "Create", rowidx + 2);
            }
            else
            {
                obj.ContactType = "CUSTOMER";
                obj.Errors = new Dictionary<string, string>();
                obj = _contactService.UpdateObject(obj);
                Log(obj, sheetname + "Update", rowidx + 2);
            }
        }

        public void DoSupplier(DataRow row, int rowidx, string sheetname)
        {
            var desc = row[8].ToString();
            var name = row[1].ToString();
            var addr = row[2].ToString();
            //var defaddr = row[3].ToString();
            //var defterm = Convert.ToInt32(GetValidNumber(row[4].ToString())); //int.Parse(row[4].ToString());
            var phone = row[3].ToString();
            //var npwp = row[7].ToString();
            var email = row[4].ToString();
            var status = row[5].ToString();
            var pic = row[6].ToString();
            var picno = row[7].ToString();
            var obj = _contactService.GetQueryable().Where(x => x.Name == name /*&& x.Description == desc*/ && !x.IsDeleted).FirstOrDefault();
            if (obj == null)
            {
                obj = new Contact()
                {
                    Name = name, //(desc.Length > name.Length) ? desc : name,
                    Description = desc, //(desc.Length > name.Length) ? name : desc,
                    Address = (addr == null || addr.Trim() == "") ? "-" : addr,
                    //DeliveryAddress = defaddr,
                    //DefaultPaymentTerm = defterm,
                    ContactNo = (phone == null || phone.Trim() == "") ? "-" : phone,
                    PIC = pic,
                    PICContactNo = picno,
                    //NPWP = npwp,
                    TaxCode = "01",
                    IsTaxable = true,
                    Email = email,
                    ContactType = "SUPPLIER", //status
                };
                obj = _contactService.CreateObject(obj);
                Log(obj, sheetname + "Create", rowidx + 2);
            }
            else
            {
                obj.ContactType = "SUPPLIER";
                obj.Errors = new Dictionary<string, string>();
                obj = _contactService.UpdateObject(obj);
                Log(obj, sheetname + "Update", rowidx + 2);
            }
        }

        public void DoItemGudangSerpong(DataRow row, int rowidx, string sheetname)
        {
            var sku = row[0].ToString();
            var typename = row[1].ToString();
            var name = row[3].ToString();
            var desc = row[5].ToString();
            var tradable = row[6].ToString();
            var uomname = row[7].ToString();
            var minqty = decimal.Parse(GetValidNumber(row[8].ToString()));
            //var maxqty = decimal.Parse(row[9].ToString());
            var qty = decimal.Parse(GetValidNumber(row[10].ToString()));
            var price = decimal.Parse(GetValidNumber(row[12].ToString()));
            var currname = row[13].ToString();
            var itemtype = _itemTypeService.GetQueryable().Where(x => x.Name == typename && !x.IsDeleted).FirstOrDefault();
            var uom = _uomService.GetQueryable().Where(x => x.Name.Replace(" ", "") == uomname.Replace(" ", "") && !x.IsDeleted).FirstOrDefault();
            //var defuom = _uomService.GetQueryable().Where(x => !x.IsDeleted).FirstOrDefault();
            var curr = _currencyService.GetQueryable().Where(x => x.Name == currname && !x.IsDeleted).FirstOrDefault();
            var defcurr = _currencyService.GetQueryable().Where(x => !x.IsDeleted).FirstOrDefault();
            var warehouse = _warehouseService.GetQueryable().Where(x => x.Code == "A1").FirstOrDefault();
            var date = new DateTime(2014, 12, 1);

            var obj = _itemService.GetQueryable().Where(x => x.Sku == sku && !x.IsDeleted).FirstOrDefault();
            if (obj == null)
            {
                obj = new Item()
                {
                    Sku = sku,
                    Name = name,
                    Description = desc,
                    IsTradeable = (tradable == "Yes") ? true : false,
                    UoMId = (uom != null) ? uom.Id : 0, //defuom.Id,
                    ItemTypeId = (itemtype != null) ? itemtype.Id : 0,
                    MinimumQuantity = (int)minqty,
                    //Quantity = qty,
                    SellingPrice = price,
                    CurrencyId = (curr != null) ? curr.Id : defcurr.Id,
                };
                obj = _itemService.CreateObject(obj, _uomService, _itemTypeService, _warehouseItemService, _warehouseService, _priceMutationService);
                Log(obj, sheetname, rowidx + 2);
                if (qty != 0 && !obj.Errors.Any())
                {
                    var obj2 = new StockAdjustment()
                    {
                        WarehouseId = warehouse.Id,
                        AdjustmentDate = date,
                    };
                    obj2 = _stockAdjustmentService.CreateObject(obj2, _warehouseService);
                    Log(obj2, sheetname, rowidx + 2);
                    if (!obj2.Errors.Any())
                    {
                        var obj2det = new StockAdjustmentDetail()
                        {
                            StockAdjustmentId = obj2.Id,
                            ItemId = obj.Id,
                            Quantity = qty,
                            //Price = avgprice,
                        };
                        obj2det = _stockAdjustmentDetailService.CreateObject(obj2det, _stockAdjustmentService, _itemService, _warehouseItemService);
                        Log(obj2det, sheetname, rowidx + 2);
                        if (!obj2det.Errors.Any())
                        {
                            _stockAdjustmentService.ConfirmObject(obj2, date, _stockAdjustmentDetailService, _stockMutationService, _itemService, _blanketService, _warehouseItemService, _accountService, _generalLedgerJournalService, _closingService);
                        }
                        else
                        {
                            _stockAdjustmentService.DeleteObject(obj2.Id);
                        }
                    }
                }
            }
        }

        public void DoItemGudangSby(DataRow row, int rowidx, string sheetname)
        {
            var sku = row[0].ToString();
            var typename = row[1].ToString();
            var name = row[3].ToString();
            var desc = row[5].ToString();
            var tradable = row[6].ToString();
            var uomname = row[7].ToString();
            var minqty = decimal.Parse(GetValidNumber(row[8].ToString()));
            //var maxqty = decimal.Parse(row[9].ToString());
            var qty = decimal.Parse(GetValidNumber(row[10].ToString()));
            var price = decimal.Parse(GetValidNumber(row[12].ToString()));
            var currname = row[13].ToString();
            var itemtype = _itemTypeService.GetQueryable().Where(x => x.Name == typename && !x.IsDeleted).FirstOrDefault();
            var uom = _uomService.GetQueryable().Where(x => x.Name.Replace(" ", "") == uomname.Replace(" ", "") && !x.IsDeleted).FirstOrDefault();
            //var defuom = _uomService.GetQueryable().Where(x => !x.IsDeleted).FirstOrDefault();
            var curr = _currencyService.GetQueryable().Where(x => x.Name == currname && !x.IsDeleted).FirstOrDefault();
            var defcurr = _currencyService.GetQueryable().Where(x => !x.IsDeleted).FirstOrDefault();
            var warehouse = _warehouseService.GetQueryable().Where(x => x.Code == "Sby").FirstOrDefault();
            var date = new DateTime(2014, 12, 1);

            var obj = _itemService.GetQueryable().Where(x => x.Sku == sku && !x.IsDeleted).FirstOrDefault();
            if (obj == null)
            {
                obj = new Item()
                {
                    Sku = sku,
                    Name = name,
                    Description = desc,
                    IsTradeable = (tradable == "Yes") ? true : false,
                    UoMId = (uom != null) ? uom.Id : 0, //defuom.Id,
                    ItemTypeId = (itemtype != null) ? itemtype.Id : 0,
                    MinimumQuantity = (int)minqty,
                    //Quantity = qty,
                    SellingPrice = price,
                    CurrencyId = (curr != null) ? curr.Id : defcurr.Id,
                };
                obj = _itemService.CreateObject(obj, _uomService, _itemTypeService, _warehouseItemService, _warehouseService, _priceMutationService);
                Log(obj, sheetname, rowidx + 2);
                if (qty != 0 && !obj.Errors.Any())
                {
                    var obj2 = new StockAdjustment()
                    {
                        WarehouseId = warehouse.Id,
                        AdjustmentDate = date,
                    };
                    obj2 = _stockAdjustmentService.CreateObject(obj2, _warehouseService);
                    Log(obj2, sheetname, rowidx + 2);
                    if (!obj2.Errors.Any())
                    {
                        var obj2det = new StockAdjustmentDetail()
                        {
                            StockAdjustmentId = obj2.Id,
                            ItemId = obj.Id,
                            Quantity = qty,
                            //Price = avgprice,
                        };
                        obj2det = _stockAdjustmentDetailService.CreateObject(obj2det, _stockAdjustmentService, _itemService, _warehouseItemService);
                        Log(obj2det, sheetname, rowidx + 2);
                        if (!obj2det.Errors.Any())
                        {
                            _stockAdjustmentService.ConfirmObject(obj2, date, _stockAdjustmentDetailService, _stockMutationService, _itemService, _blanketService, _warehouseItemService, _accountService, _generalLedgerJournalService, _closingService);
                        }
                        else
                        {
                            _stockAdjustmentService.DeleteObject(obj2.Id);
                        }
                    }
                }
            }
        }

        public void DoItemGudangSmg(DataRow row, int rowidx, string sheetname)
        {
            var sku = row[0].ToString();
            var typename = row[1].ToString();
            var name = row[3].ToString();
            var desc = row[5].ToString();
            var tradable = row[6].ToString();
            var uomname = row[7].ToString();
            var minqty = decimal.Parse(GetValidNumber(row[8].ToString()));
            //var maxqty = decimal.Parse(row[9].ToString());
            var qty = decimal.Parse(GetValidNumber(row[10].ToString()));
            var price = decimal.Parse(GetValidNumber(row[12].ToString()));
            var currname = row[13].ToString();
            var itemtype = _itemTypeService.GetQueryable().Where(x => x.Name == typename && !x.IsDeleted).FirstOrDefault();
            var uom = _uomService.GetQueryable().Where(x => x.Name.Replace(" ", "") == uomname.Replace(" ", "") && !x.IsDeleted).FirstOrDefault();
            //var defuom = _uomService.GetQueryable().Where(x => !x.IsDeleted).FirstOrDefault();
            var curr = _currencyService.GetQueryable().Where(x => x.Name == currname && !x.IsDeleted).FirstOrDefault();
            var defcurr = _currencyService.GetQueryable().Where(x => !x.IsDeleted).FirstOrDefault();
            var warehouse = _warehouseService.GetQueryable().Where(x => x.Code == "Smg").FirstOrDefault();
            var date = new DateTime(2014, 12, 1);

            var obj = _itemService.GetQueryable().Where(x => x.Sku == sku && !x.IsDeleted).FirstOrDefault();
            if (obj == null)
            {
                obj = new Item()
                {
                    Sku = sku,
                    Name = name,
                    Description = desc,
                    IsTradeable = (tradable == "Yes") ? true : false,
                    UoMId = (uom != null) ? uom.Id : 0, //defuom.Id,
                    ItemTypeId = (itemtype != null) ? itemtype.Id : 0,
                    MinimumQuantity = (int)minqty,
                    //Quantity = qty,
                    SellingPrice = price,
                    CurrencyId = (curr != null) ? curr.Id : defcurr.Id,
                };
                obj = _itemService.CreateObject(obj, _uomService, _itemTypeService, _warehouseItemService, _warehouseService, _priceMutationService);
                Log(obj, sheetname, rowidx + 2);
                if (qty != 0 && !obj.Errors.Any())
                {
                    var obj2 = new StockAdjustment()
                    {
                        WarehouseId = warehouse.Id,
                        AdjustmentDate = date,
                    };
                    obj2 = _stockAdjustmentService.CreateObject(obj2, _warehouseService);
                    Log(obj2, sheetname, rowidx + 2);
                    if (!obj2.Errors.Any())
                    {
                        var obj2det = new StockAdjustmentDetail()
                        {
                            StockAdjustmentId = obj2.Id,
                            ItemId = obj.Id,
                            Quantity = qty,
                            //Price = avgprice,
                        };
                        obj2det = _stockAdjustmentDetailService.CreateObject(obj2det, _stockAdjustmentService, _itemService, _warehouseItemService);
                        Log(obj2det, sheetname, rowidx + 2);
                        if (!obj2det.Errors.Any())
                        {
                            _stockAdjustmentService.ConfirmObject(obj2, date, _stockAdjustmentDetailService, _stockMutationService, _itemService, _blanketService, _warehouseItemService, _accountService, _generalLedgerJournalService, _closingService);
                        }
                        else
                        {
                            _stockAdjustmentService.DeleteObject(obj2.Id);
                        }
                    }
                }
            }
        }

        public void DoCoreBuilder(DataRow row, int rowidx, string sheetname)
        {
            var sku = row[0].ToString();
            var uomname = row[1].ToString();
            var uom = _uomService.GetQueryable().Where(x => x.Name.Replace(" ", "") == uomname.Replace(" ", "") && !x.IsDeleted).FirstOrDefault();
            var defuom = _uomService.GetQueryable().Where(x => x.Name == "" && !x.IsDeleted).FirstOrDefault();
            var machname = row[2].ToString();
            var mach = _machineService.GetQueryable().Where(x => x.Name == machname && !x.IsDeleted).FirstOrDefault();
            var defmach = _machineService.GetQueryable().Where(x => !x.IsDeleted).FirstOrDefault();
            var cbtype = row[3].ToString();
            var name = row[4].ToString();
            var desc = row[5].ToString();
            var obj = _coreBuilderService.GetQueryable().Where(x => x.BaseSku == sku && !x.IsDeleted).FirstOrDefault();
            if (obj == null)
            {
                obj = new CoreBuilder()
                {
                    BaseSku = sku,
                    SkuNewCore = sku+"N",
                    SkuUsedCore = sku+"U",
                    UoMId = (uom != null) ? uom.Id : defuom.Id,
                    MachineId = (mach != null) ? mach.Id : defmach.Id,
                    CoreBuilderTypeCase = (cbtype != null && cbtype.Trim() != "") ? cbtype : "None",
                    Name = name,
                    Description = desc,
                };
                obj = _coreBuilderService.CreateObject(obj, _uomService, _itemService, _itemTypeService, _warehouseItemService, _warehouseService, _priceMutationService, _machineService);
                Log(obj, sheetname, rowidx + 2);
            }
        }

        public void DoRollerBuilder(DataRow row, int rowidx, string sheetname)
        {
            var sku = row[0].ToString();
            var uomname = row[1].ToString();
            var uom = _uomService.GetQueryable().Where(x => x.Name.Replace(" ", "") == uomname.Replace(" ", "") && !x.IsDeleted).FirstOrDefault();
            var machname = row[2].ToString();
            var mach = _machineService.GetQueryable().Where(x => x.Name == machname && !x.IsDeleted).FirstOrDefault();
            var name = row[4].ToString();
            var desc = row[5].ToString();
            var obj = _rollerBuilderService.GetQueryable().Where(x => x.BaseSku == sku && !x.IsDeleted).FirstOrDefault();
            if (obj == null)
            {
                obj = new RollerBuilder()
                {
                    BaseSku = sku,
                    SkuRollerNewCore = sku+"N",
                    SkuRollerUsedCore = sku + "U",
                    UoMId = (uom != null) ? uom.Id : 0, //defuom.Id,
                    MachineId = (mach != null) ? mach.Id : 0, //defmach.Id,
                    Name = name,
                    Description = desc,
                };
                obj = _rollerBuilderService.CreateObject(obj, _machineService, _uomService, _itemService, _itemTypeService, _coreBuilderService, _rollerTypeService, _warehouseItemService, _warehouseService, _priceMutationService);
                Log(obj, sheetname, rowidx + 2);
            }
        }

        public void DoBlanket(DataRow row, int rowidx, string sheetname)
        {

        }

        // Example 1
        public int ImporttoSQL(string sPath)
        {
            // Note : HDR=Yes indicates that the first row contains column names, not data. HDR=No indicates the opposite.
            // Connect to Excel 2007 earlier version
            //string sSourceConstr = @"Provider=Microsoft.Jet.OLEDB.4.0; Data Source=C:\AgentList.xls; Extended Properties=""Excel 8.0;HDR=YES;""";
            // Connect to Excel 2007 (and later) files with the Xlsx file extension 
            string sSourceConstr = string.Format(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=""Excel 12.0 Xml;HDR=YES;IMEX=1;""", sPath); //IMEX=1

            //string sDestConstr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString; // "DBConnection"
            int count = 0;
            OleDbConnection sSourceConnection = new OleDbConnection(sSourceConstr);
            //DbContext db = new DbContext(sDestConstr);
            using (var db = new OffsetPrintingSuppliesEntities())
            {
                using (sSourceConnection)
                {
                    sSourceConnection.Open();
                    // Get Sheets list
                    DataTable dt = sSourceConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                    //String[] strExcelSheets = new String[dt.Rows.Count];
                    //int i = 0;
                    // Add the sheet name to the string array.
                    foreach (DataRow dtrow in dt.Rows)
                    {
                        //strExcelSheets[i] = row["TABLE_NAME"].ToString();
                        //i++;

                        string sheetname = dtrow["TABLE_NAME"].ToString(); // string.Format("{0}", "Sheet1$");
                        string sql = string.Format("Select [Employee ID],[Employee Name],[Designation],[Posting],[Dept] FROM [{0}]", "Sheet1$");
                        OleDbCommand command = new OleDbCommand(sql, sSourceConnection);
                        sSourceConnection.Open();
                        using (OleDbDataReader dr = command.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                //switch (sheetname)
                                //{
                                //    case SheetName.BranchOffice :
                                //        count += DoBranchOffice(dr, db);
                                //        break;
                                //    case SheetName.Department:
                                //        count += DoDepartment(dr, db);
                                //        break;
                                //}

                                //DataTable schemaTable = dr.GetSchemaTable(); // read the whole table/sheet
                                //foreach (DataRow row in schemaTable.Rows)
                                //{
                                //    foreach (DataColumn column in schemaTable.Columns)
                                //    {

                                //    }
                                //}

                                //using (SqlBulkCopy bulkCopy = new SqlBulkCopy(sDestConstr)) // copy the whole table/sheet
                                //{
                                //    bulkCopy.DestinationTableName = "Employee";
                                //    //You can mannualy set the column mapping by the following way.
                                //    //bulkCopy.ColumnMappings.Add("Employee ID", "Employee Code");
                                //    bulkCopy.WriteToServer(dr);
                                //}
                            }
                            if (dr != null)
                            {
                                dr.Close();
                                dr.Dispose();
                            }
                        }
                    }
                    if (dt != null)
                    {
                        dt.Dispose();
                    }
                    //if (sSourceConnection != null)
                    //{
                    //    sSourceConnection.Close();
                    //}
                }
                if (db != null)
                {
                    db.Dispose();
                }
            }
            return count;
        }

        // Example 2
        public int ImportFromExcel(string fileName)
        {
            // Buat satu Service menggunakan nama ExcelEntryService
            // yang memiliki Repository, dimana GetContext dapat digunakan

            // membaca setiap nama sheet dan link dengan nama table di database / domain model
            // setiap table mengarah ke service terkait, menggunakan fungsi CreateObject
            // 
            int count = 0;
            //DbContext conLinq = new DbContext("Data Source=server name;Initial Catalog=Database Name;Integrated Security=true");
            using (var conLinq = new OffsetPrintingSuppliesEntities())
            {
                try
                {
                    DataTable dtExcel = new DataTable();
                    // Note : HDR=Yes indicates that the first row contains column names, not data. HDR=No indicates the opposite.
                    string SourceConstr = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source='" + fileName + "';Extended Properties= 'Excel 12.0;HDR=Yes;IMEX=1'";
                    OleDbConnection conn = new OleDbConnection(SourceConstr);
                    string query = "Select * from [Sheet1$]";
                    OleDbDataAdapter data = new OleDbDataAdapter(query, conn);
                    conn.Open();
                    data.Fill(dtExcel);
                    for (int i = 0; i < dtExcel.Rows.Count; i++)
                    {
                        try
                        {
                            count += conLinq.Database.ExecuteSqlCommand("insert into [Sheet1$] values(" + dtExcel.Rows[i][0] + "," + dtExcel.Rows[i][1] + ",'" + dtExcel.Rows[i][2] + "'," + dtExcel.Rows[i][3] + ")");
                        }
                        catch (Exception ex)
                        {
                            continue;
                        }
                    }
                    if (count == dtExcel.Rows.Count)
                    {
                        //<--Success Message-->
                    }
                    else
                    {
                        //<--Failure Message-->
                    }
                    dtExcel.Dispose();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    conLinq.Dispose();
                }
            }
            return count;
        }

        public void DoSalesInvoiceMigration(DataRow row, int rowidx, string sheetname)
        {
            var nosurat = row[0].ToString();
            var currname = row[1].ToString() == "IDR" ? "Rupiah" : row[1].ToString();
            var curr = _currencyService.GetObjectByName(currname);
            var contactname = row[2].ToString();
            var contactid = int.Parse(GetValidNumber(row[3].ToString()));
            var contact = _contactService.GetQueryable().Where(x => x.Name == contactname && !x.IsDeleted).FirstOrDefault();
            var invdate = row[4].ToString();
            DateTime date = new DateTime(2014, 11, 30);
            if (!DateTime.TryParse(invdate, System.Globalization.CultureInfo.CreateSpecificCulture("en-US"), System.Globalization.DateTimeStyles.None, out date)) date = new DateTime(2014, 11, 30); //DateTime.ParseExact(invdate, "d/M/yy", CultureInfo.InvariantCulture, DateTimeStyles.None);
            var amount = decimal.Parse(GetValidNumber(row[5].ToString()));
            var dpp = decimal.Parse(GetValidNumber(row[6].ToString()));
            var tax = decimal.Parse(GetValidNumber(row[7].ToString()));
            var rate = decimal.Parse(GetValidNumber(row[8].ToString()));
            var usd = decimal.Parse(GetValidNumber(row[9].ToString()));
            var eur = decimal.Parse(GetValidNumber(row[10].ToString()));
            var idr = decimal.Parse(GetValidNumber(row[11].ToString()));
            var exrate = decimal.Parse(GetValidNumber(row[12].ToString()));
            var total = decimal.Parse(GetValidNumber(row[13].ToString()));
            var desc = row[14].ToString();
            var obj = _salesInvoiceMigrationService.GetQueryable().Where(x => x.NomorSurat == nosurat).FirstOrDefault();
            if (obj == null)
            {
                if (curr == null) throw new Exception("Invalid Currency");
                if (contact == null && contactid>1) throw new Exception("Invalid Contact");
                obj = new SalesInvoiceMigration()
                {
                    NomorSurat = nosurat,
                    CurrencyId = curr.Id,
                    ContactId = contactid,
                    InvoiceDate = date,
                    AmountReceivable = amount,
                    DPP = dpp,
                    Tax = tax,
                    Rate = rate,
                };
                obj = _salesInvoiceMigrationService.CreateObject(obj, _generalLedgerJournalService, _accountService, _gLNonBaseCurrencyService, _currencyService, _receivableService);
                Log(obj, sheetname, rowidx + 2);
            }
        }

        public void DoPurchaseInvoiceMigration(DataRow row, int rowidx, string sheetname)
        {
            var nosurat = row[0].ToString();
            var currname = row[1].ToString() == "IDR" ? "Rupiah" : row[1].ToString();
            var curr = _currencyService.GetObjectByName(currname);
            var contactname = row[2].ToString();
            var contactid = int.Parse(GetValidNumber(row[3].ToString()));
            var contact = _contactService.GetQueryable().Where(x => x.Name == contactname && !x.IsDeleted).FirstOrDefault();
            var invdate = row[4].ToString();
            DateTime date = new DateTime(2014, 11, 30);
            if (!DateTime.TryParse(invdate, System.Globalization.CultureInfo.CreateSpecificCulture("en-US"), System.Globalization.DateTimeStyles.None, out date)) date = new DateTime(2014, 11, 30); //DateTime.ParseExact(invdate, "d/M/yy", CultureInfo.InvariantCulture, DateTimeStyles.None);
            var amount = decimal.Parse(GetValidNumber(row[5].ToString()));
            var dpp = decimal.Parse(GetValidNumber(row[6].ToString()));
            var tax = decimal.Parse(GetValidNumber(row[7].ToString()));
            var rate = decimal.Parse(GetValidNumber(row[8].ToString()));
            var usd = decimal.Parse(GetValidNumber(row[9].ToString()));
            var eur = decimal.Parse(GetValidNumber(row[10].ToString()));
            var idr = decimal.Parse(GetValidNumber(row[11].ToString()));
            var exrate = decimal.Parse(GetValidNumber(row[12].ToString()));
            var total = decimal.Parse(GetValidNumber(row[13].ToString()));
            var desc = row[14].ToString();
            var obj = _purchaseInvoiceMigrationService.GetQueryable().Where(x => x.NomorSurat == nosurat).FirstOrDefault();
            if (obj == null)
            {
                if (curr == null) throw new Exception("Invalid Currency");
                if (contact == null && contactid > 1) throw new Exception("Invalid Contact");
                obj = new PurchaseInvoiceMigration()
                {
                    NomorSurat = nosurat,
                    CurrencyId = curr.Id,
                    ContactId = contactid,
                    InvoiceDate = date,
                    AmountPayable = amount,
                    DPP = dpp,
                    Tax = tax,
                    Rate = rate,
                };
                obj = _purchaseInvoiceMigrationService.CreateObject(obj, _generalLedgerJournalService, _accountService, _gLNonBaseCurrencyService, _currencyService, _payableService);
                Log(obj, sheetname, rowidx + 2);
            }
        }

        public int ImportMigrationFromExcel(string fileName)
        {
            // Buat satu Service menggunakan nama ExcelEntryService
            // yang memiliki Repository, dimana GetContext dapat digunakan

            // membaca setiap nama sheet dan link dengan nama table di database / domain model
            // setiap table mengarah ke service terkait, menggunakan fungsi CreateObject
            // 
            int count = 0;
            //DbContext conLinq = new DbContext("Data Source=server name;Initial Catalog=Database Name;Integrated Security=true");
            using (var conLinq = new OffsetPrintingSuppliesEntities())
            {
                //try
                {
                    DataSet dtExcel = new DataSet();
                    // Note : HDR=Yes indicates that the first row contains column names, not data. HDR=No indicates the opposite.
                    string SourceConstr = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source='" + fileName + "';Extended Properties= 'Excel 12.0;HDR=Yes;IMEX=1'";
                    OleDbConnection conn = new OleDbConnection(SourceConstr);
                    conn.Open();
                    // Get Sheets list
                    DataTable dt = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                    
                    string sheetname = "";
                    foreach (DataRow dtrow in dt.Rows)
                    {
                        sheetname = dtrow["TABLE_NAME"].ToString(); //.Trim(new[] { '$' }); // string.Format("{0}", "Sheet1$");
                        string query = string.Format("Select * from [{0}]", sheetname);
                        // Create OleDbCommand object and select data from worksheet Sheet1
                        OleDbCommand cmd = new OleDbCommand(query, conn);
                        // Create new OleDbDataAdapter
                        OleDbDataAdapter oleda = new OleDbDataAdapter();
                        oleda.SelectCommand = cmd;
                        // Fill the DataSet from the data extracted from the worksheet.
                        oleda.Fill(dtExcel, sheetname);
                        oleda.Dispose();
                        cmd.Dispose();
                    }

                    sheetname = "Customer$";
                    for (int i = 0; i < dtExcel.Tables[sheetname].Rows.Count; i++)
                    {
                        //try
                        {
                            //count += conLinq.Database.ExecuteSqlCommand("insert into [Sheet1$] values(" + dtExcel.Rows[i][0] + "," + dtExcel.Rows[i][1] + ",'" + dtExcel.Rows[i][2] + "'," + dtExcel.Rows[i][3] + ")");
                            // Find Or Create Object
                            var row = dtExcel.Tables[sheetname].Rows[i];
                            var tmp = row[0].ToString() + row[1].ToString();
                            if (tmp == null || tmp.Trim() == "") continue; // skip if the 1st 2 column is empty
                            DoCustomer(row, i, sheetname);
                        }
                        //catch (Exception ex)
                        //{
                        //    Console.WriteLine(sheetname + " Row:" + (i + 2) + " Exception:" + ex.Message);
                        //    continue;
                        //}
                    }

                    sheetname = "Supplier$";
                    for (int i = 0; i < dtExcel.Tables[sheetname].Rows.Count; i++)
                    {
                        //try
                        {
                            //count += conLinq.Database.ExecuteSqlCommand("insert into [Sheet1$] values(" + dtExcel.Rows[i][0] + "," + dtExcel.Rows[i][1] + ",'" + dtExcel.Rows[i][2] + "'," + dtExcel.Rows[i][3] + ")");
                            // Find Or Create Object
                            var row = dtExcel.Tables[sheetname].Rows[i];
                            var tmp = row[0].ToString() + row[1].ToString();
                            if (tmp == null || tmp.Trim() == "") continue; // skip if the 1st 2 column is empty
                            DoSupplier(row, i, sheetname);
                        }
                        //catch (Exception ex)
                        //{
                        //    Console.WriteLine(sheetname + " Row:" + (i + 2) + " Exception:" + ex.Message);
                        //    continue;
                        //}
                    }

                    //sheetname = "SalesInvoiceMigration$";
                    //for (int i = 0; i < dtExcel.Tables[sheetname].Rows.Count; i++)
                    //{
                    //    //try
                    //    {
                    //        //count += conLinq.Database.ExecuteSqlCommand("insert into [Sheet1$] values(" + dtExcel.Rows[i][0] + "," + dtExcel.Rows[i][1] + ",'" + dtExcel.Rows[i][2] + "'," + dtExcel.Rows[i][3] + ")");
                    //        // Find Or Create Object
                    //        var row = dtExcel.Tables[sheetname].Rows[i];
                    //        var tmp = row[0].ToString() + row[1].ToString();
                    //        if (tmp == null || tmp.Trim() == "") continue; // skip if the 1st 2 column is empty
                    //        DoSalesInvoiceMigration(row, i, sheetname);
                    //    }
                    //    //catch (Exception ex)
                    //    //{
                    //    //    Console.WriteLine(sheetname + " Row:" + (i + 2) + " Exception:" + ex.Message);
                    //    //    continue;
                    //    //}
                    //}

                    //sheetname = "PurchaseInvoiceMigration$";
                    //for (int i = 0; i < dtExcel.Tables[sheetname].Rows.Count; i++)
                    //{
                    //    //try
                    //    {
                    //        //count += conLinq.Database.ExecuteSqlCommand("insert into [Sheet1$] values(" + dtExcel.Rows[i][0] + "," + dtExcel.Rows[i][1] + ",'" + dtExcel.Rows[i][2] + "'," + dtExcel.Rows[i][3] + ")");
                    //        // Find Or Create Object
                    //        var row = dtExcel.Tables[sheetname].Rows[i];
                    //        var tmp = row[0].ToString() + row[1].ToString();
                    //        if (tmp == null || tmp.Trim() == "") continue; // skip if the 1st 2 column is empty
                    //        DoPurchaseInvoiceMigration(row, i, sheetname);
                    //    }
                    //    //catch (Exception ex)
                    //    //{
                    //    //    Console.WriteLine(sheetname + " Row:" + (i + 2) + " Exception:" + ex.Message);
                    //    //    continue;
                    //    //}
                    //}

                    //oleda.Dispose();
                    //foreach (var x in data) x.Dispose();
                    //data.Clear();
                    dtExcel.Dispose();
                }
                //catch (Exception ex)
                //{
                //    throw ex;
                //}
                //finally
                {
                    conLinq.Dispose();
                }
            }
            return count;
        }

        public int ImportDataFromExcel(string fileName)
        {
            // Buat satu Service menggunakan nama ExcelEntryService
            // yang memiliki Repository, dimana GetContext dapat digunakan

            // membaca setiap nama sheet dan link dengan nama table di database / domain model
            // setiap table mengarah ke service terkait, menggunakan fungsi CreateObject
            // 
            int count = 0;
            //DbContext conLinq = new DbContext("Data Source=server name;Initial Catalog=Database Name;Integrated Security=true");
            using (var conLinq = new OffsetPrintingSuppliesEntities())
            {
                try
                {
                    DataSet dtExcel = new DataSet();
                    // Note : HDR=Yes indicates that the first row contains column names, not data. HDR=No indicates the opposite.
                    string SourceConstr = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source='" + fileName + "';Extended Properties= 'Excel 12.0;HDR=Yes;IMEX=1'";
                    OleDbConnection conn = new OleDbConnection(SourceConstr);
                    conn.Open();
                    // Get Sheets list
                    DataTable dt = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                    
                    string sheetname = "";
                    foreach (DataRow dtrow in dt.Rows)
                    {
                        sheetname = dtrow["TABLE_NAME"].ToString(); //.Trim(new[] { '$' }); // string.Format("{0}", "Sheet1$");
                        string query = string.Format("Select * from [{0}]", sheetname);
                        // Create OleDbCommand object and select data from worksheet Sheet1
                        OleDbCommand cmd = new OleDbCommand(query, conn);
                        // Create new OleDbDataAdapter
                        OleDbDataAdapter oleda = new OleDbDataAdapter();
                        oleda.SelectCommand = cmd;
                        // Fill the DataSet from the data extracted from the worksheet.
                        oleda.Fill(dtExcel, sheetname);
                        oleda.Dispose();
                        cmd.Dispose();
                    }

                    //sheetname = "ItemType$";
                    //for (int i = 0; i < dtExcel.Tables[sheetname].Rows.Count; i++)
                    //{
                    //    try
                    //    {
                    //        //count += conLinq.Database.ExecuteSqlCommand("insert into [Sheet1$] values(" + dtExcel.Rows[i][0] + "," + dtExcel.Rows[i][1] + ",'" + dtExcel.Rows[i][2] + "'," + dtExcel.Rows[i][3] + ")");
                    //        // Find Or Create Object
                    //        var row = dtExcel.Tables[sheetname].Rows[i];
                    //        var tmp = row[0].ToString() + row[1].ToString();
                    //        if (tmp == null || tmp.Trim() == "") continue; // skip if the 1st 2 column is empty
                    //        DoItemType(row, i, sheetname);
                    //    }
                    //    catch (Exception ex)
                    //    {
                    //        Console.WriteLine(sheetname + " Row:" + (i + 2) + " Exception:" + ex.Message);
                    //        continue;
                    //    }
                    //}

                    //sheetname = "uom$";
                    //for (int i = 0; i < dtExcel.Tables[sheetname].Rows.Count; i++)
                    //{
                    //    try
                    //    {
                    //        //count += conLinq.Database.ExecuteSqlCommand("insert into [Sheet1$] values(" + dtExcel.Rows[i][0] + "," + dtExcel.Rows[i][1] + ",'" + dtExcel.Rows[i][2] + "'," + dtExcel.Rows[i][3] + ")");
                    //        // Find Or Create Object
                    //        var row = dtExcel.Tables[sheetname].Rows[i];
                    //        var tmp = row[0].ToString() + row[1].ToString();
                    //        if (tmp == null || tmp.Trim() == "") continue; // skip if the 1st 2 column is empty
                    //        DoUoM(row, i, sheetname);
                    //    }
                    //    catch (Exception ex)
                    //    {
                    //        Console.WriteLine(sheetname + " Row:" + (i + 2) + " Exception:" + ex.Message);
                    //        continue;
                    //    }
                    //}

                    //sheetname = "rollertype$";
                    //for (int i = 0; i < dtExcel.Tables[sheetname].Rows.Count; i++)
                    //{
                    //    try
                    //    {
                    //        //count += conLinq.Database.ExecuteSqlCommand("insert into [Sheet1$] values(" + dtExcel.Rows[i][0] + "," + dtExcel.Rows[i][1] + ",'" + dtExcel.Rows[i][2] + "'," + dtExcel.Rows[i][3] + ")");
                    //        // Find Or Create Object
                    //        var row = dtExcel.Tables[sheetname].Rows[i];
                    //        var tmp = row[0].ToString() + row[1].ToString();
                    //        if (tmp == null || tmp.Trim() == "") continue; // skip if the 1st 2 column is empty
                    //        DoRollerType(row, i, sheetname);
                    //    }
                    //    catch (Exception ex)
                    //    {
                    //        Console.WriteLine(sheetname + " Row:" + (i + 2) + " Exception:" + ex.Message);
                    //        continue;
                    //    }
                    //}

                    //sheetname = "machine$";
                    //for (int i = 0; i < dtExcel.Tables[sheetname].Rows.Count; i++)
                    //{
                    //    try
                    //    {
                    //        //count += conLinq.Database.ExecuteSqlCommand("insert into [Sheet1$] values(" + dtExcel.Rows[i][0] + "," + dtExcel.Rows[i][1] + ",'" + dtExcel.Rows[i][2] + "'," + dtExcel.Rows[i][3] + ")");
                    //        // Find Or Create Object
                    //        var row = dtExcel.Tables[sheetname].Rows[i];
                    //        var tmp = row[0].ToString() + row[1].ToString();
                    //        if (tmp == null || tmp.Trim() == "") continue; // skip if the 1st 2 column is empty
                    //        DoMachine(row, i, sheetname);
                    //    }
                    //    catch (Exception ex)
                    //    {
                    //        Console.WriteLine(sheetname + " Row:" + (i + 2) + " Exception:" + ex.Message);
                    //        continue;
                    //    }
                    //}

                    //sheetname = "warehouse$";
                    //for (int i = 0; i < dtExcel.Tables[sheetname].Rows.Count; i++)
                    //{
                    //    try
                    //    {
                    //        //count += conLinq.Database.ExecuteSqlCommand("insert into [Sheet1$] values(" + dtExcel.Rows[i][0] + "," + dtExcel.Rows[i][1] + ",'" + dtExcel.Rows[i][2] + "'," + dtExcel.Rows[i][3] + ")");
                    //        // Find Or Create Object
                    //        var row = dtExcel.Tables[sheetname].Rows[i];
                    //        var tmp = row[0].ToString() + row[1].ToString();
                    //        if (tmp == null || tmp.Trim() == "") continue; // skip if the 1st 2 column is empty
                    //        DoWarehouse(row, i, sheetname);
                    //    }
                    //    catch (Exception ex)
                    //    {
                    //        Console.WriteLine(sheetname + " Row:" + (i + 2) + " Exception:" + ex.Message);
                    //        continue;
                    //    }
                    //}

                    //sheetname = "currency$";
                    //for (int i = 0; i < dtExcel.Tables[sheetname].Rows.Count; i++)
                    //{
                    //    try
                    //    {
                    //        //count += conLinq.Database.ExecuteSqlCommand("insert into [Sheet1$] values(" + dtExcel.Rows[i][0] + "," + dtExcel.Rows[i][1] + ",'" + dtExcel.Rows[i][2] + "'," + dtExcel.Rows[i][3] + ")");
                    //        // Find Or Create Object
                    //        var row = dtExcel.Tables[sheetname].Rows[i];
                    //        var tmp = row[0].ToString() + row[1].ToString();
                    //        if (tmp == null || tmp.Trim() == "") continue; // skip if the 1st 2 column is empty
                    //        DoCurrency(row, i, sheetname);
                    //    }
                    //    catch (Exception ex)
                    //    {
                    //        Console.WriteLine(sheetname + " Row:" + (i + 2) + " Exception:" + ex.Message);
                    //        continue;
                    //    }
                    //}

                    sheetname = "customer2$";
                    for (int i = 0; i < dtExcel.Tables[sheetname].Rows.Count; i++)
                    {
                        try
                        {
                            //count += conLinq.Database.ExecuteSqlCommand("insert into [Sheet1$] values(" + dtExcel.Rows[i][0] + "," + dtExcel.Rows[i][1] + ",'" + dtExcel.Rows[i][2] + "'," + dtExcel.Rows[i][3] + ")");
                            // Find Or Create Object
                            var row = dtExcel.Tables[sheetname].Rows[i];
                            var tmp = row[0].ToString() + row[1].ToString();
                            if (tmp == null || tmp.Trim() == "") continue; // skip if the 1st 2 column is empty
                            DoCustomer(row, i, sheetname);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(sheetname + " Row:" + (i + 2) + " Exception:" + ex.Message);
                            continue;
                        }
                    }

                    //sheetname = "supplier2$";
                    //for (int i = 0; i < dtExcel.Tables[sheetname].Rows.Count; i++)
                    //{
                    //    try
                    //    {
                    //        //count += conLinq.Database.ExecuteSqlCommand("insert into [Sheet1$] values(" + dtExcel.Rows[i][0] + "," + dtExcel.Rows[i][1] + ",'" + dtExcel.Rows[i][2] + "'," + dtExcel.Rows[i][3] + ")");
                    //        // Find Or Create Object
                    //        var row = dtExcel.Tables[sheetname].Rows[i];
                    //        var tmp = row[0].ToString() + row[1].ToString();
                    //        if (tmp == null || tmp.Trim() == "") continue; // skip if the 1st 2 column is empty
                    //        DoSupplier(row, i, sheetname);
                    //    }
                    //    catch (Exception ex)
                    //    {
                    //        Console.WriteLine(sheetname + " Row:" + (i + 2) + " Exception:" + ex.Message);
                    //        continue;
                    //    }
                    //}

                    //sheetname = "Item_gudang_serpong$";
                    //for (int i = 0; i < dtExcel.Tables[sheetname].Rows.Count; i++)
                    //{
                    //    try
                    //    {
                    //        //count += conLinq.Database.ExecuteSqlCommand("insert into [Sheet1$] values(" + dtExcel.Rows[i][0] + "," + dtExcel.Rows[i][1] + ",'" + dtExcel.Rows[i][2] + "'," + dtExcel.Rows[i][3] + ")");
                    //        // Find Or Create Object
                    //        var row = dtExcel.Tables[sheetname].Rows[i];
                    //        var tmp = row[0].ToString() + row[1].ToString();
                    //        if (tmp == null || tmp.Trim() == "") continue; // skip if the 1st 2 column is empty
                    //        DoItemGudangSerpong(row, i, sheetname);
                    //    }
                    //    catch (Exception ex)
                    //    {
                    //        Console.WriteLine(sheetname + " Row:" + (i + 2) + " Exception:" + ex.Message);
                    //        continue;
                    //    }
                    //}

                    //sheetname = "Item_gudang_sby$";
                    //for (int i = 0; i < dtExcel.Tables[sheetname].Rows.Count; i++)
                    //{
                    //    try
                    //    {
                    //        //count += conLinq.Database.ExecuteSqlCommand("insert into [Sheet1$] values(" + dtExcel.Rows[i][0] + "," + dtExcel.Rows[i][1] + ",'" + dtExcel.Rows[i][2] + "'," + dtExcel.Rows[i][3] + ")");
                    //        // Find Or Create Object
                    //        var row = dtExcel.Tables[sheetname].Rows[i];
                    //        var tmp = row[0].ToString() + row[1].ToString();
                    //        if (tmp == null || tmp.Trim() == "") continue; // skip if the 1st 2 column is empty
                    //        DoItemGudangSby(row, i, sheetname);
                    //    }
                    //    catch (Exception ex)
                    //    {
                    //        Console.WriteLine(sheetname + " Row:" + (i + 2) + " Exception:" + ex.Message);
                    //        continue;
                    //    }
                    //}

                    //sheetname = "Item_gudang_smg$";
                    //for (int i = 0; i < dtExcel.Tables[sheetname].Rows.Count; i++)
                    //{
                    //    try
                    //    {
                    //        //count += conLinq.Database.ExecuteSqlCommand("insert into [Sheet1$] values(" + dtExcel.Rows[i][0] + "," + dtExcel.Rows[i][1] + ",'" + dtExcel.Rows[i][2] + "'," + dtExcel.Rows[i][3] + ")");
                    //        // Find Or Create Object
                    //        var row = dtExcel.Tables[sheetname].Rows[i];
                    //        var tmp = row[0].ToString() + row[1].ToString();
                    //        if (tmp == null || tmp.Trim() == "") continue; // skip if the 1st 2 column is empty
                    //        DoItemGudangSmg(row, i, sheetname);
                    //    }
                    //    catch (Exception ex)
                    //    {
                    //        Console.WriteLine(sheetname + " Row:" + (i + 2) + " Exception:" + ex.Message);
                    //        continue;
                    //    }
                    //}

                    //sheetname = "corebuilder$";
                    //for (int i = 0; i < dtExcel.Tables[sheetname].Rows.Count; i++)
                    //{
                    //    try
                    //    {
                    //        //count += conLinq.Database.ExecuteSqlCommand("insert into [Sheet1$] values(" + dtExcel.Rows[i][0] + "," + dtExcel.Rows[i][1] + ",'" + dtExcel.Rows[i][2] + "'," + dtExcel.Rows[i][3] + ")");
                    //        // Find Or Create Object
                    //        var row = dtExcel.Tables[sheetname].Rows[i];
                    //        var tmp = row[0].ToString() + row[1].ToString();
                    //        if (tmp == null || tmp.Trim() == "") continue; // skip if the 1st 2 column is empty
                    //        DoCoreBuilder(row, i, sheetname);
                    //    }
                    //    catch (Exception ex)
                    //    {
                    //        Console.WriteLine(sheetname + " Row:" + (i + 2) + " Exception:" + ex.Message);
                    //        continue;
                    //    }
                    //}

                    //oleda.Dispose();
                    //foreach (var x in data) x.Dispose();
                    //data.Clear();
                    dtExcel.Dispose();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    conLinq.Dispose();
                }
            }
            return count;
        }
    }
}
