﻿﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json.Linq;
using log4net;
using System.Data;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Globalization;

namespace WebView
{
    static class DateTimeExtensions
    {
        static int GetWeekOfYear(this DateTime time)
        {
            return _gc.GetWeekOfYear(time, CalendarWeekRule.FirstDay, DayOfWeek.Sunday);
        }

        static GregorianCalendar _gc = new GregorianCalendar();
        public static int GetWeekOfMonth(this DateTime time)
        {
            DateTime first = new DateTime(time.Year, time.Month, 1);
            return time.GetWeekOfYear() - first.GetWeekOfYear() + 1;
        }
    }

    public static class GeneralFunction
    {
        private readonly static log4net.ILog LOG = log4net.LogManager.GetLogger("GeneralFunction");

        /// <summary>
        /// Build Shipment Number Convention
        /// </summary>
        //public static string BuildShipmentNumber(string intCompany, string intCity, string jobCode, string jobNumber, string subJobNo)
        //{
        //    string shipmentNo = "";
        //    try
        //    {
        //        shipmentNo = intCompany + intCity + "." + jobCode + "." + Replicate(jobNumber, 6) + "-" + Replicate(subJobNo, 2);
        //    }
        //    catch (Exception ex)
        //    {
        //        LOG.Error("BuildShipmentNumber Failed,", ex);
        //    }

        //    return shipmentNo;
        //}

        /// <summary>
        /// Build Invoice Reference Convention
        /// </summary>
        public static string BuildInvoiceReference(string shipmentNo, string invoiceNo)
        {
            string invReference = "";
            try
            {
                invReference = "PR/" + shipmentNo.Trim().Substring(6, 12) + "/" + GeneralFunction.Replicate(invoiceNo.ToString(), 6) + "/" + DateTime.Now.Year.ToString();
            }
            catch (Exception ex)
            {
                LOG.Error("BuildInvoiceReference Failed,", ex);
            }

            return invReference;
        }

        public class JobClass
        {
            public decimal JobNumber { get; set; }
            public int SubJobNo { get; set; }
        }
        /// <summary>
        /// Check Job Entry Date to make sure if EPL is UnEditbable ornot
        /// </summary>
        /// <param name="jobcode"> jobcode is string value</param>
        /// <param name="jobcode"> jobnumber is string value</param>
        /// <param name="jobcode"> subjobno is string value</param>
        public static bool IsValidEPLDate(string jobcode, string jobnumber, string subjobno)
        {
            // Jika tanggal sekarang (server) lebih besar tanggal 15 bulan depan dari Job Entry Date (Shipment Order) maka IsValid = false,
            // Artinya EPL tidak bisa diedit lagi

            bool isValid = false;

            //            using (FEIEntities fe = new FEIEntities())
            //            {
            //                string sql = @"SELECT jobnumber, subjobno
            //                                FROM " + GeneralFunction.FileNameJob(jobcode) + @"
            //                                WHERE jobnumber = " + jobnumber + " AND subjobno = " + subjobno + @"
            //                                    AND GETDATE() < CAST(CAST(MONTH(DATEADD(month,1,EntryDate)) as VARCHAR) + '/16/' + CAST(YEAR(DATEADD(month,1,EntryDate)) as VARCHAR) as DATETIME)
            //	                                AND DATEDIFF(month, EntryDate, GETDATE()) < 2";
            //                var checkDate = fe.Database.SqlQuery<JobClass>(sql).FirstOrDefault();

            //                if (checkDate != null)
            //                    isValid = true;

            //            }

            return isValid;
        }

