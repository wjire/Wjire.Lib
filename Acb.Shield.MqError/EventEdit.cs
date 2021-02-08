using System;
using System.Windows.Forms;

namespace Acb.Shield.MqError
{
    public partial class EventEdit : Form
    {

        public string Queue { get; set; }

        public string Event { get; set; }

        public string Id { get; set; }

        public EventEdit()
        {
            InitializeComponent();
            //this.StartPosition = FormStartPosition.CenterScreen;

        }

        private void EventEdit_Load(object sender, System.EventArgs e)
        {
            txt_queue.Text = Queue;

            var temp = Newtonsoft.Json.JsonConvert.DeserializeObject(Event);
            txt_event.Text = Newtonsoft.Json.JsonConvert.SerializeObject(temp, Newtonsoft.Json.Formatting.Indented);
        }

        private async void button1_Click(object sender, System.EventArgs e)
        {
            var newEvent = txt_event.Text;
            var result = await HttpHelper.SendEvent(Id, newEvent);
            textBox1.Text = result;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
