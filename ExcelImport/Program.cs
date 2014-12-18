using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelImport
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Processing...");
            var conv = new ConversionFunction();
            conv.ImportDataFromExcel(@"D:\Other\20141218 TemplateZGA.xlsx");
            Console.WriteLine("Done.");
            Console.ReadLine();
        }
    }
}
