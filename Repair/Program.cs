﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Service;

namespace Repair
{
    class Program
    {

        static void Main(string[] args)
        {
            // Redirect Console to Text file
            FileStream ostrm = null;
            StreamWriter writer = null;
            TextWriter oldOut = Console.Out;
            try
            {
                if (File.Exists(@"./ConsoleLog.txt"))
                {
                    File.Delete(@"./ConsoleLog.txt");
                }
                Console.WriteLine("Generating ConsoleLog.txt file....");
                ostrm = new FileStream(@"./ConsoleLog.txt", FileMode.OpenOrCreate, FileAccess.Write);
                writer = new StreamWriter(ostrm);
                Console.SetOut(writer);
            }
            catch (Exception e)
            {
                //Console.SetOut(oldOut);
                Console.WriteLine("Cannot open ConsoleLog.txt for writing");
                Console.WriteLine(e.Message);
                //return;
            }

            //for (int i = 1; i < 400; i++) Console.WriteLine(i);
            //Console.ReadLine();
            RepairFunctions rb = new RepairFunctions();
            Console.WriteLine("Delete ByProducts...");
            rb.DeleteByProducts(null, null);
            //Console.WriteLine("BackUp...");
            //rb.BackUp(null, null);
            //Console.WriteLine("Delete Original...");
            //rb.DeleteOriginal(null, null);
            //rb.masterDatas.Reverse();
            Console.WriteLine("Reconfirm...");
            rb.ReConfirm(null, null);
            Console.WriteLine("Done");

            // Restore original Console
            Console.SetOut(oldOut);
            writer.Close();
            ostrm.Close();
            //Console.ReadLine();
        }
    }
}
