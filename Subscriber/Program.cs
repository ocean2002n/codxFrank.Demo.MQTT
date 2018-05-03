using System;
using System.Runtime.InteropServices;
using System.Text;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace Subscriber
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = new MqttClient("127.0.0.1");
            // https://m2mqtt.wordpress.com/using-mqttclient/
            // Username and password for client authentication (default values are null, no authentication);
            // Will message feature (default values provides NO Will message);
            // Clean session for removing subscriptions on disconnection (default value is true);
            // Keep Alive period for keeping alive connection with ping message (default value is 60 seconds);
            client.Connect(Guid.NewGuid().ToString());
            client.Subscribe(new string[] { "/hello" }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
            client.MqttMsgPublishReceived += client_PublishArrived;

            SetConsoleCtrlHandler(t =>
            {
                // 這邊簡單的顯示導致視窗關閉的來源是什麼
                Console.WriteLine(t.ToString());

                // 在這裡處理視窗關閉前想要執行的程式碼
                client.Disconnect();

                // 返回 false 將事件交回原處理函式執行正常關閉
                return false;
            },
            true);
        }

        private static void client_PublishArrived(object sender, MqttMsgPublishEventArgs e)
        {
            Console.WriteLine("Message Received");
            Console.WriteLine(e.Topic);
            Console.WriteLine(Encoding.UTF8.GetString(e.Message));
        }

        #region WinAPI 相關

        [DllImport("Kernel32")]
        public static extern bool SetConsoleCtrlHandler(ConsoleCtrlDelegate handler, bool add);

        public delegate bool ConsoleCtrlDelegate(CtrlTypes ctrlType);

        public enum CtrlTypes
        {
            CTRL_C_EVENT = 0,
            CTRL_BREAK_EVENT,
            CTRL_CLOSE_EVENT,
            CTRL_LOGOFF_EVENT = 5,
            CTRL_SHUTDOWN_EVENT
        }

        #endregion
    }
}
