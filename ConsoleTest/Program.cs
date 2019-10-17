using System;
using System.Management;
using System.Linq;

namespace ConsoleTest
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            string[] lvData = new string[6];
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_Service where Name like '%redis%'");
                               
            foreach (ManagementObject mo in searcher.Get())
            {
                //Console.WriteLine(mo.Path);
                //Console.WriteLine(mo.ClassPath);
                Console.WriteLine(mo["Name"].ToString());
                
                //foreach (var item in mo.Properties)
                //{                    
                //    Console.WriteLine("\t" + item.Name +"="+item.Value?.ToString());
                //}


                //Console.WriteLine(mo["ProcessId"]);

                //lvData[0] = mo["Name"].ToString();
                //lvData[1] = mo["DisplayName"].ToString();
                //lvData[2] = mo["StartMode"].ToString();
                //if (mo["Started"].Equals(true))
                //{
                //    lvData[3] = "Started";
                //}
                //else
                //{
                //    lvData[3] = "Stop";
                //}
                ////lvData[4] = mo[""]?.ToString();//StartName
                //lvData[5] = mo["StartName"]?.ToString() ?? "null";//StartName
                //Console.WriteLine(lvData[0] + " === " + lvData[1] + " === " + lvData[2] + " === " + lvData[3] + " ===" + lvData[4]+" ===" + lvData[5]);
            }

            Console.ReadKey();
        }
    }
}