        // Indicate have Container
        public static int TypeEPL(int TotalSub, string LoadStatus, string ContainerStatus, int ServiceNo, int subJobNo, int jobCode)
        {
            int typeEPL = 0;
            // Sea
            if (jobCode == 10 || jobCode == 11)
            {
                if (LoadStatus == "FCL")
                {
                    if (ContainerStatus == "G")
                    {
                        typeEPL = 13;
                    }
                    else if (ContainerStatus == "N")
                    {
                        typeEPL = 1;
                    }
                    else if (ContainerStatus == "P")
                    {
                        if (ServiceNo == 1)
                        {
                            typeEPL = 8;
                        }
                        else if (ServiceNo == 2)
                        {
                            typeEPL = subJobNo == 0 ? 6 : 7;
                        }
                        else
                        {
                            typeEPL = subJobNo == 0 ? 4 : 5;
                        }
                    }
                }
                else
                {
                    if (ContainerStatus == "G")
                    {
                        typeEPL = 14;
                    }
                    else if (ContainerStatus == "P")
                    {
                        typeEPL = 9;
                    }
                    else if (ContainerStatus == "N")
                    {
                        typeEPL = TotalSub == 0 ? 10 : subJobNo == 0 ? 11 : 12;
                    }
                }
            }
            else
            {
                if (subJobNo == 0)
                {
                    typeEPL = ContainerStatus == "N" ? 0 : ContainerStatus == "G" ? 2 : 4;
                }
                else
                {
                    typeEPL = ContainerStatus == "N" ? 1 : ContainerStatus == "G" ? 3 : 5;
                }
            }
            return typeEPL;
        }
        /*
        public static List<EPLModels.ContainerSize> GetContanerSizeList(string jobNumber, string subJobNumber, string companyCode, string jobCode)
        {
            List<EPLModels.ContainerSize> cs = new List<EPLModels.ContainerSize>();
            string strSQL = "";
            try
            {
                using (FEIEntities fe = new FEIEntities())
                {
                    switch (jobCode)
                    {
                        case ConfigurationModels.JobCodeSeaImport:
                        case ConfigurationModels.JobCodeSeaExport:
                            strSQL = @"SELECT COUNT(Size) as SizeCount, Size as SizeType FROM SeaContainer 
                                    WHERE JobNumber = " + jobNumber + @" AND SubJobNo = " + subJobNumber +
                                        @"  AND CompanyCode = " + companyCode + @" AND JobCode = " + jobCode +
                                        @" GROUP BY JobNumber, SubJobNo, CompanyCode, JobCode, Size";
                            cs = fe.Database.SqlQuery<EPLModels.ContainerSize>(strSQL).ToList();
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                LOG.Error("GetContanerSizeList", ex);
            }

            return cs;
        }

        public static List<ShipmentOrderModels.ContainerSize> GetContanerSizeListShipmentOrder(string jobNumber, string subJobNumber, string companyCode, string jobCode)
        {
            List<ShipmentOrderModels.ContainerSize> cs = new List<ShipmentOrderModels.ContainerSize>();
            string strSQL = "";
            try
            {
                using (FEIEntities fe = new FEIEntities())
                {
                    switch (jobCode)
                    {
                        case ConfigurationModels.JobCodeSeaImport:
                        case ConfigurationModels.JobCodeSeaExport:
                            strSQL = @"SELECT COUNT(Size) as SizeCount, Size as SizeType FROM SeaContainer 
                                    WHERE JobNumber = " + jobNumber + @" AND SubJobNo = " + subJobNumber +
                                        @"  AND CompanyCode = " + companyCode + @" AND JobCode = " + jobCode +
                                        @" GROUP BY JobNumber, SubJobNo, CompanyCode, JobCode, Size";
                            cs = fe.Database.SqlQuery<ShipmentOrderModels.ContainerSize>(strSQL).ToList();
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                LOG.Error("GetContanerSizeList", ex);
            }

            return cs;
        }
        */
        /// <summary>
        /// 
        /// </summary>
        /// <param name="codeVal"> codeVal is numeric value and then convert as string</param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string Replicate(string codeVal, int length)
        {
            if (String.IsNullOrEmpty(codeVal))
            {
                return codeVal;
            }

            string result = "";
            for (int i = codeVal.Length; i < length; i++)
            {
                result += "0";
            }
            result += codeVal;

            return result;
        }

        public static String changeCurrencyToWords(decimal numb, bool isCurrency, string currencyName, string currencyPoints)
        {
            return changeToWords(numb.ToString(), isCurrency, currencyName, currencyPoints);
        }

