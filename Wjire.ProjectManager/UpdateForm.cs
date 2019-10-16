using System;
using System.Windows.Forms;
using Wjire.ProjectManager.Model;
using Wjire.ProjectManager.Service;

namespace Wjire.ProjectManager
{
    public partial class UpdateForm : Form
    {
        private readonly AppInfo appInfo;

        public UpdateForm(AppInfo info)
        {
            appInfo = info;
            InitializeComponent();
        }


        private void button1_Click(object sender, System.EventArgs e)
        {
            try
            {

                if (string.IsNullOrWhiteSpace(tbx_localPath.Text))
                {
                    ShowMsg("本地路径不能为空");
                    return;
                }
                new DbService().Update(new AppInfo
                {
                    AppId = appInfo.AppId,
                    LocalPath = tbx_localPath.Text
                });
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

            tbx_localPath.Text = dialog.SelectedPath;
        }

        private void UpdateForm_Load(object sender, EventArgs e)
        {
            tbx_appName.Text = appInfo.AppName;
            tbx_localPath.Text = appInfo.LocalPath;
        }
    }
}
