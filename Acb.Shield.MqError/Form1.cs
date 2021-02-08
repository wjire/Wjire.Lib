using System;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Acb.Shield.MqError
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            DataGridViewButtonColumn btn1 = new DataGridViewButtonColumn
            {
                Name = "send",
                HeaderText = "操作",
                DefaultCellStyle = { NullValue = "发送" },
                AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
                Width = 80,
            };
            dataGridView1.Columns.Add(btn1);

            DataGridViewButtonColumn btn2 = new DataGridViewButtonColumn
            {
                Name = "delete",
                HeaderText = "操作",
                DefaultCellStyle = { NullValue = "删除" },
                AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
                Width = 80,
            };
            dataGridView1.Columns.Add(btn2);

            DataGridViewButtonColumn btn3 = new DataGridViewButtonColumn
            {
                Name = "sendEvent",
                HeaderText = "操作",
                DefaultCellStyle = { NullValue = "编辑后发送" },
                AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
                Width = 80,
            };
            dataGridView1.Columns.Add(btn3);
        }

        private async void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var name = dataGridView1.Columns[e.ColumnIndex].Name;
            if (name == "send")
            {
                await Send(e);
                await GetData();
                return;
            }

            if (name == "delete")
            {
                await Delete(e);
                await GetData();
                return;
            }

            if (name == "sendEvent")
            {
                UpdateAndSend(e);
                return;
            }
        }

        private void UpdateAndSend(DataGridViewCellEventArgs e)
        {
            ClearTextBox();
            var row = e.RowIndex;
            var data = dataGridView1.Rows[row];
            var id = data.Cells["Id"].Value.ToString();
            var queue = data.Cells["Queue"].Value.ToString();
            var message = data.Cells["Event"].Value.ToString();
            var form = new EventEdit()
            {
                Id = id,
                Queue = queue,
                Event = message
            };
            form.Show();
        }

        private async Task Delete(DataGridViewCellEventArgs e)
        {
            ClearTextBox();
            var row = e.RowIndex;
            var data = dataGridView1.Rows[row];
            var cell = data.Cells["Id"];
            var id = cell.Value.ToString();
            var result = await HttpHelper.Delete(id);
            textBox2.Text = result;
        }

        private async Task Send(DataGridViewCellEventArgs e)
        {
            ClearTextBox();
            var row = e.RowIndex;
            var data = dataGridView1.Rows[row];
            var cell = data.Cells["Id"];
            var id = cell.Value.ToString();
            var result = await HttpHelper.Send(id);
            textBox2.Text = result;
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            var colIndex = e.ColumnIndex;
            var name = dataGridView1.Columns[colIndex].Name;
            if (name == "send" || name == "delete" || name == "sendEvent")
            {
                return;
            }
            textBox1.Text = string.Empty;
            var row = e.RowIndex;
            var data = dataGridView1.Rows[row];
            var cell = data.Cells[colIndex];
            textBox1.Text = cell.Value.ToString();
        }

        private async Task GetData()
        {
            try
            {
                ClearTextBox();
                dataGridView1.DataSource = null;
                //ClearTextBox();
                var result = await HttpHelper.Get();
                dataGridView1.DataSource = result.OrderBy(o => o.CreatedTimeDt).ToList();
            }
            catch (Exception ex)
            {
                textBox2.Text = ex.ToString();
            }
        }

        private void ClearTextBox()
        {
            textBox1.Text = string.Empty;
            textBox2.Text = string.Empty;
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            await GetData();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SetDevelop();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            SetRelease();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            SetLocal();
        }

        private async void SetRelease()
        {
            ClearTextBox();
            button3.Enabled = false;
            button3.BackColor = Color.Green;
            button2.Enabled = true;
            button2.BackColor = Color.White;
            button4.Enabled = true;
            button4.BackColor = Color.White;
            HttpHelper.Url = System.Configuration.ConfigurationManager.AppSettings["release"];
            await GetData();
        }

        private async void SetDevelop()
        {
            ClearTextBox();
            button2.Enabled = false;
            button2.BackColor = Color.Green;
            button3.Enabled = true;
            button3.BackColor = Color.White;
            button4.Enabled = true;
            button4.BackColor = Color.White;
            HttpHelper.Url = System.Configuration.ConfigurationManager.AppSettings["develop"];
            await GetData();
        }

        private async void SetLocal()
        {
            ClearTextBox();
            button4.Enabled = false;
            button4.BackColor = Color.Green;
            button2.Enabled = true;
            button2.BackColor = Color.White;
            button3.Enabled = true;
            button3.BackColor = Color.White;
            HttpHelper.Url = System.Configuration.ConfigurationManager.AppSettings["local"];
            await GetData();
        }
    }
}
