using ICSharpCode.SharpZipLib.Zip;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Windows.Forms;
using Wjire.ProjectManager.Model;
using Wjire.ProjectManager.Service;

namespace Wjire.ProjectManager
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            BindDataGridView();
        }


        private void BindDataGridView()
        {
            List<AppInfo> source = new DbService().GetAllAppInfo();
            dgv.DataSource = source;
        }



        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                AddForm addForm = new AddForm();
                addForm.ShowDialog();
                if (addForm.DialogResult == DialogResult.OK)
                {
                    BindDataGridView();
                }
            }
            catch (Exception ex)
            {
                ShowMsg(ex.ToString());
                return;
            }
        }



        /// <summary>
        /// 发布
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click_1(object sender, EventArgs e)
        {
            if (dgv.SelectedRows.Count == 0)
            {
                return;
            }

            PublishInfo publishInfo = CreatePublishInfo(dgv.SelectedRows[0]);
            bool publishResult = PublishApp(publishInfo);
            if (publishResult)
            {
                MessageBox.Show("成功");
            }
            else
            {
                MessageBox.Show("失败");
            }
        }


        private PublishInfo CreatePublishInfo(DataGridViewRow row)
        {
            PublishInfo publishInfo = new PublishInfo
            {
                AppInfo = new AppInfo
                {
                    AppName = row.Cells["AppName"].Value.ToString(),
                    LocalPath = row.Cells["LocalPath"].Value.ToString(),
                    AppType = Convert.ToInt32(row.Cells["AppType"].Value.ToString()),
                    AppId = Convert.ToInt32(row.Cells["AppId"].Value.ToString()),
                    ServerAddress = row.Cells["ServerAddress"].Value.ToString(),
                },
            };
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $@"upload\{publishInfo.AppInfo.AppName}");
            if (Directory.Exists(path) == false)
            {
                Directory.CreateDirectory(path);
            }

            publishInfo.FileName = Path.Combine(path, $"{publishInfo.AppInfo.AppName}.zip");

            publishInfo.FileFilter = null;
            publishInfo.DirFilter = @"RPCConfig\w*";
            return publishInfo;
        }



        private bool PublishApp(PublishInfo publishInfo)
        {
            try
            {
                bool res = PublishDll(publishInfo.AppInfo);
                if (res == false)
                {
                    return false;
                }
                Pack(publishInfo);
                Upload(publishInfo);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }



        /// <summary>
        /// 发布dll
        /// </summary>
        private bool PublishDll(AppInfo appInfo)
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

                //指定cmd命令执行得路径
                //proc.StartInfo.WorkingDirectory = Info.AppInfo.LocalPath;

                proc.Start();

                //构造命令
                string command = GetCommand(appInfo);

                //向cmd窗口发送输入信息
                proc.StandardInput.WriteLine(command);
                proc.StandardInput.AutoFlush = true;

                output = proc.StandardOutput.ReadToEnd();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                proc.WaitForExit();
                proc.Close();
                Console.WriteLine(output);
            }
        }


        /// <summary>
        /// 压缩
        /// </summary>
        private void Pack(PublishInfo publishInfo)
        {
            FastZip fz = new FastZip { CreateEmptyDirectories = true };
            fz.CreateZip(publishInfo.FileName, $@"publish\{publishInfo.AppInfo.AppName}", true, publishInfo.FileFilter, publishInfo.DirFilter);
        }



        /// <summary>
        /// 上传
        /// </summary>
        private string Upload(PublishInfo publishInfo)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(publishInfo.AppInfo.ServerAddress);
                //client.BaseAddress = new Uri("http://localhost:52635");
                string apiUrl = "api/publish/upload";
                MultipartFormDataContent content = new MultipartFormDataContent();
                FileStream fs = new FileStream(publishInfo.FileName, FileMode.Open, FileAccess.Read);
                content.Add(new StreamContent(fs), "file", Path.GetFileName(publishInfo.FileName));
                content.Add(new StringContent(JsonConvert.SerializeObject(publishInfo.AppInfo)), nameof(AppInfo));
                HttpResponseMessage result = client.PostAsync(apiUrl, content).Result;
                if (result.IsSuccessStatusCode == false)
                {
                    Console.WriteLine(result.Content);
                }
                fs.Dispose();
                return result.Content.ReadAsStringAsync().Result;
            }
        }


        private string GetCommand(AppInfo appInfo)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($@"dotnet publish {appInfo.LocalPath} -c release -o publish\{appInfo.AppName} ");
            if (appInfo.AppType == 2)
            {
                //dotnet publish -c release -o test --runtime win-x64 --self-contained false
                sb.Append("--runtime win-x64 --self-contained false");
            }
            sb.Append("&exit");
            return sb.ToString();
        }



        private DialogResult ShowMsg(string msg)
        {
            return MessageBox.Show(msg);
        }


        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgv.SelectedRows.Count == 0)
                {
                    return;
                }

                DataGridViewRow row = dgv.SelectedRows[0];
                AppInfo appInfo = new AppInfo
                {
                    AppId = Convert.ToInt64(row.Cells["AppId"].Value),
                    AppName = row.Cells["AppName"].Value.ToString(),
                    LocalPath = row.Cells["LocalPath"].Value.ToString()
                };

                UpdateForm updateForm = new UpdateForm(appInfo);
                updateForm.ShowDialog();
                if (updateForm.DialogResult == DialogResult.OK)
                {
                    BindDataGridView();
                }
            }
            catch (Exception ex)
            {
                ShowMsg(ex.Message);
            }
        }



        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_deleteApp_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgv.SelectedRows.Count == 0)
                {
                    return;
                }

                DialogResult result = MessageBox.Show("确定删除吗？", "删除", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    long appId = Convert.ToInt64(dgv.SelectedRows[0].Cells["AppId"].Value);
                    new DbService().Delete(appId);
                    ShowMsg("删除成功");
                    BindDataGridView();
                }
                else { return; }
            }
            catch (Exception ex)
            {
                ShowMsg(ex.Message);
            }
        }



        private void btn_clearAll_Click(object sender, EventArgs e)
        {
            try
            {
                new DbService().Delete();
                ShowMsg("删除成功");
            }
            catch (Exception ex)
            {
                ShowMsg(ex.Message);
            }
        }
    }
}