        private static String changeToWords(String numb, bool isCurrency, string currencyName, string currencyPoints)
        {
            String val = "", wholeNo = numb, dPoints = "", andStr = "", pointStr = "";
            String endStr = ("");
            //String endStr = (isCurrency) ? ("Only") : ("");
            try
            {
                int decimalPlace = numb.IndexOf("."); if (decimalPlace > 0)
                {
                    wholeNo = numb.Substring(0, decimalPlace);
                    dPoints = numb.Substring(decimalPlace + 1);

                    if (Convert.ToInt32(dPoints) > 0)
                    {
                        andStr = ("POINT");// just to separate whole numbers from points,nps
                        endStr = (isCurrency) ? (endStr) : ("");
                        pointStr = changeNumbertoWords(dPoints);
                    }
                    else
                    {
                        andStr = ("POINT ZERO");// just to separate whole numbers from points,nps
                        endStr = (isCurrency) ? (endStr) : ("");
                        pointStr = changeNumbertoWords(dPoints);
                    }
                }
                val = String.Format("{0} {1} {2}{3} {4} {5}", currencyName, changeNumbertoWords(wholeNo).Trim(), andStr, pointStr, currencyPoints, endStr);
            }
            catch { ;} return val;
        }

        private static String changeNumbertoWords(String number)
        {
            string word = "";
            try
            {
                bool beginsZero = false;//tests for 0XX
                bool isDone = false;//test if already translated
                double dblAmt = (Convert.ToDouble(number));
                //if ((dblAmt > 0) && number.StartsWith("0"))

                if (dblAmt > 0)
                {//test for zero or digit zero in a nuemric
                    beginsZero = number.StartsWith("0");
                    int numDigits = number.Length;
                    int pos = 0;//store digit grouping
                    String place = "";//digit grouping name:hundres,thousand,etc..

                    switch (numDigits)
                    {
                        case 1://ones' range
                            word = ones(number);
                            isDone = true;
                            break;
                        case 2://tens' range
                            word = tens(number);
                            isDone = true;
                            break;
                        case 3://hundreds' range
                            pos = (numDigits % 3) + 1;
                            place = " HUNDRED ";
                            break;
                        case 4://thousands' range
                        case 5:
                        case 6:
                            pos = (numDigits % 4) + 1;
                            place = " THOUSAND ";
                            break;
                        case 7://millions' range
                        case 8:
                        case 9:
                            pos = (numDigits % 7) + 1;
                            place = " MILLION ";
                            break;
                        case 10://Billions's range
                            pos = (numDigits % 10) + 1;
                            place = " BILLION ";
                            break;
                        //add extra case options for anything above Billion..

                        default:
                            isDone = true;
                            break;
                    }

                    if (!isDone)
                    {//if transalation is not done, continue..(Recursion comes in now!!)
                        word = changeNumbertoWords(number.Substring(0, pos)) + place + changeNumbertoWords(number.Substring(pos));

                        //check for trailing zeros
                        //if (beginsZero) word = " and " + word.Trim();
                        if (beginsZero) word = " " + word.Trim();
                    }

                    //ignore digit grouping names
                    if (word.Trim().Equals(place.Trim())) word = "";
                }
            }
            catch { ;} return word.Trim();
        }

        private static String tens(String digit)
        {
            int digt = Convert.ToInt32(digit);
            String name = null; switch (digt)
            {
                case 10:
                    name = "TEN";
                    break;
                case 11:
                    name = "ELEVEN";
                    break;
                case 12:
                    name = "TWELVE";
                    break;
                case 13:
                    name = "THIRTEEN";
                    break;
                case 14:
                    name = "FOURTEEN";
                    break;
                case 15:
                    name = "FIFTEEN";
                    break;
                case 16:
                    name = "SIXTEEN";
                    break;
                case 17:
                    name = "SEVENTEEN";
                    break;
                case 18:
                    name = "EIGHTEEN";
                    break;
                case 19:
                    name = "NINETEEN";
                    break;
                case 20:
                    name = "TWENTY";
                    break;
                case 30:
                    name = "THIRTY";
                    break;
                case 40:
                    name = "FOURTY";
                    break;
                case 50:
                    name = "FIFTY";
                    break;
                case 60:
                    name = "SIXTY";
                    break;
                case 70:
                    name = "SEVENTY";
                    break;
                case 80:
                    name = "EIGHTY";
                    break;
                case 90:
                    name = "NINETY";
                    break;
                default:
                    if (digt > 0)
                    {
                        name = tens(digit.Substring(0, 1) + "0") + " " + ones(digit.Substring(1));
                    }
                    break;
            }
            return name;
        }

