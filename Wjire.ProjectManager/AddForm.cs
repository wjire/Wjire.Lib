using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Wjire.ProjectManager.Model;
using Wjire.ProjectManager.Service;

namespace Wjire.ProjectManager
{
    public partial class AddForm : Form
    {
        private readonly List<AppInfo> appInfos;

        public AddForm(List<AppInfo> appInfos)
        {
            InitializeComponent();
            this.appInfos = appInfos;
        }


        private void ProjectAddForm_Load(object sender, EventArgs e)
        {
            BindApp(appInfos);
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


        private void button1_Click(object sender, EventArgs e)
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

                AppInfo appInfo = cbx_app.SelectedItem as AppInfo;
                appInfo.LocalPath = tbx_appDir.Text;
                appInfo.AppType = 1;
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


        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Abort;
            Close();
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



        private void ShowMsg(string msg)
        {
            MessageBox.Show(msg);
        }

    }
}
