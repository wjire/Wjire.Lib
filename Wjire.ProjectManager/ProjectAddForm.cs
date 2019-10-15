using System;
using System.Windows.Forms;
using Wjire.ProjectManager.Model;
using Wjire.ProjectManager.Service;

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

        private void button1_Click(object sender, System.EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(lbl_projectName.Text))
                {
                    ShowMsg("项目名称不能为空");
                    return;
                }
                if (string.IsNullOrWhiteSpace(lbl_projectDir.Text))
                {
                    ShowMsg("项目文件夹不能为空");
                    return;
                }

                int res = _dbService.AddProjectInfo(new ProjectInfo
                {
                    ProjectName = tbx_projectName.Text,
                    ProjectDir = tbx_projectDir.Text,
                    ProjectType = rbn_iis.Checked ? 1 : 2
                });
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

            tbx_projectDir.Text = dialog.SelectedPath;
        }
    }
}
