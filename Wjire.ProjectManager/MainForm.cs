using System;
using System.Collections.Generic;
using System.IO;
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
            //string uploadApi = System.Configuration.ConfigurationManager.AppSettings["uploadApi"];
            //Text += $" 服务器 : {uploadApi}";
            BindDataGridView();
        }


        private void BindDataGridView()
        {
            List<AppInfo> source = new DbService().GetAllAppInfo();
            dgv.DataSource = source;
        }


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



        private void button2_Click_1(object sender, EventArgs e)
        {
            if (dgv.SelectedRows.Count == 0)
            {
                return;
            }

            PublishInfo publishInfo = CreatePublishInfo(dgv.SelectedRows[0]);
            PublishHandler handler = new PublishHandler(publishInfo);
            bool publishResult = handler.PublishApp();
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
            return publishInfo;
        }



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


        private DialogResult ShowMsg(string msg)
        {
            return MessageBox.Show(msg);
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
    }
}
