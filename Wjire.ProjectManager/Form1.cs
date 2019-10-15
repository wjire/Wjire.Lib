using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using Wjire.ProjectManager.Model;
using Wjire.ProjectManager.Service;

namespace Wjire.ProjectManager
{
    public partial class Form1 : Form
    {
        private readonly DbService _dbService;
        private readonly string _connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
        private List<AppInfoView> _appInfoViews;


        public Form1()
        {
            _dbService = new DbService(_connectionString);
            InitializeComponent();
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            BindSource();
            BasePublishHandler handler = new BasePublishHandler(new PublishInfo());
            _appInfoViews = handler.GetAllAPPInfo();
        }


        private void BindSource()
        {
            List<ProjectInfo> source = _dbService.GetProjectInfo();
            dgv.DataSource = source;
        }


        private void button2_Click(object sender, EventArgs e)
        {
            ProjectAddForm fm = new ProjectAddForm(_connectionString);
            fm.ShowDialog();
            if (fm.DialogResult == DialogResult.OK)
            {
                BindSource();
            }
        }



        private void button2_Click_1(object sender, EventArgs e)
        {
            if (dgv.SelectedRows.Count == 0)
            {
                return;
            }

            PublishInfo publishInfo = CreatePublishInfo(dgv.SelectedRows[0]);
            IPublishHandler handler = PublishHandlerFactory.Create(publishInfo);
            handler.PublishWeb();
        }


        private PublishInfo CreatePublishInfo(DataGridViewRow row)
        {
            PublishInfo publishInfo = new PublishInfo
            {
                ProjectInfo = new ProjectInfo
                {
                    ProjectName = row.Cells["ProjectName"].Value.ToString(),
                    ProjectDir = row.Cells["ProjectDir"].Value.ToString(),
                    ProjectType = Convert.ToInt32(row.Cells["ProjectType"].Value.ToString())
                },
            };
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $@"upload\{publishInfo.ProjectInfo.ProjectName}");
            if (Directory.Exists(path) == false)
            {
                Directory.CreateDirectory(path);
            }
            publishInfo.FileName = Path.Combine(path, $"{publishInfo.ProjectInfo.ProjectName}.zip");
            return publishInfo;
        }
    }
}
