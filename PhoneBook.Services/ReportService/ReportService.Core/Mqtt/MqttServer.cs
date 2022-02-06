using MQTTnet;
using MQTTnet.Protocol;
using MQTTnet.Server;
using System.Text;

namespace ReportService.Core.Mqtt
{
    public class MqttServer
    {
        #region Private Members

        /// <summary>
        /// MqttServer instance
        /// </summary>
        private static IMqttServer _mqttServer;

        /// <summary>
        /// Username for mqttServer
        /// </summary>
        private static string _username;

        /// <summary>
        /// Password for mqttserver
        /// </summary>
        private static string _password;

        /// <summary>
        /// Topic name for mqttServer
        /// </summary>
        private static string _topic;

        /// <summary>
        /// Mqtt Port
        /// </summary>
        private static int _port;

        #endregion

        public MqttServer()
        {
            // Set parameters
            _username = "test";
            _password = "123456";
            _topic = "mqttServerTopic";
            _port = 1884;

            CreateMqttServer();
        }

        /// <summary>
        /// Creating a MQTT server is similar to creating a MQTT client. 
        /// The following code shows the most simple way of creating a new 
        /// MQTT server with a TCP endpoint which is listening at the default port 1883.
        /// </summary>
        private static async void CreateMqttServer()
        {
            // Configure MQTT server.
            var optionsBuilder = new MqttServerOptionsBuilder()
                .WithConnectionBacklog(100)
                .WithConnectionValidator(c =>
                {
                    // Check client id length
                    if (c.ClientId.Length < 3)
                    {
                        c.ReasonCode = MqttConnectReasonCode.ClientIdentifierNotValid;
                        return;
                    }

                    // Check username
                    if (!c.Username.Contains(_username))
                    {
                        c.ReasonCode = MqttConnectReasonCode.BadUserNameOrPassword;
                        return;
                    }

                    // Check password
                    if (c.Password != _password)
                    {
                        c.ReasonCode = MqttConnectReasonCode.BadUserNameOrPassword;
                        return;
                    }

                    // Set the reasonCode
                    c.ReasonCode = MqttConnectReasonCode.Success;
                })
                .WithDefaultEndpointPort(_port);

            // Define a mqttServer
            _mqttServer = new MqttFactory().CreateMqttServer();

            // Message arrived configuration
            _mqttServer.UseApplicationMessageReceivedHandler(e =>
            {
                Console.WriteLine("### SERVER RECEIVED MESSAGE ###");
                Console.WriteLine($"+ Topic = {e.ApplicationMessage.Topic}");
                Console.WriteLine($"+ Payload = {Encoding.UTF8.GetString(e.ApplicationMessage.Payload)}");
                Console.WriteLine($"+ QoS = {e.ApplicationMessage.QualityOfServiceLevel}");
                Console.WriteLine($"+ Retain = {e.ApplicationMessage.Retain}");
                Console.WriteLine();
            });

            // When a new client connected
            _mqttServer.UseClientConnectedHandler(e =>
            {
                Console.WriteLine("***** CLIENT CONNECTED : " + e.ClientId + " *******");
            });

            // When a new client disconnected
            _mqttServer.UseClientDisconnectedHandler(e =>
            {
                Console.WriteLine("***** CLIENT DISCONNECTED : " + e.ClientId + " *******");
            });

            // When a new client disconnected
            _mqttServer.UseClientDisconnectedHandler(e =>
            {
                Console.WriteLine("***** CLIENT DISCONNECTED : " + e.ClientId + " *******");
            });

            // Start the mqtt server
            await _mqttServer.StartAsync(optionsBuilder.Build());

            // Log console 
            Console.WriteLine("MqttServer started...");
        }

        /// <summary>
        /// Stops MqttServer
        /// </summary>
        private static async void StopMqttServerAsync()
        {
            // Log Console
            Console.WriteLine("Press any key to stop the server : ");
            Console.ReadLine();

            // Stop the server
            await _mqttServer.StopAsync();

            // Log Console
            Console.WriteLine("Server stopped! ");
            Console.ReadLine();
        }

        /// <summary>
        /// Publishes the indicated message
        /// </summary>
        /// <param name="message">The message that will be published</param>
        public async Task PublishMessageAsync(string message)
        {
            // Create mqttMessage
            var mqttMessage = new MqttApplicationMessageBuilder()
                                .WithTopic(_topic)
                                .WithPayload(message)
                                .WithExactlyOnceQoS()
                                .WithRetainFlag()
                                .Build();

            // Publish the message asynchronously
            await _mqttServer.PublishAsync(mqttMessage, CancellationToken.None);
        }
    }
}
