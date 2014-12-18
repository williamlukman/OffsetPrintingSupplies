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

namespace ExcelImport
{
    // connectionString info at http://connectionstrings.com/excel-2007
    public class ConversionFunction
    {
        //public ICompanyInfoService _companyInfoService = new CompanyInfoService(new CompanyInfoRepository(), new CompanyInfoValidator());
        public IItemTypeService _itemTypeService = new ItemTypeService(new ItemTypeRepository(), new ItemTypeValidator());
        

        public ConversionFunction()
        {
            //_companyInfoService = new CompanyInfoService(new CompanyInfoRepository(), new CompanyInfoValidator());
            //_branchOfficeService = new BranchOfficeService(new BranchOfficeRepository(), new BranchOfficeValidator());
            //_departmentService = new DepartmentService(new DepartmentRepository(), new DepartmentValidator());
            //_divisionService = new DivisionService(new DivisionRepository(), new DivisionValidator());
            //_employeeService = new EmployeeService(new EmployeeRepository(), new EmployeeValidator());
            //_titleInfoService = new TitleInfoService(new TitleInfoRepository(), new TitleInfoValidator());
        }

        //public int DoBranchOffice(OleDbDataReader dr, DbContext db)
        //{
        //    int count = 0;
        //    while (dr.Read()) // read per record/row from a table/sheet
        //    {
        //        BranchOffice obj = new BranchOffice
        //        {
        //            Code = dr.GetString(0),
        //            Name = dr.GetString(1),
        //        };
        //        if (!_branchOfficeService.CreateObject(obj, _companyInfoService).Errors.Any())
        //        {
        //            count++;
        //        };
        //    }
        //    return count;
        //}

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
                    //List<OleDbDataAdapter> data = new List<OleDbDataAdapter>();
                    
                    string sheetname = "";
                    foreach (DataRow dtrow in dt.Rows)
                    {
                        sheetname = dtrow["TABLE_NAME"].ToString();//.Trim(new[] { '$' }); // string.Format("{0}", "Sheet1$");
                        string query = string.Format("Select * from [{0}]", sheetname);
                        //data.Add(new OleDbDataAdapter(query, conn));
                        //data.LastOrDefault().Fill(dtExcel, sheetname);
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

                    // 1st sheet (ItemType)
                    sheetname = "ItemType$";
                    for (int i = 0; i < dtExcel.Tables[sheetname].Rows.Count; i++)
                    {
                        try
                        {
                            //count += conLinq.Database.ExecuteSqlCommand("insert into [Sheet1$] values(" + dtExcel.Rows[i][0] + "," + dtExcel.Rows[i][1] + ",'" + dtExcel.Rows[i][2] + "'," + dtExcel.Rows[i][3] + ")");
                            // Find Or Create Object
                            var row = dtExcel.Tables[sheetname].Rows[i];
                            var name = row[0].ToString();
                            var desc = row[1].ToString();
                            var itemtype = _itemTypeService.GetQueryable().Where(x => x.Name == name && !x.IsDeleted).FirstOrDefault();
                            if (itemtype == null)
                            {
                                itemtype = _itemTypeService.CreateObject(name, desc);
                            }
                        }
                        catch (Exception ex)
                        {
                            continue;
                        }
                    }
                    if (count == dtExcel.Tables[sheetname].Rows.Count)
                    {
                        //<--Success Message-->
                    }
                    else
                    {
                        //<--Failure Message-->
                    }


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
