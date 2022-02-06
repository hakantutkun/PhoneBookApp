using ClosedXML.Excel;
using MQTTnet.Client;
using Newtonsoft.Json;
using ReportService.Core.Models;
using System.Text;
using MqttClient = ReportService.Core.Mqtt.MqttClient;

namespace ReportService.API
{
    /// <summary>
    /// Background Service
    /// Listens and manages report requests
    /// </summary>
    public class WorkerService
    {

        /// <summary>
        /// MqttClient Object
        /// </summary>
        private readonly MqttClient _mqttClient;

        /// <summary>
        /// HttpClient Object
        /// </summary>
        private readonly HttpClient _httpClient;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="mqttClient">MqttClient singleton object</param>
        public WorkerService(MqttClient mqttClient, HttpClient httpClient)
        {
            // Inject MqttClient
            _mqttClient = mqttClient;

            // Inject HttpClient
            _httpClient = httpClient;

            // Set base address of the client.
            _httpClient.BaseAddress = new Uri("http://localhost:5038/Person/");

            // When message arrived from server
            _mqttClient.mqttClient.UseApplicationMessageReceivedHandler(async e =>
            {
                Console.WriteLine("### RECEIVED MESSAGE ###");
                Console.WriteLine($"+ Topic = {e.ApplicationMessage.Topic}");
                Console.WriteLine($"+ Payload = {Encoding.UTF8.GetString(e.ApplicationMessage.Payload)}");
                Console.WriteLine($"+ QoS = {e.ApplicationMessage.QualityOfServiceLevel}");
                Console.WriteLine($"+ Retain = {e.ApplicationMessage.Retain}");
                Console.WriteLine();

                await HandleReceivedMessage(Encoding.UTF8.GetString(e.ApplicationMessage.Payload));
            });
        }

        private async Task HandleReceivedMessage(string message)
        {
            var report = JsonConvert.DeserializeObject<Report>(message);

            if (report != null)
            {
                var responseMessage = await _httpClient.GetAsync("Person/" + report.Location);

                // Check if response message status code is success
                if (responseMessage.IsSuccessStatusCode)
                {
                    // return the list of person
                    int numberOfPerson = int.Parse(await responseMessage.Content.ReadAsStringAsync());

                    report.NumberofPerson = numberOfPerson;
                }

                var phoneResponseMessage = await _httpClient.GetAsync("Phone/" + report.Location);

                // Check if response message status code is success
                if (phoneResponseMessage.IsSuccessStatusCode)
                {
                    // return the list of person
                    int numberOfPhoneNumber = int.Parse(await responseMessage.Content.ReadAsStringAsync());

                    report.NumberofPhoneNumber = numberOfPhoneNumber;
                }

                ExportToExcel(report);
            }
        }

        private void ExportToExcel(Report report)
        {
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Report");

                worksheet.Cell(1, 1).Value = "Konum";
                worksheet.Cell(1, 2).Value = "Kişi Sayısı";
                worksheet.Cell(1, 3).Value = "Tel No Sayısı";

                worksheet.Cell(2, 1).Value = report.Location;
                worksheet.Cell(2, 2).Value = report.NumberofPerson;
                worksheet.Cell(2, 3).Value = report.NumberofPhoneNumber;

                workbook.SaveAs("wwwroot/reports/"+ report.Id +".xlsx");


            }
        }
    }
}
