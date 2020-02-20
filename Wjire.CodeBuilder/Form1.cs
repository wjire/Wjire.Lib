using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Resources;
using System.Windows.Forms;
using FileService.SqlCreater;
using Newtonsoft.Json;
using Wjire.CodeBuilder.DbService;
using Wjire.CodeBuilder.FileService;
using Wjire.CodeBuilder.FileService.ConfigureCreater;
using Wjire.CodeBuilder.Model;
using Wjire.CodeBuilder.Utils;

namespace Wjire.CodeBuilder
{
    public partial class Form1 : Form
    {

        private readonly Dictionary<string, ConnectionInfo> _connectionInfos = new Dictionary<string, ConnectionInfo>();
        private readonly Cs_ModelFactory _modelCreater = new Cs_ModelFactory();
        private IDbService _dbService;
        private ITableSqlCreater _sqlCreater;
        private string _dbType = "sqlserver";

        public Form1()
        {
            InitializeComponent();
            _sqlCreater = SqlCreaterFactory.Create(_dbType);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            InitConnectionInfos();
            InitTextBox_CodePath();
            InitDbTypes();
        }


        /// <summary>
        /// 切换数据库类型
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBox_dbType_SelectedIndexChanged(object sender, EventArgs e)
        {
            _dbType = comboBox_dbType.Text;
            _sqlCreater = SqlCreaterFactory.Create(_dbType);
        }


        /// <summary>
        /// 连接到数据库
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void button_one_conn_Click(object sender, EventArgs e)
        {
            comboBox_one_dataBase.Items.Clear();
            comboBox_one_dataBase.Text = string.Empty;
            Clear();
            ConnectionInfo info = GetCurrentConnectionInfo();
            _dbService = DbServiceFactory.CreateDbService(info);
            List<string> dbList = await _dbService.GetAllDataBase();
            BindDataBase(dbList);
            GetTableNames();
            AddConnectionInfoToCombox(info);
            AddConnectionInfoToCache(info);
            SaveConnectionInfoToResource();
        }


        /// <summary>
        /// 填充数据库下拉
        /// </summary>
        /// <param name="list"></param>
        private void BindDataBase(List<string> list)
        {
            foreach (string item in list)
            {
                comboBox_one_dataBase.Items.Add(item);
            }
            comboBox_one_dataBase.Text = _dbService.ConnectionInfo.DbName;
        }



