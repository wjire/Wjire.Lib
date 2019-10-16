﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Wjire.ProjectManager.Model;
using Wjire.ProjectManager.Service;

namespace Wjire.ProjectManager
{
    public partial class Form1 : Form
    {
        private readonly DbService _dbService;
        private readonly string _connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
        private List<AppInfo> _appInfos;


        public Form1()
        {
            _dbService = new DbService(_connectionString);
            InitializeComponent();
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            BindSource();
        }


        private void BindSource()
        {
            List<AppInfo> source = _dbService.GetAllAppInfo();
            dgv.DataSource = source.Select(s => new AppInfoView
            {
                AppId = s.AppId,
                AppName = s.AppName,
                AppTypeString = s.AppTypeString,
                LocalPath = s.LocalPath
            }).ToList();
            //dgv.DataSource = source;
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
            PublishHandler handler = new PublishHandler(publishInfo);
            bool publishResult = handler.PublishWeb();
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
                    //AppType = Convert.ToInt32(row.Cells["AppType"].Value.ToString()),
                    AppId = Convert.ToInt32(row.Cells["AppId"].Value.ToString()),
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
    }
}
