using Dapper;
using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Wjire.ProjectManager.Model;

namespace Wjire.ProjectManager
{
    public partial class Form1 : Form
    {

        private string _connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
        private string _basePublishPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"\temp\");

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
            Process process = new Process();
        }


        private IPublishHandler GetPublishHandler()
        {
            return new WebPublishHandler();
        }

        
        /// <summary>
        /// 读取所有的项目
        /// </summary>
        /// <returns></returns>
        private async Task<List<ProjectInfo>> GetProjectInfo()
        {
            return await Task.Run(() =>
             {
                 string sql = "SELECT * FROM ProjectInfo";
                 using (SQLiteConnection db = new SQLiteConnection(_connectionString))
                 {
                     return db.Query<ProjectInfo>(sql).ToList();
                 }
             });
        }


        /// <summary>
        /// 添加项目
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        private int AddProjectInfo(ProjectInfo info)
        {
            string sql = "INSERT INTO ProjectInfo VALUES (@ProjectName,@ProjectDir,@ProjectType)";
            using (SQLiteConnection db = new SQLiteConnection(_connectionString))
            {
                return db.Execute(sql, info);
            }
        }


        /// <summary>
        /// 发布
        /// </summary>
        private void Publish(ProjectInfo info)
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
                StringBuilder sb = new StringBuilder();

                sb.Append($@"dotnet publish -c release -o publish\{info.ProjectName}");
                //退出
                sb.Append("&exit");

                //向cmd窗口发送输入信息
                proc.StandardInput.WriteLine(sb.ToString());
                proc.StandardInput.AutoFlush = true;

                strOuput = proc.StandardOutput.ReadToEnd();
            }
            catch (Exception)
            {

            }
            finally
            {
                proc.WaitForExit();
                proc.Close();
                Console.WriteLine(strOuput);
            }
        }


        /// <summary>
        /// 压缩
        /// </summary>
        /// <param name="info"></param>
        /// <param name="fileFilter"></param>
        /// <param name="fileName"></param>
        private void Pack(ProjectInfo info, string fileFilter, out string fileName)
        {
            fileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $@"upload\{info.ProjectName}\{info.ProjectName}.zip");
            FastZip fz = new FastZip { CreateEmptyDirectories = true };
            fz.CreateZip(fileName, info.ProjectDir, true, fileFilter);
            fz = null;
            //SharpZip.PackFiles(fileName, info.ProjectDir);
        }



        /// <summary>
        /// 上传
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="info"></param>
        private string Upload(string fileName, ProjectInfo info)
        {
            string resultStr = string.Empty;
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:9527");
                string apiUrl = "api/upload/upload";
                MultipartFormDataContent content = new MultipartFormDataContent();
                FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                content.Add(new StreamContent(fs));
                content.Add(new StringContent(info.ProjectName), nameof(info.ProjectName));
                HttpResponseMessage result = client.PostAsync(apiUrl, content).Result;
                fs.Dispose();
                resultStr = result.Content.ReadAsStringAsync().Result;
                return resultStr;
            }
        }
    }
}
