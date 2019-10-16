using System;
using System.Management;

namespace ConsoleTest
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            string[] lvData = new string[6];
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_Service");
            foreach (ManagementObject mo in searcher.Get())
            {
                lvData[0] = mo["Name"].ToString();
                lvData[1] = mo["DisplayName"].ToString();
                lvData[2] = mo["StartMode"].ToString();
                if (mo["Started"].Equals(true))
                {
                    lvData[3] = "Started";
                }
                else
                {
                    lvData[3] = "Stop";
                }
                //lvData[4] = mo["PathName"]?.ToString();//StartName
                lvData[5] = mo["StartName"]?.ToString() ?? "null";//StartName
                Console.WriteLine(lvData[0] + " === " + lvData[1] + " === " + lvData[2] + " === " + lvData[3] + " ===" + lvData[5]);
            }

            Console.ReadKey();
        }
    }
}
