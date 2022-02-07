using ClosedXML.Excel;
using MQTTnet.Client;
using Newtonsoft.Json;
using ReportService.Core.Enums;
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

        #region Members

        /// <summary>
        /// MqttClient Object
        /// </summary>
        private readonly MqttClient _mqttClient;

        /// <summary>
        /// HttpClient Object
        /// </summary>
        private readonly HttpClient _httpContactClient;

        /// <summary>
        /// HttpClient Object
        /// </summary>
        private readonly HttpClient _httpReportClient;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="mqttClient">MqttClient singleton object</param>
        public WorkerService(MqttClient mqttClient, HttpClient httpContactClient, HttpClient httpReportClient)
        {
            // Inject MqttClient
            _mqttClient = mqttClient;

            // Inject HttpClient
            _httpContactClient = httpContactClient;

            // Inject HttpClient
            _httpReportClient = httpReportClient;

            // Set base address of the contact client.
            _httpContactClient.BaseAddress = new Uri("http://localhost:5038/Person/");

            // set base address of the report client.
            _httpReportClient.BaseAddress = new Uri("http://localhost:5088/Report/");

            // When message arrived from server
            _mqttClient.mqttClient.UseApplicationMessageReceivedHandler(async e =>
            {
                // Handle received message that received from mqtt queue
                await HandleReceivedMessage(Encoding.UTF8.GetString(e.ApplicationMessage.Payload));

            });
        }

        #endregion

        #region Methods

        /// <summary>
        /// Handles Received Message
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        private async Task HandleReceivedMessage(string message)
        {
            // Deserialize received json string to report object.
            var report = JsonConvert.DeserializeObject<Report>(message);

            // Check if report is null
            if (report != null)
            {
                // Send request to contact service for getting number of persons of this report.
                var responseMessage = await _httpContactClient.GetAsync("Person/" + report.Location);

                // Check if response message status code is success
                if (responseMessage.IsSuccessStatusCode)
                {
                    // Set number of person property of the report.
                    report.NumberofPerson = int.Parse(await responseMessage.Content.ReadAsStringAsync());
                }

                // Send request to contatc service for getting number of phone numbers.
                var phoneResponseMessage = await _httpContactClient.GetAsync("Phone/" + report.Location);

                // Check if response message status code is success
                if (phoneResponseMessage.IsSuccessStatusCode)
                {
                    report.NumberofPhoneNumber = int.Parse(await responseMessage.Content.ReadAsStringAsync());
                }

                // Create excel report
                await ExportToExcel(report);
            }
        }

        /// <summary>
        /// Creates excel report
        /// </summary>
        /// <param name="report">Received report object</param>
        /// <returns></returns>
        private async Task ExportToExcel(Report report)
        {

            // Wait 10 seconds for simulating excel file creation process.
            Thread.Sleep(10000);

            // Create Excel File
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Report");

                worksheet.Cell(1, 1).Value = "Konum";
                worksheet.Cell(1, 2).Value = "Kişi Sayısı";
                worksheet.Cell(1, 3).Value = "Tel No Sayısı";

                worksheet.Cell(2, 1).Value = report.Location;
                worksheet.Cell(2, 2).Value = report.NumberofPerson;
                worksheet.Cell(2, 3).Value = report.NumberofPhoneNumber;

                // Set path of the file
                var path = "wwwroot/reports/" + report.Id + ".xlsx";

                // Save the excel file.
                workbook.SaveAs(path);

                // Set report filepath property
                report.FilePath = path;

                // Change report state as completed.
                report.ReportState = ReportState.Completed;

                // Serialize received object to string.
                var jsonData = JsonConvert.SerializeObject(report);

                // Create a string content with json data.
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                // Send update request to the report service in order to update newly added values
                await _httpReportClient.PutAsync("", content);
            }
        }

        #endregion
    }
}
