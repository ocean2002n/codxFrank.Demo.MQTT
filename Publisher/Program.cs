using System;
using System.Text;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace Publisher
{
    class Program
    {
        static void Main(string[] args)
        {
            var topic = "/hello";

            var client = new MqttClient("127.0.0.1");
            client.Connect(Guid.NewGuid().ToString());
            client.MqttMsgPublished += client_MqttMsgPublished;

            var msg = string.Empty;
            do
            {
                Console.WriteLine("Input message:");
                msg = Console.ReadLine();
                var t = client.Publish(topic,
                    Encoding.UTF8.GetBytes($"DateTime : {DateTime.Now} / Message : {msg}"),
                    MqttMsgBase.QOS_LEVEL_AT_MOST_ONCE,
                    true);
            } while (!string.Equals(msg, "Q", StringComparison.InvariantCultureIgnoreCase));

            client.Disconnect();
        }

        private static void client_MqttMsgPublished(object sender, MqttMsgPublishedEventArgs e)
        {
            Console.WriteLine("Message Published");
            Console.WriteLine(e.IsPublished);
            Console.WriteLine(e.MessageId);
        }
    }
}
