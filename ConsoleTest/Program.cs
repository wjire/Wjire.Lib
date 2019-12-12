using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Reflection;
using System.Text;

namespace ConsoleTest
{
    internal class Program
    {
        private static void Main(string[] args)
        {

            Console.ReadKey();
        }


        private static void TestUnPack()
        {
            string source = @"C:\Users\gongwei\Desktop\source";
            string file = @"C:\Users\gongwei\Desktop\copy\copy.zip";
            FastZip fz = new FastZip { CreateEmptyDirectories = true };
            fz.CreateZip(file, source, true, "");
            string newPath = @"C:\Users\gongwei\Desktop\copy\";
            using (FileStream fs = File.OpenRead(file))
            {
                UnpackFiles(fs, newPath);
            }
            Console.WriteLine("over");
        }


        /// <summary>
        /// 解压缩
        /// </summary>
        /// <param name="stream">待解压文件流</param>
        /// <param name="newPath"> 解压到哪个目录中(包含物理路径)</param>
        private static void UnpackFiles(Stream stream, string newPath)
        {
            using (ZipInputStream zipStream = new ZipInputStream(stream))
            {
                ZipEntry theEntry;
                while ((theEntry = zipStream.GetNextEntry()) != null)
                {
                    string directoryName = Path.GetDirectoryName(theEntry.Name);
                    string fileName = Path.GetFileName(theEntry.Name);
                    if (directoryName != string.Empty)
                    {
                        Directory.CreateDirectory(newPath + directoryName);
                    }

                    if (fileName == string.Empty)
                    {
                        continue;
                    }

                    using (FileStream streamWriter = File.Create(newPath + theEntry.Name))
                    {
                        byte[] data = new byte[2048];
                        while (true)
                        {
                            int size = zipStream.Read(data, 0, data.Length);
                            if (size > 0)
                            {
                                streamWriter.Write(data, 0, size);
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                }
            };
        }


        private static void FileTest(string path)
        {
            string[] files = Directory.GetFiles(path, "*.exe");
            if (files.Count() == 1)
            {
                string basePath = Path.GetDirectoryName(path);
                Console.WriteLine(Path.Combine(basePath, Path.GetFileNameWithoutExtension(files[0])));
            }
        }



        private static void ProcessTest(string path)
        {
            Process.Start(new ProcessStartInfo(path)
            {
                WorkingDirectory = @"C:\Users\Administrator\Desktop\RPC\Test1\1.1.0.2",
            });
        }


        private static void ProcessTest2(string path)
        {
            Process proc = new Process();
            string output = null;
            try
            {
                proc.StartInfo.FileName = "cmd.exe";

                //是否使用操作系统shell启动
                proc.StartInfo.UseShellExecute = false;

                //接受来自调用程序的输入信息
                proc.StartInfo.RedirectStandardInput = true;

                //输出信息
                proc.StartInfo.RedirectStandardOutput = true;

                //输出错误
                proc.StartInfo.RedirectStandardError = true;

                //不显示程序窗口
                proc.StartInfo.CreateNoWindow = true;

                proc.StartInfo.WorkingDirectory = @"C:\Users\Administrator\Desktop\RPC\Test1\1.1.0.2";

                proc.Start();

                //构造命令
                StringBuilder sb = new StringBuilder();

                sb.Append($@"service.cmd");
                //退出
                sb.Append("&exit");

                //向cmd窗口发送输入信息
                proc.StandardInput.WriteLine(sb.ToString());
                proc.StandardInput.AutoFlush = true;

                output = proc.StandardOutput.ReadToEnd();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                proc.WaitForExit();
                proc.Close();
                Console.WriteLine(output);
            }
        }



        private static void DirTest(string name)
        {
            string basePath = @"C:\\Users\\Administrator\\Desktop\\RPC";
            string path = Path.Combine(basePath, name);
            string[] dirs = Directory.GetDirectories(path);

            List<Version> versions = new List<Version>();
            foreach (string item in dirs)
            {
                string versionString = Path.GetFileName(item);
                versions.Add(new Version(versionString));
            }

            Version max = versions.Max();
            string[] arr = max.ToString().Split(".");
            int lastNumber = Convert.ToInt32(arr[arr.Length - 1]);
            arr[arr.Length - 1] = (++lastNumber).ToString();
            string newVersionString = string.Join(".", arr);
            string dir = Path.Combine(path, newVersionString);
            if (Directory.Exists(dir) == false)
            {
                Directory.CreateDirectory(dir);
            }
        }



        private static void WindowsServiceTest2()
        {
            string path = @"H:\gongwei\servers\Redis\";
            string sql = $"SELECT * FROM Win32_Service ";
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(sql);

            foreach (ManagementBaseObject mo in searcher.Get())
            {
                string pathName = mo["PathName"]?.ToString();
                if (string.IsNullOrWhiteSpace(pathName) == false)
                {
                    if (mo["PathName"].ToString().Contains(path))
                    {
                        Console.WriteLine(mo["PathName"]);
                    }
                }
            }
        }



        private static void WindowsServiceTest()
        {
            string[] lvData = new string[6];
            //ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_Service where Name like '%redis%'");
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_Service where ProcessId = 15832 ");

            foreach (ManagementObject mo in searcher.Get())
            {
                Console.WriteLine(mo["PathName"]);
                Console.WriteLine(Path.GetDirectoryName(mo["PathName"].ToString()));
                Console.WriteLine(Path.GetDirectoryName(Path.GetDirectoryName(mo["PathName"].ToString())));
                //foreach (PropertyData property in mo.Properties)
                //{
                //    Console.WriteLine(property.Name);
                //}

                break;
                //Console.WriteLine(mo.Path);
                //Console.WriteLine(mo.ClassPath);
                //Console.WriteLine(mo["Name"].ToString());

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

        }
    }
}