        /// <summary>
        /// 切换IP
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBox_IP_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox_IP.SelectedIndex < 0)
            {
                return;
            }
            ConnectionInfo connectionInfo = _connectionInfos[comboBox_IP.Text];
            SetConnectionInfoToView(connectionInfo);
        }



        /// <summary>
        /// 切换数据库
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBox_one_dataBase_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox_one_dataBase.SelectedIndex < 0)
            {
                return;
            }
            textBox_dbName.Text = comboBox_one_dataBase.Text;
            ConnectionInfo info = GetCurrentConnectionInfo();
            _dbService = DbServiceFactory.CreateDbService(info);
            GetTableNames();
        }



        /// <summary>
        /// 根据数据库名获取所有表名
        /// </summary>
        private async void GetTableNames()
        {
            List<string> list = await _dbService.GetTableNames();
            Clear();
            int index = 1;
            foreach (string item in list)
            {
                listView_one_tables.Items.Add(new ListViewItem(new[] { index++.ToString(), item }));
            }
        }



        /// <summary>
        /// 选中表,获取该表的字段信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void listView_one_tables_Click(object sender, EventArgs e)
        {
            if (listView_one_tables.SelectedItems.Count != 1)
            {
                return;
            }

            ListViewItem selectedItem = listView_one_tables.SelectedItems[0];
            string tableName = selectedItem.SubItems[1].Text;
            List<TableInfo> list = await _dbService.GetTableInfo(tableName);
            textBox_result.Text = _modelCreater.CreateCode(list, GetCurrentFormInfo(tableName));
            listView_one_tableStruct.Items.Clear();
            int index = 1;
            foreach (TableInfo item in list)
            {
                listView_one_tableStruct.Items.Add(new ListViewItem(new string[]
                {
                    index++.ToString(),
                    item.ColumnName,
                    item.ColumnType,
                    item.ColumnDescription,
                    item.IsKey,
                    item.IsNullable,
                }));
            }
        }


        /// <summary>
        /// 多选表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listView_one_tables_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            e.Item.Selected = e.Item.Checked;
        }


        /// <summary>
        /// 创建 Model 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_toClass_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem selectedItem in listView_one_tables.SelectedItems)
            {
                string tableName = selectedItem.SubItems[1].Text;
                CreateModel(tableName);
            }
            OpenCurrentNameSpaceFileFolder();
        }



        /// <summary>
        /// 创建 Repository
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_toRepository_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem selectedItem in listView_one_tables.SelectedItems)
            {
                string tableName = selectedItem.SubItems[1].Text;
                CreateRepository(tableName);
            }
            OpenCurrentNameSpaceFileFolder();
        }



        /// <summary>
        /// 创建 IRepository
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_toIRepository_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem selectedItem in listView_one_tables.SelectedItems)
            {
                string tableName = selectedItem.SubItems[1].Text;
                CreateIRepository(tableName);
            }
            OpenCurrentNameSpaceFileFolder();
        }



        /// <summary>
        /// 一键生成所有
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_toAll_Click(object sender, EventArgs e)
        {
            CreateSolution("");
            CreateWebApi("");
            foreach (ListViewItem selectedItem in listView_one_tables.SelectedItems)
            {
                string tableName = selectedItem.SubItems[1].Text;
                CreateModel(tableName);
                CreateRepository(tableName);
                //CreateIRepository(tableName);
                CreateDbContext(tableName);
                CreateLogic(tableName);
                CreateService(tableName);
                CreateIService(tableName);
                CreateController(tableName);
            }
            OpenCurrentNameSpaceFileFolder();
        }



        /// <summary>
        /// 选择自定义 excel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_ChooseExcel_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog
            {
                Multiselect = true,
                Title = "请选择文件",
                Filter = "excel文件|*.xls",
            };
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                textBox_excelPath.Text = fileDialog.FileName;
            }
        }



        /// <summary>
        /// 生成表的创建sql语句
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_CreateTableSql_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox_excelPath.Text))
            {
                OpenMessageBox("请选择 excel 文件");
                return;
            }
            string entityName = Path.GetFileNameWithoutExtension(textBox_excelPath.Text);
            string text = _sqlCreater.Create(textBox_excelPath.Text, entityName);
            string savePath = Path.Combine(Path.GetDirectoryName(textBox_excelPath.Text), $"{entityName}.sql");
            FileHelper.CreateFile(savePath, text);
            Process.Start("notepad.exe", savePath);
        }




        /// <summary>
        /// 编辑模板文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_editCodeTemplate_Click(object sender, EventArgs e)
        {
            OpenFileDialog(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Lib"), "txt文件|*.txt");
        }



        /// <summary>
        /// 下载excel模板
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_openTemplate_Click(object sender, EventArgs e)
        {
            OpenFileDialog(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Lib\\ExcelTemplate"), "excel文件|*.xls");
        }


        private void Clear()
        {
            listView_one_tables.Items.Clear();
            listView_one_tableStruct.Items.Clear();
            textBox_result.Text = string.Empty;
        }


        private void InitConnectionInfos()
        {
            LoadConnectionInfoFromResource();
            foreach (KeyValuePair<string, ConnectionInfo> info in _connectionInfos)
            {
                comboBox_IP.Items.Add(info.Key);
            }
            comboBox_IP.Text = _connectionInfos.Keys.FirstOrDefault() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(comboBox_IP.Text))
            {
                return;
            }

            ConnectionInfo connectionInfo = _connectionInfos[comboBox_IP.Text];
            SetConnectionInfoToView(connectionInfo);
        }


        private void SetConnectionInfoToView(ConnectionInfo connectionInfo)
        {
            if (connectionInfo == null)
            {
                return;
            }
            textBox_one_user.Text = connectionInfo.User;
            textBox_dbName.Text = connectionInfo.DbName;
            textBox_one_pwd.Text = connectionInfo.Pwd;
        }

        private void InitTextBox_CodePath()
        {
            string codePath = ConfigurationManager.AppSettings["CodePath"];
            if (string.IsNullOrWhiteSpace(codePath))
            {
                codePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory + "Code");
            }
            if (Directory.Exists(codePath) == false)
            {
                Directory.CreateDirectory(codePath);
            }
            textBox_codePath.Text = codePath;
        }


        private void InitDbTypes()
        {
            comboBox_dbType.Items.Add("sqlserver");
            comboBox_dbType.Items.Add("mysql");
            comboBox_dbType.Text = _dbType;
        }


        private ConnectionInfo GetCurrentConnectionInfo()
        {
            ConnectionInfo connectionInfo = new ConnectionInfo
            {
                DbName = textBox_dbName.Text,
                User = textBox_one_user.Text,
                Pwd = textBox_one_pwd.Text,
                IP = comboBox_IP.Text,
                Type = _dbType,
            };
            return connectionInfo;
        }


        private void AddConnectionInfoToCombox(ConnectionInfo info)
        {
            if (comboBox_IP.Items.Contains(info.IP) == false)
            {
                comboBox_IP.Items.Add(info.IP);
            }
        }


        private void AddConnectionInfoToCache(ConnectionInfo info)
        {
            if (_connectionInfos.ContainsKey(info.IP))
            {
                _connectionInfos[info.IP] = info;
            }
            else
            {
                _connectionInfos.Add(info.IP, info);
            }
        }



        private void SaveConnectionInfoToResource()
        {
            using (ResXResourceWriter writer = new ResXResourceWriter("Resource.resx"))
            {
                foreach (KeyValuePair<string, ConnectionInfo> keyValue in _connectionInfos)
                {
                    string json = JsonConvert.SerializeObject(keyValue.Value);
                    writer.AddResource(keyValue.Key, json);
                }
            }
        }


        private void LoadConnectionInfoFromResource()
        {
            using (ResXResourceReader reader = new ResXResourceReader("Resource.resx"))
            {
                try
                {
                    IDictionaryEnumerator iter = reader.GetEnumerator();
                    while (iter.MoveNext())
                    {
                        ConnectionInfo value = JsonConvert.DeserializeObject<ConnectionInfo>(iter.Value.ToString());
                        _connectionInfos.Add(iter.Key.ToString(), value);
                    }
                }
                catch (Exception)
                {

                }
            }
        }


        private void button_choosePath_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog
            {
                Description = "请选择要保存的路径",
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

            textBox_codePath.Text = dialog.SelectedPath;
        }



        private void OpenFileDialog(string dir, string filter = null, string process = null)
        {
            FileHelper.CheckDirectory(dir);
            OpenFileDialog fileDialog = new OpenFileDialog
            {
                Multiselect = true,
                Title = "请选择文件",
                Filter = filter,
                InitialDirectory = dir,
            };
            if (fileDialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }
            if (string.IsNullOrWhiteSpace(process))
            {
                Process.Start(fileDialog.FileName);
            }
            else
            {
                Process.Start(process, fileDialog.FileName);
            }
        }


        private void button_toNamespace_Click(object sender, EventArgs e)
        {
            OpenCurrentNameSpaceFileFolder();
        }

        private void OpenCurrentNameSpaceFileFolder()
        {
            OpenFileDialog(Path.Combine(textBox_codePath.Text, textBox_namespace.Text));
        }


        private void OpenMessageBox(string msg)
        {
            MessageBox.Show(msg);
        }



        /// <summary>
        /// 创建 DbContext
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_toDbContext_Click(object sender, EventArgs e)
        {
            CreateFile(CreateDbContext);
            OpenCurrentNameSpaceFileFolder();
        }


        /// <summary>
        /// 创建 Logic
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_toLogic_Click(object sender, EventArgs e)
        {
            CreateFile(CreateLogic);
            OpenCurrentNameSpaceFileFolder();
        }


        /// <summary>
        /// 创建 Service
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_toService_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem selectedItem in listView_one_tables.SelectedItems)
            {
                string tableName = selectedItem.SubItems[1].Text;
                CreateService(tableName);
            }
            OpenCurrentNameSpaceFileFolder();
        }


        /// <summary>
        /// 创建 IService
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_toIService_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem selectedItem in listView_one_tables.SelectedItems)
            {
                string tableName = selectedItem.SubItems[1].Text;
                CreateIService(tableName);
            }
            OpenCurrentNameSpaceFileFolder();
        }


        /// <summary>
        /// 创建 Solution
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_toSolution_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem selectedItem in listView_one_tables.SelectedItems)
            {
                string tableName = selectedItem.SubItems[1].Text;
                CreateSolution(tableName);
            }
            OpenCurrentNameSpaceFileFolder();
        }


        private void CreateSolution(string tableName)
        {
            new Solution_Factory().CreateFile(GetCurrentFormInfo(tableName));
        }


        private void CreateIService(string tableName)
        {
            FormInfo formInfo = GetCurrentFormInfo(tableName);
            new Cs_IServiceFactory().CreateFile(formInfo);
            new Csproj_IServiceFactory().CreateFile(formInfo);
        }


        private void CreateService(string tableName)
        {
            FormInfo formInfo = GetCurrentFormInfo(tableName);
            new Cs_ServiceFactory().CreateFile(formInfo);
            new Csproj_ServiceFactory().CreateFile(formInfo);
        }


        private void CreateLogic(string tableName)
        {
            new Csproj_LogicFactory().CreateFile(GetCurrentFormInfo(tableName));
            FormInfo formInfo = GetCurrentFormInfo(tableName);
            new Cs_LogicFactory().CreateFile(formInfo);
        }


        private void CreateDbContext(string tableName)
        {
            FormInfo formInfo = GetCurrentFormInfo(tableName);
            new Csproj_DbContextFactory().CreateFile(formInfo);
            new Cs_DbFactoryFactory().CreateFile(formInfo);
            new Cs_RepositoryFactoryFactory().CreateFile(formInfo);
        }


        private void CreateIRepository(string tableName)
        {
            FormInfo formInfo = GetCurrentFormInfo(tableName);
            new Cs_IRepositoryFactory().CreateFile(formInfo);
            new Csproj_IRepositoryFactory().CreateFile(formInfo);
        }


        private void CreateRepository(string tableName)
        {
            FormInfo formInfo = GetCurrentFormInfo(tableName);
            new Cs_RepositoryFactory().CreateFile(formInfo);
            new Csproj_RepositoryFactory().CreateFile(formInfo);
        }


        private async void CreateModel(string tableName)
        {
            List<TableInfo> list = await _dbService.GetTableInfo(tableName);
            FormInfo formInfo = GetCurrentFormInfo(tableName);
            _modelCreater.CreateFile(list, formInfo);
            new Csproj_ModelFactory().CreateFile(formInfo);
        }


        private void CreateWebApi(string tableName)
        {
            FormInfo formInfo = GetCurrentFormInfo(tableName);
            new Csproj_WebApiFactory().CreateFile(formInfo);
            new Cs_ProgramFactory().CreateFile(formInfo);
            new Cs_StartupFactory().CreateFile(formInfo);
            new Cs_ServicesExtensionFactory().CreateFile(formInfo);
            new Cs_AppsettingsFactory().CreateFile(formInfo);
            new Cs_AppsettingsDevFactory().CreateFile(formInfo);
        }


        private void CreateController(string tableName)
        {
            FormInfo formInfo = GetCurrentFormInfo(tableName);
            new Cs_ControllerFactory().CreateFile(formInfo);
        }


        private FormInfo GetCurrentFormInfo(string tableName = null)
        {
            return new FormInfo
            {
                BasePath = Path.Combine(textBox_codePath.Text, textBox_namespace.Text),
                NameSpaceName = textBox_namespace.Text,
                DbName = textBox_dbName.Text,
                TableName = string.IsNullOrWhiteSpace(tableName) ? string.Empty : tableName
            };
        }



        private void CreateFile(Action<string> action)
        {
            if (_dbService == null)
            {
                OpenMessageBox("请连接数据库");
                return;
            }
            foreach (ListViewItem selectedItem in listView_one_tables.SelectedItems)
            {
                action(selectedItem.SubItems[1].Text);
            }
        }


        /// <summary>
        /// 生成配置文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_toConfiguration_Click(object sender, EventArgs e)
        {
            ConnectionInfo info = GetCurrentConnectionInfo();
            BaseConfigureCreater creater = ConfigureCreaterFactory.Create(info);
            string configure = creater.CreateConfigure();
            textBox_result.Text = configure;
        }
    }
}
