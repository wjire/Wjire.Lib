using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Dapper;
using Wjire.ProjectManager.Model;

namespace Wjire.ProjectManager
{
    public partial class Form1 : Form
    {

        private string _connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //var fileName = @"C:\Users\gongwei\Desktop\test.zip";
            //var dir = @"H:\gongwei\fabu\Admin";
            //SharpZip.PackFiles(fileName, dir);


            AddProjectInfo(new ProjectInfo
            {
                ProjectDir = "1",
                ProjectName = "2",
                ProjectType = 1
            });

            MessageBox.Show("OK");
        }



        private void BuildProject(ProjectInfo info)
        {
            var process = new Process();
        }


        private void Pack(ProjectInfo info, out string fileName)
        {
            fileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $@"\project\{info.ProjectName}.zip");
            SharpZip.PackFiles(fileName, info.ProjectDir);
        }



        private void Upload(string fileName, ProjectInfo info)
        {
            var resultStr = string.Empty;
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:9527");
                string apiUrl = "api/upload/upload";
                MultipartFormDataContent content = new MultipartFormDataContent();
                FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                content.Add(new StreamContent(fs));
                content.Add(new StringContent(info.ProjectName), nameof(info.ProjectName));
                var result = client.PostAsync(apiUrl, content).Result;
                fs.Dispose();
                //resultStr = result.Content.ReadAsStringAsync().Result;
                //return resultStr;
            }
        }


        private IPublishHandler GetPublishHandler()
        {
            return new WebPublishHandler();
        }


        private int AddProjectInfo(ProjectInfo info)
        {
            var sql = "INSERT INTO ProjectInfo VALUES (@ProjectName,@ProjectDir,@ProjectType)";
            using (var db = new SQLiteConnection(_connectionString))
            {
                return db.Execute(sql, info);
            }
        }


        private async Task<List<ProjectInfo>> GetProjectInfo()
        {
            return await Task.Run(() =>
             {
                 var sql = "SELECT * FROM ProjectInfo";
                 using (var db = new SQLiteConnection(_connectionString))
                 {
                     return db.Query<ProjectInfo>(sql).ToList();
                 }
             });
        }


        /// <summary>
        /// 执行 windows cmd 命令
        /// </summary>
        private void ProcessCmd()
        {
            Process proc = new Process();
            string strOuput = null;
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

                proc.Start();

                //构造命令
                var sb = new StringBuilder();

                //如果选择 不发布依赖项
                if (rdoNo.Checked)
                {
                    if (File.Exists($"{projectDir}packages.config"))
                    {
                        //因为不发布依赖项,所以需要移动 packages.config 文件
                        sb.Append($"move {projectDir}packages.config {toolPath}");
                        sb.Append("&&");
                    }
                    sb.Append(CreateCmd());

                    //还原 packages.config 文件
                    sb.Append($"&move {toolPath}\\packages.config {projectDir}");
                }
                else
                {
                    sb.Append(CreateCmd());
                }

                //退出
                sb.Append("&exit");

                //向cmd窗口发送输入信息
                proc.StandardInput.WriteLine(sb.ToString());
                proc.StandardInput.AutoFlush = true;

                strOuput = proc.StandardOutput.ReadToEnd();
            }
            catch (Exception ex)
            {
                txtMsg.Text = ex.Message;
            }
            finally
            {
                proc.WaitForExit();
                proc.Close();
                Console.WriteLine(strOuput);
            }
        }
    }
}
