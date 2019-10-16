using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Wjire.ProjectManager.Model;
using Wjire.ProjectManager.Service;
using System.Linq;

namespace Wjire.ProjectManager
{
    public partial class ProjectAddForm : Form
    {
        private readonly DbService _dbService;

        public ProjectAddForm(string connectionString)
        {
            _dbService = new DbService(connectionString);
            InitializeComponent();
        }


        private void ProjectAddForm_Load(object sender, EventArgs e)
        {
            PublishHandler handler = new PublishHandler(new PublishInfo());
            List<AppInfo> appInfoViews = handler.GetAllAPPInfo();
            BindApp(appInfoViews);
        }



        /// <summary>
        /// 填充App下拉
        /// </summary>
        /// <param name="appInfoViews"></param>
        private void BindApp(List<AppInfo> appInfoViews)
        {
            foreach (AppInfo item in appInfoViews)
            {
                cbx_app.Items.Add(item);
            }
            cbx_app.Text = appInfoViews.FirstOrDefault()?.ToString();
        }


        private void button1_Click(object sender, System.EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(lbl_appName.Text))
                {
                    ShowMsg("项目名称不能为空");
                    return;
                }
                if (string.IsNullOrWhiteSpace(lbl_appDir.Text))
                {
                    ShowMsg("项目文件夹不能为空");
                    return;
                }

                var appInfo = cbx_app.SelectedItem as AppInfo;
                appInfo.LocalPath = tbx_appDir.Text;
                appInfo.AppType = rbn_iis.Checked ? 1 : 2;

                int res = _dbService.AddProjectInfo(appInfo);
                if (res == 1)
                {
                    DialogResult = DialogResult.OK;
                    Close();
                }
                else
                {
                    DialogResult = DialogResult.Abort;
                    ShowMsg("失败");
                }
            }
            catch (Exception ex)
            {
                ShowMsg(ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Abort;
            Close();
        }


        private void ShowMsg(string msg)
        {
            MessageBox.Show(msg);
        }



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

            tbx_appDir.Text = dialog.SelectedPath;
        }
    }
}