        private static String ones(String digit)
        {
            int digt = Convert.ToInt32(digit);
            String name = "";

            switch (digt)
            {
                case 1:
                    name = "ONE";
                    break;
                case 2:
                    name = "TWO";
                    break;
                case 3:
                    name = "THREE";
                    break;
                case 4:
                    name = "FOUR";
                    break;
                case 5:
                    name = "FIVE";
                    break;
                case 6:
                    name = "SIX";
                    break;
                case 7:
                    name = "SEVEN";
                    break;
                case 8:
                    name = "EIGHT";
                    break;
                case 9:
                    name = "NINE";
                    break;
            }
            return name;
        }

        /// <summary>
        /// Convert a List{T} to a DataTable.
        /// </summary>
        public static DataTable ToDataTable<T>(this IList<T> items)
        {
            var tb = new DataTable(typeof(T).Name);

            PropertyInfo[] props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (PropertyInfo prop in props)
            {
                Type t = GetCoreType(prop.PropertyType);
                tb.Columns.Add(prop.Name, t);
            }

            foreach (T item in items)
            {
                var values = new object[props.Length];

                for (int i = 0; i < props.Length; i++)
                {
                    values[i] = props[i].GetValue(item, null);
                }

                tb.Rows.Add(values);
            }

            return tb;
        }

        /// <summary>
        /// Determine of specified type is nullable
        /// </summary>
        public static bool IsNullable(Type type)
        {
            return !type.IsValueType || (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>));
        }

        /// <summary>
        /// Return underlying type if type is Nullable otherwise return the type
        /// </summary>
        public static Type GetCoreType(Type type)
        {
            if (type != null && IsNullable(type))
            {
                if (!type.IsValueType)
                {
                    return type;
                }
                else
                {
                    return Nullable.GetUnderlyingType(type);
                }
            }
            else
            {
                return type;
            }
        }

        /// <summary>
        /// Generate Where Clause On jqGrid
        /// </summary>
        public static string ConstructWhere(string filters)
        {
            string strWhere = "";
            string strOpr = "";
            string strAwal = "";
            string strAkhir = "";
            string strField = "";

            if (filters.Length > 0)
            {
                dynamic json = JObject.Parse(filters);
                JArray jsonArray = json.rules;

                if (json.rules.Count > 0)
                {
                    for (int i = 0; i < jsonArray.Count; i++)
                    {
                        dynamic isifilter = JObject.Parse(jsonArray[i].ToString());
                        switch ((string)isifilter.op)
                        {
                            case "eq":
                                strOpr = " = ";
                                strAwal = "'";
                                strAkhir = "'";
                                break;
                            case "ne":
                                strOpr = " <> ";
                                strAwal = "'";
                                strAkhir = "'";
                                break;
                            case "lt":
                                strOpr = " < ";
                                strAwal = "'";
                                strAkhir = "'";
                                break;
                            case "le":
                                strOpr = " <= ";
                                strAwal = "'";
                                strAkhir = "'";
                                break;
                            case "gt":
                                strOpr = " > ";
                                strAwal = "'";
                                strAkhir = "'";
                                break;
                            case "ge":
                                strOpr = " >= ";
                                strAwal = "'";
                                strAkhir = "'";
                                break;
                            case "bw":
                            case "ew":
                            case "cn":
                                strOpr = " LIKE ";
                                strAwal = "'%";
                                strAkhir = "%'";
                                break;
                            case "bn":
                            case "en":
                            case "nc":
                                strOpr = " NOT LIKE ";
                                strAwal = "'%";
                                strAkhir = "%'";
                                break;
                            case "in":
                                strOpr = " IN ";
                                strAwal = "'";
                                strAkhir = "'";
                                break;
                            case "ni":
                                strOpr = " NOT IN ";
                                strAwal = "'";
                                strAkhir = "'";
                                break;
                            case "nu":
                                strOpr = " IS NULL ";
                                strAwal = "";
                                strAkhir = "";
                                break;
                            case "nn":
                                strOpr = "IS NOT NULL";
                                strAwal = "";
                                strAkhir = "";
                                break;
                            default:
                                strOpr = "";
                                strAwal = "";
                                strAkhir = "";
                                break;
                        }

                        if (isifilter.field.ToString().ToUpper().Contains("DATE"))
                        {
                            strField = "convert(varchar(10)," + isifilter.field + ",120)";
                        }
                        else
                        {
                            strField = isifilter.field;
                        }

                        if (i == 0)
                        {
                            strWhere = strField + strOpr + strAwal + isifilter.data + strAkhir;
                        }
                        else
                        {
                            strWhere = strWhere + " AND " + strField + strOpr + strAwal + isifilter.data + strAkhir;
                        }
                    }
                    //strWhere = " WHERE " + strWhere;
                }
            }
            return strWhere;
        }

