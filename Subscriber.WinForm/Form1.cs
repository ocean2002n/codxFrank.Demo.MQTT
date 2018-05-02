using System;
using System.Text;
using System.Windows.Forms;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace Subscriber.WinForm
{
    public partial class Form1 : Form
    {
        readonly MqttClient _client;

        public Form1()
        {
            InitializeComponent();

            _client = new MqttClient("127.0.0.1");
            _client.MqttMsgPublishReceived += client_MqttMsgPublishReceived;
            _client.Connect(Guid.NewGuid().ToString());
            _client.Subscribe
                (new string[] { "/hello" }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
        }

        void client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            textBox1.Text += Encoding.UTF8.GetString(e.Message) + Environment.NewLine;
        }

        private void Form_Subscribe_FormClosed(object sender, FormClosedEventArgs e)
        {
            _client.Disconnect();
        }
    }
}
