using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Windows.Forms;
using Wjire.ProjectManager.Model;
using Wjire.ProjectManager.Service;

namespace Wjire.ProjectManager
{
    public partial class AddForm : Form
    {

        public AddForm()
        {
            InitializeComponent();
        }


        private void ProjectAddForm_Load(object sender, EventArgs e)
        {
            BindServerAddress();
        }


        /// <summary>
        /// 绑定服务器地址
        /// </summary>
        private void BindServerAddress()
        {
            List<string> servers = GetServerAddress();
            cbx_serverAddress.Items.Clear();
            foreach (string server in servers)
            {
                cbx_serverAddress.Items.Add(server);
            }
        }



        /// <summary>
        /// 确认
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(cbx_serverAddress.Text))
                {
                    ShowMsg("服务器地址不能为空");
                    return;
                }
                if (string.IsNullOrWhiteSpace(cbx_app.Text))
                {
                    ShowMsg("程序名称不能为空");
                    return;
                }
                if (string.IsNullOrWhiteSpace(tbx_localPath.Text))
                {
                    ShowMsg("本地地址不能为空");
                    return;
                }

                AppInfo appInfo = cbx_app.SelectedItem as AppInfo;
                appInfo.LocalPath = tbx_localPath.Text;
                appInfo.AppType = GetAppType();
                appInfo.ServerAddress = cbx_serverAddress.Text;
                new DbService().Add(appInfo);
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                DialogResult = DialogResult.Abort;
                ShowMsg(ex.Message);
            }
        }


        /// <summary>
        /// 取消
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Abort;
            Close();
        }



        /// <summary>
        /// 选择本地路径
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_chooseProjectDir_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog
            {
                Description = "请选择项目所在文件夹",
            };
            if (dialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            if (string.IsNullOrEmpty(dialog.SelectedPath))
            {
                MessageBox.Show(this, "文件夹路径不能为空", "提示");
                return;
            }

            tbx_localPath.Text = dialog.SelectedPath;
        }



        /// <summary>
        /// 切换 IIS/EXE
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rbn_exe_CheckedChanged(object sender, EventArgs e)
        {
            if (cbx_serverAddress.SelectedItem == null)
            {
                return;
            }
            BindApp();
        }


        /// <summary>
        /// 获取所有服务器地址
        /// </summary>
        /// <returns></returns>
        private List<string> GetServerAddress()
        {
            string appSettings = ConfigurationManager.AppSettings["serverAddress"];
            return appSettings.Split(',').ToList();
        }


        /// <summary>
        /// 切换服务器
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbx_serverAddress_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindApp();
        }



        /// <summary>
        /// 填充App下拉
        /// </summary>
        private void BindApp()
        {
            List<AppInfo> appInfos = GetAppInfosFromServer();
            BindApp(appInfos);
        }


        /// <summary>
        /// 从服务器获取App
        /// </summary>
        /// <returns></returns>
        private List<AppInfo> GetAppInfosFromServer()
        {
            int type = GetAppType();
            Uri uri = new Uri(cbx_serverAddress.SelectedItem.ToString());
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = uri;
                string apiUrl = "api/publish/getAppInfos?type=" + type;
                string result = client.GetStringAsync(apiUrl).Result;
                return JsonConvert.DeserializeObject<List<AppInfo>>(result);
            }
        }


        /// <summary>
        /// 填充App下拉
        /// </summary>
        /// <param name="appInfoViews"></param>
        private void BindApp(List<AppInfo> appInfoViews)
        {
            cbx_app.Items.Clear();
            foreach (AppInfo item in appInfoViews)
            {
                cbx_app.Items.Add(item);
            }
            cbx_app.Text = appInfoViews.FirstOrDefault()?.ToString();
        }


        /// <summary>
        /// 获取APP类型
        /// </summary>
        /// <returns></returns>
        private int GetAppType()
        {
            return rbn_iis.Checked ? 1 : 2;
        }


        private void ShowMsg(string msg)
        {
            MessageBox.Show(msg);
        }
    }
}