        public static void ConstructWhereInLinq(string whereClause, out string filter, out List<dynamic> filterValues)
        {
            filter = "";
            filterValues = new List<dynamic>();

            string[] specChars = new string[] { "<", ">", "<=", ">=", "=" };//, "like", "in", "between", "or", "and", "(", ")", "where" };

            try
            {
                string[] conditions = Regex.Split(whereClause.Trim(), "AND");
                for (int i = 0; i < conditions.Length; i++)
                {
                    if (!String.IsNullOrEmpty(conditions[i]))
                    {
                        string[] temp = conditions[i].TrimEnd().TrimStart().Split(' ');
                        // 0 : fieldName
                        // 1 : operator
                        // 2 : fieldValue

                        bool useLike = false;
                        if (temp[1].Contains("LIKE"))
                        {
                            useLike = true;

                            // replace
                            temp[1] = "=";
                        }

                        //string[] conditionStatement = Regex.Split(conditions[i], useLike ? "LIKE" : "=");

                        string filterValue = "";
                        if (useLike)
                            filterValue = temp[2].Trim().Substring(2, temp[2].Trim().Length - 4);
                        else
                            filterValue = temp[2].Trim().Substring(1, temp[2].Trim().Length - 2);

                        int value = 0;
                        DateTime date = DateTime.Now;
                        bool boolValue = false;

                        // FieldValue as Numeric
                        if (int.TryParse(filterValue, out value))
                        {
                            filterValues.Add(value);
                            filter += temp[0].Trim() + " " + temp[1].Trim() + " @" + i.ToString() + "";
                        }
                        // FieldValue as Date
                        else if (DateTime.TryParse(filterValue, out date))
                        {
                            filterValues.Add(date);
                            filter += temp[0].Trim() + " " + temp[1].Trim() + " @" + i.ToString();
                        }
                        // FieldValue as Boolean
                        else if (bool.TryParse(filterValue, out boolValue))
                        {
                            filterValues.Add(boolValue);
                            filter += temp[0].Trim() + " " + temp[1].Trim() + " @" + i.ToString() + "";
                        }
                        // FieldValue as Alphabetical
                        else
                        {
                            filterValues.Add(filterValue);
                            filter += temp[0].Trim() + ".Contains(@" + i.ToString() + ")";
                        }

                        if (i < conditions.Length - 1)
                            filter += " AND ";
                    }
                }
                //LOG.Debug(filter + "===" + filterValues.Count);
            }
            catch (Exception ex)
            {
                LOG.Error("", ex);
            }
        }

        public static void ConstructWhereInLinq(string whereClause, out string filter, out List<DateTime> filterDateValues)
        {
            filter = "";
            filterDateValues = new List<DateTime>();

            string[] specChars = new string[] { "<", ">", "<=", ">=", "=" };//, "like", "in", "between", "or", "and", "(", ")", "where" };

            try
            {
                string[] conditions = Regex.Split(whereClause.Trim(), "AND");
                for (int i = 0; i < conditions.Length; i++)
                {
                    if (!String.IsNullOrEmpty(conditions[i]))
                    {
                        string[] temp = conditions[i].TrimEnd().TrimStart().Split(' ');
                        string[] split = conditions[i].TrimEnd().TrimStart().Split(' ');
                        string actualtext = conditions[i].Substring(split[0].Length + split[1].Length + 1);
                        string actualcut = actualtext.Trim().Substring(2, actualtext.Trim().Length - 4);
                        string compare = temp[2].Trim().Substring(2, temp[2].Trim().Length - 4);
                        // 0 : fieldName
                        // 1 : operator
                        // 2 : fieldValue

                        bool useLike = false;
                        if (temp[1].Contains("LIKE"))
                        {
                            useLike = true;

                            // replace
                            temp[1] = "=";
                        }

                        //string[] conditionStatement = Regex.Split(conditions[i], useLike ? "LIKE" : "=");

                        string filterValue = "";
                        if (useLike)
                            filterValue = actualcut;
                        else
                            filterValue = temp[2].Trim().Substring(1, temp[2].Trim().Length - 2);

                        int value = 0;
                        DateTime date = DateTime.Now;
                        bool boolValue = false;

                        // FieldValue as Numeric
                        if (int.TryParse(filterValue, out value))
                        {
                            //filterDateValues.Add(value);
                            filter += temp[0].Trim() + " " + temp[1].Trim() + ".Contains(\"" + value.ToString() + "\")";
                        }
                        // FieldValue as Date
                        else if (DateTime.TryParse(filterValue, out date))
                        {
                            filterDateValues.Add(date);
                            filter += temp[0].Trim() + " " + temp[1].Trim() + " @" + i.ToString();
                        }
                        // FieldValue as Boolean
                        else if (bool.TryParse(filterValue, out boolValue))
                        {
                            //filterDateValues.Add(boolValue);
                            filter += temp[0].Trim() + " " + temp[1].Trim() + boolValue.ToString() + "";
                        }
                        // FieldValue as Alphabetical
                        else
                        {
                            //filterDateValues.Add(filterValue);
                            filter += temp[0].Trim() + ".Contains(\"" + filterValue + "\")";
                        }

                        if (i < conditions.Length - 1)
                            filter += " AND ";
                    }
                }
                //LOG.Debug(filter + "===" + filterValues.Count);
            }
            catch (Exception ex)
            {
                LOG.Error("", ex);
            }
        }

