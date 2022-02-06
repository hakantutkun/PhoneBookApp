using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Options;
using MQTTnet.Client.Subscribing;
using System.Text;

namespace ReportService.Core.Mqtt
{
    public class MqttClient
    {
        #region Private Members

        /// <summary>
        /// MqttClient instance
        /// </summary>
        public IMqttClient mqttClient;

        /// <summary>
        /// Client Id
        /// </summary>
        private static string _clientId = string.Empty;

        /// <summary>
        /// Client UserName
        /// </summary>
        private static string _username = string.Empty;

        /// <summary>
        /// Client Password
        /// </summary>
        private static string _password = string.Empty;

        /// <summary>
        /// Topic that client will be subscribed to
        /// </summary>
        private static string _topic = string.Empty;

        /// <summary>
        /// MqttServer address
        /// </summary>
        private static string _server = string.Empty;

        /// <summary>
        /// Mqtt Port
        /// </summary>
        private static int _port;

        /// <summary>
        /// The message that will be published
        /// </summary>
        private static string _message = string.Empty;

        #endregion

        public MqttClient()
        {
            // Set parameters
            _clientId = "MqttClient";
            _username = "test";
            _password = "123456";
            _server = "localhost";
            _topic = "mqttServerTopic";
            _port = 1884;

            // Create mqttClient and configure it
            CreateMqttClient();
        }

        /// <summary>
        /// Creates new MqttClient
        /// </summary>
        public async void CreateMqttClient()
        {
            // Create a factory
            var factory = new MqttFactory();

            // Create client
            mqttClient = factory.CreateMqttClient();

            // Set Client options
            var options = new MqttClientOptionsBuilder().WithClientId(_clientId)
                                                        .WithCredentials(_username, _password)
                                                        .WithTcpServer(_server, _port)
                                                        .Build();

            // When client connected to the server
            mqttClient.UseConnectedHandler(async e =>
            {
                Console.WriteLine("### CONNECTED WITH SERVER ###");

                // Subscribe to a topic
                MqttClientSubscribeResult subResult = await mqttClient.SubscribeAsync(new MqttClientSubscribeOptionsBuilder().WithTopicFilter(_topic).Build());

                Console.WriteLine("### SUBSCRIBED TO TOPIC : " + _topic);
            });

            // When message arrived from server
            mqttClient.UseApplicationMessageReceivedHandler(e =>
            {

                Console.WriteLine("### " + _clientId + " RECEIVED MESSAGE ###");
                Console.WriteLine($"+ Topic = {e.ApplicationMessage.Topic}");
                Console.WriteLine($"+ Payload = {Encoding.UTF8.GetString(e.ApplicationMessage.Payload)}");
                Console.WriteLine($"+ QoS = {e.ApplicationMessage.QualityOfServiceLevel}");
                Console.WriteLine($"+ Retain = {e.ApplicationMessage.Retain}");
                Console.WriteLine();
            });

            // Reconnect configuration
            mqttClient.UseDisconnectedHandler(async e =>
            {
                Console.WriteLine("### DISCONNECTED FROM SERVER ###");

                // wait some time before trying to reconnect
                await Task.Delay(TimeSpan.FromSeconds(5));

                try
                {
                    await mqttClient.ConnectAsync(options, CancellationToken.None);
                }
                catch
                {
                    Console.WriteLine("### RECONNECTING FAILED ###");
                }
            });

            try
            {
                // Create connection
                await mqttClient.ConnectAsync(options, CancellationToken.None);
            }
            catch (Exception ex)
            {
                Console.WriteLine("### CONNECTION FAILED ### " + ex.Message);
            }
        }

        /// <summary>
        /// Publishes the indicated message
        /// </summary>
        /// <param name="message">The message that will be published</param>
        private async Task PublishMessage(string message)
        {

            // Create mqttMessage
            var mqttMessage = new MqttApplicationMessageBuilder()
                                .WithTopic(_topic)
                                .WithPayload(message)
                                .WithExactlyOnceQoS()
                                .WithRetainFlag(false)
                                .WithDupFlag(false)
                                .Build();

            if (mqttClient.IsConnected)
            {
                // Publish the message asynchronously
                await mqttClient.PublishAsync(mqttMessage, CancellationToken.None);

                // Log console
                Console.WriteLine("Message published : " + message);

                Thread.Sleep(1500);
            }
            else
            {
                Console.WriteLine("Client is not connected yet. Message can not be published..");
            }
        }
    }
}
