using ReportService.Core.Enums;

namespace ReportService.Core.Models
{
    /// <summary>
    /// Report Model
    /// </summary>
    public class Report
    {
        /// <summary>
        /// Id of the report
        /// </summary>
        public string? Id { get; set; }

        /// <summary>
        /// The time that report was created.
        /// </summary>
        public DateTime CreationTime { get; set; }

        /// <summary>
        /// State of the report
        /// </summary>
        public ReportState ReportState { get; set; }

        /// <summary>
        /// Location info of the report
        /// </summary>
        public string? Location { get; set; }

        /// <summary>
        /// Number of persons that recorded to selected location
        /// </summary>
        public int? NumberofPerson { get; set; }

        /// <summary>
        /// Number of phone numbers that recorded to selected location.
        /// </summary>
        public int? NumberofPhoneNumber { get; set; }
    }
}