        // Make sure you are using System.Linq.Dynamic in order to use the output filter string inside Where clause/method
        public static void ConstructWhereInLinq(string whereClause, out string filter)
        {
            filter = "";
            //List<dynamic> filterValues = new List<dynamic>();

            string[] specChars = new string[] { "<", ">", "<=", ">=", "=" };//, "like", "in", "between", "or", "and", "(", ")", "where" };

            try
            {
                string[] conditions = Regex.Split(whereClause.Trim(), "AND");
                for (int i = 0; i < conditions.Length; i++)
                {
                    if (!String.IsNullOrEmpty(conditions[i]))
                    {
                        string[] temp = conditions[i].TrimEnd().TrimStart().Split(' ');
                        string[] split = conditions[i].TrimEnd().TrimStart().Split(' ');
                        string actualtext = conditions[i].Substring(split[0].Length + split[1].Length + 2);
                        string actualcut = actualtext.Trim().Substring(2, actualtext.Trim().Length - 4);
                        string compare = temp[2].Trim().Substring(2, temp[2].Trim().Length - 4);
                        // 0 : fieldName
                        // 1 : operator
                        // 2 : fieldValue

                        bool useLike = false;
                        if (temp[1].Contains("LIKE"))
                        {
                            useLike = true;

                            // replace
                            temp[1] = "=";
                        }

                        //string[] conditionStatement = Regex.Split(conditions[i], useLike ? "LIKE" : "=");

                        string filterValue = "";
                        if (useLike)
                            filterValue = actualcut;
                        else
                            filterValue = temp[2].Trim().Substring(1, temp[2].Trim().Length - 2);

                        int value = 0;
                        DateTime date = DateTime.Now;
                        bool boolValue = false;

                        // FieldValue as Numeric
                        if (int.TryParse(filterValue, out value))
                        {
                            //filterValues.Add(value);
                            filter += temp[0].Trim() + " " + temp[1].Trim() + value.ToString() + "";
                        }
                        // FieldValue as Date
                        else if (DateTime.TryParse(filterValue, out date))
                        {
                            //filterValues.Add(date);
                            filter += temp[0].Trim() + " " + temp[1].Trim() + "'" + date.ToShortDateString().ToLower() + "'"; // Still Buggy
                        }
                        // FieldValue as Boolean
                        else if (bool.TryParse(filterValue, out boolValue))
                        {
                            //filterValues.Add(boolValue);
                            filter += temp[0].Trim() + " " + temp[1].Trim() + boolValue.ToString().ToLower() + "";
                        }
                        // FieldValue as Alphabetical
                        else
                        {
                            //filterValues.Add(filterValue);
                            filter += temp[0].Trim() + ".ToLower().Contains(\"" + filterValue.ToLower() + "\")";
                        }

                        if (i < conditions.Length - 1)
                            filter += " AND ";
                    }
                }
                //LOG.Debug(filter + "===" + filterValues.Count);
            }
            catch (Exception ex)
            {
                LOG.Error("", ex);
            }
        }

        

    }
}